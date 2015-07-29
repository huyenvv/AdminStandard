using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebLib
{
    public class SessionUtilities
    {
        public static bool Exist(string key)
        {
            return HttpContext.Current.Session[key] != null;
        }

        public static object Get(string key)
        {
            return HttpContext.Current.Session[key];
        }
        public static void Set(string key, object value)
        {
            HttpContext.Current.Session[key] = value;
        }
        public static void Add(string key, object value)
        {
            HttpContext.Current.Session[key] = value;
        }

        public static void Remove(string key)
        {
            HttpContext.Current.Session[key] = null;
        }
    }
}