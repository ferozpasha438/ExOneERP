using CIN.Application;
using CIN.Application.Common;
using CIN.Application.SndDtos;
using CIN.Application.SndQuery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LS.API.Sales.Controllers.SND
{
    public class SndReportsController :  BaseController
    {
        public SndReportsController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }


        [HttpGet("getSndInvoiceReport")]
        public async Task<IActionResult> GetSndInvoiceReport([FromQuery] bool? isSummary, [FromQuery] string custCode,[FromQuery] string whCode, [FromQuery] DateTime? from, [FromQuery] DateTime? to,[FromQuery] string type, [FromQuery] int pageNumber, int pageSize)
        {
            var list = await Mediator.Send(new GetSndInvoiceReportQuery() {IsSummary=isSummary,WhCode=whCode, Type=type, CustCode = custCode, DateFrom = from, DateTo = to,PageNumber=pageNumber,PageSize=pageSize, User = UserInfo() });
            return list is not null? Ok(list): BadRequest((new ApiMessageDto { Message = "Something went wrong" }));
        }

       
         [HttpGet("getSndItemSalesReport")]
        public async Task<IActionResult> GetSndItemSalesReport([FromQuery] bool? isSummary, [FromQuery] string custCode,[FromQuery] string whCode, [FromQuery] DateTime? from, [FromQuery] DateTime? to,[FromQuery] string type,[FromQuery] string itemId, [FromQuery] int pageNumber, int pageSize)
        {
            var list = await Mediator.Send(new GetSndItemSalesReportQuery() {IsSummary=isSummary,WhCode=whCode, Type=type, CustCode = custCode, DateFrom = from, DateTo = to,ItemId=itemId, PageNumber = pageNumber, PageSize = pageSize, User = UserInfo() });
            return list is not null? Ok(list): BadRequest((new ApiMessageDto { Message = "Something went wrong" }));
        }

         [HttpGet("getItemDepartmentReport")]
        public async Task<IActionResult> GetItemDepartmentReport([FromQuery] bool? isSummary, [FromQuery] string custCode,[FromQuery] string whCode, [FromQuery] DateTime? from, [FromQuery] DateTime? to,[FromQuery] string itemCategory, [FromQuery] string itemId,[FromQuery] int pageNumber, int pageSize)
        {
            var list = await Mediator.Send(new GetItemDepartmentReportQuery() {IsSummary=isSummary,WhCode=whCode, ItemCategory = itemCategory, CustCode = custCode, DateFrom = from, DateTo = to,ItemId=itemId, PageNumber = pageNumber, PageSize = pageSize, User = UserInfo() });
            return list is not null? Ok(list): BadRequest((new ApiMessageDto { Message = "Something went wrong" }));
        }

        
         [HttpGet("getCustomerSales")]
        public async Task<IActionResult> GetCustomerSales([FromQuery] bool? isSummary, [FromQuery] string custCode, [FromQuery] DateTime? from, [FromQuery] DateTime? to, [FromQuery] int pageNumber, int pageSize)
        {
            var list = await Mediator.Send(new GetSndCustomerSalesReportQuery() {IsSummary=isSummary, CustCode = custCode, DateFrom = from, DateTo = to,PageSize=pageSize,PageNumber=pageNumber, User = UserInfo() });
            return list is not null? Ok(list): BadRequest((new ApiMessageDto { Message = "Something went wrong" }));
        }

         [HttpGet("getCustomerSalesMonthlyReport")]
        public async Task<IActionResult> GetCustomerSalesMonthlyReport([FromQuery] string custCode, [FromQuery] DateTime? from, [FromQuery] DateTime? to,[FromQuery] int pageNumber, int pageSize)
        {
            var list = await Mediator.Send(new GetCustomerSalesMonthlyReportQuery() {CustCode = custCode, DateFrom = from, DateTo = to,PageNumber=pageNumber,PageSize=pageSize, User = UserInfo() });
            return list is not null? Ok(list): BadRequest((new ApiMessageDto { Message = "Something went wrong" }));
        }

          [HttpGet("getInventoryStockLedgerReport")]
        public async Task<IActionResult> GetInventoryStockLedgerReport([FromQuery] DateTime? from, [FromQuery] DateTime? to, [FromQuery] string whCode, [FromQuery] string itemId, [FromQuery] int pageNumber,int pageSize )
        {
            var list = await Mediator.Send(new GetInventoryStockLedgerReportQuery() { WhCode = whCode, DateFrom = from, DateTo = to, ItemId = itemId,PageNumber=pageNumber,PageSize=pageSize, User = UserInfo() });
            return list is not null? Ok(list): BadRequest((new ApiMessageDto { Message = "Something went wrong" }));
        }

        [HttpGet("getInventoryStockAnalysisReport")]
        public async Task<IActionResult> GetInventoryStockAnalysisReport([FromQuery] DateTime? from, [FromQuery] DateTime? to, [FromQuery] string whCode, [FromQuery] string itemId, [FromQuery] int pageNumber,int pageSize )
        {
            var list = await Mediator.Send(new GetInventoryStockAnalysisReportQuery() { WhCode = whCode, DateFrom = from, DateTo = to, ItemId = itemId,PageNumber=pageNumber,PageSize=pageSize, User = UserInfo() });
            return list is not null? Ok(list): BadRequest((new ApiMessageDto { Message = "Something went wrong" }));
        }
        [HttpGet("getInventoryTransactionsReport")]
        public async Task<IActionResult> GetInventoryTransactionsReport([FromQuery]bool? isSummary,[FromQuery]string transactionType,[FromQuery] DateTime? from, [FromQuery] DateTime? to, [FromQuery] string whCode,[FromQuery] int pageNumber,int pageSize )
        {
            var list = await Mediator.Send(new GetInventoryTransactionsReportQuery() {TransactionType=transactionType, WhCode = whCode,DateFrom = from, DateTo = to, PageNumber=pageNumber, IsSummary= isSummary, PageSize=pageSize, User = UserInfo() });
            return list is not null? Ok(list): BadRequest((new ApiMessageDto { Message = "Something went wrong" }));
        }
        [HttpGet("getInventoryStockTransactionAnalysisReport")]
        public async Task<IActionResult> GetInventoryStockTransactionAnalysisReport([FromQuery] DateTime? from, [FromQuery] DateTime? to, [FromQuery] string whCode, [FromQuery] string itemId, [FromQuery] int pageNumber, int pageSize)
        {
            var list = await Mediator.Send(new GetInventoryStockTransactionAnalysisReportQuery() { WhCode = whCode, DateFrom = from, DateTo = to, ItemId = itemId, PageNumber = pageNumber, PageSize = pageSize, User = UserInfo() });
            return list is not null ? Ok(list) : BadRequest((new ApiMessageDto { Message = "Something went wrong" }));
        }

    }
}
