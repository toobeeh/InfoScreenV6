using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Http;
using Infoscreen_Verwaltung;


namespace ScreenCoreApp
{
    public class Screen
    {
        public static int GetSessionScreenID(HttpContext current)
        {
            return GetIntSessionVal("screen-id", current);
        }

        private static void SetSessionScreenID(string val, HttpContext current)
        {
            SetIntSessionVal("screen-id", val, current);
        }

        public static int GetPageCycleIndex(HttpContext current)
        {
            return GetIntSessionVal("page-cycle-index", current);
        }

        public static void SetPageCycleIndex(string val, HttpContext current)
        {
            SetIntSessionVal("page-cycle-index", val, current);
        }

        public static void CheckScreenID(IQueryCollection querystring, HttpContext context)
        {
            string val = querystring["id"].ToString();
            if (!String.IsNullOrEmpty(val))
            {
                SetSessionScreenID(querystring["id"].ToString(), context);
            }
        }

        public static bool GetPresentationRunningStatus(HttpContext current)
        {
            if (GetIntSessionVal("next_slide", current) <2) return false;
            else return true;
        }

        public static int GetNextPresentationSlide(HttpContext current)
        {
            return GetIntSessionVal("next_slide", current);
        }

        public static void SetNextPresentationSlide(int slide, HttpContext current)
        {
            SetIntSessionVal("next_slide", slide.ToString(), current);
        }


        private static int GetIntSessionVal(string key, HttpContext current)
        {           
            int? val = current.Session.GetInt32(key);
            if (val == null) return -1;
            else return val.GetValueOrDefault();
        }

        private static void SetIntSessionVal(string key, string val, HttpContext current)
        {
            int value;
            try
            {
                value = Convert.ToInt32(val);
                current.Session.SetInt32(key, value);
            }
            catch { }
        }

    }
}
