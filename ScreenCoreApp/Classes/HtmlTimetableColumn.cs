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
        List<Structuren.Tests> Exams;

        int TotalLessons;
        bool ZerothLesson;
        const double DayHeight = 1.8;
        const double DateHeight = 1.5;
        double LessonHeight;

        public List<string> HtmlTableData { get; private set; }


        public HtmlTimetableColumn(Structuren.StundenplanTag day, List<Structuren.Tests> exams, List<Structuren.StundenplanEntry> moved_lessons, bool _zerothLesson = false, int _totalLessons = 12)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
            Day = day;
            Moved_lessons = moved_lessons;
            TotalLessons = _totalLessons;
            ZerothLesson = _zerothLesson;
            Exams = exams;

            HtmlTableData = new List<string>();

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

            // Fill up MovedLessons and Exams so the lessons/exams are on their rigth index (compatible with the index of Day.StundenDaten)

            List<Structuren.StundenplanEntry> moved_filled = (new Structuren.StundenplanEntry[Day.StundenDaten.Length]).ToList();
            Moved_lessons.ForEach((lesson) =>
                {
                    moved_filled[lesson.Stunde] = lesson;
                }
            );
            Moved_lessons = moved_filled;

            List<Structuren.Tests> exams_filled = (new Structuren.Tests[Day.StundenDaten.Length]).ToList();
            Exams.ForEach((exam) =>
            {
                exams_filled[exam.Stunde] = exam;
            }
            );
            Exams = exams_filled;
        }

        public void CreateTableMarkup()
        {


            // Generates tabledata markup which can be later used to fill into a tablerow element

            HtmlTableData.Add("<div class='td' style='height:" + DayHeight + "vh'><div class='timetable_headDay'> <div>" + 
                Day.Datum.ToString("dddd",new System.Globalization.CultureInfo("de-DE")) + "</div> </div> </div> \n");
            HtmlTableData.Add("<div class='td' style='height:" + DateHeight + "vh'><div class='timetable_headDate'> <div>" + Day.Datum.ToString("dd.MM.yy") + "</div> </div> </div>\n");

            List<Structuren.StundenplanEntry> lessons = Day.StundenDaten.ToList();

            for(int i=0; i<lessons.Count; i++)
            {
                Structuren.StundenplanEntry lesson = lessons[i];
                if (i == 0 && !ZerothLesson) continue;

                string html = "";

                // unique day/lesson ID for tabledata
                string id = "id='" + Day.Datum.DayOfWeek.ToInt32() + "_" + i + "'";

                // class if lesson is active
                //string active = (DateTime.Now.DayOfWeek.ToInt32() == Day.Datum.DayOfWeek.ToInt32() && DatenbankAbrufen.AktuelleStunde() == i ?
                //    " timetable_activeLesson" : "");
                string active = (2 == Day.Datum.DayOfWeek.ToInt32() && 4 == i ?
                    " timetable_activeLesson" : "");


                // opening tag of td container with id and height
                html += "<div class='td " + active + "' " + id + " style='height:" + LessonHeight + "vh'>";

                //Create text span if exam is in lesson
                if (!String.IsNullOrEmpty(Exams[i].Fach)) 
                    html+= "<span class='timetable_exam' id='" + Exams[i].Datum.ToString("yyyyMMdd") + "-" + Exams[i].Fach + "'>TEST</span>";

                // lesson info container:

                // If a lesson from another day moved here
                if (!String.IsNullOrEmpty(Moved_lessons[i].Lehrer))
                    html +=     "<div class='timetable_lessonMovedIn'>" +
                                    "<div class='timetable_subjectInfo'>" +
                                        Moved_lessons[i].Fach + 
                                    "</div> " +
                                    "<div class='timetable_teacherInfo'>" +
                                        Moved_lessons[i].Lehrer + 
                                    "</div> " +
                                    "<div class='timetable_lessonMovedInfo'>" +
                                        "Von " + Moved_lessons[i].ZiehtVorDatum.ToString("dd.MM") + 
                                    "</div> " + 
                                "</div>";

                // if there is no entry on this lesson
                else if (String.IsNullOrEmpty(lesson.Fach))
                    html +=     "<div class='timetable_noLesson'> " +
                                    "<div style ='opacity: 0; background-color: transparent' class='timetable_subjectInfo'>" +
                                        "Lesson_Fill" + 
                                    "</div> " + 
                                    "<div style ='opacity: 0; background - color: divansparent' class='timetable_teacherInfo'>" +
                                        "Teacher_Fill" + 
                                    "</div>" +
                                "</div>";

                // if lesson is cancelled
                else if (lesson.Entfällt)
                    html +=     "<div class='timetable_lessonCancelled'> " +
                                    "<div class='timetable_subjectInfo'>" +
                                        lesson.Fach + 
                                    "</div> " +
                                    "<div class='timetable_teacherInfo'>" +
                                        lesson.Lehrer + 
                                    "</div> " +
                                "</div>";

                // if lesson moved to another day
                else if (lesson.ZiehtVor >= 0)
                    html +=     "<div class='timetable_lessonMovedOut'>" + 
                                    "<div class='timetable_subjectInfo'>" +
                                        lesson.Fach + 
                                    "</div>" +
                                    "<div class='timetable_teacherInfo'>" +
                                        lesson.Lehrer + 
                                    "</div> " +
                                    "<div class='timetable_lessonMovedInfo'>" +
                                        "Auf " + lesson.ZiehtVorDatum.ToString("dd.MM") + 
                                    "</div>" +
                                "</div>";

                // normal lesson
                else
                    html +=     "<div class='timetable_lesson'>" + 
                                    "<div class='timetable_subjectInfo'>" +
                                        lesson.Fach + 
                                    "</div>" + 
                                    "<div class='timetable_teacherInfo'>" +
                                        lesson.Lehrer + 
                                    "</div>" + 
                                "</div>";

                // close container
                html += "</div>";

                HtmlTableData.Add(html);
                
            }
        }
    }
}
