using CIN.Domain.OpeartionsMgt;
using CIN.Domain.SystemSetup;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIN.Domain.InvoiceSetup
{
    [Table("tblFinTrnOpmVendorPaymentHeader")]
    public class TblFinTrnOpmVendorPaymentHeader : PrimaryKey<int>
    {

        [ForeignKey(nameof(CompanyId))]
        public TblErpSysCompany SysCompany { get; set; }
        public int CompanyId { get; set; }

        [ForeignKey(nameof(BranchCode))]
        public TblErpSysCompanyBranch SysCompanyBranch { get; set; }
        public string BranchCode { get; set; }

        public int PaymentNumber { get; set; }
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

        [Column(TypeName = "decimal(18, 3)")]
        public decimal? InvoiceAmount { get; set; }

        [Column(TypeName = "decimal(18, 3)")]
        public decimal? CrAmount { get; set; }

        [Column(TypeName = "decimal(18, 3)")]
        public decimal? DiscountAmount { get; set; }

        [StringLength(50)]
        public string DocNum { get; set; }
        [StringLength(150)]
        public string InvoiceRefNumber { get; set; }
        [StringLength(20)]
        public string CheckNumber { get; set; }

        public DateTime? Checkdate { get; set; }
        [Column(TypeName = "nvarchar(max)")]
        public string Narration { get; set; }
        [StringLength(50)]
        public string Preparedby { get; set; }

        public bool IsPaid { get; set; }
        public bool Flag1 { get; set; }
        public bool Flag2 { get; set; }
        public bool IsPosted { get; set; }
        public DateTime? PostedDate { get; set; }
        public bool IsVoid { get; set; }
        public DateTime? VoidDate { get; set; }
        [StringLength(50)]
        public string Reason { get; set; }


        [Column(TypeName = "decimal(18, 3)")]
        public decimal? AdvAmount { get; set; } // used for bellow 1 & 2 conditions
        public short? HasRemaining { get; set; } = 0; //0 for none, 1 for still having Advance Amount, 2 for applied all the advance Amount

        public bool? HasPdcClearance { get; set; } = false;
        public bool? IsPdcCleared { get; set; } = false;
        public DateTime? PdcClearedDate { get; set; }
        [StringLength(100)]
        public string PdcClearedBy { get; set; }
    }
}
