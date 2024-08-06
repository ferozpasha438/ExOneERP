using CIN.Application;
using CIN.Application.Common;
using CIN.Application.OperationsMgtDtos;
using CIN.Application.OperationsMgtQuery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LS.API.OPR.Controllers.OperationsMgt
{
    public class SkillsetController : BaseController
    {

        public SkillsetController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }
        #region Skillset Master

        [HttpGet("getSkillsetsPagedList")]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {

            var list = await Mediator.Send(new GetSkillsetsPagedList() { Input = filter.Values(), User = UserInfo() });
            return Ok(list);
        }
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TblOpSkillsetDto dTO)
        {
            var id = await Mediator.Send(new CreateSkillset() { SkillsetDto = dTO, User = UserInfo() });
            if (id > 0)
            {
                return Created($"get/{id}", dTO);
            }
            else if (id == -1)
            {
                return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Duplicate(nameof(dTO.SkillSetCode)) });
            }
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });

        }

        [HttpGet("getSkillsetById/{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var obj = await Mediator.Send(new GetSkillsetById() { Id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("getSkillsetBySkillsetCode/{skillsetCode}")]
        public async Task<IActionResult> Get([FromRoute] string SkillsetCode)
        {
            var obj = await Mediator.Send(new GetSkillsetBySkillsetCode() { SkillsetCode = SkillsetCode, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("isExistCode/{code}")]
        public async Task<bool> IsExistCode([FromRoute] string Code)
        {
            var obj = await Mediator.Send(new GetSkillsetBySkillsetCode() { SkillsetCode = Code, User = UserInfo() });
            return obj is not null ? true : false;
        }

        [HttpGet("getSelectSkillsetList")]
        public async Task<IActionResult> GetSelectSkillsetList()
        {
            var item = await Mediator.Send(new GetSelectSkillsetList() { User = UserInfo() });
            return Ok(item);
        }
        [HttpGet("getAllSkillsetList")]
        public async Task<IActionResult> GetAllSkillsetList()
        {
            var item = await Mediator.Send(new GetAllSkillsetList() { User = UserInfo() });
            return Ok(item);
        }

        [HttpGet("getSkillsetCodes")]
        public async Task<IActionResult> GetSkillsetCodes()
        {
            var item = await Mediator.Send(new GetSkillsetCodes() { User = UserInfo() });
            return Ok(item);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var SkillsetId = await Mediator.Send(new DeleteSkillset() { Id = id, User = UserInfo() });
            if (SkillsetId > 0)
                return NoContent();
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }
        #endregion




        #region SkillsetPlanForProject

        [HttpPost("CreateUpdateSkillsetPlanForProject")]
        public async Task<ActionResult> Create([FromBody] OpSkillsetPlanForProjectDto dTO)
        {
            var id = await Mediator.Send(new CreateUpdateSkillsetPlanForProject() { Skillset = dTO, User = UserInfo() });
            if (id > 0)
            {
                return Created($"get/{id}", dTO);
            }
            else if (id == -1)
            {
                return BadRequest(new ApiMessageDto { Message = "Roaster_Already_Exist_You_Cannot_Edit_Skillset_Plan" });
            }
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });

        }



        [HttpGet("getSkillsetPlanForProjectBySiteCode/{siteCode}")]
        public async Task<IActionResult> GetSkillsetPlanForProjectBySiteCode([FromRoute] string siteCode)
        {
            var obj = await Mediator.Send(new GetSkillsetPlanForProjectBySiteCode() { SiteCode = siteCode, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }


        [HttpGet("getAllSkillsetPlanForProjectBySiteCode/{siteCode}")]
        public async Task<IActionResult> GetAllSkillsetPlanForProjectBySiteCode([FromRoute] string siteCode)
        {
            var item = await Mediator.Send(new GetAllSkillsetPlanForProjectBySiteCode() { SiteCode = siteCode, User = UserInfo() });
            return Ok(item);
        }
        [HttpGet("getSkillsetsByProjectCodeAndSiteCode/{projectCode}/{siteCode}")]
        public async Task<IActionResult> GetSkillsetsByProjectCodeAndSiteCode([FromRoute] string siteCode, [FromRoute] string projectCode)
        {
            var item = await Mediator.Send(new GetSkillsetsByProjectCodeAndSiteCode() { SiteCode = siteCode,ProjectCode= projectCode, User = UserInfo() });
            return Ok(item);
        }




        #endregion


        #region employeeSkillsetForMonth
        [HttpPost("employeeSkillsetForMonth")]
        public async Task<IActionResult> EmployeeSkillsetForMonth([FromBody] InpuEmployeeSkillsetDto dto)
        {
            var obj = await Mediator.Send(new EmployeeSkillsetForMonth() { Input = dto, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        #endregion

    }
}
