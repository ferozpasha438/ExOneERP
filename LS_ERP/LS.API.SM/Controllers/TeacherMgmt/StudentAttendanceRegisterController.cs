using CIN.Application;
using CIN.Application.SchoolMgtDtos;
using CIN.Application.SchoolMgtQuery;
using Microsoft.AspNetCore.Hosting;
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
    public class StudentAttendanceRegisterController : BaseController
    {
        private readonly IConfiguration _Config;
        private readonly IWebHostEnvironment _env;
        public StudentAttendanceRegisterController(IOptions<AppSettingsJson> appSettings, IConfiguration config, IWebHostEnvironment env) : base(appSettings)
        {
            _Config = config;
            _env = env;
        }
        [HttpGet("StudentAttendanceRegisterList/{branchCode}/{gradeCode}/{sectionCode}")]
        public async Task<IActionResult> StudentAttendanceRegisterList([FromRoute] string branchCode, [FromRoute] string gradeCode, [FromRoute] string sectionCode)
        {
            var list = await Mediator.Send(new StudentAttendanceRegisterList() { BranchCode = branchCode, GradeCode = gradeCode, SectionCode = sectionCode, User = UserInfo() });
            return Ok(list);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] StudentAttendanceRegisterDto dTO)
        {
            var id = await Mediator.Send(new CreateStuAttRegisterData() { StuAttRegisterData = dTO, User = UserInfo() });
            if (id > 0)
                return Created($"get/{id}", dTO);
            else if (id == -1)
            {
                return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Duplicate(nameof(dTO.Id)) });
            }
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }
        [HttpPost("CloseAttendance")]
        public async Task<IActionResult> CloseAttendance([FromBody] StudentAttendanceRegisterDto dTO)
        {
            var id = await Mediator.Send(new CloseAttendance() { StuAttRegisterData = dTO, User = UserInfo() });
            if (id > 0)
                return Created($"get/{id}", dTO);
            else if (id == -1)
            {
                return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Duplicate(nameof(dTO.Id)) });
            }
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }
        [HttpGet("StudentList/{branchCode}/{gradeCode}/{sectionCode}")]
        public async Task<IActionResult> StudentList([FromRoute] string branchCode, [FromRoute] string gradeCode, [FromRoute] string sectionCode)
        {
            var list = await Mediator.Send(new StudentList() { BranchCode = branchCode, GradeCode = gradeCode, SectionCode = sectionCode, User = UserInfo() });
            return Ok(list);
        }

    }
}
