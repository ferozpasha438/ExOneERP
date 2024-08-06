using CIN.Application;
using CIN.Application.Common;
using CIN.Application.FinanceMgtQuery;
using CIN.Application.InventoryQuery;
using CIN.Application.SystemQuery;
using CIN.Application.SystemSetupDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace LS.API.Sales.Controllers.SND
{
    public class WarehouseController : BaseController
    {
        public WarehouseController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {

        }

     
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var obj = await Mediator.Send(new GetWareHouse() { Id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }


        [HttpGet("getSelectWarehouseCodeList")]
        public async Task<IActionResult> GetSelectBranchCodeList()
        {
            var obj = await Mediator.Send(new GetSelectWarehouseList() { User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("getWarehouseInfoByCode/{warehouseCode}")]
        public async Task<IActionResult> GetWarehouseInfoByCode([FromRoute] string warehouseCode)
        {
            var obj = await Mediator.Send(new GetWarehouseInfoByCode() { User = UserInfo(),Input=warehouseCode });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }


    }
}
