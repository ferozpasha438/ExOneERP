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

namespace LS.API.SM.Controllers.Registration
{


    public class WebStudentRegistrationController : BaseController
    {
       // private readonly IConfiguration _Config;

        public WebStudentRegistrationController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
           // _Config = config;
            //_cinDbContext = cinDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {

            var list = await Mediator.Send(new GetWebStudentRegistrationList() { Input = filter, User = UserInfo() });
            return Ok(list);
        }

        [HttpGet("GetSchoolStudentRegistrationById/{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var obj = await Mediator.Send(new GetSchoolStudentRegistrationById() { Id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        //[HttpPost]
        //public async Task<ActionResult> Create(TblWebStudentRegistrationDto dTO)
        //{
        //    var id = await Mediator.Send(new CreateUpdateWebStudentRegistration() { webStudentRegistrationDto = dTO, User = UserInfo() });
        //    if (id > 0)
        //        return Created($"get/{id}", dTO);
        //    else if (id == -1)
        //    {
        //        return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Duplicate(nameof(dTO.Id)) });
        //    }
        //    return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        //}

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TblWebStudentRegistrationDto dTO)
        {
            var obj = await Mediator.Send(new CreateUpdateWebStudentRegistration() { webStudentRegistrationDto = dTO, User = UserInfo() });
            if (obj.Id > 0)
                return Created($"get/{obj.Id}", dTO);
            return BadRequest(new ApiMessageDto { Message = obj.Message });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var schoolStudentId = await Mediator.Send(new DeleteSchoolStudentRegistration() { Id = id, User = UserInfo() });
            if (schoolStudentId > 0)
                return NoContent();
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }

    }
}
