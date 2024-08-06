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
    public class ContractFormController : BaseController
    {

        public ContractFormController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }


        [HttpPost]
        public async Task<ActionResult> Create([FromBody] ContractFormDto dTO)
        {
            var id = await Mediator.Send(new CreateContractForm() { contractFormDto = dTO, User = UserInfo() });
            if (id > 0)
            {
                return Created($"get/{id}", dTO);
            }
         
      
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });

        }


       
        //[HttpGet("getContractForm/{customerCode}/{projectCode}/{siteCode}")]
        //public async Task<ContractFormDto> GetContractForm([FromRoute] string CustomerCode, [FromRoute] string ProjectCode, [FromRoute] string SiteCode)
        //{
        //    var contractForm = await Mediator.Send(new GetContractForm() {User = UserInfo(),CustomerCode=CustomerCode,ProjectCode=ProjectCode,SiteCode=SiteCode });
        //    return contractForm;
        //}

          [HttpPost("getContractForm")]
        public async Task<ContractFormDto> GetContractForm([FromBody] InputCustomerProjectSite Dto)
        {
            var contractForm = await Mediator.Send(new GetContractForm() {User = UserInfo(),Input=Dto });
            return contractForm;
        }

        

      

       
     
    }
}
