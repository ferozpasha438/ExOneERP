using CIN.Application;
using CIN.Application.Common;
using CIN.Application.OperationsMgtDtos;
using CIN.Application.OperationsMgtQuery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LS.API.OPR.Controllers.OperationsMgt
{
    public class MonthlyRoasterController : BaseController
    {

        public MonthlyRoasterController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }
        

        #region MonthlyRoasterForSite

        [HttpPost("generateMonthlyRoastersForSite")]
        public async Task<ActionResult> GenerateMonthlyRoastersForSite([FromBody] List<InputTblOpMonthlyRoasterForSiteDto> dto)
        {
            var id = await Mediator.Send(new GenerateMonthlyRoastersForSite() { Input = dto, User = UserInfo() });
            if (id > 0)
            {
                return Created($"get/{id}", dto);
            }
            else if (id == -1)
            {
                return BadRequest(new ApiMessageDto { Message = "Roaster_Already_Exist"});
            }
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });

        }



        [HttpGet("getMonthlyRoasterForSite/{customerCode}/{projectCode}/{siteCode}/{month}/{year}")]
        public async Task<IActionResult> GetMonthlyRoasterForSite([FromRoute] string customerCode, [FromRoute] string projectCode, [FromRoute] string siteCode, [FromRoute] short month, [FromRoute] short year )
        {
            var obj = await Mediator.Send(new GetMonthlyRoasterForSite() {ProjectCode=projectCode, CustomerCode = customerCode, SiteCode = siteCode, Month = month, Year = year, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("isExistMonthlyRoasterForProjectSite/{projectCode}/{siteCode}")]
        public async Task<bool> isExistMonthlyRoasterForProjectSite([FromRoute] string projectCode, [FromRoute] string siteCode)
        {
            bool res = await Mediator.Send(new IsExistMonthlyRoasterForProject() { ProjectCode = projectCode,SiteCode= siteCode, User = UserInfo() });
            return res;
        }
 [HttpGet("isExistMonthlyRoasterForProjectSiteMonth/{projectCode}/{siteCode}/{month}/{year}")]
        public async Task<bool> isExistMonthlyRoasterForProjectSiteMonth([FromRoute] string projectCode, [FromRoute] string siteCode, [FromRoute] short month, [FromRoute] short year)
        {
            bool res = await Mediator.Send(new IsExistMonthlyRoasterForSiteMonth() { ProjectCode = projectCode,SiteCode= siteCode,Month=month,Year=year, User = UserInfo() });
            return res;
        }

        [HttpPost("updateShiftsForMonthlyRoaster")]
        public async Task<ActionResult> Create([FromBody] List<TblOpMonthlyRoasterForSiteDto> dTO)
        {
            var id = await Mediator.Send(new UpdateShiftsForMonthlyRoaster() { Roasters = dTO, User = UserInfo() });
            if (id > 0)
            {
                return Created($"get/{id}", dTO);
            }
            else if (id == -1)
            {
                return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Duplicate(nameof(dTO)) });
            }
            else if(id==-2)
            {
                return BadRequest(new ApiMessageDto { Message = "Attendance_Already_Entered" });
            }
else if(id==-3)
            {
                return BadRequest(new ApiMessageDto { Message = "InComplete_Shifts_Assignment" });
            }

            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });

        }






        #endregion





        #region MonthlyRoaster

        [HttpPost("createUpdateMonthlyRoaster")]
        public async Task<ActionResult> Create([FromBody] OpMonthlyRoastersDto dTO)
        {
            var id = await Mediator.Send(new CreateUpdateMonthlyRoaster() { Roasters = dTO, User = UserInfo() });
            if (id > 0)
            {
                return Created($"get/{id}", dTO);
            }
            else if (id == -1)
            {
                return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Duplicate(nameof(dTO.SiteCode)) });
            }
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });

        }



        [HttpGet("getMonthlyRoaster/{customerCode}/{siteCode}/{month}/{year}")]
        public async Task<IActionResult> Get([FromRoute] string customerCode, [FromRoute] string siteCode, [FromRoute] short month, [FromRoute] short year)
        {
            var obj = await Mediator.Send(new GetMonthlyRoaster() { CustomerCode= customerCode, SiteCode= siteCode, Month= month, Year=year, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        #endregion


        #region SingleRoaster

        [HttpPost("updateShiftCodeForDay")]
        public async Task<int> Create([FromBody] UpdateShiftCodeForDayDto dTO)
        {
            var id = await Mediator.Send(new UpdateShiftCodeForDay() { Input = dTO, User = UserInfo() });
            return id;

        }
        [HttpPost("getSingleRoasterForEmployee")]
        public async Task<TblOpMonthlyRoasterForSiteDto> GetSingleRoasterForEmployee([FromBody] InputEmployeeSingleRoasterDto dTO)
        {
            var roaster = await Mediator.Send(new GetSingleRoasterForEmployee() { Input = dTO, User = UserInfo() });
            return roaster;

        }


        #endregion


    }
}
