﻿using SIPOS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
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

        private void FormExport_Load(object sender, EventArgs e)
        {
            //txtboxsActualizer();
            txtBox_NumOS.Text = Mediator.osNumber;
            txtBox_ExportDocName.Text = Mediator.exportDocName;
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

        public string GetNextFileNumber(string folderPath, string filePattern)
        {
            var files = Directory.GetFiles(folderPath, filePattern);
            if (files.Any())
            {
                var lastFile = files.OrderByDescending(f => f).First();
                var lastNumber = int.Parse(Path.GetFileNameWithoutExtension(lastFile).Substring(9));
                var nextNumber = lastNumber + 1;
                return nextNumber.ToString();
            }
            else
            {
                return "1";
            }
        }




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
            string filePath = Mediator.fPathOSWord + @"\" + Mediator.exportDocName + ".doc";
            string pdffilePath = Mediator.fPathOSWord + @"\" + Mediator.exportDocName + ".pdf";

            if (File.Exists(filePath))
            {
                // The Word document exists, so make the "btn_OpenWord" button visible
                btn_OpenWord.Visible = true;
                btn_reportWordFile.Visible = true;
            }
            else
            {
                // The Word document does not exist, so hide the "btn_OpenWord" button
                btn_OpenWord.Visible = false;
                btn_reportWordFile.Visible= false;
            }


            if (File.Exists(pdffilePath))
            {
                // The PDF document exists
                btn_reportPDFFile.Visible = true;
            }
            else
            {
                // The PDF document does not exists
                btn_reportPDFFile.Visible = false;
            }



        }
        private void btn_OpenWord_Click(object sender, EventArgs e)
        {

            string filePath = Mediator.fPathOSWord + @"\" + Mediator.exportDocName + ".doc";
            string wordFilePath = "\"" + Mediator.wordAppFilePath + "\"";



            //Process.Start("WORD.EXE", "C:\\Users\\huber\\source\\repos\\SIPOS\\SIPOS\\modelos_word\\exports\\2023-002-50.doc");


            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            //startInfo.FileName = "WINWORD.exe";
            startInfo.FileName = Mediator.wordAppFilePath;
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



        private void FormExport_FormClosed(object sender, FormClosedEventArgs e)
        {

            //Mediator.osNumber = txtBox_NumOS.Text;
            //Mediator.exportDocName = txtBox_ExportDocName.Text;
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
