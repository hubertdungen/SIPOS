using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Word;
using Range = Microsoft.Office.Interop.Excel.Range;
using System.Collections.Generic;
using System;
//using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.IO;
using System.Text.RegularExpressions;
//using System.Reflection.Metadata.Ecma335;
//using static System.Net.Mime.MediaTypeNames;
using LinqList;
using static System.Net.WebRequestMethods;
using File = System.IO.File;
using System.Diagnostics;
using Image = System.Drawing.Image;
using Point = System.Drawing.Point;
using System.Runtime.InteropServices;
using System.Globalization;
using System.Drawing.Text;
//using Font = System.Drawing.Font;
using Application = System.Windows.Forms.Application;
using FontAwesome.Sharp;
using System.Windows.Forms.VisualStyles;
using ContentAlignment = System.Drawing.ContentAlignment;
using SIPOS.Forms;
using SIPOS;
using Rectangle = System.Drawing.Rectangle;

namespace SIPOS
{

    public partial class frm_OS_system : Form
    {


        ////////////////////////////////////////////////////// --------------------- //
        ////////////////////////////////////////////////////// --- INITIALIZERS ---- //
        ////////////////////////////////////////////////////// --------------------- //


        // VARS -------
        // ------------

        // Details VARS
        public static string version = "v A-0.10.7";


        
        private Mediator mediator;


        // -----------------------------
        // --------------------------------------------------------------------------


        ////////////////////////////////////////////////////// FORMS 
        // -------

        // Fields
        private IconButton currentBtn;
        private Panel leftBorderBtn;
        private Form currentChildForm;
        private int borderSize = 2;
        private Size formSize; //Keep form size when it is minimized and restored.Since the form is resized because it takes into account the size of the title bar and borders

        // Field to hold whether the mouse button is held down
        private bool mouseDown;
        // Field to hold where the mouse pointer was when the mouse button was pressed
        private Point lastLocation;






        // Constructor
        public frm_OS_system()
        {
            InitializeComponent();

            
            //Form
            this.Text = String.Empty;
            //this.ControlBox = false;
            this.DoubleBuffered = true;
            //this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
            //this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.Padding = new Padding(borderSize); //Border Size
            //this.BackColor = Color.FromArgb(1, 1, 1);


            // MEDIATOR INITIALIZATION
            mediator = new Mediator();
            Mediator.menu = this;
            mediator.PathErrorCheck += Mediator.pathErrorCheck;
            //mediator.UpdateFormMenuTextBoxHandler += updateFormMenuTextBox;

            // Assign event handlers for the mouse events
            this.MouseDown += frm_OS_system_MouseDown;
            this.MouseMove += frm_OS_system_MouseMove;
            this.MouseUp += frm_OS_system_MouseUp;



            // UI
            leftBorderBtn = new Panel();
            leftBorderBtn.Size = new Size(7, 60);
            panelMenu.Controls.Add(leftBorderBtn);

            // LOAD MEMORY
            mediator.loadMemory();
            

            // TXTBOXES UPDATE
            Mediator.txtboxsActualizer();
            
            // DATE PROCESSING
            Mediator.instAfterToday();
            //selectedDay = Convert.ToString(monthCalendar.SelectionStart);
            Mediator.instDateProcess(1);

            // START ENGINE IF POSSIBLE
            LinqList.ListaManagerEscalados.escaladosList.Clear();
            Mediator.instTriagemEscalas();
            Mediator.isPathSaved = true;


            // TEXTO INICIAL NO OUTPUT TEXT
            Mediator.instTxtBox_Equal_To(EscalasEngine.outputInitialText);
        }

        private const int cGrip = 16;      // Grip size
        private const int cCaption = 32;   // Caption bar height;




        private void frm_menu_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }
        // -----------------------------
        // --------------------------------------------------------------------------




        ////////////////////////////////////////////////////// BUTTONS 
        // -------

        ///// A SER ELIMINADO 
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




        // BUTTONS: SAVE FILES ///// A SER ELIMINADO 
        private void btn_SaveFileLocPD_Click(object sender, EventArgs e)
        {

        }


        // BUTTONS: EXPORT ///// A SER ELIMINADO 
        private void btn_ExportWord_Click(object sender, EventArgs e)
        {
        }
        private void btn_SaveFolder_OSWord_Click(object sender, EventArgs e)
        {
            //openFolder();
            //txtbox_FolderDirectory_OSWord.Text = selectedFolder;
        }


        // BUTTONS: OPEN FILE DIALOGS ///// A SER ELIMINADO 
        private void btn_searchFileODU_Click(object sender, EventArgs e)
        {
            //openFile();
            //txtbox_FileDirectoryODU.Text = filePath;
        }
        private void btn_searchFileCCS_Click(object sender, EventArgs e)
        {
            //openFile();
            //txtbox_FileDirectoryCCS.Text = filePath;
        }
        private void btn_searchFilePD_Click(object sender, EventArgs e)
        {
            //openFile();
            //txtbox_FileDirectoryPD.Text = filePath;
        }
        private void btn_searchFileSD_Click(object sender, EventArgs e)
        {
            //openFile();
            //txtbox_FileDirectorySD.Text = filePath;
        }
        private void btn_searchFileFunerais_Click(object sender, EventArgs e)
        {
            //openFile();
            //txtbox_FileDirectoryFunerais.Text = filePath;
        }
        private void btn_searchFile_ModelSemana_Click(object sender, EventArgs e)
        {
            //openFile();
            //txtbox_FileDirectory_ModelSemana.Text = filePath;
        }
        private void btn_searchFile_FDSemana_Click(object sender, EventArgs e)
        {
            //openFile();
            //txtbox_FileDirectory_ModelSemana.Text = filePath;
        }
        private void btn_searchFile_Quarta_Click(object sender, EventArgs e)
        {
            //openFile();
            //txtbox_FileDirectory_ModelQuarta.Text = filePath;
        }


        // BUTTONS: DEBUGGERS ///// A SER ELIMINADO 
        public void btn_VarTester_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Mediator.filePath);
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
            MessageBox.Show(Convert.ToString(Mediator.returnEscalaDate(0)));
        }
        private void btn_Export_TestListReader_Click(object sender, EventArgs e)
        {
            Word_Processor.listToVarsEscalados(0);
        }

        // -----------------------------
        // --------------------------------------------------------------------------
        // --------------------------------------------------------------------------







        ////////////////////////////////////////////////////// --------------------- // 
        ////////////////////////////////////////////////////// ----- UI CONTROL ---- //
        ////////////////////////////////////////////////////// --------------------- //

        // UI
        private void ActivateButton(object senderBtn, Color color)
        {
            if (senderBtn != null)
            {
                DisableButton();
                //Button
                currentBtn = (IconButton)senderBtn;
                currentBtn.BackColor = Color.FromArgb(79, 49, 79);
                currentBtn.ForeColor = color;
                currentBtn.TextAlign = ContentAlignment.MiddleCenter;
                currentBtn.IconColor = color;
                currentBtn.TextImageRelation = TextImageRelation.TextBeforeImage;
                currentBtn.ImageAlign = ContentAlignment.MiddleRight;
                //Left border button
                leftBorderBtn.BackColor = color;
                leftBorderBtn.Location = new Point(0, currentBtn.Location.Y);
                leftBorderBtn.Visible = true;
                leftBorderBtn.BringToFront();
                //Icon Current Child Form
                iconCurrentChildForm.IconChar = currentBtn.IconChar;
                iconCurrentChildForm.IconColor = color;
                
            }
        }
        private void DisableButton()
        {
            if (currentBtn != null)
            {
                currentBtn.BackColor = Color.FromArgb(57, 36, 57);
                currentBtn.ForeColor = Color.Gainsboro;
                currentBtn.TextAlign = ContentAlignment.MiddleLeft;
                currentBtn.IconColor = Color.Gainsboro;
                currentBtn.TextImageRelation = TextImageRelation.ImageBeforeText;
                currentBtn.ImageAlign = ContentAlignment.MiddleLeft;
            }
        }
        private struct RGBColors
        {
            public static Color color1 = Color.FromArgb(4, 230, 255);
            public static Color color2 = Color.FromArgb(253, 138, 114);
            public static Color color3 = Color.FromArgb(141, 238, 117);
            public static Color color4 = Color.FromArgb(24, 161, 251);
            public static Color color5 = Color.FromArgb(255, 0, 249);
            public static Color color6 = Color.FromArgb(238, 224, 130);
        }                       ///----------------------------------/// COLOR CONTROL

        // UI Buttons
        private void btn_Home_Click(object sender, EventArgs e)
        {
            if (currentChildForm != null)
            {
                currentChildForm.Close();
                Reset();
            }
        }
        private void panelLogo_Click(object sender, EventArgs e)
        {
            if (currentChildForm != null)
            {
                currentChildForm.Close();
                Reset();
            }
        }
        private void btn_DadosIni_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.color1);
            OpenChildForm(new FormDados());

        }
        private void btn_Inputs_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.color2);
            OpenChildForm(new FormInfo());
        }
        private void btn_Export_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.color3);
            OpenChildForm(new FormExport());
        }
        private void btn_Modelar_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.color4);
            OpenChildForm(new FormModelar());
        }
        private void btn_Settings_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.color5);
            OpenChildForm(new FormPropriedades());
        }
        private void btn_Help_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.color6);
            OpenChildForm(new FormHelp());
        }
        private void Reset()
        {
            DisableButton();
            leftBorderBtn.Visible = false;
            iconCurrentChildForm.IconChar = IconChar.PlaneDeparture;
            iconCurrentChildForm.IconColor = Color.FromArgb(255, 0, 249);
            lblTitleChildForm.Text = "Início";
        }


        // UI Window Control
        private void panel_Designio_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
        private void pBox_Designio_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
        private void btn_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btn_maximize_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
            }
        }
        private void panelLogo_DoubleClick(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
            }
        }
        private void pBox_Designio_DoubleClick(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
            }
        }
        private void iconCurrentChildForm_DoubleClick(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
            }
        }
        private void btn_minmize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        private void panel_TitleBar_DoubleClick(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
            }
        }
        private void frm_OS_system_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
        }
        private void frm_OS_system_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                this.Location = new Point(
                    (this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);

                this.Update();

                if (this.Location.Y <= 5 && this.WindowState != FormWindowState.Maximized)
                {
                    this.WindowState = FormWindowState.Maximized;
                    mouseDown = false;
                }
            }
        }
        private void frm_OS_system_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }




        // DRAG FORM
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);
        private void panel_TitleBar_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }


        //Overridden methods
        protected override void WndProc(ref Message m)
        {
            const int WM_NCCALCSIZE = 0x0083;//Standar Title Bar - Snap Window
            const int WM_SYSCOMMAND = 0x0112;
            const int SC_MINIMIZE = 0xF020; //Minimize form (Before)
            const int SC_RESTORE = 0xF120; //Restore form (Before)
            const int WM_NCHITTEST = 0x0084;//Win32, Mouse Input Notification: Determine what part of the window corresponds to a point, allows to resize the form.
            const int resizeAreaSize = 10;
            #region Form Resize
            // Resize/WM_NCHITTEST values
            const int HTCLIENT = 1; //Represents the client area of the window
            const int HTLEFT = 10;  //Left border of a window, allows resize horizontally to the left
            const int HTRIGHT = 11; //Right border of a window, allows resize horizontally to the right
            const int HTTOP = 12;   //Upper-horizontal border of a window, allows resize vertically up
            const int HTTOPLEFT = 13;//Upper-left corner of a window border, allows resize diagonally to the left
            const int HTTOPRIGHT = 14;//Upper-right corner of a window border, allows resize diagonally to the right
            const int HTBOTTOM = 15; //Lower-horizontal border of a window, allows resize vertically down
            const int HTBOTTOMLEFT = 16;//Lower-left corner of a window border, allows resize diagonally to the left
            const int HTBOTTOMRIGHT = 17;//Lower-right corner of a window border, allows resize diagonally to the right
            ///<Doc> More Information: https://docs.microsoft.com/en-us/windows/win32/inputdev/wm-nchittest </Doc>
            if (m.Msg == WM_NCHITTEST)
            { //If the windows m is WM_NCHITTEST
                base.WndProc(ref m);
                if (this.WindowState == FormWindowState.Normal)//Resize the form if it is in normal state
                {
                    if ((int)m.Result == HTCLIENT)//If the result of the m (mouse pointer) is in the client area of the window
                    {
                        Point screenPoint = new Point(m.LParam.ToInt32()); //Gets screen point coordinates(X and Y coordinate of the pointer)                           
                        Point clientPoint = this.PointToClient(screenPoint); //Computes the location of the screen point into client coordinates                          
                        if (clientPoint.Y <= resizeAreaSize)//If the pointer is at the top of the form (within the resize area- X coordinate)
                        {
                            if (clientPoint.X <= resizeAreaSize) //If the pointer is at the coordinate X=0 or less than the resizing area(X=10) in 
                                m.Result = (IntPtr)HTTOPLEFT; //Resize diagonally to the left
                            else if (clientPoint.X < (this.Size.Width - resizeAreaSize))//If the pointer is at the coordinate X=11 or less than the width of the form(X=Form.Width-resizeArea)
                                m.Result = (IntPtr)HTTOP; //Resize vertically up
                            else //Resize diagonally to the right
                                m.Result = (IntPtr)HTTOPRIGHT;
                        }
                        else if (clientPoint.Y <= (this.Size.Height - resizeAreaSize)) //If the pointer is inside the form at the Y coordinate(discounting the resize area size)
                        {
                            if (clientPoint.X <= resizeAreaSize)//Resize horizontally to the left
                                m.Result = (IntPtr)HTLEFT;
                            else if (clientPoint.X > (this.Width - resizeAreaSize))//Resize horizontally to the right
                                m.Result = (IntPtr)HTRIGHT;
                        }
                        else
                        {
                            if (clientPoint.X <= resizeAreaSize)//Resize diagonally to the left
                                m.Result = (IntPtr)HTBOTTOMLEFT;
                            else if (clientPoint.X < (this.Size.Width - resizeAreaSize)) //Resize vertically down
                                m.Result = (IntPtr)HTBOTTOM;
                            else //Resize diagonally to the right
                                m.Result = (IntPtr)HTBOTTOMRIGHT;
                        }
                    }
                }
                return;
            }
            #endregion
            //Remove border and keep snap window
            if (m.Msg == WM_NCCALCSIZE && m.WParam.ToInt32() == 1)
            {
                return;
            }
            //Keep form size when it is minimized and restored. Since the form is resized because it takes into account the size of the title bar and borders.
            if (m.Msg == WM_SYSCOMMAND)
            {
                /// <see cref="https://docs.microsoft.com/en-us/windows/win32/menurc/wm-syscommand"/>
                /// Quote:
                /// In WM_SYSCOMMAND messages, the four low - order bits of the wParam parameter 
                /// are used internally by the system.To obtain the correct result when testing 
                /// the value of wParam, an application must combine the value 0xFFF0 with the 
                /// wParam value by using the bitwise AND operator.
                int wParam = (m.WParam.ToInt32() & 0xFFF0);
                if (wParam == SC_MINIMIZE)  //Before
                    formSize = this.ClientSize;
                if (wParam == SC_RESTORE)// Restored form(Before)
                    this.Size = formSize;
            }
            base.WndProc(ref m);
        }


        // FORM CONTROL
        private void OpenChildForm(Form childForm)
        {
            if (currentChildForm != null)
            {
                //open only form
                currentChildForm.Close();
            }
            currentChildForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            panelDesktop.Controls.Add(childForm);
            panelDesktop.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
            lblTitleChildForm.Text = childForm.Text;
        }



        

        private void makeUiTransparent()
        {
            lbl_Gravar.BackColor = System.Drawing.Color.Transparent;
        }


        // TEXTBOXES ACTUALIZER
        public static void txtboxsActualizer()
        {
            frm_OS_system formMenu = Mediator.menu;
            formMenu.lbl_version.Text = version;
        }




        // -----------------------------
        // --------------------------------------------------------------------------
        // --------------------------------------------------------------------------














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
        private void txtBox_NumOS_TextChanged(object sender, EventArgs e)
        {
            //txtBox_ExportDocName.Text = DateTime.Now.Year.ToString() + "-" + "002" + "-" + txtBox_NumOS.Text;
            //Mediator.osNumber = txtBox_NumOS.Text;
        }

        private void basic_ToolTip_Popup(object sender, PopupEventArgs e)
        {

        }

        private void frm_OS_system_Resize(object sender, EventArgs e)
        {
            AdjustForm();

        }

        private void AdjustForm()
        {
            switch(this.WindowState)
            {
                case FormWindowState.Maximized:
                    this.Padding = new Padding(0, 8, 8, 0);
                    break;
                case FormWindowState.Normal:
                    if (this.Padding.Top!=borderSize)
                    {
                        this.Padding = new Padding(borderSize);
                    }
                    break;
            }
        }

        private void frm_OS_system_Load(object sender, EventArgs e)
        {
            formSize = this.ClientSize;
        }
    }





















    // -----------------------------
    // --------------------------------------------------------------------------
    // --------------------------------------------------------------------------


}





// --------------------- // 
// ------ BACKUPS ------ //
// --------------------- //

// -----------------------------






