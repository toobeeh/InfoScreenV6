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
    public class TimetableModel : PageModel
    {
        public void OnGet()
        {
            int screenID = Screen.GetSessionScreenID(HttpContext);
            string departmentName = DatenbankAbrufen.BildschirmInformationenAbrufen(screenID).Abteilung;
            int depID = DatenbankAbrufen.GetAbteilungsIdVonAbteilungsname(departmentName);
            string defaultClass = DatenbankAbrufen.RauminfoAbrufen(screenID.ToString()).Stammklasse;

            List<Structuren.StundenplanTag> timetable_days =  SubjectFunctions.StundenplanAbrufen(defaultClass, false).ToList();

            List<List<Structuren.StundenplanTagLehrer>> moved_lessons_days = new List<List<Structuren.StundenplanTagLehrer>>();

            timetable_days.ForEach((day) =>
            {
                moved_lessons_days.Add(DatenbankAbrufen.GetMovedLessonsOfDay(depID, defaultClass, day.Datum));
            });

        }
    }
}