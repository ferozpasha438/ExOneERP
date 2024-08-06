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
    public class TeacherHomeworkController : BaseController
    {
        private IConfiguration _Config;

        public TeacherHomeworkController(IOptions<AppSettingsJson> appSettings,IConfiguration config) : base(appSettings)
        {
            _Config = config;
        }

        [HttpGet("GetHomeworkList")]
        public async Task<IActionResult> Get([FromQuery]string teacherCode, string gradeCode,DateTime date)
        {
            var obj = await Mediator.Send(new GetTeacherHomeworkList() { TeacherCode = teacherCode,GradeCode=gradeCode,Date=date, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TeacherStudentHomeWorkDto dTO)
        {
            var id = await Mediator.Send(new InsertUpdateTeacherHomework() { TeacherHomeWorkDto = dTO, User = UserInfo() });
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
