using CIN.Application;
using CIN.Application.SystemQuery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LS.API.TNA.Controllers.Shared
{
    public class BranchController : BaseController
    {
        public BranchController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {

        }

        [HttpGet("GetBranchesByCompany")]
        public async Task<IActionResult> GetBranchesByCompany([FromQuery] int id)
        {
            var obj = await Mediator.Send(new GetSelectSysBranchListByComId() { Input = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
    }
}
