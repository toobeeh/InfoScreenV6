using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Infoscreen_Verwaltung.classes;

namespace ScreenCoreApp
{
    public class ExamWarningModel : PageModel
    {
        public Structuren.Tests Exam;

        public void OnGet()
        {
            int screenID = Screen.GetSessionScreenID(HttpContext);

            Structuren.Rauminfo room = DatenbankAbrufen.RauminfoAbrufen(screenID.ToString());
            Exam = DatenbankAbrufen.ExamInRoom(room.Gebäude + "-" + room.Raumnummer);

        }
    }
}