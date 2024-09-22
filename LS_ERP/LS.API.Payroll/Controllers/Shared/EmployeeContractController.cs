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

namespace LS.API.Payroll.Controllers.Shared
{
    public class EmployeeContractController : BaseController
    {
        public EmployeeContractController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {

        }

        [HttpGet("GetEmployeeContractInformationById")]
        public async Task<IActionResult> GetEmployeeContractInformationById([FromQuery] int employeeID)
        {
            var obj = await Mediator.Send(new GetEmployeeContractInformationById() { EmployeeID = employeeID, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("GetEmployeeListByFilters")]
        public async Task<IActionResult> GetEmployeeListByFilters([FromQuery] EmployeeFilterDto filter)
        {
            var obj = await Mediator.Send(new GetEmployeeListByFilters() { Input = filter, User = UserInfo() });
            return Ok(obj);
        }
    }
}
