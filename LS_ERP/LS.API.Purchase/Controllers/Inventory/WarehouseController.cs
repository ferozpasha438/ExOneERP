using CIN.Application;
using CIN.Application.Common;
using CIN.Application.InventoryDtos;
using CIN.Application.InventoryQuery;
using CIN.Application.SystemQuery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace LS.API.Purchase.Controllers
{
    public class WarehouseController : BaseController
    {
        public WarehouseController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }


        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {
            //await Task.Delay(3000);
            var list = await Mediator.Send(new GetWarehouseList() { Input = filter.Values(), User = UserInfo() });
            return Ok(list);
        }

        [HttpGet("getSelectWarehouseList")]
        public async Task<IActionResult> GetSelectWarehouseList()
        {
            var obj = await Mediator.Send(new GetSelectWarehouseList() { User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("getSelectSysBranchList")]
        public async Task<IActionResult> getSelectSysBranchList([FromQuery] string search)
        {
            var obj = await Mediator.Send(new GetSelectSysBranch() { Input = search, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("getSelectDistributionGroupList")]
        public async Task<IActionResult> getSelectDistributionGroupList([FromQuery] string search)
        {
            var obj = await Mediator.Send(new getSelectDistributionGroupList() { Input = search, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TblInvDefWarehouseDto input)
        {
            //await Task.Delay(3000);

            var branch = await Mediator.Send(new CreateWarehouse() { Input = input, User = UserInfo() });
            if (branch.Id > 0)
            {
                if (input.Id > 0)
                    return NoContent();
                else
                    return Created($"get/{branch.Id}", input);
            }
            return BadRequest(new ApiMessageDto { Message = branch.Message });
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var obj = await Mediator.Send(new GetWareHouse() { Id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var ClassId = await Mediator.Send(new DeleteWareHouse() { Id = id, User = UserInfo() });
            if (ClassId > 0)
                return NoContent();
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }
        #region WarehouseItem
        [HttpGet("GetWarehouseItems")]
        public async Task<IActionResult> GetInventoryItems([FromQuery] string whcode)
        {
            var obj = await Mediator.Send(new GetWarehouseItem() { WHCODE = whcode, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        #endregion 
        [HttpGet("GetSelectSysTypeList")]
        public async Task<IActionResult> GetSelectSysTracking([FromQuery] string search)
        {
            var obj = await Mediator.Send(new GetSelectSysTypeList() { Input = search, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("GetSelectTrackingList")]
        public async Task<IActionResult> GetSelectTrackingList([FromQuery] string Code)
        {
            var obj = await Mediator.Send(new GetSelectTypeList() { Code = Code, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("GetWarehouseDetails")]
        public async Task<IActionResult> GetBarcode([FromQuery] string Warehouse)
        {
            var obj = await Mediator.Send(new GetWarehouseDetials() { Code = Warehouse, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
    }
}
