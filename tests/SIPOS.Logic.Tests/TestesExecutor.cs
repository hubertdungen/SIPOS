namespace SIPOS.Logic.Tests
{
    /// <summary>
    /// Colocação dos blocos pelo ExecutorModelar, com modelos que reproduzem a
    /// estrutura dos .doc reais (modelos_word/): o bloco "Para o dia
    /// &lt;dataEscalados&gt;" dentro do ponto 101, a secção dos funerais no modelo de
    /// quarta e o "102. AUSÊNCIAS..." a seguir. O resultado esperado segue as
    /// O.S. reais em modelos_word/exemplares (2022-002-186 e 2022-002-188).
    /// </summary>
    internal static class TestesExecutor
    {
        private const string Bloco =
            "Para o dia <dataEscalados>\r" +
            "- OFICIAL DE DIA\r" +
            "<ODUefectivo><ODUptpd><ODUadapt>\r" +
            "<ODUstatus>\r" +
            "RES\r" +
            "<ODUreserva>\r" +
            "- CONDUTOR DE DIA\r" +
            "\tA nomear pela Esquadrilha de Transportes.\r";

        private const string Funerais =
            "- ASSISTÊNCIA AOS FUNERAIS\r" +
            "<OAFefectivo><OAFptpd><OAFadapt>\r" +
            "<OAFstatus>\r" +
            "RES\r" +
            "<OAFreserva>\r";

        private const string Inicio = "ORDEM DE SERVIÇO\rI - PESSOAL MILITAR\r101. PESSOAL DE SERVIÇO\r";
        private const string Fim = "102. AUSÊNCIAS E IMPEDIMENTOS, 103. MOVIMENTOS DE PESSOAL\rNada a referir.\rIII - DIVERSOS\rO COMANDANTE\r";

        private const string ModeloSemana = Inicio + Bloco + Fim;
        private const string ModeloQuarta = Inicio + Bloco + Funerais + Fim;

        private const string Escalas = @"C:\SIPOS\modelos_word\modelo_escalas.doc";
        private const string Separador = @"C:\SIPOS\modelos_word\separador.doc";

        private static Dictionary<string, string> Ficheiros() => new Dictionary<string, string>
        {
            [Escalas] = Bloco,
            [Separador] = "--- separador ---\r"
        };

        private static string BlocoDo(DateTime d)
        {
            string dd = d.ToString("dd/MM");
            return Bloco.Replace("<dataEscalados>", dd)
                        .Replace("<ODUefectivo>", $"ODU[{dd}]")
                        .Replace("<ODUptpd>", "").Replace("<ODUadapt>", "").Replace("<ODUstatus>", "")
                        .Replace("<ODUreserva>", $"RES-ODU[{dd}]");
        }

        private static string FuneraisDo(DateTime d)
        {
            string dd = d.ToString("dd/MM");
            return Funerais.Replace("<OAFefectivo>", $"OAF[{dd}]")
                           .Replace("<OAFptpd>", "").Replace("<OAFadapt>", "").Replace("<OAFstatus>", "")
                           .Replace("<OAFreserva>", $"RES-OAF[{dd}]");
        }

        private static (DocumentoTexto doc, List<DateTime> lidos) Executar(string modelo, ProgramaModelar programa, params DateTime[] dias)
        {
            var doc = new DocumentoTexto(modelo, Ficheiros());
            var lidos = new List<DateTime>();
            new ExecutorModelar(doc, d => lidos.Add(d)).Executar(programa.ExpandirPlano(dias));
            return (doc, lidos);
        }

        private static ProgramaModelar Loop(params AcaoModelar[] filhos) => new ProgramaModelar
        {
            Acoes = new List<AcaoModelar>
            {
                new AcaoModelar { Tipo = TipoDeAcao.LoopDias, Nome = "Por cada dia", Filhos = filhos.ToList() }
            }
        };

        private static AcaoModelar Ler() => new AcaoModelar { Tipo = TipoDeAcao.LerEscalasDoDia, Nome = "Ler" };
        private static AcaoModelar Inserir(string f, bool ativa = true) => new AcaoModelar { Tipo = TipoDeAcao.InserirDocumento, Nome = "Inserir " + Path.GetFileName(f), Ficheiro = f, Ativa = ativa };
        private static AcaoModelar Substituir() => new AcaoModelar { Tipo = TipoDeAcao.SubstituirVariaveis, Nome = "Substituir" };
        private static AcaoModelar Quebra() => new AcaoModelar { Tipo = TipoDeAcao.QuebraDePagina, Nome = "Quebra" };

        private static readonly DateTime Sab = new DateTime(2022, 10, 1);
        private static readonly DateTime Dom = new DateTime(2022, 10, 2);
        private static readonly DateTime Seg = new DateTime(2022, 10, 3);
        private static readonly DateTime Qua = new DateTime(2022, 10, 5);   // Implantação da República
        private static readonly DateTime Qui = new DateTime(2022, 10, 6);

        // O.S. de sexta 30SET2022 (exemplar 2022-002-186): três blocos seguidos no ponto 101
        [Teste]
        private static void FimDeSemana_TresBlocosSeguidosAntesDo102()
        {
            var (doc, lidos) = Executar(ModeloSemana, ProgramaModelar.CriarProgramaClassico(Escalas), Sab, Dom, Seg);

            Assert.Igual(Inicio + BlocoDo(Sab) + BlocoDo(Dom) + BlocoDo(Seg) + Fim, doc.Texto);
            Assert.Sequencia(new[] { Sab, Dom, Seg }, lidos, "dias lidos do Excel:");
        }

        // O.S. de terça 04OUT2022 (exemplar 2022-002-188): quarta feriado + quinta; os
        // funerais ficam logo a seguir ao bloco de quarta
        [Teste]
        private static void Quarta_FuneraisFicamComOBlocoDeQuarta()
        {
            var (doc, _) = Executar(ModeloQuarta, ProgramaModelar.CriarProgramaClassico(Escalas), Qua, Qui);

            Assert.Igual(Inicio + BlocoDo(Qua) + FuneraisDo(Qua) + BlocoDo(Qui) + Fim, doc.Texto);
        }

        // Um só dia: igual ao fluxo clássico (preenche a tabela do modelo, nada inserido)
        [Teste]
        private static void UmDia_IgualAoFluxoClassico()
        {
            var (doc, _) = Executar(ModeloSemana, ProgramaModelar.CriarProgramaClassico(Escalas), Qui);

            Assert.Igual(Inicio + BlocoDo(Qui) + Fim, doc.Texto);
        }

        [Teste]
        private static void ModeloSemTabela_BlocosEntramAntesDo102()
        {
            var (doc, _) = Executar(Inicio + Fim, ProgramaModelar.CriarProgramaClassico(Escalas), Sab, Dom);

            Assert.Igual(Inicio + BlocoDo(Sab) + BlocoDo(Dom) + Fim, doc.Texto);
        }

        // Sem "102." nem tabela: vai para o fim, num parágrafo próprio (não se junta ao último)
        [Teste]
        private static void ModeloSemTituloNemTabela_BlocosNoFimEmParagrafoProprio()
        {
            const string modelo = "ORDEM DE SERVIÇO\rO COMANDANTE\r";
            var (doc, _) = Executar(modelo, ProgramaModelar.CriarProgramaClassico(Escalas), Sab, Dom);

            Assert.Igual("ORDEM DE SERVIÇO\rO COMANDANTE\r" + BlocoDo(Sab) + BlocoDo(Dom) + "\r", doc.Texto);
        }

        [Teste]
        private static void MarcadorFimEscalas_BlocosEntramNoMarcador()
        {
            const string nota = "Nota: consultar as escalas afixadas.\r";
            string modelo = Inicio + Bloco + "<fimEscalas>\r" + nota + Fim;
            var (doc, _) = Executar(modelo, ProgramaModelar.CriarProgramaClassico(Escalas), Sab, Dom);

            Assert.Igual(Inicio + BlocoDo(Sab) + BlocoDo(Dom) + nota + Fim, doc.Texto);
        }

        [Teste]
        private static void QuebraDePagina_DepoisDeCadaDia()
        {
            var (doc, _) = Executar(ModeloSemana, Loop(Ler(), Inserir(Escalas), Substituir(), Quebra()), Sab, Dom);

            Assert.Igual(Inicio + BlocoDo(Sab) + "\f\r" + BlocoDo(Dom) + "\f" + Fim, doc.Texto);
        }

        // Programa só com ler + substituir: a tabela do modelo recebe o 1.º dia
        [Teste]
        private static void SoLerESubstituir_PreencheATabelaDoModelo()
        {
            var (doc, lidos) = Executar(ModeloSemana, Loop(Ler(), Substituir()), Sab, Dom, Seg);

            Assert.Igual(Inicio + BlocoDo(Sab) + Fim, doc.Texto);
            Assert.Igual(3, lidos.Count, "dias lidos:");
        }

        [Teste]
        private static void InsercaoDesativada_NaoInsere()
        {
            var (doc, _) = Executar(ModeloSemana, Loop(Ler(), Inserir(Escalas, ativa: false), Substituir()), Sab, Dom);

            Assert.Igual(Inicio + BlocoDo(Sab) + Fim, doc.Texto);
        }

        // Um fragmento sem bloco de escala não toma o lugar da tabela do modelo
        [Teste]
        private static void FragmentoSemBloco_NaoSubstituiATabelaDoModelo()
        {
            var (doc, _) = Executar(ModeloSemana, Loop(Ler(), Inserir(Separador), Inserir(Escalas), Substituir()), Sab, Dom);

            Assert.Verdade(!doc.Texto.Contains('<'), "ficaram tags por preencher: " + doc.Texto);
            Assert.Igual(2, doc.Texto.Split("--- separador ---").Length - 1, "separadores:");
            Assert.Verdade(doc.Texto.IndexOf("Para o dia 01/10") < doc.Texto.IndexOf("Para o dia 02/10"), "dias fora de ordem");
            Assert.Verdade(doc.Texto.IndexOf("Para o dia 02/10") < doc.Texto.IndexOf("102."), "bloco depois do 102.");
        }

        // O fragmento só é aberto para ver se tem bloco enquanto a tabela do modelo está por usar
        [Teste]
        private static void VerificacaoDoFragmento_UmaVezSo()
        {
            var (doc, _) = Executar(ModeloSemana, ProgramaModelar.CriarProgramaClassico(Escalas), Sab, Dom, Seg);

            Assert.Igual(1, doc.VerificacoesDeBloco.GetValueOrDefault(Escalas), "verificações:");
        }
    }
}
