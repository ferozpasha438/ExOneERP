using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Domain.InvoiceSetup
{
    [Table("tblSequenceNumberSetting")]
    public class TblSequenceNumberSetting : PrimaryKey<int>
    {
        public int InvoiceSeq { get; set; }
        public int InvCredSeq { get; set; }
        public int CreditSeq { get; set; }
        public int VendCreditSeq { get; set; }
        public int VoucherSeq { get; set; }
        public int ApVoucherSeq { get; set; }
        public int JvVoucherSeq { get; set; }
        public int CvVoucherSeq { get; set; }
        public int BvVoucherSeq { get; set; }
        public int BrsVoucherSeq { get; set; } //For BankReconciliation table
        public int ArPaymentNumber { get; set; }
        public int ApPaymentNumber { get; set; }


        public int PONumber { get; set; }
        public int PRNumber { get; set; }
        public int GRNNumber { get; set; }
        public int SDQuoteNumber { get; set; }
        public int SDInvoiceNumber { get; set; }
        public int SDOrderNumber { get; set; }
        public int SDDeliveryNumber { get; set; }
        public int SDInvRetNumber { get; set; }
        [StringLength(20)]
        public string  BranchCode { get; set; }
    }
}
