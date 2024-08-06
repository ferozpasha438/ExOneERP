using CIN.Application;
using CIN.Application.SystemQuery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LS.API.HRM.Admin.Controllers.Common
{
    public class StateController : BaseController
    {
        public StateController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {

        }

        [HttpGet("GetStatesByCountry")]
        public async Task<IActionResult> GetStatesByCountry([FromQuery] string countryCode)
        {
            var list = await Mediator.Send(new GetStatesByCountry() { CountryCode = countryCode, User = UserInfo() });
            return Ok(list);
        }
    }
}
