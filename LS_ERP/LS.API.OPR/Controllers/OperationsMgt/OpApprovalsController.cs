using CIN.Application;
using CIN.Application.Common;
using CIN.Application.OperationsMgtDtos;
using CIN.Application.OperationsMgtQuery;
using CIN.Domain.OpeartionsMgt;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LS.API.OPR.Controllers.OperationsMgt
{
    public class OpApprovalsController : BaseController
    {

        public OpApprovalsController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }

       
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TblOprTrnApprovalsDto dTO)
        {
            var id = await Mediator.Send(new CreateOpApprovals() { Input = dTO, User = UserInfo() });
            if (id > 0)
                return Created($"get/{id}", dTO);
            else if (id == -1)
            {
                return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Duplicate(nameof(dTO.AppAuth)) });
            }
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }

     

        [HttpGet("getOpApprovalsByUserId")]
        public async Task<IActionResult> GetOpAuthoritiesByUserId()
        {
            var obj = await Mediator.Send(new GetOpApprovalsByUserId() {User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        

      

       
     
    }
}
