using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    }
}
