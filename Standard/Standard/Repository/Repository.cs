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
        public TicketRepository(DB_9CF750_dbEntities db)
            : base(db)
        {
        }
    }
    public class TicketDetailRepository : DB.BaseClass<TicketDetails>
    {
        public TicketDetailRepository(DB_9CF750_dbEntities db)
            : base(db)
        {
        }
    }
    public class DeptRepository : DB.BaseClass<Dept>
    {
        public DeptRepository(DB_9CF750_dbEntities db)
            : base(db)
        {
        }
        public List<Dept> GetPhongBan()
        {
            return Find(m => m.Type == null).ToList();
        }
        public List<Dept> GetKiemSoat()
        {
            return Find(m => m.Type == 1).ToList();
        }
        public List<Dept> GetKiemSoatNB()
        {
            return Find(m => m.Type == 2).ToList();
        }
    }
    public class TicketUserRepository : DB.BaseClass<TicketUser>
    {
        public TicketUserRepository(DB_9CF750_dbEntities db)
            : base(db)
        {
        }
    }
}