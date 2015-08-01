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
    
    public partial class Checkout
    {
        public Checkout()
        {
            this.CheckoutDetails = new HashSet<CheckoutDetails>();
            this.ChkFeedback = new HashSet<ChkFeedback>();
            this.CheckoutUser = new HashSet<CheckoutUser>();
        }
    
        public int ID { get; set; }
        public System.DateTime Created { get; set; }
        public int CreatedBy { get; set; }
        public string No { get; set; }
        public string Ben { get; set; }
        public int DeptID { get; set; }
        public Nullable<int> PaymentMethod { get; set; }
        public System.DateTime PaymentDate { get; set; }
        public decimal SumTotal { get; set; }
        public Nullable<decimal> AdvandPayment { get; set; }
        public Nullable<decimal> BankingCharge { get; set; }
        public decimal Total { get; set; }
        public string InWords { get; set; }
        public Nullable<int> OnExpenses { get; set; }
        public string Track { get; set; }
        public int Current { get; set; }
        public Nullable<int> Director { get; set; }
        public Nullable<int> InternalControl { get; set; }
        public Nullable<int> ChkFeedbackID { get; set; }
        public int Status { get; set; }
    
        public virtual ICollection<CheckoutDetails> CheckoutDetails { get; set; }
        public virtual ICollection<ChkFeedback> ChkFeedback { get; set; }
        public virtual ICollection<CheckoutUser> CheckoutUser { get; set; }
    }
}
