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
    public partial class lehrersupplierung : System.Web.UI.Page
    {
        static bool tabellezeichnen = false;
        static List<TextBox> supplehrer = new List<TextBox>();
        static List<ToggleButton> entfall = new List<ToggleButton>();

        string ebene = "../../";

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<string> GetLehrer(string pre)
        {
            string front;
            if (pre.Contains('/'))
            {
                front = pre.Remove(pre.LastIndexOf('/')) + "/";
            }
            else
            {
                front = "";
            }

            try { pre = pre.Remove(0, pre.LastIndexOf('/') + 1); }
            catch { }

            List<string> lehrer = new List<string>();
            using (classes.Entities db = new classes.Entities())
            {
                lehrer = (from a in db.LehrerTesten1 where (a.LehrerKuerzel.StartsWith(pre)) select a.LehrerKuerzel).ToList();
            }
            lehrer.DoppelteEinträgeEntfernen();

            for (int i = 0; i < lehrer.Count; i++)
            {
                lehrer[i] = front + lehrer[i];
            }
            return lehrer;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!classes.Login.Angemeldet) Response.Redirect("../../login.aspx");
            if (!classes.Login.Rechte.Supplierung.OneBiggerThan(1)) Response.Redirect("../../");

            TopMenu.Text = classes.Anzeige.TopBar(ebene);
            Menu.Text = classes.Anzeige.Menue(ebene, "conf");

            if (!Page.IsPostBack)
            {
                string[] abteilungen;
                if (classes.Login.Rechte.Supplierung[2].ToInt32() > 1) // Superadmin
                    abteilungen = classes.DatenbankAbrufen.AbteilungenAbrufen();
                else if (classes.Login.Rechte.Supplierung[1].ToInt32() > 1) // Schulwart, AV
                    abteilungen = classes.Login.Abteilungen;
                else abteilungen = new string[] { "Elektronik" }; //Schüler, Lehrer

                foreach (string abteilung in abteilungen)
                    DropDownListAbteilung.Items.Add(abteilung);

                    DropDownListAbteilung.SelectedValue = classes.Login.StammAbteilung;

                string[] lehrer;             
                lehrer = classes.DatenbankAbrufen.LehrerKurzel(classes.DatenbankAbrufen.GetAbteilungsIdVonAbteilungsname(DropDownListAbteilung.SelectedValue));

                foreach (string einlehrer in lehrer)
                    DropDownListLehrer.Items.Add(einlehrer);

                Session["25_Abteilung"] = null;
                Session["25_Lehrer"] = null;
                Session["25_Datum"] = null;
            }

            if (Session["25_Datum"] != null)
            TabelleZeichnen();

            SupplierungKlasse.Text = "Supplierung für " + DropDownListLehrer.SelectedValue.ToString() + " eintragen";
        }

        protected void DropDownListAbteilung_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["25_Abteilung"] = DropDownListAbteilung.SelectedValue;
            Session["25_Lehrer"] = DropDownListLehrer.SelectedValue;
            Session["25_Datum"] = Calendar1.SelectedDate;

            SupplierungKlasse.Text = "Supplierung für " + Session["25_Lehrer"].ToString() + " eintragen";
            TabelleZeichnen();
        }
        protected void DropDownListLehrer_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["25_Abteilung"] = DropDownListAbteilung.SelectedValue;
            Session["25_Lehrer"] = DropDownListLehrer.SelectedValue;
            Session["25_Datum"] = Calendar1.SelectedDate;

            SupplierungKlasse.Text = "Supplierung für " + Session["25_Lehrer"].ToString() + " eintragen";
            DatumPruefen();
        }
     
        protected void Calendar1_SelectionChanged(object sender, EventArgs e)
        {
            DatumPruefen();
        }
        protected void Bestaetigen_Click(object sender, EventArgs e)
        {
            int abtID = DatenbankAbrufen.GetAbteilungsIdVonAbteilungsname(DropDownListAbteilung.SelectedValue);
            lbHinweis.Visible = true;
            lbHinweis.Text = "Nichts ausgewählt";
            lbHinweis.ForeColor = System.Drawing.Color.Red;


            classes.Structuren.StundenplanTagLehrer[] stundenplan = classes.DatenbankAbrufen.GetLehrerStundenplanfürDatum(DropDownListLehrer.SelectedValue, Calendar1.SelectedDate);
            if (stundenplan.Length == 0)
            {
                return;
            }
            if(tbGrund.Text=="")
            {
                lbHinweis.Text = "Bitte Grund anführen!";
                return;
            }
            classes.Structuren.Supplierungen eintrag = new Structuren.Supplierungen();

            eintrag.Datum = Calendar1.SelectedDate;
            eintrag.Ursprungslehrer = DropDownListLehrer.SelectedValue;
            eintrag.ZiehtVor = -1;
            eintrag.ZiehtVorDatum = "".ToDateTime();

            for (int i = 0; i < stundenplan.Length; i++)
            {
                eintrag.Klasse = stundenplan[i].Klasse;
                eintrag.Stunde = stundenplan[i].Stunde;
                eintrag.Ersatzfach = stundenplan[i].Fach;

                if (supplehrer[i].Text == "")
                {
                    if (entfall[i].Checked == true) //Entfall
                    {
                        eintrag.Entfällt = true;                        
                        eintrag.Ersatzlehrer = DropDownListLehrer.SelectedValue;

                        try
                        {
                            DatenbankSchreiben.SupplierungenEintragen(abtID, eintrag, -1, tbGrund.Text);
                            //Response.Redirect("table.aspx");
                            // ISv6: Redir to table
                            supplehrer[i].Enabled = true;
                            entfall[i].Checked = false;
                            lbHinweis.Text = "Supplierung erfolgreich eingetragen!";
                            lbHinweis.ForeColor = System.Drawing.Color.Green;
                        }
                        catch
                        {
                            lbHinweis.Text = "Eintrag für " + eintrag.Stunde.ToString() + " .Stunde bereits vorhanden!";
                            lbHinweis.ForeColor = System.Drawing.Color.Red;
                            return;
                        }
                    }
                }
                else //Supplierung
                {
                    if (supplehrer[i].Text.IndexOf('/') == -1) // Wenn 1 Lehrer eingegeben wurde
                    {
                        if (DatenbankAbrufen.LehrerKuerzelExists(supplehrer[i].Text, abtID) == 0) // Wenn Lehrer Kürzel nicht vorhanden
                        {
                            lbHinweis.Text = "Lehrerkürzel nicht vorhanden!";
                            lbHinweis.ForeColor = System.Drawing.Color.Red;
                            return;
                        }
                    }
                    else // Wenn 2 Lehrer eingegeben wurden
                    {

                        string[] lehrer = supplehrer[i].Text.ToUpper().Split('/'); // Speichert beide Lehrer in ein Array

                        if (DatenbankAbrufen.LehrerKuerzelExists(lehrer[0], abtID) == 0 ||
                            DatenbankAbrufen.LehrerKuerzelExists(lehrer[1], abtID) == 0) // Wenn Lehrer Kürzel nicht vorhanden
                        {
                            lbHinweis.Text = "Lehrerkürzel nicht vorhanden!";
                            lbHinweis.ForeColor = System.Drawing.Color.Red;
                            return;
                        }
                        if (lehrer[0].CompareTo(lehrer[1]) > 0) // Wenn die 2 Lehrer nicht alphabethisch nicht geordnet sind
                        {
                            supplehrer[i].Text = lehrer[1] + "/" + lehrer[0]; //Tausche Lehrer
                        }
                    }

                    eintrag.Entfällt = false;
                    eintrag.Ersatzlehrer = supplehrer[i].Text;
                    try
                    {
                        DatenbankSchreiben.SupplierungenEintragen(abtID, eintrag, -1, tbGrund.Text);
                        supplehrer[i].Text = "";
                        lbHinweis.Text = "Supplierung erfolgreich eingetragen!";
                        //Response.Redirect("table.aspx");
                        // ISv6: Redir to table
                        lbHinweis.ForeColor = System.Drawing.Color.Green;
                    }
                    catch
                    {
                        lbHinweis.Text = "Eintrag für diese Stunde bereits vorhanden!";
                        lbHinweis.ForeColor = System.Drawing.Color.Red;
                        return;
                    }
                }
            }
        }
             
        protected void TabelleZeichnen()
        {
            tbLehrerStundenplan.Rows.Clear();

            if (tabellezeichnen == false) return;

            lbHinweis.Visible = false;

            supplehrer = new List<TextBox>();
            entfall = new List<ToggleButton>();

            TableRow tr;
            TableCell tc;
            TextBox tb;
            ToggleButton cb;
            Label lb;

            classes.Structuren.StundenplanTagLehrer[] stundenplan = classes.DatenbankAbrufen.GetLehrerStundenplanfürDatum(Session["25_Lehrer"].ToString(), Session["25_Datum"].ToDateTime());

            if (stundenplan.Length == 0)
            {
                tr = new TableRow();
                tc = new TableCell();
                lb = new Label();
                lb.Text = "<h2>Keine Stunden an diesem Tag</h2>";
                tc.CssClass = "head";
                tc.Controls.Add(lb);
                tr.Cells.Add(tc);
                tbLehrerStundenplan.Rows.Add(tr);
                return;
            }
            int i1;
            for (i1 = 0; i1 <= 4; i1++)
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
                            tc.Text = "Klasse";
                            tr.Cells.Add(tc);
                        }
                        tc = new TableCell(); //klasse
                        tc.Text = stundenplan[i2].Klasse;
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
                            tc.Text = "Ersatz-Lehrer";
                            tr.Cells.Add(tc);
                        }
                        tc = new TableCell(); //Lehrer suppliert
                        tb = new TextBox();
                        tb.Width = 50;
                        tb.CssClass = "autocompLehrer";
                        tb.Text = "";
                        supplehrer.Add(tb);
                        tc.Controls.Add(tb);
                        tr.Cells.Add(tc);
                    }
                    if (i1 == 4)
                    {
                        if (i2 == 0)
                        {
                            tc = new TableCell();
                            tc.Text = "Entfall";
                            tr.Cells.Add(tc);
                        }
                        tc = new TableCell(); //Entfall
                        cb = new ToggleButton();
                        cb.Checked = false;
                        cb.CheckElement.AutoPostBack = true;
                        cb.CheckElement.CheckedChanged += new EventHandler(cb_click);
                        cb.Text = i2.ToString();
                        cb.ForeColor = System.Drawing.Color.White;
                        cb.ID = i2.ToString();
                        entfall.Add(cb);
                        tc.Controls.Add(cb);
                        tr.Cells.Add(tc);
                    }
                }
                tbLehrerStundenplan.Rows.Add(tr);
            }
        }
        private void cb_click(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            entfall[cb.Text.ToInt32()].Checked = cb.Checked;
            
            if(cb.Checked == true)
            {
                supplehrer[cb.Text.ToInt32()].Text = "";
                supplehrer[cb.Text.ToInt32()].Enabled = false;
            }
            else
            {
                supplehrer[cb.Text.ToInt32()].Enabled = true;
            }
        }
        protected void Zurueck_Click(object sender, EventArgs e)
        {
            Session["25_RadioButton"] = null;
            Response.Redirect("./table.aspx");
        }
        private void DatumPruefen()
        {
            tabellezeichnen = false;

            DateTime datum = Calendar1.SelectedDate;

            if (datum.DayOfWeek.ToString() == "Saturday" || datum.DayOfWeek.ToString() == "Sunday" || datum < DateTime.Today)
            {
                lbHinweis.ForeColor = System.Drawing.Color.Red;
                lbHinweis.Visible = true;

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
                }
                else
                {
                    lbHinweis.Text = "";
                }
            }
            else
            {
                tabellezeichnen = true;
            }
            Session["25_Abteilung"] = DropDownListAbteilung.SelectedValue;
            Session["25_Lehrer"] = DropDownListLehrer.SelectedValue;
            Session["25_Datum"] = Calendar1.SelectedDate;

            SupplierungKlasse.Text = "Supplierung für " + Session["25_Lehrer"].ToString() + " eintragen";

            TabelleZeichnen();
        }
    }
}