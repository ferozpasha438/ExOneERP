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
    public class ReportsController : BaseController
    {

        public ReportsController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }

        #region ProjectSitesReports
        [HttpPost("getProjectSitesReports")]
        public async Task<IActionResult> Get([FromBody] ProjectSitesReportsInputDto queryDto)
        {

            var list = await Mediator.Send(new GetProjectSitesReports() { Input = queryDto, User = UserInfo() });
            return Ok(list);
        }
        [HttpPost("getReportByProjectSites")]
        public async Task<IActionResult> Get([FromBody] List<ProjectSiteReport> projectSites)
        {

            List<ProjectSiteReport> Reports = new();
            for (int i = 0; i < projectSites.Count; i++)
            {
                var report = await Mediator.Send(new GetReportByProjectSite() { Input = projectSites[i], User = UserInfo() });

                if (report is not null)
                {
                    Reports.Add(report);
                }
            }
            return Ok(Reports);
        }
        #endregion


        #region ProjectsCountMatrixReports
        [HttpPost("getProjectsCountMatrixReports")]
        public async Task<IActionResult> GetProjectsCountMatrixReports([FromBody] ProjectSitesReportsInputDto queryDto)
        {

            var list = await Mediator.Send(new GetProjectCountMatrixReports() { Input = queryDto, User = UserInfo() });
            return Ok(list);
        }
        [HttpPost("getProjectsCountReportByProjectSites")]
        public async Task<IActionResult> GetProjectsCountReportByProjectSites([FromBody] List<ProjectSiteReport> projectSites)
        {

            List<ProjectSiteReport> Reports = new();
            for (int i = 0; i < projectSites.Count; i++)
            {
                var report = await Mediator.Send(new GetProjectsCountReportByProjectSites() { Input = projectSites[i], User = UserInfo() });

                if (report is not null)
                {
                    Reports.Add(report);
                }
            }
            return Ok(Reports);
        }
        #endregion
#region ResourcesOnProjectsReports
        [HttpPost("getResourcesOnProjectsReports")]
        public async Task<IActionResult> GetResourcesOnProjectsReports([FromBody] ProjectSitesReportsInputDto queryDto)
        {

            var list = await Mediator.Send(new GetResourcesOnProjectReports() { Input = queryDto, User = UserInfo() });
            return Ok(list);
        }
        [HttpPost("getResourcesOnProjectsReportsByProjectSites")]
        public async Task<IActionResult> GetResourcesOnProjectsReportsProjectSites([FromBody] List<ProjectSiteReport> projectSites)
        {

            List<ProjectSiteReport> Reports = new();
            for (int i = 0; i < projectSites.Count; i++)
            {
                var report = await Mediator.Send(new GetResourcesOnProjectsReportByProjectSites() { Input = projectSites[i], User = UserInfo() });

                if (report is not null)
                {
                    Reports.Add(report);
                }
            }
            return Ok(Reports);
        }
        #endregion
#region SkillsetsOnProjectsReports
        [HttpPost("getSkillsetsOnProjectsReports")]
        public async Task<IActionResult> GetSkillsetsOnProjectsReports([FromBody] ProjectSitesReportsInputDto queryDto)
        {

            var list = await Mediator.Send(new GetSkillsetsOnProjectsReports() { Input = queryDto, User = UserInfo() });
            return Ok(list);
        }
        [HttpPost("getSkillsetsOnProjectsReportsByProjectSites")]
        public async Task<IActionResult> GetSkillsetsOnProjectsReportsProjectSites([FromBody] List<ProjectSiteReport> projectSites)
        {

            List<ProjectSiteReport> Reports = new();
            for (int i = 0; i < projectSites.Count; i++)
            {
                var report = await Mediator.Send(new GetSkillsetsOnProjectsReportByProjectSites() { Input = projectSites[i], User = UserInfo() });

                if (report is not null)
                {
                    Reports.Add(report);
                }
            }
            return Ok(Reports);
        }
        #endregion

        #region AttendancePayrollReports

        [HttpPost("getAttendancePayrollReport")]
        public async Task<IActionResult> GetAttendancePayrollReport([FromBody] Input_OpAttendanceReportForPayRollPeriodDto Dto)
        {
            var obj = await Mediator.Send(new AttendanceReportForPayRollPeriodQuery() { Input = Dto, User = UserInfo() });
            if (obj.IsValidReq)
                return Ok(obj);
            else
            {
                return BadRequest(new ApiMessageDto { Message = obj.ErrorMsg });
            }

        }
        [HttpPost("getRoasterPayrollReport")]
        public async Task<IActionResult> GetRoasterPayrollReport([FromBody] Input_OpAttendanceReportForPayRollPeriodDto Dto)
        {
            var obj = await Mediator.Send(new RoasterReportForPayRollPeriodQuery() { Input = Dto, User = UserInfo() });
            if (obj.IsValidReq.Value)
                return Ok(obj);
            else
            {
                return BadRequest(new ApiMessageDto { Message = obj.ErrorMsg });
            }

        }


        [HttpPost("getAttendanceStatusPayrollReport")]
        public async Task<IActionResult> AttendanceStatusReport([FromBody] Input_OpAttendanceStatusReportForPayRollPeriodDto queryDto)
        {

            var list = await Mediator.Send(new AttendanceStatusReportForPayRollPeriodQuery() { Input = queryDto, User = UserInfo() });
            return Ok(list);
        }







        #endregion

        #region CustomerComplaintsReports

        [HttpPost("getCustomerComplaintsReport")]
        public async Task<IActionResult> GetCustomerComplaintsReport([FromBody] CustomerComplaintsReportsInputDto Dto)
        {
            var res = await Mediator.Send(new GetCustomerComplaintsReport() { Input = Dto, User = UserInfo() });
            return Ok(res);

        }
        #endregion
        
        #region CustomerVisitsReports

        [HttpPost("getCustomerVisitsReport")]
        public async Task<IActionResult> GetCustomerVisitsReport([FromBody] CustomerVisitsReportsInputDto Dto)
        {
            var res = await Mediator.Send(new GetCustomerVisitsReport() { Input = Dto, User = UserInfo() });
            return Ok(res);

        }
        #endregion

      
      
    }
}
