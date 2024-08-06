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
    public class OpUtilsController : BaseController
    {

        public OpUtilsController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }







        [HttpPost("RefreshOffs")]
        public async Task<ActionResult> RefreshOffs([FromBody] RefreshOffsDto Dto)
        {
            var id = await Mediator.Send(new RefreshOffs() { User = UserInfo(), dto = Dto });
            if (id > 0)
                return Ok(new ApiMessageDto { Message = ApiMessageInfo.Success });

            return BadRequest(new ApiMessageDto { Message = "Offs Updation Failed" });
        }


        [HttpPost("assignEmployeesToProjectSites")]
        public async Task<ActionResult> AssignEmployeesToProjectSites([FromBody] AssignEmployeesToProjectSitesDto dto)
        {
            try
            {

                var (res, msg) = await Mediator.Send(new AssignEmployeesToProjectSitesQuery() { Dto = dto });
                if (res)
                    return Ok(new ApiMessageDto { Message = ApiMessageInfo.Success });

                return BadRequest(new ApiMessageDto { Message = msg });
            }
            catch (Exception e)
            {
                return BadRequest(new ApiMessageDto { Message = e.Message });
            }
        }

         [HttpPost("mapEmployeesToPvAddResReq")]
        public async Task<ActionResult> MapEmployeesToProjectSites([FromBody] MapEmployeesToPvAddResReqDto dto)
        {
            try
            {
                if (dto.IsNoOffDay)
                {
                    dto.OffDaysString = "";
                }

                var (res, msg) = await Mediator.Send(new MapEmployeesToPvAddResReqQuery() { Dto = dto });
                if (res)
                    return Ok(new ApiMessageDto { Message = ApiMessageInfo.Success });

                return BadRequest(new ApiMessageDto { Message = msg });
            }
            catch (Exception e)
            {
                return BadRequest(new ApiMessageDto { Message = e.Message });
            }
        }


    }
}
