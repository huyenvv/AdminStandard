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
        //
        // GET: /RequestBill/
        public ActionResult Index()
        {
            return View(_checkoutRepository.GetAll());
        }

        //
        // GET: /RequestBill/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /RequestBill/Create
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

        //
        // POST: /RequestBill/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /RequestBill/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /RequestBill/Edit/5
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
        // GET: /RequestBill/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /RequestBill/Delete/5
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
