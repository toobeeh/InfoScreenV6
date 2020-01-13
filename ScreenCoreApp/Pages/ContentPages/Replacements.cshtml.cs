using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Infoscreen_Verwaltung.classes;
using ScreenCoreApp.Classes;

namespace ScreenCoreApp
{
    public class ReplacementsModel : PageModel
    {
        public int Pagenum;
        public int item_start;
        public int item_end;
        public Structuren.Supplierungen[] Replacements;
        public Structuren.GlobalEntfall[] Globals;
        public bool DisplayGlobal = false;
        public bool NoReplacements = false;

        public void OnGet(string pagenum)
        {
            Pagenum = Convert.ToInt32(pagenum);
            if (Pagenum == 0) { NoReplacements = true; Pagenum = 1; }
            
            int screenID = Screen.GetSessionScreenID(HttpContext);
            string departmentName = DatenbankAbrufen.BildschirmInformationenAbrufen(screenID).Abteilung;
            int depID = DatenbankAbrufen.GetAbteilungsIdVonAbteilungsname(departmentName);
            int total_rows;
            Globals = DatenbankAbrufen.GetGlobaleEntfaelle(depID);

            if (NoReplacements) Replacements = new Structuren.Supplierungen[0];
            else Replacements = DatenbankAbrufen.SupplierplanAbrufen(departmentName, false, "", "", false)[0].Supplierungen;
            
            total_rows = Globals.Length > 0 ? ((Replacements.Length % 10 > 8 - Globals.Length ? ((Replacements.Length + 9) / 10) * 10 : Replacements.Length) + Globals.Length + 2) : Replacements.Length;

            item_start = (Pagenum - 1) * 10;
            item_end = (item_start + 10 > Replacements.Length -1 ? Replacements.Length - 1 : item_start + 10);

            if (Globals.Length > 0 && (NoReplacements || total_rows - item_start <= 10)) DisplayGlobal = true;

            DatenbankAbrufen.DBClose();
        }

        public string GenerateReplacementRows()
        {
            string html = "";

            if (NoReplacements)
            {
                html += @"
                    <tr>
                        <td colspan='6'>" + "Keine Supplierungen eingetragen" + @"</td>
                    </tr>";
                return html;
            }


            for (int i = item_start; i<= item_end; i++)
            {
                string replacement;
                if (Replacements[i].Entfällt) replacement = "Entfällt";
                else if (Replacements[i].ZiehtVor > 0) replacement =
                           "Verschoben auf " + StringFunctions.GetDayOfInt((int)Replacements[i].ZiehtVorDatum.DayOfWeek) + " " + Replacements[i].ZiehtVorDatum.ToShortDateString() + ", " + Replacements[i].ZiehtVor + ". Stunde";
                else replacement = Replacements[i].Ersatzlehrer;

                html += @"
                    <tr>
                        <td>" + Replacements[i].Datum.ToShortDateString() + @"</td>
                        <td>" + Replacements[i].Klasse + @"</td>
                        <td>" + Replacements[i].Stunde + @"</td>
                        <td>" + Replacements[i].Ersatzfach + @"</td>
                        <td>" + Replacements[i].Ursprungslehrer + @"</td>
                        <td>" + replacement + @"</td>
                    </tr>";
            }

            if (html.Length > 0) html = @"
                    <tr>
                        <th style = 'width:15%'> Datum </th>
                        <th style = 'width:10%'> Klasse </th>
                        <th style = 'width: 10%'> Stunde </th>  
                        <th style = 'width:15%'> Fach </th> 
                        <th style = 'width:15%'> Lehrer </th>  
                        <th style = 'width:35%'> Vertretung </th>
                      </tr>" + html;
            else
            {
                html = @"
                    <tr>
                        <th style = 'width:15%'></th>
                        <th style = 'width:10%'></th>
                        <th style = 'width: 10%'></th>  
                        <th style = 'width:15%'></th> 
                        <th style = 'width:15%'></th>  
                        <th style = 'width:35%'></th>
                      </tr>" + html;
            }

            return html;
        }

        public string GenerateGlobalRows()
        {
            if (!DisplayGlobal) return "";

            string html = "";

            html += @"
                    <tr style='opacity: 0; background-color: transparent'>
                       <td> Layout_fill </td> 
                       <td> Layout_fill </td> 
                       <td> Layout_fill </td>  
                       <td> Layout_fill </td>  
                       <td> Layout_fill </td>
                    </tr>
                      ";

            html += @"
                    <tr>
                        <th colspan='6'>" + "Konferenzen / Globaler Entfall" + @"</th>
                    </tr>";

            html += @"
                    <tr>
                        <th>" + "Datum" + @"</th>
                        <th>" + "Tag" + @"</th>
                        <th>" + "Von" + @"</th>
                        <th>" + "Bis" + @"</th>
                        <th>" + @"</th>
                        <th>" + @"</th>
                    </tr>";


            for (int i = 0; i < Globals.Length; i++)
            {
                html += @"
                    <tr>
                        <td>" + Globals[i].Datum.ToShortDateString() + @"</td>
                        <td>" + ScreenCoreApp.Classes.StringFunctions.GetDayOfInt((int)Globals[i].Datum.DayOfWeek) + @"</td>
                        <td>" + Globals[i].VonStunde + @".Stunde </td>
                        <td>" + Globals[i].BisStunde + @".Stunde </td>
                        <td>" + @"</td>
                        <td>" + @"</td>
                    </tr>";
            }

            return html;
        }

        public string GenerateFillerRows()
        {
            string html = "";
            int fill_start = ((!NoReplacements ? item_end - item_start : 0 ) > 0 ? item_end - item_start : 0) + (DisplayGlobal ? Globals.Length : 0);
            for (int i = fill_start; i <= 10; i++)
            {
                html += @"
                    <tr style='opacity: 0; background-color: transparent'>
                       <td> Layout_fill </td> 
                       <td> Layout_fill </td> 
                       <td> Layout_fill </td>  
                       <td> Layout_fill </td>  
                       <td> Layout_fill </td>
                    </tr>
                      ";
            }

            return html;
        }

    }
}