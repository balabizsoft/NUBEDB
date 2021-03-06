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
    
    public partial class CompanyDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CompanyDetail()
        {
            this.CompanyDetail1 = new HashSet<CompanyDetail>();
            this.NUBEBanks = new HashSet<NUBEBank>();
            this.NUBEBranches = new HashSet<NUBEBranch>();
            this.NUBEGenders = new HashSet<NUBEGender>();
            this.NUBELevies = new HashSet<NUBELevy>();
            this.NUBEMemberTypes = new HashSet<NUBEMemberType>();
            this.NUBEPersonTitles = new HashSet<NUBEPersonTitle>();
            this.NUBERelations = new HashSet<NUBERelation>();
            this.NUBEStatusDetails = new HashSet<NUBEStatusDetail>();
            this.NUBETDFs = new HashSet<NUBETDF>();
            this.NUBERaces = new HashSet<NUBERace>();
            this.AccountGroups = new HashSet<AccountGroup>();
            this.NUBEResignationReasons = new HashSet<NUBEResignationReason>();
        }
    
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string CityName { get; set; }
        public string PostalCode { get; set; }
        public string TelephoneNo { get; set; }
        public string MobileNo { get; set; }
        public string EMailId { get; set; }
        public string GSTNo { get; set; }
        public string FaxNo { get; set; }
        public string WebSite { get; set; }
        public bool IsActive { get; set; }
        public Nullable<int> UnderCompanyId { get; set; }
        public string CompanyType { get; set; }
        public Nullable<int> BankId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CompanyDetail> CompanyDetail1 { get; set; }
        public virtual CompanyDetail CompanyDetail2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NUBEBank> NUBEBanks { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NUBEBranch> NUBEBranches { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NUBEGender> NUBEGenders { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NUBELevy> NUBELevies { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NUBEMemberType> NUBEMemberTypes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NUBEPersonTitle> NUBEPersonTitles { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NUBERelation> NUBERelations { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NUBEStatusDetail> NUBEStatusDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NUBETDF> NUBETDFs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NUBERace> NUBERaces { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AccountGroup> AccountGroups { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NUBEResignationReason> NUBEResignationReasons { get; set; }
    }
}
