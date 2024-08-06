using CIN.Application;
using CIN.Application.Common;
using CIN.Application.FinanceMgtDtos;
using CIN.Application.FinanceMgtQuery;
using CIN.Application.SystemSetupDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace LS.API.Controllers.FInanceMgt
{
    public class SegmentTwoSetupController : BaseController
    {
        public SegmentTwoSetupController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            //await Task.Delay(3000);
            var list = await Mediator.Send(new GetSegmentTwoSetupList() { User = UserInfo() });
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var obj = await Mediator.Send(new GetSingleSegmentTwoSetup() { Id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }


        [HttpGet("getSegmentTwoSetupSelectList")]
        public async Task<IActionResult> GetSegmentTwoSetupSelectList()
        {
            var obj = await Mediator.Send(new GetSegmentTwoSetupSelectList() {  User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TblFinSysSegmentTwoSetupDto input)
        {
            //await Task.Delay(3000);

            var seg = await Mediator.Send(new CreateUpdateSegmentTwoSetup() { Input = input, User = UserInfo() });
            if (seg.Id > 0)
            {
                if (input.Id > 0)
                    return NoContent();
                else
                    return Created($"get/{seg.Id}", input);
            }
            return BadRequest(new ApiMessageDto { Message = seg.Message });
        }

    }
}
