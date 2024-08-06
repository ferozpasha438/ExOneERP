

namespace CIN.Domain.InventorySetup
{
    //class tblErpInvItemMaster
    //{
    //}
     using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using CIN.Domain.SystemSetup;

    [Table("tblErpInvItemMaster")]
    public class TblErpInvItemMaster : AutoActiveGenerateIdAuditableKey<int>
    {
        [StringLength(20)]
        [Key]
        public string ItemCode { get; set; }

        [StringLength(20)]
        public string HSNCode { get; set; }

        [StringLength(100)]
        public string ItemDescription { get; set; }

        [StringLength(100)]
        public string ItemDescriptionAr { get; set; }

        [StringLength(250)]
        public string ShortName { get; set; }
        [StringLength(250)]
        public string ShortNameAr { get; set; }

        [ForeignKey(nameof(ItemCat))]
        public TblInvDefCategory InvCategory { get; set; }
        [StringLength(20)]
        public string ItemCat { get; set; }


        [ForeignKey(nameof(ItemSubCat))]
        public TblInvDefSubCategory InvSubCategories { get; set; }
        [StringLength(20)]
        public string ItemSubCat { get; set; }



        [ForeignKey(nameof(ItemClass))]
        public TblInvDefClass InvClasses { get; set; }
        [StringLength(20)]
        public string ItemClass { get; set; }



        [ForeignKey(nameof(ItemSubClass))]
        public TblInvDefSubClass InvSubClasses { get; set; }
        [StringLength(20)]
        public string ItemSubClass { get; set; }


        [ForeignKey(nameof(ItemBaseUnit))]
        public TblInvDefUOM InvUoms { get; set; }
        [StringLength(10)]
        public string ItemBaseUnit { get; set; }


        [StringLength(10)]
        public string ItemAvgCost { get; set; }


        [StringLength(10)]
        public string ItemStandardCost { get; set; }


        [StringLength(20)]
        public string ItemPrimeVendor { get; set; }

        [Column(TypeName = "decimal(12,5)")]
        public decimal ItemStandardPrice1 { get; set; }

        [Column(TypeName = "decimal(12,5)")]
        public decimal ItemStandardPrice2 { get; set; }

        [Column(TypeName = "decimal(12,5)")]
        public decimal ItemStandardPrice3 { get; set; }

        [StringLength(20)]
        public string ItemType { get; set; }
        [StringLength(20)]
        public string ItemTracking { get; set; }
        [StringLength(20)]
        public string ItemWeight { get; set; }

        

        [ForeignKey(nameof(ItemTaxCode))]
        public TblErpSysSystemTax SystemTaxes { get; set; }
        [StringLength(20)]
        public string ItemTaxCode { get; set; }


        //[ForeignKey(nameof(ItemOutputTaxCode))]
        //public TblErpSysSystemTax OutputSystemTaxes { get; set; }
        //[StringLength(20)]
        //public string ItemOutputTaxCode { get; set; }

        public bool AllowPriceOverride { get; set; }

        public bool AllowDiscounts { get; set; }

        public bool AllowQuantityOverride { get; set; }

      



    }
}
