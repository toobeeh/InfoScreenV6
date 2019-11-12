using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using Infoscreen_Bibliotheken;
using Infoscreen_Verwaltung.classes;
using System.IO;

namespace Infoscreen_Bildschirme
{
    public partial class main : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                // Die ID auslesen
                string id = Request.QueryString["id"];

                try
                {
                    ZeitAnzeige.ZGAAbfragen(id.ToInt32());
                }
                catch { }

                Infoscreen_Verwaltung.classes.Structuren.AnzeigeElemente anzeigen;

                anzeigen = Infoscreen_Verwaltung.classes.DatenbankAbrufen.AnzuzeigendeElemente(id);

                int aktuelleNummer;

                if (Request.Cookies["WebPageZaehler"] != null)
                {
                    try
                    {
                        aktuelleNummer = Convert.ToInt32(Request.Cookies["WebPageZaehler"].Value);
                    }
                    catch
                    {
                        aktuelleNummer = -1;
                    }

                    if (Request.Cookies["SeitendurchgangBeendet"] != null)
                    {
                        if (Request.Cookies["SeitendurchgangBeendet"].Value == "0")
                        {
                            aktuelleNummer = AnzeigeNummer(aktuelleNummer, anzeigen);
                        }
                    }
                    else
                    {
                        aktuelleNummer = AnzeigeNummer(aktuelleNummer, anzeigen);
                    }


                }
                else
                {
                    aktuelleNummer = AnzeigeNummer(-1, anzeigen);
                }

                switch (aktuelleNummer)
                {
                    case 0:
                        placeholder.Src = "http://elv-screen-2/bildschirm/Stundenanzeige.aspx?id=" + id + "&nr=" + aktuelleNummer;
                        break;
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                        placeholder.Src = "http://elv-screen-2/bildschirm/Abteilungsscreen.aspx?id=" + id + "&nr=" + aktuelleNummer;
                        break;
                    case 5:
                        placeholder.Src = "http://elv-screen-2/bildschirm/Lehrerscreen.aspx?id=" + id;
                        break;
                    case 6:
                        placeholder.Src = "http://elv-screen-2/bildschirm/PowerPoint.aspx?id=" + id;
                        break;
                }

                try
                {
                    Response.Cookies["WebPageZaehler"].Value = aktuelleNummer.ToString();
                }
                catch
                { }
            }
            catch
            {
                Response.Redirect("mycustompage.htm");
            }
        }

        /*
         * 0 : Stundenplan & Klasseninfo
         * 1 : Abteilungsinfo
         * 2 : Sprechstunden
         * 3 : Raumaufteilung
         * 4 : Supplierplan
         * 5 : Lehrer Supplierung Aktuell
         * 6 : PowerPoints
         */

        int AnzeigeNummer(int _aktuell, Structuren.AnzeigeElemente _anzuzeigen)
        {
            if (_aktuell < 0)
            {
                if (_anzuzeigen.Stundenplan) return 0;
                if (_anzuzeigen.Abteilungsinfo) return 1;
                if (_anzuzeigen.Sprechstunden) return 2;
                if (_anzuzeigen.Raumaufteilung) return 3;
                if (_anzuzeigen.Supplierplan) return 4;
                if (_anzuzeigen.AktuelleSupplierungen) return 5;
                if (_anzuzeigen.PowerPoints > -1) return 6;
                return -1;
            }
            if (_anzuzeigen.Stundenplan && _aktuell < 0) return 0;
            if (_anzuzeigen.Abteilungsinfo && _aktuell < 1) return 1;
            if (_anzuzeigen.Sprechstunden && _aktuell < 2) return 2;
            if (_anzuzeigen.Raumaufteilung && _aktuell < 3) return 3;
            if (_anzuzeigen.Supplierplan && _aktuell < 4) return 4;
            if (_anzuzeigen.AktuelleSupplierungen && _aktuell < 5) return 5;
            if (_anzuzeigen.PowerPoints > -1 && _aktuell < 6) return 6;

            if (_anzuzeigen.Stundenplan) return 0;
            if (_anzuzeigen.Abteilungsinfo) return 1;
            if (_anzuzeigen.Sprechstunden) return 2;
            if (_anzuzeigen.Raumaufteilung) return 3;
            if (_anzuzeigen.Supplierplan) return 4;
            if (_anzuzeigen.AktuelleSupplierungen) return 5;
            if (_anzuzeigen.PowerPoints > -1) return 6;

            return -1;
        }
    }
}