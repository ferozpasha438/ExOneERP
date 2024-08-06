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
    public class TeacherLessonPlanController:BaseController
    {
        private IConfiguration _Config;

        public TeacherLessonPlanController(IOptions<AppSettingsJson> appSettings, IConfiguration config) : base(appSettings)
        {
            _Config = config;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TeacherLessonPlanInfoDto dTO)
        {
            var list = await Mediator.Send(new CreateTeacherLessonPlanInfo() { LessonPlanInfoDto = dTO, User = UserInfo() });
            return Ok(list);
        }

        [HttpGet("TeacherLessonPlanInfoList")]
        public async Task<IActionResult> TeacherLessonPlanInfoList([FromQuery] string teacherCode,string branchCode,string gradeCode)
        {
            var list = await Mediator.Send(new GetLessonPlanList() { TeacherCode = teacherCode,BranchCode=branchCode,GradeCode=gradeCode, User = UserInfo() });
            return Ok(list);
        }

        [HttpGet("GetLessonPlanInfoById")]
        public async Task<IActionResult> GetLessonPlanInfoById([FromQuery] int id)
        {
            var obj = await Mediator.Send(new GetLessonPlanInfoById() { Id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }



        [HttpGet("GetLessonPlanDetailsById")]
        public async Task<IActionResult> GetLessonPlanDetailsById([FromQuery] int id)
        {
            var obj = await Mediator.Send(new GetLessonPlanDetailsById() { Id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }


    

        [HttpDelete("DeleteLessonPlanDetailById")]
        public async Task<IActionResult> DeleteLessonPlanDetailById([FromQuery] int id)
        {
            var LPDetailsId = await Mediator.Send(new DeleteLessonPlanDetailById() { Id = id, User = UserInfo() });
            if (LPDetailsId > 0)
                return Ok(new MobileApiMessageDto { Message= "Successfully Deleted", Status=true});
            return BadRequest(new MobileApiMessageDto { Message = ApiMessageInfo.Failed,Status=false});
        }

        [HttpDelete("DeleteLessonPlanList")]
        public async Task<IActionResult> DeleteLessonPlanList([FromQuery] int id)
        {
            var LPlanId = await Mediator.Send(new DeleteLessonPlanList() { Id = id, User = UserInfo() });
            if (LPlanId > 0)
                return Ok(new MobileApiMessageDto { Message = "Successfully Deleted", Status=true });
            return BadRequest(new MobileApiMessageDto { Message = ApiMessageInfo.Failed,Status=false });
        }
    }
}
