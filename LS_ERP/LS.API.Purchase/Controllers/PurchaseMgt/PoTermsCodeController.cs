using CIN.Application;
using CIN.Application.Common;
using CIN.Application.FinPurchaseMgtQuery;
using CIN.Application.OperationsMgtDtos;
using CIN.Application.OperationsMgtQuery;
using CIN.Application.PurchaseSetupQuery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LS.API.Purchase.Controllers.Purchasemgt
{
    public class PoTermsCodeController : BaseController
    {
        public PoTermsCodeController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }

        [HttpGet("getSelectPoTermsCodeList")]
        public async Task<IActionResult> GetSelectPoTermsCodeList()
        {
            var obj = await Mediator.Send(new GetSelectPoTermsCodeListOne() { User = UserInfo() });
            return Ok(obj);
        }
        [HttpGet("getPoTermsByTermsCode/{salesTermsCode}")]
        public async Task<IActionResult> Get([FromRoute] string salesTermsCode)
        {
            var obj = await Mediator.Send(new GetPoTermsByTermsCode() { PoTermsCode = salesTermsCode, User = UserInfo() });
            return Ok(obj);
        }
    }
}
