using CIN.Application;
using CIN.Application.Common;
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
    public class EmployeeQualificationController : BaseController
    {
        public EmployeeQualificationController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {

        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {
            var list = await Mediator.Send(new GetEmployeeQualificationsList() { Input = filter.Values(), User = UserInfo() });
            return Ok(list);
        }

        [HttpGet("GetEmployeeQualificationById")]
        public async Task<IActionResult> Get([FromQuery] int id, [FromQuery] int employeeID)
        {
            var obj = await Mediator.Send(new GetEmployeeQualificationById() { Id = id, EmployeeID = employeeID, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TblHRMTrnEmployeeQualificationInfoDto dTO)
        {
            var result = await Mediator.Send(new CreateUpdateEmployeeQualification() { Input = dTO, User = UserInfo() });

            if (result.Id > 0)
            {
                if (dTO.Id > 0)
                    return NoContent();
                else
                    return Created($"get/{result.Id}", dTO);
            }
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var employeeQualificationId = await Mediator.Send(new DeleteEmployeeQualification() { Id = id, User = UserInfo() });
            if (employeeQualificationId > 0)
                return NoContent();
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }
    }
}
