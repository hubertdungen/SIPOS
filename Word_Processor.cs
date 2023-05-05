using System;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using Word = Microsoft.Office.Interop.Word;
using System.Drawing.Text;
using LinqList;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Word;
using Excel_Reader;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;

namespace Excel_Reader
{
		
	public class Word_Processor
	{

		// PUBLIC VARS

		// VARS: PESSOAS/DETAILS
		static string efetivoODU = "", ptpdODU = "", adaptODU = "", resODU = "", statusODU = "";	//ODU
		static string efetivoCCS = "", ptpdCCS = "", adaptCCS = "", resCCS = "", statusCCS = "";	//CCS
		static string efetivoSD = "", ptpdSD = "", adaptSD = "", resSD = "", statusSD = "";			//SD
		static string efetivoPD = "", ptpdPD = "", adaptPD = "", resPD = "", statusPD = "";			//PD
		static string efetivoOAF = "", ptpdOAF = "", adaptOAF = "", resOAF = "", statusOAF = "";			//FN






        public static void FindAndReplace(Word.Document osWordDoc, Word.Application wordApp, object ToFindText, object replaceWithText)
		{
			object matchCase = true;
			object matchWholeWord = true;
			object matchWildCards = false;
			object matchSoundLike = false;
			object nmatchAllforms = false;
			object forward = true;
			object format = false;
			object matchKashida = false;
			object matchDiactitics = false;
			object matchAlefHamza = false;
			object matchControl = false;
			object read_only = false;
			object visible = true;
			object replace = 2;
			object wrap = 1;
            object replaceAll = Microsoft.Office.Interop.Word.WdReplace.wdReplaceAll;


            //foreach (Microsoft.Office.Interop.Word.Range rng in osWordDoc.StoryRanges)
            //{
            //    rng.Find.Execute(ref ToFindText,
            //    ref matchCase, ref matchWholeWord,
            //    ref matchWildCards, ref matchSoundLike,
            //    ref nmatchAllforms, ref forward,
            //    ref wrap, ref format, ref replaceWithText,
            //    ref replace, ref matchKashida,
            //    ref matchDiactitics, ref matchAlefHamza,
            //    ref matchControl);
            //}



            wordApp.Selection.Find.Execute(ref ToFindText,
                ref matchCase, ref matchWholeWord,
                ref matchWildCards, ref matchSoundLike,
                ref nmatchAllforms, ref forward,
                ref wrap, ref format, ref replaceWithText,
                ref replace, ref matchKashida,
                ref matchDiactitics, ref matchAlefHamza,
                ref matchControl);

        }

        public static void FindAndReplaceHeader(Word.Document osWordDoc, Word.Application wordApp, object ToFindText, object replaceWithText)
        {

            object matchCase = true;
            object matchWholeWord = true;
            object matchWildCards = false;
            object matchSoundLike = false;
            object nmatchAllforms = false;
            object forward = true;
            object format = false;
            object matchKashida = false;
            object matchDiactitics = false;
            object matchAlefHamza = false;
            object matchControl = false;
            object read_only = false;
            object visible = true;
            object replace = 2;
            object wrap = 1;
            object replaceAll = Microsoft.Office.Interop.Word.WdReplace.wdReplaceAll;


            foreach (Microsoft.Office.Interop.Word.Range rng in osWordDoc.StoryRanges)
            {
                rng.Find.Execute(ref ToFindText,
                ref matchCase, ref matchWholeWord,
                ref matchWildCards, ref matchSoundLike,
                ref nmatchAllforms, ref forward,
                ref wrap, ref format, ref replaceWithText,
                ref replace, ref matchKashida,
                ref matchDiactitics, ref matchAlefHamza,
                ref matchControl);
            }

    //    object replaceAll = Microsoft.Office.Interop.Word.WdReplace.wdReplaceAll;
    //    object matchCase = true;
    //    object matchWholeWord = true;
    //    object matchWildCards = false;
    //    object matchSoundLike = false;
    //    object nmatchAllForms = false;
    //    object forward = true;
    //    object format = false;
    //    object matchKashida = false;
    //    object matchDiactitics = false;
    //    object matchAlefHamza = false;
    //    object matchControl = false;
    //    object read_only = false;
    //    object visible = true;
    //    object replace = 2; // 2 replace all the occurrence, 0 replace none, 1 replace first
    //    object wrap = 1;


    //    foreach (Microsoft.Office.Interop.Word.Section Wordsection in aDoc.Sections)
    //    {
    //        Microsoft.Office.Interop.Word.Range headerrRange = Wordsection.Headers[Microsoft.Office.Interop.Word.WdHeaderFooterIndex.wdHeaderFooterPrimary].Range;
    //        // headerrRange.Select();

    //        headerrRange.Find.Execute(ref findText, ref matchCase, ref matchWholeWord,
    //            ref matchWildCards, ref matchSoundLike, ref nmatchAllForms,
    //            ref forward, ref wrap, ref format, ref replaceWithText, ref replace,
    //            ref matchKashida, ref matchDiactitics, ref matchAlefHamza, ref matchControl);


    //        if (headerrRange.Find.Found)
    //        {

    //            Logger.Trace(Logger.GetCallerInfo(), this, "Encuentra Header  {0}:", replaceWithText);
    //        }
        }
    //}


    public static void CreateWordDocument(object filename, object SaveAs)
		{
			Word.Application wordApp = new Word.Application();
			object missing = Missing.Value;
			Word.Document osWordDoc = null;

			if (File.Exists((string)filename))
			{
				object readOnly = false;
				object isVisible = false;
				wordApp.Visible = false;

				osWordDoc = wordApp.Documents.Open(ref filename, ref missing, ref readOnly,
					ref missing, ref missing, ref missing,
					ref missing, ref missing, ref missing,
					ref missing, ref missing, ref missing,
					ref missing, ref missing, ref missing, ref missing);
				osWordDoc.Activate();

				listToVarsEscalados(); // Procura escalados na lista



                // INICIO
                Word_Processor.FindAndReplace(osWordDoc, wordApp, "<dataOS>", frm_OS_system.returnOSDateExtensoParse()); // DETALHES


                // CABEÇALHOS
                Word_Processor.FindAndReplaceHeader(osWordDoc, wordApp, "<dataEscalados>", frm_OS_system.returnEscaladosDateParse());
                Word_Processor.FindAndReplaceHeader(osWordDoc, wordApp, "<dataOS_abv>", frm_OS_system.returnOSDateABVParse());
                Word_Processor.FindAndReplaceHeader(osWordDoc, wordApp, "<numOS>", frm_OS_system.osNumber);


                // ESCALAS
                Word_Processor.FindAndReplace(osWordDoc, wordApp, "<ODUefectivo>", efetivoODU); //listToVarsEscalados();)  // ODU
                Word_Processor.FindAndReplace(osWordDoc, wordApp, "<ODUptpd>", ptpdODU);
                Word_Processor.FindAndReplace(osWordDoc, wordApp, "<ODUadapt>", adaptODU);
                Word_Processor.FindAndReplace(osWordDoc, wordApp, "<ODUstatus>", statusODU);
                Word_Processor.FindAndReplace(osWordDoc, wordApp, "<ODUreserva>", resODU);

                Word_Processor.FindAndReplace(osWordDoc, wordApp, "<CCSefectivo>", efetivoCCS);  // CCS
                Word_Processor.FindAndReplace(osWordDoc, wordApp, "<CCSptpd>", ptpdCCS);
                Word_Processor.FindAndReplace(osWordDoc, wordApp, "<CCSadapt>", adaptCCS);
                Word_Processor.FindAndReplace(osWordDoc, wordApp, "<CCSstatus>", statusCCS);
                Word_Processor.FindAndReplace(osWordDoc, wordApp, "<CCSreserva>", resCCS);

                Word_Processor.FindAndReplace(osWordDoc, wordApp, "<SDefectivo>", efetivoSD);  // SD
                Word_Processor.FindAndReplace(osWordDoc, wordApp, "<SDptpd>", ptpdSD);
                Word_Processor.FindAndReplace(osWordDoc, wordApp, "<SDadapt>", adaptSD);
                Word_Processor.FindAndReplace(osWordDoc, wordApp, "<SDstatus>", statusSD);
                Word_Processor.FindAndReplace(osWordDoc, wordApp, "<SDreserva>", resSD);

                Word_Processor.FindAndReplace(osWordDoc, wordApp, "<PDefectivo>", efetivoPD);  // PD
                Word_Processor.FindAndReplace(osWordDoc, wordApp, "<PDptpd>", ptpdPD);
                Word_Processor.FindAndReplace(osWordDoc, wordApp, "<PDadapt>", adaptPD);
                Word_Processor.FindAndReplace(osWordDoc, wordApp, "<PDstatus>", statusPD);
                Word_Processor.FindAndReplace(osWordDoc, wordApp, "<PDreserva>", resPD);

                Word_Processor.FindAndReplace(osWordDoc, wordApp, "<OAFefectivo>", efetivoOAF);  // PD
                Word_Processor.FindAndReplace(osWordDoc, wordApp, "<OAFptpd>", ptpdOAF);
                Word_Processor.FindAndReplace(osWordDoc, wordApp, "<OAFadapt>", adaptOAF);
                Word_Processor.FindAndReplace(osWordDoc, wordApp, "<OAFstatus>", statusOAF);
                Word_Processor.FindAndReplace(osWordDoc, wordApp, "<OAFreserva>", resOAF);

                //this.FindAndReplace(wordApp, "", "");  // FUN
                //this.FindAndReplace(wordApp, "", "");
                //this.FindAndReplace(wordApp, "", "");
                clearVars();
			}
			else
			{
				MessageBox.Show("Ficheiro Templare do Word não encontrado!", "FICHEIRO INEXISTENTE!", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			// Save As
			osWordDoc.SaveAs2(ref SaveAs, ref missing, ref missing, ref missing,
							ref missing, ref missing, ref missing,
							ref missing, ref missing, ref missing,
							ref missing, ref missing, ref missing,
							ref missing, ref missing, ref missing);
			osWordDoc.Close();
			wordApp.Quit();
			MessageBox.Show("Ficheiro criado com sucesso!", "EXPORTAÇÃO CONCLUÍDA", MessageBoxButtons.OK);

		}
        public static void listToVarsEscalados()
        {
			// DECLARAR LISTA
			List<Pessoa> nomeados = LinqList.ListaManagerEscalados.LoadList();
			string currentDate = Convert.ToString(frm_OS_system.returnEscalaDate());
            
            foreach (var nomeado in nomeados) { nomeado.NomeNomeado = Convert.ToString(characterTrashCleaner(nomeado.NomeNomeado)); } //Converter caracteres NewLine to (char)13 ou \r


            //foreach (var nomeado in nomeados){
            //	MessageBox.Show(nomeado.DataNomeado + " " + nomeado.EscalaNomeado + " " + nomeado.NomeNomeado + " " + nomeado.EstadoNomeado);
            //}

            //MessageBox.Show(currentDate);

            //// OFICIAL DE DIA
            ///
            //-------------
            // ODU Efectivo
            nomeados = nomeados.Where(x => x.DataNomeado == currentDate && x.EscalaNomeado == "Oficial de Dia" && x.EstadoNomeado == "Efetivo").ToList();

            foreach (var nomeado in nomeados) { efetivoODU = nomeado.NomeNomeado; } //efetivoODU = Convert.ToString(characterTrashCleaner(efetivoODU));


            // ODU PTPD
            nomeados = LinqList.ListaManagerEscalados.LoadList();
            nomeados = nomeados.Where(x => x.DataNomeado == currentDate && x.EscalaNomeado == "Oficial de Dia" && (x.EstadoNomeado == "PT" || x.EstadoNomeado == "PD")).ToList();

            foreach (var nomeado in nomeados) { if (nomeado.NomeNomeado != "" && nomeado.NomeNomeado != null) { ptpdODU = (Char)11 + nomeado.NomeNomeado; } }


            // ODU ADAPT
            nomeados = LinqList.ListaManagerEscalados.LoadList();
            nomeados = nomeados.Where(x => x.DataNomeado == currentDate && x.EscalaNomeado == "Oficial de Dia" && x.EstadoNomeado == "ADPT").ToList();

            foreach (var nomeado in nomeados) { if (nomeado.NomeNomeado != "" && nomeado.NomeNomeado != null) { adaptODU = (Char)11 + nomeado.NomeNomeado; } }


            // ODU RESERVA
            nomeados = LinqList.ListaManagerEscalados.LoadList();
            nomeados = nomeados.Where(x => x.DataNomeado == currentDate && x.EscalaNomeado == "Oficial de Dia" && x.EstadoNomeado == "Reserva").ToList();

            foreach (var nomeado in nomeados) { resODU = nomeado.NomeNomeado; }


            //// ------STATUS
            /// ---
            // ODU Status - PD/PT
            nomeados = LinqList.ListaManagerEscalados.LoadList();
            nomeados = nomeados.Where(x => x.DataNomeado == currentDate && x.EscalaNomeado == "Oficial de Dia" && (x.EstadoNomeado == "PT" || x.EstadoNomeado == "PD")).ToList();

            foreach (var nomeado in nomeados) { statusODU = (Char)11 + nomeado.EstadoNomeado; }


            // ODU Status - ADAPT
            nomeados = LinqList.ListaManagerEscalados.LoadList();
            nomeados = nomeados.Where(x => x.DataNomeado == currentDate && x.EscalaNomeado == "Oficial de Dia" && x.EstadoNomeado == "ADPT").ToList();

            foreach (var nomeado in nomeados) { if (statusODU != "" && statusODU != null) { statusODU += (Char)11 + nomeado.EstadoNomeado; } else { statusODU = (Char)11 + nomeado.EstadoNomeado; } }

            ///-------------------------------------------------



            //// CCS
            ///
            //-------------
            // CCS Efectivo
            nomeados = LinqList.ListaManagerEscalados.LoadList();
            nomeados = nomeados.Where(x => x.DataNomeado == currentDate && x.EscalaNomeado == "CCS" && x.EstadoNomeado == "Efetivo").ToList();

            foreach (var nomeado in nomeados) { efetivoCCS = nomeado.NomeNomeado; }


            // CCS PTPD
            nomeados = LinqList.ListaManagerEscalados.LoadList();
            nomeados = nomeados.Where(x => x.DataNomeado == currentDate && x.EscalaNomeado == "CCS" && (x.EstadoNomeado == "PT" || x.EstadoNomeado == "PD")).ToList();

            foreach (var nomeado in nomeados) { if (nomeado.NomeNomeado != "" && nomeado.NomeNomeado != null) { ptpdCCS = (Char)11 + nomeado.NomeNomeado; } }


            // CCS ADAPT
            nomeados = LinqList.ListaManagerEscalados.LoadList();
            nomeados = nomeados.Where(x => x.DataNomeado == currentDate && x.EscalaNomeado == "CCS" && x.EstadoNomeado == "ADPT").ToList();

            foreach (var nomeado in nomeados) { if (nomeado.NomeNomeado != "" && nomeado.NomeNomeado != null) { adaptCCS = (Char)11 + nomeado.NomeNomeado; } }


            // CCS RESERVA
            nomeados = LinqList.ListaManagerEscalados.LoadList();
            nomeados = nomeados.Where(x => x.DataNomeado == currentDate && x.EscalaNomeado == "CCS" && x.EstadoNomeado == "Reserva").ToList();

            foreach (var nomeado in nomeados) { resCCS = nomeado.NomeNomeado; }


            //// ------STATUS
            /// ---
            // CCS Status - PD/PT
            nomeados = LinqList.ListaManagerEscalados.LoadList();
            nomeados = nomeados.Where(x => x.DataNomeado == currentDate && x.EscalaNomeado == "CCS" && (x.EstadoNomeado == "PT" || x.EstadoNomeado == "PD")).ToList();

            foreach (var nomeado in nomeados) { statusCCS = (Char)11 + nomeado.EstadoNomeado; }


            // CCS Status - ADAPT
            nomeados = LinqList.ListaManagerEscalados.LoadList();
            nomeados = nomeados.Where(x => x.DataNomeado == currentDate && x.EscalaNomeado == "CCS" && x.EstadoNomeado == "ADPT").ToList();

            foreach (var nomeado in nomeados) { if (statusCCS != "" && statusCCS != null) { statusCCS += (Char)11 + nomeado.EstadoNomeado; } else { statusCCS = (Char)11 + nomeado.EstadoNomeado; } }

            ///-------------------------------------------------
            ///


            //// SARGENTO DE DIA
            ///
            //-------------
            // SD Efectivo
            nomeados = LinqList.ListaManagerEscalados.LoadList();
            nomeados = nomeados.Where(x => x.DataNomeado == currentDate && x.EscalaNomeado == "Sargento de Dia" && x.EstadoNomeado == "Efetivo").ToList();

            foreach (var nomeado in nomeados) { efetivoSD = nomeado.NomeNomeado; }


            // SD PTPD
            nomeados = LinqList.ListaManagerEscalados.LoadList();
            nomeados = nomeados.Where(x => x.DataNomeado == currentDate && x.EscalaNomeado == "Sargento de Dia" && (x.EstadoNomeado == "PT" || x.EstadoNomeado == "PD")).ToList();

            foreach (var nomeado in nomeados) { if (nomeado.NomeNomeado != "" && nomeado.NomeNomeado != null) { ptpdSD = (Char)11 + nomeado.NomeNomeado; } }


            // SD ADAPT
            nomeados = LinqList.ListaManagerEscalados.LoadList();
            nomeados = nomeados.Where(x => x.DataNomeado == currentDate && x.EscalaNomeado == "Sargento de Dia" && x.EstadoNomeado == "ADPT").ToList();

            foreach (var nomeado in nomeados) { if (nomeado.NomeNomeado != "" && nomeado.NomeNomeado != null) { adaptSD = (Char)11 + nomeado.NomeNomeado; } }


            // SD RESERVA
            nomeados = LinqList.ListaManagerEscalados.LoadList();
            nomeados = nomeados.Where(x => x.DataNomeado == currentDate && x.EscalaNomeado == "Sargento de Dia" && x.EstadoNomeado == "Reserva").ToList();

            foreach (var nomeado in nomeados) { resSD = nomeado.NomeNomeado; }


            //// ------STATUS
            /// ---
            // SD Status - PD/PT
            nomeados = LinqList.ListaManagerEscalados.LoadList();
            nomeados = nomeados.Where(x => x.DataNomeado == currentDate && x.EscalaNomeado == "Sargento de Dia" && (x.EstadoNomeado == "PT" || x.EstadoNomeado == "PD")).ToList();

            foreach (var nomeado in nomeados) { statusSD = (Char)11 + nomeado.EstadoNomeado; }


            // SD Status - ADAPT
            nomeados = LinqList.ListaManagerEscalados.LoadList();
            nomeados = nomeados.Where(x => x.DataNomeado == currentDate && x.EscalaNomeado == "Sargento de Dia" && x.EstadoNomeado == "ADPT").ToList();

            foreach (var nomeado in nomeados) { if (statusSD != "" && statusSD != null) { statusSD += (Char)11 + nomeado.EstadoNomeado; } else { statusSD = (Char)11 + nomeado.EstadoNomeado; } }

            ///-------------------------------------------------
            ///


            //// PRAÇA DE DIA
            ///
            //-------------
            // Praça de Dia Efectivo
            nomeados = LinqList.ListaManagerEscalados.LoadList();
            nomeados = nomeados.Where(x => x.DataNomeado == currentDate && x.EscalaNomeado == "Praça de Dia" && x.EstadoNomeado == "Efetivo").ToList();

            foreach (var nomeado in nomeados) { efetivoPD = nomeado.NomeNomeado; }


            // PD PTPD
            nomeados = LinqList.ListaManagerEscalados.LoadList();
            nomeados = nomeados.Where(x => x.DataNomeado == currentDate && x.EscalaNomeado == "Praça de Dia" && (x.EstadoNomeado == "PT" || x.EstadoNomeado == "PD")).ToList();

            foreach (var nomeado in nomeados) { if (nomeado.NomeNomeado != "" && nomeado.NomeNomeado != null) { ptpdPD = (Char)11 + nomeado.NomeNomeado; } }


            // PD ADAPT
            nomeados = LinqList.ListaManagerEscalados.LoadList();
            nomeados = nomeados.Where(x => x.DataNomeado == currentDate && x.EscalaNomeado == "Praça de Dia" && x.EstadoNomeado == "ADPT").ToList();

            foreach (var nomeado in nomeados) { if (nomeado.NomeNomeado != "" && nomeado.NomeNomeado != null) { adaptPD = (Char)11 + nomeado.NomeNomeado; } }


            // PD RESERVA
            nomeados = LinqList.ListaManagerEscalados.LoadList();
            nomeados = nomeados.Where(x => x.DataNomeado == currentDate && x.EscalaNomeado == "Praça de Dia" && x.EstadoNomeado == "Reserva").ToList();

            foreach (var nomeado in nomeados) { resPD = nomeado.NomeNomeado; }


            //// ------STATUS
            /// ---
            // PD Status - PD/PT
            nomeados = LinqList.ListaManagerEscalados.LoadList();
            nomeados = nomeados.Where(x => x.DataNomeado == currentDate && x.EscalaNomeado == "Praça de Dia" && (x.EstadoNomeado == "PT" || x.EstadoNomeado == "PD")).ToList();

            foreach (var nomeado in nomeados) { statusPD = (Char)11 + nomeado.EstadoNomeado; }


            // PD Status - ADAPT
            nomeados = LinqList.ListaManagerEscalados.LoadList();
            nomeados = nomeados.Where(x => x.DataNomeado == currentDate && x.EscalaNomeado == "Praça de Dia" && x.EstadoNomeado == "ADPT").ToList();

            foreach (var nomeado in nomeados) { if (statusPD != "" && statusPD != null) { statusPD += (Char)11 + nomeado.EstadoNomeado; } else { statusPD = (Char)11 + nomeado.EstadoNomeado; } }

            ///-------------------------------------------------
            ///
            
            //// OA FUNERAIS
            ///
            //-------------
            // OAF Efectivo
            nomeados = LinqList.ListaManagerEscalados.LoadList();
            nomeados = nomeados.Where(x => x.DataNomeado == currentDate && x.EscalaNomeado == "Honras Fúnebres" && x.EstadoNomeado == "Efetivo").ToList();

            foreach (var nomeado in nomeados) { efetivoOAF = nomeado.NomeNomeado; }


            // OAF PTPD
            nomeados = LinqList.ListaManagerEscalados.LoadList();
            nomeados = nomeados.Where(x => x.DataNomeado == currentDate && x.EscalaNomeado == "Honras Fúnebres" && (x.EstadoNomeado == "PT" || x.EstadoNomeado == "PD")).ToList();

            foreach (var nomeado in nomeados) { if (nomeado.NomeNomeado != "" && nomeado.NomeNomeado != null) { ptpdOAF = (Char)11 + nomeado.NomeNomeado; } }


            // OAF ADAPT
            nomeados = LinqList.ListaManagerEscalados.LoadList();
            nomeados = nomeados.Where(x => x.DataNomeado == currentDate && x.EscalaNomeado == "Honras Fúnebres" && x.EstadoNomeado == "ADPT").ToList();

            foreach (var nomeado in nomeados) { if (nomeado.NomeNomeado != "" && nomeado.NomeNomeado != null) { adaptOAF = (Char)11 + nomeado.NomeNomeado; } }


            // OAF RESERVA
            nomeados = LinqList.ListaManagerEscalados.LoadList();
            nomeados = nomeados.Where(x => x.DataNomeado == currentDate && x.EscalaNomeado == "Honras Fúnebres" && x.EstadoNomeado == "Reserva").ToList();

            foreach (var nomeado in nomeados) { resOAF = nomeado.NomeNomeado; }


            //// ------STATUS
            /// ---
            // FN Status - PD/PT
            nomeados = LinqList.ListaManagerEscalados.LoadList();
            nomeados = nomeados.Where(x => x.DataNomeado == currentDate && x.EscalaNomeado == "Honras Fúnebres" && (x.EstadoNomeado == "PT" || x.EstadoNomeado == "PD")).ToList();

            foreach (var nomeado in nomeados) { statusOAF = (Char)11 + nomeado.EstadoNomeado; }


            // FN Status - ADAPT
            nomeados = LinqList.ListaManagerEscalados.LoadList();
            nomeados = nomeados.Where(x => x.DataNomeado == currentDate && x.EscalaNomeado == "Honras Fúnebres" && x.EstadoNomeado == "ADPT").ToList();

            foreach (var nomeado in nomeados) { if (statusOAF != "" && statusOAF != null) { statusOAF += (Char)11 + nomeado.EstadoNomeado; } else { statusOAF = (Char)11 + nomeado.EstadoNomeado; } }

            ///-------------------------------------------------
            ///



        }

        public static void clearVars()
		{
			// VARS: PESSOAS/DETAILS
			efetivoODU = "";
			ptpdODU = "";
			adaptODU = "";
			resODU = "";
			statusODU = "";    //ODU

			efetivoCCS = "";
			ptpdCCS = "";
			adaptCCS = "";
			resCCS = "";
			statusCCS = "";    //CCS

			efetivoSD = "";
			ptpdSD = "";
			adaptSD = "";
			resSD = "";
			statusSD = "";         //SD

			efetivoPD = "";
			ptpdPD = "";
			adaptPD = "";
			resPD = "";
			statusPD = "";         //PD

            efetivoOAF = "";
            ptpdOAF = "";
            adaptOAF = "";
            resOAF = "";
            statusOAF = "";         //FN
            
        }

        private static object characterTrashCleaner(string trashString)
        {
            while (trashString.Contains(System.Environment.NewLine))
            {
                trashString = trashString.Replace(System.Environment.NewLine, "\r");
            }
            return trashString;
        }



    }




}





// BACKUP

////List<Pessoa> escaladosList = LinqList.ListaManagerEscalados.escaladosList;

//List<Pessoa> pessoas = LinqList.ListaManagerEscalados.LoadList();
//string currentDate = Convert.ToString(varsPublicAcess.returnSelectedDate());


////string pessoa = Convert.ToString(pessoas.Where(x => x.DataNomeado == currentDate && x.EscalaNomeado == "Oficial de Dia" && x.EstadoNomeado == "Efetivo"));
//pessoas = pessoas.Where(x => x.DataNomeado == currentDate && x.EscalaNomeado == "Oficial de Dia" && x.EstadoNomeado == "Efetivo").ToList();
////escaladosList.Select(p => p.NomeNomeado);

//foreach (var pessoa in pessoas)
//{
//    MessageBox.Show(pessoa.NomeNomeado + "\n");
//}
