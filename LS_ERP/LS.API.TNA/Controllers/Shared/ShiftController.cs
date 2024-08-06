using CIN.Application;
using CIN.Application.HumanResource.SetUp.HRMSetUpQuery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LS.API.TNA.Controllers.Shared
{
    public class ShiftController : BaseController
    {
        public ShiftController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {

        }
        [HttpGet("GetShiftSelectListItem")]
        public async Task<IActionResult> GetBankSelectListItem()
        {
            var list = await Mediator.Send(new GetShiftSelectListItem() { User = UserInfo() });
            return Ok(list);
        }
    }
}