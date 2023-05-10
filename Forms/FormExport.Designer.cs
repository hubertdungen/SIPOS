namespace SIPOS.Forms
{
    partial class FormExport
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
            this.btn_Export_TestListReader = new System.Windows.Forms.Button();
            this.btn_Export_testDaySelect_Var = new System.Windows.Forms.Button();
            this.gbox_Export = new System.Windows.Forms.GroupBox();
            this.btn_OpenPDF = new System.Windows.Forms.Button();
            this.btn_reportPDFFile_onExportFolder = new System.Windows.Forms.Button();
            this.btn_reportWordFile_onExportFolder = new System.Windows.Forms.Button();
            this.btn_OpenWord = new System.Windows.Forms.Button();
            this.btn_ExportPDF = new System.Windows.Forms.Button();
            this.btn_ExportWord = new System.Windows.Forms.Button();
            this.gbox_ExportDetails = new System.Windows.Forms.GroupBox();
            this.btn_reportPDFFile_onInspect = new System.Windows.Forms.Button();
            this.btn_reportWordFile_onInspect = new System.Windows.Forms.Button();
            this.txtBox_NumOS = new System.Windows.Forms.TextBox();
            this.lbl_NumOS = new System.Windows.Forms.Label();
            this.lbl_SaveFileName_OSWord = new System.Windows.Forms.Label();
            this.txtBox_ExportDocName = new System.Windows.Forms.TextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.gbox_Export.SuspendLayout();
            this.gbox_ExportDetails.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_Export_TestListReader
            // 
            this.btn_Export_TestListReader.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btn_Export_TestListReader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(30)))), ((int)(((byte)(51)))));
            this.btn_Export_TestListReader.FlatAppearance.BorderSize = 0;
            this.btn_Export_TestListReader.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Export_TestListReader.Font = new System.Drawing.Font("Century Gothic", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btn_Export_TestListReader.ForeColor = System.Drawing.Color.IndianRed;
            this.btn_Export_TestListReader.Location = new System.Drawing.Point(263, 355);
            this.btn_Export_TestListReader.Margin = new System.Windows.Forms.Padding(2);
            this.btn_Export_TestListReader.Name = "btn_Export_TestListReader";
            this.btn_Export_TestListReader.Size = new System.Drawing.Size(132, 49);
            this.btn_Export_TestListReader.TabIndex = 3;
            this.btn_Export_TestListReader.Text = "Test List Reader";
            this.btn_Export_TestListReader.UseVisualStyleBackColor = false;
            this.btn_Export_TestListReader.Visible = false;
            this.btn_Export_TestListReader.Click += new System.EventHandler(this.btn_Export_TestListReader_Click);
            // 
            // btn_Export_testDaySelect_Var
            // 
            this.btn_Export_testDaySelect_Var.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btn_Export_testDaySelect_Var.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(30)))), ((int)(((byte)(51)))));
            this.btn_Export_testDaySelect_Var.FlatAppearance.BorderSize = 0;
            this.btn_Export_testDaySelect_Var.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Export_testDaySelect_Var.Font = new System.Drawing.Font("Century Gothic", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btn_Export_testDaySelect_Var.ForeColor = System.Drawing.Color.IndianRed;
            this.btn_Export_testDaySelect_Var.Location = new System.Drawing.Point(119, 355);
            this.btn_Export_testDaySelect_Var.Margin = new System.Windows.Forms.Padding(2);
            this.btn_Export_testDaySelect_Var.Name = "btn_Export_testDaySelect_Var";
            this.btn_Export_testDaySelect_Var.Size = new System.Drawing.Size(132, 49);
            this.btn_Export_testDaySelect_Var.TabIndex = 2;
            this.btn_Export_testDaySelect_Var.Text = "Test DaySelect Var";
            this.btn_Export_testDaySelect_Var.UseVisualStyleBackColor = false;
            this.btn_Export_testDaySelect_Var.Visible = false;
            this.btn_Export_testDaySelect_Var.Click += new System.EventHandler(this.btn_Export_testDaySelect_Var_Click);
            // 
            // gbox_Export
            // 
            this.gbox_Export.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.gbox_Export.Controls.Add(this.btn_OpenPDF);
            this.gbox_Export.Controls.Add(this.btn_reportPDFFile_onExportFolder);
            this.gbox_Export.Controls.Add(this.btn_reportWordFile_onExportFolder);
            this.gbox_Export.Controls.Add(this.btn_OpenWord);
            this.gbox_Export.Controls.Add(this.btn_ExportPDF);
            this.gbox_Export.Controls.Add(this.btn_ExportWord);
            this.gbox_Export.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.gbox_Export.ForeColor = System.Drawing.Color.Gainsboro;
            this.gbox_Export.Location = new System.Drawing.Point(408, 103);
            this.gbox_Export.Margin = new System.Windows.Forms.Padding(2);
            this.gbox_Export.Name = "gbox_Export";
            this.gbox_Export.Padding = new System.Windows.Forms.Padding(2);
            this.gbox_Export.Size = new System.Drawing.Size(299, 234);
            this.gbox_Export.TabIndex = 1;
            this.gbox_Export.TabStop = false;
            this.gbox_Export.Text = "Exportar";
            // 
            // btn_OpenPDF
            // 
            this.btn_OpenPDF.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(25)))), ((int)(((byte)(51)))));
            this.btn_OpenPDF.FlatAppearance.BorderSize = 0;
            this.btn_OpenPDF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_OpenPDF.Font = new System.Drawing.Font("Century Gothic", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btn_OpenPDF.ForeColor = System.Drawing.Color.IndianRed;
            this.btn_OpenPDF.Location = new System.Drawing.Point(228, 134);
            this.btn_OpenPDF.Margin = new System.Windows.Forms.Padding(2);
            this.btn_OpenPDF.Name = "btn_OpenPDF";
            this.btn_OpenPDF.Size = new System.Drawing.Size(57, 70);
            this.btn_OpenPDF.TabIndex = 38;
            this.btn_OpenPDF.Text = "📁";
            this.toolTip1.SetToolTip(this.btn_OpenPDF, "Abrir ficheiro exportado.");
            this.btn_OpenPDF.UseVisualStyleBackColor = false;
            this.btn_OpenPDF.Click += new System.EventHandler(this.btn_OpenPDF_Click);
            // 
            // btn_reportPDFFile_onExportFolder
            // 
            this.btn_reportPDFFile_onExportFolder.BackColor = System.Drawing.Color.IndianRed;
            this.btn_reportPDFFile_onExportFolder.FlatAppearance.BorderSize = 0;
            this.btn_reportPDFFile_onExportFolder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_reportPDFFile_onExportFolder.Font = new System.Drawing.Font("Century Gothic", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btn_reportPDFFile_onExportFolder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(184)))), ((int)(((byte)(226)))));
            this.btn_reportPDFFile_onExportFolder.Location = new System.Drawing.Point(216, 134);
            this.btn_reportPDFFile_onExportFolder.Margin = new System.Windows.Forms.Padding(2);
            this.btn_reportPDFFile_onExportFolder.Name = "btn_reportPDFFile_onExportFolder";
            this.btn_reportPDFFile_onExportFolder.Size = new System.Drawing.Size(10, 70);
            this.btn_reportPDFFile_onExportFolder.TabIndex = 37;
            this.toolTip1.SetToolTip(this.btn_reportPDFFile_onExportFolder, "ATENÇÃO: Já existe um ficheiro PDF com o mesmo nome na pasta destino de exportaçã" +
        "o.");
            this.btn_reportPDFFile_onExportFolder.UseVisualStyleBackColor = false;
            this.btn_reportPDFFile_onExportFolder.Visible = false;
            // 
            // btn_reportWordFile_onExportFolder
            // 
            this.btn_reportWordFile_onExportFolder.BackColor = System.Drawing.Color.IndianRed;
            this.btn_reportWordFile_onExportFolder.FlatAppearance.BorderSize = 0;
            this.btn_reportWordFile_onExportFolder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_reportWordFile_onExportFolder.Font = new System.Drawing.Font("Century Gothic", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btn_reportWordFile_onExportFolder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(184)))), ((int)(((byte)(226)))));
            this.btn_reportWordFile_onExportFolder.Location = new System.Drawing.Point(216, 45);
            this.btn_reportWordFile_onExportFolder.Margin = new System.Windows.Forms.Padding(2);
            this.btn_reportWordFile_onExportFolder.Name = "btn_reportWordFile_onExportFolder";
            this.btn_reportWordFile_onExportFolder.Size = new System.Drawing.Size(10, 70);
            this.btn_reportWordFile_onExportFolder.TabIndex = 36;
            this.toolTip1.SetToolTip(this.btn_reportWordFile_onExportFolder, "ATENÇÃO: Já existe um ficheiro WORD com o mesmo nome na pasta destino de exportaç" +
        "ão.");
            this.btn_reportWordFile_onExportFolder.UseVisualStyleBackColor = false;
            this.btn_reportWordFile_onExportFolder.Visible = false;
            // 
            // btn_OpenWord
            // 
            this.btn_OpenWord.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(25)))), ((int)(((byte)(51)))));
            this.btn_OpenWord.FlatAppearance.BorderSize = 0;
            this.btn_OpenWord.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_OpenWord.Font = new System.Drawing.Font("Century Gothic", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btn_OpenWord.ForeColor = System.Drawing.Color.IndianRed;
            this.btn_OpenWord.Location = new System.Drawing.Point(228, 45);
            this.btn_OpenWord.Margin = new System.Windows.Forms.Padding(2);
            this.btn_OpenWord.Name = "btn_OpenWord";
            this.btn_OpenWord.Size = new System.Drawing.Size(57, 70);
            this.btn_OpenWord.TabIndex = 4;
            this.btn_OpenWord.Text = "📁";
            this.toolTip1.SetToolTip(this.btn_OpenWord, "Abrir ficheiro exportado.");
            this.btn_OpenWord.UseVisualStyleBackColor = false;
            this.btn_OpenWord.Click += new System.EventHandler(this.btn_OpenWord_Click);
            // 
            // btn_ExportPDF
            // 
            this.btn_ExportPDF.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(30)))), ((int)(((byte)(51)))));
            this.btn_ExportPDF.FlatAppearance.BorderSize = 0;
            this.btn_ExportPDF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_ExportPDF.Font = new System.Drawing.Font("Century Gothic", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btn_ExportPDF.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(138)))), ((int)(((byte)(28)))));
            this.btn_ExportPDF.Location = new System.Drawing.Point(17, 134);
            this.btn_ExportPDF.Margin = new System.Windows.Forms.Padding(2);
            this.btn_ExportPDF.Name = "btn_ExportPDF";
            this.btn_ExportPDF.Size = new System.Drawing.Size(268, 70);
            this.btn_ExportPDF.TabIndex = 1;
            this.btn_ExportPDF.Text = "PDF";
            this.toolTip1.SetToolTip(this.btn_ExportPDF, "Exportar o Documento PDF.");
            this.btn_ExportPDF.UseVisualStyleBackColor = false;
            // 
            // btn_ExportWord
            // 
            this.btn_ExportWord.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(30)))), ((int)(((byte)(51)))));
            this.btn_ExportWord.FlatAppearance.BorderSize = 0;
            this.btn_ExportWord.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_ExportWord.Font = new System.Drawing.Font("Century Gothic", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btn_ExportWord.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(184)))), ((int)(((byte)(226)))));
            this.btn_ExportWord.Location = new System.Drawing.Point(17, 45);
            this.btn_ExportWord.Margin = new System.Windows.Forms.Padding(2);
            this.btn_ExportWord.Name = "btn_ExportWord";
            this.btn_ExportWord.Size = new System.Drawing.Size(268, 70);
            this.btn_ExportWord.TabIndex = 0;
            this.btn_ExportWord.Text = "WORD";
            this.toolTip1.SetToolTip(this.btn_ExportWord, "Exportar o Documento Word.");
            this.btn_ExportWord.UseVisualStyleBackColor = false;
            this.btn_ExportWord.Click += new System.EventHandler(this.btn_ExportWord_Click);
            // 
            // gbox_ExportDetails
            // 
            this.gbox_ExportDetails.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.gbox_ExportDetails.Controls.Add(this.btn_reportPDFFile_onInspect);
            this.gbox_ExportDetails.Controls.Add(this.btn_reportWordFile_onInspect);
            this.gbox_ExportDetails.Controls.Add(this.txtBox_NumOS);
            this.gbox_ExportDetails.Controls.Add(this.lbl_NumOS);
            this.gbox_ExportDetails.Controls.Add(this.lbl_SaveFileName_OSWord);
            this.gbox_ExportDetails.Controls.Add(this.txtBox_ExportDocName);
            this.gbox_ExportDetails.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.gbox_ExportDetails.ForeColor = System.Drawing.Color.Gainsboro;
            this.gbox_ExportDetails.Location = new System.Drawing.Point(119, 104);
            this.gbox_ExportDetails.Margin = new System.Windows.Forms.Padding(2);
            this.gbox_ExportDetails.Name = "gbox_ExportDetails";
            this.gbox_ExportDetails.Padding = new System.Windows.Forms.Padding(2);
            this.gbox_ExportDetails.Size = new System.Drawing.Size(238, 233);
            this.gbox_ExportDetails.TabIndex = 0;
            this.gbox_ExportDetails.TabStop = false;
            this.gbox_ExportDetails.Text = "Especificações";
            // 
            // btn_reportPDFFile_onInspect
            // 
            this.btn_reportPDFFile_onInspect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(25)))), ((int)(((byte)(51)))));
            this.btn_reportPDFFile_onInspect.FlatAppearance.BorderSize = 0;
            this.btn_reportPDFFile_onInspect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_reportPDFFile_onInspect.Font = new System.Drawing.Font("Century Gothic", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btn_reportPDFFile_onInspect.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(138)))), ((int)(((byte)(28)))));
            this.btn_reportPDFFile_onInspect.Location = new System.Drawing.Point(113, 52);
            this.btn_reportPDFFile_onInspect.Margin = new System.Windows.Forms.Padding(2);
            this.btn_reportPDFFile_onInspect.Name = "btn_reportPDFFile_onInspect";
            this.btn_reportPDFFile_onInspect.Size = new System.Drawing.Size(29, 24);
            this.btn_reportPDFFile_onInspect.TabIndex = 35;
            this.btn_reportPDFFile_onInspect.Text = "P";
            this.toolTip1.SetToolTip(this.btn_reportPDFFile_onInspect, "Já existe um ficheiro PDF com este nome na pasta de Inspeção.");
            this.btn_reportPDFFile_onInspect.UseVisualStyleBackColor = false;
            this.btn_reportPDFFile_onInspect.Visible = false;
            // 
            // btn_reportWordFile_onInspect
            // 
            this.btn_reportWordFile_onInspect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(25)))), ((int)(((byte)(51)))));
            this.btn_reportWordFile_onInspect.FlatAppearance.BorderSize = 0;
            this.btn_reportWordFile_onInspect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_reportWordFile_onInspect.Font = new System.Drawing.Font("Century Gothic", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btn_reportWordFile_onInspect.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(184)))), ((int)(((byte)(226)))));
            this.btn_reportWordFile_onInspect.Location = new System.Drawing.Point(77, 52);
            this.btn_reportWordFile_onInspect.Margin = new System.Windows.Forms.Padding(2);
            this.btn_reportWordFile_onInspect.Name = "btn_reportWordFile_onInspect";
            this.btn_reportWordFile_onInspect.Size = new System.Drawing.Size(29, 24);
            this.btn_reportWordFile_onInspect.TabIndex = 5;
            this.btn_reportWordFile_onInspect.Text = "W";
            this.toolTip1.SetToolTip(this.btn_reportWordFile_onInspect, "Já existe um ficheiro WORD com este nome na pasta de Inspeção.");
            this.btn_reportWordFile_onInspect.UseVisualStyleBackColor = false;
            this.btn_reportWordFile_onInspect.Visible = false;
            // 
            // txtBox_NumOS
            // 
            this.txtBox_NumOS.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(26)))), ((int)(((byte)(45)))));
            this.txtBox_NumOS.Font = new System.Drawing.Font("Century Gothic", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.txtBox_NumOS.ForeColor = System.Drawing.Color.Cyan;
            this.txtBox_NumOS.Location = new System.Drawing.Point(15, 52);
            this.txtBox_NumOS.Margin = new System.Windows.Forms.Padding(2);
            this.txtBox_NumOS.Name = "txtBox_NumOS";
            this.txtBox_NumOS.Size = new System.Drawing.Size(54, 24);
            this.txtBox_NumOS.TabIndex = 34;
            this.toolTip1.SetToolTip(this.txtBox_NumOS, "Número da Ordem de Serviço");
            this.txtBox_NumOS.TextChanged += new System.EventHandler(this.txtBox_NumOS_TextChanged);
            // 
            // lbl_NumOS
            // 
            this.lbl_NumOS.AutoSize = true;
            this.lbl_NumOS.Font = new System.Drawing.Font("Century Gothic", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lbl_NumOS.Location = new System.Drawing.Point(14, 29);
            this.lbl_NumOS.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbl_NumOS.Name = "lbl_NumOS";
            this.lbl_NumOS.Size = new System.Drawing.Size(55, 19);
            this.lbl_NumOS.TabIndex = 33;
            this.lbl_NumOS.Text = "N.º OS:";
            this.toolTip1.SetToolTip(this.lbl_NumOS, "Número da Ordem de Serviço");
            // 
            // lbl_SaveFileName_OSWord
            // 
            this.lbl_SaveFileName_OSWord.AutoSize = true;
            this.lbl_SaveFileName_OSWord.Font = new System.Drawing.Font("Century Gothic", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lbl_SaveFileName_OSWord.Location = new System.Drawing.Point(14, 81);
            this.lbl_SaveFileName_OSWord.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbl_SaveFileName_OSWord.Name = "lbl_SaveFileName_OSWord";
            this.lbl_SaveFileName_OSWord.Size = new System.Drawing.Size(163, 19);
            this.lbl_SaveFileName_OSWord.TabIndex = 32;
            this.lbl_SaveFileName_OSWord.Text = "Nome do Documento:";
            this.toolTip1.SetToolTip(this.lbl_SaveFileName_OSWord, "Nome do documento a ser exportado.");
            // 
            // txtBox_ExportDocName
            // 
            this.txtBox_ExportDocName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(26)))), ((int)(((byte)(45)))));
            this.txtBox_ExportDocName.Font = new System.Drawing.Font("Century Gothic", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.txtBox_ExportDocName.ForeColor = System.Drawing.Color.Cyan;
            this.txtBox_ExportDocName.Location = new System.Drawing.Point(15, 106);
            this.txtBox_ExportDocName.Margin = new System.Windows.Forms.Padding(2);
            this.txtBox_ExportDocName.Name = "txtBox_ExportDocName";
            this.txtBox_ExportDocName.Size = new System.Drawing.Size(127, 24);
            this.txtBox_ExportDocName.TabIndex = 30;
            this.toolTip1.SetToolTip(this.txtBox_ExportDocName, "Nome do documento a ser exportado.");
            this.txtBox_ExportDocName.TextChanged += new System.EventHandler(this.txtBox_ExportDocName_TextChanged);
            // 
            // FormExport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(20)))), ((int)(((byte)(25)))));
            this.ClientSize = new System.Drawing.Size(853, 484);
            this.Controls.Add(this.btn_Export_TestListReader);
            this.Controls.Add(this.btn_Export_testDaySelect_Var);
            this.Controls.Add(this.gbox_ExportDetails);
            this.Controls.Add(this.gbox_Export);
            this.Name = "FormExport";
            this.Text = "FormExport";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormExport_FormClosed);
            this.Load += new System.EventHandler(this.FormExport_Load);
            this.gbox_Export.ResumeLayout(false);
            this.gbox_ExportDetails.ResumeLayout(false);
            this.gbox_ExportDetails.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Button btn_Export_TestListReader;
        private Button btn_Export_testDaySelect_Var;
        private GroupBox gbox_Export;
        private Button btn_ExportPDF;
        private Button btn_ExportWord;
        private GroupBox gbox_ExportDetails;
        private TextBox txtBox_NumOS;
        private Label lbl_NumOS;
        private Label lbl_SaveFileName_OSWord;
        private TextBox txtBox_ExportDocName;
        private Button btn_OpenWord;
        private Button btn_reportPDFFile_onInspect;
        private Button btn_reportWordFile_onInspect;
        private Button btn_reportWordFile_onExportFolder;
        private ToolTip toolTip1;
        private Button btn_reportPDFFile_onExportFolder;
        private Button btn_OpenPDF;
    }
}