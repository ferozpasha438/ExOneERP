using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CIN.Application;
using CIN.Application.Common;
using CIN.Application.InventoryMgtDtos;
using CIN.Application.InventorymgtQuery;
//using CIN.Application.PurchaseMgtDtos;
//using CIN.Application.PurchasemgtQuery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace LS.API.Invt.Controllers.Inventorymgt
{
    public class InventoryTransactionController : BaseController
    {
        public InventoryTransactionController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }
        [HttpGet("GetUserSelectList")]
        public async Task<IActionResult> GetUserSelectList([FromQuery] string search)
        {
            var obj = await Mediator.Send(new GetUserSelectList() { Input = search, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("GetToLocationList")]
        public async Task<IActionResult> GetToLocationList([FromQuery] string search)
        {
            var obj = await Mediator.Send(new GetToLocationList() { Input = search, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("GetAccountSelectList")]
        public async Task<IActionResult> GetAccountSelectList([FromQuery] string search)
        {
            var obj = await Mediator.Send(new GetAccountSelectList() { Input = search, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("GetJVNumberSelectList")]
        public async Task<IActionResult> GetJVNumberSelectList([FromQuery] string search)
        {
            var obj = await Mediator.Send(new GetJVNumberSelectList() { Input = search, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("GetBarCodeSelectList")]
        public async Task<IActionResult> GetBarCodeSelectList([FromQuery] string search)
        {
            var obj = await Mediator.Send(new GetBarCodeSelectList() { Input = search, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("GetItemCodeList")]
        public async Task<IActionResult> GetItemCodeList([FromQuery] string search)
        {
            var obj = await Mediator.Send(new GetItemCodeList() { Input = search, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("ProductUomtPriceItem/{ItemList}")]
        public async Task<IActionResult> ProductUomtPriceItem([FromRoute] string ItemList)
        {
            var obj = await Mediator.Send(new ProductUomtPriceItem() { ItemList = ItemList, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("GetUOMSelectList")]
        public async Task<IActionResult> GetUOMSelectList([FromQuery] string search)
        {
            var obj = await Mediator.Send(new GetUOMSelectList() { Input = search, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpPost("CreateIssuesRequest")]
        public async Task<ActionResult> CreateIssuesRequest([FromBody] TblInventoryReturntDto input)
        {
            var accBranch = await Mediator.Send(new CreateIssuesRequest() { Input = input, User = UserInfo() });
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
            var list = await Mediator.Send(new GetIMTransactionList() { Input = filter.Values(), User = UserInfo() });
            return Ok(list);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var obj = await Mediator.Send(new GetIMDetails() { id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var ClassId = await Mediator.Send(new DeleteIMList() { Id = id, User = UserInfo() });
            if (ClassId > 0)
                return NoContent();
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }

        //[HttpPost("CreateIssuesApproval")]
        //public async Task<ActionResult> CreateIssuesApproval([FromBody] TblTranInvoiceApprovalDto input)
        //{
        //    var result = await Mediator.Send(new CreatePurApprovals() { Input = input, User = UserInfo() });
        //    return result ? Ok(result) : BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        //}

        //[HttpPost("InvApprovals")]
        //public async Task<ActionResult> Create([FromBody] TblPurTrnApprovalsDto dTO)
        //{
        //    var id = await Mediator.Send(new CreatePurApprovals() { Input = dTO, User = UserInfo() });
        //    if (id > 0)
        //        return Created($"get/{id}", dTO);
        //    else if (id == -1)
        //    {
        //        return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Duplicate(nameof(dTO.AppAuth)) });
        //    }
        //    return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        //}
        [HttpGet("IssuesSettelementList/{id}")]
        public async Task<IActionResult> IssuesSettelementList([FromRoute] int id)
        {
            var obj = await Mediator.Send(new IssuesSettelementList() { id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
    }
}
