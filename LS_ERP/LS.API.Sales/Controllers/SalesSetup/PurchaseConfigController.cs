using CIN.Application;
using CIN.Application.SalesSetupDtos;
using CIN.Application.SalesSetupQuery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace LS.API.Sales.Controllers.SalesSetup
{
    public class PurchaseConfigController : BaseController
    {
        public PurchaseConfigController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }

        [HttpGet("canAutoGenerateVendCode")]
        public async Task<IActionResult> CanAutoGenerateVendCode()
        {
            //await Task.Delay(3000);
            var item = await Mediator.Send(new CanAutoGenerateVendCode() { User = UserInfo() });
            return Ok(item);
        }

    }
}
