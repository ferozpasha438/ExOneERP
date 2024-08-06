using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIN.Domain.SalesSetup
{
    [Table("tblInvDefSalesConfig")]
    public class TblInvDefSalesConfig : PrimaryKey<int>
    {
        public bool AutoGenCustCode { get; set; }
        public bool PrefixCatCode { get; set; }
        [StringLength(10)]
        public string NewCustIndicator { get; set; }
        public short CustLength { get; set; } //4
        public short CategoryLength { get; set; } //3

    }
}
