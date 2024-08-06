using CIN.Application;
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
    public class EmployeePayrollStructuredController : BaseController
    {
        public EmployeePayrollStructuredController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var obj = await Mediator.Send(new GetEmployeePayrollStructuredById() { EmployeeID = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] BaseEmployeePayrollStructureDto dTO)
        {
            var result = await Mediator.Send(new CreateUpdateEmployeePayrollStructure() { Input = dTO, User = UserInfo() });

            if (result.Id > 0)
            {
                if (dTO.Id > 0)
                    return NoContent();
                else
                    return Created($"get/{result.Id}", dTO);
            }
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }
    }
}
