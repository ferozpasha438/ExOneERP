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

namespace LS.API.SM.Controllers.TeacherMgmt
{
    public class LessonPlanController : BaseController
    {
        private IConfiguration _config;
        public LessonPlanController(IOptions<AppSettingsJson> appSettings, IConfiguration config) : base(appSettings)
        {
            _config = config;
        }
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {
            var list = await Mediator.Send(new GetLessonPlanList() { Input = filter, User = UserInfo() });
            return Ok(list);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TeacherLessonPlanInfoDto dTO)
        {
            var list = await Mediator.Send(new CreateLessonPlanInfo() { LessonPlanInfoDto = dTO, User = UserInfo() });
            return Ok(list);
        }
        [HttpGet("GetLessonPlanInfoById/{id}")]
        public async Task<IActionResult> GetLessonPlanInfoById([FromRoute] int id)
        {
            var obj = await Mediator.Send(new GetLessonPlanInfoById() { Id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
    }
}
