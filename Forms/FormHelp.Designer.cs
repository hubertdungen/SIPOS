namespace SIPOS.Forms
{
    partial class FormHelp
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormHelp));
            this.txtBox_IntroSIPOS = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtBox_IntroSIPOS
            // 
            this.txtBox_IntroSIPOS.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtBox_IntroSIPOS.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(20)))), ((int)(((byte)(25)))));
            this.txtBox_IntroSIPOS.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtBox_IntroSIPOS.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtBox_IntroSIPOS.ForeColor = System.Drawing.Color.Cyan;
            this.txtBox_IntroSIPOS.Location = new System.Drawing.Point(47, 76);
            this.txtBox_IntroSIPOS.Multiline = true;
            this.txtBox_IntroSIPOS.Name = "txtBox_IntroSIPOS";
            this.txtBox_IntroSIPOS.Size = new System.Drawing.Size(758, 309);
            this.txtBox_IntroSIPOS.TabIndex = 1;
            this.txtBox_IntroSIPOS.Text = resources.GetString("txtBox_IntroSIPOS.Text");
            // 
            // FormHelp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(20)))), ((int)(((byte)(25)))));
            this.ClientSize = new System.Drawing.Size(853, 484);
            this.Controls.Add(this.txtBox_IntroSIPOS);
            this.Name = "FormHelp";
            this.Text = "FormHelp";
            this.Load += new System.EventHandler(this.FormHelp_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox txtBox_IntroSIPOS;
    }
}