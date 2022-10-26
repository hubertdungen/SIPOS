namespace Excel_Reader
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_read = new System.Windows.Forms.Button();
            this.btn_searchFile = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn_read
            // 
            this.btn_read.Location = new System.Drawing.Point(30, 22);
            this.btn_read.Name = "btn_read";
            this.btn_read.Size = new System.Drawing.Size(164, 45);
            this.btn_read.TabIndex = 0;
            this.btn_read.Text = "&Read";
            this.btn_read.UseVisualStyleBackColor = true;
            this.btn_read.Click += new System.EventHandler(this.btn_read_Click);
            // 
            // btn_searchFile
            // 
            this.btn_searchFile.Location = new System.Drawing.Point(350, 22);
            this.btn_searchFile.Name = "btn_searchFile";
            this.btn_searchFile.Size = new System.Drawing.Size(160, 45);
            this.btn_searchFile.TabIndex = 1;
            this.btn_searchFile.Text = "&Search File";
            this.btn_searchFile.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(542, 317);
            this.Controls.Add(this.btn_searchFile);
            this.Controls.Add(this.btn_read);
            this.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.Name = "Form1";
            this.Text = "Excel Reader";
            this.ResumeLayout(false);

        }

        #endregion

        private Button btn_read;
        private Button btn_searchFile;
    }
}