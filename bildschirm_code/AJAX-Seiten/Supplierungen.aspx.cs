using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Infoscreen_Bibliotheken;

namespace Infoscreen_Bildschirme
{
    public partial class Supplierungen : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Variablen anlegen
            string AbteilungsID = Request.QueryString["id"];
            const int Spaltenanzahl = 7;
            const int Anz_MaxSupplierungen = 4;
            const string Cookiname = "Supplierungen_Steitencookie";
            Bildschirm_Methoden Methoden = new Bildschirm_Methoden();
            Structuren.LehrerSupplierungen[] LehrerSupplierungen = FachHelper.SupplierplanAbrufen(AbteilungsID);

            int ErsteAngezeigteSupplierungsnummer = 0;
            List<Structuren.Tabellendaten[,]> Daten = new List<Structuren.Tabellendaten[,]>();
            Structuren.Tabellendaten[,] Datenzwischenspeicher;

            int AnzSeiten;
            // Prüfen, ob die Anzahl der Supplierungslehrer ein vielfaches von 4 beträgt
            if (LehrerSupplierungen.Length / Anz_MaxSupplierungen != LehrerSupplierungen.Length / (Anz_MaxSupplierungen * 1.0))
            {
                AnzSeiten = LehrerSupplierungen.Length / Anz_MaxSupplierungen + 1;
            }
            else
            {
                AnzSeiten = LehrerSupplierungen.Length / Anz_MaxSupplierungen;
            }
            // Prüfen, ob diese Seite mehr als einen Durchgang braucht.
            if (AnzSeiten > 1)
            {
                Response.Cookies["SeitendurchgangBeendet"].Value = "1";
            }
            int AktuelleSeite = Methoden.Seitenauswahl(Cookiname, AnzSeiten);
            int Anfangssupplierung = AktuelleSeite * Anz_MaxSupplierungen + 1;

            for (int i = ErsteAngezeigteSupplierungsnummer; i < Anz_MaxSupplierungen && i < LehrerSupplierungen.Length; i++)
            {
                // Datenzwischenspeicher für die Tabellendaten anlegen
                int Überschriftszeilen = 2;
                Datenzwischenspeicher = new Structuren.Tabellendaten[LehrerSupplierungen[i].Supplierungen.Length + Überschriftszeilen, Spaltenanzahl];
                // Die Überschrifften der Splaten festlegen
                Methoden.Tabellendaten_InformationHinzufügen(ref Datenzwischenspeicher[0, 0], LehrerSupplierungen[i].Lehrer, "Abteilungsscreen_SupplierungsPrimährlerher", Spaltenanzahl);
                Methoden.Tabellendaten_InformationHinzufügen(ref Datenzwischenspeicher[1, 0], "Datum", "Abteilungsscreen_Tabellenheader");
                Methoden.Tabellendaten_InformationHinzufügen(ref Datenzwischenspeicher[1, 1], "Std", "Abteilungsscreen_Tabellenheader");
                Methoden.Tabellendaten_InformationHinzufügen(ref Datenzwischenspeicher[1, 2], "Klasse", "Abteilungsscreen_Tabellenheader");
                Methoden.Tabellendaten_InformationHinzufügen(ref Datenzwischenspeicher[1, 3], "Lehrer", "Abteilungsscreen_Tabellenheader");
                Methoden.Tabellendaten_InformationHinzufügen(ref Datenzwischenspeicher[1, 4], "Fach", "Abteilungsscreen_Tabellenheader");
                // Die Daten der Supplierung in ein Passendes Format für den Tabellengenerator bringen
                for (int Zeile = Überschriftszeilen; Zeile < LehrerSupplierungen[i].Supplierungen.Length + Überschriftszeilen; Zeile++)
                {
                    Methoden.Tabellendaten_InformationHinzufügen(ref Datenzwischenspeicher[Zeile, 0], LehrerSupplierungen[i].Supplierungen[Zeile - Überschriftszeilen].Datum.ToString("dd.MM"), "Abteilungsscreen_TabellenCell");
                    Methoden.Tabellendaten_InformationHinzufügen(ref Datenzwischenspeicher[Zeile, 1], LehrerSupplierungen[i].Supplierungen[Zeile - Überschriftszeilen].Stunde.ToString(), "Abteilungsscreen_TabellenCell");
                    Methoden.Tabellendaten_InformationHinzufügen(ref Datenzwischenspeicher[Zeile, 2], LehrerSupplierungen[i].Supplierungen[Zeile - Überschriftszeilen].Klasse, "Abteilungsscreen_TabellenCell");
                    // Prüfen, on die Stunde entfällt
                    if (LehrerSupplierungen[i].Supplierungen[Zeile - Überschriftszeilen].Entfällt == true)
                    {
                        // Prüfen, ob eine ersatzbeschäftigung eingetragen wurde
                        if (LehrerSupplierungen[i].Supplierungen[Zeile - Überschriftszeilen].Ersatzfach == null)
                        {
                            Methoden.Tabellendaten_InformationHinzufügen(ref Datenzwischenspeicher[Zeile, 3], "entfällt", "Abteilungsscreen_TabellenCell", 2);
                        }
                        else
                        {
                            Methoden.Tabellendaten_InformationHinzufügen(ref Datenzwischenspeicher[Zeile, 3], LehrerSupplierungen[i].Supplierungen[Zeile - Überschriftszeilen].Ersatzfach, "Abteilungsscreen_TabellenCell", 2);
                        }
                    }
                    else
                    {
                        if (LehrerSupplierungen[i].Supplierungen[Zeile - Überschriftszeilen].Ersatzlehrer != null)
                        {
                            Methoden.Tabellendaten_InformationHinzufügen(ref Datenzwischenspeicher[Zeile, 3], LehrerSupplierungen[i].Supplierungen[Zeile - Überschriftszeilen].Ersatzlehrer, "Abteilungsscreen_TabellenCell");
                        }
                        else
                        {
                            Methoden.Tabellendaten_InformationHinzufügen(ref Datenzwischenspeicher[Zeile, 3], LehrerSupplierungen[i].Supplierungen[Zeile - Überschriftszeilen].Ursprungsleher, "Abteilungsscreen_TabellenCell");
                        }
                        Methoden.Tabellendaten_InformationHinzufügen(ref Datenzwischenspeicher[Zeile, 4], LehrerSupplierungen[i].Supplierungen[Zeile - Überschriftszeilen].Ersatzfach, "Abteilungsscreen_TabellenCell");
                    }
                }
                // Datenstrang an einen Stamm haften
                Daten.Add(Datenzwischenspeicher);
            }

            // Die Tabellen im GUI darstellen
            switch (Daten.Count)
            {
                case 1:
                    Methoden.Tabelle_Erzeugen(ref table_Supplierung_1, Daten[0]);
                    div_Supplierung_1.Style.Add("display", "initial");
                    div_Supplierung_1.Style.Add("width", "100%");
                    div_Supplierung_1.Style.Add("height", "100%"); break;
                case 2:
                    Methoden.Tabelle_Erzeugen(ref table_Supplierung_1, Daten[0]);
                    div_Supplierung_1.Style.Add("display", "initial");
                    div_Supplierung_1.Style.Add("width", "50%");
                    div_Supplierung_1.Style.Add("height", "100%");
                    Methoden.Tabelle_Erzeugen(ref table_Supplierung_2, Daten[1]);
                    div_Supplierung_2.Style.Add("display", "initial");
                    div_Supplierung_2.Style.Add("width", "50%");
                    div_Supplierung_2.Style.Add("height", "100%"); break;
                case 3:
                    Methoden.Tabelle_Erzeugen(ref table_Supplierung_1, Daten[0]);
                    div_Supplierung_1.Style.Add("display", "initial");
                    div_Supplierung_1.Style.Add("width", "50%");
                    div_Supplierung_1.Style.Add("height", "50%");
                    Methoden.Tabelle_Erzeugen(ref table_Supplierung_2, Daten[1]);
                    div_Supplierung_2.Style.Add("display", "initial");
                    div_Supplierung_2.Style.Add("width", "50%");
                    div_Supplierung_2.Style.Add("height", "100%");
                    Methoden.Tabelle_Erzeugen(ref table_Supplierung_3, Daten[2]);
                    div_Supplierung_3.Style.Add("display", "initial");
                    div_Supplierung_3.Style.Add("width", "50%");
                    div_Supplierung_3.Style.Add("height", "50%"); break;
                case 4:
                    Methoden.Tabelle_Erzeugen(ref table_Supplierung_1, Daten[0]);
                    div_Supplierung_1.Style.Add("display", "initial");
                    div_Supplierung_1.Style.Add("width", "50%");
                    div_Supplierung_1.Style.Add("height", "50%");
                    Methoden.Tabelle_Erzeugen(ref table_Supplierung_2, Daten[1]);
                    div_Supplierung_2.Style.Add("display", "initial");
                    div_Supplierung_2.Style.Add("width", "50%");
                    div_Supplierung_2.Style.Add("height", "50%");
                    Methoden.Tabelle_Erzeugen(ref table_Supplierung_3, Daten[2]);
                    div_Supplierung_3.Style.Add("display", "initial");
                    div_Supplierung_3.Style.Add("width", "50%");
                    div_Supplierung_3.Style.Add("height", "50%");
                    Methoden.Tabelle_Erzeugen(ref table_Supplierung_4, Daten[3]);
                    div_Supplierung_4.Style.Add("display", "initial");
                    div_Supplierung_4.Style.Add("width", "50%");
                    div_Supplierung_4.Style.Add("height", "50%"); break;
            }
            // Die aktuelle Seite im GUI vermerken
            lb_Fußzeile.Text = "Seite " + (AktuelleSeite + 1) + " von " + AnzSeiten;
            // Prüfen, ob die Seite hren kompletten Content ausgegeben hat
            if (AktuelleSeite == AnzSeiten)
            {
                Response.Cookies["SeitendurchgangBeendet"].Value = "0";
            }
        }
    }
}