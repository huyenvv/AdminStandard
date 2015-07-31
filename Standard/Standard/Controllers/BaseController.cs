using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WebLib;

namespace Standard.Controllers
{
    public class BaseController : Controller
    {
        public DB_9CF750_dbEntities db = new DB_9CF750_dbEntities();
        public ActionResult AccessDenied()
        {
            return View("_AccessDenied");
        }
    }
}