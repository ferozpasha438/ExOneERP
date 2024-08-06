using CIN.Application;
using CIN.Application.Common;
using CIN.Application.OperationsMgtQuery;
using CIN.Application.SystemSetupDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace LS.API.OPR.Controllers.SystemSetup
{
    public class BranchController : BaseController
    {
        public BranchController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {

        }
        [HttpGet("getSelectBranchCodeList")]
        public async Task<IActionResult> GetSelectBranchCodeList()
        {
            var obj = await Mediator.Send(new GetSelectBranchCodeList() { User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("getBranchSelectListForUser")]
        public async Task<IActionResult> GetCitiesSelectListForUser()
        {
            var list = await Mediator.Send(new GetSelectBranchListForUser() { User = UserInfo() });
            return Ok(list);
        }



    }
}
