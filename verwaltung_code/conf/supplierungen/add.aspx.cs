using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Infoscreen_Verwaltung.conf.supplierungen
{
    public partial class add : System.Web.UI.Page
    {
        string ebene = "../../";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["13_Abteilung"] == null) Response.Redirect("./");

            TopMenu.Text = classes.Anzeige.TopBar(ebene);
            Menu.Text = classes.Anzeige.Menue(ebene, "conf");
            
        }

        protected void btÜbernehmen_Click(object sender, EventArgs e)
        {
            if (!classes.DatenbankAbrufen.IsLehrerKürzel(tbLehrer.Text))
            {
                lbError.Visible = true;
                return;
            }
            Session["13_Lehrer"] = tbLehrer.Text;
            Response.Redirect("./table.aspx");
        }

        protected void btAbbruch_Click(object sender, EventArgs e)
        {
            Response.Redirect("./");
        }

        protected void calendar_SelectionChanged(object sender, EventArgs e)
        {
            Session["13_Datum"] = calendar.SelectedDate;
            Response.Redirect("./add.aspx");
        }

        protected void tbLehrer_TextChanged(object sender, EventArgs e)
        {
            
            Response.Redirect("./add.aspx");
            lbName.Text = "1";
            int AbtID = classes.DatenbankAbrufen.GetAbteilungsIdVonAbteilungsname(Session["13_Abteilung"].ToString());
            Session["13_Lehrer"] = tbLehrer.Text;
            if (classes.DatenbankAbrufen.LehrerKuerzelExists(Session["13_Lehrer"].ToString(), AbtID) == 2) //if lehrerkürzel ist vorhanden
            {
                lbName.Text = "<h3>ausgewählter Lehrer: " + classes.DatenbankAbrufen.LehrerNamen(Session["13_Lehrer"].ToString()) + "<h3>";
                Response.Redirect("./add.aspx");
            }
            else
            {
                lbName.Text = "3";
                lbError.Visible = true;
                Response.Redirect("./add.aspx");
            }
        }
    }
}