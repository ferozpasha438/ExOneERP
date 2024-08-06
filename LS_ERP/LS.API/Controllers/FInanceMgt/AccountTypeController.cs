using CIN.Application;
using CIN.Application.Common;
using CIN.Application.FinanceMgtDtos;
using CIN.Application.FinanceMgtQuery;
using CIN.Application.SystemSetupDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace LS.API.Controllers.FInanceMgt
{
    public class AccountTypeController : BaseController
    {
        public AccountTypeController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            //await Task.Delay(3000);
            var item = await Mediator.Send(new GetAcTypeList() { User = UserInfo() });
            return Ok(item);
        }
        

    }
}
