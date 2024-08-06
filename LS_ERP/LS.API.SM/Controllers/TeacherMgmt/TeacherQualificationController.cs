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
    public class TeacherQualificationController : BaseController
    {
        private readonly IConfiguration _Config;
        public TeacherQualificationController(IOptions<AppSettingsJson> appSettings, IConfiguration config) : base(appSettings)
        {
            _Config = config;
        }
        [HttpGet("{teacherCode}")]
        public async Task<IActionResult> Get(string teacherCode)
        {
            var obj = await Mediator.Send(new GetSchoolTeacherQualificationsByTeacherCode() { TeacherCode = teacherCode, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SchoolTeacherQualificationsListDto dTO)
        {
            var id = await Mediator.Send(new SchoolTeacherQualificationsMapping() { SchoolTeacherQualificationsList = dTO.TeacherQualificationsList, User = UserInfo() });
            if (id > 0)
                return Created($"get/{id}", dTO);
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }
    }
}
