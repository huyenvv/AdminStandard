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
            obj.Status = (int)row["Status"];
            obj.Locked = (bool)row["Locked"];
            obj.Avata = GetString(row["Avata"]);
            return obj;
        }

        public fwUser GetByID(int id)
        {
            DataTable dt = DataUtilities.GetTable(query + " where id=@id", CommandType.Text, "@id", id);
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

        public fwUser Insert(fwUser obj)
        {
            var ID = DataUtilities.Insert(@"insert into fwUser([AspnetUserID], [UserName], [Email], [Status], [Locked], [Avata]) values(@AspnetUserID, @UserName, @Name, @Email, @Status, @Locked, @Avata)",
                CommandType.Text, "@AspnetUserID", obj.AspnetUserID, "@UserName", obj.UserName, "@Name", obj.Name, "@Email", obj.Email, "@Status", obj.Status, "@Locked", obj.Locked, "@Avata", obj.Avata);
            obj.ID = ID;
            return obj;
        }

        public fwUser Update(fwUser obj)
        {
            var ID = DataUtilities.ExcuteNonQuery(@"update fwUser set [UserName]=@UserName, [Email]=@Email, [Status]=@Status, [Locked]=@Locked, [Avata]=@Avata where ID=@ID",
                CommandType.Text, "@UserName", obj.UserName, "@Name", obj.Name, "@Email", obj.Email, "@Status", obj.Status, "@Locked", obj.Locked, "@Avata", obj.Avata, "@ID", obj.ID);
            return obj;
        }
    }
}
