using CIN.Application;
using CIN.Application.Common.Models;
using CIN.Application.TimeAndAttendance.Management.TNAMgmtDtos;
using CIN.Application.TimeAndAttendance.Management.TNAMgmtQuery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LS.API.TNA.Controllers.Management
{
    public class EmployeeRoasterController : BaseController
    {
        public EmployeeRoasterController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {

        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] EmployeeRoasterFilter filter)
        {
            var list = await Mediator.Send(new GetEmployeeRoasterList() { Input = filter, User = UserInfo() });
            return Ok(list);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] BaseEmployeeRoasterDto dTO)
        {
            var result = await Mediator.Send(new CreateUpdateEmployeeRoaster() { Input = dTO, User = UserInfo() });

            if (result.Id > 0)
                return Ok(new AppCtrollerDto { Message = result.Message, Id = result.Id });

            return BadRequest(new AppCtrollerDto { Message = result.Message, Id = result.Id });
        }

        [HttpGet("GenerateEmployeesRoaster")]
        public async Task<IActionResult> GenerateEmployeesRoaster([FromQuery] EmployeeRoasterFilter filter)
        {
            var list = await Mediator.Send(new GenerateEmployeesRoaster() { Input = filter, User = UserInfo() });
            return Ok(list);
        }
    }
}
