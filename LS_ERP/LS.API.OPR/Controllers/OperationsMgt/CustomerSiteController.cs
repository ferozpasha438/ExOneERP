using CIN.Application;
using CIN.Application.Common;
using CIN.Application.OperationsMgtDtos;
using CIN.Application.OperationsMgtQuery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LS.API.OPR.Controllers.OperationsMgt
{
    
    public class CustomerSiteController : BaseController
    {

        public CustomerSiteController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }

        [HttpGet("getCustomerSitesPagedList")]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {
         
            var list = await Mediator.Send(new GetCustomerSitesPagedList() { Input = filter.Values(), User = UserInfo() });
            return Ok(list);
        }
        [HttpGet("getCustomerSitesByCustCodePagedList")]
        public async Task<IActionResult> GetCustomerSitesByCustCodePagedList([FromQuery] PaginationFilterDto filter)
        {

            var list = await Mediator.Send(new GetCustomerSitesByCustCodePagedList() { Input = filter.Values(), User = UserInfo() });
            return Ok(list);
        }
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TblSndDefSiteMasterDto dTO)
        {
            var id = await Mediator.Send(new CreateSite() { SiteDto = dTO, User = UserInfo() });
            if (id > 0)
                return Created($"get/{id}", dTO);
            else if (id == -1)
            {
                return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Duplicate(nameof(dTO.SiteCode)) });
            }

            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }

        [HttpGet("getSiteById/{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var obj = await Mediator.Send(new GetSiteById() { Id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("getSiteBySiteCode/{siteCode}")]
        public async Task<IActionResult> Get([FromRoute] string siteCode)
        {
            var obj = await Mediator.Send(new GetSiteBySiteCode() { SiteCode = siteCode, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("getSelectSiteList")]
        public async Task<IActionResult> GetSelectSiteList(string search)
        {
            //await Task.Delay(3000);
            var item = await Mediator.Send(new GetSelectSiteList() { Input = search, User = UserInfo() });
            return Ok(item);
        }

        [HttpGet("getSelectSiteList2")]             //HRM Project sites
        public async Task<IActionResult> GetSelectSiteList2(string search)
        {
            //await Task.Delay(3000);
            var item = await Mediator.Send(new GetSelectSiteList2() { Input = search, User = UserInfo() });
            return Ok(item);
        }

       


        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var siteId = await Mediator.Send(new DeleteSite() { Id = id, User = UserInfo() });
            if (siteId > 0)
                return NoContent();
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }

        [HttpGet("getSelectSiteListByCustCode/{custCode}")]
        public async Task<IActionResult> GetSelectSiteListbyCustCode([FromRoute] string custCode)
        {
            //await Task.Delay(3000);
            var item = await Mediator.Send(new GetSelectSiteListbyCustCode() { Input = custCode, User = UserInfo() });
            return Ok(item);
        }


        [HttpGet("getSelectSiteListByProjectCode/{projectCode}")]
        public async Task<List<TblSndDefSiteMasterDto>> GetSelectSiteListByProjectCode([FromRoute] string projectCode)
        {
            //await Task.Delay(3000);
            var items = await Mediator.Send(new GetSelectSiteListByProjectCode() { ProjectCode = projectCode, User = UserInfo() });
            return items;
        }


        [HttpPost("getSitesPagedListWithFilter")]
        public async Task<IActionResult> GetSitesPagedListWithFilter([FromQuery] PaginationFilterDto filter, [FromBody] OprFilter filterInput)
        {

            var list = await Mediator.Send(new GetSitesPagedListWithFilter() { Input = filter.Values(), FilterInput = filterInput, User = UserInfo() });
            return Ok(list);
        }

    }
}
