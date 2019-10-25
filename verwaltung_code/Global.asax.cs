using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.UI;

namespace Infoscreen_Verwaltung
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            //ScriptResourceDefinition myScriptResDef = new ScriptResourceDefinition();
            //myScriptResDef.Path = "~/scripts/jquery-1.11.1.min.js";
            //myScriptResDef.DebugPath = "~/Scripts/jquery-1.11.1.js";
            //ScriptManager.ScriptResourceMapping.AddDefinition("jquery", null, myScriptResDef);
        }
    }
}