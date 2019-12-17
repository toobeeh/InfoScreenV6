using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ScreenCoreApp
{
    public class ConsultationsModel : PageModel
    {

        public int Pagenum;
        public void OnGet(string pagenum)
        {
            //Pagenum = Convert.ToInt32(page);
        }
    }
}