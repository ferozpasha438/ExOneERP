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
    public class SubCategoryController : BaseController
    {
        public SubCategoryController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {

        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {
            //await Task.Delay(3000);
            var list = await Mediator.Send(new GetSubCategoryList() { Input = filter.Values(), User = UserInfo() });
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var obj = await Mediator.Send(new GetSubCategory() { Id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        //[HttpGet("getSelectSysBranchList")]
        //public async Task<IActionResult> GetSelectSysBranchList([FromQuery] string search)
        //{
        //    var obj = await Mediator.Send(new GetSelectSysBranchList() { Input = search, User = UserInfo() });
        //    return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        //}
        //[HttpGet("getBranchByBranchCode")]
        //public async Task<IActionResult> GetBranchByBranchCode([FromQuery] string branchCode)
        //{
        //    var obj = await Mediator.Send(new GetBranchByBranchCode() { Input = branchCode, User = UserInfo() });
        //    return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        //}

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TblInvDefSubCategoryDto input)
        {
            //await Task.Delay(3000);

            var category = await Mediator.Send(new CreateSubCategory() { Input = input, User = UserInfo() });
            if (category.SubCategoryId > 0)
            {
                if (input.Id > 0)
                    return NoContent();
                else
                    return Created($"get/{category.SubCategoryId}", input);
            }
            return BadRequest(new ApiMessageDto { Message = category.Message });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var BranchId = await Mediator.Send(new DeleteSubCategory() { Id = id, User = UserInfo() });
            if (BranchId > 0)
                return NoContent();
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }
        #region SubCategory
        [HttpGet("GetSubCategory")]
        public async Task<IActionResult> GetSubCategory([FromQuery] string Subcode)
        {
            var obj = await Mediator.Send(new GetSubCatCode() { SubCatCode = Subcode, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        #endregion
    }
}
