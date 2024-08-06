using CIN.Application;
using CIN.Application.Common;
using CIN.Application.OperationsMgtDtos;
using CIN.Application.OperationsMgtQuery;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LS.API.OPR.Controllers.OperationsMgt
{
    public class CustomerComplaintsController : BaseController
    {
        private readonly IConfiguration _Config;
        private readonly IWebHostEnvironment _env;
        public CustomerComplaintsController(IOptions<AppSettingsJson> appSettings, IConfiguration config, IWebHostEnvironment env) : base(appSettings)
        {
            _Config = config;
            _env = env;
        }

        //[HttpGet("getCustomerComplaintsPagedList")]
        //public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        //{

        //    var list = await Mediator.Send(new GetCustomerComplaintsPagedList() { Input = filter.Values(), User = UserInfo() });
        //    return Ok(list);
        //}



        [HttpGet("getCustomerComplaintsPagedListByProjectSite/{projectCode}/{siteCode}")]
        public async Task<IActionResult> GetPvAllRequestsPagedListByProjectSite([FromQuery] OprPaginationFilterDto filter, [FromRoute] string projectCode, [FromRoute] string siteCode)
        {

            var list = await Mediator.Send(new GetCustomerComplaintsPagedListByProjectSite() { Input = filter.Values(), ProjectCode = projectCode, SiteCode = siteCode, User = UserInfo() });
            return list is not null ? Ok(list) : NoContent();
        }


        [HttpPost]
        public async Task<ActionResult> Create([FromForm] InputTblOpCustomerComplaintDto dTO)
        {
            dTO.WebRootForComplaints = $"{_env.ContentRootPath}/ProofsForComplaints";
            dTO.WebRootForActions = $"{_env.ContentRootPath}/ProofsForActions";
           
            if (!System.IO.Directory.Exists(dTO.WebRootForComplaints))
                System.IO.Directory.CreateDirectory(dTO.WebRootForComplaints);

               if (!System.IO.Directory.Exists(dTO.WebRootForActions))
                System.IO.Directory.CreateDirectory(dTO.WebRootForActions);

            var Res = await Mediator.Send(new CreateCustomerComplaint() { CustomerComplaintDto = dTO, User = UserInfo() });
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
            var obj = await Mediator.Send(new GetCustomerComplaintById() { Id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }





        [HttpGet("startAction/{id}")]
        public async Task<ActionResult> StartAction([FromRoute] int id)
        {
            var Id = await Mediator.Send(new StartActionForCustomerComplaint() { Id = id, User = UserInfo() });
            if (Id > 0)
                return Created($"get/{id}", Id);
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }
        
        
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var Id = await Mediator.Send(new DeleteCustomerComplaint() { Id = id, User = UserInfo() });
            if (Id > 0)
                return NoContent();
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }
    }
}