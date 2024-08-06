using CIN.Application;
using CIN.Application.Common;
using CIN.Application.SchoolMgtDto;
using CIN.Application.SchoolMgtQuery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LS.API.SM.Controllers.Reports
{
    public class ReportsController : BaseController
    {
        private readonly IConfiguration _Config;

        public ReportsController(IOptions<AppSettingsJson> appSettings, IConfiguration config) : base(appSettings)
        {
            _Config = config;
        }
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {
            return Ok(true);
        }
        [HttpGet("TermDuePaymentReport")]
        public async Task<IActionResult> TermDuePaymentReport([FromQuery] PaginationFilterDto filter)
        {
            var obj = await Mediator.Send(new TermDuePaymentReport() { Input = filter, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("SendTermDuePaymentNotifications/{branchCode}")]
        public async Task<IActionResult> SendTermDuePaymentNotifications([FromRoute] string branchCode)
        {
            var obj = await Mediator.Send(new SendTermDuePaymentNotifications() { BranchCode = branchCode, User = UserInfo() });
            return Ok(obj);
        }
        [HttpGet("GetFeeTerms")]
        public async Task<IActionResult> GetFeeTerms()
        {
            var obj = await Mediator.Send(new GetFeeTerms() { User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("AcademicFeeTransactionReport")]
        public async Task<IActionResult> AcademicFeeTransactionReport([FromQuery] PaginationFilterDto filter)
        {
            var obj = await Mediator.Send(new AcademicFeeTransactionReport() { Input = filter, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("StudentFeeListReport")]
        public async Task<IActionResult> StudentFeeListReport([FromQuery] PaginationFilterDto filter)
        {
            var obj = await Mediator.Send(new StudentFeeListReport() { Input = filter, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("FeeStructureSummaryReport")]
        public async Task<IActionResult> FeeStructureSummaryReport([FromQuery] PaginationFilterDto filter)
        {
            var obj = await Mediator.Send(new FeeStructureSummaryReport() { Input = filter, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("FeeStructureDetailsReport")]
        public async Task<IActionResult> FeeStructureDetailsReport([FromQuery] PaginationFilterDto filter)
        {
            var obj = await Mediator.Send(new FeeStructureDetailsReport() { Input = filter, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
    }
}
