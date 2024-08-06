using CIN.Application;
using CIN.Application.SchoolMgtDtos;
using CIN.Application.SchoolMgtQuery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LS.API.SM.Controllers.TeacherMgmt
{
    public class TeacherSubjectsMappingController : BaseController
    {
        private readonly IConfiguration _Config;
        public TeacherSubjectsMappingController(IOptions<AppSettingsJson> appSettings, IConfiguration config) : base(appSettings)
        {
            _Config = config;
        }
        [HttpGet("{teacherCode}")]
        public async Task<IActionResult> Get([FromRoute] string teacherCode)
        {
            var obj = await Mediator.Send(new GetSchoolTeacherSubjectsByTeacherCode() { TeacherCode = teacherCode, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("GetTeacherSubjectsByGradeCode/{teacherCode}/{gradeCode}")]
        public async Task<IActionResult> GetTeacherSubjectsByGradeCode([FromRoute] string teacherCode, [FromRoute] string gradeCode)
        {
            var obj = await Mediator.Send(new GetTeacherSubjectsByGradeCode() { TeacherCode = teacherCode, GradeCode = gradeCode, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SchoolTeacherSubjectsMappingListDto dTO)
        {
            var id = await Mediator.Send(new SchoolTeacherSubjectsMapping() { SchoolTeacherSubjectsMappingList = dTO.TeacherSubjectsMappingList, User = UserInfo() });
            if (id > 0)
                return Created($"get/{id}", dTO);
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }
    }
}
