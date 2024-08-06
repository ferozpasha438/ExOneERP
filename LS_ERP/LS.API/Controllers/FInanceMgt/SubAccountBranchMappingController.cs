using CIN.Application;
using CIN.Application.FinanceMgtDtos;
using CIN.Application.FinanceMgtQuery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace LS.API.Controllers.FInanceMgt
{
    public class SubAccountBranchMappingController : BaseController
    {
        public SubAccountBranchMappingController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }

        [HttpGet("getSubAccountBranchMapping")]
        public async Task<IActionResult> GetSubAccountBranchMapping([FromQuery] string branchCode)
        {
            var obj = await Mediator.Send(new GetSubAccountBranchMapping() { BranchCode = branchCode, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TblFinDefAccountBranchMappingDto input)
        {
            //await Task.Delay(3000);

            var payCode = await Mediator.Send(new CreateSubAccountBranchMapping() { Input = input, User = UserInfo() });
            if (payCode.Id > 0)
            {
                return Created($"get/{payCode.Id}", input);
            }
            return BadRequest(new ApiMessageDto { Message = payCode.Message });
        }
    }
}

