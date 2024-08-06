using CIN.Application;
using CIN.Application.SystemQuery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace LS.API.Fin.Controllers.SystemSetup
{
    public class UserTypeController : BaseController
    {
        public UserTypeController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }

        [HttpGet("getUserTypeSelectList")]
        public async Task<IActionResult> GetUserTypeSelectList()
        {
            var list = await Mediator.Send(new GetUserTypeSelectList() { User = UserInfo() });
            return Ok(list);
        }

    }
}
