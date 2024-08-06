using CIN.Application;
using CIN.Application.Common;
using CIN.Application.ExaminationMgmtQuery;
using CIN.Application.SchoolMgtDtos;
using CIN.Application.SchoolMgtQuery;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LS.API.SM.Controllers.ExaminationMgmt
{
    public class ExamResultController : BaseController
    {
        private readonly IConfiguration _Config;
        private readonly IWebHostEnvironment _env;
        public ExamResultController(IOptions<AppSettingsJson> appSettings, IConfiguration config, IWebHostEnvironment env) : base(appSettings)
        {
            _Config = config;
            _env = env;
        }
        [HttpGet("StudentResultList/{branchCode}/{gradeCode}/{examinationTypeCode}")]
        public async Task<IActionResult> StudentResultList([FromRoute] string branchCode, [FromRoute] string gradeCode, [FromRoute] string examinationTypeCode)
        {
            var list = await Mediator.Send(new LoadResultList() { BranchCode = branchCode, GradeCode = gradeCode, ExaminationTypeCode = examinationTypeCode, User = UserInfo() });
            return Ok(list);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] StudentExamResultListDto dTO)
        {
            var id = await Mediator.Send(new CreateStudentExamResults() { RequestDto = dTO, User = UserInfo() });
            if (id > 0)
                return Created($"get/{id}", dTO);
            else if (id == -1)
            {
                return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Duplicate(nameof(dTO.ExamHeaderID)) });
            }
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }


        [HttpGet("GetStudentExamResult")]
        public async Task<IActionResult> StudentExamResult([FromQuery] string admissionNumber, string branchCode, string gradeCode)
        {
            var list = await Mediator.Send(new StudentExamResult() {StuAdmNum=admissionNumber, BranchCode = branchCode, GradeCode = gradeCode, User = UserInfo() });
            return Ok(list);
        }

    }
}
