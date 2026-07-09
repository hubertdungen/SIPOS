using System.Text.Json;
using System.Text.Json.Serialization;

namespace SIPOS
{
    /// <summary>
    /// Tipos de ação que uma linha de um programa Modelar pode executar.
    /// O conjunto é extensível: novos tipos devem ser acrescentados no fim
    /// para não invalidar programas já gravados em JSON.
    /// </summary>
    public enum TipoDeAcao
    {
        /// <summary>Copia o conteúdo de um documento Word e cola-o no fim do documento final.</summary>
        InserirDocumento,

        /// <summary>Consulta as folhas Excel das escalas para o dia atual do loop e carrega as variáveis.</summary>
        LerEscalasDoDia,

        /// <summary>Substitui as variáveis &lt;tag&gt; no último bloco colado com os valores do dia atual.</summary>
        SubstituirVariaveis,

        /// <summary>Repete as ações filhas uma vez por cada dia selecionado.</summary>
        LoopDias,

        /// <summary>Insere uma quebra de página no documento final.</summary>
        QuebraDePagina
    }

    /// <summary>
    /// Uma linha de ação de um programa Modelar. Ações do tipo LoopDias
    /// transportam as suas ações aninhadas em <see cref="Filhos"/>.
    /// </summary>
    public class AcaoModelar
    {
        public TipoDeAcao Tipo { get; set; }

        /// <summary>Rótulo visível da linha (corresponde ao txtNameWBox da UI).</summary>
        public string Nome { get; set; } = "";

        /// <summary>Caminho do ficheiro Word, quando a ação usa um documento (InserirDocumento).</summary>
        public string Ficheiro { get; set; } = "";

        /// <summary>Linha ativa/inativa (corresponde ao botão ✓ da UI).</summary>
        public bool Ativa { get; set; } = true;

        /// <summary>Ações aninhadas, apenas para tipos contentores (LoopDias).</summary>
        public List<AcaoModelar> Filhos { get; set; } = new List<AcaoModelar>();
    }

    /// <summary>
    /// Uma operação concreta do plano de execução: o resultado de expandir o
    /// programa (com loops resolvidos) numa sequência linear que o motor de
    /// Word interop (B-1.5.2) consome passo a passo.
    /// </summary>
    public readonly struct OperacaoPlaneada
    {
        public OperacaoPlaneada(TipoDeAcao tipo, DateTime? dia, string ficheiro, string nome)
        {
            Tipo = tipo;
            Dia = dia;
            Ficheiro = ficheiro;
            Nome = nome;
        }

        public TipoDeAcao Tipo { get; }

        /// <summary>Dia do loop a que a operação pertence (null fora de loops).</summary>
        public DateTime? Dia { get; }

        public string Ficheiro { get; }
        public string Nome { get; }
    }

    /// <summary>
    /// Programa Modelar: sequência ordenada de ações que descreve as operações
    /// que o SIPOS executa ao exportar (B-1.5.1). Persistido em JSON ao lado do
    /// executável, tal como o settings.txt.
    /// </summary>
    public class ProgramaModelar
    {
        public string Nome { get; set; } = "Programa por omissão";
        public int Versao { get; set; } = 1;
        public List<AcaoModelar> Acoes { get; set; } = new List<AcaoModelar>();

        private static readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            Converters = { new JsonStringEnumConverter() }
        };

        /// <summary>Caminho por omissão do programa gravado, ao lado do SIPOS.exe (regra portable).</summary>
        public static string CaminhoPorOmissao()
        {
            return Path.Combine(AppContext.BaseDirectory, "modelar_programa.json");
        }

        public void Guardar(string caminho)
        {
            File.WriteAllText(caminho, JsonSerializer.Serialize(this, jsonOptions));
        }

        /// <summary>Carrega um programa gravado; devolve null se o ficheiro não existir ou estiver corrompido.</summary>
        public static ProgramaModelar? Carregar(string caminho)
        {
            try
            {
                if (!File.Exists(caminho)) { return null; }
                return JsonSerializer.Deserialize<ProgramaModelar>(File.ReadAllText(caminho), jsonOptions);
            }
            catch
            {
                return null; // JSON corrompido: o chamador decide recriar o programa por omissão
            }
        }

        /// <summary>
        /// Valida o programa segundo as regras da UI (coerente com B-1.2.5):
        /// nomes de ação não podem ser vazios nem duplicados (ignorando
        /// maiúsculas/espaços), e ações com documento precisam de um caminho.
        /// Devolve a lista de problemas encontrados (vazia = programa válido).
        /// </summary>
        public List<string> Validar()
        {
            var problemas = new List<string>();
            var nomesVistos = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            ValidarAcoes(Acoes, problemas, nomesVistos);
            return problemas;
        }

        private static void ValidarAcoes(List<AcaoModelar> acoes, List<string> problemas, HashSet<string> nomesVistos)
        {
            foreach (AcaoModelar acao in acoes)
            {
                string nome = (acao.Nome ?? "").Trim();
                if (nome.Length == 0)
                {
                    problemas.Add($"Ação do tipo {acao.Tipo} sem nome.");
                }
                else if (!nomesVistos.Add(nome))
                {
                    problemas.Add($"Nome de ação duplicado: \"{nome}\".");
                }

                if (acao.Tipo == TipoDeAcao.InserirDocumento && string.IsNullOrWhiteSpace(acao.Ficheiro))
                {
                    problemas.Add($"A ação \"{nome}\" (InserirDocumento) não tem ficheiro Word associado.");
                }

                if (acao.Tipo == TipoDeAcao.LoopDias && acao.Filhos.Count == 0)
                {
                    problemas.Add($"O loop \"{nome}\" não tem ações no interior.");
                }

                if (acao.Filhos.Count > 0)
                {
                    ValidarAcoes(acao.Filhos, problemas, nomesVistos);
                }
            }
        }

        /// <summary>
        /// Expande o programa num plano de execução linear, resolvendo os loops
        /// contra a lista de dias selecionados. Ações inativas são ignoradas.
        /// É este plano que o motor Word (B-1.5.2) executa passo a passo.
        /// </summary>
        public List<OperacaoPlaneada> ExpandirPlano(IReadOnlyList<DateTime> diasSelecionados)
        {
            var plano = new List<OperacaoPlaneada>();
            ExpandirAcoes(Acoes, diasSelecionados, null, plano);
            return plano;
        }

        private static void ExpandirAcoes(List<AcaoModelar> acoes, IReadOnlyList<DateTime> dias, DateTime? diaAtual, List<OperacaoPlaneada> plano)
        {
            foreach (AcaoModelar acao in acoes)
            {
                if (!acao.Ativa) { continue; }

                if (acao.Tipo == TipoDeAcao.LoopDias)
                {
                    foreach (DateTime dia in dias)
                    {
                        ExpandirAcoes(acao.Filhos, dias, dia, plano);
                    }
                }
                else
                {
                    plano.Add(new OperacaoPlaneada(acao.Tipo, diaAtual, acao.Ficheiro, acao.Nome));
                }
            }
        }

        /// <summary>
        /// Programa que replica o fluxo clássico atual do SIPOS num único dia
        /// ou em vários: por cada dia selecionado, lê as escalas, insere o
        /// fragmento da tabela (ex.: modelo_escalas.doc) e substitui as
        /// variáveis no bloco colado.
        /// </summary>
        public static ProgramaModelar CriarProgramaClassico(string caminhoFragmentoEscalas)
        {
            return new ProgramaModelar
            {
                Nome = "Exportação clássica de O.S.",
                Acoes = new List<AcaoModelar>
                {
                    new AcaoModelar
                    {
                        Tipo = TipoDeAcao.LoopDias,
                        Nome = "Por cada dia selecionado",
                        Filhos = new List<AcaoModelar>
                        {
                            new AcaoModelar { Tipo = TipoDeAcao.LerEscalasDoDia,     Nome = "Ler escalas do dia" },
                            new AcaoModelar { Tipo = TipoDeAcao.InserirDocumento,    Nome = "Inserir tabela de escalas", Ficheiro = caminhoFragmentoEscalas },
                            new AcaoModelar { Tipo = TipoDeAcao.SubstituirVariaveis, Nome = "Preencher variáveis do dia" }
                        }
                    }
                }
            };
        }
    }

    /// <summary>
    /// Derivação da lista de dias que uma O.S. cobre (B-1.5.3), combinando a
    /// regra clássica do fim-de-semana com o motor de feriados (Feriados.cs).
    /// </summary>
    public static class PlaneadorDeDias
    {
        /// <summary>
        /// Dias de escala cobertos pela O.S. publicada em <paramref name="diaDaOS"/>.
        /// Regra: a partir do dia seguinte à O.S., incluir todos os dias de
        /// descanso consecutivos (fim-de-semana e feriados) e terminar no
        /// primeiro dia útil. Reproduz o comportamento clássico:
        /// - O.S. de quarta normal → [quinta]
        /// - O.S. de sexta → [sábado, domingo, segunda]
        /// e generaliza-o para feriados:
        /// - O.S. de véspera de feriado → [feriado, ..., primeiro dia útil].
        /// </summary>
        public static List<DateTime> DiasDeEscala(DateTime diaDaOS, bool incluirFacultativos = false)
        {
            var dias = new List<DateTime>();
            DateTime dia = diaDaOS.Date.AddDays(1);

            // Salvaguarda: nunca expandir mais de 14 dias, mesmo com sequências
            // longas de descanso, para evitar loops infinitos com dados errados.
            for (int i = 0; i < 14; i++)
            {
                dias.Add(dia);
                if (!Feriados.IsDiaDeDescanso(dia, incluirFacultativos))
                {
                    break; // chegou ao primeiro dia útil: a próxima O.S. cobre daí em diante
                }
                dia = dia.AddDays(1);
            }
            return dias;
        }

        /// <summary>Todos os dias do intervalo [inicio, fim], inclusive, para o modo início/fim (B-1.3.2).</summary>
        public static List<DateTime> DiasDoIntervalo(DateTime inicio, DateTime fim)
        {
            var dias = new List<DateTime>();
            for (DateTime dia = inicio.Date; dia <= fim.Date; dia = dia.AddDays(1))
            {
                dias.Add(dia);
            }
            return dias;
        }
    }
}
