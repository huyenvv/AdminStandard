using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebLib.DAL;

namespace WebLib.Models
{
    public class fwMenu
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public int ParentID { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public int Order { get; set; }
        public bool Actived { get; set; }
        public string SubAction { get; set; }

        public List<fwRole> fwRole
        {
            get
            {
                return new fwRoleDAL().ListByMenu(ID);
            }
        }
        public void AddRole(int roleID)
        {
            int count = (int)DataUtilities.ExcuteScalar(string.Format("select top 1 count(1) from fwMenuRole where RoleID={0} and MenuID={1}", roleID, ID), System.Data.CommandType.Text);
            if (count == 0)
            {
                DataUtilities.ExcuteNonQuery(string.Format("insert into fwMenuRole(RoleID, MenuID) values({0},{1})", roleID, ID), System.Data.CommandType.Text);
            }
        }
        public void RemoveRole(int roleID)
        {
            DataUtilities.ExcuteNonQuery(string.Format("delete from fwMenuRole where RoleID={0} and MenuID={1}", roleID, ID), System.Data.CommandType.Text);
        }
    }
}
