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

namespace LS.API.ZatcaAPI.Controllers
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
        }
        private bool HasClaims() => GetCliams().Count() != 0;

        public int UserId => HasClaims() ? Convert.ToInt32(GetCliams().FirstOrDefault(c => c.Type == "userid").Value) : 0;
        public int CompanyId => HasClaims() ? Convert.ToInt32(GetCliams().FirstOrDefault(c => c.Type == "companyid").Value) : 0;
        public int BranchId => HasClaims() ? Convert.ToInt32(GetCliams().FirstOrDefault(c => c.Type == "branchid").Value) : 0;
        public string BranchCode => HasClaims() ? Convert.ToString(GetCliams().FirstOrDefault(c => c.Type == "branchcode").Value) : string.Empty;
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
  
}
