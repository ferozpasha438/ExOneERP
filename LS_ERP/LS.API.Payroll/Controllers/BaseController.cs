using CIN.Application;
using CIN.Application.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LS.API.Payroll.Controllers
{
    public class BaseController : ApiControllerBase
    {
        private readonly IOptions<AppSettingsJson> _appSettings;
        public BaseController(IOptions<AppSettingsJson> appSettings)
        {
            _appSettings = appSettings;
        }

        protected ApiMessageDto ApiMessage(string message) => new ApiMessageDto { Message = message };
        private IEnumerable<Claim> GetCliams()
        {
            string auth = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            //string auth = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VyaWQiOiIyIiwiY29tcGFueWlkIjoiMiIsImJyYW5jaGlkIjoiNyIsImJyYW5jaGNvZGUiOiJPTEFZQTEiLCJ0ZWFjaGVyQ29kZSI6IiIsImh0dHA6Ly9zY2hlbWFzLm15Y29tcGFueS5jb20vaWRlbnRpdHkvY2xhaW1zL2RiY29ubmVjdGlvbnN0cmluZyI6IlNlcnZlcj1sb2NhbGhvc3Q7RGF0YWJhc2U9Q0lOREJPbmU1O1RydXN0ZWRfQ29ubmVjdGlvbj1UcnVlO011bHRpcGxlQWN0aXZlUmVzdWx0U2V0cz10cnVlOyIsIm5iZiI6MTY3Mzg1MTgwNywiZXhwIjoxNjczOTIzODA3LCJpYXQiOjE2NzM4NTE4MDd9.vANAKBhBHbRh6xCTKAXqh27q2Xhux05OPGgrq2gaN1c";
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
    public class FileBaseController : BaseController
    {
        private readonly IWebHostEnvironment _env;
        private readonly IOptions<AppSettingsJson> _appSettings;
        public FileBaseController(IOptions<AppSettingsJson> appSettings, IWebHostEnvironment env) : base(appSettings)
        {
            _env = env;
            _appSettings = appSettings;
        }
    }
}
