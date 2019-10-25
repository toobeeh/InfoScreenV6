using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Infoscreen_Verwaltung.classes;
using System.Data;

// InfoScreen v6: Replaces assignment of classes to rooms for better structure (-> rooms: static, classes: can be modified)

namespace Infoscreen_Verwaltung.admin.raeume
{
    public partial class _default : System.Web.UI.Page
    {
        string ebene = "../../";
        

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!classes.Login.Angemeldet) Response.Redirect("../../login.aspx");
            if (!classes.Login.Rechte.Bildschirme.OneBiggerThan(1)) Response.Redirect("../../");

           
            TopMenu.Text = classes.Anzeige.TopBar(ebene);
            Menu.Text = classes.Anzeige.Menue(ebene, "admin");


            if (!Page.IsPostBack)
            {
                string[] abteilungen;
                if (classes.Login.Rechte.Klassen[2] != '0')  //Superadmin
                    abteilungen = classes.DatenbankAbrufen.AbteilungenAbrufen();
                else if (classes.Login.Rechte.Klassen[1] != '0') //Lehrer, AV, Schulwart, 
                    abteilungen = classes.Login.Abteilungen;
                else abteilungen = new string[] { classes.Login.StammAbteilung }; //Klassensprecher

                foreach (string abteilung in abteilungen)
                    dropDownAbteilung.Items.Add(abteilung);

                try { dropDownAbteilung.SelectedValue = classes.Login.StammAbteilung; }
                catch { dropDownAbteilung.SelectedIndex = -1; }

                if (Session["22_Abteilung"] != null)
                {
                    try { dropDownAbteilung.SelectedValue = Session["21_Abteilung"].ToString(); }
                    catch { }
                }

                Session["22_Abteilung"] = null;
             
            }

            DrawTable(dropDownAbteilung.SelectedIndex+1);
            DrawPicker();
            picker_container.Style.Value = "display:none";

        }
        
        protected void DropDownAbteilung_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["22_Abteilung"] = dropDownAbteilung.SelectedValue;
            Response.Redirect("./");
        }

        private void DrawTable(int abteilung)
        {
            DataTable raeume = new DataTable();
            raeume = DatenbankAbrufen.Raeume(abteilung);
            Table collecter = new Table();

            TableRow headRow = new TableRow();
            headRow.CssClass = "head";

            TableCell hRaum, hID, hKlasse, hName;
            hRaum = new TableCell();
            hID = new TableCell();
            hKlasse = new TableCell();
            hName = new TableCell();

            hRaum.Text = "Raum-Nummer";
            hKlasse.Text = "Standardklasse";
            hName.Text = "Bezeichnung";
            hID.Text = "Bildschirm-ID";

            hRaum.CssClass = hKlasse.CssClass = hName.CssClass = hID.CssClass = "head";
           
            headRow.Cells.Add(hRaum);
            headRow.Cells.Add(hID);
            headRow.Cells.Add(hKlasse);
            headRow.Cells.Add(hName);

            tableRaeume.Rows.Add(headRow);

            foreach (DataRow dr in raeume.Rows)
            {

                TableRow tr = new TableRow();
                
                foreach (DataColumn dc in raeume.Columns)
                {
                    TableCell tc = new TableCell();

                    tc.Text = dr[dc].ToString();

                    if (dr[dc].ToString() == "" && dc.ColumnName == "Bildschirm-ID") tc.Text = "Ohne Bildschirm";

                    if (dc.ColumnName == "Raum") while (tc.Text.Length < 4) tc.Text = "0" + tc.Text;

                    if (dc.ColumnName == "StandardKlasse") 
                    {
                        if (tc.Text != "")
                        {
                            var divBt = new HtmlGenericControl("div");

                            Label lb = new Label();
                            lb.Text = dr[dc].ToString();
                            dataButton bt = new dataButton();
                            bt.Text = "✕";
                            bt.CssClass = "DeleteButton";
                            bt.Setting = lb.Text;
                            bt.Click += BtDelClick;

                            divBt.Controls.Add(bt);
                            divBt.Style.Value = "text-align: right; display: inline; float:right;";

                            tc.Controls.Add(lb);
                            tc.Controls.Add(divBt);
                        }
                        else
                        {
                            var divBt = new HtmlGenericControl("div");
                            dataButton bt = new dataButton();

                            bt.Text = "🞣";
                            bt.CssClass = "SaveButton";
                            bt.Setting = dr[0].ToString();
                            bt.Click += BtAddClick;
                            divBt.Controls.Add(bt);
                            divBt.Style.Value = "display: inline; float:right ";

                            tc.Controls.Add(divBt);
                        }
                        
                    }

                    if(dc.ColumnName == "Bezeichnung")
                    {

                        dataTextBox tb = new dataTextBox();
                        tb.Setting = dr[0].ToInt32();
                        tb.Style.Value = "width: 100%";
                        tb.TextChanged += TbBezeichnungTextChange;
                        tb.AutoPostBack = true;
                       
                        tb.Text = tc.Text;
                        tc.Controls.Add(tb);
                    }

                    tr.Cells.Add(tc);
                }
                tableRaeume.Rows.Add(tr);
            }           
        }

        private void BtAddClick(object o, EventArgs e)
        {
            picker_container.Style.Value = "display:block";
            string room = ((dataButton)o).Setting.ToString();
            Session["picker_room"] = room.ToInt32();
            while (room.Length < 4) room = "0" + room;
            setPickerHeader(room);
        }

        private Action<string> setPickerHeader;

        private void DrawPicker()
        {
            
            Panel picker = new Panel();
            Panel cover = new Panel();
            
            picker.CssClass = "picker";
            cover.CssClass = "cover";

            Label head = new Label { Text = "Standardklasse für Raum auswählen" };
            head.Font.Size = FontUnit.XLarge;
            var divHead = new HtmlGenericControl("div");
            Button btClose = new Button();
            btClose.CssClass = "DeleteButton";

            setPickerHeader = (s) => head.Text += " ("+s+")";

            btClose.Text = "✕";
            btClose.Click += (x, y) => { Response.Redirect("/admin/raeume"); } ;

            divHead.Controls.Add(btClose);
            divHead.Style.Value = "display: inline; float:right; margin-bottom:5px;";

            picker.Controls.Add(head);
            picker.Controls.Add(divHead);

            List<string> klassen = DatenbankAbrufen.KlassenWithoutRaum(dropDownAbteilung.SelectedIndex + 1);

            HtmlGenericControl divButtons = new HtmlGenericControl();
            divButtons.Style.Value = "display:block; margin-top = 20%;";

            foreach (string s in klassen)
            {
                dataButton db = new dataButton { Text = s };
                db.CssClass = "ActionButton";
                db.Click += BtKlasseAdd;
                db.Style.Value = "width: 30%; margin:10px;";
                db.Setting = s;
                db.Font.Size = FontUnit.Large;

                divButtons.Controls.Add(db);
            }

            picker.Controls.Add(divButtons);

            TextBox customKlasse = new TextBox();
            Label lbCustomKlasse = new Label { Text = "Benutzerdefinierte Belegung: " };
            dataButton btCustomKlasse = new dataButton { Text = "Hinzufügen", Setting = "" };
            btCustomKlasse.CssClass = "SaveButton";
            Label error = new Label();

            error.Style.Value = "display:block;margin:8px" ;
            error.ForeColor = System.Drawing.Color.Red;

            btCustomKlasse.ID = "custInput";
            btCustomKlasse.Style.Value = "margin-left: 8px;";
            btCustomKlasse.Click += (x, y) => btCustomKlasse.Setting = customKlasse.Text;
            btCustomKlasse.Click += BtKlasseAdd;

            picker.Controls.Add(lbCustomKlasse);
            picker.Controls.Add(customKlasse);
            picker.Controls.Add(btCustomKlasse);
            picker.Controls.Add(error);
          
            picker_container.Controls.Add(cover);
            picker_container.Controls.Add(picker);
        }
        private void BtDelClick(object o, EventArgs e)
        {
            dataButton sender = (dataButton)o;
            DatenbankSchreiben.DeleteStandardklasseFromRaeume((string)sender.Setting, dropDownAbteilung.SelectedIndex + 1);
            Response.Redirect("/admin/raeume");
        }

        private void TbBezeichnungTextChange(object o, EventArgs e)
        {

            dataTextBox tb = (dataTextBox)o;
            if (tb.Text.Length > 45)
            {
                tb.Text = tb.Text.Substring(0, 45);
                return;
            }
            DatenbankSchreiben.UpdateBezeichnungFromRaum((int)tb.Setting, tb.Text, dropDownAbteilung.SelectedIndex+1);
         
        }

        private void BtKlasseAdd(object o, EventArgs e)
        {
            picker_container.Style.Value = "display:block;";

            string value;
            int room;
            dataButton db = (dataButton)o;

            try
            {
                value = (string)db.Setting;
                room = (int)Session["picker_room"];
            }
            catch
            {
                ((Label)db.Parent.Controls[db.Parent.Controls.Count - 1]).Text = "Runtime-Error :(";
                return;
            }

            if (value.Length > 45 || value.Length <=0 )
            {
                ((Label)db.Parent.Controls[db.Parent.Controls.Count - 1]).Text = "Die Bezeichnung muss zwischen 1-45 Zeichen enthalten!";
                return;
            }

            if (DatenbankAbrufen.ColumnLike("Klassen","Klasse",value).Count != 0 && db.ID =="custInput")
            {
                ((Label)db.Parent.Controls[db.Parent.Controls.Count - 1]).Text = "Die Bezeichnung darf kein Duplikat einer existierenden Klasse sein!";
                return;
            }

            DatenbankSchreiben.UpdateStandardKlasseFromRaeume(room,value, dropDownAbteilung.SelectedIndex + 1);
            Session["picker_room"] = null;
            Response.Redirect("/admin/raeume");

            
        }

       
    }
}