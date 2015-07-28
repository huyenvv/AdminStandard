﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using WebLib.Models;

namespace WebLib.DAL
{
    internal class fwUserDAL
    {
        private string query = "select * from fwUser";

        private fwUser CreateObj(DataRow row)
        {
            var obj = new fwUser();
            obj.ID = (int)row["ID"];
            obj.AspnetUserID = (string)row["AspnetUserID"];
            obj.UserName = (string)row["UserName"];
            obj.Name = (string)row["Name"];
            obj.Email = (string)row["Email"];
            obj.Status = (int)row["Status"];
            obj.Locked = (bool)row["Locked"];
            obj.Avata = (string)row["Avata"];
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

        public void Delete(int id)
        {
            DataUtilities.Delete("fwUser", id);
        }

        public fwUser Insert(fwUser obj)
        {
            var ID = DataUtilities.Insert(@"insert into fwUser([AspnetUserID], [UserName], [Email], [Status], [Locked], [Avata]) values(@UserName, @Name, @Email, @Status, @Locked, @Avata)",
                CommandType.Text, "@UserName", obj.UserName, "@Name", obj.Name, "@Email", obj.Email, "@Status", obj.Status, "@Locked", obj.Locked, "@Avata", obj.Avata);
            obj.ID = ID;
            return obj;
        }

        public fwUser Update(fwUser obj)
        {
            var ID = DataUtilities.ExcuteNonQuery(@"update fwUser set [AspnetUserID] = @AspnetUserID, [UserName]=@UserName, [Email]=@Email, [Status]=@Status, [Locked]=@Locked, [Avata]=@Avata where ID=@ID",
                CommandType.Text, "@UserName", obj.UserName, "@Name", obj.Name, "@Email", obj.Email, "@Status", obj.Status, "@Locked", obj.Locked, "@Avata", obj.Avata, "@ID", obj.ID);
            return obj;
        }
    }
}
