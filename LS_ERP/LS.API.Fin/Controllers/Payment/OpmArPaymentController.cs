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

namespace LS.API.Fin.Controllers.Payment
{
    public class OpmArPaymentController : BaseController
    {
        public OpmArPaymentController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TblFinTrnOpmCustomerPaymentListHeaderDto input)
        {
            //await Task.Delay(3000);

            var payment = await Mediator.Send(new CreateOpmCustomerPayment() { Input = input, User = UserInfo() });
            if (payment.Id > 0)
                return Created($"get/{payment.Id}", input);

            return BadRequest(ApiMessage(payment.Message));

        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {
            //await Task.Delay(3000);
            var list = await Mediator.Send(new GetOpmCustomerPaymentList() { Input = filter.Values(), User = UserInfo() });
            return Ok(list);
        }

        [HttpGet("getOpmArInvoiceList")]
        public async Task<IActionResult> GetOpmArInvoiceList([FromQuery] string customerCode, [FromQuery] string siteCode)
        {
            var obj = await Mediator.Send(new GetOpmArInvoiceList() { CustCode = customerCode, SiteCode = siteCode, User = UserInfo() });
            return Ok(obj);
        }

        [HttpGet("getOpmSingleItem/{id}")]
        public async Task<IActionResult> GetOpmSingleItem([FromRoute] int id)
        {
            var obj = await Mediator.Send(new GetOpmSingleItem() { Id = id, User = UserInfo() });

            var invoices = await Mediator.Send(new GetOpmArInvoiceList() { CustCode = obj.Header.CustCode, IsEdit = true, User = UserInfo() });
            obj.InvoiceList = invoices;

            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("getOpmCustomerpaymentvoucher")]
        public async Task<IActionResult> GetOpmCustomerpaymentvoucher([FromQuery] int id)
        {
            var obj = await Mediator.Send(new GetOpmCustomerpaymentvoucher() { Id = id, User = UserInfo() });
            return Ok(obj);
        }


        [HttpGet("getCustomerAdvancePaymentPazedList")]
        public async Task<IActionResult> GetCustomerAdvancePaymentPazedList([FromQuery] PaginationFilterDto filter)
        {
            //await Task.Delay(3000);
            var list = await Mediator.Send(new GetCustomerAdvancePaymentPazedList() { Input = filter.Values(), User = UserInfo() });
            return Ok(list);
        }



        [HttpGet("getOpmCustomerwallet")]
        public async Task<IActionResult> GetOpmCustomerwallet([FromQuery] string customerCode, [FromQuery] string siteCode)
        {
            var obj = await Mediator.Send(new GetOpmCustomerwallet() { CustCode = customerCode, SiteCode = siteCode, User = UserInfo() });
            return Ok(obj);
        }

        [HttpPost("createOpmCustomerAdvancePayment")]
        public async Task<ActionResult> CreateOpmCustomerAdvancePayment([FromBody] TblFinTrnAdvanceWalletDto input)
        {
            //await Task.Delay(3000);

            var res = await Mediator.Send(new CreateOpmCustomerAdvancePayment() { Input = input, User = UserInfo() });
            if (res.Id > 0)
                return Created($"get/{res.Id}", input);

            return BadRequest(ApiMessage(res.Message));

        }

        [HttpPost("customerPaymentApproval")]
        public async Task<ActionResult> CustomerPaymentApproval([FromBody] CustomerPaymentApprovalDto input)
        {
            //await Task.Delay(3000);

            var payment = await Mediator.Send(new OpmCustomerPaymentApproval() { Input = input, User = UserInfo() });
            if (payment.Id > 0)
                return Ok(ApiMessage(payment.Message));
            return BadRequest(ApiMessage(payment.Message));

        }

    }
}
