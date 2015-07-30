using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Standard.Controllers
{
    [CustomAuthorize]
    public class HomeController : BaseController
    {
        public string test()
        {
            return null;
        }
        public ActionResult Index()
        {
            return View();
        }

    }
}