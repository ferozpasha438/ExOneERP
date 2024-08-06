using CIN.Application;
using CIN.Application.HumanResource.SetUp.HRMSetUpQuery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LS.API.Payroll.Controllers.Shared
{
    public class PositionController : BaseController
    {
        public PositionController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {

        }
        [HttpGet("GetPositionSelectListItem")]
        public async Task<IActionResult> GetPositionSelectListItem()
        {
            var list = await Mediator.Send(new GetPositionSelectListItem() { User = UserInfo() });
            return Ok(list);
        }
    }
}
