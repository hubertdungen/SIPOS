using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIPOS
{
    public class RowInfo
    {
        public int Index { get; set; }  // Index of the row
        public bool IsActive { get; set; }  // Active state of "btnChkWRowActive"
        public string Name { get; set; }  // Name present on "txtNameWBox"
        public string DirectoryPath { get; set; }  // Path directory of "txtDirFicheiroW"

        public RowInfo(int index, bool isActive, string name, string directoryPath)
        {
            Index = index;
            IsActive = isActive;
            Name = name;
            DirectoryPath = directoryPath;
        }
    }

    public class TemplateInfo
    {
        public string TemplateName { get; set; }  // Template name
        public List<RowInfo> Rows { get; set; }  // List of rows

        public TemplateInfo(string templateName)
        {
            TemplateName = templateName;
            Rows = new List<RowInfo>();
        }

        public void AddRow(RowInfo row)
        {
            Rows.Add(row);
        }
    }


}
