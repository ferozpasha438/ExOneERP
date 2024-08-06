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
    public class PvReplaceResourceController : BaseController
    {

        public PvReplaceResourceController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }

        [HttpGet("getPvReplaceResourceReqsPagedList")]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {

            var list = await Mediator.Send(new GetPvReplaceResourceReqsPagedList() { Input = filter.Values(), User = UserInfo() });
            return Ok(list);
        }
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TblOpPvReplaceResourceReqDto dTO)
        {
            var CheckValidityReqRes = await Mediator.Send(new IsValidReplaceResourceRequest()
            {
                InputDto = dTO,
                User=UserInfo()
            });

            if (CheckValidityReqRes.IsValidReq)
            {
                var id = await Mediator.Send(new CreatePvReplaceResourceReq() { PvReplaceResourceReqDto = dTO, User = UserInfo() });
                if (id > 0)
                {
                    return Created($"get/{id}", dTO);
                }
            }
            else if (CheckValidityReqRes.ErrorId<0)
            {
                return BadRequest(new ApiMessageDto { Message =CheckValidityReqRes.ErrorMsg });
            }

           
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });

        }
        [HttpGet("getPvReplaceResourceReqById/{id}")]
        public async Task<IActionResult> GetPvReplaceResourceReqById([FromRoute] long id)
        {
            var obj = await Mediator.Send(new GetPvReplaceResourceReqById() { Id = id/*, User = UserInfo() */});
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
    [HttpGet("ApproveReqPvReplaceResourceReqById/{id}")]
        public async Task<IActionResult> ApproveReqPvReplaceResourceReqById([FromRoute] int id)
        {
            var dTO = await Mediator.Send(new GetPvReplaceResourceReqById(){ Id=id });
            if (dTO is null)
            { 
            return  NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });

            }

            var CheckValidityReqRes = await Mediator.Send(new IsValidReplaceResourceRequest() {InputDto = dTO,User = UserInfo()});

            if (CheckValidityReqRes.IsValidReq)
            {
                var obj = await Mediator.Send(new ApproveReqPvReplaceResourceReqById() { Id = id, User = UserInfo() });

                if (obj is not null)
                {
                    if (obj.Id > 0)
                        return Ok(obj);
                    else if (obj.Id == -1)
                        return BadRequest(new ApiMessageDto { Message = "No Request Found" });
                    else if (obj.Id == -2)
                        return BadRequest(new ApiMessageDto { Message = "Request Already Approved" });
                    else if (obj.Id == -3)
                        return BadRequest(new ApiMessageDto { Message = "Invalid project code" });
                    else if (obj.Id == -4)
                        return BadRequest(new ApiMessageDto { Message = "No Replacements Found ,Resigned Employee not having roaster" });
                    else if (obj.Id == -5)
                        return BadRequest(new ApiMessageDto { Message = "Replace Employee Already Exist" });
                    else if (obj.Id == -6)
                        return BadRequest(new ApiMessageDto { Message = "Attendance already entered" });

                    else if (obj.Id == -7)
                        return BadRequest(new ApiMessageDto { Message = "Invalid Employee  Number" });
                     else if (obj.Id == -8)
                        return BadRequest(new ApiMessageDto { Message = "Primary Site log not exist for resigned Employe" });



                }
            }
            else
            { 
                return BadRequest(new ApiMessageDto { Message =CheckValidityReqRes.ErrorMsg });
            }

            return  NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
            
        }
  [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var obj = await Mediator.Send(new DeletePvReplaceResourceReqById() { Id = id, User = UserInfo() });
            return obj>0 ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

    }
}
