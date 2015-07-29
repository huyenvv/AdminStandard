using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using WebLib.Models;

namespace WebLib.DAL
{
    public class fwMenuDAL : fwBaseDAL
    {
        public fwMenuDAL()
        {
            _TableName = "fwMenu";
        }
        private string query = "select * from fwMenu order by [Order]";

        private fwMenu CreateObj(DataRow row)
        {
            var obj = new fwMenu();
            obj.ID = (int)row["ID"];
            obj.Title = (string)row["Title"];
            obj.ParentID = GetInt(row["ParentID"]);
            obj.Url = GetString(row["Url"]);
            obj.Icon = GetString(row["Icon"]);
            obj.Order = GetInt(row["Order"]);
            obj.Actived = GetBool(row["Actived"]);
            return obj;
        }

        public fwMenu GetByID(int id)
        {
            DataTable dt = DataUtilities.GetTable("select * from fwMenu where id=@id", CommandType.Text, "@id", id);
            if (dt.Rows.Count == 0) return null;

            var obj = CreateObj(dt.Rows[0]);
            return obj;
        }

        public List<fwMenu> ListAll()
        {
            var lst = new List<fwMenu>();
            DataTable dt = DataUtilities.GetTable(query, CommandType.Text);
            foreach (DataRow row in dt.Rows)
            {
                var obj = CreateObj(row);
                lst.Add(obj);
            }

            return lst;
        }

        public List<fwMenu> ListByUser(int userID)
        {
            string query = @"select m.* from fwMenu m
inner join fwMenuRole mr on mr.MenuID=m.ID
inner join fwRoleGroup rg on rg.RoleID=mr.RoleID
inner join fwUserGroup ug on ug.GroupID=rg.GroupID
where ug.UserID=@UserID";
            var lst = new List<fwMenu>();
            DataTable dt = DataUtilities.GetTable(query, CommandType.Text, "@UserID", userID);
            foreach (DataRow row in dt.Rows)
            {
                var obj = CreateObj(row);
                lst.Add(obj);
            }

            return lst;
        }
        public List<fwMenu> ListByGroup(int groupID)
        {
            string query = @"select m.* from fwMenu m
inner join fwMenuRole mr on mr.MenuID=m.ID
inner join fwRoleGroup rg on rg.RoleID=mr.RoleID
where rg.GroupID=@GroupID";
            var lst = new List<fwMenu>();
            DataTable dt = DataUtilities.GetTable(query, CommandType.Text, "@GroupID", groupID);
            foreach (DataRow row in dt.Rows)
            {
                var obj = CreateObj(row);
                lst.Add(obj);
            }

            return lst;
        }
        public fwMenu Insert(fwMenu obj)
        {
            var ID = DataUtilities.Insert(@"insert into fwMenu([Title], [ParentID], [Url], [Icon], [Order], [Actived]) values(@Title, @ParentID, @Url, @Icon, @Order, @Actived)",
                CommandType.Text, "@Title", obj.Title, "@ParentID", obj.ParentID, "@Url", obj.Url, "@Icon", obj.Icon, "@Order", obj.Order, "@Actived", obj.Actived);
            obj.ID = ID;
            return obj;
        }

        public fwMenu Update(fwMenu obj)
        {
            var ID = DataUtilities.ExcuteNonQuery(@"update fwMenu set [Title] = @Title, [ParentID]=@ParentID, [Url]=@Url, [Icon]=@Icon, [Order]=@Order, [Actived]=@Actived where ID=@ID",
                CommandType.Text, "@Title", obj.Title, "@ParentID", obj.ParentID, "@Url", obj.Url, "@Icon", obj.Icon,
                "@Order", obj.Order, "@Actived", obj.Actived, "@ID", obj.ID);
            return obj;
        }
    }
}
