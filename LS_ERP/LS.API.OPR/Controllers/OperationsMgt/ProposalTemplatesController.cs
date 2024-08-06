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
    public class ProposalTemplatesController : BaseController
    {

        public ProposalTemplatesController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }


        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TblOpProposalTemplateDto dTO)
        {
            var id = await Mediator.Send(new CreateProposalTemplate() { ProposalTemplateDto = dTO, User = UserInfo() });
            if (id > 0)
            {
                return Created($"get/{id}", dTO);
            }
            else if (id == -1)
            {
                return BadRequest(new ApiMessageDto { Message = "You Cannot Update the Template" });
            }
      
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });

        }


        [HttpGet("getProposalTemplateById/{Id}")]
        public async Task<IActionResult> Get([FromRoute] long Id)
        {
            var obj = await Mediator.Send(new GetProposalTemplateById() { Id = Id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        //[HttpGet("getSelectionProposalTemplates/{customerCode}/{projectCode}/{sitCode}")]
        //public async Task<List<CustomSelectListItem>> GetSelectionProposalTemplates([FromRoute] string CustomerCode, [FromRoute] string ProjectCode, [FromRoute] string SiteCode)
        //{
        //    var users = await Mediator.Send(new GetProposalTemplatesSelectionList() {User = UserInfo(),CustomerCode=CustomerCode,ProjectCode=ProjectCode,SiteCode=SiteCode });
        //    return users;
   // }     
    
    [HttpPost("getSelectionProposalTemplates")]
    public async Task<List<CustomSelectListItem>> GetSelectionProposalTemplates([FromBody] InputCustomerProjectSite Dto)
    {
        var users = await Mediator.Send(new GetProposalTemplatesSelectionList() { User = UserInfo(), Input=Dto });
        return users;
    }







}
}
