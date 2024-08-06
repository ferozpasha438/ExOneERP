using CIN.Application;
using CIN.Application.Common;
using CIN.Application.OperationsMgtDtos;
using CIN.Application.OperationsMgtQuery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LS.API.OPR.Controllers.OperationsMgt
{
    public class EmployeeShiftsController : BaseController
    {

        public EmployeeShiftsController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }


        [HttpPost]
        public async Task<ActionResult> Create([FromBody] HRM_DEF_EmployeeShiftDto dTO)
        {
            var id = await Mediator.Send(new CreateEmployeeShifts() { EmployeeShiftDto = dTO, User = UserInfo() });
            if (id > 0)
            {
                return Created($"get/{id}", dTO);
            }
            else if (id == -1)
            {
                return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Duplicate(nameof(dTO.ID)) });
            }
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });

        }

       

        [HttpGet("getEmployeeShiftsByEmployeeID/{EmployeeID}")]
        public async Task<IActionResult> Get([FromRoute] long EmployeeID)
        {
            var obj = await Mediator.Send(new GetEmployeeShiftsByEmployeeID() { EmployeeID = EmployeeID, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
    }
}
