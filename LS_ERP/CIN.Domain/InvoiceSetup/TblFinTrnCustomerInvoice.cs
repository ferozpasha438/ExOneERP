using CIN.Domain.OpeartionsMgt;
using CIN.Domain.SystemSetup;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIN.Domain.InvoiceSetup
{
    [Table("tblFinTrnCustomerInvoice")]
    [Index(nameof(InvoiceNumber), Name = "IX_tblFinTrnCustomerInvoice_InvoiceNumber", IsUnique = false)]
    public class TblFinTrnCustomerInvoice : PrimaryKey<int>
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
        [StringLength(50)]
        public string InvoiceNumber { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public short CreditDays { get; set; }
        public DateTime? DueDate { get; set; }
        [StringLength(2)]
        [Required]
        public string TranSource { get; set; }
        [StringLength(50)]
        [Required]
        public string DocNum { get; set; }
        [StringLength(10)]
        [Required]
        public string Trantype { get; set; }
        [ForeignKey(nameof(LoginId))]
        public TblErpSysLogin SysLogin { get; set; }
        public int LoginId { get; set; }
        [ForeignKey(nameof(CustCode))]
        public TblSndDefCustomerMaster SndCustomerMaster { get; set; }
        [StringLength(20)]
        public string CustCode { get; set; }
        //[Key]
        //public string CustCode { get; set; }
        [StringLength(50)]
        public string ReferenceNumber { get; set; }
        [Column(TypeName = "decimal(18, 3)")]
        [Required]
        public decimal? InvoiceAmount { get; set; }
        [Column(TypeName = "decimal(18, 3)")]
        [Required]
        public decimal? DiscountAmount { get; set; }
        [Column(TypeName = "decimal(18, 3)")]
        [Required]
        public decimal? NetAmount { get; set; }
        [Column(TypeName = "decimal(18, 3)")]
        [Required]
        public decimal? PaidAmount { get; set; }
        [Column(TypeName = "decimal(18, 3)")]
        [Required]
        public decimal? BalanceAmount { get; set; }
        [Required]
        [Column(TypeName = "decimal(18, 3)")]
        public decimal? AppliedAmount { get; set; }
        public string Remarks1 { get; set; }
        [StringLength(150)]
        public string Remarks2 { get; set; }
        public bool IsPaid { get; set; }

    }
}
