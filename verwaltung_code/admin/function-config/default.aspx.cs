using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Infoscreen_Verwaltung.classes;
using System.IO;
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;

namespace Infoscreen_Verwaltung.admin
{
    public partial class FeatureConfig : System.Web.UI.Page
    {
        string ebene = "../../";

        List<FeatureSetting> Settings = new List<FeatureSetting>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!classes.Login.Angemeldet) Response.Redirect("../../login.aspx");
            if (!classes.Login.Rechte.Bildschirme.OneBiggerThan(1)) Response.Redirect("../../");

            TopMenu.Text = classes.Anzeige.TopBar(ebene);
            Menu.Text = classes.Anzeige.Menue(ebene, "admin");

            AddFeatureSettings();
            GetValues();
            DrawFeatureList();
        }

        private void AddFeatureSettings()
        {
            // Create setting objects for each setting


            Settings.Add(new IntegerFeatureSetting("switchTimeMs", "Seiten-Anzeigedauer", switchTimeMs,
               "Anzeigedauer in Sek. der einzelnen Seiten wie Stundenplan, Powerpoint oder Raumübersicht", null, 10000));

            Settings.Add(new BoolFeatureSetting("animateUpcomingExamLessons", "Tests anzeigen", animateUpcomingExamLessons,
                "Animierte Anzeige der kommenden Tests im Stundenplan", true));

            Settings.Add(new IntegerFeatureSetting("showExamTimeMs", "Tests-Anzeigedauer", showExamTimeMs,
                "Anzeigedauer in Sek. der Test-Markierung im Stundenplan", null, 2500));

            Settings.Add(new IntegerFeatureSetting("examTimesShown", "Test-Anzeigeintervall", examTimesShown,
               "Das Intervall in Sek., nach dem Tests für die definierte Zeit eingeblendet werden", null, 2));

            Settings.Add(new IntegerFeatureSetting("examAnimationDuration", "Test-Animationsdauer", examAnimationDuration,
               "Die Dauer in Sek. der Ein/Ausblendeanimation der Tests (0 = Nicht animiert)", null, 500));

            Settings.Add(new BoolFeatureSetting("--displayClockTile", "Uhr", displayClockTile,
                "Anzeige einer digitalen Uhr in der Titelleiste", true));

            Settings.Add(new BoolFeatureSetting("--displayClassDetailsTile", "Klassendetails", displayClassDetailsTile,
                "Anzeige der Klassendetails neben dem Stundenplan", true));

            Settings.Add(new BoolFeatureSetting("--displayUpcomingExamsTile", "Testkalender", displayUpcomingExamsTile,
                "Anzeige der kommenden Tests neben dem Stundenplan", true));
        }

        private void GetValues()
        {
            // get the values of each setting object and write it to the database

            foreach (FeatureSetting setting in Settings)
            {
                setting.DefaultValue = setting.ParseString(DatenbankAbrufen.GetSettingValue(setting.VarKey));
            }
        }

        private void DrawFeatureList()
        {      
           // create table row of each setting object and add it to the table

           foreach(FeatureSetting feature in Settings)
            {
                SettingsTable.Rows.Add(feature.SettingRow);
            }
                    
        }

        protected void BtSave_Click(object sender, EventArgs e)
        {
            // Try to validate values (if failed, default value is set) and write them to the database

            foreach(FeatureSetting setting in Settings)
            {
                DatenbankSchreiben.SetSettingValue(setting.VarKey, setting.GetSettingValue().ToString().ToLower(), "");
            }

            Response.Redirect("/admin/function-config/");
        }

        protected void BtCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("/admin/function-config/");
        }
    }
}