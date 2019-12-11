using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ScreenCoreApp.Classes;
using Infoscreen_Verwaltung.classes;

namespace ScreenCoreApp.Pages
{
    public class InitModel : PageModel
    {

        public void OnGet()
        {
            Screen.CheckScreenID(Request.Query, HttpContext);
            ViewData["ID"] = "Eingegebene ID per QS:" + Screen.GetSessionScreenID(HttpContext);


            int screenID = Screen.GetSessionScreenID(HttpContext);
            if (screenID < 0)
            {
                Response.Redirect("NoContent");
                return;
            }


            // Refresh mode depending on ZGA (if active)
            TimeSensitiveMode.ZGAAbfragen(screenID);

            // Check mode page-cycle index per cookies

            Structuren.AnzeigeElemente pages = DatenbankAbrufen.AnzuzeigendeElemente(Convert.ToString(screenID));
            int cycle_index = Screen.GetPageCycleIndex(HttpContext);

            if (cycle_index == -1) SetCycleIndex(ref cycle_index, 1); // Initialize cycle variable
            else // If already initialized, get next mode
            {
                GetNextCylceIndex(pages, ref cycle_index);
                SetCycleIndex(ref cycle_index, cycle_index);
            }
            

            switch (cycle_index)
            {
                case 1: // Timetable
                    //Response.Redirect("");
                    ViewData["ScreenMode"] = "Timetable";
                    break;
                case 2: // Department information
                    //Response.Redirect("");
                    ViewData["ScreenMode"] = "Department Information";
                    break;
                case 3: //Consultation table
                    //Response.Redirect("");
                    ViewData["ScreenMode"] = "Consultation table";
                    break;
                case 4: // Room table
                    //Response.Redirect("");
                    ViewData["ScreenMode"] = "Room table";
                    break;
                case 5: // Teacher replacements
                    //Response.Redirect("");
                    ViewData["ScreenMode"] = "Teacher replacements";
                    break;
                case 6:
                    //Response.Redirect("");
                    ViewData["ScreenMode"] = "...";
                    break;
                case 7: // Powerpoint
                    //Response.Redirect("");
                    ViewData["ScreenMode"] = "Powerpoint";
                    break;
                default:
                    Response.Redirect("NoContent");
                    break;
            }
        }

        private void SetCycleIndex(ref int cycle_index, int new_index)
        {
            cycle_index = new_index;
            Screen.SetPageCycleIndex(new_index.ToString(), HttpContext);
        }

        private void GetNextCylceIndex(Structuren.AnzeigeElemente pages,ref int cycle_index)
        {
            List<bool> pageList = new List<bool>() { pages.Stundenplan, pages.Abteilungsinfo, pages.Sprechstunden, pages.Raumaufteilung, pages.Supplierplan, pages.AktuelleSupplierungen, (pages.PowerPoints > -1) };

            if(!pageList.Contains(true))
            {
                cycle_index = -1;
                return;
            }

            int i = cycle_index;
            do
            {
                i++;
                if (i > pageList.Count) i = 1;
            }
            while (!pageList[i-1]);

            cycle_index = i;
            return;
        }

    }
}