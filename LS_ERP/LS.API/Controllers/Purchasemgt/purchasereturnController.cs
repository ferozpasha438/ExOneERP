using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CIN.Application;
using CIN.Application.Common;
using CIN.Application.FinPurchaseMgtDto;
using CIN.Application.PurchasemgtQuery;
using CIN.Application.PurchaseSetupDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace LS.API.Controllers.Purchasemgt
{
    public class purchasereturnController : BaseController
    {
        public purchasereturnController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {
            //await Task.Delay(3000);
            var list = await Mediator.Send(new GetPurchaseReturnList() { Input = filter.Values(), User = UserInfo() });
            return Ok(list);
        }

        [HttpGet("GetTaxSelectList")]
        public async Task<IActionResult> GetTaxSelectList([FromQuery] string search)
        {
            var obj = await Mediator.Send(new GetPRTaxList() { Input = search, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("GetCurrencySelectList")]
        public async Task<IActionResult> GetCurrencySelectList([FromQuery] string search)
        {
            var obj = await Mediator.Send(new GetPRCurrencyList() { Input = search, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("GetShipmentSelectList")]
        public async Task<IActionResult> GetShipmentSelectList([FromQuery] string search)
        {
            var obj = await Mediator.Send(new GetPRShipmentList() { Input = search, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("GetCompSelectList")]
        public async Task<IActionResult> GetCompSelectList([FromQuery] string search)
        {
            var obj = await Mediator.Send(new GetPRCompanyList() { Input = search, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("GetBranchSelectList")]
        public async Task<IActionResult> GetBranchSelectList([FromQuery] string search)
        {
            var obj = await Mediator.Send(new GetPRBranchList() { Input = search, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("GetVendorCodeSelectList")]
        public async Task<IActionResult> GetVendorCodeSelectList([FromQuery] string search)
        {
            var obj = await Mediator.Send(new GetPRVendorCodeList() { Input = search, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("GetVendorNameSelectList")]
        public async Task<IActionResult> GetVendorNameSelectList([FromQuery] string search)
        {
            var obj = await Mediator.Send(new GetPRVendorNameList() { Input = search, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("GetPaymentTermSelectList")]
        public async Task<IActionResult> GetPaymentTermSelectList([FromQuery] string search)
        {
            var obj = await Mediator.Send(new GetPRPaymentTermList() { Input = search, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }


        [HttpGet("GetItemCodeSelectList")]
        public async Task<IActionResult> GetItemCodeSelectList([FromQuery] string search)
        {
            var obj = await Mediator.Send(new GetPRItemCodeList() { Input = search, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("GetItemNameSelectList")]
        public async Task<IActionResult> GetItemNameSelectList([FromQuery] string search)
        {
            var obj = await Mediator.Send(new GetPRItemNameList() { Input = search, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("GetUOMSelectList")]
        public async Task<IActionResult> GetUOMSelectList([FromQuery] string search)
        {
            var obj = await Mediator.Send(new GetPRUOMSelectList() { Input = search, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("GetPrSelectList")]
        public async Task<IActionResult> GetPrSelectList([FromQuery] string search)
        {
            var obj = await Mediator.Send(new GetPurReturnList() { Input = search, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }


        [HttpGet("productUnitPriceItem/{Itemcode}")]
        public async Task<IActionResult> ProductUnitPriceItem([FromRoute] string Itemcode)
        {
            var obj = await Mediator.Send(new PRProductUnitPriceItem() { Itemcode = Itemcode, User = UserInfo() });
            return Ok(obj);
        }

        [HttpGet("ProductUomtPriceItem/{ItemList}")]
        public async Task<IActionResult> ProductUomtPriceItem([FromRoute] string ItemList)
        {
            var obj = await Mediator.Send(new PRProductUomtPriceItem() { ItemList = ItemList, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpPost("CreatePurchaserequest")]
        public async Task<ActionResult> CreatePurchaserequest([FromBody] TblPurchaseReturntDto input)
        {
            var accBranch = await Mediator.Send(new CreatePurchaseReturn() { Input = input, User = UserInfo() });
            if (accBranch.Id > 0)
            {
                return Created($"get/{accBranch.Id}", input);
            }
            return BadRequest(new ApiMessageDto { Message = accBranch.Message });
        }
        [HttpGet("GetPRList/{PRValue}")]
        public async Task<IActionResult> GetPRList([FromRoute] string PRValue)
        {
            var obj = await Mediator.Send(new GetPReturnDetails() { TranNumber = PRValue, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var obj = await Mediator.Send(new GetPRsDetails() { id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var ClassId = await Mediator.Send(new DeletePRList() { Id = id, User = UserInfo() });
            if (ClassId > 0)
                return NoContent();
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }

        [HttpGet("AccountsPosting/{id}")]
        public async Task<IActionResult> AccountsPosting([FromRoute] int id)
        {
            var obj = await Mediator.Send(new PRTAccountsPosting() { id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("getGRNReturnDetails/{PONO}")]
        public async Task<IActionResult> GetGRNReturnDetails([FromRoute] string PONO)
        {
            var obj = await Mediator.Send(new GetGRNReturnDetails() { PONO = PONO, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("getItemsForWarehouseSelectList")]
        public async Task<IActionResult> GetItemsForWarehouseSelectList([FromQuery] string whCode, [FromQuery] string itemCode, [FromQuery] decimal itemCount)
        {
            var obj = await Mediator.Send(new GetItemsForWarehouseSelectList() { WhCode = whCode, ItemCode = itemCode, ItemCount = itemCount, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

    }
}
