using CIN.Application;
using CIN.Application.Common;
using CIN.Application.SchoolMgtDto;
using CIN.Application.SchoolMgtQuery;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LS.API.SM.Controllers.Admin_Setups
{
    public class AcedemicClassGradeController : BaseController
    {
        private readonly IConfiguration _Config;
        private readonly IWebHostEnvironment _env;

        public AcedemicClassGradeController(IOptions<AppSettingsJson> appSettings, IConfiguration config, IWebHostEnvironment env) : base(appSettings)
        {
            _Config = config;
            //_cinDbContext = cinDbContext;
            _env = env;
        }


        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {

            var list = await Mediator.Send(new GetAcedemicClassGradeList() { Input = filter, User = UserInfo() });
            return Ok(list);
        }

        [HttpGet("GetAcademicYear")]
        public async Task<IActionResult> GetAcademicYear()
        {
            var list = await Mediator.Send(new GetAcademicYear() { User = UserInfo() });
            return Ok(list);
        }

        [HttpGet("getSelectAcedemicClassGrades")]
        public async Task<IActionResult> GetSelectAcedemicClassGrades()
        {
            var list = await Mediator.Send(new GetSelectAcedemicClassGrades() { User = UserInfo() });
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var obj = await Mediator.Send(new GetAcedemicClassGradeById() { Id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TblSysSchoolAcedemicClassGradeDto dTO)
        {
            var id = await Mediator.Send(new CreateUpdateAcedemicClassGrades() { SchoolAcedemicClassGradeDto = dTO, User = UserInfo() });
            if (id > 0)
                return Created($"get/{id}", dTO);
            else if (id == -1)
            {
                return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Duplicate(nameof(dTO.Id)) });
            }
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var section = await Mediator.Send(new DeleteAcedemicClassGrade() { Id = id, User = UserInfo() });
            if (section > 0)
                return NoContent();
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }

        [HttpPost("UploadDocument")]
        public async Task<ActionResult> UploadDocument([FromForm] GradeDoumentDto dTO)
        {
            var webRoot = $"{_env.ContentRootPath}/Signaturefiles/" + dTO.GradeCode + "/";
            bool exists = System.IO.Directory.Exists(webRoot);
            if (!exists)
                System.IO.Directory.CreateDirectory(webRoot);
            if (dTO.UploadFile != null && dTO.UploadFile.Length > 0)
            {
                var guid = Guid.NewGuid().ToString();
                string name = string.Empty;
                name = Path.GetFileNameWithoutExtension(dTO.UploadFile.FileName);
                guid = $"{guid}_{name}_{ Path.GetExtension(dTO.UploadFile.FileName)}";
                dTO.UploadFileName += guid;
                var filePath = Path.Combine(webRoot, guid);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    dTO.UploadFile.CopyTo(stream);
                }
            }
            else 
            {
                dTO.UploadFileName= "";
            }
            var id = await Mediator.Send(new UploadGradeDocument() { Dto = dTO, User = UserInfo() });
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
