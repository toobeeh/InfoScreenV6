using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

namespace Infoscreen_Verwaltung.classes
{
    public class DatenbankAbrufen
    {
        /// <summary>
        /// Führt den über _Befehl übergebenen string Befehl in der Datenbank aus und gibt das Ergebnis als DataTable zurück.
        /// </summary>
        /// <param name="_Befehl">Der in der Datenbank auszuführende Befehl</param>
        /// <returns>Das ergebnis der Datenbankabfrage</returns>

        static string conn_string = @"  data source=ELV-SCREEN-2\INFOSCREEN;
                                        initial catalog=Infoscreen_1.0;
                                        persist security info=False;
                                        user id=SQL_infoscreen;
                                        password=*x-?password-/secure80):passWoRt*;
                                        MultipleActiveResultSets=True;
                                        App=EntityFramework&quot;";


        // Open and connection and initialize command and adapter, exec query, close all
        // Preferred against keeping conn alive due to previous errors and because low frequency of SQL requests
        static public DataTable DatenbankAbfrage(string _command)
        {
            SqlConnection connection = new SqlConnection(conn_string);
            SqlCommand command = new SqlCommand(_command, connection);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable result = new DataTable();

            connection.Open();
            adapter.Fill(result);

            adapter.Dispose();
            command.Dispose();
            connection.Close();

            return result;

        }
       
        /// <summary> 
        /// Gibt die anzuzeigenden Elemente des durch die übergebene ID bestimmten Bildschirmes als Structuren.AnzeigeElemente zurück.
        /// </summary>
        /// <param name="_BildschirmID">Die ID des Bildschirmes</param>
        /// <param name="_BetriebsmodeID">Die Betreibsmode ID von welcher die Anzuzeigenden Elemente abgerufen werden sollen.</param>
        /// <returns>Anzuzeigende Elemente auf dem Bildschirm</returns>
        static public Structuren.AnzeigeElemente AnzuzeigendeElemente(string _BildschirmID, int _BetriebsmodeID = -1)
        {
            string befehl;
            if (_BetriebsmodeID < 0)
                befehl = @"SELECT
[Betriebsanzeige].[DateiID] AS PowerPoint,
[Betriebsanzeige].[Stundenplan] AS Stundenplan,
[Betriebsanzeige].[Abteilungsinfo] AS Abteilungsinfo,
[Betriebsanzeige].[Sprechstunden] AS Sprechstunden,
[Betriebsanzeige].[Raumaufteilung] AS Raumaufteilung,
[Betriebsanzeige].[Supplierplan] AS Supplierplan,
[Betriebsanzeige].[AktuelleSupplierungen] AS AktuelleSupplierungen
FROM (([Bildschirme] INNER JOIN [Abteilungen] ON [Bildschirme].[AbteilungsID]=[Abteilungen].[AbteilungsID])
INNER JOIN [Betriebsanzeige] ON [Abteilungen].[StandardBetriebsmode]=[Betriebsanzeige].[BetriebsmodeID])
WHERE [Betriebsanzeige].[BildschirmID]='" + _BildschirmID + "'";

            else
                befehl = @"SELECT
[Betriebsanzeige].[DateiID] AS PowerPoint,
[Betriebsanzeige].[Stundenplan] AS Stundenplan,
[Betriebsanzeige].[Abteilungsinfo] AS Abteilungsinfo,
[Betriebsanzeige].[Sprechstunden] AS Sprechstunden,
[Betriebsanzeige].[Raumaufteilung] AS Raumaufteilung,
[Betriebsanzeige].[Supplierplan] AS Supplierplan,
[Betriebsanzeige].[AktuelleSupplierungen] AS AktuelleSupplierungen
FROM [Betriebsanzeige]
WHERE [Betriebsanzeige].[BildschirmID]='" + _BildschirmID + @"'
AND [Betriebsanzeige].[BetriebsmodeID]='" + _BetriebsmodeID.ToString() + "'";

            DataTable daten = DatenbankAbfrage(befehl);
            Structuren.AnzeigeElemente temp = new Structuren.AnzeigeElemente();
            if (daten.Rows.Count == 0)
            {
                daten.Dispose();
                temp.Abteilungsinfo = false;
                temp.AktuelleSupplierungen = false;
                temp.Raumaufteilung = false;
                temp.Sprechstunden = false;
                temp.Stundenplan = false;
                temp.Supplierplan = false;
                temp.PowerPoints = -1;
                return temp;
            }

            temp.Abteilungsinfo = Convert.ToBoolean(daten.Rows[0]["Abteilungsinfo"]);
            temp.AktuelleSupplierungen = Convert.ToBoolean(daten.Rows[0]["AktuelleSupplierungen"]);
            Structuren.Raumaufteilung[] temp2 = RaumaufteilungAbrufen(_BildschirmID);
            /*if (temp2.Length > 0)*/ temp.Raumaufteilung = Convert.ToBoolean(daten.Rows[0]["Raumaufteilung"]);
            //else temp.Raumaufteilung = false; ISv6: WTF is that?!
            temp.Sprechstunden = Convert.ToBoolean(daten.Rows[0]["Sprechstunden"]);
            temp.Stundenplan = Convert.ToBoolean(daten.Rows[0]["Stundenplan"]);
            temp.Supplierplan = Convert.ToBoolean(daten.Rows[0]["Supplierplan"]);
            temp.PowerPoints = daten.Rows[0]["PowerPoint"].ToInt32();
            daten.Dispose();
            return temp;

        }

        /// <summary>
        /// Gibt den Stundenplan der übergebenen Klasse als Structuren.StundenplanTag Array zurück.
        /// Wobei jedes Element dieses Arrays für einen Wochentag gilt, beginnend bei 0 für Montag.
        /// </summary>
        /// <param name="_Klasse">Die Klasse als STRING, von welcher der Stundenplan zurückgegeben werden soll.</param>
        /// <param name="_LehrerAusschreiben">Gibt an, ob die Lehrernamen oder die Lehrerkürzel zurückgegeben werden sollen.</param>
        /// <returns>Stundenplan der gesammten Woche</returns>
        static public Structuren.StundenplanTag[] StundenplanAbrufen(string _Klasse, bool _LehrerAusschreiben = true, bool _FachAusschreiben = false)
        {
            string befehl = @"SELECT
[Abteilungen].[Samstag] AS Samstag
FROM
([Stundenplan] INNER JOIN [Abteilungen] ON  [Stundenplan].[AbteilungsID]=[Abteilungen].[AbteilungsID])
WHERE [Stundenplan].[Klasse]='" + _Klasse + "'";
            DataTable daten = DatenbankAbfrage(befehl);
            if (daten.Rows.Count == 0) return new Structuren.StundenplanTag[0];
            int schultage = Convert.ToBoolean(daten.Rows[0][0]) ? 6 : 5;
            daten.Dispose();

            befehl = @"SELECT
[Stundenplan].[LehrerKürzel] AS LehrerKürzel,
[Fächer].[FachKürzel] AS FachKürzel,
[Stundenplan].[Wochentag] AS Tag,
[Stundenplan].[Stunde] AS Stunde,
CASE WHEN [Stundenplan].[Standardraum] = 1 THEN '' ELSE [Stundenplan].[Gebäude] END AS Gebäude,
CASE WHEN [Stundenplan].[Standardraum] = 1 THEN '' ELSE [Stundenplan].[Raum] END AS Raum
FROM
([Stundenplan] LEFT JOIN [Fächer] ON [Stundenplan].[FachKürzel]=[Fächer].[FachKürzel])
WHERE [Stundenplan].[Klasse]='" + _Klasse + @"'
ORDER BY [Stundenplan].[Wochentag], [Stundenplan].[Stunde]";
            daten = DatenbankAbfrage(befehl);

            Structuren.StundenplanTag[] data = new Structuren.StundenplanTag[schultage];
            List<Structuren.StundenplanEntry> temp, dummy0;
            Structuren.StundenplanEntry temp2, dummy1;
            Structuren.StundenplanEntry[] temp3;
            
            bool normaleStunde = false;
            int i2 = 0;
            int i3 = 0;
            for (int i1 = 0; i1 < data.Length; i1++) 
            {
                temp = new List<Structuren.StundenplanEntry>();
                while (i2 < daten.Rows.Count) //Stunden eines Tages
                {
                    if (Convert.ToInt32(daten.Rows[i2]["Tag"]) > i1) break;
                    temp2 = new Structuren.StundenplanEntry();
                    temp2.Stunde = Convert.ToInt32(daten.Rows[i2]["Stunde"]);
                    if (_LehrerAusschreiben) temp2.Lehrer = LehrerNamen(daten.Rows[i2]["LehrerKürzel"].ToString());
                    else temp2.Lehrer = daten.Rows[i2]["LehrerKürzel"].ToString();
                    if (_FachAusschreiben) temp2.Fach = FachLang(daten.Rows[i2]["FachKürzel"].ToString());
                    else temp2.Fach = daten.Rows[i2]["FachKürzel"].ToString();
                    temp2.Gebäude = daten.Rows[i2]["Gebäude"].ToString();
                    temp2.Raum = daten.Rows[i2]["Raum"].ToInt32();
                    temp2.Supplierung = false;
                    temp2.ZiehtVor = -1;
                    temp2.Entfällt = false;
                    temp2.Ersatzlehrer = "";
                    temp2.Ersatzfach = "";
                    temp2.ZiehtVorDatum = "".ToDateTime();
                    temp.Add(temp2);
                    i2++;
                }

                temp3 = temp.ToArray();
                DateTime aktuell = DateTime.Today;
                int anzahl = ((i1 + 1) - (int)aktuell.DayOfWeek + 7) % 7;
                DateTime nächstesDatum = aktuell.AddDays(anzahl);
                data[i1].Datum = nächstesDatum;

                befehl = @"SELECT
[Supplierungen].[Stunde] AS Stunde,
[Fächer].[FachKürzel] AS FachKürzel,
[Supplierungen].[ErsatzLehrerKürzel] AS LehrerKürzel,
[Supplierungen].[Entfällt] AS Entfällt,
[Supplierungen].[ZiehtVor] AS ZiehtVor,
[Supplierungen].[ZiehtVorDatum] AS ZiehtVorDatum
FROM
([Supplierungen] LEFT JOIN [Fächer] ON [Supplierungen].[ErsatzFach]=[Fächer].[FachKürzel])
WHERE [Supplierungen].[Klasse]='" + _Klasse + @"'
AND [Supplierungen].[Datum]='" + nächstesDatum.ToString(Properties.Resources.sql_datumformat) + "'";
                DataTable daten2 = DatenbankAbfrage(befehl);

                for (i3 = 0; i3 < daten2.Rows.Count; i3++) //Supplier Eintrag
                {
                    int x = 0;
                    int temp4 = Convert.ToInt32(daten2.Rows[i3]["Stunde"]);
                    int temp5 = Convert.ToInt32(daten2.Rows[i3]["ZiehtVor"]);
                    for (int i4 = 0; i4 < temp3.Length; i4++) //Stundenplan Eintrag
                    {
                        if (temp3[i4].Stunde == temp4)
                        {
                            x = i4;
                            normaleStunde = true;
                            break;
                        }
                    }
                    if (normaleStunde)
                    {
                        temp3[x].ZiehtVorDatum = Convert.ToDateTime(daten2.Rows[i3]["ZiehtVorDatum"]);
                        temp3[x].ZiehtVor = Convert.ToInt32(daten2.Rows[i3]["ZiehtVor"]);
                        temp3[x].Entfällt = Convert.ToBoolean(daten2.Rows[i3]["Entfällt"]);
                        if (!(temp3[x].ZiehtVor > -1 || temp3[x].Entfällt)) //wenn NICHT ziehtVor oder Entfall
                        {
                            temp3[x].Supplierung = true;
                        }
                        if (_LehrerAusschreiben) temp3[x].Ersatzlehrer = LehrerNamen(daten2.Rows[i3]["LehrerKürzel"].ToString());
                        else temp3[x].Ersatzlehrer = daten2.Rows[i3]["LehrerKürzel"].ToString();
                        if (_FachAusschreiben) temp3[x].Ersatzfach = FachLang(daten2.Rows[i3]["FachKürzel"].ToString());
                        else temp3[x].Ersatzfach = daten2.Rows[i3]["FachKürzel"].ToString();
                        normaleStunde = false;
                    }
                    else
                    {
                        dummy1 = new Structuren.StundenplanEntry();
                        dummy1.Stunde = Convert.ToInt32(daten2.Rows[i3]["Stunde"]);
                        dummy1.Lehrer = "";
                        dummy1.Fach = "";
                        dummy1.Gebäude = "";
                        dummy1.Raum = 307;
                        dummy1.Supplierung = false;
                        dummy1.ZiehtVorDatum = Convert.ToDateTime(daten2.Rows[i3]["ZiehtVorDatum"]);
                        dummy1.ZiehtVor = Convert.ToInt32(daten2.Rows[i3]["ZiehtVor"]);
                        dummy1.Entfällt = Convert.ToBoolean(daten2.Rows[i3]["Entfällt"]);
                        if (!(dummy1.ZiehtVor > -1 || dummy1.Entfällt)) //wenn NICHT ziehtVor oder Entfall
                        {
                            dummy1.Supplierung = true;
                        }
                        if (_LehrerAusschreiben) dummy1.Ersatzlehrer = LehrerNamen(daten2.Rows[i3]["LehrerKürzel"].ToString());
                        else dummy1.Ersatzlehrer = daten2.Rows[i3]["LehrerKürzel"].ToString();
                        if (_FachAusschreiben) dummy1.Ersatzfach = FachLang(daten2.Rows[i3]["FachKürzel"].ToString());
                        else dummy1.Ersatzfach = daten2.Rows[i3]["FachKürzel"].ToString();

                        dummy0 = new List<Structuren.StundenplanEntry>();
                        dummy0.AddRange(temp3);
                        dummy0.Add(dummy1);
                        temp3 = dummy0.ToArray();
                    }
                }

                data[i1].StundenDaten = temp3;
                daten2.Dispose();
            }
            daten.Dispose();
            return data;
        }

        /// <summary>
        /// Gibt die Klasseninfo der angegebenen Klasse als String zurück.
        /// Der Rückgabewert enthält keinerlei Formatierungsinformationen.
        /// </summary>
        /// <param name="_Klasse">Die Klasse als STRING, von welcher die Klasseninfo zurückgegeben werden soll.</param>
        /// <returns>Klasseninfo der übergebenen Klasse</returns>
        static public string KlasseninfoAbrufen(string _Klasse)
        {
            string befehl = @"SELECT
[Klassen].[Klasseninfo] AS Klasseninfo
FROM [Klassen]
WHERE [Klassen].[Klasse]='" + _Klasse + "'";
            DataTable daten = DatenbankAbfrage(befehl);
            if (daten.Rows.Count == 0) return "Derzeit keine Info";
            return daten.Rows[0][0].ToString();
        }

        /// <summary>
        /// Gibt ein Structuren.Rauminfo zum aus der übergebenen ID bezogenen Raum zurück.
        /// </summary>
        /// <param name="_ID">Die ID des Bildschirmes aus welcher der Raum referenziert wird</param>
        /// <returns>Rauminfo des referenzierten Raumes</returns>
        static public Structuren.Rauminfo RauminfoAbrufen(string _ID)
        {
            string befehl = @"SELECT
[Raeume].[StandardKlasse] AS Stammlasse,
[Raeume].[Raum] AS Raumnummer,
[Raeume].[Gebäude] AS Gebäude,
[Abteilungen].[Abteilungsname] AS Abteilung
FROM (([Bildschirme] INNER JOIN [Abteilungen] ON [Bildschirme].[AbteilungsID]=[Abteilungen].[AbteilungsID])
INNER JOIN [Raeume] ON [Bildschirme].[Raum]=[Raeume].[Raum] AND [Bildschirme].[AbteilungsID]=[Raeume].[AbteilungsID])
WHERE [Bildschirme].[BildschirmID]='" + _ID + "'";
            DataTable daten = DatenbankAbfrage(befehl);
            if (daten.Rows.Count == 0) return new Structuren.Rauminfo();

            Structuren.Rauminfo temp = new Structuren.Rauminfo();
            temp.Stammklasse = daten.Rows[0]["Stammlasse"].ToString();
            temp.Abteilung = daten.Rows[0]["Abteilung"].ToString();
            temp.Raumnummer = daten.Rows[0]["Raumnummer"].ToString();
            temp.Gebäude = daten.Rows[0]["Gebäude"].ToString();
            daten.Dispose();

            befehl = @"SELECT
";
            temp.AktuelleKlasse = "";
            temp.Klassenvorstand = "";
            temp.NichtStören = "";
            return temp;
        }

        /// <summary>
        /// Gibt die Abteilungsinfo aufgrund der durch die BildschirmID referenzierten Abteilung als BB-Code zurück.
        /// </summary>
        /// <param name="_ID">Die ID des Bildschirmes aus welcher die Abteilung referenziert wird</param>
        /// <returns>Die referenzierte Abteilungsinfo formatiert als BB-Code</returns>
        static public string AbteilungsinfoAbrufen(string _ID)
        {
            string befehl;
            if (_ID.ToInt32() == -1)
            {
                befehl = @"SELECT
[Abteilungen].[AbteilungsInfo] AS Abteilungsinfo
FROM [Abteilungen]
WHERE [Abteilungen].[Abteilungsname]='" + _ID + "'";
            }
            else
            {
                befehl = @"SELECT
[Abteilungen].[AbteilungsInfo] AS Abteilungsinfo
FROM ([Bildschirme] INNER JOIN [Abteilungen] ON [Bildschirme].[AbteilungsID]=[Abteilungen].[AbteilungsID])
WHERE [Bildschirme].[BildschirmID]='" + _ID + "'";
            }
            DataTable daten = DatenbankAbfrage(befehl);
            if (daten.Rows.Count == 0) return "Derzeit keine Info";
            return daten.Rows[0][0].ToString();
        }

        /// <summary>
        /// Gibt die Sprechstunden aller Lehrer einer Abteilung als Structuren.Sprechstunden Array zurück.
        /// </summary>
        /// <param name="_ID">Die ID aus welcher die Abteilung referenziert wird, von der die Sprechstunden angezeigt werden sollen</param>
        /// <returns>Die Sprechstunden der referenzierten Abteilung</returns>
        static public Structuren.Sprechstunden[] SprechstundenAbrufen(string _ID)
        {
            string befehl;
            DataTable daten;
            string Abteilung;
            if (_ID.ToInt32() != -1)
            {
                befehl = @"SELECT
[Abteilungen].[Abteilungsname] AS Abteilung
FROM ([Bildschirme] INNER JOIN [Abteilungen] ON [Bildschirme].[AbteilungsID]=[Abteilungen].[AbteilungsID])
WHERE [Bildschirme].[BildschirmID]='" + _ID + "'";
                daten = DatenbankAbfrage(befehl);
                if (daten.Rows.Count == 0) return new Structuren.Sprechstunden[0];
                Abteilung = daten.Rows[0][0].ToString();
                daten.Dispose();
            }
            else
            {
                Abteilung = _ID;
            }

            befehl = @"SELECT
[LehrerTesten1].[LehrerKuerzel] AS LahrerKürzel,
[LehrerTesten1].[Gebäude] AS Gebäude,
[LehrerTesten1].[Raum] AS Raum,
[LehrerTesten1].[Durchwahl] AS Durchwahl,
[LehrerTesten1].[Sprechstunde] AS Stunde,
[LehrerTesten1].[Tag] AS Tag
FROM [LehrerTesten1]
WHERE [LehrerTesten1].[Abteilungen] LIKE '%" + Abteilung + @"%'
ORDER BY [LehrerTesten1].[Nachname]";
            daten = DatenbankAbfrage(befehl);
            if (daten.Rows.Count == 0) return new Structuren.Sprechstunden[0];

            Structuren.Sprechstunden[] temp = new Structuren.Sprechstunden[daten.Rows.Count];
            for (int i = 0; i < temp.Length; i++)
            {
                temp[i].Lehrer = LehrerNamen(daten.Rows[i]["LahrerKürzel"].ToString());

                if (daten.Rows[i]["Gebäude"].ToString() != "" && daten.Rows[i]["Raum"].ToString() != "")
                    temp[i].Raum = daten.Rows[i]["Gebäude"].ToString() + "-" + daten.Rows[i]["Raum"];
                else if (daten.Rows[i]["Gebäude"].ToString() != "")
                    temp[i].Raum = daten.Rows[i]["Gebäude"].ToString();
                else
                    temp[i].Raum = "";

                if (daten.Rows[i]["Tag"].ToString() != "")
                    temp[i].Tag = Convert.ToInt32(daten.Rows[i]["Tag"]);
                else
                    temp[i].Tag = -1;

                if (daten.Rows[i]["Stunde"].ToString() != "")
                    temp[i].Stunde = Convert.ToInt32(daten.Rows[i]["Stunde"]);
                else
                    temp[i].Stunde = -1;

                temp[i].Durchwahl = daten.Rows[i]["Durchwahl"].ToString();
            }

            return temp;
        }

        /// <summary>
        /// Gibt die aktuelle Raumaufteilung der Abteilung referenziert aus der übergebenen ID als Structuren.Raumaufteilung Array zurück.
        /// </summary>
        /// <param name="_ID">Die ID des Bildschirmes aus welcher die Abteilung refferenziert wird</param>
        /// <returns>Die aktuelle Raumaufteilung der referenzierten Abteilung</returns>
        static public Structuren.Raumaufteilung[] RaumaufteilungAbrufen(string _ID)
        {
            string befehl = @"SELECT
[Stundenplan].[LehrerKürzel] AS LehrerKürzel,
CASE WHEN [Stundenplan].[Standardraum] = 1 THEN [Raeume].[Gebäude] ELSE [Stundenplan].[Gebäude] END AS Gebäude,
CASE WHEN [Stundenplan].[Standardraum] = 1 THEN [Raeume].[Raum] ELSE [Stundenplan].[Raum] END AS Raum,
[Stundenplan].[Klasse] AS Klasse,
[Fächer].[Fach] AS Fach
FROM ((([Bildschirme] INNER JOIN [Stundenplan] ON [Bildschirme].[AbteilungsID]=[Stundenplan].[AbteilungsID])
INNER JOIN [Raeume] ON [Stundenplan].[Klasse]=[Raeume].[StandardKlasse])
LEFT JOIN [Fächer] ON [Stundenplan].[FachKürzel]=[Fächer].[FachKürzel])
WHERE [Bildschirme].[BildschirmID]='" + _ID + @"'
AND [Stundenplan].[Wochentag]='" + AktuellerTag() + @"'
AND [Stundenplan].[Stunde]='" + AktuelleStunde() + "'";
            DataTable daten = DatenbankAbfrage(befehl);
            if (daten.Rows.Count == 0) return new Structuren.Raumaufteilung[0];

            List<Structuren.Raumaufteilung> temp = new List<Structuren.Raumaufteilung>();

            for (int i = 0; i < daten.Rows.Count; i++)
            {
                befehl = @"SELECT
[Fächer].[Fach] AS Fach,
[Supplierungen].[ErsatzLehrerKürzel] AS LehrerKürzel,
[Supplierungen].[Entfällt] AS Entfällt,
[Supplierungen].[ZiehtVor] AS ZiehtVor
FROM
([Supplierungen] LEFT JOIN [Fächer] ON [Supplierungen].[ErsatzFach]=[Fächer].[FachKürzel])
WHERE [Supplierungen].[Klasse]='" + daten.Rows[i]["Klasse"].ToString() + @"'
AND [Supplierungen].[Datum]='" + DateTime.Today.ToString(Properties.Resources.sql_datumformat) + @"'
AND (
[Supplierungen].[Stunde]='" + AktuelleStunde() + @"'
OR [Supplierungen].[ZiehtVor]='" + AktuelleStunde() + @"'
)";
                DataTable daten2 = DatenbankAbfrage(befehl);
                Structuren.Raumaufteilung temp2 = new Structuren.Raumaufteilung();
                string raum = "";

                if (daten.Rows[i]["Gebäude"].ToString() != "" && daten.Rows[i]["Raum"].ToString() != "") raum = daten.Rows[i]["Gebäude"].ToString() + "-" + ((int)daten.Rows[i]["Raum"]).ToString("000");
                else if (daten.Rows[i]["Gebäude"].ToString() != "") raum = daten.Rows[i]["Gebäude"].ToString();
                else if (daten.Rows[i]["Raum"].ToString() != "") raum = ((int)daten.Rows[i]["Raum"]).ToString("000");

                if (daten2.Rows.Count > 0)
                {
                    if (Convert.ToBoolean(daten2.Rows[0]["Entfällt"])) continue;
                    if (Convert.ToInt32(daten2.Rows[0]["ZiehtVor"]) == AktuelleStunde()) continue;

                    temp2.Raum = raum;
                    temp2.Lehrer = LehrerNamen(daten2.Rows[0]["LehrerKürzel"].ToString());
                    temp2.Klasse = daten.Rows[i]["Klasse"].ToString();
                    temp2.Fach = daten2.Rows[0]["Fach"].ToString();
                    temp2.suppliert = true;
                    temp.Add(temp2);
                }
                else
                {
                    temp2.Raum = raum;
                    temp2.Lehrer = LehrerNamen(daten.Rows[i]["LehrerKürzel"].ToString());
                    temp2.Klasse = daten.Rows[i]["Klasse"].ToString();
                    temp2.Fach = daten.Rows[i]["Fach"].ToString();
                    temp2.suppliert = false;
                    temp.Add(temp2);
                }
            }

            return temp.ToArray();
        }

        /// <summary>
        /// Gibt die Supplierungen einer Abteilung welche aus der übergebenen Bildschirm-ID referenziert werden als Structuren.LehrerSupplierungen Array zurück.
        /// !!!ACHTUNG!!! - Das Element Ursprungslehrer der Structur Supplierungen muss nicht zwingend zugewiesen werden, !!!!!
        /// !!!!!!!!!!!!!   da es gleich dem Lehrer ist!!!                                                                !!!!!
        /// </summary>
        /// <param name="_ID">Die ID des Bildschirmes aus welcher die Abteilung referenziert wird</param>
        /// <returns>Die Supplierungen der referenzierten Abteilung</returns>
        static public Structuren.LehrerSupplierungen[] SupplierplanAbrufen(string _ID, bool _RichtigerName = true, string _LehrerKuerzel = "", string _Klasse = "", bool _Fachausschreiben = true, string _Sortieren = "Lehrer")
        {
            string dbOrdnen;

            if (_Sortieren == "Lehrer")
            {
                dbOrdnen = "ORDER BY [Supplierungen].[StattLehrerKürzel], [Supplierungen].[Datum], [Supplierungen].[Stunde], [Supplierungen].[Klasse]";
            }
            else
            {
                if (_Sortieren == "Klasse")
                {
                    dbOrdnen = "ORDER BY [Supplierungen].[Klasse], [Supplierungen].[Datum], [Supplierungen].[Stunde], [Supplierungen].[ErsatzLehrerKürzel]";
                }
                else
                {
                    if (_Sortieren == "Datum")
                    {
                        dbOrdnen = "ORDER BY [Supplierungen].[Datum], [Supplierungen].[Stunde], [Supplierungen].[Klasse],  [Supplierungen].[ErsatzLehrerKürzel]";
                    }
                    else
                    {
                        dbOrdnen = "ORDER BY [Supplierungen].[ErsatzLehrerKürzel], [Supplierungen].[Datum], [Supplierungen].[Stunde], [Supplierungen].[Klasse]";
                    }
                }
            }

            if (_Sortieren == "")
            {
                dbOrdnen = "ORDER BY [Supplierungen].[Datum], [Supplierungen].[Stunde], [Supplierungen].[Klasse],  [Supplierungen].[StattLehrerKürzel]";
            }

            string befehl="";
            if (_ID.ToInt32() == -1)
            {
                if (_LehrerKuerzel == "" && _Klasse == "") //alle Einträge werden abgerufen
                {
                        befehl = @"SELECT DISTINCT
[Supplierungen].[ErsatzFach] AS ErsatzFach,
[Supplierungen].[StattLehrerKürzel] AS StattLehrerKürzel,
[Supplierungen].[ErsatzLehrerKürzel] AS ErsatzLehrerKürzel,
[Supplierungen].[Entfällt] AS Entfällt,
[Supplierungen].[ZiehtVor] AS ZiehtVor,
[Supplierungen].[ZiehtVorDatum] AS ZiehtVorDatum,
[Supplierungen].[Datum] AS Datum,
[Supplierungen].[Klasse] AS Klasse,
[Supplierungen].[Stunde] AS Stunde,
[Supplierungen].[Grund] AS Grund,
CASE WHEN [Stundenplan].[Standardraum] = 1 THEN [Raeume].[Gebäude] ELSE [Stundenplan].[Gebäude] END AS Gebäude,
CASE WHEN [Stundenplan].[Standardraum] = 1 THEN [Raeume].[Raum] ELSE [Stundenplan].[Raum] END AS Raum,
CASE WHEN [Fächer].[Fach] IS NULL THEN [Supplierungen].[ErsatzFach] ELSE [Fächer].[Fach] END AS Fach
FROM
(((([Abteilungen] INNER JOIN [Supplierungen] ON [Abteilungen].[AbteilungsID]=[Supplierungen].[AbteilungsID])
INNER JOIN [Stundenplan] ON [Supplierungen].[Klasse]=[Stundenplan].[Klasse])
INNER JOIN [Raeume] ON [Supplierungen].[Klasse]=[Raeume].[StandardKlasse])
LEFT JOIN [Fächer] ON [Supplierungen].[ErsatzFach]=[Fächer].[FachKürzel])
WHERE (([Supplierungen].[Datum]>='" + DateTime.Today.ToString(Properties.Resources.sql_datumformat) + @"' OR [Supplierungen].[ZiehtVorDatum]>='" + DateTime.Today.ToString(Properties.Resources.sql_datumformat) + @"')
AND [Abteilungen].[Abteilungsname]='" + _ID + @"')
AND [Supplierungen].[GlobalEntfall] = '-1'" + dbOrdnen;
                }
                else
                {
                    if (_Klasse == "" && _LehrerKuerzel != "") //Einträge für bestimmten Lehrer (LehrerKuerzel) werden abgerufen
                    {
                        befehl = @"SELECT DISTINCT
[Supplierungen].[ErsatzFach] AS ErsatzFach,
[Supplierungen].[StattLehrerKürzel] AS StattLehrerKürzel,
[Supplierungen].[ErsatzLehrerKürzel] AS ErsatzLehrerKürzel,
[Supplierungen].[Entfällt] AS Entfällt,
[Supplierungen].[ZiehtVor] AS ZiehtVor,
[Supplierungen].[ZiehtVorDatum] AS ZiehtVorDatum,
[Supplierungen].[Datum] AS Datum,
[Supplierungen].[Klasse] AS Klasse,
[Supplierungen].[Stunde] AS Stunde,
[Supplierungen].[Grund] AS Grund,
CASE WHEN [Stundenplan].[Standardraum] = 1 THEN [Raeume].[Gebäude] ELSE [Stundenplan].[Gebäude] END AS Gebäude,
CASE WHEN [Stundenplan].[Standardraum] = 1 THEN [Raeume].[Raum] ELSE [Stundenplan].[Raum] END AS Raum,
CASE WHEN [Fächer].[Fach] IS NULL THEN [Supplierungen].[ErsatzFach] ELSE [Fächer].[Fach] END AS Fach
FROM
(((([Abteilungen] INNER JOIN [Supplierungen] ON [Abteilungen].[AbteilungsID]=[Supplierungen].[AbteilungsID])
INNER JOIN [Stundenplan] ON [Supplierungen].[Klasse]=[Stundenplan].[Klasse])
INNER JOIN [Raeume] ON [Supplierungen].[Klasse]=[Raeume].[StandardKlasse])
LEFT JOIN [Fächer] ON [Supplierungen].[ErsatzFach]=[Fächer].[FachKürzel])
WHERE (([Supplierungen].[Datum]>='" + DateTime.Today.ToString(Properties.Resources.sql_datumformat) + @"' OR [Supplierungen].[ZiehtVorDatum]>='" + DateTime.Today.ToString(Properties.Resources.sql_datumformat) + @"')
AND [Abteilungen].[Abteilungsname]='" + _ID + @"'
AND ([Supplierungen].[ErsatzLehrerKürzel] LIKE '%" + _LehrerKuerzel + @"%' OR [Supplierungen].[StattLehrerKürzel] LIKE '%" + _LehrerKuerzel + @"%')
AND [Supplierungen].[GlobalEntfall] = '-1')
ORDER BY [Supplierungen].[Datum], [Supplierungen].[Stunde], [Supplierungen].[Klasse]";
                    }
                    else //Einträge für bestimmte Klasse werden abgerufen
                    {
                        if (_Klasse != "" && _LehrerKuerzel == "")
                        {
                            befehl = @"SELECT DISTINCT
[Supplierungen].[ErsatzFach] AS ErsatzFach,
[Supplierungen].[StattLehrerKürzel] AS StattLehrerKürzel,
[Supplierungen].[ErsatzLehrerKürzel] AS ErsatzLehrerKürzel,
[Supplierungen].[Entfällt] AS Entfällt,
[Supplierungen].[ZiehtVor] AS ZiehtVor,
[Supplierungen].[ZiehtVorDatum] AS ZiehtVorDatum,
[Supplierungen].[Datum] AS Datum,
[Supplierungen].[Klasse] AS Klasse,
[Supplierungen].[Stunde] AS Stunde,
[Supplierungen].[Grund] AS Grund,
CASE WHEN [Stundenplan].[Standardraum] = 1 THEN [Raeume].[Gebäude] ELSE [Stundenplan].[Gebäude] END AS Gebäude,
CASE WHEN [Stundenplan].[Standardraum] = 1 THEN [Raeume].[Raum] ELSE [Stundenplan].[Raum] END AS Raum,
CASE WHEN [Fächer].[Fach] IS NULL THEN [Supplierungen].[ErsatzFach] ELSE [Fächer].[Fach] END AS Fach
FROM
(((([Abteilungen] INNER JOIN [Supplierungen] ON [Abteilungen].[AbteilungsID]=[Supplierungen].[AbteilungsID])
INNER JOIN [Stundenplan] ON [Supplierungen].[Klasse]=[Stundenplan].[Klasse])
INNER JOIN [Raeume] ON [Supplierungen].[Klasse]=[Raeume].[StandardKlasse])
LEFT JOIN [Fächer] ON [Supplierungen].[ErsatzFach]=[Fächer].[FachKürzel])
WHERE (([Supplierungen].[Datum]>='" + DateTime.Today.ToString(Properties.Resources.sql_datumformat) + @"' OR [Supplierungen].[ZiehtVorDatum]>='" + DateTime.Today.ToString(Properties.Resources.sql_datumformat) + @"')
AND [Abteilungen].[Abteilungsname]='" + _ID + @"'
AND [Supplierungen].[Klasse]='" + _Klasse + @"'
AND [Supplierungen].[GlobalEntfall] = '-1')
ORDER BY [Supplierungen].[Datum] , [Supplierungen].[Stunde] , [Supplierungen].[Klasse], [Supplierungen].[ErsatzLehrerKürzel]";
                        }
                    }
                }
            }
            else
            {
                befehl = @"SELECT DISTINCT
[Supplierungen].[ErsatzFach] AS ErsatzFach,
[Supplierungen].[StattLehrerKürzel] AS StattLehrerKürzel,
[Supplierungen].[ErsatzLehrerKürzel] AS ErsatzLehrerKürzel,
[Supplierungen].[Entfällt] AS Entfällt,
[Supplierungen].[ZiehtVor] AS ZiehtVor,
[Supplierungen].[ZiehtVorDatum] AS ZiehtVorDatum,
[Supplierungen].[Datum] AS Datum,
[Supplierungen].[Klasse] AS Klasse,
[Supplierungen].[Stunde] AS Stunde,
[Supplierungen].[Grund] AS Grund,
CASE WHEN [Stundenplan].[Standardraum] = 1 THEN [Raeume].[Gebäude] ELSE [Stundenplan].[Gebäude] END AS Gebäude,
CASE WHEN [Stundenplan].[Standardraum] = 1 THEN [Raeume].[Raum] ELSE [Stundenplan].[Raum] END AS Raum
FROM
(((([Bildschirme] INNER JOIN [Supplierungen] ON [Bildschirme].[AbteilungsID]=[Supplierungen].[AbteilungsID])
INNER JOIN [Stundenplan] ON [Supplierungen].[Klasse]=[Stundenplan].[Klasse])
INNER JOIN [Raeume] ON [Supplierungen].[Klasse]=[Raeume].[StandardKlasse])
LEFT JOIN [Fächer] ON [Supplierungen].[ErsatzFach]=[Fächer].[FachKürzel])
WHERE (([Supplierungen].[Datum]>='" + DateTime.Today.ToString(Properties.Resources.sql_datumformat) + @"' OR [Supplierungen].[ZiehtVorDatum]>='" + DateTime.Today.ToString(Properties.Resources.sql_datumformat) + @"')
AND [Bildschirme].[BildschirmID]='" + _ID + @"')
AND [Supplierungen].[GlobalEntfall] = '-1'
ORDER BY L1.[Nachname]";
            }
            DataTable daten = DatenbankAbfrage(befehl);
            if (daten.Rows.Count == 0) return new Structuren.LehrerSupplierungen[0];

            List<Structuren.LehrerSupplierungen> data = new List<Structuren.LehrerSupplierungen>();
            Structuren.LehrerSupplierungen temp = new Structuren.LehrerSupplierungen();
            List<Structuren.Supplierungen> temp2 = new List<Structuren.Supplierungen>();
            Structuren.Supplierungen temp3;

            for (int i = 0; i < daten.Rows.Count; i++)
            {
                temp3 = new Structuren.Supplierungen();
                temp3.Grund = daten.Rows[i]["Grund"].ToString();
                temp3.Datum = Convert.ToDateTime(daten.Rows[i]["Datum"]);
                temp3.Entfällt = Convert.ToBoolean(daten.Rows[i]["Entfällt"]);
                temp3.Ersatzfach = _Fachausschreiben ? FachLang(daten.Rows[i]["ErsatzFach"].ToString()) : daten.Rows[i]["ErsatzFach"].ToString();
                temp3.Ersatzlehrer = _RichtigerName ? LehrerNamen(daten.Rows[i]["ErsatzLehrerKürzel"].ToString()) : daten.Rows[i]["ErsatzLehrerKürzel"].ToString();
                temp3.Klasse = daten.Rows[i]["Klasse"].ToString();
                temp3.Stunde = Convert.ToInt32(daten.Rows[i]["Stunde"]);
                temp3.ZiehtVor = Convert.ToInt32(daten.Rows[i]["ZiehtVor"]);
                temp3.ZiehtVorDatum = Convert.ToDateTime(daten.Rows[i]["ZiehtVorDatum"]);
                temp3.Ursprungslehrer = _RichtigerName ? LehrerNamen(daten.Rows[i]["StattLehrerKürzel"].ToString()) : daten.Rows[i]["StattLehrerKürzel"].ToString();
                string raum = "";
                if (daten.Rows[i]["Gebäude"].ToString() != "" && daten.Rows[i]["Raum"].ToString() != "") raum = daten.Rows[i]["Gebäude"].ToString() + "-" + daten.Rows[i]["Raum"].ToString();
                else if (daten.Rows[i]["Gebäude"].ToString() != "") raum = daten.Rows[i]["Gebäude"].ToString();
                else if (daten.Rows[i]["Raum"].ToString() != "") raum = daten.Rows[i]["Raum"].ToString();
                temp3.Raum = raum;
                temp2.Add(temp3);
            }
            temp.Supplierungen = temp2.ToArray();
            temp.Lehrer = "";
            data.Add(temp);
            return data.ToArray();
        }

        /// <summary>
        /// Gibt alle Supplierungen des aktuellen Tages der Abteilung als Structuren.Supplierungen Array zurück.
        /// Die Abteilung wird aus der übergebenen Bildschirm-ID referenziert
        /// </summary>
        /// <param name="_ID">Die ID des Bildschirmes aus welcher die Abteilung referenziert wird</param>
        /// <returns>Die aktuellen Supplierungen der referenzierten Abteilung</returns>
        static public Structuren.Supplierungen[] AktuelleSupplierungenAbrufen(string _ID)
        {
            string befehl = @"SELECT
[Fächer].[Fach] AS Fach,
[Supplierungen].[StattLehrerKürzel] AS StattLehrerKürzel,
[Supplierungen].[ErsatzLehrerKürzel] AS ErsatzLehrerKürzel,
[Supplierungen].[Entfällt] AS Entfällt,
[Supplierungen].[ZiehtVor] AS ZiehtVor,
[Supplierungen].[Klasse] AS Klasse,
[Supplierungen].[Stunde] AS Stunde,
CASE WHEN [Stundenplan].[Standardraum] = 1 THEN [Raeume].[Gebäude] ELSE [Stundenplan].[Gebäude] END AS Gebäude,
CASE WHEN [Stundenplan].[Standardraum] = 1 THEN [Raeume].[Raum] ELSE [Stundenplan].[Raum] END AS Raum
FROM
(((([Bildschirme] INNER JOIN [Supplierungen] ON [Bildschirme].[AbteilungsID]=[Supplierungen].[AbteilungsID])
INNER JOIN [Stundenplan] ON [Supplierungen].[Klasse]=[Stundenplan].[Klasse] AND [Supplierungen].[Stunde]=[Stundenplan].[Stunde])
INNER JOIN [Raeume] ON [Supplierungen].[Klasse]=[Raeume].[StandardKlasse])
LEFT JOIN [Fächer] ON [Supplierungen].[ErsatzFach]=[Fächer].[FachKürzel])
WHERE [Supplierungen].[Datum]='" + DateTime.Today.ToString(Properties.Resources.sql_datumformat) + @"'
AND [Bildschirme].[BildschirmID]='" + _ID + @"'
ORDER BY [Supplierungen].[Stunde], [Supplierungen].[Klasse]";

            DataTable daten = DatenbankAbfrage(befehl);
            if (daten.Rows.Count == 0) return new Structuren.Supplierungen[0];

            Structuren.Supplierungen[] data = new Structuren.Supplierungen[daten.Rows.Count];

            for (int i = 0; i < daten.Rows.Count; i++)
            {
                data[i].Datum = Convert.ToDateTime(daten.Rows[i]["Datum"]);
                data[i].Entfällt = Convert.ToBoolean(daten.Rows[i]["Entfällt"]);
                data[i].Ersatzfach = daten.Rows[i]["Fach"].ToString();
                data[i].Ersatzlehrer = LehrerNamen(daten.Rows[i]["ErsatzLehrerKürzel"].ToString());
                data[i].Klasse = daten.Rows[i]["Klasse"].ToString();
                data[i].Stunde = Convert.ToInt32(daten.Rows[i]["Stunde"]);
                data[i].Ursprungslehrer = LehrerNamen(daten.Rows[i]["StattLehrerKürzel"].ToString());
                data[i].ZiehtVor = Convert.ToInt32(daten.Rows[i]["ZiehtVor"]);
                string raum = "";
                if (daten.Rows[i]["Gebäude"].ToString() != "" && daten.Rows[i]["Raum"].ToString() != "") raum = daten.Rows[i]["Gebäude"].ToString() + "-" + daten.Rows[i]["Raum"].ToString();
                else if (daten.Rows[i]["Gebäude"].ToString() != "") raum = daten.Rows[i]["Gebäude"].ToString();
                else if (daten.Rows[i]["Raum"].ToString() != "") raum = daten.Rows[i]["Raum"].ToString();
                data[i].Raum = raum;
            }

            return data;
        }


        /// <summary>
        /// Gibt die aktuelle Stunde aufgrund der aktuell eingestellten Zeit zurück.
        /// </summary>
        /// <returns>Die aktuelle Stunde</returns>
        static public int AktuelleStunde()
        {
            DateTime aktuelleZeit = DateTime.Now;
            if (aktuelleZeit.TimeOfDay < new TimeSpan(7, 10, 0)) return -2;
            if (aktuelleZeit.TimeOfDay < new TimeSpan(8, 0, 0)) return 0;
            if (aktuelleZeit.TimeOfDay <= new TimeSpan(8, 50, 0)) return 1;     // 1. Stunde
            if (aktuelleZeit.TimeOfDay <= new TimeSpan(9, 40, 0)) return 2;     // 2. Stunde
            if (aktuelleZeit.TimeOfDay < new TimeSpan(9, 50, 0)) return -1;     // Pause
            if (aktuelleZeit.TimeOfDay <= new TimeSpan(10, 40, 0)) return 3;    // 3. Stunde
            if (aktuelleZeit.TimeOfDay <= new TimeSpan(11, 30, 0)) return 4;    // 4. Stunde
            if (aktuelleZeit.TimeOfDay < new TimeSpan(11, 40, 0)) return -1;    // Pause
            if (aktuelleZeit.TimeOfDay <= new TimeSpan(12, 30, 0)) return 5;    // 5. Stunde
            if (aktuelleZeit.TimeOfDay <= new TimeSpan(13, 20, 0)) return 6;    // 6. Stunde
            if (aktuelleZeit.TimeOfDay <= new TimeSpan(14, 10, 0)) return 7;    // 7. Stunde
            if (aktuelleZeit.TimeOfDay < new TimeSpan(14, 20, 0)) return -1;    // Pause
            if (aktuelleZeit.TimeOfDay <= new TimeSpan(15, 10, 0)) return 8;    // 8. Stunde
            if (aktuelleZeit.TimeOfDay <= new TimeSpan(16, 0, 0)) return 9;     // 9. Stunde
            if (aktuelleZeit.TimeOfDay < new TimeSpan(16, 10, 0)) return -1;    // Pause
            if (aktuelleZeit.TimeOfDay <= new TimeSpan(17, 0, 0)) return 10;    // 10. Stunde
            if (aktuelleZeit.TimeOfDay <= new TimeSpan(17, 50, 0)) return 11;   // 11. Stunde
            if (aktuelleZeit.TimeOfDay <= new TimeSpan(18, 40, 0)) return 12;   // 12. Stunde
            return -2;
        }

        /// <summary>
        /// Gibt den aktuellen tag aufgrund der aktuell eingestellten Zeit zurück.
        /// </summary>
        /// <returns>Den aktuellen Tag</returns>
        static public int AktuellerTag()
        {
            return ((int)DateTime.Now.DayOfWeek - 1) % 7;
        }

        /// <summary>
        /// Gibt den/die Lehrernamen für das/die übergebenen Lehrerkürzel zurück.
        /// </summary>
        /// <param name="_Kürzel">Lehrerkürzel getrennt durch ein '/'</param>
        /// <returns>Namen der/des Lehrer(s)(in)</returns>
        static public string LehrerNamen(string _Kürzel)
        {
            string[] Lehrer = _Kürzel.Split('/');
            List<string> Namen = new List<string>();
            string befehl;
            DataTable daten;

            foreach (string aktuell in Lehrer)
            {
                befehl = @"SELECT
LehrerName = [LehrerTesten1].[Vorname] + ' ' + UPPER([LehrerTesten1].[Nachname])
FROM [LehrerTesten1]
WHERE [LehrerTesten1].[LehrerKuerzel]='" + aktuell + "'";
                daten = DatenbankAbfrage(befehl);
                if (daten.Rows.Count > 0)
                    Namen.Add(daten.Rows[0]["LehrerName"].ToString());
                daten.Dispose();
            }
            return String.Join(", ", Namen);
        }

        static public string FachLang(string _Kürzel)
        {
            string[] Fach = _Kürzel.Split('/');
            List<string> FachL = new List<string>();
            string befehl;
            DataTable daten;

            foreach (string aktuell in Fach)
            {
                befehl = @"SELECT
Fach
FROM [Fächer]
WHERE [Fächer].[FachKürzel]='" + aktuell + "'";
                daten = DatenbankAbfrage(befehl);
                if (daten.Rows.Count > 0)
                    FachL.Add(daten.Rows[0]["Fach"].ToString());
                daten.Dispose();
            }
            return String.Join(", ", FachL);
        }

        

        #region nur Verwaltung
        /// <summary>
        /// Ruft alle Klassen einer Abteilung ab.
        /// Wird mit _abteilung ein Leerstring übergeben, werden alle Klassen zurückgegeben.
        /// </summary>
        /// <param name="_abteilung">Die Abteilung aus welcher alle Klassen abgerufen werden.</param>
        /// <returns>Alle Klassen der Abteilung</returns>
        static public string[] KlassenAbrufen(string _abteilung)
        {
            string befehl;
            DataTable daten;
            if (_abteilung == "")
            {
                befehl = @"SELECT
[Klassen].[Klasse] AS Klasse
FROM [Klassen]
ORDER BY [Klassen].[Klasse]";
            }
            else
            {
                befehl = @"SELECT
[Klassen].[Klasse] AS Klasse
FROM ([Klassen] INNER JOIN [Abteilungen] ON [Klassen].[AbteilungsID]=[Abteilungen].[AbteilungsID])
WHERE [Abteilungen].[Abteilungsname]='" + _abteilung + @"'
ORDER BY [Klassen].[Klasse]";
            }
            daten = DatenbankAbfrage(befehl);
            string[] temp = new string[daten.Rows.Count];
            for (int i = 0; i < daten.Rows.Count; i++)
                temp[i] = daten.Rows[i]["Klasse"].ToString();
            return temp;
        }

        /// <summary>
        /// Gibt alle Abteilungsnamen als String Array zurück.
        /// </summary>
        /// <returns>alle Abteilungsnamen</returns>
        static public string[] AbteilungenAbrufen()
        {
            string befehl = @"SELECT
[Abteilungen].[Abteilungsname] AS Abteilung
FROM [Abteilungen]
ORDER BY [Abteilungen].[Abteilungsname]";
            DataTable daten = DatenbankAbfrage(befehl);

            string[] temp = new string[daten.Rows.Count];
            for (int i = 0; i < daten.Rows.Count; i++)
                temp[i] = daten.Rows[i]["Abteilung"].ToString();
            return temp;
        }

        /// <summary>
        /// Gibt alle in der Zukunft bzw. aktuellen Tests/Schularbeiten als Strukturen.Tests Arry zurück.
        /// </summary>
        /// <param name="_Klasse">Die Klasse für welche die Tests zurückgegeben werden soll.</param>
        /// <param name="_LehrerNamen">Gibt an, ob die Lehrernamen oder die Lehrerkürzel zurückgegeben werden sollen.</param>
        /// <returns>Alle Tests der Klasse</returns>
        static public Structuren.Tests[] TestsAbrufen(string _Klasse, bool _LehrerNamen = true, bool Fachkuerzel = false)
        {
            DatenbankSchreiben.RemovePastExams();

            string befehl = @"SELECT
[Tests].[LehrerKürzel] AS LehrerKürzel,
[Fächer].[Fach] AS Fach,
[Tests].[Datum] AS Datum,
[Tests].[Stunde] AS Stunde,
[Tests].[Dauer] AS Dauer,
[Tests].[Klasse] AS Klasse,
[Tests].[RaumID] AS Raum, 
[Testarten].[Testart] AS Testart
FROM (([Tests] INNER JOIN [Fächer] ON [Tests].[FachKürzel]=[Fächer].[FachKürzel])
LEFT JOIN [Testarten] ON [Tests].[TestartID]=[Testarten].[TestartID])
WHERE [Tests].[Klasse]='" + _Klasse + @"'
AND [Tests].[Datum]>='" + DateTime.Today.ToString(Properties.Resources.sql_datumformat) + @"'
ORDER BY [Tests].[Datum], [Tests].[Stunde], [Tests].[RaumID]";
            DataTable daten = DatenbankAbfrage(befehl);

            Structuren.Tests[] temp = new Structuren.Tests[daten.Rows.Count];
            for (int i = 0; i < daten.Rows.Count; i++)
            {
                temp[i].Datum = daten.Rows[i]["Datum"].ToDateTime();
                temp[i].Dauer = daten.Rows[i]["Dauer"].ToInt32();
                temp[i].Fach = Fachkuerzel ?  GetSubjectAbbreviation(daten.Rows[i]["Fach"].ToString()) :  daten.Rows[i]["Fach"].ToString();
                temp[i].Lehrer = _LehrerNamen ? daten.Rows[i]["LehrerKürzel"].ToLehrer() : daten.Rows[i]["LehrerKürzel"].ToString();
                temp[i].Stunde = daten.Rows[i]["Stunde"].ToInt32();
                temp[i].Testart = daten.Rows[i]["Testart"].ToString();
                temp[i].Raum = daten.Rows[i]["Raum"].ToString();
            }
            return temp;
        }

        /// <summary>
        /// Ruft alle Informationen zu einem Benutzer ab und liefert diese als Structuren.User zurück.
        /// </summary>
        /// <param name="_benutzer">Der Benutzername des Benutzers zu welchen die Informationen abgerufen werden sollen.</param>
        /// <returns>Alle Informationen zu einem Benutzer.</returns>
        static public Structuren.User GetUserInfo (string _benutzer)
        {
            string befehl;
            DataTable daten;

            Structuren.User userinfo = new Structuren.User();
            userinfo.Lehrer = false;
            userinfo.Superadmin = false;
            userinfo.Abteilungen = new string[0];
            userinfo.StammAbteilung = "";
            userinfo.Klassen = new string[0];
            userinfo.StammKlasse = "";

            befehl = @"SELECT
[LehrerTesten1].[Abteilungen] AS Abteilungen,
[Klassen].[Klasse] AS StammKlasse,
[Abteilungen].[Abteilungsname] AS StammAbteilung
FROM (([LehrerTesten1] LEFT JOIN [Klassen] ON [LehrerTesten1].[LehrerKuerzel]=[Klassen].[Klassenvorstand]) LEFT JOIN [Abteilungen] ON [Abteilungen].[AbteilungsID] = [Klassen].[AbteilungsID])
WHERE [LehrerTesten1].[LehrerKuerzel]='" + _benutzer + "'";
            daten = DatenbankAbfrage(befehl);
            
            if (daten.Rows.Count > 0)
            {
                userinfo.Lehrer = true;
                userinfo.Abteilungen = daten.Rows[0]["Abteilungen"].ToString().Split(';');
                userinfo.StammKlasse = daten.Rows[0]["StammKlasse"].ToString();
                userinfo.StammAbteilung = daten.Rows[0]["StammAbteilung"].ToString();
                try
                {
                    if (userinfo.StammAbteilung == "") userinfo.StammAbteilung = userinfo.Abteilungen[0];
                }
                catch { }

                befehl = @"SELECT DISTINCT
[Stundenplan].[Klasse] AS Klasse
FROM [Stundenplan]
WHERE [Stundenplan].[LehrerKürzel] LIKE '%" + _benutzer + @"%'
ORDER BY [Stundenplan].[Klasse]";
                daten = DatenbankAbfrage(befehl);

                string[] klassen = new string[daten.Rows.Count];
                for (int i = 0; i < daten.Rows.Count; i++)
                {
                    klassen[i] = daten.Rows[i]["Klasse"].ToString();
                }

                userinfo.Klassen = klassen;
                try
                {
                    if (userinfo.StammKlasse == "") userinfo.StammKlasse = userinfo.Klassen[0];
                }
                catch { }
            }

            befehl = @"SELECT
[Superadmins].[LoginKürzel] AS Login
FROM [Superadmins]
WHERE [Superadmins].[LoginKürzel]='" + _benutzer + "'";
            daten = DatenbankAbfrage(befehl);

            if (daten.Rows.Count > 0) userinfo.Superadmin = true;

            return userinfo;
        }

        /// <summary>
        /// Ruft die Gruppenrechte ab und legt diese in der aktuellen Session fest.
        /// </summary>
        /// <param name="_GruppenrechtId">Die Berechtigungsstufe des Benutzers.</param>
        /// <returns>true bei der Fehlerfreien Verarbeitung aller Einsteillungen in die Session</returns>
        static private bool SetGruppenrechte(int _GruppenrechtId)
        {
            string befehl = @"SELECT
[Gruppenrechte].[Stundenplan] AS Stundenplan,
[Gruppenrechte].[Supplierung] AS Supplierung,
[Gruppenrechte].[Klasseninfo] AS Klasseninfo,
[Gruppenrechte].[Abteilungsinfo] AS Abteilungsinfo,
[Gruppenrechte].[Sprechstunden] AS Sprechstunden,
[Gruppenrechte].[Tests] AS Tests,
[Gruppenrechte].[Klassen] AS Klassen,
[Gruppenrechte].[Räume] AS Räume,
[Gruppenrechte].[Bildschirme] AS Bildschirme,
[Gruppenrechte].[Gruppenrechte] AS Gruppenrechte
FROM [Gruppenrechte]
WHERE [Gruppenrechte].[GruppenrechtID]='" + _GruppenrechtId + "'";
            DataTable daten = DatenbankAbfrage(befehl);

            if (daten.Rows.Count == 0) return false;

            try
            {
                Login.Rechte.Stundenplan = daten.Rows[0]["Stundenplan"].ToString();
                Login.Rechte.Supplierung = daten.Rows[0]["Supplierung"].ToString();
                Login.Rechte.Klasseninfo = daten.Rows[0]["Klasseninfo"].ToString();
                Login.Rechte.Abteilungsinfo = daten.Rows[0]["Abteilungsinfo"].ToString();
                Login.Rechte.Sprechstunden = daten.Rows[0]["Sprechstunden"].ToString();
                Login.Rechte.Tests = daten.Rows[0]["Tests"].ToString();
                Login.Rechte.Klassen = daten.Rows[0]["Klassen"].ToString();
                Login.Rechte.Räume = daten.Rows[0]["Räume"].ToString();
                Login.Rechte.Bildschirme = daten.Rows[0]["Bildschirme"].ToString();
                Login.Rechte.Gruppenrechte = daten.Rows[0]["Gruppenrechte"].ToString();
                return true;
            }
            catch { return false; }
        }

        /// <summary>
        /// Ruft die Gruppenrechte ab und legt diese in der aktuellen Session fest.
        /// </summary>
        /// <param name="_benutzer">Benutzername des Benutzers</param>
        /// <returns>true bei der Fehlerfreien Verarbeitung aller Einsteillungen in die Session</returns>
        static public bool SetGruppenrechte(string _benutzer)
        {
            string befehl;
            DataTable daten;

            befehl = @"SELECT
[Superadmins].[LoginKürzel] AS Login
FROM [Superadmins]
WHERE [Superadmins].[LoginKürzel]='" + _benutzer + "'";
            daten = DatenbankAbfrage(befehl);
            if (daten.Rows.Count > 0)
            {
                if (Login.StammKlasse == "")
                {
                    Login.StammKlasse = Authentifizierung.GetKlasse(Login.User);
                    Login.Klassen = new string[] { Login.StammKlasse };
                    string Abteilung = GetAbteilungVonKlasse(Login.StammKlasse);
                    if (Abteilung != "")
                    {
                        Login.StammAbteilung = Abteilung;
                        Login.Abteilungen = new string[] { Abteilung };
                    }
                }
                return SetGruppenrechte(8);
            }

            befehl = @"SELECT
[Abteilungen].[Abteilungsname] AS Abteilungsname
FROM [Abteilungen]
WHERE [Abteilungen].[Abteilungsvorstand]='" + _benutzer + "'";
            daten = DatenbankAbfrage(befehl);
            if (daten.Rows.Count > 0)
            {
                Login.StammAbteilung = daten.Rows[0]["Abteilungsname"].ToString();
                if (!Login.Abteilungen.Contains<string>(daten.Rows[0]["Abteilungsname"].ToString()))
                    Login.Abteilungen = (daten.Rows[0]["Abteilungsname"].ToString() + "/" + String.Join("/", Login.Abteilungen)).Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                return SetGruppenrechte(7);
            }

            befehl = @"SELECT
[Abteilungen].[Abteilungsname] AS Abteilungsname
FROM [Abteilungen]
WHERE [Abteilungen].[Abteilungswart1]='" + _benutzer + @"'
OR [Abteilungen].[Abteilungswart2]='" + _benutzer + "'";
            daten = DatenbankAbfrage(befehl);
            if (daten.Rows.Count > 0)
            {
                Login.StammAbteilung = daten.Rows[0]["Abteilungsname"].ToString();
                if (!Login.Abteilungen.Contains<string>(daten.Rows[0]["Abteilungsname"].ToString()))
                    Login.Abteilungen = (daten.Rows[0]["Abteilungsname"].ToString() + "/" + String.Join("/", Login.Abteilungen)).Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                return SetGruppenrechte(6);
            }

            befehl = @"SELECT
[LehrerTesten1].[LehrerKuerzel] AS LehrerKuerzel
FROM [LehrerTesten1]
WHERE [LehrerTesten1].[LehrerKuerzel]='" + _benutzer + "'";
            daten = DatenbankAbfrage(befehl);
            if (daten.Rows.Count > 0) return SetGruppenrechte(4);

            befehl = @"SELECT
[Abteilungen].[Abteilungsname] AS Abteilungsname
FROM [Abteilungen]
WHERE [Abteilungen].[Abteilungssprecher]='" + _benutzer + "'";
            daten = DatenbankAbfrage(befehl);
            if (daten.Rows.Count > 0)
            {
                Login.StammAbteilung = daten.Rows[0]["Abteilungsname"].ToString();
                if (!Login.Abteilungen.Contains<string>(daten.Rows[0]["Abteilungsname"].ToString()))
                    Login.Abteilungen = (daten.Rows[0]["Abteilungsname"].ToString() + "/" + String.Join("/", Login.Abteilungen)).Split(new char[] {'/'}, StringSplitOptions.RemoveEmptyEntries);
                return SetGruppenrechte(3);
            }

            befehl = @"SELECT
[Klassen].[Klassensprecher] AS Klassensprecher
FROM [Klassen]
WHERE [Klassen].[Klassensprecher]='" + _benutzer + "'";
            daten = DatenbankAbfrage(befehl);


            Login.StammKlasse = Authentifizierung.GetKlasse(Login.User);
            Login.Klassen = new string[] { Login.StammKlasse };

            string Abteilungx = GetAbteilungVonKlasse(Login.StammKlasse);

            if (Abteilungx == "") return SetGruppenrechte(0);
            Login.StammAbteilung = Abteilungx;
            Login.Abteilungen = new string[] { Abteilungx };
            if (daten.Rows.Count > 0) return SetGruppenrechte(2);
            return SetGruppenrechte(1);
        }

        /// <summary>
        /// Gibt die Abteilungs ID einer Klasse zurück.
        /// </summary>
        /// <param name="_Klasse">Die Klasse von welcher die Abteilungs ID ermittelt werden soll.</param>
        /// <returns>Abteilungs ID einer Klasse</returns>
        static public int GetAbteilungsIDVonKlasse(string _Klasse)
        {
            string befehl = @"SELECT
[Klassen].[AbteilungsID] AS AbteilungsID
FROM [Klassen]
WHERE [Klassen].[Klasse]='" + _Klasse + "'";
            DataTable daten = DatenbankAbfrage(befehl);
            if (daten.Rows.Count == 0) return -1;
            return daten.Rows[0]["AbteilungsID"].ToInt32();
        }

        /// <summary>
        /// Gibt den Abteilungsnamen einer Klasse zurück
        /// </summary>
        /// <param name="_Klasse">Die Klasse von welcher der Abteilungsname ermittelt werden soll.</param>
        /// <returns>Abteilungsname einer Klasse</returns>
        static public string GetAbteilungVonKlasse (string _Klasse)
        {
            string befehl = @"SELECT
[Abteilungen].[Abteilungsname] AS Abteilungsname
FROM [Abteilungen]
WHERE [Abteilungen].[AbteilungsID]='" + GetAbteilungsIDVonKlasse(_Klasse) + "'";
            DataTable daten = DatenbankAbfrage(befehl);
            if (daten.Rows.Count == 0) return "";
            return daten.Rows[0]["Abteilungsname"].ToString();
        }

        /// <summary>
        /// Ruft alle Testarten ab und gibt sie als String Array zurück.
        /// </summary>
        /// <returns>Alle Testarten</returns>
        static public string[] TestartenAbrufen()
        {
            string befehl = @"SELECT
[Testarten].[Testart] AS Testart
FROM [Testarten]
ORDER BY [Testarten].[TestartID]";
            DataTable daten = DatenbankAbfrage(befehl);

            string[] temp = new string[daten.Rows.Count];

            for (int i = 0; i < daten.Rows.Count; i++)
            {
                temp[i] = daten.Rows[i]["Testart"].ToString();
            }

            return temp;
        }
        ///<summary>
        ///Ruft alle Räume der Abteilung ab und bringt diese als string array zurück
        ///</summary>
        static public string[] RaumAbrufen()
        {
            string befehl = @"SELECT [Raeume].[Gebäude] AS Geb,[Raeume].[Raum] AS Raum,[Raeume].[StandardKlasse] AS Klasse FROM [Raeume]";
            DataTable daten = DatenbankAbfrage(befehl);

            string[] temp = new string[daten.Rows.Count];

            for(int i = 0; i < daten.Rows.Count; i++)
            {
                temp[i] = daten.Rows[i]["Geb"].ToString()+"-"+((int)daten.Rows[i]["Raum"]).ToString("0000")+" (" + daten.Rows[i]["Klasse"].ToString()+")";
            }
            return temp;
        }

        /// <summary>
        /// Gibt das Fachkürzel zu einem übergebenen Fach zurück.
        /// </summary>
        /// <param name="_Fach">Der Name des Fachs</param>
        /// <returns>Fachkürzel</returns>
        static public string GetFachKürzelVonFach(string _Fach)
        {
            string befehl = @"SELECT
[Fächer].[FachKürzel] AS FachKürzel
FROM [Fächer]
WHERE [Fächer].[Fach]='" + _Fach + "'";
            DataTable daten = DatenbankAbfrage(befehl);
            if (daten.Rows.Count == 0) return "";
            return daten.Rows[0]["FachKürzel"].ToString();
        }

        /// <summary>
        /// Prüft, ob der übergebene String ein Lehrerkürzel ist.
        /// </summary>
        /// <param name="_Kürzel">Das vermeintliche Lehrerkürzel</param>
        /// <returns>true, wenn es sich um ein Lehrerkürzel handelt.</returns>
        static public bool IsLehrerKürzel(string _Kürzel)
        {
            string befehl = @"SELECT
[LehrerTesten1].[LehrerKuerzel] AS LehrerKuerzel
FROM [LehrerTesten1]
WHERE [LehrerTesten1].[LehrerKuerzel]='" + _Kürzel + "'";
            if (DatenbankAbfrage(befehl).Rows.Count == 0) return false;
            return true;
        }

        /// <summary>
        /// Ruft den Stundenplan eines bestimmten Lehrers ab und gibt diesen als Structuren.LehrerStundenplanEntry 2D-Array zurück.
        /// </summary>
        /// <param name="_LehrerKürzel">Das Kürzel des Lehrers, dessen Stundenplan abgerufen werden soll.</param>
        /// <param name="_Abteilung">Die Abteilung, von welcher der Stundenplan abgerufen werden soll.</param>
        /// <returns>Den Stundenplan des Lehrers der Abteilung</returns>
        static public Structuren.LehrerStundenplanEntry[,] LehrerStundenplanAbrufen(string _LehrerKürzel, string _Abteilung)
        {
            string befehl = @"SELECT
[Abteilungen].[Samstag] AS Samstag,
[Abteilungen].[AbteilungsID] AS AbteilungsID
FROM [Abteilungen]
WHERE [Abteilungen].[Abteilungsname]='" + _Abteilung + "'";
            DataTable daten = DatenbankAbfrage(befehl);
            if (daten.Rows.Count == 0) return new Structuren.LehrerStundenplanEntry[0,0];
            int schultage = Convert.ToBoolean(daten.Rows[0]["Samstag"]) ? 6 : 5;
            int Abteilung = daten.Rows[0]["AbteilungsID"].ToInt32();

            befehl = @"SELECT
[Stundenplan].[Wochentag] AS Wochentag,
[Stundenplan].[Klasse] AS Klasse,
[Stundenplan].[Stunde] AS Stunde,
CASE WHEN [Stundenplan].[Standardraum] = 1 THEN [Raeume].[Gebäude] ELSE [Stundenplan].[Gebäude] END AS Gebäude,
CASE WHEN [Stundenplan].[Standardraum] = 1 THEN [Raeume].[Raum] ELSE [Stundenplan].[Raum] END AS Raum,
CASE WHEN [Fächer].[Fach] IS NULL THEN [Stundenplan].[FachKürzel] ELSE [Fächer].[Fach] END AS Fach
FROM
(([Stundenplan] INNER JOIN [Raeume] ON [Stundenplan].[Klasse]=[Raeume].[StandardKlasse])
LEFT JOIN [Fächer] ON [Stundenplan].[FachKürzel]=[Fächer].[FachKürzel])
WHERE [Stundenplan].[LehrerKürzel] LIKE '%" + _LehrerKürzel + @"%'
AND [Stundenplan].[AbteilungsID]='" + Abteilung + @"'
ORDER BY [Stundenplan].[Stunde], [Stundenplan].[Wochentag]";
            daten = DatenbankAbfrage(befehl);
        
            List<Structuren.LehrerStundenplanEntry>[] temp = new List<Structuren.LehrerStundenplanEntry>[schultage];
            int x;
            Structuren.LehrerStundenplanEntry y;
            for (int i = 0; i < daten.Rows.Count; i++)
            {
                x = daten.Rows[i]["Wochentag"].ToInt32();
                y = new Structuren.LehrerStundenplanEntry();
                y.Klasse = daten.Rows[i]["Klasse"].ToString();
                y.Stunde = daten.Rows[i]["Stunde"].ToInt32();
                temp[i].Add(y);
            }

            return new Structuren.LehrerStundenplanEntry[0, 0];
        }

        /// <summary>
        /// Ruft alle Betriebsmodi als Structuren.Betriebsmodi Array ab.
        /// </summary>
        /// <returns>Alle Betriebsmodi</returns>
        static public Structuren.Betriebsmodi[] BetriebsmodiAbrufen()
        {
            string befehl = @"SELECT
[Betriebsmodi].[BetriebsmodeID] AS ID,
[Betriebsmodi].[Bezeichnung] AS Bezeichnung
FROM [Betriebsmodi]
ORDER BY [Betriebsmodi].[Bezeichnung]";
            DataTable daten = DatenbankAbfrage(befehl);

            Structuren.Betriebsmodi[] ret = new Structuren.Betriebsmodi[daten.Rows.Count];
            for (int i = 0; i < daten.Rows.Count; i++)
            {
                ret[i].id = daten.Rows[i]["ID"].ToInt32();
                ret[i].bezeichnung = daten.Rows[i]["Bezeichnung"].ToString();
            }
            return ret;
        }

        /// <summary>
        /// Ruft den Aktuellen Betriebsmode einer Abteilung ab und übergibt dessen ID.
        /// </summary>
        /// <param name="_Abteilung">Die Abteilung dessen aktueller Betriebsmode abgerufen werden soll.</param>
        /// <returns>Aktueller Betriebsmode einer Abteilung</returns>
        static public int AktuellenBetriebsmodeAbrufen(string _Abteilung)
        {
            string befehl = @"SELECT
[Abteilungen].[StandardBetriebsmode] AS ID
FROM [Abteilungen]
WHERE [Abteilungen].[Abteilungsname]='" + _Abteilung + "'";
            DataTable daten = DatenbankAbfrage(befehl);
            if (daten.Rows.Count == 0) return -1;
            return daten.Rows[0]["ID"].ToInt32();
        }

        /// <summary>
        /// Schließt die Abteilungs-ID aus dem Abteilungsnamen und gibt diese zurück.
        /// </summary>
        /// <param name="_Abteilungsname">Der Abteilungsname aus welchem die Abteilungs-ID referenziert werden soll.</param>
        /// <returns>Die Abteilungs-ID des Abteilungsnamen</returns>
        static public int GetAbteilungsIdVonAbteilungsname(string _Abteilungsname)
        {
            string befehl = @"SELECT
[Abteilungen].[AbteilungsID] AS AbteilungsID
FROM [Abteilungen]
WHERE [Abteilungen].[Abteilungsname]='" + _Abteilungsname + "'";
            DataTable daten = DatenbankAbfrage(befehl);

            try { return daten.Rows[0]["AbteilungsID"].ToInt32(); }
            catch { return -1; }
        }

        /// <summary>
        /// Ruft alle BildschirmIDs ab, die einem bestimmten Betriebsmode zugeordnet sind.
        /// </summary>
        /// <param name="_BetriebsmodeID">Der Betriebsmode von welchem alle Bildschirme ermittelt werden sollen.</param>
        /// <returns>Gibt ein Int Array aller BildschirmIDs zurück, welche dem Übergebenen Betriebsmode zugeordnet sind.</returns>
        static public int[] BildschirmeBetriebsmodeAbrufen(int _BetriebsmodeID)
        {
            string befehl = @"SELECT
[Betriebsanzeige].[BildschirmID]
FROM [Betriebsanzeige] INNER JOIN [Bildschirme] ON [Bildschirme].[BildschirmID] = [Betriebsanzeige].[BildschirmID]
WHERE [Betriebsanzeige].[BetriebsmodeID]='" + _BetriebsmodeID.ToString() + "' " + "ORDER BY Raum";
            DataTable daten = DatenbankAbfrage(befehl);

            int[] ret = new int[daten.Rows.Count];

            for(int i = 0; i < daten.Rows.Count; i++)
            {
                ret[i] = daten.Rows[i]["BildschirmID"].ToInt32();
            }

            return ret;
        }

        /// <summary>
        /// Gibt die Positionsinformationen eines Bildschirmes als Structuren.Bildschirm zurück.
        /// </summary>
        /// <param name="_BildschirmID">Die ID des Bildschirmes von welchem die Informationen ermittelt werden sollen</param>
        /// <returns>Die Positionsinformationen eines Bildschirmes als Structuren.Bildschirm</returns>
        static public Structuren.Bildschirm BildschirmInformationenAbrufen(int _BildschirmID)
        {
            string befehl = @"SELECT
[Abteilungen].[Abteilungsname] AS Abteilungsname,
[Raeume].[StandardKlasse] AS StandardKlasse,
[Bildschirme].[Gebäude] AS Gebäude,
[Bildschirme].[Raum] AS Raum,
[Bildschirme].[Anzeigeart] AS Anzeigeart
FROM (([Bildschirme] INNER JOIN [Abteilungen] ON [Bildschirme].[AbteilungsID]=[Abteilungen].[AbteilungsID])
INNER JOIN [Raeume] ON
[Bildschirme].[Raum]=[Raeume].[Raum])
WHERE [Bildschirme].[BildschirmID]='" + _BildschirmID.ToString() + "'";
            DataTable daten = DatenbankAbfrage(befehl);

            if (daten.Rows.Count == 0) return new Structuren.Bildschirm();

            Structuren.Bildschirm ret = new Structuren.Bildschirm();
            ret.Abteilung = daten.Rows[0]["Abteilungsname"].ToString();
            ret.Klasse = daten.Rows[0]["StandardKlasse"].ToString();
            ret.AnzeigeArt = daten.Rows[0]["Anzeigeart"].ToInt32();
            ret.Gebäude = daten.Rows[0]["Gebäude"].ToString();
            ret.id = _BildschirmID;
            ret.Raum = daten.Rows[0]["Raum"].ToInt32();

            return ret;
        }

        /// <summary>
        /// Ruft alle Dateien eines Betriebsmodes auf und übergibt diese in einem Structuren.Dateien Array.
        /// </summary>
        /// <param name="_BetriebsmodeID">Die BetriebsmodeID von welcher die Dateien abgerufen werden sollen.</param>
        /// <returns>Alle Dateien eines Betriebsmodes als Structuren.Dateien Array</returns>
        static public Structuren.Dateien[] DateienAbrufen(int _BetriebsmodeID)
        {
            string befehl = @"SELECT
[Dateien].[DateiID] AS DateiID,
[Dateien].[Dateiname] AS Dateiname
FROM [Dateien]
WHERE [Dateien].[BetriebsmodeID]='" + _BetriebsmodeID.ToString() + "'";
            DataTable daten = DatenbankAbfrage(befehl);

            Structuren.Dateien[] ret = new Structuren.Dateien[daten.Rows.Count];
            for (int i = 0; i < daten.Rows.Count; i++)
            {
                ret[i].id = daten.Rows[i]["DateiID"].ToInt32();
                ret[i].bezeichnung = daten.Rows[i]["Dateiname"].ToString();
            }

            return ret;
        }

        /// <summary>
        /// Gibt alle Abteilungsnamen als String Array zurück.
        /// </summary>
        /// <returns>Alle Abteilungsnamen</returns>
        static public string[] AlleAbteilungenAbrufen()
        {
            string befehl = @"SELECT
[Abteilungen].[Abteilungsname] AS Abteilungsname
FROM [Abteilungen]";
            DataTable daten = DatenbankAbfrage(befehl);

            string[] ret = new string[daten.Rows.Count];
            for (int i = 0; i < daten.Rows.Count; i++)
            {
                ret[i] = daten.Rows[i]["Abteilungsname"].ToString();
            }

            return ret;
        }

        /// <summary>
        /// Gibt alle Bildschirme einer Abteilung als Int Array zurück.
        /// </summary>
        /// <param name="_Abteilungsname">Der Abteilungsname von welcher alle Bildschirme zurückgegeben werden sollen.</param>
        /// <returns>Alle Bildschirme einer Abteilung als Int Array</returns>
        static public int[] BildschirmeEinerAbteilungAbrufen(string _Abteilungsname)
        {
            int id = DatenbankAbrufen.GetAbteilungsIdVonAbteilungsname(_Abteilungsname);

            string befehl = @"SELECT
[Bildschirme].[BildschirmID] AS BildschirmID
FROM [Bildschirme]
WHERE [Bildschirme].[AbteilungsID]='" + id.ToString() + "'";
            DataTable daten = DatenbankAbrufen.DatenbankAbfrage(befehl);

            int[] ret = new int[daten.Rows.Count];

            for (int i = 0; i < daten.Rows.Count; i++)
            {
                ret[i] = daten.Rows[i]["BildschirmID"].ToInt32();
            }

            return ret;
        }

        static public Structuren.Klasseneigenschaften KlasseneigenschaftenAbrufen(string _klasse)
        {
            Structuren.Klasseneigenschaften temp = new Structuren.Klasseneigenschaften();
            using (Entities db = new Entities())
            {
                temp.abteilungsID = (from a in db.Klassen where a.Klasse.Equals(_klasse) select a.AbteilungsID).ToList()[0];
                try { temp.gebäude = (from a in db.Raeume where a.StandardKlasse.Equals(_klasse) select a.Gebäude).ToList()[0]; }
                catch { temp.gebäude = ""; }
                try { temp.raum = (from a in db.Raeume where a.StandardKlasse.Equals(_klasse) select a.Raum).ToList()[0].ToString(Properties.Resources.sandardRaumnummer); }
                catch { temp.raum = ""; }
                temp.klasse = _klasse;
                temp.klassensprecher = (from a in db.Klassen where a.Klasse.Equals(_klasse) select a.Klassensprecher).ToList()[0];
                temp.klassenvorstand = (from a in db.Klassen where a.Klasse.Equals(_klasse) select a.Klassenvorstand).ToList()[0];
                temp.klasseninfo = (from a in db.Klassen where a.Klasse.Equals(_klasse) select a.Klasseninfo).ToList()[0];
            }
            return temp;
        }

        /// <summary>
        /// Prüft ob Klasse existiert.
        /// </summary>
        /// <param name="_klasse"></param>
        /// <returns></returns>
        static public bool KlasseExists(string _klasse)
        {
            using (Entities db = new Entities())
            {
                if (db.Klassen.Where(a => a.Klasse == _klasse).ToList().Count > 0)
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// Prüft ob das Gebäude existiert.
        /// </summary>
        /// <param name="_gebäude"></param>
        /// <returns></returns>
        static public bool GebäudeExists(string _gebäude)
        {
            using (Entities db = new Entities())
            {
                if (db.Raeume.Where(a => a.Gebäude == _gebäude).ToList().Count > 0)
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// Prüft, ob der Raum existiert.
        /// </summary>
        /// <param name="_raum"></param>
        /// <returns></returns>
        static public bool RaumExists(string _raum)
        {
            int raum = _raum.ToInt32();
            using (Entities db = new Entities())
            {
                if (db.Raeume.Where(a => a.Raum == raum).ToList().Count > 0)
                    return true;
                else
                    return false;
            }
        }
        #endregion

        //SAIBL
        static public bool ZGAExists(int _AbteilungsID, DateTime _Zeit) //Prüft ob ein Eintrag mit dieser Abteilung + Zeit existiert
        {
            string Befehl = @"SELECT 
            [ZeitgesteuerteAnzeige].[AbteilungsID] AS AbteilungsID
       FROM [ZeitgesteuerteAnzeige]
      WHERE [AbteilungsID] = '" + _AbteilungsID + @"'
        AND [Zeit] = '" + _Zeit + "'";

            DataTable Daten = DatenbankAbfrage(Befehl);

            try
            {
                bool dummy = (Daten.Rows[0]["AbteilungsID"].ToInt32() > 0);
                return dummy;
            }
            catch { return false; }
        }
        static public Structuren.ZGA[] ZGAAbrufen(int _AbteilungsID) //Ruft alle ZGA der übergebenen Abteilung ab
        {
            string Befehl = @"SELECT 
            [ZeitgesteuerteAnzeige].[AbteilungsID] AS AbteilungsID,
            [ZeitgesteuerteAnzeige].[BetriebsmodeID] AS BetriebsmodeID,
            [ZeitgesteuerteAnzeige].[Zeit] AS Zeit
       FROM [ZeitgesteuerteAnzeige]
      WHERE [AbteilungsID] = '" + _AbteilungsID + "'";

            DataTable Daten = DatenbankAbfrage(Befehl);

            Structuren.ZGA[] ret = new Structuren.ZGA[Daten.Rows.Count];

            for(int i = 0; i < Daten.Rows.Count; i++)
            {
                ret[i].AbteilungsID = Daten.Rows[i]["AbteilungsID"].ToInt32();
                ret[i].BetriebsmodeID = Daten.Rows[i]["BetriebsmodeID"].ToInt32();
                ret[i].Zeit = Daten.Rows[i]["Zeit"].ToDateTime();
            }

            return ret;
        }

        static public Structuren.LE[] LehrerEigenschaftenAbrufen(int _AbtID, string _LEmode)
        {
            string Befehl = @"SELECT
		    [LehrerAbteilungen].[LehrerKuerzel]
		   ,[Klassen].[AbteilungsID] AS AbteilungsID
		   ,[Vorname]
		   ,[Nachname]
		   ,[Sprechstunde]
		   ,[Tag]
		   ,[Raum]
		   ,[Gebäude]
		   ,[Klasse]
	   FROM (([dbo].[LehrerAbteilungen]
  LEFT JOIN [dbo].[LehrerTesten1]
	     ON LehrerAbteilungen.LehrerKuerzel = LehrerTesten1.LehrerKuerzel)
  LEFT JOIN [dbo].[Klassen]
	     ON LehrerAbteilungen.LehrerKuerzel = Klassen.Klassenvorstand)
	  WHERE [LehrerAbteilungen].[AbteilungsID] = '" + _AbtID + @"'
   ORDER BY [LehrerKuerzel]
           ,[AbteilungsID]
           ,[Klasse]";

            DataTable Daten = DatenbankAbfrage(Befehl);

            Structuren.LE[] ret = new Structuren.LE[Daten.Rows.Count];

            //Initialisierung der Listen
            for (int i = 0; i < Daten.Rows.Count; i++)
            {
                ret[i].KlassenvorstandKlasse = null;
            }
            for (int i = 0; i < Daten.Rows.Count; i++)
            {
                ret[i].AbteilungsIDs = null;
            }

            int diff = 0;
            int reti, dateni;
            for (reti = 0, dateni = 0; dateni < Daten.Rows.Count; reti++)
            {
                ret[reti].LehrerKuerzel = Daten.Rows[dateni]["LehrerKuerzel"].ToString();
                ret[reti].Vorname = Daten.Rows[dateni]["Vorname"].ToString();
                ret[reti].Nachname = Daten.Rows[dateni]["Nachname"].ToString();
                ret[reti].Sprechstunde = Daten.Rows[dateni]["Sprechstunde"].ToInt32();
                ret[reti].Tag = Daten.Rows[dateni]["Tag"].ToInt32();
                ret[reti].Raum = Daten.Rows[dateni]["Raum"].ToInt32();
                ret[reti].Gebäude = Daten.Rows[dateni]["Gebäude"].ToString();

                if (Daten.Rows[dateni]["AbteilungsID"] != null)
                {
                    if (ret[reti].AbteilungsIDs == null)
                    {
                        ret[reti].AbteilungsIDs = new List<int>();
                    }
                    ret[reti].AbteilungsIDs.Add(Daten.Rows[dateni]["AbteilungsID"].ToInt32());
                }
                do
                {
                    if (Daten.Rows[dateni]["Klasse"] != null)
                    {
                        if (ret[reti].KlassenvorstandKlasse == null)
                        {
                            ret[reti].KlassenvorstandKlasse = new List<string>();
                        }
                        ret[reti].KlassenvorstandKlasse.Add(Daten.Rows[dateni]["Klasse"].ToString());
                    }
                    dateni++;
                }
                while (dateni < Daten.Rows.Count && Daten.Rows[dateni]["AbteilungsID"].ToInt32() == Daten.Rows[dateni - 1]["AbteilungsID"].ToInt32() && Daten.Rows[dateni]["LehrerKuerzel"].ToString() == Daten.Rows[dateni - 1]["LehrerKuerzel"].ToString());
            }
            diff = dateni - reti;
            Array.Resize<Structuren.LE>(ref ret, Daten.Rows.Count - diff);

            if (_LEmode == "Bearbeiten")
            {
                Befehl = @"SELECT
		    [LehrerAbteilungen].[LehrerKuerzel]
		   ,[AbteilungsID]
	   FROM [dbo].[LehrerAbteilungen]
	  WHERE [AbteilungsID] != '" + _AbtID + @"'
   ORDER BY [LehrerKuerzel]";

                Daten = DatenbankAbfrage(Befehl);

                for (int i = 0; i < Daten.Rows.Count; i++)
                {
                    for (int ii = 0; ii < ret.Length; ii++)
                    {
                        if (ret[ii].LehrerKuerzel == Daten.Rows[i]["LehrerKuerzel"].ToString())
                        {
                            ret[ii].AbteilungsIDs.Add(Daten.Rows[i]["AbteilungsID"].ToInt32());
                        }
                    }
                }
            }
            return ret;
        }
        static public int LehrerKuerzelExists(string _LehrerKuerzel, int _AbtID)
        {
            string Befehl = @"SELECT 
            [LehrerAbteilungen].[LehrerKuerzel]
           ,[AbteilungsID]
       FROM [LehrerAbteilungen]
      WHERE [LehrerKuerzel] = '" + _LehrerKuerzel + "'";

            DataTable Daten = DatenbankAbfrage(Befehl);

            if (Daten.Rows.Count == 0)
            {
                return 0; //LE anlegen
            }
            else
            {
                for (int i = 0; i < Daten.Rows.Count; i++)
                {
                    if (_AbtID == Daten.Rows[i]["AbteilungsID"].ToInt32())
                    {
                        return 2; //LE vorhanden
                    }
                }
                return 1; //Abt hinzufügen
            }
        }

        //Schönegger

        /// <summary>
        /// Ruft alle Lehrerkürzel einer Abteilung ab.
        /// Wird mit _abteilung ein -1 übergeben, werden alle Lehrerkürzel zurückgegeben.
        /// </summary>
        /// <param name="_abteilung">Die Abteilung aus welcher alle Lehrerkürzel abgerufen werden.</param>
        /// <returns>Alle Lehrerkürzel der Abteilung</returns>

        static public string[] LehrerKurzel(int _abteilung)
        {
            string befehl;
            DataTable daten;
            if (_abteilung == -1)
            {
                befehl = @"SELECT
[LehrerTesten1].[LehrerKuerzel] AS LehrerKuerzel
FROM [LehrerTesten1]
ORDER BY [LehrerTesten1].[LehrerKuerzel]";
            }
            else
            {
                befehl = @"SELECT
[LehrerAbteilungen].[LehrerKuerzel] AS LehrerKuerzel
FROM [LehrerAbteilungen]
WHERE [LehrerAbteilungen].[AbteilungsID]='" + _abteilung + @"'
ORDER BY [LehrerAbteilungen].[LehrerKuerzel]";
            }
            daten = DatenbankAbfrage(befehl);
            string[] temp = new string[daten.Rows.Count];
            for (int i = 0; i < daten.Rows.Count; i++)
                temp[i] = daten.Rows[i]["LehrerKuerzel"].ToString();
            return temp;
        }
        /// <summary>
        /// Prüft ob Fach vorhanden ist.
        /// </summary>
        /// <param name="_FachKuerzel">Die Fächer die geprüft werden.</param>
        /// <returns>bool true (vorhanden), bool false (nicht vorhanden)</returns>
        static public bool FachExists(string _FachKuerzel)
        {
            string befehl;
            DataTable daten;

            befehl = @"SELECT
[Fächer].[FachKürzel] AS FachKuerzel
FROM [Fächer]
WHERE [Fächer].[FachKürzel]='" + _FachKuerzel + "'";
          
            daten = DatenbankAbfrage(befehl);

            if (daten.Rows.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        /// <summary>
        /// Liefert die vorgezogenen Stunden eines Datums.
        /// </summary>
        /// <param name="_Datum">Das Datum, aus welcher die vorgezogenen Stunden geholt werden</param>
        /// <returns>INTEGER Array aller vorgezogenen Stunden dieses Datums</returns>
        static public int[] ZiehtVorStunde(DateTime _Datum, string _Klasse)
        {
            string befehl;
            DataTable daten;

            befehl = @"SELECT
[Supplierungen].[ZiehtVor] AS Stunde
FROM [Supplierungen]
WHERE [Supplierungen].[ZiehtVorDatum]='" + _Datum.ToString(Properties.Resources.sql_datumformat) + "' AND [Supplierungen].[Klasse]='" + _Klasse + "'";
            daten = DatenbankAbfrage(befehl);

            int[] stunden = new int[daten.Rows.Count];
            for(int i=0; i<daten.Rows.Count;i++)
            {
                stunden[i] = daten.Rows[i]["Stunde"].ToInt32();
            }
            return stunden;      
        }
        /// <summary>
        /// Liefert das Datum von dem die Stunde verschoben wurde.
        /// </summary>
        /// <param name="_Datum">Das Datum auf das die Stunde verschoben wurde</param>
        /// <returns>Datum von wo die Stunde verschoben wurde</returns>
        static public DateTime GetVerschiebtVonDatum(DateTime _Datum, string _Klasse, int _VorgezogeneStunde, out string _Ersatzlehrer, out string _Ersatzfach)
        {
            string befehl;
            DataTable daten;

            befehl = @"SELECT
[Supplierungen].[Datum] AS Datum,
[Supplierungen].[ErsatzLehrerKürzel] AS Ersatzlehrer,
[Supplierungen].[ErsatzFach] AS Ersatzfach
FROM [Supplierungen]
WHERE [Supplierungen].[ZiehtVorDatum]='" + _Datum.ToString(Properties.Resources.sql_datumformat) + "' AND [Supplierungen].[Klasse]='" + _Klasse + @"'
AND [Supplierungen].[ZiehtVor]='" + _VorgezogeneStunde + "'";
            daten = DatenbankAbfrage(befehl);

            DateTime ziehtVorAufDatum = daten.Rows[0]["Datum"].ToDateTime();
            _Ersatzlehrer = LehrerNamen(daten.Rows[0]["Ersatzlehrer"].ToString());
            _Ersatzfach = FachLang(daten.Rows[0]["Ersatzfach"].ToString());
            return ziehtVorAufDatum;
        }
        /// <summary>
        /// Liefert die Bildschrim ID einer Klasse zurück.
        /// </summary>
        /// <param name="_Klasse">Die Klasse von der die Bildschirm ID geholt wird</param>
        /// <returns>INT BildschirmID</returns>
        static public int GetBildschirmIDvonKlasse(string _Klasse)
        {
            string befehl;
            DataTable daten;

            befehl = @"SELECT
[Bildschirme].[BildschirmID] AS ID
FROM [Bildschirme] INNER JOIN [Raeume] ON [Bildschirme].[Raum] = [Raeume].[Raum]
WHERE [Raeume].[StandardKlasse]='" + _Klasse + "'";
            daten = DatenbankAbfrage(befehl);

            int id = daten.Rows[0]["ID"].ToInt32();
            
            return id;
        }
        /// <summary>
        /// Liefert die Stunden einer Klasse zurück, die entfallen sollen.
        /// </summary>
        /// <param name="_Klasse">Die Klasse von der die Stunden geholt werden</param>
        /// <returns>INT Array aller Stunden</returns>
        static public int[] GetStundenDieGlobalEntfallen(string _Klasse, int _Von, int _Bis, DateTime _Datum)
        {
            int tag = 0;
            switch (_Datum.DayOfWeek.ToString()) //Tag des Vorgezogenen Datums
            {
                case "Monday":
                    tag = 0;
                    break;
                case "Tuesday":
                    tag = 1;
                    break;
                case "Wednesday":
                    tag = 2;
                    break;
                case "Thursday":
                    tag = 3;
                    break;
                case "Friday":
                    tag = 4;
                    break;
                case "Saturday":
                    tag = 5;
                    break;
            }

            string befehl;
            DataTable daten;
            
            befehl = @"SELECT
[Stundenplan].[Stunde] AS Stunde
FROM [Stundenplan]
WHERE [Stundenplan].[Klasse]='" + _Klasse + @"'
AND [Stundenplan].[Stunde]>='" + _Von + @"'
AND [Stundenplan].[Stunde]<='" + _Bis + @"'
AND [Stundenplan].[Wochentag]='" + tag + "'";

            daten = DatenbankAbfrage(befehl);

            int[] stunden = new int[daten.Rows.Count];
            for (int i = 0; i < daten.Rows.Count; i++)
            {
                stunden[i] = daten.Rows[i]["Stunde"].ToInt32();
            }

            return stunden;
        }
        /// <summary>
        /// Liefert die ID des letzten globalen Entfalls zurück.
        /// </summary>
        /// <returns>INT ID des letzten Eintrags</returns>
        static public int GetLastGlobalEntfallID()
        {
            string befehl;
            DataTable daten;

            befehl = @"SELECT
[GlobalEntfall].[GlobalEnfallID] AS ID
FROM [GlobalEntfall]
ORDER BY [GlobalEntfall].[GlobalEnfallID]";

            daten = DatenbankAbfrage(befehl);

            if(daten.Rows.Count==0)
            {
                return 0;
            }

            return daten.Rows[daten.Rows.Count - 1]["ID"].ToInt32(); //liefert die letzte ID der Tabelle
        }

        /// <summary>
        /// Liefert die Globalen Entfälle einer Abteilung zurück.
        /// </summary>
        /// <param name="_AbtID">Die Abteilung von der die Entfälle geholt werden</param>
        /// <returns>GlobalEntfall Array aller Entfälle</returns>
        static public Structuren.GlobalEntfall[] GetGlobaleEntfaelle(int _AbtID)
        {
            string befehl;
            DataTable daten;

            befehl = @"SELECT
[GlobalEntfall].[AbteilungsID] AS AbteilungsID,
[GlobalEntfall].[Datum] AS Datum,
[GlobalEntfall].[VonStunde] AS VonStunde,
[GlobalEntfall].[BisStunde] AS BisStunde,
[GlobalEntfall].[GlobalEnfallID] AS ID
FROM [GlobalEntfall]
WHERE [GlobalEntfall].[AbteilungsID]='" + _AbtID + "' AND [GlobalEntfall].[Datum]>='" + DateTime.Today.ToString(Properties.Resources.sql_datumformat) + @"'
ORDER BY [GlobalEntfall].[Datum]";

            daten = DatenbankAbfrage(befehl);

            Structuren.GlobalEntfall[] alleEintraege = new Structuren.GlobalEntfall[daten.Rows.Count];
            Structuren.GlobalEntfall einEintrag;

            if (daten.Rows.Count == 0) return alleEintraege;

            for (int i = 0; i < daten.Rows.Count; i++)
            {
                einEintrag.AbteilungsID = daten.Rows[i]["AbteilungsID"].ToInt32();
                einEintrag.Datum = daten.Rows[i]["Datum"].ToDateTime();
                einEintrag.VonStunde = daten.Rows[i]["VonStunde"].ToInt32();
                einEintrag.BisStunde = daten.Rows[i]["BisStunde"].ToInt32();
                einEintrag.GlobalerEntfallID = daten.Rows[i]["ID"].ToInt32();
                alleEintraege[i] = einEintrag;
            }

            return alleEintraege;
        }

        /// <summary>
        /// Liefert den Stundenplan eines Lehrers für einen bestimmten Tag.
        /// </summary>
        /// <param name="_LehrerKuerzel">Der lehrer von dem der Stundenplan geholt wird</param>
        /// <returns></returns>
        static public Structuren.StundenplanTagLehrer[] GetLehrerStundenplanfürDatum(string _LehrerKuerzel, DateTime _Datum)
        {
            int tag = 0;
            switch (_Datum.DayOfWeek.ToString()) //Tag des Vorgezogenen Datums
            {
                case "Monday":
                    tag = 0;
                    break;
                case "Tuesday":
                    tag = 1;
                    break;
                case "Wednesday":
                    tag = 2;
                    break;
                case "Thursday":
                    tag = 3;
                    break;
                case "Friday":
                    tag = 4;
                    break;
                case "Saturday":
                    tag = 5;
                    break;
            }

            string befehl;
            DataTable daten;

            befehl = @"SELECT
[Stundenplan].[Stunde] AS Stunde,
[Stundenplan].[Klasse] AS Klasse,
[Stundenplan].[FachKürzel] AS FachKürzel
FROM [Stundenplan]
WHERE [Stundenplan].[LehrerKürzel] LIKE '%" + _LehrerKuerzel + "%' AND [Stundenplan].[Wochentag]='" + tag + @"'
ORDER BY [Stundenplan].[Stunde]";
            daten = DatenbankAbfrage(befehl);

            Structuren.StundenplanTagLehrer[] temp = new Structuren.StundenplanTagLehrer[daten.Rows.Count];

            if (daten.Rows.Count == 0) return temp;

            for (int i = 0; i < daten.Rows.Count; i++)
            {
                temp[i].Fach = daten.Rows[i]["FachKürzel"].ToString();
                temp[i].Klasse = daten.Rows[i]["Klasse"].ToString();
                temp[i].Stunde = daten.Rows[i]["Stunde"].ToInt32();
            }

            return temp;
        }

        /// <summary>
        /// Liefert den Stundenplan einer Klasse für einen bestimmten Tag.
        /// </summary>
        /// <param name="_Klasse">Die Klasse von der der Stundenplan geholt wird</param>
        /// <returns></returns>
        static public Structuren.StundenplanTagLehrer[] GetStundenplanfürDatum(string _Klasse, DateTime _Datum, bool _MitFreistunden = false)
        {
            int tag = 0;
            switch (_Datum.DayOfWeek.ToString()) //Tag des Vorgezogenen Datums
            {
                case "Monday":
                    tag = 0;
                    break;
                case "Tuesday":
                    tag = 1;
                    break;
                case "Wednesday":
                    tag = 2;
                    break;
                case "Thursday":
                    tag = 3;
                    break;
                case "Friday":
                    tag = 4;
                    break;
                case "Saturday":
                    tag = 5;
                    break;
            }

            string befehl;
            DataTable daten;

            befehl = @"SELECT
[Stundenplan].[Stunde] AS Stunde,
[Stundenplan].[LehrerKürzel] AS LehrerKürzel,
[Stundenplan].[FachKürzel] AS FachKürzel
FROM [Stundenplan]
WHERE [Stundenplan].[Klasse] = '" + _Klasse + "' AND [Stundenplan].[Wochentag]='" + tag + @"'
ORDER BY [Stundenplan].[Stunde]";
            daten = DatenbankAbfrage(befehl);

            Structuren.StundenplanTagLehrer[] temp = new Structuren.StundenplanTagLehrer[12];

            if (daten.Rows.Count == 0) return temp;

            if (_MitFreistunden == true)
            {
                for (int i0 = 0; i0 < 12; i0++)
                {
                    temp[i0].Stunde = i0;
                }

                for (int i1 = 0; i1 < daten.Rows.Count; i1++)
                {
                    for (int i2 = 0; i2 < temp.Length; i2++)
                    {
                        if (daten.Rows[i1]["Stunde"].ToInt32() == i2)
                        {
                            temp[i2].Fach = daten.Rows[i1]["FachKürzel"].ToString();
                            temp[i2].Lehrer = daten.Rows[i1]["LehrerKürzel"].ToString();
                            temp[i2].Stunde = daten.Rows[i1]["Stunde"].ToInt32();
                        }
                    }
                }
            }
            else
            {
                temp = new Structuren.StundenplanTagLehrer[daten.Rows.Count];

                for (int i1 = 0; i1 < daten.Rows.Count; i1++)
                {
                    temp[i1].Fach = daten.Rows[i1]["FachKürzel"].ToString();
                    temp[i1].Lehrer = daten.Rows[i1]["LehrerKürzel"].ToString();
                    temp[i1].Stunde = daten.Rows[i1]["Stunde"].ToInt32();
                }
            }

            return temp;
        }
        /// <summary>
        /// Liefert alle Lehrer und Klassen die vom Globalen Entfall betroffen sind als out-Parameter.
        /// </summary>
        /// <param name="_ID">DIe GlobalEntfall ID von der die Lehrer und Klassen abgerufen werden sollen</param>
        /// <returns></returns>
        static public void GetLehrerGlobalEntfall(int _ID, int _AbtID, out string[] Klassen, out string[] lehrernew, out DateTime datum, out int vonstunde, out int bisstunde)
        {

            List<string> lehrerliste = new List<string>();

            int count = 0;
            string befehl;
            DataTable daten;

            befehl = @"SELECT
[GlobalEntfall].[Datum] AS Datum,
[GlobalEntfall].[VonStunde] AS VonStunde,
[GlobalEntfall].[BisStunde] AS BisStunde
FROM [GlobalEntfall]
WHERE [GlobalEntfall].[AbteilungsID] = '" + _AbtID + "' AND [GlobalEntfall].[GlobalEnfallID]='" + _ID + @"'
ORDER BY [GlobalEntfall].[GlobalEnfallID]";
            daten = DatenbankAbfrage(befehl);

            datum = daten.Rows[0]["Datum"].ToDateTime();

            vonstunde = daten.Rows[0]["VonStunde"].ToInt32();
            bisstunde = daten.Rows[0]["BisStunde"].ToInt32();

            int tag = 0;
            switch (datum.DayOfWeek.ToString()) //Tag des Vorgezogenen Datums
            {
                case "Monday":
                    tag = 0;
                    break;
                case "Tuesday":
                    tag = 1;
                    break;
                case "Wednesday":
                    tag = 2;
                    break;
                case "Thursday":
                    tag = 3;
                    break;
                case "Friday":
                    tag = 4;
                    break;
                case "Saturday":
                    tag = 5;
                    break;
            }
            befehl = @"SELECT DISTINCT
[Stundenplan].[LehrerKürzel] AS LehrerKürzel
FROM [Stundenplan]
WHERE [Stundenplan].[Wochentag] = '" + tag + @"'
AND [Stundenplan].[Stunde]>='" + vonstunde + @"'
AND [Stundenplan].[Stunde]<='" + bisstunde + @"'
AND [Stundenplan].[LehrerKürzel] NOT LIKE '%/%'
ORDER BY [Stundenplan].[LehrerKürzel]";
            daten = DatenbankAbfrage(befehl);

            int i;
            for (i = 0; i < daten.Rows.Count; i++)
            {
                lehrerliste.Add(daten.Rows[i]["LehrerKürzel"].ToString());
            }

            befehl = @"SELECT DISTINCT
[Stundenplan].[LehrerKürzel] AS LehrerKürzel
FROM [Stundenplan]
WHERE [Stundenplan].[Wochentag] = '" + tag + @"'
AND [Stundenplan].[Stunde]>='" + vonstunde + @"'
AND [Stundenplan].[Stunde]<='" + bisstunde + @"'
AND [Stundenplan].[LehrerKürzel] LIKE '%/%'
ORDER BY [Stundenplan].[LehrerKürzel]";
            daten = DatenbankAbfrage(befehl);

            bool hinzufügenErster = true;
            bool hinzufügenZweiter = true;

            lehrernew = new string[lehrerliste.Count + daten.Rows.Count];

            int j;
            for (j = 0; j < daten.Rows.Count; j++)
            {
                for (int x = 0; x < lehrerliste.Count; x++)
                {
                    if (daten.Rows[j]["LehrerKürzel"].ToString().Remove(0, 5) == lehrerliste[x])
                    {
                        hinzufügenErster = false;
                    }
                    if (daten.Rows[j]["LehrerKürzel"].ToString().Remove(4, 5) == lehrerliste[x])
                    {
                        hinzufügenZweiter = false;
                    }
                }
                if (hinzufügenErster == true)
                {
                    lehrerliste.Add(daten.Rows[j]["LehrerKürzel"].ToString().Remove(0, 5));
                }
                else
                {
                    hinzufügenErster = true;
                }
                if (hinzufügenZweiter == true)
                {
                    lehrerliste.Add(daten.Rows[j]["LehrerKürzel"].ToString().Remove(4, 5));
                }
                else
                {
                    hinzufügenZweiter = true;
                }
            }

            lehrernew = lehrerliste.ToArray();

            Array.Sort(lehrernew);

            //for (int x = 0; x < lehrernew.Length; x++)
            //{
            //    System.Windows.Forms.MessageBox.Show(lehrernew[x]);
            //}


            befehl = @"SELECT DISTINCT
[Stundenplan].[Klasse] AS Klasse
FROM [Stundenplan]
WHERE [Stundenplan].[Wochentag] = '" + tag + @"'
AND [Stundenplan].[Stunde]>='" + vonstunde + @"'
AND [Stundenplan].[Stunde]<='" + bisstunde + @"'
ORDER BY [Stundenplan].[Klasse]";
            daten = DatenbankAbfrage(befehl);

            Klassen = new string[daten.Rows.Count];

            int z;
            for (z = 0; z < daten.Rows.Count; z++)
            {
                Klassen[z] = daten.Rows[z]["Klasse"].ToString();
            }


        }


        #region InfoScreenV6

        /// <summary>
        ///  Searches InfoscreenV6 Database for entries of a specific which contain a certain string 
        /// </summary>
        /// <param name="input">String to search for</param>
        /// <param name="column">Column to search for the string</param>
        /// <param name="table">Table which contains the column</param>
        /// <returns>List of Fächer names which contain the input string (Select distinct)</returns>
        static public List<string> ColumnLike(string table, string column, string input)
        {
            string sql = @"SELECT DISTINCT " + column + @" FROM " + table + @" WHERE " + column + @" LIKE '%" + input + @"%'";
            List<string> entries = new List<string>();
            DataTable data = DatenbankAbfrage(sql);

            foreach (DataRow row in data.Rows)
            {
                 entries.Add(row[column].ToString());
            }
            return entries;
        }

        /// <summary>
        /// Returns a DataTable with the Raum, ID, Klasse and Bezeichnung of every Screen
        /// </summary>
        /// <param name="abteilung">Department ID</param>
        /// <returns></returns>
        static public DataTable Raeume(int abteilung)
        {
            string sql = @"SELECT Raeume.Raum, Bildschirme.BildschirmID, Raeume.StandardKlasse, Raeume.Bezeichnung
                            FROM Raeume LEFT JOIN Bildschirme ON Raeume.Raum = Bildschirme.Raum 
                            WHERE Raeume.AbteilungsID = " + abteilung ;          
            DataTable data = DatenbankAbfrage(sql);
            return data;
        }

        /// <summary>
        /// Returns a list of all classes with no room associated
        /// </summary>
        /// <param name="abteilung">Department ID</param>
        /// <returns></returns>
        static public List<string> KlassenWithoutRaum(int abteilung)
        {
            string sql = @"SELECT 
                            Klasse FROM Klassen
                            WHERE Klasse NOT IN (SELECT StandardKlasse FROM Raeume WHERE AbteilungsID = '" + abteilung + "')";
            List<string> entries = new List<string>();
            DataTable data = DatenbankAbfrage(sql);

            foreach (DataRow row in data.Rows)
            {
                entries.Add(row["Klasse"].ToString());
            }
            return entries;
        }

        /// <summary>
        /// Returns a DataTable with Gebäude NR, Raum NR, Standardklasse and Klassenvorstand of every Room
        /// </summary>
        /// <param name="abteilung"></param>
        /// <returns></returns>
        static public DataTable RoomList(int abteilung)
        {
            string sql = @"
                        SELECT
                            Raeume.Gebäude,
                            Raeume.Raum,
                            Raeume.StandardKlasse,
                            Klassen.Klassenvorstand
                        FROM Raeume LEFT JOIN Klassen ON Raeume.StandardKlasse = Klassen.Klasse 
                        WHERE Raeume.AbteilungsID = " + abteilung + " AND NOT StandardKlasse = '' ORDER BY Raeume.Gebäude, Raeume.Raum";
            DataTable data = DatenbankAbfrage(sql);
            return data;
        }

        /// <summary>
        /// Method to get all lessons which are moved to a certain date of a class
        /// </summary>
        /// <param name="departmentID">Department of the affected class</param>
        /// <param name="classname">Affected class</param>
        /// <param name="date">Date which the lesson is moved to</param>
        /// <returns>Returns a list of a stundenplanentry which contains the new subject, new teacher, the lesson and the class, sorted by earlier lesson first. </returns>
        static public List<Structuren.StundenplanEntry> GetMovedLessonsOfDay(int departmentID, string classname, DateTime date)
        {
            string sql = @"
                        SELECT ErsatzFach, ErsatzLehrerKürzel, ZiehtVor, Datum    
                        FROM Supplierungen WHERE AbteilungsID = "+departmentID.ToString() + @" AND Klasse = '" + classname + "' AND ZiehtVorDatum='" + date.ToString("yyyy-MM-dd") + "' ORDER BY ZiehtVor";
            DataTable data = DatenbankAbfrage(sql);

            List<Structuren.StundenplanEntry> moved_lessons = new List<Structuren.StundenplanEntry>();

            foreach(DataRow row in data.Rows)
            {
                Structuren.StundenplanEntry lesson = new Structuren.StundenplanEntry();
                lesson.Fach = row.ItemArray[0].ToString();
                lesson.Lehrer = row.ItemArray[1].ToString();
                lesson.Stunde = row.ItemArray[2].ToInt32();
                lesson.ZiehtVorDatum = row.ItemArray[3].ToDateTime();
                moved_lessons.Add(lesson);
            }

            return moved_lessons;

        }
        /// <summary>
        /// Get subject abbreviation of subject name
        /// </summary>
        /// <param name="subject">Long subject name, split multiple subjects by '/'</param>
        /// <returns></returns>
        static public string GetSubjectAbbreviation(string subject)
        {
            string[] subjects = subject.Split('/');
            List<string> Abbreviations = new List<string>();
            string befehl;
            DataTable daten;

            foreach (string sub in subjects)
            {
                befehl = @"SELECT
                            FachKürzel
                            FROM [Fächer]
                            WHERE [Fächer].[Fach]='" + sub + "'";
                daten = DatenbankAbfrage(befehl);
                if (daten.Rows.Count > 0)
                    Abbreviations.Add(daten.Rows[0]["FachKürzel"].ToString());
                daten.Dispose();
            }
            return String.Join(", ", Abbreviations);
        }

        public static Structuren.Klasseneigenschaften GetClassProperties(string classname, int dep_ID)
        {
            Structuren.Klasseneigenschaften properties = new Structuren.Klasseneigenschaften();
            properties.klasse = classname;
            properties.abteilungsID = dep_ID;

            DataTable result = DatenbankAbfrage(
                "SELECT Klassensprecher, KlassensprecherName, Klassenvorstand, Klasseninfo FROM Klassen WHERE Klasse='" + classname + "' AND AbteilungsID=" + dep_ID);
     
            properties.klassensprecher = result.Rows[0]["Klassensprecher"].ToString();
            properties.klassensprecherName = result.Rows[0]["KlassensprecherName"].ToString();
            properties.klassenvorstand = result.Rows[0]["Klassenvorstand"].ToString();
            properties.klasseninfo = result.Rows[0]["Klasseninfo"].ToString();

            properties.raum = DatenbankAbfrage(
                "SELECT Raum FROM Raeume WHERE StandardKlasse='" + classname + "' AND AbteilungsID=" + dep_ID).Rows[0].ItemArray[0].ToString();
            properties.gebäude = DatenbankAbfrage(
               "SELECT Gebäude FROM Raeume WHERE StandardKlasse='" + classname + "' AND AbteilungsID=" + dep_ID).Rows[0].ItemArray[0].ToString();
            
            return properties;
        }

        public static string GetSettingValue(string key, string theme = "")
        {
            string sql = "SELECT Value FROM Settings WHERE VarKey = '" + key + "'" + (theme == "" ? "" : "AND Theme = '" + theme + "'");

            DataTable result = DatenbankAbfrage(sql);
            return result.Rows.Count > 0 ?  result.Rows[0].ItemArray[0].ToString() : "";
        }

        public static List<string> GetSettingThemes()
        {
            List<string> themes = new List<string>();
            string sql = "SELECT DISTINCT Theme FROM Settings WHERE Theme != ''";
            DataTable result = DatenbankAbfrage(sql);

            foreach(DataRow dr in result.Rows)
            {
                themes.Add(dr.ItemArray[0].ToString());
            }
            return themes;
        }

        public static Structuren.Tests ExamInRoom(string building_room_Number)
        {
            building_room_Number = StringHelper.ToValidRoomBuilding(building_room_Number, 4,2);
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            string lesson = AktuelleStunde().ToString();
            Structuren.Tests exam = new Structuren.Tests();

            string sql = "SELECT Klasse, FachKürzel, Datum, Stunde, Dauer, LehrerKürzel, Testarten.Testart, RaumID FROM Tests " +
                "LEFT JOIN Testarten ON Tests.TestartID = Testarten.TestartID WHERE Datum = '"
                + date + "' AND RaumID LIKE '%" + building_room_Number + "%' AND " + lesson + " >= Stunde AND " + lesson + " < Stunde + Dauer";

            DataTable result = DatenbankAbfrage(sql);

            if (result.Rows.Count < 1) return exam;

            exam.Klasse = result.Rows[0]["Klasse"].ToString();
            exam.Fach = result.Rows[0]["FachKürzel"].ToString();
            exam.Datum = result.Rows[0]["Datum"].ToDateTime();
            exam.Stunde = result.Rows[0]["Stunde"].ToInt32();
            exam.Dauer = result.Rows[0]["Dauer"].ToInt32();
            exam.Lehrer = result.Rows[0]["LehrerKürzel"].ToString();
            exam.Testart = result.Rows[0]["Testart"].ToString();
            exam.Raum = result.Rows[0]["RaumID"].ToString();

            return exam;
        }

        /// <summary>
        /// Gets all replaced teachers for a lesson
        /// </summary>
        /// <param name="lesson">The source lesson</param>
        /// <param name="className">The affected class</param>
        /// <param name="date">The date</param>
        /// <returns></returns>
        public static Structuren.Supplierungen GetReplacementOfLesson(Structuren.StundenplanEntry lesson, string className, DateTime date)
        {
            string sql = "SELECT * FROM Supplierungen WHERE " +
                            $"Klasse='{className}' AND Datum='{date.ToString("yyyy-MM-dd")}' AND Stunde={lesson.Stunde} ";
            DataTable result = DatenbankAbfrage(sql);

            // New empty replacement
            Structuren.Supplierungen replacement = new Structuren.Supplierungen();
            replacement.Datum = date;
            replacement.Entfällt = lesson.Entfällt;
            replacement.Stunde = lesson.Stunde;
            replacement.Raum = lesson.Raum.ToString();
            replacement.ZiehtVor = lesson.ZiehtVor;
            replacement.ZiehtVorDatum = lesson.ZiehtVorDatum;

            if (result.Rows.Count < 1) return replacement;

            string repTeachers = "";
            string origTeachers = "";

            // get all replaced teachers and new teachers
            for(int row = 0; row<result.Rows.Count; row++)
            {
                if (!repTeachers.Contains(result.Rows[row]["ErsatzLehrerKürzel"].ToString())) 
                    repTeachers+= result.Rows[row]["ErsatzLehrerKürzel"].ToString() + "/";
                if (!origTeachers.Contains(result.Rows[row]["StattLehrerKürzel"].ToString()))
                    origTeachers += result.Rows[row]["StattLehrerKürzel"].ToString() + "/";
            }

            replacement.Ersatzlehrer = repTeachers.Substring(0, repTeachers.Length - 1);
            replacement.Ursprungslehrer = origTeachers.Substring(0, origTeachers.Length - 1);

            return replacement;

        }


        #endregion

    }
}