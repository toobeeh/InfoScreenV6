using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Infoscreen_Verwaltung.classes;

namespace Infoscreen_Verwaltung.admin.bildschirm_einstellungen
{
    public partial class _default : System.Web.UI.Page
    {
        string ebene = "../../";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!classes.Login.Angemeldet) Response.Redirect("../../login.aspx");
            if (!classes.Login.Rechte.Bildschirme.OneBiggerThan(1)) Response.Redirect("../../");

            TopMenu.Text = classes.Anzeige.TopBar(ebene);
            Menu.Text = classes.Anzeige.Menue(ebene);

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
                    dropDownBetriebsmode.Items.Add(new ListItem(temp.bezeichnung, temp.id.ToString()));


                if (classes.Login.Rechte.Bildschirme[2].ToInt32() > 2)
                {
                    btNeu.Visible = true;
                    btÄndern.Visible = true;
                }
                if (classes.Login.Rechte.Bildschirme[2].ToInt32() > 3)
                    btLöschen.Visible = true;

                BetriebsmodeLaden();
            }
        }

        protected void DropDownAbteilung_SelectedIndexChanged(object sender, EventArgs e)
        {
            BetriebsmodeLaden();
        }

        private void BetriebsmodeLaden()
        {
            try
            {
                dropDownBetriebsmode.SelectedValue = DatenbankAbrufen.AktuellenBetriebsmodeAbrufen(dropDownAbteilung.SelectedValue).ToString();
            }
            catch
            {
                dropDownBetriebsmode.SelectedIndex = -1;
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
    }
}