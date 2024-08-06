

namespace CIN.Domain.InventorySetup
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using CIN.Domain.SystemSetup;

    [Table("tblErpInvItemsBarcode")]
    public class TblErpInvItemsBarcode : AutoActiveGenerateIdAuditableKey<int>
    {

        [ForeignKey(nameof(ItemCode))]
        public TblErpInvItemMaster InvItemMaster { get; set; }
        [StringLength(20)]
        public string ItemCode { get; set; }

        
        [Required]
        public sbyte ItemUOMFlag { get; set; }


        [StringLength(25)]
        [Required]
        public string ItemBarcode { get; set; }

        [ForeignKey(nameof(ItemUOM))]
        public TblInvDefUOM InvUoms { get; set; }
        [StringLength(10)]
        public string ItemUOM { get; set; }

    }
}
