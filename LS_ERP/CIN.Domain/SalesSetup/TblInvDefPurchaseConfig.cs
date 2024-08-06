namespace CIN.Domain.PurchaseSetup
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    [Table("tblInvDefPurchaseConfig")]
    public class TblInvDefPurchaseConfig : PrimaryKey<int>
    {
        public bool AutoGenCustCode { get; set; }
        public bool PrefixCatCode { get; set; }
        [StringLength(10)]
        public string NewCustIndicator { get; set; }
        public short VendLength { get; set; } //4
        public short CategoryLength { get; set; } //3

    }

}
