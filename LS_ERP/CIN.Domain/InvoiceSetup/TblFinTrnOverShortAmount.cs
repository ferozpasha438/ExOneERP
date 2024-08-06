using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIN.Domain.InvoiceSetup
{
    [Table("tblFinTrnOverShortAmount")]
    public class TblFinTrnOverShortAmount : PrimaryKey<int>
    {
        public int PaymentId { get; set; }

        [Column(TypeName = "decimal(18, 3)")]
        public decimal? Amount { get; set; }

        [StringLength(10)]
        public string Source { get; set; }

        [StringLength(5)]
        public string AmtType { get; set; }
    }
}
