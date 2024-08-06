using CIN.Application;
using CIN.Application.Common;
using CIN.Application.MobileMgt.Queries;
using CIN.Application.SystemQuery;
using CIN.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LS.API.MOB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ApiControllerBase
    {
        private readonly CINServerDbContext _cinDbContext;
        private IConfiguration _Config;
        private readonly IOptions<AppMobileSettingsJson> _appSettings;
        //  public ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public LoginController(IConfiguration config, CINServerDbContext cinDbContext, IOptions<AppMobileSettingsJson> appSettings)
        {
            _Config = config;
            _cinDbContext = cinDbContext;
            _appSettings = appSettings;
        }


        #region Login 


        async Task<IActionResult> CheckCIN(CheckMobileCINServerMetaDataDto input)
        {
            Log.Info("ValidateUser() Method Begins");
            //var currentdate = DateTime.Now.Date;            

            //  var currentdate = EF.Functions.DateFromParts(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            var metaData = await _cinDbContext.MetaDataList.FirstOrDefaultAsync(e => e.CINNumber == input.CINNumber);
            if (metaData is not null)
            {

                var validDate = metaData.ValidDate.CompareTo(DateTime.Now);
                if (validDate >= 1)
                {

                    var user = await Mediator.Send(new CheckMobileLogin()
                    {
                        Input = input
                        //new()
                        //{
                        //    CINNumber = input.CINNumber,
                        //    UserName = input.UserName,
                        //    Password = input.Password,
                        //    SiteGeoLatitude = input.SiteGeoLatitude,
                        //    SiteGeoLongitude = input.SiteGeoLongitude,
                        //
                        //    SiteLocationNvMeter = _appSettings.Value.SiteLocationNvMeter,
                        //    SiteLocationPvMeter = _appSettings.Value.SiteLocationPvMeter,
                        //    SiteLocationExtraMeter = _appSettings.Value.SiteLocationExtraMeter,
                        //},
                    });

                    if (user.Id == -3)
                        return BadRequest(new MobileApiMessageDto { Message = "Your Location is Out of Site", Status = false });

                    if (user.Id == -2)
                        return BadRequest(new MobileApiMessageDto { Message = "Site is not assigned", Status = false });

                    if (user.Id == -1)
                        return BadRequest(new MobileApiMessageDto { Message = "Already SignedIn", Status = false });

                    if (user.Id > 0)
                    {
                        var branch = await Mediator.Send(new GetBranchInfoByBranchCode() { Input = user.BranchCode });
                        //await Mediator.Send(new SetSignInStatus() { Input = true, UserId = user.Id });

                        var token = GenerateJSONWebToken(new BaseCINLoginUserDTO
                        {
                            CompanyId = branch.CompanyId,
                            BranchCode = user.BranchCode,
                            BranchId = branch.Id,
                            UserId = user.Id,
                            EmployeeId = user.EmployeeId,
                            DBConnectionString = metaData.DBConnectionString,
                            SiteCode = user.SiteCode,
                        });

                        byte[] connectionString = ASCIIEncoding.ASCII.GetBytes(metaData.DBConnectionString);
                        byte[] modueCodes = ASCIIEncoding.ASCII.GetBytes(metaData.ModueCodes);
                        return Ok(new CINMobileServerMetaDataDto
                        {
                            Id = metaData.Id,
                            CINNumber = metaData.CINNumber,
                            SiteName = user.SiteName,
                            SiteNameAr = user.SiteNameAR,

                            ModuleCodes = Convert.ToBase64String(modueCodes),
                            APIEndpoint = metaData.APIEndpoint,
                            DBConnectionString = Convert.ToBase64String(connectionString),
                            InnerRadius = user.InnerRadius,
                            OuterRadius = user.OuterRadius,
                            SiteGeoLatitude = user.SiteGeoLatitude,
                            SiteGeoLongitude = user.SiteGeoLongitude,
                            SiteGeoGain = user.SiteGeoGain,
                            IsSubscribed = user.IsLoginAllow,
                            Token = token,
                        });
                    }
                    return BadRequest(new MobileApiMessageDto { Message = "Invalid Username & Password", Status = false });
                }
                return BadRequest(new MobileApiMessageDto { Message = "Your License is Expired", Status = false });

            }
            //bool isNotExpired = EF.Functions.DateFromParts(validDate.Year, validDate.Month, validDate.Day) >= currentdate;
            //if (isNotExpired)
            //{
            //    return Ok(metaData);
            //}
            return BadRequest(new MobileApiMessageDto { Message = "Invalid CINNumber", Status = false });
        }

        private string GenerateJSONWebToken(BaseCINLoginUserDTO loginUserDTO)
        {
            var signingKey = Convert.FromBase64String(_Config["Jwt:Key"]);
            var expiryDuration = int.Parse(_Config["SessionTimeout:Expiry"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = null,              // Not required as no third-party is involved
                Audience = null,            // Not required as no third-party is involved
                IssuedAt = DateTime.UtcNow,
                NotBefore = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddMinutes(expiryDuration),
                Subject = new ClaimsIdentity(GetUserClaims(loginUserDTO)),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(signingKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = jwtTokenHandler.CreateJwtSecurityToken(tokenDescriptor);
            var token = jwtTokenHandler.WriteToken(jwtToken);
            return token;
        }

        List<Claim> GetUserClaims(BaseCINLoginUserDTO loginUserDTO)
        {
            List<Claim> claims = new()
            {
                new("userid", loginUserDTO.UserId.ToString()),
                new("employeeId", loginUserDTO.EmployeeId.ToString()),
                new("companyid", loginUserDTO.CompanyId.ToString()),
                new("branchid", loginUserDTO.BranchId.ToString()),
                new("branchcode", loginUserDTO.BranchCode),
                new("sitecode", loginUserDTO.SiteCode),
                new(CustomClaimType.DbConnectionString, loginUserDTO.DBConnectionString),
            };
            return claims;

        }

        private void DeleteClaims()
        {

        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CheckMobileLoginCINServerMetaDataDto input)
        {
            Log.Info("ValidateUser() Method Begins");
            return await CheckCIN(new()
            {
                CINNumber = input.CINNumber,
                UserName = input.UserName,
                Password = input.Password,
                SiteGeoLatitude = input.SiteGeoLatitude,
                SiteGeoLongitude = input.SiteGeoLongitude,

                SiteLocationNvMeter = _appSettings.Value.SiteLocationNvMeter,
                SiteLocationPvMeter = _appSettings.Value.SiteLocationPvMeter,
                SiteLocationExtraMeter = _appSettings.Value.SiteLocationExtraMeter,

            });
        }


        //[HttpPost]
        //[Route("validateuser")]
        //public async Task<IActionResult> ValidateUser([FromBody] LoginUserDto loginModel)
        //{            
        //    //log request                   
        //    //LogFile.LogRequestPayload(Request.Headers);
        //    //LogFile.LogRequestPayload(loginModel);


        //    Log.Info("ValidateUser() Method Begins");

        //    var userData = await Mediator.Send(new LoginQuery { loginUserDto = loginModel });
        //    if (userData is not null)
        //    {
        //        Log.Info("Success =>");
        //        userData = GenerateJSONWebToken(userData);
        //        LogFile.LogRequestPayload(userData);
        //        return Ok(new APIPayload<BaseLoginUserDTO> { Data = userData });
        //    }
        //    return BadRequest(ViewResources.Resource.Invoice_Login_MsgError);
        //}












        #endregion

        #region EmployeeMobileLogin
        [HttpPost("EmployeeMobileLogin")]
        public async Task<IActionResult> EmployeeMobileLogin(MobileLoginInput input)
        {
            Log.Info("ValidateUser() Method Begins");
            //var currentdate = DateTime.Now.Date;            

            //  var currentdate = EF.Functions.DateFromParts(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            var metaData = await _cinDbContext.MetaDataList.FirstOrDefaultAsync(e => e.CINNumber == input.CINNumber);
            if (metaData is null)
            {
                return BadRequest(new MobileApiMessageDto { Message = "Invalid CINNumber", Status = false });
            }

            var validDate = metaData.ValidDate.CompareTo(DateTime.Now);
            if (validDate < 1)
            {
                return BadRequest(new MobileApiMessageDto { Message = "Your License is Expired", Status = false });

            }


            var user = await Mediator.Send(new CheckEmployeeMobileLogin()
            {
                Input = input
            });

            if (user.MobileLoginId <= 0)
                return BadRequest(new MobileApiMessageDto { Message = user.ErrorMessage, Status = false });



            if (user.MobileLoginId > 0)
            {
                user.ShiftsList = await Mediator.Send(new GetEmployeeTodaysShiftsQuery() {  EmployeeNumber=input.UserName, TodayDate=input.TodayDate});
                return Ok(user);

            }
            return BadRequest(new MobileApiMessageDto { Message = "Something Wrong", Status = false });
        }



        #endregion
    }

}