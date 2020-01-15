using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Infoscreen_Verwaltung.classes
{
    public class FeatureSetting
    {
        public Type Datatype { get; private set; }        
        public string VarKey { get; private set; }
        public string VarName { get; private set; }
        public string VarDescription { get; private set; }
        public HiddenField DataField { get; private set; }

        public object DefaultValue
        {
            get { return defaultValue; }
            set { defaultValue = value; }
        }


        private Func<object,bool> validateValue;
        private object defaultValue;
        
        public FeatureSetting(string key, string name, Type dataType, HiddenField clientField, string description, Func<object,bool> validate = null, object defaultVal = null)
        {
            VarKey = key;
            VarName = name;
            VarDescription = description;
            Datatype = dataType;
            DataField = clientField;
            validateValue = validate;
            defaultValue = defaultVal;
        }

        public TableRow GenerateSettingRow()
        {
            TableRow settingRow = new TableRow();

            TableCell tName = new TableCell { Text = VarName, HorizontalAlign = HorizontalAlign.Left };
            tName.Style.Add("padding-left","1em");

            TableCell tInput = new TableCell { HorizontalAlign = HorizontalAlign.Left };
            TableCell tDesc = new TableCell { Text = VarDescription, HorizontalAlign = HorizontalAlign.Left };

            // Label where the client script displays the value
            Label valCont = new Label { ID = VarKey + "_container" };
            valCont.Attributes["class"] = "input_val";
            if(Datatype == typeof(int)) valCont.Style.Add("margin-top", "0.5em");
            valCont.Attributes.Add("data-init", defaultValue.ToString()); // default value for the client script

            // Generate input element HTML code
            string html = "";

            if(Datatype == typeof(int)) // Slider for int
            {
                html += "<div class='jQuerySlider' id='" + VarKey + "'></div>";
            }
            else if(Datatype == typeof(bool)) // Checkbox for bool
            {
                html += "<input type='checkbox' class='feature_checkbox' id='" + VarKey + "'>";
            }

            tInput.Controls.Add(new Label { Text = html });
            tInput.Controls.Add(valCont);
            tInput.Style.Add("padding", "1em");

            settingRow.Cells.Add(tName);
            settingRow.Cells.Add(tInput);
            settingRow.Cells.Add(tDesc);

            return settingRow;
        }


        public object GetSettingValue()
        {
            object value = null;
            if (Datatype == typeof(bool)) value = DataField.Value.Contains("true");
            else if (Datatype == typeof(int)) value = Convert.ToInt32(double.Parse(DataField.Value, System.Globalization.CultureInfo.InvariantCulture));

            return value;
        }

        public bool ValidateSettingValue()
        {
            return validateValue == null ? true : validateValue(GetSettingValue());
        }


    }
}