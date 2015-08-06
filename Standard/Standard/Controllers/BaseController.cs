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

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {

            base.OnActionExecuted(filterContext);

            ViewBag.notiCount = CountNoti();
        }
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
        public string CountNoti()
        {
            var c = new WebLib.DAL.fwNotificationDAL().CountNew(DB.CurrentUser.ID);
            if (c != 0) return string.Format("<i class='tmn-counts'>{0}</i>", c);

            return null;
        }

        public void ShowMessage(string message, bool isSuccess = true)
        {
            if (isSuccess)
            {
                SessionUtilities.Set(Constant.SESSION_MessageSuccess, message);
            }
            else
            {
                SessionUtilities.Set(Constant.SESSION_MessageError, message);
            }

        }
    }
}