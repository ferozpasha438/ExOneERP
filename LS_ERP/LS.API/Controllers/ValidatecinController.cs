using CIN.Application;
using CIN.Server;
using log4net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Text;
using System.Threading.Tasks;

namespace LS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class ValidatecinController : ApiControllerBase
    {
        private readonly CINServerDbContext _cinDbContext;
        private IConfiguration _Config;
        public ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ValidatecinController(CINServerDbContext cinDbContext, IConfiguration config)
        {
            _Config = config;
            _cinDbContext = cinDbContext;

        }

        //private BaseCINLoginUserDTO GenerateJSONWebToken(BaseCINLoginUserDTO loginUserDTO)
        //{
        //    var signingKey = Convert.FromBase64String(_Config["Jwt:Key"]);
        //    var expiryDuration = int.Parse(_Config["SessionTimeout:Expiry"]);

        //    var tokenDescriptor = new SecurityTokenDescriptor
        //    {
        //        Issuer = null,              // Not required as no third-party is involved
        //        Audience = null,            // Not required as no third-party is involved
        //        IssuedAt = DateTime.UtcNow,
        //        NotBefore = DateTime.UtcNow,
        //        Expires = DateTime.UtcNow.AddMinutes(expiryDuration),
        //        Subject = new ClaimsIdentity(new List<Claim> {
        //                new Claim("userid", loginUserDTO.Id.ToString())
        //                //new Claim("roleid", loginUserDTO.RoleID.ToString()),
        //                //new Claim("companyid", loginUserDTO.CompanyId.ToString())
        //            }),
        //        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(signingKey), SecurityAlgorithms.HmacSha256Signature)
        //    };
        //    var jwtTokenHandler = new JwtSecurityTokenHandler();
        //    var jwtToken = jwtTokenHandler.CreateJwtSecurityToken(tokenDescriptor);
        //    var token = jwtTokenHandler.WriteToken(jwtToken);
        //    loginUserDTO.ApiToken = token;
        //    return loginUserDTO;
        //}


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CheckCINServerDto input)
        {

            try
            {
                Log.Info("ValidateUser() Method Begins");
                //var currentdate = DateTime.Now.Date;            

                //  var currentdate = EF.Functions.DateFromParts(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                var metaData = await _cinDbContext.MetaDataList.FirstOrDefaultAsync(e => e.CINNumber == input.CINNumber);
                if (metaData is not null)
                {

                    if (metaData.IsActive is null || metaData.IsActive == false)
                        return BadRequest(new ApiMessageDto { Message = "Your License is Expired", Type = (short)LoginStatusType.LicenseExpired });

                    var validDate = metaData.ValidDate.CompareTo(DateTime.Now);
                    if (validDate >= 1)
                    {
                        byte[] connectionString = ASCIIEncoding.ASCII.GetBytes(metaData.DBConnectionString);
                        byte[] modueCodes = ASCIIEncoding.ASCII.GetBytes(metaData.ModueCodes);
                        return Ok(new CINServerMetaDataDto
                        {
                            Id = metaData.Id,
                            CINNumber = metaData.CINNumber,
                            ModuleCodes = Convert.ToBase64String(modueCodes),
                            APIEndpoint = metaData.APIEndpoint,
                            AdmUrl = metaData.AdmUrl,
                            FinUrl = metaData.FinUrl,
                            OpmUrl = metaData.OpmUrl,
                            InvUrl = metaData.InvUrl,
                            SndUrl = metaData.SndUrl,
                            PopUrl = metaData.PopUrl,
                            HrmUrl = metaData.HrmUrl,
                            HraUrl = metaData.HraUrl,
                            HrsUrl = metaData.HrsUrl,
                            FltUrl = metaData.FltUrl,
                            SchUrl = metaData.SchUrl,
                            ScpUrl = metaData.ScpUrl,
                            SctUrl = metaData.SctUrl,
                            PosUrl = metaData.PosUrl,
                            MfgUrl = metaData.MfgUrl,
                            CrmUrl = metaData.CrmUrl,
                            DBConnectionString = Convert.ToBase64String(connectionString),
                            UtlUrl = metaData.UtlUrl!=null? Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(metaData.UtlUrl)) :null, // to get DMC connection string dynamic

                        }); ;
                    }
                    return BadRequest(new ApiMessageDto { Message = "Your License is Expired", Type = (short)LoginStatusType.LicenseExpired });

                }
            }
            catch (Exception ex)
            {

                throw;
            }

            //bool isNotExpired = EF.Functions.DateFromParts(validDate.Year, validDate.Month, validDate.Day) >= currentdate;
            //if (isNotExpired)
            //{
            //    return Ok(metaData);
            //}
            return BadRequest(new ApiMessageDto { Message = "Invalid CINNumber" });
        }


        [HttpPost("logout")]
        public async Task<IActionResult> LogOut([FromBody] CheckCINServerDto input)
        {
            Log.Info("logout() Method Begins");
            var metaData = await _cinDbContext.MetaDataList.FirstOrDefaultAsync(e => e.CINNumber == input.CINNumber);
            if (metaData is not null)
            {
                if (metaData.ConnectedUsers > 0)
                {
                    metaData.ConnectedUsers--;
                    _cinDbContext.MetaDataList.Update(metaData);
                    await _cinDbContext.SaveChangesAsync();
                }
                return Ok(new APIPayload<string> { Data = "Logout Successful" });
            }
            return BadRequest(new ApiMessageDto { Message = "Logout Failed" });
        }

        //[HttpPost]
        //public async Task<IActionResult> Post([FromBody] CheckCINServerMetaDataDto input)
        //{
        //    Log.Info("ValidateUser() Method Begins");
        //    var currentdate = DateTime.Now.Date;
        //    var metaData = await _cinDbContext.MetaDataList.FirstOrDefaultAsync(e => e.CINNumber == input.CINNumber);
        //    if (metaData is not null)
        //    {
        //        var isNotExpired = await _cinDbContext.MetaDataList.AnyAsync(e => e.ValidDate.Date >= currentdate);
        //        if (isNotExpired)
        //        {
        //            var user = await _userDbContext.Users.FirstOrDefaultAsync(e => e.UserName == input.UserName && e.Password == input.Password);
        //            if (user is not null)
        //            {
        //                return Ok(metaData);
        //            }
        //            return BadRequest("Invalid Username & Password");
        //        }
        //        return BadRequest(new ApiMessageDto { Message = "Your License Expired", Type = (short)LoginStatusType.LicenseExpired });
        //    }
        //    return BadRequest("Invalid CINNumber");
        //}

        //[HttpPost]
        //public async Task<IActionResult> Post([FromBody] CheckCINServerMetaDataDto input)
        //{
        //    Log.Info("ValidateUser() Method Begins");

        //    //var userData = await Mediator.Send(new LoginQuery { loginUserDto = loginModel });
        //    var metaData = await _context.MetaDataList.FirstOrDefaultAsync(e => e.CINNumber == input.CINNumber);
        //    //if (metaData is not null)
        //    //{
        //    //    Log.Info("Calling the Valida CIN Number of the USER Success =>");
        //    //    return Ok(new APIPayload<BaseLoginUserDTO> { Data = userData });
        //    //}
        //    //return BadRequest(ViewResources.Resource.Invoice_Login_MsgError);

        //    if (metaData is not null)
        //    {
        //        var userData = GenerateJSONWebToken(new BaseCINLoginUserDTO()
        //        {
        //            Id = metaData.Id,
        //            APIEndpoint = metaData.APIEndpoint,
        //            CINNumber = metaData.CINNumber,
        //            DBConnectionString = metaData.DBConnectionString,
        //        });
        //        return Ok(userData);
        //    }
        //    return BadRequest("Invalid CINNumber");
        //}

    }
}
