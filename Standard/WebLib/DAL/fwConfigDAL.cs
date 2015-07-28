using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using WebLib.Models;

namespace WebLib.DAL
{
    internal class fwConfigDAL
    {
        private string query = "select * from fwConfig";

        private fwConfig CreateObj(DataRow row)
        {
            var obj = new fwConfig();
            obj.ID = (int)row["ID"];
            obj.Key = (string)row["Key"];
            obj.Title = (string)row["Title"];
            obj.Type = (string)row["Type"];
            obj.Choise = (string)row["Choise"];
            return obj;
        }

        public fwConfig GetByID(int id)
        {
            DataTable dt = DataUtilities.GetTable(query+ " where id=@id", CommandType.Text, "@id", id);
            if (dt.Rows.Count == 0) return null;

            var obj = CreateObj(dt.Rows[0]);
            return obj;
        }

        public fwConfig GetByKey(string key)
        {
            DataTable dt = DataUtilities.GetTable(query + " where [key]=@Key", CommandType.Text, "@Key", key);
            if (dt.Rows.Count == 0) return null;

            var obj = CreateObj(dt.Rows[0]);
            return obj;
        }

        public List<fwConfig> ListAll()
        {
            var lst = new List<fwConfig>();
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
            DataUtilities.Delete("fwConfig", id);
        }

        public fwConfig Insert(fwConfig obj)
        {
            var ID = DataUtilities.Insert("insert into fwConfig([Key], Title, [Type], Choise) values(@Key, @Title, @Type, @Choise)",
                CommandType.Text, "@Key", obj.Key, "@Title", obj.Title, "@Type", obj.Type, "@Choise", obj.Choise);
            obj.ID = ID;
            return obj;
        }

        public fwConfig Update(fwConfig obj)
        {
            var ID = DataUtilities.ExcuteNonQuery("update fwConfig set [Key] = @Key, Title=@Title, [Type]=@Type, Choise= @Choise where ID=@ID",
                CommandType.Text, "@Key", obj.Key, "@Title", obj.Title, "@Type", obj.Type, "@Choise", obj.Choise, "@ID", obj.ID);
            return obj;
        }
    }
}
