using CIN.Domain.FinanceMgt;
using CIN.Domain.GeneralLedger;
using CIN.Domain.OpeartionsMgt;
using CIN.Domain.SystemSetup;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIN.Domain.InvoiceSetup
{
    [Table("tblFinTrnBankReconciliationItem")]
    public class TblFinTrnBankReconciliationItem : PrimaryKey<int>
    {
        [ForeignKey(nameof(BankRecId))]
        public TblFinTrnBankReconciliation BankRec { get; set; }
        public int BankRecId { get; set; }

        [ForeignKey(nameof(BranchCode))]
        public TblErpSysCompanyBranch SysCompanyBranch { get; set; }
        public string BranchCode { get; set; }

        [ForeignKey(nameof(FinAcCode))]
        public TblFinDefMainAccounts FinDefMainAccounts { get; set; }
        [StringLength(50)]
        public string FinAcCode { get; set; }
        [StringLength(150)]
        public string Description { get; set; }
        [StringLength(150)]
        public string Remarks { get; set; }

        [StringLength(150)]
        public string Batch { get; set; }
        [StringLength(150)]
        public string Batch2 { get; set; }
        public int? CostAllocation { get; set; }
        [StringLength(50)]
        public string CostSegCode { get; set; }

        [Column(TypeName = "decimal(18, 3)")]
        public decimal? DrAmount { get; set; }
        [Column(TypeName = "decimal(18, 3)")]
        public decimal? CrAmount { get; set; }       
    }
}
