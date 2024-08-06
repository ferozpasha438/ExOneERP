using CIN.Application;
using CIN.Application.Common;
using CIN.Application.SchoolMgtDto;
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
    public class SchoolGradeSubjectMappingController : BaseController
    {
        private readonly IConfiguration _Config;

        public SchoolGradeSubjectMappingController(IOptions<AppSettingsJson> appSettings, IConfiguration config) : base(appSettings)
        {
            _Config = config;
            //_cinDbContext = cinDbContext;
        }


        [HttpGet("getAllSubjectByGradeAndSemisterMapping/{GradeCode}/{SemisterCode}")]
        public async Task<IActionResult> Get([FromRoute] string GradeCode,string SemisterCode)
        {
            var obj = await Mediator.Send(new GetAllSubjectByGradeAndSemisterMapping() { GradeCode = GradeCode, SemisterCode= SemisterCode, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("getAllSubjectsByGradeCode/{gradeCode}")]
        public async Task<IActionResult> GetAllSubjectsByGradeCode([FromRoute] string gradeCode)
        {
            var obj = await Mediator.Send(new GetAllSubjectsByGradeCode() { GradeCode = gradeCode, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TblSysSchoolGradeSubjectMappingDto dTO)
        {
            var id = await Mediator.Send(new CreateUpdateSchoolGradeSubjectMapping() { SchoolGradeSubjectMapDto = dTO, User = UserInfo() });
            if (id > 0)
                return Created($"get/{id}", dTO);
            else if (id == -1)
            {
                return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Duplicate(nameof(dTO.GradeCode)) });
            }
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }
    }
}
