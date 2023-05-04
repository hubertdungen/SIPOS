using LinqList;
using Microsoft.Office.Interop.Word;
using SIPOS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SIPOS.Forms
{
    public partial class FormDados : Form
    {
        // VARS DECLARATION
        public static string DadosTextBoxString = "";

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

            if (Mediator.isItSabado)
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
            prg_Bar.Value = prg_Bar.Value + 1 + addMore;

            if (prg_Bar.Value >= prg_Bar.Maximum)
            {
                prg_Bar.Value = prg_Bar.Maximum;
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
