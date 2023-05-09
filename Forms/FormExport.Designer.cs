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
            this.btn_Export_TestListReader = new System.Windows.Forms.Button();
            this.btn_Export_testDaySelect_Var = new System.Windows.Forms.Button();
            this.gbox_Export = new System.Windows.Forms.GroupBox();
            this.btn_ExportPDF = new System.Windows.Forms.Button();
            this.btn_ExportWord = new System.Windows.Forms.Button();
            this.gbox_ExportDetails = new System.Windows.Forms.GroupBox();
            this.txtBox_NumOS = new System.Windows.Forms.TextBox();
            this.lbl_NumOS = new System.Windows.Forms.Label();
            this.lbl_SaveFileName_OSWord = new System.Windows.Forms.Label();
            this.txtBox_ExportDocName = new System.Windows.Forms.TextBox();
            this.btn_OpenWord = new System.Windows.Forms.Button();
            this.gbox_Export.SuspendLayout();
            this.gbox_ExportDetails.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_Export_TestListReader
            // 
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
            this.btn_Export_TestListReader.Click += new System.EventHandler(this.btn_Export_TestListReader_Click);
            // 
            // btn_Export_testDaySelect_Var
            // 
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
            this.btn_Export_testDaySelect_Var.Click += new System.EventHandler(this.btn_Export_testDaySelect_Var_Click);
            // 
            // gbox_Export
            // 
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
            this.btn_ExportWord.UseVisualStyleBackColor = false;
            this.btn_ExportWord.Click += new System.EventHandler(this.btn_ExportWord_Click);
            // 
            // gbox_ExportDetails
            // 
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
            this.txtBox_ExportDocName.TextChanged += new System.EventHandler(this.txtBox_ExportDocName_TextChanged);
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
            this.btn_OpenWord.UseVisualStyleBackColor = false;
            this.btn_OpenWord.Click += new System.EventHandler(this.btn_OpenWord_Click);
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
    }
}