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
    public class ContractFormTemplatesController : BaseController
    {

        public ContractFormTemplatesController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }


        [HttpPost]
        public async Task<ActionResult> Create([FromBody] ContractFormTemplateDto dTO)
        {
            var id = await Mediator.Send(new CreateContractFormTemplate() { ContractFormTemplateDto = dTO, User = UserInfo() });
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


        [HttpGet("getContractFormTemplateById/{Id}")]
        public async Task<IActionResult> Get([FromRoute] long Id)
        {
            var obj = await Mediator.Send(new GetContractFormTemplateById() { Id = Id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("getSelectionContractFormTemplates/{type}")]
        public async Task<List<CustomSelectListItem>> GetSelectionContractFormTemplates( [FromRoute] string Type)
        {
            var users = await Mediator.Send(new GetContractFormTemplatesSelectionList() {User = UserInfo() ,Type=Type});
            return users;
        }

         
        


        [HttpGet("getContractFormtemplatesPagedList")]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {

            var list = await Mediator.Send(new GetContractFormTemplatesPagedList() { Input = filter.Values(), User = UserInfo() });
            return Ok(list);
        }




    }
}
