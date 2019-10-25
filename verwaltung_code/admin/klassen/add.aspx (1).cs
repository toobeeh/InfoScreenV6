using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Infoscreen_Verwaltung.classes;

namespace Infoscreen_Verwaltung.admin.klassen
{
    public partial class add : System.Web.UI.Page
    {
        string ebene = "../../";

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<string> GetGebaeude(string pre)
        {
            List<string> Gebäude = new List<string>();
            using (classes.Entities db = new classes.Entities())
            {
                Gebäude = (from a in db.Raeume where a.Gebäude.Contains(pre) select a.Gebäude).ToList();
            }
            return Gebäude;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<string> GetKlasse(string pre)
        {
            List<string> Räume = new List<string>();
            using (classes.Entities db = new classes.Entities())
            {
                Räume = (from a in db.Raeume where a.Raum.ToString().Contains(pre) select a.Raum.ToString()).ToList();
            }
            return Räume;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!classes.Login.Angemeldet)
            {
                Session.Clear();
                Response.Redirect("../../login.aspx");
            }

            TopMenu.Text = classes.Anzeige.TopBar(ebene);
            Menu.Text = classes.Anzeige.Menue(ebene);
            
            if(Session["22_Klasse_Art"] == null)
            {
                Session["22_Klasse_Daten"] = null;
                Response.Redirect("./");
            }
                        
            switch (Session["22_Klasse_Art"].ToInt32())
            {
                case 0: //neu
                    überschrift.InnerText = "Neue Klasse anlegen";
                    break;
                case 1:
                    überschrift.InnerText = "Klasse bearbeiten";
                    tbKlasse.Enabled = false;
                    if (Session["22_Klasse_Daten"] == null)
                        goto default;
                    if (!Page.IsPostBack)
                    {
                        Structuren.Klasseneigenschaften data = (Structuren.Klasseneigenschaften)Session["22_Klasse_Daten"];
                        tbKlasse.Text = data.klasse;
                        tbGebäude.Text = data.gebäude;
                        tbRaum.Text = data.raum;
                    }
                    break;
                default:
                    Session["22_Klasse_Art"] = null;
                    Session["22_Klasse_Daten"] = null;
                    Response.Redirect("./");
                    break;
            }
        }

        protected void btÜbernehmen_Click(object sender, EventArgs e)
        {
            bool fehler = false;
            lbErrorTbGebäudeLen.Visible = false;
            lbErrorTbGebäudeNot.Visible = false;
            lbErrorTbKlasseDou.Visible = false;
            lbErrorTbKlasseLen.Visible = false;
            lbErrorTbRaumLen.Visible = false;
            lbErrorTbRaumNot.Visible = false;

            if (tbKlasse.Enabled)
            {
                if(tbKlasse.Text.Length > 10 || tbKlasse.Text.Length < 2)
                    fehler = lbErrorTbKlasseLen.Visible = true;
                if (DatenbankAbrufen.KlasseExists(tbKlasse.Text))
                    fehler = lbErrorTbKlasseDou.Visible = true;
            }

            if (tbGebäude.Text.Length > 40)
                fehler = lbErrorTbGebäudeLen.Visible = true;
            if (!DatenbankAbrufen.GebäudeExists(tbGebäude.Text))
                fehler = lbErrorTbGebäudeNot.Visible = true;

            if (tbRaum.Text.Length > 6 || tbRaum.Text.ToInt32() < 0)
                fehler = lbErrorTbRaumLen.Visible = true;
            if (!DatenbankAbrufen.RaumExists(tbRaum.Text))
                fehler = lbErrorTbRaumNot.Visible = true;

            if (!fehler)
            {

            }
        }

        protected void btAbbrechen_Click(object sender, EventArgs e)
        {
            Session["22_Klasse_Art"] = null;
            Session["22_Klasse_Daten"] = null;
            Response.Redirect("./");
        }
    }
}