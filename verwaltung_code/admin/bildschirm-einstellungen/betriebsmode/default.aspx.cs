using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Infoscreen_Verwaltung.classes;
using System.IO;

namespace Infoscreen_Verwaltung.admin.bildschirm_einstellungen.betriebsmode
{
    public partial class _default : System.Web.UI.Page
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

            überschrift.InnerText = "Betriebsmode '" + ((Structuren.Betriebsmodi)Session["21_Daten"]).bezeichnung + "' verwalten";

            BildschirmeAusgeben();
            DateienAusgeben();
        }

        private void BildschirmeAusgeben()
        {
            TableRow row = new TableRow();
            TableCell cell = new TableCell();

            cell = new TableCell();
            cell.Text = "ID";
            cell.CssClass = "head";
            row.Cells.Add(cell);

            cell = new TableCell();
            cell.Text = "Geb.";
            cell.CssClass = "head";
            row.Cells.Add(cell);

            cell = new TableCell();
            cell.Text = "Raum";
            cell.CssClass = "head";
            row.Cells.Add(cell);
 

            cell = new TableCell();
            cell.Text = "Klassen";
            cell.CssClass = "head";
            row.Cells.Add(cell);

            cell = new TableCell();
            cell.Text = "Std.-Plan";
            cell.CssClass = "head";
            row.Cells.Add(cell);

            cell = new TableCell();
            cell.Text = "Abt.-Info";
            cell.CssClass = "head";
            row.Cells.Add(cell);

            cell = new TableCell();
            cell.Text = "Sprechstd.";
            cell.CssClass = "head";
            row.Cells.Add(cell);

            cell = new TableCell();
            cell.Text = "Raumaufteilung";
            cell.CssClass = "head";
            row.Cells.Add(cell);

            cell = new TableCell();
            cell.Text = "Supplierplan";
            cell.CssClass = "head";
            row.Cells.Add(cell);

            cell = new TableCell();
            cell.Text = "PowerPoint";
            cell.CssClass = "head";
            row.Cells.Add(cell);

            tbBildschirme.Rows.Add(row);

            int[] bildschirme = DatenbankAbrufen.BildschirmeBetriebsmodeAbrufen(((Structuren.Betriebsmodi)Session["21_Daten"]).id);
            Structuren.AnzeigeElemente anzuzeigendeElemente;
            Structuren.Bildschirm bildschirmDaten;
            Structuren.Dateien[] powerPoints = DatenbankAbrufen.DateienAbrufen(((Structuren.Betriebsmodi)Session["21_Daten"]).id);

            foreach (int bildschirm in bildschirme)
            {
                anzuzeigendeElemente = DatenbankAbrufen.AnzuzeigendeElemente(bildschirm.ToString(), ((Structuren.Betriebsmodi)Session["21_Daten"]).id);
                bildschirmDaten = DatenbankAbrufen.BildschirmInformationenAbrufen(bildschirm);
                tbBildschirme.Rows.Add(new BetriebsmodeRow(bildschirm, bildschirmDaten.Gebäude, bildschirmDaten.Raum, bildschirmDaten.Klasse, bildschirmDaten.Abteilung, bildschirmDaten.AnzeigeArt, anzuzeigendeElemente, powerPoints));
            }

            Session["21_Bildschirme"] = bildschirme;
        }



        protected void btÜbernehmen_Click(object sender, EventArgs e)
        {
            int betriebId = ((Structuren.Betriebsmodi)Session["21_Daten"]).id;
            Structuren.AnzeigeElemente anzeigen;

            foreach (int bildschirm in (int[])Session["21_Bildschirme"])
            {
                anzeigen = new Structuren.AnzeigeElemente();
                anzeigen.StandardKlasse = Context.Request["ctl00$Content$id-" + bildschirm + "-kls"].ToBool();
                anzeigen.Stundenplan = Context.Request["ctl00$Content$id-" + bildschirm + "-stu"].ToBool();
                anzeigen.Abteilungsinfo = Context.Request["ctl00$Content$id-" + bildschirm + "-abt"].ToBool();
                anzeigen.Sprechstunden = Context.Request["ctl00$Content$id-" + bildschirm + "-spr"].ToBool();
                anzeigen.Raumaufteilung = Context.Request["ctl00$Content$id-" + bildschirm + "-rau"].ToBool();
                anzeigen.Supplierplan = Context.Request["ctl00$Content$id-" + bildschirm + "-sup"].ToBool();
                anzeigen.AktuelleSupplierungen = Context.Request["ctl00$Content$id-" + bildschirm + "-akt"].ToBool();
                anzeigen.PowerPoints = Context.Request["ctl00$Content$id-" + bildschirm + "-pow"].ToInt32();
                DatenbankSchreiben.AnzeigeeinstellungenUpdaten(betriebId, bildschirm, anzeigen);
            }
            Response.Redirect("./");
        }

        private void DateienAusgeben()
        {
            TableRow row = new TableRow();
            TableCell cell = new TableCell();

            cell.Text = "Dateiname";
            cell.CssClass = "head";
            row.Cells.Add(cell);

            cell = new TableCell();
            cell.CssClass = "head";
            cell.Width = 107;
            row.Cells.Add(cell);

            cell = new TableCell();
            cell.CssClass = "head";
            cell.Width = 77;
            row.Cells.Add(cell);

            cell = new TableCell();
            cell.CssClass = "head";
            cell.Width = 62;
            row.Cells.Add(cell);

            tbDateien.Rows.Add(row);

            dataButton button;

            foreach (Structuren.Dateien datei in DatenbankAbrufen.DateienAbrufen(((Structuren.Betriebsmodi)Session["21_Daten"]).id))
            {
                row = new TableRow();
                cell = new TableCell();
                cell.Text = datei.bezeichnung;
                row.Cells.Add(cell);

                cell = new TableCell();
                cell.HorizontalAlign = HorizontalAlign.Center;
                button = new dataButton();
                button.Text = "Downloaden";
                button.CssClass = "ActionButton";
                button.Click += btDateiDownloaden;
                button.Setting = datei;
                cell.Controls.Add(button);
                row.Cells.Add(cell);

                cell = new TableCell();
                cell.HorizontalAlign = HorizontalAlign.Center;
                button = new dataButton();
                button.Text = "Ändern";
                button.CssClass = "ActionButton";
                button.Click += btDateiÄndernClick;
                button.Setting = datei;
                button.Enabled = false;
                cell.Controls.Add(button);
                row.Cells.Add(cell);

                cell = new TableCell();
                cell.HorizontalAlign = HorizontalAlign.Center;
                button = new dataButton();
                button.Text = "Löschen";
                button.CssClass = "DeleteButton";
                button.Click += btDateiLöschenClick;
                button.Setting = datei;
                cell.Controls.Add(button);
                row.Cells.Add(cell);

                tbDateien.Rows.Add(row);
            }

            if (DatenbankAbrufen.DateienAbrufen(((Structuren.Betriebsmodi)Session["21_Daten"]).id).Length == 0)
            {
                tbDateien.Rows.Clear();
                row = new TableRow();
                cell = new TableCell();
                cell.Text = "Derzeit sind für diesen Betriebsmode noch keine Dateien hochgeladen worden.";
                row.Cells.Add(cell);
                tbDateien.Rows.Add(row);
            }
        }

        private void btDateiDownloaden(object sender, EventArgs e)
        {
            int betriebsmodeID = ((Structuren.Betriebsmodi)Session["21_Daten"]).id;
            int dateiID = ((Structuren.Dateien)((dataButton)sender).Setting).id;
            string dateiName = ((Structuren.Dateien)((dataButton)sender).Setting).bezeichnung;

            string filename = Properties.Resources.speicherort + betriebsmodeID.ToString() + "\\" + dateiID.ToString() + "\\Datei.ppt";
            FileInfo fileInfo = new FileInfo(filename);

            if (fileInfo.Exists)
            {
                Response.Clear();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + dateiName + ".ppt");
                Response.AddHeader("Content-Length", fileInfo.Length.ToString());
                Response.ContentType = "application/octet-stream";
                Response.Flush();
                Response.TransmitFile(fileInfo.FullName);
                Response.End();
            }
            else
            {
                filename += "x";
                fileInfo = new FileInfo(filename);
                if (fileInfo.Exists)
                {
                    Response.Clear();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + dateiName + ".pptx");
                    Response.AddHeader("Content-Length", fileInfo.Length.ToString());
                    Response.ContentType = "application/octet-stream";
                    Response.Flush();
                    Response.TransmitFile(fileInfo.FullName);
                    Response.End();
                }
            }
        }

        protected void btDateiÄndernClick(object sender, EventArgs e)
        {

            Response.Redirect("./");
        }

        protected void btDateiLöschenClick(object sender, EventArgs e)
        {
            Structuren.Dateien datei = (Structuren.Dateien)((dataButton)sender).Setting;
            DatenbankSchreiben.DateiLöschen(datei.id);
            Response.Redirect("./");
        }

        protected void btAbbrechen_Click(object sender, EventArgs e)
        {
            Response.Redirect("./");
        }



        protected void btNeueDatei_Click(object sender, EventArgs e)
        {
            Response.Redirect("./upload.aspx");
        }

        protected void btFertig_Click(object sender, EventArgs e)
        {
            Session["21_Daten"] = null;
            Session["21_Table"] = null;
            Session["21_Bildschirme"] = null;
            Session["21_Art"] = null;
            Response.Redirect("../");
        }
    }
}