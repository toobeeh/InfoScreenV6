using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Infoscreen_Verwaltung.classes;
using System.IO;

namespace ScreenCoreApp.Pages
{
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
            Screen.CheckScreenID(Request.Query, HttpContext);
            ViewData["Handler"] = "Default";

            PreloadPowerpoints();
        }

        private List<string> PptPicturePaths;

        private void PreloadPowerpoints()
        {
            PptPicturePaths = new List<string>();

            int screenID = Screen.GetSessionScreenID(HttpContext);
            if (screenID < 1) return;

            string departmentName = DatenbankAbrufen.BildschirmInformationenAbrufen(screenID).Abteilung;

            int modeID = DatenbankAbrufen.AktuellenBetriebsmodeAbrufen(departmentName);

            int powerpointID = DatenbankAbrufen.AnzuzeigendeElemente(screenID.ToString()).PowerPoints;
            if (powerpointID == -1) return;

            string presentationRoot = Path.Combine(new string[] { @"D:\infoscreen_publish\Screen\Presentations\", modeID.ToString(), powerpointID.ToString() });

            PptPicturePaths = Directory.GetFiles(presentationRoot, "*.png").ToList();

        }

        public string GeneratePreloadHtmlCode() // should be called on startup and generate preload table
        {
            string preloadMarkup = "";

            PptPicturePaths.ForEach((string path) =>
            {
                preloadMarkup += "<td id='" + Path.GetFileNameWithoutExtension(path) + "> <image src=" + path + "> </td>";
            });

            return preloadMarkup;
        }
    }
}