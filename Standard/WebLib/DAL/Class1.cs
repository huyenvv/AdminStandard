//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.SqlClient;
//using System.Linq;
//using System.Text;
//using WebLib.Models;

//namespace WebLib.DAL
//{
//    internal class CardDAL
//    {
//        private string query = "select c.ID, c.CreatedCardID, c.Status, c.Serial, c.CardChar, cc.Price, cc.CreatedDate, cc.ReleaseDate, cc.ExpireDate from Card c inner join CreatedCard cc on cc.ID = c.CreatedCardID";

//        private Card CreateObj(DataRow row)
//        {
//            var obj = new Card();
//            obj.ID = (int)row["ID"];
//            obj.CreatedCardID = (int)row["CreatedCardID"];
//            obj.Status = (int)row["Status"];
//            obj.Serial = (string)row["Serial"];
//            obj.CardChar = (string)row["CardChar"];

//            obj.Price = (decimal)row["Price"];
//            obj.CreatedDate = (DateTime)row["CreatedDate"];
//            obj.ReleaseDate = (DateTime)row["ReleaseDate"];
//            obj.ExpireDate = (DateTime)row["ExpireDate"];
//            return obj;
//        }

//        public Card GetByID(int id)
//        {
//            DataTable dt = DataUtilities.GetTable(query + " where id=@id", CommandType.Text, "@id", id);
//            if (dt.Rows.Count == 0) return null;
//            var row = dt.Rows[0];

//            var obj = CreateObj(row);
//            return obj;
//        }

//        public Card GetByCardChar(string cardChar)
//        {
//            DataTable dt = DataUtilities.GetTable("select * from Card where CardChar=@CardChar", CommandType.Text, "@CardChar", cardChar);
//            if (dt.Rows.Count == 0) return null;
//            var obj = new Card();
//            var row = dt.Rows[0];

//            return GetByID((int)row["ID"]);
//        }

//        public List<Card> ListAll()
//        {
//            var lst = new List<Card>();
//            DataTable dt = DataUtilities.GetTable(query, CommandType.Text);
//            foreach (DataRow row in dt.Rows)
//            {
//                var obj = CreateObj(row);
//                lst.Add(obj);
//            }

//            return lst;
//        }

//        public List<Card> ListByCreatedCardID(int createdCardID)
//        {
//            var lst = new List<Card>();
//            DataTable dt = DataUtilities.GetTable(query + " where CreatedCardID=@CreatedCardID", CommandType.Text, "@CreatedCardID", createdCardID);
//            foreach (DataRow row in dt.Rows)
//            {
//                var obj = CreateObj(row);
//                lst.Add(obj);
//            }

//            return lst;
//        }

//        public int Delete(int id)
//        {
//            return DataUtilities.ExcuteNonQuery("delete from Card where id=@id", CommandType.Text, "@id", id);
//        }

//        public bool ChangeStatus(int id, int status)
//        {
//            try
//            {
//                DataUtilities.ExcuteNonQuery("update Card set Status = @Status where ID = @ID", CommandType.Text, "@ID", id, "@Status", status);
//                return true;
//            }
//            catch { return false; }
//        }
//    }
//}
