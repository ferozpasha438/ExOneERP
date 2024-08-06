using CIN.Application;
using CIN.Application.Common;
using CIN.Application.FinanceMgtDtos;
using CIN.Application.FinanceMgtQuery;
using CIN.Application.SystemSetupDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace LS.API.Controllers
{
    public class AccountscategoryController : BaseController
    {
        public AccountscategoryController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var item = await Mediator.Send(new GetSelectAcCategoryList() { User = UserInfo() });
            return Ok(item);
        }

        [HttpGet("checkAccountCode")]
        public async Task<IActionResult> CheckAccountCode([FromQuery] string accountCode)
        {
            var obj = await Mediator.Send(new CheckAccountCode() { Input = accountCode, User = UserInfo() });
            return Ok(obj);
        }


        [HttpGet("getCategoryTypeList")]
        public async Task<IActionResult> GetCategoryTypeList()
        {
            var item = await Mediator.Send(new GetCategoryTypeList() { User = UserInfo() });
            return Ok(item);
        }

        [HttpGet("getAccountCode")]
        public async Task<IActionResult> GetAccountCode(int id)
        {
            var item = await Mediator.Send(new GetAccountCode() { Id = id, User = UserInfo() });
            return Ok(item);
        }

        [HttpGet("getSelectMainIntegrationAccountList")]
        public async Task<IActionResult> GetSelectMainIntegrationAccountList(string search)
        {
            var item = await Mediator.Send(new GetSelectMainAccountList() { FinIsIntegrationAC = true, IsAutoComplete = true, Search = search, User = UserInfo() });
            return Ok(item);
        }

        [HttpGet("getSelectMainPdcAccountList")]
        public async Task<IActionResult> GetSelectMainPdcAccountList(string search)
        {
            var item = await Mediator.Send(new GetSelectMainAccountList() { FinIsIntegrationAC = false, IsAutoComplete = true, Search = search, User = UserInfo() });
            return Ok(item);
        }

        [HttpGet("getSelectMainAllAccountList")]
        public async Task<IActionResult> GetSelectMainAllAccountList(string search)
        {
            var item = await Mediator.Send(new GetSelectMainAccountList() { User = UserInfo() });
            return Ok(item);
        }
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TblFinDefAccountCategoryDto input)
        {
            var category = await Mediator.Send(new CreateAcCategory() { Input = input, User = UserInfo() });
            if (category.Id > 0)
            {
                return Created($"get/{category.Id}", input);
            }
            return BadRequest(new ApiMessageDto { Message = category.Message });
        }
        [HttpPost("createaccoutnsubcategory")]
        public async Task<ActionResult> CreateAccoutnsubcategory([FromBody] TblFinDefAccountSubCategoryDto input)
        {
            var category = await Mediator.Send(new CreateAcSubCategory() { Input = input, User = UserInfo() });
            if (category.Id > 0)
            {
                return Created($"get/{category.Id}", input);
            }
            return BadRequest(new ApiMessageDto { Message = category.Message });
        }
        [HttpPost("createaccoutcode")]
        public async Task<ActionResult> Createaccoutcode([FromBody] TblFinDefMainAccountsDto input)
        {
            var category = await Mediator.Send(new CreateAccountCode() { Input = input, User = UserInfo() });
            if (category.Id > 0)
            {
                return Created($"get/{category.Id}", input);
            }
            return BadRequest(new ApiMessageDto { Message = category.Message });
        }

        
    }
}
