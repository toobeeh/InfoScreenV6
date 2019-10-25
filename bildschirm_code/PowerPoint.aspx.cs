using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Infoscreen_Bibliotheken;

using Infoscreen_Verwaltung.classes;

namespace Infoscreen_Bildschirme
{
    public partial class PowerPoint : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Variablen anlegen
            int Seitenanzahl;
            string Stammpfad = @"E:\infoscreen\bildschirm_code\presentations";
            string StammpfadWeb = @"./presentations/";

            string id = Request.QueryString["id"];

            Infoscreen_Verwaltung.classes.Structuren.AnzeigeElemente daten = Infoscreen_Verwaltung.classes.DatenbankAbrufen.AnzuzeigendeElemente(id);

            string PresentationsOrdner = Stammpfad;
            string[] Bilderpfade;

            //  Pfade der Bilder aus Ordner auslesen und Anzahl der Seiten speichern
            string[] pfade = Directory.GetDirectories(Stammpfad);
            foreach(string pfad in pfade)
            {
                if (Directory.GetDirectories(pfad, daten.PowerPoints.ToString(), SearchOption.TopDirectoryOnly).Length > 0)
                {
                    PresentationsOrdner = Directory.GetDirectories(pfad, daten.PowerPoints.ToString(), SearchOption.TopDirectoryOnly)[0];
                    StammpfadWeb += PresentationsOrdner.Split('\\')[PresentationsOrdner.Split('\\').Length - 2] + "/";
                    break;
                }
            }
            Bilderpfade = Directory.GetFiles(PresentationsOrdner, "*.png", SearchOption.TopDirectoryOnly);
            Seitenanzahl = Bilderpfade.Length;

            // Überprüfen, ob überhaubt Bilder vorhanden sind
            if (Seitenanzahl == 0)
            {
                // Fehler
                return;
            }

            int Seitenzaehler;

            if (Request.Cookies["Seitenzaehler"] == null)
            {
                Response.Cookies["Seitenzaehler"].Value = "0";
            }

            Seitenzaehler = Convert.ToInt32(Request.Cookies["Seitenzaehler"].Value);

            //  Bild übergeben
            image_Bild.ImageUrl = WebBild(Bilderpfade[Seitenzaehler], StammpfadWeb);

            if (Seitenzaehler < Seitenanzahl - 1)
            {
                Seitenzaehler++;
                Response.Cookies["SeitendurchgangBeendet"].Value = "1";
            }
            else
            {
                Seitenzaehler = 0;
                Response.Cookies["SeitendurchgangBeendet"].Value = "0";
            }

            Response.Cookies["Seitenzaehler"].Value = Seitenzaehler.ToString();
        }

        /// <summary>
        /// Konvertiert einen Windows-Dateipfad in den für den Browser geeigneten Pfad
        /// </summary>
        /// <param name="_Pfad">Dateipfade unter Windows</param>
        /// <param name="_WebPfad">Dateipfad für den Browser</param>
        /// <returns></returns>
        private string WebBild(string _Pfad, string _WebPfad)
        {
            string[] temp = _Pfad.Split('\\');
            return _WebPfad + temp[temp.Length - 2] + "/" + temp[temp.Length - 1];
        }
    }
}