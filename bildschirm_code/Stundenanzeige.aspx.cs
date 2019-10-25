using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Infoscreen_Bibliotheken;

namespace Infoscreen_Bildschirme
{
    public partial class Stundenanzeige : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            // Objekte anlegen
            string KlassenID = Request.QueryString["id"];
            string Stammklasse = Infoscreen_Verwaltung.classes.DatenbankAbrufen.RauminfoAbrufen(KlassenID).Stammklasse;
            string AktuelleKlasse = Infoscreen_Verwaltung.classes.DatenbankAbrufen.RauminfoAbrufen(KlassenID).AktuelleKlasse;
            bool StundeNull = false;
            bool Samstagsunterricht = false;
            Bildschirm_Methoden Methoden = new Bildschirm_Methoden();
            // Stundenplandaten aus der Datenbank abrufen
            Structuren.StundenplanTag[] Stundenplan = FachHelper.StundenplanAbrufen(Stammklasse, false);

            bool normaleStunde = false;
            Structuren.StundenplanEntry dummy1;
            List<Structuren.StundenplanEntry> dummy0;
            int y2 = 0;

            for (int i2 = 0; i2 < Stundenplan.Length; i2++)  // Anzahl der Wochentage
            {
                int[] vorgezogeneStunden = FachHelper.ZiehtVorStundeNeu(Stundenplan[i2].Datum, Stammklasse); //holt alle Stunden dieses Tages auf die verschoben wird

                for (int y1 = 0; y1 < vorgezogeneStunden.Length; y1++) //Durchlauf aller stunden des Tages auf die verschoben wurde
                {
                    for (y2 = 0; y2 < Stundenplan.Length; y2++) //Durchlauf aller Stunden des Tages
                    {
                        if (Stundenplan[i2].StundenDaten[y2].Stunde == vorgezogeneStunden[y1])
                        {
                            normaleStunde = true;
                            break;
                        }
                    }
                    if (!normaleStunde)
                    {
                        dummy1 = new Structuren.StundenplanEntry();
                        dummy1.Stunde = vorgezogeneStunden[y1];
                        dummy1.Lehrer = "";
                        dummy1.Fach = "";
                        dummy1.Gebäude = "";
                        dummy1.Raum = 307;
                        dummy1.Supplierung = false;
                        dummy1.ZiehtVor = 0;
                        dummy1.Entfällt = false;

                        dummy0 = new List<Structuren.StundenplanEntry>();
                        dummy0.AddRange(Stundenplan[i2].StundenDaten);
                        dummy0.Add(dummy1);
                        Stundenplan[i2].StundenDaten = dummy0.ToArray();
                    }
                    else
                    {
                        normaleStunde = false;
                    }
                }
            }

            if (Stundenplan.Length > 5) Samstagsunterricht = true;
            // Klasenbezeichnung auslesen
            lb_Klassentietel.Text = Infoscreen_Verwaltung.classes.DatenbankAbrufen.RauminfoAbrufen(KlassenID).Stammklasse;
            // Rauminformationen hinzufügen
            try
            {
                lb_Raumnummer.Text = Infoscreen_Verwaltung.classes.DatenbankAbrufen.RauminfoAbrufen(KlassenID).Gebäude + " - " + Convert.ToInt32(DatenbankAbrufen.RauminfoAbrufen(KlassenID).Raumnummer).ToString("0000");
            }
            catch
            {
                lb_Raumnummer.Text = Infoscreen_Verwaltung.classes.DatenbankAbrufen.RauminfoAbrufen(KlassenID).Gebäude + " - " + Infoscreen_Verwaltung.classes.DatenbankAbrufen.RauminfoAbrufen(KlassenID).Raumnummer;
            }
            lb_Klasse.Text = Stammklasse;
            lb_Klassenvorstand.Text = "Prof. " + FachHelper.KvAusKlasse(Stammklasse);
            // Prüfen, ob es eine zweite Klasse gibt, die in diesem Raum unterrichtet wird
            if (AktuelleKlasse == Stammklasse)
            {
                // Zusätzlichen informationsbereich trazparent schalten
                ZweiteInformationsleisteAusblenden();
            }
            else
            {
                // Die Informationen aus der Datenbank abrufen
                lb_Klasse_1.Text = Stammklasse;
                lb_Klasse_2.Text = AktuelleKlasse;
                // Überprüfen, ob zwei mal die selbe Klasse ausgewählt wurde
                if (lb_Klasse_1.Text == lb_Klasse_2.Text)
                {
                    ZweiteInformationsleisteAusblenden();
                }
                else
                {
                    div_Klasseninformation_1.Style.Add("height", "50%");
                    div_Klasseninformation_2.Style.Add("height", "50%");
                }
                lb_Klasse_1_Informationen.Text = Infoscreen_Verwaltung.classes.DatenbankAbrufen.KlasseninfoAbrufen(Stammklasse);
                lb_Klasse_2_Informationen.Text = Infoscreen_Verwaltung.classes.DatenbankAbrufen.KlasseninfoAbrufen(AktuelleKlasse);
            }
            // Die fertige Tabelle erzeugen

            int min, max;
            min = HelperNew.KleinsteStunde(Stundenplan);
            max = HelperNew.GrößteStunde(Stundenplan);
            int aktuellerTag = Infoscreen_Verwaltung.classes.DatenbankAbrufen.AktuellerTag();
            int aktuelleStunde = Infoscreen_Verwaltung.classes.DatenbankAbrufen.AktuelleStunde();
            TableRow row;
            TableRow row2;
            TableCell cell;

            row = new TableRow();
            row2 = new TableRow();
            cell = new TableCell();

            if (Stundenplan.Length < 1)
            {
                cell.Text = "Zu dieser Klasse liegen akzuell keine Daten vor.";
                row.Cells.Add(cell);
                table_Stundenanzeige.Rows.Add(row);
            }
            else
            {
                cell.SkinID = "Stundenanzeige_Wochentagsüberschrifft";
                row.Cells.Add(cell);
                cell = new TableCell();
                cell.SkinID = "Stundenanzeige_Wochentagsdatum";
                row2.Cells.Add(cell);

                cell = new TableCell();
                cell.SkinID = "Stundenanzeige_Wochentagsüberschrifft";
                cell.Text = "Montag";
                row.Cells.Add(cell);
                cell = new TableCell();
                cell.SkinID = "Stundenanzeige_Wochentagsdatum";
                cell.Text = Stundenplan[0].Datum.ToString("dd.MM.yyyy");
                row2.Cells.Add(cell);

                cell = new TableCell();
                cell.SkinID = "Stundenanzeige_Wochentagsüberschrifft";
                cell.Text = "Dienstag";
                row.Cells.Add(cell);
                cell = new TableCell();
                cell.SkinID = "Stundenanzeige_Wochentagsdatum";
                cell.Text = Stundenplan[1].Datum.ToString("dd.MM.yyyy");
                row2.Cells.Add(cell);

                cell = new TableCell();
                cell.SkinID = "Stundenanzeige_Wochentagsüberschrifft";
                cell.Text = "Mittwoch";
                row.Cells.Add(cell);
                cell = new TableCell();
                cell.SkinID = "Stundenanzeige_Wochentagsdatum";
                cell.Text = Stundenplan[2].Datum.ToString("dd.MM.yyyy");
                row2.Cells.Add(cell);

                cell = new TableCell();
                cell.SkinID = "Stundenanzeige_Wochentagsüberschrifft";
                cell.Text = "Donnerstag";
                row.Cells.Add(cell);
                cell = new TableCell();
                cell.SkinID = "Stundenanzeige_Wochentagsdatum";
                cell.Text = Stundenplan[3].Datum.ToString("dd.MM.yyyy");
                row2.Cells.Add(cell);

                cell = new TableCell();
                cell.SkinID = "Stundenanzeige_Wochentagsüberschrifft";
                cell.Text = "Freitag";
                row.Cells.Add(cell);
                cell = new TableCell();
                cell.SkinID = "Stundenanzeige_Wochentagsdatum";
                cell.Text = Stundenplan[4].Datum.ToString("dd.MM.yyyy");
                row2.Cells.Add(cell);

                if (Stundenplan.Length > 5)
                {
                    cell = new TableCell();
                    cell.SkinID = "Stundenanzeige_Wochentagsüberschrifft";
                    cell.Text = "Samstag";
                    row.Cells.Add(cell);
                    cell = new TableCell();
                    cell.SkinID = "Stundenanzeige_Wochentagsdatum";
                    cell.Text = Stundenplan[5].Datum.ToString("dd.MM.yyyy");
                    row2.Cells.Add(cell);
                }
                table_Stundenanzeige.Rows.Add(row);
                table_Stundenanzeige.Rows.Add(row2);

                for (int i1 = min; i1 <= max; i1++)
                {
                    row = new TableRow();
                    cell = new TableCell();
                    cell.Text = Methoden.Unterrichtsuhrzeit(i1);
                    cell.SkinID = "Stundenanzeige_Zeiten";
                    row.Cells.Add(cell);

                    for (int i2 = 0; i2 < Stundenplan.Length; i2++) //Anzahl der Wochentage
                    {
                        int[] vorgezogeneStunden = FachHelper.ZiehtVorStundeNeu(Stundenplan[i2].Datum, Stammklasse); //holt alle vorgezogenen Stunden dieses Tages

                        cell = new TableCell();
                        cell.SkinID = "Stundenanzeige_Stundenübersicht";
                        if (i2 == aktuellerTag)
                        {
                            cell.SkinID = "Stundenanzeige_Stundenübersicht_AktiverTag";
                            if (i1 == aktuelleStunde) cell.SkinID = "Stundenanzeige_Stundenübersicht_AktiveStunde";
                        }
                        for (int i3 = 0; i3 < Stundenplan[i2].StundenDaten.Length; i3++) // Durchlauf aller Stunden eines Tages
                        {
                            if (Stundenplan[i2].StundenDaten[i3].Stunde == i1)
                            {
                                cell.Text = Stundenplan[i2].StundenDaten[i3].Fach;
                                if (Stundenplan[i2].StundenDaten[i3].Supplierung)
                                {
                                    cell.Text = "(" + Stundenplan[i2].StundenDaten[i3].Ersatzlehrer + ")";
                                    if (i2 == aktuellerTag)
                                    {
                                        cell.SkinID = "Stundenanzeige_Stundenübersicht_AktiverTag_Supplierung";
                                        if (i1 == aktuelleStunde) cell.SkinID = "Stundenanzeige_Stundenübersicht_AktiveStunde_Supplierung";
                                    }
                                    else
                                    {
                                        cell.SkinID = "Stundenanzeige_Stundenübersicht_Supplierung";
                                    }
                                }
                                if (Stundenplan[i2].StundenDaten[i3].ZiehtVor > -1)
                                {
                                    cell.Text = "verschoben";
                                    if (i2 == aktuellerTag)
                                    {
                                        cell.SkinID = "Stundenanzeige_Stundenübersicht_AktiverTag_Vorgezogen";
                                        if (i1 == aktuelleStunde) cell.SkinID = "Stundenanzeige_Stundenübersicht_AktiveStunde_Vorgezogen";
                                    }
                                    else
                                    {
                                        cell.SkinID = "Stundenanzeige_Stundenübersicht_Vorgezogen";
                                    }
                                }
                                if (Stundenplan[i2].StundenDaten[i3].Entfällt)
                                {
                                    cell.Text = "entfällt";
                                    if (i2 == aktuellerTag)
                                    {
                                        cell.SkinID = "Stundenanzeige_Stundenübersicht_AktiverTag_Supplierung";
                                        if (i1 == aktuelleStunde) cell.SkinID = "Stundenanzeige_Stundenübersicht_AktiveStunde_Supplierung";
                                    }
                                    else
                                    {
                                        cell.SkinID = "Stundenanzeige_Stundenübersicht_Supplierung";
                                    }
                                }
                                for (int i4 = 0; i4 < vorgezogeneStunden.Length; i4++) //Durchlauf aller vorgezogenen Stunden für diesen Tag
                                {
                                    if (Stundenplan[i2].StundenDaten[i3].Stunde == vorgezogeneStunden[i4]) //Wenn die jetzige Stunde eine vorgezogene ist
                                    {
                                        string ersatzlehrer, ersatzfach;
                                        DateTime verschiebtVon = FachHelper.GetVerschiebtVonDatum(Stundenplan[i2].Datum, Stammklasse, vorgezogeneStunden[i4], out ersatzlehrer, out ersatzfach);

                                        cell.Text = "(" + ersatzlehrer + ")";

                                        if (i2 == aktuellerTag)
                                        {
                                            cell.SkinID = "Stundenanzeige_Stundenübersicht_AktiverTag_Vorgezogen";
                                            if (i1 == aktuelleStunde) cell.SkinID = "Stundenanzeige_Stundenübersicht_AktiveStunde_Vorgezogen";
                                        }
                                        else
                                        {
                                            cell.SkinID = "Stundenanzeige_Stundenübersicht_Vorgezogen";
                                        }                                      
                                    }
                                }
                                break;
                            }
                        }
                        //
                        row.Cells.Add(cell);
                    }
                    table_Stundenanzeige.Rows.Add(row);
                }
            }
        }


        /// <summary>
        /// Fügt dem aktuellen Datenarray, welches nur den Stundenplan ehthalten sollte, Zusatzinformationen wie Stunde des Unterrichts,
        /// Zeit von wann bis wann unterrichtet wird und den Wochentag des Unterrichtes hinzu
        /// </summary>
        /// <param name="Daten">Enthällt den "nakten" Stundenplan</param>
        /// <param name="StundeNull">Ob eine Nullte-Stunde unterrichtet wird</param>
        /// <param name="Samstagsunterricht">Ob am Samstag unterrichtet wird</param>
        /// <returns>Stundenplan inklusive den Zusatzinformationen, welche in der Summary beschrieben wurden</returns>
        public Structuren.Tabellendaten[,] Stundenplan_RandinformationHinzufügen(Structuren.Tabellendaten[,] Daten, bool StundeNull, bool Samstagsunterricht)
        {
            Bildschirm_Methoden Unterprogramme = new Bildschirm_Methoden();
            int HinzugefügeSpalten = 2, HinzugefügeZeilen = 1;
            // Ein neues Array anlegen, welches genug Platz für Stundenplan und Umgebungsinformationen wie Tag Uhrzeit etc. enthällt
            Structuren.Tabellendaten[,] Informationen = new Structuren.Tabellendaten[Daten.GetLength(0) + HinzugefügeZeilen, Daten.GetLength(1) + HinzugefügeSpalten];
            // Die Spalten-Überschrifften hinzufügen
            Unterprogramme.Tabellendaten_InformationHinzufügen(ref Informationen[0, 0], "Std", "Stundenanzeige_Wochentagsüberschrifft");
            Unterprogramme.Tabellendaten_InformationHinzufügen(ref Informationen[0, 1], "Zeit", "Stundenanzeige_Wochentagsüberschrifft");
            Unterprogramme.Tabellendaten_InformationHinzufügen(ref Informationen[0, 2], "MONTAG", "Stundenanzeige_Wochentagsüberschrifft");
            Unterprogramme.Tabellendaten_InformationHinzufügen(ref Informationen[0, 3], "DIENSTAG", "Stundenanzeige_Wochentagsüberschrifft");
            Unterprogramme.Tabellendaten_InformationHinzufügen(ref Informationen[0, 4], "MITTWOCH", "Stundenanzeige_Wochentagsüberschrifft");
            Unterprogramme.Tabellendaten_InformationHinzufügen(ref Informationen[0, 5], "DONNERSTAG", "Stundenanzeige_Wochentagsüberschrifft");
            Unterprogramme.Tabellendaten_InformationHinzufügen(ref Informationen[0, 6], "FREITAG", "Stundenanzeige_Wochentagsüberschrifft");
            if (Samstagsunterricht == true)
            {
                Unterprogramme.Tabellendaten_InformationHinzufügen(ref Informationen[0, 7], "SAMSTAG", "Stundenanzeige_Wochentagsüberschrifft");
            }
            // Die Stunden und Uhrzeiten in die Tabelle einsetzten
            for (int Zeile = HinzugefügeZeilen; Zeile < Informationen.GetLength(0); Zeile++)
            {
                if (StundeNull == false)
                {
                    Unterprogramme.Tabellendaten_InformationHinzufügen(ref Informationen[Zeile, 0], (Zeile).ToString(), "Stundenanzeige_Stunden");
                    Unterprogramme.Tabellendaten_InformationHinzufügen(ref Informationen[Zeile, 1], Unterprogramme.Unterrichtsuhrzeit(Zeile), "Stundenanzeige_Zeiten");
                }
                else
                {
                    Unterprogramme.Tabellendaten_InformationHinzufügen(ref Informationen[Zeile, 0], (Zeile - 1).ToString(), "Stundenanzeige_Stunden");
                    Unterprogramme.Tabellendaten_InformationHinzufügen(ref Informationen[Zeile, 1], Unterprogramme.Unterrichtsuhrzeit(Zeile - 1), "Stundenanzeige_Zeiten");
                }
            }
            // Die einzelen Unterrichtsgegenstände in den vergrößerten Datenspeicher einsetzten
            for (int Zeile = HinzugefügeZeilen; Zeile < Informationen.GetLength(0); Zeile++)
            {
                for (int Spalte = HinzugefügeSpalten; Spalte < Informationen.GetLength(1); Spalte++)
                {
                    Unterprogramme.Tabellendaten_InformationHinzufügen(ref Informationen[Zeile, Spalte], Daten[Zeile - HinzugefügeZeilen, Spalte - HinzugefügeSpalten].Text, Daten[Zeile - HinzugefügeZeilen, Spalte - HinzugefügeSpalten].SkinID);
                }
            }
            // Die modifizierten Daten übergeben
            return Informationen;
        }

        /// <summary>
        /// Belndet die zweite Informationsleiste der Informationswand aus
        /// </summary>
        private void ZweiteInformationsleisteAusblenden()
        {
            lb_Klasse_2.Visible = false;
            lb_Klasse_2_Informationen.Visible = false;
        }
    }
}