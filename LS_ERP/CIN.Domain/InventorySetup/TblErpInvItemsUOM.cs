

namespace CIN.Domain.InventorySetup
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using CIN.Domain.SystemSetup;

    [Table("tblErpInvItemsUOM")]
    public class TblErpInvItemsUOM : AutoActiveGenerateIdAuditableKey<int>
    {

        [ForeignKey(nameof(ItemCode))]
        public TblErpInvItemMaster InvItemMaster { get; set; }
        [StringLength(20)]
        public string ItemCode { get; set; }


        public sbyte ItemUOMFlag { get; set; }

        [ForeignKey(nameof(ItemUOM))]
        public TblInvDefUOM InvUoms { get; set; }
        [StringLength(10)]
        public string ItemUOM { get; set; }



        [Column(TypeName = "decimal(12,5)")]
        [Required]
        public decimal ItemConvFactor { get; set; }


        [Column(TypeName = "decimal(12,5)")]
        [Required]
        public decimal ItemUOMPrice1 { get; set; }


        [Column(TypeName = "decimal(12,5)")]
        [Required]
        public decimal ItemUOMPrice2 { get; set; }



        [Column(TypeName = "numeric(10,3)")]
        [Required]
        public decimal ItemUOMPrice3 { get; set; }


        [Column(TypeName = "numeric(6,3)")]
        [Required]
        public decimal ItemUOMDiscPer { get; set; }


        [Column(TypeName = "numeric(10,3)")]
        [Required]
        public decimal ItemUOMPrice4 { get; set; }


        [Column(TypeName = "numeric(11,5)")]
        [Required]
        public decimal ItemAvgCost { get; set; }




        [Column(TypeName = "numeric(11,5)")]
        [Required]
        public decimal ItemLastPOCost { get; set; }




        [Column(TypeName = "numeric(11,5)")]
        [Required]
        public decimal ItemLandedCost { get; set; }
    }
}
