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
using static CIN.Application.SndDtos.SndDeliveryNoteDto;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace LS.API.Sales.Controllers.SND
{
    public class SndDeliveryNoteController : FileBaseController
    {
        public SndDeliveryNoteController(IOptions<AppSettingsJson> appSettings, IWebHostEnvironment env) : base(appSettings, env)
        {
        }


        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {

            var list = await Mediator.Send(new GetSndDeliveryNotePagedList() { Input = filter.Values(), User = UserInfo() });
            return Ok(list);
        }

        [HttpGet("getSingleSndDeliveryNoteById/{id}")]
        public async Task<IActionResult> GetSingleSndQuotationById([FromRoute] int id)
        {
            //await Task.Delay(3000);
            var obj = await Mediator.Send(new GetSingleSndDeliveryNoteById() { Id = id, User = UserInfo() });
            return Ok(obj);
        }

        [HttpGet("getSndDeliveryNoteReportById/{id}")]
        public async Task<ActionResult> GetSndQuotationReportById([FromRoute] int id)
        {

            try
            {

                var deliveryNote = await Mediator.Send(new GetSingleSndDeliveryNoteById() { Id = id, User = UserInfo() });
                var Company = await Mediator.Send(new GetCompany() { Id = (int)deliveryNote.CompanyId, User = UserInfo() });
                var Customer = await Mediator.Send(new GetCustomerItem() { Id = (int)deliveryNote.CustomerId, User = UserInfo() });

                if (deliveryNote.CustName.HasValue())
                {
                    Customer.CustName = deliveryNote.CustName;
                    Customer.CustArbName = deliveryNote.CustArbName;
                }

                var Warehouse = await Mediator.Send(new GetWarehouseInfoByWarehouseCode() { Input = deliveryNote.WarehouseCode, User = UserInfo() });
                var deliveryNoteItems = deliveryNote.ItemList;




                var serviceDate1 = Convert.ToDateTime(deliveryNote.ServiceDate1, CultureInfo.InvariantCulture);

                deliveryNote.ServiceDate1 = serviceDate1.ToString("MMM", CultureInfo.InvariantCulture) + " " + serviceDate1.Year.ToString();
                string createdDate = deliveryNote.CreatedOn.Value.ToString("yyyy-MM-dd hh:mm:ss", CultureInfo.InvariantCulture);



                deliveryNote.QRCode = GenerateQRCode(Company.CompanyName.Trim(), Company?.VATNumber.Trim(), deliveryNote.TaxAmount.ToCommaInvarient().Replace(",", "").Trim(), deliveryNote.TotalAmount.ToCommaInvarient().Replace(",", "").Trim(), createdDate.Trim());// createdDate.Split(" ")[0].Trim()

                PrintSndDeliveryNoteDto printQuotation = new()
                {
                    Company = Company,
                    Customer = Customer,
                    DeliveryNoteHead = deliveryNote,
                    DeliveryNoteItems = deliveryNoteItems,
                    BankDetails = Warehouse
                };


                ToWord toWord = new ToWord(deliveryNote.TotalAmount is null ? 0 : (decimal)deliveryNote.TotalAmount, new CurrencyInfo(CurrencyInfo.Currencies.SaudiArabia));
                printQuotation.TotalAmountEn = toWord.ConvertToEnglish();
                printQuotation.TotalAmountAr = toWord.ConvertToArabic();
                return Ok(printQuotation);

            }
            catch (Exception ex)
            {

                Log.Info(ex.Message);
                return null;
            }
        }

        [HttpPost("cancelSndDeliveryNote")]
        public async Task<ActionResult> CancelSndDeliveryNote([FromBody] SndUtilDto Input)
        {
            var id = await Mediator.Send(new CancelSndQuotation() { id = Input.Id.Value, User = UserInfo() });
            if (id > 0)
            {
                return Ok(id);
            }
            else
            {
                return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed + ":-" + id.ToString() });
            }
        } 
        
        [HttpPost("generateSndDeliveryNoteByQuotationId")]
        public async Task<ActionResult> GenerateSndDeliveryNoteByQuotationId([FromBody] SndUtilDto Input)
        {
            var res = await Mediator.Send(new GenerateSndDeliveryNoteByQuotationId() { Id = Input.Id.Value, User = UserInfo() });
            if (res.IsSuccess)
            {
                return Ok(Input);
            }
            else
            {
                return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed + ":-" + res.ErrorMsg.ToString() });
            }
        }


 [HttpPost("updateSndDeliveryNoteLineDeliveryQty")]
        public async Task<ActionResult> UpdateSndDeliveryNoteLineDeliveryQty([FromBody] TblSndDeliveryNoteLineDto Input)
        {
            var res = await Mediator.Send(new UpdateSndDeliveryNoteLineDeliveryQty() { InputDto = Input, User = UserInfo() });
            if (res.IsSuccess)
            {
                return Ok(Input);
            }
            else
            {
                return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed + ":-" + res.ErrorMsg.ToString() });
            }
        }
    }
}
