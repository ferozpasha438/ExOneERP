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

namespace LS.API.SM.Controllers.Notifications
{
    public class DisciplinaryActionController :BaseController
    {
        private IConfiguration _Config;

        public DisciplinaryActionController(IOptions<AppSettingsJson> appSetting,IConfiguration config):base(appSetting)
        {
            _Config = config;
        }

        [HttpGet("StudentDisciplinaryActionList")]
        public async Task<IActionResult> Get([FromQuery] string stuAdmNum)
        {
            var list = await Mediator.Send(new GetSchoolDisciplinaryActionList() { StuAdmNum = stuAdmNum, User = UserInfo() });
            return Ok(list);
        }
    }
}
