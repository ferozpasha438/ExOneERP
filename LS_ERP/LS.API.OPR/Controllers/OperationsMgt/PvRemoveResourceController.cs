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
    public class PvRemoveResourceController : BaseController
    {

        public PvRemoveResourceController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }

        [HttpGet("getPvRemoveResourceReqsPagedList")]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {

            var list = await Mediator.Send(new GetPvRemoveResourceReqsPagedList() { Input = filter.Values(), User = UserInfo() });
            return Ok(list);
        }
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TblOpPvRemoveResourceReqDto dTO)
        {
            var Request= await Mediator.Send(new IsValidRemoveResourceRequest() { InputDto = dTO, User = UserInfo() });
            if (Request.IsValidReq)
            {
                var id = await Mediator.Send(new CreatePvRemoveResourceReq() { PvRemoveResourceReqDto = dTO, User = UserInfo() });
                if (id > 0)
                {
                    return Created($"get/{id}", dTO);
                }
            }
            else if (Request.ErrorId<0)
            {
                return BadRequest(new ApiMessageDto { Message = Request.ErrorMsg });
           }


            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });

        }
        [HttpGet("getPvRemoveResourceReqById/{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var obj = await Mediator.Send(new GetPvRemoveResourceReqById() { Id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
  [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var obj = await Mediator.Send(new DeletePvRemoveResourceReqById() { Id = id, User = UserInfo() });
            return obj>0 ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("ApproveReqPvRemoveResourceReqById/{id}")]
        public async Task<IActionResult> ApproveReqPvReplaceResourceReqById([FromRoute] int id)
        {
            var Request = await Mediator.Send(new GetPvRemoveResourceReqById() { Id = id });
            if (Request is null)
            {
                return BadRequest(new ApiMessageDto { Message = "Invalid Request Id" });
            }

            var ValidtyReqData = await Mediator.Send(new IsValidRemoveResourceRequest() { InputDto = Request, User = UserInfo() });


            if (ValidtyReqData.IsValidReq)
            {
                var obj = await Mediator.Send(new ApproveReqPvRemoveResourceReqById() { Id = id, User = UserInfo() });

                if (obj is not null)
                {

                    if (obj.Id > 0)
                        return Ok(obj);
                    else
                        return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed+",ErrorId:"+obj.Id });
                }
            }
            else if (ValidtyReqData.ErrorId < 0)

            {
                return NotFound(new ApiMessageDto { Message = ValidtyReqData.ErrorMsg });

            }
            return NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });

        }
    }
}
