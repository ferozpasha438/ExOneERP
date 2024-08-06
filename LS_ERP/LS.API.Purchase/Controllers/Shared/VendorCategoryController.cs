using CIN.Application;
using CIN.Application.Common;
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

namespace LS.API.Purchase.Controllers.Shared
{

    public class VendorCategoryController : BaseController
    {
        public VendorCategoryController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }
        [HttpGet("getSelectVendorCategoryCodeList")]
        public async Task<IActionResult> GetSelectVendorCategoryCodeList(string search)
        {
            var item = await Mediator.Send(new GetVendorCategorySelectListQuery() { Input = search, User = UserInfo() });
            return Ok(item);
        }

        [HttpGet("getCustCatByVenCatCode/{custCatCode}")]
        public async Task<IActionResult> GetCustCatByVenCatCode([FromRoute] string custCatCode)
        {
            var obj = await Mediator.Send(new GetCustCatByVenCatCode() { VendorCatCode = custCatCode, User = UserInfo() });
            return Ok(obj);
        }



    }
}
