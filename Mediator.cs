using SIPOS.Forms;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace SIPOS
{
    internal class Mediator
    {
        // Forms
        // Static properties which hold a reference to the instance of each form
        // Declare the Menu form
        public static frm_OS_system menu;
        public static FormDados? formDados;
        public static FormExport? formExport;
        public static FormHelp? formHelp;
        public static FormInfo? formInfo;
        public static FormModelar? formModelar;
        public static FormPropriedades? formPropriedades;
        public static Form currentChildForm;

        // Update Data Handler
        public delegate void UpdateDataHandler(string message);
        public event UpdateDataHandler UpdateDataEvent;

        // Path Error Check Handler
        public delegate void pathErrorCheckHandler(string path);
        public event pathErrorCheckHandler PathErrorCheck;

        
        // General Path VARS
        public static string filePath = "";
        public static string selectedFolder = "";
        public static string selectedEscala = "";
        private string escalaPreviewText = "";
        public static bool isPathSaved = true;
        private bool nonePathMissing = true;
        public static bool nonePathError = true;
        public static string osNumber = "";
        public static string exportDocName = DateTime.Now.Year.ToString() + "-" + "002" + "-";

        // Memory VARS
        public static string fPathODU = "";
        public static string fPathCCS = "";
        public static string fPathSD = "";
        public static string fPathPD = "";
        public static string fPathFunerais = "";
        public static string fPathMemory = "";
        public static string fPathModelSemana = "";
        public static string fPathModelQuarta = "";
        public static string fPathOSWord = "";
        public static bool fileMemoryDidntExist = false;

        // UI VARS
        public static bool debugMode = false;
        public static int winMode = 0; // 0 = No Windows / 1 = Low Windows / 2 = All Windows
        public static int backgroundMode = 0; // 0 = No Backgrounds / 1 = Bkg Light / 2 = Bkg Dark

        // Calendar VARS
        public static DateTime diaDeEscala = DateTime.Today;
        public static DateTime osDay = DateTime.Today;
        public static bool isItSabado = (diaDeEscala.DayOfWeek == DayOfWeek.Saturday) ? true : false;
        public static bool isItQuarta = (diaDeEscala.DayOfWeek == DayOfWeek.Wednesday) ? true : false;
        public static int plusDayIntrup = 0; // dias de interrupção / feriados / fins de semana

        public static string escalaDay = "";







        // Textbox Update Handlers
        ////public delegate void UpdateFormMenuTextBoxHandler(string value);
        ////public delegate void UpdateFormDadosTextBoxHandler(string value);
        ////public delegate void UpdateFormExportTextBoxHandler(string value);
        ////public delegate void UpdateFormHelpTextBoxHandler(string value);
        ////public delegate void UpdateFormInfoTextBoxHandler(string value);
        ////public delegate void UpdateFormModelarTextBoxHandler(string value);
        ////public delegate void UpdateFormPropriedadesTextBoxHandler(string value);

        ////public static event UpdateFormDadosTextBoxHandler UpdateFormDadosTextBox;
        ////public static event UpdateFormExportTextBoxHandler UpdateFormExportTextBox;
        ////public static event UpdateFormHelpTextBoxHandler UpdateFormHelpTextBox;
        ////public static event UpdateFormInfoTextBoxHandler UpdateFormInfoTextBox;
        ////public static event UpdateFormModelarTextBoxHandler UpdateFormModelarTextBox;
        ////public static event UpdateFormPropriedadesTextBoxHandler UpdateFormPropriedadesTextBox;



        // Method to trigger the event
        public void UpdateData(string message)
        {
            UpdateDataEvent?.Invoke(message);
        }



        

        // --------------------
        // FORMS INTERACTION //

        // Estas funções não estão a ser utilizadas porque abrem uma form fora do menu (erro); Mas poderão ser uteis se corrigir esse erro mais tarde, pois é uma forma de usar menos recursos do que ter todas as janelas abertas

        
        // Open Forms                

        public static void OpenChildForm(Form form)
        {
            currentChildForm?.Close();
            currentChildForm = form;
            form.Show();
        }

        // Close Child Forms
        public static void CloseChildForm(Form form)
        {
            form.Close();
        }

        // Hide Forms
        public static void HideChildForm(Form form)
        {
            form.Hide();
        }

        // .....................


        
        
        

        // -----------------------------
        // MULTIPLE FORMS INTERACTION //

        public static void txtboxsActualizer()
        {
            frm_OS_system.txtboxsActualizer();

            if (formDados != null)
            {
                FormDados.txtboxsActualizer();
            }

            if (formExport != null)
            {
                FormExport.txtboxsActualizer();
            }

            if (formPropriedades != null)
            {
                FormPropriedades.txtboxsActualizer();
            }
        }


        // ..............................







        // ------------------------
        // MENU FORM INTERACTION //






        // ........................





        // -----------------------------
        // ESCALAS ENGINE INTERACTION //
        public static void instTriagemEscalas()
        {
            var escalasEngine = new EscalasEngine();
            escalasEngine.triagemEscalas();
        }


        // .............................




        ////////////////////////////////////////////////////// --------------------- // 
        ////////////////////////////////////////////////////// - SAVE/LOAD MEMORY -- //
        ////////////////////////////////////////////////////// --------------------- //

        // SAVER
        public static void saveMemory()
        {
            string fMemoryPath = Directory.GetCurrentDirectory() + "\\settings.txt";

            try
            {
                TextWriter tw = new StreamWriter(fMemoryPath);

                // Debug MsgBox EachFile
                if (winMode == 2) { MessageBox.Show("fPathPD: " + fPathPD + "\nfMemoryPath: " + fMemoryPath + "\nfilePathSelected" + filePath); }

                // write lines of text to the file
                tw.WriteLine(frm_OS_system.version);
                tw.WriteLine(fPathODU);
                tw.WriteLine(fPathCCS);
                tw.WriteLine(fPathSD);
                tw.WriteLine(fPathPD);
                tw.WriteLine(fPathFunerais);
                tw.WriteLine(debugMode);
                tw.WriteLine(winMode);
                tw.WriteLine(fPathModelSemana);   //tw.WriteLine(txtbox_FileDirectory_ModelSemana.Text);
                tw.WriteLine(fPathOSWord);      //tw.WriteLine(txtbox_FolderDirectory_OSWord.Text);
                tw.WriteLine(fPathModelQuarta);   //tw.WriteLine(txtbox_FileDirectory_ModelQuarta.Text);
                tw.WriteLine(fPathOSWord);


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
            if (fileMemoryDidntExist == false) { formPropriedades.prg_SaveButton_AddInc(0); } else { formPropriedades.prg_SaveButton_Minimum(); }
        }
        
        // CHECKING ERRORS AND SAVING PATHS
        public static void chkANDsaveMemory(string txtBoxSelected)
        {
            if (txtBoxSelected.EndsWith(".xls"))
            {
                filePath = txtBoxSelected;
                formPropriedades.prg_SaveButton_AddInc(0); //add inc saving progress bar
                saveMemory();
            }
            else if ((txtBoxSelected == "") || (txtBoxSelected == null))
            {
                MessageBox.Show($"Não seleccionou um caminho para ficheiro Excel da escala {selectedEscala}!\nPor favor seleccione um ficheiro \".XLS\".", $"CAMINHO NÃO ESPECIFICADO!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtBoxSelected = "";
                nonePathError = false;
                Mediator.txtboxsActualizer();

            }
            else if (!Regex.IsMatch(txtBoxSelected, @"(.)[A-Za-z0-9]{3}$"))
            {
                MessageBox.Show($"Caminho seleccionado:\r\n{txtBoxSelected}\r\n" + $"O caminho seleccionado da escala {selectedEscala} leva o programa a uma pasta e não a um ficheiro Excel!\nPor favor seleccione um ficheiro \".XLS\".", $"PASTA SELECCIONADA!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtBoxSelected = "";
                nonePathError = false;
                Mediator.txtboxsActualizer();
            }
            else
            {
                MessageBox.Show($"Caminho seleccionado:\r\n{txtBoxSelected}\r\n" + $"O ficheiro que seleccionou para a escala {selectedEscala} não é um ficheiro de Excel compatível!\nPor favor seleccione um ficheiro \".XLS\".", $"FICHEIRO INCOMPATÍVEL!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtBoxSelected = "";
                nonePathError = false;
                Mediator.txtboxsActualizer();
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
                Mediator.txtboxsActualizer();

            }
            else if (!Regex.IsMatch(fMemoryPath, @"(.)[A-Za-z0-9]{3}$"))
            {
                MessageBox.Show($"Caminho seleccionado:\r\n{fMemoryPath}\r\n" + $"O caminho seleccionado da escala {selectedEscala} leva o programa a uma pasta e não a um ficheiro Excel!\nPor favor seleccione um ficheiro \".XLS\".", $"PASTA SELECCIONADA!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                nonePathError = false;
                Mediator.txtboxsActualizer();
            }
            else
            {
                MessageBox.Show($"Caminho seleccionado:\r\n{fMemoryPath}\r\n" + $"O ficheiro que seleccionou para a escala {selectedEscala} não é um ficheiro de Excel compatível!\nPor favor seleccione um ficheiro \".XLS\".", $"FICHEIRO INCOMPATÍVEL!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                nonePathError = false;
                Mediator.txtboxsActualizer();
            }
        }

        // LOADER
        public void loadMemory()
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

                if (frm_OS_system.version == checkVersion) // validar ficheiro  - LÊ A MEMORIA
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
                    fPathModelSemana = tr.ReadLine();
                    fPathOSWord = tr.ReadLine();
                    fPathModelQuarta = tr.ReadLine();
                    fPathOSWord = tr.ReadLine();



                    // close the stream
                    tr.Close();
                    fileMemoryDidntExist = false;
                }
                else    // CASO NÃO SEJA MESMA VERSÃO // Recria um ficheiro para evitar erros
                {
                    try
                    {
                        saveMemory();
                        MessageBox.Show($"A versão do ficheiro de memória ({checkVersion}) não era a mesma deste software: {frm_OS_system.version}.\r\nPor questões de segurança o software limpa o ficheiro de preferências sempre que há uma grande actualização.\r\n\r\nTerá de ir às propriedades carregar os ficheiros Excel e as suas preferências.", "ACTUALIZAÇÃO!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch // Caso não consiga aceder ou gravar sobre o ficheiro, dá uma mensagem de alerta.
                    {
                        MessageBox.Show($"O ficheiro de memória não pertencia à versão deste software, portanto o programa tentou actualizar o ficheiro, mas sem sucesso, o que resulta nesta instância em não ler as preferências guardadas.\r\n\r\nO software está a ter problemas em aceder ao ficheiro de memória \"settings.txt\" que se encontra localizado em: \r\n{fMemoryPath}\r\n" + "\r\nPor favor verifique se alguma aplicação está a utilizar o ficheiro. Caso não esteja a ser utilizado por nenhum software, tente o seguinte: \r\n-> Feche o programa\r\n-> Elimine o ficheiro\r\n-> Volte a entrar para o programa criar um novo ficheiro \"settings.txt\"", "ERRO AO LER E ACTUALIZAR A MEMÓRIA!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    fileMemoryDidntExist = true;
                }
            }

            txtboxsActualizer();
        }


        // OPEN DIALOGS
        // --------

        // FILE OPENER
        public static void openFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                filePath = openFileDialog.FileName;
                if (winMode == 2) { MessageBox.Show("File directory: " + filePath); }
            }
        }

        // FOLDER OPENER
        public static void openFolder()
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
        public static void pathErrorCheck(string filePath)
        {



            if (fileMemoryDidntExist == false) // CASO existia ficheiro memória presente avisa  // Caso contrário evita mensagens desnecessárias
            {
                if (!File.Exists(filePath))
                {
                    MessageBox.Show($"Caminho seleccionado:\r\n{filePath}\r\n" + $"O ficheiro que seleccionou para a escala {selectedEscala} não existe no sistema!\nPor favor seleccione um ficheiro \".XLS\".", "FICHEIRO NÃO EXISTENTE!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    nonePathError = false;
                    Mediator.txtboxsActualizer();
                }
                else if ((filePath == "") || (filePath == null))
                {
                    MessageBox.Show($"Não seleccionou um caminho para ficheiro Excel da escala {selectedEscala}!\nPor favor seleccione um ficheiro \".XLS\".", $"CAMINHO NÃO ESPECIFICADO!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    nonePathError = false;
                    Mediator.txtboxsActualizer();
                }
                else if (!Regex.IsMatch(filePath, @"(.)[A-Za-z0-9]{3}$"))
                {
                    MessageBox.Show($"Caminho seleccionado:\r\n{filePath}\r\n" + $"O caminho seleccionado da escala {selectedEscala} leva o programa a uma pasta e não a um ficheiro Excel!\nPor favor seleccione um ficheiro \".XLS\".", $"PASTA SELECCIONADA!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    nonePathError = false;
                    Mediator.txtboxsActualizer();
                }
                else
                {
                    return;
                }
            }
            else
            {
                nonePathError = false;
                Mediator.txtboxsActualizer();
            }
        }

        // INSTANCE CALLERS
        
        //public static void instpathErrorCheck(string filePath)
        //{
        //    var instfOS = new frm_OS_system();
        //    instfOS.pathErrorCheck(filePath);
        //}


        // -----------------------------
        // --------------------------------------------------------------------------
        // --------------------------------------------------------------------------






        // -------------------------
        // FORM DADOS INTERACTION //

        // PROGRESS BAR
        public static void instPrgBarAddInc(int addMore)
        {
            if (Mediator.formDados != null)
            {
                formDados.prgBarAddInc(addMore);
            }
            
        }
        public static void instPrgBarToMax()
        {
            if (Mediator.formDados != null)
            {
                formDados.prgBarToMax();
            }

        }
        public static void instPrgBarReset()
        {
            if (Mediator.formDados != null)
            {
                formDados.prgBarReset();
            }
        }
        public static void instPrgBarFix()
        {
            if (Mediator.formDados != null)
            {
                formDados.prgBarFix();
            }
        }


        // TEXT BOX PREVIEW
        public static void instTxtBox_Clear()
        {
            if (formDados != null)
            {
                formDados.txtBox_Clear();
            }
        }
        public static void instTxtBox_Equal_To(string TextInput)
        {
            if (formDados != null)
            {
                formDados.txtBox_Equal_To(TextInput);
            }
        }
        public static void instDateProcess(int num)
        {
            if (formDados != null)
            {
                formDados.dateProcess(num);
            }
        }
        public static void instAfterToday()
        {
            if (formDados != null)
            {
                formDados.afterToday();
            }
        }

        // .............................




        ////////////////////////////////////////////////////// --------------------- // 
        ////////////////////////////////////////////////////// --- VAR RETURNERS --- //
        ////////////////////////////////////////////////////// --------------------- //

        public static object returnEscalaDate(int plusDay)
        {
            // Converte a data da escala para a data escolhida no motor para ser exportada...
            // P.ex.: O.S. de sexta-feira, se o SIPOS estiver a ver o domingo, o escalaDay vai ser o sábado e o que o returnEscalaDate vai returnar é o Domingo.

            diaDeEscala.AddDays(plusDay);
            return diaDeEscala.ToString("dd-MM-yyyy");
            diaDeEscala.AddDays(-plusDay);
        }
        public static object returnOSDateExtensoParse()
        {
            string osDayString = osDay.ToString("D");
            return osDayString;
        }
        public static object returnEscaladosDateParse()
        {
            // Esta zona retorna todos os dias indicados na primeira página e nos cabeçalhos da O.S. excepto a escala de serviço.

            string diaDeSemanaDeEscala = diaDeEscala.ToString("ddd");
            if (winMode == 2) { MessageBox.Show("diaDeSemanaEscala: " + diaDeSemanaDeEscala); }

            string escaladosDayString = diaDeEscala.ToString("dd") + diaDeEscala.ToString("MMM").ToUpper() + diaDeEscala.ToString("yyyy") + " – " + FormDados.weekDayParse(diaDeEscala.ToString("ddd"));
            return escaladosDayString;
        }
        public static object returnOSDateABVParse()
        {
            string diaDeOSabv = osDay.ToString("ddd");
            if (winMode == 2) { MessageBox.Show("diaDeOS-Abreviado: " + diaDeOSabv); }

            string osDayABVString = osDay.ToString("dd") + osDay.ToString("MMM").ToUpper() + osDay.ToString("yyyy");
            return osDayABVString;
        }


        public static string returnOSextensiveDate()
        {
            string osExtensiveDate = osDay.ToString("dd") + " de " + osDay.ToString("MMMM") + " de " + osDay.ToString("yyyy");
            return osExtensiveDate;
        }

        // -----------------------------
        // --------------------------------------------------------------------------
        // --------------------------------------------------------------------------














        // BACKGROUND MODE
        private void chkBox_FundosLigados_CheckedChanged(object sender, EventArgs e)
        {
            //if (chkBox_FundosLigados.Checked)
            //{
            //    backgroundMode = 1;
            //}
            //else
            //{
            //    backgroundMode = 0;
            //}
        }
        private void backgroundModeSwitch()
        {
            if (backgroundMode == 0)
            {
                //Image formImage = new Bitmap(@"");
                //Image escservImage = new Bitmap(@"");
                //Image mensagensImage = new Bitmap(@"");
                //Image exportarImage = new Bitmap(@"");
                //Image propImage = new Bitmap(@"");

                //currentChildForm.BackgroundImage = formImage;
                //currentChildForm.EscalasServico.BackgroundImage = escservImage;
                //currentChildForm.Mensagens.BackgroundImage = mensagensImage;
                //currentChildForm.Exportar.BackgroundImage = exportarImage;
                //currentChildForm.Propriedades.BackgroundImage = propImage;
                
                
            }
            else if (backgroundMode == 1)
            {
                // escrever outros backgrounds "null"
            }
        }


        // -----------------------------
        // --------------------------------------------------------------------------
        // --------------------------------------------------------------------------







    }
}
