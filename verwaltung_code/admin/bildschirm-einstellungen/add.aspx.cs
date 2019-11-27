using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Infoscreen_Verwaltung.classes;

namespace Infoscreen_Verwaltung.admin.bildschirm_einstellungen
{
    public partial class add : System.Web.UI.Page
    {
        string ebene = "../../";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!classes.Login.Angemeldet)
            {
                Session.Clear();
                Response.Redirect("../../login.aspx");
            }
            if (classes.Login.Rechte.Bildschirme[2].ToInt32() < 3)
                btAbbrechen_Click(btAbbrechen, new EventArgs());

            TopMenu.Text = classes.Anzeige.TopBar(ebene);
            Menu.Text = classes.Anzeige.Menue(ebene, "admin");

            if (!Page.IsPostBack)
            {
                switch (Session["21_Art"].ToString())
                {
                    case "neu":
                        überschrift.InnerText = "Neuen Betriebsmode erstellen";
                        //if (Session["21_Abteilung"] == null)
                        //    cbBildschirmeHinzu.Visible = false;
                        lbTbName.Text = "Name des neuen Betriebsmode:";
                        /* debug */
                            //cbBildschirmeHinzu.Checked = true;
                            //cbBildschirmeHinzu.Enabled = false;
                        /* end */
                        break;
                    case "ändern":
                        überschrift.InnerText = "Betreibsmode ändern";
                        //cbBildschirmeHinzu.Visible = false;
                        lbTbName.Text = "Name des Betriebsmode:";
                        goto case "ändern/löschen";
                    case "löschen":
                        if (classes.Login.Rechte.Bildschirme[2].ToInt32() < 4) goto default;
                        überschrift.InnerText = "Betreibsmode löschen";
                        //cbBildschirmeHinzu.Visible = false;
                        lbTbName.Text = "Name des Betriebsmode:";
                        TextBoxName.Enabled = false;
                        btÜbernehmen.Text = "Löschen bestätigen";
                        lbWarnung.Visible = true;
                        goto case "ändern/löschen";
                    case "ändern/löschen":
                        if (Session["21_Daten"] != null)
                            TextBoxName.Text = ((Structuren.Betriebsmodi)Session["21_Daten"]).bezeichnung;
                        else
                            goto default;
                        break;
                    default:
                        Session["21_Art"] = null;
                        Session["21_Daten"] = null;
                        Response.Redirect("./");
                        break;
                }
            }
        }

        protected void btÜbernehmen_Click(object sender, EventArgs e)
        {
            if ((Session["21_Art"].ToString() == "neu" || Session["21_Art"].ToString() == "ändern") && TextBoxName.Text.Length > 45)
            {
                lbError.Visible = true;
                return;
            }
            else
                lbError.Visible = false;
                
            switch (Session["21_Art"].ToString())
            {
                case "neu":
                    Structuren.Betriebsmodi temp = new Structuren.Betriebsmodi();
                    temp.bezeichnung = TextBoxName.Text;
                    temp.id = DatenbankSchreiben.BetriebsmodeErstellen(TextBoxName.Text);
                    Session["21_Daten"] = temp;

                    //if (cbBildschirmeHinzu.Checked)
                    DatenbankSchreiben.BetriebsmodeBildschirmEinerAbteilungHinzufügen(temp.id, Session["21_Abteilung"].ToString());

                    // Session["21_Art"] = null; ISv6: Reset Art in default.aspx on btFertig click, otherwise Art isnt set when navigating back to add.aspx
                    Response.Redirect("./betriebsmode/");
                    break;
                case "ändern":
                    DatenbankSchreiben.BetriebsmodeBezeichnungÄndern(TextBoxName.Text, ((Structuren.Betriebsmodi)Session["21_Daten"]).id);
                    goto default;
                case "löschen":
                    DatenbankSchreiben.BetriebsmodeLöschen(((Structuren.Betriebsmodi)Session["21_Daten"]).id);
                    goto default;
                default:
                    Session["21_Art"] = null;
                    Session["21_Daten"] = null;
                    Response.Redirect("./");
                    break;
            }
        }

        protected void btAbbrechen_Click(object sender, EventArgs e)
        {
            Session["21_Art"] = null;
            Session["21_Daten"] = null;
            Response.Redirect("./");
        }
    }
}