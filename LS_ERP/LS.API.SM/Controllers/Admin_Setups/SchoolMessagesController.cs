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

namespace LS.API.SM.Controllers.Admin_Setups
{
    public class SchoolMessagesController : BaseController
    {
        private readonly IConfiguration _Config;

        public SchoolMessagesController(IOptions<AppSettingsJson> appSettings, IConfiguration config) : base(appSettings)
        {
            _Config = config;
            //_cinDbContext = cinDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int mobile)
        {

            var list = await Mediator.Send(new GetSchoolMessageDetails() { Mobile = mobile, User = UserInfo() });
            return Ok(list);
        }
    }
}
