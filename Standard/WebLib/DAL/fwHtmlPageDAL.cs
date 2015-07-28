using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using WebLib.Models;

namespace WebLib.DAL
{
    internal class fwHtmlPageDAL:fwBaseDAL
    {
        public fwHtmlPageDAL()
        {
            _TableName = "fwHtmlPage";
        }
        private string query = "select * from fwHtmlPage";

        private fwHtmlPage CreateObj(DataRow row)
        {
            var obj = new fwHtmlPage();
            obj.ID = (int)row["ID"];
            obj.KeyUrl = (string)row["KeyUrl"];
            obj.Content = GetString( row["Content"]);
            return obj;
        }

        public fwHtmlPage GetByID(int id)
        {
            DataTable dt = DataUtilities.GetTable(query + " where id=@id", CommandType.Text, "@id", id);
            if (dt.Rows.Count == 0) return null;

            var obj = CreateObj(dt.Rows[0]);
            return obj;
        }

        public fwHtmlPage GetByKey(string keyUrl)
        {
            DataTable dt = DataUtilities.GetTable(query + " where [KeyUrl]=@KeyUrl", CommandType.Text, "@KeyUrl", keyUrl);
            if (dt.Rows.Count == 0) return null;

            var obj = CreateObj(dt.Rows[0]);
            return obj;
        }

        public List<fwHtmlPage> ListAll()
        {
            var lst = new List<fwHtmlPage>();
            DataTable dt = DataUtilities.GetTable(query, CommandType.Text);
            foreach (DataRow row in dt.Rows)
            {
                var obj = CreateObj(row);
                lst.Add(obj);
            }

            return lst;
        }

        public fwHtmlPage Insert(fwHtmlPage obj)
        {
            var ID = DataUtilities.Insert("insert into fwHtmlPage([KeyUrl], [Content]) values(@Key, @Content)",
                CommandType.Text, "@KeyUrl", obj.KeyUrl, "@Content", obj.Content);
            obj.ID = ID;
            return obj;
        }

        public fwHtmlPage Update(fwHtmlPage obj)
        {
            var ID = DataUtilities.ExcuteNonQuery("update fwHtmlPage set [KeyUrl] = @KeyUrl, [Content]=@Content where ID=@ID",
                CommandType.Text, "@KeyUrl", obj.KeyUrl, "@Content", obj.Content, "@ID", obj.ID);
            return obj;
        }
    }
}
