using CIN.Application;
using CIN.Application.Common;
using CIN.Application.FinanceMgtDtos;
using CIN.Application.FinanceMgtQuery;
using CIN.Application.PurchaseSetupDtos;
using CIN.Application.PurchaseSetupQuery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace LS.API.Controllers.Purchasemgt
{
    public class PurchaseshipmentcodeController : BaseController
    {
        public PurchaseshipmentcodeController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {
            //await Task.Delay(3000);
            var list = await Mediator.Send(new GetPurchaseshipmentcodeList() { Input = filter.Values(), User = UserInfo() });
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var obj = await Mediator.Send(new GetPurchaseshipmentcode() { Id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TblPopDefVendorShipmentDto input)
        {
            //await Task.Delay(3000);

            var accBranch = await Mediator.Send(new CreatePurchaseshipmentcode() { Input = input, User = UserInfo() });
            if (accBranch.Id > 0)
            {
                return Created($"get/{accBranch.Id}", input);
            }
            return BadRequest(new ApiMessageDto { Message = accBranch.Message });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var BranchId = await Mediator.Send(new DeletePurchaseshipmentcode() { Id = id, User = UserInfo() });
            if (BranchId > 0)
                return NoContent();
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }

        #region ShipmentCode
        [HttpGet("GetShipmentCode")]
        public async Task<IActionResult> GetShipmentCode([FromQuery] string CatCode)
        {
            var obj = await Mediator.Send(new GetShipmentCode() { ShipmentCode = CatCode, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        #endregion

    }
}
