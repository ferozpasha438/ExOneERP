using CIN.Application;
using CIN.Application.SystemQuery;
using CIN.Application.SystemSetupDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace LS.API.Controllers.SystemSetup
{
    public class SysLoginController : BaseController
    {
        public SysLoginController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }


        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            //await Task.Delay(3000);
            var list = await Mediator.Send(new GetUserList() { User = UserInfo() });
            return Ok(list);
        }


        [HttpGet("checkUserLoginId")]
        public async Task<IActionResult> CheckUserLoginId([FromQuery] string loginId)
        {
            var obj = await Mediator.Send(new CheckUserLoginId() { Input = loginId, User = UserInfo() });
            return Ok(obj);
        }
        [HttpGet("checkUserName")]
        public async Task<IActionResult> CheckUserName([FromQuery] string userName)
        {
            var obj = await Mediator.Send(new CheckUserName() { Input = userName, User = UserInfo() });
            return Ok(obj);
        }


        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TblErpSysLoginDto input)
        {
            //await Task.Delay(3000);

            var sysLogin = await Mediator.Send(new CreateUpdateSysLogin() { Input = input, User = UserInfo() });
            if (sysLogin.Id > 0)
            {
                if (input.Id > 0)
                    return NoContent();
                else
                    return Created($"get/{sysLogin.Id}", input);
            }
            return BadRequest(new ApiMessageDto { Message = sysLogin.Message });
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var UserId = await Mediator.Send(new DeleteUser() { Id = id, User = UserInfo() });
            if (UserId > 0)
                return NoContent();
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }


    }
}
