using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Standard.Repository;

namespace Standard.Controllers
{
    [CustomAuthorize(RoleList.SystemManager)]
    public class DeptController : BaseController
    {
        private readonly DeptRepository _deptRepository;
        public DeptController()
        {
            _deptRepository = new DeptRepository();
        }
        public ActionResult Index()
        {
            return View(_deptRepository.GetAll());
        }
        public ActionResult Edit(int? id, string returnUrl)
        {
            Dept dept = new Dept();
            if (id != null)
                dept = _deptRepository.GetById(id.Value);
            return View(dept);
        }
        [HttpPost]
        public ActionResult Edit(Dept model, string returnUrl)
        {
            if (model.ID == 0)
            {
                _deptRepository.Insert(model);
            }
            else
                _deptRepository.Update(model);
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
            var obj = _deptRepository.GetById(id);
            _deptRepository.Delete(obj);
            return RedirectToAction("Index");
        }
    }
}