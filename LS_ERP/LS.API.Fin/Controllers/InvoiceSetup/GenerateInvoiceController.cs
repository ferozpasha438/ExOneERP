using CIN.Application;
using CIN.Application.Common;
using CIN.Application.InvoiceDtos;
using CIN.Application.InvoiceQuery;
using CIN.Application.OperationsMgtQuery;
using CIN.Application.SystemQuery;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Zatca.EInvoice.SDK;
using Zatca.EInvoice.SDK.Contracts.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace LS.API.Fin.Controllers
{
    public class GenerateInvoiceController : FileBaseController
    {
        private readonly IOptions<AppSettingsJson> _appSettings;
        public GenerateInvoiceController(IOptions<AppSettingsJson> appSettings, IWebHostEnvironment env) : base(appSettings, env)
        {
            _appSettings = appSettings;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {

            //await Task.Delay(3000);
            var list = await Mediator.Send(new GetArSalesInvoiceList()
            {
                Input = filter.Values(),
                InvoiceStatusId = filter.StatusId ?? (int)InvoiceStatusIdType.Invoice,
                User = UserInfo()
            });
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

        [HttpGet("getSalesInvoicePrintingList")]
        public async Task<ActionResult> GetSalesInvoicePrintingList([FromQuery] string branchCode, [FromQuery] DateTime? from, [FromQuery] DateTime? to, [FromQuery] int? id)
        {
            var result = await Mediator.Send(new GetSalesInvoicePrintingList() { DateFrom = from, DateTo = to, BranchCode = branchCode, Id = id, User = UserInfo() });
            return Ok(result);
        }

        [HttpGet("getSchoolSalesInvoicePrintingList/{id}")]
        public async Task<IActionResult> GetSchoolSalesInvoicePrintingList([FromRoute] int id)
        {
            //await Task.Delay(3000);
            var invoice = await Mediator.Send(new GetSchoolSalesInvoicePrintingList() { Id = id, User = UserInfo() });

            var Company = await Mediator.Send(new GetCompanyByID() { Id = (int)invoice.CompanyId, User = UserInfo() });
            invoice.CompanyData = Company;
            var Branch = await Mediator.Send(new GetBranchInfoByBranchCodeForSchoolInvoice() { Input = invoice.BranchCode, User = UserInfo() });
            invoice.Bank = Branch;
            //var serviceDate1 = Convert.ToDateTime(invoice.ServiceDate1, CultureInfo.InvariantCulture);

            // invoice.ServiceDate1 = serviceDate1.ToString("MMM", CultureInfo.InvariantCulture) + " " + serviceDate1.Year.ToString();
            string createdDate = invoice.CreatedOn.Value.ToString("yyyy-MM-dd hh:mm:ss", CultureInfo.InvariantCulture);

            invoice.QRCode = GenerateQRCode(Company.CompanyName.Trim(), Company?.VATNumber.Trim(), invoice.TaxAmount.ToCommaInvarient().Replace(",", "").Trim(), invoice.TotalAmount.ToCommaInvarient().Replace(",", "").Trim(), createdDate.Trim());

            ToWord toWord = new ToWord(invoice.TotalAmount is null ? 0 : (decimal)invoice.TotalAmount, new CurrencyInfo(CurrencyInfo.Currencies.SaudiArabia));
            invoice.TotalAmountEn = toWord.ConvertToEnglish();
            invoice.TotalAmountAr = toWord.ConvertToArabic();

            return Ok(invoice);
        }

        [HttpGet("getCustomerInvoiceNumberList/{id}")]
        public async Task<IActionResult> GetCustomerInvoiceNumberList([FromRoute] int id, [FromQuery] string siteCode, [FromQuery] string search)
        {
            //await Task.Delay(3000);
            var obj = await Mediator.Send(new GetCustomerInvoiceNumberList() { Id = id, SiteCode = siteCode, Search = search, User = UserInfo() });
            return Ok(obj);
        }

        [HttpGet("getCustomerInvoiceItemsByIdList/{id}")]
        public async Task<IActionResult> GetCustomerInvoiceItemsByIdList([FromRoute] int id)
        {
            //await Task.Delay(3000);
            var obj = await Mediator.Send(new GetCustomerInvoiceItemsByIdList() { Id = id, User = UserInfo() });
            return Ok(obj);
        }


        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TblTranInvoiceDto input)
        {
            //input.InvoiceStatusId = (int)InvoiceStatusIdType.Invoice;
            if (input.InvoiceStatusId == 2)
                input.IsCreditConverted = true;

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

            //var Language = System.Globalization.CultureInfo.CurrentCulture.Name;
            // Language = Language == "en-US" ? "en" : Language;

            var invoice = await Mediator.Send(new GetSingleCreditInvoiceById() { Id = id, InvoiceStatusId = (int)InvoiceStatusIdType.Invoice, User = UserInfo() });
            var Company = await Mediator.Send(new GetCompany() { Id = (int)invoice.CompanyId, User = UserInfo() });
            var Customer = await Mediator.Send(new GetCustomerItem() { Id = (int)invoice.CustomerId, SiteCode = invoice.SiteCode, User = UserInfo() });

            if (invoice.CustName.HasValue())
            {
                Customer.CustName = invoice.CustName;
                Customer.CustArbName = invoice.CustArbName;
            }

            var Branch = await Mediator.Send(new GetBranchInfoByBranchCode() { Input = invoice.BranchCode, User = UserInfo() });
            var invoiceItems = invoice.ItemList;


            //InvoiceDTO_AR invoice = (await instance.GetReportByIdAsync(invoiceId)).Data.Data;
            //invoice.Vat = invoice.TaxTariffPercentage;
            //if (Language == "ar")
            //{
            //    invoice.SubTotal_AR = invoice.SubTotal.ToCommaInvarient();
            //    invoice.TaxAmount_AR = invoice.TaxAmount.ToCommaInvarient();
            //    invoice.TotalAmount_AR = invoice.TotalAmount.ToCommaInvarient();
            //  //invoice.Vat_AR = invoice.VatPercentage.ToCommaInvarient();
            //}


            var serviceDate1 = Convert.ToDateTime(invoice.ServiceDate1, CultureInfo.InvariantCulture);

            invoice.ServiceDate1 = serviceDate1.ToString("MMM", CultureInfo.InvariantCulture) + " " + serviceDate1.Year.ToString();
            string createdDate = invoice.CreatedOn.Value.ToString("yyyy-MM-dd hh:mm:ss", CultureInfo.InvariantCulture);

            //string createdDate = invoice.CreatedOn.Value.ToString("MM/dd/yyyy hh:mm", CultureInfo.InvariantCulture);


            invoice.QRCode = GenerateQRCode(Company.CompanyName.Trim(), Company?.VATNumber.Trim(), invoice.TaxAmount.ToCommaInvarient().Replace(",", "").Trim(), invoice.TotalAmount.ToCommaInvarient().Replace(",", "").Trim(), createdDate.Trim());// createdDate.Split(" ")[0].Trim()
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
            try
            {

                ToWord toWord = new ToWord(invoice.TotalAmount is null ? 0 : (decimal)invoice.TotalAmount, new CurrencyInfo(CurrencyInfo.Currencies.SaudiArabia));
                printInvoice.TotalAmountEn = toWord.ConvertToEnglish();
                printInvoice.TotalAmountAr = toWord.ConvertToArabic();
                return Ok(printInvoice);

            }
            catch (Exception ex)
            {

                throw;
            }
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


        #region BulkPosting Invoices


        [HttpPost("getBulkPostingInvoiceList")]
        public async Task<ActionResult> GetBulkPostingInvoiceList([FromBody] BulkPostingListDto input)
        {
            var result = await Mediator.Send(new GetBulkPostingInvoiceList() { Input = input, User = UserInfo() });
            return Ok(result);
        }

        [HttpPost("bulkPostingList")]
        public async Task<ActionResult> BulkPostingList([FromBody] BulkPostingListDto input)
        {
            var result = await Mediator.Send(new BulkPostingInvoiceList() { Input = input, User = UserInfo() });
            return Ok();
        }

        #endregion


        #region Zatca Phase-2 APIS


        [HttpGet("generateComplianceCSIDSecond")]
        public async Task<ActionResult> GenerateComplianceCSIDSecond([FromQuery] int companyId, [FromQuery] int invoiceId, [FromQuery] string otp)
        {
            //invoiceId = 20080;
            var Company = await Mediator.Send(new GetCompany() { Id = companyId, User = UserInfo() });
            string csr, requestId, csid, secret;
            CsIdResultDto CsIdResult;

            if (Company.Csr.HasValue() && Company.Csr.Length > 10 && Company.DevCsid.HasValue())
            {
                csr = Company.Csr;
                requestId = Company.ProdRequestId;
                csid = Company.ProdCsid;
                secret = Company.ProdSecret;

                CsIdResult = new() { binarySecurityToken = csid, requestID = requestId, secret = secret };
            }
            else
            {
                var csrGenerator = new CsrGenerator();
                var csrResult = csrGenerator.GenerateCsr(new CsrGenerationDto(
                   "ls",
                  "1-TST|2-TST|3-ed22f1d8-e6a2-1118-9b58-d9a8f11e445f",
                  Company.VATNumber,
                  "فرع الرياض",
                  Company.CompanyName,
                  "SA",
                  "1000",
                  Company.CompanyAddress,
                  "الأنشطة العقارية"
              ), EnvironmentType.Production);

                csr = csrResult.Csr;

                try
                {
                    using (HttpClient client = new HttpClient())
                    {
                        client.BaseAddress = (new Uri(_appSettings.Value.ZatcaApi));
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Add("OTP", otp ?? "123345");
                        client.DefaultRequestHeaders.Add("Accept-Version", "V2");
                        //client.DefaultRequestHeaders.Add("Content-Type", "application/json");
                        //      client.DefaultRequestHeaders.Add("accept", "application/json");
                        //client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                        //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GlobalVariable.TokenString);
                        StringContent content = new StringContent(JsonConvert.SerializeObject(new { csr = csr }), Encoding.UTF8, "application/json");
                        HttpResponseMessage csIdresult = client.PostAsync(ZatcaVariable.GenerateCSID, content).Result;
                        string str2 = csIdresult.Content.ReadAsStringAsync().Result;
                        if (csIdresult.IsSuccessStatusCode)
                        {
                            CsIdResult = JsonConvert.DeserializeObject<CsIdResultDto>(str2);

                            client.DefaultRequestHeaders.Accept.Clear();
                            client.DefaultRequestHeaders.Clear();
                            client.DefaultRequestHeaders.Add("Accept-Version", "V2");
                            client.DefaultRequestHeaders.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes($"{CsIdResult.binarySecurityToken}:{CsIdResult.secret}")));
                            StringContent prodContent = new StringContent(JsonConvert.SerializeObject(new { compliance_request_id = CsIdResult.requestID }), Encoding.UTF8, "application/json");
                            HttpResponseMessage prodcsIdresult = client.PostAsync(ZatcaVariable.ProductionGenerateCSID, prodContent).Result;
                            string prodstr2 = prodcsIdresult.Content.ReadAsStringAsync().Result;
                            if (prodcsIdresult.IsSuccessStatusCode)
                            {
                                var prodCsIdResult = JsonConvert.DeserializeObject<CsIdResultDto>(str2);

                                await Mediator.Send(new UpdateZatcaAPISetting()
                                {
                                    CompanyDto = new()
                                    {
                                        Id = Company.Id,
                                        Csr = csrResult.Csr,
                                        DevRequestId = CsIdResult.requestID,
                                        DevCsid = CsIdResult.binarySecurityToken,
                                        DevSecret = CsIdResult.secret,

                                        ProdRequestId = prodCsIdResult.requestID,
                                        ProdCsid = prodCsIdResult.binarySecurityToken,
                                        ProdSecret = prodCsIdResult.secret
                                    },
                                    User = UserInfo()
                                });

                                CsIdResult = new() { requestID = prodCsIdResult.requestID, binarySecurityToken = prodCsIdResult.binarySecurityToken, secret = prodCsIdResult.secret };
                            }
                        }
                        else
                            return BadRequest(new ApiMessageDto { Message = str2 });
                    }
                }

                catch (Exception ex)
                {
                    return BadRequest(new ApiMessageDto { Message = ex.Message });
                }
            }

            if (CsIdResult is not null && CsIdResult.binarySecurityToken.HasValue())
            {
                try
                {
                    using (HttpClient client = new HttpClient())
                    {

                        var invoice = await Mediator.Send(new GetSingleCreditInvoiceById() { Id = invoiceId, InvoiceStatusId = (int)InvoiceStatusIdType.Invoice, User = UserInfo() });

                        string sellerName = Company.CompanyName.Trim(), vatNumber = Company?.VATNumber.Trim(),
                                            CreatedOn = invoice.CreatedOn.Value.ToString("yyyy-MM-dd hh:mm:ss", CultureInfo.InvariantCulture),
                                            totalInclAmount = invoice.TotalAmount.ToCommaInvarient().Replace(",", "").Trim(),
                                            taxAmount = invoice.TaxAmount.ToCommaInvarient().Replace(",", "").Trim();

                        var tag1 = ConvertUnicodeStringToAscii(1);
                        var tag2 = ConvertUnicodeStringToAscii(2);
                        var tag3 = ConvertUnicodeStringToAscii(3);
                        var tag4 = ConvertUnicodeStringToAscii(4);
                        var tag5 = ConvertUnicodeStringToAscii(5);

                        var sellerNameLen = ConvertUnicodeStringToAscii((sellerName).Trim().Length);
                        var vatNumberLen = ConvertUnicodeStringToAscii((vatNumber).Trim().Length);
                        var CreatedOnLen = ConvertUnicodeStringToAscii((CreatedOn).Trim().Length);
                        var totalInclAmountLen = ConvertUnicodeStringToAscii((totalInclAmount).Trim().Length);
                        var taxAmountLen = ConvertUnicodeStringToAscii((taxAmount).Trim().Length);

                        var barCode = $"{tag1}{sellerNameLen}{sellerName}{tag2}{vatNumberLen}{vatNumber}{tag3}{CreatedOnLen}{CreatedOn}{tag4}{totalInclAmountLen}{totalInclAmount}{tag5}{taxAmountLen}{taxAmount}";
                        var qrCode = Convert.ToBase64String(Encoding.UTF8.GetBytes(barCode));


                        XDocument document = XDocument.Load("Standard_Invoice.xml");
                        XmlDocument xmlDoc = new XmlDocument();
                        string xmlData = document.ToString();

                        xmlData = xmlData.Replace("{{UUID}}", invoice.InvoiceNumber).Replace("{{IssueDate}}", invoice.InvoiceDate.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture))
                            .Replace("{{IssueTime}}", invoice.InvoiceDate.Value.ToString("hh:mm:ss", CultureInfo.InvariantCulture)).Replace("{{ICV}}", invoice.InvoiceNumber.ToString()).Replace("{{VAT}}", Company.VATNumber)
                            .Replace("{{PIH}}", "NWZlY2ViNjZmZmM4NmYzOGQ5NTI3ODZjNmQ2OTZjNzljMmRiYzIzOWRkNGU5MWI0NjcyOWQ3M2EyN2ZiNTdlOQ==").Replace("{{QR}}", qrCode).Replace("{{CRN}}", Company.CrNumber)
                            .Replace("{{StreetName}}", Company.CompanyAddress).Replace("{{BuildingNumber}}", "2255").Replace("{{PlotIdentification}}", "22").Replace("{{CityName}}", Company.City)
                            .Replace("{{PostalZone}}", "12222").Replace("{{Seller}}", Company.CompanyName).Replace("{{DeliveryDate}}", invoice.InvoiceDueDate.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture))
                            .Replace("{{TaxAmount}}", $"{invoice.TaxAmount.TwoDecInvarient()}").Replace("{{TaxableAmount}}", $"{invoice.SubTotal.TwoDecInvarient()}")
                            //.Replace("{{TaxCategory}}", $"15.00")
                            .Replace("{{TaxExclusiveAmount}}", $"{invoice.SubTotal.TwoDecInvarient()}").Replace("{{TaxInclusiveAmount}}", $"{invoice.TotalAmount.TwoDecInvarient()}");
                        string itemLines = string.Empty, taxSubtotal = string.Empty;
                        decimal? totalTaxTariffPercentage = 0;

                        foreach (var item in invoice.ItemList)
                        {
                            taxSubtotal += $"\r\n<cac:TaxSubtotal>\r\n            <cbc:TaxableAmount currencyID=\"SAR\">{(item.Quantity * item.UnitPrice).TwoDecInvarient()}</cbc:TaxableAmount>\r\n            <cbc:TaxAmount currencyID=\"SAR\">{item.TaxAmount.TwoDecInvarient()}</cbc:TaxAmount>\r\n            <cac:TaxCategory>\r\n                <cbc:ID schemeID=\"UN/ECE 5305\" schemeAgencyID=\"6\">S</cbc:ID>\r\n                <cbc:Percent>{item.TaxTariffPercentage.TwoDecInvarient()}</cbc:Percent>\r\n                <cac:TaxScheme>\r\n                    <cbc:ID schemeID=\"UN/ECE 5153\" schemeAgencyID=\"6\">VAT</cbc:ID>\r\n                </cac:TaxScheme>\r\n            </cac:TaxCategory>\r\n        </cac:TaxSubtotal>";
                            //if (item.TaxTariffPercentage is not null && item.TaxTariffPercentage > 0 && totalTaxTariffPercentage <= 0)
                            //  totalTaxTariffPercentage = item.TaxTariffPercentage;

                            //totalTaxTariffPercentage = item.TaxTariffPercentage ?? 0;
                            itemLines += $"\r\n<cac:InvoiceLine>\r\n        <cbc:ID>{item.Id}</cbc:ID>\r\n        <cbc:InvoicedQuantity unitCode=\"PCE\">{item.Quantity.TwoDecInvarient()}</cbc:InvoicedQuantity>\r\n        <cbc:LineExtensionAmount currencyID=\"SAR\">{(item.Quantity * item.UnitPrice).TwoDecInvarient()}</cbc:LineExtensionAmount>\r\n        <cac:TaxTotal>\r\n            <cbc:TaxAmount currencyID=\"SAR\">{item.TaxAmount.TwoDecInvarient()}</cbc:TaxAmount>\r\n            <cbc:RoundingAmount currencyID=\"SAR\">{item.TotalAmount.TwoDecInvarient()}</cbc:RoundingAmount>\r\n        </cac:TaxTotal>\r\n        <cac:Item>\r\n            <cbc:Name>{item.ProductName} </cbc:Name>\r\n            <cac:ClassifiedTaxCategory>\r\n                <cbc:ID>S</cbc:ID>\r\n                <cbc:Percent>{item.TaxTariffPercentage.TwoDecInvarient()}</cbc:Percent>\r\n                <cac:TaxScheme>\r\n                    <cbc:ID>VAT</cbc:ID>\r\n                </cac:TaxScheme>\r\n            </cac:ClassifiedTaxCategory>\r\n        </cac:Item>\r\n        <cac:Price>\r\n            <cbc:PriceAmount currencyID=\"SAR\">{item.UnitPrice.TwoDecInvarient()}</cbc:PriceAmount>\r\n            <cac:AllowanceCharge>\r\n                <cbc:ChargeIndicator>true</cbc:ChargeIndicator>\r\n                <cbc:AllowanceChargeReason>discount</cbc:AllowanceChargeReason>\r\n                <cbc:Amount currencyID=\"SAR\">0.00</cbc:Amount>\r\n            </cac:AllowanceCharge>\r\n        </cac:Price>\r\n    </cac:InvoiceLine>";

                        }

                        xmlData = xmlData.Replace("<cac:TaxSubtotal />", $"{taxSubtotal}");
                        xmlData = xmlData.Replace("{{TaxCategory}}", $"{totalTaxTariffPercentage.TwoDecInvarient()}").Replace("</Invoice>", $"{itemLines}\r\n  </Invoice>");
                        //xmlData = xmlData.Replace("{{TaxCategory}}", $"{totalTaxTariffPercentage.TwoDecInvarient()}");
                        xmlDoc.Load(new StringReader(xmlData));

                        var requestGenerator = new Zatca.EInvoice.SDK.RequestGenerator();
                        var requestGeneratorResut = requestGenerator.GenerateRequest(xmlDoc);

                        var envHash = new Zatca.EInvoice.SDK.EInvoiceHashGenerator();
                        var hashResult = envHash.GenerateEInvoiceHashing(xmlDoc);


                        client.BaseAddress = (new Uri(_appSettings.Value.ZatcaApi));
                        client.DefaultRequestHeaders.Accept.Clear();
                        //client.DefaultRequestHeaders.Add("OTP", "123345");
                        client.DefaultRequestHeaders.Add("Accept-Version", "V2");
                        client.DefaultRequestHeaders.Add("Accept-Language", "en");
                        //client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                        //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GlobalVariable.TokenString);

                        client.DefaultRequestHeaders.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes($"{CsIdResult.binarySecurityToken}:{CsIdResult.secret}")));
                        StringContent content = new StringContent(JsonConvert.SerializeObject(new { invoiceHash = hashResult.Hash, uuid = invoice.InvoiceNumber, invoice = requestGeneratorResut.InvoiceRequest.Invoice }), Encoding.UTF8, "application/json");
                        HttpResponseMessage csIdresult = client.PostAsync(ZatcaVariable.ComplianceInvoice, content).Result;
                        string str2 = csIdresult.Content.ReadAsStringAsync().Result;
                        if (csIdresult.IsSuccessStatusCode)
                        {
                            bool status = str2.ToLower().Contains(":\"cleared\",");
                            await Mediator.Send(new UpdateZatcaClearanceStatus() { Id = invoiceId, Status = status });
                            return Ok(new ApiMessageDto { Message = str2 });
                        }
                        else
                            return BadRequest(new ApiMessageDto { Message = str2 });
                    }
                }

                catch (Exception ex)
                {
                    return BadRequest(new ApiMessageDto { Message = ex.Message });
                }
            }

            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }


        [HttpGet("complianceCSID")]
        public async Task<ActionResult> GenerateComplianceCSID([FromQuery] int companyId, [FromQuery] int invoiceId, [FromQuery] string otp)
        {
            //invoiceId = 20080;
            var Company = await Mediator.Send(new GetCompany() { Id = companyId, User = UserInfo() });
            string csr, requestId, csid, secret;
            CsIdResultDto CsIdResult;


            var csrGenerator = new CsrGenerator();

            CsrResult csrResult = new()
            {
                Csr = _appSettings.Value.Certificate,
                PrivateKey = _appSettings.Value.PrivateKey
                //csrResult.Csr = "MIICBzCCAa0CAQAwXzELMAkGA1UEBhMCU0ExFjAUBgNVBAsMDUlUIERlcGFydG1l\r\nbnQxKzApBgNVBAoMIkFsIFNhaGVyIEFsYW1uaSBmb3IgU2VjdXJpdHkgR3VhcmQx\r\nCzAJBgNVBAMMAmxzMFYwEAYHKoZIzj0CAQYFK4EEAAoDQgAEQrVCq4CD2KsZG5UB\r\nb40s4+yxiH4A6+M1K4ws2WL47tk+W7UjxqILFmETCXSYxVs3f73ZtIRxOZO7+KoL\r\n1XERoaCB7jCB6wYJKoZIhvcNAQkOMYHdMIHaMCQGCSsGAQQBgjcUAgQXExVQUkVa\r\nQVRDQS1Db2RlLVNpZ25pbmcwgbEGA1UdEQSBqTCBpqSBozCBoDE7MDkGA1UEBAwy\r\nMS1UU1R8Mi1UU1R8My1lZDIyZjFkOC1lNmEyLTExMTgtOWI1OC1kOWE4ZjExZTQ0\r\nNWYxHzAdBgoJkiaJk/IsZAEBDA8zMTA4NTg2Mjg0MDAwMDMxDTALBgNVBAwMBDEw\r\nMDAxJDAiBgNVBBoMG09sYXlhLCBSaXlhZGgsIFNhdWRpIEFyYWJpYTELMAkGA1UE\r\nDwwCSVQwCgYIKoZIzj0EAwIDSAAwRQIgdnGyBjD5V2s7EjVta0od5pJ5E4UwF7xt\r\nZeInwkcI6wECIQDDghk0T3drk/Qym/ose06YlVPLZFdK2A6T4nWDC7+/4w==";
            };

            //XDocument document = XDocument.Load("STANDARD_NoSign.xml");
            XDocument document = XDocument.Load("Standard_Invoice.xml");
            XmlDocument xmlDoc = new XmlDocument();
            string xmlData = document.ToString();

            var invoice = await Mediator.Send(new GetSingleCreditInvoiceById() { Id = invoiceId, InvoiceStatusId = (int)InvoiceStatusIdType.Invoice, User = UserInfo() });

            xmlData = xmlData
                .Replace("{{UUID}}", invoice.InvoiceNumber).Replace("{{IssueDate}}", invoice.InvoiceDate.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture))
                .Replace("{{IssueTime}}", invoice.InvoiceDate.Value.ToString("hh:mm:ss", CultureInfo.InvariantCulture)).Replace("{{ICV}}", invoice.InvoiceNumber.ToString()).Replace("{{VAT}}", Company.VATNumber)
                .Replace("{{CRN}}", Company.CrNumber).Replace("{{StreetName}}", Company.CompanyAddress).Replace("{{BuildingNumber}}", "2255").Replace("{{PlotIdentification}}", "22").Replace("{{CityName}}", Company.City)
                .Replace("{{PostalZone}}", "12222").Replace("{{Seller}}", Company.CompanyName).Replace("{{DeliveryDate}}", invoice.InvoiceDueDate.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture))
                .Replace("{{TaxAmount}}", $"{invoice.TaxAmount.TwoDecInvarient()}").Replace("{{TaxableAmount}}", $"{invoice.SubTotal.TwoDecInvarient()}")
                //.Replace("{{TaxCategory}}", $"15.00")
                .Replace("{{TaxExclusiveAmount}}", $"{invoice.SubTotal.TwoDecInvarient()}").Replace("{{TaxInclusiveAmount}}", $"{invoice.TotalAmount.TwoDecInvarient()}");
            string itemLines = string.Empty, taxSubtotal = string.Empty;
            decimal? totalTaxTariffPercentage = 0;

            foreach (var item in invoice.ItemList)
            {
                taxSubtotal += $"\r\n<cac:TaxSubtotal>\r\n            <cbc:TaxableAmount currencyID=\"SAR\">{(item.Quantity * item.UnitPrice).TwoDecInvarient()}</cbc:TaxableAmount>\r\n            <cbc:TaxAmount currencyID=\"SAR\">{item.TaxAmount.TwoDecInvarient()}</cbc:TaxAmount>\r\n            <cac:TaxCategory>\r\n                <cbc:ID schemeID=\"UN/ECE 5305\" schemeAgencyID=\"6\">S</cbc:ID>\r\n                <cbc:Percent>{item.TaxTariffPercentage.TwoDecInvarient()}</cbc:Percent>\r\n                <cac:TaxScheme>\r\n                    <cbc:ID schemeID=\"UN/ECE 5153\" schemeAgencyID=\"6\">VAT</cbc:ID>\r\n                </cac:TaxScheme>\r\n            </cac:TaxCategory>\r\n        </cac:TaxSubtotal>";
                //if (item.TaxTariffPercentage is not null && item.TaxTariffPercentage > 0 && totalTaxTariffPercentage <= 0)
                //  totalTaxTariffPercentage = item.TaxTariffPercentage;

                //totalTaxTariffPercentage = item.TaxTariffPercentage ?? 0;
                itemLines += $"\r\n<cac:InvoiceLine>\r\n        <cbc:ID>{item.Id}</cbc:ID>\r\n        <cbc:InvoicedQuantity unitCode=\"PCE\">{item.Quantity.TwoDecInvarient()}</cbc:InvoicedQuantity>\r\n        <cbc:LineExtensionAmount currencyID=\"SAR\">{(item.Quantity * item.UnitPrice).TwoDecInvarient()}</cbc:LineExtensionAmount>\r\n        <cac:TaxTotal>\r\n            <cbc:TaxAmount currencyID=\"SAR\">{item.TaxAmount.TwoDecInvarient()}</cbc:TaxAmount>\r\n            <cbc:RoundingAmount currencyID=\"SAR\">{item.TotalAmount.TwoDecInvarient()}</cbc:RoundingAmount>\r\n        </cac:TaxTotal>\r\n        <cac:Item>\r\n            <cbc:Name>{item.ProductName} </cbc:Name>\r\n            <cac:ClassifiedTaxCategory>\r\n                <cbc:ID>S</cbc:ID>\r\n                <cbc:Percent>{item.TaxTariffPercentage.TwoDecInvarient()}</cbc:Percent>\r\n                <cac:TaxScheme>\r\n                    <cbc:ID>VAT</cbc:ID>\r\n                </cac:TaxScheme>\r\n            </cac:ClassifiedTaxCategory>\r\n        </cac:Item>\r\n        <cac:Price>\r\n            <cbc:PriceAmount currencyID=\"SAR\">{item.UnitPrice.TwoDecInvarient()}</cbc:PriceAmount>\r\n            <cac:AllowanceCharge>\r\n                <cbc:ChargeIndicator>true</cbc:ChargeIndicator>\r\n                <cbc:AllowanceChargeReason>discount</cbc:AllowanceChargeReason>\r\n                <cbc:Amount currencyID=\"SAR\">0.00</cbc:Amount>\r\n            </cac:AllowanceCharge>\r\n        </cac:Price>\r\n    </cac:InvoiceLine>";

            }

            xmlData = xmlData.Replace("<cac:TaxSubtotal />", $"{taxSubtotal}");
            xmlData = xmlData.Replace("{{TaxCategory}}", $"{totalTaxTariffPercentage.TwoDecInvarient()}").Replace("</Invoice>", $"{itemLines}\r\n  </Invoice>");
            //xmlData = xmlData.Replace("{{TaxCategory}}", $"{totalTaxTariffPercentage.TwoDecInvarient()}");
            xmlDoc.Load(new StringReader(xmlData));

            var singDocument = new EInvoiceSigner().SignDocument(xmlDoc, csrResult.Csr, csrResult.PrivateKey);
            var signedEInvoiceDocument = singDocument.SignedEInvoice;
            var signedEInvoice = singDocument.SignedEInvoice.InnerXml;

            var QRGenerator = new EInvoiceQRGenerator().GenerateEInvoiceQRCode(signedEInvoiceDocument);
            var qrCode = QRGenerator.QR;

            var envHash = new EInvoiceHashGenerator();
            var hashResult = envHash.GenerateEInvoiceHashing(signedEInvoiceDocument);

            var PIHashing = hashResult.Hash;

            var requestGenerator = new RequestGenerator();
            var requestGeneratorResut = requestGenerator.GenerateRequest(signedEInvoiceDocument);
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = (new Uri(_appSettings.Value.ZatcaApi));
                    client.DefaultRequestHeaders.Accept.Clear();
                    //client.DefaultRequestHeaders.Add("OTP", "123345");
                    client.DefaultRequestHeaders.Add("Accept-Version", "V2");
                    client.DefaultRequestHeaders.Add("Accept-Language", "en");
                    //client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GlobalVariable.TokenString);

                    client.DefaultRequestHeaders.Add("Authorization", $"Basic {_appSettings.Value.Authorization}");
                    //client.DefaultRequestHeaders.Add("Authorization", "Basic VFVsSlJsRnFRME5DVDJWblFYZEpRa0ZuU1ZSRmQwRkJUbmxhWTFSS1ZHMVFPV1UwYkVGQlFrRkJRVE5LYWtGTFFtZG5jV2hyYWs5UVVWRkVRV3BDYVUxU1ZYZEZkMWxMUTFwSmJXbGFVSGxNUjFGQ1IxSlpSbUpIT1dwWlYzZDRSWHBCVWtKbmIwcHJhV0ZLYXk5SmMxcEJSVnBHWjA1dVlqTlplRVo2UVZaQ1oyOUthMmxoU21zdlNYTmFRVVZhUm1ka2JHVklVbTVaV0hBd1RWSnpkMGRSV1VSV1VWRkVSWGhLVVZKV2NFWlRWVFZYVkRCc1JGSldUa1JSVkVsMFVUQkZkMGhvWTA1TmFsRjNUV3BGTkUxVVJYcE9SRkYzVjJoalRrMXFXWGROYWtVMFRWUkZNRTVFVVhkWGFrSTBUVkZ6ZDBOUldVUldVVkZIUlhkS1ZGRlVSWEpOUTJ0SFFURlZSVU5vVFdsUlYzZG5WVEpHYjFwWVNXZFJWM2hvWWxjMWNFbEhXblpqYVVKVVdsZE9NV050YkRCbFUwSklaRmRHZVZwRVJXTk5RbTlIUVRGVlJVTjNkMVF5V1VoWmMyUnBOVWxPYVc0eVdWUlpjMlJ0U3pKTFpsbDBha1ZsVFVKM1IwRXhWVVZCZUUxV1ZVWktSbGRyUmxWUk1FVjBVVEk1YTFwVE1WUmhWMlIxWVZjMWJrMUdXWGRGUVZsSVMyOWFTWHBxTUVOQlVWbEdTelJGUlVGQmIwUlJaMEZGTjBndlZFVnVaa3RUUlRVek1DOXFaMlpRYUVVMWVscFVPR2MwY210a1ZWb3ZkRTlvYjFodWVDOW5aa2hVY1dNNVFUVlRWVEptVURaWEwxWldiREJ1TURSRUsxcE1hR05wWkc5RE9YWXJRa1p4THpGSlJYRlBRMEV5WTNkblowNXFUVWxJVWtKblRsWklVa1ZGWjJOcmQyZGpZV3RuWTAxM1oyTkJlRkJxUVRoQ1owNVdRa0ZSVFU1VVJYUlZSemw2VkcxR2RGcFlkM2xNVldNd1prUk5kRmxxYXpGTlZHaG9XbXByZEZsVVNURk5hVEF3VDBkRmVFeFViR3RhUkd0MFdrZFJkMDlVYTNoT1JGRXdUakphYUUxU09IZElVVmxMUTFwSmJXbGFVSGxNUjFGQ1FWRjNVRTE2UlhkUFJGVTBUbXBKTkU1RVFYZE5SRUY2VFZFd2QwTjNXVVJXVVZGTlJFRlJlRTFFUVhkTlUxRjNTV2RaUkZaUlVXRkVRblJRWWtkR05WbFRkMmRWYld3MVdWZFNiMHhEUWxSWldGWnJZVk5DUW1OdFJtbGhWMFY0UzBSQmJVSm5UbFpDUVRoTlNEbHBiakpaVkZsdk9XMUhNa3hVV1hRNWFYQkpUbWx1TWxsVVdYVmtiVU15UzJaWmMyUnRTekpMYTNkSVVWbEVWbEl3VDBKQ1dVVkdSamROTTA5elFXbDBZeTlJV2pGU1QyaE9NbXBwV25oamJpdHNUVUk0UjBFeFZXUkpkMUZaVFVKaFFVWkpTSGx2TTNSNVpUY3hVVzh5Y1dZNFpXcFVhbVJhTjI1SVF6Rk5TVWhzUW1kT1ZraFNPRVZuWkRCM1oyUnZkMmRrWldkblpGTm5aMlJIUjJkak5YTmFSMFozVDJrNGRrd3dUazlRVmtKR1YydFdTbFJzV2xCVFZVNUdWVEJPUWsxcE1VUlJVMmQ0UzFONFJGUnFNVkZWYkhCR1UxVTFWMVF3YkVSU1ZrSk1VMVJKYzFFd05EbFJNRkpSVEVWT1QxQldRakZaYlhod1dYbFZlVTFGZEd4bFUxVjVUVVpPYkdOdVduQlpNbFo2VEVWT1QxQldUbXhqYmxwd1dUSldla3hGVGs5UVZVNTJZbTFhY0ZvelZubFpXRkp3WWpJMGMxSkZUVGxhV0dnd1pXMUdNRmt5UlhOU1JVMDVXakk1TWt4RlVrUlFWM2gyV1RKR2MxQXlUbXhqYmxKd1dtMXNhbGxZVW14VmJWWXlZakpPYUdSSGJIWmlhM2h3WXpOUkwxbHRSbnBhVkRsMldXMXdiRmt6VWtSaVIwWjZZM294YWxWcmVFVmhXRTR3WTIxc2FXUllVbkJpTWpWUllqSnNkV1JFUTBKNloxbEpTM2RaUWtKUlZVaEJVVVZGWjJORmQyZGlOSGRuWW5OSFEwTnpSMEZSVlVaQ2VrRkRhRzlIZFdKSFVtaGpSRzkyVEhrNVJGUnFNVkZTVm5CR1UxVTFWMVF3YkVSU1ZrNUVVVlJKZEZFd1JYTlJNRFE1VVZWc1FreEZUazlRVmtJeFdXMTRjRmw1VlhsTlJYUnNaVk5WZVUxR1RteGpibHB3V1RKV2VreEZUazlRVms1c1kyNWFjRmt5Vm5wTVJVNVBVRlZPZG1KdFduQmFNMVo1V1ZoU2NHSXlOSE5TUlUwNVdsaG9NR1Z0UmpCWk1rVnpVa1ZOT1ZveU9USk1SVkpFVUZkNGRsa3lSbk5RTWs1Q1VUSldlV1JIYkcxaFYwNW9aRWRWTDFsdFJucGFWRGwyV1cxd2JGa3pVa1JpUjBaNlkzb3hhbHBZU2pCaFYxcHdXVEpHTUdGWE9YVlJXRll3WVVjNWVXRllValZOUVRSSFFURlZaRVIzUlVJdmQxRkZRWGRKU0dkRVFUaENaMnR5UW1kRlJVRlpTVE5HVVdORlRIcEJkRUpwVlhKQ1owVkZRVmxKTTBaUmFVSm9jV2RrYUU1RU4wVnZZblJ1VTFOSWVuWnpXakE0UWxaYWIwZGpNa015UkRWalZtUkJaMFpyUVdkRlVVMUNNRWRCTVZWa1NsRlJWMDFDVVVkRFEzTkhRVkZWUmtKM1RVTkNaMmR5UW1kRlJrSlJZMFJCZWtGdVFtZHJja0puUlVWQldVa3pSbEZ2UlVkcVFWbE5RVzlIUTBOelIwRlJWVVpDZDAxRFRVRnZSME5EYzBkQlVWVkdRbmROUkUxQmIwZERRM0ZIVTAwME9VSkJUVU5CTUd0QlRVVlpRMGxSUTNFelUzVXJTRU5KVTJOdmJFOUdkbmQ2WVRNdk5VZDJRVXh0YjFaU09IWndjRFp6WTNSa1J5OU9UVkZKYUVGTFVIUXdOVGRKWVhkNGJteEdPV1kxVEc4MFJsUXplRE15ZUZGYVdISnhRbTQzT0V4ME9HY3dOVUZTOkdkNFEzR3UzczJkSE4zRGhYN2xaV2dHbFQ4YTV6Q3oxcTNTa05mUCsvd3M9");
                    // client.DefaultRequestHeaders.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes($"{CsIdResult.binarySecurityToken}:{CsIdResult.secret}")));
                    StringContent content = new StringContent(JsonConvert.SerializeObject(new { invoiceHash = hashResult.Hash, uuid = invoice.InvoiceNumber, invoice = requestGeneratorResut.InvoiceRequest.Invoice }), Encoding.UTF8, "application/json");
                    HttpResponseMessage csIdresult = client.PostAsync(ZatcaVariable.ComplianceInvoice, content).Result;
                    string clearanceRes = csIdresult.Content.ReadAsStringAsync().Result;
                    if (csIdresult.IsSuccessStatusCode)
                    {                        
                        bool status = clearanceRes.ToLower().Contains(":\"cleared\",");
                        await Mediator.Send(new UpdateZatcaClearanceStatus() { Id = invoiceId, Status = status });
                        return Ok(new ApiMessageDto { Message = clearanceRes });
                    }
                    else
                        return BadRequest(new ApiMessageDto { Message = clearanceRes });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiMessageDto { Message = ex.Message });
            }
        }



        #endregion

    }
}
