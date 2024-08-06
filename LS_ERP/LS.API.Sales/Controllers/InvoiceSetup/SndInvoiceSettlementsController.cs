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

namespace LS.API.Sales.Controllers
{
    public class SndInvoiceSettlementsController : FileBaseController
    {
        public SndInvoiceSettlementsController(IOptions<AppSettingsJson> appSettings, IWebHostEnvironment env) : base(appSettings, env)
        {
        }

       

    

        [HttpPost("CreateSndInvoicePaymentSettlement")]
        public async Task<ActionResult> Create([FromBody] IOTblSndTranInvoicePaymentSettlementsDto input)
        {
           
            long id = await Mediator.Send(new CreateSndInvoicePaymentSettlement() { Input = input, User = UserInfo() });
            if (id > 0)
                return Created($"get/{id}", input);
            else if (id <0)
                return BadRequest(new ApiMessageDto { Message = "Error:"+id.ToString() });
          
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }

        

       


       



       
       

    }
}
