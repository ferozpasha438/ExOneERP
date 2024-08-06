using CIN.Application;
using CIN.Application.MobileMgt.Dtos;
using CIN.Application.MobileMgt.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace LS.API.MOB.Controllers
{
    public class LocationTrackingController : BaseController
    {
        private readonly IOptions<AppMobileSettingsJson> _appSettings;

        public LocationTrackingController(IOptions<AppMobileSettingsJson> appSettings) : base(appSettings)
        {
            _appSettings = appSettings;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LocationTrackingDto input)
        {
            input.SiteLocationNvMeter = _appSettings.Value.SiteLocationNvMeter;
            input.SiteLocationPvMeter = _appSettings.Value.SiteLocationPvMeter;
            input.SiteLocationExtraMeter = _appSettings.Value.SiteLocationExtraMeter;

            var obj = await Mediator.Send(new LocationTracking() { Input = input, User = UserInfo() });
            return obj.Status ? Ok(obj) : BadRequest(obj);
        }


    }
}
