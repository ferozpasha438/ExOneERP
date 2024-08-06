using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIN.Domain.FinanceMgt
{
    [Table("tblFinSysFinanialSetup")]
    public class TblFinSysFinanialSetup : PrimaryKey<int>
    {
        [Required]
        [Column(TypeName = "date")]
        public DateTime FYOpenDate { get; set; }
        [Required]
        [Column(TypeName = "date")]
        public DateTime FYClosingDate { get; set; }
        [Required]
        public short FYYear { get; set; }
        public sbyte FinAcCatLen { get; set; }
        public sbyte FinAcSubCatLen { get; set; }
        public sbyte FinAcLen { get; set; }
        public sbyte FinBranchPrefixLen { get; set; }
        [StringLength(50)]
        public string FinAcFormat { get; set; }
        public bool FinAllowNextYearTran { get; set; }
        public bool FinTranDateAsPostDate { get; set; }
        public bool FInSysGenAcCode { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ModifiedOn { get; set; }

        [StringLength(5)]
        public string PaymentMethod { get; set; }

        public short NumOfSeg { get; set; }
        public bool UserCostSeg { get; set; }

        [Column(TypeName = "decimal(18, 3)")]
        public decimal? MinCutOffShortAmt { get; set; } = 0;
        [Column(TypeName = "decimal(18, 3)")]
        public decimal? MaxCutOffOverAmr { get; set; } = 0;
        public bool? ArDistFlag { get; set; } = false;

    }
}
