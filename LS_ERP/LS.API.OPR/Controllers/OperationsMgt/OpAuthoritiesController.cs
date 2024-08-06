using CIN.Application;
using CIN.Application.Common;
using CIN.Application.OperationsMgtDtos;
using CIN.Application.OperationsMgtQuery;
using CIN.Domain.OpeartionsMgt;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LS.API.OPR.Controllers.OperationsMgt
{
    public class OpAuthoritiesController : BaseController
    {

        public OpAuthoritiesController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }

       
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TblOpAuthoritiesDto dTO)
        {
            var id = await Mediator.Send(new CreateOpAuthorities() { Input = dTO, User = UserInfo() });
            if (id > 0)
                return Created($"get/{id}", dTO);
            else if (id == -1)
            {
                return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Duplicate(nameof(dTO.AppAuth)) });
            }
            else if (id == -2)
            {
                return BadRequest(new ApiMessageDto { Message = "Empty_Authorities" });
            }
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }

     

        [HttpGet("getOpAuthoritiesByUserId")]
        public async Task<IActionResult> GetOpAuthoritiesByUserId()
        {
            var obj = await Mediator.Send(new GetOpAuthoritiesByUserId() {User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
[HttpGet("getOpAuthoritiesListByUserId")]
        public async Task<IActionResult> GetOpAuthoritiesListByUserId()
        {
            var obj = await Mediator.Send(new GetOpAuthoritiesListByUserId() {User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("getAuthorityById/{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var obj = await Mediator.Send(new GetAuthorityById() { Id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }


        [HttpGet("getAuthorityByBranchUserId/{branchCode}/{userId}")]
        public async Task<IActionResult> Get([FromRoute] string branchCode, [FromRoute] int userId)
        {
            var obj = await Mediator.Send(new GetAuthorityByBranchUserId() { BranchCode=branchCode,UserId = userId, User = UserInfo() });
            //return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
            return Ok(obj);
        }








        [HttpGet("getAuthoritiesPagedList")]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {

            var list = await Mediator.Send(new GetAuthoritiesPagedList() { Input = filter.Values(), User = UserInfo() });
            return Ok(list);
        }





    }
}
