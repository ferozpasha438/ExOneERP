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
    public class SurveyFormHeaderController : BaseController
    {

        public SurveyFormHeaderController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }


        [HttpGet("getSurveyFormHeaderPagedList")]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {

            var list = await Mediator.Send(new GetSurveyFormHeaderPagedList() { Input = filter.Values(), User = UserInfo() });
            return Ok(list);
        }


        [HttpGet("getSurveyFormHeaderBySuveyFormCode/{surveyFormCode}")]
        public async Task<IActionResult> Get([FromRoute] string surveyFormCode)
        {
            var obj = await Mediator.Send(new GetSurveyFormHeaderBySurveyFormCode() { SurveyFormCode = surveyFormCode, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }





        [HttpGet("getSurveyFormHeaderById/{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var obj = await Mediator.Send(new GetSurveyFormHeaderById() { Id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("getSelectSurveyFormList")]
        public async Task<IActionResult> GetSelectSurveyFormList()
        {
            var item = await Mediator.Send(new GetSelectSurveyFormHeaderList() { User = UserInfo() });
            return Ok(item);
        }

    }
}
