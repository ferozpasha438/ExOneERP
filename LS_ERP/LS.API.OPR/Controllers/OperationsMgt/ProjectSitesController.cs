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
    public class ProjectSitesController : BaseController
    {
        private readonly IConfiguration _Config;
        private readonly IWebHostEnvironment _env;
        public ProjectSitesController(IOptions<AppSettingsJson> appSettings, IConfiguration config, IWebHostEnvironment env) : base(appSettings)
        {
            _Config = config;
            _env = env;
        }



        [HttpGet("getProjectSitesPagedList")]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {

            var list = await Mediator.Send(new GetProjectSitesPagedList() { Input = filter.Values(), User = UserInfo() });
            return Ok(list);
        }



        //[HttpPost]
        //public async Task<ActionResult> Create([FromBody] OP_HRM_TEMP_ProjectDto dTO)
        //{
        //    var id = await Mediator.Send(new CreateProject() { ProjectDto = dTO, User = UserInfo() });
        //    if (id > 0)
        //    {
        //        return Created($"get/{id}", dTO);
        //    }
        //    else if (id == -1)
        //    {
        //        return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Duplicate(nameof(dTO.ProjectCode)) });
        //    }
        //    return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });

        //}
        [HttpPost("ConvertEnquiryToProjectSites")]
        public async Task<ActionResult> ConvertEnquiryToProjectSites([FromBody] ConvertCustToProjectSitesDto dTO)
        {
            var id = await Mediator.Send(new ConvertEnquiryToProjectSites() { ProjectSitesDto = dTO, User = UserInfo() });
            if (id > 0)
            {
                return Created($"get/{id}", dTO);
            }
            else if (id == -1)
            {
                return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Duplicate(nameof(dTO)) });
            }
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });

        }


        //[HttpGet("getProjectById/{id}")]
        //public async Task<IActionResult> Get([FromRoute] int id)
        //{
        //    var obj = await Mediator.Send(new GetProjectById() { Id = id, User = UserInfo() });
        //    return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        //}

        [HttpGet("getProjectSiteByProjectAndSiteCode/{ProjectCode}/{SiteCode}")]
        public async Task<IActionResult> Get([FromRoute] string ProjectCode,[FromRoute] string SiteCode)
        {
            var obj = await Mediator.Send(new GetProjectSiteByProjectAndSiteCode() { ProjectCode = ProjectCode,SiteCode=SiteCode, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("getSelectProjectSitesListByCustomerCode/{CustomerCode}")]
        public async Task<IActionResult> GetSelectProjectListByCustomerCode([FromRoute] string CustomerCode)
        {
            var item = await Mediator.Send(new GetSelectSitesListByCustomerCode() { User = UserInfo(), CustomerCode = CustomerCode });
            return Ok(item);
        }
        [HttpGet("getSelectSiteListWhicAreNotConvertedAsProjectByEnquiryNumber/{EnquiryNumber}")]
        public async Task<IActionResult> GetSelectSiteListWhicAreNotConvertedAsProjectByEnquiryNumber([FromRoute] string EnquiryNumber)
        {
            var item = await Mediator.Send(new GetSelectSiteListWhicAreNotConvertedAsProjectByEnquiryNumber() { User = UserInfo(), EnquiryNumber = EnquiryNumber });
            return Ok(item);
        }

        [HttpGet("getSelectProjectSitesList")]
        public async Task<IActionResult> GetSelectProjectSitesList()
        {
            var item = await Mediator.Send(new GetSelectSitesList() { User = UserInfo() });
            return Ok(item);
        }
         [HttpGet("getSelectProjectSitesList2")]                    //Text and value only, Text will have full details
        public async Task<IActionResult> GetSelectProjectSitesList2()
        {
            var item = await Mediator.Send(new GetSelecSitesList2() { User = UserInfo() });
            return Ok(item);
        } 
        [HttpGet("getSelectProjectSitesListByProjectCode/{projectCode}")]                    //Text and value only, Text will have full details
        public async Task<IActionResult> GetSelectProjectSitesListByProjectCode([FromRoute] string ProjectCode )
        {
            var item = await Mediator.Send(new GetSelectSitesListByProjectCode() { ProjectCode= ProjectCode, User = UserInfo() });
            return Ok(item);
        }


        //[HttpDelete("{id}")]
        //public async Task<ActionResult> Delete([FromRoute] int id)
        //{
        //    var ProjectId = await Mediator.Send(new DeleteProject() { Id = id, User = UserInfo() });
        //    if (ProjectId > 0)
        //        return NoContent();
        //    return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        //}

        [HttpPost("uploadContractForm")]
        public async Task<ActionResult> UploadContractForm([FromForm] InputUploadContractFormForProjectSite dTO)
        {
            dTO.WebRoot = $"{_env.ContentRootPath}/Uploads/ContractForms";

            if (!System.IO.Directory.Exists(dTO.WebRoot))
                System.IO.Directory.CreateDirectory(dTO.WebRoot);

            var Res = await Mediator.Send(new UploadContractForm() { dto = dTO, User = UserInfo() });
            if (Res.IsSuccess)
            {
                return Created($"get/{dTO.Id}", dTO);
            }
            else
            {
                return BadRequest(new ApiMessageDto { Message = Res.ErrorMsg });
            }

        }

        [HttpGet("getRecenetAttendanceDate/{projectCode}/{siteCode}")]
        public async Task<ActionResult> GetRecentAttendanceDate([FromRoute] string projectCode,[FromRoute] string siteCode)
        {
            var item = await Mediator.Send(new GetRecentAttendanceDateForProjectSite() { ProjectCode = projectCode, SiteCode=siteCode, User = UserInfo() });
            return Ok(item);
        }
    
    
    
    
    }
}
