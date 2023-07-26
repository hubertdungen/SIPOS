using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SIPOS.Forms
{
    public partial class FormModelar : Form
    {
        public FormModelar()
        {
            InitializeComponent();
            RepositionRows();
        }

        private void btn_plus_Click(object sender, EventArgs e)
        {
            // Get the current row and main panel
            Control currentRow = ((Control)sender).Parent;
            Control mainPanel = currentRow.Parent;

            // Clone the current row
            FlowLayoutPanel newRow = CloneRow((FlowLayoutPanel)currentRow);

            // Find the index of the current row in the main FlowLayoutPanel
            int currentIndex = mainPanel.Controls.GetChildIndex(currentRow);

            // Add the new row to the main FlowLayoutPanel right below the current one
            mainPanel.Controls.Add(newRow);
            mainPanel.Controls.SetChildIndex(newRow, currentIndex + 1);
            //mainPanel.SetFlowBreak(newRow, true);  // Add this line
            UpdateButtonVisibility();
        }

        private void btn_minus_Click(object sender, EventArgs e)
        {
            // Remove the current row from the main FlowLayoutPanel
            Control currentRow = ((Control)sender).Parent;
            Control mainPanel = currentRow.Parent;
            mainPanel.Controls.Remove(currentRow);
            UpdateButtonVisibility();
            RepositionRows();
        }



        private FlowLayoutPanel CreateNewRow()
        {
            FlowLayoutPanel newRow = new FlowLayoutPanel();
            // Set the properties of newRow as needed, such as Size and BackColor

            TextBox txtPath = new TextBox();
            // Set the properties of txtPath as needed
            newRow.Controls.Add(txtPath);

            Button btn_plus = new Button();
            btn_plus.Text = "+";
            btn_plus.Click += btn_plus_Click; // The '+' button handler we defined earlier
            newRow.Controls.Add(btn_plus);

            Button btn_minus = new Button();
            btn_minus.Text = "-";
            btn_minus.Click += btn_minus_Click; // The '-' button handler we defined earlier
            newRow.Controls.Add(btn_minus);

            return newRow;
        }


        private FlowLayoutPanel CloneRow(FlowLayoutPanel originalRow)
        {
            FlowLayoutPanel newRow = new FlowLayoutPanel();

            // Copy properties from the originalRow to newRow
            newRow.Size = originalRow.Size;
            newRow.BackColor = originalRow.BackColor;
            newRow.FlowDirection = originalRow.FlowDirection;
            newRow.WrapContents = originalRow.WrapContents;
            newRow.Padding = originalRow.Padding;
            newRow.Margin = originalRow.Margin;

            newRow.Location = new Point(originalRow.Location.X, originalRow.Location.Y + originalRow.Height + originalRow.Padding.Bottom + 2);

            foreach (Control control in originalRow.Controls)
            {
                if (control is Label originalLabel)
                {
                    Label newLabel = new Label();
                    newLabel.Text = originalLabel.Text;
                    newLabel.Size = originalLabel.Size;
                    newLabel.Font = originalLabel.Font;
                    newLabel.ForeColor = originalLabel.ForeColor;
                    newLabel.Location = originalLabel.Location;
                    newLabel.Padding = originalLabel.Padding;
                    newLabel.Margin = originalLabel.Margin;
                    newRow.Controls.Add(newLabel);
                }
                else if (control is TextBox originalTextBox)
                {
                    TextBox newTextBox = new TextBox();
                    newTextBox.Text = originalTextBox.Text;
                    newTextBox.Size = originalTextBox.Size;
                    newTextBox.Location = originalTextBox.Location;
                    newTextBox.PlaceholderText = originalTextBox.PlaceholderText;
                    newTextBox.Padding = originalTextBox.Padding;
                    newTextBox.Margin = originalTextBox.Margin;
                    newRow.Controls.Add(newTextBox);
                }
                else if (control is Button originalButton)
                {
                    Button newButton = new Button();
                    newButton.Text = originalButton.Text;
                    newButton.Size = originalButton.Size;
                    newButton.Location = originalButton.Location;
                    newButton.BackColor = originalButton.BackColor;
                    newButton.ForeColor = originalButton.ForeColor;
                    newButton.FlatStyle = originalButton.FlatStyle;
                    newButton.Font = originalButton.Font;
                    newButton.Padding = originalButton.Padding;
                    newButton.Margin = originalButton.Margin;

                    if (newButton.Text == "+")
                    {
                        newButton.Text = "+";
                        newButton.Click += (sender, e) => AddRow_Click(newRow, e);
                    }
                    else if (newButton.Text == "-")
                    {
                        newButton.Text = "-";
                        newButton.Click += (sender, e) => RemoveRow_Click(newRow, e);
                    }
                    newRow.Controls.Add(newButton);
                }
            }

            UpdateButtonVisibility();
            return newRow;
        }


        private void AddRow_Click(FlowLayoutPanel currentRow, EventArgs e)
        {
            //// Create a new row
            //FlowLayoutPanel newRow = CreateNewRow();

            //// Find the index of the current row in the main FlowLayoutPanel
            //Control mainPanel = currentRow.Parent;
            //int currentIndex = mainPanel.Controls.GetChildIndex(currentRow);

            //// Add the new row to the main FlowLayoutPanel right below the current one
            //mainPanel.Controls.Add(newRow);
            //mainPanel.Controls.SetChildIndex(newRow, currentIndex + 1);


            // Get the current row and main panel
            Control mainPanel = currentRow.Parent;


            // Clone the current row
            FlowLayoutPanel newRow = CloneRow((FlowLayoutPanel)currentRow);

            // Find the index of the current row in the main FlowLayoutPanel
            int currentIndex = mainPanel.Controls.GetChildIndex(currentRow);

            // Add the new row to the main FlowLayoutPanel right below the current one
            mainPanel.Controls.Add(newRow);
            mainPanel.Controls.SetChildIndex(newRow, currentIndex + 1);
            //mainPanel.SetFlowBreak(newRow, true);  // Add this line

            UpdateButtonVisibility();

        }

        private void RemoveRow_Click(FlowLayoutPanel currentRow, EventArgs e)
        {
            // Remove the current row from the main FlowLayoutPanel
            Control mainPanel = currentRow.Parent;
            mainPanel.Controls.Remove(currentRow);
            UpdateButtonVisibility();
            RepositionRows();
        }





        private void UpdateButtonVisibility()
        {
            Debug.WriteLine("Number of rows (Before UpdateBtnVisb): " + mainPanel.Controls.Count);

            for (int i = 0; i < mainPanel.Controls.Count; i++)
            {
                FlowLayoutPanel row = (FlowLayoutPanel)mainPanel.Controls[i];
                Button plusButton = GetPlusButton(row); // You need to implement this method

                // Check if there is another row below the current one
                bool hasRowBelow = i < mainPanel.Controls.Count - 1;

                // If there is another row below, hide the '+' button, otherwise show it
                plusButton.Visible = !hasRowBelow;
            }

            // If there is only one row left, hide its '-' button
            if (mainPanel.Controls.Count == 1)
            {
                FlowLayoutPanel lastRow = (FlowLayoutPanel)mainPanel.Controls[0];
                Button minusButton = GetMinusButton(lastRow); // You need to implement this method
                minusButton.Visible = false;
            }
            else
            {
                FlowLayoutPanel lastRow = (FlowLayoutPanel)mainPanel.Controls[0];
                Button minusButton = GetMinusButton(lastRow); // You need to implement this method
                minusButton.Visible = true;
            }

            Debug.WriteLine("Number of rows (After UpdateBtnVisb): " + mainPanel.Controls.Count);
        }

        private Button GetPlusButton(FlowLayoutPanel row)
        {
            foreach (Control control in row.Controls)
            {
                if (control is Button button && button.Text == "+")
                {
                    return button;
                }
            }

            return null; // Return null if no '+' button is found
        }

        private Button GetMinusButton(FlowLayoutPanel row)
        {
            foreach (Control control in row.Controls)
            {
                if (control is Button button && button.Text == "-")
                {
                    return button;
                }
            }

            return null; // Return null if no '-' button is found
        }


        private void RepositionRows()
        {
            int y = 0;

            foreach (Control row in mainPanel.Controls)
            {
                row.Location = new Point(row.Location.X, y);
                y += row.Height + row.Padding.Bottom + 2; // Update y for the next row
            }
        }


    }
}
