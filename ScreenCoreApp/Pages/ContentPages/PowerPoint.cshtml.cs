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

        private int Slide;
        public void OnGet(int slide)
        {
            Slide = slide;
        }

        public string GetPresentationSource()
        {
            return Slide.ToString();
        }

       
    }
}