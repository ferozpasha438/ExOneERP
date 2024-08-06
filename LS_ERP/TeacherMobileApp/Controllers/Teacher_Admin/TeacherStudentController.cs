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
    public class TeacherStudentController : BaseController
    {
        private IConfiguration _Config;

        public TeacherStudentController(IOptions<AppSettingsJson> appSettings, IConfiguration config): base(appSettings)
        {
            _Config = config;
        }

        [HttpGet("GetTeacherStudent")]
        public async Task<IActionResult> GetTeacherStudent([FromQuery] string branchCode,string gradeCode)
        {
            var list = await Mediator.Send(new GetTeacherStudentList() { BranchCode = branchCode, GradeCode= gradeCode, User = UserInfo() });
            return Ok(list);
        }

    }
}
