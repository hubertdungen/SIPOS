using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Word;
using Range = Microsoft.Office.Interop.Excel.Range;
using System.Collections.Generic;
using System;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.IO;
using System.Text.RegularExpressions;
using System.Reflection.Metadata.Ecma335;
using static System.Net.Mime.MediaTypeNames;
using LinqList;
using static System.Net.WebRequestMethods;
using File = System.IO.File;
using System.Diagnostics;
using Image = System.Drawing.Image;
using Point = System.Drawing.Point;
using System.Runtime.InteropServices;
using System.Globalization;
using System.Drawing.Text;
using Font = System.Drawing.Font;
using Application = System.Windows.Forms.Application;

namespace Excel_Reader
{





    public partial class frm_OS_system : Form
    {





        // --------------------- //
        // ----- FONT INIT ----- //
        // --------------------- //

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont,
            IntPtr pdv, [System.Runtime.InteropServices.In] ref uint pcFonts);

        private PrivateFontCollection fonts = new PrivateFontCollection();

        Font myFont;






        ////////////////////////////////////////////////////// --------------------- //
        ////////////////////////////////////////////////////// --- INITIALIZERS ---- //
        ////////////////////////////////////////////////////// --------------------- //


        // VARS -------
        // ------------

        // Details VARS
        private string version = "v A-0.8.3";

        // General VARS
        private string filePath = "";
        private string selectedFolder = "";
        private string joinedOutput = "";
        private string outputText = "";
        private string outputFullText = "";
        private string selectedEscala = "";
        private string escalaPreviewText = "";
        private bool isPathSaved = true;
        private bool nonePathMissing = true;
        private bool nonePathError = true;
        public static string osNumber = "";

        // Memory VARS
        string fPathODU = "";
        string fPathCCS = "";
        string fPathSD = "";
        string fPathPD = "";
        string fPathFunerais = "";
        bool debugMode = false;
        bool fileMemoryDidntExist = false;
        static int winMode = 0; // 0 = No Windows / 1 = Low Windows / 2 = All Windows
        int backgroundMode = 0; // 0 = No Backgrounds / 1 = Bkg Light / 2 = Bkg Dark

        // Calendar
        public static DateTime diaDeEscala = DateTime.Today;
        public static DateTime osDay;
        bool isItSabado = DateTime.Today.DayOfWeek == DayOfWeek.Saturday;
        bool isItQuarta = DateTime.Today.DayOfWeek == DayOfWeek.Wednesday;
        public static string escalaDay = "";


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
        // -----------------------------
        // --------------------------------------------------------------------------


        ////////////////////////////////////////////////////// FORMS 
        // -------
        public frm_OS_system()
        {
            InitializeComponent();

            // FONT INIT

            byte[] fontData = Properties.Resources.Agency_FB_BC;
            IntPtr fontPtr = System.Runtime.InteropServices.Marshal.AllocCoTaskMem(fontData.Length);
            System.Runtime.InteropServices.Marshal.Copy(fontData, 0, fontPtr, fontData.Length);
            uint dummy = 0;
            fonts.AddMemoryFont(fontPtr, Properties.Resources.Agency_FB_BC.Length);
            AddFontMemResourceEx(fontPtr, (uint)Properties.Resources.Agency_FB_BC.Length, IntPtr.Zero, ref dummy);
            System.Runtime.InteropServices.Marshal.FreeCoTaskMem(fontPtr);

            myFont = new Font(fonts.Families[0], 16.0F);

            lbl_DesignioSIPOS.Font = new Font("Agency FB", 10.8f, FontStyle.Italic | FontStyle.Bold);
            lbl_NomeSoftware.Font = new Font("Agency FB", 16.2f, FontStyle.Italic | FontStyle.Bold);
            lbl_version.Font = new Font("Agency FB", 7.8f, FontStyle.Italic | FontStyle.Bold);




            // LOAD MEMORY
            loadMemory();
            check_ifDebugIsActive();

            // TXTBOXES UPDATE
            txtboxsActualizer();

            // DATE PROCESSING
            afterToday();
            //selectedDay = Convert.ToString(monthCalendar.SelectionStart);
            dateProcess();

            // START ENGINE IF POSSIBLE
            LinqList.ListaManagerEscalados.escaladosList.Clear();
            triagemEscalas();
            isPathSaved = true;
        }


        //private void Form1_Load(object sender, EventArgs e)
        //{
        //    label1.Font = new Font("Agency FB", 24, FontStyle.Bold);
        //}


        private void frm_menu_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }
        // -----------------------------
        // --------------------------------------------------------------------------



        ////////////////////////////////////////////////////// BUTTONS 
        // -------

        private void btn_Refresh_Click(object sender, EventArgs e)
        {
            prg_Bar.Value = 0;  // Progress Bar to 0
            LinqList.ListaManagerEscalados.escaladosList.Clear();           //Limpa a Lista
            escalaPreviewText = "";                                         //Limpa o texto preview
            prgBarAddInc(0);

            //selectedDay = Convert.ToString(monthCalendar.SelectionStart);   //Converte o input data para string
            prgBarAddInc(0);
            dateProcess();

            // if date is not friday it will run triagemEscalas() normaly
            // if date is friday it will run triagemEscalas() for saturday, sunday and next monday
            // which means, it will run triagemEscalas() 3 times and register 3 different outputs in the same list
            //if (isItSabado == false)
            //{
            //    triagemEscalas();
            //}
            //else
            //{
            //    triagemEscalas();
            //    diaDeEscala = diaDeEscala.AddDays(1);
            //    dateProcess();
            //    triagemEscalas();
            //    diaDeEscala = diaDeEscala.AddDays(1);
            //    dateProcess();
            //    triagemEscalas();
            //    diaDeEscala = diaDeEscala.AddDays(-2);
            //    dateProcess();
            //}
            triagemEscalas();
        }
        private void btn_searchFileFMemory_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("explorer.exe", Directory.GetCurrentDirectory());
            }
            catch
            {
                MessageBox.Show("Não é possível abrir a pasta onde o ficheiro \\settings.txt\\ se encontra.\r\nÉ possível que a pasta esteja protegida ou que o programa tenha guardado mal o ficheiro.", "ERRO AO ABRIR PASTA!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            //string selectedFolder = "";
            //openFolder(selectedFolder);
            //txtBox_FMemory.Text = filePath;
        }



        // BUTTONS: SAVE FILES
        private void btn_SaveFileLocPD_Click(object sender, EventArgs e)
        {
            prg_SaveButton.Value = prg_SaveButton.Minimum;

            fPathODU = txtbox_FileDirectoryODU.Text;                      // Grava a PATH seleccionada no fPathODU
            selectedEscala = "ODU";
            chkANDsaveMemory(fPathODU); // CHECKAR TEXTO E GRAVAR 

            fPathCCS = txtbox_FileDirectoryCCS.Text;                      // Grava a PATH seleccionada no fPathCCS
            selectedEscala = "CCS";
            chkANDsaveMemory(fPathCCS); // CHECKAR TEXTO E GRAVAR 

            fPathSD = txtbox_FileDirectorySD.Text;                      // Grava a PATH seleccionada no fPathSD
            selectedEscala = "SD";
            chkANDsaveMemory(fPathSD); // CHECKAR TEXTO E GRAVAR 

            fPathPD = txtbox_FileDirectoryPD.Text;                      // Grava a PATH seleccionada no fPathPD
            selectedEscala = "PD";
            chkANDsaveMemory(fPathPD); // CHECKAR TEXTO E GRAVAR 

            fPathFunerais = txtbox_FileDirectoryFunerais.Text;                      // Grava a PATH seleccionada no fPathPD
            selectedEscala = "Funerais";
            chkANDsaveMemory(fPathFunerais); // CHECKAR TEXTO E GRAVAR 

            selectedEscala = ""; // Clear VAR

            if (isPathSaved == true)
            {
                isPathSaved = true;  // Regista que ficou guardado

                if (nonePathError == true)
                {
                    MessageBox.Show("Preferências e directórios dos ficheiros gravados com sucesso!", "GRAVADO COM SUCESSO!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    prg_SaveButton.Value = prg_SaveButton.Maximum;
                }
                else
                {
                    MessageBox.Show("Preferências e directórios dos ficheiros gravados embora hajam linhas em falta ou com erros!", "GRAVADO COM FALHAS!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else
            {
                prg_SaveButton.Value = prg_SaveButton.Minimum;
            }

        }


        // BUTTONS: EXPORT
        private void btn_ExportWord_Click(object sender, EventArgs e)
        {
            try
            {
                if (osDay.DayOfWeek == DayOfWeek.Tuesday)  // CASO A O.S. seja de TERÇA, ou seja, ESCALA DE SERVIÇO seja QUARTA-FEIRA
                { 
                    Word_Processor.CreateWordDocument(txtbox_FileDirectory_ModelQuarta.Text, txtbox_FolderDirectory_OSWord.Text + @"\" + txtBox_ExportDocName.Text + ".doc"); 
                }
                else                             // CASO A O.S. seja noutros dias de semana
                {
                    Word_Processor.CreateWordDocument(txtbox_FileDirectory_ModelSemana.Text, txtbox_FolderDirectory_OSWord.Text + @"\" + txtBox_ExportDocName.Text + ".doc");
                }


            }
            catch
            {
                MessageBox.Show("Verifique se tem os modelos de Word completos (sem faltar nenhuma variável), se colocou correctamente os caminhos de cada modelo no campo: \"propriedades\", se colocou o caminho de exportação e por fim se gravou.", "ERRO AO EXPORTAR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
            //Word_Processor.CreateWordDocument("", "");
        }
        private void btn_SaveFolder_OSWord_Click(object sender, EventArgs e)
        {
            openFolder();
            txtbox_FolderDirectory_OSWord.Text = selectedFolder;
        }


        // BUTTONS: OPEN FILE DIALOGS
        private void btn_searchFileODU_Click(object sender, EventArgs e)
        {
            openFile();
            txtbox_FileDirectoryODU.Text = filePath;
        }
        private void btn_searchFileCCS_Click(object sender, EventArgs e)
        {
            openFile();
            txtbox_FileDirectoryCCS.Text = filePath;
        }
        private void btn_searchFilePD_Click(object sender, EventArgs e)
        {
            openFile();
            txtbox_FileDirectoryPD.Text = filePath;
        }
        private void btn_searchFileSD_Click(object sender, EventArgs e)
        {
            openFile();
            txtbox_FileDirectorySD.Text = filePath;
        }
        private void btn_searchFileFunerais_Click(object sender, EventArgs e)
        {
            openFile();
            txtbox_FileDirectoryFunerais.Text = filePath;
        }
        private void btn_searchFile_ModelSemana_Click(object sender, EventArgs e)
        {
            openFile();
            txtbox_FileDirectory_ModelSemana.Text = filePath;
        }
        private void btn_searchFile_FDSemana_Click(object sender, EventArgs e)
        {
            openFile();
            txtbox_FileDirectory_ModelSemana.Text = filePath;
        }
        private void btn_searchFile_Quarta_Click(object sender, EventArgs e)
        {
            openFile();
            txtbox_FileDirectory_ModelQuarta.Text = filePath;
        }


        // BUTTONS: DEBUGGERS
        public void btn_VarTester_Click(object sender, EventArgs e)
        {
            MessageBox.Show(filePath);
        }
        private void btn_CheckEscalaList_Click(object sender, EventArgs e)
        {

            List<Pessoa> peopleLines = LinqList.ListaManagerEscalados.escaladosList;

            string messageEscalaList = "";

            foreach (var pessoaLine in peopleLines)
            {
                messageEscalaList += $"{pessoaLine.DataNomeado} {pessoaLine.EscalaNomeado} {pessoaLine.EstadoNomeado} {pessoaLine.NomeNomeado}" + "\r\n";
            }

            //var messageEscalaList = string.Join(Environment.NewLine, LinqList.ListaManagerEscalados.escaladosList);
            MessageBox.Show("Lista de pessoal escalado na LinqList do software:\r\n" + messageEscalaList, "DEBUG: Lista de Pessoal Escalado");
        }
        private void btn_Export_testDaySelect_Var_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Convert.ToString(frm_OS_system.returnEscalaDate()));
        }
        private void btn_Export_TestListReader_Click(object sender, EventArgs e)
        {
            Word_Processor.listToVarsEscalados();
        }

        // -----------------------------
        // --------------------------------------------------------------------------
        // --------------------------------------------------------------------------






        ////////////////////////////////////////////////////// --------------------- //
        ////////////////////////////////////////////////////// -- ENGINE SEARCHER -- //
        ////////////////////////////////////////////////////// --------------------- //


        // TRIAGEM DE ESCALAS
        private void triagemEscalas()
        {


            //missingPathsChecker();
            int allWithErrors = 0;

            escalaPreviewText += "_____________________________________________\r\n" + $"A seguinte lista diz respeito aos militares nomeados para dia {escalaDay}:\r\n\r\n";

            //if (nonePathMissing == true)
            //{
            // ODU
            textBox_Output.Text = "";
            selectedEscala = "Oficial de Dia";
            pathErrorCheck(fPathODU);
            if (nonePathError == true) { checkRows(fPathODU); }
            else
            {
                nonePathError = true;
                allWithErrors++;
            }

            // CCS
            textBox_Output.Text = "";
            selectedEscala = "CCS";
            pathErrorCheck(fPathCCS);
            if (nonePathError == true) { checkRows(fPathCCS); }
            else
            {
                nonePathError = true;
                allWithErrors++;
            }

            // SD
            textBox_Output.Text = "";
            selectedEscala = "Sargento de Dia";
            pathErrorCheck(fPathSD);
            if (nonePathError == true) { checkRows(fPathSD); }
            else
            {
                nonePathError = true;
                allWithErrors++;
            }

            // PD
            textBox_Output.Text = "";
            selectedEscala = "Praça de Dia";
            pathErrorCheck(fPathPD);
            if (nonePathError == true) { checkRows(fPathPD); }
            else
            {
                nonePathError = true;
                allWithErrors++;
            }

            // FUNERAIS
            textBox_Output.Text = "";
            selectedEscala = "Honras Fúnebres";
            pathErrorCheck(fPathFunerais);
            if (nonePathError == true) { checkRows(fPathFunerais); }
            else
            {
                nonePathError = true;
                allWithErrors++;
                prg_Bar.Value = prg_Bar.Maximum;
            }

            if (allWithErrors == 5)
            {
                textBox_Output.Text = "Não existem ficheiros carregados no sistema. Ou inseriu mal os caminhos dos ficheiros excel, ou esses ficheiros já não existem no local.";
                prg_Bar.Value = 0;
            }

            if (prg_Bar.Value >= prg_Bar.Maximum) { prg_Bar.Value = prg_Bar.Maximum; } else { prg_Bar.Value = prg_Bar.Minimum; }

        }
        // -----------------------------

        // CHECK THE ROWS
        public void checkRows(string filePathSelected)
        {
            outputText = "";  // Clearing the TEXT

            prgBarAddInc(0);  // progress bar add inc
            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            Workbook wb;
            Worksheet ws;

            wb = excel.Workbooks.Open(filePathSelected, false, true);
            ws = wb.Worksheets[1];

            Range searchedRange = excel.get_Range("B15", "K52");

            // LINE FINDER
            Range currentFind = searchedRange.Find(escalaDay);

            string displayResult = "";
            string displayResultOut = "";
            List<string> ResultOutList = new List<string>();
            prgBarAddInc(0);  // progress bar add inc

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




                string textToParse = "";
                prgBarAddInc(0);  // progress bar add inc

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

                while (Marshal.ReleaseComObject(ws) != 0) ;
                while (Marshal.ReleaseComObject(wb) != 0) ;
                while (Marshal.ReleaseComObject(excel) != 0) ;

                GC.Collect();
                GC.WaitForPendingFinalizers();

                // Formatador de Texto do Bloco Individual duma Escala de Serviço


                escalaPreviewFormater();
                outputFullText.Replace("\n", "\r\n");
                textBox_Output.Text = outputFullText;
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

                displayResult = "A data que procurou: \"" + escalaDay +
                        $"\" não existe na lista de {selectedEscala}.";


                escalaPreviewText = escalaPreviewText + $"\r\nA escala de {selectedEscala} não tem registos para o dia {escalaDay}.\r\n\r\n";
                textBox_Output.Text = escalaPreviewText;
            }

            lbl_Result.Text = displayResult;  // Debug Label
            prgBarAddInc(0);  // progress bar add inc
        }
        // -----------------------------

        // CALENDAR
        // --------
        private void monthCalendar_DateChanged(object sender, DateRangeEventArgs e)
        {
            //selectedDay = Convert.ToString(monthCalendar.SelectionStart);
            //dateParse();

            //if (fPathPD != "")
            //{
            //    textBox_Output.Text = "";
            //    checkRows();
            //}
        }

        private void dateProcess()
        {
            //var dateTime = monthCalendar.SelectionStart;
            diaDeEscala = monthCalendar.SelectionStart.AddDays(1);
            //dateTime = DateTime.ParseExact(monthCalendar.SelectionStart, "yy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);
            escalaDay = diaDeEscala.ToString("dd-MM-yyyy");
            osDay = monthCalendar.SelectionStart;
            //MessageBox.Show(selectedDay);
        }
        private void afterToday()
        {
            DateTime dt = DateTime.Now;
            monthCalendar.SelectionStart = dt;
            monthCalendar.SelectionEnd = dt;

            //monthCalendar.SelectionStart = dt.AddDays(1);
            //monthCalendar.SelectionEnd = dt.AddDays(1);
        }
        private void dateParse()
        {
                escalaDay = escalaDay.Substring(0, escalaDay.IndexOf(" "));
                escalaDay = escalaDay.Replace("/", "-");
        }
        public static object weekDayParse(string weekDay)
        {
            if (winMode == 2) { MessageBox.Show("weekDay: " + weekDay); }
            
            string weekDayString = "";

            switch (weekDay){

                // PORTUGUES ABV

                case "seg":
                    weekDayString = "2.ª Feira";
                    break;

                case "ter":
                    weekDayString = "3.ª Feira";
                    break;

                case "qua":
                    weekDayString = "4.ª Feira";
                    break;

                case "qui":
                    weekDayString = "5.ª Feira";
                    break;

                case "sex":
                    weekDayString = "6.ª Feira";
                    break;

                case "sáb":
                    weekDayString = "sábado";
                    break;

                case "dom":
                    weekDayString = "domingo";
                    break;

                // PORTUGUES FULL

                case "segunda":
                    weekDayString = "2.ª Feira";
                    break;

                case "terça":
                    weekDayString = "3.ª Feira";
                    break;

                case "quarta":
                    weekDayString = "4.ª Feira";
                    break;

                case "quinta":
                    weekDayString = "5.ª Feira";
                    break;

                case "sexta":
                    weekDayString = "6.ª Feira";
                    break;

                case "sábado":
                    weekDayString = "sábado";
                    break;

                case "domingo":
                    weekDayString = "domingo";
                    break;

                // ENGLISH ABV

                case "mon":
                    weekDayString = "2.ª Feira";
                    break;

                case "tue":
                    weekDayString = "3.ª Feira";
                    break;

                case "wed":
                    weekDayString = "4.ª Feira";
                    break;

                case "thu":
                    weekDayString = "5.ª Feira";
                    break;

                case "fri":
                    weekDayString = "6.ª Feira";
                    break;

                case "sat":
                    weekDayString = "sábado";
                    break;

                case "sun":
                    weekDayString = "domingo";
                    break;


                // ENGLISH FULL

                case "monday":
                    weekDayString = "2.ª Feira";
                    break;

                case "tuesday":
                    weekDayString = "3.ª Feira";
                    break;

                case "wednesday":
                    weekDayString = "4.ª Feira";
                    break;

                case "thursday":
                    weekDayString = "5.ª Feira";
                    break;

                case "friday":
                    weekDayString = "6.ª Feira";
                    break;

                case "saturday":
                    weekDayString = "sábado";
                    break;

                case "sunday":
                    weekDayString = "domingo";
                    break;

                default:
                    weekDayString = "NÃO_DETETOU_DIAdeSEMANA";
                    break;

            }

            return weekDayString;
        }

        // -----------------------------
        // --------------------------------------------------------------------------
        // --------------------------------------------------------------------------





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

                if (winMode == 2) { MessageBox.Show(textToParse, "BEFORE PARSING PHRASE"); }

                if ((textToParse != null) && (textToParse.Length > 10))
                {
                    if (textToParse.Contains("\r\n"))
                    {
                        string[] lines = textToParse.Split("\r\n");

                        foreach (string line in lines)
                        {
                            //if (line.Length > 10)
                            //{

                            string lineFinished = line;

                            while (lineFinished.Contains("  "))
                            {
                                lineFinished = lineFinished.Replace("  ", " ");
                                
                            }
                            while (lineFinished.Contains("\r\n"))
                            {
                                string[] lines2 = lineFinished.Split("\r\n");
                            }
                            //if (lineFinished.Contains("\r\n\r\n"))
                            //{
                            //    string[] lines3 = lineFinished.Split("\r\n\r\n", "\r\n");
                            //}



                            string[] parts = lineFinished.Split(" ");
                            parts[0] = parts[0].Replace("/", "\t");
                            //string partsZero = parts[0].LastIndexOf("-") > 0 ? parts[0].Replace("-", " ", (StringComparison)parts[0].LastIndexOf("-")) : parts[0];
                            //parts[0] = parts[0].Replace("-", " ");
                            //StringComparison lastIndexOfHifen = parts[0].LastIndexOf("-");
                            parts[0] = Regex.Replace(parts[0], "-(?!.*-)", " ");
                            //parts[0] = parts[0].Replace("-", " ", (StringComparison)parts[0].LastIndexOf("-"));
                            parts[1] = parts[1].Substring(0, 1);
                            outputText += parts[0] + " – " + parts[1] + ". " + parts[2] + "\n";
                            //}
                        }
                    }
                    else if (textToParse.Contains("\n"))
                    {
                        string[] lines = textToParse.Split("\n");

                        foreach (string line in lines)
                        {
                            //if (line.Length > 10)
                            //{

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
                            //string partsZero = parts[0].LastIndexOf("-") > 0 ? parts[0].Replace("-", " ", (StringComparison)parts[0].LastIndexOf("-")) : parts[0];
                            //parts[0] = parts[0].Replace("-", " ");
                            //StringComparison lastIndexOfHifen = parts[0].LastIndexOf("-");
                            parts[0] = Regex.Replace(parts[0], "-(?!.*-)", " ");
                            //parts[0] = parts[0].Replace("-", " ", (StringComparison)parts[0].LastIndexOf("-"));
                            parts[1] = parts[1].Substring(0, 1);
                            outputText += parts[0] + " – " + parts[1] + ". " + parts[2] + Environment.NewLine; // (char)13 works for Word only
                            //}
                        }
                    }

                    else if (textToParse.Contains((char)10))
                    {
                        string[] lines = textToParse.Split((char)10);

                        foreach (string line in lines)
                        {
                            //if (line.Length > 10)
                            //{

                            string lineFinished = line;

                            while (lineFinished.Contains("  "))
                            {
                                lineFinished = lineFinished.Replace("  ", " ");

                            }
                            if (lineFinished.Contains((char)10))
                            {
                                string[] lines2 = lineFinished.Split((char)10);
                            }



                            string[] parts = lineFinished.Split(" ");
                            parts[0] = parts[0].Replace("/", "\t");
                            //string partsZero = parts[0].LastIndexOf("-") > 0 ? parts[0].Replace("-", " ", (StringComparison)parts[0].LastIndexOf("-")) : parts[0];
                            //parts[0] = parts[0].Replace("-", " ");
                            //StringComparison lastIndexOfHifen = parts[0].LastIndexOf("-");
                            parts[0] = Regex.Replace(parts[0], "-(?!.*-)", " ");
                            //parts[0] = parts[0].Replace("-", " ", (StringComparison)parts[0].LastIndexOf("-"));
                            parts[1] = parts[1].Substring(0, 1);
                            outputText += parts[0] + " – " + parts[1] + ". " + parts[2] + "\n";
                            //}
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
                        //parts[0] = parts[0].Replace("-", " ");
                        parts[0] = Regex.Replace(parts[0], "-(?!.*-)", " ");
                        //parts[0] = parts[0].Replace("-", " ", (StringComparison)parts[0].LastIndexOf("-"));
                        //parts[0] = parts[0].Replace(/-(?!.*-)/, " ");
                        //.Replace(parts[0], "(-)(?!.*-)", " ");
                        //parts[0] = Regex.Replace(parts[0], "-", " ", RegexOptions.RightToLeft);
                        parts[1] = parts[1].Substring(0, 1);
                        outputText += parts[0] + " – " + parts[1] + ". " + parts[2]; // + "\r\n";
                    }
                }
                else
                {
                    outputText = "";
                }

                if (winMode == 2) { MessageBox.Show(outputText, "AFTER PARSING PHRASE"); }

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
            dateOut = escalaDay;


            efectivoPorPTPDsplitter(); // SEPARADOR DE LINHAS

            //MessageBox.Show(EfectivoOutPTPDArray[0] + " e " + EfectivoOutPTPDArray[1]);

            // CASO HAJA POR TROCA OU POR DESTROCA
            if ((state1Out.Contains("PT") || state1Out.Contains("PD") || state2Out.Contains("PT") || state2Out.Contains("PD")) && ((!state3Out.Contains("ADPT")) && (!state2Out.Contains("ADPT"))))
            {
                if (efetivoTemPTPDporLinha == true)
                {
                    contextEfectivo = $"{selectedEscala} Efetivo:\r\n{EfectivoOutPTPDArray[0]}\r\n";
                    //Excel_Reader.Escalados.escaladosList.Add(new Pessoa { DataNomeado = dateOut, NomeNomeado = EfectivoOutPTPDArray[0] });
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
                        contextAdapt = $"O seguinte militar está em ADPT:\r\n{adaptOut}\r\n";

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
            if (winMode == 1) { MessageBox.Show(escalaPreviewText, "Pessoal escalado"); }
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
                        if (winMode == 2) { MessageBox.Show(line, "RESULTADO DO TEXTO SEPARADO"); }
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






        ////////////////////////////////////////////////////// --------------------- // 
        ////////////////////////////////////////////////////// - SAVE/LOAD MEMORY -- //
        ////////////////////////////////////////////////////// --------------------- //

        // SAVER
        private void saveMemory()
        {
            string fMemoryPath = Directory.GetCurrentDirectory() + "\\settings.txt";

            try
            {
                TextWriter tw = new StreamWriter(fMemoryPath);

                // Debug MsgBox EachFile
                if (winMode == 2) { MessageBox.Show("fPathPD: " + fPathPD + "\nfMemoryPath: " + fMemoryPath + "\nfilePathSelected" + filePath); }

                // write lines of text to the file
                tw.WriteLine(version);
                tw.WriteLine(fPathODU);
                tw.WriteLine(fPathCCS);
                tw.WriteLine(fPathSD);
                tw.WriteLine(fPathPD);
                tw.WriteLine(fPathFunerais);
                tw.WriteLine(debugMode);
                tw.WriteLine(winMode);
                tw.WriteLine(txtbox_FileDirectory_ModelSemana.Text);
                tw.WriteLine(txtbox_FolderDirectory_OSWord.Text);
                tw.WriteLine(txtbox_FileDirectory_ModelQuarta.Text);


                // close the stream     
                tw.Close();
                //txtBox_FMemory.Text = fMemoryPath;
                //txtbox_FileDirectoryPD.Text = fPathPD;  
                if (winMode == 2) { MessageBox.Show($"Directorio de {selectedEscala} gravado com sucesso!", "GRAVADO!", MessageBoxButtons.OK, MessageBoxIcon.Information); }

                isPathSaved = true;
            }
            catch
            {
                MessageBox.Show($"O programa tentou aceder ao ficheiro de memória e gravar as preferências mas sem sucesso, o que resulta nesta instância não poder regista-las.\r\n\r\nO software está a ter problemas em registar informação no ficheiro de memória \"settings.txt\" colocado em: \r\n{fMemoryPath}\r\n" + "\r\nPor favor verifique se alguma aplicação está a utilizar o ficheiro ou se a pasta está inacessível. Caso não esteja a ser utilizado por nenhum software, tente o seguinte: \r\n-> Feche o programa\r\n-> Elimine o ficheiro\r\n-> Volte a entrar para o programa criar um novo ficheiro \"settings.txt\"", "ERRO AO GRAVAR PREFERÊNCIAS!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                isPathSaved = false;
            }
            if (fileMemoryDidntExist == false) { prg_SaveButton_AddInc(0); } else { prg_SaveButton.Value = 0; }
        }

        // CHECKING ERRORS AND SAVING PATHS
        private void chkANDsaveMemory(string txtBoxSelected)
        {
            if (txtBoxSelected.EndsWith(".xls"))
            {
                filePath = txtBoxSelected;
                prg_SaveButton_AddInc(0); //add inc saving progress bar
                saveMemory();
            }
            else if ((txtBoxSelected == "") || (txtBoxSelected == null))
            {
                MessageBox.Show($"Não seleccionou um caminho para ficheiro Excel da escala {selectedEscala}!\nPor favor seleccione um ficheiro \".XLS\".", $"CAMINHO NÃO ESPECIFICADO!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtBoxSelected = "";
                nonePathError = false;
                txtboxsActualizer();

            }
            else if (!Regex.IsMatch(txtBoxSelected, @"(.)[A-Za-z0-9]{3}$"))
            {
                MessageBox.Show($"Caminho seleccionado:\r\n{txtBoxSelected}\r\n" + $"O caminho seleccionado da escala {selectedEscala} leva o programa a uma pasta e não a um ficheiro Excel!\nPor favor seleccione um ficheiro \".XLS\".", $"PASTA SELECCIONADA!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtBoxSelected = "";
                nonePathError = false;
                txtboxsActualizer();
            }
            else
            {
                MessageBox.Show($"Caminho seleccionado:\r\n{txtBoxSelected}\r\n" + $"O ficheiro que seleccionou para a escala {selectedEscala} não é um ficheiro de Excel compatível!\nPor favor seleccione um ficheiro \".XLS\".", $"FICHEIRO INCOMPATÍVEL!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtBoxSelected = "";
                nonePathError = false;
                txtboxsActualizer();
            }

        }

        // CHECKING ERRORS AND SAVING MEMORY PATHS
        private void chkANDsaveMemoryFilePath(string fMemoryPath)
        {
            if (Regex.IsMatch(fMemoryPath, @"(.)[A-Za-z0-9]{3}$") && File.Exists(fMemoryPath))
            {
                saveMemory();
            }
            else if ((fMemoryPath == "") || (fMemoryPath == null))
            {
                MessageBox.Show($"Não seleccionou um caminho para ficheiro Excel da escala {selectedEscala}!\nPor favor seleccione um ficheiro \".XLS\".", $"CAMINHO NÃO ESPECIFICADO!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                nonePathError = false;
                txtboxsActualizer();

            }
            else if (!Regex.IsMatch(fMemoryPath, @"(.)[A-Za-z0-9]{3}$"))
            {
                MessageBox.Show($"Caminho seleccionado:\r\n{fMemoryPath}\r\n" + $"O caminho seleccionado da escala {selectedEscala} leva o programa a uma pasta e não a um ficheiro Excel!\nPor favor seleccione um ficheiro \".XLS\".", $"PASTA SELECCIONADA!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                nonePathError = false;
                txtboxsActualizer();
            }
            else
            {
                MessageBox.Show($"Caminho seleccionado:\r\n{fMemoryPath}\r\n" + $"O ficheiro que seleccionou para a escala {selectedEscala} não é um ficheiro de Excel compatível!\nPor favor seleccione um ficheiro \".XLS\".", $"FICHEIRO INCOMPATÍVEL!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                nonePathError = false;
                txtboxsActualizer();
            }
        }

        // LOADER
        private void loadMemory()
        {

            //MessageBox.Show(fMemoryPath);
            string fMemoryPath = Directory.GetCurrentDirectory() + "\\settings.txt";

            if (!File.Exists(fMemoryPath))  // PROCURA SE HÁ MEMORIA GRAVADA
            {
                try
                {
                    using (var myFile = File.Create(fMemoryPath)) { };
                    saveMemory();
                    MessageBox.Show($"O ficheiro de memória das preferências não existia. Como tal o programa criou um novo ficheiro em {fMemoryPath}.\r\n\r\nPara o programa funcionar terá de ir a propriedades e carregar os ficheiros Excel das escalas de serviço e de seguida gravar.", "FICHEIRO DE MEMÓRIA INEXISTENTE!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch
                {
                    MessageBox.Show($"O ficheiro de memória não existia, portanto o programa tentou cria-lo mas sem sucesso, o que resulta nesta instância não poder nem ler nem registar as preferências guardadas.\r\n\r\nO software está a ter problemas em registar ao ficheiro de memória \"settings.txt\" que se iria tentar colocar em: \r\n{fMemoryPath}\r\n" + "\r\nPor favor verifique se alguma aplicação está a utilizar a pasta ou se a pasta está inacessível.", "ERRO AO CRIAR FICHEIRO DE MEMÓRIA!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                fileMemoryDidntExist = true;
            }
            else                                           // HAVENDO FICHEIRO DE MEMÓRIA - LÊ A MEMORIA
            {
                // create reader & open file
                TextReader tr = new StreamReader(fMemoryPath);
                string checkVersion = tr.ReadLine();
                tr.Close();

                if (version == checkVersion) // validar ficheiro  - LÊ A MEMORIA
                {
                    tr = new StreamReader(fMemoryPath);

                    // read lines of text
                    checkVersion = tr.ReadLine();
                    fPathODU = tr.ReadLine();
                    fPathCCS = tr.ReadLine();
                    fPathSD = tr.ReadLine();
                    fPathPD = tr.ReadLine();
                    fPathFunerais = tr.ReadLine();
                    debugMode = Convert.ToBoolean(tr.ReadLine());
                    winMode = Convert.ToInt32(tr.ReadLine());
                    txtbox_FileDirectory_ModelSemana.Text = tr.ReadLine();
                    txtbox_FolderDirectory_OSWord.Text = tr.ReadLine();
                    txtbox_FileDirectory_ModelQuarta.Text = tr.ReadLine();

                    // close the stream
                    tr.Close();
                    fileMemoryDidntExist = false;
                }
                else    // CASO NÃO SEJA MESMA VERSÃO // Recria um ficheiro para evitar erros
                {
                    try
                    {
                        saveMemory();
                        MessageBox.Show($"A versão do ficheiro de memória ({checkVersion}) não era a mesma deste software: {version}.\r\nPor questões de segurança o software limpa o ficheiro de preferências sempre que há uma grande actualização.\r\n\r\nTerá de ir às propriedades carregar os ficheiros Excel e as suas preferências.", "ACTUALIZAÇÃO!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch // Caso não consiga aceder ou gravar sobre o ficheiro, dá uma mensagem de alerta.
                    {
                        MessageBox.Show($"O ficheiro de memória não pertencia à versão deste software, portanto o programa tentou actualizar o ficheiro, mas sem sucesso, o que resulta nesta instância em não ler as preferências guardadas.\r\n\r\nO software está a ter problemas em aceder ao ficheiro de memória \"settings.txt\" que se encontra localizado em: \r\n{fMemoryPath}\r\n" + "\r\nPor favor verifique se alguma aplicação está a utilizar o ficheiro. Caso não esteja a ser utilizado por nenhum software, tente o seguinte: \r\n-> Feche o programa\r\n-> Elimine o ficheiro\r\n-> Volte a entrar para o programa criar um novo ficheiro \"settings.txt\"", "ERRO AO LER E ACTUALIZAR A MEMÓRIA!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    fileMemoryDidntExist = true;
                }
            }
            //missingPathsChecker();
            txtboxsActualizer();
        }


        // OPEN DIALOGS
        // --------

        // FILE OPENER
        private void openFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                filePath = openFileDialog.FileName;
                if (winMode == 2) { MessageBox.Show("File directory: " + filePath); }
            }
        }

        // FOLDER OPENER
        private void openFolder()
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {

                System.Windows.Forms.DialogResult result = dialog.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    selectedFolder = dialog.SelectedPath;
                }
            }
        }


        // ERROR CHECKING
        // --------

        // MISSING PATH CHECKER
        private void missingPathsChecker()
        {
            if (fPathODU == "" || fPathODU == null)
            {
                MessageBox.Show("Não tem um caminho especificado para o ficheiro da Escala de \"Oficial de Dia\"" + "\r\n Ao clicar \"OK\" concorda em que o programa insira os dados sem a Escala de ODU.", "ALERTA!", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
                nonePathMissing = false;
            }

            if (fPathCCS == "" || fPathCCS == null)
            {
                MessageBox.Show("Não tem um caminho especificado para o ficheiro da Escala de \"Centro Coordenador de Segurança e Defesa\"" + "\r\n Ao clicar \"OK\" concorda em que o programa insira os dados sem a Escala de CCS.", "ALERTA!", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
                nonePathMissing = false;
            }

            if (fPathSD == "" || fPathSD == null)
            {
                MessageBox.Show("Não tem um caminho especificado para o ficheiro da Escala de \"Sargento de Dia\"" + "\r\n Ao clicar \"OK\" concorda em que o programa insira os dados sem a Escala de SD.", "ALERTA!", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
                nonePathMissing = false;
            }

            if (fPathPD == "" || fPathPD == null)
            {
                MessageBox.Show("Não tem um caminho especificado para o ficheiro da Escala de \"Praça de Dia\"" + "\r\n Ao clicar \"OK\" concorda em que o programa insira os dados sem a Escala de PD.", "ALERTA!", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
                nonePathMissing = false;
            }

            if (fPathFunerais == "" || fPathFunerais == null)
            {
                MessageBox.Show("Não tem um caminho especificado para o ficheiro da Escala de \"Funerais\"" + "\r\n Ao clicar \"OK\" concorda em que o programa insira os dados sem a Escala de Funerais.", "ALERTA!", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
                nonePathMissing = false;
            }


        }

        // PATH ERRORS CHECKING
        private void pathErrorCheck(string filePath)
        {
            if (fileMemoryDidntExist == false) // CASO existia ficheiro memória presente avisa  // Caso contrário evita mensagens desnecessárias
            {
                if (!File.Exists(filePath))
                {
                    MessageBox.Show($"Caminho seleccionado:\r\n{filePath}\r\n" + $"O ficheiro que seleccionou para a escala {selectedEscala} não existe no sistema!\nPor favor seleccione um ficheiro \".XLS\".", "FICHEIRO NÃO EXISTENTE!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    nonePathError = false;
                    txtboxsActualizer();
                }
                else if ((filePath == "") || (filePath == null))
                {
                    MessageBox.Show($"Não seleccionou um caminho para ficheiro Excel da escala {selectedEscala}!\nPor favor seleccione um ficheiro \".XLS\".", $"CAMINHO NÃO ESPECIFICADO!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    nonePathError = false;
                    txtboxsActualizer();
                }
                else if (!Regex.IsMatch(filePath, @"(.)[A-Za-z0-9]{3}$"))
                {
                    MessageBox.Show($"Caminho seleccionado:\r\n{filePath}\r\n" + $"O caminho seleccionado da escala {selectedEscala} leva o programa a uma pasta e não a um ficheiro Excel!\nPor favor seleccione um ficheiro \".XLS\".", $"PASTA SELECCIONADA!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    nonePathError = false;
                    txtboxsActualizer();
                }
                else
                {
                    return;
                }
            } else
            {
                nonePathError = false;
                txtboxsActualizer();
            }
        }

        // -----------------------------
        // --------------------------------------------------------------------------
        // --------------------------------------------------------------------------






        ////////////////////////////////////////////////////// --------------------- // 
        ////////////////////////////////////////////////////// ----- UI CONTROL ---- //
        ////////////////////////////////////////////////////// --------------------- //

        // PROGRESS BAR 
        private void prgBarAddInc(int addMore)
        {
            prg_Bar.Value = prg_Bar.Value + 1 + addMore;

            if (prg_Bar.Value >= prg_Bar.Maximum) {
                prg_Bar.Value = prg_Bar.Maximum;
            }
        }
        private void prg_SaveButton_AddInc(int addMore)
        {
            prg_SaveButton.Value = prg_SaveButton.Value + 1 + addMore;

            if (prg_SaveButton.Value >= prg_SaveButton.Maximum)
            {
                prg_SaveButton.Value = prg_SaveButton.Maximum;
            }
        }
        private void makeUiTransparent()
        {
            lbl_Gravar.BackColor = System.Drawing.Color.Transparent;
        }


        // TXT ACTUALIZERS
        // --------

        // TEXTBOXES ACTUALIZER
        private void txtboxsActualizer()
        {
            txtbox_FileDirectoryODU.Text = fPathODU;
            txtbox_FileDirectoryCCS.Text = fPathCCS;
            txtbox_FileDirectorySD.Text = fPathSD;
            txtbox_FileDirectoryPD.Text = fPathPD;
            txtbox_FileDirectoryFunerais.Text = fPathFunerais;
            txtBox_ExportDocName.Text = DateTime.Now.Year.ToString() + "-" + "002" + "-" + txtBox_NumOS.Text;
            txtBox_FMemory.Text = Directory.GetCurrentDirectory() + "\\settings.txt";
            chkBox_DebugerMode.Checked = debugMode;
            lbl_version.Text = "Versão: " + version;
            osNumber = txtBox_NumOS.Text;
        }

        // PATH VARS ACTUALIZED BY TEXTBOXES
        private void txtboxsActualizerInvertVars()
        {
            // IN CASE OF MEMORY NOT SAVING THIS WILL CERTIFY THAT 
            // THE PATHS WRITTEN ON THE TEXTBOXES WILL BE USED 
            // BY THE SOFTWARE, SO IT STILL WORKS EVEN NOT ABLE
            // TO SAVE PROPERLY

            fPathODU = txtbox_FileDirectoryODU.Text;
            fPathCCS = txtbox_FileDirectoryCCS.Text;
            fPathSD = txtbox_FileDirectorySD.Text;
            fPathPD = txtbox_FileDirectoryPD.Text;
            fPathFunerais = txtbox_FileDirectoryFunerais.Text;
        }

        // -----------------------------
        // --------------------------------------------------------------------------
        // --------------------------------------------------------------------------





        ////////////////////////////////////////////////////// --------------------- // 
        ////////////////////////////////////////////////////// --- VAR RETURNERS --- //
        ////////////////////////////////////////////////////// --------------------- //

        public static object returnEscalaDate()
        {
            return escalaDay;
        }
        public static object returnOSDateExtensoParse()
        {
            string osDayString = osDay.ToString("D");
            return osDayString;
        }
        public static object returnEscaladosDateParse()
        {
            string diaDeSemanaDeEscala = diaDeEscala.ToString("ddd");
            if (winMode == 2) { MessageBox.Show("diaDeSemanaEscala: " + diaDeSemanaDeEscala); }

            string escaladosDayString = diaDeEscala.ToString("dd") + diaDeEscala.ToString("MMM").ToUpper() + diaDeEscala.ToString("yyyy") + " – " + weekDayParse(diaDeEscala.ToString("ddd"));
            return escaladosDayString;
        }
        public static object returnOSDateABVParse()
        {
            string diaDeOSabv = osDay.ToString("ddd");
            if (winMode == 2) { MessageBox.Show("diaDeOS-Abreviado: " + diaDeOSabv); }

            string osDayABVString = osDay.ToString("dd") + osDay.ToString("MMM").ToUpper() + osDay.ToString("yyyy");
            return osDayABVString;
        }

        // -----------------------------
        // --------------------------------------------------------------------------
        // --------------------------------------------------------------------------






        ////////////////////////////////////////////////////// --------------------- // 
        ////////////////////////////////////////////////////// ------- MODES ------- //
        ////////////////////////////////////////////////////// --------------------- //

        // DEBUGGER MODE
        private void chkBox_DebugerMode_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBox_DebugerMode.Checked)
            {
                debugMode = true;
            }
            else
            {
                debugMode = false;
            }
            check_ifDebugIsActive();
        }
        private void check_ifDebugIsActive()
        {
            if (debugMode == true)
            {
                lbl_Result.Visible = true;
                gBox_FMemory.Visible = true;
                gBox_DebugWindows.Visible = true;
                btn_CheckEscalaList.Visible = true;
                btn_Export_testDaySelect_Var.Visible = true;
                btn_Export_TestListReader.Visible = true;
                if (tabControlOS.SelectedTab == tabControlOS.Controls[0]) { formSizeSwitch("Debug_OSMenu"); }
                if (tabControlOS.SelectedTab == Propriedades) { formSizeSwitch("Debug_Propriedades"); }
            }
            else
            {
                lbl_Result.Visible = false;
                gBox_FMemory.Visible = false;
                gBox_DebugWindows.Visible = false;
                btn_CheckEscalaList.Visible = false;
                btn_Export_testDaySelect_Var.Visible = false;
                btn_Export_TestListReader.Visible = false;
                if (tabControlOS.SelectedTab == tabControlOS.Controls[0]) { formSizeSwitch("NoDebug_OSMenu"); }
                if (tabControlOS.SelectedTab == Propriedades) { formSizeSwitch("NoDebug_Propriedades"); }
            }
        }

        // TAB INDEX CHANGE
        private void tabControlOS_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Check se mudou para outra página durante alterações nas propriedades. Avisar que não gravou alterações!
            if (tabControlOS.SelectedTab != Propriedades && isPathSaved == false)
            {
                DialogResult dialogResult = MessageBox.Show("Esqueceu-se de gravar as alterações.\r\nPretende continuar?", "ALTERAÇÕES POR GRAVAR", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.No)
                {
                    tabControlOS.SelectTab(Propriedades);
                }
                else
                {
                    isPathSaved = true;
                }
            }

            // Alterador de tamanhos conforme pagina seleccionada
            if (debugMode == false)
            {
                if (tabControlOS.SelectedTab == tabControlOS.Controls[0])                           // OS Menu seleccionado
                {
                    //formSizeGeneralSmall();
                    formSizeSwitch("NoDebug_OSMenu");
                }

                if (tabControlOS.SelectedTab == Propriedades)                                       // Propriedades seleccionado
                {
                    //formSizePropSmall();
                    formSizeSwitch("NoDebug_Propriedades");
                }

                if (tabControlOS.SelectedTab == Mensagens || tabControlOS.SelectedTab == Exportar)  // Outros seleccionado
                {
                    //formSizeGeneralMedium();
                    formSizeSwitch("NoDebug_Outros");
                }
            }
            else
            {
                if (tabControlOS.SelectedTab == tabControlOS.Controls[0])
                {
                    //formSizeDebugSmall();
                    formSizeSwitch("Debug_OSMenu");
                }

                if (tabControlOS.SelectedTab == Propriedades)
                {
                    //formSizePropTall();
                    formSizeSwitch("Debug_Propriedades");
                }

                if (tabControlOS.SelectedTab != tabControlOS.Controls[0] && tabControlOS.SelectedTab != Propriedades)
                {
                    formSizeSwitch("Debug_Outros");
                }
            }


        }

        // TEXT PATHS CHANGED
        private void txtBox_FMemory_TextChanged(object sender, EventArgs e)
        {
            isPathSaved = false;
        }
        private void txtbox_FileDirectoryFunerais_TextChanged(object sender, EventArgs e)
        {
            isPathSaved = false;
        }
        private void txtbox_FileDirectoryPD_TextChanged(object sender, EventArgs e)
        {
            isPathSaved = false;
        }
        private void txtbox_FileDirectorySD_TextChanged(object sender, EventArgs e)
        {
            isPathSaved = false;
        }
        private void txtbox_FileDirectoryCCS_TextChanged(object sender, EventArgs e)
        {
            isPathSaved = false;
        }
        private void txtbox_FileDirectoryODU_TextChanged(object sender, EventArgs e)
        {
            isPathSaved = false;
        }
        private void txtBox_NumOS_TextChanged(object sender, EventArgs e)
        {
            txtBox_ExportDocName.Text = DateTime.Now.Year.ToString() + "-" + "002" + "-" + txtBox_NumOS.Text;
            osNumber = txtBox_NumOS.Text;
        }

        // WINDOWS MODE
        private void rbutton_NoDebugWindows_CheckedChanged(object sender, EventArgs e)
        {
            if (rbutton_NoDebugWindows.Checked)
            {
                winMode = 0;
            }
        }
        private void rbutton_lowDebugWindows_CheckedChanged(object sender, EventArgs e)
        {
            if (rbutton_lowDebugWindows.Checked)
            {
                winMode = 1;
            }
        }
        private void rbutton_AllDebugWindows_CheckedChanged(object sender, EventArgs e)
        {
            if (rbutton_AllDebugWindows.Checked)
            {
                winMode = 2;
            }
        }

        // BACKGROUND MODE
        private void chkBox_FundosLigados_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBox_FundosLigados.Checked)
            {
                backgroundMode = 1;
            }
            else
            {
                backgroundMode = 0;
            }
        }
        private void backgroundModeSwitch()
        {
            if (backgroundMode == 0)
            {
                Image formImage = new Bitmap(@"");
                Image escservImage = new Bitmap(@"");
                Image mensagensImage = new Bitmap(@"");
                Image exportarImage = new Bitmap(@"");
                Image propImage = new Bitmap(@"");

                this.BackgroundImage = formImage;
                EscalasServico.BackgroundImage = escservImage;
                Mensagens.BackgroundImage = mensagensImage;
                Exportar.BackgroundImage = exportarImage;
                Propriedades.BackgroundImage = propImage;
            }
            else if (backgroundMode == 1)
            {
                // escrever outros backgrounds "null"
            }
        }

        // FORM SIZE MODES
        private void formSizeSwitch(string activeTab)
        {
            switch(activeTab)
            {
                    // NO DEBUG
                case "NoDebug_OSMenu":  // Size General Small
                    //Size = new Size(602, 390);
                    this.tabControlOS.Location = new Point(Convert.ToInt16(Size.Width*0.05f), Convert.ToInt16(Size.Height*0.10f));
                    this.tabControlOS.Size = new Size(Size.Width - (this.tabControlOS.Location.X * 3),Size.Height - (this.tabControlOS.Location.Y * 2));   // Na Form: 701; 562
                    //this.tabControlOS.Size = new Size(Convert.ToInt16(Size.Width*0.90f), Convert.ToInt16(Size.Height*0.80f));   // Na Form: 701; 562
                    //Size = new Size(this.tabControlOS.Width + 80, this.tabControlOS.Height + 112);  // Na Form: 758; 658
                    break;

                case "NoDebug_Propriedades":  // 

                    this.tabControlOS.Location = new Point(Convert.ToInt16(Size.Width * 0.05f), Convert.ToInt16(Size.Height * 0.10f));
                    this.tabControlOS.Size = new Size(Size.Width - (this.tabControlOS.Location.X * 3), Size.Height - (this.tabControlOS.Location.Y * 2));

                    //this.tabControlOS.Size = new Size(701, 475);  // Na Form: 701; 562
                    //Size = new Size(this.tabControlOS.Width + 80, this.tabControlOS.Height + 100);  // Na Form: 758; 658
                    //this.tabControlOS.Size = new Size(Size.Width - (this.tabControlOS.Location.X * 3), Size.Height - (this.tabControlOS.Location.Y * 2));
                    Properties_savingPanel.Location = new Point(tabControl_Exportar.Location.X, tabControl_Exportar.Location.Y + tabControl_Exportar.Height + 5);
                    //btn_SaveFileLocPD.Location = new Point(14, 385);
                    //prg_SaveButton.Location = new Point(15, 428);
                    //gBox_BackgroundMode.Location = new Point(179, 375);
                    //lbl_Gravar.Location = new Point(82, 397);
                    break;

                case "NoDebug_Outros":
                    //this.tabControlOS.Size = new Size(701, 355);   // Na Form: 701; 562
                    //Size = new Size(this.tabControlOS.Width + 80, this.tabControlOS.Height + 100);  // Na Form: 758; 658

                    this.tabControlOS.Location = new Point(Convert.ToInt16(Size.Width * 0.05f), Convert.ToInt16(Size.Height * 0.10f));
                    this.tabControlOS.Size = new Size(Size.Width - (this.tabControlOS.Location.X * 3), Size.Height - (this.tabControlOS.Location.Y * 2));

                    break;

                    //DEBUG
                case "Debug_OSMenu":
                    //this.tabControlOS.Size = new Size(701, 586);   // Na Form: 701; 562
                    //Size = new Size(this.tabControlOS.Width + 80, this.tabControlOS.Height + 100);  // Na Form: 758; 658

                    this.tabControlOS.Location = new Point(Convert.ToInt16(Size.Width * 0.05f), Convert.ToInt16(Size.Height * 0.10f));
                    this.tabControlOS.Size = new Size(Size.Width - (this.tabControlOS.Location.X * 3), Size.Height - (this.tabControlOS.Location.Y * 2));

                    break;

                case "Debug_Propriedades":

                    this.tabControlOS.Location = new Point(Convert.ToInt16(Size.Width * 0.05f), Convert.ToInt16(Size.Height * 0.10f));
                    this.tabControlOS.Size = new Size(Size.Width - (this.tabControlOS.Location.X * 3), Size.Height - (this.tabControlOS.Location.Y * 2));

                    //this.tabControlOS.Size = new Size(701, 586);  // Na Form: 701; 562
                    //Size = new Size(this.tabControlOS.Width + 80, this.tabControlOS.Height + 100);  // Na Form: 758; 658

                    Properties_savingPanel.Location = new Point(gBox_FMemory.Location.X, gBox_FMemory.Location.Y + gBox_FMemory.Height + 5);

                    //btn_SaveFileLocPD.Location = new Point(14, 484);
                    //prg_SaveButton.Location = new Point(15, 527);
                    //gBox_BackgroundMode.Location = new Point(179, 474);
                    //lbl_Gravar.Location = new Point(82, 496);
                    break;

                case "Debug_Outros":

                    this.tabControlOS.Location = new Point(Convert.ToInt16(Size.Width * 0.05f), Convert.ToInt16(Size.Height * 0.10f));
                    this.tabControlOS.Size = new Size(Size.Width - (this.tabControlOS.Location.X * 3), Size.Height - (this.tabControlOS.Location.Y * 2));

                    //this.tabControlOS.Size = new Size(701, 400);   // Na Form: 701; 562
                    //Size = new Size(this.tabControlOS.Width + 80, this.tabControlOS.Height + 100);  // Na Form: 758; 658
                    break;

                default:
                    this.tabControlOS.Size = new Size(701, 400);   // Na Form: 701; 562
                    Size = new Size(this.tabControlOS.Width + 80, this.tabControlOS.Height + 100);  // Na Form: 758; 658
                    break;
            }
        }
        private void formSizeGeneralSmall()
        {
            this.tabControlOS.Size = new Size(701, 355);   // Na Form: 701; 562
            Size = new Size(758, 405);  // Na Form: 758; 658
        }
        private void frm_OS_system_Resize(object sender, EventArgs e)
        {
            check_ifDebugIsActive();
        }







        // -----------------------------
        // --------------------------------------------------------------------------
        // --------------------------------------------------------------------------


    }
}






//MessageBox.Show("ANTES \n" + fPathPD);   // MENSAGENS DE DEBUG

// -----------------------------
// --------------------------------------------------------------------------
// --------------------------------------------------------------------------




// --------------------- // 
// ------ BACKUPS ------ //
// --------------------- //

// -----------------------------


//private static object returnOSNumber()
//{
//    //string osNumber
//    //public static frm_OS_system FormInstance = new frm_OS_system();

//    string osNumber = frm_OS_system.txtBox_NumOS.Text;
//    return osNumber;
//}


// CHECK EXCEL ROWS
////public void checkRows()
//{
//    outputText = "";  // Clearing the TEXT

//    Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
//    Workbook wb;
//    Worksheet ws;

//    //MessageBox.Show("AGORA \n" + fPathPD);

//    wb = excel.Workbooks.Open(fPathPD);
//    ws = wb.Worksheets[1];
//    string dateToBeSelected;

//    Range searchedRange = excel.get_Range("B15", "K52");
//    dateToBeSelected = selectedDay;

//    Range currentFind = searchedRange.Find(dateToBeSelected);

//    string displayResult = "";
//    string displayResultOut = "";
//    List<string> ResultOutList = new List<string>();


//    // -- FINDER
//    // ------------------------- //
//    if (currentFind != null)
//    {
//        displayResult = "Found at \ncolumn - " + currentFind.Column +
//                                    "\nrow - " + currentFind.Row;


//        int colmn = currentFind.Column;
//        int rowm = currentFind.Row;

//        Range FstCell = ws.Cells[rowm, colmn];
//        Range SndCell = ws.Cells[rowm, "K"];

//        Range cell = ws.Range[FstCell, SndCell];

//        //string CellValue = cell.Value;


//        foreach (string CellOfInterest in cell.Value)
//        {
//            if (CellOfInterest != null)
//            {
//                ResultOutList.Add(CellOfInterest);
//            }
//        }

//        String[] displayResultOutArray = ResultOutList.ToArray();

//        string separator = Environment.NewLine;
//        joinedOutput = string.Join(separator, displayResultOutArray);
//        namesFormater();
//        textBox_Output.Text = outputText;
//    }
//    else
//    {
//        displayResult = "A data que procurou: \"" + selectedDay +
//                "\" não existe na lista.";

//        displayResultOut = "A data que procurou: \"" + selectedDay +
//                            "\" não tem dados registados na lista.";

//        textBox_Output.Text = displayResultOut;
//    }

//    lbl_Result.Text = displayResult;

//    wb.Close(true);
//    excel.Quit();
//}



// NAMES FORMATER
//private void namesFormater()
//{
//    {
//        //stringInput = "30-10-2022\n1CAB/SAS/141368-G IÚRI PARREIRA\nTEN/SAS/140976-L CATARINA AUGUSTO\nTCOR/ABST/142343-O SARA COSTA\n2SAR/OPINF/139108-A PEDRO MANUEL";
//        //stringOutput = "1CAB\tSAS\t141368 G – I. PARREIRA\nTEN\tSAS\t140976 L – C. AUGUSTO\nTCOR\tABST\t142343 G – S. COSTA\n2SAR\tOPINF\t139108 A – P. MANUEL";

//        string[] lines = joinedOutput.Split(Environment.NewLine);


//        foreach (string line in lines)
//        {
//            if (line.Length > 10)
//            {

//                if (line.Contains("\n"))
//                {
//                    string[] lines2 = line.Split(Environment.NewLine);
//                }

//                string[] parts = line.Split(" ");
//                parts[0] = parts[0].Replace("/", "\t");
//                parts[0] = parts[0].Replace("-", " ");
//                parts[1] = parts[1].Substring(0, 1);
//                outputText += parts[0] + " – " + parts[1] + ". " + parts[2] + "\r\n";
//            }
//        }
//    }
//}
// -----------------------------

//private void formSizeGeneralMedium()
//{
//    this.tabControlOS.Size = new Size(701, 400);   // Na Form: 701; 562
//    Size = new Size(758, 500);  // Na Form: 758; 658
//}
//private void formSizeDebugSmall()
//{
//    this.tabControlOS.Size = new Size(701, 586);   // Na Form: 701; 562
//    Size = new Size(758, 673);  // Na Form: 758; 658
//}
//private void formSizePropTall()
//{
//    this.tabControlOS.Size = new Size(701, 586);  // Na Form: 701; 562
//    Size = new Size(758, 673);  // Na Form: 758; 658

//    btn_SaveFileLocPD.Location = new Point(14, 484);
//    prg_SaveButton.Location = new Point(15, 527);
//    gBox_BackgroundMode.Location = new Point(179, 474);
//    lbl_Gravar.Location = new Point(82, 496);

//}
//private void formSizePropSmall()
//{
//    this.tabControlOS.Size = new Size(701, 475);  // Na Form: 701; 562
//    Size = new Size(758, 570);  // Na Form: 758; 658

//    btn_SaveFileLocPD.Location = new Point(14, 385);
//    prg_SaveButton.Location = new Point(15, 428);
//    gBox_BackgroundMode.Location = new Point(179, 375);
//    lbl_Gravar.Location = new Point(82, 397);
//}



