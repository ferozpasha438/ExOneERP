using CIN.Application;
using CIN.Application.Common;
using CIN.Application.SchoolMgtDto;
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
    public class WebNotificationController : BaseController
    {
        private readonly IConfiguration _Config;

        public WebNotificationController(IOptions<AppSettingsJson> appSettings, IConfiguration config) : base(appSettings)
        {
            _Config = config;
        }
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {
            var list = await Mediator.Send(new GetIndividualNotificationList() { Input = filter, User = UserInfo() });
            return Ok(list);
        }
        [HttpGet("{Code}/{TypeId}")]
        public async Task<IActionResult> Get([FromRoute] string code, int typeId)
        {
            var obj = await Mediator.Send(new GetIndividualNotificationById() { Code = code, TypeId = typeId, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] IndividualNotificationsDto dTO)
        {
            var id = await Mediator.Send(new SaveWebIndividualNotification() { notificationDTO = dTO, User = UserInfo() });
            if (id > 0)
                return Created($"get/{id}", dTO);
            else if (id == -1)
            {
                return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Duplicate(nameof(dTO.Id)) });
            }
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }
        [HttpPost("CreateNotification")]
        public async Task<ActionResult> CreateNotification([FromBody] BulkNotificationsDto dTO)
        {
            var id = await Mediator.Send(new CreateNotification() { notificationDTO = dTO, User = UserInfo() });
            if (id > 0)
                return Created($"get/{id}", dTO);
            else if (id == -1)
            {
                return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Duplicate(nameof(dTO.Id)) });
            }
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }
        [HttpGet("GetNotificationById/{Id}")]
        public async Task<IActionResult> GetNotificationById([FromRoute] int Id)
        {
            var obj = await Mediator.Send(new GetNotificationById() { Id= Id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpPost("NotificationApproval")]
        public async Task<IActionResult> NotificationApproval([FromBody] TeacherNotificationsDto dTO)
        {
            var obj = await Mediator.Send(new NotificationApprovalById() { Input = dTO, User = UserInfo() });
            return Ok(obj);
        }
        [HttpPost("BulkWebNotificationApproval")]
        public async Task<IActionResult> BulkWebNotificationApproval([FromBody] TeacherNotificationsDto dTo)
        {
            int rid = await Mediator.Send(new BulkWebNotificationApproval() { Id = dTo.Id, User = UserInfo() });
            return rid != 0 ? Ok(rid) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("GetWebTopNotifications")]
        public async Task<IActionResult> GetWebTopNotifications()
        {
            var obj = await Mediator.Send(new GetWebTopNotifications() { User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("SaveStudentTemplateNoticefication/{stuAdmNum}/{TypeId}")]
        public async Task<IActionResult> SaveStudentTemplateNoticefication([FromRoute] string stuAdmNum, [FromRoute] int typeId)
        {
            var nid = await Mediator.Send(new SaveStudentTemplateNoticefication() { StuAdmNum = stuAdmNum, TypeID = typeId, User = UserInfo() });
            return nid != 0 ? Ok(nid) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
    }
}
