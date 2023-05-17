using SIPOS.Forms;
using Microsoft.Office.Interop.Excel;
using SIPOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Range = Microsoft.Office.Interop.Excel.Range;

namespace SIPOS
{
    internal class EscalasEngine
    {
        
        // General VARS
        private string filePath = "";
        private string joinedOutput = "";
        private string outputText = "";
        private string outputFullText = "";
        private string selectedEscala = "";
        public static string outputInitialText = "À espera que seleccione uma data de publicação da OS, para mostrar o pessoal escalado.";
        public static string escalaPreviewText = "";
        public static string osNumber = "";

        // Output Individual Strings
        bool efetivoTemPTPDporLinha = false;
        string[] EfectivoOutPTPDArray = { };
        string dateOut = "";
        string efectivoOut = "";
        string adaptOut = "";
        string state1Out = "";
        string state2Out = "";
        string state3Out = "";
        string reservaOut = "";


        ////////////////////////////////////////////////////// --------------------- //
        ////////////////////////////////////////////////////// -- ENGINE SEARCHER -- //
        ////////////////////////////////////////////////////// --------------------- //


        // TRIAGEM DE ESCALAS
        public void triagemEscalas()
        {


            //missingPathsChecker();
            int allWithErrors = 0;

            escalaPreviewText += "_____________________________________________\r\n" + $"A seguinte lista diz respeito aos militares nomeados para dia {Mediator.escalaDay}:\r\n\r\n";

            //if (nonePathMissing == true)
            //{
            // ODU
            Mediator.instTxtBox_Clear();
            selectedEscala = "Oficial de Dia";
            Mediator.pathErrorCheck(Mediator.fPathODU);
            if (Mediator.nonePathError == true) { checkRows(Mediator.fPathODU); }
            else
            {
                Mediator.nonePathError = true;
                allWithErrors++;
            }

            // CCS
            Mediator.instTxtBox_Clear();
            selectedEscala = "CCS";
            Mediator.pathErrorCheck(Mediator.fPathCCS);
            if (Mediator.nonePathError == true) { checkRows(Mediator.fPathCCS); }
            else
            {
                Mediator.nonePathError = true;
                allWithErrors++;
            }

            // SD
            Mediator.instTxtBox_Clear();
            selectedEscala = "Sargento de Dia";
            Mediator.pathErrorCheck(Mediator.fPathSD);
            if (Mediator.nonePathError == true) { checkRows(Mediator.fPathSD); }
            else
            {
                Mediator.nonePathError = true;
                allWithErrors++;
            }

            // PD
            Mediator.instTxtBox_Clear();
            selectedEscala = "Praça de Dia";
            Mediator.pathErrorCheck(Mediator.fPathPD);
            if (Mediator.nonePathError == true) { checkRows(Mediator.fPathPD); }
            else
            {
                Mediator.nonePathError = true;
                allWithErrors++;
            }

            // FUNERAIS
            Mediator.instTxtBox_Clear();
            selectedEscala = "Honras Fúnebres";
            Mediator.pathErrorCheck(Mediator.fPathFunerais);
            if (Mediator.nonePathError == true) { checkRows(Mediator.fPathFunerais); }
            else
            {
                Mediator.nonePathError = true;
                allWithErrors++;
                Mediator.instPrgBarToMax();
            }

            if (allWithErrors == 5)
            {
                Mediator.instTxtBox_Equal_To("Não existem ficheiros carregados no sistema. Ou inseriu mal os caminhos dos ficheiros excel, ou esses ficheiros já não existem no local.");
                Mediator.instPrgBarReset();
            }

            // FIX THE LOAD BAR
            Mediator.instPrgBarFix();

        }
        // -----------------------------

        // CHECK THE ROWS
        public void checkRows(string filePathSelected)
        {
            outputText = "";  // Clearing the TEXT

            Mediator.instPrgBarAddInc(0);  // progress bar add inc
            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            Workbook wb;
            Worksheet ws;

            wb = excel.Workbooks.Open(filePathSelected, false, true);
            ws = wb.Worksheets[1];

            Range searchedRange = excel.get_Range("B15", "K52");

            // LINE FINDER
            Range currentFind = searchedRange.Find(Mediator.escalaDay);

            string displayResult = "";
            //string displayResultOut = "";
            List<string> ResultOutList = new List<string>();
            Mediator.instPrgBarAddInc(0);  // progress bar add inc

            // -- CALLER
            // ------------------------- //
            if (currentFind != null)
            {
                displayResult = "Found at \ncolumn - " + currentFind.Column +  // Debuger
                                            "\nrow - " + currentFind.Row;

                // Data Values - Index Identifiers
                int colmn = currentFind.Column;
                int rowm = currentFind.Row;

                // Individual Identifiers
                Range dateCell = ws.Cells[rowm, colmn];
                Range efectivoCell = ws.Cells[rowm, "E"];
                Range stateCell1 = ws.Cells[rowm, "J"];
                Range stateCell2 = ws.Cells[rowm + 1, "J"];
                Range stateCell3 = ws.Cells[rowm + 2, "J"];
                Range reservaCell = ws.Cells[rowm, "K"];

                int smartAdaptIncrementer = 1;
                if (Convert.ToString(stateCell3.Value) == "ADPT") { smartAdaptIncrementer = 2; }   // SE "ADPT" está na ROW 3, O adaptCell COMPENSA +1 ROW
                Range adaptCell = ws.Cells[rowm + smartAdaptIncrementer, "E"];




                //string textToParse = "";
                Mediator.instPrgBarAddInc(0);  // progress bar add inc

                //List<string> variableTextOutsList = new List<string> ();   // LISTA COM VARIAVEIS DE STRING OUTPUT


                // PROCESSADORES DE VALORES INDIVIDUAIS
                // ------------------------------------

                // Data
                string dateOut = dateCell.Value;

                // Pessoal Efectivo
                efectivoOut = Convert.ToString(efectivoCell.Value);
                namesFormater(efectivoOut);
                efectivoOut = outputText;

                // Pessoal em Adaptação
                adaptOut = Convert.ToString(adaptCell.Value);
                namesFormater(adaptOut);
                adaptOut = outputText;

                // Pessoal de Troca ou Destroca
                state1Out = Convert.ToString(stateCell1.Value);
                if (state1Out == null) { state1Out = ""; }

                // Verificador de Troca ou Destroca ou Adaptação
                state2Out = Convert.ToString(stateCell2.Value);
                if (state2Out == null) { state2Out = ""; }

                // Verificador de Adaptação caso haja PT ou PD
                state3Out = Convert.ToString(stateCell3.Value);
                if (state3Out == null) { state3Out = ""; }

                // Pessoal de Reserva
                string reservaCellValue = reservaCell.Value;
                namesFormater(reservaCellValue);
                reservaOut = outputText;

                // TENTATIVA DE CANCELAR O LOOP DE EXECUÇÃO DO EXCEL
                //System.Runtime.InteropServices.Marshal.ReleaseComObject(searchedRange);
                //System.Runtime.InteropServices.Marshal.ReleaseComObject(currentFind);
                //System.Runtime.InteropServices.Marshal.ReleaseComObject(dateCell);
                //System.Runtime.InteropServices.Marshal.ReleaseComObject(efectivoCell);
                //System.Runtime.InteropServices.Marshal.ReleaseComObject(stateCell1);
                //System.Runtime.InteropServices.Marshal.ReleaseComObject(stateCell2);
                //System.Runtime.InteropServices.Marshal.ReleaseComObject(stateCell3);
                //System.Runtime.InteropServices.Marshal.ReleaseComObject(reservaCell);
                //System.Runtime.InteropServices.Marshal.ReleaseComObject(adaptCell);

                //System.Runtime.InteropServices.Marshal.ReleaseComObject(ws);
                wb.Close(true);
                //System.Runtime.InteropServices.Marshal.ReleaseComObject(wb);
                excel.Quit();          // QUIT EXCEL
                //System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);

                // WHILE LOOP TO CERTIFY THAT THE OBJECTS ARE RELEASED
                while (Marshal.ReleaseComObject(ws) != 0) ;
                while (Marshal.ReleaseComObject(wb) != 0) ;
                while (Marshal.ReleaseComObject(excel) != 0) ;

                GC.Collect();
                GC.WaitForPendingFinalizers();

                // Formatador de Texto do Bloco Individual duma Escala de Serviço


                escalaPreviewFormater();
                outputFullText.Replace("\n", "\r\n");
                Mediator.instTxtBox_Equal_To(outputFullText);
            }
            else  // CASO NÃO ENCONTRE A DATA SELECCIONADA NA FOLHA QUESTÃO
            {
                //System.Runtime.InteropServices.Marshal.ReleaseComObject(searchedRange);
                //System.Runtime.InteropServices.Marshal.ReleaseComObject(currentFind);
                //System.Runtime.InteropServices.Marshal.ReleaseComObject(ws);
                wb.Close(true);
                //System.Runtime.InteropServices.Marshal.ReleaseComObject(wb);
                excel.Quit();          // QUIT EXCEL
                //System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);

                while (Marshal.ReleaseComObject(ws) != 0) ;
                while (Marshal.ReleaseComObject(wb) != 0) ;
                while (Marshal.ReleaseComObject(excel) != 0) ;

                GC.Collect();
                GC.WaitForPendingFinalizers();

                displayResult = "A data que procurou: \"" + Mediator.escalaDay +
                        $"\" não existe na lista de {selectedEscala}.";


                escalaPreviewText = escalaPreviewText + $"\r\nA escala de {selectedEscala} não tem registos para o dia {Mediator.escalaDay}.\r\n\r\n";
                Mediator.instTxtBox_Equal_To(escalaPreviewText);
            }

            Mediator.instPrgBarAddInc(0);  // progress bar add inc
        }
        // -----------------------------



        ////////////////////////////////////////////////////// --------------------- // 
        ////////////////////////////////////////////////////// ------ PARSERS ------ //
        ////////////////////////////////////////////////////// --------------------- //

        // DATA FORMATER
        private void namesFormater(string textToParse)
        {
            {
                //stringInput = "30-10-2022\n1CAB/SAS/141368-G IÚRI PARREIRA\nTEN/SAS/140976-L CATARINA AUGUSTO\nTCOR/ABST/142343-O SARA COSTA\n2SAR/OPINF/139108-A PEDRO MANUEL";
                //stringOutput = "1CAB\tSAS\t141368 G – I. PARREIRA\nTEN\tSAS\t140976 L – C. AUGUSTO\nTCOR\tABST\t142343 G – S. COSTA\n2SAR\tOPINF\t139108 A – P. MANUEL";
                outputText = "";

                // Invocar o string builder
                StringBuilder outputBuilder = new StringBuilder();


                if (Mediator.winMode == 2) { MessageBox.Show(textToParse, "BEFORE PARSING PHRASE"); }

                if ((textToParse != null) && (textToParse.Length > 10))
                {
                    if (textToParse.Contains("\n"))
                    {
                        string[] lines = textToParse.Split("\n");

                        //string[] lines = textToParse.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

                        foreach (string line in lines)
                        {
                            if (line.Length > 10)
                            {

                                string lineFinished = line;

                                while (lineFinished.Contains("  "))
                                {
                                    lineFinished = lineFinished.Replace("  ", " ");

                                }
                                if (lineFinished.Contains("\n"))
                                {
                                    string[] lines2 = lineFinished.Split("\n");
                                }


                                string[] parts = lineFinished.Split(" ");

                                parts[0] = parts[0].Replace("/", "\t");
                                parts[0] = Regex.Replace(parts[0], "-(?!.*-)", " ");
                                parts[1] = parts[1].Substring(0, 1);
                                //outputText += parts[0] + " – " + parts[1] + ". " + parts[2] + "\n";
                                outputBuilder.Append(parts[0] + " – " + parts[1] + ". " + parts[2]);
                                outputBuilder.Append(Environment.NewLine);
                                outputText = outputBuilder.ToString();

                                outputText = Mediator.doubleReturnsRemover(outputText);
                            }
                        }


                    }
                    else
                    {
                        string line = textToParse;
                        while (line.Contains("  "))
                        {
                            line = line.Replace("  ", " ");

                        }
                    string[] parts = line.Split(" ");

                    parts[0] = parts[0].Replace("/", "\t");
                    parts[0] = Regex.Replace(parts[0], "-(?!.*-)", " ");
                    parts[1] = parts[1].Substring(0, 1);
                    outputText += parts[0] + " – " + parts[1] + ". " + parts[2];
                    }
                }
                else
                {
                    outputText = "";
                }

                if (Mediator.winMode == 2) { MessageBox.Show(outputText, "AFTER PARSING PHRASE"); }

            }
        }
        // -----------------------------

        // FULL SINGULAR ESCALAS FORMATER
        private void escalaPreviewFormater()
        {

            // CREATE VARS
            string contextEfectivo = "";
            string contextPTPD = "";
            string contextAdapt = "";
            string contextReserva = "";
            dateOut = Mediator.escalaDay;


            efectivoPorPTPDsplitter(); // SEPARADOR DE LINHAS

            //MessageBox.Show(EfectivoOutPTPDArray[0] + " e " + EfectivoOutPTPDArray[1]);

            // CASO HAJA POR TROCA OU POR DESTROCA
            if ((state1Out.Contains("PT") || state1Out.Contains("PD") || state2Out.Contains("PT") || state2Out.Contains("PD")) && ((!state3Out.Contains("ADPT")) && (!state2Out.Contains("ADPT"))))
            {
                if (efetivoTemPTPDporLinha == true)
                {
                    contextEfectivo = $"{selectedEscala} Efetivo:\r\n{EfectivoOutPTPDArray[0]}\r\n";
                    //SIPOS.Escalados.escaladosList.Add(new Pessoa { DataNomeado = dateOut, NomeNomeado = EfectivoOutPTPDArray[0] });
                    LinqList.ListaManagerEscalados.escaladosList.Add(new LinqList.Pessoa { DataNomeado = dateOut, EscalaNomeado = selectedEscala, EstadoNomeado = "Efetivo", NomeNomeado = EfectivoOutPTPDArray[0] });

                    if (state1Out.Contains("PT") || state2Out.Contains("PT"))
                    {
                        contextPTPD = $"POR TROCA o:\r\n{EfectivoOutPTPDArray[1]}\r\n";
                        LinqList.ListaManagerEscalados.escaladosList.Add(new LinqList.Pessoa { DataNomeado = dateOut, EscalaNomeado = selectedEscala, EstadoNomeado = "PT", NomeNomeado = EfectivoOutPTPDArray[1] });
                    }
                    else // SE FOR POR DESTROCA
                    {
                        contextPTPD = $"POR DESTROCA o:\r\n{EfectivoOutPTPDArray[1]}\r\n";
                        LinqList.ListaManagerEscalados.escaladosList.Add(new LinqList.Pessoa { DataNomeado = dateOut, EscalaNomeado = selectedEscala, EstadoNomeado = "PD", NomeNomeado = EfectivoOutPTPDArray[1] });
                    }
                }
                else  // SE NÃO ESTIVER NA MESMA CELULA
                {
                    contextEfectivo = $"{selectedEscala} Efetivo:\r\n{efectivoOut}\r\n";
                    LinqList.ListaManagerEscalados.escaladosList.Add(new LinqList.Pessoa { DataNomeado = dateOut, EscalaNomeado = selectedEscala, EstadoNomeado = "Efetivo", NomeNomeado = efectivoOut });

                    if (state1Out.Contains("PT") || state2Out.Contains("PT"))
                    {
                        contextAdapt = $"POR TROCA o:\r\n{adaptOut}\r\n";
                        LinqList.ListaManagerEscalados.escaladosList.Add(new LinqList.Pessoa { DataNomeado = dateOut, EscalaNomeado = selectedEscala, EstadoNomeado = "PT", NomeNomeado = adaptOut });
                    }
                    else // SE FOR POR DESTROCA
                    {
                        contextAdapt = $"POR DESTROCA o:\r\n{adaptOut}\r\n";
                        LinqList.ListaManagerEscalados.escaladosList.Add(new LinqList.Pessoa { DataNomeado = dateOut, EscalaNomeado = selectedEscala, EstadoNomeado = "PD", NomeNomeado = adaptOut });
                    }
                }
            }
            // CASO TENHA APENAS ADAPTAÇÃO SEM PD OU PT
            else if (state1Out.Contains("ADPT") || state2Out.Contains("ADPT"))
            {
                if (efetivoTemPTPDporLinha == true)
                {
                    contextEfectivo = $"{selectedEscala} Efectivo:\r\n{EfectivoOutPTPDArray[0]}\r\n";
                    contextAdapt = $"O seguinte militar está em Adaptação:\r\n{EfectivoOutPTPDArray[1]}\r\n";

                    LinqList.ListaManagerEscalados.escaladosList.Add(new LinqList.Pessoa { DataNomeado = dateOut, EscalaNomeado = selectedEscala, EstadoNomeado = "Efetivo", NomeNomeado = EfectivoOutPTPDArray[0] });
                    LinqList.ListaManagerEscalados.escaladosList.Add(new LinqList.Pessoa { DataNomeado = dateOut, EscalaNomeado = selectedEscala, EstadoNomeado = "Adaptação", NomeNomeado = EfectivoOutPTPDArray[1] });
                }
                else  // SE NÃO ESTIVER NA MESMA CELULA
                {
                    contextEfectivo = $"{selectedEscala} Efectivo:\r\n{efectivoOut}\r\n";
                    contextAdapt = $"O seguinte militar está em Adaptação:\r\n{adaptOut}\r\n";

                    LinqList.ListaManagerEscalados.escaladosList.Add(new LinqList.Pessoa { DataNomeado = dateOut, EscalaNomeado = selectedEscala, EstadoNomeado = "Efetivo", NomeNomeado = efectivoOut });
                    LinqList.ListaManagerEscalados.escaladosList.Add(new LinqList.Pessoa { DataNomeado = dateOut, EscalaNomeado = selectedEscala, EstadoNomeado = "Adaptação", NomeNomeado = adaptOut });
                }
            }
            // CASO TENHA PD OU PT com ADPT
            else if ((state1Out.Contains("PT") || state1Out.Contains("PD") || state2Out.Contains("PT") || state2Out.Contains("PD")) && (state3Out.Contains("ADPT")))
            {
                if (efetivoTemPTPDporLinha == true)
                {
                    contextEfectivo = $"{selectedEscala} Efectivo:\r\n{EfectivoOutPTPDArray[0]}\r\n";
                    LinqList.ListaManagerEscalados.escaladosList.Add(new LinqList.Pessoa { DataNomeado = dateOut, EscalaNomeado = selectedEscala, EstadoNomeado = "Efetivo", NomeNomeado = EfectivoOutPTPDArray[0] });

                    if (state1Out.Contains("PT") || state2Out.Contains("PT"))
                    {
                        contextPTPD = $"POR TROCA o:\r\n{EfectivoOutPTPDArray[1]}\r\n";
                        contextAdapt = $"O seguinte militar está em Adaptação:\r\n{adaptOut}\r\n";

                        LinqList.ListaManagerEscalados.escaladosList.Add(new LinqList.Pessoa { DataNomeado = dateOut, EscalaNomeado = selectedEscala, EstadoNomeado = "PT", NomeNomeado = EfectivoOutPTPDArray[1] });
                        LinqList.ListaManagerEscalados.escaladosList.Add(new LinqList.Pessoa { DataNomeado = dateOut, EscalaNomeado = selectedEscala, EstadoNomeado = "Adaptação", NomeNomeado = adaptOut });
                    }
                    else // SE FOR POR DESTROCA
                    {
                        contextPTPD = $"POR DESTROCA o:\r\n{EfectivoOutPTPDArray[1]}\r\n";
                        contextAdapt = $"O seguinte militar está em Adaptação:\r\n{adaptOut}\r\n";

                        LinqList.ListaManagerEscalados.escaladosList.Add(new LinqList.Pessoa { DataNomeado = dateOut, EscalaNomeado = selectedEscala, EstadoNomeado = "PD", NomeNomeado = EfectivoOutPTPDArray[1] });
                        LinqList.ListaManagerEscalados.escaladosList.Add(new LinqList.Pessoa { DataNomeado = dateOut, EscalaNomeado = selectedEscala, EstadoNomeado = "Adaptação", NomeNomeado = adaptOut });
                    }
                }
            }
            // CASO NÃO HAJAM PTs / PDs / ADPTs
            else if ((!state1Out.Contains("PT") && !state1Out.Contains("PD") && !state2Out.Contains("PT") && !state2Out.Contains("PD")) && (!state1Out.Contains("ADPT") && !state2Out.Contains("ADPT") && !state3Out.Contains("ADPT")))
            {
                contextEfectivo = $"{selectedEscala} Efectivo:\r\n{efectivoOut}\r\n";
                LinqList.ListaManagerEscalados.escaladosList.Add(new LinqList.Pessoa { DataNomeado = dateOut, EscalaNomeado = selectedEscala, EstadoNomeado = "Efetivo", NomeNomeado = efectivoOut });
            }

            contextReserva = $"{selectedEscala} de Reserva:\r\n{reservaOut}\r\n";
            LinqList.ListaManagerEscalados.escaladosList.Add(new LinqList.Pessoa { DataNomeado = dateOut, EscalaNomeado = selectedEscala, EstadoNomeado = "Reserva", NomeNomeado = reservaOut });

            //MessageBox.Show(efe)

            escalaPreviewText += ($"Estão nomeados para a escala de {selectedEscala} os seguintes militares:\r\n" + contextEfectivo + contextPTPD + contextAdapt + contextReserva + "\r\n");
            //escalaPreviewText.Replace("\n", "\r\n");
            if (Mediator.winMode == 1) { MessageBox.Show(escalaPreviewText, "Pessoal escalado"); }
            outputFullText = escalaPreviewText;




            //textBox_Output.AppendText(contextEfectivoPD + Environment.NewLine);
            //textBox_Output.AppendText(contextPTPD + Environment.NewLine);
            //textBox_Output.AppendText(contextAdapt + Environment.NewLine);
            //textBox_Output.AppendText(contextReserva + Environment.NewLine);



        }

        // SPLITTER DOS EFECTIVOS / PT / PD
        private void efectivoPorPTPDsplitter()
        {
            if (efectivoOut.Contains("\n"))
            {
                efetivoTemPTPDporLinha = true;
                List<string> EfectivoOuPTPDList = new List<string>();

                string[] lines = efectivoOut.Split("\n");

                foreach (string line in lines)
                {
                    if (line != null || line != "")
                    {
                        EfectivoOuPTPDList.Add(line);
                        if (Mediator.winMode == 2) { MessageBox.Show(line, "RESULTADO DO TEXTO SEPARADO"); }
                    }

                }
                String[] EfectivoOuPTPDArrayTemp = EfectivoOuPTPDList.ToArray();
                EfectivoOutPTPDArray = EfectivoOuPTPDArrayTemp;
            }
            else
            {
                efetivoTemPTPDporLinha = false;
            }
        }

        // -----------------------------
        // --------------------------------------------------------------------------
        // --------------------------------------------------------------------------

        




    }
}
