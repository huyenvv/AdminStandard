using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebLib.DAL;
using WebLib.Models;

namespace Standard.Repository
{
    public class TicketRepository : DB.BaseClass<Ticket>
    {
    }
    public class TicketDetailRepository : DB.BaseClass<TicketDetails>
    {
    }
    public class DeptRepository : DB.BaseClass<Dept>
    {
        public List<Dept> GetPhongBan()
        {
            return _db.Dept.Where(m => m.Type == null).ToList();
        }
        public List<Dept> GetKiemSoat()
        {
            return _db.Dept.Where(m => m.Type == 1).ToList();
        }
        public List<Dept> GetKiemSoatNB()
        {
            return _db.Dept.Where(m => m.Type == 2).ToList();
        }
    }
    public class TicketUserRepository : DB.BaseClass<TicketUser>
    {
    }
}