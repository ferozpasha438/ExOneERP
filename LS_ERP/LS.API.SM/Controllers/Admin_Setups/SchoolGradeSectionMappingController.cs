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
    public class SchoolGradeSectionMappingController :BaseController
    {
        private readonly IConfiguration _Config;
        private readonly IWebHostEnvironment _env;
        public SchoolGradeSectionMappingController(IOptions<AppSettingsJson> appSettings, IConfiguration config, IWebHostEnvironment env) : base(appSettings)
        {
            _Config = config;
            //_cinDbContext = cinDbContext;
            _env = env;
        }

        [HttpGet("getAllSectionsByGradeMapping/{GradeCode}")]
        public async Task<IActionResult> Get([FromRoute] string GradeCode)
        {
            var obj = await Mediator.Send(new GetAllSectionsByGradeMapping() { GradeCode = GradeCode, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpPost("CreateMapping")]
        public async Task<ActionResult> CreateMapping([FromForm] GradeSectionMappingDto dTO)
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
                dTO.UploadFileName = "";
            }
            var id = await Mediator.Send(new CreateUpdateSectionsGradeMapping() { SchoolGradeSectionMapDto = dTO, User = UserInfo() });
            if (id > 0)
                return Created($"get/{id}", dTO);
            else if (id == -1)
            {
                return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Duplicate(nameof(dTO.GradeCode)) });
            }
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }

        [HttpGet("getAllSectionsByGradeCode/{GradeCode}")]
        public async Task<IActionResult> GetAllSectionsByGradeCode([FromRoute] string GradeCode)
        {
            var obj = await Mediator.Send(new GetAllSectionsByGradeCode() { GradeCode = GradeCode, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
    }
}
