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


        //// UNIVERSAL VARIABLES //
        // -------------------------------------

        bool isProgramMenu = true;

        // --------------///----------------- //





        //// SORTABLE-LIST VARIABLES //
        // -------------------------------------

        private static int rowCount = 0;  // Static variable to count the rows

        bool dragging;
        int xoffset;
        int yoffset;
        int tickCount = 0;

        float floatI;
        int rowMarginH = 14;
        readonly int rowPadBottom = 5; // margin between rows
        static int rh = 0;  // row height



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
            // B-1.2.4/B-1.2.6: o botão 📄 estava ancorado (não docked) e podia ficar
            // por baixo dos botões docked à direita; passa a docked como os restantes.
            btnOpenWFile.Dock = DockStyle.Right;

            AddOrderArrowButtons(rowPanel_WordDoc);
            AddTipoAcaoComboBox(rowPanel_WordDoc);
            AddProgramaButtons();
            CarregarProgramaParaLinhas();
            RefreshListLayout();
        }

        // ------------------------------------------------------------------
        // B-1.2.6: LIGAÇÃO DA LISTA AO PROGRAMA MODELAR (modelar_programa.json)
        // Cada linha é uma ação: o ComboBox escolhe o tipo, o nome/ficheiro/✓
        // preenchem o resto. As linhas são gravadas como filhos de um LoopDias
        // ("por cada dia selecionado"), o caso descrito no GUIA-MODELAR.
        // ------------------------------------------------------------------

        private static readonly TipoDeAcao[] tiposDeAcaoPorIndice =
        {
            TipoDeAcao.InserirDocumento,
            TipoDeAcao.LerEscalasDoDia,
            TipoDeAcao.SubstituirVariaveis,
            TipoDeAcao.QuebraDePagina
        };

        private static readonly string[] rotulosTiposDeAcao =
        {
            "Inserir documento",
            "Ler escalas do dia",
            "Substituir variáveis",
            "Quebra de página"
        };

        // B-1.2.3: usa o CustomComboBox do projeto (design custom com borda,
        // seta desenhada e dropdown estilizado) em vez do ComboBox nativo.
        private void AddTipoAcaoComboBox(Panel row)
        {
            var cmb = CreateTipoAcaoCustomCombo();
            row.Controls.Add(cmb);
        }

        private SIPOS.Controls.CustomComboBox CreateTipoAcaoCustomCombo()
        {
            var cmb = new SIPOS.Controls.CustomComboBox
            {
                Name = "cmbTipoAcao",
                MinimumSize = new Size(150, 30),
                Dock = DockStyle.Right,
                Width = 165,
                Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point),
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = Color.FromArgb(40, 30, 40),
                ForeColor = Color.Gainsboro,
                IconColor = Color.DeepSkyBlue,
                BorderColor = Color.FromArgb(79, 49, 79),
                BorderSize = 1,
                ListBackColor = Color.FromArgb(35, 26, 45),
                ListTextColor = Color.Gainsboro
            };
            cmb.Items.AddRange(rotulosTiposDeAcao);
            cmb.SelectedIndex = 0;
            return cmb;
        }

        private SIPOS.Controls.CustomComboBox GetRowTipoCombo(Control row)
        {
            return (row as Panel)?.Controls.OfType<SIPOS.Controls.CustomComboBox>().FirstOrDefault(cb => cb.Name.Contains("cmbTipoAcao"));
        }

        private Button GetRowChkButton(Control row)
        {
            return (row as Panel)?.Controls.OfType<Button>().FirstOrDefault(b => b.Name.Contains("btnChkWRowActive"));
        }

        private TextBox GetRowFileTextBox(Control row)
        {
            return (row as Panel)?.Controls.OfType<TextBox>().FirstOrDefault(tb => tb.Name.Contains("txtDirFicheiroW"));
        }

        private List<Panel> GetDocumentRows()
        {
            return mainWordFlowPanel.Controls.OfType<Panel>()
                .Where(p => p.Name.Contains("rowPanel_WordDoc"))
                .ToList();
        }

        private void AddProgramaButtons()
        {
            Button btnGuardar = CreateMenuButton("btnGuardarPrograma", "💾 Guardar Programa", 190, Color.MediumSpringGreen);
            btnGuardar.Click += (s, args) => GuardarPrograma();
            panelMenu.Controls.Add(btnGuardar);

            Button btnRecarregar = CreateMenuButton("btnRecarregarPrograma", "⭯ Recarregar", 130, Color.Gainsboro);
            btnRecarregar.Click += (s, args) => { CarregarProgramaParaLinhas(); RefreshListLayout(); };
            panelMenu.Controls.Add(btnRecarregar);
        }

        private Button CreateMenuButton(string name, string text, int width, Color foreColor)
        {
            return new Button
            {
                Name = name,
                Text = text,
                Dock = DockStyle.Right,
                Width = width,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Century Gothic", 10F, FontStyle.Regular, GraphicsUnit.Point),
                BackColor = Color.FromArgb(40, 30, 40),
                ForeColor = foreColor,
                UseVisualStyleBackColor = false
            };
        }

        // Constrói o programa a partir das linhas visíveis (pela ordem da lista)
        // e grava-o em modelar_programa.json ao lado do SIPOS.exe.
        private void GuardarPrograma()
        {
            var loop = new AcaoModelar
            {
                Tipo = TipoDeAcao.LoopDias,
                Nome = "Por cada dia selecionado",
                Filhos = new List<AcaoModelar>()
            };

            foreach (Panel row in GetDocumentRows())
            {
                var cmb = GetRowTipoCombo(row);
                TextBox nameBox = GetRowNameTextBox(row);
                TextBox fileBox = GetRowFileTextBox(row);
                Button chk = GetRowChkButton(row);

                int idx = Math.Max(0, cmb?.SelectedIndex ?? 0);
                loop.Filhos.Add(new AcaoModelar
                {
                    Tipo = tiposDeAcaoPorIndice[Math.Min(idx, tiposDeAcaoPorIndice.Length - 1)],
                    Nome = (nameBox?.Text ?? "").Trim(),
                    Ficheiro = (fileBox?.Text ?? "").Trim(),
                    Ativa = chk == null || chk.Text == "✓"
                });
            }

            var programa = new ProgramaModelar
            {
                Nome = "Programa Modelar",
                Acoes = new List<AcaoModelar> { loop }
            };

            List<string> problemas = programa.Validar();
            if (problemas.Count > 0)
            {
                MessageBox.Show("O programa não foi gravado porque tem problemas:\r\n\r\n- " + string.Join("\r\n- ", problemas),
                    "PROGRAMA INVÁLIDO!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                programa.Guardar(ProgramaModelar.CaminhoPorOmissao());
                MessageBox.Show($"Programa gravado em:\r\n{ProgramaModelar.CaminhoPorOmissao()}\r\n\r\nA próxima exportação Word será executada por este programa. Para voltar ao fluxo clássico, apague o ficheiro.",
                    "PROGRAMA GRAVADO", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Não foi possível gravar o programa:\r\n{ex.Message}", "ERRO AO GRAVAR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Preenche as linhas da lista a partir do modelar_programa.json (se existir).
        private void CarregarProgramaParaLinhas()
        {
            ProgramaModelar programa = ProgramaModelar.Carregar(ProgramaModelar.CaminhoPorOmissao());
            if (programa == null) { return; }

            // As linhas editam os filhos do primeiro LoopDias (o caso comum);
            // se não houver loop, usa as ações de topo.
            List<AcaoModelar> acoes = programa.Acoes.FirstOrDefault(a => a.Tipo == TipoDeAcao.LoopDias)?.Filhos ?? programa.Acoes;
            if (acoes.Count == 0) { return; }

            List<Panel> rows = GetDocumentRows();

            // Garantir uma linha por ação (clonando a linha modelo)
            while (rows.Count < acoes.Count)
            {
                Panel novaLinha = CloneRow(rowPanel_WordDoc);
                mainWordFlowPanel.Controls.Add(novaLinha);
                rows.Add(novaLinha);
            }

            for (int i = 0; i < acoes.Count; i++)
            {
                SetRowFromAcao(rows[i], acoes[i]);
            }
        }

        private void SetRowFromAcao(Panel row, AcaoModelar acao)
        {
            var cmb = GetRowTipoCombo(row);
            if (cmb != null)
            {
                int idx = Array.IndexOf(tiposDeAcaoPorIndice, acao.Tipo);
                cmb.SelectedIndex = idx >= 0 ? idx : 0;
            }

            TextBox nameBox = GetRowNameTextBox(row);
            if (nameBox != null) { nameBox.Text = acao.Nome; }

            TextBox fileBox = GetRowFileTextBox(row);
            if (fileBox != null) { fileBox.Text = acao.Ficheiro; }

            Button chk = GetRowChkButton(row);
            if (chk != null) { SwitchRowActivationState(chk, acao.Ativa); }
        }

        // B-1.2.4: SETAS PARA TROCAR A ORDEM DAS LINHAS
        // Criadas em código (e não no Designer) para serem também clonadas pelo
        // CloneControls, que lhes liga os eventos por nome (btnWUp / btnWDown).
        private void AddOrderArrowButtons(Panel row)
        {
            Button btnWUp = CreateOrderArrowButton("btnWUp", "▲");
            Button btnWDown = CreateOrderArrowButton("btnWDown", "▼");

            btnWUp.Click += (s, args) => MoveRowByOffset(((Control)s).Parent, -1);
            btnWDown.Click += (s, args) => MoveRowByOffset(((Control)s).Parent, +1);

            row.Controls.Add(btnWUp);
            row.Controls.Add(btnWDown);
        }

        private Button CreateOrderArrowButton(string name, string glyph)
        {
            return new Button
            {
                Name = name,
                Text = glyph,
                Dock = DockStyle.Right,
                Width = 26,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point),
                BackColor = Color.FromArgb(40, 30, 40),
                ForeColor = Color.LightSkyBlue,
                UseVisualStyleBackColor = false,
                TabStop = false
            };
        }

        // Move a linha uma posição para cima (delta -1) ou para baixo (delta +1),
        // respeitando a validação de nomes do B-1.2.5.
        private void MoveRowByOffset(Control row, int delta)
        {
            if (row == null || row.Parent != mainWordFlowPanel) { return; }

            if (!CanMoveRow(row, out string blockReason))
            {
                FlashRowNameBox(row, blockReason);
                return;
            }

            int idx = mainWordFlowPanel.Controls.GetChildIndex(row);
            int newIdx = idx + delta;
            if (newIdx < 0 || newIdx >= mainWordFlowPanel.Controls.Count) { return; }

            mainWordFlowPanel.Controls.SetChildIndex(row, newIdx);
            RefreshListLayout();
        }






        ///////*                      *\\\\\\\
        //////*    TOP MENU BUTTONS    *\\\\\\ 



        //// TOP MENU BUTTONS                 //
        // -------------------------------------

        private void btnProgramas_Click(object sender, EventArgs e)
        {
            isProgramMenu = true;
            panelMenu_Resize(null, null);

        }

        private void btnFicheiros_Click(object sender, EventArgs e)
        {
            isProgramMenu = false;
            panelMenu_Resize(null, null);
        }



        //// MENU LOGIC                        //
        // -------------------------------------

        
        // Resize the menu buttons
        private void panelMenu_Resize(object sender, EventArgs e)
        {
            if (isProgramMenu)
            {
                btnProgramas.Size = new Size((int)(panelMenu.Width * 0.8), btnProgramas.Height);
                btnFicheiros.Size = new Size((int)(panelMenu.Width * 0.21), (int)(btnFicheiros.Height * 0.85));
                btnProgramas.Font = new Font(btnProgramas.Font, FontStyle.Bold | FontStyle.Italic);
                btnFicheiros.Font = new Font(btnFicheiros.Font, FontStyle.Italic);
                btnProgramas.BackColor = Color.DeepSkyBlue;
                btnFicheiros.BackColor = Color.FromArgb(40, 30, 40);
                btnProgramas.ForeColor = Color.FromArgb(40, 30, 40);
                btnFicheiros.ForeColor = Color.Aqua;
                btnProgramas.Dock = DockStyle.Left;
                btnFicheiros.Dock = DockStyle.Right;
            }
            else
            {
                btnProgramas.Size = new Size((int)(panelMenu.Width * 0.21), (int)(btnProgramas.Height * 0.85));
                btnFicheiros.Size = new Size((int)(panelMenu.Width * 0.8), btnFicheiros.Height);
                btnFicheiros.Font = new Font(btnFicheiros.Font, FontStyle.Bold | FontStyle.Italic);
                btnProgramas.Font = new Font(btnProgramas.Font, FontStyle.Italic);
                btnProgramas.BackColor = Color.FromArgb(40, 30, 40);
                btnFicheiros.BackColor = Color.Aqua;
                btnProgramas.ForeColor = Color.DeepSkyBlue;
                btnFicheiros.ForeColor = Color.FromArgb(40, 30, 40);
                btnProgramas.Dock = DockStyle.Left;
                btnFicheiros.Dock = DockStyle.Right;
            }
        }

        // Change panel list events based on the menu button clicked
        
        
        


        // -------------------------------------




        //////*    ------------------------------------------------------------------------------------    *\\\\\\ 










        ///////*                                 *\\\\\\\
        //////*    SORTABLE DYNAMIC LIST LOGIC    *\\\\\\ 



        //// DRAG AND DROP & MOUSE CONTROL     //
        // -------------------------------------

        // B-1.2.5: VALIDAÇÃO DE NOMES ANTES DE MOVER
        // Uma linha só pode ser arrastada/reordenada se o nome do documento não
        // estiver vazio nem for igual (ignorando maiúsculas e espaços) ao de outra linha.
        private TextBox GetRowNameTextBox(Control row)
        {
            return (row as Panel)?.Controls.OfType<TextBox>().FirstOrDefault(tb => tb.Name.Contains("txtNameWBox"));
        }

        private bool CanMoveRow(Control row, out string blockReason)
        {
            blockReason = "";
            TextBox nameBox = GetRowNameTextBox(row);
            if (nameBox == null)
            {
                return true; // linha sem caixa de nome (não é uma linha de documento): não bloquear
            }

            string name = (nameBox.Text ?? "").Trim();
            if (name.Length == 0)
            {
                blockReason = "O nome do documento está vazio. Preencha o nome antes de mover a linha.";
                return false;
            }

            foreach (Control other in mainWordFlowPanel.Controls)
            {
                if (other == row) { continue; }
                TextBox otherBox = GetRowNameTextBox(other);
                string otherName = (otherBox?.Text ?? "").Trim();
                if (otherName.Length > 0 && string.Equals(name, otherName, StringComparison.OrdinalIgnoreCase))
                {
                    blockReason = $"Já existe outra linha com o nome \"{otherName}\". Renomeie antes de mover.";
                    return false;
                }
            }
            return true;
        }

        // Realce temporário da caixa de nome que causou o bloqueio do movimento.
        private void FlashRowNameBox(Control row, string reason)
        {
            System.Media.SystemSounds.Beep.Play();

            TextBox nameBox = GetRowNameTextBox(row);
            if (nameBox == null) { return; }

            Color originalColor = nameBox.BackColor;
            nameBox.BackColor = Color.FromArgb(255, 200, 200);
            nameBox.Focus();

            var tip = new ToolTip();
            tip.Show(reason, nameBox, 0, -40, 2200);

            var restoreTimer = new System.Windows.Forms.Timer { Interval = 1200 };
            restoreTimer.Tick += (s, args) =>
            {
                nameBox.BackColor = originalColor;
                restoreTimer.Stop();
                restoreTimer.Dispose();
                tip.Dispose();
            };
            restoreTimer.Start();
        }

        private void elli_MouseDown(object sender, MouseEventArgs e)
        {
            Control c;
            c = (Control)sender;
            Control parentC = c.Parent;

            // B-1.2.5: bloquear o arrasto de linhas com nome vazio ou duplicado
            if (parentC != null && !CanMoveRow(parentC, out string blockReason))
            {
                FlashRowNameBox(parentC, blockReason);
                return; // dragging nunca fica true, por isso MouseMove/MouseUp ignoram o gesto
            }

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

        // B-1.2.6: o ✓/✗ passa a refletir (e alimentar) o campo "Ativa" do
        // programa Modelar. Linhas inativas ficam esbatidas e são saltadas
        // pelo motor de exportação.
        private void SwitchRowActivationState(Control chkButtonState, bool isActive)
        {
            chkButtonState.Text = isActive ? "✓" : "✗";
            chkButtonState.ForeColor = isActive ? Color.MediumSpringGreen : Color.DimGray;

            Control row = chkButtonState.Parent;
            if (row == null) { return; }

            foreach (TextBox txt in row.Controls.OfType<TextBox>())
            {
                txt.ForeColor = isActive ? Color.Gainsboro : Color.DimGray;
            }
            var cmb = GetRowTipoCombo(row);
            if (cmb != null) { cmb.ForeColor = isActive ? Color.Gainsboro : Color.DimGray; }
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


            ResizeTextBoxesInRow(newRow);

            return newRow;
        }

        private void CloneControls(Control original, Control clone, string rowName)
        {
            foreach (Control control in original.Controls)
            {
                Control newControl = null;
                bool skipChildren = false;




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
                    ((Label)newControl).AutoSize = originalLabel.AutoSize;

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
                    ((TextBox)newControl).AutoSize = originalTextBox.AutoSize;
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
                    else if (originalButton.Name.Contains("btnWUp") || originalButton.Text.Contains("▲"))
                    {
                        newControl.Click += (sender, e) => MoveRowByOffset(clone, -1);   // B-1.2.4
                    }
                    else if (originalButton.Name.Contains("btnWDown") || originalButton.Text.Contains("▼"))
                    {
                        newControl.Click += (sender, e) => MoveRowByOffset(clone, +1);   // B-1.2.4
                    }



                }
                else if (control is SIPOS.Controls.CustomComboBox originalCustomCombo)
                {
                    // B-1.2.3/B-1.2.6: ComboBox custom do tipo de ação — criar um novo
                    // (o construtor monta os controlos internos; não clonar os filhos)
                    var newCombo = CreateTipoAcaoCustomCombo();
                    if (newCombo.Items.Count > 0)
                    {
                        newCombo.SelectedIndex = Math.Max(0, originalCustomCombo.SelectedIndex);
                    }
                    newControl = newCombo;
                    skipChildren = true;
                }
                else if (control is Panel originalPanel)
                {
                    newControl = new Panel();
                    ((Panel)newControl).BackColor = originalPanel.BackColor;
                    ((Panel)newControl).ForeColor = originalPanel.ForeColor;
                    ((Panel)newControl).Font = originalPanel.Font;
                    ((Panel)newControl).AutoSize = originalPanel.AutoSize;
                    ((Panel)newControl).AutoSizeMode = originalPanel.AutoSizeMode;
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
                    // (skipChildren: controlos compostos como o CustomComboBox já
                    // constroem os seus controlos internos no construtor)
                    if (!skipChildren)
                    {
                        CloneControls(control, newControl, control.Name + rowName);
                    }

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



            foreach (Control row in mainWordFlowPanel.Controls)
            {
                if (row is Panel && row.Name.Contains("rowPanel_WordDoc"))
                {
                    row.Width = mainWordFlowPanel.ClientSize.Width - row.Margin.Horizontal;
                    ResizeTextBoxesInRow(row as Panel);
                }
            }

            RefreshListLayout();
        }


        private void ResizeTextBoxesInRow(Panel row)
        {
            TextBox txtNameWBox = row.Controls.OfType<TextBox>().FirstOrDefault(tb => tb.Name.Contains("txtNameWBox"));
            TextBox txtDirFicheiroW = row.Controls.OfType<TextBox>().FirstOrDefault(tb => tb.Name.Contains("txtDirFicheiroW"));

            if (txtNameWBox != null && txtDirFicheiroW != null)
            {
                int totalWidth = row.ClientSize.Width;
                int leftControlsWidth = row.Controls.Cast<Control>().Where(c => c.Dock == DockStyle.Left).Sum(c => c.Width);
                int rightControlsWidth = row.Controls.Cast<Control>().Where(c => c.Dock == DockStyle.Right).Sum(c => c.Width);

                int availableWidth = totalWidth - leftControlsWidth - rightControlsWidth - 20; // 20 for padding
                int nameBoxWidth = Math.Min(200, availableWidth / 3);
                int dirBoxWidth = availableWidth - nameBoxWidth;

                txtNameWBox.Width = nameBoxWidth;
                txtDirFicheiroW.Width = dirBoxWidth;

                txtNameWBox.Left = leftControlsWidth + 10;
                txtDirFicheiroW.Left = txtNameWBox.Right + 10;
            }
        }

        private void RefreshListLayout()
        {
            mainWordFlowPanel.SuspendLayout();
            forEachUpdateButtonVisibility();

            foreach (Control row in mainWordFlowPanel.Controls)
            {
                if (row is Panel && row.Name.Contains("rowPanel_WordDoc"))
                {
                    row.Width = mainWordFlowPanel.ClientSize.Width - row.Margin.Horizontal;
                    ResizeTextBoxesInRow(row as Panel);
                }
            }


            panelMenu_Resize(null, null);

            mainWordFlowPanel.ResumeLayout();
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
