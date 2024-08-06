using CIN.Application;
using CIN.Application.Common;
using CIN.Application.GeneralLedgerDtos;
using CIN.Application.GeneralLedgerQuery;
using CIN.Application.InvoiceDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace LS.API.Fin.Controllers.GeneralLedger
{
    public class CashVoucherController : BaseController
    {
        public CashVoucherController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {
            //await Task.Delay(3000);
            var list = await Mediator.Send(new GetCashVoucherList() { Input = filter.Values(), User = UserInfo() });
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var obj = await Mediator.Send(new GetCashVoucher() { Id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateCashVoucherDto input)
        {
            //await Task.Delay(3000);

            var jv = await Mediator.Send(new CreateCashVoucher() { Input = input, User = UserInfo() });
            if (jv.Id > 0)
            {
                if (input.Id > 0)
                    return NoContent();
                else
                    return Created($"get/{jv.Id}", input);
            }
            return BadRequest(new ApiMessageDto { Message = jv.Message });
        }

        [HttpPost("createCashVoucherApproval")]
        public async Task<ActionResult> CreateCashVoucherApproval([FromBody] TblTranInvoiceApprovalDto input)
        {
            var result = await Mediator.Send(new CreateCashVoucherApproval() { Input = input, User = UserInfo() });
            return result ? Ok(result) : BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }

        [HttpPost("createCashVoucherPosting")]
        public async Task<ActionResult> CreateCashVoucherPosting([FromBody] TblTranInvoiceSettlementDto input)
        {
            var result = await Mediator.Send(new CreateCashVoucherPosting() { Input = input, User = UserInfo() });
            return result switch
            {
                1 => Ok(result),
                -1 or -2 => BadRequest(new ApiMessageDto { Message = "Already Done" }),
                _ => BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed })
            };
        }

        [HttpGet("CashVoucherPrint/{id}")]
        public async Task<IActionResult> JournalVoucherPrint([FromRoute] int id)
        {
            var obj = await Mediator.Send(new GetCashVoucherPrint() { Id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }


    }
}
