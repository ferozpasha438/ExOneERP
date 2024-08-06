using CIN.Application;
using CIN.Application.FinanceMgtDtos;
using CIN.Application.FinanceMgtQuery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace LS.API.Sales.Controllers.SND
{
    public class SegmentSetupController : BaseController
    {
        public SegmentSetupController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }

        [HttpGet("getSegmentSetupSelectList")]
        public async Task<IActionResult> GetSegmentSetupSelectList()
        {
            var obj = await Mediator.Send(new GetSegmentSetupSelectList() { User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
    }
}
