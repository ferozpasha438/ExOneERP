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
    public class producthierarchyController : BaseController
    {
        public producthierarchyController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {

        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {
            //await Task.Delay(3000);
            var list = await Mediator.Send(new GetCategoryList() { Input = filter.Values(), User = UserInfo() });
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var obj = await Mediator.Send(new GetCategory() { Id = id, User = UserInfo() });
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
        public async Task<ActionResult> Create([FromBody] tblInvDefCategoryDto input)
        {
            var Categorye = await Mediator.Send(new CreateCategorye() { Input = input, User = UserInfo() });
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
            var BranchId = await Mediator.Send(new DeleteCategory() { Id = id, User = UserInfo() });
            if (BranchId > 0)
                return NoContent();
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }
        [HttpGet("GetCategorySelectList")]
        public async Task<IActionResult> GetCategorySelectList()
        {
            var list = await Mediator.Send(new GetCategorySelectList() { User = UserInfo() });
            return Ok(list);
        }
        #region CategoryItem
        [HttpGet("GetCategoryItem")]
        public async Task<IActionResult> GetCategoryItem([FromQuery] string CatCode)
        {
            var obj = await Mediator.Send(new GetCategoryItem() { ItemCatCode = CatCode, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        #endregion
        [HttpGet("getCategoryTypeList")]
        public async Task<IActionResult> GetCategoryTypeList()
        {
            var item = await Mediator.Send(new GetCategoryTypeList() { User = UserInfo() });
            return Ok(item);
        }
    }
}
