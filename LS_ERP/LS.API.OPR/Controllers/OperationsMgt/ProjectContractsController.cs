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
    public class ProjectContractsController : BaseController
    {

        public ProjectContractsController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }


        //[HttpPost]
        //public async Task<ActionResult> Create([FromBody] ContractForProjectDto dTO)
        //{
        //    var id = await Mediator.Send(new CreateContract() { ContractDto = dTO, User = UserInfo() });
        //    if (id > 0)
        //    {
        //        return Created($"get/{id}", dTO);
        //    }

        //    return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });

        //}


        //[HttpGet("getContractByProjectCode/{ProjectCode}")]
        //public async Task<IActionResult> Get([FromRoute] string ProjectCode)
        //{
        //    var obj = await Mediator.Send(new GetContractByProjectCode() { ProjectCode = ProjectCode, User = UserInfo() });
        //    return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        //}

        //  [HttpGet("getContractByProjectAndSiteCode/{ProjectCode}/{SiteCode}")]
        //public async Task<IActionResult> GetContractByProjectAndSiteCode([FromRoute] string ProjectCode,[FromRoute] string SiteCode)
        //{
        //    var obj = await Mediator.Send(new GetContractByProjectAndSiteCode() { ProjectCode = ProjectCode,SiteCode=SiteCode, User = UserInfo() });
        //    return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        //}



        //[HttpPost("createSiteContract")]
        //public async Task<ActionResult> CreateSiteContract([FromBody] ContractForProjectDto dTO)                        //Adendum
        //{
        //    var id = await Mediator.Send(new CreateSiteContract() { ContractDto = dTO, User = UserInfo() });
        //    if (id > 0)
        //    {
        //        return Created($"get/{id}", dTO);
        //    }

        //    return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });

        //}


        [HttpPost("approveContractForm")]           //contract Formhead and project isConverted to contract needs to update
        public async Task<ActionResult> ApproveContractForm([FromBody] InputApproveContractFormDto dTO)                      
        {
            var id = await Mediator.Send(new ApproveContractForm() { Input = dTO, User = UserInfo() });
            if (id > 0)
            {
                return Created($"get/{id}", dTO);
            }
            else if (id==-1)
            {
                return BadRequest(new ApiMessageDto { Message = "Invalid Contract Head" });
            }
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });

        }








    }
}
