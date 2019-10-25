using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Infoscreen_Verwaltung.classes;

namespace Infoscreen_Verwaltung.conf
{
    public partial class abteilungsinfo : System.Web.UI.Page
    {
        string ebene = "../";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!classes.Login.Angemeldet) Response.Redirect("../login.aspx");
            if (!classes.Login.Rechte.Stundenplan.OneBiggerThan(1)) Response.Redirect("../");
            
            TopMenu.Text = classes.Anzeige.TopBar(ebene);
            Menu.Text = classes.Anzeige.Menue(ebene, "conf");

            if (!Page.IsPostBack)
            {
                string[] abteilungen;
                if (classes.Login.Rechte.Abteilungsinfo[2].ToInt32() > 1) //Superadmin
                    abteilungen = classes.DatenbankAbrufen.AbteilungenAbrufen();
                else if (classes.Login.Rechte.Abteilungsinfo[1].ToInt32() > 0) //AV, Schulwart, Lehrer
                    abteilungen = classes.Login.Abteilungen;
                else abteilungen = new string[] { classes.Login.StammAbteilung };

                foreach (string abteilung in abteilungen)
                    dropDownAbteilung.Items.Add(abteilung);

                try { dropDownAbteilung.SelectedValue = classes.Login.StammAbteilung; }
                catch { dropDownAbteilung.SelectedIndex = -1; }

                AbteilungsinfoAbrufen();
            }
        }

        protected void DropDownAbteilung_SelectedIndexChanged(object sender, EventArgs e)
        {
            editor.InnerHtml = "";
            AbteilungsinfoAbrufen();
        }

        protected void AbteilungsinfoAbrufen()
        {
            editor.InnerHtml = classes.DatenbankAbrufen.AbteilungsinfoAbrufen(dropDownAbteilung.SelectedValue);
        }

        protected void btÜbernehmen_Click(object sender, EventArgs e)
        {
            classes.DatenbankSchreiben.AbteilungsinfoSetzen(dropDownAbteilung.SelectedValue, editor.InnerHtml);
        }

        protected void btReset_Click(object sender, EventArgs e)
        {
            AbteilungsinfoAbrufen();
        }
    }
}