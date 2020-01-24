using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Infoscreen_Verwaltung.classes;
using System.Drawing;
using System.Web.Services;
using System.Web.Script.Services;
using System.DirectoryServices;
using System.Web.UI.HtmlControls;
using System.DirectoryServices.AccountManagement;

namespace Infoscreen_Verwaltung.admin.klassen
{
    public partial class _default : System.Web.UI.Page
    {
        int count = 0;
        List<TextBox> listeTextBox_KV = new List<TextBox>();
        static List<Label> listeLabel_Klasse = new List<Label>();
        static List<TextBox> listeTextBox_Klassensprecher_ID = new List<TextBox>();
        static List<TextBox> listeTextBox_Klassensprecher_Name = new List<TextBox>();
        string ebene = "../../";

        struct KlassenBoxen
        {
            public Label AbteilungsName;
            public Label Klasse;
            public TextBox Klassensprecher_ID;
            public TextBox Klassensprecher_Name;
            public TextBox Klassenvorstand;
            public TextBox Klasseninfo;
            public Label Raum;
        }

        static KlassenBoxen[] dummy;

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<string> GetKlassensprecherID(string pre)
        {
            List<string> KlassensprecherID = new List<string>();

            DirectoryEntry entry = new DirectoryEntry("LDAP://" + Properties.Resources.standardDomain, Properties.Resources.domainUser, Properties.Resources.domainPassword, AuthenticationTypes.Secure);
            entry.Username = Properties.Resources.domainUser;
            entry.Password = Properties.Resources.domainPassword;


            DirectorySearcher search = new DirectorySearcher(entry);
            search.Filter = "(&(objectClass=user)(displayName=*"+ pre +"*))";
            search.PropertiesToLoad.Add("cn");

            string temp1;

            try
            {
                SearchResultCollection resultall = search.FindAll();

                foreach(SearchResult temp in resultall)
                {
                    KlassensprecherID.Add((string)temp.Properties["cn"][0]);
                }

            }
            catch (Exception ex)
            {
                //throw new Exception("Error obtaining id. " + ex.Message);
                temp1 = "Unbekannt";
                KlassensprecherID.Add(temp1);
            }

            return KlassensprecherID;
        }

        public static string GetKlassensprecherName(string pre)
        {
            string KlassensprecherID;

            if(pre != "")
            {
                DirectoryEntry entry = new DirectoryEntry("LDAP://" + Properties.Resources.standardDomain, Properties.Resources.domainUser, Properties.Resources.domainPassword, AuthenticationTypes.Secure);
                entry.Username = Properties.Resources.domainUser;
                entry.Password = Properties.Resources.domainPassword;


                DirectorySearcher search = new DirectorySearcher(entry);
                search.Filter = "(&(objectClass=user)(cn=" + pre + "))";
                search.PropertiesToLoad.Add("displayName");

                string temp1;

                try
                {
                    SearchResult result = search.FindOne();
                    KlassensprecherID = (string)result.Properties["displayName"][0];
                }
                catch (Exception ex)
                {
                    //throw new Exception("Error obtaining id. " + ex.Message);
                    temp1 = "Unbekannt";
                    KlassensprecherID = (temp1);
                }
            }
            else
            {
                KlassensprecherID = "";
            }
           
            return KlassensprecherID;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!classes.Login.Angemeldet) Response.Redirect("../../login.aspx");
            if (!classes.Login.Rechte.Klassen.OneBiggerThan(1)) Response.Redirect("../../");

            DrawPicker();
            picker_container.Style.Value = "display:none";

            TopMenu.Text = classes.Anzeige.TopBar(ebene);
            Menu.Text = classes.Anzeige.Menue(ebene, "admin");

            if (!IsPostBack)
            {
                string[] abteilungen;
                if (classes.Login.Rechte.Klassen[2].ToInt32() > 1)
                    abteilungen = classes.DatenbankAbrufen.AbteilungenAbrufen();
                else if (classes.Login.Rechte.Klassen[1].ToInt32() > 1)
                    abteilungen = classes.Login.Abteilungen;
                else abteilungen = new string[] { classes.Login.StammAbteilung };

                foreach (string abteilung in abteilungen)
                    dropDownAbteilung.Items.Add(abteilung);

                try { dropDownAbteilung.SelectedValue = classes.Login.StammAbteilung; }
                catch { dropDownAbteilung.SelectedIndex = -1; }

                if (Session["22_Abteilung"] != null)
                {
                    try { dropDownAbteilung.SelectedValue = Session["22_Abteilung"].ToString(); }
                    catch { }
                }

                Session["22_Abteilung"] = null;
            }

            TabelleZeichnen();

            if (classes.Login.Rechte.Klassen[2].ToInt32() > 2)
                btNeueKlasse.Enabled = true;
            else if (classes.Login.Rechte.Klassen[1].ToInt32() > 2 && classes.Login.StammAbteilung == dropDownAbteilung.SelectedValue)
                btNeueKlasse.Enabled = true;
            else
                btNeueKlasse.Enabled = false;

            //DEBUG!!!
            //btNeueKlasse.Enabled = false;

            if (classes.Login.Rechte.Klassen[2].ToInt32() > 3)
                btUebernehmen.Enabled = true;
            else if (classes.Login.Rechte.Klassen[1].ToInt32() > 3 && classes.Login.StammAbteilung == dropDownAbteilung.SelectedValue)
                btUebernehmen.Enabled = true;
            else
                btUebernehmen.Enabled = false;

            if (classes.Login.Rechte.Klassen[2].ToInt32() > 3)
                btNeuesSchuljahr.Enabled = true;
            else if (classes.Login.Rechte.Klassen[1].ToInt32() > 3 && classes.Login.StammAbteilung == dropDownAbteilung.SelectedValue)
                btNeuesSchuljahr.Enabled = true;
            else
                btNeuesSchuljahr.Enabled = false;

            if (classes.Login.Rechte.Klassen[2].ToInt32() > 3)
                btAlleStundenplaeneLeeren.Enabled = true;
            else if (classes.Login.Rechte.Klassen[1].ToInt32() > 3 && classes.Login.StammAbteilung == dropDownAbteilung.SelectedValue)
                btAlleStundenplaeneLeeren.Enabled = true;
            else
                btAlleStundenplaeneLeeren.Enabled = false;
        }

        protected void HinweisZeigen(bool _zeigen = true)
        {
            if (_zeigen)
            {
                lbHinweis1.Text = lbHinweis2.Text = "Hinweis: Zum durchführen der gewünschten Aktion bitte den betreffenden Button erneut anklicken.";
                lbHinweis1.ForeColor = lbHinweis2.ForeColor = Color.Red;
                lbHinweis1.Visible = lbHinweis2.Visible = true;
            }
            else
            {
                lbHinweis1.Visible = lbHinweis2.Visible = false;
            }
        }

        protected void dropDownAbteilung_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["22_Abteilung"] = dropDownAbteilung.SelectedValue;
            Response.Redirect("./");
        }

        protected void TabelleZeichnen()
        {
            tbKlassen.Rows.Clear();
            TableRow row = new TableRow();
            TableCell cell = new TableCell();
            cell.CssClass = "head";
            cell.Text = "Klasse";
            row.Cells.Add(cell);

            cell = new TableCell();
            cell.CssClass = "head";
            cell.Text = "Geb.";
            row.Cells.Add(cell);

            cell = new TableCell();
            cell.CssClass = "head";
            cell.Text = "Raum";
            row.Cells.Add(cell);

            cell = new TableCell();
            cell.CssClass = "head";
            cell.Text = "Klassensprecher ID";
            row.Cells.Add(cell);

            cell = new TableCell();
            cell.CssClass = "head";
            cell.Text = "Klassensprecher";
            row.Cells.Add(cell);

            cell = new TableCell();
            cell.CssClass = "head";
            cell.Text = "Klassenvorstand";
            row.Cells.Add(cell);

            cell = new TableCell();
            cell.CssClass = "head";
            cell.Width = 10;
            row.Cells.Add(cell);

            cell = new TableCell();
            cell.CssClass = "head";
            cell.Width = 10;
            row.Cells.Add(cell);

            tbKlassen.Rows.Add(row);

            string[] klassen = DatenbankAbrufen.KlassenAbrufen(dropDownAbteilung.SelectedValue);
            Structuren.Klasseneigenschaften daten = new Structuren.Klasseneigenschaften();
            
            dummy = new KlassenBoxen[klassen.Length];
            dataButton button;
           
            
            foreach (string klasse in klassen)
            {
                row = new TableRow();

                daten = DatenbankAbrufen.KlasseneigenschaftenAbrufen(klasse);

                cell = new TableCell();
                dummy[count].Klasse = new Label();
                listeLabel_Klasse.Add(dummy[count].Klasse);
                dummy[count].Klasse.Text = daten.klasse;
                dummy[count].Klasse.CssClass = "autocompFach";
                cell.Controls.Add(dummy[count].Klasse);
                row.Cells.Add(cell);

                cell = new TableCell();
                cell.Text = daten.gebäude;
                row.Cells.Add(cell);

                cell = new TableCell();
                dummy[count].Raum = new Label();
               
                if (daten.raum != "")
                {
                    dummy[count].Raum.Text = daten.raum;
                }
                else
                {
                    dummy[count].Raum.Text = "Ohne Raum";
                    dummy[count].Raum.ForeColor = Color.Red;
                }

                cell.Controls.Add(dummy[count].Raum);
                row.Cells.Add(cell);

                cell = new TableCell();
                dummy[count].Klassensprecher_ID = new TextBox();
                listeTextBox_Klassensprecher_ID.Add(dummy[count].Klassensprecher_ID);
                dummy[count].Klassensprecher_ID.Text = daten.klassensprecher;
                dummy[count].Klassensprecher_ID.CssClass = "autocompKlassensprecherID";
                dummy[count].Klasseninfo = new TextBox();
                dummy[count].Klasseninfo.Visible = false;
                dummy[count].Klasseninfo.Text = daten.klasseninfo;
                cell.Controls.Add(dummy[count].Klassensprecher_ID);
                row.Cells.Add(cell);

                cell = new TableCell();
                dummy[count].Klassensprecher_Name = new TextBox();
                dummy[count].Klassensprecher_Name.Enabled = false;
                dummy[count].Klassensprecher_Name.Text = GetKlassensprecherName(daten.klassensprecher);
                listeTextBox_Klassensprecher_Name.Add(dummy[count].Klassensprecher_Name);
                cell.Controls.Add(dummy[count].Klassensprecher_Name);
                row.Cells.Add(cell);

                cell = new TableCell();
                dummy[count].Klassenvorstand = new TextBox();
                listeTextBox_KV.Add(dummy[count].Klassenvorstand);
                dummy[count].Klassenvorstand.Text = daten.klassenvorstand;
                cell.Controls.Add(dummy[count].Klassenvorstand);
                row.Cells.Add(cell);

                cell = new TableCell();
                button = new dataButton();
                button.Text = "Stundenplan leeren";
                button.Click += btStundenplanLeerenClick;
                button.CssClass = "ActionButton";
                button.Setting = daten;
                if (classes.Login.Rechte.Stundenplan[2].ToInt32() > 2)
                    button.Enabled = true;
                else if (classes.Login.Rechte.Stundenplan[1].ToInt32() > 2 && classes.Login.StammAbteilung == dropDownAbteilung.SelectedValue)
                    button.Enabled = true;
                else if (classes.Login.Rechte.Stundenplan[0].ToInt32() > 2 && classes.Login.StammKlasse == daten.klasse)
                    button.Enabled = true;
                else
                    button.Enabled = false;
                cell.Controls.Add(button);
                row.Cells.Add(cell);

                cell = new TableCell();
                button = new dataButton();
                button.Text = "Löschen";
                button.CssClass = "DeleteButton";
                button.Click += btLöschenClick;
                button.Setting = daten;
                if (classes.Login.Rechte.Klassen[2].ToInt32() > 3)
                    button.Enabled = true;
                else if (classes.Login.Rechte.Klassen[1].ToInt32() > 3 && classes.Login.StammAbteilung == dropDownAbteilung.SelectedValue)
                    button.Enabled = true;
                else if (classes.Login.Rechte.Klassen[0].ToInt32() > 3 && classes.Login.StammKlasse == daten.klasse)
                    button.Enabled = true;
                else
                    button.Enabled = false;
                cell.Controls.Add(button);
                row.Cells.Add(cell);

                tbKlassen.Rows.Add(row);
                count++;
            }
            
        }

        protected void btNeueKlasse_Click(object sender, EventArgs e)
        {
            // InfoScreen v6: Replaced by picker
            //Session["22_Abteilung"] = dropDownAbteilung.SelectedValue;
            //Session["22_Klasse_Art"] = 0;
            //Response.Redirect("./add.aspx");

            picker_container.Style.Value = "display:block";

        }
        
        protected void btUebernehmen_Click(object sender, EventArgs e)
        {
            DatenbankSchreiben.KlasseneigenschaftenLoeschen(DatenbankAbrufen.GetAbteilungsIdVonAbteilungsname(dropDownAbteilung.SelectedValue));

            for (int i = 0; i < dummy.Length; i++)
            {   
                using (Entities db = new Entities())
                {
                    //Klassen eintrag = new classes.Klassen();
                    //eintrag.AbteilungsID = DatenbankAbrufen.GetAbteilungsIdVonAbteilungsname(dropDownAbteilung.SelectedValue);
                    //eintrag.Klasse = dummy[i].Klasse.Text;
                    //eintrag.Klassensprecher = dummy[i].Klassensprecher_ID.Text;
                    //eintrag.Klassenvorstand = dummy[i].Klassenvorstand.Text;
                    //eintrag.Klasseninfo = dummy[i].Klasseninfo.Text;
                    //db.Klassen.Add(eintrag);
                    //db.SaveChanges();
                    DatenbankSchreiben.AddKlasseToKlassen(DatenbankAbrufen.GetAbteilungsIdVonAbteilungsname(dropDownAbteilung.SelectedValue),
                        dummy[i].Klasse.Text, dummy[i].Klassensprecher_ID.Text, 
                        GetKlassensprecherName(dummy[i].Klassensprecher_ID.Text),  dummy[i].Klasseninfo.Text, dummy[i].Klassenvorstand.Text);

                }
            }
            //for (int i = 0; i < dummy.Length; i++)
            //{
            //    DatenbankSchreiben.RaeumeUpdaten(DatenbankAbrufen.GetAbteilungsIdVonAbteilungsname(dropDownAbteilung.SelectedValue), dummy[i].Klasse.Text, Convert.ToInt32(dummy[i].Raum.Text));
            //}
            // InfoScreen v6: Not necessary anymore since room numbers are set at raeume/default.aspx

            Response.Redirect("./");
        }

        private void btStundenplanLeerenClick(object sender, EventArgs e)
        {
            dataButton bt = (dataButton)sender;

            if (!bt.Enabled) Response.Redirect("./");

            Structuren.Klasseneigenschaften daten = ((Structuren.Klasseneigenschaften)bt.Setting);
            if (bt.BorderColor != Color.Red)
            {
                bt.BorderColor = Color.Red;
                HinweisZeigen();
            }
            else
            {
                DatenbankSchreiben.StundenplanLeeren(daten.klasse);
                Response.Redirect("./");
            }
        }

        private void btLöschenClick(object sender, EventArgs e)
        {
            dataButton bt = (dataButton)sender;

            if (!bt.Enabled) Response.Redirect("./");

            Structuren.Klasseneigenschaften daten = ((Structuren.Klasseneigenschaften)bt.Setting);
            if (bt.BorderColor != Color.Red)
            {
                bt.BorderColor = Color.Red;
                HinweisZeigen();
            }
            else
            {
                DatenbankSchreiben.KlasseLöschen(daten.klasse);
                Response.Redirect("./");
            }
        }

        public void btNeuesSchuljahr_Click(object sender, EventArgs e)
        {
            if (!btNeuesSchuljahr.Enabled) Response.Redirect("./");
            if (btNeuesSchuljahr.BorderColor != Color.Red)
            {
                btNeuesSchuljahr.BorderColor = Color.Red;
                HinweisZeigen();
            }
            else
            {
                string klasse, alt, neu;
                int zahl;
                count--;

                for (int i = count; i >= 0; i--)
                {
                    listeTextBox_Klassensprecher_ID[i].Text = null; //Klassensprecher löschen

                    klasse = listeLabel_Klasse[i].Text; //Klasse zwischenspeichern

                    if (klasse.Remove(0, 2) == "KELI")
                    {
                        //Klasse um 2 erhöhen
                        zahl = Convert.ToInt32(klasse.Remove(1, klasse.Length - 1)) + 2;
                        alt = Convert.ToString(zahl - 2);
                        neu = Convert.ToString(zahl);
                    }
                    else
                    {
                        //Klasse um 1 erhöhen
                        zahl = Convert.ToInt32(klasse.Remove(1, klasse.Length - 1)) + 1;
                        alt = Convert.ToString(zahl - 1);
                        neu = Convert.ToString(zahl);
                    }

                    klasse = klasse.Replace(alt, neu);

                    for (int j = count; j >= 0; j--)
                    {
                        if (String.Compare(klasse, listeLabel_Klasse[j].Text) == 0)
                        {
                            listeTextBox_KV[j].Text = listeTextBox_KV[i].Text;
                            break;
                        }
                    }

                    //KV der 1.Klassen löschen
                    if (String.Compare("1", listeLabel_Klasse[i].Text.Remove(1, listeLabel_Klasse[i].Text.Length - 1)) == 0)
                    {
                        listeTextBox_KV[i].Text = "";
                    }
                }
                lbHinweis1.Visible = false;
                lbHinweis2.Visible = false;
              
                btNeuesSchuljahr.BorderColor = btUebernehmen.BorderColor;
            }
        }

        protected void btAlleStundenplaeneLeeren_Click(object sender, EventArgs e)
        {
            if(!btAlleStundenplaeneLeeren.Enabled) Response.Redirect("./");
            if (btAlleStundenplaeneLeeren.BorderColor != Color.Red)
            {
                btAlleStundenplaeneLeeren.BorderColor = Color.Red;
                HinweisZeigen();
            }
            else
            {
                //AlleStundenplaeneLeeren
                DatenbankSchreiben.AlleStundenplaeneLeeren();
                Response.Redirect("./");
            }
        }

        private void AddKlasse(object o, EventArgs e)
        {
            picker_container.Style.Value = "display:block;";

            dataButton db = (dataButton)o;
            string klasse = (string)db.Setting;
            if (klasse.Length <= 0 || klasse.Length > 45)
            {
                ((Label)db.Parent.Controls[db.Parent.Controls.Count - 1]).Text = "Der Klassenname muss zwischen 1 und 45 Zeichen enthalten.";
                return;
            }

            if (DatenbankAbrufen.ColumnLike("Klassen","Klasse",klasse).Count != 0)
            {
                ((Label)db.Parent.Controls[db.Parent.Controls.Count - 1]).Text = "Die Klasse ist bereits vorhanden!";
                return;
            }

            DatenbankSchreiben.AddKlasseToKlassen(dropDownAbteilung.SelectedIndex + 1, klasse);
            Response.Redirect("/admin/klassen");

        }

        private void DrawPicker()
        {
            Panel picker = new Panel();
            Panel cover = new Panel();
            Label header = new Label { Text = "Neue Klasse hinzufügen" };
            Label error = new Label();
            HtmlGenericControl divInput = new HtmlGenericControl("div");
            HtmlGenericControl divClose = new HtmlGenericControl("div");
            HtmlGenericControl divForm = new HtmlGenericControl("div");
            dataButton db = new dataButton { Text = "Hinzufügen" };
            TextBox tb = new TextBox();
            Button btClose = new Button();
            Label lbtb = new Label { Text = "Klassenname:" };

            btClose.Text = "✕";
            btClose.CssClass = "DeleteButton";
            btClose.Click += (x, y) => { Response.Redirect("/admin/klassen"); };

            db.Click += (o,e) => { db.Setting = tb.Text; AddKlasse(o,e); };
            db.Style.Value = "margin: 10px";
            db.CssClass = "SaveButton";

            error.ForeColor = System.Drawing.Color.Red;
            error.Style.Value = "display:block;";

            divClose.Style.Value = "display:inline; float:right";

            header.Font.Size = FontUnit.Larger;

            lbtb.Style.Value = "margin-right:10px";

            divInput.Controls.Add(lbtb);
            divInput.Controls.Add(tb);

            divForm.Style.Value = "display:flex; flex-direction: column; align-items: center; justify-content: center; margin-top:15px; width: 100%;";
            divForm.Controls.Add(divInput);
            divForm.Controls.Add(db);
            divForm.Controls.Add(error);
  
            divClose.Controls.Add(btClose);

            picker.CssClass = "picker";
            cover.CssClass = "cover";

            picker.Controls.Add(header);
            picker.Controls.Add(divClose);
            picker.Controls.Add(divForm);

            picker_container.Controls.Add(picker);
            picker_container.Controls.Add(cover);
        }
    }
}