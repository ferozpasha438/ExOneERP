using CIN.Application;
using CIN.Application.Common;
using CIN.Application.SchoolMgtDto;
using CIN.Application.SchoolMgtDtos;
using CIN.Application.SchoolMgtQuery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LS.API.SM.Controllers.Notifications
{
    public class ParentNotificationController :BaseController
    {
        private readonly IConfiguration _Config;

        public ParentNotificationController(IOptions<AppSettingsJson> appSettings, IConfiguration config) : base(appSettings)
        {
            _Config = config;
            //_cinDbContext = cinDbContext;
        }



        

        #region School Parent Notification List

        [HttpGet("ParentPushNotificationList")]
        public async Task<IActionResult> Get([FromQuery] string mobile)
        {

            var list = await Mediator.Send(new GetParentPushNotificationList() { Mobile=mobile, User = UserInfo() });
            return Ok(list);
        }

        #endregion


        [HttpGet("ParentNotificationCount")]
        public async Task<IActionResult> ParentNotificationCount([FromQuery] string mobile)
        {

            var list = await Mediator.Send(new GetParentNotificationCount() { Mobile = mobile, User = UserInfo() });
            return Ok(list);
        }



        [HttpPost("UpdateNotification")]
        public async Task<ActionResult> UpdateNotification(int messageId)
        {
            var id = await Mediator.Send(new UpdateNotification() { MessageId = messageId, User = UserInfo() });
            if (id > 0)
                return Ok(new UpdateNotificationSuccessDto { Message = ApiMessageInfo.Success,Status=true});
            else if (id == -1)
            {
                return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Duplicate(nameof(messageId)) });
            }
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }

        [HttpPost("SendStaffTeacherNotification")]
        public async Task<ActionResult> SendNotification([FromBody] SendNotificationDto dTO)
        {
            var id = await Mediator.Send(new SendParentNotification() { sendNotificationDTO = dTO, User = UserInfo() });
            if (id > 0)
                return Created($"get/{id}", dTO);
            else if (id == -1)
            {
                return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Duplicate(nameof(dTO.Id)) });
            }
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }



        [HttpGet("GetPickupNotificationTemplate")]
        public async Task<IActionResult> GetPickupNotificationTemplate([FromQuery] string templateType)
        {

            var list = await Mediator.Send(new GetPickupNotificationTemplate() { TemplateType = templateType, User = UserInfo() });
            return Ok(list);
        }
    }
}
