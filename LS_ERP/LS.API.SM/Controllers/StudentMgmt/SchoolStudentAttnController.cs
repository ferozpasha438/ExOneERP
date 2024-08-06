using CIN.Application;
using CIN.Application.Common;
using CIN.Application.SchoolMgtDtos;
using CIN.Application.SchoolMgtQuery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LS.API.SM.Controllers.StudentMgmt
{
    public class SchoolStudentAttnController :BaseController
    {
        private readonly IConfiguration _Config;

        public SchoolStudentAttnController(IOptions<AppSettingsJson> appSettings, IConfiguration config) : base(appSettings)
        {
            _Config = config;
            //_cinDbContext = cinDbContext;
        }



        #region For Web
        [HttpGet("StudentAttendanceByStuAdmNum")]
        public async Task<IActionResult> StudentAttendanceByStuAdmNum([FromQuery] PaginationFilterDto filter)
        {
            var obj = await Mediator.Send(new StudentAttendanceByStuAdmNum() { Input = filter, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var obj = await Mediator.Send(new GetStudentAttandanceList() { User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }


        [HttpGet("StudentAttendanceById/{id}")]
        public async Task<IActionResult> StudentAttendanceById([FromRoute] int id)
        {
            var obj = await Mediator.Send(new GetStudentAttendanceById() { Id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TblDefStudentAttendanceDto dTO)
        {

            var id = await Mediator.Send(new CreateUpdateStudentAttendance() { Input = dTO, User = UserInfo() });
            if (id > 0)
                return Created($"get/{id}", dTO);
            else if (id == -1)
            {
                return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Duplicate(nameof(dTO.Id)) });
            }
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }


        [HttpDelete("id")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var studentAttn = await Mediator.Send(new DeleteStudentAttendance() { Id = id, User = UserInfo() });
            if (studentAttn > 0)
                return NoContent();
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }
        #endregion


        #region For Mobile App

        #region Student Attendance List

        [HttpGet("SchoolStudentAttnList")]
        public async Task<IActionResult> Get([FromQuery] string stuAdmNum)
        {
            
            var list = await Mediator.Send(new GetSchoolStudentAttnList(){ StuAdmNum= stuAdmNum,  User = UserInfo() });
            return Ok(list);
        }

        #endregion

        #region Student Attendance

        [HttpGet("getAllStudentAttnPercentageByRegNo")]
        public async Task<IActionResult> GetAllStudentAttnPercentageByRegNo([FromQuery] string RegNo)
        {
            string montlyAttn = "70%";
            string YearlyAttn = "70%";

            return Ok(new { montlyAttn, YearlyAttn });
        }

        #endregion

        #region Student Last Result Avarage Marks

        [HttpGet("GetLastResultAvgMarksByStuAdmNum")]
        public async Task<IActionResult> GetLastResultAvgMarksByStuAdmNum([FromQuery] string stuAdmNum)
        {
            string lastResulAvgMarks = "78%";
            

            return Ok(new { lastResulAvgMarks });
        }
        #endregion

        #endregion



    }
}
