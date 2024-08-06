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
using static CIN.Application.SndDtos.SndDeliveryNoteDto;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace LS.API.Sales.Controllers.SND
{
    public class GenerateSndInvoiceController : FileBaseController
    {
        public GenerateSndInvoiceController(IOptions<AppSettingsJson> appSettings, IWebHostEnvironment env) : base(appSettings, env)
        {
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {

            //await Task.Delay(3000);
            var list = await Mediator.Send(new GetSndSalesInvoiceList()
            {
                Input = filter.Values(),
                InvoiceStatusId = filter.StatusId ?? (int)InvoiceStatusIdType.Invoice,
                User = UserInfo()
            });
            return Ok(list);
        }



        [HttpPost]
        public async Task<ActionResult> Create([FromBody] InputTblSndTranInvoiceDto input)
        {




            //input.InvoiceStatusId = (int)InvoiceStatusIdType.Invoice;
            if (input.InvoiceStatusId == (int)InvoiceStatusIdType.Credit)
                input.IsCreditConverted = true;

            input.InvoiceModule = "SalesAndDistribution";


           input.SubTotal = 0;
            input.TaxAmount = 0;
            input.TotalPayment = 0;
            input.TotalAmount = 0;
            input.AmountBeforeTax = 0;
            input.DiscountAmount=0;
            foreach (var inv in input.ItemList)
            {
               
                inv.SubTotal = (inv.Quantity * inv.UnitPrice) ?? 0;
                decimal lineDiscount = (inv.SubTotal * inv.Discount / 100) ?? 0;
                decimal footerDiscount = ((inv.SubTotal - (inv.SubTotal * inv.Discount / 100)) * input.FooterDiscount / 100) ?? 0;
                inv.DiscountAmount = lineDiscount + footerDiscount;
                inv.AmountBeforeTax = (inv.SubTotal - inv.DiscountAmount) ?? 0;
                inv.TaxAmount = (inv.AmountBeforeTax * inv.TaxTariffPercentage / 100) ?? 0;



                inv.TotalAmount = (inv.SubTotal - inv.DiscountAmount + inv.TaxAmount) ?? 0;

                input.TaxAmount += (decimal)inv.TaxAmount;
                input.TotalAmount += (decimal)inv.TotalAmount;
                input.TotalPayment += (decimal)inv.TotalAmount;
                input.SubTotal += (decimal)inv.SubTotal;
                input.AmountBeforeTax += (decimal)inv.AmountBeforeTax;
                input.DiscountAmount += (decimal)inv.DiscountAmount;
            }

            return await CreateInvoice(input);
        }

        //[HttpPost("createCredit")]
        //public async Task<ActionResult> CreateCredit([FromBody] InputTblSndTranInvoiceDto input)
        //{
        //    input.InvoiceStatusId = (int)InvoiceStatusIdType.Credit;
        //    input.InvoiceModule = "SalesAndDistribution";

        //    if (input.ItemList is not null)
        //    {
        //        input.ItemList.ForEach(item =>
        //        {
        //            item.Quantity = -item.Quantity;
        //            item.TaxAmount = -item.TaxAmount;
        //            item.TotalAmount = -item.TotalAmount;
        //        });
        //    }

        //    input.SubTotal = -input.SubTotal;
        //    input.TaxAmount = -input.TaxAmount;
        //    input.TotalAmount = -input.TotalAmount;

        //    return await CreateInvoice(input);
        //}

        private async Task<ActionResult> CreateInvoice(InputTblSndTranInvoiceDto input)
        {

            var invoiceId = await Mediator.Send(new CreateSndInvoice() { Input = input, User = UserInfo() });

            if (invoiceId > 0)
            {

                return Created($"get/{invoiceId}", input);
            }
            else if (invoiceId == -1)
            {
                return BadRequest(new ApiMessageDto { Message = "Qty Override Not Allowed For Some Items" });
            } 
            else if (invoiceId == -2)
            {
                return BadRequest(new ApiMessageDto { Message = "Item not found in inventory" });
            }
            return BadRequest(new ApiMessageDto { Message = "Error" + invoiceId });

        }



        [HttpGet("getCustomerSndInvoiceNumberList/{id}")]
        public async Task<IActionResult> GetCustomerInvoiceNumberList([FromRoute] int id)
        {
            //await Task.Delay(3000);
            var obj = await Mediator.Send(new GetCustomerSndInvoiceNumberList() { Id = id, User = UserInfo() });
            return Ok(obj);
        }



        [HttpGet("getSingleSndInvoiceById/{id}")]
        public async Task<IActionResult> GetSingleSndInvoiceById([FromRoute] int id)
        {
            //await Task.Delay(3000);
            var obj = await Mediator.Send(new GetSingleSndCreditInvoiceById() { Id = id, InvoiceStatusId = (int)InvoiceStatusIdType.Invoice, User = UserInfo() });
            return Ok(obj);
        }

        [HttpPost("postSndInvoice")]        //generating,Aproving,Settling and posting AR Invoice
        public async Task<IActionResult> PostSndInvoiceByInvoiceId([FromBody] InputSndTranInvoicePostDto input)
        {
            //await Task.Delay(3000);
            var obj = await Mediator.Send(new PostSndInvoiceByInvoiceIdQuery() { Input = input, User = UserInfo() });
            return obj.IsSuccess ? Ok(obj) : BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed + "-" + obj.ErrorMsg });
        }

        [HttpGet("generateSndInvoiceReportById/{id}")]
        public async Task<ActionResult> GenerateSndInvoiceReportById([FromRoute] int id)
        {

            try
            {

                var invoice = await Mediator.Send(new GetSingleSndCreditInvoiceById() { Id = id, InvoiceStatusId = (int)InvoiceStatusIdType.Invoice, User = UserInfo() });
                var Company = await Mediator.Send(new GetCompany() { Id = (int)invoice.CompanyId, User = UserInfo() });
                var Customer = await Mediator.Send(new GetCustomerItem() { Id = (int)invoice.CustomerId, User = UserInfo() });

                if (invoice.CustName.HasValue())
                {
                    Customer.CustName = invoice.CustName;
                    Customer.CustArbName = invoice.CustArbName;
                }

                var Warehouse = await Mediator.Send(new GetWarehouseInfoByWarehouseCode() { Input = invoice.WarehouseCode, User = UserInfo() });
                var invoiceItems = invoice.ItemList;




                var serviceDate1 = Convert.ToDateTime(invoice.ServiceDate1, CultureInfo.InvariantCulture);

                invoice.ServiceDate1 = serviceDate1.ToString("MMM", CultureInfo.InvariantCulture) + " " + serviceDate1.Year.ToString();
                string createdDate = invoice.CreatedOn.Value.ToString("yyyy-MM-dd hh:mm:ss", CultureInfo.InvariantCulture);



                invoice.QRCode = GenerateQRCode(Company.CompanyName.Trim(), Company?.VATNumber.Trim(), invoice.TaxAmount.ToCommaInvarient().Replace(",", "").Trim(), invoice.TotalAmount.ToCommaInvarient().Replace(",", "").Trim(), createdDate.Trim());// createdDate.Split(" ")[0].Trim()

                PrintSndInvoiceDto printInvoice = new()
                {
                    Company = Company,
                    Customer = Customer,
                    Invoice = invoice,
                    InvoiceItems = invoiceItems,
                    BankDetails = Warehouse
                };


                ToWord toWord = new ToWord(invoice.TotalAmount is null ? 0 : (decimal)invoice.TotalAmount, new CurrencyInfo(CurrencyInfo.Currencies.SaudiArabia));
                printInvoice.TotalAmountEn = toWord.ConvertToEnglish();
                printInvoice.TotalAmountAr = toWord.ConvertToArabic();
                return Ok(printInvoice);

            }
            catch (Exception ex)
            {

                Log.Info(ex.Message);
                return null;
            }
        }






        [HttpPost("convertDeliveryNoteToInvoiceByDeliveryNoteId")]
        public async Task<ActionResult> ConvertDeliveryNoteToInvoiceByDeliveryNoteId([FromBody] SndUtilDto Input)
        {
            var DeliveryNoteData = await Mediator.Send(new GetSingleSndDeliveryNoteById() { Id = Input.Id.Value, User = UserInfo() });  //TblSndDeliveryNoteDto
            if (DeliveryNoteData is null)
            {
                return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed + ", Invalid Deliverynote Id:" + Input.Id });
            }
            if (DeliveryNoteData.IsConvertedDeliveryNoteToInvoice)
            {
                return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed + ", Delivery note Already Converted To Invoice" + Input.Id });
            }

            var QuotationData = await Mediator.Send(new GetSingleSndQuotationById() { Id = DeliveryNoteData.QuotationHeadId, User = UserInfo() });
            if (QuotationData is null)
            {
                return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed + ", Invalid Quotation Id:" + DeliveryNoteData.QuotationHeadId });
            }

            List<TblSndTranInvoiceItemDto> InvoiceItems = new();

            decimal SubTotal = 0;
            decimal TaxAmount = 0;
            decimal TotalPayment = 0;
            decimal TotalAmount = 0;
            decimal AmountBeforeTax = 0;
            decimal DiscountAmount = 0;
            try
            {
                foreach (var delnot in DeliveryNoteData.ItemList)
                {
                    TblSndTranInvoiceItemDto item = new();
                    item.Id = 0;
                    item.ItemCode = delnot.ItemCode;
                    item.ItemId = delnot.ItemId;
                    item.Quantity = delnot.Delivery;
                    item.UnitPrice = delnot.UnitPrice;
                    item.Discount = delnot.Discount;
                    item.Description = delnot.Description;
                    item.TaxTariffPercentage = delnot.TaxTariffPercentage ?? 0;
                    item.UnitType = delnot.UnitType;
                    item.ItemName = delnot.ItemName;
                    item.SubTotal = (delnot.Delivery * delnot.UnitPrice) ?? 0;

                    decimal lineDiscount = (item.SubTotal * item.Discount / 100) ?? 0;
                    decimal footerDiscount = ((item.SubTotal - (item.SubTotal * item.Discount / 100)) * QuotationData.FooterDiscount / 100) ?? 0;
                    item.DiscountAmount = lineDiscount + footerDiscount;
                    // item.DiscountAmount= ((item.SubTotal - (item.SubTotal * delnot.Discount / 100)) * QuotationData.FooterDiscount / 100) ?? 0;
                    item.AmountBeforeTax= (item.SubTotal - item.DiscountAmount) ?? 0;
                    item.TaxAmount = (item.AmountBeforeTax * delnot.TaxTariffPercentage / 100) ?? 0;

                    item.TotalAmount = (item.SubTotal - item.DiscountAmount + item.TaxAmount) ?? 0;

                    SubTotal += (decimal)item.SubTotal;
                    TaxAmount += (decimal)item.TaxAmount;
                    TotalPayment += (decimal)item.TotalAmount;
                    TotalAmount += (decimal)item.TotalAmount;
                    AmountBeforeTax += (decimal)item.AmountBeforeTax;
                    DiscountAmount += (decimal)item.DiscountAmount;
                    InvoiceItems.Add(item);
                }



                InputTblSndTranInvoiceDto InputInvoiceData = new()
                {
                    DeliveryNoteId = DeliveryNoteData.Id,
                    Id = 0,
                    PaymentTermId = DeliveryNoteData.PaymentTermId,
                    Remarks = "Converted Delivery Note to Invoice",

                    AccountNo = DeliveryNoteData.AccountNo,
                    AmountDue = DeliveryNoteData.AmountDue,
                    FooterDiscount = DeliveryNoteData.FooterDiscount,
                    CustomerName = DeliveryNoteData.CustomerName,
                    CustomerId = (int)DeliveryNoteData.CustomerId,
                    AmountBeforeTax = AmountBeforeTax,
                    BankAccount = DeliveryNoteData.BankAccount,
                    BankName = DeliveryNoteData.BankName,
                    Category = DeliveryNoteData.Category,
                    Company = DeliveryNoteData.Company,
                    CompanyId = DeliveryNoteData.CompanyId,
                    CompanyName = DeliveryNoteData.CompanyName,
                    CustArbName = DeliveryNoteData.CustArbName,
                    InvoiceStatusId = 1,
                    WarehouseCode = DeliveryNoteData.WarehouseCode,
                    WarehouseName = DeliveryNoteData.WarehouseName,
                    IsPaid = false,
                    IsPosted = false,
                    InvoiceStatus = "From Delivery Note",
                    VatPercentage = DeliveryNoteData.VatPercentage,
                    TaxIdNumber = DeliveryNoteData.TaxIdNumber,
                    CustName = DeliveryNoteData.CustName,
                    Source = "D",
                    DeleveryRefNumber = DeliveryNoteData.Id.ToString(),
                    IsVoid = false,
                    IsCreditConverted = false,
                    IsSettled = false,
                    InvoiceNotes = "Converted from Delivery note",
                    InvoiceModule = "Snd/DeliverynoteToInvoice",
                    ItemList = InvoiceItems,
                    InvoiceDate = DateTime.UtcNow,

                    PaidAmount = 0,
                    InvoiceDueDate = DeliveryNoteData.QuotationDueDate,

                    DiscountAmount = DiscountAmount,
                    SubTotal = SubTotal,
                    TaxAmount = TaxAmount,
                    TotalPayment = TotalPayment,
                    TotalAmount = TotalAmount,
                    IsApproved = true,
                    SaveType = EnumSaveType.ConvertDelNoteToInvoice,
                    IsQtyDeducted = true,
                      ServiceDate1=DeliveryNoteData.ServiceDate1

                };

                return await CreateInvoice(InputInvoiceData);
            }
            catch (Exception ex)
            {


                return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed + ":-" + ex.Message });
            }
        }
        [HttpPost("convertQuotationToInvoiceByQuotationId")]
        public async Task<ActionResult> ConvertQuotationToInvoiceByQuotationId([FromBody] SndUtilDto Input)
        {
            var QuotationData = await Mediator.Send(new GetSingleSndQuotationById() { Id = Input.Id.Value, User = UserInfo() });  //TblSndDeliveryNoteDto

            if (QuotationData is null)
            {
                return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed + ", Invalid Deliverynote Id:" + Input.Id });
            }
            if (QuotationData.IsConvertedToInvoice)
            {
                return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed + ", Quotation Already Converted To Invoice" + Input.Id });
            }



            List<TblSndTranInvoiceItemDto> InvoiceItems = new();

            decimal SubTotal = 0;
            decimal TaxAmount = 0;
            decimal TotalPayment = 0;
            decimal TotalAmount = 0;
            decimal AmountBeforeTax = 0;
            decimal DiscountAmount = 0;

            try
            {
                foreach (var qtn in QuotationData.ItemList)
                {
                    TblSndTranInvoiceItemDto item = new();
                    item.Id = 0;
                    item.ItemCode = qtn.ItemCode;
                    item.ItemId = qtn.ItemId;
                    item.Quantity = qtn.Quantity;
                    item.UnitPrice = qtn.UnitPrice;
                    item.Discount = qtn.Discount;
                    item.Description = qtn.Description;
                    item.TaxTariffPercentage = qtn.TaxTariffPercentage ?? 0;
                    item.UnitType = qtn.UnitType;
                    item.ItemName = qtn.ItemName;
                    item.SubTotal = (qtn.Quantity * qtn.UnitPrice) ?? 0;

                    //item.DiscountAmount = ((item.SubTotal - (item.SubTotal * qtn.Discount / 100)) * QuotationData.FooterDiscount / 100) ?? 0;

                    decimal lineDiscount = (item.SubTotal * item.Discount / 100) ?? 0;
                    decimal footerDiscount = ((item.SubTotal - (item.SubTotal * item.Discount / 100)) * QuotationData.FooterDiscount / 100) ?? 0;
                    item.DiscountAmount = lineDiscount + footerDiscount;

                    item.AmountBeforeTax = (item.SubTotal - item.DiscountAmount) ?? 0;
                    item.TaxAmount = (item.AmountBeforeTax * qtn.TaxTariffPercentage / 100) ?? 0;
                    item.TotalAmount = (item.SubTotal - item.DiscountAmount + item.TaxAmount) ?? 0;

                    




                    SubTotal += (decimal)item.SubTotal;
                    TaxAmount  += (decimal)item.TaxAmount;
                    TotalPayment+= (decimal)item.TotalAmount;
                    TotalAmount += (decimal)item.TotalAmount;
                    AmountBeforeTax += (decimal)item.AmountBeforeTax;
                    DiscountAmount += (decimal)item.DiscountAmount;



                    InvoiceItems.Add(item);
                }

                InputTblSndTranInvoiceDto InputInvoiceData = new()
                {
                    QuotationId = QuotationData.Id,
                    Id = 0,
                    PaymentTermId = QuotationData.PaymentTermId,
                    Remarks = "Converted Quotation to Invoice",

                    AccountNo = QuotationData.AccountNo,
                    AmountDue = QuotationData.AmountDue,
                    FooterDiscount = QuotationData.FooterDiscount,
                    CustomerName = QuotationData.CustomerName,
                    CustomerId = (int)QuotationData.CustomerId,
                    BankAccount = QuotationData.BankAccount,
                    BankName = QuotationData.BankName,
                    Category = QuotationData.Category,
                    Company = QuotationData.Company,
                    CompanyId = QuotationData.CompanyId,
                    CompanyName = QuotationData.CompanyName,
                    CustArbName = QuotationData.CustArbName,
                    InvoiceStatusId = 1,
                    WarehouseCode = QuotationData.WarehouseCode,
                    WarehouseName = QuotationData.WarehouseName,
                    IsPaid = false,
                    IsPosted = false,
                    InvoiceStatus = "From Quotation",
                    VatPercentage = QuotationData.VatPercentage,
                    TaxIdNumber = QuotationData.TaxIdNumber,
                    CustName = QuotationData.CustName,
                    Source = "Q",
                    DeleveryRefNumber = QuotationData.Id.ToString(),
                    IsVoid = false,
                    IsCreditConverted = false,
                    IsSettled = false,
                    InvoiceNotes = "Converted from Quotation",
                    InvoiceModule = "Snd/QuotationToInvoice",
                    ItemList = InvoiceItems,
                    InvoiceDate = DateTime.UtcNow,

                    AmountBeforeTax= AmountBeforeTax,

                    PaidAmount = 0,
                    InvoiceDueDate = QuotationData.QuotationDueDate.Value,

                    DiscountAmount = DiscountAmount,
                    SubTotal = SubTotal,
                    TaxAmount = TaxAmount,
                    TotalPayment = TotalPayment,
                    TotalAmount = TotalAmount,
                    
                    IsApproved = true,
                    SaveType = EnumSaveType.ConvertQuotToInvoice,
                    LpoContract= QuotationData.LpoContract,
                     CurrencyId=QuotationData.CurrencyId,
                       IsQtyDeducted=false,
                    ServiceDate1 = QuotationData.ServiceDate1



                };

                return await CreateInvoice(InputInvoiceData);
            }
            catch (Exception ex)
            {


                return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed + ":-" + ex.Message });
            }
        }



        [HttpPost("cancelSndlInvoice")]
        public async Task<ActionResult> cancelInvoice([FromBody] SndUtilDto Input)
        {
            var id = await Mediator.Send(new CancelSndInvoice() { id = Input.Id.Value, User = UserInfo() });
            if (id > 0)
            {
                return Ok(id);
            }
            else
            {
                return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed + ":-" + id });
            }
        }


    }
}
