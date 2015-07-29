using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebLib.Models
{
    public class fwRole
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Code { get; set; }
        public List<fwGroup> fwGroup
        {
            get
            {
                return new DAL.fwGroupDAL().ListByRole(ID);
            }
        }
        public void AddGroup(int groupID)
        {
            int count = (int)DataUtilities.ExcuteScalar(string.Format("select count(1) from fwRoleGroup where RoleID={0} and GroupID={1}", ID, groupID), System.Data.CommandType.Text);
            if (count == 0)
            {
                DataUtilities.ExcuteNonQuery(string.Format("insert into fwRoleGroup(RoleID, GroupID) values({0},{1})", ID, groupID), System.Data.CommandType.Text);
            }
        }
        public void RemoveRole(int groupID)
        {
            DataUtilities.ExcuteNonQuery(string.Format("delete from fwRoleGroup where RoleID={0} and GroupID={1}", ID, groupID), System.Data.CommandType.Text);
        }
    }
}
