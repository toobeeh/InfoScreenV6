using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Infoscreen_Verwaltung.classes
{
    // Base class with no specific components for a datatype -> abstract
    public abstract class FeatureSetting
    {
        /// <summary>
        /// Objects which outside of the class can be accessed but not be set 
        /// </summary>
        public string VarKey { get; internal set; }
        public string VarName { get; internal set; }
        public string VarDescription { get; internal set; }
        public HiddenField DataField { get; internal set; }
        public TableRow SettingRow { get; internal set; }

        /// <summary>
        /// The value which will be the default of the user input. Thus, every time the default is changed, the row is newly generated
        /// </summary>
        public virtual object DefaultValue 
        { 
            get { return defaultValue; } 
            set { defaultValue = value; /*GenerateSettingRow(); -> Setting Row has to be generated in derived class*/ } 
        }
        
        internal object defaultValue;
        internal TableCell NameCell, InputCell, DescriptionCell;
        internal Label ValueLabel, InputLabel;

        public virtual object ParseString(string value) { return value; }

        // Base constructor
        public FeatureSetting(string key, string name, HiddenField clientField, string description)
        {
            // init values
            VarKey = key;
            VarName = name;
            VarDescription = description;
            DataField = clientField;
            //DefaultValue = defaultVal; -> Derived class have to set default val so the generation of the row can be controlled
        }

        // Function to initialize the row
        internal virtual void GenerateSettingRow()
        {
            // Label where the client script displays the value
            ValueLabel = new Label { ID = VarKey + "_container" };
            ValueLabel.Attributes["class"] = "input_val ";
            ValueLabel.Attributes.Add("data-init", DefaultValue.ToString()); // default value for the client script

            // Label which will contain the control
            InputLabel = new Label();

            // Cell which contains input and value label
            InputCell = new TableCell { HorizontalAlign = HorizontalAlign.Left };
            InputCell.Controls.Add(InputLabel);
            InputCell.Controls.Add(ValueLabel);
            InputCell.Style.Add("padding", "1em");

            // Cell with variable name
            NameCell = new TableCell { Text = VarName, HorizontalAlign = HorizontalAlign.Left };
            NameCell.Style.Add("padding-left","1em");

            // Cell with the Description
            DescriptionCell = new TableCell { Text = VarDescription, HorizontalAlign = HorizontalAlign.Left };

            // Setting row with name, input and desc
            SettingRow = new TableRow();
            SettingRow.Cells.Add(NameCell);
            SettingRow.Cells.Add(InputCell);
            SettingRow.Cells.Add(DescriptionCell);
        } // Will be called every time the default value is changed

        // Object to access the value
        public virtual object GetSettingValue()
        {
            object value = DataField.Value;
            return value;
        }
    }

    // Derived class with controls for integer settings (slider)
    public class IntegerFeatureSetting : FeatureSetting
    {
        public IntegerFeatureSetting(string key, string name, HiddenField clientField, string description, Func<int, int> validate=null, int defaultVal = 0) 
            : base(key, name, clientField, description)
        {
            DefaultValue = defaultVal;
            Validate = validate;
        }

        internal override void GenerateSettingRow()
        {
            base.GenerateSettingRow();

            ValueLabel.Style.Add("margin-top", "0.5em");
            InputLabel.Text = "<div class='jQuerySlider' id='" + VarKey + "'></div>";
            
        }

        Func<int, int> Validate;

        public override object ParseString(string value)
        {
            try { return Int32.Parse(value); }
            catch{ return DefaultValue; }
        }

        public override object DefaultValue 
        { 
            get { return defaultValue; }  
            set { defaultValue = value; GenerateSettingRow(); } 
        }

        public override object GetSettingValue()
        {
            object value = base.GetSettingValue();
            int intValue;
            try
            {
                intValue = Int32.Parse(value.ToString());
            }
            catch { intValue = (int)DefaultValue; }

            if (Validate != null) intValue = Validate(intValue);

            return intValue;
        }

    }

    // Derived class with controls for bool settings (toggle button)
    public class BoolFeatureSetting : FeatureSetting
    {
        // Creates a toggle button on client-side, toggle functions and write to data field have to be programmed with JS 
        public BoolFeatureSetting(string key, string name, HiddenField clientField, string description, bool defaultVal = false)
            : base(key, name, clientField, description)
        {
            DefaultValue = defaultVal;
        }

        internal override void GenerateSettingRow()
        {
            base.GenerateSettingRow();

            ValueLabel.Attributes["class"] += " checkbox_hit";
            ValueLabel.Attributes["onclick"] += "checkHit(event, '" + VarKey + "')";
            if (!(bool)DefaultValue) ValueLabel.Attributes["class"] += " unchecked";

            InputLabel.Text = "<input type='checkbox' class='feature_checkbox' id='" + VarKey + "'>";
        }

        public override object ParseString(string value)
        {
            try { return bool.Parse(value); }
            catch { return DefaultValue; }
        }
        
        public override object DefaultValue 
        {
            get { return defaultValue; }
            set { defaultValue = value; GenerateSettingRow(); }
        }
        public override object GetSettingValue()
        {
            object value = base.GetSettingValue();
            bool boolValue;
            try
            {
                boolValue = bool.Parse(value.ToString());
            }
            catch { boolValue = (bool)DefaultValue; }
            
            return boolValue;
        }

    }

    // Derived class with controls for multiple choice values (radio button)
    public class MultiFeatureSetting : FeatureSetting
    {
        // Creates a div with radiobuttons on client-side, only animations and write to data field have to be programmed by JS / styled by CSS

        public MultiFeatureSetting(string key, string name, HiddenField clientField, string description, string[][] optionsNameValue, string defaultVal)
            : base(key, name, clientField, description)
        {
            Options = new List<string[]>();
            for(int iName = 0; iName < optionsNameValue.Length; iName++)
            {
                Options.Add(optionsNameValue[iName]);
            }

            DefaultValue = defaultVal;
        }

        private List<string[]> Options;

        internal override void GenerateSettingRow()
        {
            base.GenerateSettingRow();

            ValueLabel.Attributes["display"] = "none";

            string optionButtons = "";
            Options.ForEach((option) =>
            {
                int index = Options.IndexOf(option);
                string html = "<input type='radio' id='" + VarKey + index + "' name='" + VarKey + "' value='" + option[1] + "'>";
                html += "<label for='" + VarKey + index + "'>" + option[0] + "</label>";

                optionButtons += html;
            });

            InputLabel.Text = optionButtons;
            InputLabel.Attributes.Add("class", "radioGroup");
        }

        public override object ParseString(string value)
        {
            return value;
        }

        public override object DefaultValue
        {
            get { return defaultValue; }
            set { // check if default value is value of options array
                Options.ForEach((option) =>
                {
                    if (option[1] == value.ToString()) defaultValue = value;
                });
                if (defaultValue == null) defaultValue = Options[0][1];
                GenerateSettingRow(); 
            }
        }
        public override object GetSettingValue()
        {
            object value = base.GetSettingValue();
            try {
                return value.ToString();
            } catch { return DefaultValue; }
        }

    }
}