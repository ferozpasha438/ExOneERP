

namespace CIN.Domain.InventorySetup
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    [Table("tblInvDefUOM")]
    public class TblInvDefUOM : AutoActiveGenerateIdAuditableKey<int>
    {
        [StringLength(10)]
        [Key]
        public string UOMCode { get; set; }
        [StringLength(20)]
        [Required]
        public string UOMName { get; set; }
        [StringLength(20)]
        public string UOMDesc { get; set; }
        
    }
}
