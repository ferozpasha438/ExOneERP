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
    public class FinancialsetupController : BaseController
    {
        public FinancialsetupController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            //await Task.Delay(3000);
            var item = await Mediator.Send(new GetFinSetup() { Id = id, User = UserInfo() });
            return Ok(item);
        }

        [HttpGet("getAcCodeSegment")]
        public async Task<IActionResult> GetAcCodeSegment()
        {
            //await Task.Delay(3000);
            var item = await Mediator.Send(new GetAcCodeSegment() { User = UserInfo() });
            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TblFinSysFinanialSetupDto input)
        {
            //await Task.Delay(3000);

            var finSetup = await Mediator.Send(new CreateFinancialsetup() { BranchDto = input, User = UserInfo() });
            if (finSetup.Id > 0)
            {
                return Created($"get/{finSetup.Id}", input);
            }
            return BadRequest(new ApiMessageDto { Message = finSetup.Message });
        }


        [HttpPost("AccountCodeTopology")]
        public async Task<ActionResult> AccountCodeTopology([FromBody] TblErpSysAcCodeSegmentDto input)
        {
            //await Task.Delay(3000);

            var finSetup = await Mediator.Send(new CreateAcCodeSegment() { AcCodeDto = input, User = UserInfo() });
            if (finSetup.Id > 0)
            {
                return Created($"get/{finSetup.Id}", input);
            }
            return BadRequest(new ApiMessageDto { Message = finSetup.Message });
        }




    }
}
