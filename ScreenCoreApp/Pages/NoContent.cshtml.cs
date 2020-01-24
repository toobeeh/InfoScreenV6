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
            
            ViewData["Error"] = Request.Query["error"];

            string error = Request.Query["error"];
            if (error == "400") ViewData["error_hint"] = "Bad Request <br> The screen ID has to be provided via querystring <br> Example request: elv-infoscreen/?id=4";
            else if (error == "404") ViewData["error_hint"] = "Not found <br> The entered page could not be found";
            else if (error == "410") ViewData["error_hint"] = "Gone <br>This resource has been removed permanently";
            else if (error == "418") ViewData["error_hint"] = "I'm a teapot <br>Probably this screen doesnt belong to a class";
        }
    }
}