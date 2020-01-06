using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Infoscreen_Verwaltung.classes;

namespace Infoscreen_Verwaltung.show
{
    public partial class sprechstunden : System.Web.UI.Page
    {
        string ebene = "../";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!classes.Login.Angemeldet) Response.Redirect("../login.aspx");
            if (classes.Login.Rechte.Sprechstunden.ToInt32() == 0) Response.Redirect("../");

            TopMenu.Text = classes.Anzeige.TopBar(ebene);
            Menu.Text = classes.Anzeige.Menue(ebene, "show");

            if (!Page.IsPostBack)
            {
                string[] abteilungen;
                if (classes.Login.Rechte.Sprechstunden[2] != '0')
                    abteilungen = classes.DatenbankAbrufen.AbteilungenAbrufen();
                else if (classes.Login.Rechte.Sprechstunden[1] != '0')
                    abteilungen = classes.Login.Abteilungen;
                else abteilungen = new string[] { classes.Login.StammAbteilung };

                foreach (string abteilung in abteilungen)
                    dropDownAbteilung.Items.Add(abteilung);

                try { dropDownAbteilung.SelectedValue = classes.Login.StammAbteilung; }
                catch { dropDownAbteilung.SelectedIndex = -1; }

                Tabellezeichnen();
            }
        }

        protected void DropDownAbteilung_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbSprechstunden.Rows.Clear();
            Tabellezeichnen();
        }

        protected void Tabellezeichnen()
        {
            classes.Structuren.Sprechstunden[] temp = classes.DatenbankAbrufen.SprechstundenAbrufen(dropDownAbteilung.SelectedValue);

            TableRow row;
            TableCell cell;

            row = new TableRow();

            cell = new TableCell();
            cell.Text = "Lehrer";
            cell.CssClass = "head";
            row.Cells.Add(cell);

            cell = new TableCell();
            cell.Text = "Raum";
            cell.CssClass = "head";
            row.Cells.Add(cell);

            cell = new TableCell();
            cell.Text = "Tag";
            cell.CssClass = "head";
            row.Cells.Add(cell);

            cell = new TableCell();
            cell.Text = "Stunde";
            cell.CssClass = "head";
            row.Cells.Add(cell);

            cell = new TableCell();
            cell.Text = "Uhrzeit";
            cell.CssClass = "head";
            row.Cells.Add(cell);

            cell = new TableCell();
            cell.Text = "Durchwahl";
            cell.CssClass = "head";
            row.Cells.Add(cell);

            tbSprechstunden.Rows.Add(row);

            foreach (classes.Structuren.Sprechstunden zeile in temp)
            {
                row = new TableRow();

                cell = new TableCell();
                cell.Text = zeile.Lehrer;
                row.Cells.Add(cell);

                cell = new TableCell();
                cell.Text = StringHelper.ToValidRoomBuilding(zeile.Raum,4,2);
                row.Cells.Add(cell);

                cell = new TableCell();
                cell.Text = zeile.Tag.GetDayOfInt();
                row.Cells.Add(cell);

                cell = new TableCell();
                cell.Text = zeile.Stunde.ToString();
                row.Cells.Add(cell);

                cell = new TableCell();
                cell.Text = zeile.Stunde.GetZeitenOfStunde();
                row.Cells.Add(cell);

                cell = new TableCell();
                cell.Text = zeile.Durchwahl;
                row.Cells.Add(cell);

                tbSprechstunden.Rows.Add(row);
            }
        }
    }
}