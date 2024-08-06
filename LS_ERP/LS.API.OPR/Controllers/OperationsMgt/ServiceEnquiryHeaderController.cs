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
    public class ServiceEnquiryHeaderController : BaseController
    {

        public ServiceEnquiryHeaderController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }


        [HttpGet("getSevriceEnquiryHeaderPagedList")]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {

            var list = await Mediator.Send(new GetEnquiryFormPagedList() { Input = filter.Values(), User = UserInfo() });
            return Ok(list);
        }
        //    [HttpPost]
        //    public async Task<ActionResult> Create([FromBody] TblSndDefServiceEnquiryDto dTO)
        //    {
        //        var id = await Mediator.Send(new CreateServiceEnquiry() { ServiceEnquiryDto = dTO, User = UserInfo() });
        //        if (id > 0)
        //            return Created($"get/{id}", dTO);
        //        return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        //    }

        //    [HttpGet("getServiceEnquiryById/{id}")]
        //    public async Task<IActionResult> Get([FromRoute] int id)
        //    {
        //        var obj = await Mediator.Send(new GetServiceEnquiryById() { Id = id, User = UserInfo() });
        //        return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        //    }

        [HttpGet("getEnquiryFormHeaderByEnquiryNumber/{enquiryNumber}")]
        public async Task<IActionResult> Get([FromRoute] string enquiryNumber)
        {
            var obj = await Mediator.Send(new GetEnquiryFormHeaderByEnquiryNumber() { EnquiryNumber = enquiryNumber, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }





        [HttpGet("getEnquiryFormHeaderById/{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var obj = await Mediator.Send(new GetEnquiryFormHeaderById() { Id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }


        [HttpGet("genEnquiryHeadCode")]
        public async Task<string> GenEnquiryHeadCode()
        {
            var obj = await Mediator.Send(new GenEnquiryHeadCode() { User = UserInfo() });
            return obj;
        }

        //    [HttpGet("getSelectServiceEnquiryList")]
        //    public async Task<IActionResult> GetSelectServiceEnquiryList(string search)
        //    {
        //        var item = await Mediator.Send(new GetSelectServiceEnquiryList() { Input = search, User = UserInfo() });
        //        return Ok(item);
        //    }
        //    [HttpDelete("{id}")]
        //    public async Task<ActionResult> Delete([FromRoute] int id)
        //    {
        //        var UnitId = await Mediator.Send(new DeleteServiceEnquiry() { Id = id, User = UserInfo() });
        //        if (UnitId > 0)
        //            return NoContent();
        //        return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        //    }
        //}
    }
}
