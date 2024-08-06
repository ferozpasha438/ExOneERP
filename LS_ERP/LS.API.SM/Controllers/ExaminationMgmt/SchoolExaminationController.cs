using AutoMapper.Configuration;
using CIN.Application;
using CIN.Application.Common;
using CIN.Application.ExaminationMgmtQuery;
using CIN.Application.SchoolMgtDtos;
using CIN.Application.SchoolMgtQuery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace LS.API.SM.Controllers.ExaminationMgmt
{
    public class SchoolExaminationController :BaseController
    {
        private readonly IConfiguration _Config;

        public SchoolExaminationController(IOptions<AppSettingsJson> appSettings, IConfiguration config) : base(appSettings)
        {
            _Config = config;
        }


        //[HttpGet("{GetStudentExaminationList}")]
        //public async Task<IActionResult> Get([FromQuery] string gradeCode,string branchCode)
        //{
        //    var obj = await Mediator.Send(new GetStudentExaminationList(){GradeCode=gradeCode, BranchCode=branchCode, User = UserInfo() });
        //    return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        //}
    }
}
