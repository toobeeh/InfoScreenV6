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
    public partial class globalsupplierung : System.Web.UI.Page
    {

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
                else abteilungen = new string[] { "Elektronik" }; //Schüler, Lehrer

                foreach (string abteilung in abteilungen)
                    DropDownListAbteilung.Items.Add(abteilung);

                try
                {
                    DropDownListAbteilung.SelectedValue = classes.Login.StammAbteilung;
                    if (Session["13_Abteilung"] != null)
                        DropDownListAbteilung.SelectedValue = Session["13_Abteilung"].ToString();
                }
                catch { DropDownListAbteilung.SelectedIndex = -1; }
            }
            Session["13_Abteilung"] = null;
        }

        protected void DropDownListAbteilung_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["13_Abteilung"] = DropDownListAbteilung.SelectedValue;
            Response.Redirect("./default.aspx");
        }

        protected void Bestaetigen_Click(object sender, EventArgs e)
        {
            int i, i1, i3, j;

            int abtID = DatenbankAbrufen.GetAbteilungsIdVonAbteilungsname(DropDownListAbteilung.SelectedValue);
            int globalEntfallID = DatenbankAbrufen.GetLastGlobalEntfallID();

            int vonStunde, bisStunde;
            int tag = 0;
            string[] klassen;

            string stattlehrer;
            lbHinweis.Visible = true;
            Structuren.GlobalEntfall globalerEintrag;

            if (tb0.Text == "") //wenn Datum leer
            {
                lbHinweis.Text = "Textbox darf nicht leer sein!";
                lbHinweis.ForeColor = System.Drawing.Color.Red;                
                return;
            }

            if (tb1.Text == "" && tb2.Text == "") //wenn beide Stundeneingaben leer sind
            {
                vonStunde = 0;
                bisStunde = 12;
            }       
            else
            {
                if(tb1.Text == "") //wenn nur BIS-Stunde eingetragen wird
                {
                    if (tb2.Text.ToInt32() == -1) // prüft ob von-Stunden-Eingabe INTERGER ist 
                    {
                        lbHinweis.Text = "Keine gütige Stunde! (keine Buchstaben)";
                        lbHinweis.ForeColor = System.Drawing.Color.Red;
                        return;
                    }
                    else
                    {
                        if (tb2.Text.ToInt32() < 0 || tb2.Text.ToInt32() > 12)
                        {
                            lbHinweis.Text = "Keine gütige Stunde! (nur 0-12 erlaubt)";
                            lbHinweis.ForeColor = System.Drawing.Color.Red;
                            return;
                        }
                    }
                    vonStunde = 0;
                    bisStunde = tb2.Text.ToInt32();
                }
                else
                {
                    if(tb2.Text == "") //wenn nur VON-Stunde eingetragen wird
                    {
                        if (tb1.Text.ToInt32() == -1) // prüft ob auf-Stunden-Eingabe INTERGER ist 
                        {
                            lbHinweis.Text = "Keine gütige Stunde! (keine Buchstaben)";
                            lbHinweis.ForeColor = System.Drawing.Color.Red;
                            return;
                        }
                        else
                        {
                            if(tb1.Text.ToInt32() < 0 || tb1.Text.ToInt32() > 12)
                            {
                                lbHinweis.Text = "Keine gütige Stunde! (nur 0-12 erlaubt)";
                                lbHinweis.ForeColor = System.Drawing.Color.Red;
                                return;
                            }
                        }
                        vonStunde = tb1.Text.ToInt32();
                        bisStunde = 12;
                    }
                    else //wenn beides eingetragen wird
                    {
                        if (tb1.Text.ToInt32() == -1 || tb2.Text.ToInt32() == -1) // prüft ob Stunden-Eingaben INTERGER sind 
                        {
                            lbHinweis.Text = "Keine gütige Stunde! (keine Buchstaben)";
                            lbHinweis.ForeColor = System.Drawing.Color.Red;
                            return;
                        }
                        else
                        {
                            if (tb1.Text.ToInt32() < 0 || tb1.Text.ToInt32() > 12 || tb2.Text.ToInt32() < 0 || tb2.Text.ToInt32() > 12) // prüft ob Stunden-Eingaben in Bereich liegen 
                            {
                                lbHinweis.Text = "Keine gütige Stunde! (nur 0-12 erlaubt)";
                                lbHinweis.ForeColor = System.Drawing.Color.Red;
                                return;
                            }
                            else 
                            {
                                if(tb2.Text.ToInt32() < tb1.Text.ToInt32()) //prüft ob Von-Stunde kleiner ist als Bis-Stunde
                                {
                                    lbHinweis.Text = "Bis-Stunde muss größer als Von-Stunde sein!";
                                    lbHinweis.ForeColor = System.Drawing.Color.Red;
                                    return;
                                }
                            }
                        }
                        vonStunde = tb1.Text.ToInt32();
                        bisStunde = tb2.Text.ToInt32();
                    }
                }
            }

            Structuren.Supplierungen eintrag;
            eintrag.Datum = tb0.Text.ToDateTime();
            eintrag.Entfällt = true;
            eintrag.Ersatzfach = "";
            eintrag.Ersatzlehrer = "";
            eintrag.Ursprungslehrer = "";
            eintrag.ZiehtVor = -1;
            eintrag.ZiehtVorDatum = "".ToDateTime();
            eintrag.Raum = "";
            eintrag.Grund = "";

            switch (eintrag.Datum.DayOfWeek.ToString()) //Tag des Vorgezogenen Datums
            {
                case "Monday":
                    tag = 0;
                    break;
                case "Tuesday":
                    tag = 1;
                    break;
                case "Wednesday":
                    tag = 2;
                    break;
                case "Thursday":
                    tag = 3;
                    break;
                case "Friday":
                    tag = 4;
                    break;
                case "Saturday":
                    tag = 5;
                    break;
            }
      
            klassen = DatenbankAbrufen.KlassenAbrufen(DropDownListAbteilung.SelectedValue);
            for (i = 0; i < klassen.Length; i++)
            {
                eintrag.Klasse = klassen[i];
                classes.Structuren.StundenplanTag[] stundenplan = classes.DatenbankAbrufen.StundenplanAbrufen(eintrag.Klasse, false);

                int[] stunden = new int[bisStunde - vonStunde + 1];
                stunden = DatenbankAbrufen.GetStundenDieGlobalEntfallen(klassen[i], vonStunde, bisStunde, eintrag.Datum);

                for(j = 0; j < stunden.Length; j++)
                {
                    eintrag.Stunde = stunden[j];
                    for (i1 = stunden[0]; i1 <= stunden[stunden.Length -1]; i1++) // Anzahl Stunden
                    {
                        for (i3 = 0; i3 < stundenplan[tag].StundenDaten.Length; i3++) // Stunden für Tag
                        {
                            if (stundenplan[tag].StundenDaten[i3].Stunde == stunden[j]) //Wenn Stunde die normale Stunde ist
                            {
                                eintrag.Ursprungslehrer = stundenplan[tag].StundenDaten[i3].Lehrer;
                                stattlehrer = stundenplan[tag].StundenDaten[i3].Lehrer; //Lehrer der in der normalen Stunde unterrichtet
                                if (stattlehrer.Contains("/")) // Wenn es 2 Lehrer sind
                                {
                                    string[] beideStattLehrer = stattlehrer.Split('/');
                                    if (beideStattLehrer[0].CompareTo(beideStattLehrer[1]) > 0) // Wenn die 2 Lehrer nicht alphabetisch geordnet sind
                                    {
                                        eintrag.Ursprungslehrer = beideStattLehrer[1] + "/" + beideStattLehrer[0]; //Tausche Lehrer
                                    }
                                }
                            }
                        }
                    }
                    try
                    {
                        DatenbankSchreiben.SupplierungenEintragen(abtID, eintrag, globalEntfallID + 1);
                    }
                    catch
                    {
                        lbHinweis.Text = "Eintrag bereits vorhanden!";
                        lbHinweis.ForeColor = System.Drawing.Color.Red;
                        DatenbankSchreiben.GlobalEntfallLöschen(globalEntfallID + 1);
                        return;
                    }
                }
            }
            globalerEintrag.AbteilungsID = abtID;
            globalerEintrag.Datum = tb0.Text.ToDateTime();
            globalerEintrag.VonStunde = vonStunde;
            globalerEintrag.BisStunde = bisStunde;
            globalerEintrag.GlobalerEntfallID = globalEntfallID + 1;

            try
            {
                // ISv6: Return to table.aspx instead waiting for another entry
                //lbHinweis.Text = "Konferenz erfolgreich eingetragen!";
                //lbHinweis.ForeColor = System.Drawing.Color.Green;
                DatenbankSchreiben.GlobalEntfallEintragen(globalerEintrag);
                //tb0.Text = "";
                //tb1.Text = "";
                //tb2.Text = "";
                //Calendar1.SelectedDate = "".ToDateTime();
                Response.Redirect("table.aspx");
            }
            catch
            {
                lbHinweis.Text = "Eintrag bereits vorhanden!";
                lbHinweis.ForeColor = System.Drawing.Color.Red;
                return;
            }
        }
        protected void Calendar1_SelectionChanged(object sender, EventArgs e)
        {
            DateTime datum = Calendar1.SelectedDate;

            if ((datum.DayOfWeek.ToString() != "Saturday" && datum.DayOfWeek.ToString() != "Sunday") && (!(datum < DateTime.Today)))
            {
                tb0.Text = datum.ToString("yyyy-MM-dd");
                lbHinweis.Text = "";
            }
            else
            {
                tb0.Text = "";
                if (datum < DateTime.Today)
                {
                    lbHinweis.Text = "Ungültiges Datum (Vergangenheit)";
                }
                else
                {
                    lbHinweis.Text = "Ungültiges Datum (Wochenende)";
                }
                lbHinweis.ForeColor = System.Drawing.Color.Red;
                lbHinweis.Visible = true;
            }
        }

        protected void Zurueck_Click(object sender, EventArgs e)
        {
            Session["25_RadioButton"] = null;
            Response.Redirect("./table.aspx");
        }
    }
}