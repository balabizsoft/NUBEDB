﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DALNUBEV3
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DBNUBEMembership_3_0_1Entities : DbContext
    {
        public DBNUBEMembership_3_0_1Entities()
            : base("name=DBNUBEMembership_3_0_1Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<CompanyDetail> CompanyDetails { get; set; }
        public virtual DbSet<Journal> Journals { get; set; }
        public virtual DbSet<JournalDetail> JournalDetails { get; set; }
        public virtual DbSet<NUBEBank> NUBEBanks { get; set; }
        public virtual DbSet<NUBEBankBranch> NUBEBankBranches { get; set; }
        public virtual DbSet<NUBEBranch> NUBEBranches { get; set; }
        public virtual DbSet<NUBEGender> NUBEGenders { get; set; }
        public virtual DbSet<NUBELevy> NUBELevies { get; set; }
        public virtual DbSet<NUBEMembership> NUBEMemberships { get; set; }
        public virtual DbSet<NUBEMemberType> NUBEMemberTypes { get; set; }
        public virtual DbSet<NUBEPersonTitle> NUBEPersonTitles { get; set; }
        public virtual DbSet<NUBERace> NUBERaces { get; set; }
        public virtual DbSet<NUBERelation> NUBERelations { get; set; }
        public virtual DbSet<NUBEStatusDetail> NUBEStatusDetails { get; set; }
        public virtual DbSet<NUBETDF> NUBETDFs { get; set; }
        public virtual DbSet<NUBEMembershipNominee> NUBEMembershipNominees { get; set; }
        public virtual DbSet<AccountGroup> AccountGroups { get; set; }
        public virtual DbSet<NUBEMembershipGuardian> NUBEMembershipGuardians { get; set; }
        public virtual DbSet<Ledger> Ledgers { get; set; }
        public virtual DbSet<NUBEResignationReason> NUBEResignationReasons { get; set; }
        public virtual DbSet<NUBEMembershipResignation> NUBEMembershipResignations { get; set; }
    }
}
