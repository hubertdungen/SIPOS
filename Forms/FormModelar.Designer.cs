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
            this.components = new System.ComponentModel.Container();
            this.rowPanel_WordDoc = new System.Windows.Forms.Panel();
            this.btnWMinus = new System.Windows.Forms.Button();
            this.btnWPlus = new System.Windows.Forms.Button();
            this.btnOpenWFile = new System.Windows.Forms.Button();
            this.txtDirFicheiroW = new System.Windows.Forms.TextBox();
            this.pnlTextNameW = new System.Windows.Forms.Panel();
            this.lblWSeparador = new System.Windows.Forms.Label();
            this.btnChkWRowActive = new System.Windows.Forms.Button();
            this.txtNameWBox = new System.Windows.Forms.TextBox();
            this.lbl_ellipse = new System.Windows.Forms.Label();
            this.frmModelarTimer = new System.Windows.Forms.Timer(this.components);
            this.mainWordFlowPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.pnl_Separator = new System.Windows.Forms.Panel();
            this.panelMenu = new System.Windows.Forms.Panel();
            this.panelWordMenuSelector = new System.Windows.Forms.Panel();
            this.cmbBoxTemplateName = new System.Windows.Forms.ComboBox();
            this.rowPanel_WordDoc.SuspendLayout();
            this.pnlTextNameW.SuspendLayout();
            this.mainWordFlowPanel.SuspendLayout();
            this.panelWordMenuSelector.SuspendLayout();
            this.SuspendLayout();
            // 
            // rowPanel_WordDoc
            // 
            this.rowPanel_WordDoc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rowPanel_WordDoc.AutoSize = true;
            this.rowPanel_WordDoc.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.rowPanel_WordDoc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(30)))), ((int)(((byte)(40)))));
            this.rowPanel_WordDoc.Controls.Add(this.btnWMinus);
            this.rowPanel_WordDoc.Controls.Add(this.btnWPlus);
            this.rowPanel_WordDoc.Controls.Add(this.btnOpenWFile);
            this.rowPanel_WordDoc.Controls.Add(this.txtDirFicheiroW);
            this.rowPanel_WordDoc.Controls.Add(this.pnlTextNameW);
            this.rowPanel_WordDoc.Controls.Add(this.lbl_ellipse);
            this.rowPanel_WordDoc.Location = new System.Drawing.Point(20, 18);
            this.rowPanel_WordDoc.Margin = new System.Windows.Forms.Padding(20, 13, 20, 13);
            this.rowPanel_WordDoc.MaximumSize = new System.Drawing.Size(800, 50);
            this.rowPanel_WordDoc.MinimumSize = new System.Drawing.Size(0, 40);
            this.rowPanel_WordDoc.Name = "rowPanel_WordDoc";
            this.rowPanel_WordDoc.Size = new System.Drawing.Size(800, 50);
            this.rowPanel_WordDoc.TabIndex = 0;
            // 
            // btnWMinus
            // 
            this.btnWMinus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(30)))), ((int)(((byte)(40)))));
            this.btnWMinus.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnWMinus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnWMinus.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnWMinus.ForeColor = System.Drawing.Color.OrangeRed;
            this.btnWMinus.Location = new System.Drawing.Point(738, 0);
            this.btnWMinus.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.btnWMinus.Name = "btnWMinus";
            this.btnWMinus.Size = new System.Drawing.Size(31, 50);
            this.btnWMinus.TabIndex = 8;
            this.btnWMinus.Text = "➖";
            this.btnWMinus.UseVisualStyleBackColor = false;
            // 
            // btnWPlus
            // 
            this.btnWPlus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(30)))), ((int)(((byte)(40)))));
            this.btnWPlus.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnWPlus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnWPlus.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnWPlus.ForeColor = System.Drawing.Color.MediumSpringGreen;
            this.btnWPlus.Location = new System.Drawing.Point(769, 0);
            this.btnWPlus.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.btnWPlus.Name = "btnWPlus";
            this.btnWPlus.Size = new System.Drawing.Size(31, 50);
            this.btnWPlus.TabIndex = 7;
            this.btnWPlus.Text = "➕";
            this.btnWPlus.UseVisualStyleBackColor = false;
            // 
            // btnOpenWFile
            // 
            this.btnOpenWFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpenWFile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(30)))), ((int)(((byte)(40)))));
            this.btnOpenWFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenWFile.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnOpenWFile.ForeColor = System.Drawing.Color.PaleVioletRed;
            this.btnOpenWFile.Location = new System.Drawing.Point(690, 0);
            this.btnOpenWFile.Margin = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.btnOpenWFile.Name = "btnOpenWFile";
            this.btnOpenWFile.Size = new System.Drawing.Size(31, 50);
            this.btnOpenWFile.TabIndex = 6;
            this.btnOpenWFile.Text = "📄";
            this.btnOpenWFile.UseVisualStyleBackColor = false;
            // 
            // txtDirFicheiroW
            // 
            this.txtDirFicheiroW.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDirFicheiroW.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(30)))), ((int)(((byte)(40)))));
            this.txtDirFicheiroW.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDirFicheiroW.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtDirFicheiroW.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtDirFicheiroW.ForeColor = System.Drawing.Color.PaleVioletRed;
            this.txtDirFicheiroW.Location = new System.Drawing.Point(359, 13);
            this.txtDirFicheiroW.Margin = new System.Windows.Forms.Padding(10);
            this.txtDirFicheiroW.MaxLength = 10000;
            this.txtDirFicheiroW.Name = "txtDirFicheiroW";
            this.txtDirFicheiroW.PlaceholderText = "Caminho para o ficheiro word";
            this.txtDirFicheiroW.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txtDirFicheiroW.Size = new System.Drawing.Size(308, 20);
            this.txtDirFicheiroW.TabIndex = 5;
            this.txtDirFicheiroW.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // pnlTextNameW
            // 
            this.pnlTextNameW.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pnlTextNameW.Controls.Add(this.lblWSeparador);
            this.pnlTextNameW.Controls.Add(this.btnChkWRowActive);
            this.pnlTextNameW.Controls.Add(this.txtNameWBox);
            this.pnlTextNameW.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.pnlTextNameW.Location = new System.Drawing.Point(51, 0);
            this.pnlTextNameW.MaximumSize = new System.Drawing.Size(300, 0);
            this.pnlTextNameW.Name = "pnlTextNameW";
            this.pnlTextNameW.Size = new System.Drawing.Size(278, 50);
            this.pnlTextNameW.TabIndex = 3;
            // 
            // lblWSeparador
            // 
            this.lblWSeparador.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblWSeparador.AutoSize = true;
            this.lblWSeparador.Font = new System.Drawing.Font("Century Gothic", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblWSeparador.ForeColor = System.Drawing.Color.PaleVioletRed;
            this.lblWSeparador.Location = new System.Drawing.Point(258, 6);
            this.lblWSeparador.Name = "lblWSeparador";
            this.lblWSeparador.Size = new System.Drawing.Size(30, 32);
            this.lblWSeparador.TabIndex = 9;
            this.lblWSeparador.Text = "|";
            // 
            // btnChkWRowActive
            // 
            this.btnChkWRowActive.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnChkWRowActive.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChkWRowActive.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnChkWRowActive.ForeColor = System.Drawing.Color.PaleVioletRed;
            this.btnChkWRowActive.Location = new System.Drawing.Point(0, 0);
            this.btnChkWRowActive.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.btnChkWRowActive.Name = "btnChkWRowActive";
            this.btnChkWRowActive.Size = new System.Drawing.Size(22, 50);
            this.btnChkWRowActive.TabIndex = 5;
            this.btnChkWRowActive.Text = "✓";
            this.btnChkWRowActive.UseVisualStyleBackColor = true;
            // 
            // txtNameWBox
            // 
            this.txtNameWBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNameWBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(30)))), ((int)(((byte)(40)))));
            this.txtNameWBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtNameWBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtNameWBox.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtNameWBox.ForeColor = System.Drawing.Color.AliceBlue;
            this.txtNameWBox.Location = new System.Drawing.Point(48, 13);
            this.txtNameWBox.Margin = new System.Windows.Forms.Padding(10);
            this.txtNameWBox.MaxLength = 23;
            this.txtNameWBox.Name = "txtNameWBox";
            this.txtNameWBox.PlaceholderText = "Nome";
            this.txtNameWBox.Size = new System.Drawing.Size(204, 20);
            this.txtNameWBox.TabIndex = 4;
            // 
            // lbl_ellipse
            // 
            this.lbl_ellipse.Cursor = System.Windows.Forms.Cursors.SizeAll;
            this.lbl_ellipse.Dock = System.Windows.Forms.DockStyle.Left;
            this.lbl_ellipse.Font = new System.Drawing.Font("Arial Black", 28F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lbl_ellipse.ForeColor = System.Drawing.Color.PaleVioletRed;
            this.lbl_ellipse.Location = new System.Drawing.Point(0, 0);
            this.lbl_ellipse.Margin = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.lbl_ellipse.Name = "lbl_ellipse";
            this.lbl_ellipse.Size = new System.Drawing.Size(51, 50);
            this.lbl_ellipse.TabIndex = 2;
            this.lbl_ellipse.Text = "⋯";
            this.lbl_ellipse.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmModelarTimer
            // 
            this.frmModelarTimer.Enabled = true;
            this.frmModelarTimer.Interval = 20;
            // 
            // mainWordFlowPanel
            // 
            this.mainWordFlowPanel.AutoScroll = true;
            this.mainWordFlowPanel.AutoScrollMargin = new System.Drawing.Size(100, 0);
            this.mainWordFlowPanel.AutoScrollMinSize = new System.Drawing.Size(100, 0);
            this.mainWordFlowPanel.AutoSize = true;
            this.mainWordFlowPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.mainWordFlowPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(0)))), ((int)(((byte)(15)))));
            this.mainWordFlowPanel.Controls.Add(this.rowPanel_WordDoc);
            this.mainWordFlowPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.mainWordFlowPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.mainWordFlowPanel.Location = new System.Drawing.Point(0, 284);
            this.mainWordFlowPanel.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.mainWordFlowPanel.MinimumSize = new System.Drawing.Size(0, 200);
            this.mainWordFlowPanel.Name = "mainWordFlowPanel";
            this.mainWordFlowPanel.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.mainWordFlowPanel.Size = new System.Drawing.Size(853, 200);
            this.mainWordFlowPanel.TabIndex = 3;
            this.mainWordFlowPanel.WrapContents = false;
            // 
            // pnl_Separator
            // 
            this.pnl_Separator.BackColor = System.Drawing.Color.PaleVioletRed;
            this.pnl_Separator.Enabled = false;
            this.pnl_Separator.ForeColor = System.Drawing.Color.PaleVioletRed;
            this.pnl_Separator.Location = new System.Drawing.Point(100, 271);
            this.pnl_Separator.Name = "pnl_Separator";
            this.pnl_Separator.Size = new System.Drawing.Size(686, 4);
            this.pnl_Separator.TabIndex = 1;
            this.pnl_Separator.Visible = false;
            // 
            // panelMenu
            // 
            this.panelMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(36)))), ((int)(((byte)(57)))));
            this.panelMenu.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelMenu.Location = new System.Drawing.Point(0, 0);
            this.panelMenu.Name = "panelMenu";
            this.panelMenu.Size = new System.Drawing.Size(853, 66);
            this.panelMenu.TabIndex = 4;
            // 
            // panelWordMenuSelector
            // 
            this.panelWordMenuSelector.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(20)))), ((int)(((byte)(25)))));
            this.panelWordMenuSelector.Controls.Add(this.cmbBoxTemplateName);
            this.panelWordMenuSelector.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWordMenuSelector.Location = new System.Drawing.Point(0, 66);
            this.panelWordMenuSelector.Name = "panelWordMenuSelector";
            this.panelWordMenuSelector.Size = new System.Drawing.Size(853, 108);
            this.panelWordMenuSelector.TabIndex = 5;
            // 
            // cmbBoxTemplateName
            // 
            this.cmbBoxTemplateName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(0)))), ((int)(((byte)(15)))));
            this.cmbBoxTemplateName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbBoxTemplateName.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cmbBoxTemplateName.ForeColor = System.Drawing.Color.PaleVioletRed;
            this.cmbBoxTemplateName.FormattingEnabled = true;
            this.cmbBoxTemplateName.Location = new System.Drawing.Point(446, 38);
            this.cmbBoxTemplateName.Name = "cmbBoxTemplateName";
            this.cmbBoxTemplateName.Size = new System.Drawing.Size(251, 25);
            this.cmbBoxTemplateName.TabIndex = 0;
            // 
            // FormModelar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(20)))), ((int)(((byte)(25)))));
            this.ClientSize = new System.Drawing.Size(853, 484);
            this.Controls.Add(this.panelWordMenuSelector);
            this.Controls.Add(this.panelMenu);
            this.Controls.Add(this.mainWordFlowPanel);
            this.Controls.Add(this.pnl_Separator);
            this.Name = "FormModelar";
            this.Text = "FormModelar";
            this.rowPanel_WordDoc.ResumeLayout(false);
            this.rowPanel_WordDoc.PerformLayout();
            this.pnlTextNameW.ResumeLayout(false);
            this.pnlTextNameW.PerformLayout();
            this.mainWordFlowPanel.ResumeLayout(false);
            this.mainWordFlowPanel.PerformLayout();
            this.panelWordMenuSelector.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

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
    }
}