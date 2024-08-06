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
    public class ProposalCostingController : BaseController
    {

        public ProposalCostingController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] List<TblOpProposalCosting> dTOs)
        {
            var id = await Mediator.Send(new CreateProposalCosting() { Dtos = dTOs, User = UserInfo() });
            if (id > 0)
            {
                return Created($"get/{id}", dTOs);
            }
            

            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });

        }


        //[HttpGet("getProposalCosting/{customerCode}/{projectCode}/{sitCode}")]
        //public async Task<List<TblOpProposalCostingDto>> GetProposalCosting([FromRoute] string CustomerCode, [FromRoute] string ProjectCode, [FromRoute] string SiteCode)
        //{
        //    var list = await Mediator.Send(new GetProposalCosting() {User = UserInfo(),CustomerCode=CustomerCode,ProjectCode=ProjectCode,SiteCode=SiteCode });
        //    return list;
        //}

        [HttpPost("getProposalCosting")]
        public async Task<List<TblOpProposalCostingDto>> GetProposalCosting([FromBody] InputCustomerProjectSite Dto)
        {
            var list = await Mediator.Send(new GetProposalCosting() {User = UserInfo(),Input=Dto});
            return list;
        }

        

      

       
     
    }
}
