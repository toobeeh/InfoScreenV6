using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Infoscreen_Verwaltung.classes;
using System.IO;
using System.Text.RegularExpressions;

namespace Infoscreen_Verwaltung.admin.theme
{
    public partial class Theme : System.Web.UI.Page
    {
        string ebene = "../../";

        List<RadioButton> ThemeButtons = new List<RadioButton>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!classes.Login.Angemeldet) Response.Redirect("../../login.aspx");
            if (!classes.Login.Rechte.Bildschirme.OneBiggerThan(1)) Response.Redirect("../../");

            TopMenu.Text = classes.Anzeige.TopBar(ebene);
            Menu.Text = classes.Anzeige.Menue(ebene, "admin");
            

            DrawThemeSelector();
        }


        private List<string> LoadThemeFiles()
        {
            return Directory.GetFiles(@"D:\infoscreen_publish\ScreenCore\wwwroot\CSS\", "theme_*.css").ToList();
        }

        private void DrawThemeSelector()
        {
            Themes.Attributes.CssStyle.Add("width", "40%");
            Themes.Attributes.CssStyle.Add("margin", "5%");
            Themes.Attributes.CssStyle.Add("margin-top", "2%");

            TableRow th = new TableRow();
            TableCell thc = new TableCell { Text = "Vorhandene Themes" };
            th.CssClass = thc.CssClass = "head";
            th.Cells.Add(thc);

            Themes.Rows.Add(th);

            LoadThemeFiles().ForEach((loc) =>
            {
                TableRow tr = new TableRow();
                TableCell tc = new TableCell();
                RadioButton rb = new RadioButton { Text = Path.GetFileNameWithoutExtension(loc) };
                if (rb.Text == GetActiveTheme()) rb.Checked = true;
                rb.GroupName = "themes";
                rb.ID = loc;


                ThemeButtons.Add(rb);

                tc.Controls.Add(rb);
                tr.Cells.Add(tc);
                Themes.Rows.Add(tr);
            }
            );

            TableRow trn = new TableRow();
            TableCell tcn = new TableCell();
            Button NewTheme = new Button { CssClass = "ActionButton", Text = "Theme erstellen" };
            Button Save = new Button { CssClass = "SaveButton", Text = "Übernehmen" };

            Save.Click += (object o, EventArgs e) => { SetTheme(); };

            tcn.Controls.Add(NewTheme);
            tcn.Controls.Add(Save);

            tcn.ID = "AddTheme";
            trn.Cells.Add(tcn);

            Themes.Rows.Add(trn);
        }


        private void SetTheme()
        {
            string selected_theme = Path.GetFileNameWithoutExtension(Context.Request["ctl00$Content$themes"]);

            if (selected_theme == "") return;

            string stylesheet = File.ReadAllText(@"D:\infoscreen_publish\ScreenCore\wwwroot\CSS\style.css");

            Match match = Regex.Match(stylesheet, @"@import url\(.*\);");
            if (!match.Success) return;

            stylesheet = stylesheet.Replace(match.Captures[0].Value, @"@import url(/CSS/" + selected_theme + ".css);");

            File.WriteAllText(@"D:\infoscreen_publish\ScreenCore\wwwroot\CSS\style.css", stylesheet);

            // Set checked for postback
            ThemeButtons.ForEach((bt) =>
            {
                if (bt.ID == Context.Request["ctl00$Content$themes"]) bt.Checked = true;

            });
        }

        private string GetActiveTheme()
        {
            string stylesheet = File.ReadAllText(@"D:\infoscreen_publish\ScreenCore\wwwroot\CSS\style.css");

            Match match = Regex.Match(stylesheet, @"@import url\(.*\);");
            if (!match.Success) return "";

            string import_line = match.Captures[0].Value;
            string themename = import_line.Substring(import_line.LastIndexOf("/CSS/") + 5, import_line.IndexOf(".css") - (import_line.LastIndexOf("/CSS/") + 5));

            return themename;
        }
    }
}