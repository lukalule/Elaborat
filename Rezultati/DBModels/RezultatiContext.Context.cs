﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Rezultati.DBModels
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class RezultatiContext : DbContext
    {
        public RezultatiContext()
            : base("name=RezultatiContext")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Igrac> Igracs { get; set; }
        public virtual DbSet<Tabela> Tabelas { get; set; }
        public virtual DbSet<Tim> Tims { get; set; }
        public virtual DbSet<Utakmica> Utakmicas { get; set; }
        public virtual DbSet<Ucinak> Ucinaks { get; set; }
    }
}
