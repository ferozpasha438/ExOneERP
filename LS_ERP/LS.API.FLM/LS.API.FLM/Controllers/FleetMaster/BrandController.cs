using CIN.Application;
using CIN.Application.Common;
using CIN.Application.FleetMgtDtos;
using CIN.Application.FleetMgtQuery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LS.API.FLM.Controllers.FleetMaster
{
    public class BrandController : BaseController
    {
        private readonly IConfiguration _Config;

        public BrandController(IOptions<AppSettingsJson> appSettings, IConfiguration config) : base(appSettings)
        {
            _Config = config;
            //_cinDbContext = cinDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {
            var list = await Mediator.Send(new GetBrandMasterList() { Input = filter, User = UserInfo() });
            return Ok(list);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var obj = await Mediator.Send(new GetBrandMasterById() { Id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }


        [HttpPost]
        public async Task<ActionResult> Create([FromBody] VehicleBrandMasterDto dTO)
        {
            var id = await Mediator.Send(new CreateUpdateBrandMaster() { BrandMasterDto = dTO, User = UserInfo() });
            if (id > 0)
                return Created($"get/{id}", dTO);
            else if (id == -1)
            {
                return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Duplicate(nameof(dTO.Id)) });
            }
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }



        [HttpDelete("DeleteBrand")]
        public async Task<ActionResult> DeleteBrand([FromQuery] int id)
        {
            var brandId = await Mediator.Send(new DeleteBrandMaster() { Id = id, User = UserInfo() });
            if (brandId > 0)
                return Ok(new MobileApiMessageDto { Message = "Successfully Deleted", Status = true });
            return BadRequest(new MobileApiMessageDto { Message = ApiMessageInfo.Failed, Status = false });
        }


    }
}
