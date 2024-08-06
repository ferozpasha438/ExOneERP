using CIN.Application;
using CIN.Application.Common;
using CIN.Application.MobileMgt.Queries;
using CIN.Application.SystemQuery;
using CIN.Server;
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
    public class LogoutController : BaseController
    {
        private readonly CINServerDbContext _cinDbContext;

        public LogoutController(IOptions<AppMobileSettingsJson> appSettings, CINServerDbContext cinDbContext) : base(appSettings)
        {
            _cinDbContext = cinDbContext;
        }


        #region LogOut


        [HttpPost]
        public async Task<IActionResult> LogOut([FromBody] CheckCINServerDto input)
        {
            Log.Info("logout() Method Begins");
            await Mediator.Send(new SetSignInStatus() { Input = false, User = UserInfo() });

            var metaData = await _cinDbContext.MetaDataList.FirstOrDefaultAsync(e => e.CINNumber == input.CINNumber);
            if (metaData is not null)
            {
                if (metaData.ConnectedUsers > 0)
                {
                    metaData.ConnectedUsers--;
                    _cinDbContext.MetaDataList.Update(metaData);
                    await _cinDbContext.SaveChangesAsync();
                }
                return Ok(new APIPayload<string> { Data = "Logout Successful" });
            }

            return BadRequest(new MobileApiMessageDto { Message = "Logout Failed" });
        }


        #endregion

    }
}
