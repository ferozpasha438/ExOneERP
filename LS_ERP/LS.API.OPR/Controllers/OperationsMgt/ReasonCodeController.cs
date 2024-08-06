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
    public class ReasonCodeController : BaseController
    {

        public ReasonCodeController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }

        [HttpGet("getReasonCodesPagedList")]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {

            var list = await Mediator.Send(new GetReasonCodesPagedList() { Input = filter.Values(), User = UserInfo() });
            return Ok(list);
        }
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TblOpReasonCodeDto dTO)
        {
            var id = await Mediator.Send(new CreateReasonCode() { ReasonCodeDto = dTO, User = UserInfo() });
            if (id > 0)
            {
                return Created($"get/{id}", dTO);
            }
            else if (id == -1)
            {
                return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Duplicate(nameof(dTO.ReasonCode)) });
            }
            else if (id == -2)
            {
                return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Duplicate(nameof(dTO.Id)) });
            }
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });

        }

        [HttpGet("getReasonCodeById/{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var obj = await Mediator.Send(new GetReasonCodeById() { Id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("getReasonCodeByReasonCode/{ReasonCode}")]
        public async Task<IActionResult> Get([FromRoute] string ReasonCode)
        {
            var obj = await Mediator.Send(new GetReasonCodeByReasonCode() { ReasonCode = ReasonCode, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("getSelectReasonCodeList")]
        public async Task<IActionResult> GetSelectReasonCodeList()
        {
            var item = await Mediator.Send(new GetSelectReasonCodeList() { User = UserInfo() });
            return Ok(item);
        }

          [HttpGet("getSelectReasonCodeListForCustomerVisit")]
        public async Task<IActionResult> GetSelectReasonCodeListForCustomerVisit()
        {
            var items = await Mediator.Send(new GetSelectReasonCodeListForCustomerVisit() { User = UserInfo() });
           return Ok(items);
        }
          [HttpGet("getSelectReasonCodeListForCustomerComplaint")]
        public async Task<IActionResult> GetSelectReasonCodeListForCustomerComplaint()
        {
            var items = await Mediator.Send(new GetSelectReasonCodeListForCustomerComplaint() { User = UserInfo() });
           return Ok(items);
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var ReasonCodeId = await Mediator.Send(new DeleteReasonCode() { Id = id, User = UserInfo() });
            if (ReasonCodeId > 0)
                return NoContent();
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }
    }
}
