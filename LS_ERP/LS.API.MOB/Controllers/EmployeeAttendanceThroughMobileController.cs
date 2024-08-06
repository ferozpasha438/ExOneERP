using CIN.Application;
using CIN.Application.Common;
using CIN.Application.MobileMgt.Dtos;
using CIN.Application.MobileMgt.Queries;
using CIN.Application.OperationsMgtDtos;
using CIN.Application.SystemQuery;
using CIN.Server;
using Microsoft.AspNetCore.Hosting;
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
    public class EmployeeAttendanceThroughMobileController : ApiControllerBase
    {
        private readonly CINServerDbContext _cinDbContext;
        private IConfiguration _Config;
        private readonly IWebHostEnvironment _env;
        private readonly IOptions<AppMobileSettingsJson> _appSettings;
        //  public ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public EmployeeAttendanceThroughMobileController(IConfiguration config, CINServerDbContext cinDbContext, IOptions<AppMobileSettingsJson> appSettings, IWebHostEnvironment env)
        {
            _Config = config;
            _cinDbContext = cinDbContext;
            _appSettings = appSettings;
            _env = env;
        }

    
         [HttpPost("enterEmployeeAttendanceThroughMobile")]
        public async Task<ActionResult> EnterEmployeeAttendanceThroughMobile([FromBody] InputEnterEmployeeAttendanceThroughMobileDto input)
        {

            try
            {
                input.WebRootForAttendanceImages = $"{_env.ContentRootPath}/AttendanceImages";
               

                if (!System.IO.Directory.Exists(input.WebRootForAttendanceImages))
                    System.IO.Directory.CreateDirectory(input.WebRootForAttendanceImages);



                var (status, message) = await Mediator.Send(new EnterEmployeeAttendanceThroughMobileQuery()
                {
                    Input = input
                });

                if (status == true)
                {
                    return Ok(new MobileApiMessageDto { Message = ApiMessageInfo.Success, Status = true });
                }

                return BadRequest(new MobileApiMessageDto { Message = message, Status = false });

            }
            catch (Exception ex)
            {
                return BadRequest(new MobileApiMessageDto { Message = ex.Message, Status = false });
            }
        }
         [HttpGet("getEmployeeTodaysShifts/{todayDate}/{employeeNumber}")]
        public async Task<List<MobEmployeeScheduledSchiftsDto>> GetEmployeeTodaysShifts([FromRoute] DateTime todayDate,[FromRoute] string employeeNumber)
        {

            
                return (await Mediator.Send(new GetEmployeeTodaysShiftsQuery() {
                
                 EmployeeNumber=employeeNumber,
                TodayDate=todayDate
                }));
            }        

         [HttpPost("updateEmployeeGeoLocationLog")]
        public async Task<ActionResult> UpdateEmployeeGeoLocationLog([FromBody] TblMobMgtGeoLocationLogsDto input)
        {

          
                var (Id,Message)=await Mediator.Send(new UpdateEmployeeGeoLocationLogQuery()
                {
                    Input=input
                });
                if (Id > 0)
                {
                  return  Ok(new { Id, Message });
                }
                else
                {
                    return BadRequest(new { Id, Message});
                }
            
   
        }

          [HttpPost("logoutFromShiftByAttnId")]
        public async Task<ActionResult> LogoutFromShiftByAttnId([FromBody] InputEmployeeLogoutDto input)
        {
            input.WebRootForAttendanceImages = $"{_env.ContentRootPath}/AttendanceImages";


            if (!System.IO.Directory.Exists(input.WebRootForAttendanceImages))
                System.IO.Directory.CreateDirectory(input.WebRootForAttendanceImages);


            var (Status, Message) = await Mediator.Send(new LogoutFromShiftByAttnIdQuery()
            {
                Input = input
            });
            if (Status == false)
                return BadRequest(new MobileApiMessageDto { Message = Message, Status = Status });

            else
                return Ok((new MobileApiMessageDto { Message = Message, Status = Status }));
        }


    }

}