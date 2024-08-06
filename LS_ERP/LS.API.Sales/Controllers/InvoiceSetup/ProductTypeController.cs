using CIN.Application;
using CIN.Application.Common;
using CIN.Application.InvoiceDtos;
using CIN.Application.InvoiceQuery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LS.API.Sales.Controllers.InvoiceSetup
{
    public class ProductTypeController : BaseController
    {
        public ProductTypeController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }

        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] PaginationFilterDto filter)
        {
            var productTypeDetails = await Mediator.Send(new GetProductTypeList() { Input = filter.Values(), User = UserInfo() });
            return Ok(productTypeDetails );
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            var productTypeDetail = await Mediator.Send(new GetProductType() { ProductTypeId = id, User = UserInfo() });
            return Ok(productTypeDetail ?? new ProductTypeDTO() );
        }
        [HttpGet("GetForCompany")]
        public async Task<ActionResult> GetForCompany()
        {
            var productTypeDetail = await Mediator.Send(new GetProductTypeCompnayList() { User = UserInfo() });
            return Ok(productTypeDetail );
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] ProductTypeDTO dTO)
        {
            dTO.CompanyId = CompanyId;
            long productTypeId = await Mediator.Send(new CreateProductTypes() { ProductTypeDTO = dTO, User = UserInfo() });
            if (productTypeId > 0)
                return Created($"get/{productTypeId}", dTO);
            else if (productTypeId == -1)
                return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Duplicate("Name") });
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }
    }
}
