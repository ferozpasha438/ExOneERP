using CIN.Application;
using CIN.Application.Common;
using CIN.Application.FinanceMgtQuery;
using CIN.Application.OperationsMgtDtos;
using CIN.Application.OperationsMgtQuery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LS.API.OPR.Controllers.Shared
{
   
    public class MainAccountsController : BaseController
    {
        public MainAccountsController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }
       [HttpGet("getSelectMainAccountList")]
        public async Task<IActionResult> GetSelectMainAccountList(string search)
        {
            var item = await Mediator.Send(new GetSelectMainAccountsList() { Input = search, User = UserInfo() });
            return Ok(item);
        }

        [HttpGet("getAccountByAccountCode/{accountCode}")]
        public async Task<TblFinDefMainAccountsDto> Get([FromRoute] string accountCode)
        {
            var obj = await Mediator.Send(new GetAccountByAccountCode() { AccountCode = accountCode, User = UserInfo() });
            return obj ;
        }


    }
}
