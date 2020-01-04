using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Infoscreen_Verwaltung.classes;
using System.IO;

namespace Infoscreen_Verwaltung.admin.theme
{
    public partial class Theme : System.Web.UI.Page
    {
        string ebene = "../../";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!classes.Login.Angemeldet) Response.Redirect("../../login.aspx");
            if (!classes.Login.Rechte.Bildschirme.OneBiggerThan(1)) Response.Redirect("../../");

            TopMenu.Text = classes.Anzeige.TopBar(ebene);
            Menu.Text = classes.Anzeige.Menue(ebene, "admin");

            Themes.Attributes.CssStyle.Add("width","40%");
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
                rb.GroupName = "themes";
                tc.Controls.Add(rb);
                tr.Cells.Add(tc);
                Themes.Rows.Add(tr);
            }
            );

            TableRow trn = new TableRow();
            TableCell tcn = new TableCell();
            Button bt = new Button { CssClass = "ActionButton", Text = "Theme erstellen" };
            tcn.Controls.Add(bt);
            tcn.ID = "AddTheme";
            trn.Cells.Add(tcn);
            Themes.Rows.Add(trn);
        }


        private List<string> LoadThemeFiles()
        {
            return Directory.GetFiles(@"D:\infoscreen_publish\ScreenCore\wwwroot\CSS\", "theme_*.css").ToList();
        }
    }
}