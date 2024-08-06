
namespace CIN.Domain.InventorySetup
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    [Table("tblInvDefCategory")]
    public class TblInvDefCategory : AutoActiveGenerateIdAuditableKey<int>
    {
        [StringLength(20)]
        [Key]
        public string ItemCatCode { get; set; }
        [StringLength(50)]
        [Required]
        public string ItemCatName { get; set; }

        [StringLength(50)]
        public string ItemCatDesc { get; set; }
        [StringLength(5)]
        public string ItemCatPrefix { get; set; }

        public int LastSeq { get; set; }



    }
}
