using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.DirectoryServices;

namespace Infoscreen_Verwaltung.classes
{
    public class Helper
    {
        /// <summary>
        /// Findet die kleinste Stundennummer des übergebenen Stundenplans heraus
        /// </summary>
        /// <param name="_stundenplan">Der Stundenplan aus welchem die kleinste Stunde herausgefunden werden soll.</param>
        /// <returns>Die kleinste Stunde des Stundenplans</returns>
        static public int KleinsteStunde(Structuren.StundenplanTag[] _stundenplan)
        {
            try
            {
                int min = 20;
                for (int i1 = 0; i1 < _stundenplan.Length; i1++)
                {
                    for (int i2 = 0; i2 < _stundenplan[i1].StundenDaten.Length; i2++)
                    {
                        if (min > _stundenplan[i1].StundenDaten[i2].Stunde) min = _stundenplan[i1].StundenDaten[i2].Stunde;
                    }
                }
                return min;
            }
            catch { return 0; }
        }

        static public string GetKlassensprecherName(string pre)
        {
            string KlassensprecherID;

            if (pre != "")
            {
                DirectoryEntry entry = new DirectoryEntry("LDAP://" + Properties.Resources.standardDomain, Properties.Resources.domainUser, Properties.Resources.domainPassword, AuthenticationTypes.Secure);
                entry.Username = Properties.Resources.domainUser;
                entry.Password = Properties.Resources.domainPassword;


                DirectorySearcher search = new DirectorySearcher(entry);
                search.Filter = "(&(objectClass=user)(cn=" + pre + "))";
                search.PropertiesToLoad.Add("displayName");

                string temp1;

                try
                {
                    SearchResult result = search.FindOne();
                    KlassensprecherID = (string)result.Properties["displayName"][0];
                }
                catch (Exception ex)
                {
                    //throw new Exception("Error obtaining id. " + ex.Message);
                    temp1 = "Unbekannt";
                    KlassensprecherID = (temp1);
                }
            }
            else
            {
                KlassensprecherID = "";
            }

            return KlassensprecherID;
        }

        /// <summary>
        /// Findet die größte Stundennummer des übergebenen Stundenplans heraus
        /// </summary>
        /// <param name="_stundenplan">Der Stundenplan aus welchem die größte Stunde herausgefunden werden soll.</param>
        /// <returns>Die größte Stunde des Stundenplans</returns>
        static public int GrößteStunde(Structuren.StundenplanTag[] _stundenplan)
        {
            try
            {
                int max = 0;
                for (int i1 = 0; i1 < _stundenplan.Length; i1++)
                {
                    for (int i2 = 0; i2 < _stundenplan[i1].StundenDaten.Length; i2++)
                    {
                        if (max < _stundenplan[i1].StundenDaten[i2].Stunde) max = _stundenplan[i1].StundenDaten[i2].Stunde;
                    }
                }
                return max;
            }
            catch { return -1; }
        }

        static public string StundenZeiten (int _Stunde)
        {
            switch(_Stunde)
            {
                case 0: return "7:10 - 8:00";
                case 1: return "8:00 - 8:50";
                case 2: return "8:50 - 9:40";
                case 3: return "9:50 - 10:40";
                case 4: return "10:40 - 11:30";
                case 5: return "11:40 - 12:30";
                case 6: return "12:30 - 13:20";
                case 7: return "13:20 - 14:10";
                case 8: return "14:20 - 15:10";
                case 9: return "15:10 - 16:00";
                case 10: return "16:10 - 17:00";
                case 11: return "17:00 - 17:50";
                case 12: return "17:50 - 18:40";
            }
            return "?";
        }
    }

    public static class StringHelper
    {
        /// <summary>
        /// Konvertiert den übergebenen String in Int32.
        /// Liefert -1, wenn der String keine Zahl ist.
        /// </summary>
        /// <param name="_string">der zu konvertierende String</param>
        /// <returns>String in Int32</returns>
        public static int ToInt32(this string _string)
        {
            try { return Convert.ToInt32(_string); }
            catch { return -1; }
        }

        /// <summary>
        /// Konvertiert das übergebene Bool in Int32 (0 oder 1)
        /// </summary>
        /// <param name="_bool">das zu konvertierende Bool</param>
        /// <returns>0, wenn false
        /// 1, wenn true</returns>
        public static int ToInt32(this bool _bool)
        {
            return _bool ? 1 : 0;
        }

        /// <summary>
        /// Prüft ob eine Stelle des Strings größer als die angegebene Zahl ist.
        /// </summary>
        /// <param name="_string">Der zu prüfende String</param>
        /// <param name="_biggerThan">Die Zahl mit welcher verglichen werden soll.</param>
        /// <returns>True, wenn eine Stelle des Strings größer ist</returns>
        public static bool OneBiggerThan(this string _string, int _biggerThan)
        {
            foreach (char Element in _string)
            {
                if (Element.ToString().ToInt32() > _biggerThan) return true;
            }
            return false;
        }

        public static DateTime ToDateTime(this object _object)
        {
            try { return Convert.ToDateTime(_object); }
            catch { return new DateTime(2000, 1, 1); }
        }

        public static int ToInt32(this object _object)
        {
            try { return Convert.ToInt32(_object); }
            catch { return -1; }
        }

        public static TimeSpan ToTimeSpan(this object _object)
        {
            try { return new TimeSpan(0, _object.ToInt32(), 0); }
            catch { return new TimeSpan(0); }
        }

        public static bool ToBool(this string _string)
        {
            try { return Convert.ToBoolean(_string); }
            catch
            {
                if (_string.ToLower() == "on")
                    return true;
                return false;
            }
        }

        public static string ToLehrer(this object _object)
        {
            try { return DatenbankAbrufen.LehrerNamen(_object.ToString()); }
            catch { return ""; }
        }

        public static string GetDayOfInt(this int _int)
        {
            switch (_int)
            {
                case 0:
                    return "Mo";
                case 1:
                    return "Di";
                case 2:
                    return "Mi";
                case 3:
                    return "Do";
                case 4:
                    return "Fr";
                case 5:
                    return "Sa";
                case 6:
                    return "So";
                default:
                    return "";
            }
        }

        public static string GetZeitenOfStunde(this int _int)
        {
            switch (_int)
            {
                case 0:
                    return "07:10 - 08:00";
                case 1:
                    return "08:00 - 08:50";
                case 2:
                    return "08:50 - 09:40";
                case 3:
                    return "09:50 - 10:40";
                case 4:
                    return "10:40 - 11:30";
                case 5:
                    return "11:40 - 12:30";
                case 6:
                    return "12:30 - 13:20";
                case 7:
                    return "13:20 - 14:10";
                case 8:
                    return "14:20 - 15:10";
                case 9:
                    return "15:10 - 16:00";
                case 10:
                    return "16:10 - 17:00";
                case 11:
                    return "17:00 - 17:50";
                case 12:
                    return "17:50 - 18:40";
                default:
                    return "";
            }
        }

        public static string BBCodeToHTML(this string _string)
        {
            string BB_Code = _string;
            BB_Code = BB_Code.Replace("<", "").Replace(">", ""); //html Tags unbrauchbar machen
            BB_Code = BB_Code.Replace("[br]", "<br/>");
            BB_Code = BB_Code.Replace("\n", "<br/>");
            BB_Code = BB_Code.Replace("[s]", "<del>");
            BB_Code = BB_Code.Replace("[/s]", "</del>");
            BB_Code = BB_Code.Replace("[ulist]", "<UL>");
            BB_Code = BB_Code.Replace("[/ulist]", "</UL>");
            BB_Code = BB_Code.Replace("[olist]", "<OL>");
            BB_Code = BB_Code.Replace("[/olist]", "</OL>");
            BB_Code = BB_Code.Replace("[*]", "<LI>");
            BB_Code = BB_Code.Replace("[b]", "<B>");
            BB_Code = BB_Code.Replace("[/b]", "</B>");
            BB_Code = BB_Code.Replace("[strong]", "<STRONG>");
            BB_Code = BB_Code.Replace("[/strong]", "</STRONG>");
            BB_Code = BB_Code.Replace("[u]", "<u>");
            BB_Code = BB_Code.Replace("[/u]", "</u>");
            BB_Code = BB_Code.Replace("[i]", "<i>");
            BB_Code = BB_Code.Replace("[/i]", "</i>");
            BB_Code = BB_Code.Replace("[em]", "<em>");
            BB_Code = BB_Code.Replace("[/em]", "</em>");
            BB_Code = BB_Code.Replace("[sup]", "<sup>");
            BB_Code = BB_Code.Replace("[/sup]", "</sup>");
            BB_Code = BB_Code.Replace("[sub]", "<sub>");
            BB_Code = BB_Code.Replace("[/sub]", "</sub>");
            BB_Code = BB_Code.Replace("[hr]", "<HR>");
            BB_Code = BB_Code.Replace("[strike]", "<STRIKE>");
            BB_Code = BB_Code.Replace("[/strike]", "</STRIKE>");
            BB_Code = BB_Code.Replace("[h1]", "<h1>");
            BB_Code = BB_Code.Replace("[/h1]", "</h1>");
            BB_Code = BB_Code.Replace("[h2]", "<h2>");
            BB_Code = BB_Code.Replace("[/h2]", "</h2>");
            BB_Code = BB_Code.Replace("[h3]", "<h3>");
            BB_Code = BB_Code.Replace("[/h3]", "</h3>");
            BB_Code = BB_Code.Replace("[url=", "<A HREF=");
            BB_Code = BB_Code.Replace("[/url]", "</A>");
            BB_Code = BB_Code.Replace("']", "'>");
            // Farb und größen-Ersetzungen
            while (BB_Code.IndexOf("[size=") != -1)
            {
                // Alles vor dem Size Befehl wegschneiden
                string Befehl = BB_Code.Substring(BB_Code.IndexOf("[size="));
                // Size Befehl herrausfiltern
                Befehl = Befehl.Substring(0, Befehl.IndexOf("]"));
                // Die gewünschte größe herrausfinden
                string Size = Befehl.Substring(Befehl.IndexOf('=') + 1);
                // Den Size befehl ersetzten
                BB_Code = BB_Code.Replace("[size=" + Size + "]", "<span style=\"font-size: " + Size + "px;\">");
            }
            BB_Code = BB_Code.Replace("[/size]", "</span>");
            while (BB_Code.IndexOf("[color=") != -1)
            {
                // Alles vor dem Size Befehl wegschneiden
                string Befehl = BB_Code.Substring(BB_Code.IndexOf("[color="));
                // Size Befehl herrausfiltern
                Befehl = Befehl.Substring(0, Befehl.IndexOf("]"));
                // Die gewünschte größe herrausfinden
                string Color = Befehl.Substring(Befehl.IndexOf('=') + 1);
                // Den Size befehl ersetzten
                BB_Code = BB_Code.Replace("[color=" + Color + "]", "<span style=\"color: " + Color + ";\">");
            }
            BB_Code = BB_Code.Replace("[/color]", "</span>");
            // Den übersetzten Code übergeben
            return BB_Code;
        }

        /// <summary>
        /// Entfernt alle doppelten Einträge einer Liste.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<T> DoppelteEinträgeEntfernen<T>(this List<T> list)
        {
            List<T> newList = new List<T>();
            Dictionary<T, string> keyList = new Dictionary<T, string>();
            foreach (T item in list)
            {
                if (!keyList.ContainsKey(item))
                {
                    keyList.Add(item, string.Empty);
                    newList.Add(item);
                }
            }
            return newList;
        }

        public static string ToValidRoom(int num, int digits)
        {
            string roomnum = num.ToString();
            while (roomnum.Length < digits) roomnum = "0" + roomnum;
            return roomnum;
        }
        public static string ToValidRoom(string roomnum, int digits)
        {
            while (roomnum.Length < digits) roomnum = "0" + roomnum;
            return roomnum;
        }

        public static string ToValidRoomBuilding(string room_building, int digits_room, int digits_building)
        {
            string room = room_building.Substring(room_building.IndexOf("-") + 1);
            string building = room_building.Substring(0, room_building.IndexOf("-"));

            while (room.Length < digits_room) room = "0" + room;
            while (building.Length < digits_building) building = "0" + building;

            return building + "-" + room;
        }

    }

    public class dataButton : Button
    {
        private object setting;

        public object Setting
        {
            get { return setting; }
            set { setting = value; }
        }
    }

    public class dataTextBox : TextBox
    {
        private object setting;

        public object Setting
        {
            get { return setting; }
            set { setting = value; }
        }
    }
}