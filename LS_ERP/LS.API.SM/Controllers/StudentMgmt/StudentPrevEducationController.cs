using CIN.Application;
using CIN.Application.Common;
using CIN.Application.SchoolMgtDtos;
using CIN.Application.SchoolMgtQuery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LS.API.SM.Controllers.StudentMgmt
{
    public class StudentPrevEducationController :BaseController
    {
        private IConfiguration _Config;

        public StudentPrevEducationController(IOptions<AppSettingsJson> appSettings, IConfiguration config) : base(appSettings)
        {
            _Config = config;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {
            var list = await Mediator.Send(new GetAllStudentPrevEducationList() { Input = filter, User = UserInfo() });
            return Ok(list);
        }
        [HttpGet("GetPrevEducationListByStuAdmNum")]
        public async Task<IActionResult> GetPrevEducationListByStuAdmNum([FromQuery] PaginationFilterDto filter)
        {
            var list = await Mediator.Send(new GetPrevEducationListByStuAdmNum() { Input = filter, User = UserInfo() });
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var obj = await Mediator.Send(new GetStudentPrevEducationById() { Id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });

        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TblDefStudentPrevEducationDto dTO)
        {
            var id = await Mediator.Send(new CreateUpdateStudentPrevEducation() { Input = dTO, User = UserInfo() });
            if (id > 0)
                return Created($"get/{id}", dTO);
            else if(id == -1)
            {
                return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Duplicate(nameof(dTO.Id)) });
            }
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult>Delete([FromRoute]int id)
        {
            var studentPrevEducation = await Mediator.Send(new DeleteStudentPrevEducation() { Id = id, User = UserInfo() });
            if (studentPrevEducation > 0)
                return NoContent();
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }
        
    }
}
