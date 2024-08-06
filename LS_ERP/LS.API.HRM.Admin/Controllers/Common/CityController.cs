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
    public class CityController : BaseController
    {
        public CityController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {

        }

        [HttpGet("GetCitiesByState")]
        public async Task<IActionResult> GetCitiesByState([FromQuery] string stateCode)
        {
            var list = await Mediator.Send(new GetCitiesByState() { StateCode = stateCode, User = UserInfo() });
            return Ok(list);
        }
    }
}
