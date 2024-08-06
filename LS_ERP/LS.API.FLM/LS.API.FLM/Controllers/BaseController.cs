using CIN.Application;
using CIN.Application.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using ZXing;

namespace LS.API.FLM.Controllers
{
    //[Authorize]
    public class BaseController : ApiControllerBase
    {
        private readonly IOptions<AppSettingsJson> _appSettings;

        public BaseController(IOptions<AppSettingsJson> appSettings)
        {
            _appSettings = appSettings;
        }
        [HttpGet("isAuthenticated")]

        public bool IsAuthenticated() => UserId > 0;

        protected ApiMessageDto ApiMessage(string message) => new ApiMessageDto { Message = message };
        protected ApiMessageDto ApiMessageNotFound() => new ApiMessageDto { Message = ApiMessageInfo.NotFound };
        private IEnumerable<Claim> GetCliams()
        {
            string auth = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            var jwtEncodedString = auth.Substring(7);
            var token = new JwtSecurityToken(jwtEncodedString: jwtEncodedString);
            return token.Claims;
            // return new List<Claim>();
        }
        private bool HasClaims() => GetCliams().Count() != 0;

        public int UserId => HasClaims() ? Convert.ToInt32(GetCliams().FirstOrDefault(c => c.Type == "userid").Value) : 0;
        public int CompanyId => HasClaims() ? Convert.ToInt32(GetCliams().FirstOrDefault(c => c.Type == "companyid").Value) : 0;
        public int BranchId => HasClaims() ? Convert.ToInt32(GetCliams().FirstOrDefault(c => c.Type == "branchid").Value) : 0;
        public string BranchCode => HasClaims() ? Convert.ToString(GetCliams().FirstOrDefault(c => c.Type == "branchcode").Value) : string.Empty;
        public string TeacherCode => HasClaims() ? Convert.ToString(GetCliams().FirstOrDefault(c => c.Type == "teacherCode").Value) : string.Empty;
        protected string GetModuleCodes()
        {
            var moduleCodes = HttpContext.Request.Headers["ModuleCodes"].FirstOrDefault();
            if (moduleCodes is not null)
            {
                byte[] moduleCode = System.Convert.FromBase64String(moduleCodes);
                return System.Text.ASCIIEncoding.ASCII.GetString(moduleCode);
            }
            return string.Empty;
        }

        protected string Culture => HttpContext.Request.Headers["Accept-Language"].ToString() ?? "en-US";
        //protected string Culture => HttpContext.Items["SelectedLng"]?.ToString() ?? "en-US";

        protected UserIdentityDto UserInfo() => new UserIdentityDto { UserId = UserId, CompanyId = CompanyId, BranchCode = BranchCode, BranchId = BranchId, Culture = Culture, ModuleCodes = "ADM,FI,FIN,INVT,PURC,SALE,OPERT" }; //ConnectionString = GetConnectionString(),

    }

    public class FileBaseController : BaseController
    {
        private readonly IWebHostEnvironment _env;
        private readonly IOptions<AppSettingsJson> _appSettings;
        public FileBaseController(IOptions<AppSettingsJson> appSettings, IWebHostEnvironment env) : base(appSettings)
        {
            _env = env;
            _appSettings = appSettings;
        }

        protected string ContentRootPath => _env.ContentRootPath;
        private static char ConvertUnicodeStringToAscii(int value) => Microsoft.VisualBasic.Strings.Chr(value);

        protected string GenerateQRCode(string sellerName, string vatNumber, string taxAmount, string totalInclAmount, string CreatedOn)
        {
            taxAmount = taxAmount == "" ? "0.00" : taxAmount;
            totalInclAmount = totalInclAmount == "" ? "0.00" : totalInclAmount;

            string folderPath = _appSettings.Value.QRCodeImagePath.ToString();
            // string imagePath = $"{folderPath}{new Random().Next(999, 999999)}_QrCode.jpg";
            string path = $"{folderPath}{new Random().Next(999, 999999)}_QrCode.jpg";

            // var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", imagePath);
            //var path = Path.Combine(webRoot, imagePath);

            //// If the directory doesn't exist then create it.
            //if (!Directory.Exists(Path.Combine(WebRootPath, folderPath)))
            //{
            //    Directory.CreateDirectory(Path.Combine(WebRootPath, folderPath));
            //}

            var barcodeWriter = new BarcodeWriter();
            barcodeWriter.Format = BarcodeFormat.QR_CODE;

            // barcodeWriter.Options.GS1Format = true;
            //barcodeWriter.Options.PureBarcode = true;

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


            var base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(barCode));
            var result = barcodeWriter.Write(base64);
            //var result = barcodeWriter.Write($"Name: {sellerName},Vat Number: { vatNumber},Vat Amount: {taxAmount },Total Amount: {totalInclAmount},Date: {CreatedOn}");
            // var result = barcodeWriter.Write($"Name: {sellerName},Vat No: { vatNumber},Date: {CreatedOn}, Total Amt: {totalInclAmount},Vat Amt: {taxAmount }");
            //var result = barcodeWriter.Write($"Invoice Number: {qrcodeText}, Vat Number: { vatNumber}\n Vat Amount: {taxAmount }, Total Incl Vat: {totalInclAmount}\n Invoice Date: {CreatedOn}\n");

            string barcodePath = $"{ContentRootPath}{path}";
            var barcodeBitmap = new Bitmap(result);
            using (MemoryStream memory = new MemoryStream())
            {
                using (FileStream fs = new FileStream(barcodePath, FileMode.Create, FileAccess.ReadWrite))
                {
                    barcodeBitmap.Save(memory, ImageFormat.Jpeg);
                    byte[] bytes = memory.ToArray();
                    fs.Write(bytes, 0, bytes.Length);
                }
            }

            byte[] imageArray = System.IO.File.ReadAllBytes(barcodePath);
            return $"data:image/jpg;base64,{Convert.ToBase64String(imageArray)}";

            //return path;
        }
    }
}
