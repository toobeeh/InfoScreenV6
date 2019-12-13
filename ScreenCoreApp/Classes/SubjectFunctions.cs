using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using Infoscreen_Verwaltung.classes;

namespace ScreenCoreApp.Classes
{
    public static class SubjectFunctions
    {
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
                daten = Infoscreen_Verwaltung.classes.DatenbankAbrufen.DatenbankAbfrage(befehl);
                if (daten.Rows.Count > 0)
                    FachL.Add(daten.Rows[0]["Fach"].ToString());
                daten.Dispose();
            }
            return String.Join(", ", FachL);
        }
        static public Structuren.StundenplanTag[] StundenplanAbrufen(string _Klasse, bool _LehrerAusschreiben = true, bool _FachAusschreiben = false)
        {
            string befehl = @"SELECT
[Abteilungen].[Samstag] AS Samstag
FROM
([Stundenplan] INNER JOIN [Abteilungen] ON  [Stundenplan].[AbteilungsID]=[Abteilungen].[AbteilungsID])
WHERE [Stundenplan].[Klasse]='" + _Klasse + "'";
            DataTable daten = Infoscreen_Verwaltung.classes.DatenbankAbrufen.DatenbankAbfrage(befehl);
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
            daten = Infoscreen_Verwaltung.classes.DatenbankAbrufen.DatenbankAbfrage(befehl);

            Structuren.StundenplanTag[] data = new Structuren.StundenplanTag[schultage];
            List<Structuren.StundenplanEntry> temp, dummy0;
            Structuren.StundenplanEntry temp2, dummy1;
            Structuren.StundenplanEntry[] temp3;
            bool normaleStunde = false;
            int i2 = 0;
            for (int i1 = 0; i1 < data.Length; i1++)
            {
                temp = new List<Structuren.StundenplanEntry>();
                while (i2 < daten.Rows.Count)
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
[Supplierungen].[ZiehtVor] AS ZiehtVor
FROM
([Supplierungen] LEFT JOIN [Fächer] ON [Supplierungen].[ErsatzFach]=[Fächer].[FachKürzel])
WHERE [Supplierungen].[Klasse]='" + _Klasse + @"'
AND [Supplierungen].[Datum]='" + nächstesDatum.ToString("yyyy-MM-dd") + "'";
                DataTable daten2 = Infoscreen_Verwaltung.classes.DatenbankAbrufen.DatenbankAbfrage(befehl);

                for (int i3 = 0; i3 < daten2.Rows.Count; i3++)
                {
                    int x = 0;
                    int temp4 = Convert.ToInt32(daten2.Rows[i3]["Stunde"]);
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
                        //temp3[x].ZiehtVorDatum = Convert.ToDateTime(daten2.Rows[i3]["ZiehtVorDatum"]);
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
                        //dummy1.ZiehtVorDatum = Convert.ToDateTime(daten2.Rows[i3]["ZiehtVorDatum"]);
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
                daten = Infoscreen_Verwaltung.classes.DatenbankAbrufen.DatenbankAbfrage(befehl);
                if (daten.Rows.Count > 0)
                    Namen.Add(daten.Rows[0]["LehrerName"].ToString());
                daten.Dispose();
            }
            return String.Join(", ", Namen);
        }

        //Schönegger

        /// <summary>
        /// Liefert die vorgezogenen Stunden eines Datums.
        /// </summary>
        /// <param name="_Datum">Das Datum, aus welcher die vorgezogenen Stunden geholt werden</param>
        /// <returns>INTEGER Array aller vorgezogenen Stunden dieses Datums</returns>
        static public int[] ZiehtVorStundeNeu(DateTime _Datum, string _Klasse)
        {
            string befehl;
            DataTable daten;

            befehl = @"SELECT
[Supplierungen].[ZiehtVor] AS Stunde
FROM [Supplierungen]
WHERE [Supplierungen].[ZiehtVorDatum]='" + _Datum.ToString("yyyy-MM-dd") + "' AND [Supplierungen].[Klasse]='" + _Klasse + "'";
            daten = Infoscreen_Verwaltung.classes.DatenbankAbrufen.DatenbankAbfrage(befehl);


            int[] stunden = new int[daten.Rows.Count];
            for (int i = 0; i < daten.Rows.Count; i++)
            {
                stunden[i] = daten.Rows[i]["Stunde"].ToInt32();
            }
            return stunden;
        }

        /// <summary>
        /// Liefert das Datum auf welches die Stunde verschoben wird.
        /// </summary>
        /// <param name="_Datum">Das Datum von wo die Stunde verschoben wurde</param>
        /// <returns>Datum auf die die Stunde verschoben wurde</returns>
        static public DateTime GetVerschiebtVonDatum(DateTime _Datum, string _Klasse, int _VorgezogeneStunde, out string _Ersatzlehrer, out string _Ersatzfach)
        {
            string befehl;
            DataTable daten;

            befehl = @"SELECT
[Supplierungen].[Datum] AS Datum,
[Supplierungen].[ErsatzLehrerKürzel] AS Ersatzlehrer,
[Supplierungen].[ErsatzFach] AS Ersatzfach
FROM [Supplierungen]
WHERE [Supplierungen].[ZiehtVorDatum]='" + _Datum.ToString("yyyy-MM-dd") + "' AND [Supplierungen].[Klasse]='" + _Klasse + @"'
AND [Supplierungen].[ZiehtVor]='" + _VorgezogeneStunde + "'";
            daten = Infoscreen_Verwaltung.classes.DatenbankAbrufen.DatenbankAbfrage(befehl);

            DateTime ziehtVorAufDatum = daten.Rows[0]["Datum"].ToDateTime();
            _Ersatzlehrer = daten.Rows[0]["Ersatzlehrer"].ToString();
            _Ersatzfach = daten.Rows[0]["Ersatzfach"].ToString();
            return ziehtVorAufDatum;
        }
        /// <summary>
        /// Liefert die vorgezogenen Stunden eines Datums.
        /// </summary>
        /// <param name="_Datum">Das Datum, aus welcher die vorgezogenen Stunden geholt werden</param>
        /// <returns>INTEGER Array aller vorgezogenen Stunden dieses Datums</returns>
        static public string KvAusKlasse(string _Klasse)
        {
            string befehl;
            DataTable daten;

            befehl = @"SELECT
[Klassen].[Klassenvorstand] AS KV
FROM [Klassen]
WHERE [Klassen].[Klasse]='" + _Klasse + "'";

            daten = Infoscreen_Verwaltung.classes.DatenbankAbrufen.DatenbankAbfrage(befehl);
            string KlassenVorstand;
            try
            {
                KlassenVorstand = daten.Rows[0]["KV"].ToString();
            }
            catch
            {
                KlassenVorstand = "";
            }
            return KlassenVorstand;
        }
        /// <summary>
        /// Gibt die Supplierungen einer Abteilung welche aus der übergebenen Bildschirm-ID referenziert werden als Structuren.LehrerSupplierungen Array zurück.
        /// !!!ACHTUNG!!! - Das Element Ursprungslehrer der Structur Supplierungen muss nicht zwingend zugewiesen werden, !!!!!
        /// !!!!!!!!!!!!!   da es gleich dem Lehrer ist!!!                                                                !!!!!
        /// </summary>
        /// <param name="_ID">Die ID des Bildschirmes aus welcher die Abteilung referenziert wird</param>
        /// <returns>Die Supplierungen der referenzierten Abteilung</returns>
        static public Structuren.LehrerSupplierungen[] SupplierplanAbrufen(string _ID, bool _RichtigerName = true, string _LehrerKuerzel = "", string _Klasse = "", bool _Fachausschreiben = true)
        {
            string befehl = "";
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
CASE WHEN [Stundenplan].[Standardraum] = 1 THEN [Raeume].[Gebäude] ELSE [Stundenplan].[Gebäude] END AS Gebäude,
CASE WHEN [Stundenplan].[Standardraum] = 1 THEN [Raeume].[Raum] ELSE [Stundenplan].[Raum] END AS Raum,
CASE WHEN [Fächer].[Fach] IS NULL THEN [Supplierungen].[ErsatzFach] ELSE [Fächer].[Fach] END AS Fach
FROM
(((([Abteilungen] INNER JOIN [Supplierungen] ON [Abteilungen].[AbteilungsID]=[Supplierungen].[AbteilungsID])
INNER JOIN [Stundenplan] ON [Supplierungen].[Klasse]=[Stundenplan].[Klasse] AND [Supplierungen].[Stunde]=[Stundenplan].[Stunde])
INNER JOIN [Raeume] ON [Supplierungen].[Klasse]=[Raeume].[StandardKlasse])
LEFT JOIN [Fächer] ON [Supplierungen].[ErsatzFach]=[Fächer].[FachKürzel])
WHERE (([Supplierungen].[Datum]>='" + DateTime.Today.ToString("yyyy-MM-dd") + @"' OR [Supplierungen].[ZiehtVorDatum]>='" + DateTime.Today.ToString("yyyy-MM-dd") + @"')
AND [Abteilungen].[Abteilungsname]='" + _ID + @"')
ORDER BY [Supplierungen].[ErsatzLehrerKürzel], [Supplierungen].[Datum] , [Supplierungen].[Stunde], [Supplierungen].[Klasse]";
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
CASE WHEN [Stundenplan].[Standardraum] = 1 THEN [Raeume].[Gebäude] ELSE [Stundenplan].[Gebäude] END AS Gebäude,
CASE WHEN [Stundenplan].[Standardraum] = 1 THEN [Raeume].[Raum] ELSE [Stundenplan].[Raum] END AS Raum,
CASE WHEN [Fächer].[Fach] IS NULL THEN [Supplierungen].[ErsatzFach] ELSE [Fächer].[Fach] END AS Fach
FROM
(((([Abteilungen] INNER JOIN [Supplierungen] ON [Abteilungen].[AbteilungsID]=[Supplierungen].[AbteilungsID])
INNER JOIN [Stundenplan] ON [Supplierungen].[Klasse]=[Stundenplan].[Klasse] AND [Supplierungen].[Stunde]=[Stundenplan].[Stunde])
INNER JOIN [Raeume] ON [Supplierungen].[Klasse]=[Raeume].[StandardKlasse])
LEFT JOIN [Fächer] ON [Supplierungen].[ErsatzFach]=[Fächer].[FachKürzel])
WHERE (([Supplierungen].[Datum]>='" + DateTime.Today.ToString("yyyy-MM-dd") + @"' OR [Supplierungen].[ZiehtVorDatum]>='" + DateTime.Today.ToString("yyyy-MM-dd") + @"')
AND [Abteilungen].[Abteilungsname]='" + _ID + @"'
AND [Supplierungen].[ErsatzLehrerKürzel] LIKE '%" + _LehrerKuerzel + @"%')
OR [Supplierungen].[Datum]>='" + DateTime.Today.ToString("yyyy-MM-dd") + @"' OR [Supplierungen].[ZiehtVorDatum]>='" + DateTime.Today.ToString("yyyy-MM-dd") + @"')
AND (([Abteilungen].[Abteilungsname]='" + _ID + @"'
AND [Supplierungen].[StattLehrerKürzel] LIKE '%" + _LehrerKuerzel + @"%'
AND [Supplierungen].[Entfällt]='1" + @"')
ORDER BY [Supplierungen].[ErsatzLehrerKürzel], [Supplierungen].[Datum] , [Supplierungen].[Stunde], [Supplierungen].[Klasse]";
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
CASE WHEN [Stundenplan].[Standardraum] = 1 THEN [Raeume].[Gebäude] ELSE [Stundenplan].[Gebäude] END AS Gebäude,
CASE WHEN [Stundenplan].[Standardraum] = 1 THEN [Raeume].[Raum] ELSE [Stundenplan].[Raum] END AS Raum,
CASE WHEN [Fächer].[Fach] IS NULL THEN [Supplierungen].[ErsatzFach] ELSE [Fächer].[Fach] END AS Fach
FROM
(((([Abteilungen] INNER JOIN [Supplierungen] ON [Abteilungen].[AbteilungsID]=[Supplierungen].[AbteilungsID])
INNER JOIN [Stundenplan] ON [Supplierungen].[Klasse]=[Stundenplan].[Klasse] AND [Supplierungen].[Stunde]=[Stundenplan].[Stunde])
INNER JOIN [Raeume] ON [Supplierungen].[Klasse]=[Raeume].[StandardKlasse])
LEFT JOIN [Fächer] ON [Supplierungen].[ErsatzFach]=[Fächer].[FachKürzel])
WHERE (([Supplierungen].[Datum]>='" + DateTime.Today.ToString("yyyy-MM-dd") + @"' OR [Supplierungen].[ZiehtVorDatum]>='" + DateTime.Today.ToString("yyyy-MM-dd") + @"')
AND [Abteilungen].[Abteilungsname]='" + _ID + @"'
AND [Supplierungen].[Klasse]='" + _Klasse + @"')
OR (([Supplierungen].[Datum]>='" + DateTime.Today.ToString("yyyy-MM-dd") + @"' OR [Supplierungen].[ZiehtVorDatum]>='" + DateTime.Today.ToString("yyyy-MM-dd") + @"')
AND [Abteilungen].[Abteilungsname]='" + _ID + @"'
AND [Supplierungen].[Klasse]='" + _Klasse + @"'
AND [Supplierungen].[Entfällt]='1" + @"')
ORDER BY [Supplierungen].[ErsatzLehrerKürzel], [Supplierungen].[Datum] , [Supplierungen].[Stunde] , [Supplierungen].[Klasse]";
                        }
                    }
                }
            }
            else
            {
                befehl = @"SELECT
[Supplierungen].[ErsatzFach] AS ErsatzFach,
[Supplierungen].[StattLehrerKürzel] AS StattLehrerKürzel,
[Supplierungen].[ErsatzLehrerKürzel] AS ErsatzLehrerKürzel,
[Supplierungen].[Entfällt] AS Entfällt,
[Supplierungen].[ZiehtVor] AS ZiehtVor,
[Supplierungen].[ZiehtVorDatum] AS ZiehtVorDatum,
[Supplierungen].[Datum] AS Datum,
[Supplierungen].[Klasse] AS Klasse,
[Supplierungen].[Stunde] AS Stunde,
CASE WHEN [Stundenplan].[Standardraum] = 1 THEN [Raeume].[Gebäude] ELSE [Stundenplan].[Gebäude] END AS Gebäude,
CASE WHEN [Stundenplan].[Standardraum] = 1 THEN [Raeume].[Raum] ELSE [Stundenplan].[Raum] END AS Raum
FROM
(((([Bildschirme] INNER JOIN [Supplierungen] ON [Bildschirme].[AbteilungsID]=[Supplierungen].[AbteilungsID])
INNER JOIN [Stundenplan] ON [Supplierungen].[Klasse]=[Stundenplan].[Klasse] AND [Supplierungen].[Stunde]=[Stundenplan].[Stunde])
INNER JOIN [Raeume] ON [Supplierungen].[Klasse]=[Raeume].[StandardKlasse])
LEFT JOIN [Fächer] ON [Supplierungen].[ErsatzFach]=[Fächer].[FachKürzel])
WHERE (([Supplierungen].[Datum]>='" + DateTime.Today.ToString("yyyy-MM-dd") + @"' OR [Supplierungen].[ZiehtVorDatum]>='" + DateTime.Today.ToString("yyyy-MM-dd") + @"')
AND [Bildschirme].[BildschirmID]='" + _ID + "')";
                //ORDER BY L1.[Nachname]";
            }
            DataTable daten = Infoscreen_Verwaltung.classes.DatenbankAbrufen.DatenbankAbfrage(befehl);
            if (daten.Rows.Count == 0) return new Structuren.LehrerSupplierungen[0];

            List<Structuren.LehrerSupplierungen> data = new List<Structuren.LehrerSupplierungen>();
            Structuren.LehrerSupplierungen temp = new Structuren.LehrerSupplierungen();
            temp.Lehrer = _RichtigerName ? LehrerNamen(daten.Rows[0]["ErsatzLehrerKürzel"].ToString()) : daten.Rows[0]["ErsatzLehrerKürzel"].ToString();
            List<Structuren.Supplierungen> temp2 = new List<Structuren.Supplierungen>();
            Structuren.Supplierungen temp3;

            string tempx;

            for (int i = 0; i < daten.Rows.Count; i++)
            {
                tempx = _RichtigerName ? LehrerNamen(daten.Rows[i]["ErsatzLehrerKürzel"].ToString()) : daten.Rows[i]["ErsatzLehrerKürzel"].ToString();
                if (tempx != temp.Lehrer)
                {
                    temp.Supplierungen = temp2.ToArray();
                    data.Add(temp);
                    temp2.Clear();
                    temp.Lehrer = _RichtigerName ? LehrerNamen(daten.Rows[i]["ErsatzLehrerKürzel"].ToString()) : daten.Rows[i]["ErsatzLehrerKürzel"].ToString();
                }

                temp3 = new Structuren.Supplierungen();
                temp3.Datum = Convert.ToDateTime(daten.Rows[i]["Datum"]);
                temp3.Entfällt = Convert.ToBoolean(daten.Rows[i]["Entfällt"]);
                temp3.Ersatzfach = _Fachausschreiben ? FachLang(daten.Rows[i]["ErsatzFach"].ToString()) : daten.Rows[i]["ErsatzFach"].ToString();
                temp3.Ersatzlehrer = _RichtigerName ? LehrerNamen(daten.Rows[i]["ErsatzLehrerKürzel"].ToString()) : daten.Rows[i]["ErsatzLehrerKürzel"].ToString();
                temp3.Klasse = daten.Rows[i]["Klasse"].ToString();
                temp3.Stunde = Convert.ToInt32(daten.Rows[i]["Stunde"]);
                temp3.ZiehtVor = Convert.ToInt32(daten.Rows[i]["ZiehtVor"]);
                //temp3.ZiehtVorDatum = Convert.ToDateTime(daten.Rows[i]["ZiehtVorDatum"]);
                temp3.Ursprungslehrer = _RichtigerName ? LehrerNamen(daten.Rows[i]["StattLehrerKürzel"].ToString()) : daten.Rows[i]["StattLehrerKürzel"].ToString();
                string raum = "";
                if (daten.Rows[i]["Gebäude"].ToString() != "" && daten.Rows[i]["Raum"].ToString() != "") raum = daten.Rows[i]["Gebäude"].ToString() + "-" + daten.Rows[i]["Raum"].ToString();
                else if (daten.Rows[i]["Gebäude"].ToString() != "") raum = daten.Rows[i]["Gebäude"].ToString();
                else if (daten.Rows[i]["Raum"].ToString() != "") raum = daten.Rows[i]["Raum"].ToString();
                temp3.Raum = raum;
                temp2.Add(temp3);
            }

            temp.Supplierungen = temp2.ToArray();
            data.Add(temp);

            return data.ToArray();
        }
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
                daten = Infoscreen_Verwaltung.classes.DatenbankAbrufen.DatenbankAbfrage(befehl);
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
            daten = Infoscreen_Verwaltung.classes.DatenbankAbrufen.DatenbankAbfrage(befehl);
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
    }
}
