using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ScreenCoreApp.Classes;
using Infoscreen_Verwaltung.classes;
using System.IO;

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
                bool show_running = Screen.GetPresentationRunningStatus(HttpContext);
                bool consultations_running = Screen.GetConsultationsPage(HttpContext) > 1 ? true: false;
                bool roomtable_running = Screen.GetRoomTablePage(HttpContext) > 1 ? true : false;
                bool replacements_running = Screen.GetReplacementsPage(HttpContext) > 1 ? true : false;

                if (show_running)
                {
                    RedirectToSlide();
                    return;
                }
                else if (consultations_running)
                {
                    RedirectToConsultations();
                    return;
                }
                else if (roomtable_running)
                {
                    RedirectToRoomTable();
                    return;
                }
                else if (replacements_running)
                {
                    RedirectToReplacements();
                    return;
                }
                else
                {
                    GetNextCylceIndex(pages, ref cycle_index);
                    SetCycleIndex(ref cycle_index, cycle_index);   
                }          
            }
            

            switch (cycle_index)
            {
                case 1: // Timetable
                    //Response.Redirect("");
                    ViewData["ScreenMode"] = "Timetable";
                    break;
                case 2: // Department information
                    Response.Redirect("/ContentPages/DepartmentInfo");
                    ViewData["ScreenMode"] = "Department Information";
                    break;
                case 3: //Consultation table
                    RedirectToConsultations();
                    ViewData["ScreenMode"] = "Consultation table";
                    break;
                case 4: // Room table
                    RedirectToRoomTable();
                    ViewData["ScreenMode"] = "Room table";
                    break;
                case 5: // Teacher replacements
                    RedirectToReplacements();
                    ViewData["ScreenMode"] = "Teacher replacements";
                    break;
                case 6:
                    //Response.Redirect("");
                    ViewData["ScreenMode"] = "...";
                    break;
                case 7: // Powerpoint
                    RedirectToSlide();
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

        private void RedirectToSlide()
        {
            int slide = Screen.GetNextPresentationSlide(HttpContext);
            string presentation;
            
            int count = GetSlideCount(out presentation);

            if (slide >= count ) Screen.SetNextPresentationSlide(1, HttpContext);
            else Screen.SetNextPresentationSlide(slide + 1, HttpContext);

            Response.Redirect("ContentPages/PowerPoint/" + slide.ToString() + "_" + presentation);
        }

        private int GetSlideCount(out string presentation)
        {
            int screenID = Screen.GetSessionScreenID(HttpContext);
            string departmentName = DatenbankAbrufen.BildschirmInformationenAbrufen(screenID).Abteilung;
            int modeID = DatenbankAbrufen.AktuellenBetriebsmodeAbrufen(departmentName);
            
            Structuren.AnzeigeElemente elements = DatenbankAbrufen.AnzuzeigendeElemente(screenID.ToString());

            presentation = "";
            int powerpointID = elements.PowerPoints;
            if (powerpointID == -1) return -1;


            string presentationRoot = Path.Combine(new string[] { @"D:\infoscreen_publish\Screen\Presentations\", modeID.ToString(),powerpointID.ToString() });
            presentation = powerpointID.ToString();
            return Directory.GetFiles(presentationRoot, "*.png").ToList().Count;
        }

        private void RedirectToConsultations()
        {
            int page = Screen.GetConsultationsPage(HttpContext);
            int pages;
            int screenID = Screen.GetSessionScreenID(HttpContext);
            string departmentName = DatenbankAbrufen.BildschirmInformationenAbrufen(screenID).Abteilung;
            int depID = DatenbankAbrufen.GetAbteilungsIdVonAbteilungsname(departmentName);

            Structuren.Sprechstunden[] entries = SubjectFunctions.SprechstundenAbrufen(depID.ToString());
            pages = entries.Length / 15 + (entries.Length % 15 > 0 ? 1 : 0);

            if (page < pages) Screen.SetConsultationsPage((page + 1).ToString(), HttpContext);
            else Screen.SetConsultationsPage("1", HttpContext);

            Response.Redirect("ContentPages/Consultations/" + page);
        }

        private void RedirectToRoomTable()
        {
            int page = Screen.GetRoomTablePage(HttpContext);
            int pages;
            int screenID = Screen.GetSessionScreenID(HttpContext);
            string departmentName = DatenbankAbrufen.BildschirmInformationenAbrufen(screenID).Abteilung;
            int depID = DatenbankAbrufen.GetAbteilungsIdVonAbteilungsname(departmentName);
            Structuren.Raumaufteilung[] rooms = DatenbankAbrufen.RaumaufteilungAbrufen(depID.ToString()); 
            pages = rooms.Length / 10 + (rooms.Length % 10 > 0 ? 1 : 0);

            if (page < pages) Screen.SetRoomTablePage((page + 1).ToString(), HttpContext);
            else Screen.SetRoomTablePage("1", HttpContext);

            Response.Redirect("ContentPages/Rooms/" + page);
        }

        private void RedirectToReplacements()
        {
            int page = Screen.GetReplacementsPage(HttpContext);
            int pages, total_rows = 0;
            int screenID = Screen.GetSessionScreenID(HttpContext);
            string departmentName = DatenbankAbrufen.BildschirmInformationenAbrufen(screenID).Abteilung;
            int depID = DatenbankAbrufen.GetAbteilungsIdVonAbteilungsname(departmentName);
            Structuren.Supplierungen[] replacements;
            Structuren.GlobalEntfall[] globals = DatenbankAbrufen.GetGlobaleEntfaelle(depID);

            try
            {
                replacements = DatenbankAbrufen.SupplierplanAbrufen(departmentName)[0].Supplierungen;
            }
            catch
            {
                Response.Redirect("ContentPages/Replacements/0");
                return;
            }

            // Layout: Classic replacements, 1 spacer line, globals header, globals - all globals have to fit on one page
            total_rows = globals.Length > 0 ? ((replacements.Length % 10 > 8-globals.Length ? ((replacements.Length+9) / 10) * 10 : replacements.Length) + globals.Length + 2) : replacements.Length;

            pages = total_rows / 10 + (total_rows % 10 > 0 ? 1 : 0);

            if (page < pages) Screen.SetReplacementsPage((page + 1).ToString(), HttpContext);
            else Screen.SetReplacementsPage("1", HttpContext);

            Response.Redirect("ContentPages/Replacements/" + page);
        }


    }
}