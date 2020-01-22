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

        // User with Permission to edit preset themes
        // -> Not all superadmins to have always a working theme as backup
        const string themeMaster = "20150562";

        // Dictoinarys to assign Theme Builder Color Buttons to variable names
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

            // Add varaibles to dictionarys
            Variables.Add("Hintergrundfarbe", "--background_col");
            Variables.Add("Tabellen-Hintergrund", "--tr_background_col_nth");
            Variables.Add("Kopfzeilen-Hintergrund", "--header_back_col");
            Variables.Add("Kopfzeilen-Schatten", "--header_shadow_col");
            Variables.Add("Kopfzeilen-Schriftfarbe", "--header_font_col");
            Variables.Add("Schriftfarbe", "--font_col");
            Variables.Add("Tabellenkopf-Schriftfarbe", "--th_font_color");
            Variables.Add("Seitennummer-Schriftfarbe", "--pagenum_font_col"); 
            Variables.Add("Stundenplan-Kachel-Rahmenfarbe", "--timetable_border_col");
            Variables.Add("Stundenplan-Kachel Überschriftfarbe", "--timetable_tileHeaderText_col");
            Variables.Add("Stundenentfall Textfarbe", "--timetable_lessonCancelled_textCol");
            Variables.Add("Stunde eingeschoben Textfarbe", "--timetable_lessonMovedIn_textCol");
            Variables.Add("Stunde verschoben Textfarbe", "--timetable_lessonMovedOut_textCol");
            Variables.Add("Stunde Testinfo Textfarbe", "--timetable_examInfoCol");
            Variables.Add("Aktive Stunde Hintergrundfarbe", "--timetable_activeLessonMarkerCol");

            DrawThemeSelector();
            DrawThemeBuilder();
            DrawPicker();

            if (!IsPostBack)
            {      
                // Create dropdown list items
                DropdownPreset.Items.Add(new ListItem { Text = " - ", Value = "" });
                DatenbankAbrufen.GetSettingThemes().ForEach((name) =>
                {
                    ListItem li = new ListItem { Text = name, Value = name };
                    DropdownPreset.Items.Add(li);
                    
                });
                // Hide picker popup by default
                picker_container.Style.Value = "display:none";
            }
        }

        protected void PresetChanged(object sender, EventArgs e)
        {
            // Load selected theme if dropdown was changed

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

        private void LoadPreset(string theme)
        {
            // Load values of theme and set them as background of the assigned builder button

            foreach (KeyValuePair<string, string> variable in Variables)
            {
                string val = DatenbankAbrufen.GetSettingValue(variable.Value, theme);

                BuilderButtons[variable.Key].Style.Value = "width: 100%; background: " + val;
            }
        }

        private void InitEditTheme(string editTheme)
        {
            // Set buttons and dropdown according to which theme should be edited

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

            LoadPreset(editTheme);
        }

        private void DeleteTheme(string theme)
        {
            // Remove theme from database and set default theme if deleted theme was active 

            DatenbankSchreiben.DeleteThemeSettings(theme);

            if (theme == DatenbankAbrufen.GetSettingValue("activeTheme"))
            {
                SetTheme(true);
            }
            Response.Redirect("./");
        }

        private void DrawThemeSelector()
        {
            // Create the table with existing themes

            Themes.Attributes.CssStyle.Add("width", "40%");
            Themes.Attributes.CssStyle.Add("margin", "5%");
            Themes.Attributes.CssStyle.Add("margin-top", "2%");
            Themes.Attributes.CssStyle.Add("float", "right");

            TableRow th = new TableRow();
            TableCell thc = new TableCell { Text = "Vorhandene Themes" };
            th.CssClass = thc.CssClass = "head";
            th.Cells.Add(thc);

            Themes.Rows.Add(th);

            string activeTheme = DatenbankAbrufen.GetSettingValue("activeTheme");

            // For each theme in the database

            DatenbankAbrufen.GetSettingThemes().ForEach((theme) =>
            {
                TableRow tr = new TableRow();
                TableCell tc = new TableCell();
                RadioButton rb = new RadioButton { Text = theme};
                if (theme == activeTheme) rb.Checked = true;
                rb.GroupName = "themes";
                rb.ID = theme;

                // Edit button (Not of preset themes)

                Button edit = new Button { CssClass = "ActionButton" };
                edit.Text = "✎";
                edit.Style.Value = "float:right";
                edit.Click += (object o, EventArgs e) =>
                {
                    InitEditTheme(rb.Text);
                };

                // Remove button (not on preset themes)
                
                Button rem = new Button { CssClass = "DeleteButton" };
                rem.Text = "✕";
                rem.Style.Value = "float:right";
                rem.Click += (object o, EventArgs e) =>
                {
                    DeleteTheme(theme);
                };
                if (theme.IndexOf("Dark Preset") >= 0 || theme.IndexOf("Light Preset") >= 0)
                {
                    rem.Enabled = false;
                    if(classes.Login.User != themeMaster) edit.Enabled = false;
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
            // Get the selected theme of radio button group and set it as active theme in database

            string selected_theme = Context.Request["ctl00$Content$themes"];

            // If not theme is selected return
            if (selected_theme == "" && !setStandardTheme) return;
            if (setStandardTheme) selected_theme = "Theme Dark";

            DatenbankSchreiben.SetSettingValue("activeTheme", selected_theme);
            Response.Redirect("./");
        }

        private void DrawThemeBuilder()
        {
            // Create the table with builder buttons for each css theme variable in the dictionaries

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

                // Open color picker on button click
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
            // Check if the desired name is valid, get all background values of the builder buttons and save theme in database

            if (!Regex.IsMatch(name, @"^[a-zA-Z0-9_ ]+$")) return false;
            if (DropdownPreset.Enabled &&  DatenbankAbrufen.ColumnLike("Settings", "Theme", name).Count > 0) return false;

            foreach(KeyValuePair<string, string> variable in Variables)
            {
                string color = BuilderButtons[variable.Key].Style["Background"];
                DatenbankSchreiben.SetSettingValue(variable.Value, color == "" ? "none" : color, name);
            }
            
            return true;
        }

        private void OpenPicker(string key, string init_color)
        {
            // Open color picker popup for specific css theme variable

            picker_container.Style.Value = "display:block"; 
            setPickerHeader(key, init_color);
            Session["VarKey"] = key;
        }

        private Action<string, string> setPickerHeader;
        private void DrawPicker()
        {
            // Init color picker popup

            Panel picker = new Panel();
            Panel cover = new Panel();

            picker.CssClass = "picker";
            cover.CssClass = "cover";

            // Close button
            Button btClose = new Button();
            btClose.CssClass = "DeleteButton";
            btClose.Text = "✕";
            btClose.Click += (x, y) => { picker_container.Style.Value = "display:none"; };

            // Header, add close button to header
            Label head = new Label();
            head.Font.Size = FontUnit.XLarge;
            var divHead = new HtmlGenericControl("div");
            divHead.Controls.Add(btClose);
            divHead.Style.Value = "display: inline; float:right; margin-bottom:5px;";

            // Container where the color picker will be created with jQuery
            var divPicker = new HtmlGenericControl("div");
            divPicker.InnerHtml = "<div id='col_picker'></div>";
            divPicker.Style.Value = "float:left; margin: 5%; width:40%";

            // Preview input where th color is previewn, synced with the colorpicker, also accepts any CSS input like "red, none" etc
            var color_prev = new HtmlGenericControl("input");
            color_prev.Attributes.Add("type", "text");
            color_prev.Attributes.Add("name", "colorcode");
            color_prev.Attributes.Add("value", "#66c144");
            color_prev.ID = "prev";
            color_prev.Style.Value = "width:100%; border:none";

            // Set action to change header dynamically on every openig
            setPickerHeader = (key, col) => 
            { 
                head.Text = key;
                if(col != "none") color_prev.Attributes["value"] = col; 
             };


            // Confirm button
            Button confirm = new Button { CssClass = "SaveButton" };
            confirm.Text = "Farbe festlegen";
            confirm.Style.Value = "width:100%; margin-top: 5%";
            confirm.Click += (object o, EventArgs e) =>
            {
                BuilderButtons[Session["VarKey"].ToString()].Style.Value = "width:100%; background: " + Context.Request["colorcode"];
                picker_container.Style.Value = "display:none";
            };


            // Button to set color none
            Button noBack = new Button { CssClass = "DeleteButton" };
            noBack.Text = "Keine Farbe ('none')";
            noBack.Style.Value = "width:100%; margin-top: 5%";
            noBack.Click += (object o, EventArgs e) =>
            {
                BuilderButtons[Session["VarKey"].ToString()].Style.Value = "width:100%; background: none";
                picker_container.Style.Value = "display:none";
            };

            // div which contains the buttons
            var divInput = new HtmlGenericControl("div");
            divInput.Style.Value = "float:right; margin: 5%; width:40%";
            divInput.Controls.Add(color_prev);
            divInput.Controls.Add(confirm);
            divInput.Controls.Add(noBack);

            // div which contains picker and input
            var divBody = new HtmlGenericControl("div");
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