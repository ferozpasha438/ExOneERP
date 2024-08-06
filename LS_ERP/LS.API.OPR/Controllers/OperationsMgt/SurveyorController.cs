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
    public class SurveyorsController : BaseController
    {

        public SurveyorsController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }

        [HttpGet("getSurveyorsPagedList")]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {

            var list = await Mediator.Send(new GetSurveyorsPagedList() { Input = filter.Values(), User = UserInfo() });
            return Ok(list);
        }
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TblSndDefSurveyorDto dTO)
        {
            var id = await Mediator.Send(new CreateSurveyor() { SurveyorDto = dTO, User = UserInfo() });
            if (id > 0)
            {
                return Created($"get/{id}", dTO);
            }
            else if (id == -1)
            {
                return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Duplicate(nameof(dTO.SurveyorCode)) });
            }
            else if (id == -2)
            {
                return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Duplicate(nameof(dTO.UserId)) });
            }
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });

        }

        [HttpGet("getSurveyorById/{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var obj = await Mediator.Send(new GetSurveyorById() { Id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("getSurveyorBySurveyorCode/{SurveyorCode}")]
        public async Task<IActionResult> Get([FromRoute] string SurveyorCode)
        {
            var obj = await Mediator.Send(new GetSurveyorBySurveyorCode() { SurveyorCode = SurveyorCode, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("getSelectSurveyorList")]
        public async Task<IActionResult> GetSelectSurveyorList()
        {
            var item = await Mediator.Send(new GetSelectSurveyorList() { User = UserInfo() });
            return Ok(item);
        }
         [HttpGet("getSelectSurveyorListForBranch/{branchCode}")]
        public async Task<IActionResult> GetSelectSurveyorListForBranch([FromRoute] string BranchCode)
        {
            var item = await Mediator.Send(new GetSelectSurveyorListForBranch() { User = UserInfo() ,BranchCode=BranchCode});
            return Ok(item);
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var SurveyorId = await Mediator.Send(new DeleteSurveyor() { Id = id, User = UserInfo() });
            if (SurveyorId > 0)
                return NoContent();
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }
    }
}
