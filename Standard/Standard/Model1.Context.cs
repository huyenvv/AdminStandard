﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DB_9CF750_dbEntities : DbContext
    {
        public DB_9CF750_dbEntities()
            : base("name=DB_9CF750_dbEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Checkout> Checkout { get; set; }
        public virtual DbSet<CheckoutDetails> CheckoutDetails { get; set; }
        public virtual DbSet<ChkFeedback> ChkFeedback { get; set; }
        public virtual DbSet<Dept> Dept { get; set; }
        public virtual DbSet<Feedback> Feedback { get; set; }
        public virtual DbSet<Ticket> Ticket { get; set; }
        public virtual DbSet<TicketDetails> TicketDetails { get; set; }
        public virtual DbSet<CheckoutUser> CheckoutUser { get; set; }
        public virtual DbSet<TicketUser> TicketUser { get; set; }
    }
}
