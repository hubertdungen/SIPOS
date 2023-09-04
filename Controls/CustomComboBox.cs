using System;

using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.Xml;
using System.Windows.Navigation;
using System.Drawing.Design;


namespace SIPOS.Controls
{
    [DefaultEvent("OnSelectedIndexChanged")]
    class CustomComboBox : UserControl
    {
       
        //Fields
        private Color backColor = Color.WhiteSmoke;
        private Color iconColor = Color.MediumSlateBlue;
        private Color listBackColor = Color.FromArgb(230, 228, 245);
        private Color listTextColor = Color.DimGray;
        private Color borderColor = Color.MediumSlateBlue;
        private int borderSize = 1;


        //Items
        private ComboBox cmbList;
        private Label lblText;
        private Button btnIcon;

        //Properties
        //-> Appearance
        [Category("HD - Custom Appearance")]
        public new Color BackColor { get { return backColor; } set { backColor = value; lblText.BackColor = backColor; btnIcon.BackColor = backColor; } }
        [Category("HD - Custom Appearance")]
        public Color IconColor { get { return iconColor; } set { iconColor = value; btnIcon.Invalidate();/*Redraw icon*/ } }
        [Category("HD - Custom Appearance")]
        public Color ListBackColor { get { return listBackColor; }  set { listBackColor = value; cmbList.BackColor = listBackColor; } }
        [Category("HD - Custom Appearance")]
        public Color ListTextColor { get { return listTextColor; } set { listTextColor = value; cmbList.ForeColor = listTextColor; } }
        [Category("HD - Custom Appearance")]
        public Color BorderColor { get { return borderColor; } set { borderColor = value; base.BackColor = borderColor; /*Border Color*/ } }
        [Category("HD - Custom Appearance")]
        public int BorderSize { get { return borderSize; } set { borderSize = value; this.Padding = new Padding(borderSize); /*Border Size*/ AdjustComboBoxDimensions(); } }
        [Category("HD - Custom Appearance")]
        public override Color ForeColor { get { return base.ForeColor; } set { base.ForeColor = value; lblText.ForeColor = value; } }
        [Category("HD - Custom Appearance")]
        public override Font Font { get { return base.Font; } set { base.Font = value; lblText.Font = value; cmbList.Font = value; } }
        [Category("HD - Custom Appearance")]
        public string Texts {  get { return lblText.Text; } set { lblText.Text = value; } }

        [Category ("HD - Custom Appearance")]
        public ComboBoxStyle DropDownStyle { get { return cmbList.DropDownStyle; } set { if (cmbList.DropDownStyle != ComboBoxStyle.Simple) { cmbList.DropDownStyle = value;  } } }

        //-> Data
        [Category("HD - Custom Data")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Editor("System.Windows.Forms.Design.ListControlStringCollectionEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [Localizable(true)]
        [MergableProperty(false)]
        public ComboBox.ObjectCollection Items
        {
            get { return cmbList.Items; }
        }

        [Category("HD - Custom Data")]
        [AttributeProvider(typeof(IListSource))]
        [DefaultValue(null)]
        public object DataSource
        {
            get { return cmbList.DataSource; }
            set { cmbList.DataSource = value; }
        }



        [Category("HD - Custom Data")]
        [Browsable(true)]
        [DefaultValue(AutoCompleteMode.None)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public AutoCompleteMode AutoCompleteMode
        {
            get { return cmbList.AutoCompleteMode; }
            set { cmbList.AutoCompleteMode = value; }
        }

        [Category("HD - Custom Data")]
        [Browsable(true)]
        [DefaultValue(AutoCompleteSource.None)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public AutoCompleteSource AutoCompleteSource
        {
            get { return cmbList.AutoCompleteSource; }
            set { cmbList.AutoCompleteSource = value; }
        }

        [Category("HD - Custom Data")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Editor("System.Windows.Forms.Design.ListControlStringCollectionEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Localizable(true)]
        public AutoCompleteStringCollection AutoCompleteCustomSource
        {
            get { return cmbList.AutoCompleteCustomSource; }
            set { cmbList.AutoCompleteCustomSource = value; }
        }


        [Category("HD - Custom Data")]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int SelectedIndex
        {
            get { return cmbList.SelectedIndex; }
            set { cmbList.SelectedIndex = value; }
        }


        [Category("HD - Custom Data")]
        [Bindable(true)]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object SelectedItem
        {
            get { return cmbList.SelectedItem; }
            set { cmbList.SelectedItem = value; }
        }



        [Category("HD - Custom Data")]
        [DefaultValue("")]
        [Editor("System.Windows.Forms.Design.DataMemberFieldEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [TypeConverter("System.Windows.Forms.Design.DataMemberFieldConverter, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
        public string DisplayMember
        {
            get { return cmbList.DisplayMember; }
            set { cmbList.DisplayMember = value; }
        }


        [Category("HD - Custom Data")]
        [DefaultValue("")]
        [Editor("System.Windows.Forms.Design.DataMemberFieldEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        public string ValueMember
        {
            get { return cmbList.ValueMember; }
            set { cmbList.ValueMember = value; }
        }




        //Events
        public event EventHandler OnSelectedIndexChanged;//Default event


        //Constructor
        public CustomComboBox()
        {
            cmbList = new ComboBox();
            lblText = new Label();
            btnIcon = new Button();
            this.SuspendLayout();

            //ComboBox: Dropdonn list
            cmbList.BackColor = listBackColor;
            cmbList.Font = new Font(this.Font.Name, 10F);
            cmbList.ForeColor = backColor;
            cmbList.SelectedIndexChanged += new EventHandler(ComboBox_SelectedIndexChanged); //Default event
            cmbList.TextChanged += new EventHandler(ComboBox_TextChanged); //Refresh text

            //Button: Icon
            btnIcon.Dock = DockStyle.Right;
            btnIcon.FlatStyle = FlatStyle.Flat;
            btnIcon.FlatAppearance.BorderSize = 0;
            btnIcon.BackColor = backColor;
            btnIcon.Size = new Size(36, 36);
            btnIcon.Cursor = Cursors.Hand;
            btnIcon.Click += new EventHandler(Icon_Click);//Open Dropdown list
            btnIcon.Paint += new PaintEventHandler(Icon_Paint);//Draw Icon

            //Label: Text
            lblText.Dock = DockStyle.Fill;
            lblText.AutoSize = false;
            lblText.BackColor = backColor;
            lblText.TextAlign = ContentAlignment.MiddleLeft;
            lblText.Padding = new Padding(8, 0, 0, 0);
            lblText.Font = new Font(this.Font.Name, 10F);
            lblText.Click += new EventHandler(Surface_Click);//Select combo box

            //User Control
            this.Controls.Add(lblText);//2
            this.Controls.Add(btnIcon);//1
            this.Controls.Add(cmbList);//0
            this.MinimumSize = new Size(200, 30);
            this.Size = new Size(200, 30);
            this.ForeColor = Color.DimGray;
            this.Padding = new Padding(borderSize);//Border Size
            this.ResumeLayout();
            AdjustComboBoxDimensions();

        }


        //Private methods
        private void AdjustComboBoxDimensions()
        {
            cmbList.Width = lblText.Width;
            cmbList.Location = new Point()
            {
                X = this.Width - this.Padding.Right - cmbList.Width,
                Y = lblText.Bottom - cmbList.Height
            };
        }


        // EVENT METHODS

        //-> Default Event
        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (OnSelectedIndexChanged != null)
                OnSelectedIndexChanged.Invoke(sender, e);
            //Refresh text
            lblText.Text = cmbList.Text;
        }

        //-> Draw icon
        private void Icon_Paint(object sender, PaintEventArgs e)
        {
            //Fields
            int iconWidth = 14;
            int iconHeight = 6;
            var rectIcon = new Rectangle((btnIcon.Width - iconWidth) / 2, (btnIcon.Height - iconHeight) / 2, iconWidth, iconHeight);
            Graphics graph = e.Graphics;

            //Draw arrow down icon
            using (GraphicsPath path = new GraphicsPath())
            using (Pen pen = new Pen(iconColor, 2))
            {
                graph.SmoothingMode = SmoothingMode.AntiAlias;
                path.AddLine(rectIcon.X, rectIcon.Y, rectIcon.X + (iconWidth / 2), rectIcon.Bottom);
                path.AddLine(rectIcon.X + (iconWidth / 2), rectIcon.Bottom, rectIcon.Right, rectIcon.Y);
                graph.DrawPath(pen, path);


            }
        }




        //-> Item Actions
        private void Icon_Click(object sender, EventArgs e)
        {
            //Open dropdown list
            cmbList.Select();
            cmbList.DroppedDown = true;
        }
        private void Surface_Click(object sender, EventArgs e)
        {
            //Selected combo box
            cmbList.Select();
            if (cmbList.DropDownStyle == ComboBoxStyle.DropDownList)
            {
                cmbList.DroppedDown = true; // Open dropdown list 
            }
        }

        private void ComboBox_TextChanged(object sender, EventArgs e)
        {
            //Refresh text
            lblText.Text = cmbList.Text;
        }


    }
}
