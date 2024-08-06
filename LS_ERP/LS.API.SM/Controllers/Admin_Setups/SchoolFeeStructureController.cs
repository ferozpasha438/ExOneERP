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
    public class SchoolFeeStructureController :BaseController
    {
        private readonly IConfiguration _Config;
        public SchoolFeeStructureController(IOptions<AppSettingsJson> appSettings, IConfiguration config) : base(appSettings)
        {
            _Config = config;
            //_cinDbContext = cinDbContext;
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TblSysSchoolFeeStructureDto dTO)
        {
            var id = await Mediator.Send(new CreateUpdateSchoolFeeStructure() { SchoolFeeStructureDto = dTO, User = UserInfo() });
            if (id > 0)
                return Created($"get/{id}", dTO);
            else if (id == -1)
            {
                return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Duplicate(nameof(dTO.FeeStructCode)) });
            }
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }

        [HttpGet("GetFeeStructureHeaderById/{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var obj = await Mediator.Send(new GetFeeStructureHeaderById() { Id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }



        [HttpGet("getSysSchoolFeeStructureHeaderList")]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {

            var list = await Mediator.Send(new GetSysSchoolFeeStructureHeaderList() { Input=filter, User = UserInfo() });
            return Ok(list);
        }


        [HttpGet("getSchoolFeeDetailsByFeeStructCode/{FeeStructCode}")]
        public async Task<IActionResult> Get([FromRoute] string FeeStructCode)
        {
            var obj = await Mediator.Send(new GetSchoolFeeDetailsByFeeStructCode() { FeeStructCode = FeeStructCode, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }



        [HttpGet("getSchoolFeeDetailsforAllTerm/{FeeStructCode}")]
        public async Task<IActionResult> GetAllTerm([FromRoute] string FeeStructCode)
        {
            var obj = await Mediator.Send(new GetSchoolFeeDetailsforAllTerm() { FeeStructCode = FeeStructCode, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("GetFeeStructureCodesByGradeCode/{gradeCode}")]
        public async Task<IActionResult> GetFeeStructureCodesByGradeCode([FromRoute] string gradeCode)
        {
            var obj = await Mediator.Send(new GetFeeStructureCodesByGradeCode() { gradeCode = gradeCode, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("GetFeeStructureCodesByGradeANDBranch/{gradeCode}/{branchCode}")]
        public async Task<IActionResult> GetFeeStructureCodesByGradeANDBranch([FromRoute] string gradeCode,string branchCode)
        {
            var obj = await Mediator.Send(new GetFeeStructureCodesByGradeANDBranch() { gradeCode = gradeCode, branchCode= branchCode, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("GetTotalTermFeeByFeeStructCode/{FeeStructCode}")]
        public async Task<IActionResult> GetTotalTermFeeByFeeStructCode([FromRoute] string FeeStructCode)
        {
            var obj = await Mediator.Send(new GetTotalTermFeeByFeeStructCode() { FeeStructCode = FeeStructCode, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }


    }
}
