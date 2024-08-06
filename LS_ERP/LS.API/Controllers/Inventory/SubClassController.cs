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
    public class SubClassController : BaseController
    {
        public SubClassController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {

        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {
            //await Task.Delay(3000);
            var list = await Mediator.Send(new GetSubClassList() { Input = filter.Values(), User = UserInfo() });
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var obj = await Mediator.Send(new GetSubClass() { Id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TblInvDefSubClassDto input)
        {
            //await Task.Delay(3000);

            var SubClass = await Mediator.Send(new CreateSubclass() { Input = input, User = UserInfo() });
            if (SubClass.SubClassId > 0)
            {
                if (input.Id > 0)
                    return NoContent();
                else
                    return Created($"get/{SubClass.SubClassId}", input);
            }
            return BadRequest(new ApiMessageDto { Message = SubClass.Message });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var ClassId = await Mediator.Send(new DeleteSubClass() { Id = id, User = UserInfo() });
            if (ClassId > 0)
                return NoContent();
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }
        #region SubClassItem
        [HttpGet("GetSubClassItems")]
        public async Task<IActionResult> GetSubClassItems([FromQuery] string SubClassCode)
        {
            var obj = await Mediator.Send(new GetSubClassItem() { SubClassCode = SubClassCode, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        #endregion
    }
}
