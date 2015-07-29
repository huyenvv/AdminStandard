using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebLib.Models
{
    public class fwUser
    {
        public int ID { get; set; }
        public string AspnetUserID { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int Status { get; set; }
        public bool Locked { get; set; }
        public string Avata { get; set; }
        public List<fwMenu> Menu()
        {
            return new DAL.fwMenuDAL().ListByUser(ID);
        }
        public List<fwGroup> fwGroup { get { return new WebLib.DAL.fwGroupDAL().ListByUser(ID); } }
        public void AddGroup(int groupID)
        {
            int count = (int)DataUtilities.ExcuteScalar(string.Format("select count(1) from fwUserGroup where UserID={0} and GroupID={1}", ID, groupID), System.Data.CommandType.Text);
            if (count == 0)
            {
                DataUtilities.ExcuteNonQuery(string.Format("insert into fwUserGroup(UserID, GroupID) values({0},{1})", ID, groupID), System.Data.CommandType.Text);
            }
        }
        public void RemoveGroup(int groupID)
        {
            DataUtilities.ExcuteNonQuery(string.Format("delete from fwUserGroup where UserID={0} and GroupID={1}", ID, groupID), System.Data.CommandType.Text);
        }
    }
}
