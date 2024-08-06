using CIN.Application;
using CIN.Application.Common;
using CIN.Application.FinanceMgtQuery;
using CIN.Application.SchoolMgtDto;
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

namespace LS.API.SM.Controllers.Admin_Setups
{
    public class BranchController : BaseController
    {
        private IConfiguration _config;
        public BranchController(IOptions<AppSettingsJson> appSettings, IConfiguration config) : base(appSettings)
        {
            _config = config;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {
            var list = await Mediator.Send(new GetSchoolAccountBranchList() { Input = filter.Values(), User = UserInfo() });
            return Ok(list);
        }

        [HttpGet("{branchCode}")]
        public async Task<IActionResult> Get([FromRoute] string branchCode)
        {
            var obj = await Mediator.Send(new GetSchoolBranchDetails() { BranchCode = branchCode, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] SchoolBranchesDto input)
        {
            var accBranch = await Mediator.Send(new CreateSchoolBranch() { Input = input, User = UserInfo() });
            if (accBranch.Id > 0)
            {
                return Created($"get/{accBranch.Id}", input);
            }
            return BadRequest(new ApiMessageDto { Message = accBranch.Message });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var BranchId = await Mediator.Send(new DeleteSchoolBranches() { Id = id, User = UserInfo() });
            if (BranchId > 0)
                return NoContent();
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }
        [HttpGet("getBranchByBranchCode")]
        public async Task<IActionResult> GetBranchByBranchCode([FromQuery] string branchCode)
        {
            var obj = await Mediator.Send(new GetBranchByBranchCode() { Input = branchCode, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("getBranchDataByBranchCode")]
        public async Task<IActionResult> GetBranchDataByBranchCode([FromQuery] string branchCode)
        {
            var obj = await Mediator.Send(new GetBranchDataByBranchCode() { Input = branchCode, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
[HttpGet("getSchoolBranchDataByBranchCode")]
        public async Task<IActionResult> GetSchoolBranchDataByBranchCode([FromQuery] string branchCode)
        {
            var obj = await Mediator.Send(new GetBranchDataByBranchCode() { Input = branchCode, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("getSelectSysBranchList")]
        public async Task<IActionResult> GetSelectSysBranchList([FromQuery] string search)
        {
            var obj = await Mediator.Send(new GetSelectSysBranchList() { Search = search, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("GetBranchHolidays")]
        public async Task<IActionResult> GetBranchHolidays([FromQuery] string branchCode)
        {

            var list = await Mediator.Send(new GetBranchHolidays() { BranchCode = branchCode, User = UserInfo() });
            return Ok(list);
        }
        [HttpPost("AddUpdateHolidays")]
        public async Task<ActionResult> AddUpdateHolidays([FromBody] TblSysSchoolHolidayCalanderStudentDto input)
        {
            var id = await Mediator.Send(new AddUpdateHolidays() { Input = input, User = UserInfo() });
            if (id > 0)
                return Created($"get/{id}", input);
            else if (id == -1)
            {
                return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Duplicate(nameof(input.Id)) });
            }
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }

        [HttpGet("GetBranchEventsHolidaysData/{branchCode}")]
        public async Task<ActionResult> GetBranchEventsHolidaysData([FromRoute] string branchCode)
        {
            var data= await Mediator.Send(new GetBranchEventsHolidaysData() { BranchCode= branchCode, User = UserInfo() });
            return Ok(data);
        }
        [HttpGet("GetBranchDashBoard")]
        public async Task<IActionResult> GetBranchDashBoard()
        {

            var list = await Mediator.Send(new GetBranchDashBoard() { User = UserInfo() });
            return Ok(list);
        }
    }
}
