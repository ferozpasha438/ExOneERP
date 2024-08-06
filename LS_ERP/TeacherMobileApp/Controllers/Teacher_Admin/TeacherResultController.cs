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
    public class TeacherResultController :BaseController
    {
        private IConfiguration _Config;

        public TeacherResultController(IOptions<AppSettingsJson> appSettings, IConfiguration config) : base(appSettings)
        {
            _Config = config;
        }

    


        [HttpGet("TeacherExamResult")]
        public async Task<IActionResult> TeacherExamResult([FromQuery]  string branchCode,int academicYear,string examinationType,string admissionNumber,string grade)
        {
            var list = await Mediator.Send(new StudentExamResult() {  BranchCode = branchCode,AcademicYear=academicYear,ExaminationType=examinationType,AdmissionNumber=admissionNumber,Grade=grade, User = UserInfo() });
            return Ok(list);
        }



        [HttpGet("GetExamType")]
        public async Task<IActionResult> GetExamType([FromQuery] string gradeCode)
        {
            var list = await Mediator.Send(new GetExamTypeList() { Input = gradeCode, User = UserInfo() });
            return Ok(list);
        }
    }
}
