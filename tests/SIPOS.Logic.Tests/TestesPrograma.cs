namespace SIPOS.Logic.Tests
{
    internal static class TestesPrograma
    {
        private const string Escalas = @"C:\SIPOS\modelos_word\modelo_escalas.doc";

        private static readonly DateTime[] TresDias = { new DateTime(2022, 10, 1), new DateTime(2022, 10, 2), new DateTime(2022, 10, 3) };

        [Teste]
        private static void ProgramaClassico_EValido()
        {
            Assert.Igual(0, ProgramaModelar.CriarProgramaClassico(Escalas).Validar().Count, "problemas:");
        }

        [Teste]
        private static void Validar_DetetaNomesVaziosDuplicadosFicheirosELoopsVazios()
        {
            var programa = new ProgramaModelar
            {
                Acoes = new List<AcaoModelar>
                {
                    new AcaoModelar { Tipo = TipoDeAcao.LerEscalasDoDia, Nome = "  " },
                    new AcaoModelar { Tipo = TipoDeAcao.SubstituirVariaveis, Nome = "Preencher" },
                    new AcaoModelar { Tipo = TipoDeAcao.SubstituirVariaveis, Nome = " preencher " },
                    new AcaoModelar { Tipo = TipoDeAcao.InserirDocumento, Nome = "Tabela", Ficheiro = "" },
                    new AcaoModelar { Tipo = TipoDeAcao.LoopDias, Nome = "Loop" }
                }
            };

            List<string> problemas = programa.Validar();
            Assert.Igual(4, problemas.Count, string.Join(" | ", problemas));
        }

        [Teste]
        private static void ExpandirPlano_UmBlocoDeTresOperacoesPorDia()
        {
            List<OperacaoPlaneada> plano = ProgramaModelar.CriarProgramaClassico(Escalas).ExpandirPlano(TresDias);

            Assert.Igual(9, plano.Count, "operações:");
            Assert.Sequencia(
                new[] { TipoDeAcao.LerEscalasDoDia, TipoDeAcao.InserirDocumento, TipoDeAcao.SubstituirVariaveis },
                plano.Take(3).Select(o => o.Tipo));
            Assert.Sequencia(TresDias.SelectMany(d => new DateTime?[] { d, d, d }), plano.Select(o => o.Dia));
        }

        [Teste]
        private static void ExpandirPlano_SaltaInativasEAcoesDeTopoNaoTemDia()
        {
            var programa = new ProgramaModelar
            {
                Acoes = new List<AcaoModelar>
                {
                    new AcaoModelar { Tipo = TipoDeAcao.QuebraDePagina, Nome = "Quebra inicial" },
                    new AcaoModelar
                    {
                        Tipo = TipoDeAcao.LoopDias, Nome = "Loop",
                        Filhos = new List<AcaoModelar>
                        {
                            new AcaoModelar { Tipo = TipoDeAcao.LerEscalasDoDia, Nome = "Ler" },
                            new AcaoModelar { Tipo = TipoDeAcao.InserirDocumento, Nome = "Tabela", Ficheiro = Escalas, Ativa = false }
                        }
                    }
                }
            };

            List<OperacaoPlaneada> plano = programa.ExpandirPlano(TresDias);
            Assert.Igual(4, plano.Count, "operações:");
            Assert.Igual<DateTime?>(null, plano[0].Dia, "dia da ação de topo:");
            Assert.Verdade(plano.Skip(1).All(o => o.Tipo == TipoDeAcao.LerEscalasDoDia), "a ação desativada entrou no plano");
        }

        [Teste]
        private static void GuardarECarregar_IdaEVolta()
        {
            string caminho = Path.Combine(Path.GetTempPath(), $"sipos_teste_{Guid.NewGuid():N}.json");
            try
            {
                ProgramaModelar.CriarProgramaClassico(Escalas).Guardar(caminho);
                Assert.Verdade(File.ReadAllText(caminho).Contains("\"LoopDias\""), "o tipo deve ser gravado por extenso");

                ProgramaModelar? lido = ProgramaModelar.Carregar(caminho);
                Assert.Verdade(lido != null, "não carregou");
                Assert.Igual(3, lido!.Acoes[0].Filhos.Count, "filhos:");
                Assert.Igual(Escalas, lido.Acoes[0].Filhos[1].Ficheiro);
            }
            finally
            {
                File.Delete(caminho);
            }
        }

        [Teste]
        private static void Carregar_JsonCorrompidoOuTipoDesconhecido_DevolveNull()
        {
            Assert.Verdade(CarregarTexto("{ isto não é json") == null, "JSON corrompido devia dar null");
            Assert.Verdade(CarregarTexto("{\"Acoes\":[{\"Tipo\":\"Inventado\",\"Nome\":\"x\"}]}") == null, "tipo desconhecido devia dar null");
        }

        // JSON escrito à mão com null não pode rebentar a validação nem a expansão
        [Teste]
        private static void Carregar_NullsNoJson_SaoNormalizados()
        {
            ProgramaModelar? p = CarregarTexto("{\"Nome\":null,\"Acoes\":[{\"Tipo\":\"LoopDias\",\"Nome\":\"Loop\",\"Filhos\":null},null]}");
            Assert.Verdade(p != null, "não carregou");
            Assert.Igual(1, p!.Acoes.Count, "ações:");
            Assert.Igual(1, p.Validar().Count, "problemas (loop vazio):");
            Assert.Igual(0, p.ExpandirPlano(TresDias).Count, "operações:");

            ProgramaModelar? semAcoes = CarregarTexto("{\"Acoes\":null}");
            Assert.Igual(0, semAcoes!.Acoes.Count, "ações:");
        }

        [Teste]
        private static void EhEditavelNaLista()
        {
            Assert.Verdade(new ProgramaModelar().EhEditavelNaLista(), "programa vazio");
            Assert.Verdade(ProgramaModelar.CriarProgramaClassico(Escalas).EhEditavelNaLista(), "programa clássico");

            var doisDeTopo = ProgramaModelar.CriarProgramaClassico(Escalas);
            doisDeTopo.Acoes.Add(new AcaoModelar { Tipo = TipoDeAcao.QuebraDePagina, Nome = "Quebra" });
            Assert.Verdade(!doisDeTopo.EhEditavelNaLista(), "ação fora do loop");

            var aninhado = ProgramaModelar.CriarProgramaClassico(Escalas);
            aninhado.Acoes[0].Filhos.Add(new AcaoModelar { Tipo = TipoDeAcao.LoopDias, Nome = "Loop interior" });
            Assert.Verdade(!aninhado.EhEditavelNaLista(), "loop dentro do loop");
        }

        // O exemplo documentado em docs/ tem de carregar, validar e expandir
        [Teste]
        private static void ExemploDaDocumentacao_CarregaEValida()
        {
            string? pasta = AppContext.BaseDirectory;
            while (pasta != null && !File.Exists(Path.Combine(pasta, "docs", "modelar_programa.exemplo.json")))
            {
                pasta = Path.GetDirectoryName(pasta);
            }
            Assert.Verdade(pasta != null, "docs/modelar_programa.exemplo.json não encontrado");

            ProgramaModelar? exemplo = ProgramaModelar.Carregar(Path.Combine(pasta!, "docs", "modelar_programa.exemplo.json"));
            Assert.Verdade(exemplo != null, "o exemplo não carregou");
            Assert.Igual(0, exemplo!.Validar().Count, "problemas:");
            Assert.Igual(9, exemplo.ExpandirPlano(TresDias).Count, "operações:");
        }

        private static ProgramaModelar? CarregarTexto(string json)
        {
            string caminho = Path.Combine(Path.GetTempPath(), $"sipos_teste_{Guid.NewGuid():N}.json");
            try
            {
                File.WriteAllText(caminho, json);
                return ProgramaModelar.Carregar(caminho);
            }
            finally
            {
                File.Delete(caminho);
            }
        }
    }
}
