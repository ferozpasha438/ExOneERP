using AutoMapper;
using CIN.Application;
using CIN.Application.Common;
using CIN.Application.TeacherMgtDtos;
using CIN.Application.TeacherAppMgtQuery;
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

namespace LS.API.TeacherApp.Controllers.Teacher_Admin
{
    public class TeacherNotificationController:BaseController
    {
        private IConfiguration _Config;

        public TeacherNotificationController(IOptions<AppSettingsJson> appSettings, IConfiguration config):base(appSettings)
        {
            _Config = config;
        }

        [HttpGet("GetTeacherNotificationList")]
        public async Task<IActionResult> Get([FromQuery] string teacherCode)
        {

            var list = await Mediator.Send(new GetTeacherNotificationList() { TeacherCode = teacherCode, User = UserInfo() });
            return Ok(list);
        }

       [HttpGet("GetTeacherNotificationCount")]
       public async Task<IActionResult> GetNotificationCount( [FromQuery]string teacherCode) 
        {

            var result = await Mediator.Send(new GetTeacherNotificationCount() { TeacherCode = teacherCode, User = UserInfo() });
            return Ok(result);
        }


        [HttpPost("UpdateTeacherNotification")]
        public async Task<ActionResult> UpdateNotification(int messageId)
        {
            var id = await Mediator.Send(new TeacherUpdateNotification() { MessageId = messageId, User = UserInfo() });
            if (id > 0)
                return Ok(new UpdateNotificationSuccessDto { Message = ApiMessageInfo.Success, Status = true });
            else if (id == -1)
            {
                return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Duplicate(nameof(messageId)) });
            }
            return BadRequest(new UpdateNotificationFailedDto { Message = ApiMessageInfo.Failed,Status=false });
        }

        //[HttpPost("SendStaffTeacherNotification")]
        //public async Task<ActionResult> SendNotification([FromBody] SendNotificationDto dTO)
        //{
        //    var id = await Mediator.Send(new SendParentNotification() { sendNotificationDTO = dTO, User = UserInfo() });
        //    if (id > 0)
        //        return Created($"get/{id}", dTO);
        //    else if (id == -1)
        //    {
        //        return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Duplicate(nameof(dTO.Id)) });
        //    }
        //    return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        //}


    }
}
