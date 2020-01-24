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
    public class DepartmentInfoModel : PageModel
    {

        public string InfoHtmlMarkup;
        public IActionResult OnGet()
        {
            int screenID = Screen.GetSessionScreenID(HttpContext);

            InfoHtmlMarkup = GeneralFunctions.ConvertBBtoHTML(DatenbankAbrufen.AbteilungsinfoAbrufen(screenID.ToString()));

            return Page();
        }
    }
}