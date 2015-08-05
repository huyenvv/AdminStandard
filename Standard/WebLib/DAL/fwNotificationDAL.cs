using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using WebLib.Models;

namespace WebLib.DAL
{
    public class fwNotificationDAL : fwBaseDAL
    {
        public fwNotificationDAL()
        {
            _TableName = "fwNotification";
        }

        private fwNotification CreateObj(DataRow row)
        {
            var obj = new fwNotification();
            obj.ID = (int)row["ID"];
            obj.Title = GetString(row["Title"]);
            obj.Link = GetString(row["Link"]);
            obj.UserID = GetInt(row["UserID"]);
            obj.FullName = GetString(row["Name"]);
            obj.Read = (bool)row["Read"];
            return obj;
        }

        public fwNotification GetByID(int id)
        {
            DataTable dt = DataUtilities.GetTable(@"select Top 1 [no].*, u.Name, u.Avata from fwNotification [no]
inner join fwUser u on u.ID=[no].UserID where [no].ID=" + id, CommandType.Text);
            if (dt.Rows.Count == 0) return null;

            var obj = CreateObj(dt.Rows[0]);
            return obj;
        }

        public List<fwNotification> ListByUser(int userID)
        {
            string query = @"select Top 8 [no].*, u.Name, u.Avata from fwNotification [no]
inner join fwUser u on u.ID=[no].UserID where u.ID=" + userID + " order by [no].ID desc ";
            var lst = new List<fwNotification>();
            DataTable dt = DataUtilities.GetTable(query, CommandType.Text);
            foreach (DataRow row in dt.Rows)
            {
                var obj = CreateObj(row);
                lst.Add(obj);
            }

            return lst;
        }

        public fwNotification Insert(fwNotification obj)
        {
            var ID = DataUtilities.Insert("insert into fwNotification([Title], [Link], [UserID], [Read]) values(@Title,@Link,@UserID,@Read)",
                CommandType.Text, "@Title", obj.Title, "@Link", obj.Link, "@UserID", obj.UserID, "@Read", obj.Read);
            obj.ID = ID;
            return obj;
        }

        public void Read(int id)
        {
            var ID = DataUtilities.ExcuteNonQuery("update fwNotification set [Read] = @Read where ID = @ID",
                CommandType.Text, "@Read", true, "@ID", id);
        }
        public void RemoveCount(int UserID)
        {
            var ID = DataUtilities.ExcuteNonQuery("update fwUser set [NotiCount] = 0 where ID = @ID",
                CommandType.Text, "@ID", UserID);
        }
        public void Clear(int UserID)
        {
            var ID = DataUtilities.ExcuteNonQuery("delete from fwNotification where UserID = @ID",
                CommandType.Text, "@ID", UserID);
        }
        public int CountNew(int userID)
        {
            var xx = "select NotiCount from fwUser where ID=" + userID;
            var c = DataUtilities.ExcuteScalar(xx, CommandType.Text);
            if (c != null && c != System.DBNull.Value) return (int)c;
            return 0;
        }
    }
}
