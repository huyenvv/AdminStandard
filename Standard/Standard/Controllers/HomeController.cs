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

            ViewBag.checkoutCount = db.CheckoutUser.Where(m => m.UserID == DB.CurrentUser.ID && m.Checkout.Current == DB.CurrentUser.ID && m.Checkout.Status != TicketStatus.DaDuyet).Count();
            return View();
        }

    }
}