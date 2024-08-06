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
    public class BranchExamScheduleController : BaseController
    {
        private IConfiguration _Config;

        public BranchExamScheduleController(IOptions<AppSettingsJson> appSettings,IConfiguration config) : base(appSettings)
        {
            _Config = config;
        }


        [HttpGet("GetBranchExamScheduleList")]
        public async Task<IActionResult> Get([FromQuery]  string branchCode)
        {
            var obj = await Mediator.Send(new GetExamSheduleOfBranch() { BranchCode = branchCode, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
    }
}
