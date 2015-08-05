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

        public void CreateNoti(int userID, string message, string link)
        {
            var obj = new WebLib.Models.fwNotification();
            obj.UserID = userID; obj.Title = message; obj.Link = link;
            new WebLib.DAL.fwNotificationDAL().Insert(obj);
            var u = new WebLib.DAL.fwUserDAL().GetByID(userID);
            u.NotiCount += 1;
            new WebLib.DAL.fwUserDAL().Update(u);
        }
    }
}