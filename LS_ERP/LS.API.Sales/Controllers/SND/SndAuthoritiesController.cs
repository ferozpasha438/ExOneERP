using CIN.Application;
using CIN.Application.Common;
using CIN.Application.OperationsMgtDtos;

using CIN.Application.SndDtos;
using CIN.Application.SndQuery;
using CIN.Domain.OpeartionsMgt;
using LS.API.Sales.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LS.API.OPR.Controllers.SND
{
    public class SndAuthoritiesController : BaseController
    {

        public SndAuthoritiesController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }

       
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TblSndAuthoritiesDto dTO)
        {
            var id = await Mediator.Send(new CreateSndAuthorities() { Input = dTO, User = UserInfo() });
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

     

        [HttpGet("getSndAuthoritiesByUserId")]
        public async Task<IActionResult> GetSndAuthoritiesByUserId()
        {
            var obj = await Mediator.Send(new GetSndAuthoritiesByUserId() {User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
[HttpGet("getSndAuthoritiesListByUserId")]
        public async Task<IActionResult> GetSndAuthoritiesListByUserId()
        {
            var obj = await Mediator.Send(new GetSndAuthoritiesListByUserId() {User = UserInfo() });
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

         [HttpGet("getAuthorityByBranchCurrentUser/{branchCode}")]
        public async Task<IActionResult> getAuthorityByBranchCurrentUser([FromRoute] string branchCode)
        {
            var obj = await Mediator.Send(new GetAuthorityByBranchCurrentUser() { BranchCode=branchCode, User = UserInfo() });
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
