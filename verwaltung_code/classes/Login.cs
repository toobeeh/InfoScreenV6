using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Infoscreen_Verwaltung.classes
{
    public class Login
    {
        public static bool Angemeldet
        {
            get { return HttpContext.Current.Session["active"] != null ? true : false; }
            set { HttpContext.Current.Session["active"] = value ? "true" : null; }
        }

        public static string[] Abteilungen
        {
            get { return HttpContext.Current.Session["abteilungen"] != null ? HttpContext.Current.Session["abteilungen"].ToString().Split('/') : new string[0]; }
            set { HttpContext.Current.Session["abteilungen"] = String.Join("/", value); }
        }

        public static string StammAbteilung
        {
            get { return HttpContext.Current.Session["stammAbteilung"] == null ? "" : HttpContext.Current.Session["stammAbteilung"].ToString(); }
            set { HttpContext.Current.Session["stammAbteilung"] = value; }
        }

        public static string[] Klassen
        {
            get { return HttpContext.Current.Session["klassen"] != null ? HttpContext.Current.Session["klassen"].ToString().Split('/') : new string[0]; }
            set { HttpContext.Current.Session["klassen"] = String.Join("/", value); }
        }

        public static string StammKlasse
        {
            get { return HttpContext.Current.Session["stammKlasse"] == null ? "" : HttpContext.Current.Session["stammKlasse"].ToString(); }
            set { HttpContext.Current.Session["stammKlasse"] = value; }
        }

        public static string Name
        {
            get { return HttpContext.Current.Session["name"].ToString(); }
            set { HttpContext.Current.Session["name"] = value; }
        }

        public static string User
        {
            get { return HttpContext.Current.Session["user"].ToString(); }
            set { HttpContext.Current.Session["user"] = value; }
        }

        public class Rechte
        {
            public static string Stundenplan
            {
                get { return HttpContext.Current.Session["rechteStundenplan"] == null ? "" : HttpContext.Current.Session["rechteStundenplan"].ToString(); }
                set { HttpContext.Current.Session["rechteStundenplan"] = value; }
            }

            public static string Supplierung
            {
                get { return HttpContext.Current.Session["rechteSupplierung"] == null ? "" : HttpContext.Current.Session["rechteSupplierung"].ToString(); }
                set { HttpContext.Current.Session["rechteSupplierung"] = value; }
            }

            public static string Klasseninfo
            {
                get { return HttpContext.Current.Session["rechteKlasseninfo"] == null ? "" : HttpContext.Current.Session["rechteKlasseninfo"].ToString(); }
                set { HttpContext.Current.Session["rechteKlasseninfo"] = value; }
            }

            public static string Abteilungsinfo
            {
                get { return HttpContext.Current.Session["rechteAbteilungsinfo"] == null ? "" : HttpContext.Current.Session["rechteAbteilungsinfo"].ToString(); }
                set { HttpContext.Current.Session["rechteAbteilungsinfo"] = value; }
            }

            public static string Sprechstunden
            {
                get { return HttpContext.Current.Session["rechteSprechstunden"] == null ? "" : HttpContext.Current.Session["rechteSprechstunden"].ToString(); }
                set { HttpContext.Current.Session["rechteSprechstunden"] = value; }
            }

            public static string Tests
            {
                get { return HttpContext.Current.Session["rechteTests"] == null ? "" : HttpContext.Current.Session["rechteTests"].ToString(); }
                set { HttpContext.Current.Session["rechteTests"] = value; }
            }

            public static string Klassen
            {
                get { return HttpContext.Current.Session["rechteKlassen"] == null ? "" : HttpContext.Current.Session["rechteKlassen"].ToString(); }
                set { HttpContext.Current.Session["rechteKlassen"] = value; }
            }

            public static string Räume
            {
                get { return HttpContext.Current.Session["rechteRäume"] == null ? "" : HttpContext.Current.Session["rechteRäume"].ToString(); }
                set { HttpContext.Current.Session["rechteRäume"] = value; }
            }

            public static string Bildschirme
            {
                get { return HttpContext.Current.Session["rechteBildschirme"] == null ? "" : HttpContext.Current.Session["rechteBildschirme"].ToString(); }
                set { HttpContext.Current.Session["rechteBildschirme"] = value; }
            }

            public static string Gruppenrechte
            {
                get { return HttpContext.Current.Session["rechteGruppenrechte"] == null ? "" : HttpContext.Current.Session["rechteGruppenrechte"].ToString(); }
                set { HttpContext.Current.Session["rechteGruppenrechte"] = value; }
            }

            public static bool Lehrer
            {
                get { return HttpContext.Current.Session["istLehrer"] == null ? false : HttpContext.Current.Session["istLehrer"].ToString() == "Ja" ? true : false; }
                set { HttpContext.Current.Session["istLehrer"] = value ? "Ja" : "Nein"; }
            }

            public static bool Superadmin
            {
                get { return HttpContext.Current.Session["istSuperadmin"] == null ? false : HttpContext.Current.Session["istSuperadmin"].ToString() == "Ja" ? true : false; }
                set { HttpContext.Current.Session["istSuperadmin"] = value ? "Ja" : "Nein"; }
            }
            public static string LehrerKürzel
            {
                get { return HttpContext.Current.Session["LehrerKürzel"].ToString(); }
                set { HttpContext.Current.Session["LehrerKürzel"] = value; }
            }
        }
    }
}