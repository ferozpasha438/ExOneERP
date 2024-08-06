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
    public class StudentHomeWorkController :BaseController
    {

        private IConfiguration _config;

        public StudentHomeWorkController(IOptions<AppSettingsJson> appSettings, IConfiguration config) : base(appSettings)
        {
            _config = config;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string grade, DateTime homeWorkDate)
        {
            var list = await Mediator.Send(new GetStudentHomeWork() { Grade = grade, HomeworkDate = homeWorkDate, User = UserInfo() });
            return Ok(list);
        }
        [HttpGet("GetStudentHomeWorkList")]
        public async Task<IActionResult> GetStudentHomeWorkList([FromQuery] PaginationFilterDto filter)
        {
            var list = await Mediator.Send(new GetSchoolStudentHomeworkList() { Input = filter, User = UserInfo() });
            return Ok(list);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TblStudentHomeWorkDto dTO)
        {
            var id = await Mediator.Send(new InsertANDUpdateStudentHomework() { StudentHomeWorkDto = dTO, User = UserInfo() });
            if (id > 0)
                return Created($"get/{id}", dTO);
            else if (id == -1)
            {
                return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Duplicate(nameof(dTO.Id)) });
            }
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }

        [HttpGet("GetStudentHomeWorkById/{id}")]
        public async Task<IActionResult> GetStudentHomeWorkById([FromRoute] int id)
        {
            var obj = await Mediator.Send(new GetStudentHomeWorkById() { Id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
    }
}
