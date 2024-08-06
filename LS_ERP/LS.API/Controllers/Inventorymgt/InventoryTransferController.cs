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

namespace LS.API.Controllers.Inventorymgt
{
    public class InventoryTransferController : BaseController
    {

        public InventoryTransferController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }
        [HttpGet("GetTransferUserSelectList")]
        public async Task<IActionResult> GetTransferUserSelectList([FromQuery] string search)
        {
            var obj = await Mediator.Send(new GetTransferUserSelectList() { Input = search, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("GetTransferToLocationList")]
        public async Task<IActionResult> GetTransferToLocationList([FromQuery] string search)
        {
            var obj = await Mediator.Send(new GetTransferToLocationList() { Input = search, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("GetTransferAccountSelectList")]
        public async Task<IActionResult> GetTransferAccountSelectList([FromQuery] string search)
        {
            var obj = await Mediator.Send(new GetTransferAccountSelectList() { Input = search, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("GetTransferJVNumberSelectList")]
        public async Task<IActionResult> GetTransferJVNumberSelectList([FromQuery] string search)
        {
            var obj = await Mediator.Send(new GetTransferJVNumberSelectList() { Input = search, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("GetTransferBarCodeSelectList")]
        public async Task<IActionResult> GetTransferBarCodeSelectList([FromQuery] string search)
        {
            var obj = await Mediator.Send(new GetTransferBarCodeSelectList() { Input = search, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("GetTransferItemCodeList")]
        public async Task<IActionResult> GetTransferItemCodeList([FromQuery] string search)
        {
            var obj = await Mediator.Send(new GetTransferItemCodeList() { Input = search, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("TransferProductUomtPriceItem/{ItemList}")]
        public async Task<IActionResult> TransferProductUomtPriceItem([FromRoute] string ItemList)
        {
            var obj = await Mediator.Send(new TransferProductUomtPriceItem() { ItemList = ItemList, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("GetTransferUOMSelectList")]
        public async Task<IActionResult> GetTransferUOMSelectList([FromQuery] string search)
        {
            var obj = await Mediator.Send(new GetTransferUOMSelectList() { Input = search, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpPost("TransferCreateIssuesRequest")]
        public async Task<ActionResult> TransferCreateIssuesRequest([FromBody] TblTransferInventoryReturntDto input)
        {
            var accBranch = await Mediator.Send(new TransferCreateIssuesRequest() { Input = input, User = UserInfo() });
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
            var list = await Mediator.Send(new GetIMTransferTransactionList() { Input = filter.Values(), User = UserInfo() });
            return Ok(list);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var obj = await Mediator.Send(new GetTransferIMDetails() { id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var ClassId = await Mediator.Send(new TransferDeleteIMList() { Id = id, User = UserInfo() });
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
        [HttpGet("StockTransfer/{id}")]
        public async Task<IActionResult> StockTransfer([FromRoute] int id)
        {
            var obj = await Mediator.Send(new stockTransfer() { id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

    }
}
