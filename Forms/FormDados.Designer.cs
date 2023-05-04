namespace SIPOS.Forms
{
    partial class FormDados
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
            System.Windows.Forms.Label lbl_numUpDownDayIntrup;
            this.numUpDow_diasIntp = new System.Windows.Forms.NumericUpDown();
            this.lbl_PreviewESnoDia = new System.Windows.Forms.Label();
            this.lbl_CalendarioOS = new System.Windows.Forms.Label();
            this.prg_Bar = new System.Windows.Forms.ProgressBar();
            this.textBox_Output = new System.Windows.Forms.TextBox();
            this.monthCalendar = new System.Windows.Forms.MonthCalendar();
            this.btn_refresh = new System.Windows.Forms.Button();
            this.btn_CheckEscalaList = new System.Windows.Forms.Button();
            lbl_numUpDownDayIntrup = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDow_diasIntp)).BeginInit();
            this.SuspendLayout();
            // 
            // lbl_numUpDownDayIntrup
            // 
            lbl_numUpDownDayIntrup.AutoSize = true;
            lbl_numUpDownDayIntrup.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            lbl_numUpDownDayIntrup.ForeColor = System.Drawing.Color.Gainsboro;
            lbl_numUpDownDayIntrup.Location = new System.Drawing.Point(72, 360);
            lbl_numUpDownDayIntrup.Name = "lbl_numUpDownDayIntrup";
            lbl_numUpDownDayIntrup.Size = new System.Drawing.Size(164, 25);
            lbl_numUpDownDayIntrup.TabIndex = 13;
            lbl_numUpDownDayIntrup.Text = "Dias de interrupção";
            lbl_numUpDownDayIntrup.UseCompatibleTextRendering = true;
            // 
            // numUpDow_diasIntp
            // 
            this.numUpDow_diasIntp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(26)))), ((int)(((byte)(45)))));
            this.numUpDow_diasIntp.Font = new System.Drawing.Font("Century Gothic", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.numUpDow_diasIntp.ForeColor = System.Drawing.Color.Gainsboro;
            this.numUpDow_diasIntp.Location = new System.Drawing.Point(242, 356);
            this.numUpDow_diasIntp.Name = "numUpDow_diasIntp";
            this.numUpDow_diasIntp.Size = new System.Drawing.Size(57, 32);
            this.numUpDow_diasIntp.TabIndex = 12;
            this.numUpDow_diasIntp.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numUpDow_diasIntp.ValueChanged += new System.EventHandler(this.numUpDow_diasIntp_ValueChanged);
            // 
            // lbl_PreviewESnoDia
            // 
            this.lbl_PreviewESnoDia.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_PreviewESnoDia.AutoSize = true;
            this.lbl_PreviewESnoDia.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lbl_PreviewESnoDia.ForeColor = System.Drawing.Color.Gainsboro;
            this.lbl_PreviewESnoDia.Location = new System.Drawing.Point(377, 58);
            this.lbl_PreviewESnoDia.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbl_PreviewESnoDia.Name = "lbl_PreviewESnoDia";
            this.lbl_PreviewESnoDia.Size = new System.Drawing.Size(282, 21);
            this.lbl_PreviewESnoDia.TabIndex = 11;
            this.lbl_PreviewESnoDia.Text = "Identificação do pessoal escalado:";
            // 
            // lbl_CalendarioOS
            // 
            this.lbl_CalendarioOS.AutoSize = true;
            this.lbl_CalendarioOS.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lbl_CalendarioOS.ForeColor = System.Drawing.Color.Gainsboro;
            this.lbl_CalendarioOS.Location = new System.Drawing.Point(72, 59);
            this.lbl_CalendarioOS.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbl_CalendarioOS.Name = "lbl_CalendarioOS";
            this.lbl_CalendarioOS.Size = new System.Drawing.Size(209, 21);
            this.lbl_CalendarioOS.TabIndex = 10;
            this.lbl_CalendarioOS.Text = "Dia de Publicação da OS:";
            // 
            // prg_Bar
            // 
            this.prg_Bar.Location = new System.Drawing.Point(72, 314);
            this.prg_Bar.Margin = new System.Windows.Forms.Padding(2);
            this.prg_Bar.MarqueeAnimationSpeed = 50;
            this.prg_Bar.Maximum = 22;
            this.prg_Bar.Name = "prg_Bar";
            this.prg_Bar.Size = new System.Drawing.Size(227, 11);
            this.prg_Bar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.prg_Bar.TabIndex = 9;
            // 
            // textBox_Output
            // 
            this.textBox_Output.AcceptsReturn = true;
            this.textBox_Output.AcceptsTab = true;
            this.textBox_Output.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_Output.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(26)))), ((int)(((byte)(45)))));
            this.textBox_Output.ForeColor = System.Drawing.Color.Cyan;
            this.textBox_Output.Location = new System.Drawing.Point(377, 86);
            this.textBox_Output.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_Output.MaxLength = 40000;
            this.textBox_Output.Multiline = true;
            this.textBox_Output.Name = "textBox_Output";
            this.textBox_Output.ReadOnly = true;
            this.textBox_Output.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_Output.Size = new System.Drawing.Size(403, 326);
            this.textBox_Output.TabIndex = 7;
            this.textBox_Output.Text = "À espera que seleccione uma data de publicação da OS, para mostrar o pessoal esca" +
    "lado.";
            // 
            // monthCalendar
            // 
            this.monthCalendar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(20)))), ((int)(((byte)(25)))));
            this.monthCalendar.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.monthCalendar.Location = new System.Drawing.Point(72, 88);
            this.monthCalendar.Margin = new System.Windows.Forms.Padding(8);
            this.monthCalendar.MaxSelectionCount = 1;
            this.monthCalendar.Name = "monthCalendar";
            this.monthCalendar.TabIndex = 4;
            this.monthCalendar.TodayDate = new System.DateTime(2022, 10, 29, 0, 0, 0, 0);
            this.monthCalendar.DateChanged += new System.Windows.Forms.DateRangeEventHandler(this.monthCalendar_DateChanged);
            // 
            // btn_refresh
            // 
            this.btn_refresh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(30)))), ((int)(((byte)(51)))));
            this.btn_refresh.FlatAppearance.BorderSize = 0;
            this.btn_refresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_refresh.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btn_refresh.ForeColor = System.Drawing.Color.Cyan;
            this.btn_refresh.Location = new System.Drawing.Point(72, 251);
            this.btn_refresh.Margin = new System.Windows.Forms.Padding(2);
            this.btn_refresh.Name = "btn_refresh";
            this.btn_refresh.Size = new System.Drawing.Size(227, 62);
            this.btn_refresh.TabIndex = 3;
            this.btn_refresh.Text = "&REFRESH";
            this.btn_refresh.UseVisualStyleBackColor = false;
            this.btn_refresh.Click += new System.EventHandler(this.btn_refresh_Click_1);
            // 
            // btn_CheckEscalaList
            // 
            this.btn_CheckEscalaList.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btn_CheckEscalaList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(30)))), ((int)(((byte)(51)))));
            this.btn_CheckEscalaList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_CheckEscalaList.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btn_CheckEscalaList.ForeColor = System.Drawing.Color.Cyan;
            this.btn_CheckEscalaList.Location = new System.Drawing.Point(72, 419);
            this.btn_CheckEscalaList.Margin = new System.Windows.Forms.Padding(2);
            this.btn_CheckEscalaList.Name = "btn_CheckEscalaList";
            this.btn_CheckEscalaList.Size = new System.Drawing.Size(227, 45);
            this.btn_CheckEscalaList.TabIndex = 14;
            this.btn_CheckEscalaList.Text = "Check Lista de Escalados";
            this.btn_CheckEscalaList.UseVisualStyleBackColor = false;
            this.btn_CheckEscalaList.Click += new System.EventHandler(this.btn_CheckEscalaList_Click);
            // 
            // FormDados
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(20)))), ((int)(((byte)(25)))));
            this.ClientSize = new System.Drawing.Size(853, 484);
            this.Controls.Add(this.btn_CheckEscalaList);
            this.Controls.Add(this.prg_Bar);
            this.Controls.Add(this.numUpDow_diasIntp);
            this.Controls.Add(lbl_numUpDownDayIntrup);
            this.Controls.Add(this.monthCalendar);
            this.Controls.Add(this.lbl_PreviewESnoDia);
            this.Controls.Add(this.lbl_CalendarioOS);
            this.Controls.Add(this.textBox_Output);
            this.Controls.Add(this.btn_refresh);
            this.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Name = "FormDados";
            this.Text = "FormDados";
            this.Load += new System.EventHandler(this.FormDados_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numUpDow_diasIntp)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private NumericUpDown numUpDow_diasIntp;
        private Label lbl_PreviewESnoDia;
        private Label lbl_CalendarioOS;
        private ProgressBar prg_Bar;
        private TextBox textBox_Output;
        private MonthCalendar monthCalendar;
        private Button btn_refresh;
        private Button btn_CheckEscalaList;
    }
}