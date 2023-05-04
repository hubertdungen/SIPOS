using Microsoft.Office.Interop.Word;
using SIPOS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
