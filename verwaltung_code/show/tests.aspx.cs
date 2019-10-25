using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Infoscreen_Verwaltung.classes;

namespace Infoscreen_Verwaltung.show
{
    public partial class tests : System.Web.UI.Page
    {
        string ebene = "../";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!classes.Login.Angemeldet) Response.Redirect("../login.aspx");
            if (classes.Login.Rechte.Tests.ToInt32() == 0) Response.Redirect("../");

            TopMenu.Text = classes.Anzeige.TopBar(ebene);
            Menu.Text = classes.Anzeige.Menue(ebene, "show");

            if (!Page.IsPostBack)
            {
                string[] klassen;
                if (classes.Login.Rechte.Tests[2] != '0') //Superadmin
                {
                    klassen = classes.DatenbankAbrufen.KlassenAbrufen("");
                    lb_AnzeigeSA.Text = "<h1>Schularbeiten und Tests</h1>";
                }
                else if (classes.Login.Rechte.Tests[1] != '0') // Lehrer, AV, Schulwart
                {
                    klassen = classes.DatenbankAbrufen.KlassenAbrufen("Elektronik");
                    lb_AnzeigeSA.Text = "Willkommen " + classes.Login.Name + "<br>Schularbeit / Test anzeige</br>";
                }
                else
                {
                    klassen = new string[] { classes.Login.StammKlasse };
                    lb_AnzeigeSA.Text = "Willkommen " + classes.Login.Name + "<br>Schularbeit/Test anzeige " + classes.Login.StammKlasse + "</br>";
                }
                foreach (string klasse in klassen)
                    DropDownListKlasse.Items.Add(klasse);

                try { DropDownListKlasse.SelectedValue = classes.Login.StammKlasse; }
                catch { DropDownListKlasse.SelectedIndex = -1; }

                TabelleZeichnen();
            }
        }

        protected void DropDownListKlasse_SelectedIndexChanged(object sender, EventArgs e)
        {
            TestListe.Rows.Clear();
            TabelleZeichnen();
        }

        protected void TabelleZeichnen()
        {
            classes.Structuren.Tests[] temp = classes.DatenbankAbrufen.TestsAbrufen(DropDownListKlasse.SelectedValue);
            TableRow row;
            TableCell cell;
            if (temp.Length == 0)
            {
                row = new TableRow();
                cell = new TableCell();
                cell.Text = "Derzeit keine künftigen Tests eingetragen";
                row.Cells.Add(cell);
                TestListe.Rows.Add(row);
            }
            else
            {
                row = new TableRow();

                cell = new TableCell();
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
                TestListe.Rows.Add(row);

                foreach (classes.Structuren.Tests zeile in temp)
                {
                    row = new TableRow();

                    cell = new TableCell();
                    cell.Text = zeile.Datum.ToString("dddd, dd.MM.yyyy");
                    row.Cells.Add(cell); 

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

                    TestListe.Rows.Add(row);
                }
            }
        }
    }
}