using CIN.Application;
using CIN.Application.Common;
using CIN.Application.OperationsMgtDtos;
using CIN.Application.OperationsMgtQuery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LS.API.OPR.Controllers.OperationsMgt
{
    public class TestController : BaseController
    {

        public TestController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }

     
        [HttpPost("CopyEmployees")]
        public async Task<ActionResult> CopyEmployees()
        {
            var id = await Mediator.Send(new CopyEmployees() { User = UserInfo() });
            if (id > 0)
                return Ok(new ApiMessageDto { Message = ApiMessageInfo.Success });
            
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }
         [HttpPost("CopyShifts")]
        public async Task<ActionResult> CopyShifts()
        {
            var id = await Mediator.Send(new CopyShifts() { User = UserInfo() });
            if (id > 0)
                return Ok(new ApiMessageDto { Message = ApiMessageInfo.Success });
            
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }




        [HttpPost("CopyProjectsAndSitesDataToHRM")]
        public async Task<ActionResult> CopyProjectsToHRM()
        {
            var id = await Mediator.Send(new CopyProjectsAndSitesDataToHRM() { User = UserInfo() });
            if (id > 0)
                return Ok(new ApiMessageDto { Message = ApiMessageInfo.Success });
            else if (id == -2)
            {
                return BadRequest(new ApiMessageDto { Message = "No Projects in Operations" });
            }
            else if(id==-1)
            {
                return BadRequest(new ApiMessageDto { Message = "No Sites in DMC" });
            }
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }
    }
}
