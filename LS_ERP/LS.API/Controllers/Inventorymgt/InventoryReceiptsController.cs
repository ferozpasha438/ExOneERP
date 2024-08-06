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
    public class InventoryReceiptsController : BaseController
    {

        public InventoryReceiptsController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }
        [HttpGet("GetReceiptsUserSelectList")]
        public async Task<IActionResult> GetReceiptsUserSelectList([FromQuery] string search)
        {
            var obj = await Mediator.Send(new GetReceiptsUserSelectList() { Input = search, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("GetReceiptsToLocationList")]
        public async Task<IActionResult> GetReceiptsToLocationList([FromQuery] string search)
        {
            var obj = await Mediator.Send(new GetReceiptsToLocationList() { Input = search, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("GetReceiptsAccountSelectList")]
        public async Task<IActionResult> GetReceiptsAccountSelectList([FromQuery] string search)
        {
            var obj = await Mediator.Send(new GetReceiptsAccountSelectList() { Input = search, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("GetReceiptsJVNumberSelectList")]
        public async Task<IActionResult> GetReceiptsJVNumberSelectList([FromQuery] string search)
        {
            var obj = await Mediator.Send(new GetReceiptsJVNumberSelectList() { Input = search, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("GetReceiptsBarCodeSelectList")]
        public async Task<IActionResult> GetReceiptsBarCodeSelectList([FromQuery] string search)
        {
            var obj = await Mediator.Send(new GetReceiptsBarCodeSelectList() { Input = search, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("GetReceiptsItemCodeList")]
        public async Task<IActionResult> GetReceiptsItemCodeList([FromQuery] string search)
        {
            var obj = await Mediator.Send(new GetReceiptsItemCodeList() { Input = search, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("ReceiptsProductUomtPriceItem/{ItemList}")]
        public async Task<IActionResult> ReceiptsProductUomtPriceItem([FromRoute] string ItemList)
        {
            var obj = await Mediator.Send(new ReceiptsProductUomtPriceItem() { ItemList = ItemList, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("GetReceiptsUOMSelectList")]
        public async Task<IActionResult> GetReceiptsUOMSelectList([FromQuery] string search)
        {
            var obj = await Mediator.Send(new GetReceiptsUOMSelectList() { Input = search, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpPost("ReceiptsCreateIssuesRequest")]
        public async Task<ActionResult> ReceiptsCreateIssuesRequest([FromBody] TblReceiptsInventoryReturntDto input)
        {
            var accBranch = await Mediator.Send(new ReceiptsCreateIssuesRequest() { Input = input, User = UserInfo() });
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
            var list = await Mediator.Send(new GetIMReceiptsTransactionList() { Input = filter.Values(), User = UserInfo() });
            return Ok(list);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var obj = await Mediator.Send(new GetReceiptsIMDetails() { id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var ClassId = await Mediator.Send(new ReceiptsDeleteIMList() { Id = id, User = UserInfo() });
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
        //[HttpGet("ReceiptsSettelementList")]
        //public async Task<IActionResult> ReceiptsSettelementList([FromRoute] int id)
        //{
        //    var obj = await Mediator.Send(new ReceiptsSettelementList() { id = id, User = UserInfo() });
        //    return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        //}
        [HttpGet("ReceiptsSettelementList/{id}")]
        public async Task<IActionResult> ReceiptsSettelementList([FromRoute] int id)
        {
            var obj = await Mediator.Send(new ReceiptsSettelementList() { id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("GetReceiptsBranchList")]
        public async Task<IActionResult> GetReceiptsBranchList([FromQuery] string search)
        {
            var obj = await Mediator.Send(new GetReceiptsBranchList() { Input = search, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }


    }
}
