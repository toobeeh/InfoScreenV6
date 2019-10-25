using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Infoscreen_Verwaltung.classes;
using System.Drawing;
using System.Web.Services;
using System.Web.Script.Services;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using Excel = Microsoft.Office.Interop.Excel;

namespace Infoscreen_Verwaltung.conf.supplierungen
{
    public partial class table : System.Web.UI.Page
    {

        string ebene = "../../";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!classes.Login.Angemeldet) Response.Redirect("../../login.aspx");
            if (Convert.ToInt32(classes.Login.Rechte.Supplierung) == 0) Response.Redirect("../../");

            TopMenu.Text = classes.Anzeige.TopBar(ebene);
            Menu.Text = classes.Anzeige.Menue(ebene, "conf");
            if (!Page.IsPostBack)
            {
                string[] abteilungen;
                if (classes.Login.Rechte.Supplierung[2] != '0') //Superadmin
                    abteilungen = classes.DatenbankAbrufen.AbteilungenAbrufen();
                else if (classes.Login.Rechte.Supplierung[1] != '0')  // Lehrer, AV, Schulwart
                    abteilungen = new string[] { "Elektronik" };  //wenn System von anderen Abteilungen verwendet wird: abteilungen = classes.Login.Abteilungen; 
                else abteilungen = new string[] { classes.Login.StammAbteilung }; //Schüler

                foreach (string abteilung in abteilungen)
                    dropDownAbteilung.Items.Add(abteilung);

                try { dropDownAbteilung.SelectedValue = classes.Login.StammAbteilung; }
                catch { dropDownAbteilung.SelectedIndex = -1; }
                if (Session["25_Abteilung"] != null)
                {
                    try { dropDownAbteilung.SelectedValue = Session["25_Abteilung"].ToString(); }
                    catch { }
                }
                Session["25_Abteilung"] = null;

                if (Session["25_RadioButton"] != null)
                {
                    try
                    {
                        if (Session["25_RadioButton"].ToString() == "Lehrer")
                        {
                            Sortieren.SelectedIndex = 0;
                        }
                        else
                        {
                            if (Session["25_RadioButton"].ToString() == "Ersatzlehrer")
                            {
                                Sortieren.SelectedIndex = 1;
                            }
                            else
                            {
                                if (Session["25_RadioButton"].ToString() == "Datum")
                                {
                                    Sortieren.SelectedIndex = 2;
                                }
                                else
                                {
                                    Sortieren.SelectedIndex = 3;
                                }
                            }
                        }
                    }
                    catch { }
                }
            }
            cont.Controls.Clear();
            SupplierungenZeichnen();
        }

        protected void DropDownAbteilung_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["25_Abteilung"] = dropDownAbteilung.SelectedValue;
            Response.Redirect("./table.aspx");
        }

        protected void SupplierungenZeichnen()
        {

            int abtID = DatenbankAbrufen.GetAbteilungsIdVonAbteilungsname(dropDownAbteilung.SelectedValue);

            //man bekommt für jeden Ersatz-Lehrer alle seine Supplierungen 
            classes.Structuren.LehrerSupplierungen[] daten = classes.DatenbankAbrufen.SupplierplanAbrufen(dropDownAbteilung.SelectedValue, false, "", "", false, Sortieren.SelectedItem.Text);
            classes.Structuren.GlobalEntfall[] globalEntfall = classes.DatenbankAbrufen.GetGlobaleEntfaelle(abtID);
            Label lb;
            Table tb;
            TableRow tr;
            TableCell tc;

            if (daten.Length == 0 && globalEntfall.Length == 0)
            {
                lb = new Label();
                lb.Text = "<h2>Derzeit keine Supplierungen</h2>";
                cont.Controls.Add(lb);
                return;
            }

            if (globalEntfall.Length != 0)
            {
                lb = new Label();
                lb.Text = "<h2>Lehrerkonferenzen</h2>";
                cont.Controls.Add(lb);

                tb = new Table();
                tb.CssClass = "tableSupplierung";
                tr = new TableRow();

                tc = new TableCell();
                tc.Text = "Datum";
                tc.CssClass = "head";
                tc.Width = 10;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = "Tag";
                tc.CssClass = "head";
                tc.Width = 6;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = "Von";
                tc.CssClass = "head";
                tc.Width = 63;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = "Bis";
                tc.CssClass = "head";
                tc.Width = 60;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = "Art der Supplierung";
                tc.CssClass = "head";
                //  tc.Width = 350;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.CssClass = "head";
                tc.Width = 10;
                tr.Cells.Add(tc);

                tb.Rows.Add(tr);

                foreach (classes.Structuren.GlobalEntfall eintrag in globalEntfall)
                {
                    tr = new TableRow();

                    tc = new TableCell(); //datum
                    tc.Text = eintrag.Datum.ToString("dd.MM.");
                    tr.Cells.Add(tc);

                    tc = new TableCell(); //wochentag
                    tc.Text = eintrag.Datum.ToString("dddd").Substring(0, 2);
                    tr.Cells.Add(tc);

                    tc = new TableCell(); //VonStunde
                    tc.Text = eintrag.VonStunde.GetZeitenOfStunde().Remove(5);
                    tr.Cells.Add(tc);

                    tc = new TableCell(); //BisStunde
                    tc.Text = eintrag.BisStunde.GetZeitenOfStunde().Remove(0, 8);
                    tr.Cells.Add(tc);

                    tc = new TableCell(); //Art der Supplierung
                    tc.Text = "Entfall für alle Klassen";
                    tc.CssClass = "entfällt";
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    dataButton button = new dataButton();
                    button.Setting = eintrag;
                    button.Text = "Löschen";
                    button.CssClass = "DeleteButton";
                    button.Click += bt_click_global;
                    tc.Controls.Add(button);
                    tr.Cells.Add(tc);
                    tb.Rows.Add(tr);
                }
                cont.Controls.Add(tb);
                lb = new Label();
                lb.Text = "<hr />";
                cont.Controls.Add(lb);
            }

            if (daten.Length != 0)
            {
                lb = new Label();
                lb.Text = "<h2>Supplierungen</h2>";
                cont.Controls.Add(lb);

                tb = new Table();
                tb.CssClass = "tableSupplierung";
                tr = new TableRow();

                tc = new TableCell();
                tc.Text = "Datum";
                tc.CssClass = "head";
                tc.Width = 10;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = "Tag";
                tc.CssClass = "head";
                tc.Width = 6;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = "Stunde";
                tc.CssClass = "head";
                tc.Width = 11;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = "Klasse";
                tc.CssClass = "head";
                tc.Width = 10;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = "Original-Lehrer";
                tc.CssClass = "head";
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = "Ersatz-Lehrer";
                tc.CssClass = "head";
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = "Fach";
                tc.CssClass = "head";
                tc.Width = 80;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = "Art der Supplierung";
                tc.CssClass = "head";
                tc.Width = 350;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.CssClass = "head";
                tc.Width = 10;
                tr.Cells.Add(tc);

                tb.Rows.Add(tr);

                foreach (classes.Structuren.LehrerSupplierungen temp in daten) //daten ist array von allen einträgen. temp ist ein einzelner eintrag
                {
                    foreach (classes.Structuren.Supplierungen zeile in temp.Supplierungen)
                    {
                        tr = new TableRow();

                        tc = new TableCell(); //datum
                        tc.Text = zeile.Datum.ToString("dd.MM.");
                        tr.Cells.Add(tc);

                        tc = new TableCell(); //wochentag
                        tc.Text = zeile.Datum.ToString("dddd").Substring(0, 2);
                        tr.Cells.Add(tc);

                        tc = new TableCell(); //stunde
                        tc.Text = zeile.Stunde.ToString();
                        tr.Cells.Add(tc);

                        tc = new TableCell(); //klasse
                        tc.Text = zeile.Klasse;
                        tr.Cells.Add(tc);

                        if (zeile.Entfällt) // Entfällt
                        {
                            tc = new TableCell();
                            tc.Text = zeile.Ursprungslehrer;
                            tr.Cells.Add(tc);

                            tc = new TableCell();
                            tc.Text = "-";
                            tr.Cells.Add(tc);

                            tc = new TableCell();
                            tc.Text = zeile.Ersatzfach;
                            tr.Cells.Add(tc);

                            tc = new TableCell();
                            tc.Text = "ENTFÄLLT";
                            tc.CssClass = "entfällt";
                            tr.Cells.Add(tc);

                            tc = new TableCell();
                            dataButton button = new dataButton();
                            button.Setting = zeile;
                            button.Text = "Löschen";
                            button.CssClass = "DeleteButton";
                            button.Click += bt_click;
                            tc.Controls.Add(button);
                            tr.Cells.Add(tc);
                        }
                        if (zeile.ZiehtVor >= 0) //Verschiebung
                        {
                            tc = new TableCell();
                            tc.Text = zeile.Ursprungslehrer;
                            tr.Cells.Add(tc);

                            tc = new TableCell();
                            tc.Text = "-";
                            tr.Cells.Add(tc);

                            tc = new TableCell();
                            tc.Text = zeile.Ersatzfach;
                            tr.Cells.Add(tc);

                            tc = new TableCell();
                            tc.Text = "Verschiebt auf: " + zeile.ZiehtVorDatum.ToString("dddd") + ", " + zeile.ZiehtVorDatum.ToString("dd.MM.") + ", " + zeile.ZiehtVor.ToString() + ". Stunde";
                            tc.CssClass = "vorgezogen";
                            tr.Cells.Add(tc);

                            tc = new TableCell();
                            dataButton button = new dataButton();
                            button.Setting = zeile;
                            button.Text = "Löschen";
                            button.CssClass = "DeleteButton";
                            button.Click += bt_click;
                            tc.Controls.Add(button);
                            tr.Cells.Add(tc);
                        }
                        if (!zeile.Entfällt && zeile.ZiehtVor < 0) //Supplierung
                        {
                            tc = new TableCell();
                            tc.Text = zeile.Ursprungslehrer;
                            tr.Cells.Add(tc);

                            tc = new TableCell();
                            tc.Text = zeile.Ersatzlehrer;
                            tr.Cells.Add(tc);

                            tc = new TableCell();
                            tc.Text = zeile.Ersatzfach;
                            tr.Cells.Add(tc);

                            tc = new TableCell();
                            tc.Text = "Wird suppliert";
                            tc.CssClass = "suppliert";
                            tr.Cells.Add(tc);

                            tc = new TableCell();
                            dataButton button = new dataButton();
                            button.Setting = zeile;
                            button.Text = "Löschen";
                            button.CssClass = "DeleteButton";
                            button.Click += bt_click;
                            tc.Controls.Add(button);
                            tr.Cells.Add(tc);
                        }
                        tb.Rows.Add(tr);
                    }
                    cont.Controls.Add(tb);
                }
            }
        }

        private void bt_click(object sender, EventArgs e)
        {
            dataButton bt = (dataButton)sender;

            Structuren.Supplierungen daten = ((Structuren.Supplierungen)bt.Setting);

            DatenbankSchreiben.SupplierungLoeschen(daten.Datum, daten.Klasse, daten.Stunde);

            Response.Redirect("./table.aspx");
        }
        private void bt_click_global(object sender, EventArgs e)
        {
            dataButton bt = (dataButton)sender;

            Structuren.GlobalEntfall daten = ((Structuren.GlobalEntfall)bt.Setting);

            DatenbankSchreiben.GlobalEntfallLöschen(daten.GlobalerEntfallID);

            Response.Redirect("./table.aspx");
        }
        protected void btHinzufuegen_Click(object sender, EventArgs e)
        {
            Session["13_Abteilung"] = null;
            Session["13_Klasse"] = null;
            Session["13_VonDatum"] = null;
            Session["13_AufDatum"] = null;
            Session["25_RadioButton"] = null;
            Response.Redirect("./default.aspx");
        }

        protected void Sortieren_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["25_RadioButton"] = Sortieren.SelectedItem.Text;
            Response.Redirect("./table.aspx");
        }

        protected void btGlobalEntfall_Click(object sender, EventArgs e)
        {
            Session["25_RadioButton"] = null;
            Response.Redirect("./globalsupplierung.aspx");
        }

        protected void btLehrerSupp_Click(object sender, EventArgs e)
        {
            Session["25_RadioButton"] = null;
            Session["25_Abteilung"] = null;
            Session["25_Lehrer"] = null;
            Session["25_Datum"] = null;
            Response.Redirect("./lehrersupplierung.aspx");
        }


        /*----------------------------------------------------------- A N F A N G   E X C E L -----------------------------------------------------------*/

        Excel.Application myExcelApplication;
        Excel.Workbook myExcelWorkbook;
        Excel.Worksheet myExcelWorkSheet;
        Excel.Range range;
        protected void ExcelGrundFormat()
        {
            range = myExcelApplication.get_Range("A1", "XFD1048576"); //alle Cells in Excel auswählen
            range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter; //Horizontale-Ausrichtung auf: Center
            range.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter; //Vertikale-Ausrichtung auf: Center
            range.NumberFormat = "@"; //Text-Format           
            range.RowHeight = 12; //Zeilenhöhe in cm (12cm = 16 Pixel)  

            range = myExcelWorkSheet.get_Range("A1");
            range.ColumnWidth = 39.86; //Spaltenbreite (für Spalte A)  in cm (39,86cm = 284 Pixel)
        }
        protected void ExcelHeadFormat(int row = 1)
        {
            string cell1, cell2, cell3;
            cell1 = "A" + row;
            row++;
            cell2 = "A" + row;
            row++;
            cell3 = "A" + row;
            row = row - 2;
            range = myExcelWorkSheet.get_Range(cell1, cell3);
            range.Font.Name = "Arial Black";
            range.BorderAround2(Excel.XlLineStyle.xlContinuous, Excel.XlBorderWeight.xlMedium);
            range.Font.Size = 10; // A1 Schriftgröße
            //range.Range[cell2].Font.Size = 11;
        }
        protected void ExcelHead(string lehrer, string grund, DateTime anfang, DateTime ende, int row = 1)
        {
            //vor split: (TT.MM.JJJJ HH:MM:SS)
            string[] aus1 = (anfang.ToString()).Split(' '); //0: Datum  1: Zeit
            string[] aus2 = (ende.ToString()).Split(' ');
            //nach split: (TT.MM.JJJJ) (HH:MM:SS)

            myExcelWorkSheet.Cells[row, 1] = "Supplierung für Prof. " + lehrer;
            row++;
            if (aus1[0] == aus2[0])
            {
                myExcelWorkSheet.Cells[row, 1] = "am " + aus1[0];
            }
            else
            {
                myExcelWorkSheet.Cells[row, 1] = "von " + aus1[0] + " bis " + aus2[0];
            }
            row++;
            if (grund != "")
            {
                myExcelWorkSheet.Cells[row, 1] = "wegen " + grund;
            }
            row = row - 2;
            ExcelHeadFormat(row);
        }
        protected void ExcelEintrag(string klasse, string ersatzlehrer, string fach, int stunde, bool entfall, int zeile = 4) //int ziehtvor, DateTime datum, )
        {
            if (klasse != "" && zeile > 3)
            {
                stunde++;
                /*if (ziehtvor != -1)
                {
                    string[] aus1 = (datum.ToString()).Split('.'); //0: Tag     1: Monat    2: Jahr + Zeit
                    //myExcelWorkSheet.Cells[zeile, stunde] = ersatzlehrer;
                    myExcelWorkSheet.Cells[zeile, stunde] = "von " + aus1[0] + "." + aus1[1] + ".";
                    range.get_Range(zeile, stunde).Font.Size = 12;
                    zeile++;
                    myExcelWorkSheet.Cells[zeile, stunde] = ziehtvor + ". Std";
                    range.get_Range(zeile, stunde).Font.Size = 12;
                    zeile++;
                    myExcelWorkSheet.Cells[zeile, stunde] = "verschoben";
                    range.get_Range(zeile, stunde).Font.Size = 11;
                    zeile = zeile - 2;
                    return;
                }*/
                myExcelWorkSheet.Cells[zeile, stunde] = klasse;
                zeile++;
                if (entfall)
                {
                    myExcelWorkSheet.Cells[zeile, stunde] = "ent-";
                    zeile++;
                    myExcelWorkSheet.Cells[zeile, stunde] = "fällt";
                    zeile = zeile - 2;
                    return;
                }
                myExcelWorkSheet.Cells[zeile, stunde] = ersatzlehrer;
                zeile++;
                myExcelWorkSheet.Cells[zeile, stunde] = fach;
                zeile = zeile - 2;
                stunde--;
            }
        }
        protected void DoubleLineFormat(string colname, int zeile, int lines = 3, bool doppeltelinie = true, string colname2 = "") //wenn stundenformat lines = 2, wenn eintragformat lines = 3
        {
            string cell1, cell2;
            cell1 = colname + zeile;
            zeile = zeile + lines;
            if (colname2 != "")
                cell2 = colname2 + zeile;
            else
                cell2 = colname + zeile;
            range = myExcelWorkSheet.get_Range(cell1, cell2);
            if (doppeltelinie)
                range.Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlDouble;
            range.Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlContinuous;
            range.Borders[Excel.XlBordersIndex.xlEdgeLeft].Weight = Excel.XlBorderWeight.xlThin;
            zeile = zeile - lines;
        }
        protected void ExcelEintragFormat(bool erstezeile = true, int zeile = 1) //wenn eintrag erstezeile = false
        {
            string cell, cell2;
            int lines;
            if (erstezeile)
                lines = 2;
            else
                lines = 3;
            /* C */
            DoubleLineFormat("C", zeile, lines);
            /* E */
            DoubleLineFormat("E", zeile, lines);
            /* G */
            DoubleLineFormat("G", zeile, lines, false);
            /* H */
            DoubleLineFormat("H", zeile, lines);
            /* J */
            DoubleLineFormat("J", zeile, lines);
            /* B1 - K1 */
            cell = "B" + zeile;
            cell2 = "K" + zeile;
            range = myExcelWorkSheet.get_Range(cell, cell2);
            range.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
            range.Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlThin;
            /* B1 - K3  (+ Schriftformatierung)*/
            cell = "B" + zeile;
            zeile = zeile + lines;
            cell2 = "K" + zeile;
            range = myExcelWorkSheet.get_Range(cell, cell2);
            range.BorderAround2(Excel.XlLineStyle.xlContinuous, Excel.XlBorderWeight.xlMedium);
            range.Font.Name = "Arial";
            range.Font.Size = 14;
            zeile = zeile - lines;

        }
        protected void ExcelDatum(DateTime datum, int zeile = 4)
        {
            string cell, cell2;
            int hilf = zeile + 3;
            cell = "A" + zeile;
            //zeile = zeile + 3;
            cell2 = "A" + hilf; //zeile + 3
            range = myExcelWorkSheet.get_Range(cell, cell2);
            range.Merge();
            range.BorderAround2(Excel.XlLineStyle.xlContinuous, Excel.XlBorderWeight.xlMedium);
            range.Font.Name = "Arial";
            range.Font.Size = 18;
            range.NumberFormat = "DDDD, DD.MM.YYYY";
            //range.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);

            myExcelWorkSheet.Cells[zeile, 1] = datum;
            //zeile = zeile - 3;
        }
        protected void ExcelStundenZeiten(int col = 1)
        {
            //Stunden und Stundenzeiten
            myExcelWorkSheet.Cells[col, 2] = "1";
            myExcelWorkSheet.Cells[col, 3] = "2";
            myExcelWorkSheet.Cells[col, 4] = "3";
            myExcelWorkSheet.Cells[col, 5] = "4";
            myExcelWorkSheet.Cells[col, 6] = "5";
            myExcelWorkSheet.Cells[col, 7] = "6";
            myExcelWorkSheet.Cells[col, 8] = "7";
            myExcelWorkSheet.Cells[col, 9] = "8";
            myExcelWorkSheet.Cells[col, 10] = "9";
            myExcelWorkSheet.Cells[col, 11] = "10";
            col++;
            myExcelWorkSheet.Cells[col, 2] = "08:00 -";
            myExcelWorkSheet.Cells[col, 3] = "08:50 -";
            myExcelWorkSheet.Cells[col, 4] = "09:50 -";
            myExcelWorkSheet.Cells[col, 5] = "10:40 -";
            myExcelWorkSheet.Cells[col, 6] = "11:40 -";
            myExcelWorkSheet.Cells[col, 7] = "12:30 -";
            myExcelWorkSheet.Cells[col, 8] = "13:20 -";
            myExcelWorkSheet.Cells[col, 9] = "14:20 -";
            myExcelWorkSheet.Cells[col, 10] = "15:10 -";
            myExcelWorkSheet.Cells[col, 11] = "16:10 -";
            col++;
            myExcelWorkSheet.Cells[col, 2] = "08:50";
            myExcelWorkSheet.Cells[col, 3] = "09:40";
            myExcelWorkSheet.Cells[col, 4] = "10:40";
            myExcelWorkSheet.Cells[col, 5] = "11:30";
            myExcelWorkSheet.Cells[col, 6] = "12:30";
            myExcelWorkSheet.Cells[col, 7] = "13:20";
            myExcelWorkSheet.Cells[col, 8] = "14:10";
            myExcelWorkSheet.Cells[col, 9] = "15:10";
            myExcelWorkSheet.Cells[col, 10] = "16:00";
            myExcelWorkSheet.Cells[col, 11] = "17:00";
        }
        protected void GetColumnChar(int col, int row, out string cell) //GetColumnChar funktioniert nur bis 26 = Z !!!
        {
            char column;
            switch (col)
            {
                case 1:
                    {
                        column = 'A';
                        cell = "" + column + row;
                        break;
                    }
                case 2:
                    {
                        column = 'B';
                        cell = "" + column + row;
                        break;
                    }
                case 3:
                    {
                        column = 'C';
                        cell = "" + column + row;
                        break;
                    }
                case 4:
                    {
                        column = 'D';
                        cell = "" + column + row;
                        break;
                    }
                case 5:
                    {
                        column = 'E';
                        cell = "" + column + row;
                        break;
                    }
                case 6:
                    {
                        column = 'F';
                        cell = "" + column + row;
                        break;
                    }
                case 7:
                    {
                        column = 'G';
                        cell = "" + column + row;
                        break;
                    }
                case 8:
                    {
                        column = 'H';
                        cell = "" + column + row;
                        break;
                    }
                case 9:
                    {
                        column = 'I';
                        cell = "" + column + row;
                        break;
                    }
                case 10:
                    {
                        column = 'J';
                        cell = "" + column + row;
                        break;
                    }
                case 11:
                    {
                        column = 'K';
                        cell = "" + column + row;
                        break;
                    }
                case 12:
                    {
                        column = 'L';
                        cell = "" + column + row;
                        break;
                    }
                case 13:
                    {
                        column = 'M';
                        cell = "" + column + row;
                        break;
                    }
                case 14:
                    {
                        column = 'N';
                        cell = "" + column + row;
                        break;
                    }
                case 15:
                    {
                        column = 'O';
                        cell = "" + column + row;
                        break;
                    }
                case 16:
                    {
                        column = 'P';
                        cell = "" + column + row;
                        break;
                    }
                case 17:
                    {
                        column = 'Q';
                        cell = "" + column + row;
                        break;
                    }
                case 18:
                    {
                        column = 'R';
                        cell = "" + column + row;
                        break;
                    }
                case 19:
                    {
                        column = 'S';
                        cell = "" + column + row;
                        break;
                    }
                case 20:
                    {
                        column = 'T';
                        cell = "" + column + row;
                        break;
                    }
                case 21:
                    {
                        column = 'U';
                        cell = "" + column + row;
                        break;
                    }
                case 22:
                    {
                        column = 'V';
                        cell = "" + column + row;
                        break;
                    }
                case 23:
                    {
                        column = 'W';
                        cell = "" + column + row;
                        break;
                    }
                case 24:
                    {
                        column = 'X';
                        cell = "" + column + row;
                        break;
                    }
                case 25:
                    {
                        column = 'Y';
                        cell = "" + column + row;
                        break;
                    }
                case 26:
                    {
                        column = 'Z';
                        cell = "" + column + row;
                        break;
                    }
                default:
                    {
                        column = ' ';
                        cell = "" + column + row;
                        break;
                    }
            }
        }
        protected void ExcelGlobalEntfall(string[] klassen, string[] lehrer, DateTime globaldatum, int vonstunde, int bisstunde)
        {
            int row = 1, col = 1;
            string cell1, cell2;
            string[] aus1 = (globaldatum.ToString()).Split(' ');
            myExcelWorkSheet.Name = "Konferenz";
            myExcelWorkSheet.Cells[row, col] = "Konferenz am " + aus1[0] + " von " + vonstunde + ". Stunde bis " + bisstunde + ". Stunde";
            // FORMATIERUNG
            GetColumnChar(col, row, out cell1);
            range = myExcelWorkSheet.get_Range(cell1);
            range.Font.Size = 24;
            range.Font.Name = "Arial Black";
            //ENDE FORMATIERUNG
            row += 4;
            //col++;
            for (int j = 0; j < klassen.Length; j++)
            {
                if (j == 8)
                {
                    col += 4;
                    row = 5;
                }
                myExcelWorkSheet.Cells[row, col] = klassen[j] + ":"; // Klasse Schreiben
                GetColumnChar(col, row, out cell1);
                row++;
                GetColumnChar(col, row, out cell2);
                range = myExcelWorkSheet.get_Range(cell1, cell2);
                range.Merge();
                range.Font.Name = "Arial";
                range.Font.Size = 20;
                range.ColumnWidth = 15;
                range.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight; //Horizontale-Ausrichtung auf: Rechts
                range.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter; //Vertikale-Ausrichtung auf: Center
                //Unterschriftslinie
                col++;
                GetColumnChar(col, row, out cell1);
                col++;
                GetColumnChar(col, row, out cell2);
                range = myExcelWorkSheet.get_Range(cell1, cell2);
                range.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
                range.Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlMedium;
                //zelle darunter merge
                //unterschrifts linie daneben (2 zellen lang)
                // row = 1, 2, 3, 4,...         col = A, B, C, D,...
                col -= 2;
                row += 6;
            }
            row = 5;
            col += 4;
            bool a = false;
            for (int j = 0; j < lehrer.Length; j++)
            {
                if (j == 0)
                {
                    j = 1;
                    a = true;
                }
                else
                    a = false;
                if (j % 8 == 0)
                {
                    col += 4;
                    row = 5;
                }
                if (a) j = 0;
                myExcelWorkSheet.Cells[row, col] = lehrer[j] + ":";
                GetColumnChar(col, row, out cell1);
                row++;
                GetColumnChar(col, row, out cell2);
                range = myExcelWorkSheet.get_Range(cell1, cell2);
                range.Merge();
                range.Font.Name = "Arial";
                range.Font.Size = 20;
                range.ColumnWidth = 15;
                range.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight; //Horizontale-Ausrichtung auf: Rechts
                range.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter; //Vertikale-Ausrichtung auf: Center
                //Unterschriftslinie
                col++;
                GetColumnChar(col, row, out cell1);
                col++;
                GetColumnChar(col, row, out cell2);
                range = myExcelWorkSheet.get_Range(cell1, cell2);
                range.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
                range.Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlMedium;
                //zelle darunter merge
                //unterschrifts linie daneben (2 zellen lang)
                // row = 1, 2, 3, 4,...         col = A, B, C, D,...
                col -= 2;
                row += 6;
            }
        }
        protected void ExcelBlockFormat(int col, int row)
        {
            string cell1, cell2;
            col++;
            GetColumnChar(col, row, out cell1);
            row += 3;
            col += 3;
            GetColumnChar(col, row, out cell2);
            range = myExcelWorkSheet.get_Range(cell1, cell2);
            range.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
        }
        struct BlockVergleich
        {
            public string ErsatzLehrer;
            public string OriginalLehrer;
            public string Klasse;
            public int Count;
            public DateTime Datum;
            public int Stunde;
        }
        protected void ExcelButton_Click(object sender, EventArgs e)
        {
            myExcelApplication = null;
            try
            {
                myExcelApplication = new Excel.Application();
                myExcelApplication.Visible = true;
                myExcelApplication.ScreenUpdating = true;

                var myCount = myExcelApplication.Workbooks.Count;
                myExcelWorkbook = (Excel.Workbook)(myExcelApplication.Workbooks.Add(System.Reflection.Missing.Value));
                var xlSheets = myExcelWorkbook.Sheets as Excel.Sheets;
                myExcelWorkSheet = (Excel.Worksheet)myExcelWorkbook.ActiveSheet;

                int abtID = DatenbankAbrufen.GetAbteilungsIdVonAbteilungsname(dropDownAbteilung.SelectedValue);

                //man bekommt für jeden Ersatz-Lehrer alle seine Supplierungen 
                classes.Structuren.LehrerSupplierungen[] daten = classes.DatenbankAbrufen.SupplierplanAbrufen(dropDownAbteilung.SelectedValue, false, "", "", false, "Lehrer");
                classes.Structuren.GlobalEntfall[] globalEntfall = classes.DatenbankAbrufen.GetGlobaleEntfaelle(abtID);

                string vergleich = "", grund = "";
                DateTime datum = DateTime.Today;
                DateTime start = DateTime.Today;
                DateTime globaldatum;
                int col = 4, i = 0, headcol = 1;
                string[] klassen, lehrer;
                int globalvon, globalbis;

                BlockVergleich blockvergleich;
                blockvergleich.Klasse = "";
                blockvergleich.ErsatzLehrer = "";
                blockvergleich.OriginalLehrer = "";
                blockvergleich.Count = 1;
                blockvergleich.Datum = DateTime.Today;
                blockvergleich.Stunde = 0;
                int zaehler = 0;


                if (daten.Length == 0 && globalEntfall.Length == 0) //wenn keine Supplierungen eingetragen sind
                {
                    myExcelWorkSheet.Cells[1, 1] = "Derzeit keine Supplierungen";
                    return;
                }
                else {
                    if (globalEntfall.Length != 0)
                    {
                        for (int w = 0; w < globalEntfall.Length; w++)
                        {
                            DatenbankAbrufen.GetLehrerGlobalEntfall(globalEntfall[w].GlobalerEntfallID, abtID, out klassen, out lehrer, out globaldatum, out globalvon, out globalbis);
                            ExcelGlobalEntfall(klassen, lehrer, globaldatum, globalvon, globalbis);
                        }
                    }
                    if (daten.Length != 0) //wenn Supplierungen eingetragen sind
                    {
                        int[] position = new int[daten[0].Supplierungen.Length];

                        foreach (classes.Structuren.LehrerSupplierungen temp in daten)
                        {
                            foreach (classes.Structuren.Supplierungen zeile in temp.Supplierungen)
                            {
                                if (blockvergleich.OriginalLehrer == zeile.Ursprungslehrer && blockvergleich.Datum == zeile.Datum && ++blockvergleich.Stunde == zeile.Stunde && blockvergleich.Klasse == zeile.Klasse && blockvergleich.ErsatzLehrer == zeile.Ersatzlehrer)
                                {
                                    blockvergleich.Count++;
                                    if (blockvergleich.Count == 4)
                                    {
                                        blockvergleich.Count = 1;
                                        position[zaehler - 3] = 1;
                                    }
                                    //System.Windows.Forms.MessageBox.Show("count: " + blockvergleich.Count.ToString());
                                }
                                else
                                {
                                    blockvergleich.Count = 1;
                                }
                                blockvergleich.OriginalLehrer = zeile.Ursprungslehrer;
                                blockvergleich.Datum = zeile.Datum;
                                blockvergleich.Stunde = zeile.Stunde;
                                blockvergleich.ErsatzLehrer = zeile.Ersatzlehrer;
                                blockvergleich.Klasse = zeile.Klasse;
                                zaehler++;
                            }
                        }
                        zaehler = 0;
                        foreach (classes.Structuren.LehrerSupplierungen temp in daten) //daten ist array von allen einträgen. temp ist ein einzelner eintrag
                        {
                            foreach (classes.Structuren.Supplierungen zeile in temp.Supplierungen)
                            {
                                //datum = zeile.Datum;
                                if (zeile.ZiehtVor != -1)
                                {

                                }
                                else if (vergleich != zeile.Ursprungslehrer) //wenn neuer original lehrer
                                {
                                    i++;
                                    col = 4; //erste Eintragszeile

                                    start = zeile.Datum;

                                    myExcelWorkSheet = (Excel.Worksheet)xlSheets.Add(xlSheets[i], Type.Missing, Type.Missing, Type.Missing); //neues Worksheet hinzufügen
                                    myExcelWorkSheet.Name = zeile.Ursprungslehrer;  //Worksheet Titel

                                    /*if (zeile.ZiehtVor != -1)
                                    {
                                        start = zeile.ZiehtVorDatum;
                                        ExcelHead(zeile.Ursprungslehrer, zeile.Grund, start, zeile.ZiehtVorDatum); //lehrer, von bis, grund schreiben
                                    }
                                    else
                                    {*/
                                    ExcelHead(zeile.Ursprungslehrer, zeile.Grund, start, zeile.Datum); //lehrer, von bis, grund schreiben
                                                                                                      //}
                                    ExcelGrundFormat(); //Grundformatierung

                                    ExcelDatum(zeile.Datum); //formatierung für das datum

                                    ExcelEintragFormat(); //formatierung für die stundenzeiten
                                    ExcelStundenZeiten(); //stundenzeiten schreiben

                                    ExcelEintragFormat(false, col); //formatierung für den eintrag
                                    ExcelEintrag(zeile.Klasse, zeile.Ersatzlehrer, zeile.Ersatzfach, zeile.Stunde, zeile.Entfällt); // zeile.ZiehtVor, zeile.ZiehtVorDatum); //eintrag schreiben
                                    //if (position[zaehler] == 1)
                                    //{
                                    //    ExcelBlockFormat(zeile.Stunde, col);
                                    //}

                                }
                                else //wenn selber original lehrer
                                {
                                    if (zeile.Grund != grund) //wenn anderer grund wie voriger eintrag --> neue Tabelle
                                    {
                                        col = col + 5; //nächste tabelle anfangen
                                        headcol = col;
                                        /* if (zeile.ZiehtVor != -1)
                                            ExcelHead(zeile.Ursprungslehrer, zeile.Grund, start, zeile.ZiehtVorDatum, col); //lehrer, von bis, grund schreiben*/
                                        //else
                                        start = zeile.Datum;
                                        ExcelHead(zeile.Ursprungslehrer, zeile.Grund, start, zeile.Datum, headcol); //lehrer, von bis, grund schreiben

                                        ExcelEintragFormat(true, col); //stundenzeitenformat
                                        ExcelStundenZeiten(col); //stundenzeiten schreiben

                                        col = col + 3; //eintragszeile auswählen

                                        ExcelDatum(zeile.Datum, col); //formatierung für das datum

                                        ExcelEintragFormat(false, col); //formatierung für eintrag
                                        ExcelEintrag(zeile.Klasse, zeile.Ersatzlehrer, zeile.Ersatzfach, zeile.Stunde, zeile.Entfällt, col); //, zeile.ZiehtVor, zeile.ZiehtVorDatum, col); //eintrag schreiben
                                    }
                                    else if (zeile.Datum == datum) //wenn selber grund und selbes datum wie voriger eintrag --> Eintrag in selber Zeile
                                    {
                                        ExcelEintrag(zeile.Klasse, zeile.Ersatzlehrer, zeile.Ersatzfach, zeile.Stunde, zeile.Entfällt, col); //, zeile.ZiehtVor, zeile.ZiehtVorDatum, col); //eintrag schreiben
                                        //if (position[zaehler] == 1)
                                        //{
                                        //    ExcelBlockFormat(zeile.Stunde, col);
                                        //}

                                    }
                                    else // (zeile.Grund == grund) (zeile.Datum != datum)        // wenn selber grund und anderes datum wie voriger eintrag --> neue Zeile
                                    {
                                        ExcelHead(zeile.Ursprungslehrer, zeile.Grund, start, zeile.Datum, headcol); //lehrer, von bis, grund schreiben
                                        col = col + 4; //nächste eintrags zeile
                                        ExcelEintragFormat(false, col); //formatierung für den eintrag
                                        ExcelEintrag(zeile.Klasse, zeile.Ersatzlehrer, zeile.Ersatzfach, zeile.Stunde, zeile.Entfällt, col); //, zeile.ZiehtVor, zeile.ZiehtVorDatum, col); //eintrag schreiben
                                        ExcelDatum(zeile.Datum, col); //formatierung für das datum
                                    }
                                }
                                grund = zeile.Grund;
                                datum = zeile.Datum; //datum des eintrags für vergleich von nächsten datum abspeichern
                                vergleich = zeile.Ursprungslehrer; //lehrer für vergleich abspeichern
                                if (position[zaehler] == 1)
                                {
                                    ExcelBlockFormat(zeile.Stunde, col);
                                    //System.Windows.Forms.MessageBox.Show("iiii");
                                }
                                zaehler++;
                            }
                        }
                    }
                    //myExcelWorkbook.Close(true, "C:\\supplierplan.xls", System.Reflection.Missing.Value);
                }
            }
            catch (Exception ex)
            {
                String myErrorString = ex.Message;
                LabelNachricht.Text = myErrorString;
                LabelNachricht.Visible = true;
            }
            finally
            {
                // Excel beenden 
                /*if (myExcelApplication != null)
                {
                    myExcelApplication.Quit();
                }*/
            }
        }
        /*----------------------------------------------------------- E N D E   E X C E L -----------------------------------------------------------*/
    }
}