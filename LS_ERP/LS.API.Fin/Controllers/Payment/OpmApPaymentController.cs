using CIN.Application;
using CIN.Application.Common;
using CIN.Application.InvoiceDtos;
using CIN.Application.InvoiceQuery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace LS.API.Fin.Controllers.Payment
{
    public class OpmApPaymentController : BaseController
    {
        public OpmApPaymentController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {
            //await Task.Delay(3000);
            var list = await Mediator.Send(new GetOpmVendorPaymentList() { Input = filter.Values(), User = UserInfo() });
            return Ok(list);
        }

        [HttpGet("getOpmApInvoiceList")]
        public async Task<IActionResult> GetOpmApInvoiceList([FromQuery] string customerCode)
        {
            var obj = await Mediator.Send(new GetOpmApInvoiceList() { CustCode = customerCode, User = UserInfo() });
            return Ok(obj);
        }

        [HttpGet("getOpmVendorSingleItem/{id}")]
        public async Task<IActionResult> GetOpmVendorSingleItem([FromRoute] int id)
        {
            var obj = await Mediator.Send(new GetOpmVendorSingleItem() { Id = id, User = UserInfo() });

            var invoices = await Mediator.Send(new GetOpmApInvoiceList() { CustCode = obj.Header.CustCode, IsEdit = true, User = UserInfo() });
            obj.InvoiceList = invoices;

            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("getOpmVendorpaymentvoucher")]
        public async Task<IActionResult> GetOpmVendorpaymentvoucher([FromQuery] int id)
        {
            var obj = await Mediator.Send(new GetOpmVendorpaymentvoucher() { Id = id, User = UserInfo() });
            return Ok(obj);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TblFinTrnOpmCustomerPaymentListHeaderDto input)
        {
            //await Task.Delay(3000);

            var payment = await Mediator.Send(new CreateOpmVendorPayment() { Input = input, User = UserInfo() });
            if (payment.Id > 0)
                return Created($"get/{payment.Id}", input);

            return BadRequest(ApiMessage(payment.Message));

        }

        [HttpGet("getOpmVendorwallet")]
        public async Task<IActionResult> GetOpmVendorwallet([FromQuery] string customerCode, [FromQuery] string siteCode)
        {
            var obj = await Mediator.Send(new GetOpmCustomerwallet() { CustCode = customerCode, SiteCode = siteCode, Source = "AP", User = UserInfo() });
            return Ok(obj);
        }

        [HttpPost("opmVendorPaymentApproval")]
        public async Task<ActionResult> OpmVendorPaymentApproval([FromBody] CustomerPaymentApprovalDto input)
        {
            //await Task.Delay(3000);

            var payment = await Mediator.Send(new OpmVendorPaymentApproval() { Input = input, User = UserInfo() });
            if (payment.Id > 0)
                return Ok(ApiMessage(payment.Message));
            return BadRequest(ApiMessage(payment.Message));

        }

        

    }
}
