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
    public class SurveyFormController : BaseController
    {

        public SurveyFormController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TblSndDefSurveyFormDto dTO)
        {
            var id = await Mediator.Send(new CreateSurveyForm() { SurveyFormDto = dTO, User = UserInfo() });
            if (id > 0)
                return Created($"get/{id}", dTO);
            else if (id == -1)
            {
                return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Duplicate(nameof(dTO.SurveyFormCode)) });
            }
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }



        




        [HttpGet("getSurveyFormHeadById/{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var obj = await Mediator.Send(new GetSurveyFormHeadById() { Id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }


        [HttpGet("getPrintableSurveyFormByEnquiryId/{enquiryID}")]
        public async Task<IActionResult> GetPrintableSurveyForm([FromRoute] int enquiryID)
        {
            var obj = await Mediator.Send(new GetPrintableSurveyFormByEnquiryId() { EnquiryID = enquiryID, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("getSurveyFormTemplateById/{id}")]
        public async Task<IActionResult> GetSurveyFormTemplateById([FromRoute] int id)
        {
            var obj = await Mediator.Send(new GetSurveyFormTemplateById() { Id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

    }
}
