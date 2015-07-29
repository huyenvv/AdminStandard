using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using WebLib.Models;

namespace WebLib.DAL
{
    public class fwBaseDAL
    {
        public fwBaseDAL()
        {
        }
        public fwBaseDAL(string tableName)
        {
            _TableName = tableName;
        }
        public string _TableName;
        public string GetString(object dr)
        {
            return dr == null ? null : dr.ToString();
        }
        public int GetInt(object dr)
        {
            return dr == System.DBNull.Value ? 0 : (int)dr;
        }
        public bool GetBool(object dr)
        {
            return dr == System.DBNull.Value ? false : (bool)dr;
        }
        public void Delete(int id)
        {
            DataUtilities.Delete(_TableName, id);
        }
    }
}
