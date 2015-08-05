using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebLib.DAL;
namespace WebLib.Models
{
    public class fwNotification
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Link { get; set; }
        public int UserID { get; set; }
        public string FullName { get; set; }
        public string Avata { get; set; }
        public bool Read { get; set; }
    }
}
