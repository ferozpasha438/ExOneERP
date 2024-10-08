using CIN.Application;
using CIN.Application.TimeAndAttendance.Setup.TNASetUpQuery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LS.API.Payroll.Controllers.Shared
{
    public class PayrollGroupController : BaseController
    {
        public PayrollGroupController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {

        }

        [HttpGet("GetPayrollGroupSelectListItem")]
        public async Task<IActionResult> GetPayrollGroupSelectListItem()
        {
            var obj = await Mediator.Send(new GetPayrollGroupSelectListItem() { User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
    }
}
