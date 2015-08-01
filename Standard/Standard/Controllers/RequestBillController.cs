using Standard.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebLib;
using WebLib.DAL;

namespace Standard.Controllers
{
    public class RequestBillController : BaseController
    {
        private readonly CheckoutRepository _checkoutRepository;
        private readonly CheckoutDetailsRepository _checkoutDetailRepository;
        private readonly DeptRepository _deptRepository;
        private DB_9CF750_dbEntities db;

        public RequestBillController()
        {
            db = DB.Entities;
            _checkoutRepository = new CheckoutRepository(db);
            _deptRepository = new DeptRepository(db);
            _checkoutDetailRepository = new CheckoutDetailsRepository(db);
        }
        public ActionResult Index(int? status)
        {
            var list = DB.CurrentUser.UserName == WebLib.Constant.AdminFix ? db.Checkout : db.CheckoutUser.Where(m => m.UserID == DB.CurrentUser.ID).Select(m => m.Checkout);
            if (status.HasValue)
            {
                if (status.Value != -1)
                {
                    list = list.Where(m => m.Status == status.Value);
                }
            }
            else
            {
                list = list.Where(m => m.Current == DB.CurrentUser.ID);
            }
            return View(list.ToList());
        }

        public ActionResult Details(int id)
        {
            ViewBag.listDept = _deptRepository.GetKiemSoatNB();
            var obj = db.Checkout.FirstOrDefault(m => m.ID == id);
            return View(obj);
        }

        public ActionResult Create(int CheckoutId = 0)
        {
            ViewBag.listDept = _deptRepository.GetKiemSoatNB();
            var checkout = _checkoutRepository.GetById(CheckoutId);
            if (checkout == null) return View(new Checkout { Created = DateTime.Now });

            if (checkout.CreatedBy != fwUserDAL.GetCurrentUser().ID || checkout.Status != CheckoutStatus.KhoiTao)
                return AccessDenied();

            SessionUtilities.Set(Constant.SESSION_CheckoutDetails, checkout.CheckoutDetails.ToList());
            return View(checkout);
        }

        [HttpPost]
        public ActionResult Create(FormCollection frm, Checkout model, int ticketID)
        {
            try
            {
                model.Created = DateTime.Now;
                model.CreatedBy = DB.CurrentUser.ID;
                model.SumTotal = 0;
                model.Total = 0;
                model.Track += ";#" + model.CreatedBy;

                //Lấy bộ phận kiểm duyệt
                var bp = _deptRepository.GetById(model.DeptID);
                //Lấy người trong bộ phận kiểm duyệt
                var u = new fwGroupDAL().GetByID(bp.GroupID).fwUser.FirstOrDefault();
                model.Current = u.ID;

                model.Status = CheckoutStatus.ChoKiemSoat;
                db.Checkout.Add(model);
                db.SaveChanges();

                var obj = db.Ticket.FirstOrDefault(m => m.ID == ticketID);
                foreach (var item in obj.TicketDetails)
                {
                    var checkoutdetails = new CheckoutDetails();
                    checkoutdetails.Title = item.Title;
                    checkoutdetails.VND = decimal.Parse("0" + frm["details_" + item.ID].Replace(",0", null));
                    checkoutdetails.CheckoutID = model.ID;
                    model.SumTotal += checkoutdetails.VND;
                    model.CheckoutDetails.Add(checkoutdetails);
                }
                model.Total = model.SumTotal;

                obj.CheckoutID = model.ID;

                db.SaveChanges();
                db.Database.ExecuteSqlCommand(string.Format("insert into CheckoutUser values({0},{1})", model.ID, model.Current));
                return RedirectToAction("Index");
            }
            catch
            {
                return Redirect(Request.RawUrl);
            }
        }

        public ActionResult Delete(int id)
        {
            new WebLib.DAL.fwBaseDAL("Ticket").Delete(id);
            return View("Index");
        }


        public ActionResult KiemDuyet(int id)
        {
            var obj = db.Checkout.FirstOrDefault(m => m.ID == id);
            if (!CanKiemDuyet(obj)) return AccessDenied();
            obj.Track += ";#" + DB.CurrentUser.ID;
            obj.Status = CheckoutStatus.ChoDuyet;
            obj.ChkFeedbackID = null;
            //lấy người có quyền phê duyệt
            var u = new fwUserDAL().ListByRole(RoleList.ApproveTicket).FirstOrDefault();
            obj.Current = u.ID;
            if (!obj.CheckoutUser.Any(m => m.UserID == u.ID))
                db.Database.ExecuteSqlCommand(string.Format("insert into CheckoutUser values({0},{1})", obj.ID, u.ID));

            db.SaveChanges();
            return RedirectToAction("Index", "RequestBill");
        }
        public ActionResult Duyet(int id)
        {
            var obj = db.Checkout.FirstOrDefault(m => m.ID == id);
            if (!CanDuyet(obj)) return AccessDenied();
            obj.Track += ";#" + DB.CurrentUser.ID;
            obj.Status = CheckoutStatus.DaDuyet;
            obj.ChkFeedbackID = null;
            //lấy người có quyền kế toán
            var u = new fwUserDAL().ListByRole(RoleList.Accounting).FirstOrDefault();
            obj.Current = u.ID;
            if (!obj.CheckoutUser.Any(m => m.UserID == u.ID))
                db.Database.ExecuteSqlCommand(string.Format("insert into CheckoutUser values({0},{1})", obj.ID, u.ID));

            db.SaveChanges();

            return RedirectToAction("Index", "RequestBill");
        }

        #region Check role
        public static bool CanGuiYeuCau(Checkout obj)
        {
            return obj.Status == CheckoutStatus.KhoiTao && obj.Current == DB.CurrentUser.ID;
        }
        public static bool CanKiemDuyet(Checkout obj)
        {
            return DB.CurrentUser.ID == obj.Current && obj.Status == CheckoutStatus.ChoKiemSoat;
        }
        public static bool CanDuyet(Checkout obj)
        {
            return DB.CurrentUser.ID == obj.Current && obj.Status == CheckoutStatus.ChoDuyet && new fwUserDAL().UserInRole(DB.CurrentUser.ID, RoleList.ApproveTicket);
        }
        #endregion
    }
}
