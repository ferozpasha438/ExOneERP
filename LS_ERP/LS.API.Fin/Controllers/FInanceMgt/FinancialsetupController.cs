using CIN.Application;
using CIN.Application.FinanceMgtQuery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LS.API.Fin.Controllers.FInanceMgt
{
    public class FinancialsetupController : BaseController
    {
        public FinancialsetupController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }

        [HttpGet("getFinSetup")]
        public async Task<IActionResult> GetCategoryTypeList()
        {
            var item = await Mediator.Send(new GetFinSetup() { User = UserInfo() });
            return Ok(item);
        }
    }
}
