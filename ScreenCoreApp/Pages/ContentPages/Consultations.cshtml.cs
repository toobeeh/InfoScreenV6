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
    public class ConsultationsModel : PageModel
    {

        public int Pagenum;
        public int item_start;
        public int item_end;
        public Structuren.Sprechstunden[] Consultations;

        public void OnGet(string pagenum)
        {
            Pagenum = Convert.ToInt32(pagenum);
            
            int screenID = Screen.GetSessionScreenID(HttpContext);
            string departmentName = DatenbankAbrufen.BildschirmInformationenAbrufen(screenID).Abteilung;
            int depID = DatenbankAbrufen.GetAbteilungsIdVonAbteilungsname(departmentName);

            Consultations = SubjectFunctions.SprechstundenAbrufen(depID.ToString());

            item_start = (Pagenum - 1) * 15;
            item_end = (item_start + 15 > Consultations.Length - 1 ? Consultations.Length - 1 : item_start + 15);
        }
    }
}