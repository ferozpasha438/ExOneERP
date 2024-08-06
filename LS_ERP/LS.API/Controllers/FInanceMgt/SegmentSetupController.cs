using CIN.Application;
using CIN.Application.FinanceMgtDtos;
using CIN.Application.FinanceMgtQuery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace LS.API.Controllers.FInanceMgt
{
    public class SegmentSetupController : BaseController
    {
        public SegmentSetupController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            //await Task.Delay(3000);
            var list = await Mediator.Send(new GetSegmentSetupList() { User = UserInfo() });
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var obj = await Mediator.Send(new GetSingleSegmentSetup() { Id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }


        [HttpGet("getSegmentSetupSelectList")]
        public async Task<IActionResult> GetSegmentSetupSelectList()
        {
            var obj = await Mediator.Send(new GetSegmentSetupSelectList() { User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TblFinSysSegmentSetupDto input)
        {
            //await Task.Delay(3000);

            var seg = await Mediator.Send(new CreateUpdateSegmentSetup() { Input = input, User = UserInfo() });
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
