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
            ViewBag.ticketCount = db.TicketUser.Where(m => m.UserID == DB.CurrentUser.ID && m.Ticket.Current == DB.CurrentUser.ID && m.Ticket.CheckoutID == null).Count();


            ViewBag.checkoutCount = db.CheckoutUser.Where(m => m.UserID == DB.CurrentUser.ID && m.Checkout.Current == DB.CurrentUser.ID).Count();

            ViewBag.notiCount = CountNoti();

            return View();
        }


        #region Notification
        public ActionResult ListNoti()
        {
            new WebLib.DAL.fwNotificationDAL().RemoveCount(DB.CurrentUser.ID);
            var lst = new WebLib.DAL.fwNotificationDAL().ListByUser(DB.CurrentUser.ID);
            return PartialView(lst);
        }
        public string CountNoti()
        {
            var c = new WebLib.DAL.fwNotificationDAL().CountNew(DB.CurrentUser.ID);
            if (c != 0) return string.Format("<i class='tmn-counts'>{0}</i>", c);

            return null;
        }
        public void ClearNoti()
        {
            new WebLib.DAL.fwNotificationDAL().Clear(DB.CurrentUser.ID);
        }
        #endregion

        #region Ticket Type
        public ActionResult ListTicketType()
        {
            return View(DB.Entities.TicketType.ToList());
        }

        public ActionResult EditTicketType(int? id, string returnUrl)
        {
            var obj = id.HasValue ? DB.Entities.TicketType.FirstOrDefault(m => m.ID == id) : null;
            return View(obj == null ? new TicketType() : obj);
        }
        [HttpPost]
        public ActionResult EditTicketType(TicketType model, string returnUrl)
        {
            if (model.ID == 0)
                db.TicketType.Add(model);
            else
            {
                db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            }
            db.SaveChanges();
            if (returnUrl == null) return RedirectToAction("ListTicketType"); else return Redirect(returnUrl);
        }
        public ActionResult DeleteTicketType(int id, string returnUrl)
        {
            new WebLib.DAL.fwBaseDAL("TicketType").Delete(id);
            if (returnUrl == null) return RedirectToAction("ListTicketType"); else return Redirect(returnUrl);
        }
        #endregion

    }
}