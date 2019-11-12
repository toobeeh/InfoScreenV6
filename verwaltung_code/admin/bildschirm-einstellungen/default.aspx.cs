using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using Infoscreen_Verwaltung.classes;

namespace Infoscreen_Verwaltung.admin.bildschirm_einstellungen
{
    public partial class _default : System.Web.UI.Page
    {
        string ebene = "../../";
        static string ZGAmode = "H";
        static Structuren.ZGA ZGAalt = new Structuren.ZGA();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!classes.Login.Angemeldet) Response.Redirect("../../login.aspx");
            if (!classes.Login.Rechte.Bildschirme.OneBiggerThan(1)) Response.Redirect("../../");

            TopMenu.Text = classes.Anzeige.TopBar(ebene);
            Menu.Text = classes.Anzeige.Menue(ebene, "admin");

            ZGALaden();

            if (!Page.IsPostBack)
            {
                string[] abteilungen;
                if (classes.Login.Rechte.Bildschirme[2].ToInt32() > 1)
                    abteilungen = classes.DatenbankAbrufen.AbteilungenAbrufen();
                else if (classes.Login.Rechte.Bildschirme[1].ToInt32() > 1)
                    abteilungen = classes.Login.Abteilungen;
                else abteilungen = new string[] { classes.Login.StammAbteilung };

                foreach (string abteilung in abteilungen)
                    dropDownAbteilung.Items.Add(abteilung);

                try { dropDownAbteilung.SelectedValue = classes.Login.StammAbteilung; }
                catch { dropDownAbteilung.SelectedIndex = -1; }
                if (Session["21_Abteilung"] != null)
                {
                    try { dropDownAbteilung.SelectedValue = Session["21_Abteilung"].ToString(); }
                    catch {}
                }

                Session["21_Abteilung"] = null;

                Structuren.Betriebsmodi[] betriebsmodi = DatenbankAbrufen.BetriebsmodiAbrufen();
                foreach (Structuren.Betriebsmodi temp in betriebsmodi)
                {
                    dropDownBetriebsmode.Items.Add(new ListItem(temp.bezeichnung, temp.id.ToString()));
                    dropDownBetriebsmode2.Items.Add(new ListItem(temp.bezeichnung, temp.id.ToString()));
                }


                if (classes.Login.Rechte.Bildschirme[2].ToInt32() > 2)
                {
                    btNeu.Visible = true;
                    btÄndern.Visible = true;
                }
                if (classes.Login.Rechte.Bildschirme[2].ToInt32() > 3)
                    btLöschen.Visible = true;

                ZGALaden();
                ZGAModusReset();
            }
        }

        protected void DropDownAbteilung_SelectedIndexChanged(object sender, EventArgs e)
        {
            ZGALaden();
            ZGAModusReset();
        }

        private void BetriebsmodeLaden()
        {
            try
            {
                dropDownBetriebsmode.SelectedValue = DatenbankAbrufen.AktuellenBetriebsmodeAbrufen(dropDownAbteilung.SelectedValue).ToString();
                dropDownBetriebsmode2.SelectedValue = DatenbankAbrufen.AktuellenBetriebsmodeAbrufen(dropDownAbteilung.SelectedValue).ToString();
            }
            catch
            {
                dropDownBetriebsmode.SelectedIndex = -1;
                dropDownBetriebsmode2.SelectedIndex = -1;
            }
        }

        private void ZGALaden() //SAIBL - Erstellt die Tabelle
        {
            DatenbankSchreiben.ZGAAbfragen(15);
            TischZGA.Rows.Clear();

            TableRow R = new TableRow();
            TableCell C = new TableCell();

            C = new TableCell();
            C.Text = "Betriebsmodus";
            C.CssClass = "head";
            R.Cells.Add(C);

            C = new TableCell();
            C.Text = "Datum und Zeit";
            C.Width = 188;
            C.CssClass = "head";
            R.Cells.Add(C);

            C = new TableCell();
            C.Text = "";
            C.Width = 65;
            C.CssClass = "head";
            R.Cells.Add(C);

            C = new TableCell();
            C.Text = "";
            C.Width = 70;
            C.CssClass = "head";
            R.Cells.Add(C);

            TischZGA.Rows.Add(R);

            Structuren.ZGA[] Data = DatenbankAbrufen.ZGAAbrufen(DatenbankAbrufen.GetAbteilungsIdVonAbteilungsname(dropDownAbteilung.SelectedValue));
            Structuren.Betriebsmodi[] Betriebsmodi = DatenbankAbrufen.BetriebsmodiAbrufen();

            if(Data.Length == 0)
            {
                TableRow tr = new TableRow();
                TableCell tc = new TableCell { ColumnSpan = 4 };
                tc.Text = "Keine zeitgesteuerte Anzeige geplant";
                tr.Cells.Add(tc);
                TischZGA.Rows.Add(tr);
            }
            
            for (int i = 0; i < Data.Length; i++) 
            {
                R = new TableRow();
                string Bez = "Kein Betriebsmodus gefunden!";

                for (int ii = 0; ii < Betriebsmodi.Length; ii++)
                {
                    if (Betriebsmodi[ii].id == Data[i].BetriebsmodeID) Bez = Betriebsmodi[ii].bezeichnung;
                }

                C = new TableCell();
                C.Text = "" + Bez;
                C.CssClass = "body";
                R.Cells.Add(C);

                C = new TableCell();
                C.Text = Data[i].Zeit.Day.ToString() + "." + Data[i].Zeit.Month.ToString() + "." + Data[i].Zeit.Year.ToString() + " - " + Data[i].Zeit.Hour.ToString("D2") + ":" + Data[i].Zeit.Minute.ToString("D2");
                C.CssClass = "body";
                R.Cells.Add(C);

                Button btnA = new Button();
                btnA.ID = Data[i].AbteilungsID.ToString() + "-" + Data[i].BetriebsmodeID.ToString() + "-" + Data[i].Zeit.ToString().Replace(':', 'D').Replace('.', 'P') + "-" + i + "-A";
                btnA.Text = "Ändern";
                btnA.CssClass = "ActionButton";
                btnA.Click += btnA_Click;

                C = new TableCell();
                C.Controls.Add(btnA);
                R.Cells.Add(C);

                Button btnL = new Button();
                btnL.ID = Data[i].AbteilungsID.ToString() + "-" + Data[i].BetriebsmodeID.ToString() + "-" + Data[i].Zeit.ToString().Replace(':', 'D').Replace('.', 'P') + "-" + i + "-L";
                btnL.Text = "Löschen";
                btnL.CssClass = "DeleteButton";
                btnL.Click += btnL_Click;

                C = new TableCell();
                C.Controls.Add(btnL);
                R.Cells.Add(C);

                TischZGA.Rows.Add(R);
            }
        }

        protected void btÜbernehmen_Click(object sender, EventArgs e)
        {
            DatenbankSchreiben.AktuellenBetriebsmodeÄndern(dropDownAbteilung.SelectedValue, dropDownBetriebsmode.SelectedValue.ToInt32());
        }

        protected void btNeu_Click(object sender, EventArgs e)
        {
            Session["21_Abteilung"] = dropDownAbteilung.SelectedValue;
            Session["21_Art"] = "neu";
            Response.Redirect("./add.aspx");
        }

        protected void btÄndern_Click(object sender, EventArgs e)
        {
            Session["21_Abteilung"] = dropDownAbteilung.SelectedValue;
            Session["21_Art"] = "ändern";
            Structuren.Betriebsmodi temp = new Structuren.Betriebsmodi();
            temp.bezeichnung = dropDownBetriebsmode.SelectedItem.Text;
            temp.id = dropDownBetriebsmode.SelectedValue.ToInt32();
            Session["21_Daten"] = temp;
            Response.Redirect("./add.aspx");
        }

        protected void btLöschen_Click(object sender, EventArgs e)
        {
            Session["21_Abteilung"] = dropDownAbteilung.SelectedValue;
            Session["21_Art"] = "löschen";
            Structuren.Betriebsmodi temp = new Structuren.Betriebsmodi();
            temp.bezeichnung = dropDownBetriebsmode.SelectedItem.Text;
            temp.id = dropDownBetriebsmode.SelectedValue.ToInt32();
            Session["21_Daten"] = temp;
            Response.Redirect("./add.aspx");
        }

        protected void btBetriebsmodeEinstellungen_Click(object sender, EventArgs e)
        {
            Session["21_Abteilung"] = dropDownAbteilung.SelectedValue;
            Structuren.Betriebsmodi temp = new Structuren.Betriebsmodi();
            temp.bezeichnung = dropDownBetriebsmode.SelectedItem.Text;
            temp.id = dropDownBetriebsmode.SelectedValue.ToInt32();
            Session["21_Daten"] = temp;
            Response.Redirect("./betriebsmode/");
        }
        protected void btnSave_Click(object sender, EventArgs e) //SAIBL
        {
            Regex Z = new Regex("^[0-9][0-9]:[0-9][0-9]$");
            string Zeit = tbZeit.Text;

            if (!Z.IsMatch(Zeit)) 
            {
                lbError.Text = "FEHLER: Ungültiges Zeitformat! - (hh:mm)";
                lbError.Visible = true;
            }
            else
            {
                Z = new Regex(":");
                string[] hhmm;
                hhmm = Z.Split(Zeit);
                int hh = Convert.ToInt32(hhmm[0]);
                int mm = Convert.ToInt32(hhmm[1]);

                if ((hh < 0 || hh > 24) || (mm < 0 || mm > 59))
                {
                    lbError.Text = "FEHLER: Ungültige Zeiteingabe! - (Max. 23:59)";
                    lbError.Visible = true;
                }
                else
                {
                    DateTime Datum = Kalender.SelectedDate;
                    Datum = Datum.AddHours(hh);
                    Datum = Datum.AddMinutes(mm);

                    if(DateTime.Compare(Datum, DateTime.Now.AddMinutes(1)) == -1)
                    {
                        lbError.Text = "FEHLER: Ungültige Zeiteingabe! - (Min. " + DateTime.Now.AddMinutes(1) + ")";
                        lbError.Visible = true;
                    }
                    else
                    {
                        Structuren.ZGA ZGA = new Structuren.ZGA();
                        ZGA.AbteilungsID = DatenbankAbrufen.GetAbteilungsIdVonAbteilungsname(dropDownAbteilung.SelectedValue);
                        ZGA.BetriebsmodeID = dropDownBetriebsmode2.SelectedValue.ToInt32();
                        ZGA.Zeit = Datum;

                        if (ZGAmode == "H")
                        {

                            if (DatenbankAbrufen.ZGAExists(ZGA.AbteilungsID, ZGA.Zeit = Datum))
                            {
                                lbError.Text = "FEHLER: Es existiert bereits ein Eintrag mit dieser Abteilung und Zeit!";
                                lbError.Visible = true;
                            }
                            else
                            {
                                DatenbankSchreiben.ZGASetzen(ZGA);

                                ZGALaden();
                                ZGAModusReset();
                            }
                        }
                        else
                        {
                            string[] mode = ZGAmode.Split('-');
                            if(mode[0] == "A")
                            {
                                DatenbankSchreiben.ZGAAendern(ZGAalt, ZGA);

                                TischZGA.Rows[mode[1].ToInt32()].BackColor = System.Drawing.Color.White;

                                ZGALaden();
                                ZGAModusReset();
                            }
                            else
                            {
                                lbError.Text = "FEHLER: Es ist ein unbekannter Fehler aufgetreten!";
                                lbError.Visible = true;
                            }
                        }
                    }
                }
            }
        }
        protected void btnL_Click(object sender, EventArgs e)
        {
            Button dummy = (Button)sender;
            string[] dummy2 = dummy.ID.Split('-');
            Structuren.ZGA dummy3 = new Structuren.ZGA();
            dummy3.AbteilungsID = dummy2[0].ToInt32();
            dummy3.BetriebsmodeID = dummy2[1].ToInt32();
            dummy3.Zeit = dummy2[2].Replace('D', ':').Replace('P', '.').ToDateTime();

            DatenbankSchreiben.ZGALoeschen(dummy3);

            ZGALaden();
            ZGAModusReset();
        }
        protected void btnA_Click(object sender, EventArgs e)
        {
            if(ZGAmode != "H")
            {
                string[] mode = ZGAmode.Split('-');
                TischZGA.Rows[mode[1].ToInt32()].BackColor = System.Drawing.Color.White;
            }
            Button dummy = (Button)sender;
            string[] dummy2 = dummy.ID.Split('-');
            Structuren.ZGA dummy3 = new Structuren.ZGA();
            dummy3.AbteilungsID = dummy2[0].ToInt32();
            dummy3.BetriebsmodeID = dummy2[1].ToInt32();
            dummy3.Zeit = dummy2[2].Replace('D', ':').Replace('P', '.').ToDateTime();

            int Zeile = dummy2[3].ToInt32() + 1;
            TischZGA.Rows[Zeile].BackColor = System.Drawing.Color.Yellow;

            lbKalender.Text = "Zeitgesteuerte Anzeige ändern:";
            Kalender.SelectedDate = dummy3.Zeit.Date;
            tbZeit.Text = dummy3.Zeit.Hour.ToString("D2") + ":" + dummy3.Zeit.Minute.ToString("D2");
            dropDownBetriebsmode2.SelectedValue = dummy3.BetriebsmodeID.ToString();

            ZGAmode = "A-" + Zeile;
            ZGAalt = dummy3;
        }
        private void ZGAModusReset()
        {
            lbKalender.Text = "Zeitgesteuerte Anzeige hinzufügen:";
            Kalender.SelectedDate = DateTime.Today;
            tbZeit.Text = "00:00";
            lbError.Visible = false;
            ZGAmode = "H";
            ZGAalt = new Structuren.ZGA();
            BetriebsmodeLaden();
        }
    }
}