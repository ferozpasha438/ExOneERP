using CIN.Application;
using CIN.Application.Common;
using CIN.Application.HumanResource.SetUp.HRMSetUpDtos;
using CIN.Application.HumanResource.SetUp.HRMSetUpQuery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace LS.API.HRM.Admin.Controllers.SystemSetup
{
    public class LeaveTypeController : BaseController
    {
        public LeaveTypeController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {

        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {
            var list = await Mediator.Send(new GetLeaveTypeList() { Input = filter.Values(), User = UserInfo() });
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLeaveTypeById([FromRoute] int id)
        {
            var obj = await Mediator.Send(new GetLeaveTypeById() { Id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("getLeaveTypeSelectListItem")]
        public async Task<IActionResult> GetLeaveTypeSelectListItem([FromQuery] int employeeId, [FromQuery] string requestType)
        {
            var list = await Mediator.Send(new GetLeaveTypeSelectListItem() { EmployeeId = employeeId, RequestType = requestType, User = UserInfo() });
            return Ok(list);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TblHRMSysLeaveTypeDto dTO)
        {
            var result = await Mediator.Send(new CreateUpdateLeaveType() { Input = dTO, User = UserInfo() });

            if (result.Id > 0)
            {
                if (dTO.Id > 0)
                    return NoContent();
                else
                    return Created($"get/{result.Id}", dTO);
            }
            return BadRequest(new ApiMessageDto { Message = result.Message });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var GenderId = await Mediator.Send(new DeleteLeaveType() { Id = id, User = UserInfo() });
            if (GenderId > 0)
                return NoContent();
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }

    }
}
