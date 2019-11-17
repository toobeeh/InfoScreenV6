using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;
using System.Security;
using System.IO;

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
            Process converter = new Process();

            converter.StartInfo.FileName = @"D:\infoscreen_publish\PPT2PNG\PPT2PNG.exe";
            converter.StartInfo.Arguments = @"D:\infoscreen_publish\Screen\presentations\21\10343";
            converter.StartInfo.LoadUserProfile = true;
            converter.Start();

            converter.WaitForExit();

        }


    }
}