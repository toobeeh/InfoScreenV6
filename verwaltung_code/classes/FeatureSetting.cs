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
            set { defaultValue = value; GenerateSettingRow(); } 
        }
        
        internal object defaultValue;
        internal TableCell NameCell, InputCell, DescriptionCell;
        internal Label ValueLabel, InputLabel;

        public virtual object ParseString(string value) { return value; }

        // Base constructor
        public FeatureSetting(string key, string name, HiddenField clientField, string description, object defaultVal = null)
        {
            // init values
            VarKey = key;
            VarName = name;
            VarDescription = description;
            DataField = clientField;
            DefaultValue = defaultVal;
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
            : base(key, name, clientField, description, defaultVal)
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
        public BoolFeatureSetting(string key, string name, HiddenField clientField, string description, bool defaultVal = false)
            : base(key, name, clientField, description, defaultVal)
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


}