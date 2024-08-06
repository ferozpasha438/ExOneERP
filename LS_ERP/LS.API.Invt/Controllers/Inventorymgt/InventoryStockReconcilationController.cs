using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CIN.Application;
using CIN.Application.Common;
using CIN.Application.InventoryMgtDtos;
using CIN.Application.InventorymgtQuery;
using Microsoft.Extensions.Options;

namespace LS.API.Invt.Controllers.Inventorymgt
{
    public class InventoryStockReconcilationController : BaseController
    {

        public InventoryStockReconcilationController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }
        [HttpGet("GetStockReconcilationUserSelectList")]
        public async Task<IActionResult> GetStockReconcilationUserSelectList([FromQuery] string search)
        {
            var obj = await Mediator.Send(new GetStockReconcilationUserSelectList() { Input = search, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("GetStockReconcilationToLocationList")]
        public async Task<IActionResult> GetStockReconcilationToLocationList([FromQuery] string search)
        {
            var obj = await Mediator.Send(new GetStockReconcilationToLocationList() { Input = search, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("GetStockReconcilationAccountSelectList")]
        public async Task<IActionResult> GetStockReconcilationAccountSelectList([FromQuery] string search)
        {
            var obj = await Mediator.Send(new GetStockReconcilationAccountSelectList() { Input = search, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("GetStockReconcilationJVNumberSelectList")]
        public async Task<IActionResult> GetStockReconcilationJVNumberSelectList([FromQuery] string search)
        {
            var obj = await Mediator.Send(new GetStockReconcilationJVNumberSelectList() { Input = search, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("GetStockReconcilationBarCodeSelectList")]
        public async Task<IActionResult> GetStockReconcilationBarCodeSelectList([FromQuery] string search)
        {
            var obj = await Mediator.Send(new GetStockReconcilationBarCodeSelectList() { Input = search, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("GetStockReconcilationItemCodeList")]
        public async Task<IActionResult> GetStockReconcilationItemCodeList([FromQuery] string search)
        {
            var obj = await Mediator.Send(new GetStockReconcilationItemCodeList() { Input = search, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("StockReconcilationProductUomtPriceItem/{ItemList}")]
        public async Task<IActionResult> StockReconcilationProductUomtPriceItem([FromRoute] string ItemList)
        {
            var obj = await Mediator.Send(new StockReconcilationProductUomtPriceItem() { ItemList = ItemList, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("GetStockReconcilationUOMSelectList")]
        public async Task<IActionResult> GetStockReconcilationUOMSelectList([FromQuery] string search)
        {
            var obj = await Mediator.Send(new GetStockReconcilationUOMSelectList() { Input = search, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpPost("StockReconcilationCreateRequest")]
        public async Task<ActionResult> StockReconcilationCreateRequest([FromBody] TblStockReconcilationInventoryReturntDto input)
        {
            var accBranch = await Mediator.Send(new StockReconcilationCreateRequest() { Input = input, User = UserInfo() });
            if (accBranch.Id > 0)
            {
                return Created($"get/{accBranch.Id}", input);
            }
            return BadRequest(new ApiMessageDto { Message = accBranch.Message });
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {
            //await Task.Delay(3000);
            var list = await Mediator.Send(new GetIMStockReconcilationTransactionList() { Input = filter.Values(), User = UserInfo() });
            return Ok(list);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var obj = await Mediator.Send(new GetStockReconcilationIMDetails() { id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var ClassId = await Mediator.Send(new StockReconcilationDeleteIMList() { Id = id, User = UserInfo() });
            if (ClassId > 0)
                return NoContent();
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }

        //[HttpPost("CreateIssuesApproval")]
        //public async Task<ActionResult> CreateIssuesApproval([FromBody] TblTranInvoiceApprovalDto input)
        //{
        //    var result = await Mediator.Send(new CreateIssuesApproval() { Input = input, User = UserInfo() });
        //    return result ? Ok(result) : BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        //}

    }
}
