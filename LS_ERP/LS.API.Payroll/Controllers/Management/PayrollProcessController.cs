using CIN.Application;
using CIN.Application.HumanResource.EmployeeMgmt.HRMgmtDtos;
using CIN.Application.Payroll.Management.Dtos;
using CIN.Application.Payroll.Management.Query;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LS.API.Payroll.Controllers.Management
{
    public class PayrollProcessController : BaseController
    {
        public PayrollProcessController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {

        }

        [HttpGet("{employeeID}")]
        public async Task<IActionResult> Get([FromRoute] int employeeID)
        {
            var obj = await Mediator.Send(new GetEmployeePaySlip() { EmployeeID = employeeID, User = UserInfo() });
            if (obj is not null)
            {
                return string.IsNullOrEmpty(obj.StatusMessage) ? Ok(obj) : NotFound(new ApiMessageDto { Message = obj.StatusMessage });
            }
            else
                return NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("ProcessEmployeePayroll")]
        public async Task<IActionResult> ProcessEmployeePayroll([FromQuery] int employeeID, [FromQuery] bool isApproved, [FromQuery] bool isReleased)
        {
            var obj = await Mediator.Send(new ProcessEmployeePayroll() { EmployeeID = employeeID, IsApproved = isApproved, IsReleased = isReleased, User = UserInfo() });

            if (obj is not null)
            {
                return string.IsNullOrEmpty(obj.StatusMessage) ? Ok(obj) : NotFound(new ApiMessageDto { Message = obj.StatusMessage });
            }
            else
                return NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("GetPayrollProcessFilterLog")]
        public async Task<IActionResult> GetPayrollProcessFilterLog([FromQuery] PayrollProcessFiltersDto filter)
        {
            var obj = await Mediator.Send(new GetPayrollProcessFilterLog() { Input = filter, User = UserInfo() });
            return Ok(obj);
        }

        [HttpGet("CreateUpdatePayrollProcessFilterLog")]
        public async Task<IActionResult> CreateUpdatePayrollProcessFilterLog([FromQuery] TblPRLTrnPayrollProcessFiltersLogDto dTO)
        {
            var obj = await Mediator.Send(new CreateUpdatePayrollProcessFilterLog() { Input = dTO, User = UserInfo() });
            return Ok(obj);
        }
    }
}
