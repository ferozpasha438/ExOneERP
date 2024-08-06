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
    public class ProjectBudgetCostingController : BaseController
    {

        public ProjectBudgetCostingController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }
        #region ProjectResourceCostingForSite


        [HttpPost("createProjectResourceCostingForSite")]
        public async Task<ActionResult> CreateProjectResourceCostingForSite([FromBody] TblOpProjectBudgetCostingDto dTO)
        {
            var id = await Mediator.Send(new CreateProjectResourceCostingForSite() { Input = dTO, User = UserInfo() });
            if (id > 0)
            {
                return Created($"get/{id}", dTO);
            }

            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });

        }

        [HttpGet("getProjectResourceCostingForSite/{customerCode}/{projectCode}/{siteCode}")]
        public async Task<TblOpProjectBudgetCostingDto> GetProjectResourceCostingForSite([FromRoute] string customerCode, [FromRoute] string projectCode, [FromRoute] string siteCode)
        {
            var obj = await Mediator.Send(new GetProjectResourceCostingForSite() { SiteCode = siteCode, CustomerCode = customerCode, ProjectCode = projectCode, User = UserInfo() });
            return obj;
        }

        #endregion



        #region ProjectLogisticsCostingForSite


        [HttpPost("createProjectLogisticsCostingForSite")]
        public async Task<ActionResult> CreateProjectLogisticsCostingForSite([FromBody] TblOpProjectBudgetCostingDto dTO)
        {
            var id = await Mediator.Send(new CreateProjectLogisticsCostingForSite() { Input = dTO, User = UserInfo() });
            if (id > 0)
            {
                return Created($"get/{id}", dTO);
            }

            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });

        }

        [HttpGet("getProjectLogisticsCostingForSite/{customerCode}/{projectCode}/{siteCode}")]
        public async Task<TblOpProjectBudgetCostingDto> GetProjectLogisticsCostingForSite([FromRoute] string customerCode, [FromRoute] string projectCode, [FromRoute] string siteCode)
        {
            var obj = await Mediator.Send(new GetProjectLogisticsCostingForSite() { SiteCode = siteCode, CustomerCode = customerCode, ProjectCode = projectCode, User = UserInfo() });
            return obj;
        }

        #endregion


        #region ProjectMaterialEquipmentCostingForSite


        [HttpPost("createProjectMaterialEquipmentCostingForSite")]
        public async Task<ActionResult> CreateProjectMaterialEquipmentCostingForSite([FromBody] TblOpProjectBudgetCostingDto dTO)
        {
            var id = await Mediator.Send(new CreateProjectMaterialEquipmentCostingForSite() { Input = dTO, User = UserInfo() });
            if (id > 0)
            {
                return Created($"get/{id}", dTO);
            }

            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });

        }

        [HttpGet("getProjectMaterialEquipmentCostingForSite/{customerCode}/{projectCode}/{siteCode}")]
        public async Task<TblOpProjectBudgetCostingDto> GetProjectMaterialEquipmentCostingForSite([FromRoute] string customerCode, [FromRoute] string projectCode, [FromRoute] string siteCode)
        {
            var obj = await Mediator.Send(new GetProjectMaterialEquipmentCostingForSite() { SiteCode = siteCode, CustomerCode = customerCode, ProjectCode = projectCode, User = UserInfo() });
            return obj;
        }

        #endregion

        #region ProjectFinancialExpenseCostingForSite


        [HttpPost("createProjectFinancialExpenseCostingForSite")]
        public async Task<ActionResult> CreateProjectFinancialExpenseCostingForSite([FromBody] TblOpProjectBudgetCostingDto dTO)
        {
            var id = await Mediator.Send(new CreateProjectFinancialExpenseCostingForSite() { Input = dTO, User = UserInfo() });
            if (id > 0)
            {
                return Created($"get/{id}", dTO);
            }

            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });

        }

        [HttpGet("getProjectFinancialExpenseCostingForSite/{customerCode}/{projectCode}/{siteCode}")]
        public async Task<TblOpProjectBudgetCostingDto> GetProjectFinancialExpenseCostingForSite([FromRoute] string customerCode, [FromRoute] string projectCode, [FromRoute] string siteCode)
        {
            var obj = await Mediator.Send(new GetProjectFinancialExpenseCostingForSite() { SiteCode = siteCode, CustomerCode = customerCode, ProjectCode = projectCode, User = UserInfo() });
            return obj;
        }







        #endregion


        #region ProjectEstimationForProject


        //[HttpPost("createProjectEstimationForProject")]
        //public async Task<ActionResult> CreateProjectEstimationForProject([FromBody] TblOpProjectBudgetCostingDto dTO)
        //{
        //    var id = await Mediator.Send(new CreateProjectEstimationForProject() { Input = dTO, User = UserInfo() });
        //    if (id > 0)
        //    {
        //        return Created($"get/{id}", dTO);
        //    }

        //    return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });

        //}

        [HttpGet("getProjectEstimation/{customerCode}/{projectCode}")]
        public async Task<TblOpProjectBudgetEstimationDto> GetProjectEstimation([FromRoute] string customerCode, [FromRoute] string projectCode)
        {
            var obj = await Mediator.Send(new GetProjectEstimation() { CustomerCode = customerCode, ProjectCode = projectCode, User = UserInfo() });
            return obj;
        }


        [HttpPost("convertProjectToProposal")]
        public async Task<ActionResult> ConvertProjectToProposal([FromBody] TblOpProjectBudgetCostingDto Project)
        {
            var id = await Mediator.Send(new ConvertProjectToProposal() { Project = Project, User = UserInfo() });
            if (id > 0)
                return Created($"get/{id}", Project);
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }

        #endregion

        #region ProjectSiteEstimationForSite


        [HttpGet("getProjectSiteEstimation/{customerCode}/{projectCode}/{siteCode}")]
        public async Task<TblOpProjectBudgetEstimationDto> GetProjectSiteEstimation([FromRoute] string customerCode, [FromRoute] string projectCode, [FromRoute] string siteCode)
        {
            var obj = await Mediator.Send(new GetProjectSiteEstimation() { CustomerCode = customerCode, SiteCode = siteCode, ProjectCode = projectCode, User = UserInfo() });
            return obj;
        }


        [HttpPost("convertProjectSiteToProposal")]                  //Adendum To Proposal
        public async Task<ActionResult> ConvertProjectSiteToProposal([FromBody] TblOpProjectSites_PaginationDto Project)
        {
            var id = await Mediator.Send(new ConvertProjectSiteToProposal() { Project = Project, User = UserInfo() });
            if (id > 0)
                return Created($"get/{id}", Project);
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }


        #endregion

        #region SkippEstimation         //Applicable To Project or project Site
        [HttpPost("skippEstimation")]
        public async Task<ActionResult> SkippEstimation([FromBody] TblOpProjectSites_PaginationDto Project)
        {
            var id = await Mediator.Send(new SkippEstimation() { Project = Project, User = UserInfo() });
            if (id > 0)
                return Created($"get/{id}", Project);
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }
        #endregion
        #region SkippEstimationType         //Applicable   project Site
        [HttpPost("skippEstimationType")]
        public async Task<ActionResult> SkippEstimationType([FromBody] SkippEstimationTypeDto input)
        {
            var id = await Mediator.Send(new SkippEstimationType() { Input = input, User = UserInfo() });
            if (id > 0)
                return Created($"get/{id}", input);
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }
        #endregion


    }
}




