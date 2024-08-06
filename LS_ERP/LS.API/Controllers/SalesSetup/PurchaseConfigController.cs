using CIN.Application;
using CIN.Application.SalesSetupDtos;
using CIN.Application.SalesSetupQuery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace LS.API.Controllers.SalesSetup
{
    public class PurchaseConfigController : BaseController
    {
        public PurchaseConfigController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var obj = await Mediator.Send(new GetSinglePurchaseConfig() { User = UserInfo() });
            return Ok(obj);
        }

        
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TblInvDefPurchaseConfigDto input)
        {
            //await Task.Delay(3000);

            var PurchaseConfig = await Mediator.Send(new CreateUpdatePurchaseConfig() { Input = input, User = UserInfo() });
            if (PurchaseConfig.Id > 0)
            {
                if (input.Id > 0)
                    return NoContent();
                else
                    return Created($"get/{PurchaseConfig.Id}", input);
            }
            return BadRequest(new ApiMessageDto { Message = PurchaseConfig.Message });
        }
    }
}
