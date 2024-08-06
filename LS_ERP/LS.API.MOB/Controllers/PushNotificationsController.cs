using CIN.Application;
using CIN.Application.Common;
using CIN.Application.MobileMgt.Dtos;
using CIN.Application.MobileMgt.Queries;
using CIN.Application.OperationsMgtDtos;
using CIN.Application.SystemQuery;
using CIN.Domain.MobileMgt;
using CIN.Server;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LS.API.MOB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PushoNotificationsController : ApiControllerBase
    {
        private readonly CINServerDbContext _cinDbContext;
        private IConfiguration _Config;
        private readonly IWebHostEnvironment _env;
        private readonly IOptions<AppMobileSettingsJson> _appSettings;
        //  public ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public PushoNotificationsController(IConfiguration config, CINServerDbContext cinDbContext, IOptions<AppMobileSettingsJson> appSettings, IWebHostEnvironment env)
        {
            _Config = config;
            _cinDbContext = cinDbContext;
            _appSettings = appSettings;
            _env = env;
        }

    
         [HttpPost("pushNotificationAction")]
        public async Task<ActionResult> PushNotificationAction([FromBody] TblErpSysPushNotification input)
        {

            
                var res = await Mediator.Send(new PushNotificationActionQuery(){Input = input});

            return Ok(res);
        }
     

    }

}