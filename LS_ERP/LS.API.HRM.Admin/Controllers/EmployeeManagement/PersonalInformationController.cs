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
    public class PersonalInformationController : BaseController
    {
        public PersonalInformationController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {

        }
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {
            var list = await Mediator.Send(new GetEmployeeList() { Input = filter.Values(), User = UserInfo() });
            return Ok(list);
        }

        [HttpGet("GetEmployeePersonalInformationById")]
        public async Task<IActionResult> GetEmployeePersonalInformationById([FromQuery] string employeeNumber)
        {
            var obj = await Mediator.Send(new GetEmployeePersonalInformationById() { EmployeeNumber = employeeNumber, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("getEmployeeSelectListItem")]
        public async Task<IActionResult> GetEmployeeSelectListItem()
        {
            var obj = await Mediator.Send(new GetEmployeeSelectListItem() { User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TblHRMTrnPersonalInformationDto dTO)
        {
            var result = await Mediator.Send(new CreateUpdatePersonalInformation() { Input = dTO, User = UserInfo() });

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
