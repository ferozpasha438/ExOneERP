//using CIN.Application;
//using CIN.Application.Common;
//using CIN.Application.OperationsMgtDtos;
//using CIN.Application.OperationsMgtQuery;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Options;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace LS.API.OPR.Controllers.OperationsMgt
//{
//    public class SurveyFormElementMappController : BaseController
//    {

//        public SurveyFormElementMappController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
//        {
//        }


//        [HttpPost]
//        public async Task<ActionResult> Create([FromBody] TblSndDefSurveyFormElementsMappDto dTO)
//        {
//            var id = await Mediator.Send(new CreateSurveyFormElementMapping() { Dto = dTO, User = UserInfo() });
//            if (id > 0)
//                return Created($"get/{id}", dTO);
//            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
//        }


//        [HttpDelete("{id}")]
//        public async Task<ActionResult> Delete([FromRoute] int id)
//        {
//            var SurveyFormElementId = await Mediator.Send(new DeleteSurveyFormElementMapping() { Id = id, User = UserInfo() });
//            if (SurveyFormElementId > 0)
//                return NoContent();
//            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
//        }
//    }
//}
