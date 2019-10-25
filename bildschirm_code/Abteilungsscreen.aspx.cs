using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Infoscreen_Bibliotheken;

namespace Infoscreen_Bildschirme
{
    public partial class Abteilungsscreen : System.Web.UI.Page
    {
        /// <summary>
        ///  Die Page_PreInit-Met - Methode wird noch vor der Page_Load - Methode aufgerufen.
        ///  In diesem Fall wird sie verwendet um die Aktiven Tabs als diese zu makiern
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreInit(object sender, EventArgs e)
        {
            int aktuelleNummer = Convert.ToInt32(Request.QueryString["nr"]);
            switch (aktuelleNummer)
            {
                case 1:
                    tab3.SkinID = "AktiverTab";
                    break;
                case 2:
                    tab4.SkinID = "AktiverTab";
                    break;
                case 3:
                    tab5.SkinID = "AktiverTab";
                    break;
                case 4:
                    tab6.SkinID = "AktiverTab";
                    break;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Variaben anlegen
            string bildschirmID = Request.QueryString["id"];
            int aktuelleNummer = Convert.ToInt32(Request.QueryString["nr"]);
            Bildschirm_Methoden Methoden = new Bildschirm_Methoden();
            // Sichtbarkeit der Tabs bestimmen
            int Anz_SichtbareTabs = 0;
            if (tab3.Visible = Infoscreen_Verwaltung.classes.DatenbankAbrufen.AnzuzeigendeElemente(bildschirmID).Abteilungsinfo == true) Anz_SichtbareTabs++;
            if (tab4.Visible = Infoscreen_Verwaltung.classes.DatenbankAbrufen.AnzuzeigendeElemente(bildschirmID).Sprechstunden == true) Anz_SichtbareTabs++;
            if (tab5.Visible = Infoscreen_Verwaltung.classes.DatenbankAbrufen.AnzuzeigendeElemente(bildschirmID).Raumaufteilung == true) Anz_SichtbareTabs++;
            if (tab6.Visible = Infoscreen_Verwaltung.classes.DatenbankAbrufen.AnzuzeigendeElemente(bildschirmID).Supplierplan == true) Anz_SichtbareTabs++;
            // Größe der Tabs bestimmen
            int PlatzProTab = 100 / Anz_SichtbareTabs;
            if (PlatzProTab == 100)
            {
                div_Tabs.Visible = false;
            }
            else
            {
                tab3.Width = PlatzProTab;
                tab4.Width = PlatzProTab;
                tab5.Width = PlatzProTab;
                tab6.Width = PlatzProTab;
            }
            // IDs den AJAX-Seiten zuordnen
            switch (aktuelleNummer)
            {
                case 1:
                    placeholder.Src = "./AJAX-Seiten/Abteilungsinfo.aspx?id=" + bildschirmID;
                    break;
                case 2:
                    placeholder.Src = "./AJAX-Seiten/Sprechstunden.aspx?id=" + bildschirmID;
                    break;
                case 3:
                    placeholder.Src = "./AJAX-Seiten/Raumaufteilung.aspx?id=" + bildschirmID;
                    break;
                case 4:
                    placeholder.Src = "./AJAX-Seiten/Supplierungen.aspx?id=" + bildschirmID;
                    break;
            }
        }
    }
}