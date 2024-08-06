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
    public class ServiceEnquiryFormController : BaseController
    {

        public ServiceEnquiryFormController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] ServicesEnquiryFormDto dTO)
        {
            var id = await Mediator.Send(new CreateServicesEnquiryForm() { ServicesEnquiryFormDto = dTO, User = UserInfo() });
            if (id > 0)
                return Created($"get/{id}", dTO);
            else if (id == -1)
            {
                return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Duplicate(nameof(dTO.EnquiryNumber)) });
            }

            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }


        //[HttpPost("approveEnquiryForm")]
        //public async Task<ActionResult> Create(string EnquiryNumber)
        //{
        //    var EnquiryForm = await Mediator.Send(new ApproveEnquiryForm() { EnquiryNumber = EnquiryNumber, User = UserInfo() });
        //    if (EnquiryForm.Id > 0)
        //        return Created($"get/{EnquiryForm.Id}", EnquiryNumber);
        //    return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        //}



        [HttpPost("changeEnquiryFormStatus")]
        public async Task<ActionResult> Create(string EnquiryNumber,string EnquiryStatus)
        {
            var EnquiryForm = await Mediator.Send(new ChangeEnquiryFormStatus() {EnquiryStatus=EnquiryStatus, EnquiryNumber = EnquiryNumber, User = UserInfo() });
            if (EnquiryForm.Id > 0)
                return Created($"get/{EnquiryForm.Id}", EnquiryNumber);
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }

        [HttpGet("getEnquiryFormByEnquiryNumber/{enquiryNumber}")]
        public async Task<IActionResult> Get([FromRoute] string enquiryNumber)
        {
            var obj = await Mediator.Send(new GetEnquiryFormByEnquiryNumber() { EnquiryNumber = enquiryNumber, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("getEnquiryFormById/{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var obj = await Mediator.Send(new GetEnquiryFormById() { Id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
    }
}
