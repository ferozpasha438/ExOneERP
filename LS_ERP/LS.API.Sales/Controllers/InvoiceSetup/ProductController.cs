using CIN.Application;
using CIN.Application.Common;
using CIN.Application.InvoiceDtos;
using CIN.Application.InvoiceQuery;
using CIN.Application.SystemQuery;
using CIN.Application.SystemSetupDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace LS.API.Sales.Controllers
{
    public class ProductController : BaseController
    {
        public ProductController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }

        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] PaginationFilterDto filter)
        {
            var ProductDetails = await Mediator.Send(new GetProductList() { Input = filter.Values(), User = UserInfo() });
            return Ok( ProductDetails );
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            var ProductDetail = await Mediator.Send(new GetProduct() { Id = id, User = UserInfo() });
            return Ok(ProductDetail ?? new ProductDTO() );
        }
        [HttpGet("GetForCompany")]
        public async Task<ActionResult> GetForCompany()
        {
            var ProductDetail = await Mediator.Send(new GetCompanyProductList() { User = UserInfo() });
            return Ok( ProductDetail );
        }

        [HttpGet("GetProductUnitPrice/{productId}")]
        public async Task<ActionResult> GetProductUnitPrice(string productId)
        {
            var ProductDetail = await Mediator.Send(new GetProductUnitPice() { Id = productId, User = UserInfo() });
            return Ok( ProductDetail );
        }



        [HttpPost]
        public async Task<ActionResult> Create([FromBody] ProductDTO dTO)
        {

            dTO.CompanyId = CompanyId;
            long ProductId = await Mediator.Send(new CreateProducts() { ProductDTO = dTO, User = UserInfo() });
            if (ProductId > 0)
                return Created($"get/{ProductId}", dTO);
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed }); 
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
