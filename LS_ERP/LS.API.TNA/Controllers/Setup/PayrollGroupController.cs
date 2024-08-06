using CIN.Application;
using CIN.Application.Common;
using CIN.Application.TimeAndAttendance.Setup.TNASetUpDtos;
using CIN.Application.TimeAndAttendance.Setup.TNASetUpQuery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LS.API.TNA.Controllers.Setup
{
    public class PayrollGroupController : BaseController
    {
        public PayrollGroupController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {

        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {
            var list = await Mediator.Send(new GetPayrollGroupList() { Input = filter.Values(), User = UserInfo() });
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var obj = await Mediator.Send(new GetPayrollGroupById() { Id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TblTNASysPayrollGroupDto dTO)
        {
            var result = await Mediator.Send(new CreateUpdatePayrollGroup() { Input = dTO, User = UserInfo() });

            if (result.Id > 0)
            {
                if (dTO.Id > 0)
                    return NoContent();
                else
                    return Created($"get/{result.Id}", dTO);
            }
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var AddressTypeId = await Mediator.Send(new DeletePayrollGroup() { Id = id, User = UserInfo() });
            if (AddressTypeId > 0)
                return NoContent();
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }

        [HttpGet("GetPayrollGroupSelectListItem")]
        public async Task<IActionResult> GetAddressTypeSelectListItem()
        {
            var list = await Mediator.Send(new GetPayrollGroupSelectListItem() { User = UserInfo() });
            return Ok(list);
        }
    }
}
