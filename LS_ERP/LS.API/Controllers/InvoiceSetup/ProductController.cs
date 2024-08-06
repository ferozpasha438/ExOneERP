using CIN.Application;
using CIN.Application.Common;
using CIN.Application.InvoiceDtos;
using CIN.Application.InvoiceQuery;
using CIN.Application.SystemQuery;
using CIN.Application.SystemSetupDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;


namespace LS.API.Controllers.InvoiceSetup
{
    public class ProductController : BaseController
    {
        public ProductController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }

        [HttpGet("getSelectProductList")]
        public async Task<IActionResult> GetSelectProductList([FromQuery] string search)
        {
            var obj = await Mediator.Send(new GetSelectProductList() { Input = search, User = UserInfo() });
            return Ok(obj);
        }

        [HttpGet("productUnitPriceItem/{id}")]
        public async Task<IActionResult> ProductUnitPriceItem([FromRoute] int id)
        {
            var obj = await Mediator.Send(new ProductUnitPriceItem() { Id = id, User = UserInfo() });
            return Ok(obj);
        }


    }
}
