using CIN.Application;
using CIN.Application.HumanResource.SetUp.HRMSetUpQuery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LS.API.HRM.Admin.Controllers.Common
{
    public class CountryController : BaseController
    {
        public CountryController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {

        }

        [HttpGet("getCountrySelectListItem")]
        public async Task<IActionResult> GetCountrySelectListItem()
        {
            var list = await Mediator.Send(new GetCountrySelectListItem() { User = UserInfo() });
            return Ok(list);
        }
    }
}
