using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using WebLib.Models;

namespace WebLib.DAL
{
    public class fwRoleDAL:fwBaseDAL
    {
        public fwRoleDAL()
        {
            _TableName = "fwRole";
        }
        private string query = "select * from fwRole";

        private fwRole CreateObj(DataRow row)
        {
            var obj = new fwRole();
            obj.ID = (int)row["ID"];
            obj.Title = (string)row["Title"];
            obj.Code = (string)row["Code"];
            return obj;
        }

        public fwRole GetByID(int id)
        {
            DataTable dt = DataUtilities.GetTable(query + " where id=@id", CommandType.Text, "@id", id);
            if (dt.Rows.Count == 0) return null;

            var obj = CreateObj(dt.Rows[0]);
            return obj;
        }
        public List<fwRole> ListByGroup(int userID)
        {
            string query = "select r.* from fwRole r inner join fwRoleGroup rg on rg.RoleID=r.ID where rg.RoleID=" + userID;
            var lst = new List<fwRole>();
            DataTable dt = DataUtilities.GetTable(query, CommandType.Text);
            foreach (DataRow row in dt.Rows)
            {
                var obj = CreateObj(row);
                lst.Add(obj);
            }

            return lst;
        }
        public List<fwRole> ListAll()
        {
            var lst = new List<fwRole>();
            DataTable dt = DataUtilities.GetTable(query, CommandType.Text);
            foreach (DataRow row in dt.Rows)
            {
                var obj = CreateObj(row);
                lst.Add(obj);
            }

            return lst;
        }

        public fwRole Insert(fwRole obj)
        {
            var ID = DataUtilities.Insert("insert into fwRole([Title], [Code]) values(@Title, @Code)",
                CommandType.Text, "@Title", obj.Title, "@Code", obj.Code);
            obj.ID = ID;
            return obj;
        }

        public fwRole Update(fwRole obj)
        {
            var ID = DataUtilities.ExcuteNonQuery("update fwRole set [Title] = @Title, [Code] = @Code where ID=@ID",
                CommandType.Text, "@Title", obj.Title, "@Code", obj.Code, "@ID", obj.ID);
            return obj;
        }
    }
}
