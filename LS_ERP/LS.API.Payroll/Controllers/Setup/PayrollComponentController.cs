using CIN.Application;
using CIN.Application.Common;
using CIN.Application.Payroll.SetUp.Dtos;
using CIN.Application.Payroll.SetUp.Query;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LS.API.Payroll.Controllers.Setup
{

    public class PayrollComponentController : BaseController
    {
        public PayrollComponentController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {

        }


        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {
            var list = await Mediator.Send(new GetPayrollComponentList() { Input = filter.Values(), User = UserInfo() });
            return Ok(list);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var obj = await Mediator.Send(new GetPayrollComponentById() { Id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TblPRLSysPayrollComponentDto dTO)
        {
            var result = await Mediator.Send(new CreateUpdatePayrollComponent() { Input = dTO, User = UserInfo() });

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
            var PayrollPeriodId = await Mediator.Send(new DeletePayrollComponent() { Id = id, User = UserInfo() });
            if (PayrollPeriodId > 0)
                return NoContent();
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }

        [HttpGet("GetPayrollComponentSelectListItem")]
        public async Task<IActionResult> GetPayrollComponentSelectListItem([FromQuery] bool isStructured)
        {
            var list = await Mediator.Send(new GetPayrollComponentSelectListItem() { IsStructured = isStructured, User = UserInfo() });
            return Ok(list);
        }

        [HttpGet("GetPayrollComponentTypeSelectListItem")]
        public async Task<IActionResult> GetPayrollComponentTypeSelectListItem()
        {
            var list = await Mediator.Send(new GetPayrollComponentTypeSelectListItem() { User = UserInfo() });
            return Ok(list);
        }

        [HttpGet("GetPayrollComponentByCode")]
        public async Task<IActionResult> GetPayrollComponentByCode([FromQuery] string payrollComponentCode)
        {
            var obj = await Mediator.Send(new GetPayrollComponentByCode() { PayrollComponentCode = payrollComponentCode, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("GetPayrollComponents")]
        public async Task<IActionResult> GetPayrollComponents([FromQuery] bool isStructured)
        {
            var list = await Mediator.Send(new GetPayrollComponents() { IsStructured = isStructured, User = UserInfo() });
            return Ok(list);
        }
    }
}
