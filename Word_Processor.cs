using System;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using Word = Microsoft.Office.Interop.Word;
using System.Drawing.Text;
using LinqList;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Word;
using SIPOS;
using Microsoft.Office.Interop.Excel;
using Application = Microsoft.Office.Interop.Word.Application;
using System.Text;

namespace SIPOS
{

    public class Word_Processor
    {

        // PUBLIC VARS

        // VARS: PESSOAS / DETALHES
        //
        //
        static string efetivoODU = "", ptpdODU = "", adaptODU = "", resODU = "", statusODU = "";    //ODU
        static string efetivoCCS = "", ptpdCCS = "", adaptCCS = "", resCCS = "", statusCCS = "";    //CCS
        static string efetivoSD = "", ptpdSD = "", adaptSD = "", resSD = "", statusSD = "";         //SD
        static string efetivoPD = "", ptpdPD = "", adaptPD = "", resPD = "", statusPD = "";         //PD
        static string efetivoFN = "", ptpdFN = "", adaptFN = "", resFN = "", statusFN = "";         //FN

        int plusDay = 0; //ir buscar ao form

        // PARSING CHAR VARS
        //
        //
        static string returnChar = "\v";
        static string tabChar = "\t";



        // ------------------------
        // MÉTODOS FIND AND REPLACE
        //
        //
        public static void FindAndReplace(Word.Document osWordDoc, Word.Application wordApp, object ToFindText, object replaceWithText)
        {
            object matchCase = true;
            object matchWholeWord = true;
            object matchWildCards = false;
            object matchSoundLike = false;
            object nmatchAllforms = false;
            object forward = true;
            object format = false;
            object matchKashida = false;
            object matchDiactitics = false;
            object matchAlefHamza = false;
            object matchControl = false;
            object read_only = false;
            object visible = true;
            object replace = 2;
            object wrap = 1;
            object replaceAll = Microsoft.Office.Interop.Word.WdReplace.wdReplaceAll;

            wordApp.Selection.Find.Execute(ref ToFindText,
                ref matchCase, ref matchWholeWord,
                ref matchWildCards, ref matchSoundLike,
                ref nmatchAllforms, ref forward,
                ref wrap, ref format, ref replaceWithText,
                ref replace, ref matchKashida,
                ref matchDiactitics, ref matchAlefHamza,
                ref matchControl);
        }
        public static void FindAndReplaceHeader(Word.Document osWordDoc, Word.Application wordApp, object ToFindText, object replaceWithText)
        {

            object matchCase = true;
            object matchWholeWord = true;
            object matchWildCards = false;
            object matchSoundLike = false;
            object nmatchAllforms = false;
            object forward = true;
            object format = false;
            object matchKashida = false;
            object matchDiactitics = false;
            object matchAlefHamza = false;
            object matchControl = false;
            object read_only = false;
            object visible = true;
            object replace = 2;
            object wrap = 1;
            object replaceAll = Microsoft.Office.Interop.Word.WdReplace.wdReplaceAll;


            foreach (Microsoft.Office.Interop.Word.Range rng in osWordDoc.StoryRanges)
            {
                rng.Find.Execute(ref ToFindText,
                ref matchCase, ref matchWholeWord,
                ref matchWildCards, ref matchSoundLike,
                ref nmatchAllforms, ref forward,
                ref wrap, ref format, ref replaceWithText,
                ref replace, ref matchKashida,
                ref matchDiactitics, ref matchAlefHamza,
                ref matchControl);
            }
        }



        // CRIAR DOCUMENTO WORD DA OS
        //
        //
        public static void CreateWordDocument(object filename, object SaveAs)
        {
            Word.Application wordApp = new Word.Application();
            object missing = Missing.Value;
            Word.Document osWordDoc = null;

            if (File.Exists((string)filename))
            {
                object readOnly = false;
                object isVisible = Mediator.isExportVisible;
                wordApp.Visible = Mediator.isExportVisible;

                osWordDoc = wordApp.Documents.Open(ref filename, ref missing, ref readOnly,
                    ref missing, ref missing, ref missing,
                    ref missing, ref missing, ref missing,
                    ref missing, ref missing, ref missing,
                    ref missing, ref missing, ref missing, ref missing);
                osWordDoc.Activate();



                //-----------------
                // FIND AND REPLACE EXECUTERS


                // Escalas de Serviço
                for (int i = 0; i < 1; i++)
                {
                    listToVarsEscalados(Mediator.plusDayIntrup); // Procura escalados na lista


                    if (i == 0)
                    {

                        string osExtensiveDate = (string)Mediator.returnOSextensiveDate();
                        string osDateABVParse = (string)Mediator.returnOSDateABVParse();


                        // CABEÇALHOS E RODAPÉS
                        Word_Processor.FindAndReplaceHeader(osWordDoc, wordApp, "<numOS>", Mediator.osNumber);
                        Word_Processor.FindAndReplaceHeader(osWordDoc, wordApp, "<dataOS>", osExtensiveDate);
                        Word_Processor.FindAndReplaceHeader(osWordDoc, wordApp, "<dataOS_abv>", osDateABVParse);

                        // ALTERAR A PAGINAÇÃO 

                        string folderPath = Mediator.inspFilePath;

                        // Exibir o nome do arquivo anterior
                        string previousOSFileName = Mediator.GetPreviousOSFileName(Mediator.inspFilePath);
                        if (Mediator.winMode == 2) { MessageBox.Show("Nome do Arquivo Anterior: " + previousOSFileName, "Informação de Troubleshooting"); }

                        // Construir o caminho completo do último documento
                        string lastDocPath = Path.Combine(Mediator.inspFilePath, previousOSFileName + ".doc");
                        if (Mediator.winMode == 2) { MessageBox.Show("Caminho do Último Documento: " + lastDocPath, "Informação de Troubleshooting"); }




                        if (File.Exists(lastDocPath))
                        {
                            if (Mediator.winMode == 2) { MessageBox.Show("O arquivo existe.", "Informação de Troubleshooting"); }

                            int lastPageNumber = GetLastPageNumber(lastDocPath);
                            int afterLastPageLastOS = lastPageNumber % 2 == 0 ? lastPageNumber + 1 : lastPageNumber + 2;
                            if (Mediator.winMode == 2) { MessageBox.Show("Último Número de Página: " + lastPageNumber, "Informação de Troubleshooting"); }


                            // Set the starting page number
                            osWordDoc.Sections[1].Footers[Word.WdHeaderFooterIndex.wdHeaderFooterPrimary].PageNumbers.StartingNumber = afterLastPageLastOS;

                            if (Mediator.winMode == 2) { MessageBox.Show("Número de Página para Novo Documento: " + afterLastPageLastOS, "Informação de Troubleshooting"); }

                        }
                        else
                        {
                            MessageBox.Show("O arquivo não existe.", "Informação de Troubleshooting");
                        }

                    }

                    string diaDeEscalaParsedExt = (string)Mediator.returnEscaladosDateParse();
                    Word_Processor.FindAndReplace(osWordDoc, wordApp, "<dataEscalados>", diaDeEscalaParsedExt);

                    // ODU
                    Word_Processor.FindAndReplace(osWordDoc, wordApp, "<ODUefectivo>", efetivoODU); //listToVarsEscalados();)  // ODU
                    Word_Processor.FindAndReplace(osWordDoc, wordApp, "<ODUptpd>", ptpdODU);
                    Word_Processor.FindAndReplace(osWordDoc, wordApp, "<ODUadapt>", adaptODU);
                    Word_Processor.FindAndReplace(osWordDoc, wordApp, "<ODUstatus>", statusODU);
                    Word_Processor.FindAndReplace(osWordDoc, wordApp, "<ODUreserva>", resODU);
                    
                    // CCS
                    Word_Processor.FindAndReplace(osWordDoc, wordApp, "<CCSefectivo>", efetivoCCS);  // CCS
                    Word_Processor.FindAndReplace(osWordDoc, wordApp, "<CCSptpd>", ptpdCCS);
                    Word_Processor.FindAndReplace(osWordDoc, wordApp, "<CCSadapt>", adaptCCS);
                    Word_Processor.FindAndReplace(osWordDoc, wordApp, "<CCSstatus>", statusCCS);
                    Word_Processor.FindAndReplace(osWordDoc, wordApp, "<CCSreserva>", resCCS);

                    // SD
                    Word_Processor.FindAndReplace(osWordDoc, wordApp, "<SDefectivo>", efetivoSD);  // SD
                    Word_Processor.FindAndReplace(osWordDoc, wordApp, "<SDptpd>", ptpdSD);
                    Word_Processor.FindAndReplace(osWordDoc, wordApp, "<SDadapt>", adaptSD);
                    Word_Processor.FindAndReplace(osWordDoc, wordApp, "<SDstatus>", statusSD);
                    Word_Processor.FindAndReplace(osWordDoc, wordApp, "<SDreserva>", resSD);

                    // PD
                    Word_Processor.FindAndReplace(osWordDoc, wordApp, "<PDefectivo>", efetivoPD);  // PD
                    Word_Processor.FindAndReplace(osWordDoc, wordApp, "<PDptpd>", ptpdPD);
                    Word_Processor.FindAndReplace(osWordDoc, wordApp, "<PDadapt>", adaptPD);
                    Word_Processor.FindAndReplace(osWordDoc, wordApp, "<PDstatus>", statusPD);
                    Word_Processor.FindAndReplace(osWordDoc, wordApp, "<PDreserva>", resPD);


                    if (Mediator.isItQuarta == true)
                    {
                        // find and replace OAF
                        Word_Processor.FindAndReplace(osWordDoc, wordApp, "<OAFefectivo>", efetivoFN);  // FN
                        Word_Processor.FindAndReplace(osWordDoc, wordApp, "<OAFptpd>", ptpdFN);
                        Word_Processor.FindAndReplace(osWordDoc, wordApp, "<OAFadapt>", adaptFN);
                        Word_Processor.FindAndReplace(osWordDoc, wordApp, "<OAFstatus>", statusFN);
                        Word_Processor.FindAndReplace(osWordDoc, wordApp, "<OAFreserva>", resFN);
                    }


                    clearVars();
                }



            }
            else
            {
                MessageBox.Show("Ficheiro Templare do Word não encontrado!", "FICHEIRO INEXISTENTE!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Save As
            osWordDoc.SaveAs2(ref SaveAs, ref missing, ref missing, ref missing,
                            ref missing, ref missing, ref missing,
                            ref missing, ref missing, ref missing,
                            ref missing, ref missing, ref missing,
                            ref missing, ref missing, ref missing);
            osWordDoc.Close();
            wordApp.Quit();
            MessageBox.Show("Ficheiro criado com sucesso!", "EXPORTAÇÃO CONCLUÍDA", MessageBoxButtons.OK);

        }



        // APLICAR LISTA DE ESCALADOS
        // 
        //
        public static void listToVarsEscalados(int plusDay)
        {

            // E

            // DECLARAR LISTA
            List<Pessoa> nomeados = LinqList.ListaManagerEscalados.LoadList();
            string currentDate = Convert.ToString(Mediator.returnEscalaDate(plusDay));

            //foreach (var nomeado in nomeados){
            //	MessageBox.Show(nomeado.DataNomeado + " " + nomeado.EscalaNomeado + " " + nomeado.NomeNomeado + " " + nomeado.EstadoNomeado);
            //}

            //MessageBox.Show(currentDate);



            //// OFICIAL DE DIA
            ///
            //-------------
            // ODU Efectivo
            nomeados = nomeados.Where(x => x.DataNomeado == currentDate && x.EscalaNomeado == "Oficial de Dia" && x.EstadoNomeado == "Efetivo").ToList();

            foreach (var nomeado in nomeados) { efetivoODU = nomeado.NomeNomeado; }


            // ODU PTPD
            nomeados = LinqList.ListaManagerEscalados.LoadList();
            nomeados = nomeados.Where(x => x.DataNomeado == currentDate && x.EscalaNomeado == "Oficial de Dia" && (x.EstadoNomeado == "PT" || x.EstadoNomeado == "PD")).ToList();

            foreach (var nomeado in nomeados) { if (nomeado.NomeNomeado != "" && nomeado.NomeNomeado != null) { ptpdODU = returnChar + nomeado.NomeNomeado; } }


            // ODU ADAPT
            nomeados = LinqList.ListaManagerEscalados.LoadList();
            nomeados = nomeados.Where(x => x.DataNomeado == currentDate && x.EscalaNomeado == "Oficial de Dia" && x.EstadoNomeado == "Adaptação").ToList();

            foreach (var nomeado in nomeados) { if (nomeado.NomeNomeado != "" && nomeado.NomeNomeado != null) { adaptODU = returnChar + nomeado.NomeNomeado; } }


            // ODU RESERVA
            nomeados = LinqList.ListaManagerEscalados.LoadList();
            nomeados = nomeados.Where(x => x.DataNomeado == currentDate && x.EscalaNomeado == "Oficial de Dia" && x.EstadoNomeado == "Reserva").ToList();

            foreach (var nomeado in nomeados) { resODU = nomeado.NomeNomeado; }


            //// ------STATUS
            /// ---
            // ODU Status - PD/PT
            nomeados = LinqList.ListaManagerEscalados.LoadList();
            nomeados = nomeados.Where(x => x.DataNomeado == currentDate && x.EscalaNomeado == "Oficial de Dia" && (x.EstadoNomeado == "PT" || x.EstadoNomeado == "PD")).ToList();

            foreach (var nomeado in nomeados) { statusODU = returnChar + nomeado.EstadoNomeado; }


            // ODU Status - ADAPT
            nomeados = LinqList.ListaManagerEscalados.LoadList();
            nomeados = nomeados.Where(x => x.DataNomeado == currentDate && x.EscalaNomeado == "Oficial de Dia" && x.EstadoNomeado == "Adaptação").ToList();

            foreach (var nomeado in nomeados) { if (statusODU != "" && statusODU != null) { statusODU += returnChar + nomeado.EstadoNomeado; } else { statusODU = returnChar + nomeado.EstadoNomeado; } }

            ///-------------------------------------------------



            //// CCS
            ///
            //-------------
            // CCS Efectivo
            nomeados = LinqList.ListaManagerEscalados.LoadList();
            nomeados = nomeados.Where(x => x.DataNomeado == currentDate && x.EscalaNomeado == "CCS" && x.EstadoNomeado == "Efetivo").ToList();

            foreach (var nomeado in nomeados) { /*efetivoCCS = nomeado.NomeNomeado;*/ efetivoCCS = GetTextWithNoParagraphs(nomeado.NomeNomeado); }


            // CCS PTPD
            nomeados = LinqList.ListaManagerEscalados.LoadList();
            nomeados = nomeados.Where(x => x.DataNomeado == currentDate && x.EscalaNomeado == "CCS" && (x.EstadoNomeado == "PT" || x.EstadoNomeado == "PD")).ToList();

            foreach (var nomeado in nomeados) { if (nomeado.NomeNomeado != "" && nomeado.NomeNomeado != null) { ptpdCCS = returnChar + GetTextWithNoParagraphs(nomeado.NomeNomeado); } }


            // CCS ADAPT
            nomeados = LinqList.ListaManagerEscalados.LoadList();
            nomeados = nomeados.Where(x => x.DataNomeado == currentDate && x.EscalaNomeado == "CCS" && x.EstadoNomeado == "Adaptação").ToList();

            foreach (var nomeado in nomeados) { if (nomeado.NomeNomeado != "" && nomeado.NomeNomeado != null) { adaptCCS = returnChar + GetTextWithNoParagraphs(nomeado.NomeNomeado); } }


            // CCS RESERVA
            nomeados = LinqList.ListaManagerEscalados.LoadList();
            nomeados = nomeados.Where(x => x.DataNomeado == currentDate && x.EscalaNomeado == "CCS" && x.EstadoNomeado == "Reserva").ToList();

            foreach (var nomeado in nomeados) { resCCS = GetTextWithNoParagraphs(nomeado.NomeNomeado); }


            //// ------STATUS
            /// ---
            // CCS Status - PD/PT
            nomeados = LinqList.ListaManagerEscalados.LoadList();
            nomeados = nomeados.Where(x => x.DataNomeado == currentDate && x.EscalaNomeado == "CCS" && (x.EstadoNomeado == "PT" || x.EstadoNomeado == "PD")).ToList();

            foreach (var nomeado in nomeados) { statusCCS = returnChar + GetTextWithNoParagraphs(nomeado.EstadoNomeado); }


            // CCS Status - ADAPT
            nomeados = LinqList.ListaManagerEscalados.LoadList();
            nomeados = nomeados.Where(x => x.DataNomeado == currentDate && x.EscalaNomeado == "CCS" && x.EstadoNomeado == "Adaptação").ToList();

            foreach (var nomeado in nomeados) { if (statusCCS != "" && statusCCS != null) { statusCCS += returnChar + GetTextWithNoParagraphs(nomeado.EstadoNomeado); } else { statusCCS = returnChar + GetTextWithNoParagraphs(nomeado.EstadoNomeado); } }

            ///-------------------------------------------------



            //// SD
            ///
            //-------------
            // SD Efectivo
            nomeados = LinqList.ListaManagerEscalados.LoadList();
            nomeados = nomeados.Where(x => x.DataNomeado == currentDate && x.EscalaNomeado == "Sargento de Dia" && x.EstadoNomeado == "Efetivo").ToList();

            foreach (var nomeado in nomeados) { efetivoSD = nomeado.NomeNomeado; }


            // SD PTPD
            nomeados = LinqList.ListaManagerEscalados.LoadList();
            nomeados = nomeados.Where(x => x.DataNomeado == currentDate && x.EscalaNomeado == "Sargento de Dia" && (x.EstadoNomeado == "PT" || x.EstadoNomeado == "PD")).ToList();

            foreach (var nomeado in nomeados) { if (nomeado.NomeNomeado != "" && nomeado.NomeNomeado != null) { ptpdSD = returnChar + nomeado.NomeNomeado; } }


            // SD ADAPT
            nomeados = LinqList.ListaManagerEscalados.LoadList();
            nomeados = nomeados.Where(x => x.DataNomeado == currentDate && x.EscalaNomeado == "Sargento de Dia" && x.EstadoNomeado == "Adaptação").ToList();

            foreach (var nomeado in nomeados) { if (nomeado.NomeNomeado != "" && nomeado.NomeNomeado != null) { adaptSD = returnChar + nomeado.NomeNomeado; } }


            // SD RESERVA
            nomeados = LinqList.ListaManagerEscalados.LoadList();
            nomeados = nomeados.Where(x => x.DataNomeado == currentDate && x.EscalaNomeado == "Sargento de Dia" && x.EstadoNomeado == "Reserva").ToList();

            foreach (var nomeado in nomeados) { resSD = nomeado.NomeNomeado; }


            //// ------STATUS
            /// ---
            // SD Status - PD/PT
            nomeados = LinqList.ListaManagerEscalados.LoadList();
            nomeados = nomeados.Where(x => x.DataNomeado == currentDate && x.EscalaNomeado == "Sargento de Dia" && (x.EstadoNomeado == "PT" || x.EstadoNomeado == "PD")).ToList();

            foreach (var nomeado in nomeados) { statusSD = returnChar + nomeado.EstadoNomeado; }


            // SD Status - ADAPT
            nomeados = LinqList.ListaManagerEscalados.LoadList();
            nomeados = nomeados.Where(x => x.DataNomeado == currentDate && x.EscalaNomeado == "Sargento de Dia" && x.EstadoNomeado == "Adaptação").ToList();

            foreach (var nomeado in nomeados) { if (statusSD != "" && statusSD != null) { statusSD += returnChar + nomeado.EstadoNomeado; } else { statusSD = returnChar + nomeado.EstadoNomeado; } }

            ///-------------------------------------------------




            //// PD
            ///
            //-------------
            // PD Efectivo
            nomeados = LinqList.ListaManagerEscalados.LoadList();
            nomeados = nomeados.Where(x => x.DataNomeado == currentDate && x.EscalaNomeado == "Praça de Dia" && x.EstadoNomeado == "Efetivo").ToList();

            foreach (var nomeado in nomeados) { efetivoPD = nomeado.NomeNomeado; }


            // PD PTPD
            nomeados = LinqList.ListaManagerEscalados.LoadList();
            nomeados = nomeados.Where(x => x.DataNomeado == currentDate && x.EscalaNomeado == "Praça de Dia" && (x.EstadoNomeado == "PT" || x.EstadoNomeado == "PD")).ToList();

            foreach (var nomeado in nomeados) { if (nomeado.NomeNomeado != "" && nomeado.NomeNomeado != null) { ptpdPD = returnChar + nomeado.NomeNomeado; } }


            // PD ADAPT
            nomeados = LinqList.ListaManagerEscalados.LoadList();
            nomeados = nomeados.Where(x => x.DataNomeado == currentDate && x.EscalaNomeado == "Praça de Dia" && x.EstadoNomeado == "Adaptação").ToList();

            foreach (var nomeado in nomeados) { if (nomeado.NomeNomeado != "" && nomeado.NomeNomeado != null) { adaptPD = returnChar + nomeado.NomeNomeado; } }


            // PD RESERVA
            nomeados = LinqList.ListaManagerEscalados.LoadList();
            nomeados = nomeados.Where(x => x.DataNomeado == currentDate && x.EscalaNomeado == "Praça de Dia" && x.EstadoNomeado == "Reserva").ToList();

            foreach (var nomeado in nomeados) { resPD = nomeado.NomeNomeado; }


            //// ------STATUS
            /// ---
            // PD Status - PD/PT
            nomeados = LinqList.ListaManagerEscalados.LoadList();
            nomeados = nomeados.Where(x => x.DataNomeado == currentDate && x.EscalaNomeado == "Praça de Dia" && (x.EstadoNomeado == "PT" || x.EstadoNomeado == "PD")).ToList();

            foreach (var nomeado in nomeados) { statusPD = returnChar + nomeado.EstadoNomeado; }


            // PD Status - ADAPT
            nomeados = LinqList.ListaManagerEscalados.LoadList();
            nomeados = nomeados.Where(x => x.DataNomeado == currentDate && x.EscalaNomeado == "Praça de Dia" && x.EstadoNomeado == "Adaptação").ToList();

            foreach (var nomeado in nomeados) { if (statusPD != "" && statusPD != null) { statusPD += returnChar + nomeado.EstadoNomeado; } else { statusPD = returnChar + nomeado.EstadoNomeado; } }

            ///-------------------------------------------------



            //// OAF / FN
            ///
            //-------------
            // FN Efectivo
            nomeados = LinqList.ListaManagerEscalados.LoadList();
            nomeados = nomeados.Where(x => x.DataNomeado == currentDate && x.EscalaNomeado == "Honras Fúnebres" && x.EstadoNomeado == "Efetivo").ToList();

            foreach (var nomeado in nomeados) { efetivoFN = nomeado.NomeNomeado; }


            // FN PTPD
            nomeados = LinqList.ListaManagerEscalados.LoadList();
            nomeados = nomeados.Where(x => x.DataNomeado == currentDate && x.EscalaNomeado == "Honras Fúnebres" && (x.EstadoNomeado == "PT" || x.EstadoNomeado == "PD")).ToList();

            foreach (var nomeado in nomeados) { if (nomeado.NomeNomeado != "" && nomeado.NomeNomeado != null) { ptpdFN = returnChar + nomeado.NomeNomeado; } }


            // FN ADAPT
            nomeados = LinqList.ListaManagerEscalados.LoadList();
            nomeados = nomeados.Where(x => x.DataNomeado == currentDate && x.EscalaNomeado == "Honras Fúnebres" && x.EstadoNomeado == "Adaptação").ToList();

            foreach (var nomeado in nomeados) { if (nomeado.NomeNomeado != "" && nomeado.NomeNomeado != null) { adaptFN = returnChar + nomeado.NomeNomeado; } }


            // FN RESERVA
            nomeados = LinqList.ListaManagerEscalados.LoadList();
            nomeados = nomeados.Where(x => x.DataNomeado == currentDate && x.EscalaNomeado == "Honras Fúnebres" && x.EstadoNomeado == "Reserva").ToList();

            foreach (var nomeado in nomeados) { resFN = nomeado.NomeNomeado; }


            //// ------STATUS
            /// ---
            // FN Status - PD/PT
            nomeados = LinqList.ListaManagerEscalados.LoadList();
            nomeados = nomeados.Where(x => x.DataNomeado == currentDate && x.EscalaNomeado == "Honras Fúnebres" && (x.EstadoNomeado == "PT" || x.EstadoNomeado == "PD")).ToList();

            foreach (var nomeado in nomeados) { statusFN = returnChar + nomeado.EstadoNomeado; }


            // FN Status - ADAPT
            nomeados = LinqList.ListaManagerEscalados.LoadList();
            nomeados = nomeados.Where(x => x.DataNomeado == currentDate && x.EscalaNomeado == "Honras Fúnebres" && x.EstadoNomeado == "Adaptação").ToList();

            foreach (var nomeado in nomeados) { if (statusFN != "" && statusFN != null) { statusFN += returnChar + nomeado.EstadoNomeado; } else { statusFN = returnChar + nomeado.EstadoNomeado; } }

            ///-------------------------------------------------

        }


        // VERIFICADOR DE ESPAÇOS A MAIS
        //
        //
        public static string GetTextWithNoParagraphs(string textToParse)
        {
            string parsedText = textToParse;

            while (parsedText.Contains("\n") || parsedText.Contains("\r") || parsedText.Contains("\r\n") || parsedText.Contains("\v"))
            {
                parsedText = parsedText.Replace("\n", "")
                                          .Replace("\r", "")
                                          .Replace("\r\n", "")
                                          .Replace("\v", "");
            }
            return parsedText;
        }


        // LIMPAR VARIAVEIS
        //
        //
        public static void clearVars()
        {
            // VARS: PESSOAS/DETAILS
            efetivoODU = "";
            ptpdODU = "";
            adaptODU = "";
            resODU = "";
            statusODU = "";    //ODU

            efetivoCCS = "";
            ptpdCCS = "";
            adaptCCS = "";
            resCCS = "";
            statusCCS = "";    //CCS

            efetivoSD = "";
            ptpdSD = "";
            adaptSD = "";
            resSD = "";
            statusSD = "";         //SD

            efetivoPD = "";
            ptpdPD = "";
            adaptPD = "";
            resPD = "";
            statusPD = "";         //PD

            efetivoFN = "";
            ptpdFN = "";
            adaptFN = "";
            resFN = "";
            statusFN = "";         //FN
        }



        // APLICAR MODELO DE ESCALA
        //
        //
        public static void applyModeloEscala(Word.Application wordApp)
        {
            //Open the word model file
            Microsoft.Office.Interop.Word.Document wordDoc = wordApp.Documents.Open(@"C:\Users\Public\Documents\Word\escalas.docx");

            //Copy the text from the word file
            wordDoc.Select();
            wordDoc.Range().Copy();

            //Paste the text into the last line of the CreateWordDocument(object filename, object SaveAs) word document
            wordApp.Selection.EndKey();
            wordApp.Selection.TypeParagraph();
            wordApp.Selection.Paste();

            //Save the CreateWordDocument(object filename, object SaveAs) word document
            wordDoc.SaveAs2(@"C:\Users\Public\Documents\Word\escalas.docx");

            //Close the word file
            wordDoc.Close();
        }



        // DETECTAR A ÚLTIMA PÁGINA
        //
        //
        public static int GetLastPageNumber(string lastDocPath)
        {
            int lastPageNumber = 0;
            Word.Application wordApp = new Word.Application();
            object missing = Type.Missing;
            object readOnly = true;
            object isVisible = false;
            wordApp.Visible = false;

            object lastDocPathObj = lastDocPath;
            Word.Document lastDoc = wordApp.Documents.Open(ref lastDocPathObj, ref missing, ref readOnly,
                ref missing, ref missing, ref missing,
                ref missing, ref missing, ref missing,
                ref missing, ref missing, ref missing,
                ref missing, ref missing, ref missing, ref missing);
            lastDoc.Activate();

            // Get the last page number from the footer
            Word.Range lastPageRange = lastDoc.Range(lastDoc.Content.End - 1, lastDoc.Content.End);
            lastPageRange.Select();
            lastPageNumber = lastPageRange.Information[Word.WdInformation.wdActiveEndAdjustedPageNumber];



            //foreach (Word.Section section in lastDoc.Sections)
            //{
            //    Word.Range footerRange = section.Footers[Word.WdHeaderFooterIndex.wdHeaderFooterPrimary].Range;
            //    lastPageNumber = Math.Max(lastPageNumber, footerRange.Information[Word.WdInformation.wdActiveEndAdjustedPageNumber]);
            //}

            lastDoc.Close(ref missing, ref missing, ref missing);
            wordApp.Quit(ref missing, ref missing, ref missing);

            return lastPageNumber;
        }


        // SEM USO POR ENQUANTO -> APAGAR ASSIM QUE FOR CONFIRMADO QUE NÃO TEM USO
        static void detectLastPageNumber(string[] args)
        {
            string directory = @"C:\Documents";
            string[] files = Directory.GetFiles(directory, "*.doc");
            string latestFile = string.Empty;
            DateTime latestDate = DateTime.MinValue;

            foreach (string file in files)
            {
                DateTime creationDate = File.GetCreationTime(file);

                if (creationDate > latestDate)
                {
                    latestDate = creationDate;
                    latestFile = file;
                }
            }

            if (!string.IsNullOrEmpty(latestFile))
            {
                Application wordApp = new Application();
                Document doc = wordApp.Documents.Open(latestFile);
                int lastPageNumber = doc.Content.Information[WdInformation.wdNumberOfPagesInDocument];

                Console.WriteLine("Last page number: " + lastPageNumber);

                doc.Close();
                wordApp.Quit();
            }
            else
            {
                Console.WriteLine("No .doc files found in " + directory);
            }




        }


    }


}


