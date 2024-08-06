using CIN.Application;
using CIN.Application.InvoiceQuery;
using CIN.Application.OperationsMgtQuery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace LS.API.Fin.Controllers
{
    public class CustomerController : BaseController
    {
        public CustomerController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }


        [HttpGet("getSelectCustomerList")]
        public async Task<IActionResult> GetSelectCustomerList([FromQuery] bool? isPayment)
        {
            var obj = await Mediator.Send(new GetCustomersCustomList() { IsPayment = isPayment });
            return obj is not null ? Ok(obj) : NotFound(ApiMessageNotFound());
        }
        [HttpGet("getSelectLanCustomerList")]
        public async Task<IActionResult> GetSelectLanCustomerList([FromQuery] bool? isPayment, [FromQuery] string search)
        {
            var obj = await Mediator.Send(new GetLanCustomersCustomList() { IsPayment = isPayment, Search = search });
            return obj is not null ? Ok(obj) : NotFound(ApiMessageNotFound());
        }

        [HttpGet("getCustomerById/{id}")]
        public async Task<IActionResult> GetCustomerById(int id)
        {
            var obj = await Mediator.Send(new GetCustomerItem() { Id = id });
            return obj is not null ? Ok(obj) : NotFound(ApiMessageNotFound());
        }

        [HttpGet("getCustomerCodeSelectList")]
        public async Task<IActionResult> GetCustomerCodeSelectList([FromQuery] string search)
        {
            var obj = await Mediator.Send(new GetCustomerCodeSelectList() { Input = search });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("getCustomerSitesSelectList/{id}")]
        public async Task<IActionResult> GetCustomerSitesSelectList([FromRoute] int id, [FromQuery] string custCode = "")
        {
            var obj = await Mediator.Send(new GetCustomerSitesSelectList() { Id = id, CustCode = custCode });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

    }
}
