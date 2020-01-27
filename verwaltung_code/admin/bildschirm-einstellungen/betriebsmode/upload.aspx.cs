using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Infoscreen_Verwaltung.classes;

namespace Infoscreen_Verwaltung.admin.bildschirm_einstellungen.betriebsmode
{
    public partial class upload : System.Web.UI.Page
    {
        string ebene = "../../../";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!classes.Login.Angemeldet)
            {
                Session.Clear();
                Response.Redirect("../../../login.aspx");
            }
            if (classes.Login.Rechte.Bildschirme[2].ToInt32() < 2)
                btAbbrechen_Click(btAbbrechen, new EventArgs());

            TopMenu.Text = classes.Anzeige.TopBar(ebene);
            Menu.Text = classes.Anzeige.Menue(ebene, "admin");

            if (Session["21_Daten"] == null)
            {
                Response.Redirect("../");
                return;
            }
        }

        protected void btHochladen_Click(object sender, EventArgs e)
        {
            string fileende = file.HasFile ? file.FileName.Split('.')[file.FileName.Split('.').Length - 1] : "";
            List<string> fehler = new List<string>();
            if (!file.HasFile) fehler.Add("Keine Datei ausgewählt");
            if (fileende != "pptx" && fileende != "ppt")
                fehler.Add("Es sind nur PowerPoint Dateien gültig");
            if (tbName.Text == "") fehler.Add("Es muss ein Name angegeben werden");

            if (fehler.Count > 0)
            {
                lbFehler.Text = "<ul><li>" + String.Join("</li><li>", fehler) + "</li></ul>";
                return;
            }
            lbFehler.Text = "";

            int id = DatenbankSchreiben.NeueDatei(((Structuren.Betriebsmodi)Session["21_Daten"]).id, tbName.Text);
            string pfad = Properties.Resources.speicherort + ((Structuren.Betriebsmodi)Session["21_Daten"]).id.ToString() + "\\" + id.ToString() + "\\";
            System.IO.Directory.CreateDirectory(pfad);
            file.SaveAs(pfad + "Datei." + fileende);

            PowerPoint.ZuBild(pfad, "Datei." + fileende);
            Response.Redirect("./");
        }

        protected void btAbbrechen_Click(object sender, EventArgs e)
        {
            Response.Redirect("./");
        }
    }
}