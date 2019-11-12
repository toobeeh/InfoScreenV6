using Infoscreen_Verwaltung.classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Web.Script.Services;

namespace Infoscreen_Verwaltung.conf.tests
{
    
    public partial class modify : System.Web.UI.Page
    {
        string ebene = "../../";


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
            if (!classes.Login.Angemeldet) Response.Redirect("../../login.aspx");
            if (!classes.Login.Rechte.Tests.OneBiggerThan(1)) Response.Redirect("../../");


            TopMenu.Text = classes.Anzeige.TopBar(ebene);
            Menu.Text = classes.Anzeige.Menue(ebene, "conf");

            lb_SA.Text = "Schularbeit/Test erstellen [" + Session["12_Klasse"] + "]"; //Klasse anzzeigen, wenn der Admin die Klasse wählt
            

            if (!Page.IsPostBack)
            {
                tbLehrer.CssClass = "autocompLehrer";
                tbFach.CssClass = "autocompFach";
                for (int i = 0; i < DatenbankAbrufen.TestartenAbrufen().Length; i++) //Testarten als Werte für die DropDownListe hinzufügen
                {
        
                        dropdlTestart.Items.Add(DatenbankAbrufen.TestartenAbrufen()[i]);
                }
                for (int i = 0; i < DatenbankAbrufen.RaumAbrufen().Length; i++)
                {
                    DropListRaum.Items.Add(DatenbankAbrufen.RaumAbrufen()[i]);
                    if(DatenbankAbrufen.RaumAbrufen()[i].Contains(Session["12_Klasse"].ToString()))
                    {
                        DropListRaum.Items[i].Selected = true;
                    }
                }
                DropListRaum.Items.Add("Ersatzraum");

                if (Session["12_Klasse"] == null)
                {
                    Response.Redirect("./");
                    return;
                }

                if (Session["12_Neu"] != null)
                {
                    datum.SelectedDate = DateTime.Today;
                    datum.VisibleDate = DateTime.Today;
                }
                else if (Session["12_Daten"] != null)
                {
                    Structuren.Tests temp = (Structuren.Tests)Session["12_Daten"];
                    datum.SelectedDate = temp.Datum;
                    datum.VisibleDate = temp.Datum;
                    /*if (dropdlTestart.SelectedValue.ToString() == "Matura")
                    {
                        tbDauer.Text = "4"; //Wenn Testart der Matura entspricht -> Dauer auf 4h einstellen
                    }
                    else if (dropdlTestart.SelectedValue.ToString() != "Matura" || dropdlTestart.SelectedValue.ToString() != "Schularbeit")
                    {
                        tbDauer.Text = "1"; // Wenn die Testart einer SMÜ entspricht -> Dauer auf 1h einstellen
                    }*/
                    tbDauer.Text = temp.Dauer.ToInt32().ToString();
                    tbStunde.Text = temp.Stunde.ToString();
                    tbFach.Text = DatenbankAbrufen.GetFachKürzelVonFach(temp.Fach);
                    tbLehrer.Text = temp.Lehrer;
                    dropdlTestart.SelectedValue = temp.Testart;
                    DropListRaum.SelectedValue = temp.Raum.ToString();
                }
                else
                {
                    Response.Redirect("./");
                    return;
                }
            }
        }

        protected void DropListRaum_SelectionChanged(object sender, EventArgs e)
        {
            if (DropListRaum.SelectedValue == "Ersatzraum") tbErsatzraum.Enabled = true;
            else tbErsatzraum.Enabled = false;
        }
        protected void btÜbernehmen_Click(object sender, EventArgs e)
        {
            List<string> fehler = new List<string>();
            if (tbStunde.Text.ToInt32() == -1) fehler.Add("Die Stunde darf nur aus Zahlen bestehen.");
            else if (tbStunde.Text.ToInt32() < 0 || tbStunde.Text.ToInt32() > Properties.Resources.maximaleStunden.ToInt32())
                fehler.Add(String.Format("Die Stunde muss zwischen {0} und {1} liegen!", 0, Properties.Resources.maximaleStunden));
            if (tbDauer.Text.ToInt32() == -1) fehler.Add("Die Dauer darf nur aus Zahlen bestehen. Sie wird in Stunden angegeben.");
            else if (tbDauer.Text.ToInt32() < 1 || tbDauer.Text.ToInt32() > 4)
                fehler.Add(String.Format("Die Dauer muss zwischen {0} und {1} liegen!", 1, 4));
            else if (DateTime.Now.ToInt32() > datum.SelectedDate.ToInt32())
                fehler.Add(String.Format("Das Datum ist schon abgelaufen!"));
            if (fehler.Count > 0)
            {
                LabelFehler.Text = "<ul>\r\n<li>" + String.Join("</li>\r\n<li>", fehler) + "</li>\r\n</ul>";
            }
            else
            {
                DatenbankSchreiben.FachPrüfen(tbFach.Text);
                Structuren.Tests daten = new Structuren.Tests();
                daten.Datum = datum.SelectedDate;
                daten.Stunde = tbStunde.Text.ToInt32();
                daten.Fach = tbFach.Text;
                daten.Dauer = tbDauer.Text.ToInt32();
                daten.Lehrer = tbLehrer.Text;
                daten.Testart = dropdlTestart.SelectedIndex.ToString();
                daten.Raum = DropListRaum.SelectedValue.ToString();
                string vergleiche = "Ersatzraum";
                if (string.Compare(DropListRaum.SelectedValue.ToString(), vergleiche) == 0)
                    daten.Raum = tbErsatzraum.Text;
                if (Session["12_Neu"] != null)
                {
                    DatenbankSchreiben.TestAnlegen(Session["12_Klasse"].ToString(), daten);
                }
                else if (Session["12_Daten"] != null)
                {
                    DatenbankSchreiben.TestÄndern(Session["12_Klasse"].ToString(), daten, (Structuren.Tests)Session["12_Daten"]);
                }
                else
                {
                    LabelFehler.Text = "Unbekannter Fehler beim Abrufen der Session-Daten für diese Seite";
                }

                Session["12_Neu"] = null;
                Session["12_Daten"] = null;

                Response.Redirect("./");
            }
        }
    }
}