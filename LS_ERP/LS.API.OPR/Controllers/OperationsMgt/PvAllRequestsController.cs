
using CIN.Application;
using CIN.Application.Common;
using CIN.Application.OperationsMgtDtos;
using CIN.Application.OperationsMgtQuery;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
namespace LS.API.OPR.Controllers.OperationsMgt
{
    public class PvAllRequestsController : BaseController
    {

        
        //public PvAllRequestsController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        //{
        //}


        private readonly IConfiguration _Config;
        private readonly IWebHostEnvironment _env;
        public PvAllRequestsController(IOptions<AppSettingsJson> appSettings, IConfiguration config, IWebHostEnvironment env) : base(appSettings)
        {
            _Config = config;
            _env = env;
        }




        [HttpGet("getPvAllRequestsPagedList")]
        public async Task<IActionResult> Get([FromQuery] OprPaginationFilterDto filter)
        {

            var list = await Mediator.Send(new GetPvAllRequestsPagedList() { Input = filter.Values(), User = UserInfo() });
            return Ok(list);
        }

        [HttpGet("getPvAllRequestsPagedListByProjectSite/{projectCode}/{siteCode}")]
        public async Task<IActionResult> GetPvAllRequestsPagedListByProjectSite([FromQuery] OprPaginationFilterDto filter,[FromRoute] string projectCode , [FromRoute] string siteCode)
        {

            var list = await Mediator.Send(new GetPvAllRequestsPagedListByProjectSite() { Input = filter.Values(),ProjectCode=projectCode,SiteCode=siteCode, User = UserInfo() });
            return Ok(list);
        }
        [HttpPost("getRecentApprovedPvRequestData")]
        public async Task<IActionResult> GetRecentApprovedPvRequestData([FromBody] InputGetRecentPvRequest input)
        {

            var  res=await Mediator.Send(new GetRecentApprovedPvRequestData() {Dto=input, User = UserInfo() });
           return Ok(res);
        }



        [HttpPost("fileUpload")]
        public async Task<ActionResult> FileUpload([FromForm] PvRequestsFileUploadDto dTO)
        {
            dTO.WebRoot = $"{_env.ContentRootPath}/Uploads/Adendums";

            if (!System.IO.Directory.Exists(dTO.WebRoot))
                System.IO.Directory.CreateDirectory(dTO.WebRoot);



            var Res = await Mediator.Send(new PvRequestsFileUpload() { dto = dTO, User = UserInfo() });
            if (Res.IsSuccess)
            {
                return Created($"get/{dTO.Id}", dTO);
            }
            else
            {
                return BadRequest(new ApiMessageDto { Message = Res.ErrorMsg });
            }


        }











    }
}
