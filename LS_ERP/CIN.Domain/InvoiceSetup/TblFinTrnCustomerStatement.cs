using CIN.Domain.OpeartionsMgt;
using CIN.Domain.SystemSetup;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIN.Domain.InvoiceSetup
{
    [Table("tblFinTrnCustomerStatement")]
    [Index(nameof(TranNumber), Name = "IX_tblFinTrnCustomerStatement_TranNumber", IsUnique = false)]
    public class TblFinTrnCustomerStatement : PrimaryKey<int>
    {
        [ForeignKey(nameof(InvoiceId))]
        public TblTranInvoice Invoice { get; set; }
        public long? InvoiceId { get; set; }

        [ForeignKey(nameof(CompanyId))]
        public TblErpSysCompany SysCompany { get; set; }
        public int CompanyId { get; set; }

        [ForeignKey(nameof(BranchCode))]
        public TblErpSysCompanyBranch SysCompanyBranch { get; set; }
        public string BranchCode { get; set; }
        public DateTime? TranDate { get; set; }
        [Required]
        [StringLength(2)]
        public string TranSource { get; set; }
        [StringLength(50)]
        [Required]
        public string TranNumber { get; set; }
        [Required]
        [StringLength(10)]
        public string Trantype { get; set; }
        [ForeignKey(nameof(CustCode))]
        public TblSndDefCustomerMaster SndCustomerMaster { get; set; }
        [StringLength(20)]
        public string CustCode { get; set; }
        //[Key]
        //[StringLength(20)]
        //public string CustCode { get; set; }
        [StringLength(50)]
        [Required]
        public string DocNum { get; set; }
        [StringLength(50)]
        public string ReferenceNumber { get; set; }

        [StringLength(10)]
        public string PaymentType { get; set; }
        [StringLength(20)]
        public string PamentCode { get; set; }
        [StringLength(10)]
        public string CheckNumber { get; set; }
        [StringLength(150)]
        public string Remarks1 { get; set; }
        [StringLength(150)]
        public string Remarks2 { get; set; }
        [ForeignKey(nameof(LoginId))]
        public TblErpSysLogin SysLogin { get; set; }
        public int LoginId { get; set; }
        [Column(TypeName = "decimal(18, 3)")]
        public decimal? DrAmount { get; set; }
        [Column(TypeName = "decimal(18, 3)")]
        public decimal? CrAmount { get; set; }

        public int? PaymentId { get; set; }

        [StringLength(50)]
        public string SiteCode { get; set; }
    }
}
