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
            this.flp_RowWordDoc = new System.Windows.Forms.FlowLayoutPanel();
            this.lbl_Retis = new System.Windows.Forms.Label();
            this.txtBox_DocDrctPath = new System.Windows.Forms.TextBox();
            this.btn_plus = new System.Windows.Forms.Button();
            this.btn_minus = new System.Windows.Forms.Button();
            this.mainPanel = new System.Windows.Forms.Panel();
            this.flp_RowWordDoc.SuspendLayout();
            this.mainPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // flp_RowWordDoc
            // 
            this.flp_RowWordDoc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(30)))), ((int)(((byte)(40)))));
            this.flp_RowWordDoc.Controls.Add(this.lbl_Retis);
            this.flp_RowWordDoc.Controls.Add(this.txtBox_DocDrctPath);
            this.flp_RowWordDoc.Controls.Add(this.btn_plus);
            this.flp_RowWordDoc.Controls.Add(this.btn_minus);
            this.flp_RowWordDoc.Location = new System.Drawing.Point(19, 15);
            this.flp_RowWordDoc.Name = "flp_RowWordDoc";
            this.flp_RowWordDoc.Size = new System.Drawing.Size(729, 43);
            this.flp_RowWordDoc.TabIndex = 1;
            // 
            // lbl_Retis
            // 
            this.lbl_Retis.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lbl_Retis.AutoSize = true;
            this.lbl_Retis.Cursor = System.Windows.Forms.Cursors.SizeAll;
            this.lbl_Retis.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lbl_Retis.ForeColor = System.Drawing.Color.PaleVioletRed;
            this.lbl_Retis.Location = new System.Drawing.Point(3, 0);
            this.lbl_Retis.Name = "lbl_Retis";
            this.lbl_Retis.Size = new System.Drawing.Size(31, 38);
            this.lbl_Retis.TabIndex = 1;
            this.lbl_Retis.Text = "...";
            this.lbl_Retis.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtBox_DocDrctPath
            // 
            this.txtBox_DocDrctPath.AllowDrop = true;
            this.txtBox_DocDrctPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txtBox_DocDrctPath.Location = new System.Drawing.Point(57, 10);
            this.txtBox_DocDrctPath.Margin = new System.Windows.Forms.Padding(20, 10, 10, 3);
            this.txtBox_DocDrctPath.Name = "txtBox_DocDrctPath";
            this.txtBox_DocDrctPath.PlaceholderText = "Directory Path";
            this.txtBox_DocDrctPath.Size = new System.Drawing.Size(362, 23);
            this.txtBox_DocDrctPath.TabIndex = 0;
            // 
            // btn_plus
            // 
            this.btn_plus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_plus.Location = new System.Drawing.Point(432, 5);
            this.btn_plus.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.btn_plus.Name = "btn_plus";
            this.btn_plus.Size = new System.Drawing.Size(37, 30);
            this.btn_plus.TabIndex = 2;
            this.btn_plus.Text = "+";
            this.btn_plus.UseVisualStyleBackColor = true;
            this.btn_plus.Click += new System.EventHandler(this.btn_plus_Click);
            // 
            // btn_minus
            // 
            this.btn_minus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_minus.Location = new System.Drawing.Point(475, 5);
            this.btn_minus.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.btn_minus.Name = "btn_minus";
            this.btn_minus.Size = new System.Drawing.Size(37, 30);
            this.btn_minus.TabIndex = 3;
            this.btn_minus.Text = "-";
            this.btn_minus.UseVisualStyleBackColor = true;
            this.btn_minus.Click += new System.EventHandler(this.btn_minus_Click);
            // 
            // mainPanel
            // 
            this.mainPanel.AutoScroll = true;
            this.mainPanel.Controls.Add(this.flp_RowWordDoc);
            this.mainPanel.Location = new System.Drawing.Point(28, 102);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(778, 329);
            this.mainPanel.TabIndex = 2;
            // 
            // FormModelar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(20)))), ((int)(((byte)(25)))));
            this.ClientSize = new System.Drawing.Size(853, 484);
            this.Controls.Add(this.mainPanel);
            this.Name = "FormModelar";
            this.Text = "FormModelar";
            this.flp_RowWordDoc.ResumeLayout(false);
            this.flp_RowWordDoc.PerformLayout();
            this.mainPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private FlowLayoutPanel flp_RowWordDoc;
        private Label lbl_Retis;
        private TextBox txtBox_DocDrctPath;
        private Button btn_plus;
        private Button btn_minus;
        private Panel mainPanel;
    }
}