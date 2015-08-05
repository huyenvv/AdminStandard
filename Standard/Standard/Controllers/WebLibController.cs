using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebLib.DAL;
using WebLib.Models;

namespace Standard.Controllers
{
    [CustomAuthorize(RoleList.SystemManager)]
    public class WebLibController : BaseController
    {
        public ActionResult Delete(int id, string tableName, string returnUrl)
        {
            new fwBaseDAL(tableName).Delete(id);
            return Redirect(returnUrl);
        }

        #region User
        public ActionResult ListUser(int? groupID)
        {
            if (groupID != null)
            {
                return View(new fwUserDAL().ListByGroup(groupID.Value));
            }
            return View(new fwUserDAL().ListAll());
        }
        public ActionResult EditUser(int? id, string returnUrl)
        {
            var obj = id == null ? new fwUser() : new fwUserDAL().GetByID(id.Value);
            return View(obj);
        }
        [HttpPost]
        public ActionResult EditUser(fwUser model, string returnUrl)
        {
            if (model.ID == 0)
                new fwUserDAL().Insert(model);
            else
                new fwUserDAL().Update(model);
            return Redirect(returnUrl);
        }
        #endregion

        #region Group
        public ActionResult ListGroup()
        {
            return View(new fwGroupDAL().ListAll());
        }
        public ActionResult EditGroup(int? id, string returnUrl)
        {
            var obj = id == null ? new fwGroup() : new fwGroupDAL().GetByID(id.Value);
            return View(obj);
        }
        [HttpPost]
        public ActionResult EditGroup(fwGroup model, string returnUrl)
        {
            if (model.ID == 0)
                new fwGroupDAL().Insert(model);
            else
                new fwGroupDAL().Update(model);
            return Redirect(returnUrl);
        }
        public ActionResult AddUserToGroup(int groupID)
        {
            var dal = new fwUserDAL();
            ViewBag.groupName = new fwGroupDAL().GetByID(groupID).Title;
            var lstUser = dal.ListByGroup(groupID);
            var lstUserID = lstUser.Select(m => m.ID).ToList();
            ViewBag.lstUser = dal.ListAll().Where(m => !lstUserID.Contains(m.ID)).ToList();
            ViewBag.groupID = groupID;
            return View(lstUser);
        }
        [HttpPost]
        public ActionResult AddUserToGroup(int groupID, int userID)
        {
            var g = new fwGroup();
            g.ID = groupID;
            g.AddUser(userID);
            return RedirectToAction("AddUserToGroup", new { groupID = groupID });
        }
        public ActionResult RemoveFromGroup(int groupID, int userID)
        {
            var g = new fwGroup();
            g.ID = groupID;
            g.RemoveUser(userID);
            return RedirectToAction("AddUserToGroup", new { groupID = groupID });
        }
        public ActionResult AddRoleToGroup(int id)
        {
            var g = new fwGroupDAL().GetByID(id);
            ViewBag.LstRole = g.fwRole.Select(m => m.ID).ToList();
            ViewBag.groupID = id;
            ViewBag.groupTitle = g.Title;
            return View(new fwRoleDAL().ListAll());
        }
        [HttpPost]
        public ActionResult AddRoleToGroup(int groupID, string listID)
        {
            var g = new fwGroup() { ID = groupID };
            var lst = listID.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in g.fwRole.Select(m => m.ID).ToList())
            {
                if (!lst.Contains(item.ToString()))
                    g.RemoveRole(item);
            }
            foreach (var item in lst)
            {
                g.AddRole(int.Parse(item));
            }
            return RedirectToAction("ListGroup");
        }
        #endregion

        #region Menu
        public ActionResult ListMenu()
        {
            return View(new fwMenuDAL().ListAll());
        }
        public ActionResult EditMenu(int? id, string returnUrl)
        {
            var obj = id == null ? new fwMenu() : new fwMenuDAL().GetByID(id.Value);
            return View(obj);
        }
        [HttpPost]
        public ActionResult EditMenu(fwMenu model)
        {
            if (model.ID == 0)
                new fwMenuDAL().Insert(model);
            else
                new fwMenuDAL().Update(model);
            return RedirectToAction("ListMenu");
        }
        #endregion

        #region Role
        public ActionResult ListRole()
        {
            return View(new fwRoleDAL().ListAll());
        }
        #endregion

    }
}