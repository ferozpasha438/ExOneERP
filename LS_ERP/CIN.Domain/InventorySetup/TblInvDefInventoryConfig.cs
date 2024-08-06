namespace CIN.Domain.InventorySetup
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    [Table("tblInvDefInventoryConfig")]
    public class TblInvDefInventoryConfig : AutoGenerateIdAuditableKey<int>
    {
        [ForeignKey(nameof(CentralWHCode))]
        public TblInvDefWarehouse InvWarehouse { get; set; }
        [StringLength(10)]
        public string CentralWHCode { get; set; }
        public bool AutoGenItemCode { get; set; }
        public bool PrefixCatCode { get; set; }

        [StringLength(10)]
        public string NewItemIndicator { get; set; }

        public sbyte ItemLength { get; set; }
        public sbyte CategoryLength { get; set; }
       

    }
}
