using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Infoscreen_Verwaltung.classes;
using System.Web.Services;
using System.Web.Script.Services;

namespace Infoscreen_Verwaltung.conf.supplierungen
{
    public partial class _default : System.Web.UI.Page
    {
        static bool vonfehler = false;
        static bool auffehler = false;

        static List<CheckBox> Vonstunden = new List<CheckBox>();
        static List<CheckBox> Aufstunden = new List<CheckBox>();

        string ebene = "../../";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!classes.Login.Angemeldet) Response.Redirect("../../login.aspx");
            if (!classes.Login.Rechte.Supplierung.OneBiggerThan(1)) Response.Redirect("../../");

            TopMenu.Text = classes.Anzeige.TopBar(ebene);
            Menu.Text = classes.Anzeige.Menue(ebene, "conf");

            if (!Page.IsPostBack)
            {
                string[] abteilungen;
                if (classes.Login.Rechte.Supplierung[2].ToInt32() > 1) // Superdamin
                    abteilungen = classes.DatenbankAbrufen.AbteilungenAbrufen();
                else if (classes.Login.Rechte.Supplierung[1].ToInt32() > 1) // Schulwart, AV
                    abteilungen = classes.Login.Abteilungen;
                else abteilungen = new string[] { "Elektronik" };

                foreach (string abteilung in abteilungen)
                    DropDownListAbteilung.Items.Add(abteilung);

                try
                {
                    DropDownListAbteilung.SelectedValue = classes.Login.StammAbteilung;
                    if (Session["13_Abteilung"] != null)
                        DropDownListAbteilung.SelectedValue = Session["13_Abteilung"].ToString();
                }
                catch { DropDownListAbteilung.SelectedIndex = -1; }

                string[] klassen;
                klassen = classes.DatenbankAbrufen.KlassenAbrufen(DropDownListAbteilung.SelectedValue);

                foreach (string klasse in klassen)
                    DropDownListKlasse.Items.Add(klasse);

                Session["13_Abteilung"] = null;
                Session["13_Klasse"] = null;
                Session["13_VonDatum"] = null;
                Session["13_AufDatum"] = null;
            }
            if (Session["13_VonDatum"] != null)
                VonDatumPruefen();

            if (Session["13_AufDatum"] != null)
                AufDatumPruefen();

            SupplierungKlasse.Text = "Supplierung der Klasse " + DropDownListKlasse.SelectedValue.ToString() + " eintragen";
        }
        
        protected void DropDownListAbteilung_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["13_Abteilung"] = DropDownListAbteilung.SelectedValue;
            Session["13_Klasse"] = DropDownListKlasse.SelectedValue;
            Session["13_VonDatum"] = Calendar1.SelectedDate;
            Session["13_AufDatum"] = Calendar2.SelectedDate;

            SupplierungKlasse.Text = "Supplierung der Klasse " + Session["13_Klasse"].ToString() + " eintragen";

            VonDatumTabelleZeichnen();
            AufDatumTabelleZeichnen();
        }
        protected void DropDownListKlasse_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["13_Abteilung"] = DropDownListAbteilung.SelectedValue;
            Session["13_Klasse"] = DropDownListKlasse.SelectedValue;
            Session["13_VonDatum"] = Calendar1.SelectedDate;
            Session["13_AufDatum"] = Calendar2.SelectedDate;

            SupplierungKlasse.Text = "Supplierung der Klasse " + Session["13_Klasse"].ToString() + " eintragen";

            VonDatumPruefen();
            AufDatumPruefen();
        }

        private void VonDatumTabelleZeichnen()
        {
            tbStundenplanVon.Rows.Clear();

            if (vonfehler == true) return;

            lbHinweis.Visible = false;

            Vonstunden = new List<CheckBox>();

            TableRow tr;
            TableCell tc;
            CheckBox cb;
            Label lb;

            classes.Structuren.StundenplanTagLehrer[] stundenplan = classes.DatenbankAbrufen.GetStundenplanfürDatum(Session["13_Klasse"].ToString(), Session["13_VonDatum"].ToDateTime());

            if (stundenplan.Length == 0)
            {
                tr = new TableRow();
                tc = new TableCell();
                lb = new Label();
                lb.Text = "<h2>Keine Stunden an diesem Tag</h2>";
                tc.CssClass = "head";
                tc.Controls.Add(lb);
                tr.Cells.Add(tc);
                tbStundenplanVon.Rows.Add(tr);
                return;
            }

            int i1;
            for (i1 = 0; i1 <= 3; i1++)
            {
                tr = new TableRow();

                int i2;
                for (i2 = 0; i2 < stundenplan.Length; i2++)
                {
                    if (i1 == 0)
                    {
                        if (i2 == 0)
                        {
                            tc = new TableCell();
                            tc.CssClass = "head";
                            tc.Text = "Stunde";
                            tr.Cells.Add(tc);
                        }
                        tc = new TableCell(); //Stunde
                        tc.CssClass = "head";
                        tc.Text = stundenplan[i2].Stunde.ToString();
                        tr.Cells.Add(tc);
                    }
                    if (i1 == 1)
                    {
                        if (i2 == 0)
                        {
                            tc = new TableCell();
                            tc.Text = "Lehrer";
                            tr.Cells.Add(tc);
                        }
                        tc = new TableCell(); //klasse
                        tc.Text = stundenplan[i2].Lehrer;
                        tr.Cells.Add(tc);
                    }
                    if (i1 == 2)
                    {
                        if (i2 == 0)
                        {
                            tc = new TableCell();
                            tc.Text = "Fach";
                            tr.Cells.Add(tc);
                        }
                        tc = new TableCell(); //fach
                        tc.Text = stundenplan[i2].Fach;
                        tr.Cells.Add(tc);
                    }
                    if (i1 == 3)
                    {
                        if (i2 == 0)
                        {
                            tc = new TableCell();
                            tc.Text = "Verschiebt";
                            tr.Cells.Add(tc);
                        }
                        tc = new TableCell(); //Entfall
                        cb = new CheckBox();
                        cb.Checked = false;
                        cb.AutoPostBack = true;
                        //cb.CheckedChanged += new EventHandler(von_cb_click);
                        cb.Text = stundenplan[i2].Stunde.ToString();
                        cb.ForeColor = System.Drawing.Color.White;
                        Vonstunden.Add(cb);
                        tc.Controls.Add(cb);
                        tr.Cells.Add(tc);
                    }
                }
                tbStundenplanVon.Rows.Add(tr);
            }
        }
        private void AufDatumTabelleZeichnen()
        {
            tbStundenplanAuf.Rows.Clear();

            if (auffehler == true) return;

            lbHinweis.Visible = false;

            Aufstunden = new List<CheckBox>();

            TableRow tr;
            TableCell tc;
            CheckBox cb;
            Label lb;

            classes.Structuren.StundenplanTagLehrer[] stundenplan = classes.DatenbankAbrufen.GetStundenplanfürDatum(Session["13_Klasse"].ToString(), Session["13_AufDatum"].ToDateTime(), true);

            if (stundenplan.Length == 0)
            {
                tr = new TableRow();
                tc = new TableCell();
                lb = new Label();
                lb.Text = "<h2>Keine Stunden an diesem Tag</h2>";
                tc.CssClass = "head";
                tc.Controls.Add(lb);
                tr.Cells.Add(tc);
                tbStundenplanAuf.Rows.Add(tr);
                return;
            }

            int i1;
            for (i1 = 0; i1 <= 3; i1++)
            {
                tr = new TableRow();

                int i2;
                for (i2 = 0; i2 < stundenplan.Length; i2++)
                {
                    if (i1 == 0)
                    {
                        if (i2 == 0)
                        {
                            tc = new TableCell();
                            tc.CssClass = "head";
                            tc.Text = "Stunde";
                            tr.Cells.Add(tc);
                        }
                        tc = new TableCell(); //Stunde
                        tc.CssClass = "head";
                        tc.Text = i2.ToString();                
                        tr.Cells.Add(tc);
                    }
                    if (i1 == 1)
                    {
                        if (i2 == 0)
                        {
                            tc = new TableCell();
                            tc.Text = "Lehrer";
                            tr.Cells.Add(tc);
                        }
                        tc = new TableCell(); //klasse
                        tc.Text = stundenplan[i2].Lehrer;
                        tr.Cells.Add(tc);
                    }
                    if (i1 == 2)
                    {
                        if (i2 == 0)
                        {
                            tc = new TableCell();
                            tc.Text = "Fach";
                            tr.Cells.Add(tc);
                        }
                        tc = new TableCell(); //fach
                        tc.Text = stundenplan[i2].Fach;
                        tr.Cells.Add(tc);
                    }
                    if (i1 == 3)
                    {
                        if (i2 == 0)
                        {
                            tc = new TableCell();
                            tc.Text = "Verschiebt";
                            tr.Cells.Add(tc);
                        }
                        tc = new TableCell(); //Entfall
                        cb = new CheckBox();
                        cb.Checked = false;
                        cb.AutoPostBack = true;
                        //cb.CheckedChanged += new EventHandler(auf_cb_click);
                        Aufstunden.Add(cb);
                        tc.Controls.Add(cb);
                        tr.Cells.Add(tc);
                    }
                }
                tbStundenplanAuf.Rows.Add(tr);
            }
        }

        protected void Bestaetigen_Click(object sender, EventArgs e)
        {
            int abtID = DatenbankAbrufen.GetAbteilungsIdVonAbteilungsname(DropDownListAbteilung.SelectedValue);
            string stattLehrer;
            lbHinweis.Visible = true;

            classes.Structuren.StundenplanTag[] stundenplan = classes.DatenbankAbrufen.StundenplanAbrufen(Session["13_Klasse"].ToString(), false);
            classes.Structuren.Supplierungen eintrag = new Structuren.Supplierungen();

            int zaehler1 = 0;
            for (int x = 0; x < Vonstunden.Count; x++)
            {
                if (Vonstunden[x].Checked == true)
                {
                    zaehler1++;
                    eintrag.Stunde = Vonstunden[x].Text.ToInt32();
                }
            }
            int zaehler2 = 0;
            for (int x = 0; x < Aufstunden.Count; x++)
            {
                if (Aufstunden[x].Checked == true)
                {
                    zaehler2++;
                    eintrag.ZiehtVor = x;
                }
            }
            if (zaehler1 != zaehler2)
            {
                lbHinweis.Text = "Es müssen gleich viele Stunden ausgewählt werden!";
                lbHinweis.ForeColor = System.Drawing.Color.Red;
                return;
            }

            int vontag = 0;
            DateTime Datum1 = Calendar1.SelectedDate; //von Datum

            switch (Datum1.DayOfWeek.ToString())
            {
                case "Monday":
                    vontag = 0;
                    break;
                case "Tuesday":
                    vontag = 1;
                    break;
                case "Wednesday":
                    vontag = 2;
                    break;
                case "Thursday":
                    vontag = 3;
                    break;
                case "Friday":
                    vontag = 4;
                    break;
                case "Saturday":
                    vontag = 5;
                    break;
            }

            eintrag.Datum = Session["13_VonDatum"].ToDateTime();
            eintrag.Klasse = Session["13_Klasse"].ToString();
            eintrag.Entfällt = false;
            eintrag.ZiehtVorDatum = Session["13_AufDatum"].ToDateTime();

            for (int i2 = 0; i2 < zaehler1; i2++)
            {
                for (int i3 = 0; i3 < stundenplan[vontag].StundenDaten.Length; i3++) // Stunden für Tag
                {
                    if (Vonstunden[i3].Checked == true)
                    {
                        Vonstunden[i3].Checked = false;

                        eintrag.Stunde = stundenplan[vontag].StundenDaten[i3].Stunde;
                        eintrag.Ursprungslehrer = stundenplan[vontag].StundenDaten[i3].Lehrer;
                        eintrag.Ersatzfach = stundenplan[vontag].StundenDaten[i3].Fach;

                        stattLehrer = stundenplan[vontag].StundenDaten[i3].Lehrer; //Lehrer der in der normalen Stunde unterrichtet
                        if (stattLehrer.Contains("/")) // Wenn es 2 Lehrer sind
                        {
                            string[] beideStattLehrer = stattLehrer.Split('/');
                            if (beideStattLehrer[0].CompareTo(beideStattLehrer[1]) > 0) // Wenn die 2 Lehrer nicht alphabetisch geordnet sind
                            {
                                eintrag.Ursprungslehrer = beideStattLehrer[1] + "/" + beideStattLehrer[0]; //Tausche Lehrer
                            }
                        }
                        eintrag.Ersatzlehrer = eintrag.Ursprungslehrer;

                        for (int i4 = 0; i4 < 12; i4++)
                        {
                            if (Aufstunden[i4].Checked == true)
                            {
                                Aufstunden[i4].Checked = false;
                                eintrag.ZiehtVor = i4;
                                try
                                {
                                    DatenbankSchreiben.SupplierungenEintragen(abtID, eintrag);

                                    lbHinweis.Text = "Verschiebung erfolgreich eingetragen!";
                                    Response.Redirect("table.aspx");
                                    // ISv6: Redir to table
                                    lbHinweis.ForeColor = System.Drawing.Color.Green;
                                }
                                catch
                                {
                                    lbHinweis.Text = "Eintrag für diese Stunde bereits vorhanden!";
                                    lbHinweis.ForeColor = System.Drawing.Color.Red;
                                    return;
                                }
                                break;
                            }
                        }
                    }
                }            
            }           
        }
        
        private void VonDatumPruefen()
        {
            vonfehler = false;

            DateTime datum = Calendar1.SelectedDate;

            if ((datum.DayOfWeek.ToString() != "Saturday" && datum.DayOfWeek.ToString() != "Sunday") && (!(datum < DateTime.Today)))
            {
                lbHinweis.Text = "";
            }
            else
            {
                if (Calendar1.SelectedDate.Year.ToString() != "1")
                {
                    if (datum < DateTime.Today)
                    {
                        lbHinweis.Text = "Ungültiges Datum (Vergangenheit)";
                    }
                    else
                    {
                        lbHinweis.Text = "Ungültiges Datum (Wochenende)";
                    }
                    vonfehler = true;
                    lbHinweis.ForeColor = System.Drawing.Color.Red;
                    lbHinweis.Visible = true;
                }
                else
                {
                    vonfehler = true;
                    lbHinweis.Text = "";
                }
            }

            Session["13_Abteilung"] = DropDownListAbteilung.SelectedValue;
            Session["13_Klasse"] = DropDownListKlasse.SelectedValue;
            Session["13_VonDatum"] = Calendar1.SelectedDate;
            Session["13_AufDatum"] = Calendar2.SelectedDate;

            SupplierungKlasse.Text = "Supplierung der Klasse " + Session["13_Klasse"].ToString() + " eintragen";

            VonDatumTabelleZeichnen();
        }
        private void AufDatumPruefen()
        {
            auffehler = false;

            DateTime datum = Calendar2.SelectedDate;

            if ((datum.DayOfWeek.ToString() != "Saturday" && datum.DayOfWeek.ToString() != "Sunday") && (!(datum < DateTime.Today)))
            {
                lbHinweis.Text = "";
            }
            else
            {
                if (Calendar2.SelectedDate.Year.ToString() != "1")
                {
                    if (datum < DateTime.Today)
                    {
                        lbHinweis.Text = "Ungültiges Datum (Vergangenheit)";
                    }
                    else
                    {
                        lbHinweis.Text = "Ungültiges Datum (Wochenende)";
                    }
                    auffehler = true;
                    lbHinweis.ForeColor = System.Drawing.Color.Red;
                    lbHinweis.Visible = true;
                }
                else
                {
                    auffehler = true;
                    lbHinweis.Text = "";
                }
            }

            Session["13_Abteilung"] = DropDownListAbteilung.SelectedValue;
            Session["13_Klasse"] = DropDownListKlasse.SelectedValue;
            Session["13_VonDatum"] = Calendar1.SelectedDate;
            Session["13_AufDatum"] = Calendar2.SelectedDate;

            SupplierungKlasse.Text = "Supplierung der Klasse " + Session["13_Klasse"].ToString() + " eintragen";

            AufDatumTabelleZeichnen();
        }
        protected void Calendar1_SelectionChanged(object sender, EventArgs e)
        {
            VonDatumPruefen();
        }
        protected void Calendar2_SelectionChanged(object sender, EventArgs e)
        {
            AufDatumPruefen();
        }

        protected void Zurueck_Click(object sender, EventArgs e)
        {
            Session["25_RadioButton"] = null;
            Response.Redirect("./table.aspx");
        }
    }
}