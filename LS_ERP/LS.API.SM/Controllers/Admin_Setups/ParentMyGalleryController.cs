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
    public class ParentMyGalleryController :BaseController
    {
        private IConfiguration _Config;

        public ParentMyGalleryController(IOptions<AppSettingsJson> appSettings, IConfiguration config) : base(appSettings)
        {
            _Config = config;
        }

        [HttpGet("GetParentMyGalleryList")]
        public async Task<IActionResult> Get([FromQuery] string mobile)
        {
            var list = await Mediator.Send(new GetAllParentMyGalleryByMobile() { Mobile = mobile, User = UserInfo() });
            return Ok(list);
        }
    }
}
