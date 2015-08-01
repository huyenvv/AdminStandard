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
    [CustomAuthorize]
    public class TicketController : BaseController
    {
        
        public ActionResult Index()
        {
            
            return View(db.Ticket.ToList());
        }

        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        public JsonResult AddTicketDetail(TicketDetail detail)
        {
            var listTicketDetail = new List<TicketDetail>();
            if (SessionUtilities.Exist(Constant.SESSION_TicketDetails))
            {
                listTicketDetail = (List<TicketDetail>)SessionUtilities.Get(Constant.SESSION_TicketDetails);
            }
            listTicketDetail.Add(detail);
            SessionUtilities.Set(Constant.SESSION_TicketDetails, listTicketDetail);
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Create(int type, int deptId)
        {
            try
            {
                // TODO: Add insert logic here
                if (type > 0 && deptId > 0)
                {
                    if (SessionUtilities.Exist(Constant.SESSION_TicketDetails))
                    {
                        var db = DB.Entites;
                        var listTicketDetail = (List<TicketDetail>)SessionUtilities.Get(Constant.SESSION_TicketDetails);
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
                                    Status = TicketStatus.ChoDuyet,
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
                                SessionUtilities.Set(Constant.SESSION_TicketDetails, null);
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

        public ActionResult Edit(int id)
        {
            return View();
        }

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

        public ActionResult Delete(int id)
        {
            return View();
        }

        // GET: /Ticket/Delete/5
        public JsonResult DeleteTicketDetail(int id)
        {
            var listTicketDetail = new List<TicketDetail>();
            if (SessionUtilities.Exist(Constant.SESSION_TicketDetails))
            {
                listTicketDetail = (List<TicketDetail>)SessionUtilities.Get(Constant.SESSION_TicketDetails);
                var k = listTicketDetail.FindIndex(m => m.Id == id);
                listTicketDetail.RemoveAt(k);
                SessionUtilities.Set(Constant.SESSION_TicketDetails, listTicketDetail);
            }
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }
    }
}
