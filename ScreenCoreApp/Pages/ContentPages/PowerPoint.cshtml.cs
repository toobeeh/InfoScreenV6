using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace ScreenCoreApp.Pages.ContentPages
{
    public class PowerPointModel : PageModel
    {

        public string Slide;
        public string Webpath;
        public IActionResult OnGet(string slide)
        {
            Slide = slide.Substring(0,slide.IndexOf('_'));
            Webpath = slide.Substring(slide.IndexOf('_') + 1);
            Webpath = Webpath.Replace("-", "\\");

            return Page();
        }

    }
}