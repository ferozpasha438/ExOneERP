using CIN.Application;
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
    public class PersonalInformationController : BaseController
    {
        public PersonalInformationController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {

        }

        [HttpGet("GetEmployeePersonalInformationById")]
        public async Task<IActionResult> GetEmployeePersonalInformationById([FromQuery] string employeeNumber)
        {
            var obj = await Mediator.Send(new GetEmployeePersonalInformationById() { EmployeeNumber = employeeNumber, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
    }
}
