using AutoMapper;
using CIN.Application.Common;
using CIN.Domain.InventorySetup;
using CIN.Domain.InvoiceSetup;
using CIN.Domain.OpeartionsMgt;
using CIN.Domain.SystemSetup;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Application.InvoiceDtos
{
    public class OpmPartialCustomerPaymentDto
    {
        public long Id { get; set; }
        public decimal? Amount { get; set; }
        public decimal? AppliedAmount { get; set; }
    }
    public class AdvancePaymentDto
    {
        public int HasData { get; set; }
        public string AmountType { get; set; }
        public string Source { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal RemainingAdvAmount { get; set; }
        public decimal AdvAmount { get; set; }
        public decimal WriteOffAmount { get; set; }
    }
    public class TblFinTrnOpmCustomerPaymentListHeaderDto : TblFinTrnCustomerPaymentDto
    {
        public List<OpmPartialCustomerPaymentDto> InviceIds { get; set; }
        public AdvancePaymentDto AdvancePayment { get; set; }
    }

    public class OpmCustomerPaymentHeaderSummaryListDto : CommonSummaryListDto
    {
        public List<OpmCustomerPaymentHeaderSummaryDto> ListItems { get; set; }
    }
    public class OpmCustomerPaymentHeaderSummaryDto : TblFinTrnOpmCustomerPaymentHeaderDto
    {
        public string Name { get; set; }
    }

    public class OpmCustomerPaymentDetailListDto : CommonSummaryListDto
    {
        public ReportListDto<List<OpmCustomerPaymentDetailDto>> ListItems { get; set; }
    }
    public class OpmCustomerPaymentDetailDto : TblFinTrnOpmCustomerPaymentDto
    {
        public string Name { get; set; }
    }


    public class TblFinTrnOpmCustomerPaymentHeaderDto : PrimaryKeyDto<int>
    {
        public int CompanyId { get; set; }
        public string BranchCode { get; set; }
        public int PaymentNumber { get; set; }
        public DateTime? TranDate { get; set; }

        [StringLength(20)]
        public string CustCode { get; set; }

        [StringLength(10)]
        public string PayType { get; set; }

        public string PayCode { get; set; }
        [StringLength(150)]
        public string Remarks { get; set; }
        public decimal? Amount { get; set; }
        public decimal? InvoiceAmount { get; set; }
        public decimal? CrAmount { get; set; }
        public decimal? DiscountAmount { get; set; }
        [StringLength(50)]
        public string DocNum { get; set; }
        [StringLength(150)]
        public string InvoiceRefNumber { get; set; }
        [StringLength(20)]
        public string CheckNumber { get; set; }

        public DateTime? Checkdate { get; set; }
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
        public bool IsPdcCleared { get; set; }
        public DateTime? PdcClearedDate { get; set; }
        public string PdcClearedBy { get; set; }
    }

    public class TblFinTrnOpmCustomerPaymentDto : PrimaryKeyDto<int>
    {
        public string BranchCode { get; set; }
        public int PaymentId { get; set; }
        public long? InvoiceId { get; set; }
        [StringLength(150)]
        public string InvoiceNumber { get; set; }
        public string VoucherNumber { get; set; }
        public DateTime? TranDate { get; set; }
        [StringLength(20)]
        public string CustCode { get; set; }
        public decimal? Amount { get; set; }
        public decimal? CrAmount { get; set; }

        public decimal? DiscountAmount { get; set; }

        public decimal? NetAmount { get; set; }

        [StringLength(50)]
        public string DocNum { get; set; }
        [StringLength(150)]
        public string InvoiceRefNumber { get; set; }

        public DateTime? InvoiceDate { get; set; }

        public DateTime? InvoiceDueDate { get; set; }
        public bool IsPaid { get; set; }
        public bool Flag1 { get; set; }
        public bool Flag2 { get; set; }
        [StringLength(150)]
        public string Remarks { get; set; }
    }


    public class TblFinTrnOpmVendorPaymentHeaderDto : PrimaryKeyDto<int>
    {
        public int CompanyId { get; set; }

        public string BranchCode { get; set; }

        public int PaymentNumber { get; set; }
        public DateTime? TranDate { get; set; }

        [StringLength(20)]
        public string VendCode { get; set; }

        [StringLength(10)]
        public string PayType { get; set; }

        public string PayCode { get; set; }
        [StringLength(150)]
        public string Remarks { get; set; }
        public decimal? Amount { get; set; }
        public decimal? InvoiceAmount { get; set; }

        public decimal? CrAmount { get; set; }
        public decimal? DiscountAmount { get; set; }

        [StringLength(50)]
        public string DocNum { get; set; }
        [StringLength(150)]
        public string InvoiceRefNumber { get; set; }
        [StringLength(20)]
        public string CheckNumber { get; set; }

        public DateTime? Checkdate { get; set; }

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

        public bool IsPdcCleared { get; set; }
        public DateTime? PdcClearedDate { get; set; }
        public string PdcClearedBy { get; set; }
    }

    public class TblFinTrnOpmVendorPaymentDto : PrimaryKeyDto<int>
    {
        public string BranchCode { get; set; }
        public int PaymentId { get; set; }
        public long? InvoiceId { get; set; }
        [StringLength(150)]
        public string InvoiceNumber { get; set; }
        public DateTime? TranDate { get; set; }

        [StringLength(20)]
        public string VendCode { get; set; }
        public decimal? Amount { get; set; }
        public decimal? CrAmount { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? NetAmount { get; set; }

        [StringLength(50)]
        public string DocNum { get; set; }
        [StringLength(150)]
        public string InvoiceRefNumber { get; set; }

        public DateTime? InvoiceDate { get; set; }

        public DateTime? InvoiceDueDate { get; set; }
        public bool IsPaid { get; set; }
        public bool Flag1 { get; set; }
        public bool Flag2 { get; set; }
        [StringLength(150)]
        public string Remarks { get; set; }
    }

    [AutoMap(typeof(TblFinTrnAdvanceWallet))]
    public class TblFinTrnAdvanceWalletDto : AuditableCreatedEntityDto<int>
    {
        public int PaymentId { get; set; }
        public int SNDId { get; set; }
        public int SNDInvNum { get; set; }
        public DateTime? TranDate { get; set; }
        public int CompanyId { get; set; }
        public string BranchCode { get; set; }
        public string CustCode { get; set; }
        public decimal AdvAmount { get; set; } = 0;
        public decimal AppliedAmount { get; set; } = 0;
        public decimal PostedAmount { get; set; } = 0;

        [StringLength(20)]
        public string PayCode { get; set; }

        [StringLength(250)]
        public string Remarks { get; set; }
        [StringLength(250)]
        public string Notes { get; set; }
        [StringLength(50)]
        public string PreparedBy { get; set; }
        [StringLength(50)]
        public string SiteCode { get; set; }
        [StringLength(50)]
        public string DocNum { get; set; }
        [StringLength(50)]
        public string InvoiceNumber { get; set; }
        public string CustomerName { get; set; }
    }


    public class BankReconciliationStatementListDto
    {
       // public bool HasPending { get; set; }
        public bool IsSuccess { get; set; }
        public short Status { get; set; } // 1 : Customer, 2 : Vendor, 3 : Bank, 4 : Cust PDC, 5 : Vend PDC
        public string Vouchers { get; set; }
        //public List<BankReconciliationStatementDto> List { get; set; }
    }
    //public class BankReconciliationStatementDto
    //{
    //    public bool HasPending { get; set; }
    //    public string Vouchers { get; set; }
    //}
}
