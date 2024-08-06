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
    public class SurveyFormElementController : BaseController
    {

        public SurveyFormElementController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }

        [HttpGet("getSurveyFormElementsPagedList")]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {

            var list = await Mediator.Send(new GetSurveyFormElementsPagedList() { Input = filter.Values(), User = UserInfo() });
            return Ok(list);
        }
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TblSndDefSurveyFormElementDto dTO)
        {
            var id = await Mediator.Send(new CreateSurveyFormElement() { SurveyFormElementDto = dTO, User = UserInfo() });
            if (id > 0)
                return Created($"get/{id}", dTO);
            else if (id == -1)
            {
                return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Duplicate(nameof(dTO.FormElementCode)) });
            }
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }

        [HttpGet("getSurveyFormElementById/{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var obj = await Mediator.Send(new GetSurveyFormElementById() { Id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("getSurveyFormElementBySurveyFormElementCode/{SurveyFormElementCode}")]
        public async Task<IActionResult> Get([FromRoute] string SurveyFormElementCode)
        {
            var obj = await Mediator.Send(new GetSurveyFormElementByElementCode() { SurveyFormElementCode = SurveyFormElementCode, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("getSelectSurveyFormElementList")]
        public async Task<IActionResult> GetSelectSurveyFormElementList()
        {
            var item = await Mediator.Send(new GetSelectSurveyFormElementList() {User = UserInfo() });
            return Ok(item);
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var SurveyFormElementId = await Mediator.Send(new DeleteSurveyFormElement() { Id = id, User = UserInfo() });
            if (SurveyFormElementId > 0)
                return NoContent();
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }
    }
}
