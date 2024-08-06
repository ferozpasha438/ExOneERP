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
    public class ServiceUnitMapController : BaseController
    {

        public ServiceUnitMapController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }

        [HttpGet("getServiceUnitMapPagedList")]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {

            var list = await Mediator.Send(new GetServiceUnitMapPagedList() { Input = filter.Values(), User = UserInfo() });
            return Ok(list);
        }
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TblSndDefServiceUnitMapDto dTO)
        {
            var id = await Mediator.Send(new CreateServiceUnitMap() { supDto = dTO, User = UserInfo() });
            if (id > 0)
                return Created($"get/{id}", dTO);
            else if (id == -1)
            {
                return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Duplicate("Mapping of "+nameof(dTO.ServiceCode)+" "+nameof(dTO.UnitCode)) });
            }
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }

        [HttpGet("getServiceUnitMapById/{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var obj = await Mediator.Send(new GetServiceUnitMapById() { Id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("getServiceUnitMapByServiceAndUnitCode")]
        public async Task<IActionResult> Get(string UnitCode, string ServiceCode)
        {
            var obj = await Mediator.Send(new GetServiceUnitMapByServiceAndUnitCode() { UnitCode = UnitCode, ServiceCode = ServiceCode, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("getSelectServiceUnitMapList")]
        public async Task<IActionResult> GetSelectUnitList()
        {
            var item = await Mediator.Send(new GetSelectServiceUnitMapList() { User = UserInfo() });
            return Ok(item);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var ServiceUnitMap = await Mediator.Send(new DeleteServiceUnitMap() { Id = id, User = UserInfo() });
            if (ServiceUnitMap > 0)
                return NoContent();
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }


    
    }
}
