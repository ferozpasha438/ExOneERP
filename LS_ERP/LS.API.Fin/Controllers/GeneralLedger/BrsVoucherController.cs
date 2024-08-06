using CIN.Application;
using CIN.Application.Common;
using CIN.Application.GeneralLedgerDtos;
using CIN.Application.GeneralLedgerQuery;
using CIN.Application.InvoiceDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LS.API.Fin.Controllers.GeneralLedger
{
    public class BrsVoucherController : BaseController //(BRS) Bank reconciliation Statement
    {
        public BrsVoucherController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }

        #region BRS Methods



        [HttpGet]
        public async Task<IActionResult> GetAllBankReconcialationStatement([FromQuery] PaginationFilterDto filter)
        {
            var obj = await Mediator.Send(new GetBRSVoucherList() { Input = filter.Values(), User = UserInfo() });
            return Ok(obj);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var obj = await Mediator.Send(new GetBankReconciliation() { Id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }


        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateBankReconciliationDto input)
        {
            //await Task.Delay(3000);

            var jv = await Mediator.Send(new CreateUpdateBankReconciliation() { Input = input, User = UserInfo() });
            if (jv.Id > 0)
            {
                if (input.Id > 0)
                    return NoContent();
                else
                    return Created($"get/{jv.Id}", input);
            }
            return BadRequest(jv.Message.APIError());
        }


        [HttpPost("createBrsVoucherApproval")]
        public async Task<ActionResult> CreateBankReconciliationVoucherApproval([FromBody] TblTranInvoiceApprovalDto input)
        {
            var result = await Mediator.Send(new CreateBankReconciliationVoucherApproval() { Input = input, User = UserInfo() });
            return Ok(result);
        }

        [HttpPost("createBrsVoucherPosting")]
        public async Task<ActionResult> CreateJournalVoucherPosting([FromBody] TblTranInvoiceSettlementDto input)
        {
            var result = await Mediator.Send(new CreateBankReconciliationVoucherPosting() { Input = input, User = UserInfo() });
            return result switch
            {
                1 => Ok(result),
                -1 or -2 => BadRequest(new ApiMessageDto { Message = "Already Done" }),
                _ => BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed })
            };
        }

        //[HttpGet("JournalVoucherPrint/{id}")]
        //public async Task<IActionResult> JournalVoucherPrint([FromRoute] int id)
        //{
        //    var obj = await Mediator.Send(new GetJournalVoucherPrint() { Id = id, User = UserInfo() });
        //    return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        //}

        #endregion


        #region PDC Methods

        [HttpGet("getPdcCustVendPaymentList")]
        public async Task<IActionResult> GetPDCCustVendPaymentList([FromQuery] PaginationFilterDto filter)
        {
            var obj = await Mediator.Send(new GetPDCCustVendPaymentList() { Input = filter.Values(), User = UserInfo() });
            return Ok(obj);
        }

        [HttpGet("getPDCCustVendPaymentItem/{id}")]
        public async Task<IActionResult> GetPDCCustVendPaymentItem([FromRoute] int id, [FromQuery] string type)
        {
            var obj = await Mediator.Send(new GetPDCCustVendPaymentItem() { Id = id, Type = type, User = UserInfo() });
            return Ok(obj);
        }

        [HttpPost("changeCheckDate")]
        public async Task<IActionResult> ChangeCheckDate([FromBody] TblFinTrnCustomerPaymentDto input)
        {
            var obj = await Mediator.Send(new ChangeCheckDate() { Input = input, User = UserInfo() });
            return Ok(obj);
        }

        [HttpPost("postingPdc")]
        public async Task<IActionResult> PostingPdc([FromBody] TblFinTrnCustomerPaymentDto input)
        {
            var obj = await Mediator.Send(new PostingPDC() { Input = input, User = UserInfo() });
            if (obj.Id > 0)
            {
                return Ok(obj);
            }
            return BadRequest(obj.Message.APIError());
        }

        #endregion

    }
}
