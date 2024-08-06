//using CIN.Application;
//using CIN.Application.Common;
//using CIN.Application.MobileMgt.Dtos;
//using CIN.Application.MobileMgt.Queries;
//using CIN.Application.SystemQuery;
//using CIN.Server;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Options;
//using Microsoft.IdentityModel.Tokens;
//using System;
//using System.Collections.Generic;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;
//using System.Threading.Tasks;

//namespace LS.API.MOB.Controllers
//{
//    public class CheckGeoLocationController : BaseController
//    {
//        public CheckGeoLocationController(IOptions<AppMobileSettingsJson> appSettings) : base(appSettings)
//        {
//        }
        
//        [HttpPost]
//        public async Task<IActionResult> Create([FromBody] CheckGeoLocationDto input)
//        {
//            var obj = await Mediator.Send(new CheckGeoLocation() { Input = input, User = UserInfo() });
//            return obj.Status ? Ok(obj) : BadRequest(obj);
//        }


//    }
//}
