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
    public class UnitsController : BaseController
    {

        public UnitsController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }

        [HttpGet("getUnitsPagedList")]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {

            var list = await Mediator.Send(new GetUnitsPagedList() { Input = filter.Values(), User = UserInfo() });
            return Ok(list);
        }
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TblSndDefUnitMasterDto dTO)
        {
            var id = await Mediator.Send(new CreateUnit() { UnitDto = dTO, User = UserInfo() });
            if (id > 0)
                return Created($"get/{id}", dTO);
            else if (id == -1)
            {
                return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Duplicate(nameof(dTO.UnitCode)) });
            }
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }

        [HttpGet("getUnitById/{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var obj = await Mediator.Send(new GetUnitById() { Id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("getUnitByUnitCode/{UnitCode}")]
        public async Task<IActionResult> Get([FromRoute] string UnitCode)
        {
            var obj = await Mediator.Send(new GetUnitByUnitCode() { UnitCode = UnitCode, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("getSelectUnitList")]
        public async Task<IActionResult> GetSelectUnitList()
        {
            var item = await Mediator.Send(new GetSelectUnitList() {User = UserInfo() });
            return Ok(item);
        }

        [HttpGet("getSelectUnitListByServiceCode/{serviceCode}")]
        public async Task<IActionResult> getSelectUnitListByServiceCode([FromRoute]  string serviceCode)
        {
            var item = await Mediator.Send(new GetSelectUnitListByServiceCode() { User = UserInfo(),ServiceCode= serviceCode });
            return Ok(item);
        }

        [HttpGet("GetAutoFillUnitList")]
        public async Task<IActionResult> GetAutoFillUnitList(string search)
        {
            var item = await Mediator.Send(new GetAutoFillUnitList() { Input = search, User = UserInfo() });
            return Ok(item);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var UnitId = await Mediator.Send(new DeleteUnit() { Id = id, User = UserInfo() });
            if (UnitId > 0)
                return NoContent();
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }
    }
}
