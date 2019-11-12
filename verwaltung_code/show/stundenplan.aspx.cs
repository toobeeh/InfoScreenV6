using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Infoscreen_Verwaltung.classes;

namespace Infoscreen_Verwaltung.show
{
    public partial class stundenplan : System.Web.UI.Page
    {
        string ebene = "../";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!classes.Login.Angemeldet) Response.Redirect("../login.aspx");
            if (classes.Login.Rechte.Stundenplan.ToInt32() == 0) Response.Redirect("../");

            TopMenu.Text = classes.Anzeige.TopBar(ebene);
            Menu.Text = classes.Anzeige.Menue(ebene, "show");

            if (!Page.IsPostBack)
            {
                string[] klassen;
                if (classes.Login.Rechte.Stundenplan[2] != '0')
                    klassen = classes.DatenbankAbrufen.KlassenAbrufen("");
                else if (classes.Login.Rechte.Stundenplan[1] != '0' || classes.Login.Rechte.Lehrer)
                    klassen = classes.DatenbankAbrufen.KlassenAbrufen("Elektronik"); // System wird nur von Elektronik verwendet
                else
                    klassen = new string[] { classes.Login.StammKlasse };

                foreach (string klasse in klassen)
                    DropDownListKlasse.Items.Add(klasse);

                try { DropDownListKlasse.SelectedValue = classes.Login.StammKlasse; }
                catch { DropDownListKlasse.SelectedIndex = -1; }
                TabelleZeichnen();
            }
        }

        protected void DropDownListKlasse_SelectedIndexChanged(object sender, EventArgs e)
        {
            Stundenplan.Rows.Clear();
            TabelleZeichnen();
        }

        protected void TabelleZeichnen()
        {
            bool normaleStunde = false;
            Structuren.StundenplanEntry dummy1;
            List<Structuren.StundenplanEntry> dummy0;
            int y2 = 0;
            int y1;

            string klasse = DropDownListKlasse.SelectedValue;
            classes.Structuren.StundenplanTag[] stundenplan = classes.DatenbankAbrufen.StundenplanAbrufen(klasse, true, true);

            TableRow row_test;
            TableCell cell_test;

            row_test = new TableRow();
            cell_test = new TableCell();

            for (int i2 = 0; i2 < stundenplan.Length; i2++)  // Anzahl der Wochentage
            {
                int[] vorgezogeneStunden = DatenbankAbrufen.ZiehtVorStunde(stundenplan[i2].Datum, klasse); //holt alle Stunden dieses Tages auf die verschoben wird

                for (y1 = 0; y1 < vorgezogeneStunden.Length; y1++) //Durchlauf aller stunden des Tages auf die verschoben wurde
                {
                    for (y2 = 0; y2 < stundenplan.Length; y2++) //Durchlauf aller Stunden des Tages
                    {
                        if (stundenplan[i2].StundenDaten[y2].Stunde == vorgezogeneStunden[y1])
                        {
                            normaleStunde = true;
                            break;
                        }
                    }
                    if (!normaleStunde)
                    {
                        dummy1 = new Structuren.StundenplanEntry();
                        dummy1.Stunde = vorgezogeneStunden[y1];
                        dummy1.Lehrer = "";
                        dummy1.Fach = "";
                        dummy1.Gebäude = "";
                        dummy1.Raum = 307;
                        dummy1.Supplierung = false;
                        dummy1.ZiehtVorDatum = "".ToDateTime();
                        dummy1.ZiehtVor = 0;
                        dummy1.Entfällt = false;

                        dummy0 = new List<Structuren.StundenplanEntry>();
                        dummy0.AddRange(stundenplan[i2].StundenDaten);
                        dummy0.Add(dummy1);
                        stundenplan[i2].StundenDaten = dummy0.ToArray();
                    }
                    else
                    {
                        normaleStunde = false;
                    }
                }
            }
            int min, max;
            min = classes.Helper.KleinsteStunde(stundenplan);
            max = classes.Helper.GrößteStunde(stundenplan);
            TableRow row;
            TableCell cell;

            row = new TableRow();
            cell = new TableCell();
            if (stundenplan.Length < 1)
            {
                cell.Text = "Zu dieser Klasse liegen akzuell keine Daten vor.";
                row.Cells.Add(cell);
                Stundenplan.Rows.Add(row);
            }
            else
            {
                cell.CssClass = "head";
                row.Cells.Add(cell);

                cell = new TableCell();
                cell.CssClass = "head";
                cell.Text = "Montag" + "<br />" + stundenplan[0].Datum.ToString("dd.MM.yyyy");
                row.Cells.Add(cell);

                cell = new TableCell();
                cell.CssClass = "head";
                cell.Text = "Dienstag" + "<br />" + stundenplan[1].Datum.ToString("dd.MM.yyyy");
                row.Cells.Add(cell);

                cell = new TableCell();
                cell.CssClass = "head";
                cell.Text = "Mittwoch" + "<br />" + stundenplan[2].Datum.ToString("dd.MM.yyyy");
                row.Cells.Add(cell);

                cell = new TableCell();
                cell.CssClass = "head";
                cell.Text = "Donnerstag" + "<br />" + stundenplan[3].Datum.ToString("dd.MM.yyyy");
                row.Cells.Add(cell);

                cell = new TableCell();
                cell.CssClass = "head";
                cell.Text = "Freitag" + "<br />" + stundenplan[4].Datum.ToString("dd.MM.yyyy");
                row.Cells.Add(cell);

                if (stundenplan.Length > 5)
                {
                    cell = new TableCell();
                    cell.CssClass = "head";
                    cell.Text = "Samstag" + "<br />" + stundenplan[5].Datum.ToString("dd.MM.yyyy");
                    row.Cells.Add(cell);
                }
                Stundenplan.Rows.Add(row);
                int i2;
                int i3;
                int i4;
                for (int i1 = min; i1 <= max; i1++) // Anzahl Stunden
                {
                    row = new TableRow();
                    cell = new TableCell();
                    cell.Text = i1.ToString() + ".<br />" + classes.Helper.StundenZeiten(i1);
                    row.Cells.Add(cell);

                    for (i2 = 0; i2 < stundenplan.Length; i2++)  // Anzahl der Wochentage
                    {
                        int[] vorgezogeneStunden = DatenbankAbrufen.ZiehtVorStunde(stundenplan[i2].Datum, klasse); //holt alle Stunden dieses Tages auf die verschoben wird

                        cell = new TableCell();
                        for (i3 = 0; i3 < stundenplan[i2].StundenDaten.Length; i3++) // Durchlauf aller Stunden eines Tages
                        {
                            if (stundenplan[i2].StundenDaten[i3].Stunde == i1)
                            {
                                cell.Text = stundenplan[i2].StundenDaten[i3].Lehrer + "<br />" + stundenplan[i2].StundenDaten[i3].Fach;

                                if (stundenplan[i2].StundenDaten[i3].Supplierung || stundenplan[i2].StundenDaten[i3].Entfällt)
                                {
                                    cell.CssClass = "änderung";
                                }
                                if (stundenplan[i2].StundenDaten[i3].Supplierung)
                                {
                                    cell.Text = stundenplan[i2].StundenDaten[i3].Ersatzlehrer + "<br />" + stundenplan[i2].StundenDaten[i3].Ersatzfach;
                                }
                                if (stundenplan[i2].StundenDaten[i3].ZiehtVor > -1)
                                {
                                    cell.CssClass = "vorgezogen";
                                    cell.Text = "Verschoben auf " + stundenplan[i2].StundenDaten[i3].ZiehtVorDatum.ToString("dd.MM.yyyy");
                                }
                                if (stundenplan[i2].StundenDaten[i3].Entfällt)
                                {
                                    cell.Text = "Entfällt";
                                }
                                for (i4 = 0; i4 < vorgezogeneStunden.Length; i4++) //Durchlauf aller Stunden dieses Tages auf die verschoben wurde
                                {
                                    if (stundenplan[i2].StundenDaten[i3].Stunde == vorgezogeneStunden[i4]) //Wenn die jetzige Stunde eine vorgezogene ist
                                    {
                                        string ersatzlehrer, ersatzfach;
                                        DateTime verschiebtVon = DatenbankAbrufen.GetVerschiebtVonDatum(stundenplan[i2].Datum, klasse, vorgezogeneStunden[i4], out ersatzlehrer, out ersatzfach);

                                        cell.Text = ersatzlehrer +
                                        "<br />verschiebt von " +
                                        verschiebtVon.ToString("dd.MM.yyyy") + " " +
                                        ersatzfach;
                                        cell.CssClass = "vorgezogen";
                                    }
                                }

                                break;
                            }
                        }
                        row.Cells.Add(cell);
                    }

                    Stundenplan.Rows.Add(row);
                }
            }
        }
    }
}