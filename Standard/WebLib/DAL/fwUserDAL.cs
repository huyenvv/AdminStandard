using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using WebLib.Models;

namespace WebLib.DAL
{
    public class fwUserDAL : fwBaseDAL
    {
        #region DAL
        public fwUserDAL()
        {
            _TableName = "fwUser";
        }

        private string query = "select * from fwUser";

        private fwUser CreateObj(DataRow row)
        {
            var obj = new fwUser();
            obj.ID = (int)row["ID"];
            obj.AspnetUserID = row["AspnetUserID"].ToString();
            obj.UserName = (string)row["UserName"];
            obj.Name = GetString(row["Name"]);
            obj.Email = GetString(row["Email"]);
            obj.PhoneNumber = GetString(row["PhoneNumber"]);
            obj.Address = GetString(row["Address"]);
            obj.Status = (int)row["Status"];
            obj.Locked = (bool)row["Locked"];
            obj.Avata = GetString(row["Avata"]);
            obj.NotiCount = GetInt(row["NotiCount"]);
            obj.Pass = GetString(row["Pass"]);
            return obj;
        }

        public fwUser GetByID(int id)
        {
            DataTable dt = DataUtilities.GetTable(query + " where id=@id", CommandType.Text, "@id", id);
            if (dt.Rows.Count == 0) return null;

            var obj = CreateObj(dt.Rows[0]);
            return obj;
        }

        public fwUser GetByAspNetUserID(string aspNetID)
        {
            DataTable dt = DataUtilities.GetTable(query + " where AspnetUserID=@id", CommandType.Text, "@id", aspNetID);
            if (dt.Rows.Count == 0) return null;

            var obj = CreateObj(dt.Rows[0]);
            return obj;
        }

        public fwUser GetByUserName(string userName)
        {
            DataTable dt = DataUtilities.GetTable(query + " where UserName=@userName COLLATE SQL_Latin1_General_CP1_CI_AS", CommandType.Text, "@userName", userName);
            if (dt.Rows.Count == 0) return null;

            var obj = CreateObj(dt.Rows[0]);
            return obj;
        }

        public List<fwUser> ListAll()
        {
            var lst = new List<fwUser>();
            DataTable dt = DataUtilities.GetTable(query, CommandType.Text);
            foreach (DataRow row in dt.Rows)
            {
                var obj = CreateObj(row);
                lst.Add(obj);
            }

            return lst;
        }

        public List<fwUser> ListByGroup(int groupID)
        {
            string query = "select u.* from fwUser u inner join fwUserGroup ug on ug.UserID=u.ID where ug.GroupID=" + groupID;
            var lst = new List<fwUser>();
            DataTable dt = DataUtilities.GetTable(query, CommandType.Text);
            foreach (DataRow row in dt.Rows)
            {
                var obj = CreateObj(row);
                lst.Add(obj);
            }

            return lst;
        }
        public List<fwUser> ListByRole(string roleCode)
        {
            string query = @"select u.* from fwUser u 
inner join fwUserGroup ug on ug.UserID=u.ID
inner join fwRoleGroup rg on rg.GroupID=ug.GroupID
inner join fwRole r on r.ID=rg.RoleID
where r.Code='" + roleCode + "'";
            var lst = new List<fwUser>();
            DataTable dt = DataUtilities.GetTable(query, CommandType.Text);
            foreach (DataRow row in dt.Rows)
            {
                var obj = CreateObj(row);
                lst.Add(obj);
            }

            return lst;
        }
        public fwUser Insert(fwUser obj)
        {
            var ID = DataUtilities.Insert(@"insert into fwUser([AspnetUserID], [UserName], [Name], [Email], [Status], [Locked], [Avata], [NotiCount], [Pass]) values(@AspnetUserID, @UserName, @Name, @Email, @Status, @Locked, @Avata, @NotiCount, @Pass)",
                CommandType.Text, "@AspnetUserID", obj.AspnetUserID, "@UserName", obj.UserName, "@Name", obj.Name, "@Email", obj.Email, "@Status", obj.Status, "@Locked", obj.Locked, "@Avata", obj.Avata, "@NotiCount", obj.NotiCount, "@Pass", obj.Pass);
            obj.ID = ID;
            return obj;
        }

        public fwUser Update(fwUser obj)
        {
            var ID = DataUtilities.ExcuteNonQuery(@"update fwUser set [UserName]=@UserName, [Email]=@Email, [Status]=@Status, [Locked]=@Locked, [Avata]=@Avata, [NotiCount] = @NotiCount, [Pass]=@Pass where ID=@ID",
                CommandType.Text, "@UserName", obj.UserName, "@Name", obj.Name, "@Email", obj.Email, "@Status", obj.Status, "@Locked", obj.Locked, "@Avata", obj.Avata, "@NotiCount", obj.NotiCount, "@Pass", obj.Pass, "@ID", obj.ID);
            return obj;
        }
        #endregion

        #region bussiness
        public static fwUser GetCurrentUser()
        {
            if (!SessionUtilities.Exist(Constant.Session_CurrentUser))
                if (StringHelper.HttpContext.Request.Cookies.Get(Constant.Session_CurrentUser) != null)
                {
                    var id = int.Parse(StringHelper.HttpContext.Request.Cookies.Get(Constant.Session_CurrentUser).Value);
                    var u = new fwUserDAL().GetByID(id);
                    SessionUtilities.Set(Constant.Session_CurrentUser, u);
                    return u;
                }
                else return null;
            return (fwUser)SessionUtilities.Get(Constant.Session_CurrentUser);
        }
        public bool Login(string username, string pass, bool rememberMe = true)
        {
            var user = GetByUserName(username);
            if (user == null || user.Pass != pass) return false;
            if (rememberMe)
                StringHelper.HttpContext.Response.Cookies.Add(new System.Web.HttpCookie(Constant.Session_CurrentUser, user.ID.ToString()));
            SessionUtilities.Add(Constant.Session_CurrentUser, user);
            return true;
        }
        public static void Logout()
        {
            //StringHelper.HttpContext.Response.Cookies.Remove(Constant.Session_CurrentUser);
            if (StringHelper.HttpContext.Request.Cookies[Constant.Session_CurrentUser] != null)
            {
                var c = new System.Web.HttpCookie(Constant.Session_CurrentUser);
                c.Expires = DateTime.Now.AddDays(-1);
                StringHelper.HttpContext.Response.Cookies.Add(c);
            }
            SessionUtilities.Remove(Constant.Session_CurrentUser);
        }
        public int Authorize(params string[] roles)
        {
            var user = GetCurrentUser();
            if (user == null) return 1;
            if (roles == null || roles.Length == 0) return 0;
            if (!UserInRole(user.ID, roles)) return 2;
            return 0;
        }
        public bool UserInRole(params string[] roles)
        {
            var user = GetCurrentUser();
            if (user == null) return false;
            return UserInRole(user.ID, roles);
        }
        public bool UserInRole(fwUser user, params string[] roles)
        {
            if (user == null) return false;
            return UserInRole(user.ID, roles);
        }
        public bool UserInRole(int userID, params string[] roles)
        {
            var currentRoles = new fwRoleDAL().ListByUser(userID).Select(m => m.Code).ToArray();
            bool kq = false;
            foreach (var role in currentRoles)
            {
                if (roles.Contains(role))
                {
                    kq = true;
                    break;
                }
            }
            return kq;
        }
        //public bool UserInRole(string userIdentity, params string[] roles)
        //{
        //    var u = GetByAspNetUserID(userIdentity);
        //    if (u == null) return false;
        //    var currentRoles = new fwRoleDAL().ListByUser(u.ID).Select(m => m.Code).ToArray();
        //    bool kq = false;
        //    foreach (var role in currentRoles)
        //    {
        //        if (roles.Contains(role))
        //        {
        //            kq = true;
        //            break;
        //        }
        //    }
        //    return kq;
        //}
        #endregion
    }
}
