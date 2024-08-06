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
    public class PostingMonthlyAttendanceController : BaseController
    {

        public PostingMonthlyAttendanceController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }






        [HttpPost("postEmployeeAttendance")]
        public async Task<ActionResult> PostEmployeeAttendance([FromBody] List<TblOpEmployeeAttendanceDto> dto)
        {
            var id = await Mediator.Send(new PostEmployeeAttendance() { Input = dto, User = UserInfo() });
            if (id > 0)
            {
                return Created($"get/{id}", dto);
            }
            else if (id==-1) {
                return BadRequest(new ApiMessageDto { Message ="Base Attendance Not Found" });

            } else if (id==-2) {
                return BadRequest(new ApiMessageDto { Message ="Base Attendance Already Posted" });

            }
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });

        }
        [HttpPost("postEmployeeAttendanceWithDate")]
        public async Task<ActionResult> PostEmployeeAttendanceWithDate([FromBody] InputPostingAttendanceWithDate dto)
        {
            var res = await Mediator.Send(new PostEmployeeAttendanceWithDate() { Input = dto, User = UserInfo() });
            if (res > 0)
            {
                return Ok(dto);
            }
            else if (res==0) {
                return BadRequest(new ApiMessageDto { Message ="No Updates Found" });

            } 
            else if (res<0) {
                return BadRequest(new ApiMessageDto { Message ="Error:"+res.ToString() });

            } 

            
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });

        }







   
      

    }
}
