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

namespace LS.API.SM.Controllers.SchoolNews
{
    public class SchoolNewsController : BaseController
    {
        private readonly IConfiguration _Config;

        public SchoolNewsController(IOptions<AppSettingsJson> appSettings, IConfiguration config) : base(appSettings)
        {
            _Config = config;
            //_cinDbContext = cinDbContext;
        }

        [HttpGet("SchoolNewsList")]
        public async Task<IActionResult> Get([FromQuery] DateTime newsDate)
        {

            var list = await Mediator.Send(new GetSchoolNewsList() { NewsDate = newsDate, User = UserInfo() });
            return Ok(list);
        }
    }
}
