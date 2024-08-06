using CIN.Application;
using CIN.Application.FinanceMgtDtos;
using CIN.Application.FinanceMgtQuery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace LS.API.Sales.Controllers.SND
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


    }
}
