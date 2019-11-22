using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace Infoscreen_Verwaltung.classes
{
    public class Anzeige
    {
        /// <summary>
        /// Gibt je nach Berechtigung den Code für das entsprechende Menü zurück.
        /// Die Berechtigung wird hierbei aus der aktuellen Session ausgelesen.
        /// </summary>
        /// <param name="_Ebene">Die Ebene an der das Menü stehen wird (um die URLs richtig zu erstellen)</param>
        /// <returns>Code für das Menü</returns>
        /// 

        static private string displayNav(string actDir, string navObj, string navObj2="error")
        {
            if (actDir != navObj && actDir != navObj2) return "style='display:none'";
            else return "";
        }
        static public string Menue(string _Ebene, string dir)
        {

            //
            string ret = "";

            //Anzeigen
            string temp = "";
            string wrap = "";
            wrap += "<div class='navContainer' "+displayNav(dir,"show")+">";            
            if (Login.Rechte.Stundenplan.ToInt32() > 0) temp += "<a href='" + _Ebene + "show/stundenplan.aspx'>Stundenplan</a>\r\n"; //ALLE 
            if (Login.Rechte.Supplierung.ToInt32() > 0) temp += "<a href='" + _Ebene + "show/supplierungen.aspx'>Supplierungen</a>\r\n"; //ALLE
            if (Login.Rechte.Tests.ToInt32() > 0) temp += "<a href='" + _Ebene + "show/tests.aspx'>Schularbeiten / Tests</a>\r\n"; //ALLE
            if (Login.Rechte.Sprechstunden.ToInt32() > 0) temp += "<a href='" + _Ebene + "show/sprechstunden.aspx'>Sprechstunden</a>\r\n"; //ALLE
            if (Login.Rechte.Klasseninfo.ToInt32() > 0) temp += "<a href='" + _Ebene + "show/klasseninfo.aspx'>Klasseninfo</a>\r\n"; //ALLE
            if (Login.Rechte.Abteilungsinfo.ToInt32() > 0) temp += "<a href='" + _Ebene + "show/abteilungsinfo.aspx'>Abteilungsinfo</a>\r\n"; //ALLE
            wrap += temp + "</div>";
            if (temp != "") ret += "<h1 onclick='showNav()'>Anzeigen </h1>" + wrap;


            //Konfigurieren
            wrap = "<div class='navContainer' " + displayNav(dir, "conf") + ">";
            temp = "";
            if (Login.Rechte.Stundenplan.OneBiggerThan(1)) temp += "<a href='" + _Ebene + "conf/stundenplan.aspx'>Stundenplan</a>\r\n"; //Schulwart, AV, Superadmin
            //if (Login.Rechte.Supplierung.OneBiggerThan(1)) temp += "<a href='" + _Ebene + "conf/x.aspx'>Supplierungen</a>\r\n";
            if (Login.Rechte.Supplierung.OneBiggerThan(1)) temp += "<a href='" + _Ebene + "conf/supplierungen/table.aspx'>Supplierungen</a>\r\n"; //Schulwart, AV, und Superadmin
            if (Login.Rechte.Tests.OneBiggerThan(1)) temp += "<a href='" + _Ebene + "conf/tests/'>Schularbeiten / Tests</a>\r\n"; //ALLE außer Schüler
            if (Login.Rechte.Klasseninfo.OneBiggerThan(1)) temp += "<a href='" + _Ebene + "conf/klasseninfo.aspx'>Klasseninfo</a>\r\n"; //ALLE außer Schüler, Lehrer
            if (Login.Rechte.Abteilungsinfo.OneBiggerThan(1)) temp += "<a href='" + _Ebene + "conf/abteilungsinfo.aspx'>Abteilungsinfo</a>\r\n"; //Schulwart, AV, Abteilungssprecher, Superadmin
            wrap += temp + "</div>";
            if (temp != "") ret += "<h1 onclick='showNav()'>Konfiguration </h1>" + wrap;

            //ADMIN
            wrap = "<div class='navContainer' " + displayNav(dir, "admin") + ">";
            temp  = "";
            if (Login.Rechte.Bildschirme.OneBiggerThan(1)) temp += "<a href='" + _Ebene + "admin/bildschirm-einstellungen/'>Bildschirm-Modi</a>\r\n";
            if (Login.Rechte.Bildschirme.OneBiggerThan(1)) temp += "<a href='" + _Ebene + "admin/converter/'>PPT Converter</a>\r\n";
            //if (Login.Rechte.Gruppenrechte.OneBiggerThan(1)) temp += "<a href='" + _Ebene + "admin/x.aspx'>Berechtigungen</a>\r\n";
            if (Login.Rechte.Klassen.OneBiggerThan(1)) temp += "<a href='" + _Ebene + "admin/klassen/'>Klasseneigenschaften</a>\r\n";        
            if (Login.Rechte.Superadmin) temp += "<a href='" + _Ebene + "admin/lehrer/'>Lehrereigenschaften</a>\r\n";
            if (Login.Rechte.Superadmin) temp += "<a href='" + _Ebene + "admin/raeume/default.aspx'>Raumeigenschaften</a>\r\n";
            //if (Login.Rechte.Superadmin) temp += "<a href='" + _Ebene + "admin/x.aspx'>Untis Datei einspielen</a>\r\n";
            wrap += temp + "</div>";
            if (temp != "") ret += "<h1 onclick='showNav()'>Admin </h1>" + wrap;

           
            ret += @"<h1 onclick='showNav()'>Info</h1> <div class='navContainer' " + displayNav(dir, "info", "login") + ">" +
            "<a href='" + _Ebene + @"info/impressum.aspx'>Impressum</a>
            <a href='" + _Ebene + @"info/kontakt.aspx'>Kontakt</a>
            <a href='" + _Ebene + @"info/version.aspx'>Versionsinfo</a> </div> ";
            
            return ret;
        }

        /// <summary>
        /// Diese Methode gibt die TopBar abhängig von der aktuellen Session zurück
        /// </summary>
        /// <param name="_Ebene">Die aktuelle Ebene der Seite in welcher die TopBar angezeigt wird. Dies ist nötig, um die TopBar-Links in der richtigen Relativität zu erstellen</param>
        /// <returns>Ein in HTML formatierter string, der die anzuzeigende TopBar enthält</returns>
        static public string TopBar(string _Ebene)
        {
            if (Login.Angemeldet)
            {
                string ret = @"<a href='" + _Ebene + @"login.aspx'>Logout</a>
    <a href='" + _Ebene + @"config.aspx'>Benutzerprofil</a>";
    //<p>Derzeit Angemeldet: " + Login.Name + "</p> ";



                return ret;
            }
            else
            {
                string ret = "<a href='" + _Ebene + "login.aspx'>Login</a><p>Login erforderlich</p> ";
                return ret;
            }
        }
    }
}