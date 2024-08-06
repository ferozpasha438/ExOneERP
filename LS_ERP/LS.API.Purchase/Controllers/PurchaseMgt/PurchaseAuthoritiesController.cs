using CIN.Application;
using CIN.Application.Common;
using CIN.Application.PurchaseMgtDtos;
using CIN.Application.PurchasemgtQuery;
//using CIN.Application.OperationsMgtDtos;
//using CIN.Application.OperationsMgtQuery;
using CIN.Domain.OpeartionsMgt;
using CIN.Domain.PurchaseMgt;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LS.API.Purchase.Controllers.Purchasemgt
{
    public class PurchaseAuthoritiesController : BaseController
    {

        public PurchaseAuthoritiesController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }


        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TrnPurAuthoritiesDto dTO)
        {
            var id = await Mediator.Send(new CreateOpAuthorities() { Input = dTO, User = UserInfo() });
            if (id > 0)
                return Created($"get/{id}", dTO);
            else if (id == -1)
            {
                return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Duplicate(nameof(dTO.AppAuth)) });
            }
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }



        [HttpGet("getOpAuthoritiesByUserId")]
        public async Task<IActionResult> GetOpAuthoritiesByUserId()
        {
            var obj = await Mediator.Send(new GetOpAuthoritiesByUserId() { User = UserInfo() });
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
            var obj = await Mediator.Send(new GetAuthorityByBranchUserId() { BranchCode = branchCode, UserId = userId, User = UserInfo() });
            //return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
            return Ok(obj);
        }

       
        [HttpPost("PurApprovals")]
        public async Task<ActionResult> Create([FromBody] TblPurTrnApprovalsDto dTO)
        {
            var id = await Mediator.Send(new CreatePurApprovals() { Input = dTO, User = UserInfo() });
            if (id > 0)
                return Created($"get/{id}", dTO);
            else if (id == -1)
            {
                return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Duplicate(nameof(dTO.AppAuth)) });
            }
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }






        [HttpGet("getAuthoritiesPagedList")]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {

            var list = await Mediator.Send(new GetAuthoritiesPagedList() { Input = filter.Values(), User = UserInfo() });
            return Ok(list);
        }

        [HttpGet("getSelectBranchCodeList")]
        public async Task<IActionResult> GetSelectBranchCodeList()
        {
            var obj = await Mediator.Send(new GetSelectBranchCodeList() { User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("GetUserSelectionList")]
        public async Task<List<CustomSelectListItem>> GetUserSelectionList()
        {
            var users = await Mediator.Send(new GetUserSelectionList() { User = UserInfo() });
            return users;
        }



    }
}
