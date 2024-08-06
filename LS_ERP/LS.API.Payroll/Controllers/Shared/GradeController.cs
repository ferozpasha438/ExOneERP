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
    public class GradeController : BaseController
    {
        public GradeController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {

        }
        [HttpGet("GetGradeSelectListItem")]
        public async Task<IActionResult> GetGradeSelectListItem()
        {
            var list = await Mediator.Send(new GetGradeSelectListItem() { User = UserInfo() });
            return Ok(list);
        }
    }
}
