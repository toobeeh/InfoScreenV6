using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Infoscreen_Verwaltung.classes;
using System.Timers;

namespace Infoscreen_Verwaltung.admin.converter
{
    public partial class Converter : System.Web.UI.Page
    {
        string ebene = "../../";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!classes.Login.Angemeldet) Response.Redirect("../../login.aspx");
            if (!classes.Login.Rechte.Bildschirme.OneBiggerThan(1)) Response.Redirect("../../");

            TopMenu.Text = classes.Anzeige.TopBar(ebene);
            Menu.Text = classes.Anzeige.Menue(ebene, "admin");

            FillRunFileContents();
        }

        private void FillRunFileContents()
        {
            string linebreak = "<br />";
            string dir = @"D:\infoscreen_publish\PPT2PNG\";

            string run = "";
            if (File.Exists(dir + "run.txt")) run = File.ReadAllText(dir + "run.txt");
            List<string> runInfo = run.Split('\n').ToList<string>();

            string convert = "";
            if (File.Exists(dir + "convert.txt")) convert = File.ReadAllText(dir + "convert.txt");
            List<string> convertInfo = convert.Split('\n').ToList<string>();

            if (runInfo.Count > 1 && (DateTime.Now - DateTime.ParseExact(runInfo[0], "MM/dd/yyyy HH:mm:ss",null)).TotalSeconds < 20)
            {
                Status.Text += "Aktueller Status: Aktiv" + linebreak;
                Status.Text += "Ausführender Benutzer: " + runInfo[1] + linebreak;
            }
            else Status.Text += "Aktueller Status: Beendet" + linebreak;

            Status.Text += "\n\nAusstehende Konvertierungen:" + linebreak;

            if (convertInfo.Count == 0) Status.Text = " - Keine Konvertierung ausstehend";

            foreach (string line in convertInfo)
            {
                if (line.Length > 1) Status.Text += "- " + line + linebreak;
            }
        }



    }
}