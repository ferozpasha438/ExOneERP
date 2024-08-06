using CIN.Application;
using CIN.Application.Common;
using CIN.Application.SystemQuery;
using CIN.Application.SystemSetupDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace LS.API.Fin.Controllers
{
    public class CompanyController : BaseController
    {
        public CompanyController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {

        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {
           // await Task.Delay(3000);
            var list = await Mediator.Send(new GetCompanyList() { Input = filter.Values(), User = UserInfo() });
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var obj = await Mediator.Send(new GetCompany() { Id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("getSelectCompanyList")]
        public async Task<IActionResult> GetSelectCompanyList(string search)
        {
            //await Task.Delay(3000);
            var item = await Mediator.Send(new GetCompanySelectItemList() { Input = search, User = UserInfo() });
            return Ok(item);
        }

        //[httpget("get")]
        //public async task<iactionresult> get()
        //{
        //    await task.delay(3000);
        //    var list = await mediator.send(new getcompanyselectitemlist() { input = filter.values(), user = userinfo() });
        //    return ok(list);
        //}


        [HttpGet("checkCompanyName")]
        public async Task<IActionResult> CheckCompanyName([FromQuery] string companyName)
        {
            var obj = await Mediator.Send(new CheckCompanyName() { Input = companyName, User = UserInfo() });
            return Ok(obj);
        }


        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TblErpSysCompanyDto dTO)
        {
            var companyId = await Mediator.Send(new CreateCompany() { CompanyDto = dTO, User = UserInfo() });
            if (companyId > 0)
                return Created($"get/{companyId}", dTO);
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }

    }
}
