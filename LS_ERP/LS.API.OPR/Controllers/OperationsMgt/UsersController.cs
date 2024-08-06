using CIN.Application;
using CIN.Application.Common;
using CIN.Application.OperationsMgtDtos;
using CIN.Application.OperationsMgtQuery;
using CIN.Domain.OpeartionsMgt;
using CIN.Domain.SystemSetup;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LS.API.OPR.Controllers.OperationsMgt
{
    public class UsersController : BaseController
    {

        public UsersController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }

       


     

        [HttpGet("getUserByUserId/{UserId}")]
        public async Task<IActionResult> Get([FromRoute] int UserId)
        {
            var obj = await Mediator.Send(new GetUserByUserId() { Userid = UserId, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("GetUserSelectionList")]
        public async Task<List<CustomSelectListItem>> GetUserSelectionList()
        {
            var users = await Mediator.Send(new GetUserSelectionList() {User = UserInfo() });
            return users;
        }

        

      

       
     
    }
}
