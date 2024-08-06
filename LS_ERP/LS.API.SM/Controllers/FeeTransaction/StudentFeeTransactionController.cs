using CIN.Application;
using CIN.Application.Common;
using CIN.Application.SchoolMgtDtos;
using CIN.Application.SchoolMgtQuery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LS.API.SM.Controllers.FeeTransaction
{
    public class StudentFeeTransactionController :BaseController
    {
        private IConfiguration _config;

        public StudentFeeTransactionController(IOptions<AppSettingsJson>appSettings, IConfiguration config):base(appSettings)
        {
            _config = config;
        }
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {

            var list = await Mediator.Send(new GetStudentFeeTransactionList() { Input = filter, User = UserInfo() });
            return Ok(list);
        }

        [HttpGet("GetOnlinePaymentHistory")]
        public async Task<IActionResult> GetOnlinePaymentHistory(string addmissionNumber)
        {
            var list = await Mediator.Send(new GetOnlineFeePaymentHistory() { AddmissionNumber = addmissionNumber, User = UserInfo() });
            return Ok(list);
        }

        [HttpPost("CreateOnlineFeeTransaction")]
        public async Task<ActionResult> CreateOnlineFeeTransaction([FromBody] OnlineFeeTransactionDto dTO)
        {
            var id = await Mediator.Send(new CreateStudentOnlinePayment() { Input = dTO, User = UserInfo() });
            if (id > 0)
                return Ok(new OnlinePaymentApiMessageDto {Message="Transaction Successfully Done",Status=true,TrnNumber= "1234" });
            else if (id == -1)
            {
                return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Duplicate(nameof(dTO.StudNum)) });
            }
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }


        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TblTranFeeTransactionDto dTO)
        {
            var id = await Mediator.Send(new CreateUpdateStudentFeeTransaction() { Input = dTO, User = UserInfo() });
            if (id > 0)
                return Created($"get/{id}", dTO);
            else if (id == -1)
            {
                return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Duplicate(nameof(dTO.Id)) });
            }
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }
        [HttpPost("CreateFeeTransaction")]
        public async Task<ActionResult> CreateFeeTransaction([FromBody] StuFeeTransactionDto dTO)
        {
            var id = await Mediator.Send(new CreateFeeTransaction() { Input = dTO, User = UserInfo() });
            if (id > 0)
                return Created($"get/{id}", dTO);
            else if (id == -1)
            {
                return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Duplicate(nameof(dTO.AdmissionNumber)) });
            }
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }


        [HttpPost("CreateBulkFeeTransaction")]
        public async Task<ActionResult> CreateBulkFeeTransaction([FromBody] StuBulkFeeTransactionDto dTO)
        {
            var id = await Mediator.Send(new CreateBulkFeeTransaction() { Input = dTO, User = UserInfo() });
            if (id > 0)
                return Created($"get/{id}", dTO);
            else if (id == -1)
            {
                return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Duplicate(nameof(dTO.AdmissionNumber)) });
            }
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }



        [HttpGet("GetFeeTransactionDetails/{receiptVoucher}")]
        public async Task<IActionResult> GetFeeTransactionDetails([FromRoute] string receiptVoucher)
        {

            var list = await Mediator.Send(new GetFeeTransactionDetails() { ReceiptVoucher = receiptVoucher, User = UserInfo() });
            return Ok(list);
        }
    }
}
