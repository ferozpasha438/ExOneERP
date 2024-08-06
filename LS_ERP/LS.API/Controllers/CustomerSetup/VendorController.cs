using CIN.Application;
using CIN.Application.FinPurchaseMgtQuery;
using CIN.Application.OperationsMgtQuery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace LS.API.Controllers.CustomerSetup
{
    public class VendorController : BaseController
    {
        public VendorController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }

        [HttpGet("getVendorsCustomList")]
        public async Task<IActionResult> GetVendorsCustomList()
        {
            var obj = await Mediator.Send(new GetVendorsCustomList() { User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("getLanVendorsCustomList")]
        public async Task<IActionResult> GetLanVendorsCustomList()
        {
            var obj = await Mediator.Send(new GetLanVendorsCustomList() { User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("getVendorById/{id}")]
        public async Task<IActionResult> GetVendorById(int id)
        {
            var obj = await Mediator.Send(new GetVendorItem() { Id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(ApiMessageNotFound());
        }
    }
}
