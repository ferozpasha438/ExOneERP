namespace CIN.Domain.InventorySetup
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    [Table("tblInvDefClass")]
    public class TblInvDefClass : AutoActiveGenerateIdAuditableKey<int>
    {
        [StringLength(20)]
        [Key]
        public string ItemClassCode { get; set; }
        [StringLength(50)]
        [Required]
        public string ItemClassName { get; set; }
        [StringLength(50)]
        public string ItemClassDesce { get; set; }
        

    }
}
