using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Infoscreen_Bildschirme
{
    public partial class NichtStoeren : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Den Grund für den "Nich stören" hinweis asulesen
            string NichtStoeren_Grund = Request.QueryString["id"];
            // Den Grund nur einblenden, wenn einer Vorhanden ist
            if (NichtStoeren_Grund != null && NichtStoeren_Grund != "")
            {
                lb_NichtStören4.Text = "<h1>" + NichtStoeren_Grund + "</h1>";
                lb_NichtStören4.BorderStyle = BorderStyle.Solid;
            }
            table_NichtStoeren.BackColor = lb_NichtStören1.BackColor;
        }
    }
}