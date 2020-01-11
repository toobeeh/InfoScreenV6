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

        private Button btSave;
        private Button btDiscard;
        private TableCell name;
        private TextBox tbName;

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
            Variables.Add("Kopfzeilen-Schriftfarbe", "--header_font_col");
            Variables.Add("Schriftfarbe", "--font_col");
            Variables.Add("Tabellenkopf-Schriftfarbe", "--th_font_color");
            Variables.Add("Seitennummer-Schriftfarbe", "--pagenum_font_col"); 
            Variables.Add("Stundenplan-Widget-Rahmenfarbe", "--timetable_border_col");
            Variables.Add("Stundenplan-Widget Überschriftfarbe", "--timetable_widgetHeaderText_col");
            Variables.Add("Stundenentfall Textfarbe", "--timetable_lessonCancelled_textCol");
            Variables.Add("Stunde eingeschoben Textfarbe", "--timetable_lessonMovedIn_textCol");
            Variables.Add("Stunde verschoben Textfarbe", "--timetable_lessonMovedOut_textCol");

            DrawThemeSelector();
            DrawThemeBuilder();
            DrawPicker();

            if (!IsPostBack)
            {      
                DropdownPreset.Items.Add(new ListItem { Text = " - ", Value = "" });
                LoadThemeFiles().ForEach((name) =>
                {
                    ListItem li = new ListItem { Text = Path.GetFileNameWithoutExtension(name).Substring(6), Value = name };
                    DropdownPreset.Items.Add(li);
                    
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

            LoadPreset(li.Value);
        }

        private void LoadPreset(string path)
        {
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

        private void InitEditTheme(string editTheme)
        {
            TitleCell.Text = "Bearbeiten";
            DropdownPreset.SelectedItem.Selected = false;
            for (int i = 0; i < DropdownPreset.Items.Count; i++)
            {
                if (DropdownPreset.Items[i].Text == editTheme) DropdownPreset.Items[i].Selected = true;
            }

            btDiscard.Style["display"] = "block";
            tbName.Style["display"] = "none";
            tbName.Text = editTheme;
            btSave.Text = "Übernehmen";
            name.Text = "";
            DropdownPreset.Enabled = false;

            LoadPreset(@"D:\infoscreen_publish\ScreenCore\wwwroot\CSS\theme_" + editTheme + ".css");
        }

        private void DeleteTheme(string location)
        {
            if (location.IndexOf(GetActiveTheme() + ".css") >= 0)
            {
                File.Delete(location);
                SetTheme(true);
            }
            File.Delete(location);
            Response.Redirect("./");
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
                RadioButton rb = new RadioButton { Text = Path.GetFileNameWithoutExtension(loc).Substring(6) };
                if (Path.GetFileNameWithoutExtension(loc) == GetActiveTheme()) rb.Checked = true;
                rb.GroupName = "themes";
                rb.ID = loc;

                Button edit = new Button { CssClass = "ActionButton" };
                edit.Text = "✎";
                edit.Style.Value = "float:right";
                edit.Click += (object o, EventArgs e) =>
                {
                    InitEditTheme(rb.Text);
                };

                Button rem = new Button { CssClass = "DeleteButton" };
                rem.Text = "✕";
                rem.Style.Value = "float:right";
                rem.Click += (object o, EventArgs e) =>
                {
                    DeleteTheme(loc);
                };
                if (loc.IndexOf("theme_light.css") > 0 || loc.IndexOf("theme_dark.css") > 0)
                {
                    rem.Enabled = false;
                    rem.CssClass = "ActionButton";
                }

                ThemeButtons.Add(rb);
                tc.Controls.Add(rem);
                tc.Controls.Add(edit);
                tc.Controls.Add(rb);
                
                tr.Cells.Add(tc);
                Themes.Rows.Add(tr);
            }
            );

            TableRow trn = new TableRow();
            TableCell tcn = new TableCell();
            Button Save = new Button { CssClass = "SaveButton", Text = "Theme aktivieren" };

            Save.Click += (object o, EventArgs e) => { SetTheme(); };

            tcn.Controls.Add(Save);

            tcn.ID = "AddTheme";
            trn.Cells.Add(tcn);

            Themes.Rows.Add(trn);
        }

        private void SetTheme(bool setStandardTheme = false)
        {
            string selected_theme = Path.GetFileNameWithoutExtension(Context.Request["ctl00$Content$themes"]);

            if (selected_theme == "" && !setStandardTheme) return;

            string stylesheet = File.ReadAllText(@"D:\infoscreen_publish\ScreenCore\wwwroot\CSS\style.css");

            Match match = Regex.Match(stylesheet, @"@import url\(.*\);");
            if (!match.Success) return;

            if(setStandardTheme) stylesheet = stylesheet.Replace(match.Captures[0].Value, @"@import url(/CSS/theme_dark.css);");
            else stylesheet = stylesheet.Replace(match.Captures[0].Value, @"@import url(/CSS/" + selected_theme + ".css);");

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
                bt.Click += (object o, EventArgs e) => { OpenPicker(variable.Key, bt.Style["background"]); };
                
                color.Controls.Add(bt);

                tr.Cells.Add(title);
                tr.Cells.Add(color);
                ThemeBuilder.Rows.Add(tr);
            }

            TableRow tfoot = new TableRow();
            name = new TableCell { Text = "Name des Themes:" };
            TableCell textbox = new TableCell();
            TableCell button = new TableCell();

            tbName = new TextBox();

            btDiscard = new Button { CssClass = "DeleteButton" };
            btDiscard.Text = "Abbrechen";
            btDiscard.Style["display"] = "none";
            btDiscard.Click += (object o, EventArgs e) =>
            {
                Response.Redirect("/admin/theme");
            };

            textbox.Controls.Add(tbName);
            textbox.Controls.Add(btDiscard);

            btSave = new Button { CssClass = "SaveButton" };
            btSave.Text = "Theme Speichern";
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
            if (!Regex.IsMatch(name, @"^[a-zA-Z0-9_]+$") || 
                File.Exists(@"D:\infoscreen_publish\ScreenCore\wwwroot\CSS\theme_" + name + ".css") && tbName.Style["display"]!="none")
                return false;

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

        private void OpenPicker(string key, string init_color)
        {
            picker_container.Style.Value = "display:block"; 
            setPickerHeader(key, init_color);
            Session["VarKey"] = key;
        }

        private Action<string, string> setPickerHeader;
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

            btClose.Text = "✕";
            btClose.Click += (x, y) => { picker_container.Style.Value = "display:none"; };

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

            setPickerHeader = (key, col) => 
            { 
                head.Text = key;
                if(col != "none") color_prev.Attributes["value"] = col; 
             };

            Button confirm = new Button { CssClass = "SaveButton" };
            confirm.Text = "Farbe festlegen";
            confirm.Style.Value = "width:100%; margin-top: 5%";
            confirm.Click += (object o, EventArgs e) =>
            {
                BuilderButtons[Session["VarKey"].ToString()].Style.Value = "width:100%; background: " + Context.Request["colorcode"];
                picker_container.Style.Value = "display:none";
            };

            Button noBack = new Button { CssClass = "DeleteButton" };
            noBack.Text = "Keine Farbe ('none')";
            noBack.Style.Value = "width:100%; margin-top: 5%";
            noBack.Click += (object o, EventArgs e) =>
            {
                BuilderButtons[Session["VarKey"].ToString()].Style.Value = "width:100%; background: none";
                picker_container.Style.Value = "display:none";
            };

            divInput.Controls.Add(color_prev);
            divInput.Controls.Add(confirm);
            divInput.Controls.Add(noBack);

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