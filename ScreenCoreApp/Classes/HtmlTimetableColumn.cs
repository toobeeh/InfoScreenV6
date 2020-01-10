using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infoscreen_Verwaltung;
using Infoscreen_Verwaltung.classes;

namespace ScreenCoreApp.Classes
{
    public class HtmlTimetableColumn
    {
        Structuren.StundenplanTag Day;
        List<Structuren.StundenplanEntry> Moved_lessons;
        int TotalLessons;
        bool ZerothLesson;
        const double DayHeight = 1.8;
        const double DateHeight = 1.5;
        double LessonHeight;

        public List<string> HtmlTableData = new List<string>();


        public HtmlTimetableColumn(Structuren.StundenplanTag _day, List<Structuren.StundenplanEntry> moved_lessons, bool _zerothLesson = false, int _totalLessons = 12)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
            Day = _day;
            Moved_lessons = moved_lessons;
            TotalLessons = _totalLessons;
            ZerothLesson = _zerothLesson;
            // calc height depending on displayed lessons, unit vh, container has 80 vh
            LessonHeight = (80 - DayHeight - DateHeight) / TotalLessons;
            ResizeContainer();
            CreateTableMarkup();
        }



        private void ResizeContainer()
        {
            //Resize Array so that lesson 0 to 11 is present
            Structuren.StundenplanEntry[] lessons = new Structuren.StundenplanEntry[TotalLessons];
            for(int lessons_index=0; lessons_index < lessons.Length; lessons_index++)
            {
                for(int day_index = 0; day_index < Day.StundenDaten.Length; day_index++)
                {
                    if (Day.StundenDaten[day_index].Stunde == lessons_index) lessons[lessons_index] = Day.StundenDaten[day_index];
                }
            }
            Day.StundenDaten = lessons;

            // Fill up MovedLessons so the lessons are on their rigth index (compatible with the index of Day.StundenDaten)
            List<Structuren.StundenplanEntry> moved_filled = (new Structuren.StundenplanEntry[Day.StundenDaten.Length]).ToList();
            Moved_lessons.ForEach((lesson) =>
                {
                    moved_filled[lesson.Stunde-1] = lesson;
                }
            );

            Moved_lessons = moved_filled;
        }

        public void CreateTableMarkup()
        {
            // Generates Markup which can be later used to fill into a table data

            HtmlTableData.Add("<div class='td' style='height:" + DayHeight + "vh'><div class='timetable_headDay'> <div>" + 
                Day.Datum.ToString("dddd",new System.Globalization.CultureInfo("de-DE")) + "</div> </div> </div> \n");
            HtmlTableData.Add("<div class='td' style='height:" + DateHeight + "vh'><div class='timetable_headDate'> <div>" + Day.Datum.ToString("dd.MM.yy") + "</div> </div> </div>\n");

            List<Structuren.StundenplanEntry> lessons = Day.StundenDaten.ToList();
           for(int i=0; i<lessons.Count; i++)
            {
                Structuren.StundenplanEntry lesson = lessons[i];
                if (i == 0 && !ZerothLesson) continue;
                
                // If a lesson from another day moved here
                if (!String.IsNullOrEmpty(Moved_lessons[i].Lehrer))
                    HtmlTableData.Add("<div class='td' style='height:" + LessonHeight + "vh'><div class='timetable_lessonMovedIn'> \n <div class='timetable_subjectInfo'>" +
                            Moved_lessons[i].Fach + "</div> \n <div class='timetable_teacherInfo'>" +
                            Moved_lessons[i].Lehrer + "</div> \n <div class='timetable_lessonMovedInfo'>" +
                            "Von " + Moved_lessons[i].ZiehtVorDatum.ToString("dd.MM") + "</div> </div> \n" +
                            "</div> \n");
                // if there is no entry on this lesson
                else if (String.IsNullOrEmpty(lesson.Fach))
                    HtmlTableData.Add("<div class='td' style='height:" + LessonHeight + "vh'><div class='timetable_noLesson'> \n <div style ='opacity: 0; background-color: transparent' class='timetable_subjectInfo'>" +
                            "Lesson_Fill" + "</div> \n <div style ='opacity: 0; background - color: divansparent' class='timetable_teacherInfo'>" +
                            "Teacher_Fill" + "</div> \n" +
                            "</div> </div>\n");
                // if lesson is cancelled
                else if (lesson.Entfällt)
                    HtmlTableData.Add("<div class='td' style='height:" + LessonHeight + "vh'><div class='timetable_lessonCancelled'> \n <div class='timetable_subjectInfo'>" +
                            lesson.Fach + "</div> \n <div class='timetable_teacherInfo'>" +
                            lesson.Lehrer + "</div> \n" +
                            "</div> </div>\n");
                // if lesson moved to another day
                else if (lesson.ZiehtVor >= 0)
                    HtmlTableData.Add("<div class='td' style='height:" + LessonHeight + "vh'><div class='timetable_lessonMovedOut'> \n <div class='timetable_subjectInfo'>" +
                            lesson.Fach + "</div> \n <div class='timetable_teacherInfo'>" +
                            lesson.Lehrer + "</div> \n <div class='timetable_lessonMovedInfo'>" +
                            "Auf " + lesson.ZiehtVorDatum.ToString("dd.MM") + "</div>  \n" +
                            "</div> </div>\n");
                // if normal lesson
                else
                    HtmlTableData.Add("<div class='td' style='height:" + LessonHeight + "vh'><div class='timetable_lesson'> \n <div class='timetable_subjectInfo'>" +
                            lesson.Fach + "</div> \n <div class='timetable_teacherInfo'>" +
                            lesson.Lehrer + "</div> \n" +
                            "</div> </div>\n");
                
            }
        }


    }
}
