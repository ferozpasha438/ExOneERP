using CIN.Application;
using CIN.Application.FinanceMgtQuery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace LS.API.Fin.Controllers.FInanceMgt
{
    public class AccountsbranchmappingController : BaseController
    {
        public AccountsbranchmappingController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }


        [HttpGet("getSelectAccountMappingList")]
        public async Task<IActionResult> GetBranchAccountMappingList([FromQuery] string branchCode)
        {            
            var list = await Mediator.Send(new GetSelectAccountMappingList() { Input = branchCode, User = UserInfo() });
            return Ok(list);
        }

        //[HttpGet("getSelectAccountCodeListBybranchCode")]
        //public async Task<IActionResult> GetSelectSysBranchList([FromQuery] string branchCode)
        //{
        //    var obj = await Mediator.Send(new GetSelectAccountCodeListBybranchCode() { Input = branchCode, User = UserInfo() });
        //    return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        //}

    }
}
