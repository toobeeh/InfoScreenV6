using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Infoscreen_Verwaltung.classes;

namespace Infoscreen_Verwaltung.show
{
    public partial class abteilungsinfo : System.Web.UI.Page
    {
        string ebene = "../";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!classes.Login.Angemeldet) Response.Redirect("../login.aspx");
            if (classes.Login.Rechte.Abteilungsinfo.ToInt32() == 0) Response.Redirect("../");

            TopMenu.Text = classes.Anzeige.TopBar(ebene);
            Menu.Text = classes.Anzeige.Menue(ebene, "show");

            if (!Page.IsPostBack)
            {
                string[] abteilungen;
                if (classes.Login.Rechte.Abteilungsinfo[2] != '0') //Superadmin
                    abteilungen = classes.DatenbankAbrufen.AbteilungenAbrufen();
                else if (classes.Login.Rechte.Abteilungsinfo[1] != '0') //AV, Schulwart, Lehrer
                    abteilungen = new string[] { "Elektronik" }; //wenn System auch von anderen Abteilungen verwendet wird: abteilungen = classes.Login.Abteilungen;
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
            lbAbteilungsinfo.Text = "";
            AbteilungsinfoAbrufen();
        }

        protected void AbteilungsinfoAbrufen()
        {
            lbAbteilungsinfo.Text = classes.DatenbankAbrufen.AbteilungsinfoAbrufen(dropDownAbteilung.SelectedValue).BBCodeToHTML();
        }
    }
}