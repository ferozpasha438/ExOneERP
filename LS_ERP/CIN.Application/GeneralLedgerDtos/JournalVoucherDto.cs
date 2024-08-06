using AutoMapper;
using CIN.Domain.GeneralLedger;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CIN.Application.GeneralLedgerDtos
{

    public class JournalVoucherCopyDto
    {
        public int Id { get; set; }
        public bool Copy { get; set; }
    }

    public class JvPrintDto
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

        public decimal? TotalDrAmount { get; set; }
        public decimal? TotalCrAmount { get; set; }
        public decimal? Amount { get; set; }
        public string TotalAmountText { get; set; }
        public List<TblFinTrnJournalVoucherItemDto> ItemList { get; set; }

    }


    public class CreateJournalVoucherDto : TblFinTrnJournalVoucherDto
    {
        public List<TblFinTrnJournalVoucherItemDto> ItemList { get; set; }
    }

    public class FinTrnJournalVoucherListDto : TblFinTrnJournalVoucherDto
    {
        public bool ApprovedUser { get; set; }
        public bool IsApproved { get; set; }
        public bool IsSettled { get; set; }
        public bool IsVoid { get; set; }
        public bool CanSettle { get; set; }
        public bool HasAuthority { get; set; }
        public bool IsDrCrAmtSame { get; set; }
    }

    public class TblFinTrnJournalVoucherDto : PrimaryKeyDto<int>
    {
        [StringLength(50)]
        public string VoucherNumber { get; set; }
        public DateTime? JvDate { get; set; }
        public int CompanyId { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }

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

    [AutoMap(typeof(TblFinTrnJournalVoucherItem))]
    public class TblFinTrnJournalVoucherItemDto : CommonLedgerDto
    {
        public int JournalVoucherId { get; set; }
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
        public string Batch2Name { get; set; }      

        public decimal? DrAmount { get; set; }
        public decimal? CrAmount { get; set; }
        public string SiteCode { get; set; }
    }
}
