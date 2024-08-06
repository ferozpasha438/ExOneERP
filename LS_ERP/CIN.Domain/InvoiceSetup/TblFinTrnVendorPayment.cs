using CIN.Domain.OpeartionsMgt;
using CIN.Domain.SystemSetup;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIN.Domain.InvoiceSetup
{
    [Table("tblFinTrnVendorPayment")]
    public class TblFinTrnVendorPayment : PrimaryKey<int>
    {

        [ForeignKey(nameof(CompanyId))]
        public TblErpSysCompany SysCompany { get; set; }
        public int CompanyId { get; set; }

        [ForeignKey(nameof(BranchCode))]
        public TblErpSysCompanyBranch SysCompanyBranch { get; set; }
        public string BranchCode { get; set; }

        public int VoucherNumber { get; set; }
        public DateTime? TranDate { get; set; }

        [ForeignKey(nameof(VendCode))]
        public TblSndDefVendorMaster SndVendorMaster { get; set; }

        [StringLength(20)]
        public string VendCode { get; set; }

        [StringLength(10)]
        public string PayType { get; set; }
        [StringLength(20)]
        public string PayCode { get; set; }
        [StringLength(150)]
        public string Remarks { get; set; }
        [Column(TypeName = "decimal(18, 3)")]
        public decimal? Amount { get; set; }
        [StringLength(50)]
        [Required]
        public string DocNum { get; set; }
        [StringLength(20)]
        public string CheckNumber { get; set; }

        public DateTime? Checkdate { get; set; }
        [Column(TypeName = "nvarchar(max)")]
        public string Narration { get; set; }
        [StringLength(50)]
        public string Preparedby { get; set; }

        public bool IsPaid { get; set; }
    }
}
