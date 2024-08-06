using CIN.Application;
using CIN.Application.Common;
using CIN.Application.SchoolMgtDtos;
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

namespace LS.API.SM.Controllers.TeacherMgmt
{
    public class TeacherMasterController : BaseController
    {
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _env;

        public TeacherMasterController(IOptions<AppSettingsJson> appSettings, IConfiguration config, IWebHostEnvironment env) : base(appSettings)
        {
            _config = config;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {

            var list = await Mediator.Send(new GetSchoolTeacherMasterList() { Input = filter, User = UserInfo() });
            return Ok(list);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] SchoolTeacherMasterDto dTO)
        {
            var files = HttpContext.Request.Form.Files;
            int loopCount = 0;
            var webRoot = $"{_env.ContentRootPath}/Teacherfiles";
            bool exists = System.IO.Directory.Exists(webRoot);
            if (!exists)
                System.IO.Directory.CreateDirectory(webRoot);
            if (files != null && files.Count > 0)
            {
                foreach (var file in files)
                {
                    if (file != null && file.Length > 0)
                    {
                        var guid = Guid.NewGuid().ToString();
                        string name = string.Empty;
                        if (loopCount == 0)
                        {
                            name = Path.GetFileNameWithoutExtension(dTO.ThumbNailimageFileName.FileName);
                            guid = $"{guid}_{name}_{ Path.GetExtension(dTO.ThumbNailimageFileName.FileName)}";
                            dTO.ThumbNailImagePath += guid;
                            var filePath = Path.Combine(webRoot, guid);
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                file.CopyTo(stream);
                            }
                        }
                        else
                        {
                            name = Path.GetFileNameWithoutExtension(dTO.FullImageFileName.FileName);
                            guid = $"{guid}_{name}_{ Path.GetExtension(dTO.FullImageFileName.FileName)}";
                            dTO.FullImageParh += guid;
                            var filePath = Path.Combine(webRoot, guid);
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                file.CopyTo(stream);
                            }
                        }
                    }
                    loopCount++;
                }
            }
            else if (dTO.Id == 0)
            {
                dTO.ThumbNailImagePath += "default_thumb.jpg";
                dTO.FullImageParh += "default.jpg";
            }
            else
            {
                dTO.ThumbNailImagePath = string.Empty;
                dTO.FullImageParh = string.Empty;
            }
            var id = await Mediator.Send(new AllSchoolTeacherMasterData() { TeacherMasterDataDto = dTO, User = UserInfo() });
            if (id > 0)
                return Created($"get/{id}", dTO);
            else if (id == -1)
            {
                return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Duplicate(nameof(dTO.Id)) });
            }
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }

        [HttpGet("GetTeacherMasterDataById/{id}")]
        public async Task<IActionResult> GetTeacherMasterDataById([FromRoute] int id)
        {
            var obj = await Mediator.Send(new GetTeacherMasterDataById() { Id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("ValidateUserName/{username}")]
        public async Task<IActionResult> ValidateUserName([FromRoute] string username)
        {
            var obj = await Mediator.Send(new ValidateUsername() { Username = username, User = UserInfo() });
            return Ok(obj);
        }
        [HttpGet("GetTeachersByBranchcode/{branchCode}")]
        public async Task<IActionResult> GetTeachersByBranchcode([FromRoute] string branchCode)
        {
            var obj = await Mediator.Send(new GetTeachersByBranchcode() { BranchCode = branchCode, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("GetModerators")]
        public async Task<IActionResult> GetModerators()
        {
            var obj = await Mediator.Send(new GetModerators() { User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("GetBranchTeacherGrades/{teacherCode}")]
        public async Task<IActionResult> GetBranchTeacherGrades([FromRoute] string teacherCode)
        {
            var obj = await Mediator.Send(new GetBranchTeacherGrades() { TeacherCode = teacherCode, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("GetTeacherGradeSubjects/{teacherCode}/{gradeCode}/{sectionCode}")]
        public async Task<IActionResult> GetTeacherGradeSubjects([FromRoute] string teacherCode, [FromRoute] string gradeCode, [FromRoute] string sectionCode)
        {
            var obj = await Mediator.Send(new GetTeacherGradeSubjects() { TeacherCode = teacherCode, GradeCode = gradeCode, SectionCode = sectionCode, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("GetTeachersList")]
        public async Task<IActionResult> GetTeachersList()
        {

            var list = await Mediator.Send(new GetTeachersList() { User = UserInfo() });
            return Ok(list);
        }
        [HttpGet("IsApprovalLoginTeacher/{typeId}")]
        public async Task<IActionResult> IsApprovalLoginTeacher([FromRoute] int typeId)
        {
            var list = await Mediator.Send(new IsApprovalLoginTeacher() { TypeId = typeId, TeacherCode = TeacherCode, User = UserInfo() });
            return Ok(list);
        }
    }
}
