using CIN.Application;
using CIN.Application.InventoryDtos;
using CIN.Application.InventoryQuery;
using CIN.Application.SystemQuery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace LS.API.Controllers
{
    public class InventoryconfigurationController : BaseController
    {
        public InventoryconfigurationController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }

        [HttpGet("getInvtConfig")]
        public async Task<ActionResult> GetInvtConfig()
        {
            var config = await Mediator.Send(new GetInvtConfig() { });
            return Ok(config);
        }



        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TblInvDefInventoryConfigDto input)
        {
            //await Task.Delay(3000);

            var branch = await Mediator.Send(new CreateInvtConfig() { Input = input, User = UserInfo() });
            if (branch.InvconfigId > 0)
            {
                if (input.Id > 0)
                    return NoContent();
                else
                    return Created($"get/{branch.InvconfigId}", input);
            }
            return BadRequest(new ApiMessageDto { Message = branch.msg });
        }

    }
}
