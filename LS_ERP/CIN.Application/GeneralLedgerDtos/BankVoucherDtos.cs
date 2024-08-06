using AutoMapper;
using CIN.Domain.GeneralLedger.BankVoucher;
using CIN.Domain.GeneralLedger.CashVoucher;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace CIN.Application.GeneralLedgerDtos
{
    public class BvPrintDto
    {
        public string VoucherNumber { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string Logo { get; set; }
        public DateTime? CDate { get; set; }
        public string BranchName { get; set; }
        public string Batch { get; set; }
        public string User { get; set; }
        public string DocNum { get; set; }
        public string Remarks { get; set; }
        public DateTime? JvDate { get; set; }
        public string BankCode { get; set; }
        public decimal? TotalPayment { get; set; }
        public decimal? TotalDrPayment { get; set; }
        public decimal? TotalCrPayment { get; set; }
        public string TotalAmountText { get; set; }
        public string TotalCrDrAmountText { get; set; }
        public string ChequeNumber { get; set; }
        public string Source { get; set; }
        public string VoucherType { get; set; }
        public string TrnMode { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? ChequeDate { get; set; }
        public string TotalPaymentText { get; set; }
        public bool AccountPayee { get; set; }
        public bool PDC { get; set; }
        public List<TblFinTrnBankVoucherItemDto> ItemList { get; set; }

    }

    public class CreateBankVoucherDto : TblFinTrnBankVoucherDto
    {
        public List<TblFinTrnBankVoucherItemDto> ItemList { get; set; }
    }

    public class FinTrnBankVoucherListDto : TblFinTrnBankVoucherDto
    {
        public bool ApprovedUser { get; set; }
        public bool IsApproved { get; set; }
        public bool IsSettled { get; set; }
        public bool IsVoid { get; set; }
        public bool CanSettle { get; set; }
        public bool HasAuthority { get; set; }
        public bool IsDrCrAmtSame { get; set; }
    }

    public class TblFinTrnBankVoucherDto : PrimaryKeyDto<int>
    {
        [StringLength(50)]
        public string VoucherNumber { get; set; }
        public DateTime? JvDate { get; set; }
        public int CompanyId { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }

        [StringLength(50)]
        public string BankCode { get; set; }

        [StringLength(50)]
        public string TrnMode { get; set; }
        [StringLength(50)]
        public string ChequeNumber { get; set; }

        public DateTime? ChequeDate { get; set; }

        public bool AccountPayee { get; set; }
        public bool PDC { get; set; }

        [StringLength(50)]
        public string VoucherType { get; set; }

        [StringLength(50)]
        public string DocNum { get; set; }
        [StringLength(150)]
        public string Batch { get; set; }
        public decimal? Amount { get; set; }

        [StringLength(10)]
        public string Source { get; set; }

        [StringLength(150)]
        public string Remarks { get; set; }
        public string Narration { get; set; }
        public bool Approved { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public bool Posted { get; set; }
        public DateTime? PostedDate { get; set; }
        public DateTime? CDate { get; set; }
        public string SiteCode { get; set; }
    }

    [AutoMap(typeof(TblFinTrnBankVoucherItem))]
    public class TblFinTrnBankVoucherItemDto : CommonLedgerDto
    {
        public int BankVoucherId { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }

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

        public decimal? Payment { get; set; }

        public decimal? DrAmount { get; set; }
        public decimal? CrAmount { get; set; }
    }

}
