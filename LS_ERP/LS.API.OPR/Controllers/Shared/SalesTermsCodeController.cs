using CIN.Application;
using CIN.Application.Common;
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
   
    public class SalesTermsCodeController : BaseController
    {
        public SalesTermsCodeController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }
       [HttpGet("getSelectSalesTermsCodeList")]
        public async Task<IActionResult> GetSelectSalesTermsCodeList(string search)
        {
            var item = await Mediator.Send(new GetSelectSalesTermsCodeList() { Input = search, User = UserInfo() });
            return Ok(item);
        }
        [HttpGet("getSalesTermsByTermsCode/{salesTermsCode}")]
        public async Task<TblSndDefSalesTermsCodeDto> Get([FromRoute] string salesTermsCode)
        {
            var obj = await Mediator.Send(new GetSalesTermsByTermsCode() { SalesTermsCode = salesTermsCode, User = UserInfo() });
            return obj;
        }
    }
}
