

namespace CIN.Domain.InventorySetup
{
    using CIN.Domain.SystemSetup;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    [Table("tblInvDefType")]
    public class TblErpInvItemtype : AutoActiveGenerateIdAuditableKey<int>
    {
        [StringLength(10)]
        [Key]
        public string TypeCode { get; set; }
       
        [StringLength(50)]
        [Required]
        public string TypeName { get; set; }
    }
}
