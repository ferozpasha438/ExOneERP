using CIN.Application;
using CIN.Application.Common;
using CIN.Application.FinanceMgtDtos;
using CIN.Application.FinanceMgtQuery;
using CIN.Application.SystemSetupDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace LS.API.Controllers
{
    public class AccountsbranchmappingController : BaseController
    {
        public AccountsbranchmappingController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }

        [HttpGet("getBranchAccountMappingList")]
        public async Task<IActionResult> GetBranchAccountMappingList()
        {
            //await Task.Delay(3000);
            var list = await Mediator.Send(new GetBranchAccountMappingList() { User = UserInfo() });
            return Ok(list);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateBranchesMainAccountsDto input)
        {
            //await Task.Delay(3000);

            var payCode = await Mediator.Send(new CreateAcBranchMapping() { Input = input, User = UserInfo() });
            if (payCode.Id > 0)
            {
                return Created($"get/{payCode.Id}", input);
            }
            return BadRequest(new ApiMessageDto { Message = payCode.Message });
        }
    }
}
