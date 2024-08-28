using CIN.Application;
using CIN.Application.SystemQuery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LS.API.Payroll.Controllers.Shared
{
    public class CompanyController : BaseController
    {
        public CompanyController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {

        }

        [HttpGet("GetCompanySelectItemList")]
        public async Task<IActionResult> GetCompanySelectItemList(string search)
        {
            var obj = await Mediator.Send(new GetCompanySelectItemList() { Input = search, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
    }
}
