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
    public class EmployeesToResorceMapController : BaseController
    {

        public EmployeesToResorceMapController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }






        [HttpPost("mapEmployeesToResources")]
        public async Task<ActionResult> MapEmployeesToResources([FromBody] List<TblOpEmployeeToResourceMapDto> dto)
        {
            var id = await Mediator.Send(new MapEmployeesToResources() { Input = dto, User = UserInfo() });
            if (id > 0)
            {
                return Ok(new ApiMessageDto { Message = ApiMessageInfo.Success});
            }
            
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });

        }



        [HttpGet("getEmployeesToResourcesMapForProjectSite/{projectCode}/{siteCode}")]
        public async Task<IActionResult> GetEmployeesToResourcesMapForProjectSite([FromRoute] string projectCode, [FromRoute] string siteCode)
        {
            var obj = await Mediator.Send(new GetEmployeesToResourcesMapForProjectSite() { ProjectCode = projectCode,  SiteCode = siteCode, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }



        


    }
}
