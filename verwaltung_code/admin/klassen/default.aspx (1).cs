using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Infoscreen_Verwaltung.classes;
using System.Drawing;

namespace Infoscreen_Verwaltung.admin.klassen
{
    public partial class _default : System.Web.UI.Page
    {
        string ebene = "../../";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!classes.Login.Angemeldet) Response.Redirect("../../login.aspx");
            if (!classes.Login.Rechte.Klassen.OneBiggerThan(1)) Response.Redirect("../../");

            TopMenu.Text = classes.Anzeige.TopBar(ebene);
            Menu.Text = classes.Anzeige.Menue(ebene);

            if (!IsPostBack)
            {
                string[] abteilungen;
                if (classes.Login.Rechte.Klassen[2].ToInt32() > 1)
                    abteilungen = classes.DatenbankAbrufen.AbteilungenAbrufen();
                else if (classes.Login.Rechte.Klassen[1].ToInt32() > 1)
                    abteilungen = classes.Login.Abteilungen;
                else abteilungen = new string[] { classes.Login.StammAbteilung };

                foreach (string abteilung in abteilungen)
                    dropDownAbteilung.Items.Add(abteilung);

                try { dropDownAbteilung.SelectedValue = classes.Login.StammAbteilung; }
                catch { dropDownAbteilung.SelectedIndex = -1; }

                if (Session["22_Abteilung"] != null)
                {
                    try { dropDownAbteilung.SelectedValue = Session["21_Abteilung"].ToString(); }
                    catch { }
                }

                Session["22_Abteilung"] = null;
            }

            TabelleZeichnen();

            if (classes.Login.Rechte.Klassen[2].ToInt32() > 2)
                btNeueKlasse.Enabled = true;
            else if (classes.Login.Rechte.Klassen[1].ToInt32() > 2 && classes.Login.StammAbteilung == dropDownAbteilung.SelectedValue)
                btNeueKlasse.Enabled = true;
            else
                btNeueKlasse.Enabled = false;

            //DEBUG!!!
            //btNeueKlasse.Enabled = false;

            if (classes.Login.Rechte.Klassen[2].ToInt32() > 3)
                btAlleKlassenLöschen.Enabled = true;
            else if (classes.Login.Rechte.Klassen[1].ToInt32() > 3 && classes.Login.StammAbteilung == dropDownAbteilung.SelectedValue)
                btAlleKlassenLöschen.Enabled = true;
            else
                btAlleKlassenLöschen.Enabled = false;
        }

        protected void HinweisZeigen(bool _zeigen = true)
        {
            if (_zeigen)
            {
                lbHinweis1.Text = lbHinweis2.Text = "Hinweis: Zum durchführen der gewünschten Aktion bitte den betreffenden Button erneut anklicken.";
                lbHinweis1.ForeColor = lbHinweis2.ForeColor = Color.Red;
                lbHinweis1.Visible = lbHinweis2.Visible = true;
            }
            else
            {
                lbHinweis1.Visible = lbHinweis2.Visible = false;
            }
        }

        protected void dropDownAbteilung_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabelleZeichnen();
        }

        protected void TabelleZeichnen()
        {
            tbKlassen.Rows.Clear();
            TableRow row = new TableRow();
            TableCell cell = new TableCell();
            cell.CssClass = "head";
            cell.Text = "Klasse";
            row.Cells.Add(cell);

            cell = new TableCell();
            cell.CssClass = "head";
            cell.Text = "Standardraum";
            row.Cells.Add(cell);

            cell = new TableCell();
            cell.CssClass = "head";
            cell.Text = "Klassensprecher";
            row.Cells.Add(cell);

            cell = new TableCell();
            cell.CssClass = "head";
            cell.Width = 10;
            row.Cells.Add(cell);

            cell = new TableCell();
            cell.CssClass = "head";
            cell.Width = 10;
            row.Cells.Add(cell);

            cell = new TableCell();
            cell.CssClass = "head";
            cell.Width = 10;
            row.Cells.Add(cell);

            tbKlassen.Rows.Add(row);

            string[] klassen = DatenbankAbrufen.KlassenAbrufen(dropDownAbteilung.SelectedValue);
            Structuren.Klasseneigenschaften daten;
            dataButton button;
            foreach (string klasse in klassen)
            {
                row = new TableRow();

                daten = DatenbankAbrufen.KlasseneigenschaftenAbrufen(klasse);

                cell = new TableCell();
                cell.Text = daten.klasse;
                row.Cells.Add(cell);

                cell = new TableCell();
                cell.Text = daten.gebäude + " - " + daten.raum;
                row.Cells.Add(cell);

                cell = new TableCell();
                cell.Text = daten.klassensprecher;
                row.Cells.Add(cell);

                cell = new TableCell();
                button = new dataButton();
                button.Text = "Ändern";
                button.Click += btÄndernClick;
                button.Setting = daten; if (classes.Login.Rechte.Klassen[2].ToInt32() > 1)
                    button.Enabled = true;
                else if (classes.Login.Rechte.Klassen[1].ToInt32() > 1 && classes.Login.StammAbteilung == dropDownAbteilung.SelectedValue)
                    button.Enabled = true;
                else if (classes.Login.Rechte.Klassen[0].ToInt32() > 1 && classes.Login.StammKlasse == daten.klasse)
                    button.Enabled = true;
                else
                    button.Enabled = false;

                //DEBUG!!!
                button.Enabled = false;

                cell.Controls.Add(button);
                row.Cells.Add(cell);

                cell = new TableCell();
                button = new dataButton();
                button.Text = "Stundenplan leeren";
                button.Click += btStundenplanLeerenClick;
                button.Setting = daten;
                if (classes.Login.Rechte.Stundenplan[2].ToInt32() > 2)
                    button.Enabled = true;
                else if (classes.Login.Rechte.Stundenplan[1].ToInt32() > 2 && classes.Login.StammAbteilung == dropDownAbteilung.SelectedValue)
                    button.Enabled = true;
                else if (classes.Login.Rechte.Stundenplan[0].ToInt32() > 2 && classes.Login.StammKlasse == daten.klasse)
                    button.Enabled = true;
                else
                    button.Enabled = false;
                cell.Controls.Add(button);
                row.Cells.Add(cell);

                cell = new TableCell();
                button = new dataButton();
                button.Text = "Löschen";
                button.Click += btLöschenClick;
                button.Setting = daten;
                if (classes.Login.Rechte.Klassen[2].ToInt32() > 3)
                    button.Enabled = true;
                else if (classes.Login.Rechte.Klassen[1].ToInt32() > 3 && classes.Login.StammAbteilung == dropDownAbteilung.SelectedValue)
                    button.Enabled = true;
                else if (classes.Login.Rechte.Klassen[0].ToInt32() > 3 && classes.Login.StammKlasse == daten.klasse)
                    button.Enabled = true;
                else
                    button.Enabled = false;
                cell.Controls.Add(button);
                row.Cells.Add(cell);

                tbKlassen.Rows.Add(row);
            }
        }

        protected void btNeueKlasse_Click(object sender, EventArgs e)
        {
            Session["22_Abteilung"] = dropDownAbteilung.SelectedValue;
            Session["22_Klasse_Art"] = 0;
            Response.Redirect("./add.aspx");
        }

        protected void btAlleKlassenLöschen_Click(object sender, EventArgs e)
        {
            if (!btAlleKlassenLöschen.Enabled) Response.Redirect("./");

            if (btAlleKlassenLöschen.BorderColor != Color.Red)
            {
                btAlleKlassenLöschen.BorderColor = Color.Red;
                HinweisZeigen();
            }
            else
            {
                DatenbankAbrufen.KlassenAbrufen(dropDownAbteilung.SelectedValue).ToList().ForEach(DatenbankSchreiben.KlasseLöschen);
                Response.Redirect("./");
            }
        }

        private void btÄndernClick(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void btStundenplanLeerenClick(object sender, EventArgs e)
        {
            dataButton bt = (dataButton)sender;

            if (!bt.Enabled) Response.Redirect("./");

            Structuren.Klasseneigenschaften daten = ((Structuren.Klasseneigenschaften)bt.Setting);
            if (bt.BorderColor != Color.Red)
            {
                bt.BorderColor = Color.Red;
                HinweisZeigen();
            }
            else
            {
                DatenbankSchreiben.StundenplanLeeren(daten.klasse);
                Response.Redirect("./");
            }
        }

        private void btLöschenClick(object sender, EventArgs e)
        {
            dataButton bt = (dataButton)sender;

            if (!bt.Enabled) Response.Redirect("./");

            Structuren.Klasseneigenschaften daten = ((Structuren.Klasseneigenschaften)bt.Setting);
            if (bt.BorderColor != Color.Red)
            {
                bt.BorderColor = Color.Red;
                HinweisZeigen();
            }
            else
            {
                DatenbankSchreiben.KlasseLöschen(daten.klasse);
                Response.Redirect("./");
            }
        }
    }
}