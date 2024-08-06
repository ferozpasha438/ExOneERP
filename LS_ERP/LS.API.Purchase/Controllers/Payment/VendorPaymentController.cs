using CIN.Application;
using CIN.Application.Common;
using CIN.Application.InventoryDtos;
using CIN.Application.InventoryQuery;
using CIN.Application.InvoiceDtos;
using CIN.Application.InvoiceQuery;
using CIN.Application.SystemQuery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace LS.API.Purchase.Controllers
{
    public class VendorPaymentController : BaseController
    {
        public VendorPaymentController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {
            //await Task.Delay(3000);
            var list = await Mediator.Send(new GetVendorPaymentList() { Input = filter.Values(), User = UserInfo() });
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var obj = await Mediator.Send(new GetVendorSingleItem() { Id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        //[HttpGet("getVendorStatementList")]
        //public async Task<IActionResult> GetVendorStatementList([FromQuery] string customerCode)
        //{
        //    var obj = await Mediator.Send(new GetVendorStatementList() { CustomerCode = customerCode, User = UserInfo() });
        //    return Ok(obj);
        //}
        //[HttpGet("getVendorInvoiceStatementList")]
        //public async Task<IActionResult> GetVendorInvoiceStatementList([FromQuery] string customerCode)
        //{
        //    var obj = await Mediator.Send(new GetVendorInvoiceStatementList() { CustomerCode = customerCode, User = UserInfo() });
        //    return Ok(obj);
        //}

        [HttpGet("getVendorToBePaidAmount")]
        public async Task<IActionResult> GetVendorToBePaidAmount([FromQuery] string customerCode)
        {
            var obj = await Mediator.Send(new GetVendorToBePaidAmount() { CustomerCode = customerCode, User = UserInfo() });
            return Ok(obj);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TblFinTrnVendorPaymentDto input)
        {
            //await Task.Delay(3000);

            var payment = await Mediator.Send(new CreateVendorPayment() { Input = input, User = UserInfo() });
            if (payment.Id > 0)
                return Created($"get/{payment.Id}", input);

            return BadRequest(ApiMessage(payment.Message));

        }

        [HttpPost("vendorPaymentApproval")]
        public async Task<ActionResult> VendorPaymentApproval([FromBody] CustomerPaymentApprovalDto input)
        {
            //await Task.Delay(3000);

            var payment = await Mediator.Send(new VendorPaymentApprovalTwo() { Input = input, User = UserInfo() });
            if (payment.Id > 0)
                return Ok(ApiMessage(payment.Message));
            return BadRequest(ApiMessage(payment.Message));
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var BranchId = await Mediator.Send(new DeleteVendorPayment() { Id = id, User = UserInfo() });
            if (BranchId > 0)
                return NoContent();
            return BadRequest(ApiMessage(ApiMessageInfo.Failed));
        }

        [HttpGet("getVendorpaymentvoucher")]
        public async Task<IActionResult> GetVendorpaymentvoucher([FromQuery] int id)
        {
            var obj = await Mediator.Send(new GetVendorpaymentvoucher() { Id = id, User = UserInfo() });
            return Ok(obj);
        }

    }
}
