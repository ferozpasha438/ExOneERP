﻿using CIN.Application;
using CIN.Application.Common;
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

namespace LS.API.SM.Controllers.StudentMgmt
{
    public class StudentAcademicsController  : BaseController
    {
        private IConfiguration _Config;

        public StudentAcademicsController(IOptions<AppSettingsJson> appSettings, IConfiguration config) : base(appSettings)
        {
            _Config = config;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {
            var list = await Mediator.Send(new GetAllStudentAcademicsList() { Input = filter, User = UserInfo() });
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var obj = await Mediator.Send(new GetStudentAcademicById() { Id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });

        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TblDefStudentAcademicsDto dTO)
        {
            var id = await Mediator.Send(new CreateUpdateStudentAcademics() { Input = dTO, User = UserInfo() });
            if (id > 0)
                return Created($"get/{id}", dTO);
            else if (id == -1)
            {
                return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Duplicate(nameof(dTO.Id)) });
            }
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var studentcAcademic = await Mediator.Send(new DeleteStudentAcademic() { Id = id, User = UserInfo() });
            if (studentcAcademic > 0)
                return NoContent();
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }

    }
}
