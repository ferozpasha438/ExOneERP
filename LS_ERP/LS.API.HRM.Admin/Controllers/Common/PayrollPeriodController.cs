using CIN.Application;
using CIN.Application.Payroll.SetUp.Query;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LS.API.HRM.Admin.Controllers.Common
{
    public class PayrollPeriodController : BaseController
    {
        public PayrollPeriodController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {


        }

        [HttpGet("GetPayrollPeriodSelectListItem")]
        public async Task<IActionResult> GetPayrollPeriodSelectListItem()
        {
            var list = await Mediator.Send(new GetPayrollPeriodSelectListItem() { User = UserInfo() });
            return Ok(list);
        }
    }
}
