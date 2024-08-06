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
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using static CIN.Application.SndDtos.SndQuotationDto;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace LS.API.Sales.Controllers.SND
{
    public class GenerateSndQuotationController : FileBaseController
    {
        public GenerateSndQuotationController(IOptions<AppSettingsJson> appSettings, IWebHostEnvironment env) : base(appSettings, env)
        {
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {

            var list = await Mediator.Send(new GetSndQuotationPagedList(){Input = filter.Values(),User = UserInfo()});
            return Ok(list);
        }

 


        [HttpPost]
        public async Task<ActionResult> CreateSndQuotation(InputTblSndTranQuotationDto input)
        {
            var invoiceId = await Mediator.Send(new CreateSndQuotation() { Input = input, User = UserInfo() });
            if (invoiceId > 0)
            {

                return Created($"get/{invoiceId}", input);
            }
            return BadRequest(new ApiMessageDto { Message = "Error:-" + invoiceId });

        }

        [HttpGet("getSingleSndQuotationById/{id}")]
        public async Task<IActionResult> GetSingleSndQuotationById([FromRoute] int id)
        {
            //await Task.Delay(3000);
            var obj = await Mediator.Send(new GetSingleSndQuotationById() { Id = id, User = UserInfo() });
            return Ok(obj);
        }


        [HttpGet("generateSndQuotationReportById/{id}")]
        public async Task<ActionResult> GenerateSndQuotationReportById([FromRoute] int id)
        {

            try
            {

                var quotation = await Mediator.Send(new GetSingleSndQuotationById() { Id = id, User = UserInfo() });
                var Company = await Mediator.Send(new GetCompany() { Id = (int)quotation.CompanyId, User = UserInfo() });
                var Customer = await Mediator.Send(new GetCustomerItem() { Id = (int)quotation.CustomerId, User = UserInfo() });

                if (quotation.CustName.HasValue())
                {
                    Customer.CustName = quotation.CustName;
                    Customer.CustArbName = quotation.CustArbName;
                }

                var Warehouse = await Mediator.Send(new GetWarehouseInfoByWarehouseCode() { Input = quotation.WarehouseCode, User = UserInfo() });
                var quotationItems = quotation.ItemList;




                var serviceDate1 = Convert.ToDateTime(quotation.ServiceDate1, CultureInfo.InvariantCulture);

                quotation.ServiceDate1 = serviceDate1.ToString("MMM", CultureInfo.InvariantCulture) + " " + serviceDate1.Year.ToString();
                string createdDate = quotation.CreatedOn.Value.ToString("yyyy-MM-dd hh:mm:ss", CultureInfo.InvariantCulture);



                quotation.QRCode = GenerateQRCode(Company.CompanyName.Trim(), Company?.VATNumber.Trim(), quotation.TaxAmount.ToCommaInvarient().Replace(",", "").Trim(), quotation.TotalAmount.ToCommaInvarient().Replace(",", "").Trim(), createdDate.Trim());// createdDate.Split(" ")[0].Trim()

                PrintSndQuotationDto printQuotation = new()
                {
                    Company = Company,
                    Customer = Customer,
                    Quotation = quotation,
                    QuotationItems = quotationItems,
                    BankDetails = Warehouse
                };


                ToWord toWord = new ToWord(quotation.TotalAmount is null ? 0 : (decimal)quotation.TotalAmount, new CurrencyInfo(CurrencyInfo.Currencies.SaudiArabia));
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

        //[HttpPost("createSndQuotationApproval")]
        //public async Task<ActionResult> CreateSndQuotationApproval([FromBody] TblSndTrnApprovalsDto input)
        //{
        //    var result = await Mediator.Send(new CreateSndQuotationApproval() { Input = input, User = UserInfo() });
        //    return result ? Ok(result) : BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        //}

        [HttpPost("cancelSndlQuotation")]
        public async Task<ActionResult> CancelQuotation([FromBody] SndUtilDto Input)
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

        [HttpPost("quotationStockAvailabilty")]
        public async Task<List<ItemStockAvailabilityDto>> QuotationStockAvailabilty([FromBody] InputQuotationStockAvailabilityDto Input)
        {
            var Res = await Mediator.Send(new QuotationStockAvailabilty() { inputDto = Input, User = UserInfo() });
            return Res;
        }

    }
}
