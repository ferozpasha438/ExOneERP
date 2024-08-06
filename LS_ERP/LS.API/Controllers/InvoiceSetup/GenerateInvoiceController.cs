using CIN.Application;
using CIN.Application.Common;
using CIN.Application.InvoiceDtos;
using CIN.Application.InvoiceQuery;
using CIN.Application.OperationsMgtQuery;
using CIN.Application.SystemQuery;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace LS.API.Controllers
{
    public class GenerateInvoiceController : FileBaseController
    {
        public GenerateInvoiceController(IOptions<AppSettingsJson> appSettings, IWebHostEnvironment env) : base(appSettings, env)
        {
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {
           
            //await Task.Delay(3000);
            var list = await Mediator.Send(new GetArSalesInvoiceList() { Input = filter.Values(), InvoiceStatusId = (int)InvoiceStatusIdType.Invoice, User = UserInfo() });
            return Ok(list);
        }

        [HttpGet("getCreditList")]
        public async Task<IActionResult> GetCreditList([FromQuery] PaginationFilterDto filter)
        {
            //await Task.Delay(3000);
            var list = await Mediator.Send(new GetArSalesInvoiceList() { Input = filter.Values(), InvoiceStatusId = (int)InvoiceStatusIdType.Credit, User = UserInfo() });
            return Ok(list);
        }

        [HttpGet("getSingleSaleInvoiceById/{id}")]
        public async Task<IActionResult> GetSingleInvoiceById([FromRoute] int id)
        {
            //await Task.Delay(3000);
            var obj = await Mediator.Send(new GetSingleCreditInvoiceById() { Id = id, InvoiceStatusId = (int)InvoiceStatusIdType.Invoice, User = UserInfo() });
            return Ok(obj);
        }

        [HttpGet("getSingleCreditInvoiceById/{id}")]
        public async Task<IActionResult> GetSingleCreditInvoiceById([FromRoute] int id)
        {
            //await Task.Delay(3000);
            var obj = await Mediator.Send(new GetSingleCreditInvoiceById() { Id = id, InvoiceStatusId = (int)InvoiceStatusIdType.Credit, User = UserInfo() });
            return Ok(obj);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TblTranInvoiceDto input)
        {
            input.InvoiceStatusId = (int)InvoiceStatusIdType.Invoice;
            input.InvoiceModule = "Finance";
            return await CreateInvoice(input);
        }

        [HttpPost("createCredit")]
        public async Task<ActionResult> CreateCredit([FromBody] TblTranInvoiceDto input)
        {
            input.InvoiceStatusId = (int)InvoiceStatusIdType.Credit;
            input.InvoiceModule = "Finance";

            if (input.ItemList is not null)
            {
                input.ItemList.ForEach(item =>
                {
                    item.Quantity = -item.Quantity;
                    item.TaxAmount = -item.TaxAmount;
                    item.TotalAmount = -item.TotalAmount;
                });
            }

            input.SubTotal = -input.SubTotal;
            input.TaxAmount = -input.TaxAmount;
            input.TotalAmount = -input.TotalAmount;

            return await CreateInvoice(input);
        }

        private async Task<ActionResult> CreateInvoice(TblTranInvoiceDto input)
        {
            var invoice = await Mediator.Send(new CreateInvoice() { Input = input, User = UserInfo() });
            if (invoice.Id > 0)
            {
                if (input.Id > 0)
                    return NoContent();
                else
                    return Created($"get/{invoice.Id}", input);
            }
            return BadRequest(new ApiMessageDto { Message = invoice.Message });

        }



        [HttpGet("generateInvoiceReportById/{id}")]
        public async Task<ActionResult> GenerateInvoiceReportById([FromRoute] int id)
        {

            var Language = System.Globalization.CultureInfo.CurrentCulture.Name;
            Language = Language == "en-US" ? "en" : Language;

            var invoice = await Mediator.Send(new GetSingleCreditInvoiceById() { Id = id, InvoiceStatusId = (int)InvoiceStatusIdType.Invoice, User = UserInfo() });
            var Company = await Mediator.Send(new GetCompany() { Id = (int)invoice.CompanyId, User = UserInfo() });
            var Customer = await Mediator.Send(new GetCustomerItem() { SiteCode = invoice.SiteCode, Id = (int)invoice.CustomerId, User = UserInfo() });
            var Branch = await Mediator.Send(new GetBranchInfoByBranchCode() { Input = invoice.BranchCode, User = UserInfo() });
            var invoiceItems = invoice.ItemList;


            //InvoiceDTO_AR invoice = (await instance.GetReportByIdAsync(invoiceId)).Data.Data;
            //invoice.Vat = invoice.TaxTariffPercentage;

            //invoice.SubTotal_AR = invoice.SubTotal.ToCommaInvarient();
            //invoice.TaxAmount_AR = invoice.TaxAmount.ToCommaInvarient();
            //invoice.TotalAmount_AR = invoice.TotalAmount.ToCommaInvarient();
            //invoice.Vat_AR = invoice.Vat.ToCommaInvarient();

            var serviceDate1 = Convert.ToDateTime(invoice.ServiceDate1, CultureInfo.InvariantCulture);

            invoice.ServiceDate1 = serviceDate1.ToString("MMM", CultureInfo.InvariantCulture) + " " + serviceDate1.Year.ToString();
            string createdDate = invoice.CreatedOn.Value.ToString("yyyy-MM-dd hh:mm:ss", CultureInfo.InvariantCulture);

            //string createdDate = invoice.CreatedOn.Value.ToString("MM/dd/yyyy hh:mm", CultureInfo.InvariantCulture);


            invoice.QRCode = GenerateQRCode(Company.CompanyName.Trim(), Company?.VATNumber.Trim(), invoice.TaxAmount.DecToString().Replace(",", "").Trim(), invoice.TotalAmount.DecToString().Replace(",", "").Trim(), createdDate.Trim());
            //foreach (var item in invoiceItems)
            //{
            //    item.Quantity_AR = item.Quantity.ToInvarient();
            //    item.UnitPrice_AR = item.UnitPrice.ToInvarient();
            //    item.DiscountAmount_AR = item.DiscountAmount.ToInvarient();
            //    item.TaxTariffPercentage_AR = item.TaxTariffPercentage.ToInvarient();
            //    item.AmountBeforeTax_AR = item.AmountBeforeTax.ToInvarient();
            //    item.TaxAmount_AR = item.UnitPrice.ToInvarient();
            //    item.SubTotal_AR = item.SubTotal.ToInvarient();
            //    item.TotalAmount_AR = item.TotalAmount.ToCommaInvarient();
            //    item.NameEN = item.NameEN;
            //    item.NameAR = item.NameAR;
            //}

            //var Language = System.Globalization.CultureInfo.CurrentCulture.Name;
            //Language = Language == "en-US" ? "en" : Language;
            //if (Language != "en-US")
            //{
            //    foreach (var item in invoiceItems)
            //    {
            //        item
            //    }
            //}
            PrintInvoiceDto printInvoice = new()
            {
                Company = Company,
                Customer = Customer,
                Invoice = invoice,
                InvoiceItems = invoiceItems,
                BankDetails = Branch
            };

            ToWord toWord = new ToWord((decimal)invoice.TotalAmount, new CurrencyInfo(CurrencyInfo.Currencies.SaudiArabia));
            printInvoice.TotalAmountEn = toWord.ConvertToEnglish();
            printInvoice.TotalAmountAr = toWord.ConvertToArabic();
            return Ok(printInvoice);

        }


        [HttpPost("createInvoiceApproval")]
        public async Task<ActionResult> CreateInvoiceApproval([FromBody] TblTranInvoiceApprovalDto input)
        {
            var result = await Mediator.Send(new CreateInvoiceApproval() { Input = input, User = UserInfo() });
            return result ? Ok(result) : BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }

        [HttpPost("createSettlePayment")]
        public async Task<ActionResult> CreateSettlePayment([FromBody] TblTranInvoiceSettlementDto input)
        {
            var result = await Mediator.Send(new CreateInvoiceSettlement() { Input = input, User = UserInfo() });
            return result ? Ok(result) : BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }

    }
}
