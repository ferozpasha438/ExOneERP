using CIN.Domain.OpeartionsMgt;
using CIN.Domain.SystemSetup;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIN.Domain.GeneralLedger.CashVoucher
{


    [Table("tblFinTrnCashVoucherApproval")]
    [Index(nameof(TranNumber), Name = "IX_tblFinTrnCashVoucherApproval_TranNumber", IsUnique = false)]
    public class TblFinTrnCashVoucherApproval : PrimaryKey<int>
    {
        [ForeignKey(nameof(CashVoucherId))]
        public TblFinTrnCashVoucher Invoice { get; set; }
        public int? CashVoucherId { get; set; }

        [ForeignKey(nameof(CompanyId))]
        public TblErpSysCompany SysCompany { get; set; }
        public int CompanyId { get; set; }

        [ForeignKey(nameof(BranchCode))]
        public TblErpSysCompanyBranch SysCompanyBranch { get; set; }
        public string BranchCode { get; set; }
        public DateTime? JvDate { get; set; }
        [Required]
        [StringLength(2)]
        public string TranSource { get; set; }
        [StringLength(50)]
        public string TranNumber { get; set; }
        [Required]
        [StringLength(10)]
        public string Trantype { get; set; }

        [Required]
        [StringLength(50)]
        public string DocNum { get; set; }

        [ForeignKey(nameof(LoginId))]
        public TblErpSysLogin SysLogin { get; set; }
        public int LoginId { get; set; }
        [StringLength(50)]
        public string AppRemarks { get; set; }
        public bool IsApproved { get; set; }
    }
}
