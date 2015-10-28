using System;
using System.Globalization;
using System.Net;
using System.Threading;
using System.Web;

namespace Standard
{
    public class Culture
    {
        public const string VietNamese = "vi-VN";
        public const string English = "en-US";
        public static string GetClientIP()
        {
            string ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (ip == null)
            {
                ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }

            return ip;
            //return "113.190.242.198";
        }
        //s: dd/MM/yyyy
        public static DateTime ToDateTime(string s, string culture = VietNamese)
        {
            CultureInfo cul = new CultureInfo(culture);
            var dt = DateTime.Parse(s, cul);
            return dt;
        }

    }
}