﻿using CIN.Application;
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

namespace LS.API.SM.Controllers.Admin_Setups
{
    public class SchoolFeeTypeController :BaseController
    {
        private readonly IConfiguration _Config;

        public SchoolFeeTypeController(IOptions<AppSettingsJson> appSettings, IConfiguration config) : base(appSettings)
        {
            _Config = config;
            //_cinDbContext = cinDbContext;
        }



        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {
            var list = await Mediator.Send(new GetSysSchoolFeeTypeList() {Input=filter, User = UserInfo() });
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var obj = await Mediator.Send(new GetSysSchoolFeeTypeById() { Id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TblSysSchoolFeeTypeDto dTO)
        {
            // var id = await Mediator.Send(new CreateUpdateSchoolSemister() { schoolFee = dTO, User = UserInfo() });
            var id = await Mediator.Send(new CreateUpdateSchoolFeeType() { SchoolFeeTypeDto = dTO, User = UserInfo() });
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
            var feeterms = await Mediator.Send(new DeleteSchoolFeeType() { Id = id, User = UserInfo() });
            if (feeterms > 0)
                return NoContent();
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }
    }
}
