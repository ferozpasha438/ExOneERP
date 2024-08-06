using CIN.Application;
using CIN.Application.Common;
using CIN.Application.SystemQuery;
using CIN.Application.SystemSetupDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace LS.API.Purchase.Controllers
{
    public class ZoneController : BaseController
    {
        public ZoneController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }

        [HttpGet("getZoneSelectList")]
        public async Task<IActionResult> GetZoneSelectList()
        {
            var list = await Mediator.Send(new GetZoneSelectList() { User = UserInfo() });
            return Ok(list);
        }

    }
}
