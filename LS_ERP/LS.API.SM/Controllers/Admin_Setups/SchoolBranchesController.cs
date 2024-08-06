using CIN.Application;
using CIN.Application.Common;
using CIN.Application.SchoolMgtDto;
using CIN.Application.SchoolMgtQuery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LS.API.SM.Controllers.Admin_Setups
{
    public class SchoolBranchesController : BaseController
    {
        private readonly IConfiguration _Config;

        public SchoolBranchesController(IOptions<AppSettingsJson> appSettings, IConfiguration config) : base(appSettings)
        {
            _Config = config;
            //_cinDbContext = cinDbContext;
        }

        [HttpGet("getSchoolBranchList")]
        public async Task<IActionResult> GetSchoolBranchList()
        {
            var item = await Mediator.Send(new GetSchoolBranchList() { User = UserInfo() });
            return Ok(item);
        }
    }
}
