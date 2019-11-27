using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Infoscreen_Verwaltung.classes;

namespace Infoscreen_Verwaltung
{
    public partial class login : System.Web.UI.Page
    {
        string ebene = "./";

        protected void Page_Load(object sender, EventArgs e)
        {
            DatenbankAbrufen.DBClose();
            Session.Clear();
            Response.BufferOutput = true;

            TopMenu.Text = classes.Anzeige.TopBar(ebene);
            Menu.Text = classes.Anzeige.Menue(ebene, "login");
            TextBoxUser.Focus();
        }

        protected void ButtonLogin_Click(object sender, EventArgs e)
        {
//#if DEBUG
            /* Wenn es nicht funktioniert: -) Hashtags auskommentieren 
                                           -) Alles zwischen #else und #endif auskommentieren */
            //if (TextBoxUser.Text.ToLower() == "czak" && TextBoxPassword.Text == "2580")
            //{
            //    classes.Login.Angemeldet = true;
            //    classes.Login.Name = "Debug User";
            //    classes.Login.User = "czak";
            //    classes.Login.Abteilungen = new string[] { "Elektronik" };
            //    classes.Login.StammAbteilung = "Elektronik";
            //    classes.Login.StammKlasse = "2AHEL";
            //    classes.Login.Rechte.Abteilungsinfo = "010";
            //    classes.Login.Rechte.Bildschirme = "000";
            //    classes.Login.Rechte.Gruppenrechte = "100";
            //    classes.Login.Rechte.Klassen = "000";
            //    classes.Login.Rechte.Klasseninfo = "010";
            //    classes.Login.Rechte.Lehrer = true;
            //    classes.Login.Rechte.Räume = "000";
            //    classes.Login.Rechte.Sprechstunden = "210";
            //    classes.Login.Rechte.Stundenplan = "100";
            //    classes.Login.Rechte.Superadmin = false;
            //    classes.Login.Rechte.Supplierung = "010";
            //    classes.Login.Rechte.Tests = "210";

            //    Response.Redirect("./");
            //}
            //else
            //{
            //    Error.Text = "Der Infoscreen befindet sich derzeit im Wartungsmodus.\r\nEine anmeldung ist daher derzeit nicht möglich.";
            //    Error.Visible = true;
            //}
//#else
            string domain = Properties.Resources.standardDomain;
            string user = TextBoxUser.Text;
            if (TextBoxUser.Text.Contains('\\'))
            {
                string[] temp = user.Split('\\');
                domain = temp[0];
                user = temp[1];
            }
            if (TextBoxUser.Text.Contains('@'))
            {
                string[] temp = user.Split('@');
                domain = temp[1];
                user = temp[0];
            }

            if (classes.Authentifizierung.IsAuthenticated(domain, user, TextBoxPassword.Text))
            {
                classes.Login.Angemeldet = true;
                classes.Login.User = user;
                classes.Login.Name = classes.Authentifizierung.GetName(user);
                classes.Structuren.User benutzer = classes.DatenbankAbrufen.GetUserInfo(user);
                classes.Login.Rechte.Lehrer = benutzer.Lehrer;
                classes.Login.Rechte.Superadmin = benutzer.Superadmin;
                if (benutzer.Lehrer)
                {
                    classes.Login.StammKlasse = benutzer.StammKlasse;
                    classes.Login.Klassen = benutzer.Klassen;
                    classes.Login.StammAbteilung = benutzer.StammAbteilung;
                    classes.Login.Abteilungen = benutzer.Abteilungen;
                }
                classes.DatenbankAbrufen.SetGruppenrechte(user);

                Response.Redirect("default.aspx");

                if (classes.Login.Rechte.Bildschirme.OneBiggerThan(1)) //admin sieht Bildschirmeinstellungen als Startbildschirm
                {
                    Response.Redirect("./admin/bildschirm-einstellungen/");
                }
                if (classes.Login.Rechte.Stundenplan[0].ToInt32() > 0) //Schüler, Klassensprecher, Abteilungssprecher, Lehrer sieht Stundenplan als Startbildschirm
                {
                    Response.Redirect("./show/stundenplan.aspx");
                }
                if(classes.Login.Rechte.Supplierung[1].ToInt32() > 0) //AV, Schulwart sieht Supplierungen als Startbildschirm
                {
                    Response.Redirect("./show/supplierungen.aspx");
                }
            }
            else
            {
                Error.Visible = true;
            }
//#endif
        }
    }
}