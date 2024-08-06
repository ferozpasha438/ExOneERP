using CIN.Application;
using CIN.Application.PurchasemgtQuery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace LS.API.Purchase.Controllers
{
    public class PurchaseReportController : BaseController
    {
        public PurchaseReportController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }

        [HttpGet("getPurchaseOrderSummaryList")]
        public async Task<IActionResult> GetPurchaseOrderSummaryList([FromQuery] string orderType, [FromQuery] string vendCode, [FromQuery] string itemCode, [FromQuery] string type, [FromQuery] string branchCode, [FromQuery] bool isAllVendors, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            if (orderType != "grn")
            {
                if (orderType == "prt")
                {
                    var prtList = await Mediator.Send(new GetPurchaseOrderReturnSummaryList() { OrderType = orderType, IsAllVendors = isAllVendors, Type = type, ItemCode = itemCode, BranchCode = branchCode, VendCode = vendCode, DateFrom = from, DateTo = to, User = UserInfo() });
                    return Ok(prtList);
                }

                var list = await Mediator.Send(new GetPurchaseOrderSummaryList() { OrderType = orderType, IsAllVendors = isAllVendors, Type = type, ItemCode = itemCode, BranchCode = branchCode, VendCode = vendCode, DateFrom = from, DateTo = to, User = UserInfo() });
                return Ok(list);
            }

            var grnList = await Mediator.Send(new GetPurchaseOrderGrnSummaryList() { IsAllVendors = isAllVendors, Type = type, ItemCode = itemCode, BranchCode = branchCode, VendCode = vendCode, DateFrom = from, DateTo = to, User = UserInfo() });
            return Ok(grnList);
        }

        //[HttpGet("getPurchaseOrderGrnSummaryList")]
        //public async Task<IActionResult> GetPurchaseOrderGrnSummaryList([FromQuery] string vendCode, [FromQuery] string type, [FromQuery] string branchCode, [FromQuery] bool isAllVendors, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
        //{
        //    //await Task.Delay(3000);
        //    var list = await Mediator.Send(new GetPurchaseOrderGrnSummaryList() { IsAllVendors = isAllVendors, Type = type, BranchCode = branchCode, VendCode = vendCode, DateFrom = from, DateTo = to, User = UserInfo() });
        //    return Ok(list);
        //}

        [HttpGet("getVendorPOList")]
        public async Task<IActionResult> GetVendorPOList([FromQuery] string vendCode, [FromQuery] string branchCode, [FromQuery] bool isAllVendors, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            //await Task.Delay(3000);
            var list = await Mediator.Send(new GetVendorPOList() { IsAllVendors = isAllVendors, BranchCode = branchCode, VendCode = vendCode, DateFrom = from, DateTo = to, User = UserInfo() });
            return Ok(list);
        }

        [HttpGet("getpoItemAnalysisSummary")]
        public async Task<IActionResult> GetPOItemAnalysisSummary([FromQuery] int page, [FromQuery] int pageCount, [FromQuery] string vendCode, [FromQuery] string itemCode, [FromQuery] string branchCode, [FromQuery] string type, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            //await Task.Delay(3000);
            var list = await Mediator.Send(new GetPOItemAnalysisSummary() { ReportIndex = page, ReportCount = pageCount, BranchCode = branchCode, ItemCode = itemCode, VendCode = vendCode, Type = type, DateFrom = from, DateTo = to, User = UserInfo() });
            return Ok(list);
        }
    }
}
