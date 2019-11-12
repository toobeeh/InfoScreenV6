using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace Infoscreen_Verwaltung.classes
{
    public class Structuren
    {
        /// <summary>
        /// Eine Struktur zur Speicherung der Daten für eine Stunde.
        /// </summary>
        public struct StundenplanEntry
        {
            public int Stunde;
            public string Lehrer;
            public string Fach;
            public bool Supplierung;
            public int ZiehtVor; // -1, wenn nicht
            public bool Entfällt;
            public string Ersatzlehrer;
            public string Ersatzfach;
            public string Gebäude;
            public int Raum;
            public DateTime ZiehtVorDatum;
        }
        public struct LehrerStundenplanEntry
        {
            public int Stunde;
            public string Klasse;
            public string Fach;
            public string Gebäude;
            public int Raum;
        }

        /// <summary>
        /// Eine Struktur zur Speicherung aller Stunden eines Tages.
        /// </summary>
        public struct StundenplanTag
        {
            public StundenplanEntry[] StundenDaten;
            public DateTime Datum;
        }

        /// <summary>
        /// Eine Struktur zur Speicherung aller Informationen zu einem Raum.
        /// </summary>
        public struct Rauminfo
        {
            public string Raumnummer;
            public string Abteilung;
            public string Stammklasse;
            public string Klassenvorstand;
            public string AktuelleKlasse;
            public string Gebäude;
            public string NichtStören;  // Wenn leerstring, kein Nicht Stören anzeigen
        }

        /// <summary>
        /// Eine Struktur zur Festlegung, was ein Bildschirm anzeigen soll
        /// </summary>
        public struct AnzeigeElemente
        {
            public bool StandardKlasse;
            public bool Stundenplan;
            public bool Abteilungsinfo;
            public bool Sprechstunden;
            public bool Raumaufteilung;
            public bool Supplierplan;
            public bool AktuelleSupplierungen;
            public int PowerPoints;  // Wenn -1, keine PowerPoints anzeigen
        }

        /// <summary>
        /// Eine Struktur zur Speicherung von Sprechstunden und allen zugehörigen Informationen
        /// </summary>
        public struct Sprechstunden
        {
            public string Lehrer;
            public string Raum;
            public int Tag; // Tag als int - Montag = 0, Dienstag = 1, ...
            public int Stunde;
            public string Durchwahl;
        }

        /// <summary>
        /// Eine Struktur zur Speicherung der Raumaufteilung
        /// </summary>
        public struct Raumaufteilung
        {
            public string Lehrer;
            public string Raum;
            public string Klasse;
            public string Fach;
            public bool suppliert;
        }


        /// <summary>
        /// Eine Struktur zur Speicherung von Supplierungen zu einem gewissen Lehrer
        /// !!!ACHTUNG!!! - Das Element Ursprungslehrer der Struktur Supplierungen muss nicht zwingend zugewiesen werden, !!!!!
        /// !!!!!!!!!!!!!   da es gleich dem Lehrer ist!!!                                                                !!!!!
        /// </summary>
        public struct LehrerSupplierungen
        {
            public string Lehrer;
            public Supplierungen[] Supplierungen;
        }

        #region nur Verwaltung

        public struct Tests
        {
            public string Fach;
            public DateTime Datum;
            public int Stunde;
            public int Dauer;
            public string Lehrer;
            public string Testart;
            public string Raum;
        }

        public struct User
        {
            public bool Lehrer;
            public bool Superadmin;
            public string[] Abteilungen;
            public string StammAbteilung;
            public string[] Klassen;
            public string StammKlasse;
        }

        

        public struct Betriebsmodi
        {
            public int id;
            public string bezeichnung;
        }

        public struct Bildschirm
        {
            public int id;
            public string Klasse;
            public string Abteilung;
            public string Gebäude;
            public int Raum;
            public int AnzeigeArt;
        }

        public struct Dateien
        {
            public int id;
            public string bezeichnung;
        }

        public struct Klasseneigenschaften
        {
            public string klasse;
            public int abteilungsID;
            public string gebäude;
            public string raum;
            public string klassensprecher;
            public string klassenvorstand;
            public string klasseninfo;
        }
        #endregion

        //SAIBL
        public struct ZGA
        {
            public int AbteilungsID;
            public int BetriebsmodeID;
            public DateTime Zeit;
        }
        public struct LE
        {
            public string LehrerKuerzel;
            public string Vorname;
            public string Nachname;
            public int Sprechstunde;
            public int Tag;
            public int Raum;
            public string Gebäude;
            public List<string> KlassenvorstandKlasse;
            public List<int> AbteilungsIDs;
        }

        //Schönegger
        public struct GlobalEntfall
        {
            public int AbteilungsID;
            public DateTime Datum;
            public int VonStunde;
            public int BisStunde;
            public int GlobalerEntfallID;
        }
        public struct StundenplanTagLehrer
        {
            public string Klasse;
            public int Stunde;
            public string Fach;
            public string Lehrer;
        }
        public struct LEmitSprechstundenDaten
        {
            public string LehrerKuerzel;
            public string Vorname;
            public string Nachname;
            public List<string> KlassenvorstandKlassen;
            public string Gebäude;
            public string Raum;
            public string Durchwahl;
            public string Sprechstunde;
            public string Tag;
            public string Abteilungen;
        }
        /// <summary>
        /// Eine Struktur zur Speicherung von Supplierungen
        /// </summary>
        public struct Supplierungen
        {
            public DateTime Datum;
            public int Stunde;
            public string Klasse;
            public string Raum;
            public string Ursprungslehrer;
            public string Ersatzlehrer; // NICHT definiert, wenn Entfällt = true
            public string Ersatzfach; // wenn Entfällt = true - Text der angezeigt wird z.B. "still beschäftigen"
            public bool Entfällt; // wenn true - Ersatzfach = Text der angezeigt wird z.B. "still beschäftigen"
            public int ZiehtVor; // Stunde aus der vorgezogen wird :: -1 wenn nicht vorgezogen wird // NICHT definiert, wenn Entfällt = true
            public DateTime ZiehtVorDatum; //Datum von dem Stunde vorgezogen wird
            public string Grund;
        }
    }
}