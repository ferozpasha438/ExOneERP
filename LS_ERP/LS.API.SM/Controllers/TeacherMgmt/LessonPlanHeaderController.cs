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

namespace LS.API.SM.Controllers.TeacherMgmt
{
    public class LessonPlanHeaderController :BaseController
    {
        private IConfiguration _config;

        public LessonPlanHeaderController(IOptions<AppSettingsJson>appSettings,IConfiguration config):base(appSettings)
        {
            _config = config;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] DateTime startDate,string grade,string branch)
        {
            var list = await Mediator.Send(new GetLessonPlanHeader() { StartDate = startDate, Grade = grade, BranchCode = branch, User = UserInfo() });
            return Ok(list);
        }
    }
}
