using CIN.Application;
using CIN.Application.Common;
using CIN.Application.InventoryDtos;
using CIN.Application.InventoryQuery;
using CIN.Application.SystemQuery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace LS.API.Controllers.Inventory
{
    public class UnitofmeasureController : BaseController
    {
        public UnitofmeasureController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {

        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {
            var list = await Mediator.Send(new GetUnitList() { Input = filter.Values(), User = UserInfo() });
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var obj = await Mediator.Send(new GetUnit() { Id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TblInvDefUOMDto input)
        {
            //await Task.Delay(3000);
            var Unit = await Mediator.Send(new CreateUnit() { Input = input, User = UserInfo() });
            if (Unit.UnitId > 0)
            {
                if (input.Id > 0)
                    return NoContent();
                else
                    return Created($"get/{Unit.UnitId}", input);
            }
            return BadRequest(new ApiMessageDto { Message = Unit.Message });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var ClassId = await Mediator.Send(new DeleteUnit() { Id = id, User = UserInfo() });
            if (ClassId > 0)
                return NoContent();
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }
        #region UOMItem
        [HttpGet("GetUomItems")]
        public async Task<IActionResult> GetUomItems([FromQuery] string UOMCode)
        {
            var obj = await Mediator.Send(new GetUomItems() { UomCode = UOMCode, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        #endregion
    }
}
