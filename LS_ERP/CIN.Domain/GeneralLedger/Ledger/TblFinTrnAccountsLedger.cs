using CIN.Domain.OpeartionsMgt;
using CIN.Domain.SystemSetup;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIN.Domain.GeneralLedger.Ledger
{
    [Table("tblFinTrnAccountsLedger")]
    public class TblFinTrnAccountsLedger : PrimaryKey<int>
    {
        [StringLength(15)]
        public string Jvnum { get; set; }
        public int? GlId { get; set; }
        public DateTime TransDate { get; set; }
        [StringLength(2)]
        public string Source { get; set; }
        [StringLength(50)]
        public string AcCode { get; set; }
        [StringLength(150)]
        public string AcDesc { get; set; }
        [StringLength(25)]
        public string MainAcCode { get; set; }
        [ForeignKey(nameof(BranchCode))]
        public TblErpSysCompanyBranch SysCompanyBranch { get; set; }
        public string BranchCode { get; set; }
        [Column(TypeName = "decimal(18, 3)")]
        public decimal? CrAmount { get; set; }
        [Column(TypeName = "decimal(18, 3)")]
        public decimal? DrAmount { get; set; }
        [StringLength(150)]
        public string Narration { get; set; }
        [StringLength(150)]
        public string Remarks { get; set; }
        public bool PostedFlag { get; set; }
        public DateTime? PostDate { get; set; }
        [Column(TypeName = "decimal(18, 3)")]
        public decimal? ExRate { get; set; }
        public bool VoidFlag { get; set; }
        public bool ReverseFlag { get; set; }
        public bool IsApproved { get; set; }
        [StringLength(150)]
        public string Batch { get; set; }
        [StringLength(150)]
        public string Remarks2 { get; set; }

        [StringLength(50)]
        public string SiteCode { get; set; }
        public int? CostAllocation { get; set; }
        [StringLength(50)]
        public string CostSegCode { get; set; }
    }
}
