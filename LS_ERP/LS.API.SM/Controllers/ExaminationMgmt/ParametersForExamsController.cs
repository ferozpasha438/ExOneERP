using CIN.Application;
using CIN.Application.Common;
using CIN.Application.ExaminationMgmtQuery;
using CIN.Application.SchoolMgtDtos;
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
    public class ParametersForExamsController : BaseController
    {
        private readonly IConfiguration _Config;
        private readonly IWebHostEnvironment _env;

        public ParametersForExamsController(IOptions<AppSettingsJson> appSettings, IConfiguration config, IWebHostEnvironment env) : base(appSettings)
        {
            _Config = config;
            _env = env;
        }
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {
            var list = await Mediator.Send(new ParametersForExamsList() { Input = filter, User = UserInfo() });
            return Ok(list);
        }

        [HttpGet("{gradeCode}")]
        public async Task<IActionResult> Get([FromRoute] string gradeCode)
        {
            var obj = await Mediator.Send(new GetParametersForExamsById() { GradeCode = gradeCode, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] SchoolExamManagementDto dTO)
        {
            var id = await Mediator.Send(new CreateUpdateExams() { SchoolExamMgtDto = dTO, User = UserInfo() });
            if (id > 0)
                return Created($"get/{id}", dTO);
            else if (id == -1)
            {
                return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Duplicate(nameof(dTO.Id)) });
            }
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }
        [HttpPost("CreateParametersForExams")]
        public async Task<ActionResult> CreateParametersForExams([FromBody] ParametersForExamsDto dTO)
        {
            var id = await Mediator.Send(new CreateParametersForExams() { Dto = dTO, User = UserInfo() });
            if (id > 0)
                return Created($"get/{id}", dTO);
            else if (id == -1)
            {
                return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Duplicate(nameof(dTO.GradeCode)) });
            }
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }

        [HttpGet("GetAllExamTypesList")]
        public async Task<IActionResult> GetAllExamTypesList()
        {
            var item = await Mediator.Send(new GetAllExamTypesList() { User = UserInfo() });
            return Ok(item);
        }
        [HttpGet("GetAllUsersList/{gradeCode}/{branchCode}")]
        public async Task<IActionResult> GetAllUsersList([FromRoute] string gradeCode, [FromRoute] string branchCode)
        {
            var item = await Mediator.Send(new GetAllUsersList() { GradeCode = gradeCode, BranchCode = branchCode, User = UserInfo() });
            return Ok(item);
        }

    }
}
