//using CIN.Application;
//using CIN.Application.Common;
//using CIN.Application.InventoryQuery;
//using CIN.Application.InvoiceDtos;
//using CIN.Application.InvoiceQuery;
//using CIN.Application.OperationsMgtQuery;
//using CIN.Application.SNdDtos;
//using CIN.Application.SndQuery;
//using CIN.Application.SystemQuery;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Options;
//using System;
//using System.Globalization;
//using System.Threading.Tasks;
//using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

//namespace LS.API.Sales.Controllers
//{
//    public class GenerateSndInvoiceController : FileBaseController
//    {
//        public GenerateSndInvoiceController(IOptions<AppSettingsJson> appSettings, IWebHostEnvironment env) : base(appSettings, env)
//        {
//        }

//        [HttpGet]
//        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
//        {

//            //await Task.Delay(3000);
//            var list = await Mediator.Send(new GetSndSalesInvoiceList()
//            {
//                Input = filter.Values(),
//                InvoiceStatusId = filter.StatusId ?? (int)InvoiceStatusIdType.Invoice,
//                User = UserInfo()
//            });
//            return Ok(list);
//        }



//        [HttpPost]
//        public async Task<ActionResult> Create([FromBody] InputTblSndTranInvoiceDto input)
//        {
//            //input.InvoiceStatusId = (int)InvoiceStatusIdType.Invoice;
//            if (input.InvoiceStatusId == (int)InvoiceStatusIdType.Credit)
//                input.IsCreditConverted = true;

//            input.InvoiceModule = "SalesAndDistribution";
//            return await CreateInvoice(input);
//        }

//        //[HttpPost("createCredit")]
//        //public async Task<ActionResult> CreateCredit([FromBody] InputTblSndTranInvoiceDto input)
//        //{
//        //    input.InvoiceStatusId = (int)InvoiceStatusIdType.Credit;
//        //    input.InvoiceModule = "SalesAndDistribution";

//        //    if (input.ItemList is not null)
//        //    {
//        //        input.ItemList.ForEach(item =>
//        //        {
//        //            item.Quantity = -item.Quantity;
//        //            item.TaxAmount = -item.TaxAmount;
//        //            item.TotalAmount = -item.TotalAmount;
//        //        });
//        //    }

//        //    input.SubTotal = -input.SubTotal;
//        //    input.TaxAmount = -input.TaxAmount;
//        //    input.TotalAmount = -input.TotalAmount;

//        //    return await CreateInvoice(input);
//        //}

//        private async Task<ActionResult> CreateInvoice(InputTblSndTranInvoiceDto input)
//        {
//            var invoiceId = await Mediator.Send(new CreateSndInvoice() { Input = input, User = UserInfo() });
//            if (invoiceId > 0)
//            {

//                return Created($"get/{invoiceId}", input);
//            }
//            return BadRequest(new ApiMessageDto { Message = "Error:-" + invoiceId });

//        }



//        [HttpGet("getCustomerSndInvoiceNumberList/{id}")]
//        public async Task<IActionResult> GetCustomerInvoiceNumberList([FromRoute] int id)
//        {
//            //await Task.Delay(3000);
//            var obj = await Mediator.Send(new GetCustomerSndInvoiceNumberList() { Id = id, User = UserInfo() });
//            return Ok(obj);
//        }



//        [HttpGet("getSingleSndInvoiceById/{id}")]
//        public async Task<IActionResult> GetSingleSndInvoiceById([FromRoute] int id)
//        {
//            //await Task.Delay(3000);
//            var obj = await Mediator.Send(new GetSingleSndCreditInvoiceById() { Id = id, InvoiceStatusId = (int)InvoiceStatusIdType.Invoice, User = UserInfo() });
//            return Ok(obj);
//        }

//        [HttpGet("generateSndInvoiceReportById/{id}")]
//        public async Task<ActionResult> GenerateSndInvoiceReportById([FromRoute] int id)
//        {

//            try
//            {

//                var invoice = await Mediator.Send(new GetSingleSndCreditInvoiceById() { Id = id, InvoiceStatusId = (int)InvoiceStatusIdType.Invoice, User = UserInfo() });
//                var Company = await Mediator.Send(new GetCompany() { Id = (int)invoice.CompanyId, User = UserInfo() });
//                var Customer = await Mediator.Send(new GetCustomerItem() { Id = (int)invoice.CustomerId, User = UserInfo() });

//                if (invoice.CustName.HasValue())
//                {
//                    Customer.CustName = invoice.CustName;
//                    Customer.CustArbName = invoice.CustArbName;
//                }

//                var Warehouse = await Mediator.Send(new GetWarehouseInfoByWarehouseCode() { Input = invoice.WarehouseCode, User = UserInfo() });
//                var invoiceItems = invoice.ItemList;




//                var serviceDate1 = Convert.ToDateTime(invoice.ServiceDate1, CultureInfo.InvariantCulture);

//                invoice.ServiceDate1 = serviceDate1.ToString("MMM", CultureInfo.InvariantCulture) + " " + serviceDate1.Year.ToString();
//                string createdDate = invoice.CreatedOn.Value.ToString("yyyy-MM-dd hh:mm:ss", CultureInfo.InvariantCulture);



//                invoice.QRCode = GenerateQRCode(Company.CompanyName.Trim(), Company?.VATNumber.Trim(), invoice.TaxAmount.ToCommaInvarient().Replace(",", "").Trim(), invoice.TotalAmount.ToCommaInvarient().Replace(",", "").Trim(), createdDate.Trim());// createdDate.Split(" ")[0].Trim()

//                PrintSndInvoiceDto printInvoice = new()
//                {
//                    Company = Company,
//                    Customer = Customer,
//                    Invoice = invoice,
//                    InvoiceItems = invoiceItems,
//                    BankDetails = Warehouse
//                };


//                ToWord toWord = new ToWord(invoice.TotalAmount is null ? 0 : (decimal)invoice.TotalAmount, new CurrencyInfo(CurrencyInfo.Currencies.SaudiArabia));
//                printInvoice.TotalAmountEn = toWord.ConvertToEnglish();
//                printInvoice.TotalAmountAr = toWord.ConvertToArabic();
//                return Ok(printInvoice);

//            }
//            catch (Exception ex)
//            {

//                Log.Info(ex.Message);
//                return null;
//            }
//        }


//        [HttpPost("createSndInvoiceApproval")]
//        public async Task<ActionResult> CreateSndInvoiceApproval([FromBody] TblSndTrnApprovalsDto input)
//        {
//            var result = await Mediator.Send(new CreateSndInvoiceApproval() { Input = input, User = UserInfo() });
//            return result ? Ok(result) : BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
//        }




//        [HttpPost("cancelSndlInvoice")]
//        public async Task<ActionResult> cancelInvoice([FromBody] SndUtilDto Input)
//        {
//            var id = await Mediator.Send(new CancelSndInvoice() { id = Input.Id.Value, User = UserInfo() });
//            if (id > 0)
//            {
//                return Ok(id);
//            }
//            else
//            {
//                return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed + ":-" + id });
//            }
//        }


//    }
//}
