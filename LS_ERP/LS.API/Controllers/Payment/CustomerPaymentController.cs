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

namespace LS.API.Controllers.Payment
{
    public class CustomerPaymentController : BaseController
    {
        public CustomerPaymentController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {
            //await Task.Delay(3000);
            var list = await Mediator.Send(new GetCustomerPaymentList() { Input = filter.Values(), User = UserInfo() });
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var obj = await Mediator.Send(new GetSingleItem() { Id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        //[HttpGet("getCustomerStatementList")]
        //public async Task<IActionResult> GetCustomerStatementList([FromQuery] string customerCode)
        //{
        //    var obj = await Mediator.Send(new GetCustomerStatementList() { CustomerCode = customerCode, User = UserInfo() });
        //    return Ok(obj);
        //}
        //[HttpGet("getCustomerInvoiceStatementList")]
        //public async Task<IActionResult> GetCustomerInvoiceStatementList([FromQuery] string customerCode)
        //{
        //    var obj = await Mediator.Send(new GetCustomerInvoiceStatementList() { CustomerCode = customerCode, User = UserInfo() });
        //    return Ok(obj);
        //}

        [HttpGet("getCustomerToBePaidAmount")]
        public async Task<IActionResult> GetCustomerToBePaidAmount([FromQuery] string customerCode)
        {
            var obj = await Mediator.Send(new GetCustomerToBePaidAmount() { CustomerCode = customerCode, User = UserInfo() });
            return Ok(obj);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TblFinTrnCustomerPaymentDto input)
        {
            //await Task.Delay(3000);

            var payment = await Mediator.Send(new CreateCustomerPayment() { Input = input, User = UserInfo() });
            if (payment.Id > 0)
                return Created($"get/{payment.Id}", input);

            return BadRequest(ApiMessage(payment.Message));

        }

        [HttpPost("customerPaymentApproval")]
        public async Task<ActionResult> CustomerPaymentApproval([FromBody] CustomerPaymentApprovalDto input)
        {
            //await Task.Delay(3000);

            var payment = await Mediator.Send(new CustomerPaymentApprovalTwo() { Input = input, User = UserInfo() });
            if (payment.Id > 0)
                return Ok(ApiMessage(payment.Message));
            return BadRequest(ApiMessage(payment.Message));

        }



        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var BranchId = await Mediator.Send(new DeleteCustomerPayment() { Id = id, User = UserInfo() });
            if (BranchId > 0)
                return NoContent();
            return BadRequest(ApiMessage(ApiMessageInfo.Failed));
        }


        [HttpGet("getCustomerpaymentvoucher")]
        public async Task<IActionResult> GetCustomerpaymentvoucher([FromQuery] int id)
        {
            var obj = await Mediator.Send(new GetCustomerpaymentvoucher() { Id = id, User = UserInfo() });
            return Ok(obj);
        }

    }
}
