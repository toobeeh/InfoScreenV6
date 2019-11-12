using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Infoscreen_Verwaltung.classes;

namespace Infoscreen_Verwaltung.show
{
    public partial class supplierungen : System.Web.UI.Page
    {
        string ebene = "../";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!classes.Login.Angemeldet) Response.Redirect("../login.aspx");
            if (classes.Login.Rechte.Supplierung.ToInt32() == 0) Response.Redirect("../");

            TopMenu.Text = classes.Anzeige.TopBar(ebene);
            Menu.Text = classes.Anzeige.Menue(ebene, "show");

            if (!Page.IsPostBack)
            {
                string[] abteilungen;
                if (classes.Login.Rechte.Supplierung[2] != '0') //Superadmin
                    abteilungen = classes.DatenbankAbrufen.AbteilungenAbrufen();
                else if (classes.Login.Rechte.Supplierung[1] != '0') //Lehrer, Schulwart, AV
                    abteilungen = new string[] { "Elektronik" };  // wenn System von anderen Abteilungen verwendet wird: abteilungen = classes.Login.Abteilungen;
                else abteilungen = new string[] { classes.Login.StammAbteilung };

                foreach (string abteilung in abteilungen)
                    dropDownAbteilung.Items.Add(abteilung);

                try { dropDownAbteilung.SelectedValue = classes.Login.StammAbteilung; }
                catch { dropDownAbteilung.SelectedIndex = -1; }

                SupplierungenZeichnen();
            }
        }

        protected void DropDownAbteilung_SelectedIndexChanged(object sender, EventArgs e)
        {
            cont.Controls.Clear();
            SupplierungenZeichnen();
        }

        protected void SupplierungenZeichnen() 
        {
            classes.Structuren.LehrerSupplierungen[] daten; //Alle Supplierungen an der 0.Stelle von daten

            if (classes.Login.Rechte.Supplierung[0] == '1') //wenn Schüler, Klassen- oder Abteilungssprecher angemeldet ist
            {
                string lehrer = "";
                string klasse = classes.Login.StammKlasse.ToString();

                //alle Supplierungen die den angemeldeten Schüler betreffen abrufen
                daten = classes.DatenbankAbrufen.SupplierplanAbrufen(dropDownAbteilung.SelectedValue, false, lehrer, klasse, false,"");
            }
            else
            {
                if (classes.Login.Rechte.Supplierung[1] == '1') //wenn Lehrer angemeldet ist
                {
                    string angemeldeterLehrer = classes.Login.User;
                    string klasse = "";
                    //alle Supplierungen die den angemeldeten Lehrer betreffen abrufen
                    daten = classes.DatenbankAbrufen.SupplierplanAbrufen(dropDownAbteilung.SelectedValue, false, angemeldeterLehrer, klasse, false,"");
                }
                else //wenn Superadmin, AV, Schulwart
                {
                    //alle Supplierungen abrufen            
                    daten = classes.DatenbankAbrufen.SupplierplanAbrufen(dropDownAbteilung.SelectedValue, false, "","",false, ""); 
                }
            }

            int abtID = DatenbankAbrufen.GetAbteilungsIdVonAbteilungsname(dropDownAbteilung.SelectedValue);

            classes.Structuren.GlobalEntfall[] globalEntfall = classes.DatenbankAbrufen.GetGlobaleEntfaelle(abtID);

            Label lb;
            Table tb;
            TableRow tr;
            TableCell tc;

            if (daten.Length == 0 && globalEntfall.Length == 0)
            {
                lb = new Label();
                lb.Text = "<h2>Derzeit keine Supplierungen</h2>";
                cont.Controls.Add(lb);
                return;
            }

            if (globalEntfall.Length != 0)
            {
                lb = new Label();
                lb.Text = "<h2>Lehrerkonferenzen</h2>";
                cont.Controls.Add(lb);

                tb = new Table();
                tb.CssClass = "tableSupplierung";
                tr = new TableRow();

                tc = new TableCell();
                tc.Text = "Datum";
                tc.CssClass = "head";
                tc.Width = 10;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = "Tag";
                tc.CssClass = "head";
                tc.Width = 6;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = "Von";
                tc.CssClass = "head";
                tc.Width = 63;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = "Bis";
                tc.CssClass = "head";
                tc.Width = 60;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = "Art der Supplierung";
                tc.CssClass = "head";
                //  tc.Width = 350;
                tr.Cells.Add(tc);

                tb.Rows.Add(tr);

                foreach (classes.Structuren.GlobalEntfall eintrag in globalEntfall)
                {
                    tr = new TableRow();

                    tc = new TableCell(); //datum
                    tc.Text = eintrag.Datum.ToString("dd.MM.");
                    tr.Cells.Add(tc);

                    tc = new TableCell(); //wochentag
                    tc.Text = eintrag.Datum.ToString("dddd").Substring(0, 2);
                    tr.Cells.Add(tc);

                    tc = new TableCell(); //VonStunde
                    tc.Text = eintrag.VonStunde.GetZeitenOfStunde().Remove(5);
                    tr.Cells.Add(tc);

                    tc = new TableCell(); //BisStunde
                    tc.Text = eintrag.BisStunde.GetZeitenOfStunde().Remove(0,8);
                    tr.Cells.Add(tc);

                    tc = new TableCell(); //Art der Supplierung
                    tc.Text = "Entfall für alle Klassen";
                    tc.CssClass = "entfällt";
                    tr.Cells.Add(tc);

                    tb.Rows.Add(tr);
                }
                cont.Controls.Add(tb);
                lb = new Label();
                lb.Text = "<hr />";
                cont.Controls.Add(lb);
            }

            if (daten.Length != 0)
            {
                lb = new Label();
                lb.Text = "<h2>Supplierungen</h2>";
                cont.Controls.Add(lb);

                tb = new Table();
                tb.CssClass = "tableSupplierung";
                tr = new TableRow();

                tc = new TableCell();
                tc.Text = "Datum";
                tc.CssClass = "head";
                tc.Width = 10;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = "Tag";
                tc.CssClass = "head";
                tc.Width = 6;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = "Stunde";
                tc.CssClass = "head";
                tc.Width = 11;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = "Klasse";
                tc.CssClass = "head";
                tc.Width = 10;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = "Original-Lehrer";
                tc.CssClass = "head";
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = "Ersatz-Lehrer";
                tc.CssClass = "head";
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = "Fach";
                tc.CssClass = "head";
                tc.Width = 80;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = "Art der Supplierung";
                tc.CssClass = "head";
                tc.Width = 350;
                tr.Cells.Add(tc);

                tb.Rows.Add(tr);

                foreach (classes.Structuren.LehrerSupplierungen temp in daten)
                {
                    foreach (classes.Structuren.Supplierungen zeile in temp.Supplierungen)
                    {
                        tr = new TableRow();

                        tc = new TableCell(); //datum
                        tc.Text = zeile.Datum.ToString("dd.MM.");
                        tr.Cells.Add(tc);

                        tc = new TableCell(); //wochentag
                        tc.Text = zeile.Datum.ToString("dddd").Substring(0, 2);
                        tr.Cells.Add(tc);

                        tc = new TableCell(); //stunde
                        tc.Text = zeile.Stunde.ToString();
                        tr.Cells.Add(tc);

                        tc = new TableCell(); //klasse
                        tc.Text = zeile.Klasse;
                        tr.Cells.Add(tc);

                        if (zeile.Entfällt) // Entfällt
                        {
                            tc = new TableCell();
                            tc.Text = zeile.Ursprungslehrer;
                            tr.Cells.Add(tc);

                            tc = new TableCell();
                            tc.Text = "-";
                            tr.Cells.Add(tc);

                            tc = new TableCell();
                            tc.Text = zeile.Ersatzfach;
                            tr.Cells.Add(tc);

                            tc = new TableCell();
                            tc.Text = "ENTFÄLLT";
                            tc.CssClass = "entfällt";
                            tr.Cells.Add(tc);
                        }
                        if (zeile.ZiehtVor >= 0) //Verschiebung
                        {
                            tc = new TableCell();
                            tc.Text = zeile.Ursprungslehrer;
                            tr.Cells.Add(tc);

                            tc = new TableCell();
                            tc.Text = "-";
                            tr.Cells.Add(tc);

                            tc = new TableCell();
                            tc.Text = zeile.Ersatzfach;
                            tr.Cells.Add(tc);

                            tc = new TableCell();
                            tc.Text = "Verschiebt auf: " + zeile.ZiehtVorDatum.ToString("dddd") + ", " + zeile.ZiehtVorDatum.ToString("dd.MM.") + ", " + zeile.ZiehtVor.ToString() + ". Stunde";
                            tc.CssClass = "vorgezogen";
                            tr.Cells.Add(tc);
                        }
                        if (!zeile.Entfällt && zeile.ZiehtVor < 0) //Supplierung
                        {
                            tc = new TableCell();
                            tc.Text = zeile.Ursprungslehrer;
                            tr.Cells.Add(tc);

                            tc = new TableCell();
                            tc.Text = zeile.Ersatzlehrer;
                            tr.Cells.Add(tc);

                            tc = new TableCell();
                            tc.Text = zeile.Ersatzfach;
                            tr.Cells.Add(tc);

                            tc = new TableCell();
                            tc.Text = "Wird suppliert";
                            tc.CssClass = "suppliert";
                            tr.Cells.Add(tc);
                        }
                        tb.Rows.Add(tr);
                    }
                    cont.Controls.Add(tb);
                }
            }
        }
    }
}