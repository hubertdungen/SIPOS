using SIPOS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SIPOS.Forms
{
    public partial class FormExport : Form
    {
        public FormExport()
        {
            InitializeComponent();
            FormExport formExport = Mediator.formExport;
            Mediator.formExport = this;
        }
        

        // FORM CONTROL
        // ----------
        private void FormExport_Load(object sender, EventArgs e)
        {
            //txtboxsActualizer();
            
            // Will update the boxes with the next OS number.
            UpdateOSMediatorVars(Mediator.inspFilePath);



            txtBox_NumOS.Text = Mediator.osNumber;
            txtBox_ExportDocName.Text = Mediator.exportDocName;
            doesExportFilesExist();
        }
        private void FormExport_FormClosed(object sender, FormClosedEventArgs e)
        {

            //Mediator.osNumber = txtBox_NumOS.Text;
            //Mediator.exportDocName = txtBox_ExportDocName.Text;
        }
        private void FormExport_Enter(object sender, EventArgs e)
        {
            doesExportFilesExist();
        }
        private void FormExport_MouseDown(object sender, MouseEventArgs e)
        {
            doesExportFilesExist();
        }


        // UI CONTROL
        // ----------

        private void txtBox_NumOS_TextChanged(object sender, EventArgs e)
        {
            txtBox_ExportDocName.Text = DateTime.Now.Year.ToString() + "-" + "002" + "-" + txtBox_NumOS.Text;
            Mediator.osNumber = txtBox_NumOS.Text;

            doesExportFilesExist();

        }
        private void txtBox_ExportDocName_TextChanged(object sender, EventArgs e)
        {
            Mediator.exportDocName = txtBox_ExportDocName.Text;

            doesExportFilesExist();
        }
        public static void txtboxsActualizer()
        {
            FormExport frmExport = Mediator.formExport;
            frmExport.txtBox_ExportDocName.Text = DateTime.Now.Year.ToString() + "-" + "002" + "-" + frmExport.txtBox_NumOS.Text;
            Mediator.osNumber = frmExport.txtBox_NumOS.Text;
            //Mediator.osNumber = txtBox_NumOS.Text;
            //Mediator.exportDocName = txtBox_ExportDocName.Text;

        }



        // PREDICT THE NEXT FOLDER NAME
        // ----------

        public static int GetNextOSNumber(string folderPath)
        {
            var docFiles = Directory.GetFiles(folderPath, "*.doc");

            var regexPattern = new Regex(@"(\d{4})-(\d{3})-(\d+)\.doc");

            var orderedFiles = docFiles
                .Select(file => regexPattern.Match(file))
                .Where(match => match.Success)
                .Select(match => new
                {
                    FilePath = match.Value,
                    Year = int.Parse(match.Groups[1].Value),
                    Digits = int.Parse(match.Groups[3].Value)
                })
                .OrderByDescending(file => file.Year)
                .ThenByDescending(file => file.Digits)
                .ToList();

            if (orderedFiles.Count > 0)
            {
                var lastFile = orderedFiles.First();
                return lastFile.Digits + 1;
            }
            else
            {
                return 1;
            }
        }
        public static void UpdateOSMediatorVars(string folderPath)
        {
            int nextOSnumber = GetNextOSNumber(folderPath);
            Mediator.osNumber = nextOSnumber.ToString();

            int currentYear = DateTime.Now.Year;
            Mediator.exportDocName = $"{currentYear}-002-{nextOSnumber}.doc";
        }




        //public string GetNextFileNumber(string folderPath, string filePattern)
        //{
        //    var files = Directory.GetFiles(folderPath, filePattern);
        //    if (files.Any())
        //    {
        //        var lastFile = files.OrderByDescending(f => f).First();
        //        var lastNumber = int.Parse(Path.GetFileNameWithoutExtension(lastFile).Substring(9));
        //        var nextNumber = lastNumber + 1;
        //        return nextNumber.ToString();
        //    }
        //    else
        //    {
        //        return "1";
        //    }
        //}




        // EXPORT
        // ----------

        private void btn_ExportWord_Click(object sender, EventArgs e)
        {
            try
            {
                if (Mediator.osDay.DayOfWeek == DayOfWeek.Tuesday)  // CASO A O.S. seja de TERÇA, ou seja, ESCALA DE SERVIÇO seja QUARTA-FEIRA
                {
                    Word_Processor.CreateWordDocument(Mediator.fPathModelQuarta, Mediator.fPathOSWord + @"\" + txtBox_ExportDocName.Text + ".doc");
                }
                else                             // CASO A O.S. seja noutros dias de semana
                {
                    Word_Processor.CreateWordDocument(Mediator.fPathModelSemana, Mediator.fPathOSWord + @"\" + txtBox_ExportDocName.Text + ".doc");
                }


                doesExportFilesExist();


            }
            catch
            {
                MessageBox.Show("Verifique se tem os modelos de Word completos (sem faltar nenhuma variável), se colocou correctamente os caminhos de cada modelo no campo: \"propriedades\", se colocou o caminho de exportação e por fim se gravou.", "ERRO AO EXPORTAR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            //Word_Processor.CreateWordDocument("", "");
        }
        public void doesExportFilesExist()
        {

            // BOTÕES DO EXPORTADOR

            string filePath = Mediator.fPathOSWord + @"\" + Mediator.exportDocName + ".doc";
            string pdffilePath = Mediator.fPathOSWord + @"\" + Mediator.exportDocName + ".pdf";

            if (File.Exists(filePath))
            {
                // The Word document exists, so make the "btn_OpenWord" button visible
                btn_OpenWord.Visible = true;
                btn_reportWordFile_onExportFolder.Visible = true;
            }
            else
            {
                // The Word document does not exist, so hide the "btn_OpenWord" button
                btn_OpenWord.Visible = false;
                btn_reportWordFile_onExportFolder.Visible= false;
            }


            if (File.Exists(pdffilePath))
            {
                // The PDF document exists
                btn_OpenPDF.Visible = true;
                btn_reportPDFFile_onExportFolder.Visible = true;
            }
            else
            {
                // The PDF document does not exists
                btn_OpenPDF.Visible = false;
                btn_reportPDFFile_onExportFolder.Visible = false;
            }



            // BOTÕES DO INSPECTOR
            

            string inspFilePath = Mediator.inspFilePath + @"\" + Mediator.exportDocName + ".doc";
            string inspPDFfilePath = Mediator.inspFilePath + @"\" + Mediator.exportDocName + ".pdf";
            string inspPDFsignedFilePath = Mediator.inspFilePath + @"\" + Mediator.exportDocName + "_signed.pdf";

            if (File.Exists(inspFilePath))
            {
                // The Word document exists, so make the "btn_OpenWord" button visible
                btn_reportWordFile_onInspect.Visible = true;
            }
            else
            {
                // The Word document does not exist, so hide the "btn_OpenWord" button
                btn_reportWordFile_onInspect.Visible = false;
            }


            if (File.Exists(inspPDFfilePath) || File.Exists(inspPDFsignedFilePath))
            {
                // The PDF document exists
                btn_reportPDFFile_onInspect.Visible = true;
            }
            else
            {
                // The PDF document does not exists
                btn_reportPDFFile_onInspect.Visible = false;
            }

        }
        private void btn_OpenWord_Click(object sender, EventArgs e)
        {

            string filePath = Mediator.fPathOSWord + @"\" + Mediator.exportDocName + ".doc";

            string wordFilePath = "\"" + Mediator.wordAppFilePath + "\"";



            //Process.Start("WORD.EXE", "C:\\Users\\huber\\source\\repos\\SIPOS\\SIPOS\\modelos_word\\exports\\2023-002-50.doc");


            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            //startInfo.FileName = "WINWORD.exe";
            startInfo.FileName = wordFilePath;
            //startInfo.Arguments = wordFilePath;
            startInfo.Arguments = "\"" + filePath + "\"";
            System.Diagnostics.Process.Start(startInfo);

            //filePath = @"""{filePath}""";
            //filePath = "\"" + filePath + "\"";
            //Process.Start("WINWORD.EXE", filePath);

            //try
            //{
            //    string filePath = Mediator.fPathOSWord + @"\" + Mediator.exportDocName + ".doc";
            //    filePath = @"""{filePath}""";
            //    Process.Start("WINWORD.EXE", filePath);
            //}
            //catch (Exception ex)
            //{
            //    // Handle the exception with a pop-up message box in Portuguese
            //    string message = "Ocorreu um erro ao abrir o arquivo: " + ex.Message;
            //    string caption = "Erro ao abrir arquivo";
            //    MessageBoxButtons buttons = MessageBoxButtons.OK;
            //    MessageBoxIcon icon = MessageBoxIcon.Error;
            //    MessageBox.Show(message, caption, buttons, icon);
            //}
        }
        private void btn_OpenPDF_Click(object sender, EventArgs e)
        {

            string filePath = Mediator.fPathOSWord + @"\" + Mediator.exportDocName + ".pdf";
            string pdfFilePath = "\"" + Mediator.pdfAppFilePath + "\"";



            //Process.Start("WORD.EXE", "C:\\Users\\huber\\source\\repos\\SIPOS\\SIPOS\\modelos_word\\exports\\2023-002-50.doc");


            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            //startInfo.FileName = "WINWORD.exe";
            startInfo.FileName = pdfFilePath;
            startInfo.Arguments = "\"" + filePath + "\"";
            System.Diagnostics.Process.Start(startInfo);

        }
        private void btn_reportPDFFile_onInspect_Click(object sender, EventArgs e)
        {

            string pdfFilePath = "\"" + Mediator.pdfAppFilePath + "\"";
            string inspPDFfilePath = Mediator.inspFilePath + @"\" + Mediator.exportDocName + ".pdf";
            string inspPDFsignedFilePath = Mediator.inspFilePath + @"\" + Mediator.exportDocName + "_signed.pdf";

            // ABRIR O PDF QUE ESTÁ NA PASTA DE INSPEÇÃO

            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();

            if (File.Exists(inspPDFsignedFilePath))
            {
                startInfo.FileName = pdfFilePath;
                startInfo.Arguments = "\"" + inspPDFsignedFilePath + "\"";
                System.Diagnostics.Process.Start(startInfo);

            }
            else if (File.Exists(inspPDFfilePath))
            {
                startInfo.FileName = pdfFilePath;
                startInfo.Arguments = "\"" + inspPDFfilePath + "\"";
                System.Diagnostics.Process.Start(startInfo);

            }

        }
        private void btn_reportWordFile_onInspect_Click(object sender, EventArgs e)
        {
            string inspWordfilePath = Mediator.inspFilePath + @"\" + Mediator.exportDocName + ".doc";
            inspWordfilePath = "\"" + inspWordfilePath + "\"";
            string wordFilePath = "\"" + Mediator.wordAppFilePath + "\"";

            // ABRIR O WORD QUE ESTÁ NA PASTA DE INSPEÇÃO

            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.FileName = wordFilePath;
            startInfo.Arguments = inspWordfilePath;
            System.Diagnostics.Process.Start(startInfo);

        }


        // DEBUG MODE
        // ----------

        private void check_ifDebugIsActive()
        {
            if (Mediator.debugMode == true)
            {
                btn_Export_testDaySelect_Var.Visible = true;
                btn_Export_TestListReader.Visible = true;
                //if (tabControlOS.SelectedTab == tabControlOS.Controls[0]) { formSizeSwitch("Debug_OSMenu"); }
                //if (tabControlOS.SelectedTab == Propriedades) { formSizeSwitch("Debug_Propriedades"); }
            }
            else
            {
                btn_Export_testDaySelect_Var.Visible = false;
                btn_Export_TestListReader.Visible = false;
                //if (tabControlOS.SelectedTab == tabControlOS.Controls[0]) { formSizeSwitch("NoDebug_OSMenu"); }
                //if (tabControlOS.SelectedTab == Propriedades) { formSizeSwitch("NoDebug_Propriedades"); }
            }
        }





        private void btn_Export_testDaySelect_Var_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Convert.ToString(Mediator.returnEscalaDate(0)));
        }

        private void btn_Export_TestListReader_Click(object sender, EventArgs e)
        {
            Word_Processor.listToVarsEscalados(0);
        }


    }
}
