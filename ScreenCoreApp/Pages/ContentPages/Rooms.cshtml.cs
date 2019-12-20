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
    public class RoomsModel : PageModel
    {
        public int Pagenum;
        public int item_start;
        public int item_end;
        public Structuren.Raumaufteilung[] Rooms;
        public bool Break = false;
        public bool TeachingTime = true;

        public void OnGet(string pagenum)
        {
            if (DatenbankAbrufen.AktuelleStunde() == -2) { TeachingTime = false;  return; }

            Pagenum = Convert.ToInt32(pagenum);

            int screenID = Screen.GetSessionScreenID(HttpContext);
            string departmentName = DatenbankAbrufen.BildschirmInformationenAbrufen(screenID).Abteilung;
            int depID = DatenbankAbrufen.GetAbteilungsIdVonAbteilungsname(departmentName);

            Rooms = DatenbankAbrufen.RaumaufteilungAbrufen(depID.ToString());

            item_start = (Pagenum - 1) * 10;
            item_end = (item_start + 10 > Rooms.Length - 1 ? Rooms.Length - 1 : item_start + 10);
        }
    }
}