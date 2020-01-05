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

namespace Infoscreen_Verwaltung.admin.theme
{
    public partial class Theme : System.Web.UI.Page
    {
        string ebene = "../../";

        private IDictionary<string, string> Variables = new Dictionary<string, string>();
        private IDictionary<string, Button> BuilderButtons = new Dictionary<string, Button>();

        List<RadioButton> ThemeButtons = new List<RadioButton>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!classes.Login.Angemeldet) Response.Redirect("../../login.aspx");
            if (!classes.Login.Rechte.Bildschirme.OneBiggerThan(1)) Response.Redirect("../../");

            TopMenu.Text = classes.Anzeige.TopBar(ebene);
            Menu.Text = classes.Anzeige.Menue(ebene, "admin");

            Variables.Add("Hintergrundfarbe", "--background_col");
            Variables.Add("Tabellen-Hintergrund", "--tr_background_col_nth");
            Variables.Add("Kopfzeilen-Hintergrund", "--header_back_col");
            Variables.Add("Kopfzeilen-Schatten", "--header_shadow_col");
            Variables.Add("Schriftfarbe", "--font_col");
            Variables.Add("Tabellenkopf-Schriftfarbe", "--th_font_color");
            Variables.Add("Seitennummer-Schriftfarbe", "--pagenum_font_col");

            DrawThemeSelector();
            DrawThemeBuilder();
            DrawPicker();

            if (!IsPostBack)
            {      
                DropdownPreset.Items.Add(new ListItem { Text = " - ", Value = "" });
                LoadThemeFiles().ForEach((name) =>
                {
                    DropdownPreset.Items.Add(new ListItem { Text = Path.GetFileNameWithoutExtension(name), Value = name });
                });
                picker_container.Style.Value = "display:none";
            }
        }

        protected void PresetChanged(object sender, EventArgs e)
        {
            ListItem li = DropdownPreset.SelectedItem;
            if (li.Value == "")
            {
                foreach (KeyValuePair<string, string> variable in Variables)
                {
                    BuilderButtons[variable.Key].Style.Value = "width: 100%; background: none";
                }
                return;
            }

            string path = li.Value;
            string css = File.ReadAllText(path);

            foreach (KeyValuePair<string, string> variable in Variables)
            {
                int startpos = css.IndexOf(variable.Value) + variable.Value.Length + 1;
                int endpos = startpos;
                while (css[endpos] != ';') endpos++;

                string val = css.Substring(startpos, endpos - startpos);

                BuilderButtons[variable.Key].Style.Value = "width: 100%; background: " + val;
            }

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
            Themes.Attributes.CssStyle.Add("float", "right");

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
 
                

                Button rem = new Button { CssClass = "DeleteButton" };
                rem.Text = "✕";
                rem.Style.Value = "float:right";
                rem.Click += (object o, EventArgs e) =>
                {
                    File.Delete(loc);
                    Response.Redirect("./");
                };
                if (loc.IndexOf("theme_light") > 0 || loc.IndexOf("theme_dark") > 0)
                {
                    rem.Enabled = false;
                    rem.CssClass = "ActionButton";
                }

                ThemeButtons.Add(rb);
                tc.Controls.Add(rem);
                tc.Controls.Add(rb);
                
                tr.Cells.Add(tc);
                Themes.Rows.Add(tr);
            }
            );

            TableRow trn = new TableRow();
            TableCell tcn = new TableCell();
            Button Save = new Button { CssClass = "SaveButton", Text = "Übernehmen" };

            Save.Click += (object o, EventArgs e) => { SetTheme(); };

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

            Response.Redirect("./");
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

        private void DrawThemeBuilder()
        {
            ThemeBuilder.Attributes.CssStyle.Add("width", "40%");
            ThemeBuilder.Attributes.CssStyle.Add("margin", "5%");
            ThemeBuilder.Attributes.CssStyle.Add("margin-top", "2%");
            ThemeBuilder.Attributes.CssStyle.Add("float", "left");

            foreach(KeyValuePair<string,string> variable in Variables)
            {
                TableRow tr = new TableRow();
                TableCell title = new TableCell { Text = variable.Key };
                TableCell color = new TableCell();

                title.ColumnSpan = 2;
                Button bt = new Button { CssClass = "ActionButton" };

                BuilderButtons.Add(variable.Key, bt);

                bt.Style.Value = "background:none; width:100%";
                bt.Click += (object o, EventArgs e) => { OpenPicker(variable.Key); };
                
                color.Controls.Add(bt);

                tr.Cells.Add(title);
                tr.Cells.Add(color);
                ThemeBuilder.Rows.Add(tr);
            }

            TableRow tfoot = new TableRow();
            TableCell name = new TableCell { Text = "Name des Themes:" };
            TableCell textbox = new TableCell();
            TableCell button = new TableCell();

            TextBox tbName = new TextBox();
            textbox.Controls.Add(tbName);

            Button btSave = new Button { CssClass = "SaveButton" };
            btSave.Text = "Theme speichern";
            btSave.Click += (object o, EventArgs e) =>
            {
                if (SaveTheme(tbName.Text)) Response.Redirect("/admin/theme");
                else btSave.CssClass = "DeleteButton";
            };
            button.Controls.Add(btSave);

            tfoot.Cells.Add(name);
            tfoot.Cells.Add(textbox);
            tfoot.Cells.Add(button);

            ThemeBuilder.Rows.Add(tfoot);

        }

        private bool SaveTheme(string name)
        {
            if (!Regex.IsMatch(name, @"^[a-zA-Z0-9_]+$")) return false;

            string css = ":root{\n";
            foreach (KeyValuePair<string, string> variable in Variables)
            {
                css += variable.Value + ":";
                css += BuilderButtons[variable.Key].Style["background"];
                css += ";\n";
            }
            css += "}";

            File.WriteAllText(@"D:\infoscreen_publish\ScreenCore\wwwroot\CSS\theme_" + name + ".css", css);
            
            return true;
        }

        private void OpenPicker(string key)
        {
            picker_container.Style.Value = "display:block"; 
            setPickerHeader(key);
            Session["VarKey"] = key;
        }

        private Action<string> setPickerHeader;
        private void DrawPicker()
        {

            Panel picker = new Panel();
            Panel cover = new Panel();

            picker.CssClass = "picker";
            cover.CssClass = "cover";

            Label head = new Label ();
            head.Font.Size = FontUnit.XLarge;
            var divHead = new HtmlGenericControl("div");
            Button btClose = new Button();
            btClose.CssClass = "DeleteButton";

            setPickerHeader = (s) => head.Text = s;

            btClose.Text = "✕";
            btClose.Click += (x, y) => { Response.Redirect("/admin/theme"); };

            divHead.Controls.Add(btClose);
            divHead.Style.Value = "display: inline; float:right; margin-bottom:5px;";

            var divBody = new HtmlGenericControl("div");

            var divPicker = new HtmlGenericControl("div");
            divPicker.InnerHtml = "<div id='col_picker'></div>";
            divPicker.Style.Value = "float:left; margin: 5%; width:40%";

            var divInput = new HtmlGenericControl("div");
            divInput.Style.Value = "float:right; margin: 5%; width:40%";


            var color_prev = new HtmlGenericControl("input");
            color_prev.Attributes.Add("type", "text");
            color_prev.Attributes.Add("name", "colorcode");
            color_prev.Attributes.Add("value", "#66c144");
            color_prev.ID = "prev";
            color_prev.Style.Value = "width:100%; border:none";

            Button confirm = new Button { CssClass = "SaveButton" };
            confirm.Text = "Farbe festlegen";
            confirm.Style.Value = "width:100%; margin-top: 5%";
            confirm.Click += (object o, EventArgs e) =>
            {
                //Session[Session["VarKey"].ToString()] = Context.Request["colorcode"];
                //Response.Redirect("/admin/theme");
                BuilderButtons[Session["VarKey"].ToString()].Style.Value = "width:100%; background: " + Context.Request["colorcode"];
                picker_container.Style.Value = "display:none";
            };

            divInput.Controls.Add(color_prev);
            divInput.Controls.Add(confirm);

            divBody.Controls.Add(divPicker);
            divBody.Controls.Add(divInput);

            picker.Controls.Add(head);
            picker.Controls.Add(divHead);
            picker.Controls.Add(divBody);

            picker_container.Controls.Add(cover);
            picker_container.Controls.Add(picker);
        }

    }
}