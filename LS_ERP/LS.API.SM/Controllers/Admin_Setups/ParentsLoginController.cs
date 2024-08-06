using CIN.Application;
using CIN.Application.Common.Email;
using CIN.Application.SchoolMgtDto;
using CIN.Application.SchoolMgtQuery;
using CIN.DB;
using CIN.Server;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace LS.API.SM.Controllers.Admin_Setups
{
    
    public class ParentsLoginController : ApiControllerBase
    {
        private readonly CINServerDbContext _cinDbContext;
        private readonly CINDBOneContext _context;
        private IConfiguration _Config;
        private readonly IOptions<AppMobileSettingsJson> _appSettings;
        //public ParentsLoginController(IConfiguration config, CINServerDbContext cinDbContext)
        public ParentsLoginController(IConfiguration config, CINServerDbContext cinDbContext, CINDBOneContext context, IOptions<AppMobileSettingsJson> appSettings)
        {
            _Config = config;
            _cinDbContext = cinDbContext;
            _context = context;
            _appSettings = appSettings;
        }

        [HttpPost("validatecin")]
        public async Task<IActionResult> ValidateCinNumber([FromBody] CheckCINParentServerDto Input)
        {
            var metaData = await _cinDbContext.MetaDataList.FirstOrDefaultAsync(e => e.CINNumber == Input.CINNumber);
            if ((metaData != null))
            {

                byte[] connectionString = ASCIIEncoding.ASCII.GetBytes(metaData.DBConnectionString);
                var DbConString = Convert.ToBase64String(connectionString);
                var BaseUrl = metaData.SchUrl;
                return Ok(new ParentLoginSuccessMessageDto { Message = "Successfully Validate CIN", Connectionstring = DbConString,BaseUrl=BaseUrl, Status = true });
            }

            return BadRequest(new ValidateCinFailedMessageDto { Message = "Invalid CIN Number", Status = false });
        }



        [HttpPost("validateparentlogin")]
        public async Task<IActionResult> ValidateParentLogin([FromBody] CheckParentLoginMetaDataDto Input)
        {
            var metaDataSchoolBranches = await _context.SchoolBranches.FirstOrDefaultAsync();
           
            if (Input.UserName != null && Input.Password != null)
            {
                var user = await Mediator.Send(new CheckParentslogin()
                {

                    Input = Input

                });
                if (user.Id == -1)
                {
                    return Ok(new ParentLoginMessageDto {
                        Message = "Successful Login",
                        StartWeekDay= metaDataSchoolBranches.StartWeekDay,
                        NumberOfWeekDays=Convert.ToInt32(metaDataSchoolBranches.NumberOfWeekDays),
                        WeekOff1=metaDataSchoolBranches.WeekOff1,
                        WeekOff2=metaDataSchoolBranches.WeekOff2,
                        CurrencyCode = metaDataSchoolBranches.CurrencyCode,
                        PrivacyPolicy= metaDataSchoolBranches.PrivacyPolicyUrl,
                        Website=metaDataSchoolBranches.Website,
                        Status = true });
                }

                return BadRequest(new ParentLoginMessageDto { Message = "Invalid Credential", Status = false });

            }
            return BadRequest(new ParentLoginMessageDto { Message = "Empty Username & Password", Status = false });
        }


        [HttpPost("Registraion")]
        public async Task<IActionResult> Registraion([FromBody] TblParentsLoginDto parentsRegistration)
        {
            var user = await Mediator.Send(new ParentsRegistraion() { parentsRegistration = parentsRegistration });
            if (user == 2)
            {
                return BadRequest(new ParentLoginMessageDto { Message = "Ward Number Not Exists ", Status = false });
            }
            if(user == 3)
            {
                return BadRequest(new ParentLoginMessageDto { Message = "Register Mobile Number Not Exists ", Status = false });
            }
            if (user == -2)
            {
                return BadRequest(new ParentLoginMessageDto { Message = "Username Already Exists ", Status = false });
            }
            if (user != null && user != -1)
            {
                return Ok(new ParentLoginMessageDto {Message= "Successfully Registration Done", Status=true });
            }
            return BadRequest(new ParentLoginMessageDto { Message = "Invalid Registration ", Status = false });

        }



        [HttpGet("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromQuery] string EmailId)
        {
            if (EmailId is not null)
            {
                var user = await Mediator.Send(new ForgotPassword()
                {

                    EmailId = EmailId

                });
                if (user is not null)
                {
                   // SentEmailNotification obj = new();
                    SentEmailNotification.Send(new EmailDtos()
                    {
                        To = "alilogicsystems@gmail.com",
                        Subject = "Good Morning",
                        MessageBody = "This is test Email from Shamsher"
                    }, _Config);
                    return Ok(new ForgotPasswordDto { Password = user.Password,Status=true});
                }

                return BadRequest(new ParentLoginMessageDto { Message = "Invalid Email", Status = false });

            }
            return BadRequest(new ParentLoginMessageDto { Message = "Empty Email ", Status = false });
        }


        [HttpGet("GetOTP")]
        public async Task<IActionResult> GetOTP([FromQuery] int OTP,string Mobile)
        {
            if (OTP != 0 && Mobile is not null)
            {
                var user = await Mediator.Send(new GetOtpByMobile()
                {
                    OTP = OTP,
                    Mobile=Mobile
                });
                if (user is not null)
                {
                    return Ok(new OTPMobileDto { OTP = user.OTP,Mobile=user.Mobile,Status=true});
                }

                return BadRequest(new ParentLoginMessageDto { Message = "Invalid Input", Status = false });

            }
            return BadRequest(new ParentLoginMessageDto { Message = "Invalid Input", Status = false });
        }


        [HttpPost("ParentAddRequest")]
        public async Task<ActionResult> ParentAddRequest([FromBody] TblParentAddRequestDto dTO)
        {
            var id = await Mediator.Send(new CreateParentAddRequest() { Input = dTO });
            if (id > 0)
                return Created($"get/{id}", dTO);
            else if (id == -1)
            {
                return BadRequest(new MobileApiMessageDto { Message = ApiMessageInfo.Duplicate(nameof(dTO.Id)),Status=true });
            }
            return BadRequest(new MobileApiMessageDto { Message = ApiMessageInfo.Failed,Status=false });
        }


    }


}

