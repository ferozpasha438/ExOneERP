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
    public class MaterialequipmentController : BaseController
    {

        public MaterialequipmentController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }

        [HttpGet("getMaterialequipmentsPagedList")]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {

            var list = await Mediator.Send(new GetMaterialequipmentsPagedList() { Input = filter.Values(), User = UserInfo() });
            return Ok(list);
        }
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TblOpMaterialEquipmentDto dTO)
        {
            var id = await Mediator.Send(new CreateMaterialequipment() { MaterialequipmentDto = dTO, User = UserInfo() });
            if (id > 0)
            {
                return Created($"get/{id}", dTO);
            }
            else if (id == -1)
            {
                return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Duplicate(nameof(dTO.Code)) });
            }
           return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });

        }

        [HttpGet("getMaterialequipmentById/{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var obj = await Mediator.Send(new GetMaterialequipmentById() { Id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("getMaterialequipmentByCode/{MaterialequipmentCode}")]
        public async Task<IActionResult> Get([FromRoute] string MaterialequipmentCode)
        {
            var obj = await Mediator.Send(new GetMaterialequipmentByCode() { MaterialequipmentCode = MaterialequipmentCode, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("getSelectMaterialequipmentList")]
        public async Task<IActionResult> GetSelectMaterialequipmentList()
        {
            var item = await Mediator.Send(new GetSelectMaterialequipmentList() { User = UserInfo() });
            return Ok(item);
        }

        [HttpGet("getAutoSelectMaterialEquipmentList")]
        public async Task<IActionResult> GetAutoSelectMaterialEquipmentList(string search)
        {
            var items = await Mediator.Send(new GetAutoSelectMaterialEquipmentList() { SearchKey = search, User = UserInfo() });
            return Ok(items);
        }

        [HttpGet("getMaterialequipmentCodes")]
        public async Task<IActionResult> GetMaterialequipmentCodes()
        {
            var item = await Mediator.Send(new GetMaterialequipmentCodes() { User = UserInfo() });
            return Ok(item);
        }
        [HttpGet("isExistCode/{Code}")]
        public async Task<bool> IsExistCode([FromRoute] string Code)
        {
            var obj = await Mediator.Send(new GetMaterialequipmentByCode() { MaterialequipmentCode = Code, User = UserInfo() });
            return obj is not null ? true : false;
        }




        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var MaterialequipmentId = await Mediator.Send(new DeleteMaterialequipment() { Id = id, User = UserInfo() });
            if (MaterialequipmentId > 0)
                return NoContent();
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }
    }
}
