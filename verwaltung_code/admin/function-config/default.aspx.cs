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
            Settings.Add(new FeatureSetting("switchTimeMs", "Seiten-Anzeigedauer", typeof(int), switchTimeMs,
               "Anzeigedauer in Sek. der einzelnen Seiten wie Stundenplan, Powerpoint oder Raumübersicht", null, 10000));

            Settings.Add(new FeatureSetting("animateUpcomingExamLessons", "Tests anzeigen", typeof(bool), animateUpcomingExamLessons,
                "Animierte Anzeige der kommenden Tests im Stundenplan", null, true));

            Settings.Add(new FeatureSetting("showExamTimeMs", "Tests-Anzeigedauer", typeof(int), showExamTimeMs,
                "Anzeigedauer in Sek. der Test-Markierung im Stundenplan", null, 2500));

            Settings.Add(new FeatureSetting("examTimesShown", "Test-Anzeigeintervall", typeof(int), examTimesShown,
               "Das Intervall in Sek., nach dem Tests für die definierte Zeit eingeblendet werden", null, 2));

            Settings.Add(new FeatureSetting("examAnimationDuration", "Test-Animationsdauer", typeof(int), examAnimationDuration,
               "Die Dauer in Sek. der Ein/Ausblendeaniation der Tests (0 = Nicht animiert", null, 500));

            Settings.Add(new FeatureSetting("displayClockTile", "Uhr", typeof(bool), displayClockTile,
                "Anzeige einer digitalen Uhr in der Titelleiste", null, true));

            Settings.Add(new FeatureSetting("displayClassDetailsTile", "Klassendetails", typeof(bool), displayClassDetailsTile,
                "Anzeige der Klassendetails neben dem Stundenplan", null, true));

            Settings.Add(new FeatureSetting("displayUpcomingExamsTile", "Testkalender", typeof(bool), displayUpcomingExamsTile,
                "Anzeige der kommenden Tests neben dem Stundenplan", null, true));
        }

        private void GetValues()
        {
            string values = File.ReadAllText(@"D:\infoscreen_publish\ScreenCore\wwwroot\JS\constants.js");

            foreach (FeatureSetting setting in Settings)
            {
                int startindex = values.IndexOf(setting.VarKey);
                string cut = values.Substring(startindex);
                startindex = cut.IndexOf("=") + 1;
                cut = cut.Substring(startindex);
                cut = cut.Substring(0, cut.IndexOf(";"));

                if (setting.Datatype == typeof(bool)) setting.DefaultValue = cut.Contains("true");
                else if (setting.Datatype == typeof(int)) setting.DefaultValue = Convert.ToInt32(cut);
            }
        }



        private void DrawFeatureList()
        {       
           foreach(FeatureSetting feature in Settings)
            {
                SettingsTable.Rows.Add(feature.GenerateSettingRow());
            }
                    
        }

        protected void BtSave_Click(object sender, EventArgs e)
        {
            string values = "";

            foreach(FeatureSetting setting in Settings)
            {
                values += setting.VarKey + "=" + (setting.ValidateSettingValue() ? setting.GetSettingValue().ToString().ToLower() : setting.DefaultValue.ToString().ToLower() ) + ";\n";
            }

            File.WriteAllText(@"D:\infoscreen_publish\ScreenCore\wwwroot\JS\constants.js", values);

            Response.Redirect("/admin/function-config/");
        }

        protected void BtCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("/admin/function-config/");
        }
    }
}