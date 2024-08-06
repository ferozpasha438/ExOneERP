using CIN.Application;
using CIN.Application.HumanResource.EmployeeMgmt.HRMgmtDtos;
using CIN.Application.HumanResource.EmployeeMgmt.HRMgmtQuery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LS.API.HRM.Admin.Controllers.EmployeeManagement
{
    public class EmployeeLeaveController : BaseController
    {
        public EmployeeLeaveController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var obj = await Mediator.Send(new GetEmployeeLeaveInformationById() { EmployeeID = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] BaseEmployeeLeaveInformationDto dTO)
        {
            var result = await Mediator.Send(new CreateUpdateEmployeeLeaveInformation() { Input = dTO, User = UserInfo() });

            if (result.Id > 0)
                return NoContent();

            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }

        [HttpGet("GetEmployeeLeaveTemplateMappings")]
        public async Task<IActionResult> GetEmployeeLeaveTemplateMappings([FromQuery] string templateCode)
        {
            var obj = await Mediator.Send(new GetEmployeeLeaveTemplateMappings() { TemplateCode = templateCode, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
    }
}
