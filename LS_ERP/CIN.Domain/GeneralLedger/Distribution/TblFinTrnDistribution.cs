using CIN.Domain.FinanceMgt;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIN.Domain.GeneralLedger.Distribution
{
    [Table("tblFinTrnDistribution")]
    public class TblFinTrnDistribution : PrimaryKey<int>
    {
        public long? InvoiceId { get; set; }

        [ForeignKey(nameof(FinAcCode))]
        public TblFinDefMainAccounts FinDefMainAccounts { get; set; }
        [StringLength(50)]
        public string FinAcCode { get; set; }

        [StringLength(20)]
        public string Type { get; set; }

        [StringLength(10)]
        public string Source { get; set; }

        [StringLength(6)]
        public string Gl { get; set; }

        [Column(TypeName = "decimal(18, 3)")]
        public decimal? DrAmount { get; set; }

        [Column(TypeName = "decimal(18, 3)")]
        public decimal? CrAmount { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
