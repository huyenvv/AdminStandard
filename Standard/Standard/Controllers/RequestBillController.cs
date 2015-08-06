using Newtonsoft.Json;
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
        private readonly TicketRepository _ticketRepository;
        private DB_9CF750_dbEntities db;

        public RequestBillController()
        {
            db = DB.Entities;
            _checkoutRepository = new CheckoutRepository(db);
            _deptRepository = new DeptRepository(db);
            _checkoutDetailRepository = new CheckoutDetailsRepository(db);
            _ticketRepository = new TicketRepository(db);
        }
        public ActionResult ThongKe(int? y, int? m)
        {
            var list = db.Checkout.Where(x => (y.HasValue ? x.Created.Year == y.Value : true) && (m.HasValue ? x.Created.Month == m.Value : true)).ToList();
            return View(list);
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
            if (obj.Current == DB.CurrentUser.ID && obj.Status == CheckoutStatus.DaDuyet && new fwUserDAL().UserInRole(RoleList.Accounting))
                return RedirectToAction("Create", new { CheckoutId = id });
            return View(obj);
        }

        public ActionResult Create(int CheckoutId = 0, int ticketId = 0)
        {
            ViewBag.listDept = _deptRepository.GetKiemSoatNB();
            var checkout = _checkoutRepository.GetById(CheckoutId);
            if (checkout == null)
            {
                var ticket = _ticketRepository.GetById(ticketId);
                if (ticket == null)
                {
                    ShowMessage("Không tìm thấy phiếu đề nghị dụng cụ làm việc.", false);
                    return RedirectToAction("Index");
                }

                if (ticket.Status != TicketStatus.DaDuyet)
                {
                    ShowMessage("Phiếu đề nghị dụng cụ làm việc chưa được duyệt. Bạn không thể tạo đề nghị thanh toán", false);
                    return RedirectToAction("Index");
                }
                if (ticket.CheckoutID.HasValue)
                {
                    ShowMessage("Phiếu đề nghị thanh toán chỉ được tạo một lần cho mỗi phiếu đề nghị dụng cụ làm việc.", false);
                    return RedirectToAction("Index");
                }
                return View(new Checkout { Created = DateTime.Now, PaymentMethod = PaymentMethod.ChuyenKhoan });
            }
            if (checkout.CreatedBy != fwUserDAL.GetCurrentUser().ID || checkout.Status != CheckoutStatus.KhoiTao)
                return AccessDenied();

            SessionUtilities.Set(Constant.SESSION_CheckoutDetails, checkout.CheckoutDetails.ToList());
            return View(checkout);
        }

        [HttpPost]
        public ActionResult Create(FormCollection frm, Checkout model, int ticketID)
        {
            //Kế toán sửa rồi thanh toán
            if (model.ID != 0)
            {
                var listCheckoutDetailJson = new List<CheckoutDetails>();
                try
                {
                    listCheckoutDetailJson = JsonConvert.DeserializeObject<List<CheckoutDetails>>(frm["listCheckoutDetailJson"]);
                }
                catch { }

                var obj = db.Checkout.FirstOrDefault(m => m.ID == model.ID);
                int i = 0;
                if (listCheckoutDetailJson.Count == obj.CheckoutDetails.Count)
                    foreach (var item in obj.CheckoutDetails)
                    {
                        item.DeptCode = listCheckoutDetailJson[i].DeptCode;
                        item.No = listCheckoutDetailJson[i].No;
                        item.Date = listCheckoutDetailJson[i].Date;
                    }
                db.SaveChanges();
                return ThanhToan(model.ID);
            }

            try
            {
                model.Created = DateTime.Now;
                model.CreatedBy = DB.CurrentUser.ID;
                model.SumTotal = 0;
                model.Total = 0;
                model.Track += ";#" + model.CreatedBy;
                model.Current = model.CreatedBy;
                model.Status = CheckoutStatus.KhoiTao;


                var listCheckoutDetailJson = new List<CheckoutDetails>();
                try
                {
                    listCheckoutDetailJson = JsonConvert.DeserializeObject<List<CheckoutDetails>>(frm["listCheckoutDetailJson"]);
                }
                catch { }

                var tamUng = new CheckoutDetails();
                try
                {
                    tamUng = JsonConvert.DeserializeObject<CheckoutDetails>(frm["tamUng"]);
                }
                catch { }

                var phiNganHang = new CheckoutDetails();
                try
                {
                    phiNganHang = JsonConvert.DeserializeObject<CheckoutDetails>(frm["phiNganHang"]);
                }
                catch { }
                model.AdvandPayment = tamUng.VND;
                model.BankingCharge = phiNganHang.VND;
                db.Checkout.Add(model);
                db.SaveChanges();

                var details = listCheckoutDetailJson.Select(m => new CheckoutDetails()
                    {
                        Date = m.Date,
                        DeptCode = m.DeptCode,
                        No = m.No,
                        Title = m.Title,
                        CheckoutID = model.ID,
                        VND = m.VND,
                        USD = m.USD,
                    });
                model.SumTotal = details.Sum(m => m.VND);
                model.Total = model.SumTotal - model.AdvandPayment.Value + model.BankingCharge.Value;
                db.CheckoutDetails.AddRange(details);
                //cập nhật lại ticket
                var ticket = db.Ticket.FirstOrDefault(m => m.ID == ticketID);
                ticket.CheckoutID = model.ID;

                db.SaveChanges();

                db.Database.ExecuteSqlCommand(string.Format("insert into CheckoutUser values({0},{1})", model.ID, model.Current));



                return GuiKiemTra(model.ID);
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

        #region Process Line
        public ActionResult GuiKiemTra(int id)
        {
            var obj = db.Checkout.FirstOrDefault(m => m.ID == id);
            if (!CanGuiYeuCau(obj)) return AccessDenied();
            obj.Track += ";#" + DB.CurrentUser.ID;
            obj.Status = CheckoutStatus.ChoKiemTra;
            obj.ChkFeedbackID = null;
            //lấy người có quyền kế toán
            var u = new fwUserDAL().ListByRole(RoleList.Accounting).FirstOrDefault();
            if (u == null)
            {
                ShowMessage("Hiện tại hệ thống chưa có kế toán!", false);
                RedirectToAction("Details", new { id = id });
            }
            obj.Current = u.ID;
            if (!obj.CheckoutUser.Any(m => m.UserID == u.ID))
                db.Database.ExecuteSqlCommand(string.Format("insert into CheckoutUser values({0},{1})", obj.ID, u.ID));

            db.SaveChanges();

            ShowMessage("Gửi yêu cầu thành công!");

            CreateNoti(obj.Current, "Cần kiểm tra phiếu yêu cầu <br /> thanh toán", Url.Action("Details", new { id = id }));

            return RedirectToAction("Index", "RequestBill");
        }
        public ActionResult KiemTra(int id)
        {
            var obj = db.Checkout.FirstOrDefault(m => m.ID == id);
            if (!CanKiemTra(obj)) return AccessDenied();
            obj.Track += ";#" + DB.CurrentUser.ID;
            obj.Status = CheckoutStatus.ChoDuyet;
            obj.ChkFeedbackID = null;
            //lấy người có quyền duyệt
            var u = new fwUserDAL().ListByRole(RoleList.ApproveTicket).FirstOrDefault();
            if (u == null)
            {
                ShowMessage("Hiện tại hệ thống chưa có người duyệt phiếu thanh toán!", false);
                RedirectToAction("Details", new { id = id });
            }
            obj.Current = u.ID;
            if (!obj.CheckoutUser.Any(m => m.UserID == u.ID))
                db.Database.ExecuteSqlCommand(string.Format("insert into CheckoutUser values({0},{1})", obj.ID, u.ID));

            db.SaveChanges();

            ShowMessage("Đồng ý kiểm duyệt thành công!");
            CreateNoti(obj.Current, "Cần duyệt phiếu yêu cầu thanh toán", Url.Action("Details", new { id = id }));
            CreateNoti(obj.CreatedBy, "Phiếu yêu cầu thanh toán <br /> đã được kiểm tra", Url.Action("Details", new { id = id }));

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
            if (u == null)
            {
                ShowMessage("Hiện tại hệ thống chưa có kế toán duyệt phiếu!", false);
                RedirectToAction("Details", new { id = id });
            }
            obj.Current = u.ID;
            if (!obj.CheckoutUser.Any(m => m.UserID == u.ID))
                db.Database.ExecuteSqlCommand(string.Format("insert into CheckoutUser values({0},{1})", obj.ID, u.ID));

            db.SaveChanges();

            ShowMessage("Phê duyệt thành công!");
            CreateNoti(obj.Current, "Cần xử lý phiếu yêu cầu thanh toán", Url.Action("Details", new { id = id }));
            CreateNoti(obj.CreatedBy, "Phiếu yêu cầu thanh toán <br /> đã được duyệt", Url.Action("Details", new { id = id }));

            return RedirectToAction("Index", "RequestBill");
        }
        public ActionResult ThanhToan(int id)
        {
            var obj = db.Checkout.FirstOrDefault(m => m.ID == id);
            if (!CanThanhToan(obj)) return AccessDenied();
            obj.Track += ";#" + DB.CurrentUser.ID;
            obj.Status = CheckoutStatus.HoanThanh;
            obj.ChkFeedbackID = null;
            obj.Current = obj.CreatedBy;

            db.SaveChanges();

            ShowMessage("Thanh toán hoàn tất!");
            CreateNoti(obj.CreatedBy, "Phiếu yêu cầu thanh toán đã <br /> được hoàn thành ", Url.Action("Details", new { id = id }));

            return RedirectToAction("Index", "RequestBill");
        }
        #endregion

        #region Check role
        public static bool CanGuiYeuCau(Checkout obj)
        {
            return obj.Status == CheckoutStatus.KhoiTao && obj.Current == DB.CurrentUser.ID;
        }
        public static bool CanKiemTra(Checkout obj)
        {
            return DB.CurrentUser.ID == obj.Current && obj.Status == CheckoutStatus.ChoKiemTra;
        }
        public static bool CanDuyet(Checkout obj)
        {
            return DB.CurrentUser.ID == obj.Current && obj.Status == CheckoutStatus.ChoDuyet && new fwUserDAL().UserInRole(DB.CurrentUser.ID, RoleList.ApproveTicket);
        }
        public static bool CanThanhToan(Checkout obj)
        {
            return DB.CurrentUser.ID == obj.Current && obj.Status == CheckoutStatus.DaDuyet && new fwUserDAL().UserInRole(DB.CurrentUser.ID, RoleList.Accounting);
        }
        #endregion
    }
}
