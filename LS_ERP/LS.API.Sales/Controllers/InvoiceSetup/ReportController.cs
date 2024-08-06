using CIN.Application;
using CIN.Application.Common;
using CIN.Application.InvoiceDtos;
using CIN.Application.InvoiceQuery;
using CIN.Application.OperationsMgtQuery;
using CIN.Application.SystemQuery;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace LS.API.Sales.Controllers.InvoiceSetup
{
    public class ReportController : FileBaseController
    {
        public ReportController(IOptions<AppSettingsJson> appSettings, IWebHostEnvironment env) : base(appSettings, env)
        {
        }

        #region AR Or Customer Reports


        [HttpGet("getCustomerBalanceSummaryList")]
        public async Task<IActionResult> GetCustomerBalanceSummaryList([FromQuery] bool isAllVendors, [FromQuery] string custCode, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            var list = await Mediator.Send(new GetCustomerBalanceSummaryList() { IsAllVendors = isAllVendors, CustCode = custCode, DateFrom = from, DateTo = to, User = UserInfo() });
            return Ok(list);
        }

        [HttpGet("getCustomerBalanceDetailsList")]
        public async Task<IActionResult> GetCustomerBalanceDetailsList([FromQuery] bool isSummary, [FromQuery] string custCode, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            var list = await Mediator.Send(new GetCustomerBalanceDetailsList() { IsSummary = isSummary, CustCode = custCode, DateFrom = from, DateTo = to, User = UserInfo() });
            return Ok(list);
        }


        //[HttpGet("getCustomerAllBalanceSummaryList")]
        //public async Task<IActionResult> GetCustomerAllBalanceSummaryList([FromQuery] bool isAllVendors, [FromQuery] string type, [FromQuery] string custCode, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
        //{
        //    var list = await Mediator.Send(new GetCustomerAllBalanceSummaryList() { IsAllVendors = isAllVendors, Type = type, CustCode = custCode, DateFrom = from, DateTo = to, User = UserInfo() });
        //    return Ok(list);
        //}

        [HttpGet("getCustomerPaymentSummaryList")]
        public async Task<IActionResult> GetCustomerPaymentSummaryList([FromQuery] bool isAllVendors, [FromQuery] string custCode, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            var list = await Mediator.Send(new GetCustomerPaymentSummaryList() { IsAllVendors = isAllVendors, CustCode = custCode, DateFrom = from, DateTo = to, User = UserInfo() });
            return Ok(list);
        }

        [HttpGet("getCustomerVoucherSummaryList")]
        public async Task<IActionResult> GetCustomerVoucherSummaryList([FromQuery] bool isAllBranches, [FromQuery] string type, [FromQuery] string custCode, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            var list = await Mediator.Send(new GetCustomerVoucherSummaryList() { Type = type, IsAllBranches = isAllBranches, CustCode = custCode, DateFrom = from, DateTo = to, User = UserInfo() });
            return Ok(list);
        }

        [HttpGet("getCustomerInvoiceSummaryList")]
        public async Task<IActionResult> GetCustomerInvoiceSummaryList([FromQuery] bool isAllVendors, [FromQuery] string type, [FromQuery] string custCode, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            var list = await Mediator.Send(new GetCustomerInvoiceSummaryList() { Type = type, IsAllVendors = isAllVendors, CustCode = custCode, DateFrom = from, DateTo = to, User = UserInfo() });
            return Ok(list);
        }

        [HttpGet("getCustomerInvoiceDetailList")]
        public async Task<IActionResult> GetCustomerInvoiceDetailList([FromQuery] bool isAllVendors, [FromQuery] string type, [FromQuery] string custCode, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            var list = await Mediator.Send(new GetCustomerInvoiceDetailList() { Type = type, IsAllVendors = isAllVendors, CustCode = custCode, DateFrom = from, DateTo = to, User = UserInfo() });
            return Ok(list);
        }

        [HttpGet("getCustomerVoucherPaymentDetailsList")]
        public async Task<IActionResult> GetCustomerVoucherPaymentDetailsList([FromQuery] bool isAllVendors, [FromQuery] string custCode, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            var list = await Mediator.Send(new GetCustomerVoucherPaymentDetailsList() { IsAllVendors = isAllVendors, CustCode = custCode, DateFrom = from, DateTo = to, User = UserInfo() });
            return Ok(list);
        }

        [HttpGet("getCustomerVoucherPaymentSummaryList")]
        public async Task<IActionResult> GetCustomerVoucherPaymentSummaryList([FromQuery] bool isAllVendors, [FromQuery] string custCode, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            var list = await Mediator.Send(new GetCustomerVoucherPaymentSummaryList() { IsAllVendors = isAllVendors, CustCode = custCode, DateFrom = from, DateTo = to, User = UserInfo() });
            return Ok(list);
        }


        #endregion

        #region AP Or Vendor Reports

        [HttpGet("getVendorBalanceSummaryList")]
        public async Task<IActionResult> GetVendorBalanceSummaryList([FromQuery] bool isAllVendors, [FromQuery] string custCode, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            var list = await Mediator.Send(new GetVendorBalanceSummaryList() { IsAllVendors = isAllVendors, CustCode = custCode, DateFrom = from, DateTo = to, User = UserInfo() });
            return Ok(list);
        }

        [HttpGet("getVendorBalanceDetailsList")]
        public async Task<IActionResult> GetVendorBalanceDetailsList([FromQuery] bool isSummary, [FromQuery] string custCode, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            var list = await Mediator.Send(new GetVendorBalanceDetailsList() { IsSummary = isSummary, CustCode = custCode, DateFrom = from, DateTo = to, User = UserInfo() });
            return Ok(list);
        }


        //[HttpGet("getVendorAllBalanceSummaryList")]
        //public async Task<IActionResult> GetVendorAllBalanceSummaryList([FromQuery] bool isAllVendors, [FromQuery] string type, [FromQuery] string custCode, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
        //{
        //    var list = await Mediator.Send(new GetVendorAllBalanceSummaryList() { IsAllVendors = isAllVendors, Type = type, CustCode = custCode, DateFrom = from, DateTo = to, User = UserInfo() });
        //    return Ok(list);
        //}


        [HttpGet("getVendorPaymentSummaryList")]
        public async Task<IActionResult> GetVendorPaymentSummaryList([FromQuery] bool isAllVendors, [FromQuery] string custCode, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            var list = await Mediator.Send(new GetVendorPaymentSummaryList() { IsAllVendors = isAllVendors, CustCode = custCode, DateFrom = from, DateTo = to, User = UserInfo() });
            return Ok(list);
        }


        [HttpGet("getVendorVoucherSummaryList")]
        public async Task<IActionResult> GetVendorVoucherSummaryList([FromQuery] bool isAllBranches, [FromQuery] string type, [FromQuery] string custCode, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            var list = await Mediator.Send(new GetVendorVoucherSummaryList() { Type = type, IsAllBranches = isAllBranches, CustCode = custCode, DateFrom = from, DateTo = to, User = UserInfo() });
            return Ok(list);
        }


        [HttpGet("getVendorInvoiceSummaryList")]
        public async Task<IActionResult> GetVendorInvoiceSummaryList([FromQuery] bool isAllVendors, [FromQuery] string type, [FromQuery] string custCode, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            var list = await Mediator.Send(new GetVendorInvoiceSummaryList() { Type = type, IsAllVendors = isAllVendors, CustCode = custCode, DateFrom = from, DateTo = to, User = UserInfo() });
            return Ok(list);
        }

        [HttpGet("getVendorInvoiceDetailList")]
        public async Task<IActionResult> GetVendorInvoiceDetailList([FromQuery] bool isAllVendors, [FromQuery] string type, [FromQuery] string custCode, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            var list = await Mediator.Send(new GetVendorInvoiceDetailList() { Type = type, IsAllVendors = isAllVendors, CustCode = custCode, DateFrom = from, DateTo = to, User = UserInfo() });
            return Ok(list);
        }


        [HttpGet("getVendorVoucherPaymentDetailsList")]
        public async Task<IActionResult> GetVendorVoucherPaymentDetailsList([FromQuery] bool isAllVendors, [FromQuery] string custCode, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            var list = await Mediator.Send(new GetVendorVoucherPaymentDetailsList() { IsAllVendors = isAllVendors, CustCode = custCode, DateFrom = from, DateTo = to, User = UserInfo() });
            return Ok(list);
        }


        [HttpGet("getVendorVoucherPaymentSummaryList")]
        public async Task<IActionResult> GetVendorVoucherPaymentSummaryList([FromQuery] bool isAllVendors, [FromQuery] string custCode, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            var list = await Mediator.Send(new GetVendorVoucherPaymentSummaryList() { IsAllVendors = isAllVendors, CustCode = custCode, DateFrom = from, DateTo = to, User = UserInfo() });
            return Ok(list);
        }


        #endregion

        #region OPM AR Customer Reports

        [HttpGet("getOpmCustomerSummaryList")]
        public async Task<IActionResult> GetOpmCustomerSummaryList([FromQuery] bool isAllCustomers, [FromQuery] string type, [FromQuery] string custCode, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            var list = await Mediator.Send(new GetOpmCustomerSummaryList() { IsAllCustomers = isAllCustomers, Type = type, CustCode = custCode, DateFrom = from, DateTo = to, User = UserInfo() });
            return Ok(list);
        }
        #endregion

        #region OPM AP Venor Reports

        [HttpGet("getOpmVendorSummaryList")]
        public async Task<IActionResult> GetOpmVendorSummaryList([FromQuery] bool isAllCustomers, [FromQuery] string type, [FromQuery] string custCode, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            var list = await Mediator.Send(new GetOpmVendorSummaryList() { IsAllCustomers = isAllCustomers, Type = type, CustCode = custCode, DateFrom = from, DateTo = to, User = UserInfo() });
            return Ok(list);
        }
        #endregion


        #region ProfitAndLossList

        [HttpGet("getProfitAndLossList")]
        public async Task<IActionResult> GetProfitAndLossList([FromQuery] string type, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            var list = await Mediator.Send(new GetProfitAndLossList() { DateFrom = from, DateTo = to, Type = type, User = UserInfo() });
            return Ok(list);
        }

        #endregion

        #region TaxReporting Pringting

        [HttpGet("getTaxReportingPrintList")]
        public async Task<IActionResult> GetTaxReportingPrintList([FromQuery] string branchCode, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            var list = await Mediator.Send(new GetTaxReportingPrintList() { DateFrom = from, DateTo = to, BranchCode = branchCode, User = UserInfo() });
            return Ok(list);
        }

        #endregion


        #region CustomerRevenueAnalysis Pringting

        [HttpGet("getCustomerRevenueAnalysis")]
        public async Task<IActionResult> GetCustomerRevenueAnalysis([FromQuery] string custCode, [FromQuery] string branchCode, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            var list = await Mediator.Send(new GetCustomerRevenueAnalysis() { DateFrom = from, DateTo = to, CustCode = custCode, BranchCode = branchCode, User = UserInfo() });
            return Ok(list);
        }

        #endregion


        #region getCustomerVendorAgeingAnalysisList

        [HttpGet("getCustomerVendorAgeingAnalysisList")]
        public async Task<IActionResult> GetCustomerVendorAgeingAnalysisList([FromQuery] string custCode, [FromQuery] string type)
        {
            var list = await Mediator.Send(new GetCustomerVendorAgeingAnalysisList() { CustCode = custCode, Type = type, User = UserInfo() });
            return Ok(list);
        }

        #endregion
    }
}
