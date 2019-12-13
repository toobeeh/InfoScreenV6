using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infoscreen_Verwaltung.classes;
using System.Data;

namespace ScreenCoreApp.Classes
{
    public static class TimeSensitiveMode
    {
        public struct ZGA
        {
            public int AbteilungsID;
            public int BetriebsmodeID;
            public DateTime Zeit;
        }
        static public ZGA[] ZGAAbrufen(int _AbteilungsID) //Ruft alle ZGA der übergebenen Abteilung ab
        {
            string Befehl = @"SELECT 
            [ZeitgesteuerteAnzeige].[AbteilungsID] AS AbteilungsID,
            [ZeitgesteuerteAnzeige].[BetriebsmodeID] AS BetriebsmodeID,
            [ZeitgesteuerteAnzeige].[Zeit] AS Zeit
       FROM [ZeitgesteuerteAnzeige]
      WHERE [AbteilungsID] = '" + _AbteilungsID + "'";

            DataTable Daten = Infoscreen_Verwaltung.classes.DatenbankAbrufen.DatenbankAbfrage(Befehl);

            ZGA[] ret = new ZGA[Daten.Rows.Count];

            for (int i = 0; i < Daten.Rows.Count; i++)
            {
                ret[i].AbteilungsID = Daten.Rows[i]["AbteilungsID"].ToInt32();
                ret[i].BetriebsmodeID = Daten.Rows[i]["BetriebsmodeID"].ToInt32();
                ret[i].Zeit = Daten.Rows[i]["Zeit"].ToDateTime();
            }

            return ret;
        }
        static public void ZGALoeschen(ZGA _ZGA) //Löscht die übergebene ZGA in der DB
        {
            string Befehl = @"DELETE FROM [ZeitgesteuerteAnzeige]
      WHERE [AbteilungsID] = '" + _ZGA.AbteilungsID + @"'
        AND [BetriebsmodeID] = '" + _ZGA.BetriebsmodeID + @"'
        AND [Zeit] = '" + _ZGA.Zeit + "'";

            Infoscreen_Verwaltung.classes.DatenbankAbrufen.DatenbankAbfrage(Befehl);
        }
        static public void ZGAAbfragen(int _BildschirmID) //Überprüft ob die ZGA ausgelöst wurde, ändert den Standard-Betriebsmodus und löscht den Eintrag
        {
            string Befehl = @"SELECT 
            AbteilungsID
       FROM Bildschirme
      WHERE BildschirmID = " + _BildschirmID;

            DataTable Daten = Infoscreen_Verwaltung.classes.DatenbankAbrufen.DatenbankAbfrage(Befehl);

            int AbtID = Daten.Rows[0]["AbteilungsID"].ToInt32();
            ZGA[] dummy = ZGAAbrufen(AbtID);

            if (dummy == null || dummy.Length == 0) return;

            if (DateTime.Compare(dummy[0].Zeit, DateTime.Now) == -1)
            {
                Befehl = @"UPDATE [Abteilungen]
            SET [StandardBetriebsmode] = '" + dummy[0].BetriebsmodeID + @"'
          WHERE [AbteilungsID] = '" + dummy[0].AbteilungsID + "'";

                Infoscreen_Verwaltung.classes.DatenbankAbrufen.DatenbankAbfrage(Befehl);

                ZGALoeschen(dummy[0]);
            }
        }
    }
    
}
