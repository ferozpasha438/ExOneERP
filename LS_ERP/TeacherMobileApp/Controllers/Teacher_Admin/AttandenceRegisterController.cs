using AutoMapper;
using CIN.Application;
using CIN.Application.Common;
using CIN.Application.TeacherMgtDtos;
using CIN.Application.TeacherAppMgtQuery;
using CIN.DB;
using CIN.Server;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LS.API.TeacherApp.Controllers.Teacher_Admin
{
    public class AttandenceRegisterController :BaseController
    {
        private IConfiguration _Config;

        public AttandenceRegisterController(IOptions<AppSettingsJson> appSettings, IConfiguration config) : base(appSettings)
        {
            _Config = config;
        }

        [HttpGet("TeacherAttendanceRegisterList")]
        public async Task<IActionResult> TeacherAttendanceRegisterList([FromQuery] DateTime date,[FromQuery] string branchCode, [FromQuery] string gradeCode, [FromQuery] string sectionCode,[FromQuery] string teacherCode)
        {
            var list = await Mediator.Send(new TeacherAttendanceRegisterList() {Date=date, BranchCode = branchCode, GradeCode = gradeCode, SectionCode = sectionCode, TeacherCode=teacherCode, User = UserInfo() });
            return Ok(list);
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TeacherAttendanceRegisterDto dTO)
        {
            var id = await Mediator.Send(new CreateAttRegisterData() { AttRegisterData = dTO, User = UserInfo() });
            if (id > 0)
                return Created($"get/{id}", dTO);
            else if (id == -1)
            {
                return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Duplicate(nameof(dTO.Id)) });
            }
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }


        [HttpPost("CloseAttendance")]
        public async Task<IActionResult> CloseAttendance([FromBody] TeacherAttendanceRegisterDto dTO)
        {
            var id = await Mediator.Send(new CloseAttendance() { AttRegisterData = dTO, User = UserInfo() });
            if (id > 0)
                return Created($"get/{id}", dTO);
            else if (id == -1)
            {
                return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Duplicate(nameof(dTO.Id)) });
            }
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }

    }



}
