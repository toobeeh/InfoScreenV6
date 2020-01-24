using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Infoscreen_Verwaltung.classes;

namespace Infoscreen_Verwaltung.conf.tests
{
    public partial class _default : System.Web.UI.Page
    {
        string ebene = "../../";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!classes.Login.Angemeldet) Response.Redirect("../../login.aspx");
            if (!classes.Login.Rechte.Tests.OneBiggerThan(1)) Response.Redirect("../../");

            TopMenu.Text = classes.Anzeige.TopBar(ebene);
            Menu.Text = classes.Anzeige.Menue(ebene, "conf");

            if (!Page.IsPostBack)
            {
                string[] klassen;
                if (classes.Login.Rechte.Tests[2].ToInt32() > 1) //Superadmin
                {
                    klassen = classes.DatenbankAbrufen.KlassenAbrufen("");                   
                }
                else if (classes.Login.Rechte.Tests[1].ToInt32() > 1) //AV, Schulwart
                {
                    klassen = classes.DatenbankAbrufen.KlassenAbrufen(classes.Login.StammAbteilung);                   
                }
                else
                {                   
                    klassen = new string[] { classes.Login.StammKlasse };
                }

                foreach (string klasse in klassen)
                    DropDownListKlasse.Items.Add(klasse);

                try
                {
                    DropDownListKlasse.SelectedValue = classes.Login.StammKlasse;
                    if (Session["12_Klasse"] != null)
                        DropDownListKlasse.SelectedValue = Session["12_Klasse"].ToString();
                }
                catch { DropDownListKlasse.SelectedIndex = -1; }
            }
            TestsAbrufen();
            Session["12_Klasse"] = null;
        }

        protected void DropDownListKlasse_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["12_Klasse"] = DropDownListKlasse.SelectedValue;
            Response.Redirect("./");
        }

        protected void TestsAbrufen()
        {
            if (classes.Login.Rechte.Tests[2].ToInt32() > 2)
                ButtonAdd.Visible = true;
            else if (classes.Login.Rechte.Tests[1].ToInt32() > 2 && classes.Login.Abteilungen.Contains<string>(DatenbankAbrufen.GetAbteilungVonKlasse(DropDownListKlasse.SelectedValue)))
                ButtonAdd.Visible = true;
            else if (classes.Login.Rechte.Tests[0].ToInt32() > 2 && classes.Login.Klassen.Contains<string>(DropDownListKlasse.SelectedValue))
                ButtonAdd.Visible = true;

            Structuren.Tests[] daten = DatenbankAbrufen.TestsAbrufen(DropDownListKlasse.SelectedValue, false);
            TableRow row;
            TableCell cell;
            row = new TableRow();
            cell = new TableCell();

            if (daten.Length == 0)
            {
                cell.Text = "Derzeit keine Einträge vorhanden.";
                row.Cells.Add(cell);
                tbTests.Rows.Add(row);
                return;
            }

            cell.Text = "Datum";
            cell.CssClass = "head";
            row.Cells.Add(cell);

            cell = new TableCell();
            cell.Text = "Stunde";
            cell.CssClass = "head";
            row.Cells.Add(cell);

            cell = new TableCell();
            cell.Text = "Fach";
            cell.CssClass = "head";
            row.Cells.Add(cell);

            cell = new TableCell();
            cell.Text = "Dauer (h)";
            cell.CssClass = "head";
            row.Cells.Add(cell);

            cell = new TableCell();
            cell.Text = "Lehrer";
            cell.CssClass = "head";
            row.Cells.Add(cell);

            cell = new TableCell();
            cell.Text = "Art";
            cell.CssClass = "head";
            row.Cells.Add(cell);

            cell = new TableCell();
            cell.Text = "Raum";
            cell.CssClass = "head";
            row.Cells.Add(cell);

            cell = new TableCell();
            cell.CssClass = "head";
            row.Cells.Add(cell);

            tbTests.Rows.Add(row);

            foreach (classes.Structuren.Tests zeile in daten)
            {
                row = new TableRow();

                cell = new TableCell();
                cell.Text = zeile.Datum.ToString("dddd, dd.MM.yyyy");
                row.Cells.Add(cell); ;

                cell = new TableCell();
                cell.Text = zeile.Stunde.ToString();
                row.Cells.Add(cell);

                cell = new TableCell();
                cell.Text = zeile.Fach;
                row.Cells.Add(cell);

                cell = new TableCell();
                cell.Text = zeile.Dauer.ToInt32().ToString();
                row.Cells.Add(cell);

                cell = new TableCell();
                cell.Text = zeile.Lehrer;
                row.Cells.Add(cell);

                cell = new TableCell();
                cell.Text = zeile.Testart;
                row.Cells.Add(cell);

                cell = new TableCell();
                cell.Text = zeile.Raum;
                row.Cells.Add(cell);

                cell = new TableCell();
                dataButton bt = new dataButton();
                bt.Setting = zeile;
                bt.CssClass = "ActionButton";
                bt.Text = "Ändern";
                bt.Click += btÄndern_Click;
                bt.OnClientClick = "btÄndern_Click";
                cell.Controls.Add(bt);
                bt = new dataButton();
                bt.Setting = zeile;
                bt.Text = "Löschen";
                bt.CssClass = "DeleteButton";
                bt.Click += btLöschen_Click;
                bt.OnClientClick = "btLöschen_Click";
                cell.Controls.Add(bt);
                row.Cells.Add(cell);

                tbTests.Rows.Add(row);
            }
        }

        protected void ButtonAdd_Click(object sender, EventArgs e)
        {
            if (ButtonAdd.Visible)
                Session["12_Neu"] = true;

            Session["12_Klasse"] = DropDownListKlasse.SelectedValue;

            Response.Redirect("./modify.aspx");
        }

        private void btÄndern_Click(object sender, EventArgs e)
        {
            Session["12_Neu"] = null;

            Session["12_Daten"] = ((dataButton)sender).Setting;

            Session["12_Klasse"] = DropDownListKlasse.SelectedValue;

            Response.Redirect("./modify.aspx");
        }

        private void btLöschen_Click(object sender, EventArgs e)
        {
            Structuren.Tests temp = (Structuren.Tests)((dataButton)sender).Setting;
            DatenbankSchreiben.TestLöschen(DropDownListKlasse.SelectedValue, temp.Datum, temp.Stunde, temp.Raum);
            tbTests.Rows.Clear();
            TestsAbrufen();
        }
    }
}