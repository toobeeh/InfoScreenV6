using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ScreenCoreApp.Classes;
using Infoscreen_Verwaltung.classes;
using System.IO;
using System.Data;

namespace ScreenCoreApp.Pages
{
    public class InitModel : PageModel
    {

        public IActionResult OnGet()
        {
            Screen.CheckScreenID(Request.Query, HttpContext);
            ViewData["ID"] = "Eingegebene ID per QS:" + Screen.GetSessionScreenID(HttpContext);


            int screenID = Screen.GetSessionScreenID(HttpContext);
            if (screenID < 0)
            {
                return Redirect("NoContent/?error=400");
            }

            // Check if exam in room is active (highest priority)
            Structuren.Rauminfo room = DatenbankAbrufen.RauminfoAbrufen(screenID.ToString());
            Structuren.Tests exam = DatenbankAbrufen.ExamInRoom(room.Gebäude + "-" + room.Raumnummer);

            // If subject isnt empt, redirect to exam screen
            if (!String.IsNullOrEmpty(exam.Fach) )
            {
                return Redirect("/ContentPages/ExamWarning");
            }

            // Refresh mode depending on ZGA (if active)
            TimeSensitiveMode.ZGAAbfragen(screenID);

            // Check mode page-cycle index per cookies

            Structuren.AnzeigeElemente pages = DatenbankAbrufen.AnzuzeigendeElemente(Convert.ToString(screenID));
            int cycle_index = Screen.GetPageCycleIndex(HttpContext);

            if (cycle_index == -1) SetCycleIndex(ref cycle_index, 1); // Initialize cycle variable
            else // If already initialized, get next mode
            {
                int consPage = Screen.GetConsultationsPage(HttpContext);
                int roomPage = Screen.GetRoomTablePage(HttpContext);
                int repPage = Screen.GetReplacementsPage(HttpContext);


                bool show_running = Screen.GetPresentationRunningStatus(HttpContext);
                bool consultations_running = consPage > 1 ? true: false;
                bool roomtable_running = roomPage > 1 ? true : false;
                bool replacements_running = repPage > 1 ? true : false;

                if (show_running)
                {
                    return RedirectToSlide();
                }
                else if (consultations_running)
                {
                    return RedirectToConsultations();
                }
                else if (roomtable_running)
                {
                   return  RedirectToRoomTable();
                }
                else if (replacements_running)
                {
                    return RedirectToReplacements();
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
                    return Redirect("/ContentPages/Timetable");

                case 2: // Department information
                    return Redirect("/ContentPages/DepartmentInfo");

                case 3: //Consultation table
                    return RedirectToConsultations();                    

                case 4: // Room table
                    return RedirectToRoomTable();

                case 5: // Teacher replacements
                    return RedirectToReplacements();

                case 6:
                    return Redirect("NoContent/?error=410");

                case 7: // Powerpoint
                    return RedirectToSlide();

                default:
                    return Redirect("NoContent/?error=410");
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

        private IActionResult RedirectToSlide()
        {
            int slide = Screen.GetNextPresentationSlide(HttpContext);
            string presentation;
            
            int count = GetSlideCount(out presentation);

            if (slide >= count ) Screen.SetNextPresentationSlide(1, HttpContext);
            else Screen.SetNextPresentationSlide(slide + 1, HttpContext);

            return Redirect("ContentPages/PowerPoint/" + slide.ToString() + "_" + presentation);
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


            string presentationRoot = Path.Combine(new string[] {Properties.Resources.pptPath, modeID.ToString(),powerpointID.ToString() });
            presentation = powerpointID.ToString();
            return Directory.GetFiles(presentationRoot, "*.png").ToList().Count;
        }

        private IActionResult RedirectToConsultations()
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

            return Redirect("ContentPages/Consultations/" + page);
        }

        private IActionResult RedirectToRoomTable()
        {
            int page = Screen.GetRoomTablePage(HttpContext);
            int pages;
            int screenID = Screen.GetSessionScreenID(HttpContext);
            string departmentName = DatenbankAbrufen.BildschirmInformationenAbrufen(screenID).Abteilung;
            int depID = DatenbankAbrufen.GetAbteilungsIdVonAbteilungsname(departmentName);

            DataTable Rooms = DatenbankAbrufen.RoomList(depID);
            int rows = Rooms.Rows.Count;

            pages = rows / 22 + (rows % 22 > 0 ? 1 : 0);

            if (page < pages) Screen.SetRoomTablePage((page + 1).ToString(), HttpContext);
            else Screen.SetRoomTablePage("1", HttpContext);

            return Redirect("ContentPages/Rooms/" + page);
        }

        private IActionResult RedirectToReplacements()
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
                return Redirect("ContentPages/Replacements/0");
            }

            // Layout: Classic replacements, 1 spacer line, globals header, globals - all globals have to fit on one page
            total_rows = globals.Length > 0 ? ((replacements.Length % 10 > 8-globals.Length ? ((replacements.Length+9) / 10) * 10 : replacements.Length) + globals.Length + 2) : replacements.Length;

            pages = total_rows / 10 + (total_rows % 10 > 0 ? 1 : 0);

            if (page < pages) Screen.SetReplacementsPage((page + 1).ToString(), HttpContext);
            else Screen.SetReplacementsPage("1", HttpContext);

            return Redirect("ContentPages/Replacements/" + page);
        }


    }
}