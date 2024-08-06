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
    public class InventoryAdjustmentsController : BaseController
    {
        public InventoryAdjustmentsController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }
        [HttpGet("GetAdjustmentsUserSelectList")]
        public async Task<IActionResult> GetAdjustmentsUserSelectList([FromQuery] string search)
        {
            var obj = await Mediator.Send(new GetAdjustmentsUserSelectList() { Input = search, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("GetAdjustmentToLocationList")]
        public async Task<IActionResult> GetAdjustmentToLocationList([FromQuery] string search)
        {
            var obj = await Mediator.Send(new GetAdjustmentToLocationList() { Input = search, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("GetAdjustmentAccountSelectList")]
        public async Task<IActionResult> GetAdjustmentAccountSelectList([FromQuery] string search)
        {
            var obj = await Mediator.Send(new GetAdjustmentAccountSelectList() { Input = search, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("GetAdjustmentJVNumberSelectList")]
        public async Task<IActionResult> GetAdjustmentJVNumberSelectList([FromQuery] string search)
        {
            var obj = await Mediator.Send(new GetAdjustmentJVNumberSelectList() { Input = search, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("GetAdjustmentBarCodeSelectList")]
        public async Task<IActionResult> GetAdjustmentBarCodeSelectList([FromQuery] string search)
        {
            var obj = await Mediator.Send(new GetAdjustmentBarCodeSelectList() { Input = search, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("GetAdjustmentItemCodeList")]
        public async Task<IActionResult> GetAdjustmentItemCodeList([FromQuery] string search)
        {
            var obj = await Mediator.Send(new GetAdjustmentItemCodeList() { Input = search, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("AdjustmentProductUomtPriceItem/{ItemList}")]
        public async Task<IActionResult> AdjustmentProductUomtPriceItem([FromRoute] string ItemList)
        {
            var obj = await Mediator.Send(new AdjustmentProductUomtPriceItem() { ItemList = ItemList, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("GetAdjustmentUOMSelectList")]
        public async Task<IActionResult> GetAdjustmentUOMSelectList([FromQuery] string search)
        {
            var obj = await Mediator.Send(new GetAdjustmentUOMSelectList() { Input = search, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpPost("AdjustmentCreateIssuesRequest")]
        public async Task<ActionResult> AdjustmentCreateIssuesRequest([FromBody] TblAdjustmentsInventoryReturntDto input)
        {
            var accBranch = await Mediator.Send(new AdjustmentCreateIssuesRequest() { Input = input, User = UserInfo() });
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
            var list = await Mediator.Send(new GetIMAdjustmentsTransactionList() { Input = filter.Values(), User = UserInfo() });
            return Ok(list);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var obj = await Mediator.Send(new GetAdjustmentIMDetails() { id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var ClassId = await Mediator.Send(new AdjustmentDeleteIMList() { Id = id, User = UserInfo() });
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
        [HttpGet("AdjustmentSettelementList/{id}")]
        public async Task<IActionResult> AdjustmentSettelementList([FromRoute] int id)
        {
            var obj = await Mediator.Send(new AdjustmentSettelementList() { id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

    }
}
