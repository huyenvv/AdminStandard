using System;
using System.Data;
using System.Data.SqlClient;

namespace WebLib
{
    public class DataUtilities
    {
        public static string strConnection;
        /// <summary>
        /// Delete row in table with id
        /// </summary>
        public static void Delete(string tableName, int id)
        {
            DataUtilities.ExcuteNonQuery("delete from " + tableName + " where id=@id", CommandType.Text, "@id", id);
        }
        /// <summary>
        /// Return the Identity of the last row has inserted to the table in current session
        /// </summary>
        /// <param name="command"></param>
        /// <param name="comType"></param>
        /// <param name="pars"></param>
        /// <returns></returns>
        public static int Insert(string command, CommandType comType, params object[] pars)
        {
            command += "; SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY];";
            var x = ExcuteScalar(command, CommandType.Text, pars);
            return (int)(decimal)x;
        }

        /// <summary>
        /// Return number of row affected
        /// </summary>
        /// <param name="command"></param>
        /// <param name="comType"></param>
        /// <param name="pars"></param>
        /// <returns></returns>
        public static int ExcuteNonQuery(string command, CommandType comType, params object[] pars)
        {
            SqlConnection conn = new SqlConnection(strConnection);
            SqlCommand co = new SqlCommand(command, conn);
            co.CommandType = comType;
            SqlParameter pa;

            if (pars.Length % 2 != 0) throw new Exception("Exception on parameter count");
            for (int i = 0; i < pars.Length; i += 2)
            {
                pa = new SqlParameter(pars[i].ToString(), pars[i + 1]);
                if (pars[i + 1] == null)
                    pa.Value = DBNull.Value;
                co.Parameters.Add(pa);
            }
            if (conn.State == ConnectionState.Open)
                conn.Close();
            conn.Open();
            int n = co.ExecuteNonQuery();

            co.Dispose();
            conn.Close();
            return n;
        }

        /// <summary>
        /// Return number of row affected
        /// </summary>
        /// <param name="command"></param>
        /// <param name="comType"></param>
        /// <param name="pars"></param>
        /// <returns></returns>
        public static int ExcuteNonQuery(string command, CommandType comType, SqlParameter[] pars)
        {
            SqlConnection conn = new SqlConnection(strConnection);
            SqlCommand co = new SqlCommand(command, conn);
            co.CommandType = comType;

            foreach (var pa in pars)
            {
                co.Parameters.Add(pa);
            }
            if (conn.State == ConnectionState.Open)
                conn.Close();
            conn.Open();
            int n = co.ExecuteNonQuery();
            co.Dispose();
            conn.Close();
            return n;
        }
        public static int ExcuteNonQueryOpenManual(SqlConnection con, string command, CommandType comType, SqlParameter[] pars)
        {
            SqlCommand co = new SqlCommand(command, con);
            co.CommandType = comType;

            foreach (var pa in pars)
            {
                co.Parameters.Add(pa);
            }

            //conn.Open();
            int n = co.ExecuteNonQuery();
            co.Dispose();
            //conn.Close();
            return n;
        }
        /// <summary>
        /// Return the first row and first columm. Null if no data is selected
        /// </summary>
        /// <param name="command"></param>
        /// <param name="comType"></param>
        /// <param name="pars"></param>
        /// <returns></returns>
        public static object ExcuteScalar(string command, CommandType comType, params object[] pars)
        {
            SqlConnection conn = new SqlConnection(strConnection);
            SqlCommand co = new SqlCommand(command, conn);
            co.CommandType = comType;
            SqlParameter pa;

            if (pars.Length % 2 != 0) throw new Exception("Exception on parameter count");
            for (int i = 0; i < pars.Length; i += 2)
            {
                pa = new SqlParameter(pars[i].ToString(), pars[i + 1]);
                if (pars[i + 1] == null)
                    pa.Value = DBNull.Value;
                co.Parameters.Add(pa);
            }
            if (conn.State == ConnectionState.Open)
                conn.Close();
            conn.Open();
            object n = co.ExecuteScalar();
            co.Dispose();
            conn.Close();
            return n;
        }

        /// <summary>
        /// Return the first row and first columm. Null if no date is selected
        /// </summary>
        /// <param name="command"></param>
        /// <param name="comType"></param>
        /// <param name="pars"></param>
        /// <returns></returns>
        public static object ExcuteScalar(string command, CommandType comType, SqlParameter[] pars)
        {
            SqlConnection conn = new SqlConnection(strConnection);
            SqlCommand co = new SqlCommand(command, conn);
            co.CommandType = comType;

            foreach (var pa in pars)
            {
                co.Parameters.Add(pa);
            }

            if (conn.State == ConnectionState.Open)
                conn.Close();
            conn.Open();
            object n = co.ExecuteScalar();
            co.Dispose();
            conn.Close();
            return n;
        }

        public static DataTable GetTable(string command, CommandType comType, params object[] pars)
        {
            SqlConnection conn = new SqlConnection(strConnection);
            SqlCommand co = new SqlCommand(command, conn);
            co.CommandType = comType;
            SqlParameter pa;

            if (pars.Length % 2 != 0) throw new Exception("Exception on parameter count");
            for (int i = 0; i < pars.Length; i += 2)
            {
                pa = new SqlParameter(pars[i].ToString(), pars[i + 1]);
                if (pars[i + 1] == null)
                    pa.Value = DBNull.Value;
                co.Parameters.Add(pa);
            }
            SqlDataAdapter da = new SqlDataAdapter(co);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }
    }
}