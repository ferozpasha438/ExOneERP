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
    public class LessonPlanDetailsController : BaseController
    {
        private IConfiguration _config;

        public LessonPlanDetailsController(IOptions<AppSettingsJson> appSettings, IConfiguration config) : base(appSettings)
        {
            _config = config;
        }
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] DateTime fromDate, DateTime todate, string grade, string section,string branch)
        {
            var list = await Mediator.Send(new GetLessonPlanDetailList() { FromDate = fromDate, ToDate= todate, Grade = grade, Section = section,Branch=branch, User = UserInfo() });
            return Ok(list);
        }
    }
}
