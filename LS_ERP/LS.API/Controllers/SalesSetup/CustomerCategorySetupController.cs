using CIN.Application;
using CIN.Application.SalesSetupDtos;
using CIN.Application.SalesSetupQuery;
using CIN.Application.SystemQuery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace LS.API.Controllers.SalesSetup
{
    public class CustomerCategorySetupController : BaseController
    {
        public CustomerCategorySetupController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var obj = await Mediator.Send(new GetCustCategoryList() { User = UserInfo() });
            return Ok(obj);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TblSndDefCustomerCategoryDto input)
        {
            //await Task.Delay(3000);

            var branch = await Mediator.Send(new CreateUpdateCustCategory() { Input = input, User = UserInfo() });
            if (branch.Id > 0)
            {
                if (input.Id > 0)
                    return NoContent();
                else
                    return Created($"get/{branch.Id}", input);
            }
            return BadRequest(new ApiMessageDto { Message = branch.Message });
        }
    }
}
