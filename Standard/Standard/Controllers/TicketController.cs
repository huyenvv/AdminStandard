using Standard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebLib;
using WebLib.DAL;

namespace Standard.Controllers
{
    public class TicketController : Controller
    {
        //
        // GET: /Ticket/
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Ticket/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Ticket/Create
        public ActionResult Create()
        {
            return View();
        }

        public JsonResult AddTicketDetail(TicketDetail detail)
        {
            var listTicketDetail = new List<TicketDetail>();
            if (SessionUtilities.Exist(SESSION.TicketDetail))
            {
                listTicketDetail = (List<TicketDetail>)SessionUtilities.Get(SESSION.TicketDetail);
            }
            listTicketDetail.Add(detail);
            SessionUtilities.Set(SESSION.TicketDetail, listTicketDetail);
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /Ticket/Create
        [HttpPost]
        public ActionResult Create(int type, int deptId)
        {
            try
            {
                // TODO: Add insert logic here
                if (type > 0 && deptId > 0)
                {
                    if (SessionUtilities.Exist(SESSION.TicketDetail))
                    {
                        var db = DB.Entites;
                        var listTicketDetail = (List<TicketDetail>)SessionUtilities.Get(SESSION.TicketDetail);
                        if (listTicketDetail.Count > 0)
                        {
                            var currentUser = new fwUserDAL().GetByUserName(DB.CurrentUser.Identity.Name);
                            if (currentUser != null)
                            {
                                // Create ticket
                                var ticket = new Ticket
                                {
                                    Current = currentUser.ID,
                                    Created = DateTime.Now,
                                    CreatedBy = currentUser.ID,
                                    Status = (int)Status.ChoDuyet,
                                    Track = currentUser.ID + "#;",
                                    DeptID = deptId
                                };
                                db.Set<Ticket>().Add(ticket);
                                db.SaveChanges();

                                // create ticket detail
                                var listDetail = new List<TicketDetails>();
                                foreach (var item in listTicketDetail)
                                {
                                    var ticketDetail = new TicketDetails
                                    {
                                        DateRequire = item.NgayCan,
                                        Quantity = item.SoLuong,
                                        Reason = item.LyDo,
                                        Title = item.DienGiai,
                                        TicketID = ticket.ID
                                    };
                                    listDetail.Add(ticketDetail);
                                }
                                db.Set<TicketDetails>().AddRange(listDetail);
                                db.SaveChanges();

                                // remove ticket detail session
                                SessionUtilities.Set(SESSION.TicketDetail, null);
                            }

                        }
                    }
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Ticket/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Ticket/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Ticket/Delete/5
        public JsonResult DeleteTicketDetail(int id)
        {
            var listTicketDetail = new List<TicketDetail>();
            if (SessionUtilities.Exist(SESSION.TicketDetail))
            {
                listTicketDetail = (List<TicketDetail>)SessionUtilities.Get(SESSION.TicketDetail);
                var k = listTicketDetail.FindIndex(m => m.Id == id);
                listTicketDetail.RemoveAt(k);
                SessionUtilities.Set(SESSION.TicketDetail, listTicketDetail);
            }
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /Ticket/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
