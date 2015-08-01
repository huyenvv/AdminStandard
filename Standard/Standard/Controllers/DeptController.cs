using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Standard.Controllers
{
    public class DeptController : BaseController
    {
        public ActionResult Index()
        {
            return View(db.Dept.ToList());
        }
        public ActionResult Edit(int? id, string returnUrl)
        {
            Dept dept = new Dept();
            if (id != null)
                dept = db.Dept.FirstOrDefault(m => m.ID == id.Value);
            return View(dept);
        }
        [HttpPost]
        public ActionResult Edit(Dept model, string returnUrl)
        {
            if (model.ID == 0)
            {
                db.Dept.Add(model);
            }
            else
                db.Entry(model).State = System.Data.Entity.EntityState.Modified;

            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public string ListUser(int g)
        {
            var gr = new WebLib.Models.fwGroup() { ID = g };
            string s = "<select name='LeaderUserID' class='form-control'><option value=''> -:- </option>";
            foreach (var item in gr.fwUser)
            {
                s += string.Format("<option value='{0}'> {1} </option>", item.ID, item.Name);
            }
            return s + "</select>";
        }
        public ActionResult Delete(int id, string returnUrl)
        {
            var obj = db.Dept.FirstOrDefault(m => m.ID == id);
            db.Dept.Remove(obj);

            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}