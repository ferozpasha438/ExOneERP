using CIN.Application;
using CIN.Application.FinanceMgtDtos;
using CIN.Application.FinanceMgtQuery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace LS.API.Fin.Controllers.FInanceMgt
{
    public class SegmentTwoSetupController : BaseController
    {
        public SegmentTwoSetupController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }        

        [HttpGet("getSegmentTwoSetupSelectList")]
        public async Task<IActionResult> GetSegmentTwoSetupSelectList()
        {
            var obj = await Mediator.Send(new GetSegmentTwoSetupSelectList() { User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("getSegmentTwoSetupSearchSelectList")]
        public async Task<IActionResult> GetSegmentTwoSetupSearchSelectList([FromQuery] string search)
        {
            var obj = await Mediator.Send(new GetSegmentTwoSetupSearchSelectList() { Search = search, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
    }
}
