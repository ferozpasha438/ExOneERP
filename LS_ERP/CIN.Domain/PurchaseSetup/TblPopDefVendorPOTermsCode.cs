namespace CIN.Domain.PurchaseSetup
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    [Table("tblPopDefVendorPOTermsCode")]
    public class TblPopDefVendorPOTermsCode : AutoActiveGenerateIdAuditableKey<int>
    {
        [StringLength(20)]
        [Key]
        public string POTermsCode { get; set; }
        [StringLength(50)]
        public string POTermsName { get; set; }
        [StringLength(50)]
        public string POTermsDesc { get; set; }
        public sbyte POTermsDueDays { get; set; }
        public sbyte POTermDiscDays { get; set; }
        
    }
}
