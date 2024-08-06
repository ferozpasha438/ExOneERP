
namespace CIN.Domain.InventorySetup
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    [Table("tblInvDefSubClass")]
    public class TblInvDefSubClass : AutoActiveGenerateIdAuditableKey<int>
    {
        [StringLength(20)]
        [Key]

        public string ItemSubClassCode { get; set; }
        [StringLength(50)]
        public string ItemSubClassName { get; set; }
        [StringLength(50)]
        public string ItemSubClassDesce { get; set; }
      
    }
}
