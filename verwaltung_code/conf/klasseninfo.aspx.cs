using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Infoscreen_Verwaltung.classes;

namespace Infoscreen_Verwaltung.conf
{
    public partial class klasseninfo : System.Web.UI.Page
    {
        string ebene = "../";
        bool Umschalter = false;
        int count = 0;
        struct KlassenInfo
        {
            public Label Klasse;
            public TextBox Klasseninfo;
        }

        KlassenInfo[] dummy;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!classes.Login.Angemeldet) Response.Redirect("../login.aspx");
            if (!classes.Login.Rechte.Klasseninfo.OneBiggerThan(1)) Response.Redirect("../");

            TopMenu.Text = classes.Anzeige.TopBar(ebene);
            Menu.Text = classes.Anzeige.Menue(ebene, "conf");

            if (!Page.IsPostBack)
            {
                string[] abteilungen;
                if (classes.Login.Rechte.Klassen[2] != '0')  //Superadmin
                    abteilungen = classes.DatenbankAbrufen.AbteilungenAbrufen();
                else if (classes.Login.Rechte.Klassen[1] != '0') //Lehrer, AV, Schulwart, 
                    abteilungen = classes.Login.Abteilungen;
                else abteilungen = new string[] { classes.Login.StammAbteilung }; //Klassensprecher

                foreach (string abteilung in abteilungen)
                    DropDownListAbteilung.Items.Add(abteilung);

                try { DropDownListAbteilung.SelectedValue = classes.Login.StammAbteilung; }
                catch { DropDownListAbteilung.SelectedIndex = -1; }

                if (Session["22_Abteilung"] != null)
                {
                    try { DropDownListAbteilung.SelectedValue = Session["21_Abteilung"].ToString(); }
                    catch { }
                }

                Session["22_Abteilung"] = null;
            }

            if (classes.Login.Rechte.Klasseninfo[2] == '4' || classes.Login.Rechte.Klasseninfo[1] != '0') //Superadmin oder Abteilungswart und -vorstand, Lehrer
            {
                TabelleZeichnen();
            }
            else //Klassensprecher
            {
                Umschalter = true;
                TabelleZeichnen();
            }
        }

        protected void btReset_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dummy.Length; i++)
            {
                dummy[i].Klasseninfo.Text = "";
            }
        }

        protected void DropDownListAbteilung_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabelleZeichnen();
        }
        protected void TabelleZeichnen()
        {
            tbKlassenInfo.Rows.Clear();
            TableRow row = new TableRow();
            TableCell cell = new TableCell();
            cell.CssClass = "head";
            cell.Text = "Klasse";
            row.Cells.Add(cell);

            cell = new TableCell();
            cell.CssClass = "head";
            cell.Text = "Klasseninformation";
            row.Cells.Add(cell);


            tbKlassenInfo.Rows.Add(row);

            string[] klassen = DatenbankAbrufen.KlassenAbrufen(DropDownListAbteilung.SelectedValue);
            Structuren.Klasseneigenschaften daten = new Structuren.Klasseneigenschaften();

  

            if (Umschalter == true)
            {
                string[] temp = new string[1];
                Array.Copy(klassen, temp, 1);
                klassen = temp;
                klassen[0] = classes.Login.StammKlasse;
            }
            dummy = new KlassenInfo[klassen.Length];

            foreach (string klasse in klassen)
            {
                row = new TableRow();

                daten = DatenbankAbrufen.KlasseneigenschaftenAbrufen(klasse);

                cell = new TableCell();
                dummy[count].Klasse = new Label();
                dummy[count].Klasse.Text = daten.klasse;
                dummy[count].Klasse.CssClass = "autocompFach";
                cell.Controls.Add(dummy[count].Klasse);
                row.Cells.Add(cell);

                cell = new TableCell();
                dummy[count].Klasseninfo = new TextBox();
                dummy[count].Klasseninfo.Text = daten.klasseninfo;
                dummy[count].Klasseninfo.Width = 1150;
                dummy[count].Klasseninfo.CssClass = "autocompFach";
                cell.Controls.Add(dummy[count].Klasseninfo);
                row.Cells.Add(cell);

                tbKlassenInfo.Rows.Add(row);
                count++;
            }
        }
        protected void btÜbernehmen_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dummy.Length; i++)
            {
                DatenbankSchreiben.KlasseninfoUpdaten(1, dummy[i].Klasse.Text, dummy[i].Klasseninfo.Text);
            }
        }

        protected void DropDownListKlasse_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}