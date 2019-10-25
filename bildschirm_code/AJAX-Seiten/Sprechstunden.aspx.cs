using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using Infoscreen_Bibliotheken;

namespace Infoscreen_Bildschirme
{
    public partial class Sprechstunden : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Variablen anlegen
            string AbteilungsID = Request.QueryString["id"];
            const int Spaltenanzahl = 5;
            const int MaxLehrerProSeite = 15;
            const string Cookiname = "Sprechstunden_Steitencookie";
            Bildschirm_Methoden Methoden = new Bildschirm_Methoden();
            // Die Informationen über die Sprechtunden aud der Datenbank auf den Server übertragen
            Structuren.Sprechstunden[] Sprechstunden = FachHelper.SprechstundenAbrufen(AbteilungsID);
            // Einen speicher anlegen, mit dem der Tabellengenerator etwas anfangen kann
            Structuren.Tabellendaten[,] Daten_Sprechstunden = new Structuren.Tabellendaten[Sprechstunden.Length + 1, Spaltenanzahl];
            int AnzSeiten;
            // Prüfen, ob die Anzahl der Lehrer ein vielfaches von 15 beträgt
            if (Sprechstunden.Length / MaxLehrerProSeite != Sprechstunden.Length / (MaxLehrerProSeite * 1.0))
            {
                AnzSeiten = Sprechstunden.Length / MaxLehrerProSeite + 1;
            }
            else
            {
                AnzSeiten = Sprechstunden.Length / MaxLehrerProSeite;
            }
            // Prüfen, ob diese Seite mehr als einen Durchgang braucht.
            if (AnzSeiten > 1)
            {
                Response.Cookies["SeitendurchgangBeendet"].Value = "1";
            }
            // Herrausfinden, wieviele Lehrer pro Seite sinnvollerweise Angezeit werden sollen
            int AktuelleSeite = Methoden.Seitenauswahl(Cookiname, AnzSeiten);
            int LehrerProSeite; //= Sprechstunden.Length / AnzSeiten;
            if (AktuelleSeite != AnzSeiten - 1) LehrerProSeite = MaxLehrerProSeite;
            else LehrerProSeite = Sprechstunden.Length - ((AnzSeiten - 1) * MaxLehrerProSeite);
            // den Tabellen-Datenspeicher mit Informationen füllen (Überschrifften)
            Daten_Sprechstunden[0, 0].Text = "LEHRER"; Daten_Sprechstunden[0, 0].SkinID = "Abteilungsscreen_Tabellenheader";
            Daten_Sprechstunden[0, 1].Text = "RAUM"; Daten_Sprechstunden[0, 1].SkinID = "Abteilungsscreen_Tabellenheader";
            Daten_Sprechstunden[0, 2].Text = "TAG"; Daten_Sprechstunden[0, 2].SkinID = "Abteilungsscreen_Tabellenheader";
            Daten_Sprechstunden[0, 3].Text = "STUNDE"; Daten_Sprechstunden[0, 3].SkinID = "Abteilungsscreen_Tabellenheader";
            Daten_Sprechstunden[0, 4].Text = "UHRZEIT"; Daten_Sprechstunden[0, 4].SkinID = "Abteilungsscreen_Tabellenheader";
            // den Tabellen-Datenspeicher mit Informationen füllen (Sprechstunden)
            int Anfangssprechstunde = AktuelleSeite * LehrerProSeite + 1;
            // Korrekturrechnung, falls eine ungerade Zahle an Einträgen (Mehrseitig) ausgegeben werden soll
            int UngeradeAnzahlAusgleicher = 0;
            if (AktuelleSeite == AnzSeiten - 1 && 1.0 * Sprechstunden.Length / AnzSeiten != LehrerProSeite)
                UngeradeAnzahlAusgleicher = 1;
            for (int i = Anfangssprechstunde + 1; i < LehrerProSeite + Anfangssprechstunde + 1 + UngeradeAnzahlAusgleicher; i++)
            {
                // Die Tabellendaten mit Text füllen
                Daten_Sprechstunden[i - Anfangssprechstunde, 0].Text = Sprechstunden[i - 2].Lehrer;
                Daten_Sprechstunden[i - Anfangssprechstunde, 1].Text = Sprechstunden[i - 2].Raum;
                Daten_Sprechstunden[i - Anfangssprechstunde, 2].Text = Methoden.Wochentag(Sprechstunden[i - 2].Tag);
                Daten_Sprechstunden[i - Anfangssprechstunde, 3].Text = Sprechstunden[i - 2].Stunde.ToString();
                Daten_Sprechstunden[i - Anfangssprechstunde, 4].Text = Methoden.Unterrichtsuhrzeit(Sprechstunden[i - 2].Stunde).Replace("<br />", " - ");
                // Die Designregel hinzufügen
                for (int j = 0; j < Spaltenanzahl; j++)
                {
                    // Designregel für die Informationszellen
                    Daten_Sprechstunden[i - Anfangssprechstunde, j].SkinID = "Abteilungsscreen_SprechstundenCell";
                }
            }
            // Die Sprechstunden-Tabelle erzeugen
            Methoden.Tabelle_Erzeugen(ref table_Sprechstunden, Daten_Sprechstunden);
            // Die aktuelle Seite im GUI vermerken
            lb_Fußzeile.Text = "Seite " + (AktuelleSeite + 1) + " von " + AnzSeiten;
            // Prüfen, ob die Seite hren kompletten Content ausgegeben hat
            if (AktuelleSeite == AnzSeiten - 1)
            {
                Response.Cookies["SeitendurchgangBeendet"].Value = "0";
            }
        }
    }
}