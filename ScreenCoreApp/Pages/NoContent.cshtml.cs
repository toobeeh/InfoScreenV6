using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ScreenCoreApp.Pages
{
    public class NoContentModel : PageModel
    {
        public NoContentModel()
        {   

        }

        public void OnGet()
        {
            Screen.CheckScreenID(Request.Query, HttpContext);
            ViewData["ID"] = "Eingegebene ID per QS:" + Screen.GetSessionScreenID(HttpContext);
            ViewData["Error"] = Request.Query["error"];
        }
    }
}