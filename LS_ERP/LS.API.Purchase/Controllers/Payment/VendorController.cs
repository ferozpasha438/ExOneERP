using CIN.Application;
using CIN.Application.FinPurchaseMgtQuery;
using CIN.Application.OperationsMgtQuery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace LS.API.Purchase.Controllers
{
    public class VendorController : BaseController
    {
        public VendorController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }

        [HttpGet("getVendorsCustomList")]
        public async Task<IActionResult> GetVendorsCustomList([FromQuery] bool? isPayment)
        {
            var obj = await Mediator.Send(new GetVendorsCustomList() { IsPayment = isPayment, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("getLanVendorsCustomList")]
        public async Task<IActionResult> GetLanVendorsCustomList([FromQuery] bool? isPayment, [FromQuery] string search)
        {
            var obj = await Mediator.Send(new GetLanVendorsCustomList() { IsPayment = isPayment, Search = search, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("getVendorById/{id}")]
        public async Task<IActionResult> GetVendorById(int id)
        {
            var obj = await Mediator.Send(new GetVendorItem() { Id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(ApiMessageNotFound());
        }


        [HttpGet("getVendorSelectItemList")]
        public async Task<IActionResult> GetVendorSelectItemList([FromQuery] string search)
        {
            var obj = await Mediator.Send(new GetVendorSelectItemList() { Input = search, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

    }
}
