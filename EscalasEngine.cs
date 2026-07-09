using Microsoft.Office.Interop.Excel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using Range = Microsoft.Office.Interop.Excel.Range;

namespace SIPOS
{
    internal class EscalasEngine
    {

        // General VARS
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

            // FIX THE LOAD BAR
            Mediator.instPrgBarFix();

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
            //Mediator.instTxtBox_Clear();
            selectedEscala = "CCS";
            Mediator.pathErrorCheck(Mediator.fPathCCS);
            if (Mediator.nonePathError == true) { checkRows(Mediator.fPathCCS); }
            else
            {
                Mediator.nonePathError = true;
                allWithErrors++;
            }

            // SD
            //Mediator.instTxtBox_Clear();
            selectedEscala = "Sargento de Dia";
            Mediator.pathErrorCheck(Mediator.fPathSD);
            if (Mediator.nonePathError == true) { checkRows(Mediator.fPathSD); }
            else
            {
                Mediator.nonePathError = true;
                allWithErrors++;
            }

            // PD
            //Mediator.instTxtBox_Clear();
            selectedEscala = "Praça de Dia";
            Mediator.pathErrorCheck(Mediator.fPathPD);
            if (Mediator.nonePathError == true) { checkRows(Mediator.fPathPD); }
            else
            {
                Mediator.nonePathError = true;
                allWithErrors++;
            }

            // FUNERAIS
            //Mediator.instTxtBox_Clear();
            selectedEscala = "Honras Fúnebres";
            Mediator.pathErrorCheck(Mediator.fPathFunerais);
            if (Mediator.nonePathError == true) { checkRows(Mediator.fPathFunerais); Mediator.instPrgBarToMax(); }
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
            //Mediator.instPrgBarFix();

        }
        // -----------------------------



        // MOTOR DE PESQUISA DE COLUNAS
        private Dictionary<string, int> FindColumnIndexes(Worksheet ws, int startRow = 1, int endRow = 20)
        {
            Dictionary<string, int> columnIndexes = new Dictionary<string, int>();

            if (Mediator.autoExcelSearch)
            {
                int lastColumn = ws.UsedRange.Columns.Count;

                for (int row = startRow; row <= endRow; row++)
                {
                    Range headerRow = ws.Range[ws.Cells[row, 1], ws.Cells[row, lastColumn]];
                    foreach (Range cell in headerRow.Cells)
                    {
                        string headerText = cell.Value?.ToString().Trim().ToUpper();
                        if (!string.IsNullOrEmpty(headerText))
                        {
                            if (headerText == Mediator.autoExcelData.ToUpper())
                                columnIndexes["Date"] = cell.Column;
                            else if (headerText == Mediator.autoExcelEfetivo.ToUpper())
                                columnIndexes["Efectivo"] = cell.Column;
                            else if (headerText == Mediator.autoExcelReserva.ToUpper())
                                columnIndexes["Reserva"] = cell.Column;
                        }
                    }

                    if (columnIndexes.Count == 3)
                        break;
                }
            }

            // Apply forced indexes for any columns that weren't found
            if (!columnIndexes.ContainsKey("Date"))
                columnIndexes["Date"] = GetColumnIndexFromLetter(Mediator.forcedExcelData);
            if (!columnIndexes.ContainsKey("Efectivo"))
                columnIndexes["Efectivo"] = GetColumnIndexFromLetter(Mediator.forcedExcelEfetivo);
            if (!columnIndexes.ContainsKey("Reserva"))
                columnIndexes["Reserva"] = GetColumnIndexFromLetter(Mediator.forcedExcelReserva);


            //System.Windows.Forms.MessageBox.Show($"Date column index: {columnIndexes["Date"]}");
            //System.Windows.Forms.MessageBox.Show($"Efectivo column index: {columnIndexes["Efectivo"]}");
            //System.Windows.Forms.MessageBox.Show($"Reserva column index: {columnIndexes["Reserva"]}");
            //System.Windows.Forms.MessageBox.Show($"Row found for date {Mediator.escalaDay}: {rowm}");


            return columnIndexes;
        }




        private int GetColumnIndexFromLetter(string columnLetter)
        {
            int columnNumber = 0;
            for (int i = 0; i < columnLetter.Length; i++)
            {
                columnNumber *= 26;
                columnNumber += (columnLetter[i] - 'A' + 1);
            }
            return columnNumber;
        }

        // -----------------------------



        // CHECK THE ROWS (UPDATED)
        public void checkRows(string filePathSelected)
        {
            outputText = "";
            Microsoft.Office.Interop.Excel.Application excelApp = null;

            Workbook wb = null;
            Worksheet ws = null;

            try
            {
                excelApp = new Microsoft.Office.Interop.Excel.Application();
                wb = excelApp.Workbooks.Open(filePathSelected, false, true);
                //excelApp.Visible = true;  // This line makes Excel visible
                ws = (Worksheet)wb.Worksheets[1];

                Dictionary<string, int> columnIndexes = FindColumnIndexes(ws, 1, 20);

                Debug.WriteLine("Column Indexes:");
                foreach (var kvp in columnIndexes)
                {
                    Debug.WriteLine($"{kvp.Key}: {kvp.Value}");
                }

                if (!columnIndexes.ContainsKey("Efectivo") || !columnIndexes.ContainsKey("Reserva"))
                {
                    throw new Exception("Required columns not found in the Excel file.");
                }

                // Define a range similar to the original script
                Range searchedRange = ws.get_Range("A1", "K" + ws.UsedRange.Rows.Count);
                Debug.WriteLine($"Used rows count: {ws.UsedRange.Rows.Count} and for columns: {ws.UsedRange.Columns.Count}");


                Debug.WriteLine($"Searching for date: {Mediator.escalaDay}");

                // Use Range.Find() method similar to the old script
                Range currentFind = searchedRange.Find(
                    What: Mediator.escalaDay,
                    LookIn: XlFindLookIn.xlFormulas,
                    LookAt: XlLookAt.xlPart,
                    SearchOrder: XlSearchOrder.xlByRows,
                    SearchDirection: XlSearchDirection.xlNext,
                    MatchCase: false);


                if (currentFind == null)
                {
                    currentFind = searchedRange.Find(
                    What: Mediator.escalaDay,
                    LookIn: XlFindLookIn.xlValues,
                    LookAt: XlLookAt.xlPart,
                    SearchOrder: XlSearchOrder.xlByRows,
                    SearchDirection: XlSearchDirection.xlNext,
                    MatchCase: false);
                }
                if (currentFind == null)
                {
                    currentFind = searchedRange.Find(
                    What: Mediator.escalaDay,
                    LookIn: XlFindLookIn.xlFormulas,
                    LookAt: XlLookAt.xlWhole,
                    SearchOrder: XlSearchOrder.xlByRows,
                    SearchDirection: XlSearchDirection.xlNext,
                    MatchCase: false);
                }
                if (currentFind == null)
                {
                    currentFind = searchedRange.Find(
                    What: Mediator.escalaDay,
                    LookIn: XlFindLookIn.xlValues,
                    LookAt: XlLookAt.xlWhole,
                    SearchOrder: XlSearchOrder.xlByRows,
                    SearchDirection: XlSearchDirection.xlNext,
                    MatchCase: false);
                }


                Debug.WriteLine($"Date: {dateOut} and Efectivo: {efectivoOut}");
                Debug.WriteLine($"escalaDay: {Mediator.escalaDay}");
                Debug.WriteLine($"diaDeEscala: {Mediator.diaDeEscala}");

                if (currentFind != null)
                {
                    int rowm = currentFind.Row;
                    int colmn = currentFind.Column;
                    Debug.WriteLine($"Date found at row {rowm}, column {colmn}");

                    // Use the found column as the date column
                    Range dateCell = (Range)ws.Cells[rowm, colmn];

                    // This will make sure the "Efectivo" column is the one before "Efectivo" Column if the current one is empty
                    int efectivoColumn = columnIndexes["Efectivo"];
                    Range efectivoCell = (Range)ws.Cells[rowm, efectivoColumn];
                    if (string.IsNullOrWhiteSpace(Convert.ToString(efectivoCell.Value)))
                    {
                        efectivoColumn--;
                        efectivoCell = (Range)ws.Cells[rowm, efectivoColumn];
                        Debug.WriteLine($"Efectivo column adjusted to {efectivoColumn}");
                    }

                    Range reservaCell = (Range)ws.Cells[rowm, columnIndexes["Reserva"]];

                    // Assuming state cells are next to "Reserva"
                    int stateColumn = columnIndexes["Reserva"] - 1;
                    Range stateCell1 = (Range)ws.Cells[rowm, stateColumn];
                    Range stateCell2 = (Range)ws.Cells[rowm + 1, stateColumn];
                    Range stateCell3 = (Range)ws.Cells[rowm + 2, stateColumn];

                    int smartAdaptIncrementer = Convert.ToString(stateCell3.Value) == "ADPT" ? 2 : 1;
                    Range adaptCell = (Range)ws.Cells[rowm + smartAdaptIncrementer, columnIndexes["Efectivo"]];

                    // Process the data
                    dateOut = Convert.ToString(dateCell.Value);
                    efectivoOut = Convert.ToString(efectivoCell.Value);
                    adaptOut = Convert.ToString(adaptCell.Value);
                    state1Out = Convert.ToString(stateCell1.Value) ?? "";
                    state2Out = Convert.ToString(stateCell2.Value) ?? "";
                    state3Out = Convert.ToString(stateCell3.Value) ?? "";
                    reservaOut = Convert.ToString(reservaCell.Value);

                    // Debug: Print extracted values      
                    Debug.WriteLine($"Date: {dateOut} and Efectivo: {efectivoOut}");
                    Debug.WriteLine($"Adapt: {adaptOut}");
                    Debug.WriteLine($"State1: {state1Out}");
                    Debug.WriteLine($"State2: {state2Out}");
                    Debug.WriteLine($"State3: {state3Out}");
                    Debug.WriteLine($"Reserva: {reservaOut}");
                    Debug.WriteLine($"escalaDay: {Mediator.escalaDay}");
                    Debug.WriteLine($"diaDeEscala: {Mediator.diaDeEscala}");

                    // Apply name formatting
                    namesFormater(efectivoOut);
                    efectivoOut = outputText;

                    namesFormater(adaptOut);
                    adaptOut = outputText;

                    namesFormater(reservaOut);
                    reservaOut = outputText;

                    escalaPreviewFormater();
                    outputFullText = outputFullText.Replace("\n", "\r\n");
                    Mediator.instTxtBox_Equal_To(outputFullText);

                    Debug.WriteLine("Data processing completed successfully.");
                }
                else
                {
                    //System.Windows.Forms.MessageBox.Show($"Date {Mediator.escalaDay} not found in the Excel file.");
                    escalaPreviewText += $"\r\nA escala de {selectedEscala} não tem registos para o dia {Mediator.escalaDay}.\r\n\r\n";
                    Mediator.instTxtBox_Equal_To(escalaPreviewText);
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show($"An error occurred: {ex.Message}");
                System.Windows.Forms.MessageBox.Show($"Stack Trace: {ex.StackTrace}");
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Clean up — cada passo protegido individualmente para que uma falha COM
                // num deles não impeça os seguintes (senão ficava um EXCEL.EXE órfão).
                if (ws != null)
                {
                    try { Marshal.ReleaseComObject(ws); } catch { }
                }
                if (wb != null)
                {
                    try { wb.Close(false); } catch { }
                    try { Marshal.ReleaseComObject(wb); } catch { }
                }
                if (excelApp != null)
                {
                    try { excelApp.Quit(); } catch { }
                    try { Marshal.ReleaseComObject(excelApp); } catch { }
                }
                GC.Collect();
                GC.WaitForPendingFinalizers();
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

                    textToParse = parseUniversalNamesFixer(textToParse);


                    // Parser de texto com enters // Separa alinhas por valores / variaveis diferentes
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





        // UNIVERSAL NAME'S FIXER FOR OS WORD
        private string parseUniversalNamesFixer(string nameToFix)
        {

            // Corrector de tipos de enters
            while (nameToFix.Contains("\r\n") || nameToFix.Contains("\r"))
            {
                nameToFix = nameToFix.Replace("\r\n", "\n");
                nameToFix = nameToFix.Replace("\r", "\n");
            }


            // Corrector de enters a mais
            while (nameToFix.Contains("\n\n"))
            {
                nameToFix = nameToFix.Replace("\n\n", "\n");
            }

            // Corrigindo espaços em branco em torno de "/"   // Exemplos como: "2SAR/ SAS /138863-A"
            nameToFix = Regex.Replace(nameToFix, @"(?<=/)\s+|\s+(?=/)", "");

            // Removendo espaços antes e depois do texto
            nameToFix = nameToFix.Trim();

            // Removendo espaços imediatamente antes e depois de "-"
            nameToFix = Regex.Replace(nameToFix, @"\s+-\s+", "-");

            return nameToFix;
        }





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
                    LinqList.ListaManagerEscalados.escaladosList.Add(new LinqList.Pessoa { DataNomeado = dateOut, EscalaNomeado = selectedEscala, EstadoNomeado = "ADPT", NomeNomeado = EfectivoOutPTPDArray[1] });
                }
                else  // SE NÃO ESTIVER NA MESMA CELULA
                {
                    contextEfectivo = $"{selectedEscala} Efectivo:\r\n{efectivoOut}\r\n";
                    contextAdapt = $"O seguinte militar está em Adaptação:\r\n{adaptOut}\r\n";

                    LinqList.ListaManagerEscalados.escaladosList.Add(new LinqList.Pessoa { DataNomeado = dateOut, EscalaNomeado = selectedEscala, EstadoNomeado = "Efetivo", NomeNomeado = efectivoOut });
                    LinqList.ListaManagerEscalados.escaladosList.Add(new LinqList.Pessoa { DataNomeado = dateOut, EscalaNomeado = selectedEscala, EstadoNomeado = "ADPT", NomeNomeado = adaptOut });
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
                        LinqList.ListaManagerEscalados.escaladosList.Add(new LinqList.Pessoa { DataNomeado = dateOut, EscalaNomeado = selectedEscala, EstadoNomeado = "ADPT", NomeNomeado = adaptOut });
                    }
                    else // SE FOR POR DESTROCA
                    {
                        contextPTPD = $"POR DESTROCA o:\r\n{EfectivoOutPTPDArray[1]}\r\n";
                        contextAdapt = $"O seguinte militar está em Adaptação:\r\n{adaptOut}\r\n";

                        LinqList.ListaManagerEscalados.escaladosList.Add(new LinqList.Pessoa { DataNomeado = dateOut, EscalaNomeado = selectedEscala, EstadoNomeado = "PD", NomeNomeado = EfectivoOutPTPDArray[1] });
                        LinqList.ListaManagerEscalados.escaladosList.Add(new LinqList.Pessoa { DataNomeado = dateOut, EscalaNomeado = selectedEscala, EstadoNomeado = "ADPT", NomeNomeado = adaptOut });
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
