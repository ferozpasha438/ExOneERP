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
    public class CustomerVisitFormController : BaseController
    {

        public CustomerVisitFormController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }

        //[HttpGet("getCustomerVisitFormsPagedList")]
        //public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        //{

        //    var list = await Mediator.Send(new GetCustomerVisitFormsPagedList() { Input = filter.Values(), User = UserInfo() });
        //    return Ok(list);
        //}



        [HttpGet("getCustomerVisitFormsPagedListByProjectSite/{projectCode}/{siteCode}")]
        public async Task<IActionResult> GetPvAllRequestsPagedListByProjectSite([FromQuery] OprPaginationFilterDto filter, [FromRoute] string projectCode, [FromRoute] string siteCode)
        {

            var list = await Mediator.Send(new GetCustomerVisitFormsPagedListByProjectSite() { Input = filter.Values(), ProjectCode = projectCode, SiteCode = siteCode, User = UserInfo() });
            return list is not null? Ok(list): NoContent();
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] InputTblOpCustomerVisitFormDto dTO)
        {
            var Res = await Mediator.Send(new CreateCustomerVisitForm() { CustomerVisitFormDto = dTO, User = UserInfo() });
            if (Res.IsSuccess)
            {
                return Created($"get/{dTO.Id}", dTO);
            }
            else 
            {
                return BadRequest(new ApiMessageDto { Message = Res.ErrorMsg });
            }
           

        }

        [HttpGet("getById/{id}")]
        public async Task<IActionResult> Get([FromRoute] long id)
        {
            var obj = await Mediator.Send(new GetCustomerVisitFormById() { Id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }





        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var Id = await Mediator.Send(new DeleteCustomerVisitForm() { Id = id, User = UserInfo() });
            if (Id > 0)
                return NoContent();
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }
    }
}
