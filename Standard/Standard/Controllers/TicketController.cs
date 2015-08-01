﻿using Standard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Standard.Repository;
using WebLib;
using WebLib.DAL;

namespace Standard.Controllers
{
    [CustomAuthorize]
    public class TicketController : BaseController
    {
        private readonly TicketRepository _ticketRepository;
        private readonly TicketDetailRepository _ticketDetailRepository;
        private readonly DeptRepository _deptRepository;
        private readonly TicketUserRepository _ticketUserRepository;
        private DB_9CF750_dbEntities db;
        public TicketController()
        {
            db = DB.Entities;
            _ticketRepository = new TicketRepository(db);
            _deptRepository = new DeptRepository(db);
            _ticketDetailRepository = new TicketDetailRepository(db);
            _ticketUserRepository = new TicketUserRepository(db);
        }
        public ActionResult Index()
        {

            return View(_ticketRepository.GetAll());
        }

        public ActionResult Details(int id)
        {
            return View(db.Ticket.FirstOrDefault(m => m.ID == id));
        }

        public ActionResult Create(int id = 0)
        {
            ViewBag.listDept = _deptRepository.GetAll();
            var tick = _ticketRepository.GetById(id);
            if (tick == null) return View(new Ticket { Created = DateTime.Now });

            if (tick.CreatedBy != fwUserDAL.GetCurrentUser().ID || tick.Status != TicketStatus.KhoiTao)
                return RedirectToAction("AccessDenied", "Home");

            SessionUtilities.Set(Constant.SESSION_TicketDetails, tick.TicketDetails.ToList());
            return View(tick);
        }

        public JsonResult AddTicketDetail(TicketDetails detail)
        {
            var listTicketDetail = new List<TicketDetails>();
            if (SessionUtilities.Exist(Constant.SESSION_TicketDetails))
            {
                listTicketDetail = (List<TicketDetails>)SessionUtilities.Get(Constant.SESSION_TicketDetails);
            }
            listTicketDetail.Add(detail);
            SessionUtilities.Set(Constant.SESSION_TicketDetails, listTicketDetail);
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Create(Ticket tick, bool isSend = false)
        {
            try
            {
                // TODO: Add insert logic here
                if (tick.Type > 0 && tick.DeptID > 0)
                {
                    if (SessionUtilities.Exist(Constant.SESSION_TicketDetails))
                    {
                        var listTicketDetail = (List<TicketDetails>)SessionUtilities.Get(Constant.SESSION_TicketDetails);
                        if (listTicketDetail.Count > 0)
                        {
                            var currentUser = fwUserDAL.GetCurrentUser();
                            var getTicket = _ticketRepository.GetById(tick.ID);
                            if (getTicket != null && getTicket.TicketDetails.Count > 0)
                            {
                                // edit
                                getTicket.Type = tick.Type;
                                getTicket.DeptID = tick.DeptID;
                                _ticketRepository.Update(getTicket);

                                var lstTicketDetailModel = getTicket.TicketDetails.Select(m => m.ID).ToList();
                                _ticketDetailRepository.Delete(lstTicketDetailModel);
                            }
                            else
                            {
                                // Create ticket
                                getTicket = new Ticket
                                {
                                    Current = currentUser.ID,
                                    Created = DateTime.Now,
                                    CreatedBy = currentUser.ID,
                                    Status = TicketStatus.KhoiTao,
                                    Track = currentUser.ID + "#;",
                                    DeptID = tick.DeptID
                                };
                                _ticketRepository.Insert(getTicket);
                            }


                            // create ticket detail
                            var listDetail = listTicketDetail.Select(item => new TicketDetails
                            {
                                DateRequire = item.DateRequire,
                                Quantity = item.Quantity,
                                Reason = item.Reason,
                                Title = item.Title,
                                TicketID = getTicket.ID
                            }).ToList();
                            _ticketDetailRepository.Insert(listDetail);

                            // remove ticket detail session
                            SessionUtilities.Set(Constant.SESSION_TicketDetails, null);

                            // isSend = true => send for lead dept
                            if (isSend)
                            {
                                var dept = _deptRepository.GetById(tick.DeptID);
                                if (dept != null && dept.LeaderUserID.HasValue)
                                {
                                    getTicket.Track += dept.LeaderUserID + "#;";
                                    getTicket.Status = TicketStatus.ChoThongQua;
                                    _ticketRepository.Update(getTicket);

                                    // add to table Ticket User 
                                    _ticketUserRepository.Insert(new TicketUser
                                    {
                                        TicketID = getTicket.ID,
                                        UserID = dept.LeaderUserID.Value
                                    });
                                }

                            }


                        }
                    }
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View(new Ticket { Created = DateTime.Now });
            }
        }

        public ActionResult Delete(int id)
        {
            var tick = _ticketRepository.GetById(id);
            if (tick.CreatedBy != fwUserDAL.GetCurrentUser().ID || tick.Status != TicketStatus.KhoiTao)
                return RedirectToAction("AccessDenied", "Home");

            // delete 
            _ticketRepository.Delete(tick);
            return RedirectToAction("Index");
        }
        public JsonResult DeleteTicketDetail(int id)
        {
            if (SessionUtilities.Exist(Constant.SESSION_TicketDetails))
            {
                var listTicketDetail = (List<TicketDetails>)SessionUtilities.Get(Constant.SESSION_TicketDetails);
                var k = listTicketDetail.FindIndex(m => m.ID == id);
                listTicketDetail.RemoveAt(k);
                SessionUtilities.Set(Constant.SESSION_TicketDetails, listTicketDetail);
            }
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PhanHoi(string ykien)
        {
            return AccessDenied();
        }
        public ActionResult ThongQua(int id, string returnUrl)
        {
            var obj = db.Ticket.FirstOrDefault(m => m.ID == id);
            if (!CanThongQua(obj)) return AccessDenied();
            obj.Track += ";#" + DB.CurrentUser.ID;
            obj.Status = TicketStatus.CanKiemDuyet;
            var bp = _deptRepository.GetById(obj.DeptID);
            var u = new fwGroupDAL().GetByID(bp.GroupID).fwUser.FirstOrDefault();
            obj.Current = u.ID;
            if (!obj.TicketUser.Any(m => m.UserID == u.ID))
                obj.TicketUser.Add(new TicketUser() { TicketID = obj.ID, UserID = u.ID });

            if (!string.IsNullOrEmpty(returnUrl)) Redirect(returnUrl);
            return RedirectToAction("Index", "Home");
        }
        public ActionResult KiemDuyet(int id, string returnUrl)
        {
            var obj = db.Ticket.FirstOrDefault(m => m.ID == id);
            if (!CanKiemDuyet(obj)) return AccessDenied();
            obj.Track += ";#" + DB.CurrentUser.ID;
            obj.Status = TicketStatus.ChoDuyet;
            var u = new fwUserDAL().ListByRole(RoleList.ApproveTicket).FirstOrDefault();
            obj.Current = u.ID;
            if (!obj.TicketUser.Any(m => m.UserID == u.ID))
                obj.TicketUser.Add(new TicketUser() { TicketID = obj.ID, UserID = u.ID });

            if (!string.IsNullOrEmpty(returnUrl)) Redirect(returnUrl);
            return RedirectToAction("Index", "Home");
        }
        public ActionResult Duyet(int id, string returnUrl)
        {
            var obj = db.Ticket.FirstOrDefault(m => m.ID == id);
            if (!CanDuyet(obj)) return AccessDenied();
            obj.Track += ";#" + DB.CurrentUser.ID;
            obj.Status = TicketStatus.DaDuyet;

            if (!string.IsNullOrEmpty(returnUrl)) Redirect(returnUrl);
            return RedirectToAction("Index", "Home");
        }
        #region Check role
        public static bool CanThongQua(Ticket obj)
        {
            return DB.CurrentUser.ID == obj.Current && obj.Status == TicketStatus.ChoThongQua;
        }
        public static bool CanKiemDuyet(Ticket obj)
        {
            return DB.CurrentUser.ID == obj.Current && obj.Status == TicketStatus.CanKiemDuyet;
        }
        public static bool CanDuyet(Ticket obj)
        {
            return DB.CurrentUser.ID == obj.Current && obj.Status == TicketStatus.ChoDuyet && new fwUserDAL().UserInRole(RoleList.ApproveTicket);
        }
        #endregion
    }
}
