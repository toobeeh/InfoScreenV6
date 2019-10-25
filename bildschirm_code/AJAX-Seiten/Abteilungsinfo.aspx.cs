using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Infoscreen_Bibliotheken;

namespace Infoscreen_Bildschirme
{
    public partial class Abteilungsinfo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Variablen anlegen
            string AbteilungsID = Request.QueryString["id"];
            Bildschirm_Methoden Methoden = new Bildschirm_Methoden();
            // Anzuzeigenden Text aus der Datenbank herunterladen
            string AnzeigeText = Infoscreen_Verwaltung.classes.DatenbankAbrufen.AbteilungsinfoAbrufen(AbteilungsID);
            // Anzuzeigenden Text zu einem HTML-Format umformatiernen
            AnzeigeText = Methoden.BBzuHTML_Konverter(AnzeigeText);
            // Anzuzeigenden Text ausgeben
            tablecell_Abteilungsinfo.Text = AnzeigeText;
        }
    }
}