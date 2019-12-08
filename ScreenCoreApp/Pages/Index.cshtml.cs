using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ScreenCoreApp.Pages
{
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
            Screen.CheckScreenID(Request.Query, HttpContext);
                ViewData["Handler"] = "Default";
        }

        

        public string GeneratePreloadHtmlCode() // should be called on startup and generate preload table
        {
            return "<td>generated text</td>";
        }
    }
}