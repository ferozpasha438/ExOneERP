using CIN.Application;
using CIN.Application.Common;
using CIN.Application.InvoiceDtos;
using CIN.Application.InvoiceQuery;
using LS.API.OPR.Controllers;
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

        [HttpGet("getCustomerStatementList")]
        public async Task<IActionResult> GetCustomerStatementList([FromQuery] string customerCode)
        {
            var obj = await Mediator.Send(new GetCustomerStatementList() { CustomerCode = customerCode, User = UserInfo() });
            return Ok(obj);
        }

        [HttpGet("getCustomerInvoiceStatementList")]
        public async Task<IActionResult> GetCustomerInvoiceStatementList([FromQuery] string customerCode)
        {
            var obj = await Mediator.Send(new GetCustomerInvoiceStatementList() { CustomerCode = customerCode, User = UserInfo() });
            return Ok(obj);
        }

    }
}
