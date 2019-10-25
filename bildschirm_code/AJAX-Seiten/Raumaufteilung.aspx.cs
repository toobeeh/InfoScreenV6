using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Infoscreen_Bibliotheken;

namespace Infoscreen_Bildschirme
{
    public partial class Raumaufteilung : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Variablen anlegen
            string AbteilungsID = Request.QueryString["id"];
            const int Spaltenanzahl = 4;
            const int MaxLehrerProSeite = 15;
            const string Cookiname = "Raumaufteilung_Steitencookie";
            Bildschirm_Methoden Methoden = new Bildschirm_Methoden();
            // Die Informationen über die Sprechtunden aud der Datenbank auf den Server übertragen
            Infoscreen_Verwaltung.classes.Structuren.Raumaufteilung[] Raumaufteilung = Infoscreen_Verwaltung.classes.DatenbankAbrufen.RaumaufteilungAbrufen(AbteilungsID);
            // Einen speicher anlegen, mit dem der Tabellengenerator etwas anfangen kann
            Structuren.Tabellendaten[,] Daten_Raumaufteilung = new Structuren.Tabellendaten[Raumaufteilung.Length + 1, Spaltenanzahl];
            int AnzSeiten;
            // Prüfen, ob die Anzahl der Lehrer ein vielfaches von 15 beträgt
            if (Raumaufteilung.Length / MaxLehrerProSeite != Raumaufteilung.Length / (MaxLehrerProSeite * 1.0))
            {
                AnzSeiten = Raumaufteilung.Length / MaxLehrerProSeite + 1;
            }
            else
            {
                AnzSeiten = Raumaufteilung.Length / MaxLehrerProSeite;
            }
            // Prüfen, ob diese Seite mehr als einen Durchgang braucht.
            if (AnzSeiten > 1)
            {
                Response.Cookies["SeitendurchgangBeendet"].Value = "1";
            }
            // Herrausfinden, wieviele Lehrer pro Seite sinnvollerweise Angezeit werden sollen
            int LehrerProSeite = Raumaufteilung.Length / AnzSeiten;
            // den Tabellen-Datenspeicher mit Informationen füllen (Überschrifften)
            Daten_Raumaufteilung[0, 0].Text = "LEHRER"; Daten_Raumaufteilung[0, 0].SkinID = "Abteilungsscreen_Tabellenheader";
            Daten_Raumaufteilung[0, 1].Text = "RAUM"; Daten_Raumaufteilung[0, 1].SkinID = "Abteilungsscreen_Tabellenheader";
            Daten_Raumaufteilung[0, 2].Text = "KLASSE"; Daten_Raumaufteilung[0, 2].SkinID = "Abteilungsscreen_Tabellenheader";
            Daten_Raumaufteilung[0, 3].Text = "FACH"; Daten_Raumaufteilung[0, 3].SkinID = "Abteilungsscreen_Tabellenheader";
            // den Tabellen-Datenspeicher mit Informationen füllen
            int AktuelleSeite = Methoden.Seitenauswahl(Cookiname, AnzSeiten);
            int AnfangsRaumaufteilung = AktuelleSeite * LehrerProSeite + 1;
            // Korrekturrechnung, falls eine ungerade Zahle an Einträgen (Mehrseitig) ausgegeben werden soll
            int UngeradeAnzahlAusgleicher = 0;
            if (AktuelleSeite == AnzSeiten - 1 && 1.0 * Raumaufteilung.Length / AnzSeiten != LehrerProSeite)
                UngeradeAnzahlAusgleicher = 1;
            for (int i = AnfangsRaumaufteilung + 1; i < LehrerProSeite + AnfangsRaumaufteilung + 1 + UngeradeAnzahlAusgleicher; i++)
            {
                // Die Tabellendaten mit Text füllen
                Daten_Raumaufteilung[i - AnfangsRaumaufteilung, 0].Text = Raumaufteilung[i - 2].Lehrer;
                Daten_Raumaufteilung[i - AnfangsRaumaufteilung, 1].Text = Raumaufteilung[i - 2].Raum;
                Daten_Raumaufteilung[i - AnfangsRaumaufteilung, 2].Text = Raumaufteilung[i - 2].Klasse;
                Daten_Raumaufteilung[i - AnfangsRaumaufteilung, 3].Text = Raumaufteilung[i - 2].Fach;
                // Die Designregel hinzufügen
                for (int j = 0; j < Spaltenanzahl; j++)
                {
                    // Designregel für die Informationszellen
                    Daten_Raumaufteilung[i - AnfangsRaumaufteilung, j].SkinID = "Abteilungsscreen_TabellenCell";
                }
            }
            // Die Sprechstunden-Tabelle erzeugen
            Methoden.Tabelle_Erzeugen(ref table_Raumaufteilung, Daten_Raumaufteilung);
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