using CIN.Domain.FinanceMgt;
using CIN.Domain.InventorySetup;
using CIN.Domain.SalesSetup;
using CIN.Domain.SystemSetup;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIN.Domain.InvoiceSetup
{
    [Table("tblSndTranInvoicePaymentSettlements")]
    public class TblSndTranInvoicePaymentSettlements : PrimaryKey<long>
    {
        [ForeignKey(nameof(InvoiceId))]
        public TblSndTranInvoice TblSndTranInvoice { get; set; }
        public long InvoiceId { get; set; }

        [ForeignKey(nameof(PaymentCode))]
        public TblFinDefAccountlPaycodes TblFinDefAccountlPaycodes { get; set; }
        public string PaymentCode { get; set; }

        public decimal? SettledAmount { get; set; }
        [Column(TypeName = "date")]
        public DateTime? SettledDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime? DueDate { get; set; }
        public long? SettledBy { get; set; }
        public bool? IsPaid { get; set; }

        public string Remarks { get; set; }


    }
}