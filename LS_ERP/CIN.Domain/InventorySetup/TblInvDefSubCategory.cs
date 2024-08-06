
namespace CIN.Domain.InventorySetup
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    [Table("tblInvDefSubCategory")]
    public class TblInvDefSubCategory : AutoActiveGenerateIdAuditableKey<int>
    {
       
        [StringLength(20)]
        [Key]
        public string ItemSubCatCode { get; set; }
        [StringLength(41)]

        public string SubCatKey { get; set; }
        [ForeignKey(nameof(ItemCatCode))]
        public TblInvDefCategory InvCategory { get; set; }
        [StringLength(20)]
        public string ItemCatCode { get; set; }
        [StringLength(50)]
        [Required]
        public string ItemSubCatName { get; set; }
        [StringLength(50)]
        public string ItemSubCatDesc { get; set; }
       
    }
}
