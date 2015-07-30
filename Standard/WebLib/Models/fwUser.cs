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
        public int NotiCount { get; set; }
        public string Pass { get; set; }
        public List<fwMenu> Menu()
        {
            return new DAL.fwMenuDAL().ListByUser(ID);
        }
        public List<fwGroup> fwGroup { get { return new WebLib.DAL.fwGroupDAL().ListByUser(ID); } }
    }
}
