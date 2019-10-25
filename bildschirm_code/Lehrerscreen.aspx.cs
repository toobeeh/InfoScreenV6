using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Infoscreen_Bibliotheken;
//using Infoscreen_Verwaltung.classes;

namespace Infoscreen_Bildschirme
{
    public partial class Lehrerscreen : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Variablen anlegen
            //string AbteilungsID = Request.QueryString["id"];
            string AbteilungsID = "1";
            const int Spaltenanzahl = 7; //original: 6
            const int SupplierenProSeite = 9;
            const string Cookiname = "Lehrerscreen_Steitencookie";
            Bildschirm_Methoden Methoden = new Bildschirm_Methoden();

            // Die Informationen über die Supplierungen aus der Datenbank auf den Server übertragen
            Infoscreen_Verwaltung.classes.Structuren.Supplierungen[] Supplierungen_Zwischenspeicher = Infoscreen_Verwaltung.classes.DatenbankAbrufen.AktuelleSupplierungenAbrufen("1");

            // Stunden, die entfallen von anfang an wegfiltern
            /*int Anz_EntfalleneStunden = 0;
            for (int i = 0; i < Supplierungen_Zwischenspeicher.Length; i++)
            {
                if (Supplierungen_Zwischenspeicher[i].Entfällt == true) Anz_EntfalleneStunden++;
            }
            Structuren.Supplierungen[] Supplierungen = new Structuren.Supplierungen[Supplierungen_Zwischenspeicher.Length - Anz_EntfalleneStunden];
            int Anz_Filterungen = 0;
            for (int i = 0; i < Supplierungen_Zwischenspeicher.Length; i++)
            {
                if (Supplierungen_Zwischenspeicher[i].Entfällt == false)
                {
                    Supplierungen[i - Anz_Filterungen] = Supplierungen_Zwischenspeicher[i];
                }
                else
                {
                    Anz_Filterungen++;
                }
            }*/

            // Einen speicher anlegen, mit dem der Tabellengenerator etwas anfangen kann
            Structuren.Tabellendaten[,] Daten_Supplierungen = new Structuren.Tabellendaten[SupplierenProSeite + 1, Spaltenanzahl];

            int AnzSeiten=1; //experimentell!!!

            // Prüfen, ob die Anzahl der Lehrer ein vielfaches von 9 beträgt
            if (Supplierungen_Zwischenspeicher.Length / SupplierenProSeite != Supplierungen_Zwischenspeicher.Length / (SupplierenProSeite * 1.0))
            {
                AnzSeiten = Supplierungen_Zwischenspeicher.Length / SupplierenProSeite + 1;
            }
            else
            {
                AnzSeiten = Supplierungen_Zwischenspeicher.Length / SupplierenProSeite;
            }

            // Prüfen, ob diese Seite mehr als einen Durchgang braucht.
            if (AnzSeiten > 1)
            {
                Response.Cookies["SeitendurchgangBeendet"].Value = "1";
            }

            // Die Standard - Designregel setzten
            for (int i = 0; i < Daten_Supplierungen.GetLength(0); i++)
            {
                for (int j = 0; j < Daten_Supplierungen.GetLength(1); j++)
                {
                    Daten_Supplierungen[i, j].Text = " ";
                    Daten_Supplierungen[i, j].SkinID = "Lehrerscreen_TabellenCell";
                }
            }

            // den Tabellen-Datenspeicher mit Informationen füllen (Überschrifften)
            Daten_Supplierungen[0, 0].Text = "Datum";
            Daten_Supplierungen[0, 1].Text = "Std";
            Daten_Supplierungen[0, 2].Text = "Lehrer";
            Daten_Supplierungen[0, 3].Text = "Statt-Lehrer";
            Daten_Supplierungen[0, 4].Text = "Klasse";
            Daten_Supplierungen[0, 5].Text = "Raum";
            Daten_Supplierungen[0, 6].Text = "Ersatzfach";
            Daten_Supplierungen[0, 7].Text = "Art";
            ZeileSkinIDZuweisen(ref Daten_Supplierungen, 0, "Lehrerscreen_Tabellenheader");

            // den Tabellen-Datenspeicher mit Informationen füllen (Supplierungen)
            int AktuelleSeite = Methoden.Seitenauswahl(Cookiname, AnzSeiten);
            int Anfangssupplierung = AktuelleSeite * SupplierenProSeite + 1;
            //int EntfalleneStunden = 0;
            for (int i = Anfangssupplierung + 1; i <= (Supplierungen_Zwischenspeicher.Length / (AnzSeiten + Anfangssupplierung)); i++)
            {
                // prüfen, ob die Stunde nicht entfällt
                /*if (Supplierungen[i - Anfangssupplierung - 1].Entfällt == true)
                {
                    EntfalleneStunden++;
                    continue;
                }*/

                // Die Tabellendaten mit Text füllen
                Daten_Supplierungen[i - Anfangssupplierung, 0].Text = Supplierungen_Zwischenspeicher[i - 2].Datum.ToString();
                Daten_Supplierungen[i - Anfangssupplierung, 1].Text = Supplierungen_Zwischenspeicher[i - 2].Stunde.ToString();

                // Prüfen, ob die Supplierung zur aktuellen Stunde anfällt
                if (Methoden.AktuelleUnterrichtsStunde() == Supplierungen_Zwischenspeicher[i - Anfangssupplierung - 1].Stunde)
                {
                    ZeileSkinIDZuweisen(ref Daten_Supplierungen, i - Anfangssupplierung, "Lehrerscreen_TabellenCell_AktiveStunde");
                }

                Daten_Supplierungen[i - Anfangssupplierung, 2].Text = Supplierungen_Zwischenspeicher[i - Anfangssupplierung - 1].Ersatzlehrer;
                Daten_Supplierungen[i - Anfangssupplierung, 3].Text = Supplierungen_Zwischenspeicher[i - Anfangssupplierung - 1].Ursprungslehrer;
                Daten_Supplierungen[i - Anfangssupplierung, 4].Text = Supplierungen_Zwischenspeicher[i - Anfangssupplierung - 1].Klasse;
                Daten_Supplierungen[i - Anfangssupplierung, 5].Text = Supplierungen_Zwischenspeicher[i - Anfangssupplierung - 1].Raum;
                Daten_Supplierungen[i - Anfangssupplierung, 6].Text = Supplierungen_Zwischenspeicher[i - Anfangssupplierung - 1].Ersatzfach;

                if (Supplierungen_Zwischenspeicher[i - Anfangssupplierung - 1].ZiehtVor >= 0)
                {
                    Daten_Supplierungen[i - Anfangssupplierung, 7].Text = "aus " + Supplierungen_Zwischenspeicher[i - Anfangssupplierung - 1].ZiehtVor.ToString() + ". Std";
                }
                else
                {
                    if(Supplierungen_Zwischenspeicher[i - Anfangssupplierung - 1].Entfällt)
                        Daten_Supplierungen[i - Anfangssupplierung, 7].Text = "entfällt";
                    else
                    {
                        Daten_Supplierungen[i - Anfangssupplierung, 7].Text = "suppliert";
                    }
                }
            }

            // Die Sprechstunden-Tabelle erzeugen
            Methoden.Tabelle_Erzeugen(ref table_Lehrerscreen, Daten_Supplierungen);

            // Die aktuelle Seite im GUI vermerken
            lb_Fußzeile.Text = "Seite " + (AktuelleSeite + 1) + " von " + AnzSeiten;

            // Prüfen, ob die Seite hren kompletten Content ausgegeben hat
            if (AktuelleSeite == AnzSeiten - 1)
            {
                Response.Cookies["SeitendurchgangBeendet"].Value = "0";
            }
        }

        /// <summary>
        /// Unterprogramm das die SkinID einer Zeile in den gewünschten wert ändert
        /// </summary>
        /// <param name="Tabellendaten">Speicher der Tabellendaten</param>
        /// <param name="Zeile">Zeile welche geändert werden soll</param>
        /// <param name="SkinID">SkinID welche eingesetzt werden soll</param>
        private void ZeileSkinIDZuweisen(ref Structuren.Tabellendaten[,] Tabellendaten, int Zeile, string SkinID)
        {
            for (int Spalte = 0; Spalte < Tabellendaten.GetLength(1); Spalte++)
            {
                Tabellendaten[Zeile, Spalte].SkinID = SkinID;
            }
        }
    }
}