using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Infoscreen_Verwaltung
{
    public partial class test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lb_stunde.Text = classes.DatenbankAbrufen.AktuelleStunde().ToString();
        }

        protected void abfrage_Click(object sender, EventArgs e)
        {
            lb_stunde.Text = classes.DatenbankAbrufen.AktuelleStunde().ToString() + "<br />";
            lb_stunde.Text += classes.DatenbankAbrufen.AktuellerTag().ToString();

            classes.Structuren.AnzeigeElemente data1 = classes.DatenbankAbrufen.AnzuzeigendeElemente(tb_id.Text);
            CheckBoxList1.Items[0].Selected = data1.Stundenplan;
            CheckBoxList1.Items[1].Selected = data1.Sprechstunden;
            CheckBoxList1.Items[2].Selected = data1.Raumaufteilung;
            CheckBoxList1.Items[3].Selected = data1.Supplierplan;
            CheckBoxList1.Items[4].Selected = data1.Abteilungsinfo;
            CheckBoxList1.Items[5].Selected = data1.AktuelleSupplierungen;
            CheckBoxList1.Items[6].Selected = data1.PowerPoints != -1 ? true : false;

            classes.Structuren.Rauminfo data2 = classes.DatenbankAbrufen.RauminfoAbrufen(tb_id.Text);
            BulletedList1.Items[0].Text = "Stammklasse: " + data2.Stammklasse;
            BulletedList1.Items[1].Text = "Abteilung: " + data2.Abteilung;
            BulletedList1.Items[2].Text = "Raumnummer: " + data2.Raumnummer;
            BulletedList1.Items[3].Text = "Gebäude: " + data2.Gebäude;
            BulletedList1.Items[4].Text = "Aktuelle Klasse: " + data2.AktuelleKlasse;
            BulletedList1.Items[5].Text = "Klassenvorstand: " + data2.Klassenvorstand;
            BulletedList1.Items[6].Text = "Nicht Stören: " + data2.NichtStören;

            lb_klasseninfo.Text = classes.DatenbankAbrufen.KlasseninfoAbrufen(data2.Stammklasse);

            lb_abteilungsinfo.Text = classes.DatenbankAbrufen.AbteilungsinfoAbrufen(tb_id.Text);

            classes.DatenbankAbrufen.AktuelleSupplierungenAbrufen(tb_id.Text);
        }
    }
}