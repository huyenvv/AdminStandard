using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using WebLib.Models;

namespace WebLib.DAL
{
    public class fwGroupDAL : fwBaseDAL
    {
        public fwGroupDAL()
        {
            _TableName = "fwGroup";
        }
        private string query = "select * from fwGroup";

        private fwGroup CreateObj(DataRow row)
        {
            var obj = new fwGroup();
            obj.ID = (int)row["ID"];
            obj.Title = GetString(row["Title"]);
            return obj;
        }

        public fwGroup GetByID(int id)
        {
            DataTable dt = DataUtilities.GetTable(query + " where id=@id", CommandType.Text, "@id", id);
            if (dt.Rows.Count == 0) return null;

            var obj = CreateObj(dt.Rows[0]);
            return obj;
        }

        public List<fwGroup> ListByUser(int userID)
        {
            string query = "select g.* from fwGroup g inner join fwUserGroup ug on ug.GroupID=g.ID where ug.UserID=" + userID;
            var lst = new List<fwGroup>();
            DataTable dt = DataUtilities.GetTable(query, CommandType.Text);
            foreach (DataRow row in dt.Rows)
            {
                var obj = CreateObj(row);
                lst.Add(obj);
            }

            return lst;
        }
        public List<fwGroup> ListByRole(int roleID)
        {
            string query = "select g.* from fwGroup g inner join fwRoleGroup rg on rg.GroupID=g.ID where rg.RoleID=" + roleID;
            var lst = new List<fwGroup>();
            DataTable dt = DataUtilities.GetTable(query, CommandType.Text);
            foreach (DataRow row in dt.Rows)
            {
                var obj = CreateObj(row);
                lst.Add(obj);
            }

            return lst;
        }
        public List<fwGroup> ListAll()
        {
            var lst = new List<fwGroup>();
            DataTable dt = DataUtilities.GetTable(query, CommandType.Text);
            foreach (DataRow row in dt.Rows)
            {
                var obj = CreateObj(row);
                lst.Add(obj);
            }

            return lst;
        }

        public fwGroup Insert(fwGroup obj)
        {
            var ID = DataUtilities.Insert("insert into fwGroup([Title]) values(@Title)",
                CommandType.Text, "@Title", obj.Title);
            obj.ID = ID;
            return obj;
        }

        public fwGroup Update(fwGroup obj)
        {
            var ID = DataUtilities.ExcuteNonQuery("update fwGroup set [Title] = @Title where ID = @ID",
                CommandType.Text, "@Title", obj.Title, "@ID", obj.ID);
            return obj;
        }
    }
}
