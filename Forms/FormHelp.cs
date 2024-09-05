namespace SIPOS.Forms
{
    public partial class FormHelp : Form
    {
        public FormHelp()
        {
            InitializeComponent();
            Mediator.formHelp = this;
        }

        private void FormHelp_Load(object sender, EventArgs e)
        {

        }


        public static void txtboxsActualizer()
        {
            FormHelp formHelp = Mediator.formHelp;
        }
    }
}
