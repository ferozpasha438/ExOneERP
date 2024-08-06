using CIN.Application;
using CIN.Application.Common;
using CIN.Application.HumanResource.EmployeeMgmt.HRMgmtDtos;
using CIN.Application.HumanResource.SetUp.HRMSetUpDtos;
using CIN.Application.HumanResource.SetUp.HRMSetUpQuery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace LS.API.HRM.Admin.Controllers.SystemSetup
{
    public class LeaveTemplateController : BaseController
    {
        public LeaveTemplateController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {

        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {
            var list = await Mediator.Send(new GetLeaveTemplateList() { Input = filter.Values(), User = UserInfo() });
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLeaveTemplateById([FromRoute] int id)
        {
            var obj = await Mediator.Send(new GetLeaveTemplateById() { Id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("getLeaveTemplateMappingList")]
        public async Task<IActionResult> GetLeaveTemplateMappingList([FromQuery] string templateCode)
        {
            var obj = await Mediator.Send(new GetLeaveTemplateMappingList() { TemplateCode = templateCode, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TblHRMSysLeaveTemplateDto dTO)
        {
            var result = await Mediator.Send(new CreateUpdateLeaveTemplate() { Input = dTO, User = UserInfo() });

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
            var GenderId = await Mediator.Send(new DeleteLeaveTemplate() { Id = id, User = UserInfo() });
            if (GenderId > 0)
                return NoContent();
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }

        [HttpGet("GetLeaveTemplateSelectListItem")]
        public async Task<IActionResult> GetLeaveTemplateSelectListItem([FromQuery] EmployeeLeaveInfoFilterDto filter)
        {
            var list = await Mediator.Send(new GetLeaveTemplateSelectListItem() { User = UserInfo(), Filter = filter });
            return Ok(list);
        }
    }
}
