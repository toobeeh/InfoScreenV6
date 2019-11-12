using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Infoscreen_Verwaltung.classes;
using System.Web.Services;
using System.Web.Script.Services;

namespace Infoscreen_Verwaltung.conf
{
    public partial class stundenplan : System.Web.UI.Page
    {
        string ebene = "../";

        struct StundenplanBoxen
        {
            public TextBox Fach;
            public TextBox Lehrer;
            public TextBox Gebäude;
            public TextBox Raum;
        }

        StundenplanBoxen[,] daten;

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<string> GetFach(string pre)
        {
            List<string> fächer = new List<string>();
            using (classes.Entities db = new classes.Entities())
            {
                fächer = (from a in db.Fächer where (a.FachKürzel.StartsWith(pre)) select a.FachKürzel).ToList();
            }
            fächer.DoppelteEinträgeEntfernen();
            return fächer;
        }

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
            if (!classes.Login.Angemeldet) Response.Redirect("../login.aspx");
            if (!classes.Login.Rechte.Stundenplan.OneBiggerThan(1)) Response.Redirect("../");
            
            TopMenu.Text = classes.Anzeige.TopBar(ebene);
            Menu.Text = classes.Anzeige.Menue(ebene, "conf");

            if (!Page.IsPostBack)
            {
                string[] klassen;
                if (classes.Login.Rechte.Stundenplan[2].ToInt32() > 1) //Superadmin
                    klassen = classes.DatenbankAbrufen.KlassenAbrufen("");
                else if (classes.Login.Rechte.Stundenplan[1].ToInt32() > 1) //AV, Schulwart
                    klassen = classes.DatenbankAbrufen.KlassenAbrufen(classes.Login.StammAbteilung);
                else
                    klassen = new string[] { classes.Login.StammKlasse };

                foreach (string klasse in klassen)
                    DropDownListKlasse.Items.Add(klasse);

                try
                {
                    DropDownListKlasse.SelectedValue = classes.Login.StammKlasse;
                    if (Session["10_Klasse"] != null)
                        DropDownListKlasse.SelectedValue = Session["10_Klasse"].ToString();
                }
                catch { DropDownListKlasse.SelectedIndex = -1; }
            }
            TabelleZeichnen();
            Session["10_Klasse"] = null;
        }
        
        protected void DropDownListKlasse_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["10_Klasse"] = DropDownListKlasse.SelectedValue;
            Response.Redirect("./stundenplan.aspx");
        }

        protected void TabelleZeichnen()
        {
            string klasse = DropDownListKlasse.SelectedValue;
            classes.Structuren.StundenplanTag[] stundenplan = classes.DatenbankAbrufen.StundenplanAbrufen(klasse, false);
            
            //DEBUG!!!
            if (stundenplan.Length == 0) stundenplan = new Structuren.StundenplanTag[5];

            TableRow row1, row2, row3;
            TableCell cell;
            bool eintrag;
            Label trennung;
            daten = new StundenplanBoxen[stundenplan.Length, Properties.Resources.maximaleStunden.ToInt32() + 1];

            row1 = new TableRow();
            cell = new TableCell();
            cell.CssClass = "head";
            row1.Cells.Add(cell);

            cell = new TableCell();
            cell.CssClass = "head";
            row1.Cells.Add(cell);

            cell = new TableCell();
            cell.CssClass = "head";
            cell.Text = "Montag";
            row1.Cells.Add(cell);

            cell = new TableCell();
            cell.CssClass = "head";
            cell.Text = "Dienstag";
            row1.Cells.Add(cell);

            cell = new TableCell();
            cell.CssClass = "head";
            cell.Text = "Mittwoch";
            row1.Cells.Add(cell);

            cell = new TableCell();
            cell.CssClass = "head";
            cell.Text = "Donnerstag";
            row1.Cells.Add(cell);

            cell = new TableCell();
            cell.CssClass = "head";
            cell.Text = "Freitag";
            row1.Cells.Add(cell);

            if (stundenplan.Length > 5)
            {
                cell = new TableCell();
                cell.CssClass = "head";
                cell.Text = "Samstag";
                row1.Cells.Add(cell);
            }
            Stundenplan.Rows.Add(row1);

            for (int i1 = 0; i1 <= Properties.Resources.maximaleStunden.ToInt32(); i1++)
            {
                row1 = new TableRow();
                row2 = new TableRow();
                row3 = new TableRow();
                cell = new TableCell();
                cell.Text = i1.ToString();
                cell.RowSpan = 3;
                row1.Cells.Add(cell);

                cell = new TableCell();
                cell.Text = "Fach:";
                row1.Cells.Add(cell);

                cell = new TableCell();
                cell.Text = "Lehrer:";
                cell.ToolTip = "Mehrere Lehrer durch '/' trennen";
                row2.Cells.Add(cell);

                cell = new TableCell();
                cell.Text = "Gebäude - Raum:";
                row3.Cells.Add(cell);

                for (int i2 = 0; i2 < stundenplan.Length; i2++)
                {
                    eintrag = false;

                    try
                    {
                        for (int i3 = 0; i3 < stundenplan[i2].StundenDaten.Length; i3++)
                        {
                            if (stundenplan[i2].StundenDaten[i3].Stunde == i1)
                            {
                                cell = new TableCell();
                                daten[i2, i1].Fach = new TextBox();
                                daten[i2, i1].Fach.Text = stundenplan[i2].StundenDaten[i3].Fach;
                                daten[i2, i1].Fach.CssClass = "autocompFach";
                                cell.Controls.Add(daten[i2, i1].Fach);
                                row1.Cells.Add(cell);

                                cell = new TableCell();
                                daten[i2, i1].Lehrer = new TextBox();
                                daten[i2, i1].Lehrer.Text = stundenplan[i2].StundenDaten[i3].Lehrer;
                                daten[i2, i1].Lehrer.CssClass = "autocompLehrer";
                                cell.Controls.Add(daten[i2, i1].Lehrer);
                                row2.Cells.Add(cell);

                                cell = new TableCell();
                                daten[i2, i1].Gebäude = new TextBox();
                                if (stundenplan[i2].StundenDaten[i3].Gebäude != "" && stundenplan[i2].StundenDaten[i3].Raum > -1)
                                    daten[i2, i1].Gebäude.Text = stundenplan[i2].StundenDaten[i3].Gebäude;
                                else
                                    daten[i2, i1].Gebäude.Text = "";
                                daten[i2, i1].Gebäude.MaxLength = 5;
                                daten[i2, i1].Gebäude.Width = 50;
                                trennung = new Label();
                                trennung.Text = " - ";
                                daten[i2, i1].Raum = new TextBox();
                                if (stundenplan[i2].StundenDaten[i3].Gebäude != "" && stundenplan[i2].StundenDaten[i3].Raum > -1)
                                    daten[i2, i1].Raum.Text = stundenplan[i2].StundenDaten[i3].Raum.ToString("0000");
                                else
                                    daten[i2, i1].Gebäude.Text = "";
                                daten[i2, i1].Raum.MaxLength = 3;
                                daten[i2, i1].Raum.Width = 30;
                                cell.Controls.Add(daten[i2, i1].Gebäude);
                                cell.Controls.Add(trennung);
                                cell.Controls.Add(daten[i2, i1].Raum);
                                row3.Cells.Add(cell);

                                eintrag = true;
                                break;
                            }
                        }
                    }
                    catch {}
                    if (!eintrag)
                    {
                        cell = new TableCell();
                        daten[i2, i1].Fach = new TextBox();
                        daten[i2, i1].Fach.Text = "";
                        daten[i2, i1].Fach.CssClass = "autocompFach";
                        cell.Controls.Add(daten[i2, i1].Fach);
                        row1.Cells.Add(cell);

                        cell = new TableCell();
                        daten[i2, i1].Lehrer = new TextBox();
                        daten[i2, i1].Lehrer.Text = "";
                        daten[i2, i1].Lehrer.CssClass = "autocompLehrer";
                        cell.Controls.Add(daten[i2, i1].Lehrer);
                        row2.Cells.Add(cell);

                        cell = new TableCell();
                        daten[i2, i1].Gebäude = new TextBox();
                        daten[i2, i1].Gebäude.Text = "";
                        daten[i2, i1].Gebäude.MaxLength = 5;
                        daten[i2, i1].Gebäude.Width = 50;
                        trennung = new Label();
                        trennung.Text = " - ";
                        daten[i2, i1].Raum = new TextBox();
                        daten[i2, i1].Raum.Text = "";
                        daten[i2, i1].Raum.MaxLength = 3;
                        daten[i2, i1].Raum.Width = 30;
                        cell.Controls.Add(daten[i2, i1].Gebäude);
                        cell.Controls.Add(trennung);
                        cell.Controls.Add(daten[i2, i1].Raum);
                        row3.Cells.Add(cell);
                    }
                }

                Stundenplan.Rows.Add(row1);
                Stundenplan.Rows.Add(row2);
                Stundenplan.Rows.Add(row3);
            }
        }

        protected void btÜbernehmen_Click(object sender, EventArgs e)
        {
            List<string> fehler = new List<string>();
            for(int i1 = 0; i1 < daten.GetLength(0); i1++)
            {
                for (int i2 = 0; i2 < daten.GetLength(1); i2++)
                {
                    if (daten[i1, i2].Fach.Text != "" && daten[i1, i2].Lehrer.Text != "")
                    {
                        if (daten[i1, i2].Raum.Text != "" && daten[i1, i2].Raum.Text.ToInt32() < 0)
                            fehler.Add(String.Format("'{0}' ist kein gültiger Raum. Räume dürfen nur aus Zahlen bestehen!", daten[i1, i2].Raum.Text));
                        if (daten[i1, i2].Gebäude.Text != "" && daten[i1, i2].Raum.Text != "")
                        {
                            using (Entities db = new Entities())
                            {
                                string gebäude = daten[i1, i2].Gebäude.Text;
                                int raum = daten[i1, i2].Raum.Text.ToInt32();
                                if (db.Raeume.Where(a => a.Gebäude == gebäude && a.Raum == raum).FirstOrDefault() == null)
                                    fehler.Add(String.Format("Die Kombination aus '{0}' als Gebäude und '{1} als Raum existiert nicht!", daten[i1, i2].Gebäude.Text, daten[i1, i2].Raum.Text));
                            }
                        }

                        using (Entities db = new Entities())
                        {
                            string fach = daten[i1, i2].Fach.Text;
                            if (db.Fächer.Where(a => a.FachKürzel == fach).FirstOrDefault() == null)
                                fehler.Add(String.Format("Das angegebene Fach '{0}' existiert nicht!", daten[i1, i2].Fach.Text));

                            foreach(string lehrer in daten[i1, i2].Lehrer.Text.Split('/'))
                            {
                                if (db.LehrerTesten1.Where(a => a.LehrerKuerzel == lehrer).FirstOrDefault() == null)
                                    fehler.Add(String.Format("Der Lehrer mit dem Kürzel '{0}' existiert nicht.", lehrer));
                            }
                        }
                    }

                    
                }
            }
            if (fehler.Count > 0)
            {
                LabelFehler.Text = "<ul>\r\n<li>" + String.Join("</li>\r\n<li>", fehler) + "</li>\r\n</ul>";
            }
            else
            {
                LabelFehler.Text = "";
                string klasse = DropDownListKlasse.SelectedValue;

                DatenbankSchreiben.StundenplanLeeren(klasse);

                for(int i1 = 0; i1 < daten.GetLength(0); i1++)
                {
                    for (int i2 = 0; i2 < daten.GetLength(1); i2++)
                    {
                        if (daten[i1, i2].Fach.Text != "" && daten[i1, i2].Lehrer.Text != "")
                        {
                            using (Entities db = new Entities())
                            {
                                Stundenplan eintrag = new classes.Stundenplan();
                                eintrag.AbteilungsID = DatenbankAbrufen.GetAbteilungsIDVonKlasse(klasse);
                                eintrag.FachKürzel = daten[i1, i2].Fach.Text;
                                eintrag.Klasse = klasse;
                                eintrag.LehrerKürzel = daten[i1, i2].Lehrer.Text;
                                eintrag.Stunde = i2;
                                eintrag.Wochentag = i1;
                                if (daten[i1, i2].Gebäude.Text != "" && daten[i1, i2].Raum.Text != "")
                                {
                                    eintrag.Standardraum = false;
                                    eintrag.Gebäude = daten[i1, i2].Gebäude.Text;
                                    eintrag.Raum = daten[i1, i2].Raum.Text.ToInt32();
                                }
                                else
                                {
                                    eintrag.Standardraum = true;
                                }
                                db.Stundenplan.Add(eintrag);
                                db.SaveChanges();
                            }
                        }
                    }
                }
                Session["10_Klasse"] = DropDownListKlasse.SelectedValue;
                Response.Redirect("./stundenplan.aspx");
            }
        }

        protected void btReset_Click(object sender, EventArgs e)
        {
            Session["10_Klasse"] = DropDownListKlasse.SelectedValue;
            Response.Redirect("./stundenplan.aspx");
        }
    }
}