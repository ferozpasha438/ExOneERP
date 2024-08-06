using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Application.GeneralLedgerDtos
{
    public class TrialBalanceReportDto
    {
        public string FinAcCode { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public decimal? DrAmount { get; set; }
        public decimal? CrAmount { get; set; }
        public decimal? Balance { get; set; }
        public decimal? DrBalance { get; set; }
        public decimal? CrBalance { get; set; }
    }

    public class TrialBalanceReportListDto
    {
        public List<TrialBalanceReportDto> List { get; set; }
        public CommonDataLedgerDto Company { get; set; }
        public decimal? TotalDrBalance { get; set; }
        public decimal? TotalCrBalance { get; set; }
        public string CompanyName { get; set; }
    }
    //public class TrialBalanceCutOffReportListDto : TrialBalanceReportListDto
    //{
    //    public decimal? TotalAftDr { get; set; }
    //    public decimal? TotalAftCr { get; set; }
    //    public decimal? TotalAftBalance { get; set; }


    //    public decimal? TotalDrSumBalance { get; set; }
    //    public decimal? TotalCrSumBalance { get; set; }
    //    public decimal? TotalSumBalance { get; set; }
    //}

    public class LedgerReportDto
    {
        public string Jvnum { get; set; }
        public DateTime TransDate { get; set; }
        public string FinAcCode { get; set; }
        public string FinAcName { get; set; }
        public DateTime? PostDate { get; set; }


        public decimal? CrAmount { get; set; }
        public decimal? DrAmount { get; set; }
        public decimal? Balance { get; set; }

        public decimal? ChangeCrAmount { get; set; }
        public decimal? ChangeDrAmount { get; set; }
        public decimal? ChangeBalance { get; set; }

        public decimal? ClosingCrAmount { get; set; }
        public decimal? ClosingDrAmount { get; set; }
        public decimal? ClosingBalance { get; set; }


    }

    public class LedgerReportListDto
    {
        public List<LedgerReportDto> List { get; set; }

        public decimal? TotalDr { get; set; }
        public decimal? TotalCr { get; set; }
        public decimal? TotalDrCutOff { get; set; }
        public decimal? TotalCrCutOff { get; set; }
        public decimal? TotalDrClosing { get; set; }
        public decimal? TotalCrClosing { get; set; }


        public decimal? TotalDrBalance { get; set; }
        public decimal? TotalCrBalance { get; set; }
        public decimal? TotalBalance { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public CommonDataLedgerDto Company { get; set; }
    }

    public class ViewLedgerReportListDto : LedgerReportDto
    {

        public string Source { get; set; }
        public string Remarks { get; set; }
        public string Narration { get; set; }
        public bool IsOpening { get; set; }
    }

    public class ViewLedgerReportItemListDto : CommonBalanceDto
    {
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string Logo { get; set; }
        public CommonDataLedgerDto Company { get; set; }
        public List<ViewLedgerReportListDto> List { get; set; }


    }


    public class AccountVoucherPrintListDto
    {
        public CommonDataLedgerDto Company { get; set; }
        public List<AccountVoucherPrintDto> List { get; set; }

    }
    public class AccountVoucherPrintDto
    {
        public string BranchCode { get; set; }
        public string BranchName { get; set; }

        public int VoucherNumber { get; set; }
        public DateTime? TranDate { get; set; }


        public string CustCode { get; set; }

        public string PayType { get; set; }

        public string PayCode { get; set; }

        public string Remarks { get; set; }
        public decimal? Amount { get; set; }

        public string DocNum { get; set; }

        public string CheckNumber { get; set; }

        public DateTime? Checkdate { get; set; }

        public string Narration { get; set; }

        public string Preparedby { get; set; }

        public bool IsPaid { get; set; }
    }


    public class LedgerBranchViewListDto
    {
        public string BranchName { get; set; }
        public List<CustomSelectListItem> List { get; set; }
    }



}
