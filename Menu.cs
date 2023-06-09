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
        public static string version = "v A-0.10.6";



        
        private Mediator mediator;


        // -----------------------------
        // --------------------------------------------------------------------------


        ////////////////////////////////////////////////////// FORMS 
        // -------

        // Fields
        private IconButton currentBtn;
        private Panel leftBorderBtn;
        private Form currentChildForm;

        // Field to hold whether the mouse button is held down
        private bool mouseDown;
        // Field to hold where the mouse pointer was when the mouse button was pressed
        private Point lastLocation;






        // Constructor
        public frm_OS_system()
        {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.None;
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.ResizeRedraw, true);


            // MEDIATOR INITIALIZATION
            mediator = new Mediator();
            Mediator.menu = this;
            mediator.PathErrorCheck += Mediator.pathErrorCheck;
            //mediator.UpdateFormMenuTextBoxHandler += updateFormMenuTextBox;

            // Assign event handlers for the mouse events
            this.MouseDown += frm_OS_system_MouseDown;
            this.MouseMove += frm_OS_system_MouseMove;
            this.MouseUp += frm_OS_system_MouseUp;


            // Form is Double Buffered // It prevents the flickering of the form
            this.DoubleBuffered = true;

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
                MessageBox.Show("N�o � poss�vel abrir a pasta onde o ficheiro \\settings.txt\\ se encontra.\r\n� poss�vel que a pasta esteja protegida ou que o programa tenha guardado mal o ficheiro.", "ERRO AO ABRIR PASTA!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
            lblTitleChildForm.Text = "In�cio";
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




        // UMA TENTATIVA DE CRIAR UMA RESIZABLE WINDOW
        protected override void OnPaint(PaintEventArgs e) // you can safely omit this method if you want
        {
            e.Graphics.FillRectangle(Brushes.Green, Top);
            e.Graphics.FillRectangle(Brushes.Green, Left);
            e.Graphics.FillRectangle(Brushes.Green, Right);
            e.Graphics.FillRectangle(Brushes.Green, Bottom);
        }

        private const int
            HTLEFT = 10,
            HTRIGHT = 11,
            HTTOP = 12,
            HTTOPLEFT = 13,
            HTTOPRIGHT = 14,
            HTBOTTOM = 15,
            HTBOTTOMLEFT = 16,
            HTBOTTOMRIGHT = 17;

        const int _ = 10; // you can rename this variable if you like

        Rectangle Top { get { return new Rectangle(0, 0, this.ClientSize.Width, _); } }
        Rectangle Left { get { return new Rectangle(0, 0, _, this.ClientSize.Height); } }
        Rectangle Bottom { get { return new Rectangle(0, this.ClientSize.Height - _, this.ClientSize.Width, _); } }
        Rectangle Right { get { return new Rectangle(this.ClientSize.Width - _, 0, _, this.ClientSize.Height); } }

        Rectangle TopLeft { get { return new Rectangle(0, 0, _, _); } }
        Rectangle TopRight { get { return new Rectangle(this.ClientSize.Width - _, 0, _, _); } }
        Rectangle BottomLeft { get { return new Rectangle(0, this.ClientSize.Height - _, _, _); } }
        Rectangle BottomRight { get { return new Rectangle(this.ClientSize.Width - _, this.ClientSize.Height - _, _, _); } }


        protected override void WndProc(ref Message message)
        {
            base.WndProc(ref message);

            if (message.Msg == 0x84) // WM_NCHITTEST
            {
                var cursor = this.PointToClient(Cursor.Position);

                if (TopLeft.Contains(cursor)) message.Result = (IntPtr)HTTOPLEFT;
                else if (TopRight.Contains(cursor)) message.Result = (IntPtr)HTTOPRIGHT;
                else if (BottomLeft.Contains(cursor)) message.Result = (IntPtr)HTBOTTOMLEFT;
                else if (BottomRight.Contains(cursor)) message.Result = (IntPtr)HTBOTTOMRIGHT;

                else if (Top.Contains(cursor)) message.Result = (IntPtr)HTTOP;
                else if (Left.Contains(cursor)) message.Result = (IntPtr)HTLEFT;
                else if (Right.Contains(cursor)) message.Result = (IntPtr)HTRIGHT;
                else if (Bottom.Contains(cursor)) message.Result = (IntPtr)HTBOTTOM;
            }
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






