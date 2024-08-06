using CIN.Application;
using CIN.Application.Common;
using CIN.Application.OperationsMgtDtos;
using CIN.Application.OperationsMgtQuery;
using CIN.Domain.OpeartionsMgt;
using CIN.Domain.SystemSetup;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LS.API.OPR.Controllers.OperationsMgt
{
    public class EmployeesToProjectSiteController : BaseController
    {

        public EmployeesToProjectSiteController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }






        [HttpPost("assignEmployeesToProjectSite")]
        public async Task<ActionResult> AssignEmployeesToProjectSite([FromBody] List<TblOpEmployeesToProjectSiteDto> dto)
        {
            var id = await Mediator.Send(new AssignEmployeesToProjectSite() { Input = dto, User = UserInfo() });
            if (id > 0)
            {
                return Created($"get/{id}", dto);
            }
            else if (id == -1)
            {
                return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Duplicate(nameof(dto)) });
            }
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });

        }



        [HttpGet("getEmployeesOfProjectSite/{projectCode}/{siteCode}")]
        public async Task<IActionResult> GetMonthlyRoasterForSite([FromRoute] string projectCode, [FromRoute] string siteCode)
        {
            var obj = await Mediator.Send(new GetEmployeesOfProjectSite() { ProjectCode = projectCode,  SiteCode = siteCode, User = UserInfo() });
            return Ok(obj);
        }
[HttpGet("getEmployeeOfProjectSiteByEmpNumber/{projectCode}/{siteCode}/{employeeNumber}")]
        public async Task<IActionResult> GetEmployeeOfProjectSiteByEmpNumber([FromRoute] string projectCode, [FromRoute] string siteCode,[FromRoute] string employeeNumber)
        {
            var obj = await Mediator.Send(new GetEmployeeOfProjectSiteByEmpNumber() { ProjectCode = projectCode,  SiteCode = siteCode,EmployeeNumber=employeeNumber, User = UserInfo() });
            return Ok(obj);
        }



        [HttpGet("getAutoFillEmployeeListForProjectSite/{projectCode}/{siteCode}/")]
        public async Task<IActionResult> GetAutoFillEmployeeListForProjectSite([FromRoute] string projectCode, [FromRoute] string siteCode,string Search)
        {
            var item = await Mediator.Send(new GetAutoFillEmployeeListForProjectSite() { ProjectCode = projectCode, SiteCode = siteCode, Search = Search, User = UserInfo() });
            return Ok(item);
        }



    }
}
