using CIN.Application;
using CIN.Application.FinanceMgtDtos;
using CIN.Application.FinanceMgtQuery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace LS.API.Sales.Controllers.SND
{
    public class PaymentcodesController : BaseController
    {
        public PaymentcodesController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }

      

        [HttpGet("getSelectPaymentCodeList")]
        public async Task<IActionResult> GetSelectPaymentCodeList()
        {
            var obj = await Mediator.Send(new GetSelectPaymentCodeList() { User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

    }
}
