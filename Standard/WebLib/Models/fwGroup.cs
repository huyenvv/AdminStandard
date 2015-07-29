using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebLib.DAL;
namespace WebLib.Models
{
    public class fwGroup
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public List<fwUser> fwUser
        {
            get
            {
                return new fwUserDAL().ListByGroup(ID);
            }
        }
        public void AddUser(int userID)
        {
            int count = (int)DataUtilities.ExcuteScalar(string.Format("select count(1) from fwUserGroup where UserID={0} and GroupID={1}", userID, ID), System.Data.CommandType.Text);
            if (count == 0)
            {
                DataUtilities.ExcuteNonQuery(string.Format("insert into fwUserGroup(UserID, GroupID) values({0},{1})", userID, ID), System.Data.CommandType.Text);
            }
        }
        public void RemoveUser(int userID)
        {
            DataUtilities.ExcuteNonQuery(string.Format("delete from fwUserGroup where UserID={0} and GroupID={1}", userID, ID), System.Data.CommandType.Text);
        }
        public List<fwRole> fwRole
        {
            get
            {
                return new fwRoleDAL().ListByGroup(ID);
            }
        }
        public void AddRole(int roleID)
        {
            int count = (int)DataUtilities.ExcuteScalar(string.Format("select count(1) from fwRoleGroup where RoleID={0} and GroupID={1}", roleID, ID), System.Data.CommandType.Text);
            if (count == 0)
            {
                DataUtilities.ExcuteNonQuery(string.Format("insert into fwRoleGroup(RoleID, GroupID) values({0},{1})", roleID, ID), System.Data.CommandType.Text);
            }
        }
        public void RemoveRole(int roleID)
        {
            DataUtilities.ExcuteNonQuery(string.Format("delete from fwRoleGroup where RoleID={0} and GroupID={1}", roleID, ID), System.Data.CommandType.Text);
        }
        public List<fwMenu> fwMenu
        {
            get { return new DAL.fwMenuDAL().ListByGroup(ID); }
        }
    }
}
