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
        public string ClassName="";
        public List<Classes.HtmlTimetableColumn> Days;

        public void OnGet()
        {
            // Get data necessairy to fetch timetable
            int screenID = Screen.GetSessionScreenID(HttpContext);
            string departmentName = DatenbankAbrufen.BildschirmInformationenAbrufen(screenID).Abteilung;
            int depID = DatenbankAbrufen.GetAbteilungsIdVonAbteilungsname(departmentName);
            string defaultClass = ClassName = DatenbankAbrufen.RauminfoAbrufen(screenID.ToString()).Stammklasse;

            //Get timetable
            List<Structuren.StundenplanTag> timetable_days =  SubjectFunctions.StundenplanAbrufen(defaultClass, false).ToList();
            
            List<List<Structuren.StundenplanEntry>> moved_lessons_days = new List<List<Structuren.StundenplanEntry>>();

            //Get all moved lessons per day for each days in timetable
            timetable_days.ForEach((day) =>
            {
                moved_lessons_days.Add(DatenbankAbrufen.GetMovedLessonsOfDay(depID, defaultClass, day.Datum));
            });

            //Check if a zeroth lesson should be displayed
            bool zerothLesson = false;
            timetable_days.ForEach((day) => 
            { 
                if (day.StundenDaten[0].Stunde == 0) zerothLesson = true; 
            });

            // Create HTML capable objects for each day
            List<Classes.HtmlTimetableColumn> columns = new List<HtmlTimetableColumn>();
            timetable_days.ForEach((day) =>
            {
                columns.Add(new HtmlTimetableColumn(day, moved_lessons_days[timetable_days.IndexOf(day)], zerothLesson));
            });

            Days = columns;

        }
    }
}