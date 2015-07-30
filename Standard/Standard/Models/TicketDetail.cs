using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Standard.Models
{
    public class TicketDetail
    {
        public int Id { get; set; }
        public string DienGiai { get; set; }
        public string SoLuong { get; set; }
        public string LyDo { get; set; }
        public string NgayCan { get; set; }
    }
}