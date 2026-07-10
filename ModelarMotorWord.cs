using System.Runtime.InteropServices;
using Word = Microsoft.Office.Interop.Word;

namespace SIPOS
{
    /// <summary>
    /// Motor de execução Word do programa Modelar (B-1.5.2).
    ///
    /// Recebe o plano linear produzido por <see cref="ProgramaModelar.ExpandirPlano"/>
    /// e executa-o passo a passo contra o Microsoft Word:
    ///
    /// - InserirDocumento: abre o fragmento (ex.: modelo_escalas.doc) em modo
    ///   leitura, copia o conteúdo todo e cola-o no fim do documento final,
    ///   registando o range colado;
    /// - LerEscalasDoDia: aponta o Mediator para o dia da operação e corre a
    ///   triagem das folhas Excel para carregar os escalados desse dia;
    /// - SubstituirVariaveis: preenche as tags &lt;...&gt; APENAS dentro do último
    ///   range colado, com os valores do dia da operação (ao contrário do fluxo
    ///   clássico, que usa wdReplaceAll e dá o mesmo valor a todas as cópias);
    /// - QuebraDePagina: insere uma quebra de página no fim do documento.
    ///
    /// O documento base (modelo de semana/quarta) é aberto uma vez, os
    /// cabeçalhos/rodapés e a numeração de páginas são tratados como no fluxo
    /// clássico, e o Word é sempre fechado mesmo em erro (padrão endurecido
    /// da Beta 1.3.1).
    /// </summary>
    public static class ModelarMotorWord
    {
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

            Word.Application wordApp = new Word.Application();
            Word.Document doc = null;
            bool exportOk = false;

            try
            {
                wordApp.Visible = Mediator.isExportVisible;
                doc = wordApp.Documents.Open(caminhoModeloBase, ReadOnly: false, Visible: Mediator.isExportVisible);
                doc.Activate();

                // Cabeçalhos, rodapés e numeração — uma vez, como no fluxo clássico
                PrepararCabecalhosEPaginacao(doc, wordApp);

                // Estado do bloco colado mais recente (para substituição por âmbito)
                int inicioUltimoBloco = -1;
                int fimUltimoBloco = -1;

                foreach (OperacaoPlaneada op in plano)
                {
                    switch (op.Tipo)
                    {
                        case TipoDeAcao.LerEscalasDoDia:
                            LerEscalasDoDia(op.Dia);
                            break;

                        case TipoDeAcao.InserirDocumento:
                            InserirDocumentoNoFim(wordApp, doc, op.Ficheiro, out inicioUltimoBloco, out fimUltimoBloco);
                            break;

                        case TipoDeAcao.SubstituirVariaveis:
                            SubstituirVariaveisDaOperacao(doc, op.Dia, inicioUltimoBloco, fimUltimoBloco);
                            break;

                        case TipoDeAcao.QuebraDePagina:
                            InserirQuebraDePagina(doc);
                            break;
                    }
                }

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
                try { wordApp.Quit(); } catch { }
                try { Marshal.ReleaseComObject(wordApp); } catch { }
            }

            if (exportOk)
            {
                MessageBox.Show("Ficheiro criado com sucesso pelo programa Modelar!", "EXPORTAÇÃO CONCLUÍDA", MessageBoxButtons.OK);
            }
            return exportOk;
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

            string previousOSFileName = Mediator.GetPreviousOSFileName(Mediator.inspFilePath);
            if (previousOSFileName == null) { return; }

            string lastDocPath = Path.Combine(Mediator.inspFilePath, previousOSFileName + ".doc");
            if (!File.Exists(lastDocPath)) { return; }

            int lastPageNumber = Word_Processor.GetLastPageNumber(lastDocPath);
            int afterLastPageLastOS = lastPageNumber % 2 == 0 ? lastPageNumber + 1 : lastPageNumber + 2;
            doc.Sections[1].Footers[Word.WdHeaderFooterIndex.wdHeaderFooterPrimary].PageNumbers.StartingNumber = afterLastPageLastOS;
        }

        // LerEscalasDoDia: aponta o estado global de datas para o dia da operação
        // e corre a triagem Excel, acumulando os escalados desse dia na lista.
        private static void LerEscalasDoDia(DateTime? dia)
        {
            if (dia == null) { return; }

            Mediator.diaDeEscala = dia.Value;
            Mediator.escalaDay = dia.Value.ToString("dd-MM-yyyy");
            Mediator.isItSabado = dia.Value.DayOfWeek == DayOfWeek.Saturday;
            Mediator.isItQuarta = dia.Value.DayOfWeek == DayOfWeek.Wednesday;

            Mediator.instTriagemEscalas();
        }

        // InserirDocumento: copia o conteúdo do fragmento e cola-o no fim do
        // documento final, devolvendo o range [inicio, fim) do bloco colado.
        private static void InserirDocumentoNoFim(Word.Application wordApp, Word.Document doc, string caminhoFragmento, out int inicioBloco, out int fimBloco)
        {
            inicioBloco = -1;
            fimBloco = -1;

            if (!File.Exists(caminhoFragmento))
            {
                throw new FileNotFoundException($"Fragmento Word não encontrado: {caminhoFragmento}");
            }

            Word.Document fragmento = null;
            try
            {
                fragmento = wordApp.Documents.Open(caminhoFragmento, ReadOnly: true, Visible: false);
                fragmento.Range().Copy();
            }
            finally
            {
                try { fragmento?.Close(false); } catch { }
            }

            doc.Activate();
            Word.Range fim = doc.Range(doc.Content.End - 1, doc.Content.End - 1);
            int posicaoAntes = fim.Start;

            fim.Paste();

            inicioBloco = posicaoAntes;
            fimBloco = doc.Content.End;
        }

        // SubstituirVariaveis: preenche as tags do dia APENAS no último bloco
        // colado. Se ainda não houve colagem (programa sem InserirDocumento),
        // cai no documento inteiro para manter compatibilidade com modelos
        // clássicos que já trazem as tags no próprio modelo base.
        private static void SubstituirVariaveisDaOperacao(Word.Document doc, DateTime? dia, int inicioBloco, int fimBloco)
        {
            if (dia != null)
            {
                Mediator.diaDeEscala = dia.Value;
                Mediator.escalaDay = dia.Value.ToString("dd-MM-yyyy");
            }

            // Carrega as variáveis (efetivoODU, resCCS, ...) dos escalados do dia atual
            Word_Processor.listToVarsEscalados(0);

            Word.Range alvo = (inicioBloco >= 0 && fimBloco > inicioBloco)
                ? doc.Range(inicioBloco, Math.Min(fimBloco, doc.Content.End))
                : doc.Range();

            Word_Processor.SubstituirVariaveisNoRange(alvo, dia ?? Mediator.diaDeEscala);
        }

        private static void InserirQuebraDePagina(Word.Document doc)
        {
            Word.Range fim = doc.Range(doc.Content.End - 1, doc.Content.End - 1);
            fim.InsertBreak(Word.WdBreakType.wdPageBreak);
        }
    }
}
