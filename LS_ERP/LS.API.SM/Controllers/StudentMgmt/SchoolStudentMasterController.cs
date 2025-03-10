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
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace LS.API.SM.Controllers.StudentMgmt
{
    public class SchoolStudentMasterController : BaseController
    {
        private readonly IConfiguration _Config;
        private readonly IWebHostEnvironment _env;
        public SchoolStudentMasterController(IOptions<AppSettingsJson> appSettings, IConfiguration config, IWebHostEnvironment env) : base(appSettings)
        {
            _Config = config;
            _env = env;
            //_cinDbContext = cinDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {

            var list = await Mediator.Send(new GetSchoolStudentManagementList() { Input = filter, User = UserInfo() });
            return Ok(list);
        }
        [HttpGet("GetAllStudentsList")]
        public async Task<IActionResult> GetAllStudentsList()
        {

            var list = await Mediator.Send(new GetAllStudentsList() { User = UserInfo() });
            return Ok(list);
        }

        //for Mobile app
        [HttpGet("SchoolStudentMasterDetailsByMobile")]
        public async Task<IActionResult> Get([FromQuery] string mobile)
        {

            var list = await Mediator.Send(new GetSchoolStudentMasterDetailsByMobile() { Mobile = mobile, User = UserInfo() });

            return Ok(list);
        }

        [HttpGet("GetStudentDetailsByStuAdmNum")]
        public async Task<IActionResult> GetStudentDetailsByStuAdmNum([FromQuery] string stuAdmNum)
        {

            var list = await Mediator.Send(new GetStudentDetailsByStuAdmNum() { StuAdmNum = stuAdmNum, User = UserInfo() });
            return Ok(list);
        }

        [HttpGet("GetStudentFeeHeaderByStuAdmNum")]
        public async Task<IActionResult> GetStudentFeeHeaderByStuAdmNum([FromQuery] string stuAdmNum, [FromQuery] string year)
        {
            var list = await Mediator.Send(new GetStudentFeeHeaderByStuAdmNum() { StuAdmNum = stuAdmNum, Year = year, User = UserInfo() });
            return Ok(list);
        }
        [HttpGet("GetSchoolStudentFeeDetailsByStuAdmNum")]
        public async Task<IActionResult> GetSchoolStudentFeeDetailsByStuAdmNum([FromQuery] string stuAdmNum)
        {

            var list = await Mediator.Send(new GetSchoolStudentFeeDetailsByStuAdmNum() { StuAdmNum = stuAdmNum, User = UserInfo() });
            return Ok(list);
        }

        [HttpGet("GetSchoolStudentById/{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var obj = await Mediator.Send(new GetSchoolStudentById() { Id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TblDefSchoolStudentMasterDto dTO)
        {
            var id = await Mediator.Send(new CreateSchoolStudentManagement() { SchoolStudentMasterDto = dTO, User = UserInfo() });
            if (id > 0)
                return Created($"get/{id}", dTO);
            else if (id == -1)
            {
                return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Duplicate(nameof(dTO.Id)) });
            }
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }
                

        [HttpPost("SaveAllStudentMasterData")]
        public async Task<IActionResult> SaveAllStudentMasterData([FromForm] AllStudentMasterDataDto dTO)
        {
            var webRoot = $"{_env.ContentRootPath}/Signaturefiles";
            bool exists = System.IO.Directory.Exists(webRoot);
            if (!exists)
                System.IO.Directory.CreateDirectory(webRoot);
            if (dTO.StudentImage != null && dTO.StudentImage.Length > 0)
            {
                var guid = Guid.NewGuid().ToString();
                string name = string.Empty;
                name = Path.GetFileNameWithoutExtension(dTO.StudentImage.FileName);
                guid = $"{guid}_{name}_{Path.GetExtension(dTO.StudentImage.FileName)}";
                dTO.StudentImageFileName += guid;
                var filePath = Path.Combine(webRoot, guid);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    dTO.StudentImage.CopyTo(stream);
                }
            }
            else if (dTO.Id == 0)
            {
                dTO.StudentImageFileName += "default_thumb.jpg";
            }
            else
            {
                dTO.StudentImageFileName = string.Empty;
            }
            if (dTO.FatherSignature != null && dTO.FatherSignature.Length > 0)
            {
                var guid = Guid.NewGuid().ToString();
                string name = string.Empty;
                name = Path.GetFileNameWithoutExtension(dTO.FatherSignature.FileName);
                guid = $"{guid}_{name}_{Path.GetExtension(dTO.FatherSignature.FileName)}";
                dTO.FatherSignatureFileName += guid;
                var filePath = Path.Combine(webRoot, guid);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    dTO.FatherSignature.CopyTo(stream);
                }
            }
            else if (dTO.Id == 0)
            {
                dTO.FatherSignatureFileName += "default_thumb.jpg";
            }
            else
            {
                dTO.FatherSignatureFileName = string.Empty;
            }
            if (dTO.MotherSignature != null && dTO.MotherSignature.Length > 0)
            {
                var guid = Guid.NewGuid().ToString();
                string name = string.Empty;
                name = Path.GetFileNameWithoutExtension(dTO.MotherSignature.FileName);
                guid = $"{guid}_{name}_{Path.GetExtension(dTO.MotherSignature.FileName)}";
                dTO.MotherSignatureFileName += guid;
                var filePath = Path.Combine(webRoot, guid);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    dTO.MotherSignature.CopyTo(stream);
                }
            }
            else if (dTO.Id == 0)
            {
                dTO.MotherSignatureFileName += "default_thumb.jpg";
            }
            else
            {
                dTO.MotherSignatureFileName = string.Empty;
            }
            var id = await Mediator.Send(new AllSchoolStudentMasterData() { AllStudentMasterDataDto = dTO, User = UserInfo() });
            if (id > 0)
                return Created($"get/{id}", dTO);
            else if (id == -1)
            {
                return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Duplicate(nameof(dTO.Id)) });
            }
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }

        [HttpGet("GetStudentMasterDataById/{id}")]
        public async Task<IActionResult> GetStudentMasterDataById([FromRoute] int id)
        {
            var obj = await Mediator.Send(new GetStudentMasterFormDataById() { Id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("GetStudentFeeHeaderByStuID")]
        public async Task<IActionResult> GetStudentFeeHeaderByStuID([FromQuery] PaginationFilterDto filter)
        {

            var list = await Mediator.Send(new GetStudentFeeHeaderByStuID() { Input = filter, User = UserInfo() });
            return Ok(list);
        }
        [HttpGet("GetSchoolStudentFeeDetailsByStuAdmNumANDTermCode")]
        public async Task<IActionResult> GetSchoolStudentFeeDetailsByStuAdmNumANDTermCode([FromQuery] string stuAdmNum, string termCode)
        {

            var list = await Mediator.Send(new GetSchoolStudentFeeDetailsByStuAdmNumANDTermCode() { StuAdmNum = stuAdmNum, TermCode = termCode, User = UserInfo() });
            return Ok(list);
        }
        [HttpGet("UpdateStatus/{StuAdmNum}/{isChecked}")]
        public async Task<IActionResult> UpdateStatus([FromRoute] string StuAdmNum, [FromRoute] bool IsChecked)
        {
            var obj = await Mediator.Send(new UpdateStatus() { StuAdmNum = StuAdmNum, IsChecked = IsChecked, User = UserInfo() });
            return Ok(obj);
        }
        [HttpGet("GetStudentAttandace/{StuAdmNum}/{SelectedYear}/{SelectedMonth}")]
        public async Task<IActionResult> GetStudentAttandace([FromRoute] string StuAdmNum, [FromRoute] int SelectedYear, [FromRoute] int SelectedMonth)
        {
            var obj = await Mediator.Send(new GetStudentAttandace() { StuAdmNum = StuAdmNum, Year = SelectedYear, Month = SelectedMonth, User = UserInfo() });
            return Ok(obj);
        }

        [HttpGet("GetStudentFeeDetails")]
        public async Task<IActionResult> GetStudentFeeDetails([FromQuery] string stuAdmNum)
        {

            var list = await Mediator.Send(new GetStudentFeeDetails() { StuAdmNum = stuAdmNum, User = UserInfo() });
            return Ok(list);
        }

        [HttpGet("GetStudentFeeHeaderWithVatByStuAdmNum")]
        public async Task<IActionResult> GetStudentFeeHeaderWithVatByStuAdmNum([FromQuery] string stuAdmNum)
        {
            var list = await Mediator.Send(new GetStudentFeeHeaderWithVatByStuAdmNum() { StuAdmNum = stuAdmNum, User = UserInfo() });
            return Ok(list);
        }

        [HttpGet("addStudentsToNextGrade/{academicYear}")]
        public async Task<IActionResult> AddStudentsToNextGrade([FromRoute] int academicYear)
        {
            var obj = await Mediator.Send(new AddStudentsToNextGrade() { AcademicYear = academicYear, User = UserInfo() });
            return Ok(obj);
        }
        [HttpGet("addFeeStructure/{academicYear}")]
        public async Task<IActionResult> AddFeeStructure([FromRoute] int academicYear)
        {
            var obj = await Mediator.Send(new AddFeeStructure() { AcademicYear = academicYear, User = UserInfo() });
            return Ok(obj);
        }


        [HttpDelete]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var schoolStudentId = await Mediator.Send(new DeleteSchoolStudent() { Id = id, User = UserInfo() });
            if (schoolStudentId > 0)
                return NoContent();
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }


    }
}
