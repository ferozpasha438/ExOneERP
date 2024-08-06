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

namespace LS.API.Sales.Controllers.SND
{
   
    public class CustomerCategoryController : BaseController
    {
        public CustomerCategoryController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }
       [HttpGet("getSelectCustomerCategoryCodeList")]
        public async Task<IActionResult> GetSelectCompanyList(string search)
        {
            var item = await Mediator.Send(new GetCustomerCategorySelectListQuery() { Input = search, User = UserInfo() });
            return Ok(item);
        }

        [HttpGet("getCustCatByCustCatCode/{custCatCode}")]
        public async Task<TblSndDefCustomerCategoryDto> Get([FromRoute] string custCatCode)
        {
            var obj = await Mediator.Send(new GetCustCatByCustCatCode() { CustomerCatCode = custCatCode, User = UserInfo() });
            return obj;
        }



    }
}
