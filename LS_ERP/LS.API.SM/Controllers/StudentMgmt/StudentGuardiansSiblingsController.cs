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
    public class StudentGuardiansSiblingsController : BaseController
    {
        private IConfiguration _Config;

        public StudentGuardiansSiblingsController(IOptions<AppSettingsJson> appSettings, IConfiguration config) : base(appSettings)
        {
            _Config = config;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {
            var list = await Mediator.Send(new GetGuardiansSiblingsList() { Input = filter, User = UserInfo() });
            return Ok(list);
        }
        [HttpGet("GetGuardiansSiblingsByStuAdmNum")]
        public async Task<IActionResult> GetGuardiansSiblingsByStuAdmNum([FromQuery] PaginationFilterDto filter)
        {
            var list = await Mediator.Send(new GetGuardiansSiblingsByStuAdmNum() { Input = filter, User = UserInfo() });
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromQuery] int id)
        {
            var obj = await Mediator.Send(new GetGuardiansSiblingsById() { Id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }


        [HttpGet("GetParentProfile")]
        public async Task<IActionResult> GetParentProfile([FromQuery] string mobile)
        {
            var obj = await Mediator.Send(new GetParentProfile() { Mobile=mobile, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }


        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TblDefStudentGuardiansSiblingsDto dTO)
        {

            var id = await Mediator.Send(new CreateUpdateGuardiansSiblings() { StudentGuardiansSiblingsDto = dTO, User = UserInfo() });
            if (id > 0)
                return Created($"get/{id}", dTO);
            else if (id == -1)
            {
                return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Duplicate(nameof(dTO.Id)) });
            }
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
            
        }

        [HttpDelete]
        public async Task<ActionResult>Delete([FromRoute] int id)
        {
            var gaurdiaSiblingId = await Mediator.Send(new DeleteGaurdianSiblings() { Id = id, User = UserInfo() });
            if (gaurdiaSiblingId > 0)
                return NoContent();
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }

       
    }
}
