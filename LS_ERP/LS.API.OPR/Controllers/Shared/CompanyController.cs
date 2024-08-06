using CIN.Application;
using CIN.Application.Common;
using CIN.Application.FinanceMgtQuery;
using CIN.Application.OperationsMgtDtos;
using CIN.Application.OperationsMgtQuery;
using CIN.Domain.SystemSetup;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LS.API.OPR.Controllers.Shared
{
   
    public class CompanyController : BaseController
    {
        public CompanyController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }


        [HttpGet("getCompanyByCityCode/{cityCode}")]
        public async Task<TblErpSysCompany> Get([FromRoute] string cityCode)
        {
            var obj = await Mediator.Send(new GetCompanyByCityCode() { CityCode = cityCode, User = UserInfo() });
            return obj ;
        }


    }
}
