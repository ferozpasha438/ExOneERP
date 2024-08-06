using CIN.Application;
using CIN.Application.SalesSetupDtos;
using CIN.Application.SalesSetupQuery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace LS.API.Controllers.SalesSetup
{
    public class SalesConfigController : BaseController
    {
        public SalesConfigController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var obj = await Mediator.Send(new GetSingleSalesConfig() { User = UserInfo() });
            return Ok(obj);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TblInvDefSalesConfigDto input)
        {
            //await Task.Delay(3000);

            var salesConfig = await Mediator.Send(new CreateUpdateSalesConfig() { Input = input, User = UserInfo() });
            if (salesConfig.Id > 0)
            {
                if (input.Id > 0)
                    return NoContent();
                else
                    return Created($"get/{salesConfig.Id}", input);
            }
            return BadRequest(new ApiMessageDto { Message = salesConfig.Message });
        }
    }
}
