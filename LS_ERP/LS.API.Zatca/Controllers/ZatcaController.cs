using CIN.Application;
using CIN.Application.InvoiceDtos;
using CIN.Application.InvoiceQuery;
using CIN.Application.SystemQuery;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Zatca.EInvoice.SDK;
using Zatca.EInvoice.SDK.Contracts.Models;

namespace LS.API.ZatcaAPI.Controllers
{
    public class ZatcaController : BaseController
    {
        private readonly IOptions<AppSettingsJson> _appSettings;

        public ZatcaController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
            _appSettings = appSettings;
        }

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
                        // invoice.InvoiceNumber = "8d487816-70b8-4ade-a618-9d620b73814a";
                        xmlData = xmlData
                            .Replace("{{UUID}}", invoice.InvoiceNumber).Replace("{{IssueDate}}", invoice.InvoiceDate.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture))
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

                        var requestGenerator = new RequestGenerator();
                        var requestGeneratorResut = requestGenerator.GenerateRequest(xmlDoc);

                        var envHash = new EInvoiceHashGenerator();
                        var hashResult = envHash.GenerateEInvoiceHashing(xmlDoc);


                        client.BaseAddress = (new Uri(_appSettings.Value.ZatcaApi));
                        client.DefaultRequestHeaders.Accept.Clear();
                        //client.DefaultRequestHeaders.Add("OTP", "123345");
                        client.DefaultRequestHeaders.Add("Accept-Version", "V2");
                        client.DefaultRequestHeaders.Add("Accept-Language", "en");
                        //client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                        //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GlobalVariable.TokenString);

                        client.DefaultRequestHeaders.Add("Authorization", "Basic VFVsSlExbDZRME5CWjNGblFYZEpRa0ZuU1VkQldUSTRRMEZPSzAxQmIwZERRM0ZIVTAwME9VSkJUVU5OUWxWNFJYcEJVa0puVGxaQ1FVMU5RMjFXU21KdVduWmhWMDV3WW0xamQwaG9ZMDVOYWxGM1RXcEZORTFVUlRCT1JFVjZWMmhqVGsxcWEzZE5ha1V6VFdwRmQwMUVRWGRYYWtJMFRWRnpkME5SV1VSV1VWRkhSWGRLVkZGVVJXTk5RbTlIUVRGVlJVTjNkMVF5V1VoWmMyUnBOVWxPYVc0eVdWUlpjMlJ0U3pKTFpsbDBha1Z5VFVOclIwRXhWVVZEWjNkcFVWZDNaMVV5Um05YVdFbG5VVmQ0YUdKWE5YQkpSMXAyWTJsQ1ZGcFhUakZqYld3d1pWTkNTR1JYUm5sYVJFVmxUVUozUjBFeFZVVkJkM2RXVlVaS1JsZHJSbFZSTUVWMFVUSTVhMXBUTVZSaFYyUjFZVmMxYmsxR1dYZEZRVmxJUzI5YVNYcHFNRU5CVVZsR1N6UkZSVUZCYjBSUlowRkZOMGd2VkVWdVprdFRSVFV6TUM5cVoyWlFhRVUxZWxwVU9HYzBjbXRrVlZvdmRFOW9iMWh1ZUM5blpraFVjV001UVRWVFZUSm1VRFpYTDFaV2JEQnVNRFJFSzFwTWFHTnBaRzlET1hZclFrWnhMekZKUlhGUFFqVlVRMEkwYWtGTlFtZE9Wa2hTVFVKQlpqaEZRV3BCUVUxSlNGSkNaMDVXU0ZKRlJXZGphM2RuWTJGcloyTk5kMmRqUVhoUWFrRTRRbWRPVmtKQlVVMU9WRVYwVlVjNWVsUnRSblJhV0hkNVRGVmpNR1pFVFhSWmFtc3hUVlJvYUZwcWEzUlpWRWt4VFdrd01FOUhSWGhNVkd4cldrUnJkRnBIVVhkUFZHdDRUa1JSTUU0eVdtaE5VamgzU0ZGWlMwTmFTVzFwV2xCNVRFZFJRa0ZSZDFCTmVrVjNUMFJWTkU1cVNUUk9SRUYzVFVSQmVrMVJNSGREZDFsRVZsRlJUVVJCVVhoTlJFRjNUVk5SZDBsbldVUldVVkZoUkVKMFVHSkhSalZaVTNkblZXMXNOVmxYVW05TVEwSlVXVmhXYTJGVFFrSmpiVVpwWVZkRmVFdEVRVzFDWjA1V1FrRTRUVWc1YVc0eVdWUlpiemx0UnpKTVZGbDBPV2x3U1U1cGJqSlpWRmwxWkcxRE1rdG1XWE5rYlVzeVMydDNRMmRaU1V0dldrbDZhakJGUVhkSlJGSjNRWGRTUVVsblpWUnlURkpRUTFnMVFVbDBiVFo1ZVdvdllqaHphVE5JUkVzM2F6TjRka2d4ZDIxWmRqa3JSRWh1VVVOSlF6bHJOa3hrZFdjNVFsVXpPV2t6YVdaemJVUnNWWE5yVWpOS2FHaHpRU3RhU214QmREQndSVVIwUWc9PTovTHRZUmlycjJYeDgrSWxQKzduZUY2MjJHVlBsWVdOMUlPeXlTbVA1aGN3PQ==");
                        //client.DefaultRequestHeaders.Add("Authorization", "Basic VFVsSlJsRnFRME5DVDJWblFYZEpRa0ZuU1ZSRmQwRkJUbmxhWTFSS1ZHMVFPV1UwYkVGQlFrRkJRVE5LYWtGTFFtZG5jV2hyYWs5UVVWRkVRV3BDYVUxU1ZYZEZkMWxMUTFwSmJXbGFVSGxNUjFGQ1IxSlpSbUpIT1dwWlYzZDRSWHBCVWtKbmIwcHJhV0ZLYXk5SmMxcEJSVnBHWjA1dVlqTlplRVo2UVZaQ1oyOUthMmxoU21zdlNYTmFRVVZhUm1ka2JHVklVbTVaV0hBd1RWSnpkMGRSV1VSV1VWRkVSWGhLVVZKV2NFWlRWVFZYVkRCc1JGSldUa1JSVkVsMFVUQkZkMGhvWTA1TmFsRjNUV3BGTkUxVVJYcE9SRkYzVjJoalRrMXFXWGROYWtVMFRWUkZNRTVFVVhkWGFrSTBUVkZ6ZDBOUldVUldVVkZIUlhkS1ZGRlVSWEpOUTJ0SFFURlZSVU5vVFdsUlYzZG5WVEpHYjFwWVNXZFJWM2hvWWxjMWNFbEhXblpqYVVKVVdsZE9NV050YkRCbFUwSklaRmRHZVZwRVJXTk5RbTlIUVRGVlJVTjNkMVF5V1VoWmMyUnBOVWxPYVc0eVdWUlpjMlJ0U3pKTFpsbDBha1ZsVFVKM1IwRXhWVVZCZUUxV1ZVWktSbGRyUmxWUk1FVjBVVEk1YTFwVE1WUmhWMlIxWVZjMWJrMUdXWGRGUVZsSVMyOWFTWHBxTUVOQlVWbEdTelJGUlVGQmIwUlJaMEZGTjBndlZFVnVaa3RUUlRVek1DOXFaMlpRYUVVMWVscFVPR2MwY210a1ZWb3ZkRTlvYjFodWVDOW5aa2hVY1dNNVFUVlRWVEptVURaWEwxWldiREJ1TURSRUsxcE1hR05wWkc5RE9YWXJRa1p4THpGSlJYRlBRMEV5WTNkblowNXFUVWxJVWtKblRsWklVa1ZGWjJOcmQyZGpZV3RuWTAxM1oyTkJlRkJxUVRoQ1owNVdRa0ZSVFU1VVJYUlZSemw2VkcxR2RGcFlkM2xNVldNd1prUk5kRmxxYXpGTlZHaG9XbXByZEZsVVNURk5hVEF3VDBkRmVFeFViR3RhUkd0MFdrZFJkMDlVYTNoT1JGRXdUakphYUUxU09IZElVVmxMUTFwSmJXbGFVSGxNUjFGQ1FWRjNVRTE2UlhkUFJGVTBUbXBKTkU1RVFYZE5SRUY2VFZFd2QwTjNXVVJXVVZGTlJFRlJlRTFFUVhkTlUxRjNTV2RaUkZaUlVXRkVRblJRWWtkR05WbFRkMmRWYld3MVdWZFNiMHhEUWxSWldGWnJZVk5DUW1OdFJtbGhWMFY0UzBSQmJVSm5UbFpDUVRoTlNEbHBiakpaVkZsdk9XMUhNa3hVV1hRNWFYQkpUbWx1TWxsVVdYVmtiVU15UzJaWmMyUnRTekpMYTNkSVVWbEVWbEl3VDBKQ1dVVkdSamROTTA5elFXbDBZeTlJV2pGU1QyaE9NbXBwV25oamJpdHNUVUk0UjBFeFZXUkpkMUZaVFVKaFFVWkpTSGx2TTNSNVpUY3hVVzh5Y1dZNFpXcFVhbVJhTjI1SVF6Rk5TVWhzUW1kT1ZraFNPRVZuWkRCM1oyUnZkMmRrWldkblpGTm5aMlJIUjJkak5YTmFSMFozVDJrNGRrd3dUazlRVmtKR1YydFdTbFJzV2xCVFZVNUdWVEJPUWsxcE1VUlJVMmQ0UzFONFJGUnFNVkZWYkhCR1UxVTFWMVF3YkVSU1ZrSk1VMVJKYzFFd05EbFJNRkpSVEVWT1QxQldRakZaYlhod1dYbFZlVTFGZEd4bFUxVjVUVVpPYkdOdVduQlpNbFo2VEVWT1QxQldUbXhqYmxwd1dUSldla3hGVGs5UVZVNTJZbTFhY0ZvelZubFpXRkp3WWpJMGMxSkZUVGxhV0dnd1pXMUdNRmt5UlhOU1JVMDVXakk1TWt4RlVrUlFWM2gyV1RKR2MxQXlUbXhqYmxKd1dtMXNhbGxZVW14VmJWWXlZakpPYUdSSGJIWmlhM2h3WXpOUkwxbHRSbnBhVkRsMldXMXdiRmt6VWtSaVIwWjZZM294YWxWcmVFVmhXRTR3WTIxc2FXUllVbkJpTWpWUllqSnNkV1JFUTBKNloxbEpTM2RaUWtKUlZVaEJVVVZGWjJORmQyZGlOSGRuWW5OSFEwTnpSMEZSVlVaQ2VrRkRhRzlIZFdKSFVtaGpSRzkyVEhrNVJGUnFNVkZTVm5CR1UxVTFWMVF3YkVSU1ZrNUVVVlJKZEZFd1JYTlJNRFE1VVZWc1FreEZUazlRVmtJeFdXMTRjRmw1VlhsTlJYUnNaVk5WZVUxR1RteGpibHB3V1RKV2VreEZUazlRVms1c1kyNWFjRmt5Vm5wTVJVNVBVRlZPZG1KdFduQmFNMVo1V1ZoU2NHSXlOSE5TUlUwNVdsaG9NR1Z0UmpCWk1rVnpVa1ZOT1ZveU9USk1SVkpFVUZkNGRsa3lSbk5RTWs1Q1VUSldlV1JIYkcxaFYwNW9aRWRWTDFsdFJucGFWRGwyV1cxd2JGa3pVa1JpUjBaNlkzb3hhbHBZU2pCaFYxcHdXVEpHTUdGWE9YVlJXRll3WVVjNWVXRllValZOUVRSSFFURlZaRVIzUlVJdmQxRkZRWGRKU0dkRVFUaENaMnR5UW1kRlJVRlpTVE5HVVdORlRIcEJkRUpwVlhKQ1owVkZRVmxKTTBaUmFVSm9jV2RrYUU1RU4wVnZZblJ1VTFOSWVuWnpXakE0UWxaYWIwZGpNa015UkRWalZtUkJaMFpyUVdkRlVVMUNNRWRCTVZWa1NsRlJWMDFDVVVkRFEzTkhRVkZWUmtKM1RVTkNaMmR5UW1kRlJrSlJZMFJCZWtGdVFtZHJja0puUlVWQldVa3pSbEZ2UlVkcVFWbE5RVzlIUTBOelIwRlJWVVpDZDAxRFRVRnZSME5EYzBkQlVWVkdRbmROUkUxQmIwZERRM0ZIVTAwME9VSkJUVU5CTUd0QlRVVlpRMGxSUTNFelUzVXJTRU5KVTJOdmJFOUdkbmQ2WVRNdk5VZDJRVXh0YjFaU09IWndjRFp6WTNSa1J5OU9UVkZKYUVGTFVIUXdOVGRKWVhkNGJteEdPV1kxVEc4MFJsUXplRE15ZUZGYVdISnhRbTQzT0V4ME9HY3dOVUZTOkdkNFEzR3UzczJkSE4zRGhYN2xaV2dHbFQ4YTV6Q3oxcTNTa05mUCsvd3M9");
                        // client.DefaultRequestHeaders.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes($"{CsIdResult.binarySecurityToken}:{CsIdResult.secret}")));
                        StringContent content = new StringContent(JsonConvert.SerializeObject(new { invoiceHash = hashResult.Hash, uuid = invoice.InvoiceNumber, invoice = requestGeneratorResut.InvoiceRequest.Invoice }), Encoding.UTF8, "application/json");
                        HttpResponseMessage csIdresult = client.PostAsync(ZatcaVariable.ComplianceInvoice, content).Result;
                        string apoResponse = csIdresult.Content.ReadAsStringAsync().Result;
                        if (csIdresult.IsSuccessStatusCode)
                        {
                            bool status = apoResponse.ToLower().Contains(":\"cleared\",");
                            await Mediator.Send(new UpdateZatcaClearanceStatus() { Id = invoiceId, Status = status });
                            return Ok(new ApiMessageDto { Message = apoResponse });
                        }
                        else
                            return BadRequest(new ApiMessageDto { Message = apoResponse });
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(new ApiMessageDto { Message = ex.Message });
                }

            }
            else
                return BadRequest(new ApiMessageDto { Message = "Zatca Configuration failed, add first and then submit Invoice(s)." });
            //return BadRequest(new ApiMessageDto { Message = "Zatca Config failed, Generate first and then submit Invoice(s)." });
        }

        [HttpGet("complianceCSIDold")]
        public async Task<ActionResult> GenerateComplianceCSIDOld([FromQuery] int companyId, [FromQuery] int invoiceId, [FromQuery] string otp)
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
              ), EnvironmentType.Simulation);




                //  var csrResult = csrGenerator.GenerateCsr(new CsrGenerationDto(
                //     "AM",
                //    "1-TST|2-TST|3-515851ae-8e07-4167-81c6-497565aecb6a",
                //    "310054439500003",
                //    "فرع ",
                //    "AMSteel - Port & Steel Operations",
                //    "SA",
                //    "1000",
                //    "Prince Sultan Jeddah",
                //    "test"
                //), EnvironmentType.Simulation);

                csr = csrResult.Csr;

                try
                {
                    using (HttpClient client = new HttpClient())
                    {

                        client.BaseAddress = (new Uri(_appSettings.Value.ZatcaApi));
                        //client.DefaultRequestHeaders.Accept.Clear();
                        //client.DefaultRequestHeaders.Add("OTP", otp ?? "123345");
                        //client.DefaultRequestHeaders.Add("Accept-Version", "V2");
                        ////client.DefaultRequestHeaders.Add("Content-Type", "application/json");
                        ////      client.DefaultRequestHeaders.Add("accept", "application/json");
                        ////client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                        ////client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GlobalVariable.TokenString);
                        //StringContent content = new StringContent(JsonConvert.SerializeObject(new { csr = csr }), Encoding.UTF8, "application/json");
                        //HttpResponseMessage csIdresult = client.PostAsync(ZatcaVariable.GenerateCSID, content).Result;
                        //string str2 = csIdresult.Content.ReadAsStringAsync().Result;
                        //if (csIdresult.IsSuccessStatusCode)
                        //{
                        //    CsIdResult = JsonConvert.DeserializeObject<CsIdResultDto>(str2);

                        //if (true)
                        //{
                        //    client.DefaultRequestHeaders.Accept.Clear();
                        //    client.DefaultRequestHeaders.Clear();
                        //    client.DefaultRequestHeaders.Add("Accept-Version", "V2");
                        //    client.DefaultRequestHeaders.Add("Authorization", "Basic VFVsSlExbDZRME5CWjNGblFYZEpRa0ZuU1VkQldUSTRRMEZPSzAxQmIwZERRM0ZIVTAwME9VSkJUVU5OUWxWNFJYcEJVa0puVGxaQ1FVMU5RMjFXU21KdVduWmhWMDV3WW0xamQwaG9ZMDVOYWxGM1RXcEZORTFVUlRCT1JFVjZWMmhqVGsxcWEzZE5ha1V6VFdwRmQwMUVRWGRYYWtJMFRWRnpkME5SV1VSV1VWRkhSWGRLVkZGVVJXTk5RbTlIUVRGVlJVTjNkMVF5V1VoWmMyUnBOVWxPYVc0eVdWUlpjMlJ0U3pKTFpsbDBha1Z5VFVOclIwRXhWVVZEWjNkcFVWZDNaMVV5Um05YVdFbG5VVmQ0YUdKWE5YQkpSMXAyWTJsQ1ZGcFhUakZqYld3d1pWTkNTR1JYUm5sYVJFVmxUVUozUjBFeFZVVkJkM2RXVlVaS1JsZHJSbFZSTUVWMFVUSTVhMXBUTVZSaFYyUjFZVmMxYmsxR1dYZEZRVmxJUzI5YVNYcHFNRU5CVVZsR1N6UkZSVUZCYjBSUlowRkZOMGd2VkVWdVprdFRSVFV6TUM5cVoyWlFhRVUxZWxwVU9HYzBjbXRrVlZvdmRFOW9iMWh1ZUM5blpraFVjV001UVRWVFZUSm1VRFpYTDFaV2JEQnVNRFJFSzFwTWFHTnBaRzlET1hZclFrWnhMekZKUlhGUFFqVlVRMEkwYWtGTlFtZE9Wa2hTVFVKQlpqaEZRV3BCUVUxSlNGSkNaMDVXU0ZKRlJXZGphM2RuWTJGcloyTk5kMmRqUVhoUWFrRTRRbWRPVmtKQlVVMU9WRVYwVlVjNWVsUnRSblJhV0hkNVRGVmpNR1pFVFhSWmFtc3hUVlJvYUZwcWEzUlpWRWt4VFdrd01FOUhSWGhNVkd4cldrUnJkRnBIVVhkUFZHdDRUa1JSTUU0eVdtaE5VamgzU0ZGWlMwTmFTVzFwV2xCNVRFZFJRa0ZSZDFCTmVrVjNUMFJWTkU1cVNUUk9SRUYzVFVSQmVrMVJNSGREZDFsRVZsRlJUVVJCVVhoTlJFRjNUVk5SZDBsbldVUldVVkZoUkVKMFVHSkhSalZaVTNkblZXMXNOVmxYVW05TVEwSlVXVmhXYTJGVFFrSmpiVVpwWVZkRmVFdEVRVzFDWjA1V1FrRTRUVWc1YVc0eVdWUlpiemx0UnpKTVZGbDBPV2x3U1U1cGJqSlpWRmwxWkcxRE1rdG1XWE5rYlVzeVMydDNRMmRaU1V0dldrbDZhakJGUVhkSlJGSjNRWGRTUVVsblpWUnlURkpRUTFnMVFVbDBiVFo1ZVdvdllqaHphVE5JUkVzM2F6TjRka2d4ZDIxWmRqa3JSRWh1VVVOSlF6bHJOa3hrZFdjNVFsVXpPV2t6YVdaemJVUnNWWE5yVWpOS2FHaHpRU3RhU214QmREQndSVVIwUWc9PTovTHRZUmlycjJYeDgrSWxQKzduZUY2MjJHVlBsWVdOMUlPeXlTbVA1aGN3PQ==");
                        //    // client.DefaultRequestHeaders.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes($"{CsIdResult.binarySecurityToken}:{CsIdResult.secret}")));
                        //    StringContent prodContent = new StringContent(JsonConvert.SerializeObject(new { compliance_request_id = "1708256658302" }), Encoding.UTF8, "application/json");
                        //    HttpResponseMessage prodcsIdresult = client.PostAsync(ZatcaVariable.ProductionGenerateCSID, prodContent).Result;
                        //    string prodstr2 = prodcsIdresult.Content.ReadAsStringAsync().Result;
                        //    if (prodcsIdresult.IsSuccessStatusCode)
                        //    {
                        //        var prodCsIdResult = JsonConvert.DeserializeObject<CsIdResultDto>(prodstr2);

                        //        await Mediator.Send(new UpdateZatcaAPISetting()
                        //        {
                        //            CompanyDto = new()
                        //            {
                        //                Id = Company.Id,
                        //                Csr = csrResult.Csr,
                        //                DevRequestId = "1708256658302",
                        //              //  DevCsid = CsIdResult.binarySecurityToken,
                        //               // DevSecret = CsIdResult.secret,

                        //                ProdRequestId = prodCsIdResult.requestID,
                        //                ProdCsid = prodCsIdResult.binarySecurityToken,
                        //                ProdSecret = prodCsIdResult.secret
                        //            },
                        //            User = UserInfo()
                        //        });

                        //        CsIdResult = new() { requestID = prodCsIdResult.requestID, binarySecurityToken = prodCsIdResult.binarySecurityToken, secret = prodCsIdResult.secret };
                        //    }
                        //    else
                        //        return BadRequest(new ApiMessageDto { Message = prodstr2 });
                        //}
                        //else
                        //    return BadRequest(new ApiMessageDto { Message = "" });


                    }
                }

                catch (Exception ex)
                {
                    return BadRequest(new ApiMessageDto { Message = ex.Message });
                }
            }

            if (true)//CsIdResult is not null && CsIdResult.binarySecurityToken.HasValue())
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
                        // invoice.InvoiceNumber = "8d487816-70b8-4ade-a618-9d620b73814a";
                        xmlData = xmlData
                            .Replace("{{UUID}}", invoice.InvoiceNumber).Replace("{{IssueDate}}", invoice.InvoiceDate.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture))
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

                        var requestGenerator = new RequestGenerator();
                        var requestGeneratorResut = requestGenerator.GenerateRequest(xmlDoc);

                        var envHash = new EInvoiceHashGenerator();
                        var hashResult = envHash.GenerateEInvoiceHashing(xmlDoc);


                        client.BaseAddress = (new Uri(_appSettings.Value.ZatcaApi));
                        client.DefaultRequestHeaders.Accept.Clear();
                        //client.DefaultRequestHeaders.Add("OTP", "123345");
                        client.DefaultRequestHeaders.Add("Accept-Version", "V2");
                        client.DefaultRequestHeaders.Add("Accept-Language", "en");
                        //client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                        //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GlobalVariable.TokenString);

                        client.DefaultRequestHeaders.Add("Authorization", "Basic VFVsSlExbDZRME5CWjNGblFYZEpRa0ZuU1VkQldUSTRRMEZPSzAxQmIwZERRM0ZIVTAwME9VSkJUVU5OUWxWNFJYcEJVa0puVGxaQ1FVMU5RMjFXU21KdVduWmhWMDV3WW0xamQwaG9ZMDVOYWxGM1RXcEZORTFVUlRCT1JFVjZWMmhqVGsxcWEzZE5ha1V6VFdwRmQwMUVRWGRYYWtJMFRWRnpkME5SV1VSV1VWRkhSWGRLVkZGVVJXTk5RbTlIUVRGVlJVTjNkMVF5V1VoWmMyUnBOVWxPYVc0eVdWUlpjMlJ0U3pKTFpsbDBha1Z5VFVOclIwRXhWVVZEWjNkcFVWZDNaMVV5Um05YVdFbG5VVmQ0YUdKWE5YQkpSMXAyWTJsQ1ZGcFhUakZqYld3d1pWTkNTR1JYUm5sYVJFVmxUVUozUjBFeFZVVkJkM2RXVlVaS1JsZHJSbFZSTUVWMFVUSTVhMXBUTVZSaFYyUjFZVmMxYmsxR1dYZEZRVmxJUzI5YVNYcHFNRU5CVVZsR1N6UkZSVUZCYjBSUlowRkZOMGd2VkVWdVprdFRSVFV6TUM5cVoyWlFhRVUxZWxwVU9HYzBjbXRrVlZvdmRFOW9iMWh1ZUM5blpraFVjV001UVRWVFZUSm1VRFpYTDFaV2JEQnVNRFJFSzFwTWFHTnBaRzlET1hZclFrWnhMekZKUlhGUFFqVlVRMEkwYWtGTlFtZE9Wa2hTVFVKQlpqaEZRV3BCUVUxSlNGSkNaMDVXU0ZKRlJXZGphM2RuWTJGcloyTk5kMmRqUVhoUWFrRTRRbWRPVmtKQlVVMU9WRVYwVlVjNWVsUnRSblJhV0hkNVRGVmpNR1pFVFhSWmFtc3hUVlJvYUZwcWEzUlpWRWt4VFdrd01FOUhSWGhNVkd4cldrUnJkRnBIVVhkUFZHdDRUa1JSTUU0eVdtaE5VamgzU0ZGWlMwTmFTVzFwV2xCNVRFZFJRa0ZSZDFCTmVrVjNUMFJWTkU1cVNUUk9SRUYzVFVSQmVrMVJNSGREZDFsRVZsRlJUVVJCVVhoTlJFRjNUVk5SZDBsbldVUldVVkZoUkVKMFVHSkhSalZaVTNkblZXMXNOVmxYVW05TVEwSlVXVmhXYTJGVFFrSmpiVVpwWVZkRmVFdEVRVzFDWjA1V1FrRTRUVWc1YVc0eVdWUlpiemx0UnpKTVZGbDBPV2x3U1U1cGJqSlpWRmwxWkcxRE1rdG1XWE5rYlVzeVMydDNRMmRaU1V0dldrbDZhakJGUVhkSlJGSjNRWGRTUVVsblpWUnlURkpRUTFnMVFVbDBiVFo1ZVdvdllqaHphVE5JUkVzM2F6TjRka2d4ZDIxWmRqa3JSRWh1VVVOSlF6bHJOa3hrZFdjNVFsVXpPV2t6YVdaemJVUnNWWE5yVWpOS2FHaHpRU3RhU214QmREQndSVVIwUWc9PTovTHRZUmlycjJYeDgrSWxQKzduZUY2MjJHVlBsWVdOMUlPeXlTbVA1aGN3PQ==");
                        //client.DefaultRequestHeaders.Add("Authorization", "Basic VFVsSlJsRnFRME5DVDJWblFYZEpRa0ZuU1ZSRmQwRkJUbmxhWTFSS1ZHMVFPV1UwYkVGQlFrRkJRVE5LYWtGTFFtZG5jV2hyYWs5UVVWRkVRV3BDYVUxU1ZYZEZkMWxMUTFwSmJXbGFVSGxNUjFGQ1IxSlpSbUpIT1dwWlYzZDRSWHBCVWtKbmIwcHJhV0ZLYXk5SmMxcEJSVnBHWjA1dVlqTlplRVo2UVZaQ1oyOUthMmxoU21zdlNYTmFRVVZhUm1ka2JHVklVbTVaV0hBd1RWSnpkMGRSV1VSV1VWRkVSWGhLVVZKV2NFWlRWVFZYVkRCc1JGSldUa1JSVkVsMFVUQkZkMGhvWTA1TmFsRjNUV3BGTkUxVVJYcE9SRkYzVjJoalRrMXFXWGROYWtVMFRWUkZNRTVFVVhkWGFrSTBUVkZ6ZDBOUldVUldVVkZIUlhkS1ZGRlVSWEpOUTJ0SFFURlZSVU5vVFdsUlYzZG5WVEpHYjFwWVNXZFJWM2hvWWxjMWNFbEhXblpqYVVKVVdsZE9NV050YkRCbFUwSklaRmRHZVZwRVJXTk5RbTlIUVRGVlJVTjNkMVF5V1VoWmMyUnBOVWxPYVc0eVdWUlpjMlJ0U3pKTFpsbDBha1ZsVFVKM1IwRXhWVVZCZUUxV1ZVWktSbGRyUmxWUk1FVjBVVEk1YTFwVE1WUmhWMlIxWVZjMWJrMUdXWGRGUVZsSVMyOWFTWHBxTUVOQlVWbEdTelJGUlVGQmIwUlJaMEZGTjBndlZFVnVaa3RUUlRVek1DOXFaMlpRYUVVMWVscFVPR2MwY210a1ZWb3ZkRTlvYjFodWVDOW5aa2hVY1dNNVFUVlRWVEptVURaWEwxWldiREJ1TURSRUsxcE1hR05wWkc5RE9YWXJRa1p4THpGSlJYRlBRMEV5WTNkblowNXFUVWxJVWtKblRsWklVa1ZGWjJOcmQyZGpZV3RuWTAxM1oyTkJlRkJxUVRoQ1owNVdRa0ZSVFU1VVJYUlZSemw2VkcxR2RGcFlkM2xNVldNd1prUk5kRmxxYXpGTlZHaG9XbXByZEZsVVNURk5hVEF3VDBkRmVFeFViR3RhUkd0MFdrZFJkMDlVYTNoT1JGRXdUakphYUUxU09IZElVVmxMUTFwSmJXbGFVSGxNUjFGQ1FWRjNVRTE2UlhkUFJGVTBUbXBKTkU1RVFYZE5SRUY2VFZFd2QwTjNXVVJXVVZGTlJFRlJlRTFFUVhkTlUxRjNTV2RaUkZaUlVXRkVRblJRWWtkR05WbFRkMmRWYld3MVdWZFNiMHhEUWxSWldGWnJZVk5DUW1OdFJtbGhWMFY0UzBSQmJVSm5UbFpDUVRoTlNEbHBiakpaVkZsdk9XMUhNa3hVV1hRNWFYQkpUbWx1TWxsVVdYVmtiVU15UzJaWmMyUnRTekpMYTNkSVVWbEVWbEl3VDBKQ1dVVkdSamROTTA5elFXbDBZeTlJV2pGU1QyaE9NbXBwV25oamJpdHNUVUk0UjBFeFZXUkpkMUZaVFVKaFFVWkpTSGx2TTNSNVpUY3hVVzh5Y1dZNFpXcFVhbVJhTjI1SVF6Rk5TVWhzUW1kT1ZraFNPRVZuWkRCM1oyUnZkMmRrWldkblpGTm5aMlJIUjJkak5YTmFSMFozVDJrNGRrd3dUazlRVmtKR1YydFdTbFJzV2xCVFZVNUdWVEJPUWsxcE1VUlJVMmQ0UzFONFJGUnFNVkZWYkhCR1UxVTFWMVF3YkVSU1ZrSk1VMVJKYzFFd05EbFJNRkpSVEVWT1QxQldRakZaYlhod1dYbFZlVTFGZEd4bFUxVjVUVVpPYkdOdVduQlpNbFo2VEVWT1QxQldUbXhqYmxwd1dUSldla3hGVGs5UVZVNTJZbTFhY0ZvelZubFpXRkp3WWpJMGMxSkZUVGxhV0dnd1pXMUdNRmt5UlhOU1JVMDVXakk1TWt4RlVrUlFWM2gyV1RKR2MxQXlUbXhqYmxKd1dtMXNhbGxZVW14VmJWWXlZakpPYUdSSGJIWmlhM2h3WXpOUkwxbHRSbnBhVkRsMldXMXdiRmt6VWtSaVIwWjZZM294YWxWcmVFVmhXRTR3WTIxc2FXUllVbkJpTWpWUllqSnNkV1JFUTBKNloxbEpTM2RaUWtKUlZVaEJVVVZGWjJORmQyZGlOSGRuWW5OSFEwTnpSMEZSVlVaQ2VrRkRhRzlIZFdKSFVtaGpSRzkyVEhrNVJGUnFNVkZTVm5CR1UxVTFWMVF3YkVSU1ZrNUVVVlJKZEZFd1JYTlJNRFE1VVZWc1FreEZUazlRVmtJeFdXMTRjRmw1VlhsTlJYUnNaVk5WZVUxR1RteGpibHB3V1RKV2VreEZUazlRVms1c1kyNWFjRmt5Vm5wTVJVNVBVRlZPZG1KdFduQmFNMVo1V1ZoU2NHSXlOSE5TUlUwNVdsaG9NR1Z0UmpCWk1rVnpVa1ZOT1ZveU9USk1SVkpFVUZkNGRsa3lSbk5RTWs1Q1VUSldlV1JIYkcxaFYwNW9aRWRWTDFsdFJucGFWRGwyV1cxd2JGa3pVa1JpUjBaNlkzb3hhbHBZU2pCaFYxcHdXVEpHTUdGWE9YVlJXRll3WVVjNWVXRllValZOUVRSSFFURlZaRVIzUlVJdmQxRkZRWGRKU0dkRVFUaENaMnR5UW1kRlJVRlpTVE5HVVdORlRIcEJkRUpwVlhKQ1owVkZRVmxKTTBaUmFVSm9jV2RrYUU1RU4wVnZZblJ1VTFOSWVuWnpXakE0UWxaYWIwZGpNa015UkRWalZtUkJaMFpyUVdkRlVVMUNNRWRCTVZWa1NsRlJWMDFDVVVkRFEzTkhRVkZWUmtKM1RVTkNaMmR5UW1kRlJrSlJZMFJCZWtGdVFtZHJja0puUlVWQldVa3pSbEZ2UlVkcVFWbE5RVzlIUTBOelIwRlJWVVpDZDAxRFRVRnZSME5EYzBkQlVWVkdRbmROUkUxQmIwZERRM0ZIVTAwME9VSkJUVU5CTUd0QlRVVlpRMGxSUTNFelUzVXJTRU5KVTJOdmJFOUdkbmQ2WVRNdk5VZDJRVXh0YjFaU09IWndjRFp6WTNSa1J5OU9UVkZKYUVGTFVIUXdOVGRKWVhkNGJteEdPV1kxVEc4MFJsUXplRE15ZUZGYVdISnhRbTQzT0V4ME9HY3dOVUZTOkdkNFEzR3UzczJkSE4zRGhYN2xaV2dHbFQ4YTV6Q3oxcTNTa05mUCsvd3M9");
                        // client.DefaultRequestHeaders.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes($"{CsIdResult.binarySecurityToken}:{CsIdResult.secret}")));
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
        static char ConvertUnicodeStringToAscii(int value) => Microsoft.VisualBasic.Strings.Chr(value);

        //For Live or Simulation Multi_XML Files
        [HttpGet("complianceMultiCSID")]
        public async Task<ActionResult> GenerateComplianceMultiCSID([FromQuery] int companyId, [FromQuery] int invoiceId, [FromQuery] string otp)
        {
            //invoiceId = 20080;
            var Company = await Mediator.Send(new GetCompany() { Id = companyId, User = UserInfo() });
            string csr, requestId, csid, secret;
            CsIdResultDto CsIdResult;


            var csrGenerator = new CsrGenerator();

            //var csrResult = csrGenerator.GenerateCsr(new CsrGenerationDto(
            //   "ls",
            //  "1-TST|2-TST|3-ed22f1d8-e6a2-1118-9b58-d9a8f11e445f",
            //  Company.VATNumber,
            //  "IT Department",
            //  Company.CompanyName,
            //  "SA",
            //  "1000",
            //  Company.CompanyAddress,
            //  "IT"
            //  ), EnvironmentType.Simulation);

            CsrResult csrResult = new()
            {
                Csr = _appSettings.Value.Certificate,
                PrivateKey = _appSettings.Value.PrivateKey
            };

            //  byte[] b = System.Convert.FromBase64String(csrResult.Csr);
            //string dbConnetion = System.Text.ASCIIEncoding.ASCII.GetString(b);
            //  string dbConnetion = System.Text.ASCIIEncoding.UTF8.GetString(b);

            // csrResult.Csr = dbConnetion;
            //XDocument document = XDocument.Load("STANDARD_NoSign.xml");
            int count = 0;
            List<string> invoices = new() { "Standard_Invoice.xml", "Standard_Credit.xml", "Standard_Debit.xml" };
            foreach (var invFile in invoices)
            {
                try
                {
                    XDocument document = XDocument.Load(invFile);
                    //XDocument document = XDocument.Load("Standard_Invoice.xml");
                    XmlDocument xmlDoc = new XmlDocument();
                    string xmlData = document.ToString();

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

                    //  var csrResult = csrGenerator.GenerateCsr(new CsrGenerationDto(
                    //     "AM",
                    //    "1-TST|2-TST|3-515851ae-8e07-4167-81c6-497565aecb6a",
                    //    "310054439500003",
                    //    "فرع ",
                    //    "AMSteel - Port & Steel Operations",
                    //    "SA",
                    //    "1000",
                    //    "Prince Sultan Jeddah",
                    //    "test"
                    //), EnvironmentType.Simulation);

                    using (HttpClient client = new HttpClient())
                    {
                        client.BaseAddress = (new Uri(_appSettings.Value.ZatcaApi));
                        client.DefaultRequestHeaders.Accept.Clear();
                        //client.DefaultRequestHeaders.Add("OTP", "123345");
                        client.DefaultRequestHeaders.Add("Accept-Version", "V2");
                        client.DefaultRequestHeaders.Add("Accept-Language", "en");
                        client.DefaultRequestHeaders.Add("Clearance-Status", "1");
                        //client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                        //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GlobalVariable.TokenString);
                        var authKey = "VFVsSlJsRnFRME5DVDJWblFYZEpRa0ZuU1ZSRmQwRkJUbmxhWTFSS1ZHMVFPV1UwYkVGQlFrRkJRVE5LYWtGTFFtZG5jV2hyYWs5UVVWRkVRV3BDYVUxU1ZYZEZkMWxMUTFwSmJXbGFVSGxNUjFGQ1IxSlpSbUpIT1dwWlYzZDRSWHBCVWtKbmIwcHJhV0ZLYXk5SmMxcEJSVnBHWjA1dVlqTlplRVo2UVZaQ1oyOUthMmxoU21zdlNYTmFRVVZhUm1ka2JHVklVbTVaV0hBd1RWSnpkMGRSV1VSV1VWRkVSWGhLVVZKV2NFWlRWVFZYVkRCc1JGSldUa1JSVkVsMFVUQkZkMGhvWTA1TmFsRjNUV3BGTkUxVVJYcE9SRkYzVjJoalRrMXFXWGROYWtVMFRWUkZNRTVFVVhkWGFrSTBUVkZ6ZDBOUldVUldVVkZIUlhkS1ZGRlVSWEpOUTJ0SFFURlZSVU5vVFdsUlYzZG5WVEpHYjFwWVNXZFJWM2hvWWxjMWNFbEhXblpqYVVKVVdsZE9NV050YkRCbFUwSklaRmRHZVZwRVJXTk5RbTlIUVRGVlJVTjNkMVF5V1VoWmMyUnBOVWxPYVc0eVdWUlpjMlJ0U3pKTFpsbDBha1ZsVFVKM1IwRXhWVVZCZUUxV1ZVWktSbGRyUmxWUk1FVjBVVEk1YTFwVE1WUmhWMlIxWVZjMWJrMUdXWGRGUVZsSVMyOWFTWHBxTUVOQlVWbEdTelJGUlVGQmIwUlJaMEZGTjBndlZFVnVaa3RUUlRVek1DOXFaMlpRYUVVMWVscFVPR2MwY210a1ZWb3ZkRTlvYjFodWVDOW5aa2hVY1dNNVFUVlRWVEptVURaWEwxWldiREJ1TURSRUsxcE1hR05wWkc5RE9YWXJRa1p4THpGSlJYRlBRMEV5WTNkblowNXFUVWxJVWtKblRsWklVa1ZGWjJOcmQyZGpZV3RuWTAxM1oyTkJlRkJxUVRoQ1owNVdRa0ZSVFU1VVJYUlZSemw2VkcxR2RGcFlkM2xNVldNd1prUk5kRmxxYXpGTlZHaG9XbXByZEZsVVNURk5hVEF3VDBkRmVFeFViR3RhUkd0MFdrZFJkMDlVYTNoT1JGRXdUakphYUUxU09IZElVVmxMUTFwSmJXbGFVSGxNUjFGQ1FWRjNVRTE2UlhkUFJGVTBUbXBKTkU1RVFYZE5SRUY2VFZFd2QwTjNXVVJXVVZGTlJFRlJlRTFFUVhkTlUxRjNTV2RaUkZaUlVXRkVRblJRWWtkR05WbFRkMmRWYld3MVdWZFNiMHhEUWxSWldGWnJZVk5DUW1OdFJtbGhWMFY0UzBSQmJVSm5UbFpDUVRoTlNEbHBiakpaVkZsdk9XMUhNa3hVV1hRNWFYQkpUbWx1TWxsVVdYVmtiVU15UzJaWmMyUnRTekpMYTNkSVVWbEVWbEl3VDBKQ1dVVkdSamROTTA5elFXbDBZeTlJV2pGU1QyaE9NbXBwV25oamJpdHNUVUk0UjBFeFZXUkpkMUZaVFVKaFFVWkpTSGx2TTNSNVpUY3hVVzh5Y1dZNFpXcFVhbVJhTjI1SVF6Rk5TVWhzUW1kT1ZraFNPRVZuWkRCM1oyUnZkMmRrWldkblpGTm5aMlJIUjJkak5YTmFSMFozVDJrNGRrd3dUazlRVmtKR1YydFdTbFJzV2xCVFZVNUdWVEJPUWsxcE1VUlJVMmQ0UzFONFJGUnFNVkZWYkhCR1UxVTFWMVF3YkVSU1ZrSk1VMVJKYzFFd05EbFJNRkpSVEVWT1QxQldRakZaYlhod1dYbFZlVTFGZEd4bFUxVjVUVVpPYkdOdVduQlpNbFo2VEVWT1QxQldUbXhqYmxwd1dUSldla3hGVGs5UVZVNTJZbTFhY0ZvelZubFpXRkp3WWpJMGMxSkZUVGxhV0dnd1pXMUdNRmt5UlhOU1JVMDVXakk1TWt4RlVrUlFWM2gyV1RKR2MxQXlUbXhqYmxKd1dtMXNhbGxZVW14VmJWWXlZakpPYUdSSGJIWmlhM2h3WXpOUkwxbHRSbnBhVkRsMldXMXdiRmt6VWtSaVIwWjZZM294YWxWcmVFVmhXRTR3WTIxc2FXUllVbkJpTWpWUllqSnNkV1JFUTBKNloxbEpTM2RaUWtKUlZVaEJVVVZGWjJORmQyZGlOSGRuWW5OSFEwTnpSMEZSVlVaQ2VrRkRhRzlIZFdKSFVtaGpSRzkyVEhrNVJGUnFNVkZTVm5CR1UxVTFWMVF3YkVSU1ZrNUVVVlJKZEZFd1JYTlJNRFE1VVZWc1FreEZUazlRVmtJeFdXMTRjRmw1VlhsTlJYUnNaVk5WZVUxR1RteGpibHB3V1RKV2VreEZUazlRVms1c1kyNWFjRmt5Vm5wTVJVNVBVRlZPZG1KdFduQmFNMVo1V1ZoU2NHSXlOSE5TUlUwNVdsaG9NR1Z0UmpCWk1rVnpVa1ZOT1ZveU9USk1SVkpFVUZkNGRsa3lSbk5RTWs1Q1VUSldlV1JIYkcxaFYwNW9aRWRWTDFsdFJucGFWRGwyV1cxd2JGa3pVa1JpUjBaNlkzb3hhbHBZU2pCaFYxcHdXVEpHTUdGWE9YVlJXRll3WVVjNWVXRllValZOUVRSSFFURlZaRVIzUlVJdmQxRkZRWGRKU0dkRVFUaENaMnR5UW1kRlJVRlpTVE5HVVdORlRIcEJkRUpwVlhKQ1owVkZRVmxKTTBaUmFVSm9jV2RrYUU1RU4wVnZZblJ1VTFOSWVuWnpXakE0UWxaYWIwZGpNa015UkRWalZtUkJaMFpyUVdkRlVVMUNNRWRCTVZWa1NsRlJWMDFDVVVkRFEzTkhRVkZWUmtKM1RVTkNaMmR5UW1kRlJrSlJZMFJCZWtGdVFtZHJja0puUlVWQldVa3pSbEZ2UlVkcVFWbE5RVzlIUTBOelIwRlJWVVpDZDAxRFRVRnZSME5EYzBkQlVWVkdRbmROUkUxQmIwZERRM0ZIVTAwME9VSkJUVU5CTUd0QlRVVlpRMGxSUTNFelUzVXJTRU5KVTJOdmJFOUdkbmQ2WVRNdk5VZDJRVXh0YjFaU09IWndjRFp6WTNSa1J5OU9UVkZKYUVGTFVIUXdOVGRKWVhkNGJteEdPV1kxVEc4MFJsUXplRE15ZUZGYVdISnhRbTQzT0V4ME9HY3dOVUZTOkdkNFEzR3UzczJkSE4zRGhYN2xaV2dHbFQ4YTV6Q3oxcTNTa05mUCsvd3M9";
                        client.DefaultRequestHeaders.Add("Authorization", $"Basic {authKey}");
                        //client.DefaultRequestHeaders.Add("Authorization", $"Basic {_appSettings.Value.Authorization}");
                        //client.DefaultRequestHeaders.Add("Authorization", "Basic VFVsSlJsRnFRME5DVDJWblFYZEpRa0ZuU1ZSRmQwRkJUbmxhWTFSS1ZHMVFPV1UwYkVGQlFrRkJRVE5LYWtGTFFtZG5jV2hyYWs5UVVWRkVRV3BDYVUxU1ZYZEZkMWxMUTFwSmJXbGFVSGxNUjFGQ1IxSlpSbUpIT1dwWlYzZDRSWHBCVWtKbmIwcHJhV0ZLYXk5SmMxcEJSVnBHWjA1dVlqTlplRVo2UVZaQ1oyOUthMmxoU21zdlNYTmFRVVZhUm1ka2JHVklVbTVaV0hBd1RWSnpkMGRSV1VSV1VWRkVSWGhLVVZKV2NFWlRWVFZYVkRCc1JGSldUa1JSVkVsMFVUQkZkMGhvWTA1TmFsRjNUV3BGTkUxVVJYcE9SRkYzVjJoalRrMXFXWGROYWtVMFRWUkZNRTVFVVhkWGFrSTBUVkZ6ZDBOUldVUldVVkZIUlhkS1ZGRlVSWEpOUTJ0SFFURlZSVU5vVFdsUlYzZG5WVEpHYjFwWVNXZFJWM2hvWWxjMWNFbEhXblpqYVVKVVdsZE9NV050YkRCbFUwSklaRmRHZVZwRVJXTk5RbTlIUVRGVlJVTjNkMVF5V1VoWmMyUnBOVWxPYVc0eVdWUlpjMlJ0U3pKTFpsbDBha1ZsVFVKM1IwRXhWVVZCZUUxV1ZVWktSbGRyUmxWUk1FVjBVVEk1YTFwVE1WUmhWMlIxWVZjMWJrMUdXWGRGUVZsSVMyOWFTWHBxTUVOQlVWbEdTelJGUlVGQmIwUlJaMEZGTjBndlZFVnVaa3RUUlRVek1DOXFaMlpRYUVVMWVscFVPR2MwY210a1ZWb3ZkRTlvYjFodWVDOW5aa2hVY1dNNVFUVlRWVEptVURaWEwxWldiREJ1TURSRUsxcE1hR05wWkc5RE9YWXJRa1p4THpGSlJYRlBRMEV5WTNkblowNXFUVWxJVWtKblRsWklVa1ZGWjJOcmQyZGpZV3RuWTAxM1oyTkJlRkJxUVRoQ1owNVdRa0ZSVFU1VVJYUlZSemw2VkcxR2RGcFlkM2xNVldNd1prUk5kRmxxYXpGTlZHaG9XbXByZEZsVVNURk5hVEF3VDBkRmVFeFViR3RhUkd0MFdrZFJkMDlVYTNoT1JGRXdUakphYUUxU09IZElVVmxMUTFwSmJXbGFVSGxNUjFGQ1FWRjNVRTE2UlhkUFJGVTBUbXBKTkU1RVFYZE5SRUY2VFZFd2QwTjNXVVJXVVZGTlJFRlJlRTFFUVhkTlUxRjNTV2RaUkZaUlVXRkVRblJRWWtkR05WbFRkMmRWYld3MVdWZFNiMHhEUWxSWldGWnJZVk5DUW1OdFJtbGhWMFY0UzBSQmJVSm5UbFpDUVRoTlNEbHBiakpaVkZsdk9XMUhNa3hVV1hRNWFYQkpUbWx1TWxsVVdYVmtiVU15UzJaWmMyUnRTekpMYTNkSVVWbEVWbEl3VDBKQ1dVVkdSamROTTA5elFXbDBZeTlJV2pGU1QyaE9NbXBwV25oamJpdHNUVUk0UjBFeFZXUkpkMUZaVFVKaFFVWkpTSGx2TTNSNVpUY3hVVzh5Y1dZNFpXcFVhbVJhTjI1SVF6Rk5TVWhzUW1kT1ZraFNPRVZuWkRCM1oyUnZkMmRrWldkblpGTm5aMlJIUjJkak5YTmFSMFozVDJrNGRrd3dUazlRVmtKR1YydFdTbFJzV2xCVFZVNUdWVEJPUWsxcE1VUlJVMmQ0UzFONFJGUnFNVkZWYkhCR1UxVTFWMVF3YkVSU1ZrSk1VMVJKYzFFd05EbFJNRkpSVEVWT1QxQldRakZaYlhod1dYbFZlVTFGZEd4bFUxVjVUVVpPYkdOdVduQlpNbFo2VEVWT1QxQldUbXhqYmxwd1dUSldla3hGVGs5UVZVNTJZbTFhY0ZvelZubFpXRkp3WWpJMGMxSkZUVGxhV0dnd1pXMUdNRmt5UlhOU1JVMDVXakk1TWt4RlVrUlFWM2gyV1RKR2MxQXlUbXhqYmxKd1dtMXNhbGxZVW14VmJWWXlZakpPYUdSSGJIWmlhM2h3WXpOUkwxbHRSbnBhVkRsMldXMXdiRmt6VWtSaVIwWjZZM294YWxWcmVFVmhXRTR3WTIxc2FXUllVbkJpTWpWUllqSnNkV1JFUTBKNloxbEpTM2RaUWtKUlZVaEJVVVZGWjJORmQyZGlOSGRuWW5OSFEwTnpSMEZSVlVaQ2VrRkRhRzlIZFdKSFVtaGpSRzkyVEhrNVJGUnFNVkZTVm5CR1UxVTFWMVF3YkVSU1ZrNUVVVlJKZEZFd1JYTlJNRFE1VVZWc1FreEZUazlRVmtJeFdXMTRjRmw1VlhsTlJYUnNaVk5WZVUxR1RteGpibHB3V1RKV2VreEZUazlRVms1c1kyNWFjRmt5Vm5wTVJVNVBVRlZPZG1KdFduQmFNMVo1V1ZoU2NHSXlOSE5TUlUwNVdsaG9NR1Z0UmpCWk1rVnpVa1ZOT1ZveU9USk1SVkpFVUZkNGRsa3lSbk5RTWs1Q1VUSldlV1JIYkcxaFYwNW9aRWRWTDFsdFJucGFWRGwyV1cxd2JGa3pVa1JpUjBaNlkzb3hhbHBZU2pCaFYxcHdXVEpHTUdGWE9YVlJXRll3WVVjNWVXRllValZOUVRSSFFURlZaRVIzUlVJdmQxRkZRWGRKU0dkRVFUaENaMnR5UW1kRlJVRlpTVE5HVVdORlRIcEJkRUpwVlhKQ1owVkZRVmxKTTBaUmFVSm9jV2RrYUU1RU4wVnZZblJ1VTFOSWVuWnpXakE0UWxaYWIwZGpNa015UkRWalZtUkJaMFpyUVdkRlVVMUNNRWRCTVZWa1NsRlJWMDFDVVVkRFEzTkhRVkZWUmtKM1RVTkNaMmR5UW1kRlJrSlJZMFJCZWtGdVFtZHJja0puUlVWQldVa3pSbEZ2UlVkcVFWbE5RVzlIUTBOelIwRlJWVVpDZDAxRFRVRnZSME5EYzBkQlVWVkdRbmROUkUxQmIwZERRM0ZIVTAwME9VSkJUVU5CTUd0QlRVVlpRMGxSUTNFelUzVXJTRU5KVTJOdmJFOUdkbmQ2WVRNdk5VZDJRVXh0YjFaU09IWndjRFp6WTNSa1J5OU9UVkZKYUVGTFVIUXdOVGRKWVhkNGJteEdPV1kxVEc4MFJsUXplRE15ZUZGYVdISnhRbTQzT0V4ME9HY3dOVUZTOkdkNFEzR3UzczJkSE4zRGhYN2xaV2dHbFQ4YTV6Q3oxcTNTa05mUCsvd3M9");
                        // client.DefaultRequestHeaders.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes($"{CsIdResult.binarySecurityToken}:{CsIdResult.secret}")));
                        StringContent content = new StringContent(JsonConvert.SerializeObject(new { invoiceHash = hashResult.Hash, uuid = "8d487816-70b8-4ade-a618-9d620b73814a", invoice = requestGeneratorResut.InvoiceRequest.Invoice }), Encoding.UTF8, "application/json");

                        HttpResponseMessage csIdresult1 = client.PostAsync("invoices/clearance/single", content).Result;
                        string clearanceRes1 = csIdresult1.Content.ReadAsStringAsync().Result;
                        if (csIdresult1.IsSuccessStatusCode)
                        {

                        }

                        client.DefaultRequestHeaders.Remove("Clearance-Status");
                        client.DefaultRequestHeaders.Remove("Authorization");
                        client.DefaultRequestHeaders.Add("Authorization", $"Basic {_appSettings.Value.Authorization}");
                        HttpResponseMessage csIdresult = client.PostAsync(ZatcaVariable.ComplianceInvoice, content).Result;

                        string clearanceRes = csIdresult.Content.ReadAsStringAsync().Result;
                        if (csIdresult.IsSuccessStatusCode)
                        {
                            count++;

                            bool status = clearanceRes.ToLower().Contains(":\"cleared\",");
                            if (status)
                            {
                                await Mediator.Send(new UpdateZatcaClearanceStatus() { Id = invoiceId, Status = status });
                                if (count == 3)
                                    return Ok(new ApiMessageDto { Message = clearanceRes });
                            }
                            else
                                return BadRequest(new ApiMessageDto { Message = clearanceRes });
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

            //if (count == 3)
            //{
            //    using (HttpClient client = new HttpClient())
            //    {
            //        client.BaseAddress = (new Uri(_appSettings.Value.ZatcaApi));
            //        client.DefaultRequestHeaders.Accept.Clear();
            //        //client.DefaultRequestHeaders.Add("OTP", "123345");
            //        client.DefaultRequestHeaders.Add("Accept-Version", "V2");
            //        client.DefaultRequestHeaders.Add("Accept-Language", "en");

            //        var binarySecurityToken = Convert.ToBase64String(Encoding.ASCII.GetBytes($"TUlJRlFqQ0NCT2VnQXdJQkFnSVRFd0FBTnlaY1RKVG1QOWU0bEFBQkFBQTNKakFLQmdncWhrak9QUVFEQWpCaU1SVXdFd1lLQ1pJbWlaUHlMR1FCR1JZRmJHOWpZV3d4RXpBUkJnb0praWFKay9Jc1pBRVpGZ05uYjNZeEZ6QVZCZ29Ka2lhSmsvSXNaQUVaRmdkbGVIUm5ZWHAwTVJzd0dRWURWUVFERXhKUVJWcEZTVTVXVDBsRFJWTkRRVEl0UTBFd0hoY05NalF3TWpFNE1URXpORFF3V2hjTk1qWXdNakU0TVRFME5EUXdXakI0TVFzd0NRWURWUVFHRXdKVFFURXJNQ2tHQTFVRUNoTWlRV3dnVTJGb1pYSWdRV3hoYlc1cElHWnZjaUJUWldOMWNtbDBlU0JIZFdGeVpERWNNQm9HQTFVRUN3d1QyWUhZc2RpNUlOaW4yWVRZc2RtSzJLZll0akVlTUJ3R0ExVUVBeE1WVUZKRldrRlVRMEV0UTI5a1pTMVRhV2R1YVc1bk1GWXdFQVlIS29aSXpqMENBUVlGSzRFRUFBb0RRZ0FFN0gvVEVuZktTRTUzMC9qZ2ZQaEU1elpUOGc0cmtkVVovdE9ob1hueC9nZkhUcWM5QTVTVTJmUDZXL1ZWbDBuMDREK1pMaGNpZG9DOXYrQkZxLzFJRXFPQ0EyY3dnZ05qTUlIUkJnTlZIUkVFZ2Nrd2djYWtnY013Z2NBeFBqQThCZ05WQkFRTU5URXRVRzl6VG1GdFpYd3lMVWMwZkRNdFlqazFNVGhoWmprdFlUSTFNaTAwT0dFeExUbGtaRGt0WkdRd09Ua3hORFEwTjJaaE1SOHdIUVlLQ1pJbWlaUHlMR1FCQVF3UE16RXdPRFU0TmpJNE5EQXdNREF6TVEwd0N3WURWUVFNREFReE1EQXdNU1F3SWdZRFZRUWFEQnRQYkdGNVlTd2dVbWw1WVdSb0xDQlRZWFZrYVNCQmNtRmlhV0V4S0RBbUJnTlZCQThNSDlpbjJZVFlvOW1HMkxUWXQ5aXBJTmluMllUWXVkbUMyS2ZZc2RtSzJLa3dIUVlEVlIwT0JCWUVGRjdNM09zQWl0Yy9IWjFST2hOMmppWnhjbitsTUI4R0ExVWRJd1FZTUJhQUZJSHlvM3R5ZTcxUW8ycWY4ZWpUamRaN25IQzFNSUhsQmdOVkhSOEVnZDB3Z2Rvd2dkZWdnZFNnZ2RHR2djNXNaR0Z3T2k4dkwwTk9QVkJGV2tWSlRsWlBTVU5GVTBOQk1pMURRU2d4S1N4RFRqMVFVbHBGU1U1V1QwbERSVkJMU1RJc1EwNDlRMFJRTEVOT1BWQjFZbXhwWXlVeU1FdGxlU1V5TUZObGNuWnBZMlZ6TEVOT1BWTmxjblpwWTJWekxFTk9QVU52Ym1acFozVnlZWFJwYjI0c1JFTTlaWGgwZW1GMFkyRXNSRU05WjI5MkxFUkRQV3h2WTJGc1AyTmxjblJwWm1sallYUmxVbVYyYjJOaGRHbHZia3hwYzNRL1ltRnpaVDl2WW1wbFkzUkRiR0Z6Y3oxalVreEVhWE4wY21saWRYUnBiMjVRYjJsdWREQ0J6Z1lJS3dZQkJRVUhBUUVFZ2NFd2diNHdnYnNHQ0NzR0FRVUZCekFDaG9HdWJHUmhjRG92THk5RFRqMVFSVnBGU1U1V1QwbERSVk5EUVRJdFEwRXNRMDQ5UVVsQkxFTk9QVkIxWW14cFl5VXlNRXRsZVNVeU1GTmxjblpwWTJWekxFTk9QVk5sY25acFkyVnpMRU5PUFVOdmJtWnBaM1Z5WVhScGIyNHNSRU05WlhoMGVtRjBZMkVzUkVNOVoyOTJMRVJEUFd4dlkyRnNQMk5CUTJWeWRHbG1hV05oZEdVL1ltRnpaVDl2WW1wbFkzUkRiR0Z6Y3oxalpYSjBhV1pwWTJGMGFXOXVRWFYwYUc5eWFYUjVNQTRHQTFVZER3RUIvd1FFQXdJSGdEQThCZ2tyQmdFRUFZSTNGUWNFTHpBdEJpVXJCZ0VFQVlJM0ZRaUJocWdkaE5EN0VvYnRuU1NIenZzWjA4QlZab0djMkMyRDVjVmRBZ0ZrQWdFUU1CMEdBMVVkSlFRV01CUUdDQ3NHQVFVRkJ3TUNCZ2dyQmdFRkJRY0RBekFuQmdrckJnRUVBWUkzRlFvRUdqQVlNQW9HQ0NzR0FRVUZCd01DTUFvR0NDc0dBUVVGQndNRE1Bb0dDQ3FHU000OUJBTUNBMGtBTUVZQ0lRQ3EzU3UrSENJU2NvbE9Gdnd6YTMvNUd2QUxtb1ZSOHZwcDZzY3RkRy9OTVFJaEFLUHQwNTdJYXd4bmxGOWY1TG80RlQzeDMyeFFaWHJxQm43OEx0OGcwNUFS:Gd4Q3Gu3s2dHN3DhX7lZWgGlT8a5zCz1q3SkNfP+/ws="));
            //        //var binarySecurityToken = "MIIFQjCCBOegAwIBAgITEwAANyZcTJTmP9e4lAABAAA3JjAKBggqhkjOPQQDAjBiMRUwEwYKCZImiZPyLGQBGRYFbG9jYWwxEzARBgoJkiaJk/IsZAEZFgNnb3YxFzAVBgoJkiaJk/IsZAEZFgdleHRnYXp0MRswGQYDVQQDExJQRVpFSU5WT0lDRVNDQTItQ0EwHhcNMjQwMjE4MTEzNDQwWhcNMjYwMjE4MTE0NDQwWjB4MQswCQYDVQQGEwJTQTErMCkGA1UEChMiQWwgU2FoZXIgQWxhbW5pIGZvciBTZWN1cml0eSBHdWFyZDEcMBoGA1UECwwT2YHYsdi5INin2YTYsdmK2KfYtjEeMBwGA1UEAxMVUFJFWkFUQ0EtQ29kZS1TaWduaW5nMFYwEAYHKoZIzj0CAQYFK4EEAAoDQgAE7H/TEnfKSE530/jgfPhE5zZT8g4rkdUZ/tOhoXnx/gfHTqc9A5SU2fP6W/VVl0n04D+ZLhcidoC9v+BFq/1IEqOCA2cwggNjMIHRBgNVHREEgckwgcakgcMwgcAxPjA8BgNVBAQMNTEtUG9zTmFtZXwyLUc0fDMtYjk1MThhZjktYTI1Mi00OGExLTlkZDktZGQwOTkxNDQ0N2ZhMR8wHQYKCZImiZPyLGQBAQwPMzEwODU4NjI4NDAwMDAzMQ0wCwYDVQQMDAQxMDAwMSQwIgYDVQQaDBtPbGF5YSwgUml5YWRoLCBTYXVkaSBBcmFiaWExKDAmBgNVBA8MH9in2YTYo9mG2LTYt9ipINin2YTYudmC2KfYsdmK2KkwHQYDVR0OBBYEFF7M3OsAitc/HZ1ROhN2jiZxcn+lMB8GA1UdIwQYMBaAFIHyo3tye71Qo2qf8ejTjdZ7nHC1MIHlBgNVHR8Egd0wgdowgdeggdSggdGGgc5sZGFwOi8vL0NOPVBFWkVJTlZPSUNFU0NBMi1DQSgxKSxDTj1QUlpFSU5WT0lDRVBLSTIsQ049Q0RQLENOPVB1YmxpYyUyMEtleSUyMFNlcnZpY2VzLENOPVNlcnZpY2VzLENOPUNvbmZpZ3VyYXRpb24sREM9ZXh0emF0Y2EsREM9Z292LERDPWxvY2FsP2NlcnRpZmljYXRlUmV2b2NhdGlvbkxpc3Q/YmFzZT9vYmplY3RDbGFzcz1jUkxEaXN0cmlidXRpb25Qb2ludDCBzgYIKwYBBQUHAQEEgcEwgb4wgbsGCCsGAQUFBzAChoGubGRhcDovLy9DTj1QRVpFSU5WT0lDRVNDQTItQ0EsQ049QUlBLENOPVB1YmxpYyUyMEtleSUyMFNlcnZpY2VzLENOPVNlcnZpY2VzLENOPUNvbmZpZ3VyYXRpb24sREM9ZXh0emF0Y2EsREM9Z292LERDPWxvY2FsP2NBQ2VydGlmaWNhdGU/YmFzZT9vYmplY3RDbGFzcz1jZXJ0aWZpY2F0aW9uQXV0aG9yaXR5MA4GA1UdDwEB/wQEAwIHgDA8BgkrBgEEAYI3FQcELzAtBiUrBgEEAYI3FQiBhqgdhND7EobtnSSHzvsZ08BVZoGc2C2D5cVdAgFkAgEQMB0GA1UdJQQWMBQGCCsGAQUFBwMCBggrBgEFBQcDAzAnBgkrBgEEAYI3FQoEGjAYMAoGCCsGAQUFBwMCMAoGCCsGAQUFBwMDMAoGCCqGSM49BAMCA0kAMEYCIQCq3Su+HCIScolOFvwza3/5GvALmoVR8vpp6sctdG/NMQIhAKPt057IawxnlF9f5Lo4FT3x32xQZXrqBn78Lt8g05AR\u0019\u0010kgG7p_YZ\u0001Oƹ,t5\v";
            //        //var requestID = 1708256658302;
            //        //var requestID = 14118100749;

            //        client.DefaultRequestHeaders.Add("Authorization", $"Basic {binarySecurityToken}");
            //        // StringContent prodContent = new StringContent(JsonConvert.SerializeObject(new { compliance_request_id = requestID }), Encoding.UTF8, "application/json");
            //        HttpResponseMessage prodcsIdresult = client.GetAsync("Reporting/Clerance/").Result;
            //        string prodstr2 = prodcsIdresult.Content.ReadAsStringAsync().Result;
            //        if (prodcsIdresult.IsSuccessStatusCode)
            //        {

            //        }
            //    }
            //}

            return BadRequest(new ApiMessageDto { Message = "failed" });
        }


        //For Zatca Third Party Service Live or Simulation
        [HttpPost("zatcacompliance")]
        public async Task<ActionResult> GenerateComplianceCSID([FromBody] ZatcaInvoiceDto input)
        {
            await Task.Delay(0);
            //invoiceId = 20080;
            var Company = input;
           // string csr, requestId, csid, secret;
           // CsIdResultDto CsIdResult;


            var csrGenerator = new CsrGenerator();

            //var csrResult = csrGenerator.GenerateCsr(new CsrGenerationDto(
            //   "ls",
            //  "1-TST|2-TST|3-ed22f1d8-e6a2-1118-9b58-d9a8f11e445f",
            //  Company.VATNumber,
            //  "IT Department",
            //  Company.CompanyName,
            //  "SA",
            //  "1000",
            //  Company.CompanyAddress,
            //  "IT"
            //  ), EnvironmentType.Simulation);

            CsrResult csrResult = new()
            {
                Csr = input.Certificate,
                PrivateKey = input.PrivateKey
            };

            //  byte[] b = System.Convert.FromBase64String(csrResult.Csr);
            //string dbConnetion = System.Text.ASCIIEncoding.ASCII.GetString(b);
            //  string dbConnetion = System.Text.ASCIIEncoding.UTF8.GetString(b);

            // csrResult.Csr = dbConnetion;
            //XDocument document = XDocument.Load("STANDARD_NoSign.xml");
            
            XDocument document = XDocument.Load("Standard_Invoice_old.xml");
            XmlDocument xmlDoc = new XmlDocument();
            string xmlData = document.ToString();

            var invoice = input;

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
            int itemId = 1;
            foreach (var item in input.Lines)
            {
                taxSubtotal += $"\r\n<cac:TaxSubtotal>\r\n            <cbc:TaxableAmount currencyID=\"SAR\">{(item.Quantity * item.UnitPrice).TwoDecInvarient()}</cbc:TaxableAmount>\r\n            <cbc:TaxAmount currencyID=\"SAR\">{item.TaxAmount.TwoDecInvarient()}</cbc:TaxAmount>\r\n            <cac:TaxCategory>\r\n                <cbc:ID schemeID=\"UN/ECE 5305\" schemeAgencyID=\"6\">S</cbc:ID>\r\n                <cbc:Percent>{item.TaxTariffPercentage.TwoDecInvarient()}</cbc:Percent>\r\n                <cac:TaxScheme>\r\n                    <cbc:ID schemeID=\"UN/ECE 5153\" schemeAgencyID=\"6\">VAT</cbc:ID>\r\n                </cac:TaxScheme>\r\n            </cac:TaxCategory>\r\n        </cac:TaxSubtotal>";
                //if (item.TaxTariffPercentage is not null && item.TaxTariffPercentage > 0 && totalTaxTariffPercentage <= 0)
                //  totalTaxTariffPercentage = item.TaxTariffPercentage;

                //totalTaxTariffPercentage = item.TaxTariffPercentage ?? 0;
                itemLines += $"\r\n<cac:InvoiceLine>\r\n        <cbc:ID>{itemId}</cbc:ID>\r\n        <cbc:InvoicedQuantity unitCode=\"PCE\">{item.Quantity.TwoDecInvarient()}</cbc:InvoicedQuantity>\r\n        <cbc:LineExtensionAmount currencyID=\"SAR\">{(item.Quantity * item.UnitPrice).TwoDecInvarient()}</cbc:LineExtensionAmount>\r\n        <cac:TaxTotal>\r\n            <cbc:TaxAmount currencyID=\"SAR\">{item.TaxAmount.TwoDecInvarient()}</cbc:TaxAmount>\r\n            <cbc:RoundingAmount currencyID=\"SAR\">{item.TotalAmount.TwoDecInvarient()}</cbc:RoundingAmount>\r\n        </cac:TaxTotal>\r\n        <cac:Item>\r\n            <cbc:Name>{item.ProductName} </cbc:Name>\r\n            <cac:ClassifiedTaxCategory>\r\n                <cbc:ID>S</cbc:ID>\r\n                <cbc:Percent>{item.TaxTariffPercentage.TwoDecInvarient()}</cbc:Percent>\r\n                <cac:TaxScheme>\r\n                    <cbc:ID>VAT</cbc:ID>\r\n                </cac:TaxScheme>\r\n            </cac:ClassifiedTaxCategory>\r\n        </cac:Item>\r\n        <cac:Price>\r\n            <cbc:PriceAmount currencyID=\"SAR\">{item.UnitPrice.TwoDecInvarient()}</cbc:PriceAmount>\r\n            <cac:AllowanceCharge>\r\n                <cbc:ChargeIndicator>true</cbc:ChargeIndicator>\r\n                <cbc:AllowanceChargeReason>discount</cbc:AllowanceChargeReason>\r\n                <cbc:Amount currencyID=\"SAR\">0.00</cbc:Amount>\r\n            </cac:AllowanceCharge>\r\n        </cac:Price>\r\n    </cac:InvoiceLine>";
                itemId++;
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

            //  var csrResult = csrGenerator.GenerateCsr(new CsrGenerationDto(
            //     "AM",
            //    "1-TST|2-TST|3-515851ae-8e07-4167-81c6-497565aecb6a",
            //    "310054439500003",
            //    "فرع ",
            //    "AMSteel - Port & Steel Operations",
            //    "SA",
            //    "1000",
            //    "Prince Sultan Jeddah",
            //    "test"
            //), EnvironmentType.Simulation);
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

                    client.DefaultRequestHeaders.Add("Authorization", $"Basic {input.Authorization}");
                    //client.DefaultRequestHeaders.Add("Authorization", "Basic VFVsSlJsRnFRME5DVDJWblFYZEpRa0ZuU1ZSRmQwRkJUbmxhWTFSS1ZHMVFPV1UwYkVGQlFrRkJRVE5LYWtGTFFtZG5jV2hyYWs5UVVWRkVRV3BDYVUxU1ZYZEZkMWxMUTFwSmJXbGFVSGxNUjFGQ1IxSlpSbUpIT1dwWlYzZDRSWHBCVWtKbmIwcHJhV0ZLYXk5SmMxcEJSVnBHWjA1dVlqTlplRVo2UVZaQ1oyOUthMmxoU21zdlNYTmFRVVZhUm1ka2JHVklVbTVaV0hBd1RWSnpkMGRSV1VSV1VWRkVSWGhLVVZKV2NFWlRWVFZYVkRCc1JGSldUa1JSVkVsMFVUQkZkMGhvWTA1TmFsRjNUV3BGTkUxVVJYcE9SRkYzVjJoalRrMXFXWGROYWtVMFRWUkZNRTVFVVhkWGFrSTBUVkZ6ZDBOUldVUldVVkZIUlhkS1ZGRlVSWEpOUTJ0SFFURlZSVU5vVFdsUlYzZG5WVEpHYjFwWVNXZFJWM2hvWWxjMWNFbEhXblpqYVVKVVdsZE9NV050YkRCbFUwSklaRmRHZVZwRVJXTk5RbTlIUVRGVlJVTjNkMVF5V1VoWmMyUnBOVWxPYVc0eVdWUlpjMlJ0U3pKTFpsbDBha1ZsVFVKM1IwRXhWVVZCZUUxV1ZVWktSbGRyUmxWUk1FVjBVVEk1YTFwVE1WUmhWMlIxWVZjMWJrMUdXWGRGUVZsSVMyOWFTWHBxTUVOQlVWbEdTelJGUlVGQmIwUlJaMEZGTjBndlZFVnVaa3RUUlRVek1DOXFaMlpRYUVVMWVscFVPR2MwY210a1ZWb3ZkRTlvYjFodWVDOW5aa2hVY1dNNVFUVlRWVEptVURaWEwxWldiREJ1TURSRUsxcE1hR05wWkc5RE9YWXJRa1p4THpGSlJYRlBRMEV5WTNkblowNXFUVWxJVWtKblRsWklVa1ZGWjJOcmQyZGpZV3RuWTAxM1oyTkJlRkJxUVRoQ1owNVdRa0ZSVFU1VVJYUlZSemw2VkcxR2RGcFlkM2xNVldNd1prUk5kRmxxYXpGTlZHaG9XbXByZEZsVVNURk5hVEF3VDBkRmVFeFViR3RhUkd0MFdrZFJkMDlVYTNoT1JGRXdUakphYUUxU09IZElVVmxMUTFwSmJXbGFVSGxNUjFGQ1FWRjNVRTE2UlhkUFJGVTBUbXBKTkU1RVFYZE5SRUY2VFZFd2QwTjNXVVJXVVZGTlJFRlJlRTFFUVhkTlUxRjNTV2RaUkZaUlVXRkVRblJRWWtkR05WbFRkMmRWYld3MVdWZFNiMHhEUWxSWldGWnJZVk5DUW1OdFJtbGhWMFY0UzBSQmJVSm5UbFpDUVRoTlNEbHBiakpaVkZsdk9XMUhNa3hVV1hRNWFYQkpUbWx1TWxsVVdYVmtiVU15UzJaWmMyUnRTekpMYTNkSVVWbEVWbEl3VDBKQ1dVVkdSamROTTA5elFXbDBZeTlJV2pGU1QyaE9NbXBwV25oamJpdHNUVUk0UjBFeFZXUkpkMUZaVFVKaFFVWkpTSGx2TTNSNVpUY3hVVzh5Y1dZNFpXcFVhbVJhTjI1SVF6Rk5TVWhzUW1kT1ZraFNPRVZuWkRCM1oyUnZkMmRrWldkblpGTm5aMlJIUjJkak5YTmFSMFozVDJrNGRrd3dUazlRVmtKR1YydFdTbFJzV2xCVFZVNUdWVEJPUWsxcE1VUlJVMmQ0UzFONFJGUnFNVkZWYkhCR1UxVTFWMVF3YkVSU1ZrSk1VMVJKYzFFd05EbFJNRkpSVEVWT1QxQldRakZaYlhod1dYbFZlVTFGZEd4bFUxVjVUVVpPYkdOdVduQlpNbFo2VEVWT1QxQldUbXhqYmxwd1dUSldla3hGVGs5UVZVNTJZbTFhY0ZvelZubFpXRkp3WWpJMGMxSkZUVGxhV0dnd1pXMUdNRmt5UlhOU1JVMDVXakk1TWt4RlVrUlFWM2gyV1RKR2MxQXlUbXhqYmxKd1dtMXNhbGxZVW14VmJWWXlZakpPYUdSSGJIWmlhM2h3WXpOUkwxbHRSbnBhVkRsMldXMXdiRmt6VWtSaVIwWjZZM294YWxWcmVFVmhXRTR3WTIxc2FXUllVbkJpTWpWUllqSnNkV1JFUTBKNloxbEpTM2RaUWtKUlZVaEJVVVZGWjJORmQyZGlOSGRuWW5OSFEwTnpSMEZSVlVaQ2VrRkRhRzlIZFdKSFVtaGpSRzkyVEhrNVJGUnFNVkZTVm5CR1UxVTFWMVF3YkVSU1ZrNUVVVlJKZEZFd1JYTlJNRFE1VVZWc1FreEZUazlRVmtJeFdXMTRjRmw1VlhsTlJYUnNaVk5WZVUxR1RteGpibHB3V1RKV2VreEZUazlRVms1c1kyNWFjRmt5Vm5wTVJVNVBVRlZPZG1KdFduQmFNMVo1V1ZoU2NHSXlOSE5TUlUwNVdsaG9NR1Z0UmpCWk1rVnpVa1ZOT1ZveU9USk1SVkpFVUZkNGRsa3lSbk5RTWs1Q1VUSldlV1JIYkcxaFYwNW9aRWRWTDFsdFJucGFWRGwyV1cxd2JGa3pVa1JpUjBaNlkzb3hhbHBZU2pCaFYxcHdXVEpHTUdGWE9YVlJXRll3WVVjNWVXRllValZOUVRSSFFURlZaRVIzUlVJdmQxRkZRWGRKU0dkRVFUaENaMnR5UW1kRlJVRlpTVE5HVVdORlRIcEJkRUpwVlhKQ1owVkZRVmxKTTBaUmFVSm9jV2RrYUU1RU4wVnZZblJ1VTFOSWVuWnpXakE0UWxaYWIwZGpNa015UkRWalZtUkJaMFpyUVdkRlVVMUNNRWRCTVZWa1NsRlJWMDFDVVVkRFEzTkhRVkZWUmtKM1RVTkNaMmR5UW1kRlJrSlJZMFJCZWtGdVFtZHJja0puUlVWQldVa3pSbEZ2UlVkcVFWbE5RVzlIUTBOelIwRlJWVVpDZDAxRFRVRnZSME5EYzBkQlVWVkdRbmROUkUxQmIwZERRM0ZIVTAwME9VSkJUVU5CTUd0QlRVVlpRMGxSUTNFelUzVXJTRU5KVTJOdmJFOUdkbmQ2WVRNdk5VZDJRVXh0YjFaU09IWndjRFp6WTNSa1J5OU9UVkZKYUVGTFVIUXdOVGRKWVhkNGJteEdPV1kxVEc4MFJsUXplRE15ZUZGYVdISnhRbTQzT0V4ME9HY3dOVUZTOkdkNFEzR3UzczJkSE4zRGhYN2xaV2dHbFQ4YTV6Q3oxcTNTa05mUCsvd3M9");
                    // client.DefaultRequestHeaders.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes($"{CsIdResult.binarySecurityToken}:{CsIdResult.secret}")));
                    StringContent content = new StringContent(JsonConvert.SerializeObject(new { invoiceHash = hashResult.Hash, uuid = invoice.InvoiceNumber, invoice = requestGeneratorResut.InvoiceRequest.Invoice }), Encoding.UTF8, "application/json");
                    HttpResponseMessage csIdresult = client.PostAsync(ZatcaVariable.ComplianceInvoice, content).Result;
                    string clearanceRes = csIdresult.Content.ReadAsStringAsync().Result;
                    if (csIdresult.IsSuccessStatusCode)
                    {
                        bool status = clearanceRes.ToLower().Contains(":\"cleared\",");
                       // await Mediator.Send(new UpdateZatcaClearanceStatus() { Id = invoiceId, Status = status });
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



        //For Live or Simulation
        [HttpGet("complianceCSID")]
        public async Task<ActionResult> GenerateComplianceCSID([FromQuery] int companyId, [FromQuery] int invoiceId, [FromQuery] string otp)
        {
            //invoiceId = 20080;
            var Company = await Mediator.Send(new GetCompany() { Id = companyId, User = UserInfo() });
            string csr, requestId, csid, secret;
            CsIdResultDto CsIdResult;


            var csrGenerator = new CsrGenerator();

            //var csrResult = csrGenerator.GenerateCsr(new CsrGenerationDto(
            //   "ls",
            //  "1-TST|2-TST|3-ed22f1d8-e6a2-1118-9b58-d9a8f11e445f",
            //  Company.VATNumber,
            //  "IT Department",
            //  Company.CompanyName,
            //  "SA",
            //  "1000",
            //  Company.CompanyAddress,
            //  "IT"
            //  ), EnvironmentType.Simulation);

            CsrResult csrResult = new()
            {
                Csr = _appSettings.Value.Certificate,
                PrivateKey = _appSettings.Value.PrivateKey
            };

            //  byte[] b = System.Convert.FromBase64String(csrResult.Csr);
            //string dbConnetion = System.Text.ASCIIEncoding.ASCII.GetString(b);
            //  string dbConnetion = System.Text.ASCIIEncoding.UTF8.GetString(b);

            // csrResult.Csr = dbConnetion;
            //XDocument document = XDocument.Load("STANDARD_NoSign.xml");
            XDocument document = XDocument.Load("Standard_Invoice_old.xml");
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

            //  var csrResult = csrGenerator.GenerateCsr(new CsrGenerationDto(
            //     "AM",
            //    "1-TST|2-TST|3-515851ae-8e07-4167-81c6-497565aecb6a",
            //    "310054439500003",
            //    "فرع ",
            //    "AMSteel - Port & Steel Operations",
            //    "SA",
            //    "1000",
            //    "Prince Sultan Jeddah",
            //    "test"
            //), EnvironmentType.Simulation);
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

        //Third Pary CSID
        [HttpGet("complianceCSID0")]
        public async Task<ActionResult> GenerateComplianceCSID0([FromQuery] int companyId, [FromQuery] int invoiceId, [FromQuery] string otp)
        {
            //invoiceId = 20080;
            var Company = await Mediator.Send(new GetCompany() { Id = companyId, User = UserInfo() });
            string csr, requestId, csid, secret;
            CsIdResultDto CsIdResult;


            var csrGenerator = new CsrGenerator();

            //var csrResult = csrGenerator.GenerateCsr(new CsrGenerationDto(
            //   "ls",
            //  "1-TST|2-TST|3-ed22f1d8-e6a2-1118-9b58-d9a8f11e445f",
            //  Company.VATNumber,
            //  "IT Department",
            //  Company.CompanyName,
            //  "SA",
            //  "1000",
            //  Company.CompanyAddress,
            //  "IT"
            //  ), EnvironmentType.Simulation);

            CsrResult csrResult = new()
            {
                //Csr = _appSettings.Value.Certificate,
                //PrivateKey = _appSettings.Value.PrivateKey

                Csr = "MIICPDCCAeKgAwIBAgIGAY3FHbgIMAoGCCqGSM49BAMCMBUxEzARBgNVBAMMCmVJbnZvaWNpbmcwHhcNMjQwMjIwMDYwNDMwWhcNMjkwMjE5MjEwMDAwWjBxMQswCQYDVQQGEwJTQTEWMBQGA1UECwwNSmVkZGFoIEJyYW5jaDEqMCgGA1UECgwhQU1TdGVlbC1Qb3J0IGFuZCBTdGVlbCBPcGVyYXRpb25zMR4wHAYDVQQDDBVQUkVaQVRDQS1Db2RlLVNpZ25pbmcwVjAQBgcqhkjOPQIBBgUrgQQACgNCAAQDvDD98JLPYnr04KTWG4cKlV7y0UhP8SXd7TkElQKl46jpUdhzdCkEv6skX54c4gF9+S6sishbQWzAqNujeOmAo4HEMIHBMAwGA1UdEwEB/wQCMAAwgbAGA1UdEQSBqDCBpaSBojCBnzE+MDwGA1UEBAw1MS1Qb3NOYW1lfDItRzR8My1iYjQ4NmM1My05YTg4LTRlOWEtYTkzYi0zNTI2NGFhZDc5NmUxHzAdBgoJkiaJk/IsZAEBDA8zMTAwNTQ0Mzk1MDAwMDMxDTALBgNVBAwMBDEwMDAxFTATBgNVBBoMDFAuTy5Cb3ggNDg0MzEWMBQGA1UEDwwNTWFudWZhY3R1cmluZzAKBggqhkjOPQQDAgNIADBFAiEA8Vw+GZEcNkCBbVQ6NcuyzDuNYXKo4EYwVakQU0b9x6gCICgeai1qNlX52dIt7iEL5gzl5UAuHOG22opLWK/qTzCL",
                PrivateKey = "MHQCAQEEIF525L+/BIR5wcDflARDR7f3/4ovMG3Af4EbfYt/5wAMoAcGBSuBBAAKoUQDQgAEA7ww/fCSz2J69OCk1huHCpVe8tFIT/El3e05BJUCpeOo6VHYc3QpBL+rJF+eHOIBffkurIrIW0FswKjbo3jpgA=="
            };

            //  byte[] b = System.Convert.FromBase64String(csrResult.Csr);
            //string dbConnetion = System.Text.ASCIIEncoding.ASCII.GetString(b);
            //  string dbConnetion = System.Text.ASCIIEncoding.UTF8.GetString(b);

            // csrResult.Csr = dbConnetion;
            //XDocument document = XDocument.Load("STANDARD_NoSign.xml");
            XDocument document = XDocument.Load("Standard_Invoice.xml");
            XmlDocument xmlDoc = new XmlDocument();
            string xmlData = document.ToString();

            var invoice = await Mediator.Send(new GetSingleCreditInvoiceById() { Id = invoiceId, InvoiceStatusId = (int)InvoiceStatusIdType.Invoice, User = UserInfo() });
            invoice.InvoiceNumber = "92356";
            invoice.TaxAmount = 37500;
            invoice.SubTotal = 250000;

            xmlData = xmlData
                .Replace("{{UUID}}", invoice.InvoiceNumber).Replace("{{IssueDate}}", invoice.InvoiceDate.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture))
                .Replace("{{IssueTime}}", invoice.InvoiceDate.Value.ToString("hh:mm:ss", CultureInfo.InvariantCulture)).Replace("{{ICV}}", invoice.InvoiceNumber.ToString()).Replace("{{VAT}}", "310054439500003")
                .Replace("{{CRN}}", Company.CrNumber).Replace("{{StreetName}}", Company.CompanyAddress).Replace("{{BuildingNumber}}", "2255").Replace("{{PlotIdentification}}", "22").Replace("{{CityName}}", Company.City)
                .Replace("{{PostalZone}}", "12222").Replace("{{Seller}}", "AMS").Replace("{ {DeliveryDate}}", invoice.InvoiceDueDate.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture))
                .Replace("{{TaxAmount}}", $"{invoice.TaxAmount.TwoDecInvarient()}").Replace("{{TaxableAmount}}", $"{invoice.SubTotal.TwoDecInvarient()}")
                //.Replace("{{TaxCategory}}", $"15.00")
                .Replace("{{TaxExclusiveAmount}}", $"{invoice.SubTotal.TwoDecInvarient()}").Replace("{{TaxInclusiveAmount}}", $"{invoice.TotalAmount.TwoDecInvarient()}");
            string itemLines = string.Empty, taxSubtotal = string.Empty;
            decimal? totalTaxTariffPercentage = 0;

            var item = invoice.ItemList[0];
            item.Quantity = 1000;
            item.UnitPrice = 250;
            item.TaxAmount = 37500;
            item.TotalAmount = 287500;
            item.TaxTariffPercentage = 15;
            item.ProductName = "Steel bar";

            taxSubtotal += $"\r\n<cac:TaxSubtotal>\r\n            <cbc:TaxableAmount currencyID=\"SAR\">{(item.Quantity * item.UnitPrice).TwoDecInvarient()}</cbc:TaxableAmount>\r\n            <cbc:TaxAmount currencyID=\"SAR\">{item.TaxAmount.TwoDecInvarient()}</cbc:TaxAmount>\r\n            <cac:TaxCategory>\r\n                <cbc:ID schemeID=\"UN/ECE 5305\" schemeAgencyID=\"6\">S</cbc:ID>\r\n                <cbc:Percent>{item.TaxTariffPercentage.TwoDecInvarient()}</cbc:Percent>\r\n                <cac:TaxScheme>\r\n                    <cbc:ID schemeID=\"UN/ECE 5153\" schemeAgencyID=\"6\">VAT</cbc:ID>\r\n                </cac:TaxScheme>\r\n            </cac:TaxCategory>\r\n        </cac:TaxSubtotal>";
            //if (item.TaxTariffPercentage is not null && item.TaxTariffPercentage > 0 && totalTaxTariffPercentage <= 0)
            //  totalTaxTariffPercentage = item.TaxTariffPercentage;

            //totalTaxTariffPercentage = item.TaxTariffPercentage ?? 0;
            itemLines = $"\r\n<cac:InvoiceLine>\r\n        <cbc:ID>{1}</cbc:ID>\r\n        <cbc:InvoicedQuantity unitCode=\"PCE\">{item.Quantity.TwoDecInvarient()}</cbc:InvoicedQuantity>\r\n        <cbc:LineExtensionAmount currencyID=\"SAR\">{(item.Quantity * item.UnitPrice).TwoDecInvarient()}</cbc:LineExtensionAmount>\r\n        <cac:TaxTotal>\r\n            <cbc:TaxAmount currencyID=\"SAR\">{item.TaxAmount.TwoDecInvarient()}</cbc:TaxAmount>\r\n            <cbc:RoundingAmount currencyID=\"SAR\">{item.TotalAmount.TwoDecInvarient()}</cbc:RoundingAmount>\r\n        </cac:TaxTotal>\r\n        <cac:Item>\r\n            <cbc:Name>{item.ProductName} </cbc:Name>\r\n            <cac:ClassifiedTaxCategory>\r\n                <cbc:ID>S</cbc:ID>\r\n                <cbc:Percent>{item.TaxTariffPercentage.TwoDecInvarient()}</cbc:Percent>\r\n                <cac:TaxScheme>\r\n                    <cbc:ID>VAT</cbc:ID>\r\n                </cac:TaxScheme>\r\n            </cac:ClassifiedTaxCategory>\r\n        </cac:Item>\r\n        <cac:Price>\r\n            <cbc:PriceAmount currencyID=\"SAR\">{item.UnitPrice.TwoDecInvarient()}</cbc:PriceAmount>\r\n            <cac:AllowanceCharge>\r\n                <cbc:ChargeIndicator>true</cbc:ChargeIndicator>\r\n                <cbc:AllowanceChargeReason>discount</cbc:AllowanceChargeReason>\r\n                <cbc:Amount currencyID=\"SAR\">0.00</cbc:Amount>\r\n            </cac:AllowanceCharge>\r\n        </cac:Price>\r\n    </cac:InvoiceLine>";


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

            //  var csrResult = csrGenerator.GenerateCsr(new CsrGenerationDto(
            //     "AM",
            //    "1-TST|2-TST|3-515851ae-8e07-4167-81c6-497565aecb6a",
            //    "310054439500003",
            //    "فرع ",
            //    "AMSteel - Port & Steel Operations",
            //    "SA",
            //    "1000",
            //    "Prince Sultan Jeddah",
            //    "test"
            //), EnvironmentType.Simulation);
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
