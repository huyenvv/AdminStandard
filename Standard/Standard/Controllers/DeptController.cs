using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Standard.Controllers
{
    public class DeptController : BaseController
    {
        public ActionResult Index()
        {
            return View(db.Dept.ToList());
        }
	}
}