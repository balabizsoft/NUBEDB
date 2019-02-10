//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class NUBEMembership
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NUBEMembership()
        {
            this.NUBEMembershipNominees = new HashSet<NUBEMembershipNominee>();
            this.NUBEMembershipGuardians = new HashSet<NUBEMembershipGuardian>();
            this.NUBEMembershipResignations = new HashSet<NUBEMembershipResignation>();
        }
    
        public int Id { get; set; }
        public int LedgerId { get; set; }
        public int NUBEBankBranchId { get; set; }
        public int NUBEMemberTypeId { get; set; }
        public Nullable<int> NUBEPersonTitleId { get; set; }
        public Nullable<int> NUBEGenderId { get; set; }
        public Nullable<int> NUBERaceId { get; set; }
        public int NUBEStatusId { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public Nullable<System.DateTime> DOJ { get; set; }
        public Nullable<System.DateTime> DOE { get; set; }
        public Nullable<System.DateTime> DOL { get; set; }
        public Nullable<bool> Rejoin { get; set; }
        public Nullable<bool> AI_Insurance { get; set; }
        public string AI__InsuranceNo { get; set; }
        public Nullable<bool> GE_Insurance { get; set; }
        public string GE_ContractNo { get; set; }
        public string NRIC_N { get; set; }
        public string NRIC_O { get; set; }
        public Nullable<decimal> Salary { get; set; }
        public Nullable<int> NUBELevyId { get; set; }
        public Nullable<decimal> LevyAmount { get; set; }
        public Nullable<System.DateTime> LevyPayDate { get; set; }
        public Nullable<int> NUBETDFId { get; set; }
        public Nullable<decimal> TDFAmount { get; set; }
        public Nullable<System.DateTime> TDFPayDate { get; set; }
        public string V2Code { get; set; }
    
        public virtual NUBEMemberType NUBEMemberType { get; set; }
        public virtual NUBEPersonTitle NUBEPersonTitle { get; set; }
        public virtual NUBEStatusDetail NUBEStatusDetail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NUBEMembershipNominee> NUBEMembershipNominees { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NUBEMembershipGuardian> NUBEMembershipGuardians { get; set; }
        public virtual Ledger Ledger { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NUBEMembershipResignation> NUBEMembershipResignations { get; set; }
    }
}
