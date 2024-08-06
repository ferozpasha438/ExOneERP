using CIN.Application;
using CIN.Application.Common;
using CIN.Application.InventoryQuery;
using CIN.Application.InvoiceDtos;
using CIN.Application.InvoiceQuery;
using CIN.Application.OperationsMgtQuery;
using CIN.Application.SNdDtos;
using CIN.Application.SndQuery;
using CIN.Application.SystemQuery;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Globalization;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace LS.API.Sales.Controllers.SND
{
    public class SndApprovalController : FileBaseController
    {
        public SndApprovalController(IOptions<AppSettingsJson> appSettings, IWebHostEnvironment env) : base(appSettings, env)
        {
        }

     


        [HttpPost("createSndApproval")]
        public async Task<ActionResult> CreateSndApproval([FromBody] TblSndTrnApprovalsDto input)
        {
            var result = await Mediator.Send(new CreateSndApproval() { Input = input, User = UserInfo() });
            return result ? Ok(result) : BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }




      

    }
}
