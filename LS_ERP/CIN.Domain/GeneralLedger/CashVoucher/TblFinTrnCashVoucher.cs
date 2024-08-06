using CIN.Domain.OpeartionsMgt;
using CIN.Domain.SystemSetup;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIN.Domain.GeneralLedger.CashVoucher
{
    [Table("tblFinTrnCashVoucher")]
    [Index(nameof(VoucherNumber), Name = "IX_tblFinTrnCashVoucher_VoucherNumber", IsUnique = false)]
    public class TblFinTrnCashVoucher : PrimaryKey<int>
    {
        [StringLength(50)]
        public string SpVoucherNumber { get; set; }

        [StringLength(50)]
        public string VoucherNumber { get; set; }
        public DateTime? JvDate { get; set; }

        [ForeignKey(nameof(CompanyId))]
        public TblErpSysCompany SysCompany { get; set; }
        public int CompanyId { get; set; }

        [ForeignKey(nameof(BranchCode))]
        public TblErpSysCompanyBranch SysCompanyBranch { get; set; }
        public string BranchCode { get; set; }

        [StringLength(100)]
        public string CBookCode { get; set; } 

        [StringLength(50)]
        public string VoucherType { get; set; } 

        [StringLength(50)]
        public string DocNum { get; set; }
        [StringLength(150)]
        public string Batch { get; set; }
        [Column(TypeName = "decimal(18, 3)")]
        public decimal? Amount { get; set; }

        [StringLength(10)]
        public string Source { get; set; }

        [StringLength(150)]
        public string Remarks { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string Narration { get; set; }
        public bool Approved { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public bool Posted { get; set; }
        public DateTime? PostedDate { get; set; }
        public bool Void { get; set; }
        public DateTime? CDate { get; set; }
        [StringLength(50)]
        public string SiteCode { get; set; }
    }
}
