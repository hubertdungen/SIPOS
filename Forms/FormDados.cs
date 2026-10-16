using LinqList;
using System.Runtime.InteropServices;

namespace SIPOS.Forms
{
    public partial class FormDados : Form
    {
        // VARS DECLARATION
        public static string DadosTextBoxString = "";

        // B-1.3.2: checkBox para ativar seleção de data de início e fim no calendário
        private CheckBox chkRangeSelection;

        public FormDados()
        {
            InitializeComponent();

            Mediator.formDados = this;
            //Mediator.UpdateFormDadosTextBox += UpdateFormDadosTextBox;
        }

        private void FormDados_Load(object sender, EventArgs e)
        {
            check_ifDebugIsActive();
            monthCalendar.SelectionStart = Mediator.osDay;
            numUpDow_diasIntp.Value = Mediator.plusDayIntrup;
            textBox_Output.Text = EscalasEngine.outputInitialText;
            AplicarTemaEscuro();
            AddRangeSelectionCheckBox();
        }

        // ---------- TEMA ESCURO DOS CONTROLOS NATIVOS ----------
        // MonthCalendar, ProgressBar e a barra de deslocamento da TextBox são desenhados
        // pelo Windows (claros) e ignoravam as cores do formulário.
        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
        private static extern int SetWindowTheme(IntPtr hWnd, string? pszSubAppName, string? pszSubIdList);

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);
        private const int MCM_SETCOLOR = 0x100A;
        private const int MCSC_BACKGROUND = 0;

        private static readonly Color corPainel = Color.FromArgb(35, 26, 45);
        private static readonly Color corContorno = Color.FromArgb(79, 49, 79);

        // Aplica já e volta a aplicar se o controlo recriar o handle.
        // tema "" = sem visual styles (o controlo passa a usar BackColor/ForeColor).
        private static void TemaNativo(Control controlo, string tema)
        {
            string? lista = tema.Length == 0 ? "" : null;
            controlo.HandleCreated += (s, e) => SetWindowTheme(controlo.Handle, tema, lista);
            SetWindowTheme(controlo.Handle, tema, lista);
        }

        private SetasCalendario? setasCalendario;

        // Sem visual styles, as setas de mudar de mês são botões cinzentos clássicos que
        // não aceitam cores; são redesenhadas por cima, no tom do título. Só pintura:
        // os cliques continuam a ir para o controlo nativo.
        private sealed class SetasCalendario : NativeWindow
        {
            private const int WM_PAINT = 0x000F, WM_TIMER = 0x0113, WM_LBUTTONDOWN = 0x0201, WM_LBUTTONUP = 0x0202, WM_CAPTURECHANGED = 0x0215;
            private const int MCM_GETCALENDARGRIDINFO = 0x1018, MCGIP_NEXT = 1, MCGIP_PREV = 2, MCGIF_RECT = 2;
            private readonly MonthCalendar cal;

            [StructLayout(LayoutKind.Sequential)]
            private struct MCGRIDINFO
            {
                public int cbSize, dwPart, dwFlags, iCalendar, iRow, iCol, bSelected;
                public int st0, st1, st2, st3, st4, st5, st6, st7; // stStart e stEnd (2 × SYSTEMTIME)
                public int left, top, right, bottom;
                public IntPtr pszName, cchName;
            }

            [DllImport("user32.dll")]
            private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, ref MCGRIDINFO lParam);

            public SetasCalendario(MonthCalendar calendario)
            {
                cal = calendario;
                cal.HandleCreated += (s, e) => AssignHandle(cal.Handle);
                cal.HandleDestroyed += (s, e) => ReleaseHandle();
                if (cal.IsHandleCreated) { AssignHandle(cal.Handle); }
            }

            protected override void WndProc(ref Message m)
            {
                base.WndProc(ref m);
                if (m.Msg is WM_PAINT or WM_TIMER or WM_LBUTTONDOWN or WM_LBUTTONUP or WM_CAPTURECHANGED) { Pintar(); }
            }

            private void Pintar()
            {
                using Graphics g = Graphics.FromHwnd(Handle);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                using var fundo = new SolidBrush(cal.TitleBackColor);
                using var seta = new SolidBrush(cal.TitleForeColor);
                foreach (int parte in new[] { MCGIP_PREV, MCGIP_NEXT })
                {
                    var info = new MCGRIDINFO { cbSize = Marshal.SizeOf<MCGRIDINFO>(), dwPart = parte, dwFlags = MCGIF_RECT };
                    if (SendMessage(Handle, MCM_GETCALENDARGRIDINFO, IntPtr.Zero, ref info) == IntPtr.Zero) { continue; }
                    Rectangle r = Rectangle.FromLTRB(info.left, info.top, info.right, info.bottom);
                    if (r.Width <= 0 || r.Height <= 0) { continue; }

                    g.FillRectangle(fundo, Rectangle.Inflate(r, 2, 2)); // inclui o relevo 3D do botão
                    float cx = r.Left + r.Width / 2f, cy = r.Top + r.Height / 2f;
                    PointF[] triangulo = parte == MCGIP_PREV
                        ? new[] { new PointF(cx + 2.5f, cy - 5f), new PointF(cx + 2.5f, cy + 5f), new PointF(cx - 3.5f, cy) }
                        : new[] { new PointF(cx - 2.5f, cy - 5f), new PointF(cx - 2.5f, cy + 5f), new PointF(cx + 3.5f, cy) };
                    g.FillPolygon(seta, triangulo);
                }
            }
        }

        // MCSC_BACKGROUND: a margem à volta do mês, que o WinForms não expõe (ficava branca).
        private void MargemCalendario()
        {
            SendMessage(monthCalendar.Handle, MCM_SETCOLOR, (IntPtr)MCSC_BACKGROUND, (IntPtr)ColorTranslator.ToWin32(corPainel));
        }

        private void AplicarTemaEscuro()
        {
            // Calendário escuro: roxo no título, nos dias da semana e na seleção, ciano no
            // texto do título. O retângulo azul do "hoje" (cor do sistema) destoava do tema;
            // o rodapé "Hoje: dd/mm/aaaa" mantém-se.
            monthCalendar.ShowTodayCircle = false;
            TemaNativo(monthCalendar, "");
            monthCalendar.HandleCreated += (s, e) => MargemCalendario();
            MargemCalendario();
            monthCalendar.BackColor = corPainel;
            monthCalendar.ForeColor = Color.Gainsboro;
            monthCalendar.TitleBackColor = Color.FromArgb(125, 80, 140);
            monthCalendar.TitleForeColor = Color.Cyan;
            monthCalendar.TrailingForeColor = Color.FromArgb(105, 90, 115);
            setasCalendario = new SetasCalendario(monthCalendar);

            // Sem visual styles o calendário fica mais alto: recalcular o tamanho e descer
            // o REFRESH, a barra e os "Dias de interrupção" na mesma medida.
            monthCalendar.Height += 1;
            int deslocamento = monthCalendar.Bottom + 1 - btn_refresh.Top;
            if (deslocamento > 0)
            {
                foreach (Control? c in new Control?[] { btn_refresh, prg_Bar, numUpDow_diasIntp, Controls["lbl_numUpDownDayIntrup"] })
                {
                    if (c != null) { c.Top += deslocamento; }
                }
            }

            // Barra de progresso: faixa escura em repouso (era uma tira cinzenta clara),
            // preenchimento ciano durante o processamento.
            TemaNativo(prg_Bar, "");
            prg_Bar.BackColor = Color.FromArgb(41, 30, 51);
            prg_Bar.ForeColor = Color.Cyan;

            // Caixa de saída: moldura discreta com margem interior em vez do contorno
            // branco nativo, e barra de deslocamento escura.
            var moldura = new Panel
            {
                Name = "pnlMolduraOutput",
                BackColor = textBox_Output.BackColor,
                Bounds = textBox_Output.Bounds,
                Anchor = textBox_Output.Anchor,
                Padding = new Padding(8, 6, 1, 1),
            };
            moldura.Paint += (s, e) =>
            {
                using var caneta = new Pen(corContorno);
                e.Graphics.DrawRectangle(caneta, 0, 0, moldura.Width - 1, moldura.Height - 1);
            };
            moldura.Resize += (s, e) => moldura.Invalidate();
            Controls.Add(moldura);
            textBox_Output.BorderStyle = BorderStyle.None;
            textBox_Output.Dock = DockStyle.Fill;
            moldura.Controls.Add(textBox_Output);
            TemaNativo(textBox_Output, "DarkMode_Explorer");
        }

        // B-1.3.2: criado em código para dispensar alterações no Designer.
        // Desligado (estado inicial) o calendário comporta-se exatamente como antes
        // (seleção de um único dia). Ligado, permite escolher um intervalo
        // início→fim e os "Dias de interrupção" passam a ser calculados
        // automaticamente a partir desse intervalo.
        private void AddRangeSelectionCheckBox()
        {
            chkRangeSelection = new CheckBox
            {
                Name = "chkRangeSelection",
                Text = "Ativar data de início e fim",
                AutoSize = true,
                Font = new Font("Century Gothic", 10F, FontStyle.Regular, GraphicsUnit.Point),
                ForeColor = Color.Gainsboro,
                // Fundo sólido igual ao do formulário (Transparent deixava uma caixa mais
                // clara) e na coluna da esquerda, por baixo dos "Dias de interrupção"
                // (antes ficava à direita e sobrepunha-se à caixa do pessoal escalado).
                BackColor = this.BackColor,
                Location = new Point(monthCalendar.Left, numUpDow_diasIntp.Bottom + 14),
                Checked = false
            };
            chkRangeSelection.CheckedChanged += chkRangeSelection_CheckedChanged;
            monthCalendar.Parent.Controls.Add(chkRangeSelection);
            chkRangeSelection.BringToFront();

            // O FormDados é recriado a cada visita ao separador e a checkbox nasce
            // desligada: o estado partilhado com o Export tem de acompanhar, senão
            // a exportação continuava em modo intervalo sem o utilizador o ver.
            Mediator.rangeAtivo = false;
            Mediator.rangeInicio = monthCalendar.SelectionStart.Date;
            Mediator.rangeFim = monthCalendar.SelectionEnd.Date;
        }

        private void chkRangeSelection_CheckedChanged(object sender, EventArgs e)
        {
            Mediator.rangeAtivo = chkRangeSelection.Checked;

            if (chkRangeSelection.Checked)
            {
                monthCalendar.MaxSelectionCount = 62;
            }
            else
            {
                // Voltar ao comportamento clássico: colapsar o intervalo num único dia
                monthCalendar.SelectionEnd = monthCalendar.SelectionStart;
                monthCalendar.MaxSelectionCount = 1;
            }
        }

        // Dias de interrupção do intervalo selecionado, ou null se o modo
        // intervalo estiver desligado ou só estiver escolhido um dia. O início é
        // o dia da O.S. e o fim o último dia coberto (o primeiro dia útil), por
        // isso os dias de interrupção são os que ficam entre os dois — ex.:
        // sexta→segunda dá 2 (sábado e domingo), como a regra clássica do sábado.
        private int? GetSelectedRangeInterruptionDays()
        {
            if (chkRangeSelection == null || !chkRangeSelection.Checked)
            {
                return null;
            }
            int dias = (monthCalendar.SelectionEnd.Date - monthCalendar.SelectionStart.Date).Days;
            return dias > 0 ? dias - 1 : null;
        }


        // UI CONTROL
        // ----------
        public static void txtboxsActualizer()
        {
            FormDados formDados = Mediator.formDados;
        }
        private void numUpDow_diasIntp_ValueChanged(object sender, EventArgs e)
        {
            //whenever the value of the numericUpDown is changed, it will update the value of plusDayIntrup
            Mediator.plusDayIntrup = Convert.ToInt32(numUpDow_diasIntp.Value);
        }
        private void UpdateFormDadosTextBox(string value)
        {

        }


        private void btn_refresh_Click_1(object sender, EventArgs e)
        {
            prg_Bar.Value = 0;  // Progress Bar to 0
            LinqList.ListaManagerEscalados.escaladosList.Clear();           //Limpa a Lista
            EscalasEngine.escalaPreviewText = "";                                         //Limpa o texto preview
            Mediator.instPrgBarAddInc(0);

            //selectedDay = Convert.ToString(monthCalendar.SelectionStart);   //Converte o input data para string
            Mediator.instPrgBarAddInc(0);

            dateProcess(1);

            //if selectedDay == friday it will run triagemEscalas() for saturday, sunday and monday (inputing their dates on triagemEscalas, store the values at the list and show on the info on the preview textBox
            //else it will run triagemEscalas() for the selected day
            if (Mediator.isItSabado == true)
            {
                Mediator.instTriagemEscalas();
                dateProcess(2);
                Mediator.instTriagemEscalas();
                dateProcess(3);
                Mediator.instTriagemEscalas();
                dateProcess(1);
                //diaDeEscala = monthCalendar.SelectionStart.AddDays(1);
            }
            else
            {
                Mediator.instTriagemEscalas();
            }

            EscalasEngine.outputInitialText = textBox_Output.Text;

        }
        public void txtBox_Clear()
        {
            textBox_Output.Text = "";
            DadosTextBoxString = "";
        }
        public void txtBox_Equal_To(string TextInput)
        {
            textBox_Output.Text = TextInput;
            DadosTextBoxString = TextInput;
        }





        // CALENDAR
        // --------

        private void monthCalendar_DateChanged(object sender, DateRangeEventArgs e)
        {
            dateProcess(1);

            // B-1.5.2: expor o intervalo atual ao motor Modelar
            Mediator.rangeInicio = monthCalendar.SelectionStart.Date;
            Mediator.rangeFim = monthCalendar.SelectionEnd.Date;

            // B-1.3.2: com o modo início/fim ativo, os dias de interrupção derivam do
            // intervalo selecionado no calendário; caso contrário mantém-se a regra
            // clássica do sábado (fim-de-semana = 2 dias de interrupção).
            int? rangeInterruptionDays = GetSelectedRangeInterruptionDays();
            if (rangeInterruptionDays.HasValue)
            {
                decimal cappedDays = Math.Min(rangeInterruptionDays.Value, numUpDow_diasIntp.Maximum);
                numUpDow_diasIntp.Value = cappedDays;
                Mediator.plusDayIntrup = (int)cappedDays;
            }
            else if (Mediator.isItSabado)
            {
                numUpDow_diasIntp.Value = 2;
                Mediator.plusDayIntrup = 2;
            }
            else
            {
                numUpDow_diasIntp.Value = 0;
                Mediator.plusDayIntrup = 0;
            }

            dateProcess(1);
        }
        public void dateProcess(int addDay)
        {

            Mediator.diaDeEscala = monthCalendar.SelectionStart.AddDays(addDay);

            Mediator.isItSabado = (Mediator.diaDeEscala.DayOfWeek == DayOfWeek.Saturday) ? true : false;
            Mediator.isItQuarta = (Mediator.diaDeEscala.DayOfWeek == DayOfWeek.Wednesday) ? true : false;

            Mediator.escalaDay = Mediator.diaDeEscala.ToString("dd-MM-yyyy");
            Mediator.osDay = monthCalendar.SelectionStart;

        }
        public void afterToday()
        {
            DateTime dt = DateTime.Now;
            monthCalendar.SelectionStart = dt;
            monthCalendar.SelectionEnd = dt;

            //monthCalendar.SelectionStart = dt.AddDays(1);
            //monthCalendar.SelectionEnd = dt.AddDays(1);
        }
        public void dateParse()
        {
            Mediator.escalaDay = Mediator.escalaDay.Substring(0, Mediator.escalaDay.IndexOf(" "));
            Mediator.escalaDay = Mediator.escalaDay.Replace("/", "-");
        }
        public static object weekDayParse(string weekDay)
        {
            if (Mediator.winMode == 2) { MessageBox.Show("weekDay: " + weekDay); }

            string weekDayString = "";

            switch (weekDay)
            {

                // PORTUGUES ABV

                case "seg":
                    weekDayString = "2.ª Feira";
                    break;

                case "ter":
                    weekDayString = "3.ª Feira";
                    break;

                case "qua":
                    weekDayString = "4.ª Feira";
                    break;

                case "qui":
                    weekDayString = "5.ª Feira";
                    break;

                case "sex":
                    weekDayString = "6.ª Feira";
                    break;

                case "sáb":
                    weekDayString = "sábado";
                    break;

                case "dom":
                    weekDayString = "domingo";
                    break;

                // PORTUGUES FULL

                case "segunda":
                    weekDayString = "2.ª Feira";
                    break;

                case "terça":
                    weekDayString = "3.ª Feira";
                    break;

                case "quarta":
                    weekDayString = "4.ª Feira";
                    break;

                case "quinta":
                    weekDayString = "5.ª Feira";
                    break;

                case "sexta":
                    weekDayString = "6.ª Feira";
                    break;

                case "sábado":
                    weekDayString = "sábado";
                    break;

                case "domingo":
                    weekDayString = "domingo";
                    break;

                // ENGLISH ABV

                case "mon":
                    weekDayString = "2.ª Feira";
                    break;

                case "tue":
                    weekDayString = "3.ª Feira";
                    break;

                case "wed":
                    weekDayString = "4.ª Feira";
                    break;

                case "thu":
                    weekDayString = "5.ª Feira";
                    break;

                case "fri":
                    weekDayString = "6.ª Feira";
                    break;

                case "sat":
                    weekDayString = "sábado";
                    break;

                case "sun":
                    weekDayString = "domingo";
                    break;


                // ENGLISH FULL

                case "monday":
                    weekDayString = "2.ª Feira";
                    break;

                case "tuesday":
                    weekDayString = "3.ª Feira";
                    break;

                case "wednesday":
                    weekDayString = "4.ª Feira";
                    break;

                case "thursday":
                    weekDayString = "5.ª Feira";
                    break;

                case "friday":
                    weekDayString = "6.ª Feira";
                    break;

                case "saturday":
                    weekDayString = "sábado";
                    break;

                case "sunday":
                    weekDayString = "domingo";
                    break;

                default:
                    weekDayString = "NÃO_DETETOU_DIAdeSEMANA";
                    break;

            }

            return weekDayString;
        }


        // PROGRESS BAR 
        // ------------
        public void prgBarAddInc(int addMore)
        {
            try
            {
                prg_Bar.Value = prg_Bar.Value + 1 + addMore;

                if (prg_Bar.Value >= prg_Bar.Maximum)
                {
                    prg_Bar.Value = prg_Bar.Maximum;
                }
            }
            catch
            {
                prgBarFix();
            }

        }
        public void prgBarToMax()
        {
            //var inst_fOS = new frm_OS_system();
            prg_Bar.Value = prg_Bar.Maximum;
        }
        public void prgBarReset()
        {
            prg_Bar.Value = 0;
        }
        public void prgBarFix()
        {
            if (prg_Bar.Value >= prg_Bar.Maximum) { prg_Bar.Value = prg_Bar.Maximum; } else { prg_Bar.Value = prg_Bar.Minimum; }
        }




        // DEBUG MODE
        // ----------

        private void check_ifDebugIsActive()
        {
            if (Mediator.debugMode == true)
            {
                btn_CheckEscalaList.Visible = true;

            }
            else
            {
                btn_CheckEscalaList.Visible = false;
            }
        }

        private void btn_CheckEscalaList_Click(object sender, EventArgs e)
        {

            List<Pessoa> peopleLines = LinqList.ListaManagerEscalados.escaladosList;

            string messageEscalaList = "";

            foreach (var pessoaLine in peopleLines)
            {
                messageEscalaList += $"{pessoaLine.DataNomeado} {pessoaLine.EscalaNomeado} {pessoaLine.EstadoNomeado} {pessoaLine.NomeNomeado}" + "\r\n";
            }

            //var messageEscalaList = string.Join(Environment.NewLine, LinqList.ListaManagerEscalados.escaladosList);
            MessageBox.Show("Lista de pessoal escalado na LinqList do software:\r\n" + messageEscalaList, "DEBUG: Lista de Pessoal Escalado");
        }










        // -----------------------------
        // --------------------------------------------------------------------------
        // --------------------------------------------------------------------------





    }
}
