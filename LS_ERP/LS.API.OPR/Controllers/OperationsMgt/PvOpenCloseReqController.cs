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
    public class PvOpenCloseReqController : BaseController
    {

        public PvOpenCloseReqController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }

        [HttpGet("getPvOpenCloseReqsPagedList")]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {

            var list = await Mediator.Send(new GetPvOpenCloseReqsPagedList() { Input = filter.Values(), User = UserInfo() });
            return Ok(list);
        }
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TblOpPvOpenCloseReqDto dTO)
        {
            var id = await Mediator.Send(new CreatePvOpenCloseReq() { PvOpenCloseReqDto = dTO, User = UserInfo() });
            if (id > 0)
            {
                return Created($"get/{id}", dTO);
            }
            else if (id == -1)
            {
                return BadRequest(new ApiMessageDto { Message = "Project Variations Request Already Exist" });
            }

            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });

        }



        [HttpGet("GetPvOpenCloseReqById/{id}")]
        public async Task<IActionResult> GetPvOpenCloseReqById([FromRoute] long id)
        {
            var obj = await Mediator.Send(new GetPvOpenCloseReqById() { Id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }





        [HttpGet("ApproveReqPvOpenCloseReqById/{id}")]
        public async Task<IActionResult> ApproveReqPvOpenCloseReqById([FromRoute] long id)
        {
            var res = await Mediator.Send(new ApproveReqPvOpenCloseReqById() { Id = id, User = UserInfo() });


            if (res > 0)
                return Ok(res);

            else if (res == -1)
            {
                return BadRequest(new ApiMessageDto { Message = "Request_not_exist" });
            }
            else if (res == -2)
            {
                return BadRequest(new ApiMessageDto { Message = "Request_Already_Approved" });
            }
            else if (res == -3)
            {
                return BadRequest(new ApiMessageDto { Message = "invalid_project_code" });
            }
            else if (res == -4)
            {
                return NotFound(new ApiMessageDto { Message = "invalid_Site_code" });
            }
            else if (res == -5)
            {
                return BadRequest(new ApiMessageDto { Message = "Attendance_Already_Exist" });
            }
       
            else if (res == -6)
            {
                return BadRequest(new ApiMessageDto { Message = "No Roaster Found" });
            }

             else if (res == -7)
            {
                return BadRequest(new ApiMessageDto { Message = "Remove Attendance From Recent Month, ReOpen Project form Next month" });
            }
            else if (res == -8)
            {
                return BadRequest(new ApiMessageDto { Message = "Invalid Request Type" });
            }
             else if (res == -9)
            {
                return BadRequest(new ApiMessageDto { Message = "Previous Ending Month Date Not Having Roaster" });
            }
            
              else if (res == -101)
            {
                return BadRequest(new ApiMessageDto { Message = "Reopen functionality blocked" });
            }
             else if (res == -102)
            {
                return BadRequest(new ApiMessageDto { Message = "Closing project Functionality blocked" });
            }
            



            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });

        }






        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var res = await Mediator.Send(new DeletePvOpenCloseReqById() { Id = id, User = UserInfo() });
            return res >0 ? Ok(id) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }






    }
}
