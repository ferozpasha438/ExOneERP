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
    public class ServiceEnquiriesController : BaseController
    {

        public ServiceEnquiriesController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }


        [HttpGet("getSevriceEnquiriesPagedList")]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {

            var list = await Mediator.Send(new GetServiceEnquiriesPagedList() { Input = filter.Values(), User = UserInfo() });
            return Ok(list);
        }


        [HttpGet("getSevriceEnquiriesByEnquiryNumberPagedList")]
        public async Task<IActionResult> GetSevriceEnquiriesByEnquiryNumberPagedList([FromQuery] PaginationFilterDto filter)
        {

            var list = await Mediator.Send(new GetEnquiriesByEnquiryNumberPagedList() { Input = filter.Values(), User = UserInfo() });
            return Ok(list);
        }
[HttpGet("getSevriceEnquiriesByEnquiryNumber/{enquiryNumber}")]
        public async Task<IActionResult> GetSevriceEnquiriesByEnquiryNumberPagedList([FromRoute] string enquiryNumber)
        {

            var list = await Mediator.Send(new GetEnquiriesByEnquiryNumber() {EnquiryNumber= enquiryNumber, User = UserInfo() });
            return Ok(list);
        }

        [HttpGet("getSevriceEnquiriesBySurveyorCodePagedList")]
        public async Task<IActionResult> GetSevriceEnquiriesBySurveyorCodePagedList([FromQuery] PaginationFilterDto filter)
        {

            var list = await Mediator.Send(new GetServiceEnquiriesBySurveyorCodePagedList() { Input = filter.Values(), User = UserInfo() });
            return Ok(list);
        }






        [HttpPost("addSurveyorToEnquiry")]
        public async Task<ActionResult> AddSurveyorToEnquiry(TblSndDefServiceEnquiriesDto enqDto)
        {
            var EnquiryForm = await Mediator.Send(new AddSurveyorToEnquiry() { SurveyorCode = enqDto.SurveyorCode, EnquiryID = enqDto.EnquiryID, User = UserInfo() });
            if (EnquiryForm.Id > 0)
                return Created($"get/{EnquiryForm.Id}", enqDto.EnquiryID);
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }

        [HttpPost("changeEnquiryStatus")]
        public async Task<ActionResult> Create([FromBody] TblSndDefServiceEnquiriesDto enqDto)
        {
            var EnquiryForm = await Mediator.Send(new ChangeEnquiryStatus() {EnquiryID=enqDto.EnquiryID,StatusEnquiry=enqDto.StatusEnquiry, User = UserInfo() });
            if (EnquiryForm.Id > 0)
                return Created($"get/{EnquiryForm.Id}", EnquiryForm.Id);
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }
        [HttpGet("getEnquiryByEnquiryId/{enquiryID}")]
        public async Task<IActionResult> Get([FromRoute] int enquiryID)
        {
            var obj = await Mediator.Send(new GetEnquiryByEnquiryId() { EnquiryID = enquiryID, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var ServiceId = await Mediator.Send(new DeleteServiceEnquiry() { Id = id, User = UserInfo() });
            if (ServiceId > 0)
                return NoContent();
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }
    }
}
