using CIN.Application;
using CIN.Application.OperationsMgtQuery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace LS.API.Controllers
{
    public class CustomerController : BaseController
    {
        public CustomerController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }

        [HttpGet("getSelectCustomerList")]
        public async Task<IActionResult> GetSelectCustomerList()
        {
            var obj = await Mediator.Send(new GetCustomersCustomList() { User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(ApiMessageNotFound());
        }
        [HttpGet("getSelectLanCustomerList")]
        public async Task<IActionResult> GetSelectLanCustomerList()
        {
            var obj = await Mediator.Send(new GetLanCustomersCustomList() { User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(ApiMessageNotFound());
        }

        [HttpGet("getCustomerById/{id}")]
        public async Task<IActionResult> GetCustomerById(int id)
        {
            var obj = await Mediator.Send(new GetCustomerItem() { Id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(ApiMessageNotFound());
        }

    }
}
