using CIN.Application;
using CIN.Application.Common;
using CIN.Application.SchoolMgtDtos;
using CIN.Application.SchoolMgtQuery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LS.API.SM.Controllers.StudentMgmt
{
    public class StudentApplyLeaveController :BaseController
    {
        private IConfiguration _Config;

        public StudentApplyLeaveController(IOptions<AppSettingsJson> appSettings, IConfiguration config) : base(appSettings)
        {
            _Config = config;
        }



        #region GetStudentApplyLeaveList
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var obj = await Mediator.Send(new GetStudentApplyLeaveList() { User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        #endregion

        [HttpGet("GetStudentLeaves")]
        public async Task<IActionResult> GetStudentLeaves([FromQuery] PaginationFilterDto filter)
        {

            var list = await Mediator.Send(new GetStudentLeaves() { Input = filter, User = UserInfo() });
            return Ok(list);
        }

        [HttpGet("GetStudentLeaveById/{id}")]
        public async Task<IActionResult> GetStudentLeaveById([FromRoute] int id)
        {
            var obj = await Mediator.Send(new GetStudentApplyLeaveById() { Id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("GetLeaveCodes")]
        public async Task<IActionResult> GetLeaveCodes()
        {
            var obj = await Mediator.Send(new GetLeaveCodes() { User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }


        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TblDefStudentApplyLeaveDto dTO)
        {

            var id = await Mediator.Send(new CreateUpdateStudentLeaveApply() { Input = dTO, User = UserInfo() });
            if (id > 0)
                return Created($"get/{id}", dTO);
            else if (id == -1)
            {
                return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Duplicate(nameof(dTO.Id)) });
            }
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }


        [HttpDelete("id")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var studentAttn = await Mediator.Send(new DeleteStudentLeaveApply() { Id = id, User = UserInfo() });
            if (studentAttn > 0)
                return NoContent();
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }
     


    }
}
