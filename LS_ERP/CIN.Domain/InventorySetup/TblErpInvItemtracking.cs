

namespace CIN.Domain.InventorySetup
{
    using CIN.Domain.SystemSetup;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    [Table("tblInvDefTracking")]
    public class TblErpInvItemtracking : AutoActiveGenerateIdAuditableKey<int>
    {
        [StringLength(10)]
        [Key]
        public string TrCode { get; set; }
        [StringLength(10)]
        public string TypeCode { get; set; }
        [StringLength(50)]
        [Required]
        public string TrName { get; set; }
  
    }
}
