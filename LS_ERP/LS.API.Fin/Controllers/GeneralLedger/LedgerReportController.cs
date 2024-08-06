using CIN.Application;
using CIN.Application.Common;
using CIN.Application.GeneralLedgerDtos;
using CIN.Application.GeneralLedgerQuery;
using CIN.Application.InvoiceDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace LS.API.Fin.Controllers.GeneralLedger
{
    public class LedgerReportController : BaseController
    {
        public LedgerReportController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }

        [HttpGet("trialBalanceReport")]
        public async Task<IActionResult> TrialBalanceReport([FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            var list = await Mediator.Send(new TrialBalanceReportList() { DateFrom = from, DateTo = to, User = UserInfo() });
            return Ok(list);
        }

        [HttpGet("trialBalanceCutOffReport")]
        public async Task<IActionResult> TrialBalanceCutOffReport([FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            var list = await Mediator.Send(new TrialBalanceCutOffReportList() { DateFrom = from, DateTo = to, User = UserInfo() });
            return Ok(list);
        }

        [HttpGet("ledgerReport")]
        public async Task<IActionResult> LedgerReport([FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            var list = await Mediator.Send(new LedgerAnalysysReportList() { DateFrom = from, DateTo = to, User = UserInfo() });
            return Ok(list);
        }

        [HttpGet("ledgerBranchViewList")]
        public async Task<IActionResult> LedgerBranchViewList()
        {
            var list = await Mediator.Send(new LedgerBranchViewList() { User = UserInfo() });
            return Ok(list);
        }

        [HttpGet("viewLedgerReport")]
        public async Task<IActionResult> ViewLedgerReportList([FromQuery] string finAcCode, [FromQuery] string branchCode, [FromQuery] DateTime? from, [FromQuery] DateTime? to, [FromQuery] string seg1, [FromQuery] string seg2, [FromQuery] string costAllocation, [FromQuery] string costSegCode, [FromQuery] string batch)
        {
            //await Task.Delay(2000);
            var list = await Mediator.Send(new ViewLedgerReportList() { FinAcCode = finAcCode, BranchCode = branchCode, Seg1 = seg1, Seg2 = seg2, CostAllocation = costAllocation, CostSegCode = costSegCode, Batch = batch, From = from, To = to, User = UserInfo() });
            return Ok(list);
        }

        [HttpGet("accountVoucherPrint")]
        public async Task<IActionResult> AccountVoucherPrint([FromQuery] string voucherType, [FromQuery] string narration, [FromQuery] string docNum, [FromQuery] string branchCode, [FromQuery] string remarks, [FromQuery] string transactionType, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            var list = await Mediator.Send(new AccountVoucherPrint() { VoucherType = voucherType, Narration = narration, DocNum = docNum, BranchCode = branchCode, Remarks = remarks, From = from, To = to, User = UserInfo() });
            return Ok(list);
        }

        //[HttpGet("accountVoucherPrint")]
        //public async Task<IActionResult> AccountVoucherPrint([FromQuery] string voucherType, [FromQuery] string narration, [FromQuery] string batch, [FromQuery] string remarks, [FromQuery] string branchCode, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
        //{
        //    var list = await Mediator.Send(new AccountVoucherPrint()
        //    {
        //        VoucherType = voucherType,
        //        Narration = narration,
        //        Batch = batch,
        //        Remarks = remarks,
        //        BranchCode = branchCode,
        //        From = from,
        //        To = to,
        //        User = UserInfo()
        //    });
        //    return Ok(list);
        //}

    }
}
