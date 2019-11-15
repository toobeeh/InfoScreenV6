using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;

namespace Infoscreen_Verwaltung
{
    public partial class _default : System.Web.UI.Page
    {
        string ebene = "./";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!classes.Login.Angemeldet) Response.Redirect("./login.aspx");
            TopMenu.Text = classes.Anzeige.TopBar(ebene);
            Menu.Text = classes.Anzeige.Menue(ebene, "");
        }

        protected void Click(object o, EventArgs e)
        {
            var secure = new System.Security.SecureString();
            foreach (char c in "HTC_Sense_8") // lol... sicher af, bitte nicht missbrauchen XDDD
            {
                secure.AppendChar(c);
            }


            Process x = new Process ();
            x.StartInfo.UserName = "20150562";
            x.StartInfo.Password = secure;

            x.StartInfo.FileName = @"D:\infoscreen_publish\PPT2PNG\PPT2PNG.exe";
            x.StartInfo.RedirectStandardOutput = true;
            x.StartInfo.UseShellExecute = false;
            x.StartInfo.Arguments = @"D:\infoscreen_publish\Screen\presentations\21\10343";
            x.Start();
            string line = x.StandardOutput.ReadToEnd();
            line = line + ".";
            ((Button)o).Text = line;
        }
        
    }
}