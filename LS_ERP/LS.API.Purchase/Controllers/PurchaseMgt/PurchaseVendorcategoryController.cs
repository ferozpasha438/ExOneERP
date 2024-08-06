using CIN.Application;
using CIN.Application.Common;
using CIN.Application.PurchaseSetupDtos;
//using CIN.Application.InventoryDtos;
//using CIN.Application.InventoryQuery;
using CIN.Application.PurchaseSetupQuery;
using CIN.Application.SystemQuery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace LS.API.Purchase.Controllers
{
    public class PurchaseVendorcategoryController : BaseController
    {
        public PurchaseVendorcategoryController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {

        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {
            //await Task.Delay(3000);
            var list = await Mediator.Send(new GetVenCategoryList() { Input = filter.Values(), User = UserInfo() });
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var obj = await Mediator.Send(new GetVenCategory() { Id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
       

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TblPopDefVendorCategoryDto input)
        {
            var Categorye = await Mediator.Send(new CreateVenCategorye() { Input = input, User = UserInfo() });
            if (Categorye.Id > 0)
            {
                if (input.Id > 0)
                    return NoContent();
                else
                    return Created($"get/{Categorye.Id}", input);
            }
            return BadRequest(new ApiMessageDto { Message = Categorye.Message });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var BranchId = await Mediator.Send(new DeleteVenCategory() { Id = id, User = UserInfo() });
            if (BranchId > 0)
                return NoContent();
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }
       
        #region VenCategoryItem
        [HttpGet("GetVenCategoryItem")]
        public async Task<IActionResult> GetCategoryItem([FromQuery] string CatCode)
        {
            var obj = await Mediator.Send(new GetCategoryItem() { VenCatCode = CatCode, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        #endregion
       
    }
}
