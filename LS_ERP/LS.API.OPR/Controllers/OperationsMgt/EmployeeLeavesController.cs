using CIN.Application;
using CIN.Application.Common;
using CIN.Application.OperationsMgtDtos;
using CIN.Application.OperationsMgtQuery;
using CIN.Domain.OpeartionsMgt;
using CIN.Domain.SystemSetup;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LS.API.OPR.Controllers.OperationsMgt
{
    public class EmployeeLeavesController : BaseController
    {

        public EmployeeLeavesController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }






        [HttpPost("enterEmployeeLeave")]
        public async Task<ActionResult> EnterEmployeeLeave([FromBody] TblOpEmployeeLeavesDto dto)
        {
            var id = await Mediator.Send(new EnterEmployeeLeave() { Input = dto, User = UserInfo() });
            if (id > 0)
            {
                return Created($"get/{id}", dto);
            }
            else if (id == -1)
            {
                return BadRequest(new ApiMessageDto { Message = "Employee_Attendance_Already_Posted" });

            }

            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });

        }





        //[HttpDelete]
        //public async Task<ActionResult> Delete([FromRoute] long id)
        //{
        //    var EmployeeId = await Mediator.Send(new CancelLeave() { Id = id, User = UserInfo() });
        //    if (EmployeeId > 0)
        //        return NoContent();
        //    return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        //}

        [HttpGet("cancelLeave/{id}")]
        public async Task<ActionResult> CancelLeave([FromRoute] long id)
        {
            var res = await Mediator.Send(new CancelLeave() { Id = id, User = UserInfo() });
            if (res > 0)
                return NoContent();
            else if (res==-1)
            {
                return BadRequest(new ApiMessageDto { Message ="Invalid Request" });
            }
            else if (res==-2)
            {
                return BadRequest(new ApiMessageDto { Message = "You Can't Cancel Withdrawal:Attedance Is Already Posted and Approved in HRM" });
            }
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }











    }



}
