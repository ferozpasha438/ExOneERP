using CIN.Domain.OpeartionsMgt;
using CIN.Domain.SystemSetup;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIN.Domain.InvoiceSetup
{
    [Table("tblFinTrnVendorApproval")]
    [Index(nameof(TranNumber), Name = "IX_tblFinTrnVendorApproval_TranNumber", IsUnique = false)]
    public class TblFinTrnVendorApproval : PrimaryKey<int>
    {
        [ForeignKey(nameof(InvoiceId))]
        public TblTranVenInvoice Invoice { get; set; }
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
        public string TranNumber { get; set; }
        [Required]
        [StringLength(10)]
        public string Trantype { get; set; }
        [ForeignKey(nameof(VendCode))]
        public TblSndDefVendorMaster SndVendorMaster { get; set; }

        [StringLength(20)]
        public string VendCode { get; set; }
        //[Key]
        //[StringLength(20)]
        //public string AppCode { get; set; }
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
