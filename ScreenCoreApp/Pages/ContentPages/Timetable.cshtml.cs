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
        public List<Structuren.Tests> Exams;
        public int LessonCount;
        public bool ZerothLesson;
        public string ClassInformation;
        public Structuren.Klasseneigenschaften Properties;

        Dictionary<DateTime, List<Structuren.Tests>> ExamDays = new Dictionary<DateTime, List<Structuren.Tests>>();

        public IActionResult OnGet()
        {
            // Get data necessairy to fetch timetable
            int screenID = Screen.GetSessionScreenID(HttpContext);
            string departmentName = DatenbankAbrufen.BildschirmInformationenAbrufen(screenID).Abteilung;
            int depID = DatenbankAbrufen.GetAbteilungsIdVonAbteilungsname(departmentName);
            string defaultClass = ClassName = DatenbankAbrufen.RauminfoAbrufen(screenID.ToString()).Stammklasse;

            if (String.IsNullOrEmpty(defaultClass))
            {
                return Redirect("/NoContent/?error=418");
            }


            //Get class information
            ClassInformation = DatenbankAbrufen.KlasseninfoAbrufen(defaultClass);

            //Get class properties
            Properties = DatenbankAbrufen.GetClassProperties(ClassName, depID);

            //Get Tests
            Exams = DatenbankAbrufen.TestsAbrufen(ClassName, false, true).ToList();

            //Get timetable
            List<Structuren.StundenplanTag> timetable_days =  SubjectFunctions.StundenplanAbrufen(defaultClass, false).ToList();
            
            List<List<Structuren.StundenplanEntry>> moved_lessons_days = new List<List<Structuren.StundenplanEntry>>();

            //Get all moved lessons per day for each day in timetable
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
            ZerothLesson = zerothLesson;

            //Get highest amount of lessons per day
            int maxLesson = 0;
            timetable_days.ForEach((day) =>
            {
                if (day.StundenDaten[day.StundenDaten.Length-1].Stunde > maxLesson) maxLesson = day.StundenDaten[day.StundenDaten.Length - 1].Stunde;
            });
            LessonCount = maxLesson + (zerothLesson ? 1 : 0);

            //Fill Test-Day Dictionary
            Exams.ForEach((exam) =>
            {
                if (ExamDays.ContainsKey(exam.Datum)) ExamDays[exam.Datum].Add(exam);
                else
                {
                    ExamDays[exam.Datum] = new List<Structuren.Tests>();
                    ExamDays[exam.Datum].Add(exam);
                }
            });

            // Create HTML capable objects for each day
            List<Classes.HtmlTimetableColumn> columns = new List<HtmlTimetableColumn>();
            timetable_days.ForEach((day) =>
            {
                columns.Add(new HtmlTimetableColumn(day, (ExamDays.ContainsKey(day.Datum) ? ExamDays[day.Datum] : new List<Structuren.Tests>()), moved_lessons_days[timetable_days.IndexOf(day)], zerothLesson, LessonCount));
            });

            Days = columns;

            return Page();
        }
    }
}