namespace CIN.Domain.PurchaseSetup
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    [Table("tblPopDefVendorCategory")]
    public class TblPopDefVendorCategory : AutoActiveGenerateIdAuditableKey<int>
    {
        [StringLength(20)]
        [Key]
        public string VenCatCode { get; set; }
        [StringLength(50)]
        public string VenCatName { get; set; }
        [StringLength(50)]
        public string VenCatDesc { get; set; }
        [StringLength(3)]
        public string CatPrefix { get; set; }
        public int LastSeq { get; set; }
    }
}
