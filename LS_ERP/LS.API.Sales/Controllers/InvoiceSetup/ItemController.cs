using CIN.Application;
using CIN.Application.Common;
using CIN.Application.InvoiceDtos;
using CIN.Application.InvoiceQuery;
using CIN.Application.SndQuery;
using CIN.Application.SystemQuery;
using CIN.Application.SystemSetupDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace LS.API.Sales.Controllers
{
    public class ItemController : BaseController
    {
        public ItemController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }

     

        [HttpGet("getSelectItemList")]
        public async Task<IActionResult> GetSelectItemList([FromQuery] string search)
        {
            var obj = await Mediator.Send(new GetSelectItemList() { Input = search, User = UserInfo() });
            return Ok(obj);
        }

        [HttpGet("getSelectItemMOUList")]
        public async Task<IActionResult> GetSelectItemMOUList()
        {
            var obj = await Mediator.Send(new GetSelectItemMOUList() {  User = UserInfo() });
            return Ok(obj);
        }
          [HttpGet("getSelectItemMOUUnitTypeListByItem/{itemCode}")]
        public async Task<IActionResult> GetSelectItemMOUUnitTypeListByItem([FromRoute] string itemCode)
        {
            var obj = await Mediator.Send(new GetSelectItemMOUUnitTypeListByItem() { Input = itemCode, User = UserInfo() });
            return Ok(obj);
        }

        [HttpGet("itemUnitPriceItem/{id}")]
        public async Task<IActionResult> ItemUnitPriceItem([FromRoute] int id)
        {
            var obj = await Mediator.Send(new ItemUnitPriceItem() { Id = id, User = UserInfo() });
            return Ok(obj);
        }
         [HttpGet("itemUnitPriceItemUnit/{itemCode}/{unitType}")]
        public async Task<IActionResult> ItemUnitPriceItem([FromRoute] string itemCode,[FromRoute] string unitType)
        {
            var obj = await Mediator.Send(new ItemUnitPriceItemUnit() { ItemCode=itemCode,UnitType = unitType, User = UserInfo() });
            return Ok(obj);
        }
          [HttpGet("getItemUomMapByItemUnit/{itemCode}/{unitType}")]
        public async Task<IActionResult> GetItemUomMapByItemUnit([FromRoute] string itemCode,[FromRoute] string unitType)
        {
            var obj = await Mediator.Send(new GetItemUomMapByItemUnit() { ItemCode=itemCode,UnitType = unitType, User = UserInfo() });
            return Ok(obj);
        }

         [HttpGet("itemStockAvailability/{itemCode}")]
        public async Task<IActionResult> ItemStockAvailability([FromRoute] string itemCode)
        {
            var obj = await Mediator.Send(new ItemStockAvailability() { ItemCode=itemCode, User = UserInfo() });
            return Ok(obj);
        }

          [HttpGet("getItemByItemCode/{itemCode}")]
        public async Task<IActionResult> GetItemByItemCode([FromRoute] string itemCode)
        {
            var obj = await Mediator.Send(new GetItemByItemCode() { ItemCode=itemCode, User = UserInfo() });
            return Ok(obj);
        }


        [HttpGet("getSelctedItemByItemBarcode/{barcode}")]
        public async Task<IActionResult> GetSlectedItemByItemBarcode([FromRoute] string barcode)
        {
            var obj = await Mediator.Send(new GetSelectedItemByItemBarcode() { Barcode = barcode, User = UserInfo() });
            return Ok(obj);
        }


    }
}
