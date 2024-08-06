using CIN.Application;
using CIN.Application.Common;
using CIN.Application.FinPurchaseMgtDto;
using CIN.Application.FinPurchaseMgtQuery;
using CIN.Application.InvoiceDtos;
using CIN.Application.OperationsMgtDtos;
using CIN.Application.OperationsMgtQuery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LS.API.Purchase.Controllers
{
    public class VendorMasterController : BaseController
    {
        public VendorMasterController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
           
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {
            //await Task.Delay(3000);
            var list = await Mediator.Send(new GetVendorsPagedList() { Input = filter.Values(), User = UserInfo() });
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var obj = await Mediator.Send(new GetVendor() { Id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("getVendorSelectItemList")]
        public async Task<IActionResult> GetVendorSelectItemList(string search)
        {
            //await Task.Delay(3000);
            var item = await Mediator.Send(new GetVendorSelectItemList() { Input = search, User = UserInfo() });
            return Ok(item);
        }

        [HttpGet("getVendorByVendorCode/{custCode}")]
        public async Task<TblSndDefVendorMasterDto> GetVendorByVendorCode([FromRoute] string custCode)
        {
            var obj = await Mediator.Send(new GetVendorByVendorCode() { VendorCode = custCode, User = UserInfo() });
            return obj;
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TblSndDefVendorMasterDto dTO)
        {
            var id = await Mediator.Send(new CreateVendor() { VendorDto = dTO, User = UserInfo() });
            if (id > 0)
                return Created($"get/{id}", dTO);
            else if (id == -1)
            {
                return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Duplicate(nameof(dTO.VendCode)) });
            }
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var customerId = await Mediator.Send(new DeleteVendor() { Id = id, User = UserInfo() });
            if (customerId > 0)
                return NoContent();
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }
        
    }
}
