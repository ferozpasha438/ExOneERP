using CIN.Application;
using CIN.Application.Common;
using CIN.Application.GeneralLedgerDtos;
using CIN.Application.GeneralLedgerQuery;
using CIN.Application.InvoiceDtos;
using CIN.Application.OperationsMgtQuery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;

namespace LS.API.Fin.Controllers
{
    public class JournalVoucherController : BaseController
    {
        public JournalVoucherController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {
            //await Task.Delay(3000);
            var list = await Mediator.Send(new GetJournalVoucherList() { Input = filter.Values(), User = UserInfo() });
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var obj = await Mediator.Send(new GetJournalVoucher() { Id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateJournalVoucherDto input)
        {
            //await Task.Delay(3000);

            var jv = await Mediator.Send(new CreateJournalVoucher() { Input = input, User = UserInfo() });
            if (jv.Id > 0)
            {
                if (input.Id > 0)
                    return NoContent();
                else
                    return Created($"get/{jv.Id}", input);
            }
            return BadRequest(new ApiMessageDto { Message = jv.Message });
        }

        [HttpPost("createJournalVoucherApproval")]
        public async Task<ActionResult> CreateJournalVoucherApproval([FromBody] TblTranInvoiceApprovalDto input)
        {
            var result = await Mediator.Send(new CreateJournalVoucherApproval() { Input = input, User = UserInfo() });
            return result ? Ok(result) : BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }

        [HttpPost("createJournalVoucherPosting")]
        public async Task<ActionResult> CreateJournalVoucherPosting([FromBody] TblTranInvoiceSettlementDto input)
        {
            var result = await Mediator.Send(new CreateJournalVoucherPosting() { Input = input, User = UserInfo() });
            return result switch
            {
                1 => Ok(result),
                -1 or -2 => BadRequest(new ApiMessageDto { Message = "Already Done" }),
                _ => BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed })
            };
        }

        [HttpGet("JournalVoucherPrint/{id}")]
        public async Task<IActionResult> JournalVoucherPrint([FromRoute] int id)
        {
            var obj = await Mediator.Send(new GetJournalVoucherPrint() { Id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }


        [HttpPost("createJournalVoucherCopy")]
        public async Task<ActionResult> CreateJournalVoucherCopy([FromBody] JournalVoucherCopyDto input)
        {
            var result = await Mediator.Send(new CreateJournalVoucherCopy() { Input = input, User = UserInfo() });
            return result switch
            {
                1 => Ok(result),
                -1 or -2 => BadRequest(new ApiMessageDto { Message = "Already Done" }),
                _ => BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed })
            };
        }



    }
}
