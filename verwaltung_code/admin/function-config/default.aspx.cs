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

        Dictionary<string, string> JsConstants;
        Dictionary<string, string> JsConstantsValues;
        Dictionary<string, string> JsConstantsDescription;
        Dictionary<string, HtmlGenericControl> JsConstantsInputs;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!classes.Login.Angemeldet) Response.Redirect("../../login.aspx");
            if (!classes.Login.Rechte.Bildschirme.OneBiggerThan(1)) Response.Redirect("../../");

            TopMenu.Text = classes.Anzeige.TopBar(ebene);
            Menu.Text = classes.Anzeige.Menue(ebene, "admin");

            JsConstantsInputs = new Dictionary<string, HtmlGenericControl>();

            JsConstants = new Dictionary<string, string>();
            JsConstants.Add("switchTimeMs", "Seiten-Anzeigedauer");
            JsConstants.Add("animateUpcomingExamLessons", "Tests animieren");
            JsConstants.Add("showExamTimeMs", "Test-Anzeigedauer");
            JsConstants.Add("examTimesShown", "Test-Anzeigeabstand");
            JsConstants.Add("displayClockTile", "Uhr");
            JsConstants.Add("displayClassDetailsTile", "Klassendetails");
            JsConstants.Add("displayUpcomingExamsTile", "Testkalender");

            JsConstantsDescription = new Dictionary<string, string>();
            JsConstantsDescription.Add("switchTimeMs", "Anzeigedauer der einzelnen Seiten wie Stundenplan, Powerpoint oder Raumübersicht in Sekunden");
            JsConstantsDescription.Add("animateUpcomingExamLessons", "Animierte Anzeige der kommenden Tests im Stundenplan");
            JsConstantsDescription.Add("showExamTimeMs", "Anzeigedauer der Test-Markierung im Stundenplan in Sekunden");
            JsConstantsDescription.Add("examTimesShown", "Abstand in Sekunden in dem die Tests im Stundenplan eingeblendet werden");
            JsConstantsDescription.Add("displayClockTile", "Anzeige einer digitalen Uhr in der Titelleiste");
            JsConstantsDescription.Add("displayClassDetailsTile", "Anzeige der Klassendetails neben dem Stundenplan");
            JsConstantsDescription.Add("displayUpcomingExamsTile", "Anzeige der kommenden Tests neben dem Stundenplan");


            GetValues();

            DrawFeatureList();
             
          
        }

        private void GetValues()
        {
            string values = File.ReadAllText(@"D:\infoscreen_publish\ScreenCore\wwwroot\JS\constants.js");
            JsConstantsValues = new Dictionary<string, string>();

            foreach(KeyValuePair<string, string> item in JsConstants)
            {
                int startindex = values.IndexOf(item.Key);
                string cut = values.Substring(startindex);
                startindex = cut.IndexOf("=") + 1;
                cut = cut.Substring(startindex);
                cut = cut.Substring(0, cut.IndexOf(";"));

                JsConstantsValues[item.Key] = cut;
            }

        }

        TableRow NewSettingsTableRow(string name, string  inputHtml, string description, string key)
        {
            TableRow tr = new TableRow();

            TableCell tName = new TableCell { Text= name, HorizontalAlign=HorizontalAlign.Left };
            TableCell tInput = new TableCell {HorizontalAlign = HorizontalAlign.Left };
            TableCell tDesc = new TableCell { Text = description, HorizontalAlign = HorizontalAlign.Left };

            HtmlGenericControl valCont = new HtmlGenericControl { ID = key + "_container" };
            valCont.Attributes["class"] = "input_val";
            valCont.Attributes.Add("data-init", JsConstantsValues[key]);
            JsConstantsInputs.Add(key, valCont);
            tInput.Controls.Add(new HtmlGenericControl { InnerHtml = inputHtml });
            tInput.Controls.Add(valCont);
            

            tr.Controls.Add(tName);
            tr.Controls.Add(tInput);
            
            tr.Controls.Add(tDesc);

            return tr;
        }

        private void DrawFeatureList()
        {       
            foreach(KeyValuePair<string,string> item in JsConstants)
            {
                if(item.Key == "switchTimeMs" || item.Key == "showExamTimeMs" || item.Key == "examTimesShown")
                {
                    SettingsTable.Rows.Add(NewSettingsTableRow(JsConstants[item.Key], 
                        "<div class='jQuerySlider' id='" + item.Key + "'></div>", JsConstantsDescription[item.Key], item.Key));
                }
                else if (item.Key == "animateUpcomingExamLessons" || item.Key == "displayClockTile" || item.Key == "displayClassDetailsTile" || item.Key == "displayUpcomingExamsTile")
                {
                    SettingsTable.Rows.Add(NewSettingsTableRow(JsConstants[item.Key],
                        "<input type='checkbox' class='feature_checkbox' id='" + item.Key + "'></div>", JsConstantsDescription[item.Key], item.Key));
                }

            }
                    
        }

        protected void BtSave_Click(object sender, EventArgs e)
        {
            string values = "";
            foreach(KeyValuePair<string,string> item in JsConstants)
            {
                if (item.Key == "switchTimeMs" || item.Key == "showExamTimeMs" || item.Key == "examTimesShown")
                {
                    int val;
                    try { val = double.Parse(Context.Request[item.Key], System.Globalization.CultureInfo.InvariantCulture).ToInt32(); }
                    catch { val = 0; }
                    values += item.Key + " = " + val + ";\n";
                }
                else if (item.Key == "animateUpcomingExamLessons" || item.Key == "displayClockTile" || item.Key == "displayClassDetailsTile" || item.Key == "displayUpcomingExamsTile")
                {
                    values += item.Key + " = " + (Context.Request[item.Key] == "on" ? "true" : "false") + ";\n";
                }
                
            }

            File.WriteAllText(@"D:\infoscreen_publish\ScreenCore\wwwroot\JS\constants.js", values);
        }
    }
}