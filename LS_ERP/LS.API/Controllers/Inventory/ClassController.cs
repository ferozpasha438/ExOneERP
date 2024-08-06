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
    public class ClassController : BaseController
    {
        public ClassController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {

        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {
            //await Task.Delay(3000);
            var list = await Mediator.Send(new GetClassList() { Input = filter.Values(), User = UserInfo() });
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var obj = await Mediator.Send(new GetClass() { Id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TblInvDefClassDto input)
        {
            //await Task.Delay(3000);

            var Class = await Mediator.Send(new Createclass() { Input = input, User = UserInfo() });
            if (Class.ClassId > 0)
            {
                if (input.Id > 0)
                    return NoContent();
                else
                    return Created($"get/{Class.ClassId}", input);
            }
            return BadRequest(new ApiMessageDto { Message = Class.Message });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var ClassId = await Mediator.Send(new DeleteClass() { Id = id, User = UserInfo() });
            if (ClassId > 0)
                return NoContent();
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }
        #region ClassItems
        [HttpGet("GetClassItems")]
        public async Task<IActionResult> GetClassItems([FromQuery] string ClassCode)
        {
            var obj = await Mediator.Send(new GetClassItem() { ClassCode = ClassCode, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        #endregion
    }
}
