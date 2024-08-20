using Microsoft.Office.Interop.Word;
using SIPOS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SIPOS.Forms
{
    public partial class FormPropriedades : Form
    {
        public FormPropriedades()
        {
            InitializeComponent();
            FormPropriedades formPropriedades = Mediator.formPropriedades;
            Mediator.formPropriedades = this;


        }


        private void FormPropriedades_Load(object sender, EventArgs e)
        {
            check_ifDebugIsActive();

            txtbox_FileDirectoryODU.Text = Mediator.fPathODU;
            txtbox_FileDirectoryCCS.Text = Mediator.fPathCCS;
            txtbox_FileDirectorySD.Text = Mediator.fPathSD;
            txtbox_FileDirectoryPD.Text = Mediator.fPathPD;
            txtbox_FileDirectoryFunerais.Text = Mediator.fPathFunerais;
            txtbox_FileDirectory_ModelSemana.Text = Mediator.fPathModelSemana;
            txtbox_FileDirectory_ModelQuarta.Text = Mediator.fPathModelQuarta;
            txtbox_FolderDirectory_OSWord.Text = Mediator.fPathOSWord;
            txtBox_WordAppPath.Text = Mediator.wordAppFilePath;
            txtBox_PDFAppPath.Text = Mediator.pdfAppFilePath;
            txtBox_InspFilePath.Text = Mediator.inspFilePath;
            chkBox_DebugerMode.Checked = Mediator.debugMode;
            chkBox_AutoExcelSearch.Checked = Mediator.autoExcelSearch;
            chkBox_ForcedExcelSearch.Checked = !Mediator.autoExcelSearch;
            txtbox_ColAutoData.Text = Mediator.autoExcelData;
            txtbox_ColAutoEfetivo.Text = Mediator.autoExcelEfetivo;
            txtbox_ColAutoReserva.Text = Mediator.autoExcelReserva;
            txtbox_ColForcedData.Text = Mediator.forcedExcelData;
            txtbox_ColForcedEfetivo.Text = Mediator.forcedExcelEfetivo;
            txtbox_ColForcedReserva.Text = Mediator.forcedExcelReserva;
            

            if (Mediator.winMode == 1 && Mediator.debugMode == true) { rbutton_lowDebugWindows.Checked = true; } else if (Mediator.winMode == 2 && Mediator.debugMode == true) { rbutton_AllDebugWindows.Checked = true; }

        }



        // TXT ACTUALIZERS
        // --------

        // TEXTBOXES ACTUALIZER
        public static void txtboxsActualizer()
        {
            FormPropriedades frmPropriedades = Mediator.formPropriedades;
            frmPropriedades.txtbox_FileDirectoryODU.Text = Mediator.fPathODU;
            frmPropriedades.txtbox_FileDirectoryCCS.Text = Mediator.fPathCCS;
            frmPropriedades.txtbox_FileDirectorySD.Text = Mediator.fPathSD;
            frmPropriedades.txtbox_FileDirectoryPD.Text = Mediator.fPathPD;
            frmPropriedades.txtbox_FileDirectoryFunerais.Text = Mediator.fPathFunerais;
            frmPropriedades.txtBox_FMemory.Text = Mediator.fPathMemory;
            frmPropriedades.txtbox_FileDirectory_ModelSemana.Text = Mediator.fPathModelSemana;
            frmPropriedades.txtbox_FileDirectory_ModelQuarta.Text = Mediator.fPathModelQuarta;
            frmPropriedades.txtbox_FolderDirectory_OSWord.Text = Mediator.fPathOSWord;
            frmPropriedades.chkBox_DebugerMode.Checked = Mediator.debugMode;
            frmPropriedades.chkBox_AutoExcelSearch.Checked = Mediator.autoExcelSearch;
            frmPropriedades.chkBox_ForcedExcelSearch.Checked =! Mediator.autoExcelSearch;
            frmPropriedades.txtbox_ColAutoData.Text = Mediator.autoExcelData;
            frmPropriedades.txtbox_ColAutoEfetivo.Text = Mediator.autoExcelEfetivo;
            frmPropriedades.txtbox_ColAutoReserva.Text = Mediator.autoExcelReserva;
            frmPropriedades.txtbox_ColForcedData.Text = Mediator.forcedExcelData;
            frmPropriedades.txtbox_ColForcedEfetivo.Text = Mediator.forcedExcelEfetivo;
            frmPropriedades.txtbox_ColForcedReserva.Text = Mediator.forcedExcelReserva;
            if (Mediator.winMode == 1 && Mediator.debugMode == true) { frmPropriedades.rbutton_lowDebugWindows.Checked = true; } else if (Mediator.winMode == 2 && Mediator.debugMode == true) { frmPropriedades.rbutton_AllDebugWindows.Checked = true; }
        }

        // PATH VARS ACTUALIZED BY TEXTBOXES
        public void txtboxsActualizerInvertVars()
        {
            // IN CASE OF MEMORY NOT SAVING THIS WILL CERTIFY THAT 
            // THE PATHS WRITTEN ON THE TEXTBOXES WILL BE USED 
            // BY THE SOFTWARE, SO IT STILL WORKS EVEN NOT ABLE
            // TO SAVE PROPERLY

            Mediator.fPathODU = txtbox_FileDirectoryODU.Text;
            Mediator.fPathCCS = txtbox_FileDirectoryCCS.Text;
            Mediator.fPathSD = txtbox_FileDirectorySD.Text;
            Mediator.fPathPD = txtbox_FileDirectoryPD.Text;
            Mediator.fPathFunerais = txtbox_FileDirectoryFunerais.Text;
        }



        // TEXT PATHS CHANGED
        private void txtBox_FMemory_TextChanged(object sender, EventArgs e)
        {
            Mediator.isPathSaved = false;
        }
        private void txtbox_FileDirectoryFunerais_TextChanged(object sender, EventArgs e)
        {
            Mediator.isPathSaved = false;

        }
        private void txtbox_FileDirectoryPD_TextChanged(object sender, EventArgs e)
        {
            Mediator.isPathSaved = false;

        }
        private void txtbox_FileDirectorySD_TextChanged(object sender, EventArgs e)
        {
            Mediator.isPathSaved = false;

        }
        private void txtbox_FileDirectoryCCS_TextChanged(object sender, EventArgs e)
        {
            Mediator.isPathSaved = false;

        }
        private void txtbox_FileDirectoryODU_TextChanged(object sender, EventArgs e)
        {
            Mediator.isPathSaved = false;

        }
        private void txtbox_FileDirectory_ModelSemana_TextChanged(object sender, EventArgs e)
        {
            Mediator.isPathSaved = false;

        }
        private void txtbox_FileDirectory_ModelFDSemana_TextChanged(object sender, EventArgs e)
        {
            Mediator.isPathSaved = false;

        }
        private void txtbox_FolderDirectory_OSWord_TextChanged(object sender, EventArgs e)
        {
            Mediator.isPathSaved = false;

        }
        private void txtBox_WordAppPath_TextChanged(object sender, EventArgs e)
        {
            Mediator.isPathSaved = false;
        }




        // BUTTONS
        private void btn_SaveFileLocPD_Click(object sender, EventArgs e)
        {
            prg_SaveButton.Value = prg_SaveButton.Minimum;


            //// FILEPATHS DAS ESCALAS 
            Mediator.fPathODU = txtbox_FileDirectoryODU.Text;                      // Grava a PATH seleccionada no fPathODU
            Mediator.selectedEscala = "ODU";
            Mediator.chkANDsaveMemory(Mediator.fPathODU); // CHECKAR TEXTO E GRAVAR 

            Mediator.fPathCCS = txtbox_FileDirectoryCCS.Text;                      // Grava a PATH seleccionada no fPathCCS
            Mediator.selectedEscala = "CCS";
            Mediator.chkANDsaveMemory(Mediator.fPathCCS); // CHECKAR TEXTO E GRAVAR 

            Mediator.fPathSD = txtbox_FileDirectorySD.Text;                      // Grava a PATH seleccionada no fPathSD
            Mediator.selectedEscala = "SD";
            Mediator.chkANDsaveMemory(Mediator.fPathSD); // CHECKAR TEXTO E GRAVAR 

            Mediator.fPathPD = txtbox_FileDirectoryPD.Text;                      // Grava a PATH seleccionada no fPathPD
            Mediator.selectedEscala = "PD";
            Mediator.chkANDsaveMemory(Mediator.fPathPD); // CHECKAR TEXTO E GRAVAR 

            Mediator.fPathFunerais = txtbox_FileDirectoryFunerais.Text;                      // Grava a PATH seleccionada no fPathFunerais
            Mediator.selectedEscala = "Funerais";
            Mediator.chkANDsaveMemory(Mediator.fPathFunerais); // CHECKAR TEXTO E GRAVAR 


            //// FILEPATHS DOS WORDS
            Mediator.fPathModelSemana = txtbox_FileDirectory_ModelSemana.Text;                      // Grava a PATH seleccionada do Modelo de Semana
            Mediator.fPathModelQuarta = txtbox_FileDirectory_ModelQuarta.Text;                      // Grava a PATH seleccionada do Modelo de Quarta
            Mediator.fPathOSWord = txtbox_FolderDirectory_OSWord.Text;

            Mediator.wordAppFilePath = txtBox_WordAppPath.Text;
            Mediator.pdfAppFilePath = txtBox_PDFAppPath.Text;
            Mediator.inspFilePath = txtBox_InspFilePath.Text;

            //// ^^^^^ ATENÇÃO! NÃO ESQUECER DE CRIAR UM METODO QUE CHEQUE PELO PREENCHIMENTO CORRECTO DESTAS VARIAVEIS | TÊM DE SER SÓ ACEITES EM FORMATO WORD (.docx) !!!


            Mediator.saveMemory();



            Mediator.selectedEscala = ""; // Clear VAR

            if (Mediator.isPathSaved == true)
            {
                Mediator.isPathSaved = true;  // Regista que ficou guardado

                if (Mediator.nonePathError == true)
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

        // BUTTONS: OPEN FILE DIALOGS
        private void btn_searchFile_ODU_Click(object sender, EventArgs e)
        {
            Mediator.openFile();
            txtbox_FileDirectoryODU.Text = Mediator.filePath;
        }
        private void btn_searchFile_CCS_Click(object sender, EventArgs e)
        {
            Mediator.openFile();
            txtbox_FileDirectoryCCS.Text = Mediator.filePath;
        }
        private void btn_searchFile_PD_Click(object sender, EventArgs e)
        {
            Mediator.openFile();
            txtbox_FileDirectoryPD.Text = Mediator.filePath;
        }
        private void btn_searchFile_SD_Click(object sender, EventArgs e)
        {
            Mediator.openFile();
            txtbox_FileDirectorySD.Text = Mediator.filePath;
        }
        private void btn_searchFile_Funerais_Click(object sender, EventArgs e)
        {
            Mediator.openFile();
            txtbox_FileDirectoryFunerais.Text = Mediator.filePath;
        }
        private void btn_search_File_ModelSemana_Click(object sender, EventArgs e)
        {
            Mediator.openFile();
            txtbox_FileDirectory_ModelSemana.Text = Mediator.filePath;
        }
        private void btn_search_File_FDSemana_Click(object sender, EventArgs e)
        {
            Mediator.openFile();
            txtbox_FileDirectory_ModelFDSemana.Text = Mediator.filePath;
        }
        private void btn_search_File_Quarta_Click(object sender, EventArgs e)
        {
            Mediator.openFile();
            txtbox_FileDirectory_ModelQuarta.Text = Mediator.filePath;
        }
        private void btn_searchFile_WordApp_Click(object sender, EventArgs e)
        {
            Mediator.openFile();
            txtBox_WordAppPath.Text = Mediator.filePath;
        }
        private void btn_searchFile_PDFApp_Click(object sender, EventArgs e)
        {
            Mediator.openFile();
            txtBox_PDFAppPath.Text = Mediator.filePath;
        }
        private void btn_searchFolder_InspFiles_Click(object sender, EventArgs e)
        {
            Mediator.openFolder();
            txtBox_InspFilePath.Text = Mediator.selectedFolder;
        }
        private void btn_Save_Folder_OSWord_Click(object sender, EventArgs e)
        {
            Mediator.openFolder();
            txtbox_FolderDirectory_OSWord.Text = Mediator.selectedFolder;
        }




        // WINDOWS MODE

        private void rbutton_NoDebugWindows_CheckedChanged_1(object sender, EventArgs e)
        {
            if (rbutton_NoDebugWindows.Checked)
            {
                Mediator.winMode = 0;
                //MessageBox.Show("Mediator.winMode: " + Mediator.winMode, "Informação de Troubleshooting");
            }
        }

        private void rbutton_lowDebugWindows_CheckedChanged_1(object sender, EventArgs e)
        {
            if (rbutton_NoDebugWindows.Checked)
            {
                Mediator.winMode = 0;
                //MessageBox.Show("Mediator.winMode: " + Mediator.winMode, "Informação de Troubleshooting");
            }
        }

        private void rbutton_AllDebugWindows_CheckedChanged_1(object sender, EventArgs e)
        {
            if (rbutton_AllDebugWindows.Checked)
            {
                Mediator.winMode = 2;
                //MessageBox.Show("Mediator.winMode: " + Mediator.winMode, "Informação de Troubleshooting");
            }
        }




        // FORM CONTROL
        public void prg_SaveButton_AddInc(int addMore)
        {

            if (prg_SaveButton.Value == prg_SaveButton.Minimum)
            {
                prg_SaveButton.Value += prg_SaveButton.Value + 1 + addMore;
            }
            else if (prg_SaveButton.Value + 1 > prg_SaveButton.Minimum)
            {
                prg_SaveButton.Value += (int)((prg_SaveButton.Maximum - prg_SaveButton.Value) * 0.5f);
            }
            else
            {
                prg_SaveButton.Value += 1;
            }


            if (prg_SaveButton.Value >= prg_SaveButton.Maximum)
            {
                prg_SaveButton.Value = prg_SaveButton.Maximum;
            }
        }
        public void prg_SaveButton_Minimum()
        {
            prg_SaveButton.Value = 0;
        }



        ////////////////////////////////////////////////////// --------------------- // 
        ////////////////////////////////////////////////////// ------- MODES ------- //
        ////////////////////////////////////////////////////// --------------------- //

        // DEBUGGER MODE
        private void chkBox_DebugerMode_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBox_DebugerMode.Checked)
            {
                Mediator.debugMode = true;
            }
            else
            {
                Mediator.debugMode = false;
            }
            check_ifDebugIsActive();
        }

        private void check_ifDebugIsActive()
        {
            if (Mediator.debugMode == true)
            {
                gBox_FMemory.Visible = true;
                gBox_DebugWindows.Visible = true;
                chkBox_DebugerMode.Checked = true;
                chkBox_VisibleWordExportProcess.Visible = true;
                //tpage_Debug.Locked = false;
                //tabControl_Propriedades.TabIndex(4).Locked = false;



            }
            else
            {
                gBox_FMemory.Visible = false;
                gBox_DebugWindows.Visible = false;
                chkBox_DebugerMode.Checked = false;
                chkBox_VisibleWordExportProcess.Visible = false;


                //tabControl_Propriedades.TabIndex(4).Locked = true;
                //if (tabControlOS.SelectedTab == tabControlOS.Controls[0]) { formSizeSwitch("NoDebug_OSMenu"); }
                //if (tabControlOS.SelectedTab == Propriedades) { formSizeSwitch("NoDebug_Propriedades"); }
            }
        }

        private void tabControl_Exportar_SelectedIndexChanged(object sender, EventArgs e)
        {

        }



        ////////////////////////////////////////////////////// -------------------------------------------- // 
        ////////////////////////////////////////////////////// ------- ESCALAS ENGINE EXCEL FINDER ------- //
        ////////////////////////////////////////////////////// ------------------------------------------ //

        private void chkBox_VisibleWordExportProcess_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBox_VisibleWordExportProcess.Checked == true)
            {
                Mediator.isExportVisible = true;
            }
            else
            {
                Mediator.isExportVisible = false;
            }
        }

        private void chkBox_AutoExcelSearch_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBox_AutoExcelSearch.Checked == true)
            {
                Mediator.autoExcelSearch = true;
                chkBox_ForcedExcelSearch.Checked = false;
            }
            else
            {
                Mediator.autoExcelSearch = false;
                chkBox_ForcedExcelSearch.Checked = true;
            }
        }

        private void chkBox_ForcedExcelSearch_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBox_ForcedExcelSearch.Checked == true)
            {
                Mediator.autoExcelSearch = false;
                chkBox_AutoExcelSearch.Checked = false;
            }
            else
            {
                Mediator.autoExcelSearch = true;
                chkBox_AutoExcelSearch.Checked = true;
            }
        }

        private void txtbox_ColAutoData_TextChanged(object sender, EventArgs e)
        {
            Mediator.autoExcelData = txtbox_ColAutoData.Text;
            Mediator.isPathSaved = false;
        }

        private void txtbox_ColAutoEfetivo_TextChanged(object sender, EventArgs e)
        {
            Mediator.autoExcelEfetivo = txtbox_ColAutoEfetivo.Text;
            Mediator.isPathSaved = false;
        }

        private void txtbox_ColAutoReserva_TextChanged(object sender, EventArgs e)
        {
            Mediator.autoExcelReserva = txtbox_ColAutoReserva.Text;
            Mediator.isPathSaved = false;
        }

        private void txtbox_ColForcedData_TextChanged(object sender, EventArgs e)
        {
            Mediator.forcedExcelData = txtbox_ColForcedData.Text;
            Mediator.isPathSaved = false;
        }

        private void txtbox_ColForcedEfetivo_TextChanged(object sender, EventArgs e)
        {
            Mediator.forcedExcelEfetivo = txtbox_ColForcedEfetivo.Text;
            Mediator.isPathSaved = false;
        }

        private void txtbox_ColForcedReserva_TextChanged(object sender, EventArgs e)
        {
            Mediator.forcedExcelReserva = txtbox_ColForcedReserva.Text;
            Mediator.isPathSaved = false;
        }
    }
}