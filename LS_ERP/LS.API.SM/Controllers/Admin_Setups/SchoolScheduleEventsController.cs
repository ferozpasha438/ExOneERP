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
namespace LS.API.SM.Controllers.Admin_Setups
{
    public class SchoolScheduleEventsController : BaseController
    {
        private readonly IConfiguration _Config;

        public SchoolScheduleEventsController(IOptions<AppSettingsJson> appSettings, IConfiguration config) : base(appSettings)
        {
            _Config = config;
            //_cinDbContext = cinDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int month, int year)
        {

            var list = await Mediator.Send(new GetSchoolScheduleEvents() { Month = month, Year = year, User = UserInfo() });
            return Ok(list);
        }

        [HttpGet("getScheduleEventsPagedList")]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {

            var list = await Mediator.Send(new GetScheduleEventsPagedList() { Input = filter.Values(), User = UserInfo() });
            return Ok(list);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TblSysSchooScheduleEventsDto dTO)
        {
            var id = await Mediator.Send(new CreateSheduleEvent() { SheduleEventDto = dTO, User = UserInfo() });
            if (id > 0)
            {
                return Created($"get/{id}", dTO);
            }

            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });

        }

        [HttpGet("getScheduleEventById/{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var obj = await Mediator.Send(new GetScheduleEventById() { Id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var SurveyorId = await Mediator.Send(new DeleteScheduleEvent() { Id = id, User = UserInfo() });
            if (SurveyorId > 0)
                return NoContent();
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }
        [HttpPost("EventApproval")]
        public async Task<IActionResult> EventApproval([FromBody] ApprovalEventsDto approvalEventsDto)
        {
            int rid = await Mediator.Send(new EventApproval() { Id = approvalEventsDto.Id, User = UserInfo() });
            return rid != 0 ? Ok(rid) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }


    }
}
