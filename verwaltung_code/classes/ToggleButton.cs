using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace Infoscreen_Verwaltung.classes
{
    public class ToggleButton : System.Web.UI.WebControls.Label
    {
        HtmlGenericControl label;
        CheckBox checkbox;

        public ToggleButton() : base()
        {
            label = new HtmlGenericControl();
            label.Style["height"] = "1em";
            label.Style["width"] = "2em";
            label.Attributes["class"] = "checkbox_hit";

            checkbox = new CheckBox();
            checkbox.Style["display"] = "none";

            this.Controls.Add(label);
            this.Controls.Add(checkbox);
        }

        public override string ID
        {
            set
            {
                checkbox.ID = value;

                string function = @"this.classList.toggle('unchecked');
                                document.getElementById('Content_" + value + @"').click();";

                label.Attributes["onclick"] = function;           
            }
            get { return checkbox.ID; }
            
        }

        public override string Text
        {
            get { return checkbox.Text; }
            set { checkbox.Text = value; }
        }

        public bool Checked
        {
            get { return checkbox.Checked; }
            set
            {
                checkbox.Checked = value;
                label.Attributes["class"] = value ? "checkbox_hit" : "checkbox_hit unchecked";
            }
        }

        public CheckBox CheckElement { get { return checkbox; } set { checkbox = value; } }
        
    }
}