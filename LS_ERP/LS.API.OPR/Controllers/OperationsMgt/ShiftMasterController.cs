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
    public class ShiftMasterController : BaseController
    {

        public ShiftMasterController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }
        #region ShiftMaster
        [HttpGet("getShiftMastersPagedList")]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {

            var list = await Mediator.Send(new GetShiftMastersPagedList() { Input = filter.Values(), User = UserInfo() });
            return Ok(list);
        }
        //[HttpPost]
        //public async Task<ActionResult> Create([FromBody] HRM_DEF_EmployeeShiftMasterAddUpdateDto dTO)
        //{
        //    var id = await Mediator.Send(new CreateShiftMaster() { ShiftMasterDto = dTO, User = UserInfo() });
        //    if (id > 0)
        //    {
        //        return Created($"get/{id}", dTO);
        //    }
        //    else if (id == -1)
        //    {
        //        return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Duplicate(nameof(dTO.ShiftCode)) });
        //    }
        //    return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });

        //}
        


        [HttpGet("getShiftMasterById/{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var obj = await Mediator.Send(new GetShiftMasterById() { Id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("getShiftMasterByShiftMasterCode/{ShiftMasterCode}")]
        public async Task<IActionResult> Get([FromRoute] string ShiftMasterCode)
        {
            var obj = await Mediator.Send(new GetShiftMasterByShiftMasterCode() { ShiftMasterCode = ShiftMasterCode, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

       



        [HttpGet("getSelectShiftMasterList")]
        public async Task<IActionResult> GetSelectShiftMasterList()
        {
            var item = await Mediator.Send(new GetSelectShiftMasterList() { User = UserInfo() });
            return Ok(item);
        }

         [HttpGet("getSelectShiftMasterListForProjectSite/{projectCode}/{siteCode}")]                // based on sitecode we can get working hours and it filters
        public async Task<IActionResult> GetSelectShiftMasterListForSite([FromRoute] string SiteCode,[FromRoute] string ProjectCode)
        {
            var item = await Mediator.Send(new GetSelectShiftMasterListForProjectSite() {projectCode=ProjectCode,siteCode=SiteCode, User = UserInfo() });
            return Ok(item);
        }


        //[HttpDelete("{id}")]
        //public async Task<ActionResult> Delete([FromRoute] int id)
        //{
        //    var ShiftMasterId = await Mediator.Send(new DeleteShiftMaster() { Id = id, User = UserInfo() });
        //    if (ShiftMasterId > 0)
        //        return NoContent();
        //    return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        //}
#endregion

        #region ShiftsPlanForProject

        [HttpPost("CreateUpdateShiftsPlanForProject")]
        public async Task<ActionResult> Create([FromBody] OpShiftsPlanForprojectDto dTO)
        {
            var id = await Mediator.Send(new CreateUpdateShiftsPlanForProject() { Shifts = dTO, User = UserInfo() });
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



        [HttpGet("getShiftsByProjectAndSiteCode/{projectCode}/{siteCode}")]
        public async Task<IActionResult> GetShiftsByProjectAndSiteCode( [FromRoute] string projectCode, [FromRoute] string siteCode)
        {
            var obj = await Mediator.Send(new GetShiftsByProjectAndSiteCode() { SiteCode = siteCode, ProjectCode = projectCode, User = UserInfo() });
            return Ok(obj);
        }
         [HttpGet("canDeleteShiftForProjectAndSiteCode/{shiftCode}/{projectCode}/{siteCode}")]
        public async Task<bool>CanDeleteShiftForProjectAndSiteCode([FromRoute] string shiftCode, [FromRoute] string projectCode, [FromRoute] string siteCode)
        {
            return await Mediator.Send(new CanDeleteShiftForProjectAndSiteCode() { ShiftCode=shiftCode,SiteCode = siteCode, ProjectCode = projectCode, User = UserInfo() });
        }

         [HttpGet("getShiftsByProjectAndSiteCode2/{projectCode}/{siteCode}")]
        public async Task<IActionResult> GetShiftsByProjectAndSiteCode2( [FromRoute] string projectCode, [FromRoute] string siteCode)
        {
            var obj = await Mediator.Send(new GetShiftsByProjectAndSiteCode2() { SiteCode = siteCode, ProjectCode = projectCode, User = UserInfo() });
            return Ok(obj);
        }








       


        

        #endregion
//#region ShiftsToSites

//        [HttpPost("CreateUpdateShiftsToSiteMaps")]
//        public async Task<ActionResult> Create([FromBody] OpShiftSiteMapDto dTO)
//        {
//            var id = await Mediator.Send(new CreateUpdateShiftsToSiteMap() { Shifts = dTO, User = UserInfo() });
//            if (id > 0)
//            {
//                return Created($"get/{id}", dTO);
//            }
//            else if (id == -1)
//            {
//                return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Duplicate(nameof(dTO.SiteCode)) });
//            }
//            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });

//        }



//        //[HttpGet("getShiftsToSiteBySiteCode/{siteCode}")]
//        //public async Task<IActionResult> GetShiftsToSiteBySiteCode([FromRoute] string siteCode)
//        //{
//        //    var obj = await Mediator.Send(new GetShiftsToSiteBySiteCode() { SiteCode = siteCode, User = UserInfo() });
//        //    return Ok(obj);
//        //}

        





//        [HttpGet("getSelectShiftsToSiteBySiteCode/{siteCode}")]
//        public async Task<IActionResult> GetSelectShiftsToSiteBySiteCode([FromRoute] string siteCode)
//        {
//            var item = await Mediator.Send(new GetSelectShiftsToSiteBySiteCode() {SiteCode=siteCode, User = UserInfo() });
//            return Ok(item);
//        }


        

//        #endregion





    }
}
