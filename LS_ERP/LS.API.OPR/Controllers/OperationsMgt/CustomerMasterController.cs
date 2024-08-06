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

    public class CustomerMasterController : BaseController
    {
        public CustomerMasterController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {
            //await Task.Delay(3000);
            var list = await Mediator.Send(new GetCustomersPagedList() { Input = filter.Values(), User = UserInfo() });
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var obj = await Mediator.Send(new GetCustomer() { Id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TblSndDefCustomerMasterDto dTO)
        {
            var id = await Mediator.Send(new CreateCustomer() { CustomerDto = dTO, User = UserInfo() });
            if (id > 0)
                return Created($"get/{id}", dTO);
            else if (id == -1)
            {
                return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Duplicate(nameof(dTO.CustCode)) });
            }
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }
        [HttpGet("getSelectCustomerList")]
        public async Task<IActionResult> GetSelectCustomerList(string search)
        {
            //await Task.Delay(3000);
            var item = await Mediator.Send(new GetCustomerSelectItemList() { Input = search, User = UserInfo() });
            return Ok(item);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var customerId = await Mediator.Send(new DeleteCustomer() { Id = id, User = UserInfo() });
            if (customerId > 0)
                return NoContent();
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }

        [HttpGet("getCustomerByCustomerCode/{custCode}")]
        public async Task<TblSndDefCustomerMasterDto> Get([FromRoute]string custCode)
        {
            var obj = await Mediator.Send(new GetCustomerByCustomerCode() { CustomerCode = custCode, User = UserInfo() });
            return obj;
        }

        [HttpGet("canAutoGenerateCustCode")]
        public async Task<IActionResult> CanAutoGenerateCustCode()
        {
            //await Task.Delay(3000);
            var item = await Mediator.Send(new CanAutoGenerateCustCode() { User = UserInfo() });
            return Ok(item);
        }


        [HttpPost("getCustomersPagedListWithFilter")]
        public async Task<IActionResult> GetCustomersPagedListWithFilter([FromQuery] PaginationFilterDto filter, [FromBody] OprFilter filterInput)
        {

            var list = await Mediator.Send(new GetCustomersPagedListWithFilter() { Input = filter.Values(), FilterInput = filterInput, User = UserInfo() });
            return Ok(list);
        }
    }
}
