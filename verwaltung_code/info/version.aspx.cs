using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Infoscreen_Verwaltung.info
{
    public partial class version : System.Web.UI.Page
    {
        string ebene = "../";

        protected void Page_Load(object sender, EventArgs e)
        {
            TopMenu.Text = classes.Anzeige.TopBar(ebene);
            Menu.Text = classes.Anzeige.Menue(ebene, "info");
        }
    }
}