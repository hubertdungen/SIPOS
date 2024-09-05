namespace SIPOS.Forms
{
    partial class FormModelar
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            rowPanel_WordDoc = new Panel();
            btnWMinus = new Button();
            btnWPlus = new Button();
            btnOpenWFile = new Button();
            txtDirFicheiroW = new TextBox();
            pnlTextNameW = new Panel();
            lblWSeparador = new Label();
            btnChkWRowActive = new Button();
            txtNameWBox = new TextBox();
            lbl_ellipse = new Label();
            frmModelarTimer = new System.Windows.Forms.Timer(components);
            mainWordFlowPanel = new FlowLayoutPanel();
            pnl_Separator = new Panel();
            panelMenu = new Panel();
            btnProgramas = new Button();
            btnFicheiros = new Button();
            panelWordMenuSelector = new Panel();
            richtxtBox_ProgramaHints = new RichTextBox();
            btnAddtoList = new Button();
            cmbBoxTemplateName = new ComboBox();
            toolTip1 = new ToolTip(components);
            pnl_spacer = new Panel();
            panel1 = new Panel();
            rowPanel_WordDoc.SuspendLayout();
            pnlTextNameW.SuspendLayout();
            mainWordFlowPanel.SuspendLayout();
            panelMenu.SuspendLayout();
            panelWordMenuSelector.SuspendLayout();
            pnl_spacer.SuspendLayout();
            SuspendLayout();
            // 
            // rowPanel_WordDoc
            // 
            rowPanel_WordDoc.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            rowPanel_WordDoc.AutoSize = true;
            rowPanel_WordDoc.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            rowPanel_WordDoc.BackColor = Color.FromArgb(40, 30, 40);
            rowPanel_WordDoc.Controls.Add(btnWMinus);
            rowPanel_WordDoc.Controls.Add(btnWPlus);
            rowPanel_WordDoc.Controls.Add(btnOpenWFile);
            rowPanel_WordDoc.Controls.Add(txtDirFicheiroW);
            rowPanel_WordDoc.Controls.Add(pnlTextNameW);
            rowPanel_WordDoc.Controls.Add(lbl_ellipse);
            rowPanel_WordDoc.Location = new Point(20, 18);
            rowPanel_WordDoc.Margin = new Padding(20, 13, 20, 13);
            rowPanel_WordDoc.MaximumSize = new Size(800, 50);
            rowPanel_WordDoc.MinimumSize = new Size(0, 40);
            rowPanel_WordDoc.Name = "rowPanel_WordDoc";
            rowPanel_WordDoc.Size = new Size(800, 50);
            rowPanel_WordDoc.TabIndex = 0;
            // 
            // btnWMinus
            // 
            btnWMinus.BackColor = Color.FromArgb(40, 30, 40);
            btnWMinus.Dock = DockStyle.Right;
            btnWMinus.FlatStyle = FlatStyle.Flat;
            btnWMinus.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            btnWMinus.ForeColor = Color.OrangeRed;
            btnWMinus.Location = new Point(738, 0);
            btnWMinus.Margin = new Padding(3, 5, 3, 3);
            btnWMinus.Name = "btnWMinus";
            btnWMinus.Size = new Size(31, 50);
            btnWMinus.TabIndex = 8;
            btnWMinus.Text = "➖";
            btnWMinus.UseVisualStyleBackColor = false;
            // 
            // btnWPlus
            // 
            btnWPlus.BackColor = Color.FromArgb(40, 30, 40);
            btnWPlus.Dock = DockStyle.Right;
            btnWPlus.FlatStyle = FlatStyle.Flat;
            btnWPlus.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            btnWPlus.ForeColor = Color.MediumSpringGreen;
            btnWPlus.Location = new Point(769, 0);
            btnWPlus.Margin = new Padding(3, 5, 3, 3);
            btnWPlus.Name = "btnWPlus";
            btnWPlus.Size = new Size(31, 50);
            btnWPlus.TabIndex = 7;
            btnWPlus.Text = "➕";
            btnWPlus.UseVisualStyleBackColor = false;
            // 
            // btnOpenWFile
            // 
            btnOpenWFile.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            btnOpenWFile.BackColor = Color.FromArgb(40, 30, 40);
            btnOpenWFile.FlatStyle = FlatStyle.Flat;
            btnOpenWFile.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            btnOpenWFile.ForeColor = Color.PaleVioletRed;
            btnOpenWFile.Location = new Point(690, 0);
            btnOpenWFile.Margin = new Padding(0, 0, 5, 0);
            btnOpenWFile.Name = "btnOpenWFile";
            btnOpenWFile.Size = new Size(31, 50);
            btnOpenWFile.TabIndex = 6;
            btnOpenWFile.Text = "📄";
            btnOpenWFile.UseVisualStyleBackColor = false;
            // 
            // txtDirFicheiroW
            // 
            txtDirFicheiroW.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtDirFicheiroW.BackColor = Color.FromArgb(40, 30, 40);
            txtDirFicheiroW.BorderStyle = BorderStyle.None;
            txtDirFicheiroW.CharacterCasing = CharacterCasing.Upper;
            txtDirFicheiroW.Font = new Font("Century Gothic", 12F, FontStyle.Regular, GraphicsUnit.Point);
            txtDirFicheiroW.ForeColor = Color.PaleVioletRed;
            txtDirFicheiroW.Location = new Point(359, 13);
            txtDirFicheiroW.Margin = new Padding(10);
            txtDirFicheiroW.MaxLength = 10000;
            txtDirFicheiroW.Name = "txtDirFicheiroW";
            txtDirFicheiroW.PlaceholderText = "Caminho para o ficheiro word";
            txtDirFicheiroW.RightToLeft = RightToLeft.Yes;
            txtDirFicheiroW.Size = new Size(308, 20);
            txtDirFicheiroW.TabIndex = 5;
            txtDirFicheiroW.TextAlign = HorizontalAlignment.Right;
            // 
            // pnlTextNameW
            // 
            pnlTextNameW.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            pnlTextNameW.Controls.Add(lblWSeparador);
            pnlTextNameW.Controls.Add(btnChkWRowActive);
            pnlTextNameW.Controls.Add(txtNameWBox);
            pnlTextNameW.ForeColor = SystemColors.ActiveCaption;
            pnlTextNameW.Location = new Point(51, 0);
            pnlTextNameW.MaximumSize = new Size(300, 0);
            pnlTextNameW.Name = "pnlTextNameW";
            pnlTextNameW.Size = new Size(278, 50);
            pnlTextNameW.TabIndex = 3;
            // 
            // lblWSeparador
            // 
            lblWSeparador.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lblWSeparador.AutoSize = true;
            lblWSeparador.Font = new Font("Century Gothic", 20.25F, FontStyle.Bold, GraphicsUnit.Point);
            lblWSeparador.ForeColor = Color.PaleVioletRed;
            lblWSeparador.Location = new Point(258, 6);
            lblWSeparador.Name = "lblWSeparador";
            lblWSeparador.Size = new Size(30, 32);
            lblWSeparador.TabIndex = 9;
            lblWSeparador.Text = "|";
            // 
            // btnChkWRowActive
            // 
            btnChkWRowActive.Dock = DockStyle.Left;
            btnChkWRowActive.FlatStyle = FlatStyle.Flat;
            btnChkWRowActive.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnChkWRowActive.ForeColor = Color.PaleVioletRed;
            btnChkWRowActive.Location = new Point(0, 0);
            btnChkWRowActive.Margin = new Padding(10, 3, 3, 3);
            btnChkWRowActive.Name = "btnChkWRowActive";
            btnChkWRowActive.Size = new Size(22, 50);
            btnChkWRowActive.TabIndex = 5;
            btnChkWRowActive.Text = "✓";
            btnChkWRowActive.UseVisualStyleBackColor = true;
            // 
            // txtNameWBox
            // 
            txtNameWBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtNameWBox.BackColor = Color.FromArgb(40, 30, 40);
            txtNameWBox.BorderStyle = BorderStyle.None;
            txtNameWBox.CharacterCasing = CharacterCasing.Upper;
            txtNameWBox.Font = new Font("Century Gothic", 12F, FontStyle.Regular, GraphicsUnit.Point);
            txtNameWBox.ForeColor = Color.AliceBlue;
            txtNameWBox.Location = new Point(48, 13);
            txtNameWBox.Margin = new Padding(10);
            txtNameWBox.MaxLength = 23;
            txtNameWBox.Name = "txtNameWBox";
            txtNameWBox.PlaceholderText = "Nome";
            txtNameWBox.Size = new Size(204, 20);
            txtNameWBox.TabIndex = 4;
            // 
            // lbl_ellipse
            // 
            lbl_ellipse.Cursor = Cursors.SizeAll;
            lbl_ellipse.Dock = DockStyle.Left;
            lbl_ellipse.Font = new Font("Arial Black", 28F, FontStyle.Bold, GraphicsUnit.Point);
            lbl_ellipse.ForeColor = Color.PaleVioletRed;
            lbl_ellipse.Location = new Point(0, 0);
            lbl_ellipse.Margin = new Padding(0, 0, 10, 0);
            lbl_ellipse.Name = "lbl_ellipse";
            lbl_ellipse.Size = new Size(51, 50);
            lbl_ellipse.TabIndex = 2;
            lbl_ellipse.Text = "⋯";
            lbl_ellipse.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // frmModelarTimer
            // 
            frmModelarTimer.Enabled = true;
            frmModelarTimer.Interval = 20;
            // 
            // mainWordFlowPanel
            // 
            mainWordFlowPanel.AutoScroll = true;
            mainWordFlowPanel.AutoScrollMargin = new Size(100, 0);
            mainWordFlowPanel.AutoScrollMinSize = new Size(100, 0);
            mainWordFlowPanel.AutoSize = true;
            mainWordFlowPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            mainWordFlowPanel.BackColor = Color.FromArgb(15, 0, 15);
            mainWordFlowPanel.Controls.Add(rowPanel_WordDoc);
            mainWordFlowPanel.Dock = DockStyle.Bottom;
            mainWordFlowPanel.FlowDirection = FlowDirection.TopDown;
            mainWordFlowPanel.Location = new Point(0, 284);
            mainWordFlowPanel.Margin = new Padding(3, 5, 3, 5);
            mainWordFlowPanel.MinimumSize = new Size(0, 200);
            mainWordFlowPanel.Name = "mainWordFlowPanel";
            mainWordFlowPanel.Padding = new Padding(0, 5, 0, 5);
            mainWordFlowPanel.Size = new Size(853, 200);
            mainWordFlowPanel.TabIndex = 3;
            mainWordFlowPanel.WrapContents = false;
            // 
            // pnl_Separator
            // 
            pnl_Separator.BackColor = Color.PaleVioletRed;
            pnl_Separator.Enabled = false;
            pnl_Separator.ForeColor = Color.PaleVioletRed;
            pnl_Separator.Location = new Point(100, 271);
            pnl_Separator.Name = "pnl_Separator";
            pnl_Separator.Size = new Size(686, 4);
            pnl_Separator.TabIndex = 1;
            pnl_Separator.Visible = false;
            // 
            // panelMenu
            // 
            panelMenu.BackColor = Color.FromArgb(57, 36, 57);
            panelMenu.Controls.Add(btnProgramas);
            panelMenu.Controls.Add(btnFicheiros);
            panelMenu.Dock = DockStyle.Top;
            panelMenu.Location = new Point(0, 0);
            panelMenu.Name = "panelMenu";
            panelMenu.Size = new Size(853, 66);
            panelMenu.TabIndex = 4;
            // 
            // btnProgramas
            // 
            btnProgramas.BackColor = Color.FromArgb(40, 30, 40);
            btnProgramas.Dock = DockStyle.Left;
            btnProgramas.FlatStyle = FlatStyle.Flat;
            btnProgramas.Font = new Font("Century Gothic", 12F, FontStyle.Italic, GraphicsUnit.Point);
            btnProgramas.ForeColor = Color.DeepSkyBlue;
            btnProgramas.Location = new Point(0, 0);
            btnProgramas.Margin = new Padding(3, 5, 3, 3);
            btnProgramas.Name = "btnProgramas";
            btnProgramas.Size = new Size(163, 66);
            btnProgramas.TabIndex = 8;
            btnProgramas.Text = "PROGRAMAS";
            toolTip1.SetToolTip(btnProgramas, "Menu de criação de programas a serem executados pelo SIPOS personalizados.");
            btnProgramas.UseVisualStyleBackColor = false;
            btnProgramas.Click += btnProgramas_Click;
            // 
            // btnFicheiros
            // 
            btnFicheiros.BackColor = Color.FromArgb(40, 30, 40);
            btnFicheiros.Dock = DockStyle.Right;
            btnFicheiros.FlatStyle = FlatStyle.Flat;
            btnFicheiros.Font = new Font("Century Gothic", 12F, FontStyle.Italic, GraphicsUnit.Point);
            btnFicheiros.ForeColor = Color.Aqua;
            btnFicheiros.Location = new Point(690, 0);
            btnFicheiros.Margin = new Padding(3, 5, 3, 3);
            btnFicheiros.Name = "btnFicheiros";
            btnFicheiros.Size = new Size(163, 66);
            btnFicheiros.TabIndex = 9;
            btnFicheiros.Text = "FICHEIROS";
            toolTip1.SetToolTip(btnFicheiros, "Ordem de ficheiros que irão ter alguma ação segundo os programas.");
            btnFicheiros.UseVisualStyleBackColor = false;
            btnFicheiros.Click += btnFicheiros_Click;
            // 
            // panelWordMenuSelector
            // 
            panelWordMenuSelector.BackColor = Color.FromArgb(27, 20, 25);
            panelWordMenuSelector.Controls.Add(richtxtBox_ProgramaHints);
            panelWordMenuSelector.Controls.Add(btnAddtoList);
            panelWordMenuSelector.Controls.Add(cmbBoxTemplateName);
            panelWordMenuSelector.Dock = DockStyle.Top;
            panelWordMenuSelector.Location = new Point(0, 66);
            panelWordMenuSelector.Name = "panelWordMenuSelector";
            panelWordMenuSelector.Size = new Size(853, 78);
            panelWordMenuSelector.TabIndex = 5;
            // 
            // richtxtBox_ProgramaHints
            // 
            richtxtBox_ProgramaHints.BackColor = Color.FromArgb(27, 20, 25);
            richtxtBox_ProgramaHints.BorderStyle = BorderStyle.None;
            richtxtBox_ProgramaHints.Font = new Font("Century Gothic", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            richtxtBox_ProgramaHints.ForeColor = Color.PaleVioletRed;
            richtxtBox_ProgramaHints.Location = new Point(20, 12);
            richtxtBox_ProgramaHints.Name = "richtxtBox_ProgramaHints";
            richtxtBox_ProgramaHints.ScrollBars = RichTextBoxScrollBars.None;
            richtxtBox_ProgramaHints.Size = new Size(339, 58);
            richtxtBox_ProgramaHints.TabIndex = 0;
            richtxtBox_ProgramaHints.Text = "Aqui encontra várias dicas sobre as partes da costumização de programas.";
            // 
            // btnAddtoList
            // 
            btnAddtoList.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnAddtoList.BackColor = Color.FromArgb(40, 30, 40);
            btnAddtoList.FlatStyle = FlatStyle.Flat;
            btnAddtoList.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            btnAddtoList.ForeColor = Color.MediumSpringGreen;
            btnAddtoList.Location = new Point(769, 23);
            btnAddtoList.Margin = new Padding(3, 5, 3, 3);
            btnAddtoList.Name = "btnAddtoList";
            btnAddtoList.Size = new Size(69, 32);
            btnAddtoList.TabIndex = 10;
            btnAddtoList.Text = "➕";
            toolTip1.SetToolTip(btnAddtoList, "Adicionar lista.");
            btnAddtoList.UseVisualStyleBackColor = false;
            // 
            // cmbBoxTemplateName
            // 
            cmbBoxTemplateName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            cmbBoxTemplateName.BackColor = Color.FromArgb(15, 0, 15);
            cmbBoxTemplateName.FlatStyle = FlatStyle.Flat;
            cmbBoxTemplateName.Font = new Font("Century Gothic", 15.75F, FontStyle.Regular, GraphicsUnit.Point);
            cmbBoxTemplateName.ForeColor = Color.PaleVioletRed;
            cmbBoxTemplateName.FormattingEnabled = true;
            cmbBoxTemplateName.Location = new Point(379, 23);
            cmbBoxTemplateName.Name = "cmbBoxTemplateName";
            cmbBoxTemplateName.Size = new Size(374, 32);
            cmbBoxTemplateName.TabIndex = 0;
            toolTip1.SetToolTip(cmbBoxTemplateName, "Seleccionar lista ou inserir nome da lista.");
            // 
            // pnl_spacer
            // 
            pnl_spacer.BackColor = Color.FromArgb(15, 0, 15);
            pnl_spacer.Controls.Add(panel1);
            pnl_spacer.Dock = DockStyle.Fill;
            pnl_spacer.Location = new Point(0, 144);
            pnl_spacer.Name = "pnl_spacer";
            pnl_spacer.Size = new Size(853, 140);
            pnl_spacer.TabIndex = 6;
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(15, 0, 15);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(853, 50);
            panel1.TabIndex = 7;
            // 
            // FormModelar
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(27, 20, 25);
            ClientSize = new Size(853, 484);
            Controls.Add(pnl_spacer);
            Controls.Add(panelWordMenuSelector);
            Controls.Add(panelMenu);
            Controls.Add(mainWordFlowPanel);
            Controls.Add(pnl_Separator);
            Name = "FormModelar";
            Text = "FormModelar";
            Load += FormModelar_Load;
            rowPanel_WordDoc.ResumeLayout(false);
            rowPanel_WordDoc.PerformLayout();
            pnlTextNameW.ResumeLayout(false);
            pnlTextNameW.PerformLayout();
            mainWordFlowPanel.ResumeLayout(false);
            mainWordFlowPanel.PerformLayout();
            panelMenu.ResumeLayout(false);
            panelWordMenuSelector.ResumeLayout(false);
            pnl_spacer.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.Timer frmModelarTimer;
        private Panel rowPanel_WordDoc;
        private TextBox txtNameWBox;
        private Label lbl_ellipse;
        private FlowLayoutPanel mainWordFlowPanel;
        private Panel pnlTextNameW;
        private TextBox txtDirFicheiroW;
        private Button btnOpenWFile;
        private Button btnWMinus;
        private Button btnWPlus;
        private Label lblWSeparador;
        private Panel pnl_Separator;
        private Panel panelMenu;
        private Panel panelWordMenuSelector;
        private Button btnChkWRowActive;
        private ComboBox cmbBoxTemplateName;
        private Button btnAddtoList;
        private Button btnFicheiros;
        private Button btnProgramas;
        private ToolTip toolTip1;
        private Panel pnl_spacer;
        private Panel panel1;
        private RichTextBox richtxtBox_ProgramaHints;
    }
}