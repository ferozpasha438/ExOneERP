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
    public class ProjectController : BaseController
    {

        public ProjectController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }

      

        [HttpGet("getProjectsPagedList")]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {

            var list = await Mediator.Send(new GetProjectsPagedList() { Input = filter.Values(), User = UserInfo() });
            return Ok(list);
        }
        [HttpPost("getProjectsPagedListWithFilter")]
        public async Task<IActionResult> GetProjectsPagedListWithFilter([FromQuery] PaginationFilterDto filter,[FromBody]OprFilter filterInput)
        {

            var list = await Mediator.Send(new GetProjectsPagedListWithFilter() { Input = filter.Values(),FilterInput=filterInput, User = UserInfo() });
            return Ok(list);
        }



        [HttpPost]
        public async Task<ActionResult> Create([FromBody] OP_HRM_TEMP_ProjectDto dTO)
        {
            var id = await Mediator.Send(new CreateProject() { ProjectDto = dTO, User = UserInfo() });
            if (id > 0)
            {
                return Created($"get/{id}", dTO);
            }
            else if (id == -1)
            {
                return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Duplicate(nameof(dTO.ProjectCode)) });
            }
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });

        }
        [HttpPost("ConvertEnquiryToProject")]
        public async Task<ActionResult> ConvertEnquiryToProject([FromBody] ConvertCustToProjectDto dTO)
        {
            var id = await Mediator.Send(new ConvertEnquiryToProject() { ProjectDto = dTO, User = UserInfo() });
            if (id > 0)
            {
                return Created($"get/{id}", dTO);
            }
            else if (id == -1)
            {
                return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Duplicate(nameof(dTO.ProjectCode)) });
            }
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });

        }


        [HttpGet("getProjectById/{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var obj = await Mediator.Send(new GetProjectById() { Id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("getProjectByProjectCode/{ProjectCode}")]
        public async Task<IActionResult> Get([FromRoute] string ProjectCode)
        {
            var obj = await Mediator.Send(new GetProjectByProjectCode() { ProjectCode = ProjectCode, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("getSelectProjectListByCustomerCode/{CustomerCode}")]
        public async Task<IActionResult> GetSelectProjectListByCustomerCode([FromRoute] string CustomerCode)
        {
            var item = await Mediator.Send(new GetSelectProjectListByCustomerCode() { User = UserInfo(),CustomerCode= CustomerCode});
            return Ok(item);
        }

        [HttpGet("getSelectProjectList")]
        public async Task<IActionResult> GetSelectProjectList()
        {
            var item = await Mediator.Send(new GetSelectProjectList() { User = UserInfo() });
            return Ok(item);
        }
        [HttpGet("getSelectProjectList2")]
        public async Task<IActionResult> GetSelectProjectList2()
        {
            var item = await Mediator.Send(new GetSelectProjectList2() { User = UserInfo() });   //text conatains all
            return Ok(item);
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var ProjectId = await Mediator.Send(new DeleteProject() { Id = id, User = UserInfo() });
            if (ProjectId > 0)
                return NoContent();
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }
    }
}
