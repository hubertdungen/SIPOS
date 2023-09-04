using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
//using System.Windows.Controls;
using System.Windows.Forms;

namespace SIPOS.Forms
{
    public partial class FormModelar : Form
    {



        //// UNIVERSAL SORTABLE-LIST VARIABLES //
        // -------------------------------------

        private static int rowCount = 0;  // Static variable to count the rows

        Control initialRow;
        bool dragging;
        int xoffset;
        int yoffset;
        int tickCount = 0;

        int rowIndex;
        float floatI;
        int rowNumber = -1;
        int rowMarginH = 14;
        readonly int rowPadBottom = 5; // margin between rows
        static int rh = 0;  // row height
        int totalRows = 0;
        int lineAnimation = 0;

        Panel originalRow;
        Panel separator;

        // ---------------///----------------- //




        // FORM LOAD
        public FormModelar()
        {
            InitializeComponent();
            RepositionRows();



            Panel initialRow = CloneRow(rowPanel_WordDoc);
            // Add the cloned row to the main panel
            mainWordFlowPanel.Controls.Add(initialRow);

            // Remove the template row
            mainWordFlowPanel.Controls.Remove(rowPanel_WordDoc);

            // Turn off drag sensor
            dragging = false;

            //originalRow.Enabled = false;



            // Set initial size and position
            ResizeBottomPanel();

            // Register the Resize event
            this.Resize += new EventHandler(FormModelar_Resize);



        }

        private void FormModelar_Load(object sender, EventArgs e)
        {
            RefreshListLayout();
        }




        ///////*                                 *\\\\\\\
        //////*    SORTABLE DYNAMIC LIST LOGIC    *\\\\\\ 



        //// DRAG AND DROP & MOUSE CONTROL     //
        // -------------------------------------

        private void elli_MouseDown(object sender, MouseEventArgs e)
        {
            Control c;
            c = (Control)sender;
            Control parentC = c.Parent;

            mainWordFlowPanel.SuspendLayout();
            //c.Dock = DockStyle.None;


            separator = new Panel();
            separator.BackColor = pnl_Separator.BackColor;
            separator.BorderStyle = BorderStyle.None;
            separator.Size = pnl_Separator.Size;
            int newWidth = (int)(mainWordFlowPanel.Size.Width / 1.5);
            int newHeight = separator.Size.Height;  // Or whatever value you want to set
            separator.Size = new Size(newWidth, newHeight);

            int? idxMouse = 0;
            if (GetIndexOfMouseLocation(e) != null) { idxMouse = GetIndexOfMouseLocation(e); };

            idxMouse = GetFloatIndexConvertedToInt((int)idxMouse);

            if (idxMouse.HasValue)
            {

                separator.Location = new Point(
                    mainWordFlowPanel.Size.Width / 2 - separator.Size.Width / 2,
                    mainWordFlowPanel.Top + (idxMouse.Value * rh) + Convert.ToInt32(separator.Height * 1));
            }

            //char randomLetter = (char)rnd.Next('A', 'Z' + 1);
            //separator.Name = pnl_Separator.Name + randomLetter;


            // Add separator as a child to parentC
            Controls.Add(separator);
            separator.BringToFront();




            // Calculate offsets relative to the parent panel
            if (parentC != null)
            {



                //Controls.SetChildIndex(separator, 1); // Smaller index = closer to the front
                //mainWordFlowPanel.Controls.SetChildIndex(parentC, 0); // Smaller index = closer to the front
                //Controls.SetChildIndex(mainWordFlowPanel, 2); // Smaller index = closer to the front

                parentC.BringToFront();

                xoffset = e.X + c.Left;
                yoffset = e.Y + c.Top;
            }
            else
            {

                xoffset = e.X;  // Here e.X and e.Y are positions within Button
                yoffset = e.Y;
            }



            dragging = true;
        }

        private void elli_MouseMove(object sender, MouseEventArgs e)
        {
            Control c;
            c = (Control)sender;
            Control parentC = c.Parent;


            // Position change of mouse pointer (relative to row coordinates) 
            int XMoved;
            int YMoved;
            // Calculated position change of mouse pointer (relative to Form coordinates)
            int newRowX;
            int newRowY;


            if (dragging)
            {
                // calculate mouse pointer movement
                XMoved = e.Location.X - xoffset;
                YMoved = e.Location.Y - yoffset;

                // Calculate new position of row as its current pos plus
                // number of pixels that the mouse was moved so that the
                // pointer offset is retained relative to the row
                newRowX = parentC.Location.X + XMoved;
                newRowY = parentC.Location.Y + YMoved;


                parentC.BringToFront();

                // Move Row
                parentC.Location = new Point(newRowX, newRowY);


                // Draw Splitter when over other rows
                int? idx;
                idx = GetIndexOfOverlappedRow(c);


                Debug.WriteLine($"Index of TargetI: Index: {idx}, Name: {c.Name}");



                //idx--;

                float idxFloat = 0;

                if (idx != null)
                {
                    idxFloat = (float)idx;
                    idxFloat = idxFloat - 0.5f;
                }
                else
                {
                    idxFloat = -1;
                }


                Debug.WriteLine($"The idx value is: {idx}");
                Debug.WriteLine($"The idxFloat value is: {idxFloat}");

                // Update the position of the splitterIndicator based on idx
                if (floatI == -1)
                {
                    foreach (Control ctrl in mainWordFlowPanel.Controls)
                    {
                        if (ctrl is Panel && ctrl.BackColor == pnl_Separator.BackColor)
                        {
                            mainWordFlowPanel.Controls.Remove(ctrl);
                            break;
                        }
                    }
                    return;
                }
                else if (floatI < 0.5)
                {
                    separator.Location = new Point(mainWordFlowPanel.Size.Width / 2 - separator.Size.Width / 2, mainWordFlowPanel.Top + Convert.ToInt32(separator.Height * 1));
                    Debug.WriteLine($"IdxFloat right now under 0.5");
                }
                else if (floatI >= 0.5 && floatI < 1)
                {
                    separator.Location = new Point(mainWordFlowPanel.Size.Width / 2 - separator.Size.Width / 2, mainWordFlowPanel.Top + (rh) + (separator.Height));
                    Debug.WriteLine($"IdxFloat right now between 0.5 and 1");
                }
                else if (floatI < mainWordFlowPanel.Controls.Count - 1 && floatI >= 2)
                {
                    separator.Location = new Point(mainWordFlowPanel.Size.Width / 2 - separator.Size.Width / 2, mainWordFlowPanel.Top + Convert.ToInt32((idx * rh) + rh) + (separator.Height));
                    Debug.WriteLine($"IdxFloat right now between 1 and bellow the total number of rows");
                }
                else if (floatI >= mainWordFlowPanel.Controls.Count - 1)
                {
                    separator.Location = new Point(mainWordFlowPanel.Size.Width / 2 - separator.Size.Width / 2, mainWordFlowPanel.Top + ((mainWordFlowPanel.Controls.Count) * rh - rh) + (separator.Height));
                    Debug.WriteLine($"IdxFloat right now above the number of rows");
                }

                if (floatI != -1)
                {
                    separator.Visible = true;
                }

                // Remove separator from parentC if it exists
                foreach (Control ctrl in mainWordFlowPanel.Controls)
                {
                    if (ctrl is Panel && ctrl.BackColor == pnl_Separator.BackColor)
                    {
                        mainWordFlowPanel.Controls.Remove(ctrl);
                        break;
                    }
                }


                if (idx != null)
                {
                    Debug.WriteLine($"Index of TargetI: IdxValue: {idx.Value}, rh: {rh}, Idx * rh:{idx.Value * rh}, mainWordFlowPanel Top value: {mainWordFlowPanel.Top}");

                }


            }

        }

        private void elli_MouseUp(object sender, MouseEventArgs e)
        {
            Control c;
            int? idx;  // Change to nullable int
            c = (Control)sender;
            Control parentC = c.Parent;
            dragging = false;

            idx = GetIndexOfOverlappedRow(c);
            if (idx.HasValue && idx.Value != -1)  // Check if idx has a value and if it's not -1
            {
                mainWordFlowPanel.Controls.SetChildIndex(parentC, idx.Value);
            }

            // Hide the splitterIndicator
            // Remove separator from parentC if it exists
            foreach (Control ctrl in Controls)
            {
                if (ctrl is Panel && ctrl.BackColor == pnl_Separator.BackColor)
                {
                    Controls.Remove(ctrl);
                    break;
                }
            }

            foreach (Control ctrl in mainWordFlowPanel.Controls)
            {
                if (ctrl is Panel && ctrl.BackColor == pnl_Separator.BackColor)
                {
                    mainWordFlowPanel.Controls.Remove(ctrl);
                    break;
                }
            }

            foreach (Panel row in mainWordFlowPanel.Controls)
            {
                UpdateButtonVisibility(row);
            }

            mainWordFlowPanel.ResumeLayout();
        }

        private void lbl_ellipse_MouseLeave(object sender, EventArgs e)
        {

            foreach (Control ctrl in mainWordFlowPanel.Controls)
            {
                if (ctrl is Panel && ctrl.BackColor == pnl_Separator.BackColor)
                {
                    mainWordFlowPanel.Controls.Remove(ctrl);
                    break;
                }
            }

            foreach (Panel row in mainWordFlowPanel.Controls)
            {
                UpdateButtonVisibility(row);
            }

            mainWordFlowPanel.ResumeLayout();
        }


        // FIND THE INDEX OF OVERLAPPING ROWS
        private int? GetIndexOfOverlappedRow(Control c)
        {
            Control parentC = ((Control)c).Parent;

            Debug.WriteLine($"Overlapped of \"parentC\": Type: {parentC.GetType().Name}, Name: {parentC.Name}");
            Debug.WriteLine($"Overlapped of \"c\": Type: {c.GetType().Name}, Name: {c.Name}");

            // Calculate the parameters needed for the new equation
            float tp = parentC.Location.Y; // assuming Y-coordinate represents the vertical position
            rh = parentC.Height + parentC.Margin.Vertical; // assuming all rows have the same height
            int ti = mainWordFlowPanel.Controls.Count; // total number of rows
            int ci;
            try
            {
                ci = mainWordFlowPanel.Controls.GetChildIndex(parentC); // current index of the dragged row
            }
            catch
            {
                ci = mainWordFlowPanel.Controls.GetChildIndex(c); // current index of the dragged row
            }

            float offset = mainWordFlowPanel.Location.Y; // Y-coordinate where rows start

            // Call the new method to get the target index
            int? targetIndex = GetIndexFromPixelHeight(tp, rh, ti, ci, offset);


            return targetIndex;
        }

        private int? GetIndexOfMouseLocation(MouseEventArgs e)
        {

            // Calculate the parameters needed for the new equation
            float tp = e.Y;
            rh = rowPanel_WordDoc.Height + rowPanel_WordDoc.Margin.Vertical; // assuming all rows have the same height
            int ti = mainWordFlowPanel.Controls.Count; // total number of rows
            int ci = e.Y / rh;

            float offset = mainWordFlowPanel.Location.Y; // Y-coordinate where rows start

            // Call the new method to get the target index
            int? targetIndex = GetIndexFromPixelHeight(tp, rh, ti, ci, offset);


            return targetIndex;
        }

        public int? GetIndexFromPixelHeight(float tp, float rh, int ti, int ci, float offset)
        {
            int i;

            //tp -= offset; // adjust tp for the offset

            // If tp is less than or equal to half the row height, row is moved to the top
            if (tp <= rh / 2)
            {
                i = 0;
                floatI = 0;
            }
            // If tp is greater than or equal to half the height of the last row, row is moved to the bottom
            else if (tp >= rh * ti - rh / 2)
            {
                i = ti + 1; // To account for the "fictional" index
                floatI = ti + 1;
            }
            // For any other value of tp, calculate the index based on the row height
            else
            {
                i = (int)Math.Ceiling(tp / rh);
                floatI = tp / rh;
            }

            // Check if the row is being moved to the same position, or to the immediate next position
            if (ci == i || ci == i + 1)
            {
                return null; // Ignore / Return
                floatI = -1;
            }

            // If the row is being moved to a position with an index greater than ci + 1, subtract 1 from the result
            if (i > ci + 1)
            {
                i -= 1;
                floatI -= i - 1;
            }

            return i;
        }

        public int? GetFloatIndexConvertedToInt(int idx)
        {
            int convertedIndex;
            // Update the position of the splitterIndicator based on idx
            if (floatI == -1)
            {
                foreach (Control ctrl in mainWordFlowPanel.Controls)
                {
                    if (ctrl is Panel && ctrl.BackColor == pnl_Separator.BackColor)
                    {
                        mainWordFlowPanel.Controls.Remove(ctrl);
                        break;
                    }
                }

                return null;
            }
            else if (floatI < 0.5)
            {
                convertedIndex = mainWordFlowPanel.Top + Convert.ToInt32(separator.Height * 1);
                return convertedIndex;
                Debug.WriteLine($"ConvertedIdxToInt right now under 0.5");
            }
            else if (floatI >= 0.5 && floatI < 1)
            {
                convertedIndex = mainWordFlowPanel.Top + (rh) + (separator.Height);
                return convertedIndex;
                Debug.WriteLine($"ConvertedIdxToInt right now between 0.5 and 1");
            }
            else if (floatI < mainWordFlowPanel.Controls.Count && floatI >= 2)
            {
                convertedIndex = mainWordFlowPanel.Top + Convert.ToInt32((idx * rh) + rh) + (separator.Height);
                return convertedIndex;
                Debug.WriteLine($"ConvertedIdxToInt right now between 1 and bellow the total number of rows");
            }
            else if (floatI >= mainWordFlowPanel.Controls.Count)
            {
                convertedIndex = mainWordFlowPanel.Top + ((mainWordFlowPanel.Controls.Count) * rh) + (separator.Height);
                return convertedIndex;
                Debug.WriteLine($"ConvertedIdxToInt right now above the number of rows");
            }
            else
            {
                return null;
            }


        }

        private void frmModelarTimer_Tick(object sender, EventArgs e)
        {

            if (tickCount >= 100)
            {

                if (!dragging)
                {
                    RefreshListLayout();
                }

                tickCount = 0;
            }
            else
            {
                //Debug.WriteLine($"Timer ticked.{tickCount}");
                tickCount++;
            }

        }

        // ---------------///----------------- //





        //// SORTABLE LIST BUTTONS             //
        // -------------------------------------

        // IF PLUS CLICK
        private void btnWPlus_Click(object sender, EventArgs e)
        {
            // Get the current row and main panel
            Control currentRow = ((Control)sender).Parent;
            AddRow_Click(currentRow, e);
        }

        // IF MINUS CLICK
        private void btnWMinus_Click(object sender, EventArgs e)
        {
            // Remove the current row from the main FlowLayoutPanel
            Control currentRow = ((Control)sender).Parent;
            RemoveRow_Click(currentRow, e);
        }

        // IF OPEN FILE CLICK
        private void btnOpenWFile_Click(object sender, EventArgs e)
        {
            Control currentRow = ((Control)sender);
            Control parentRow = currentRow.Parent;
            Mediator.openFile();


            Debug.WriteLine($"Current Row Type: {currentRow.GetType().Name}, CurrentRow Name: {currentRow.Name}");


            foreach (Control control in currentRow.Controls)
            {
                Debug.WriteLine($"Control Type: {control.GetType().Name}, Control Name: {control.Name}");
            }

            foreach (Control control in currentRow.Controls)
            {
                if (control is TextBox txtBox)  // Check if the control is a TextBox
                {
                    if (txtBox.Name.Contains("txtDirFicheiroW") || txtBox.PlaceholderText.Contains("Caminho para o ficheiro word"))
                    {
                        txtBox.Text = Mediator.filePath;

                        Debug.WriteLine("Current Row: " + currentRow.Name);
                        Debug.WriteLine("Mediator.filePath: " + Mediator.filePath);
                    }
                }
            }

        }

        // ACTIVE CHCK LIST BUTTON CLICK
        private void btnChkWRowActive_Click(object sender, EventArgs e)
        {
            Control ctrl = sender as Control;

            Debug.WriteLine($"Sender Type: {sender.GetType().Name} / Sender Name: {ctrl.Name}");

            Button chkButton = sender as Button;  // Safely cast sender to Button
            if (chkButton == null)
            {
                Debug.WriteLine("Sender is not a button");
                return;
            }

            bool wasActive = chkButton.Text == "✓";
            Debug.WriteLine($"Was Active: {wasActive}");

            if (wasActive)
            {
                chkButton.Text = " ";
            }
            else
            {
                chkButton.Text = "✓";
            }

            // Force the button to repaint
            chkButton.Refresh();

            Debug.WriteLine($"The button CheckRowActive was clicked and it has a text: {chkButton.Text}. This button is named: {chkButton.Name}.");

            SwitchRowActivationState(chkButton, !wasActive);
        }

        // ---------------///----------------- //





        //// LIST LOGIC                        //
        // -------------------------------------

        private void SwitchRowActivationState(Control chkButtonState, bool isActive)
        {


        }


        // ---------------///----------------- //









        //// CLONING AND REMOVING ROWS LOGIC   //
        // -------------------------------------


        ///-> ADDING AND REMOVING <-///
        ///
        // ADD ROW ACTIONS
        private void AddRow_Click(Control currentRow, EventArgs e)
        {

            // Get the current row and main panel
            Control mainPanel = currentRow.Parent;


            // Clone the current row
            Panel newRow = CloneRow((Panel)currentRow);

            // Find the index of the current row in the main mainPanel
            int currentIndex = mainPanel.Controls.GetChildIndex(currentRow);

            // Add the new row to the main FlowLayoutPanel right below the current one
            mainPanel.Controls.Add(newRow);
            mainPanel.Controls.SetChildIndex(newRow, currentIndex + 1);
            //mainPanel.SetFlowBreak(newRow, true);  // Add this line

            UpdateButtonVisibility(originalRow);

        }

        // REMOVE ROW ACTIONS
        private void RemoveRow_Click(Control currentRow, EventArgs e)
        {
            // Remove the current row from the main FlowLayoutPanel
            Control mainPanel = currentRow.Parent;
            try
            {
                mainPanel.Controls.Remove(currentRow);
                UpdateButtonVisibility(originalRow);
                RepositionRows();
                rowCount--;
            }
            catch
            {

            }

        }




        ///-> ROWS LOGIC <-///
        ///
        // CLONE / COPY-PASTE IDENTITY OF ROWS 
        private Panel CloneRow(Panel originalRow)
        {
            Panel newRow = new Panel();


            // Generate a new unique name for the row
            rowCount++;  // Increment the row counter
            Random rnd = new Random();
            char randomLetter = (char)rnd.Next('A', 'Z' + 1);
            newRow.Name = originalRow.Name + rowCount.ToString() + randomLetter;
            string newRowName = newRow.Name;


            Debug.WriteLine($"Created new row: Type: {newRow.GetType().Name}, Name: {newRow.Name}");



            // Copy common properties
            Padding newMargin = new Padding(originalRow.Margin.Left, rowMarginH / 2, originalRow.Margin.Right, rowMarginH / 2);


            newRow.Size = originalRow.Size;
            newRow.BackColor = originalRow.BackColor;
            newRow.Padding = originalRow.Padding;
            newRow.Margin = newMargin;
            newRow.AutoSize = originalRow.AutoSize;
            newRow.AutoSizeMode = originalRow.AutoSizeMode;
            newRow.Anchor = originalRow.Anchor;
            newRow.Location = new Point(originalRow.Location.X, originalRow.Location.Y + originalRow.Height + originalRow.Padding.Bottom + rowPadBottom);

            // Clone only the top-level controls in the originalRow
            CloneControls(originalRow, newRow, newRowName);

            Debug.WriteLine($"Total controls inside new row: {newRow.Controls.Count}");
            UpdateButtonVisibility(originalRow);

            return newRow;
        }

        private void CloneControls(Control original, Control clone, string rowName)
        {
            foreach (Control control in original.Controls)
            {
                Control newControl = null;




                if (control is Label originalLabel)
                {
                    newControl = new Label();
                    ((Label)newControl).Text = originalLabel.Text;
                    ((Label)newControl).Dock = originalLabel.Dock;
                    ((Label)newControl).Anchor = originalLabel.Anchor;
                    ((Label)newControl).Font = originalLabel.Font;
                    ((Label)newControl).BackColor = originalLabel.BackColor;
                    ((Label)newControl).ForeColor = originalLabel.ForeColor;
                    ((Label)newControl).Cursor = originalLabel.Cursor;

                    if (originalLabel.Name.Contains("lbl_ellipse") || originalLabel.Text.Contains("⋯"))
                    {
                        newControl.MouseDown += (sender, e) => elli_MouseDown(newControl, e);
                        newControl.MouseMove += (sender, e) => elli_MouseMove(newControl, e);
                        newControl.MouseUp += (sender, e) => elli_MouseUp(newControl, e);
                    }

                }
                else if (control is TextBox originalTextBox)
                {
                    newControl = new TextBox();
                    ((TextBox)newControl).BorderStyle = originalTextBox.BorderStyle;
                    ((TextBox)newControl).Text = "";
                    ((TextBox)newControl).PlaceholderText = originalTextBox.PlaceholderText;
                    ((TextBox)newControl).Font = originalTextBox.Font;
                    ((TextBox)newControl).BackColor = originalTextBox.BackColor;
                    ((TextBox)newControl).ForeColor = originalTextBox.ForeColor;
                }
                else if (control is Button originalButton)
                {
                    newControl = new Button();
                    ((Button)newControl).Text = originalButton.Text;
                    ((Button)newControl).BackColor = originalButton.BackColor;
                    ((Button)newControl).ForeColor = originalButton.ForeColor;
                    ((Button)newControl).FlatStyle = originalButton.FlatStyle;
                    ((Button)newControl).Font = originalButton.Font;
                    ((Button)newControl).Dock = originalButton.Dock;


                    if (originalButton.Name.Contains("btnWPlus") || originalButton.Text.Contains("➕"))
                    {
                        newControl.Click += (sender, e) => AddRow_Click(clone, e);
                    }
                    else if (originalButton.Name.Contains("btnWMinus") || originalButton.Text.Contains("➖"))
                    {
                        newControl.Click += (sender, e) => RemoveRow_Click(clone, e);
                    }
                    else if (originalButton.Name.Contains("btnOpenWFile") || originalButton.Text.Contains("📄"))
                    {
                        newControl.Click += (sender, e) => btnOpenWFile_Click(clone, e);
                    }
                    else if (originalButton.Name.Contains("btnChkWRowActive") || originalButton.Text.Contains("✓"))
                    {
                        newControl.Click += (sender, e) => btnChkWRowActive_Click(newControl, e);
                    }



                }
                else if (control is Panel originalPanel)
                {
                    newControl = new Panel();
                    ((Panel)newControl).BackColor = originalPanel.BackColor;
                    ((Panel)newControl).ForeColor = originalPanel.ForeColor;
                    ((Panel)newControl).Font = originalPanel.Font;
                }
                else if (control is Control originalControl)
                {
                    newControl = new Control();
                    newControl.Text = originalControl.Text;
                    newControl.BackColor = originalControl.BackColor;
                    newControl.ForeColor = originalControl.ForeColor;
                    newControl.Font = originalControl.Font;
                }

                if (newControl != null)
                {
                    newControl.Size = control.Size;
                    newControl.Location = control.Location;
                    newControl.Padding = control.Padding;
                    newControl.Margin = control.Margin;
                    newControl.Dock = control.Dock;
                    newControl.Anchor = control.Anchor;
                    newControl.Name = control.Name + rowName;
                    clone.Controls.Add(newControl);

                    // Clone events
                    CloneEvents(control, newControl);

                    // Recursive call to handle nested controls
                    CloneControls(control, newControl, control.Name + rowName);

                    Debug.WriteLine($"Created new control inside row: Type: {newControl.GetType().Name}, Name: {newControl.Name}");
                }
            }
        }

        private void CopyProperties(Control destination, Control source)
        {
            foreach (PropertyInfo property in source.GetType().GetProperties())
            {
                if (property.CanRead && property.CanWrite)
                {
                    try
                    {
                        property.SetValue(destination, property.GetValue(source, null), null);
                    }
                    catch (Exception ex)
                    {
                        // Handle any exceptions that occur during property copying.
                        // This can happen if some properties have special requirements
                        // for setting their values.
                        Console.WriteLine($"Could not copy property {property.Name}: {ex.Message}");
                    }
                }
            }
        }

        private void CloneEvents(Control original, Control clone)
        {
            foreach (EventInfo ev in original.GetType().GetEvents())
            {
                FieldInfo fieldInfo = (typeof(Control).GetField(ev.Name, BindingFlags.Instance | BindingFlags.NonPublic) ?? original.GetType().GetField(ev.Name, BindingFlags.Instance | BindingFlags.NonPublic));
                if (fieldInfo != null)
                {
                    Delegate del = fieldInfo.GetValue(original) as Delegate;
                    if (del != null)
                    {
                        foreach (Delegate handler in del.GetInvocationList())
                        {
                            ev.AddEventHandler(clone, handler);
                            Debug.WriteLine($"Cloned event {ev.Name} for control {original.Name} to {clone.Name}");
                        }
                    }
                }
            }
        }




        //// SORTABLE LIST FORM CONTROL        //
        // -------------------------------------

        // FORM RESIZE
        private void mainWordFlowPanel_Layout(object sender, LayoutEventArgs e)
        {
            mainWordFlowPanel.SuspendLayout();
            foreach (Control ctrl in mainWordFlowPanel.Controls)
            {
                if (ctrl is Panel && ctrl.Name.Contains("rowPanel_WordDoc")) ctrl.Width = mainWordFlowPanel.Width - ctrl.Padding.Horizontal - ctrl.Margin.Horizontal;
            }
            mainWordFlowPanel.ResumeLayout();
            ResizeBottomPanel();
        }

        private void FormModelar_Resize(object sender, EventArgs e)
        {
            ResizeBottomPanel();
        }

        private void mainWordFlowPanel_Resize(object sender, EventArgs e)
        {
            ResizeBottomPanel();
        }

        private void ResizeBottomPanel()
        {
            // Set the panel to be 70% of the window height

            int topPanelsHeigh = panelMenu.Size.Height + panelWordMenuSelector.Size.Height;
            int heightBelowTopPanels = this.ClientSize.Height - topPanelsHeigh;

            if (mainWordFlowPanel.Height < heightBelowTopPanels)
            {
                mainWordFlowPanel.MinimumSize = new Size(mainWordFlowPanel.MinimumSize.Width, (int)(this.ClientSize.Height * 0.7));
            }
            else
            {
                mainWordFlowPanel.MinimumSize = new Size(mainWordFlowPanel.MinimumSize.Width, (int)heightBelowTopPanels);
            }


            mainWordFlowPanel.Width = this.ClientSize.Width;

            // Position the panel at the bottom
            mainWordFlowPanel.Top = this.ClientSize.Height - mainWordFlowPanel.Height;
            mainWordFlowPanel.Left = 0;

            RefreshListLayout();
        }

        private void RefreshListLayout()
        {
            forEachUpdateButtonVisibility();

            foreach (Control row in mainWordFlowPanel.Controls)
            {
                if (row is Panel && row.Name.Contains("rowPanel_WordDoc"))
                    row.Size = new Size(mainWordFlowPanel.Size.Width - row.Margin.Horizontal, row.Height); 
            }
        }

        // ---------------///----------------- //




        ///-> REFRESH <-///
        ///
        // REFRESH "+" AND "-" BTN VISIBILITY       ---> conditions depend on indexes
        private void forEachUpdateButtonVisibility()
        {
            foreach (Panel row in mainWordFlowPanel.Controls)
            {
                UpdateButtonVisibility(row);
            }
        }

        private void UpdateButtonVisibility(Panel originalRow)
        {
            Debug.WriteLine("Number of rows (Before UpdateBtnVisb): " + mainWordFlowPanel.Controls.Count);

            Debug.WriteLine("Controls inside mainWordFlowPanel:");
            foreach (Control ctrl in mainWordFlowPanel.Controls)
            {
                Debug.WriteLine($"Type: {ctrl.GetType().Name}, Name: {ctrl.Name}");
            }

            for (int i = 0; i < mainWordFlowPanel.Controls.Count; i++)
            {
                Panel row = (Panel)mainWordFlowPanel.Controls[i];
                if (row == originalRow && originalRow != null) continue;

                Button plusButton = GetPlusButton(row);

                // Check if there is another row below the current one
                bool hasRowBelow = i < mainWordFlowPanel.Controls.Count - 1;

                // If there is another row below, hide the '+' button, otherwise show it
                plusButton.Visible = !hasRowBelow;
            }

            // If there is only one row, hide its '-' button
            if (mainWordFlowPanel.Controls.Count == 1)
            {
                Panel lastRow = (Panel)mainWordFlowPanel.Controls[0];
                Button minusButton = GetMinusButton(lastRow);
                minusButton.Visible = false;
            }
            else if (mainWordFlowPanel.Controls.Count > 1)
            {
                Panel lastRow = (Panel)mainWordFlowPanel.Controls[0];
                Button minusButton = GetMinusButton(lastRow);
                minusButton.Visible = true;
            }

            Debug.WriteLine("Number of rows (After UpdateBtnVisb): " + mainWordFlowPanel.Controls.Count);
        }


        // DETECT IF ROW CONTAINS A "+" BUTTON
        private Button GetPlusButton(Panel row)
        {
            foreach (Control control in row.Controls)
            {
                if (control is Button button)
                {
                    if (button.Name.Contains("btnWPlus") || button.Text.Contains("➕"))
                    {
                        return button;
                    }

                }
            }

            return null; // Return null if no '+' button is found
        }

        // DETECT IF ROW CONTAINS A "-" BUTTON
        private Button GetMinusButton(Panel row)
        {
            foreach (Control control in row.Controls)
            {
                if (control is Button button)
                {
                    if (button.Name.Contains("btnWMinus") || button.Text.Contains("➖"))
                    {
                        return button;
                    }

                }
            }

            return null; // Return null if no '-' button is found
        }

        // REFRESH ROWS PLACE & POSITION
        private void RepositionRows()
        {
            int y = 0;

            //foreach (Control row in mainWordFlowPanel.Controls)
            //{
            //    row.Location = new Point(row.Location.X, y);
            //    y += row.Height + row.Padding.Bottom + rowPadBottom; // Update y for the next row
            //}
        }


        // ---------------///----------------- //

    }
}
