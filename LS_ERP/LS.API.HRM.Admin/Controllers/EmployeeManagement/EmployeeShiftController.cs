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

    public class EmployeeShiftController : BaseController
    {
        public EmployeeShiftController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {

        }

        [HttpGet("GetEmployeeShiftById")]
        public async Task<IActionResult> Get([FromQuery] int employeeID)
        {
            var obj = await Mediator.Send(new GetEmployeeShiftById() { EmployeeID = employeeID, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TblHRMTrnEmployeeShiftInfoDto dTO)
        {
            var result = await Mediator.Send(new CreateUpdateEmployeeShift() { Input = dTO, User = UserInfo() });

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
            var employeeShiftId = await Mediator.Send(new DeleteEmployeeShift() { Id = id, User = UserInfo() });
            if (employeeShiftId > 0)
                return NoContent();
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }

        [HttpGet("GetCopyFromEmployeesList")]
        public async Task<IActionResult> GetCopyFromEmployeesList()
        {
            var list = await Mediator.Send(new GetEmployeeShiftsSelectListItem() { User = UserInfo() });
            return Ok(list);
        }
    }
}
