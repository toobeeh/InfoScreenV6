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
    public class IndexModel : PageModel
    {

        public void OnGet()
        {
            Screen.CheckScreenID(Request.Query, HttpContext);
            ViewData["ID"] = "Eingegebene ID per QS:" + Screen.GetSessionScreenID(HttpContext);

            int screenID = Screen.GetSessionScreenID(HttpContext);
            if (screenID < 0) Response.Redirect("NoContent");

            // Refresh mode depending on ZGA (if active)
            TimeSensitiveMode.ZGAAbfragen(screenID);

            // Check mode page-cycle index per cookies

            Structuren.AnzeigeElemente pages = DatenbankAbrufen.AnzuzeigendeElemente(Convert.ToString(screenID));



        }
    }
}