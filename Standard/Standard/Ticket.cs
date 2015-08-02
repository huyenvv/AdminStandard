//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Standard
{
    using System;
    using System.Collections.Generic;
    
    public partial class Ticket
    {
        public Ticket()
        {
            this.TicketDetails = new HashSet<TicketDetails>();
            this.TicketUser = new HashSet<TicketUser>();
            this.Feedback = new HashSet<Feedback>();
        }
    
        public int ID { get; set; }
        public int Type { get; set; }
        public System.DateTime Created { get; set; }
        public int CreatedBy { get; set; }
        public int DeptID { get; set; }
        public string Track { get; set; }
        public int Current { get; set; }
        public int Status { get; set; }
        public string FilePath { get; set; }
        public Nullable<int> PassedBy { get; set; }
        public Nullable<int> ApprovedBy { get; set; }
        public Nullable<int> CheckoutID { get; set; }
        public Nullable<int> FeedbackID { get; set; }
    
        public virtual ICollection<TicketDetails> TicketDetails { get; set; }
        public virtual ICollection<TicketUser> TicketUser { get; set; }
        public virtual ICollection<Feedback> Feedback { get; set; }
    }
}
