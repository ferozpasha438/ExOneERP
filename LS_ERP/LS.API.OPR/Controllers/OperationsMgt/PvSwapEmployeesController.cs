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
    public class PvSwapEmployeesController : BaseController
    {

        public PvSwapEmployeesController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }




        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TblOpPvSwapEmployeesReqDto dTO)
        {
            var ValidtyReqData = await Mediator.Send(new IsValidSwapEmployeesRequest() { InputDto = dTO, User = UserInfo() });


            if (ValidtyReqData.IsValidReq)
            {
                var res = await Mediator.Send(new CreatePvSwapEmployeesReq() { InputDto = dTO, User = UserInfo() });
                if (res.IsSuccess)
                {
                    return Created($"get/{dTO.Id}", dTO);
                }
                return BadRequest(new ApiMessageDto {Message= ApiMessageInfo.Failed });
            }
            else
            { 
            return BadRequest(new ApiMessageDto { Message ="Error Code" + ValidtyReqData.ErrorId.ToString()+",Error:"+ ValidtyReqData.ErrorMsg });

            }



        }



        [HttpGet("getPvSwapEmployeesReqsPagedList")]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {

            var list = await Mediator.Send(new GetPvSwapEmployeesReqsPagedList() { Input = filter.Values(), User = UserInfo() });
            return Ok(list);
        }




        [HttpGet("getPvSwapEmployeesReqById/{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var obj = await Mediator.Send(new GetPvSwapEmployeesReqById() { Id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var obj = await Mediator.Send(new DeletePvSwapEmployeesReqById() { Id = id, User = UserInfo() });
            return obj > 0 ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("ApproveReqPvSwapEmployeesReqById/{id}")]
        public async Task<IActionResult> ApproveReqPvReplaceResourceReqById([FromRoute] int id)
        {
            var obj = await Mediator.Send(new GetPvSwapEmployeesReqById() { Id=id});
            if (obj is null) { 
                return BadRequest(new ApiMessageDto { Message = "Invalid Request Id" });
            }

            var ValidtyReqData = await Mediator.Send(new IsValidSwapEmployeesRequest() { InputDto = obj, User = UserInfo() });


            if (ValidtyReqData.IsValidReq)
            {
                var res = await Mediator.Send(new ApproveReqPvSwapEmployeesReqById() { Id = id, User = UserInfo() });
                if (res.IsSuccess)
                {
                    return Created($"get/{id}", res);
                }
                return BadRequest(new ApiMessageDto { Message = res.ErrorMsg });
            }
            else
            {
                return BadRequest(new ApiMessageDto { Message = "Error Code" + ValidtyReqData.ErrorId.ToString() + ",Error:" + ValidtyReqData.ErrorMsg });

            }


        }









    }
       
}
