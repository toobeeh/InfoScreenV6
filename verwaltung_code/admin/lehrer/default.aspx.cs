using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using Infoscreen_Verwaltung.classes;
using System.Net;
using System.ComponentModel;

namespace Infoscreen_Verwaltung.admin.lehrer
{
    public partial class _default : System.Web.UI.Page
    {
        string ebene = "../../";
        static string LEmode = "Anzeigen"; //Anzeigen - Hinzufügen - Bearbeiten
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!classes.Login.Angemeldet) Response.Redirect("../../login.aspx");
            if (!classes.Login.Rechte.Bildschirme.OneBiggerThan(1)) Response.Redirect("../../");

            TopMenu.Text = classes.Anzeige.TopBar(ebene);
            Menu.Text = classes.Anzeige.Menue(ebene, "admin");

            LELaden();

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

                LEModusReset();
                LELaden();
            }
        }

        protected void DropDownAbteilung_SelectedIndexChanged(object sender, EventArgs e)
        {
            LEModusReset();
            LELaden();
        }

        private void LELaden()
        {

            TischLE.Rows.Clear();

            TableRow R = new TableRow();
            TableCell C = new TableCell();

            C = new TableCell();
            C.Text = "Lehrerkürzel";
            C.Width = 42;
            C.CssClass = "head";
            R.Cells.Add(C);

            C = new TableCell();
            C.Text = "Vorname";
            C.CssClass = "head";
            R.Cells.Add(C);

            C = new TableCell();
            C.Text = "Nachname";
            C.CssClass = "head";
            R.Cells.Add(C);

            C = new TableCell();
            C.Text = "Sprechstunde";
            C.Width = 42;
            C.CssClass = "head";
            R.Cells.Add(C);

            C = new TableCell();
            C.Text = "Tag";
            C.Width = 42;
            C.CssClass = "head";
            R.Cells.Add(C);

            C = new TableCell();
            C.Text = "Klassenvorstand<br/>von Klasse";
            C.CssClass = "head";
            R.Cells.Add(C);

            C = new TableCell();
            C.Text = "Raum";
            C.Width = 42;
            C.CssClass = "head";
            R.Cells.Add(C);

            if (LEmode != "Anzeigen")
            {
                C = new TableCell();
                C.Text = "Gebäude";
                C.Width = 42;
                C.CssClass = "head";
                R.Cells.Add(C);
            }
            Structuren.LE[] Data = null;

            if (LEmode != "Anzeigen")
            {
                C = new TableCell();
                C.Text = "AbteilungsID";
                C.CssClass = "head";
                R.Cells.Add(C);
            }
            
            if (LEmode != "Hinzufügen")
            {
                C = new TableCell(); //Spalte für die Löschen-Buttons
                C.Text = "";
                C.Width = 70;
                C.CssClass = "head";
                R.Cells.Add(C);

                Data = DatenbankAbrufen.LehrerEigenschaftenAbrufen(DatenbankAbrufen.GetAbteilungsIdVonAbteilungsname(dropDownAbteilung.SelectedValue), LEmode);
            }

            TischLE.Rows.Add(R);

            switch (LEmode)
            {
                case "Anzeigen":

                    for (int i = 0; i < Data.Length; i++)
                    {
                        R = new TableRow();

                        C = new TableCell();
                        C.Text = Data[i].LehrerKuerzel;
                        C.CssClass = "body";
                        R.Cells.Add(C);

                        C = new TableCell();
                        C.Text = Data[i].Vorname;
                        C.CssClass = "body";
                        R.Cells.Add(C);

                        C = new TableCell();
                        C.Text = Data[i].Nachname;
                        C.CssClass = "body";
                        R.Cells.Add(C);

                        C = new TableCell();
                        C.Text = Data[i].Sprechstunde.GetZeitenOfStunde();
                        C.CssClass = "body";
                        R.Cells.Add(C);

                        C = new TableCell();
                        C.Text = Data[i].Tag.GetDayOfInt();
                        C.CssClass = "body";
                        R.Cells.Add(C);

                        C = new TableCell();
                        C.Text = SetKlasseString(Data[i].KlassenvorstandKlasse);
                        C.CssClass = "body";
                        R.Cells.Add(C);

                        C = new TableCell();
                        if (Data[i].Gebäude.ToString() != "" && Data[i].Raum != -1)
                            C.Text = Data[i].Gebäude + "-" + Data[i].Raum.ToString("D4");
                        else if (Data[i].Gebäude != "")
                            C.Text = Data[i].Gebäude;
                        else
                            C.Text = "";
                        C.CssClass = "body";
                        R.Cells.Add(C);

                        /*C = new TableCell();
                        C.Text = Data[i].Gebäude;
                        C.CssClass = "body";
                        R.Cells.Add(C);*/

                        Button btnL = new Button();
                        btnL.ID = Data[i].LehrerKuerzel + "-btnL";
                        btnL.CssClass = "DeleteButton";
                        btnL.Text = "Löschen";
                        btnL.Click += btnL_Click;

                        C = new TableCell();
                        C.Controls.Add(btnL);
                        R.Cells.Add(C);

                        TischLE.Rows.Add(R);

                        btnAdd.Visible = true;
                        btnChange.Visible = true;
                        btnSave.Visible = false;
                        SprechstundenUpdate.Visible = true;
                    }

                    break;

                case "Bearbeiten":

                    for (int i = 0; i < Data.Length; i++)
                    {
                        R = new TableRow();

                        C = new TableCell();
                        C.Text = Data[i].LehrerKuerzel;
                        C.CssClass = "body";
                        R.Cells.Add(C);

                        TextBox tbVorname = new TextBox();
                        tbVorname.ID = Data[i].LehrerKuerzel + "-tbVorname";
                        tbVorname.Text = Data[i].Vorname;
                        C = new TableCell();
                        C.Controls.Add(tbVorname);
                        R.Cells.Add(C);

                        TextBox tbNachname = new TextBox();
                        tbNachname.ID = Data[i].LehrerKuerzel + "-tbNachname";
                        tbNachname.Text = Data[i].Nachname;
                        C = new TableCell();
                        C.Controls.Add(tbNachname);
                        R.Cells.Add(C);

                        TextBox tbSprechstunde = new TextBox();
                        tbSprechstunde.ID = Data[i].LehrerKuerzel + "-tbSprechstunde";
                        tbSprechstunde.Text = Data[i].Sprechstunde.ToString();
                        C = new TableCell();
                        C.Controls.Add(tbSprechstunde);
                        R.Cells.Add(C);

                        TextBox tbTag = new TextBox();
                        tbTag.ID = Data[i].LehrerKuerzel + "-tbTag";
                        tbTag.Text = Data[i].Tag.ToString();
                        tbTag.Width = 50;
                        C = new TableCell();
                        C.Controls.Add(tbTag);
                        R.Cells.Add(C);

                        TextBox tbKlassenvorstandKlasse = new TextBox();
                        tbKlassenvorstandKlasse.ID = Data[i].LehrerKuerzel + "-tbKlassenvorstandKlasse";
                        tbKlassenvorstandKlasse.Text = SetKlasseString(Data[i].KlassenvorstandKlasse);
                        C = new TableCell();
                        C.Controls.Add(tbKlassenvorstandKlasse);
                        R.Cells.Add(C);

                        TextBox tbRaum = new TextBox();
                        tbRaum.ID = Data[i].LehrerKuerzel + "-tbRaum";
                        tbRaum.Text = Data[i].Raum.ToString("D4");
                        tbRaum.Width = 75;
                        C = new TableCell();
                        C.Controls.Add(tbRaum);
                        R.Cells.Add(C);

                        TextBox tbGebäude = new TextBox();
                        tbGebäude.ID = Data[i].LehrerKuerzel + "-tbGebäude";
                        tbGebäude.Text = Data[i].Gebäude;
                        tbGebäude.Width = 75;
                        C = new TableCell();
                        C.Controls.Add(tbGebäude);
                        R.Cells.Add(C);

                        TextBox tbAbteilungen = new TextBox();
                        tbAbteilungen.ID = Data[i].LehrerKuerzel + "-tbAbteilungen";
                        tbAbteilungen.Text = SetAbtString(Data[i].AbteilungsIDs);
                        C = new TableCell();
                        C.Controls.Add(tbAbteilungen);
                        R.Cells.Add(C);

                        Button btnL = new Button();
                        btnL.ID = Data[i].LehrerKuerzel + "-btnL";
                        btnL.CssClass = "DeleteButton";
                        btnL.Text = "Löschen";
                        btnL.Click += btnL_Click;

                        C = new TableCell();
                        C.Controls.Add(btnL);
                        R.Cells.Add(C);

                        TischLE.Rows.Add(R);
                    }

                    TischLE.Rows.Add(ZusatzInfoLE());

                    btnAdd.Visible = false;
                    btnChange.Visible = false;
                    btnSave.Visible = true;
                    SprechstundenUpdate.Visible = false;

                    break;

                case "Hinzufügen":

                    R = new TableRow();

                    TextBox tbLehrerKuerzelH = new TextBox();
                    tbLehrerKuerzelH.ID = "tbLehrerKuerzel";
                    C = new TableCell();
                    C.Controls.Add(tbLehrerKuerzelH);
                    R.Cells.Add(C);

                    TextBox tbVornameH = new TextBox();
                    tbVornameH.ID = "tbVorname";
                    C = new TableCell();
                    C.Controls.Add(tbVornameH);
                    R.Cells.Add(C);

                    TextBox tbNachnameH = new TextBox();
                    tbNachnameH.ID = "tbNachname";
                    C = new TableCell();
                    C.Controls.Add(tbNachnameH);
                    R.Cells.Add(C);

                    TextBox tbSprechstundeH = new TextBox();
                    tbSprechstundeH.ID = "tbSprechstunde";
                    C = new TableCell();
                    C.Controls.Add(tbSprechstundeH);
                    R.Cells.Add(C);

                    TextBox tbTagH = new TextBox();
                    tbTagH.ID = "tbTag";
                    tbTagH.Width = 50;
                    C = new TableCell();
                    C.Controls.Add(tbTagH);
                    R.Cells.Add(C);

                    TextBox tbKlassenvorstandKlasseH = new TextBox();
                    tbKlassenvorstandKlasseH.ID = "tbKlassenvorstandKlasse";
                    C = new TableCell();
                    C.Controls.Add(tbKlassenvorstandKlasseH);
                    R.Cells.Add(C);

                    TextBox tbRaumH = new TextBox();
                    tbRaumH.ID = "tbRaum";
                    tbRaumH.Width = 75;
                    C = new TableCell();
                    C.Controls.Add(tbRaumH);
                    R.Cells.Add(C);

                    TextBox tbGebäudeH = new TextBox();
                    tbGebäudeH.ID = "tbGebäude";
                    tbGebäudeH.Width = 75;
                    C = new TableCell();
                    C.Controls.Add(tbGebäudeH);
                    R.Cells.Add(C);

                    TextBox tbAbteilungenH = new TextBox();
                    tbAbteilungenH.ID = "tbAbteilungen";
                    C = new TableCell();
                    C.Controls.Add(tbAbteilungenH);
                    R.Cells.Add(C);

                    TischLE.Rows.Add(R);

                    TischLE.Rows.Add(ZusatzInfoLE());

                    btnAdd.Visible = false;
                    btnChange.Visible = false;
                    btnSave.Visible = true;
                    SprechstundenUpdate.Visible = false;

                    break;
            }
        } 

        protected TableRow ZusatzInfoLE()
        {
            TableRow R = new TableRow();

            TableCell C = new TableCell();
            C.CssClass = "body";
            R.Cells.Add(C);

            C = new TableCell();
            C.CssClass = "body";
            R.Cells.Add(C);

            C = new TableCell();
            C.CssClass = "body";
            R.Cells.Add(C);

            C = new TableCell();
            C.Text = "&nbsp;&nbsp;0 = 07:10-08:00<br/>&nbsp;&nbsp;1 = 08:00-08:50<br/>&nbsp;&nbsp;2 = 08:50-09:40<br/>"
                   + "&nbsp;&nbsp;3 = 09:50-10:40<br/>&nbsp;&nbsp;4 = 10:40-11:30<br/>&nbsp;&nbsp;5 = 11:40-12:30<br/>"
                   + "&nbsp;&nbsp;6 = 12:30-13:20<br/>&nbsp;&nbsp;7 = 13:20-14:10<br/>&nbsp;&nbsp;8 = 14:20-15:10<br/>"
                   + "&nbsp;&nbsp;9 = 15:10-16:00<br/>10 = 16:10-17:00<br/>11 = 17:00-17:50<br/>12 = 17:50-18:40";
            C.CssClass = "body";
            R.Cells.Add(C);

            C = new TableCell();
            C.Text = "0 = Mo<br/>1 = Di<br/>2 = Mi<br/>3 = Do<br/>4 = Fr<br/>5 = Sa<br/>6 = So";
            C.CssClass = "body";
            R.Cells.Add(C);

            C = new TableCell();
            C.Text = "Mit '/' trennen!<br/><br/>z.B.:<br/>1AHEL/2BHEL";
            C.CssClass = "body";
            R.Cells.Add(C);

            C = new TableCell();
            C.CssClass = "body";
            R.Cells.Add(C);

            C = new TableCell();
            C.CssClass = "body";
            R.Cells.Add(C);

            if (LEmode == "Hinzufügen")
            {
                C = new TableCell();
                C.Text = "Nur ein Eintrag gültig!<br/><br/>" + GetAllAbtString();
                C.CssClass = "body";
                R.Cells.Add(C);
            }
            else
            {
                C = new TableCell();
                C.Text = "Mit '/' trennen!<br/><br/>" + GetAllAbtString();
                C.CssClass = "body";
                R.Cells.Add(C);
            }

            return R;
        } 

        protected string SetKlasseString(List<string> dummy)
        {
            string ret = "";
            if (dummy != null)
            {
                foreach (string Klasse in dummy)
                {
                    if (ret == "")
                    {
                        ret += Klasse;
                    }
                    else
                    {
                        ret += "/" + Klasse;
                    }
                }
            }
            return ret;
        }

        protected string SetAbtString(List<int> dummy)
        {
            string ret = "";
            if (dummy != null)
            {
                foreach (int Abt in dummy)
                {
                    if (ret == "")
                    {
                        ret += Abt;
                    }
                    else
                    {
                        ret += "/" + Abt;
                    }
                }
            }
            return ret;
        }

        protected string GetAllAbtString()
        {
            string ret = "";

            string Befehl = @"SELECT 
            [Abteilungen].[AbteilungsID] AS AbteilungsID,
            [Abteilungen].[Abteilungsname] AS Abteilungsname
       FROM [Abteilungen]";

            DataTable Daten = DatenbankAbrufen.DatenbankAbfrage(Befehl);

            for (int i = 0; i < Daten.Rows.Count; i++)
            {
                ret += Daten.Rows[i]["AbteilungsID"].ToString() + " = " + Daten.Rows[i]["AbteilungsName"].ToString() + "<br/>";
            }

            return ret;
        }

        protected string NameFormatter(string name)
        {
            if (name.Length == 0) return "";

            if (name.Length == 1) return name.ToUpper();

            // 1. Buchstabe GROSS
            name = name.Substring(0, 1).ToUpper() + name.Substring(1).ToLower();

            // Doppelnamen mit '-'
            int index = 0;
            while ((index = name.IndexOf('-', index)) != -1)
            {
                index++;
                name = name.Substring(0, index) + name.Substring(index, 1).ToUpper() + name.Substring(index + 1);
            }

            // Doppelnamen mit ' '
            index = 0;
            while ((index = name.IndexOf(' ', index)) != -1)
            {
                index++;
                name = name.Substring(0, index) + name.Substring(index, 1).ToUpper() + name.Substring(index + 1);
            }

            return name;
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            LEmode = "Hinzufügen";
            LELaden();
        }

        protected void btnChange_Click(object sender, EventArgs e)
        {
            LEmode = "Bearbeiten";
            LELaden();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Regex LKAlt = new Regex("^[A-Z]{4}$");
                Regex LKNeu = new Regex("^[0-9]{8}$");
                Regex Name = new Regex(@"^[- \w]+$");
                Regex NameNot = new Regex(@"[_\d]");
                Regex Klasse = new Regex("^[0-9][A-Z]{1,10}$");

                switch (LEmode)
                {
                    case "Hinzufügen":

                        int errorH = 0;
                        Structuren.LE EintragH = new Structuren.LE();
                        EintragH.KlassenvorstandKlasse = null;

                        foreach (TableRow TR in TischLE.Rows)
                        {
                            foreach (TableCell TC in TR.Cells)
                            {
                                foreach (TextBox TB in TC.Controls.OfType<TextBox>())
                                {
                                    switch (TB.ID)
                                    {
                                        case "tbLehrerKuerzel":

                                            if ((!LKAlt.IsMatch(TB.Text.ToUpper())) && (!LKNeu.IsMatch(TB.Text)))
                                            {
                                                TB.BackColor = System.Drawing.Color.Red;
                                                errorH++;
                                            }
                                            else
                                            {
                                                TB.BackColor = System.Drawing.Color.White;
                                                EintragH.LehrerKuerzel = TB.Text.ToUpper();

                                                if(EintragH.LehrerKuerzel != TB.Text)
                                                {
                                                    TB.Text = EintragH.LehrerKuerzel;
                                                }
                                            }

                                            break;

                                        case "tbVorname":

                                            if (!Name.IsMatch(TB.Text) || NameNot.IsMatch(TB.Text))
                                            {
                                                TB.BackColor = System.Drawing.Color.Red;
                                                errorH++;
                                            }
                                            else
                                            {
                                                TB.BackColor = System.Drawing.Color.White;
                                                EintragH.Vorname = NameFormatter(TB.Text);

                                                if (EintragH.Vorname != TB.Text)
                                                {
                                                    TB.Text = EintragH.Vorname;
                                                }
                                            }

                                            break;

                                        case "tbNachname":

                                            if (!Name.IsMatch(TB.Text) || NameNot.IsMatch(TB.Text))
                                            {
                                                TB.BackColor = System.Drawing.Color.Red;
                                                errorH++;
                                            }
                                            else
                                            {
                                                TB.BackColor = System.Drawing.Color.White;
                                                EintragH.Nachname = NameFormatter(TB.Text);

                                                if (EintragH.Nachname != TB.Text)
                                                {
                                                    TB.Text = EintragH.Nachname;
                                                }
                                            }

                                            break;

                                        case "tbSprechstunde":

                                            if ((TB.Text == "") || (TB.Text.ToInt32() < 0) || (TB.Text.ToInt32() > 12))
                                            {
                                                TB.BackColor = System.Drawing.Color.Red;
                                                errorH++;
                                            }
                                            else
                                            {
                                                TB.BackColor = System.Drawing.Color.White;
                                                EintragH.Sprechstunde = TB.Text.ToInt32();
                                            }

                                            break;

                                        case "tbTag":

                                            if ((TB.Text == "") || (TB.Text.ToInt32() < 0) || (TB.Text.ToInt32() > 6))
                                            {
                                                TB.BackColor = System.Drawing.Color.Red;
                                                errorH++;
                                            }
                                            else
                                            {
                                                TB.BackColor = System.Drawing.Color.White;
                                                EintragH.Tag = TB.Text.ToInt32();
                                            }

                                            break;

                                        case "tbRaum":

                                            if ((TB.Text == "") || (TB.Text.ToInt32() < 0))
                                            {
                                                TB.BackColor = System.Drawing.Color.Red;
                                                errorH++;
                                            }
                                            else
                                            {
                                                TB.BackColor = System.Drawing.Color.White;
                                                EintragH.Raum = TB.Text.ToInt32();
                                            }

                                            break;

                                        case "tbGebäude":

                                            if ((TB.Text == "") || (TB.Text.ToInt32() < 0))
                                            {
                                                TB.BackColor = System.Drawing.Color.Red;
                                                errorH++;
                                            }
                                            else
                                            {
                                                TB.BackColor = System.Drawing.Color.White;
                                                EintragH.Gebäude = TB.Text;
                                            }

                                            break;

                                        case "tbKlassenvorstandKlasse":

                                            bool temp = true;
                                            if (TB.Text == "")
                                            {
                                                TB.BackColor = System.Drawing.Color.White;
                                                if (EintragH.KlassenvorstandKlasse == null)
                                                {
                                                    EintragH.KlassenvorstandKlasse = new List<string>();
                                                }
                                                EintragH.KlassenvorstandKlasse.Add(TB.Text);
                                                break;
                                            }
                                            foreach (string dummyH in TB.Text.Split('/'))
                                            {
                                                if (!Klasse.IsMatch(dummyH.ToUpper()))
                                                {
                                                    TB.BackColor = System.Drawing.Color.Red;
                                                    errorH++;
                                                    temp = false;
                                                    break;
                                                }
                                            }
                                            if (temp)
                                            {
                                                foreach (string dummyH in TB.Text.Split('/'))
                                                {
                                                    TB.BackColor = System.Drawing.Color.White;
                                                    if (EintragH.KlassenvorstandKlasse == null)
                                                    {
                                                        EintragH.KlassenvorstandKlasse = new List<string>();
                                                    }
                                                    EintragH.KlassenvorstandKlasse.Add(dummyH.ToUpper());
                                                }
                                            }

                                            break;

                                        case "tbAbteilungen": //Nur eine Eingabe erlaubt

                                            if ((TB.Text == "") || (TB.Text.ToInt32() < 0))
                                            {
                                                TB.BackColor = System.Drawing.Color.Red;
                                                errorH++;
                                            }
                                            else
                                            {
                                                TB.BackColor = System.Drawing.Color.White;
                                                EintragH.AbteilungsIDs = new List<int>();
                                                EintragH.AbteilungsIDs.Add(TB.Text.ToInt32());
                                            }

                                            break;
                                    }
                                }
                            }
                        }

                        if (errorH != 0)
                        {
                            lbError.Text = "Es wurden " + errorH + " Fehler gefunden!";
                            lbError.Visible = true;
                        }
                        else
                        {
                            int mode = DatenbankAbrufen.LehrerKuerzelExists(EintragH.LehrerKuerzel, EintragH.AbteilungsIDs.First<int>());
                            switch (mode)
                            {
                                case 0:

                                    DatenbankSchreiben.LehrerEigenschaftHinzufuegen(EintragH, mode);
                                    LEModusReset();
                                    LELaden();
                                    break;

                                case 1:

                                    lbError.Text = "Der Lehrer " + EintragH.LehrerKuerzel + " ist bereits vorhanden und wurde der angegebenen Abteilung hinzugefügt!</br>Prüfen Sie bitte, ob alle Informationen korrekt sind!";
                                    lbError.Visible = true;
                                    DatenbankSchreiben.LehrerEigenschaftHinzufuegen(EintragH, mode);
                                    LEmode = "Bearbeiten";
                                    LELaden();
                                    break;

                                case 2:

                                    lbError.Text = "Der Lehrer "+ EintragH.LehrerKuerzel + " ist in der angegeben Abteilung bereits vorhanden!";
                                    lbError.Visible = true;
                                    break;
                            }
                        }

                        break;

                    case "Bearbeiten":

                        int errorB = 0;
                        int zeile = 0;
                        Structuren.LE[] EintragB = new Structuren.LE[TischLE.Rows.Count.ToInt32() - 2];

                        //KlassenvorstandsKlasse Liste  Initialisieren (null) => keine Klasse als KV
                        for (int i = 0; i < TischLE.Rows.Count.ToInt32() - 2; i++)
                        {
                            EintragB[i].KlassenvorstandKlasse = null;
                        }
                        //AbteilungsIDs Liste  Initialisieren (null) => keiner Abteilung zugeteilt (nicht möglich)
                        for (int i = 0; i < TischLE.Rows.Count.ToInt32() - 2; i++)
                        {
                            EintragB[i].AbteilungsIDs = null;
                        }

                        foreach (TableRow TR in TischLE.Rows)
                        {
                            foreach (TableCell TC in TR.Cells)
                            {
                                foreach (TextBox TB in TC.Controls.OfType<TextBox>())
                                {
                                    switch (TB.ID.Split('-')[1])
                                    {
                                        case "tbVorname":

                                            if (!Name.IsMatch(TB.Text) || NameNot.IsMatch(TB.Text))
                                            {
                                                TB.BackColor = System.Drawing.Color.Red;
                                                errorB++;
                                            }
                                            else
                                            {
                                                TB.BackColor = System.Drawing.Color.White;
                                                EintragB[zeile].Vorname = NameFormatter(TB.Text);

                                                if (EintragB[zeile].Vorname != TB.Text)
                                                {
                                                    TB.Text = EintragB[zeile].Vorname;
                                                }
                                            }

                                            break;

                                        case "tbNachname":

                                            if (!Name.IsMatch(TB.Text) || NameNot.IsMatch(TB.Text))
                                            {
                                                TB.BackColor = System.Drawing.Color.Red;
                                                errorB++;
                                            }
                                            else
                                            {
                                                TB.BackColor = System.Drawing.Color.White;
                                                EintragB[zeile].Nachname = NameFormatter(TB.Text);

                                                if (EintragB[zeile].Nachname != TB.Text)
                                                {
                                                    TB.Text = EintragB[zeile].Nachname;
                                                }
                                            }

                                            break;

                                        case "tbSprechstunde":

                                            if ((TB.Text == "") || (TB.Text.ToInt32() < 0) || (TB.Text.ToInt32() > 12))
                                            {
                                                TB.BackColor = System.Drawing.Color.Red;
                                                errorB++;
                                            }
                                            else
                                            {
                                                TB.BackColor = System.Drawing.Color.White;
                                                EintragB[zeile].Sprechstunde = TB.Text.ToInt32();
                                            }

                                            break;

                                        case "tbTag":

                                            if ((TB.Text == "") || (TB.Text.ToInt32() < 0) || (TB.Text.ToInt32() > 6))
                                            {
                                                TB.BackColor = System.Drawing.Color.Red;
                                                errorB++;
                                            }
                                            else
                                            {
                                                TB.BackColor = System.Drawing.Color.White;
                                                EintragB[zeile].Tag = TB.Text.ToInt32();
                                            }

                                            break;

                                        case "tbRaum":

                                            if ((TB.Text == "") || (TB.Text.ToInt32() < 0))
                                            {
                                                TB.BackColor = System.Drawing.Color.Red;
                                                errorB++;
                                            }
                                            else
                                            {
                                                TB.BackColor = System.Drawing.Color.White;
                                                EintragB[zeile].Raum = TB.Text.ToInt32();
                                            }

                                            break;

                                        case "tbGebäude":

                                            if ((TB.Text == "") || (TB.Text.ToInt32() < 0))
                                            {
                                                TB.BackColor = System.Drawing.Color.Red;
                                                errorB++;
                                            }
                                            else
                                            {
                                                TB.BackColor = System.Drawing.Color.White;
                                                EintragB[zeile].Gebäude = TB.Text;
                                            }

                                            break;

                                        case "tbKlassenvorstandKlasse":

                                            bool temp = true;
                                            if (TB.Text == "")
                                            {
                                                TB.BackColor = System.Drawing.Color.White;
                                                if (EintragB[zeile].KlassenvorstandKlasse == null)
                                                {
                                                    EintragB[zeile].KlassenvorstandKlasse = new List<string>();
                                                }
                                                EintragB[zeile].KlassenvorstandKlasse.Add(TB.Text);
                                                break;
                                            }
                                            foreach (string dummyB in TB.Text.Split('/'))
                                            {
                                                if (!Klasse.IsMatch(dummyB.ToUpper()))
                                                {
                                                    TB.BackColor = System.Drawing.Color.Red;
                                                    errorB++;
                                                    temp = false;
                                                    break;
                                                }
                                            }
                                            if (temp)
                                            {
                                                foreach (string dummyB in TB.Text.Split('/'))
                                                {
                                                    TB.BackColor = System.Drawing.Color.White;
                                                    if (EintragB[zeile].KlassenvorstandKlasse == null)
                                                    {
                                                        EintragB[zeile].KlassenvorstandKlasse = new List<string>();
                                                    }
                                                    EintragB[zeile].KlassenvorstandKlasse.Add(dummyB.ToUpper());
                                                }
                                            }

                                            break;

                                        case "tbAbteilungen":

                                            bool dummy = false;
                                            try
                                            {
                                                foreach (string Abt in TB.Text.Split('/'))
                                                {
                                                    if (Abt.ToInt32() < 0)
                                                        dummy = true;
                                                }
                                            }
                                            catch
                                            { dummy = true; }

                                            if ((TB.Text == "") || dummy)
                                            {
                                                TB.BackColor = System.Drawing.Color.Red;
                                                errorB++;
                                            }
                                            else
                                            {
                                                TB.BackColor = System.Drawing.Color.White;
                                                foreach (string Abt in TB.Text.Split('/'))
                                                {
                                                    if (EintragB[zeile].AbteilungsIDs == null)
                                                    {
                                                        EintragB[zeile].AbteilungsIDs = new List<int>();
                                                    }
                                                    EintragB[zeile].AbteilungsIDs.Add(Abt.ToInt32());
                                                }
                                            }
                                            EintragB[zeile].LehrerKuerzel = TB.ID.Split('-')[0];
                                            zeile++;

                                            break;
                                    }
                                }
                            }
                        }

                        if (errorB != 0)
                        {
                            lbError.Text = "Es wurden " + errorB + " Fehler gefunden!";
                            lbError.Visible = true;
                        }
                        else
                        {
                            DatenbankSchreiben.LehrerEigenschaftAendern(EintragB);

                            LEModusReset();
                            LELaden();
                        }

                        break;
                }
            }
            catch
            {
                lbError.Text = "FEHLER: Ein unbekannter Fehler ist aufgetreten!";
                lbError.Visible = true;
            }
        }

        protected void btnL_Click(object sender, EventArgs e)
        {
            Button dummy = (Button)sender;
            string[] dummy2 = dummy.ID.Split('-');

            DatenbankSchreiben.LehrerEigenschaftLöschen(dummy2[0]);

            LEModusReset();
            LELaden();
        }

        private void LEModusReset()
        {
            lbError.Visible = false;
            LEmode = "Anzeigen";
        }

        protected void SprechstundenUpdate_Click(object sender, EventArgs e)
        {
            string line;
            int index = 0;
            string zwischenspeicher;

            classes.Structuren.LEmitSprechstundenDaten lehrerdaten;

            string[] alle_daten; //Alle daten einer Zeile
            string[] delimitter = { "\",\"" };
            try
            {
                lbError.Text = "";
                lbError.Visible = false;
                WebClient webClient = new WebClient();
                webClient.DownloadFile("http://services.htl.moedling.at/export/sprechstunden.txt", @"C:\infoscreen\verwaltung_code\Dateien\Sprechstunden\LE");
            }
            catch
            {
                lbError.Text = "FEHLER BEIM DOWNLOAD DER DATEI!";
                lbError.Visible = true;
                return;
            }
            System.IO.StreamReader file =
            new System.IO.StreamReader(@"C:\infoscreen\verwaltung_code\Dateien\Sprechstunden\LE");

            DatenbankSchreiben.LehrerTabelleLoeschen();

            string text = file.ReadToEnd().Replace("\r", "");
            string[] seperator = { "\n" };
            string[] zeilen = text.Split(seperator, StringSplitOptions.RemoveEmptyEntries);

            int i1, i2;

            for (i1 = 0; i1 < zeilen.Length; i1++)
            {
                line = zeilen[i1];

                lehrerdaten.KlassenvorstandKlassen = new List<string>(); //Liste initialisieren

                // " und alles davor Löschen
                index = line.IndexOf('"'); //Erstes " suchen
                line = line.Remove(0, index + 1);  // " und alles davor Löschen

                // ", am Schluss löschen
                line = line.Remove(line.Length-2);

                
                alle_daten = line.Split(delimitter,StringSplitOptions.None); //nach "," splitten

                for(i2 = 0; i2 < alle_daten.Length; i2++) 
                {                                    
                    if (alle_daten[i2].Contains(';')) // wo ein ; vorkommt
                    {
                        alle_daten[i2] = alle_daten[i2].Remove(alle_daten[i2].Length - 1); //letztes ; löschen
                    }                    
                }

                lehrerdaten.LehrerKuerzel = alle_daten[0];
                lehrerdaten.Vorname = alle_daten[1];
                lehrerdaten.Nachname = alle_daten[2];

                zwischenspeicher = alle_daten[3];
                if (zwischenspeicher.Contains(';'))
                {
                    lehrerdaten.KlassenvorstandKlassen.AddRange(zwischenspeicher.Split(';'));
                }
                else
                {
                    lehrerdaten.KlassenvorstandKlassen.Add(zwischenspeicher);
                }

                zwischenspeicher = alle_daten[5];
               
                if(zwischenspeicher.Contains(':')) //wenn raum und nummer vorhanden
                {
                    if (zwischenspeicher.Contains("Vereinbarung"))
                    {
                        lehrerdaten.Raum = "";
                        lehrerdaten.Gebäude = "";
                    }
                    else
                    {
                        lehrerdaten.Gebäude = zwischenspeicher.Remove(2);
                        lehrerdaten.Raum = zwischenspeicher.Remove(0, 2).Remove(4);
                    }

                    index = zwischenspeicher.IndexOf(':');
                    lehrerdaten.Durchwahl = zwischenspeicher.Remove(0, index + 1);
                }
                else
                {
                    lehrerdaten.Gebäude = "";
                    lehrerdaten.Raum = "";
                    lehrerdaten.Durchwahl = zwischenspeicher;
                }
                lehrerdaten.Sprechstunde = alle_daten[6];
                lehrerdaten.Tag = (alle_daten[7].ToInt32() - 1).ToString();

                lehrerdaten.Abteilungen = alle_daten[10];

                try
                {
                    DatenbankSchreiben.Sprechstunden_Aktualisieren(lehrerdaten);
                }
                catch
                {
                }
            }
            file.Close();
            lbError.Text = "Lehrereigenschaften erfolgreich aktualisiert";
            lbError.ForeColor = System.Drawing.Color.Green;
            lbError.Visible = true;
            LELaden();
        }
    }
}