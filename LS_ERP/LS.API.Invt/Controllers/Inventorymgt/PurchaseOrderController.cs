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

namespace LS.API.Invt.Controllers.Purchasemgt
{
    public class PurchaseOrderController : BaseController
    {
        public PurchaseOrderController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {
            //await Task.Delay(3000);
            var list = await Mediator.Send(new GetPurchaseRequestList() { Input = filter.Values(), User = UserInfo() });
            return Ok(list);
        }
        [HttpGet("GetPagedPurchaseOrderList")]
        public async Task<IActionResult> GetPagedPurchaseOrderList([FromQuery] PaginationFilterDto filter)
        {
            //await Task.Delay(3000);
            var list = await Mediator.Send(new GetPagedPurchaseOrderList() { Input = filter.Values(), User = UserInfo() });
            return Ok(list);
        }
        [HttpGet("GetTaxSelectList")]
        public async Task<IActionResult> GetTaxSelectList([FromQuery] string search)
        {
            var obj = await Mediator.Send(new GetTaxList() { Input = search, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("GetCurrencySelectList")]
        public async Task<IActionResult> GetCurrencySelectList([FromQuery] string search)
        {
            var obj = await Mediator.Send(new GetCurrencyList() { Input = search, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("GetShipmentSelectList")]
        public async Task<IActionResult> GetShipmentSelectList([FromQuery] string search)
        {
            var obj = await Mediator.Send(new GetShipmentList() { Input = search, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("GetCompSelectList")]
        public async Task<IActionResult> GetCompSelectList([FromQuery] string search)
        {
            var obj = await Mediator.Send(new GetCompanyList() { Input = search, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("GetBranchSelectList")]
        public async Task<IActionResult> GetBranchSelectList([FromQuery] string search)
        {
            var obj = await Mediator.Send(new GetBranchList() { Input = search, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("GetVendorCodeSelectList")]
        public async Task<IActionResult> GetVendorCodeSelectList([FromQuery] string search)
        {
            var obj = await Mediator.Send(new GetVendorCodeList() { Input = search, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("GetVendorNameSelectList")]
        public async Task<IActionResult> GetVendorNameSelectList([FromQuery] string search)
        {
            var obj = await Mediator.Send(new GetVendorNameList() { Input = search, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("GetPaymentTermSelectList")]
        public async Task<IActionResult> GetPaymentTermSelectList([FromQuery] string search)
        {
            var obj = await Mediator.Send(new GetPaymentTermList() { Input = search, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }


        [HttpGet("GetItemCodeSelectList")]
        public async Task<IActionResult> GetItemCodeSelectList([FromQuery] string search)
        {
            var obj = await Mediator.Send(new GetItemCodeList() { Input = search, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("GetItemNameSelectList")]
        public async Task<IActionResult> GetItemNameSelectList([FromQuery] string search)
        {
            var obj = await Mediator.Send(new GetItemNameList() { Input = search, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("GetUOMSelectList")]
        public async Task<IActionResult> GetUOMSelectList([FromQuery] string search)
        {
            var obj = await Mediator.Send(new GetUOMSelectList() { Input = search, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("GetPrSelectList")]
        public async Task<IActionResult> GetPrSelectList([FromQuery] string search)
        {
            var obj = await Mediator.Send(new GetPurRequestList() { Input = search, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        //[HttpGet("getSelectSubClassList")]
        //public async Task<IActionResult> getSelectSubClassList([FromQuery] string search)
        //{
        //    var obj = await Mediator.Send(new getSelectSubClassList() { Input = search, User = UserInfo() });
        //    return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        //}

        [HttpGet("productUnitPriceItem/{Itemcode}")]
        public async Task<IActionResult> ProductUnitPriceItem([FromRoute] string Itemcode)
        {
            var obj = await Mediator.Send(new ProductUnitPriceItem() { Itemcode = Itemcode, User = UserInfo() });
            return Ok(obj);
        }

        [HttpGet("ProductUomtPriceItem/{ItemList}")]
        public async Task<IActionResult> ProductUomtPriceItem([FromRoute] string ItemList)
        {
            var obj = await Mediator.Send(new ProductUomtPriceItem() { ItemList = ItemList, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("ProductVenPriceItem/{Vencode}")]
        public async Task<IActionResult> ProductVenPriceItem([FromRoute] string Vencode)
        {
            var obj = await Mediator.Send(new ProductVenPriceItem() { Vencode = Vencode, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("ProductTaxPrice/{ItemCode}")]
        public async Task<IActionResult> ProductTaxPrice([FromRoute] string ItemCode)
        {
            var obj = await Mediator.Send(new ProductTaxPriceItem() { ItemCode = ItemCode, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpPost("CreatePurchaserequest")]
        public async Task<ActionResult> CreatePurchaserequest([FromBody] TblPurchaseReturntDto input)
        {
            var accBranch = await Mediator.Send(new CreatePurchaseRequest() { Input = input, User = UserInfo() });
            if (accBranch.Id > 0)
            {
                return Created($"get/{accBranch.Id}", input);
            }
            return BadRequest(new ApiMessageDto { Message = accBranch.Message });
        }
        [HttpGet("GetPRList/{PRValue}")]
        public async Task<IActionResult> GetPRList([FromRoute] string PRValue)
        {
            var obj = await Mediator.Send(new GetPRDetails() { TranNumber = PRValue, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var obj = await Mediator.Send(new GetPoDetails() { id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("getPOPrintingOneList/{id}")]
        public async Task<IActionResult> GetPOPrintingOneList([FromRoute] int id, [FromQuery] string type)
        {
            var obj = await Mediator.Send(new GetPOPrintingOneList() { id = id, Type = type, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("getPOPrintingTwoList/{id}")]
        public async Task<IActionResult> GetPOPrintingTwoList([FromRoute] int id, [FromQuery] string type)
        {
            var obj = await Mediator.Send(new GetPOPrintingTwoList() { id = id, Type = type, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var ClassId = await Mediator.Send(new DeletePOList() { Id = id, User = UserInfo() });
            if (ClassId > 0)
                return NoContent();
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }
        [HttpGet("AccountsPosting/{id}")]
        public async Task<IActionResult> AccountsPosting([FromRoute] int id)
        {
            var obj = await Mediator.Send(new AccountsPosting() { id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("GRNDetails/{id}")]
        public async Task<IActionResult> GRNDetails([FromRoute] int id)
        {
            var obj = await Mediator.Send(new GRNDetails() { id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        //#region UOMTab
        //[HttpGet("GetUOMListByID")]
        //public async Task<IActionResult> GetUOMListByID([FromQuery] string itemvalue)
        //{
        //    var obj = await Mediator.Send(new GetUOMListByID() { Code = itemvalue, User = UserInfo() });
        //    return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        //}
        ////[HttpPost("CreateUomItem")]
        ////public async Task<ActionResult> CreateInventoryItem([FromBody] TblINVTblErpInvItemsUOMDto input)
        ////{
        ////    var accBranch = await Mediator.Send(new CreateUOMItem() { Input = input, User = UserInfo() });
        ////    if (accBranch.Id > 0)
        ////    {
        ////        return Created($"get/{accBranch.Id}", input);
        ////    }
        ////    return BadRequest(new ApiMessageDto { Message = accBranch.Message });
        ////}
        //#endregion 
        //#region  GenerateItemNumber
        //[HttpGet("GenerateItemNumber")]
        //public async Task<IActionResult> GenerateItemNumber([FromQuery] PaginationFilterDto filter)
        //{
        //    var obj = await Mediator.Send(new GetItemGenerate() { Input = filter.Values(), User = UserInfo() });
        //    return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        //}
        //#endregion
        //#region InventoryItem
        //[HttpGet("GetInventoryItems")]
        //public async Task<IActionResult> GetInventoryItems([FromQuery] string ItemCode)
        //{
        //    var obj = await Mediator.Send(new GetInventoryItem() { ItemCode = ItemCode, User = UserInfo() });
        //    return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        //}
        //#endregion 
        [HttpGet("itemTaxPrice/{ItemCode}")]
        public async Task<IActionResult> itemTaxPrice([FromRoute] string ItemCode)
        {
            var obj = await Mediator.Send(new ItemTaxPriceItem() { ItemCode = ItemCode, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("GetUOMItemList/{ItemUomCode}")]
        public async Task<IActionResult> GetUOMItemList([FromRoute] string ItemUomCode)
        {
            var obj = await Mediator.Send(new GetUOMItemList() { Input = ItemUomCode, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("ProductWarehouse/{branchCode}")]
        public async Task<IActionResult> ProductBranch([FromRoute] string branchCode)
        {
            var obj = await Mediator.Send(new ProductWarehouse() { BranchCode = branchCode, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("Productcompany/{branchCode}")]
        public async Task<IActionResult> Productcompany([FromRoute] string branchCode)
        {
            var obj = await Mediator.Send(new Productcompany() { BranchCode = branchCode, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpPost("CreateGRN")]
        public async Task<ActionResult> CreateGRN([FromBody] TblGRNDetailsDto input)
        {
            var accBranch = await Mediator.Send(new CreateGRN() { Input = input, User = UserInfo() });
            if (accBranch.Id > 0)
            {
                return Created($"get/{accBranch.Id}", input);
            }
            return BadRequest(new ApiMessageDto { Message = accBranch.Message });
        }

        [HttpGet("GetPagedGRNList")]
        public async Task<IActionResult> GetPagedGRNList([FromQuery] PaginationFilterDto filter)
        {
            //await Task.Delay(3000);
            var list = await Mediator.Send(new GetPagedGRNList() { Input = filter.Values(), User = UserInfo() });
            return Ok(list);
        }
        [HttpDelete("GRNDelete/{id}")]
        public async Task<ActionResult> GRNDelete([FromRoute] int id)
        {
            var ClassId = await Mediator.Send(new DeleteGRNList() { Id = id, User = UserInfo() });
            if (ClassId > 0)
                return NoContent();
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }
        [HttpGet("GRNAccountsPosting/{id}")]
        public async Task<IActionResult> GRNAccountsPosting([FromRoute] int id)
        {
            var obj = await Mediator.Send(new GRNAccountsPosting() { id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("GetGRNList/{PONO}")]
        public async Task<IActionResult> GetGRNList([FromRoute] string PONO)
        {
            var obj = await Mediator.Send(new GetGRNDetails() { PONO = PONO, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("GetGRNSelectList")]
        public async Task<IActionResult> GetGRNSelectList([FromQuery] string search)
        {
            var obj = await Mediator.Send(new GetGRNList() { Input = search, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("GRNUpdate/{id}")]
        public async Task<ActionResult> GRNUpdate([FromRoute] string id)
        {
            var ClassId = await Mediator.Send(new GRNUpdateList() { Id = id, User = UserInfo() });
            if (ClassId > 0)
                return NoContent();
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });

        }
    }
}
