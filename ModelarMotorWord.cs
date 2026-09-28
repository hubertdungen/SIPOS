using System.Runtime.InteropServices;
using Word = Microsoft.Office.Interop.Word;

namespace SIPOS
{
    /// <summary>
    /// Motor de execução Word do programa Modelar (B-1.5.2).
    ///
    /// Recebe o plano linear produzido por <see cref="ProgramaModelar.ExpandirPlano"/>
    /// e executa-o contra o Microsoft Word. A decisão de onde fica cada bloco é do
    /// <see cref="ExecutorModelar"/> (lógica pura, testável); aqui ficam as
    /// validações, a abertura/fecho do Word e as operações de documento:
    ///
    /// - LerEscalasDoDia: aponta o Mediator para o dia da operação e corre a
    ///   triagem das folhas Excel para carregar os escalados desse dia;
    /// - InserirDocumento: insere o ficheiro (ex.: modelo_escalas.doc) com
    ///   Range.InsertFile, sem usar a área de transferência;
    /// - SubstituirVariaveis: preenche as tags &lt;...&gt; APENAS na zona indicada,
    ///   com os valores do dia da operação (ao contrário do fluxo clássico, que
    ///   usa wdReplaceAll e dá o mesmo valor a todas as cópias);
    /// - QuebraDePagina: insere uma quebra de página.
    ///
    /// O documento base (modelo de semana/quarta) é aberto uma vez, os
    /// cabeçalhos/rodapés e a numeração de páginas são tratados como no fluxo
    /// clássico, o Word é sempre fechado mesmo em erro (padrão endurecido da
    /// Beta 1.3.1) e o estado global de datas do Mediator é reposto no fim.
    /// </summary>
    public static class ModelarMotorWord
    {
        /// <summary>Marcador do início de um bloco de escala ("Para o dia &lt;dataEscalados&gt;").</summary>
        internal const string MarcadorBlocoEscala = "<dataEscalados>";

        /// <summary>Marcador opcional, num parágrafo próprio do modelo base, do sítio onde entram os blocos dos dias seguintes.</summary>
        internal const string MarcadorFimEscalas = "<fimEscalas>";

        /// <summary>Título da secção que se segue ao "101. PESSOAL DE SERVIÇO" nas O.S.</summary>
        internal const string TituloSeguinteAsEscalas = "102.";

        /// <summary>
        /// Executa um programa Modelar completo.
        /// </summary>
        /// <param name="programa">Programa a executar (validado antes de correr).</param>
        /// <param name="dias">Dias selecionados que alimentam os LoopDias.</param>
        /// <param name="caminhoModeloBase">Modelo principal da O.S. (semana/quarta).</param>
        /// <param name="caminhoDestino">Caminho do .doc final a gravar.</param>
        /// <returns>true se a exportação foi concluída e gravada.</returns>
        public static bool Executar(ProgramaModelar programa, IReadOnlyList<DateTime> dias, string caminhoModeloBase, string caminhoDestino)
        {
            List<string> problemas = programa.Validar();
            if (problemas.Count > 0)
            {
                MessageBox.Show("O programa Modelar tem problemas e não pode ser executado:\r\n\r\n- " + string.Join("\r\n- ", problemas),
                    "PROGRAMA MODELAR INVÁLIDO!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!File.Exists(caminhoModeloBase))
            {
                MessageBox.Show("Ficheiro Template do Word não encontrado!", "FICHEIRO INEXISTENTE!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            List<OperacaoPlaneada> plano = programa.ExpandirPlano(dias);
            if (plano.Count == 0)
            {
                MessageBox.Show("O programa Modelar não tem ações ativas para executar.", "PROGRAMA MODELAR VAZIO!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Fragmentos em falta ou iguais ao modelo base: verificar antes de arrancar o Word
            List<string> problemasFicheiros = VerificarFragmentos(plano, caminhoModeloBase);
            if (problemasFicheiros.Count > 0)
            {
                MessageBox.Show("Não é possível executar o programa Modelar:\r\n\r\n- " + string.Join("\r\n- ", problemasFicheiros),
                    "FICHEIROS DO PROGRAMA MODELAR!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            var datasOriginais = new EstadoDeDatas();
            Word.Application? wordApp = null;
            Word.Document? doc = null;
            bool exportOk = false;

            try
            {
                wordApp = new Word.Application();
                wordApp.Visible = Mediator.isExportVisible;
                doc = wordApp.Documents.Open(caminhoModeloBase, ReadOnly: false, Visible: Mediator.isExportVisible);
                doc.Activate();

                // Cabeçalhos, rodapés e numeração — uma vez, como no fluxo clássico
                PrepararCabecalhosEPaginacao(doc, wordApp);

                new ExecutorModelar(new DocumentoWord(wordApp, doc), LerEscalasDoDia).Executar(plano);

                doc.SaveAs2(caminhoDestino);
                exportOk = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocorreu um erro durante a execução do programa Modelar:\r\n{ex.Message}", "ERRO NA EXPORTAÇÃO MODELAR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                try { doc?.Close(false); } catch { }
                try { wordApp?.Quit(); } catch { }
                if (wordApp != null)
                {
                    try { Marshal.ReleaseComObject(wordApp); } catch { }
                }
                Word_Processor.clearVars();
                datasOriginais.Repor();
            }

            if (exportOk)
            {
                MessageBox.Show("Ficheiro criado com sucesso pelo programa Modelar!", "EXPORTAÇÃO CONCLUÍDA", MessageBoxButtons.OK);
            }
            return exportOk;
        }

        // Fragmentos que não existem, ou que são o próprio modelo base (abri-lo
        // outra vez fecharia o documento em construção).
        private static List<string> VerificarFragmentos(List<OperacaoPlaneada> plano, string caminhoModeloBase)
        {
            var problemas = new List<string>();
            string baseCompleto = Path.GetFullPath(caminhoModeloBase);

            foreach (string ficheiro in plano.Where(op => op.Tipo == TipoDeAcao.InserirDocumento)
                                             .Select(op => op.Ficheiro)
                                             .Distinct(StringComparer.OrdinalIgnoreCase))
            {
                if (!File.Exists(ficheiro))
                {
                    problemas.Add($"Ficheiro Word não encontrado: {ficheiro}");
                }
                else if (string.Equals(Path.GetFullPath(ficheiro), baseCompleto, StringComparison.OrdinalIgnoreCase))
                {
                    problemas.Add($"O ficheiro a inserir é o próprio modelo base da O.S.: {ficheiro}");
                }
            }
            return problemas;
        }

        // Cabeçalhos/rodapés (<numOS>, <dataOS>, <dataOS_abv>) e numeração inicial
        // de páginas — mesma lógica do CreateWordDocument clássico.
        private static void PrepararCabecalhosEPaginacao(Word.Document doc, Word.Application wordApp)
        {
            string osExtensiveDate = (string)Mediator.returnOSextensiveDate();
            string osDateABVParse = (string)Mediator.returnOSDateABVParse();

            Word_Processor.FindAndReplaceHeader(doc, wordApp, "<numOS>", Mediator.osNumber);
            Word_Processor.FindAndReplaceHeader(doc, wordApp, "<dataOS>", osExtensiveDate);
            Word_Processor.FindAndReplaceHeader(doc, wordApp, "<dataOS_abv>", osDateABVParse);

            string? previousOSFileName = Mediator.GetPreviousOSFileName(Mediator.inspFilePath);
            if (previousOSFileName == null) { return; }

            string lastDocPath = Path.Combine(Mediator.inspFilePath, previousOSFileName + ".doc");
            if (!File.Exists(lastDocPath)) { return; }

            int lastPageNumber = Word_Processor.GetLastPageNumber(lastDocPath);
            int afterLastPageLastOS = lastPageNumber % 2 == 0 ? lastPageNumber + 1 : lastPageNumber + 2;
            doc.Sections[1].Footers[Word.WdHeaderFooterIndex.wdHeaderFooterPrimary].PageNumbers.StartingNumber = afterLastPageLastOS;
        }

        // LerEscalasDoDia: aponta o estado global de datas para o dia da operação
        // e corre a triagem Excel. As entradas desse dia que já estivessem na lista
        // (ex.: de um "Atualizar" no separador Dados) são retiradas antes, para o
        // dia não ficar em duplicado.
        private static void LerEscalasDoDia(DateTime dia)
        {
            ApontarDatasPara(dia);
            LinqList.ListaManagerEscalados.escaladosList.RemoveAll(p => p.DataNomeado == Mediator.escalaDay);

            Mediator.instTriagemEscalas();
        }

        private static void ApontarDatasPara(DateTime dia)
        {
            Mediator.diaDeEscala = dia;
            Mediator.escalaDay = dia.ToString("dd-MM-yyyy");
            Mediator.isItSabado = dia.DayOfWeek == DayOfWeek.Saturday;
            Mediator.isItQuarta = dia.DayOfWeek == DayOfWeek.Wednesday;
        }

        /// <summary>Operações de documento do <see cref="ExecutorModelar"/> sobre o Word.</summary>
        private sealed class DocumentoWord : IDocumentoModelar
        {
            private readonly Word.Application wordApp;
            private readonly Word.Document doc;

            public DocumentoWord(Word.Application wordApp, Word.Document doc)
            {
                this.wordApp = wordApp;
                this.doc = doc;
            }

            public int FimDoTexto => doc.Content.End - 1;

            public int InicioDoBlocoPorPreencher()
            {
                Word.Range? r = Procurar(0, MarcadorBlocoEscala);
                return r == null ? -1 : r.Paragraphs[1].Range.Start;
            }

            public int PontoDeInsercao(int aPartirDe)
            {
                Word.Range? marcador = Procurar(aPartirDe, MarcadorFimEscalas);
                if (marcador != null)
                {
                    Word.Range paragrafo = marcador.Paragraphs[1].Range;
                    int posicao = paragrafo.Start;
                    paragrafo.Delete();
                    return posicao;
                }

                // Primeiro "102." que abra um parágrafo (só espaços antes)
                int inicio = aPartirDe;
                Word.Range? titulo;
                while ((titulo = Procurar(inicio, TituloSeguinteAsEscalas)) != null)
                {
                    int inicioParagrafo = titulo.Paragraphs[1].Range.Start;
                    if (string.IsNullOrWhiteSpace(doc.Range(inicioParagrafo, titulo.Start).Text))
                    {
                        return inicioParagrafo;
                    }
                    inicio = titulo.End;
                }
                return FimDoTexto;
            }

            public bool FicheiroTemBlocoDeEscala(string caminho)
            {
                Word.Document? fragmento = null;
                try
                {
                    fragmento = wordApp.Documents.Open(caminho, ReadOnly: true, AddToRecentFiles: false, Visible: false);
                    return fragmento.Content.Text.Contains(MarcadorBlocoEscala);
                }
                finally
                {
                    try { fragmento?.Close(false); } catch { }
                }
            }

            public int InserirFicheiro(int posicao, string caminho)
            {
                return MedirVariacao(() =>
                {
                    int p = posicao;
                    // A meio ou no fim de um parágrafo com texto (ex.: fim do documento),
                    // abrir um parágrafo novo, senão o 1.º parágrafo inserido juntava-se a ele
                    if (p > 0 && doc.Range(p - 1, p).Text != "\r")
                    {
                        doc.Range(p, p).InsertParagraphAfter();
                        p++;
                    }
                    doc.Range(p, p).InsertFile(caminho);
                });
            }

            public int InserirQuebraDePagina(int posicao)
            {
                return MedirVariacao(() => doc.Range(posicao, posicao).InsertBreak(Word.WdBreakType.wdPageBreak));
            }

            public int SubstituirVariaveis(int inicio, int fim, DateTime? dia)
            {
                if (dia != null) { ApontarDatasPara(dia.Value); }

                // Carrega as variáveis (efetivoODU, resCCS, ...) dos escalados do dia atual
                Word_Processor.clearVars();
                Word_Processor.listToVarsEscalados(0);

                int fimValido = Math.Min(fim, doc.Content.End);
                int d = MedirVariacao(() => Word_Processor.SubstituirVariaveisNoRange(doc.Range(inicio, fimValido), dia ?? Mediator.diaDeEscala));

                Word_Processor.clearVars();
                return d;
            }

            // Procura o texto (maiúsculas exatas) a partir da posição; devolve o
            // range encontrado ou null.
            private Word.Range? Procurar(int aPartirDe, string texto)
            {
                int fim = doc.Content.End;
                if (aPartirDe >= fim) { return null; }

                Word.Range r = doc.Range(aPartirDe, fim);
                Word.Find find = r.Find;
                find.ClearFormatting();
                bool encontrado = find.Execute(FindText: texto,
                    MatchCase: true,
                    MatchWholeWord: false,
                    MatchWildcards: false,
                    Forward: true,
                    Wrap: Word.WdFindWrap.wdFindStop,
                    Format: false);
                return encontrado ? r : null;
            }

            private int MedirVariacao(Action alteracao)
            {
                int antes = doc.Content.End;
                alteracao();
                return doc.Content.End - antes;
            }
        }

        /// <summary>
        /// Guarda e repõe o estado global de datas do Mediator, que o motor
        /// altera dia a dia; sem isto, o separador Dados e uma exportação clássica
        /// seguinte ficavam a apontar para o último dia do loop.
        /// </summary>
        private sealed class EstadoDeDatas
        {
            private readonly DateTime diaDeEscala = Mediator.diaDeEscala;
            private readonly string escalaDay = Mediator.escalaDay;
            private readonly bool isItSabado = Mediator.isItSabado;
            private readonly bool isItQuarta = Mediator.isItQuarta;

            public void Repor()
            {
                Mediator.diaDeEscala = diaDeEscala;
                Mediator.escalaDay = escalaDay;
                Mediator.isItSabado = isItSabado;
                Mediator.isItQuarta = isItQuarta;
            }
        }
    }
}
