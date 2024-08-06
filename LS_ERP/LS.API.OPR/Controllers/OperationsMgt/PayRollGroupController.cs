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
    public class PayRollGroupController : BaseController
    {

        public PayRollGroupController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }

        [HttpGet("getPayRollGroupsPagedList")]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {

            var list = await Mediator.Send(new GetPayRollGroupsPagedList() { Input = filter.Values(), User = UserInfo() });
            return Ok(list);
        }
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] HRM_DEF_PayrollGroupDto dTO)
        {
            var id = await Mediator.Send(new CreatePayRollGroup() { PayRollGroupDto = dTO, User = UserInfo() });
            if (id > 0)
            {
                return Created($"get/{id}", dTO);
            }
            else if (id == -1)
            {
                return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Duplicate(nameof(dTO.PayrollGroupID)) });
            }
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });

        }
        

        [HttpGet("getPayRollGroupById/{id}")]
        public async Task<IActionResult> Get([FromRoute] long id)
        {
            var obj = await Mediator.Send(new GetPayRollGroupById() { PayrollGroupID = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        //[HttpGet("getPayRollGroupByPayRollGroupCode/{PayRollGroupCode}")]
        //public async Task<IActionResult> Get([FromRoute] long PayRollGroupID)
        //{
        //    var obj = await Mediator.Send(new GetPayRollGroupByPayRollGroupCode() { PayRollGroupID = PayRollGroupID, User = UserInfo() });
        //    return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        //}

        [HttpGet("getSelectPayRollGroupList")]
        public async Task<IActionResult> GetSelectPayRollGroupList()
        {
            var item = await Mediator.Send(new GetSelectPayRollGroupList() { User = UserInfo() });
            return Ok(item);
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var PayRollGroupId = await Mediator.Send(new DeletePayRollGroup() { PayrollGroupID = id, User = UserInfo() });
            if (PayRollGroupId > 0)
                return NoContent();
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }
    }
}
