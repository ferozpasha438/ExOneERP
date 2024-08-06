using CIN.Application;
using CIN.Application.Common;
using CIN.Application.FinanceMgtDtos;
using CIN.Application.FinanceMgtQuery;
using CIN.Application.SystemSetupDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace LS.API.Fin.Controllers.FInanceMgt
{
    public class AccountscategoryController : BaseController
    {
        public AccountscategoryController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }

        [HttpGet("getCategoryTypeList")]
        public async Task<IActionResult> GetCategoryTypeList()
        {
            var item = await Mediator.Send(new GetCategoryTypeList() { User = UserInfo() });
            return Ok(item);
        }
    }
}
