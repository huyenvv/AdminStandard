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
    }
}
