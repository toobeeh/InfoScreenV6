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

        public HtmlTimetableColumn(Structuren.StundenplanTag _day, List<Structuren.StundenplanEntry> moved_lessons, bool _zerothLesson = false, int _totalLessons = 12)
        {
            Day = _day;
            Moved_lessons = moved_lessons;
            TotalLessons = _totalLessons;
            ZerothLesson = _zerothLesson;
            ResizeContainer();
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

        public string CreateTableMarkup()
        {
            string html = "<div class='timetable_dayColumn'>\n";

            html += "<div class='timetable_headDay'> <div>" + Day.Datum.DayOfWeek + "</div> </div> \n";
            html += "<div class='timetable_headDate'> <div>" + Day.Datum.ToShortDateString() + "</div> </div> \n";

            List<Structuren.StundenplanEntry> lessons = Day.StundenDaten.ToList();
           for(int i=0; i<lessons.Count; i++)
            {
                Structuren.StundenplanEntry lesson = lessons[i];
                if (i == 0 && !ZerothLesson) continue;
                
                // If a lesson from another day moved here
                if (!String.IsNullOrEmpty(Moved_lessons[i].Lehrer))
                    html += "<div class='timetable_lessonMovedIn'> \n <div class='timetable_subjectInfo'>" +
                            Moved_lessons[i].Fach + "</div> \n <div class='timetable_teacherInfo'>" +
                            Moved_lessons[i].Lehrer + "</div> \n <div class='timetable_lessonMovedOut'>" +
                            "➜ Von " + Moved_lessons[i].ZiehtVorDatum.ToString("dd.MM") + "</div>  \n" +
                            "</div> \n";
                // if there is no entry on this lesson
                else if (String.IsNullOrEmpty(lesson.Fach))
                    html += "<div> \n <div style ='opacity: 0; background - color: divansparent' class='timetable_subjectInfo'>" +
                            "Lesson_Fill" + "</div> \n <div style ='opacity: 0; background - color: divansparent' class='timetable_teacherInfo'>" +
                            "Teacher_Fill" + "</div> \n" +
                            "</div> \n";
                // if lesson is cancelled
                else if (lesson.Entfällt)
                    html += "<div class='timetable_lessonCancelled'> \n <div class='timetable_subjectInfo'>" +
                            lesson.Fach + "</div> \n <div class='timetable_teacherInfo'>" +
                            lesson.Lehrer + "</div> \n" +
                            "</div> \n";
                // if lesson moved to another day
                else if (lesson.ZiehtVor >= 0)
                    html += "<div class='timetable_lessonMovedOut'> \n <div class='timetable_subjectInfo'>" +
                            lesson.Fach + "</div> \n <div class='timetable_teacherInfo'>" +
                            lesson.Lehrer + "</div> \n <div class='timetable_lessonMovedOut'>" +
                            "➜ Auf " + lesson.ZiehtVorDatum.ToString("dd.MM") + "</div>  \n" +
                            "</div> \n";
                // if normal lesson
                else
                    html += "<div class='timetable_lesson'> \n <div class='timetable_subjectInfo'>" +
                            lesson.Fach + "</div> \n <div class='timetable_teacherInfo'>" +
                            lesson.Lehrer + "</div> \n" +
                            "</div> \n";
                
            }
        
           

            html += "</div>\n";

            return html;
        }


    }
}
