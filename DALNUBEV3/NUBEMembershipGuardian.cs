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
    
    public partial class NUBEMembershipGuardian
    {
        public int Id { get; set; }
        public Nullable<int> NUBEMembershipId { get; set; }
        public string Name { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string CityName { get; set; }
        public string StateName { get; set; }
        public string PostalCode { get; set; }
        public Nullable<int> NUBEGenderId { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public string PhoneNo { get; set; }
        public string MobileNo { get; set; }
        public string EMailId { get; set; }
        public string NRIC_N { get; set; }
        public string NRIC_O { get; set; }
        public Nullable<int> NUBERelationId { get; set; }
    
        public virtual NUBEGender NUBEGender { get; set; }
        public virtual NUBEMembership NUBEMembership { get; set; }
        public virtual NUBERelation NUBERelation { get; set; }
    }
}
