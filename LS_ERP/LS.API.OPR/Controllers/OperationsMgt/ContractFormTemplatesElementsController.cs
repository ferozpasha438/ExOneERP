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
    public class ContractFormTemplateElementsController : BaseController
    {

        public ContractFormTemplateElementsController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }


        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TblOpContractClauseDto dTO)
        {
            var id = await Mediator.Send(new CreateContractFormTemplateElement() { Input = dTO, User = UserInfo() });
            if (id > 0)
            {
                return Created($"get/{id}", dTO);
            }
            
      
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });

        }


        [HttpGet("getContractFormTemplateElementById/{Id}")]
        public async Task<IActionResult> Get([FromRoute] long Id)
        {
            var obj = await Mediator.Send(new GetContractFormTemplateElementById() { Id = Id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }



        [HttpGet("getSelectionContractFormTemplateElements")]
        public async Task<List<CustomSelectListItem>> GetSelectionContractFormTemplateElements()
        {
            var clauses = await Mediator.Send(new GetContractFormTemplateElementsSelectionList() {User = UserInfo() });
            return clauses;
        }
         [HttpGet("getAllContractFormTemplateElements")]
        public async Task<List<TblOpContractClauseDto>> GetAllContractFormTemplateElements()
        {
            var clauses = await Mediator.Send(new GetAllContractFormTemplateElementsSelectionList() {User = UserInfo() });
            return clauses;
        }


        [HttpGet("getContractFormTemplateElementsPagedList")]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {

            var list = await Mediator.Send(new GetContractFormTemplateElementsPagedList() { Input = filter.Values(), User = UserInfo() });
            return Ok(list);
        }




    }
}
