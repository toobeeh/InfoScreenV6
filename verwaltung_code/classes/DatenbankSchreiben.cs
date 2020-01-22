using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Infoscreen_Verwaltung.classes
{
    public class DatenbankSchreiben
    {
        static public void AbteilungsinfoSetzen(string _Abteilung, string _Abteilungsinfo)
        {
            string befehl = @"UPDATE [Abteilungen]
SET [AbteilungsInfo] = '" + _Abteilungsinfo + @"'
 WHERE [Abteilungsname]='" + _Abteilung + "'";
            DatenbankAbrufen.DatenbankAbfrage(befehl);
        }

        static public void KlasseninfoSetzen(string _Klasse, string _Klasseninfo)
        {
            string befehl = @"UPDATE [Klassen]
SET [Klasseninfo] = '" + _Klasseninfo + @"'
 WHERE [Klasse]='" + _Klasse + "'";
            DatenbankAbrufen.DatenbankAbfrage(befehl);
        }

        static public void TestLöschen(string _Klasse, DateTime _Datum, int _Stunde, string _Raum)
        {
            string befehl = @"DELETE
FROM [Tests]
WHERE [Klasse]='" + _Klasse + @"' AND
[Datum]='" + _Datum.ToString(Properties.Resources.sql_datumformat) + @"' AND
[Stunde]='" + _Stunde.ToString() + @"' AND [RaumID]='" + _Raum + "'";
            DatenbankAbrufen.DatenbankAbfrage(befehl);
        }

        static public bool FachPrüfen(string _FachKürzel, bool _Anlegen = true)
        {
            string befehl;
            befehl = @"SELECT
[Fächer].[FachKürzel] AS FachKürzel
FROM [Fächer]
WHERE [Fächer].[FachKürzel]='" + _FachKürzel + "'";
            if (DatenbankAbrufen.DatenbankAbfrage(befehl).Rows.Count > 0) return true;

            if (!_Anlegen) return false;

            befehl = @"INSERT
INTO [Fächer]
([FachKürzel], [Fach])
VALUES ('" + _FachKürzel + @"',
'" + _FachKürzel + "')";
            DatenbankAbrufen.DatenbankAbfrage(befehl);
            return true;
        }

        static public void TestAnlegen(string _Klasse, Structuren.Tests _Test)
        {
            string befehl = @"INSERT
    INTO [Tests]
           ([AbteilungsID]
           ,[Klasse]
           ,[FachKürzel]
           ,[Datum]
           ,[Stunde]
           ,[Dauer]
           ,[LehrerKürzel]
           ,[TestartID]
           ,[RaumID])
     VALUES
           ('" + DatenbankAbrufen.GetAbteilungsIDVonKlasse(_Klasse) + @"'
           ,'" + _Klasse + @"'
           ,'" + _Test.Fach + @"'
           ,'" + _Test.Datum.ToString(Properties.Resources.sql_datumformat) + @"'
           ,'" + _Test.Stunde + @"'
           ,'" + _Test.Dauer.ToInt32() + @"'
           ,'" + _Test.Lehrer + @"'
           ,'" + _Test.Testart + @"'
           ,'" + _Test.Raum + "')";


            DatenbankAbrufen.DatenbankAbfrage(befehl);
        }

        static public void TestÄndern(string _Klasse, Structuren.Tests _Neu, Structuren.Tests _Alt)
        {
            string befehl = @"UPDATE [Tests]
   SET [FachKürzel] = '" + _Neu.Fach + @"'
      ,[Datum] = '" + _Neu.Datum.ToString(Properties.Resources.sql_datumformat) + @"'
      ,[Stunde] = '" + _Neu.Stunde + @"'
      ,[Dauer] = '" + _Neu.Dauer.ToInt32() + @"'
      ,[LehrerKürzel] = '" + _Neu.Lehrer + @"'
      ,[TestartID] = '" + _Neu.Testart + @"'
      ,[RaumID] = ' " + _Neu.Raum + @"'
 WHERE
       [AbteilungsID] = '" + DatenbankAbrufen.GetAbteilungsIDVonKlasse(_Klasse) + @"' AND
       [Klasse] = '" + _Klasse + @"' AND
       [FachKürzel] = '" + DatenbankAbrufen.GetFachKürzelVonFach(_Alt.Fach) + @"' AND
       [Datum] = '" + _Alt.Datum.ToString(Properties.Resources.sql_datumformat) + @"' AND
       [Stunde] = '" + _Alt.Stunde + @"' AND
       [Dauer] = '" + _Alt.Dauer.ToInt32() + @"' AND
       [LehrerKürzel] = '" + _Alt.Lehrer + @"'   AND
       [RaumID] = '" + _Alt.Raum + "'";

            DatenbankAbrufen.DatenbankAbfrage(befehl);
        }

        static public void AktuellenBetriebsmodeÄndern(string _Abteilung, int _Betriebsmode)
        {
            string befehl = @"UPDATE [Abteilungen]
   SET [StandardBetriebsmode] = '" + _Betriebsmode.ToString() + @"'
 WHERE [Abteilungsname] = '" + _Abteilung + "'";

            DatenbankAbrufen.DatenbankAbfrage(befehl);
        }

        static public void BetriebsmodeBezeichnungÄndern(string _NeueBezeichnung, int _BetriebsmodeID)
        {
            string befehl = @"UPDATE [Betriebsmodi]
   SET [Bezeichnung] = '" + _NeueBezeichnung + @"'
 WHERE [BetriebsmodeID] = '" + _BetriebsmodeID.ToString() + "'";

            DatenbankAbrufen.DatenbankAbfrage(befehl);
        }

        static public void BetriebsmodeLöschen(int _BetriebsmodeID)
        {
            string befehl = @"DELETE FROM [Betriebsmodi]
      WHERE [BetriebsmodeID] = '" + _BetriebsmodeID.ToString() + "'";
            DatenbankAbrufen.DatenbankAbfrage(befehl);

            befehl = @"DELETE FROM [Dateien]
      WHERE [BetriebsmodeID] = '" + _BetriebsmodeID.ToString() + "'";
            DatenbankAbrufen.DatenbankAbfrage(befehl);

            befehl = @"DELETE FROM [Betriebsanzeige]
      WHERE [BetriebsmodeID] = '" + _BetriebsmodeID.ToString() + "'";
            DatenbankAbrufen.DatenbankAbfrage(befehl);

            try { System.IO.Directory.Delete(Properties.Resources.speicherort + _BetriebsmodeID.ToString(), true); }
            catch { }

            //SAIBL - Löscht beim Entfernen eines Betriebsmodus auch alle ZGA mit diesem
            befehl = @"DELETE FROM [ZeitgesteuerteAnzeige]
      WHERE [BetriebsmodeID] = '" + _BetriebsmodeID + "'";

            DatenbankAbrufen.DatenbankAbfrage(befehl);
        }

        /// <summary>
        /// Erstellt einen neuen Betriebsmode mit der angegebenen Bezeichnung.
        /// </summary>
        /// <param name="_Bezeichnung">Die Bezeichnung für den neuen Betriebsmode.</param>
        /// <returns>Gibt die ID des angelegten Betriebsmode zurück.</returns>
        static public int BetriebsmodeErstellen(string _Bezeichnung)
        {
            string befehl = @"INSERT INTO [Betriebsmodi]
           ([Bezeichnung])
     VALUES ('" + _Bezeichnung + "')";
            DatenbankAbrufen.DatenbankAbfrage(befehl);

            befehl = @"SELECT
[Betriebsmodi].[BetriebsmodeID] AS ID
FROM [Betriebsmodi]
WHERE [Betriebsmodi].[Bezeichnung]='" + _Bezeichnung + @"'
ORDER BY [Betriebsmodi].[BetriebsmodeID] DESC";
            DataTable daten = DatenbankAbrufen.DatenbankAbfrage(befehl);
            int ret = 0;
            try
            {
                ret = daten.Rows[0]["ID"].ToInt32();
                System.IO.Directory.CreateDirectory(Properties.Resources.speicherort + ret.ToString());
            }
            catch { }
            return ret;
        }

        static public void BetriebsmodeBildschirmHinzufügen(int _BetriebsmodeID, int _BildschirmID, Structuren.AnzeigeElemente _Anzeigen)
        {
            string befehl = @"INSERT INTO [Betriebsanzeige]
           ([BildschirmID]
           ,[BetriebsmodeID]
           ,[DateiID]
           ,[Stundenplan]
           ,[Klasseninfo]
           ,[Abteilungsinfo]
           ,[Sprechstunden]
           ,[Raumaufteilung]
           ,[Supplierplan]
           ,[AktuelleSupplierungen])
     VALUES
           ('" + _BildschirmID + @"'
           ,'" + _BetriebsmodeID + @"'
           ,'" + _Anzeigen.PowerPoints.ToInt32().ToString() + @"'
           ,'" + _Anzeigen.Stundenplan.ToInt32().ToString() + @"'
           ,'" + _Anzeigen.Stundenplan.ToInt32().ToString() + @"'
           ,'" + _Anzeigen.Abteilungsinfo.ToInt32().ToString() + @"'
           ,'" + _Anzeigen.Sprechstunden.ToInt32().ToString() + @"'
           ,'" + _Anzeigen.Raumaufteilung.ToInt32().ToString() + @"'
           ,'" + _Anzeigen.Supplierplan.ToInt32().ToString() + @"'
           ,'" + _Anzeigen.AktuelleSupplierungen.ToInt32().ToString() + "')";
            DatenbankAbrufen.DatenbankAbfrage(befehl);
        }

        static public void BetriebsmodeBildschirmEinerAbteilungHinzufügen(int _BetriebsmodeID, string _Abteilungsname)
        {
            int[] data = DatenbankAbrufen.BildschirmeEinerAbteilungAbrufen(_Abteilungsname);

            Structuren.AnzeigeElemente temp = new Structuren.AnzeigeElemente();
            temp.Abteilungsinfo = false;
            temp.AktuelleSupplierungen = false;
            temp.PowerPoints = -1;
            temp.Raumaufteilung = false;
            temp.Sprechstunden = false;
            temp.Stundenplan = false;
            temp.Supplierplan = false;

            for (int i = 0; i < data.Length; i++)
            {
                BetriebsmodeBildschirmHinzufügen(_BetriebsmodeID, data[i], temp);
            }
        }

        /// <summary>
        /// Ändert die Anzeigeeinstellungen des übergebenen Bildschirmes für den übergebenen Betriebsmode.
        /// </summary>
        /// <param name="_BetriebsmodeID">Der Betriebsmode, für welchen geänbdert werden soll.</param>
        /// <param name="_BildschirmID">Der Bildschirm, dessen Anzeigeeinstellungen geändert werden sollen.</param>
        /// <param name="_AnzeigeEinstellungen">Die neuen Anzeigeeinstellungen.</param>
        static public void AnzeigeeinstellungenUpdaten(int _BetriebsmodeID, int _BildschirmID, Structuren.AnzeigeElemente _AnzeigeEinstellungen)
        {
            string befehl = @"UPDATE [Betriebsanzeige]
   SET [DateiID] = '" + _AnzeigeEinstellungen.PowerPoints.ToString() + @"'
      ,[Stundenplan] = '" + _AnzeigeEinstellungen.Stundenplan.ToInt32().ToString() + @"'
      ,[Klasseninfo] = '" + _AnzeigeEinstellungen.Stundenplan.ToInt32().ToString() + @"'
      ,[Abteilungsinfo] = '" + _AnzeigeEinstellungen.Abteilungsinfo.ToInt32().ToString() + @"'
      ,[Sprechstunden] = '" + _AnzeigeEinstellungen.Sprechstunden.ToInt32().ToString() + @"'
      ,[Raumaufteilung] = '" + _AnzeigeEinstellungen.Raumaufteilung.ToInt32().ToString() + @"'
      ,[Supplierplan] = '" + _AnzeigeEinstellungen.Supplierplan.ToInt32().ToString() + @"'
      ,[AktuelleSupplierungen] = '" + _AnzeigeEinstellungen.AktuelleSupplierungen.ToInt32().ToString() + @"'
 WHERE [BildschirmID] = '" + _BildschirmID.ToString() + @"'
   AND [BetriebsmodeID] = '" + _BetriebsmodeID.ToString() + "'";
            DatenbankAbrufen.DatenbankAbfrage(befehl);
        }

        /// <summary>
        /// Legt eine neue Datei mit dem übergebenen Namen für den übergebenen Betriebsmode an und gibt die ID der Datei zurück.
        /// </summary>
        /// <param name="_BetriebsmodeID">Die ID des Betriebsmode</param>
        /// <param name="_Dateiname">Der Dateiname</param>
        /// <returns>Die ID der angelegten Datei</returns>
        static public int NeueDatei(int _BetriebsmodeID, string _Dateiname)
        {
            string befehl = @"INSERT INTO [Dateien]
           ([BetriebsmodeID], [Dateiname])
     VALUES ('" + _BetriebsmodeID.ToString() + "', '" + _Dateiname + "')";
            DatenbankAbrufen.DatenbankAbfrage(befehl);

            befehl = @"SELECT
[Dateien].[DateiID] AS ID
FROM [Dateien]
WHERE [Dateien].[Dateiname]='" + _Dateiname + @"'
ORDER BY [Dateien].[DateiID] DESC";
            DataTable daten = DatenbankAbrufen.DatenbankAbfrage(befehl);
            try { return daten.Rows[0]["ID"].ToInt32(); }
            catch { return -1; }
        }

        static public void DateiLöschen(int _DateiID)
        {
            string befehl = @"SELECT
[Dateien].[BetriebsmodeID] AS ID
FROM [Dateien]
WHERE [Dateien].[DateiID]='" + _DateiID + "'";
            DataTable daten = DatenbankAbrufen.DatenbankAbfrage(befehl);
            int id = daten.Rows[0]["ID"].ToInt32();

            befehl = @"DELETE FROM [Dateien]
      WHERE [DateiID] = '" + _DateiID + "'";
            DatenbankAbrufen.DatenbankAbfrage(befehl);

            try { System.IO.Directory.Delete(Properties.Resources.speicherort + id.ToString() + "\\" + _DateiID.ToString(), true); }
            catch { }
        }

        static public void StundenplanLeeren(string _klasse)
        {
            try
            {
                using (Entities db = new Entities())
                {
                    db.Stundenplan.RemoveRange(db.Stundenplan.Where(a => a.Klasse == _klasse));
                    db.SaveChanges();
                }
            }
            catch { }
        }

        static public void KlasseLöschen(string _klasse)
        {
            StundenplanLeeren(_klasse);
            //try
            //{
            //    using (Entities db = new Entities())
            //    {
            //        // Delete from klassen table
            //        db.Klassen.Remove(db.Klassen.Where(a => a.Klasse == _klasse).FirstOrDefault());
            //        // Delete from Supplierungen table
            //        db.Supplierungen.RemoveRange(db.Supplierungen.Where(a => a.Klasse == _klasse));
            //        // Delete from tests
            //        db.Tests.RemoveRange(db.Tests.Where(a => a.Klasse == _klasse));
            //        // delete from Räume
            //        db.Raeume.Where(a => a.StandardKlasse == _klasse).FirstOrDefault().StandardKlasse = null;
            //        db.SaveChanges();
            //    }
            //}
            //catch(Exception e) {

            //} 
            // InfoScreen V6: Didnt work :(((( Replaced with SQL query instead of entity object

            string Befehl;
            try
            {
                Befehl= @"DELETE FROM Klassen WHERE Klasse = '" + _klasse + "'";
                DatenbankAbrufen.DatenbankAbfrage(Befehl);

                Befehl = @"DELETE FROM Supplierungen WHERE Klasse = '" + _klasse + "'";
                DatenbankAbrufen.DatenbankAbfrage(Befehl);

                Befehl = @"DELETE FROM Tests WHERE Klasse = '" + _klasse + "'";
                DatenbankAbrufen.DatenbankAbfrage(Befehl);

                DeleteStandardklasseFromRaeume(_klasse, 1);
            }
            catch { }
            
        }



        //SAIBL
        static public void ZGASetzen(Structuren.ZGA _ZGA) //Speichert die übergebene ZGA in die DB
        {
            string Befehl = @"INSERT INTO [ZeitgesteuerteAnzeige]
           ([AbteilungsID]
           ,[BetriebsmodeID]
           ,[Zeit])
     VALUES
           ('" + _ZGA.AbteilungsID + @"'
           ,'" + _ZGA.BetriebsmodeID + @"'
           ,'" + _ZGA.Zeit + "')";

            DatenbankAbrufen.DatenbankAbfrage(Befehl);
        }
        static public void ZGAAendern(Structuren.ZGA _ZGAalt, Structuren.ZGA _ZGAneu) //Ändert die ZGA in der DB (Abteilung bleibt immer gleich)
        {
            string Befehl = @"UPDATE [ZeitgesteuerteAnzeige]
        SET [BetriebsmodeID] = '" + _ZGAneu.BetriebsmodeID + @"'
           ,[Zeit] = '" + _ZGAneu.Zeit + @"'
      WHERE [AbteilungsID] = '" + _ZGAalt.AbteilungsID + @"'
        AND [BetriebsmodeID] = '" + _ZGAalt.BetriebsmodeID + @"'
        AND [Zeit] = '" + _ZGAalt.Zeit + "'";

            DatenbankAbrufen.DatenbankAbfrage(Befehl);
        }
        static public void ZGALoeschen(Structuren.ZGA _ZGA) //Löscht die übergebene ZGA in der DB
        {
            string Befehl = @"DELETE FROM [ZeitgesteuerteAnzeige]
      WHERE [AbteilungsID] = '" + _ZGA.AbteilungsID + @"'
        AND [BetriebsmodeID] = '" + _ZGA.BetriebsmodeID + @"'
        AND [Zeit] = '" + _ZGA.Zeit + "'";

            DatenbankAbrufen.DatenbankAbfrage(Befehl);
        }
        static public void ZGAAbfragen(int _BildschirmID) //Überprüft ob die ZGA ausgelöst wurde, ändert den Standard-Betriebsmodus und löscht den Eintrag
        {
            string Befehl = @"SELECT 
            AbteilungsID
       FROM Bildschirme
      WHERE BildschirmID = " + _BildschirmID;

            DataTable Daten = DatenbankAbrufen.DatenbankAbfrage(Befehl);

            int AbtID = Daten.Rows[0]["AbteilungsID"].ToInt32();
            Structuren.ZGA[] dummy = DatenbankAbrufen.ZGAAbrufen(AbtID);

            if (dummy == null || dummy.Length == 0) return;

            if (DateTime.Compare(dummy[0].Zeit, DateTime.Now) == -1)
            {
                Befehl = @"UPDATE [Abteilungen]
            SET [StandardBetriebsmode] = '" + dummy[0].BetriebsmodeID + @"'
          WHERE [AbteilungsID] = '" + dummy[0].AbteilungsID + "'";

                DatenbankAbrufen.DatenbankAbfrage(Befehl);

                ZGALoeschen(dummy[0]);
            }
        }

        static public void KlasseneigenschaftenLoeschen(int _abtID)
        {
            string Befehl = @"DELETE FROM [Klassen]
      WHERE [AbteilungsID] = '" + _abtID + "'";

            DatenbankAbrufen.DatenbankAbfrage(Befehl);
        }
        
        static public void LehrerEigenschaftHinzufuegen(Structuren.LE _LE, int _Mode)
        {
            if (_Mode == 0)
            {
                string Befehl = "";

                foreach (int Abt in _LE.AbteilungsIDs)
                {
                    Befehl = @"INSERT INTO [LehrerAbteilungen]
               ([LehrerKuerzel]
               ,[AbteilungsID])
            VALUES
               ('" + _LE.LehrerKuerzel + @"'
               ,'" + Abt + "')";

                    DatenbankAbrufen.DatenbankAbfrage(Befehl);
                }

                Befehl = @"INSERT INTO [LehrerTesten1]
           ([LehrerKuerzel]
           ,[Vorname]
           ,[Nachname]
           ,[Sprechstunde]
           ,[Tag]
           ,[Raum]
           ,[Gebäude])
     VALUES
           ('" + _LE.LehrerKuerzel + @"'
           ,'" + _LE.Vorname + @"'
           ,'" + _LE.Nachname + @"'
           ,'" + _LE.Sprechstunde + @"'
           ,'" + _LE.Tag + @"'
           ,'" + _LE.Raum + @"'
           ,'" + _LE.Gebäude + "')";

                DatenbankAbrufen.DatenbankAbfrage(Befehl);

                foreach (string KV in _LE.KlassenvorstandKlasse)
                {
                    Befehl = @"UPDATE [Klassen]
               SET [Klassenvorstand] = '" + _LE.LehrerKuerzel + "' WHERE [Klasse] = '" + KV + "'";

                    DatenbankAbrufen.DatenbankAbfrage(Befehl);
                }
            }
            else
            {
                string Befehl = "";

                Befehl = @"INSERT INTO [LehrerAbteilungen]
               ([LehrerKuerzel]
               ,[AbteilungsID])
            VALUES
               ('" + _LE.LehrerKuerzel + @"'
               ,'" + _LE.AbteilungsIDs.First<int>() + "')";

                DatenbankAbrufen.DatenbankAbfrage(Befehl);
            }
        }

        static public void LehrerEigenschaftAendern(Structuren.LE[] _LEListe)
        {
            foreach (Structuren.LE LEEintrag in _LEListe)
            {
                string Befehl = @"UPDATE [LehrerTesten1]
        SET [Vorname] = '" + LEEintrag.Vorname + @"'
           ,[Nachname] = '" + LEEintrag.Nachname + @"'
           ,[Sprechstunde] = '" + LEEintrag.Sprechstunde + @"'
           ,[Tag] = '" + LEEintrag.Tag + @"'
           ,[Raum] = '" + LEEintrag.Raum + @"'
           ,[Gebäude] = '" + LEEintrag.Gebäude + @"'
      WHERE [LehrerKuerzel] = '" + LEEintrag.LehrerKuerzel + "'";
                DatenbankAbrufen.DatenbankAbfrage(Befehl);


                Befehl = @"DELETE FROM [LehrerAbteilungen]
      WHERE [LehrerKuerzel] = '" + LEEintrag.LehrerKuerzel + "'";
                DatenbankAbrufen.DatenbankAbfrage(Befehl);
                foreach (int Abt in LEEintrag.AbteilungsIDs)
                {
                    Befehl = @"INSERT INTO [LehrerAbteilungen]
               ([LehrerKuerzel]
               ,[AbteilungsID])
            VALUES
               ('" + LEEintrag.LehrerKuerzel + @"'
               ,'" + Abt + "')";

                    DatenbankAbrufen.DatenbankAbfrage(Befehl);
                }

                foreach (string Klasse in LEEintrag.KlassenvorstandKlasse)
                {
                    Befehl = @"UPDATE [Klassen]
                SET [Klassenvorstand] = '" + LEEintrag.LehrerKuerzel + "' WHERE [Klasse] = '" + Klasse + "'";
                    DatenbankAbrufen.DatenbankAbfrage(Befehl);
                }               
            }
        }

        static public void LehrerEigenschaftLöschen(string _LehrerKuerzel)
        {
            string Befehl = @"DELETE FROM [LehrerAbteilungen]
      WHERE [LehrerKuerzel] = '" + _LehrerKuerzel + "'";
            DatenbankAbrufen.DatenbankAbfrage(Befehl);

            Befehl = @"DELETE FROM [LehrerTesten1]
      WHERE [LehrerKuerzel] = '" + _LehrerKuerzel + "'";
            DatenbankAbrufen.DatenbankAbfrage(Befehl);

            Befehl = @"UPDATE [Klassen]
      SET [Klassenvorstand] = '' WHERE [Klassenvorstand] = '" + _LehrerKuerzel + "'";
            DatenbankAbrufen.DatenbankAbfrage(Befehl);
        }
        //Schönegger
        static public void RaeumeUpdaten(int _abtID, string _klasse, int _raum)
        {
            string Befehl = @"UPDATE [Raeume]
                SET [Raum] =" + _raum + @"
                WHERE ([Raeume].[StandardKlasse] ='" + _klasse + @"'
                AND [Raeume].[AbteilungsID] =" + _abtID + ")";


            DatenbankAbrufen.DatenbankAbfrage(Befehl);
        }
        static public void KlasseninfoUpdaten(int _abtID, string _klasse, string _klasseninfo)
        {
            string Befehl = @"UPDATE [Klassen]
                SET [Klasseninfo] = '" + _klasseninfo + @"'
                WHERE ([Klassen].[Klasse] ='" + _klasse + @"'
                AND [Klassen].[AbteilungsID] =" + _abtID + ")";

            DatenbankAbrufen.DatenbankAbfrage(Befehl);
        }
        
        static public void SupplierungenEintragen(int _abtID, Structuren.Supplierungen _sup, int _GlobalEntfall = -1, string _Grund = "")
        {
            string Befehl = @"INSERT INTO [Supplierungen]
            ([AbteilungsID]
            ,[Klasse]
            ,[Datum]
            ,[Stunde]
            ,[ErsatzFach]
            ,[ErsatzLehrerKürzel]
            ,[StattLehrerKürzel]
            ,[Entfällt]
            ,[ZiehtVor]
            ,[ZiehtVorDatum]
            ,[GlobalEntfall]
            ,[Grund])
          VALUES
           ('" + _abtID + @"'
           ,'" + _sup.Klasse + @"'
           ,'" + _sup.Datum.ToString(Properties.Resources.sql_datumformat) + @"'
           ,'" + _sup.Stunde + @"'
           ,'" + _sup.Ersatzfach + @"'
           ,'" + _sup.Ersatzlehrer + @"'
           ,'" + _sup.Ursprungslehrer + @"'
           ,'" + _sup.Entfällt + @"'
           ,'" + _sup.ZiehtVor + @"'
           ,'" + _sup.ZiehtVorDatum.ToString(Properties.Resources.sql_datumformat) + @"'
           ,'" + _GlobalEntfall + @"'
           ,'" + _Grund + "')";

            DatenbankAbrufen.DatenbankAbfrage(Befehl);

            Befehl = @"DELETE FROM [Supplierungen]
            WHERE [Supplierungen].[Datum] <'" + DateTime.Today.ToString(Properties.Resources.sql_datumformat) + @"'
            AND [Supplierungen].[ZiehtVorDatum] < '" + DateTime.Today.ToString(Properties.Resources.sql_datumformat) + "'";

            DatenbankAbrufen.DatenbankAbfrage(Befehl);
        }
        static public void SupplierungLoeschen(DateTime _datum, string _klasse, int _stunde)
        {
            string Befehl = @"DELETE FROM [Supplierungen]
            WHERE [Supplierungen].[Datum] = '" + _datum.ToString(Properties.Resources.sql_datumformat) + @"'
            AND [Supplierungen].[Klasse] = '" + _klasse + @"'
            AND [Supplierungen].[Stunde] = '" + _stunde + "'";

            DatenbankAbrufen.DatenbankAbfrage(Befehl);
        }
        static public void AlleStundenplaeneLeeren()
        {
            string Befehl = "DELETE FROM [Stundenplan]";
            DatenbankAbrufen.DatenbankAbfrage(Befehl);
        }
        static public void Sprechstunden_Aktualisieren(Structuren.LEmitSprechstundenDaten daten)
        {
             string Befehl = @"INSERT INTO [LehrerTesten1]
             ([LehrerKuerzel]
             ,[Vorname]
             ,[Nachname]
             ,[Abteilungen]
             ,[Sprechstunde]
             ,[Tag]
             ,[Gebäude]
             ,[Raum]
             ,[Durchwahl])
           VALUES
            ('" + daten.LehrerKuerzel + @"'
            ,'" + daten.Vorname + @"'
            ,'" + daten.Nachname + @"'
            ,'" + daten.Abteilungen + @"'
            ,'" + daten.Sprechstunde + @"'
            ,'" + daten.Tag + @"'
            ,'" + daten.Gebäude + @"'
            ,'" + daten.Raum + @"'
            ,'" + daten.Durchwahl + "')";

             DatenbankAbrufen.DatenbankAbfrage(Befehl);

            if (daten.Abteilungen.Contains("Elektronik"))   //Alle Lehrer der Elektronik-Abteilung in LehrerAbteilungen-Tabelle schreiben
            {
                Befehl = @"INSERT INTO [LehrerAbteilungen]
             ([LehrerKuerzel]
             ,[AbteilungsID])
           VALUES
            ('" + daten.LehrerKuerzel + @"'
            ,'" + 1 + @"')";

                DatenbankAbrufen.DatenbankAbfrage(Befehl);
            }

            for (int i = 0; i < daten.KlassenvorstandKlassen.Count; i++)
              {
                  Befehl = @"UPDATE [Klassen]
          SET [Klassenvorstand] = '" + daten.LehrerKuerzel + @"'
        WHERE [Klasse] = '" + daten.KlassenvorstandKlassen[i] + "'";

              DatenbankAbrufen.DatenbankAbfrage(Befehl);
              }
        }
        static public void LehrerTabelleLoeschen()
        {
            string Befehl = @"DELETE FROM [LehrerTesten1] WHERE [LehrerTesten1].[LehrerKuerzel] != 'W_EL'";
            DatenbankAbrufen.DatenbankAbfrage(Befehl);

            Befehl = @"DELETE FROM [LehrerAbteilungen] WHERE [LehrerAbteilungen].[LehrerKuerzel] != 'W_EL'";
            DatenbankAbrufen.DatenbankAbfrage(Befehl);
        }
        static public void GlobalEntfallEintragen(Structuren.GlobalEntfall daten)
        {
            string Befehl = @"INSERT INTO [GlobalEntfall]
            ([AbteilungsID]
            ,[Datum]
            ,[VonStunde]
            ,[BisStunde]
            ,[GlobalEnfallID])
          VALUES
           ('" + daten.AbteilungsID + @"'
           ,'" + daten.Datum.ToString(Properties.Resources.sql_datumformat) + @"'
           ,'" + daten.VonStunde + @"'
           ,'" + daten.BisStunde + @"'
           ,'" + daten.GlobalerEntfallID + @"')";

            DatenbankAbrufen.DatenbankAbfrage(Befehl);

            Befehl = @"DELETE FROM [GlobalEntfall]
            WHERE [GlobalEntfall].[Datum] <'" + DateTime.Today.ToString(Properties.Resources.sql_datumformat) + "'";
           
            DatenbankAbrufen.DatenbankAbfrage(Befehl);
        }
        static public void GlobalEntfallLöschen(int _ID)
        {
            string Befehl = @"DELETE FROM [GlobalEntfall]
            WHERE [GlobalEntfall].[GlobalEnfallID] ='" + _ID + "'";

            DatenbankAbrufen.DatenbankAbfrage(Befehl);

            Befehl = @"DELETE FROM [Supplierungen]
            WHERE [Supplierungen].[GlobalEntfall] ='" + _ID + "'";

            DatenbankAbrufen.DatenbankAbfrage(Befehl);
        }

        #region InfoScreenV6

        /// <summary>
        /// Fügt eine Klasse in die Tabelle "Klassen" hinzu.
        /// </summary>
        /// <param name="abteilungsID">Abteilung der Klasse</param>
        /// <param name="klasse">Klasse die hinzugefügt werden soll</param>
        /// <param name="klassensprecher">Klassensprecher-ID der Klasse</param>
        /// <param name="klassensprecherName">Klassensprecher-Name der Klasse</param>
        /// <param name="klasseninfo">Klasseninfo der Klasse</param>
        /// <param name="klassenvorstand">KV der Klasse</param>
        /// <returns></returns>
        static public bool AddKlasseToKlassen(int abteilungsID, string klasse, string klassensprecher = "", string klassensprecherName = "", string klasseninfo = "", string klassenvorstand = "")
        {
            string sql;
            if (DatenbankAbrufen.ColumnLike("Klassen", "Klasse", klasse).Count > 0)
            {
                sql = "UPDATE Klassen SET AbteilungsID = " + abteilungsID + ", Klassensprecher='"+klassensprecher+"', KlassensprecherName = '"+
                    klassensprecherName + "', Klasseninfo='" + klasseninfo + "', Klassenvorstand = '" + klassenvorstand + 
                    "' WHERE Klasse = '" + klasse + "'";
            }
            else sql = @"  INSERT INTO Klassen (AbteilungsID, Klasse, Klassensprecher, KlassensprecherName, Klasseninfo, Klassenvorstand) 
                                VALUES (" + abteilungsID + ",'" + klasse + "','" + klassensprecher + "','" + klassensprecherName + "','" + klasseninfo + "','" + klassenvorstand + "')"; ;
            
            try { DatenbankAbrufen.DatenbankAbfrage(sql); }
            catch { return false; }           
            return true;
        }

        static public bool DeleteStandardklasseFromRaeume(string standardklasse, int abteilung)
        {

            string Befehl = @"UPDATE Raeume SET StandardKlasse = '' WHERE StandardKlasse='"+standardklasse+"' AND AbteilungsID="+abteilung; 
            try { DatenbankAbrufen.DatenbankAbfrage(Befehl); }
            catch { return false; }

            return true;
        }

        static public bool UpdateBezeichnungFromRaum(int raumnummer, string bezeichnung, int abteilung)
        {
            string Befehl = @"UPDATE Raeume SET Bezeichnung = '"+bezeichnung+"' WHERE Raum=" + raumnummer + " AND AbteilungsID = "+abteilung;
            try { DatenbankAbrufen.DatenbankAbfrage(Befehl); }
            catch { return false; }

            return true;
        }

        static public bool UpdateStandardKlasseFromRaeume(int raumnummer, string klasse, int abteilung)
        {
            string Befehl = @"UPDATE Raeume SET StandardKlasse = '" + klasse + "' WHERE Raum=" + raumnummer +" AND AbteilungsID = " + abteilung;
            try { DatenbankAbrufen.DatenbankAbfrage(Befehl); }
            catch { return false; }

            return true;
        }


        public static bool SetSettingValue(string key, string value, string theme = "")
        {

            bool newThemeKey = DatenbankAbrufen.DatenbankAbfrage(
                "SELECT * FROM Settings WHERE Theme='" + theme + "' AND VarKey='" + key + "'").Rows.Count < 1;
           
            string sql;
            
            if (newThemeKey) // If Key-Theme pair value is not yet present
                sql = "INSERT INTO Settings (VarKey, Value, Theme) VALUES ('" + key + "','" + value + "','" + theme + "')";           

            else  // If there is already a key and value defined for the theme, value will be updated
                sql = "UPDATE Settings SET Value = '" + value + "' WHERE VarKey ='" + key + "' AND Theme ='" + theme + "'" ;

            try { DatenbankAbrufen.DatenbankAbfrage(sql); return true; }
            catch { return false; }
        }

        public static bool DeleteThemeSettings(string theme)
        {
            string sql = "DELETE FROM Settings WHERE Theme = '" + theme + "'";

            try { DatenbankAbrufen.DatenbankAbfrage(sql); return true; }
            catch { return false; }
        }

        public static bool RemovePastExams()
        {
            int lesson = DatenbankAbrufen.AktuelleStunde();
            //if (DateTime.Now.Hour > 8 && lesson == -2) lesson = 12;

            string sql = "DELETE FROM Tests WHERE Datum < '" + DateTime.Now.ToString("yyyy-MM-dd") + 
                "' OR Datum = '" + DateTime.Now.ToString("yyyy-MM-dd") + "' AND Stunde + Dauer < " + lesson;

            try { DatenbankAbrufen.DatenbankAbfrage(sql); return true; }
            catch { return false; }
        }




        #endregion
    }
}
 