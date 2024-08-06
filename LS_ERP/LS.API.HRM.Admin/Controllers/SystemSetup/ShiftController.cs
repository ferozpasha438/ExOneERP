using CIN.Application;
using CIN.Application.Common;
using CIN.Application.HumanResource.SetUp.HRMSetUpDtos;
using CIN.Application.HumanResource.SetUp.HRMSetUpQuery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LS.API.HRM.Admin.Controllers.SystemSetup
{

    public class ShiftController : BaseController
    {
        public ShiftController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {

        }
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {
            var list = await Mediator.Send(new GetShiftList() { Input = filter.Values(), User = UserInfo() });
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var obj = await Mediator.Send(new GetShiftById() { Id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TblHRMSysShiftDto dTO)
        {
            try
            {
                var result = await Mediator.Send(new CreateUpdateShift() { Input = dTO, User = UserInfo() });

                if (result.Id > 0)
                {
                    if (dTO.Id > 0)
                        return NoContent();
                    else
                        return Created($"get/{result.Id}", dTO);
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var BankId = await Mediator.Send(new DeleteShift() { Id = id, User = UserInfo() });
            if (BankId > 0)
                return NoContent();
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }

        [HttpGet("GetShiftSelectListItem")]
        public async Task<IActionResult> GetBankSelectListItem()
        {
            var list = await Mediator.Send(new GetShiftSelectListItem() { User = UserInfo() });
            return Ok(list);
        }
    }
}
