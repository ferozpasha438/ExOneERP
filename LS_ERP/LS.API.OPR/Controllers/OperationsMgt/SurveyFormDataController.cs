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
    public class SurveyFormDataController : BaseController
    {

        public SurveyFormDataController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] EditableSurveyFormDataDto dTO)
        {
            var id = await Mediator.Send(new CreateSurveyFormData() { Input = dTO, User = UserInfo() });
            if (id > 0)
                return Created($"get/{id}", dTO.EnquiryID);
          
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }

        [HttpGet("getSurveyFormDataByEnquiryId/{enquiryID}")]
        public async Task<IActionResult> Get([FromRoute] int enquiryID)
        {
            var obj = await Mediator.Send(new GetSurveyFormDataByEnquiryId() { EnquiryID = enquiryID, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }







    }
}
