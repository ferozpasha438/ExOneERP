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
    public class RoutePlanController :BaseController
    {

        private readonly IConfiguration _Config;

        public RoutePlanController(IOptions<AppSettingsJson> appSettings, IConfiguration config) : base(appSettings)
        {
            _Config = config;
            //_cinDbContext = cinDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {
            var list = await Mediator.Send(new GetRoutePlanList() { Input = filter, User = UserInfo() });
            return Ok(list);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RoutePlanHeaderDto dTO)
        {
            var list = await Mediator.Send(new CreateRoutePlanInfo() { RoutePlanInfoDto = dTO, User = UserInfo() });
            return Ok(list);
        }

   
        [HttpGet("GetRoutePlanInfoById/{id}")]
        public async Task<IActionResult> GetRoutePlanInfoById(int id)
        {
            var obj = await Mediator.Send(new GetRoutePlanInfoById() { Id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }


        [HttpGet("getRoutePlanByRouteCode/{routeCode}")]
        public async Task<IActionResult> Get([FromRoute] string routeCode)
        {
            var obj = await Mediator.Send(new GetRoutePlanByRouteCode() { RouteCode =routeCode , User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
    }
}
