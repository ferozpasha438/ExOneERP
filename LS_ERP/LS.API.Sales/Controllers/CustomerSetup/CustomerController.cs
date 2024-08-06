using CIN.Application;
using CIN.Application.OperationsMgtQuery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace LS.API.Sales.Controllers.CustomerSetup
{
    public class CustomerController : BaseController
    {
        public CustomerController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }


        [HttpGet("getSelectCustomerList")]
        public async Task<IActionResult> GetSelectCustomerList([FromQuery] bool? isPayment)
        {
            var obj = await Mediator.Send(new GetCustomersCustomList() { IsPayment = isPayment, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("getSelectLanCustomerList")]
        public async Task<IActionResult> GetSelectLanCustomerList([FromQuery] bool? isPayment, [FromQuery] string search)
        {
            var obj = await Mediator.Send(new GetLanCustomersCustomList() { IsPayment = isPayment, Search = search, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("getCustomerById/{id}")]
        public async Task<IActionResult> GetCustomerById(int id)
        {
            var obj = await Mediator.Send(new GetCustomerItem() { Id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("getCustomerCodeSelectList")]
        public async Task<IActionResult> GetCustomerCodeSelectList([FromQuery] string search)
        {
            var obj = await Mediator.Send(new GetCustomerCodeSelectList() { Input = search, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

    }
}
