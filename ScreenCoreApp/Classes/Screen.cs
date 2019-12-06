using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Http;
using Infoscreen_Bibliotheken;


namespace ScreenCoreApp
{
    public class Screen
    {
        public static int GetSessionScreenID(HttpContext current)
        {
            int? id = current.Session.GetInt32("screen-id");
            if (id == null) return -1;
            else return id.GetValueOrDefault();
        }

        private static void SetSessionScreenID(string val, HttpContext current)
        {
            int id;
            try
            {
                id = Convert.ToInt32(val);
                current.Session.SetInt32("screen-id", id);
            }
            catch{}  
        }

        public static void CheckScreenID(IQueryCollection querystring, HttpContext context)
        {
            string val = querystring["id"].ToString();
            if (!String.IsNullOrEmpty(val))
            {
                SetSessionScreenID(querystring["id"].ToString(), context);
            }
        }

    }
}
