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
    public class ZoneController : BaseController
    {
        public ZoneController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {

        }

        [HttpGet("GetZoneSelectListItem")]
        public async Task<IActionResult> GetZoneSelectListItem()
        {
            var list = await Mediator.Send(new GetZoneSelectListItem() { User = UserInfo() });
            return Ok(list);
        }
    }
}
