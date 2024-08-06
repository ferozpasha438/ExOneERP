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
    public class LogisticsandvehicleController : BaseController
    {

        public LogisticsandvehicleController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }

        [HttpGet("getLogisticsandvehiclesPagedList")]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {

            var list = await Mediator.Send(new GetLogisticsandvehiclesPagedList() { Input = filter.Values(), User = UserInfo() });
            return Ok(list);
        }
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TblOpLogisticsandvehicleDto dTO)
        {
            var id = await Mediator.Send(new CreateLogisticsandvehicle() { LogisticsandvehicleDto = dTO, User = UserInfo() });
            if (id > 0)
            {
                return Created($"get/{id}", dTO);
            }
            else if (id == -1)
            {
                return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Duplicate(nameof(dTO.VehicleNumber)) });
            }
           return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });

        }

        [HttpGet("getLogisticsandvehicleById/{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var obj = await Mediator.Send(new GetLogisticsandvehicleById() { Id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("getLogisticsandvehicleByCode/{LogisticsandvehicleCode}")]
        public async Task<IActionResult> Get([FromRoute] string LogisticsandvehicleCode)
        {
            var obj = await Mediator.Send(new GetLogisticsandvehicleByCode() { LogisticsandvehicleCode = LogisticsandvehicleCode, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("getSelectLogisticsandvehicleList")]
        public async Task<IActionResult> GetSelectLogisticsandvehicleList()
        {
            var item = await Mediator.Send(new GetSelectLogisticsandvehicleList() { User = UserInfo() });
            return Ok(item);
        }

        [HttpGet("getAutoSelectLogisticsandvehicleList")]
        public async Task<IActionResult> GetAutoSelectLogisticsandvehicleList(string search)
        {
            var items = await Mediator.Send(new GetAutoSelectLogisticsandvehicleList() { SearchKey=search, User = UserInfo() });
            return Ok(items);
        }

        [HttpGet("getLogisticsandvehicleCodes")]
        public async Task<IActionResult> GetLogisticsandvehicleCodes()
        {
            var item = await Mediator.Send(new GetLogisticsandvehicleCodes() { User = UserInfo() });
            return Ok(item);
        }
        [HttpGet("isExistCode/{Code}")]
        public async Task<bool> IsExistCode([FromRoute] string Code)
        {
            var obj = await Mediator.Send(new GetLogisticsandvehicleByCode() { LogisticsandvehicleCode = Code, User = UserInfo() });
            return obj is not null ? true : false;
        }




        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var LogisticsandvehicleId = await Mediator.Send(new DeleteLogisticsandvehicle() { Id = id, User = UserInfo() });
            if (LogisticsandvehicleId > 0)
                return NoContent();
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }
    }
}
