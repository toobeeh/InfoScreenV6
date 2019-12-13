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

            Screen.SetNextPresentationSlide(1, HttpContext); // Presentation begins at slide 1
            PptPicturePaths = new List<string>();
            pptID = 0;

            PreloadPowerpoints();
        }


        private List<string> PptPicturePaths;
        private int pptID;

        private void PreloadPowerpoints()
        {
            
            int screenID = Screen.GetSessionScreenID(HttpContext);
            if (screenID < 1) return;

            string departmentName = DatenbankAbrufen.BildschirmInformationenAbrufen(screenID).Abteilung;

            int modeID = DatenbankAbrufen.AktuellenBetriebsmodeAbrufen(departmentName);

            Structuren.AnzeigeElemente elements = DatenbankAbrufen.AnzuzeigendeElemente(screenID.ToString());

            int powerpointID = elements.PowerPoints;
            if (powerpointID == -1) return;

            string presentationRoot = Path.Combine(new string[] { @"D:\infoscreen_publish\Screen\Presentations\", modeID.ToString(), powerpointID.ToString() });

            PptPicturePaths = Directory.GetFiles(presentationRoot, "*.png").ToList();
            pptID = powerpointID;

        }

        public string GeneratePreloadHtmlCode() // should be called on startup and generate preload table
        {
            string preloadMarkup = "";

            PptPicturePaths.ForEach((string path) =>
            {
                // new tabledata with presentation and slide number as ID and refers to the sub-website /presentations
                // sample: <td id="1"> <img src="Presentations\21\10359\1.png"> </td>
                preloadMarkup += "<td id='"  + pptID + "/" + Path.GetFileNameWithoutExtension(path)  + "'> <img src='\\" + path.Substring(path.IndexOf("Presentations")) + "'> </td>";
            });

            return preloadMarkup;
        }
    }
}