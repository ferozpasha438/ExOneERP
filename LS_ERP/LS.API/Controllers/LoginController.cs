using CIN.Application;
using CIN.Application.Common;
using CIN.Application.SchoolMgtQuery;
using CIN.Application.SystemQuery;
using CIN.DB;
using CIN.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ApiControllerBase
    {
        private readonly CINServerDbContext _cinDbContext;
        private IConfiguration _Config;
        //  public ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public LoginController(IConfiguration config, CINServerDbContext cinDbContext)
        {
            _Config = config;
            _cinDbContext = cinDbContext;
        }

        private string GenerateJSONWebToken(BaseCINLoginUserDTO loginUserDTO)
        {
            var signingKey = Convert.FromBase64String(_Config["Jwt:Key"]);
            var issuer = _Config["Jwt:Issuer"].ToString();
            var expiryDuration = int.Parse(_Config["SessionTimeout:Expiry"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = issuer,            // Not required as no third-party is involved
                Audience = issuer,            // Not required as no third-party is involved
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
                new("companyid", loginUserDTO.CompanyId.ToString()),
                new("branchid", loginUserDTO.BranchId.ToString()),
                new("branchcode", loginUserDTO.BranchCode),
                new("teacherCode", loginUserDTO.TeacherCode),
                new(CustomClaimType.DbConnectionString, loginUserDTO.DBConnectionString),
                new(CustomClaimType.DbHRMConnectionString, loginUserDTO.DBHRMConnectionString)

            };
            return claims;

        }

        private void DeleteClaims()
        {

        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CheckCINServerMetaDataDto input)
        {
            Log.Info("ValidateUser() Method Begins");
            try
            {
                var (userId, branchCode, loginType) = await Mediator.Send(new CheckLogin() { Input = input });
                if (userId > 0)
                {
                    var metaData = await _cinDbContext.MetaDataList.FirstOrDefaultAsync(e => e.CINNumber == input.CINNumber);
                    short concurrentUsers = metaData.ConcurrentUsers;
                    short connectedUsers = metaData.ConnectedUsers;
                    var userSideMenuList = await Mediator.Send(new GetSideMenuOptionList() { User = new UserIdentityDto { UserId = userId } });
                    metaData.ConnectedUsers++;
                    _cinDbContext.MetaDataList.Update(metaData);
                    await _cinDbContext.SaveChangesAsync();
                    var branch = await Mediator.Send(new GetBranchInfoByBranchCode() { Input = branchCode });
                    var company = await Mediator.Send(new GetCompany() { Id = branch.CompanyId });
                    string teacherCode = string.Empty;
                    if (loginType == "Teacher")
                    {
                        teacherCode = await Mediator.Send(new GetTeacherCode() { Id = userId });
                    }
                    var token = GenerateJSONWebToken(new BaseCINLoginUserDTO { CompanyId = branch.CompanyId, BranchCode = branchCode, BranchId = branch.Id, UserId = userId, DBConnectionString = GetConnectionString(), DBHRMConnectionString = GetHRMConnectionString(), TeacherCode = teacherCode });

                    return Ok(new { userSideMenuList, token, company.LogoURL });


                    //if (connectedUsers < concurrentUsers)
                    //{
                    //    var userSideMenuList = await Mediator.Send(new GetSideMenuOptionList() { User = new UserIdentityDto { UserId = userId } });
                    //    metaData.ConnectedUsers++;
                    //    _cinDbContext.MetaDataList.Update(metaData);
                    //    await _cinDbContext.SaveChangesAsync();

                    //    var branch = await Mediator.Send(new GetBranchInfoByBranchCode() { Input = branchCode });
                    //    var company = await Mediator.Send(new GetCompany() { Id = branch.CompanyId });
                    //    string teacherCode = string.Empty;
                    //    if (loginType== "Teacher")
                    //    {
                    //        teacherCode=await Mediator.Send(new GetTeacherCode() { Id = userId });
                    //    }
                    //    var token = GenerateJSONWebToken(new BaseCINLoginUserDTO { CompanyId = branch.CompanyId, BranchCode = branchCode, BranchId = branch.Id, UserId = userId, DBConnectionString = GetConnectionString(), TeacherCode= teacherCode });

                    //    return Ok(new { userSideMenuList, token, company.LogoURL });

                    //}
                    //return BadRequest(new ApiMessageDto { Message = $"more than {concurrentUsers} are not allowed to login", Type = (short)LoginStatusType.ConcurrentUsers });

                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return BadRequest(new ApiMessageDto { Message = "Invalid Username & Password" });
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

    }

}