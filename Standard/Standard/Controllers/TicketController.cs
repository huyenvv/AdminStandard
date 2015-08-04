using Standard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Standard.Repository;
using WebLib;
using WebLib.DAL;
using Newtonsoft.Json;
using System.Globalization;
using System.Threading;

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
        public ActionResult Index(int? status)
        {
            var list = DB.CurrentUser.UserName == WebLib.Constant.AdminFix ? db.Ticket : db.TicketUser.Where(m => m.UserID == DB.CurrentUser.ID).Select(m => m.Ticket);
            if (status.HasValue)
            {
                if (status.Value != -1)
                {
                    list = list.Where(m => m.Status == status.Value);
                }
            }
            else
            {
                list = list.Where(m => m.Current == DB.CurrentUser.ID && m.CheckoutID == null);
            }
            return View(list.ToList());
        }

        public ActionResult Details(int id)
        {
            return View(db.Ticket.FirstOrDefault(m => m.ID == id));
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
        public ActionResult Create(int id = 0)
        {
            ViewBag.listDept = _deptRepository.GetKiemSoat();
            var tick = _ticketRepository.GetById(id);
            if (tick == null) return View(new Ticket { Created = DateTime.Now });

            if (tick.CreatedBy != fwUserDAL.GetCurrentUser().ID || tick.Status != TicketStatus.KhoiTao)
                return RedirectToAction("AccessDenied", "Home");

            SessionUtilities.Set(Constant.SESSION_TicketDetails, tick.TicketDetails.ToList());
            return View(tick);
        }
        [HttpPost]
        public ActionResult Create(Ticket tick, List<HttpPostedFileBase> files, string listTicketDetailJson, bool isSend = false)
        {
            // TODO: Add insert logic here
            if (tick.Type > 0 && tick.DeptID > 0)
            {
                var listTicketDetails = new List<Fuck>();
                if (!string.IsNullOrEmpty(listTicketDetailJson))
                {
                    try
                    {
                        System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                        listTicketDetails = serializer.Deserialize<List<Fuck>>(listTicketDetailJson);
                    }
                    catch (Exception)
                    {
                        listTicketDetails = new List<Fuck>();
                    }
                }
                if (listTicketDetails.Count > 0)
                {
                    if (listTicketDetails.Count > 0)
                    {
                        var currentUser = fwUserDAL.GetCurrentUser();
                        // xu ly file 
                        string fileNames = "";
                        foreach (var item in files)
                        {
                            if (item != null)
                            {
                                var fileName = FileUpload.CreateFile(item, UploadFolder.Ticket, false);
                                fileNames += fileName + ";#";
                            }
                        }
                        var getTicket = _ticketRepository.GetById(tick.ID);
                        if (getTicket != null && getTicket.TicketDetails.Count > 0)
                        {
                            // edit
                            getTicket.FilePath += fileNames;
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
                                FilePath = fileNames,
                                Current = currentUser.ID,
                                Created = DateTime.Now,
                                CreatedBy = currentUser.ID,
                                Status = TicketStatus.KhoiTao,
                                Track = currentUser.ID + "#;",
                                DeptID = tick.DeptID,
                                Type = tick.Type
                            };
                            _ticketRepository.Insert(getTicket);
                            db.Database.ExecuteSqlCommand(string.Format("insert into TicketUser values({0},{1})", getTicket.ID, DB.CurrentUser.ID));
                        }

                        // create ticket detail
                        var listDetail = listTicketDetails.Select(item => new TicketDetails
                        {
                            DateRequire = DateTime.Parse(item.DateRequire),
                            Quantity = item.Quantity,
                            Reason = item.Reason,
                            Title = item.Title,
                            TicketID = getTicket.ID
                        }).ToList();
                        _ticketDetailRepository.Insert(listDetail);

                        // remove ticket detail session

                        // isSend = true => send for lead dept
                        if (isSend)
                        {
                            //Lấy trưởng phòng của người tạo
                            return RedirectToAction("GuiYeuCau", new { id = getTicket.ID });
                        }
                    }
                }
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            var tick = _ticketRepository.GetById(id);
            if (!new fwUserDAL().UserInRole(RoleList.SystemManager))
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
        [HttpPost]

        #region Process line
        public ActionResult PhanHoi(int id, string ykien, string returnUrl)
        {
            var obj = db.Ticket.FirstOrDefault(m => m.ID == id);
            var feedback = db.Feedback.Add(new Feedback() { Created = DateTime.Now, TicketID = id, UserID = DB.CurrentUser.ID, Title = ykien });
            db.SaveChanges();

            obj.Status = TicketStatus.KhoiTao;
            obj.Track += ";#" + DB.CurrentUser.ID;
            obj.FeedbackID = feedback.ID;
            obj.Current = obj.CreatedBy;
            db.SaveChanges();
            if (!string.IsNullOrEmpty(returnUrl)) Redirect(returnUrl);
            return RedirectToAction("Index", "Ticket");
        }
        public ActionResult GuiYeuCau(int id, string returnUrl)
        {
            var obj = db.Ticket.FirstOrDefault(m => m.ID == id);
            if (!CanGuiYeuCau(obj)) return AccessDenied();
            obj.Track += ";#" + DB.CurrentUser.ID;
            obj.Status = TicketStatus.ChoThongQua;
            obj.FeedbackID = null;
            //Lấy trưởng phòng của người tạo
            var userDAL = new fwUserDAL();
            var lstNhom = userDAL.GetByID(obj.CreatedBy).fwGroup.Select(m => m.ID).ToList();
            var dept = db.Dept.Where(m => lstNhom.Contains(m.GroupID)).FirstOrDefault();
            if (dept == null) return RedirectToAction("Details", new { id = id });

            obj.Current = dept.LeaderUserID.Value;
            db.SaveChanges();
            //Phân quyền xem
            if (!obj.TicketUser.Any(m => m.UserID == dept.LeaderUserID.Value))
                db.Database.ExecuteSqlCommand(string.Format("insert into TicketUser values({0},{1})", obj.ID, dept.LeaderUserID.Value));

            CreateNoti(obj.Current, "Cần thông qua phiếu đề nghị dụng cụ làm việc", Url.Action("Details", new { id = id }));

            if (!string.IsNullOrEmpty(returnUrl)) Redirect(returnUrl);
            return RedirectToAction("Index", "Home");
        }
        public ActionResult ThongQua(int id, string returnUrl)
        {
            var obj = db.Ticket.FirstOrDefault(m => m.ID == id);
            if (!CanThongQua(obj)) return AccessDenied();
            obj.Track += ";#" + DB.CurrentUser.ID;
            obj.Status = TicketStatus.ChoKiemDuyet;
            obj.FeedbackID = null;
            //Lấy bộ phận kiểm duyệt
            var bp = _deptRepository.GetById(obj.DeptID);
            //Lấy người trong bộ phận kiểm duyệt
            var u = new fwGroupDAL().GetByID(bp.GroupID).fwUser.FirstOrDefault();
            obj.Current = u.ID;
            if (!obj.TicketUser.Any(m => m.UserID == u.ID))
                db.Database.ExecuteSqlCommand(string.Format("insert into TicketUser values({0},{1})", obj.ID, u.ID));

            CreateNoti(obj.CreatedBy, "Phiếu đề nghị dụng cụ làm việc của bạn đã được thông qua", Url.Action("Details", new { id = id }));
            CreateNoti(obj.Current, "Cần kiểm tra phiếu đề nghị dụng cụ làm việc", Url.Action("Details", new { id = id }));

            db.SaveChanges();
            if (!string.IsNullOrEmpty(returnUrl)) Redirect(returnUrl);
            return RedirectToAction("Index", "Home");
        }
        public ActionResult KiemDuyet(int id, string returnUrl)
        {
            var obj = db.Ticket.FirstOrDefault(m => m.ID == id);
            if (!CanKiemDuyet(obj)) return AccessDenied();
            obj.Track += ";#" + DB.CurrentUser.ID;
            obj.Status = TicketStatus.ChoDuyet;
            obj.FeedbackID = null;
            //lấy người có quyền phê duyệt
            var u = new fwUserDAL().ListByRole(RoleList.ApproveTicket).FirstOrDefault();
            obj.Current = u.ID;
            if (!obj.TicketUser.Any(m => m.UserID == u.ID))
                db.Database.ExecuteSqlCommand(string.Format("insert into TicketUser values({0},{1})", obj.ID, u.ID));


            CreateNoti(obj.CreatedBy, "Phiếu đề nghị dụng cụ làm việc của bạn đã được kiểm duyệt", Url.Action("Details", new { id = id }));
            CreateNoti(obj.Current, "Cần duyệt tra phiếu đề nghị dụng cụ làm việc", Url.Action("Details", new { id = id }));

            db.SaveChanges();
            if (!string.IsNullOrEmpty(returnUrl)) Redirect(returnUrl);
            return RedirectToAction("Index", "Home");
        }
        public ActionResult Duyet(int id, string returnUrl)
        {
            var obj = db.Ticket.FirstOrDefault(m => m.ID == id);
            if (!CanDuyet(obj)) return AccessDenied();
            obj.Track += ";#" + DB.CurrentUser.ID;
            obj.Status = TicketStatus.DaDuyet;
            obj.FeedbackID = null;
            //Gán người xử lý giờ là người tạo phiếu, người tạo phiếu sẽ tiếp tục tạo phiếu thanh toán
            obj.Current = obj.CreatedBy;


            CreateNoti(obj.CreatedBy, "Phiếu đề nghị dụng cụ làm việc của bạn đã được duyệt", Url.Action("Details", new { id = id }));

            db.SaveChanges();
            if (!string.IsNullOrEmpty(returnUrl)) Redirect(returnUrl);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult TaoCheckout(int ticketID)
        {
            var obj = db.Ticket.FirstOrDefault(m => m.ID == ticketID);
            if (!CanTaoCheckout(obj)) return AccessDenied();
            return RedirectToAction("Create", "RequestBill", new { ticketID = ticketID });
        }
        #endregion

        #region Check role
        public static bool CanGuiYeuCau(Ticket obj)
        {
            return obj.Status == TicketStatus.KhoiTao && obj.Current == DB.CurrentUser.ID;
        }
        public static bool CanThongQua(Ticket obj)
        {
            return DB.CurrentUser.ID == obj.Current && obj.Status == TicketStatus.ChoThongQua;
        }
        public static bool CanKiemDuyet(Ticket obj)
        {
            return DB.CurrentUser.ID == obj.Current && obj.Status == TicketStatus.ChoKiemDuyet;
        }
        public static bool CanDuyet(Ticket obj)
        {
            return DB.CurrentUser.ID == obj.Current && obj.Status == TicketStatus.ChoDuyet && new fwUserDAL().UserInRole(DB.CurrentUser.ID, RoleList.ApproveTicket);
        }
        public static bool CanTaoCheckout(Ticket obj)
        {
            return DB.CurrentUser.ID == obj.Current && obj.Status == TicketStatus.DaDuyet && obj.CheckoutID == null;
        }
        #endregion

        public class Fuck
        {
            public string Title { get; set; }
            public string Reason { get; set; }
            public string DateRequire { get; set; }
            public int Quantity { get; set; }
        }
    }
}
