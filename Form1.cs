using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;


namespace Excel_Reader
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btn_read_Click(object sender, EventArgs e)
        {
            readExcel();
        }

        private void readExcel()
        {
            string filePath = "";
            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            Workbook wb;
            Worksheet ws;

            wb = excel.Workbook.Open(filePath);
            ws = wb.Worksheets[1];

            //Object cell = ws.Cells[1, 1];
            //Range cell = ws.Cells[1, 1];
            //Range cell = ws.Range["A1"];
            //string CellValue = cell.Value;


            MessageBox.Show(CellValue)

        }



    }
}