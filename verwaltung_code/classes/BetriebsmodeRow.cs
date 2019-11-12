using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Infoscreen_Verwaltung.classes
{
    /// <summary>
    /// Diese Klasse ist von der Klasse TableRow abgeleitet und ist speziell für die einzelnen Zeilen der Betriebsmodetabelle entwickelt.
    /// </summary>
    public class BetriebsmodeRow : TableRow
    {
        private Label bildschirmID, standardKlasse, abteilung, gebäude, raum, anzeigeArt;
        private CheckBox stundenplan, abteilungsinfo, sprechstunden, raumaufteilung, supplierplan, aktuelleSupplierungen;
        DropDownList powerPoints;

        /// <summary>
        /// Erstellt eine neue BetriebsmodeRow und weißt die nötigen Variablen zu.
        /// </summary>
        /// <param name="_BildschirmID">Die ID des Bildschirmes</param>
        /// <param name="_Gebäude">Das Gebäude in welchem sich der Bildschirm befindet</param>
        /// <param name="_Raum">Der Raum über welchem sich der Bildschirm befindet</param>
        /// <param name="_StandardKlasse">Die Klasse</param>
        /// <param name="_Abteilung">Die Abteilung in welcher sich der Bildschirm befindet</param>
        /// <param name="_AnzeigeArt">Die Position an welcher sich der Bildschirm befindet</param>
        /// <param name="_AnzeigeElemente">Die Elemente die am Bildschirm angezeigt werden sollen</param>
        /// <param name="_PowerPoints">Die Liste der PowerPoint Dateien, welche zur Auswahl stehen</param>
        public BetriebsmodeRow(int _BildschirmID, string _Gebäude, int _Raum, string _StandardKlasse, string _Abteilung, int _AnzeigeArt, Structuren.AnzeigeElemente _AnzeigeElemente, Structuren.Dateien[] _PowerPoints)
        {
            Init();

            foreach(Structuren.Dateien x in _PowerPoints)
            {
                powerPoints.Items.Add(new ListItem(x.bezeichnung, x.id.ToString()));
            }

            BildschirmID = _BildschirmID;
            Gebäude = _Gebäude;
            Raum = _Raum;
            StandardKlasse = _StandardKlasse;
            Abteilung = _Abteilung;
            AnzeigeArt = _AnzeigeArt;
            AnzuzeigendeElemente = _AnzeigeElemente;

            standardKlasse.ID = "id-" + BildschirmID + "-kls";
            stundenplan.ID = "id-" + BildschirmID + "-stu";
            abteilungsinfo.ID = "id-" + BildschirmID + "-abt";
            sprechstunden.ID = "id-" + BildschirmID + "-spr";
            raumaufteilung.ID = "id-" + BildschirmID + "-rau";
            supplierplan.ID = "id-" + BildschirmID + "-sup";
            aktuelleSupplierungen.ID = "id-" + BildschirmID + "-akt";
            powerPoints.ID = "id-" + BildschirmID + "-pow";

            Ausgeben();
        }

        /// <summary>
        /// Initialisiert alle Elemente der Klasse
        /// </summary>
        private void Init()
        {
            bildschirmID = new Label();
            abteilung = new Label();
            gebäude = new Label();
            raum = new Label();
            standardKlasse = new Label();
            anzeigeArt = new Label();
            stundenplan = new CheckBox();
            abteilungsinfo = new CheckBox();
            sprechstunden = new CheckBox();
            raumaufteilung = new CheckBox();
            supplierplan = new CheckBox();
            aktuelleSupplierungen = new CheckBox();
            powerPoints = new DropDownList();


            powerPoints.Items.Add(new ListItem("----Keine----", "-1"));
        }

        /// <summary>
        /// Erstellt die Zeile neu, indem der Inhalt gelöscht wird und die einzelnen Zellen
        /// neu hinzugefügt werden.
        /// </summary>
        private void Ausgeben()
        {
            this.Cells.Clear();

            TableCell cell = new TableCell();
            cell.Controls.Add(bildschirmID);
            this.Cells.Add(cell);

            cell = new TableCell();
            cell.Controls.Add(gebäude);
            this.Cells.Add(cell);

            cell = new TableCell();
            cell.Controls.Add(raum);
            this.Cells.Add(cell);

            cell = new TableCell();
            cell.Controls.Add(standardKlasse);
            this.Cells.Add(cell);

            cell = new TableCell();
            cell.Controls.Add(stundenplan);
            this.Cells.Add(cell);

            cell = new TableCell();
            cell.Controls.Add(abteilungsinfo);
            this.Cells.Add(cell);

            cell = new TableCell();
            cell.Controls.Add(sprechstunden);
            this.Cells.Add(cell);

            cell = new TableCell();
            cell.Controls.Add(raumaufteilung);
            this.Cells.Add(cell);

            cell = new TableCell();
            cell.Controls.Add(supplierplan);
            this.Cells.Add(cell);

            cell = new TableCell();
            cell.Controls.Add(aktuelleSupplierungen);
            this.Cells.Add(cell);

            cell = new TableCell();

            HtmlGenericControl wrap = new HtmlGenericControl("div");
            wrap.Attributes.Add("class", "selectWrap");
            wrap.Controls.Add(powerPoints);

            cell.Controls.Add(wrap);
            this.Cells.Add(cell);
        }

        /// <summary>
        /// Ruft die Bildschirm ID ab oder legt diese fest.
        /// </summary>
        public int BildschirmID
        {
            get { return bildschirmID.Text.ToInt32(); }
            set { bildschirmID.Text = value.ToString(); }
        }

        /// <summary>
        /// Ruft die StandardKlasse ab oder legt diese fest.
        /// </summary>
        public string StandardKlasse
        {
            get { return standardKlasse.Text; }
            set { standardKlasse.Text = value; }
        }

        /// <summary>
        /// Ruft den Raum ab oder legt diesen fest.
        /// </summary>
        public int Raum
        {
            get { return raum.Text.ToInt32(); }
            set { raum.Text = value.ToString("D4"); }
        }

        /// <summary>
        /// Ruft die Anzeigeart ab oder legt diese fest.
        /// </summary>
        public int AnzeigeArt
        {
            get
            {
                switch (anzeigeArt.Text)
                {
                    case "Über Klasse":
                        return 0;
                    case "Über Raum":
                        return 1;
                    case "Im Gang":
                        return 2;
                    default:
                        return -1;
                }
            }
            set
            {
                switch (value)
                {
                    case 0:
                        anzeigeArt.Text = "Über Klasse";
                        break;
                    case 1:
                        anzeigeArt.Text = "Über Raum";
                        break;
                    case 2:
                        anzeigeArt.Text = "Im Gang";
                        break;
                    default:
                        anzeigeArt.Text = "Unbekannt";
                        break;
                }
            }
        }

        /// <summary>
        /// Ruft das Gebäude ab oder legt dieses fest.
        /// </summary>
        public string Gebäude
        {
            get { return gebäude.Text; }
            set { gebäude.Text = value; }
        }

        /// <summary>
        /// Ruft die Abteilung ab oder legt diese fest.
        /// </summary>
        public string Abteilung
        {
            get { return abteilung.Text; }
            set { abteilung.Text = value; }
        }

        /// <summary>
        /// Ruft die Anzuzeigenden Elemente ab oder legt diese fest.
        /// </summary>
        public Structuren.AnzeigeElemente AnzuzeigendeElemente
        {
            get
            {
                Structuren.AnzeigeElemente temp = new Structuren.AnzeigeElemente();
                temp.Abteilungsinfo = abteilungsinfo.Checked;
                temp.AktuelleSupplierungen = aktuelleSupplierungen.Checked;
                if (powerPoints.SelectedIndex == 0) temp.PowerPoints = -1;
                else temp.PowerPoints = powerPoints.SelectedValue.ToInt32();
                temp.Raumaufteilung = raumaufteilung.Checked;
                temp.Sprechstunden = sprechstunden.Checked;
                temp.Stundenplan = stundenplan.Checked;
                temp.Supplierplan = supplierplan.Checked;

                return temp;
            }

            set
            {
                abteilungsinfo.Checked = value.Abteilungsinfo;
                aktuelleSupplierungen.Checked = value.AktuelleSupplierungen;
                if (value.PowerPoints == -1) powerPoints.SelectedIndex = 0;
                else try { powerPoints.SelectedValue = value.PowerPoints.ToString(); }
                    catch { powerPoints.SelectedIndex = 0; }
                raumaufteilung.Checked = value.Raumaufteilung;
                sprechstunden.Checked = value.Sprechstunden;
                stundenplan.Checked = value.Stundenplan;
                supplierplan.Checked = value.Supplierplan;
            }
        }
    }
}