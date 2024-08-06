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
    public class PvTransferResourceController : BaseController
    {

        public PvTransferResourceController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }

        [HttpGet("getPvTransferResourceReqsPagedList")]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {

            var list = await Mediator.Send(new GetPvTransferResourceReqsPagedList() { Input = filter.Values(), User = UserInfo() });
            return Ok(list);
        }
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TblOpPvTransferResourceReqDto dTO)
        {
            var ValidtyReqData = await Mediator.Send(new IsValidTransferResourceRequest() { InputDto = dTO, User = UserInfo() });


            if (ValidtyReqData.IsValidReq)
            {
                var id = await Mediator.Send(new CreatePvTransferResourceReq() { PvTransferResourceReqDto = dTO, User = UserInfo() });
                if (id > 0)
                {
                    return Created($"get/{id}", dTO);
                }
            }
            else
            {
                return BadRequest(new ApiMessageDto { Message = "Error Code" + ValidtyReqData.ErrorId.ToString() + ",Error:" + ValidtyReqData.ErrorMsg });

            }




            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });

        }
        [HttpGet("getPvTransferResourceReqById/{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var obj = await Mediator.Send(new GetPvTransferResourceReqById() { Id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var obj = await Mediator.Send(new DeletePvTransferResourceReqById() { Id = id, User = UserInfo() });
            return obj > 0 ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("ApproveReqPvTransferResourceReqById/{id}")]
        public async Task<IActionResult> ApproveReqPvTransferResourceReqById([FromRoute] int id)
        {
            var Request = await Mediator.Send(new GetPvTransferResourceReqById() { Id = id });
            if (Request is null)
            {
                return BadRequest(new ApiMessageDto { Message = "Invalid Request Id" });
            }

            var ValidtyReqData = await Mediator.Send(new IsValidTransferResourceRequest() { InputDto = Request, User = UserInfo() });


            if (ValidtyReqData.IsValidReq)
            {
                var obj = await Mediator.Send(new ApproveReqPvTransferResourceReqById() { Id = id, User = UserInfo() });

                if (obj is not null)
                {
                    if (obj.Id > 0)
                        return Ok(obj);
                    else if (obj.Id == -1)
                        return BadRequest(new ApiMessageDto { Message = "No Request Found" });
                    else if (obj.Id == -2)
                        return BadRequest(new ApiMessageDto { Message = "Request Already Approved" });
                    else if (obj.Id == -3)
                        return BadRequest(new ApiMessageDto { Message = "invalid SrcProject code" });
                    else if (obj.Id == -4)
                        return BadRequest(new ApiMessageDto { Message = "invalid DesPproject code" });
                    else if (obj.Id == -5)
                        return BadRequest(new ApiMessageDto { Message = "Incompatible Projects Dates" });
                    else if (obj.Id == -6)
                        return BadRequest(new ApiMessageDto { Message = "Employee not having roaster" });
                    else if (obj.Id == -7)
                        return BadRequest(new ApiMessageDto { Message = "Employee Already Exist in Dest Project" });
                    else if (obj.Id == -8)
                        return BadRequest(new ApiMessageDto { Message = "Attendance already entered" });

                    else if (obj.Id == -9)
                        return BadRequest(new ApiMessageDto { Message = "Roasters Incomplete or not found in Dest Project" });
                    else if (obj.Id == -10)
                        return BadRequest(new ApiMessageDto { Message = "Roaster For Partial Month Not Found in Destination" });

                    else if (obj.Id == -11)
                        return BadRequest(new ApiMessageDto { Message = "Shifts Not Assigned In Source Project" });

                    else if (obj.Id == -12)
                        return BadRequest(new ApiMessageDto { Message = " Invalid Employee  Number" });
                    else if (obj.Id == -13)
                    {
                        return BadRequest(new ApiMessageDto { Message = "EMployee_Transfer_Log_Not_Found" });

                    }
                    else if (obj.Id == -14)
                    {
                        return BadRequest(new ApiMessageDto { Message = "Destination Site Not Having Roaster" });

                    }


                }

            }
            return NotFound(new ApiMessageDto { Message = ApiMessageInfo.Failed });

        }
    }
    }
