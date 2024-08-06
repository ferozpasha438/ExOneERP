using AutoMapper;
using CIN.Application.Common;
using CIN.Application.OperationsMgtDtos;
using CIN.DB;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using System.Linq.Dynamic.Core;
using CIN.Domain.OpeartionsMgt;
using CIN.Application.SystemSetupDtos;
using System.Globalization;

namespace CIN.Application.OperationsMgtQuery
{
    #region ProjectSitesReports

    #region GetProjectSitesReports

    public class GetProjectSitesReports : IRequest<ProjectSitesReportsOutputDto>
    {
        public UserIdentityDto User { get; set; }
        public ProjectSitesReportsInputDto Input { get; set; }
    }

    public class GetProjectSitesReportsHandler : IRequestHandler<GetProjectSitesReports,ProjectSitesReportsOutputDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetProjectSitesReportsHandler(CINDBOneContext context, AutoMapper.IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ProjectSitesReportsOutputDto> Handle(GetProjectSitesReports request, CancellationToken cancellationToken)
        {

           
                ProjectSitesReportsOutputDto Res = new();
                List<TblOpProjectSitesDto> resList = new();
           
                var Branch = request.Input.CityCode == "" || request.Input.CityCode == null ?
                    await _context.CompanyBranches.FirstOrDefaultAsync(e => e.Id == request.User.BranchId) :
                    await _context.CompanyBranches.FirstOrDefaultAsync(e => e.BranchCode == request.Input.CityCode);





                var Company = await _context.Companies.ProjectTo<TblErpSysCompanyDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.Id == Branch.CompanyId);
            try
            {
                if (request.Input.CustomerCode != null && request.Input.CustomerCode != "")
                {

                    if (resList.Count > 0)
                    {
                        resList = resList.Where(e => e.CustomerCode == request.Input.CustomerCode).ToList();

                    }
                    else
                    {
                        resList = await _context.TblOpProjectSites.Where(e => e.CustomerCode == request.Input.CustomerCode
                         && e.StartDate >= request.Input.FromDate
                         && e.EndDate <= request.Input.ToDate
                         && e.IsConvrtedToContract
                        ).ProjectTo<TblOpProjectSitesDto>(_mapper.ConfigurationProvider).ToListAsync();

                    }

                }

                if (request.Input.CityCode != null && request.Input.CityCode != "")
                {

                    if (resList.Count > 0)
                    {
                        resList = resList.Where(e => e.BranchCode == request.Input.CityCode).ToList();

                    }
                    else
                    {
                        resList = await _context.TblOpProjectSites.Where(e => e.BranchCode == request.Input.CityCode
                         && e.StartDate >= request.Input.FromDate
                         && e.EndDate <= request.Input.ToDate
                         && e.IsConvrtedToContract
                        ).ProjectTo<TblOpProjectSitesDto>(_mapper.ConfigurationProvider).ToListAsync();

                        Company = await _context.Companies.ProjectTo<TblErpSysCompanyDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.Id == Branch.CompanyId);

                    }

                }

                if (request.Input.StatusCode != null && request.Input.StatusCode != "")
                {

                    if (resList.Count > 0)
                    {

                        if (request.Input.StatusCode == "InProgress")
                            resList = resList.Where(e => !e.IsInActive && e.EndDate >= DateTime.UtcNow).ToList();
                        else if (request.Input.StatusCode == "Closed")
                            resList = resList.Where(e => e.IsInActive).ToList();
                        else if (request.Input.StatusCode == "Suspended")
                            resList = resList.Where(e => e.IsSuspended).ToList();
                        else if (request.Input.StatusCode == "InActive")
                            resList = resList.Where(e => e.IsInActive).ToList();
                        else if (request.Input.StatusCode == "Completed")
                            resList = resList.Where(e => !e.IsInActive && e.EndDate < DateTime.UtcNow).ToList();
                    }
                    else
                    {
                        resList = await _context.TblOpProjectSites.Where(e => e.StartDate >= request.Input.FromDate
                         && e.EndDate <= request.Input.ToDate
                         && e.IsConvrtedToContract
                        ).ProjectTo<TblOpProjectSitesDto>(_mapper.ConfigurationProvider).ToListAsync();

                        if (request.Input.StatusCode == "InProgress")
                            resList = resList.Where(e => !e.IsInActive && e.EndDate >= DateTime.UtcNow).ToList();
                        else if (request.Input.StatusCode == "Closed")
                            resList = resList.Where(e => e.IsInActive).ToList();
                        else if (request.Input.StatusCode == "Suspended")
                            resList = resList.Where(e => e.IsSuspended).ToList();
                        else if (request.Input.StatusCode == "InActive")
                            resList = resList.Where(e => e.IsInActive).ToList();
                        else if (request.Input.StatusCode == "Completed")
                            resList = resList.Where(e => !e.IsInActive && e.EndDate < DateTime.UtcNow).ToList();

                    }

                }

                if (request.Input.ServiceCode != null && request.Input.ServiceCode != "")
                {

                    var EnquiryNumbersWithServiceCode =  _context.OprEnquiries.Where(e => e.ServiceCode == request.Input.ServiceCode).Select(s => new { s.EnquiryNumber,s.SiteCode,s.ServiceCode }).ToList();
                    var EnquiriesConvertedToProjects =  _context.OprEnquiryHeaders.Where(e => e.IsConvertedToProject).ToList();

                    var resSiteList = (from e in EnquiryNumbersWithServiceCode join p in EnquiriesConvertedToProjects on e.EnquiryNumber equals p.EnquiryNumber select new { p.EnquiryNumber, e.ServiceCode,e.SiteCode }).ToList();



                    if (resList.Count > 0 && resSiteList.Count > 0)
                    {
                        var tempList = resList;

                        for (int i = 0; i < resList.Count; i++)
                        {
                            if (!resSiteList.Any(e => e.SiteCode == resList[i].SiteCode && e.ServiceCode == request.Input.ServiceCode))
                            {
                                tempList.RemoveAt(i);

                            }



                        }
                        resList = tempList;
                    }

                    else if (resSiteList.Count > 0)
                    {
                        resList = _context.TblOpProjectSites.Where(e => e.StartDate >= request.Input.FromDate
                        && e.EndDate <= request.Input.ToDate
                        && e.IsConvrtedToContract
                        ).ProjectTo<TblOpProjectSitesDto>(_mapper.ConfigurationProvider).ToList();


                        var tempList = resList;

                        for (int i = 0; i < resList.Count; i++)
                        {
                            if (!resSiteList.Any(e => e.SiteCode == resList[i].SiteCode))
                            {
                                tempList.RemoveAt(i);

                            }



                        }
                        resList = tempList;
                    }
                    else
                    {

                        resList = new();

                    }


                }

                if (request.Input.ServiceCode == "" && request.Input.CityCode == "" && request.Input.CustomerCode == "" && request.Input.StatusCode == "")
                {
                    resList = await _context.TblOpProjectSites.Where(e => e.StartDate >= request.Input.FromDate
                            && e.EndDate <= request.Input.ToDate
                            && e.IsConvrtedToContract
                           ).ProjectTo<TblOpProjectSitesDto>(_mapper.ConfigurationProvider).ToListAsync();

                }




                Res.Company = Company is not null ? Company : new() { CompanyAddress = "", CompanyName = "", LogoImagePath = "", LogoURL = "" };
                Res.ProjectSites = resList;

                if (Res.ProjectSites.Count > 0)
                {
                    var customerCodes = Res.ProjectSites.GroupBy(e => e.CustomerCode).Select(c => new { CustomerCode = c.Key }).ToList();

                    var Customers = (from cc in customerCodes join c in _context.OprCustomers.ProjectTo<TblSndDefCustomerMasterDto>(_mapper.ConfigurationProvider) on cc.CustomerCode equals c.CustCode select c).ToList();
                    Res.Customers = Customers;
                }
                else
                {
                    Res.Customers = new();
                }
                return Res;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetProjectSitesReports Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return new() { Company=Company,ProjectSites=new()};
            }
        }
}
    #endregion
    #region GetReportByProjectSite

    public class GetReportByProjectSite : IRequest<ProjectSiteReport>
    {
        public UserIdentityDto User { get; set; }
        public ProjectSiteReport Input { get; set; }
    }

    public class GetReportByProjectSiteHandler : IRequestHandler<GetReportByProjectSite, ProjectSiteReport>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetReportByProjectSiteHandler(CINDBOneContext context, AutoMapper.IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ProjectSiteReport> Handle(GetReportByProjectSite request, CancellationToken cancellationToken)
        {
            var siteData = await _context.OprSites.FirstOrDefaultAsync(e=>e.SiteCode==request.Input.SiteCode);
            request.Input.SiteNameArb = siteData.SiteArbName;
            request.Input.SiteNameEng = siteData.SiteName;
            request.Input.EstimationCost = 0;

            var Estimation = await _context.TblOpProjectBudgetEstimations.FirstOrDefaultAsync(e=>e.CustomerCode== request.Input.CustomerCode && e.ProjectCode== request.Input.ProjectCode);
            if (Estimation is not null)
            {
                var projectBudgetCostings = await _context.TblOpProjectBudgetCostings.Where(e => e.ProjectBudgetEstimationId == Estimation.ProjectBudgetEstimationId && e.SiteCode == request.Input.SiteCode).ToListAsync();
                decimal rescost = 0;
                decimal logcost = 0;
                decimal matcost = 0;
                decimal expcost = 0;

                if (projectBudgetCostings.Count > 0)
                {
                    var resCostings = new TblOpProjectResourceCostingDto();
                    foreach (var pbc in projectBudgetCostings)
                    {
                        var resCostingsForSite = await _context.TblOpProjectResourceCosting.ProjectTo<TblOpProjectResourceCostingDto>(_mapper.ConfigurationProvider).Where(e => e.ProjectBudgetCostingId == pbc.ProjectBudgetCostingId).ToListAsync();
                        if (resCostingsForSite.Count > 0)
                        {
                            foreach (var rc in resCostingsForSite)
                            {
                                rescost += rc.Quantity * (rc.Margin + rc.CostPerUnit);

                            }



                        }
                    }
                    var logCostings = new TblOpProjectLogisticsCostingDto();
                    foreach (var pbc in projectBudgetCostings)
                    {
                        var logCostingsForSite = await _context.TblOpProjectLogisticsCostings.ProjectTo<TblOpProjectLogisticsCostingDto>(_mapper.ConfigurationProvider).Where(e => e.ProjectBudgetCostingId == pbc.ProjectBudgetCostingId).ToListAsync();
                        if (logCostingsForSite.Count > 0)
                        {
                            foreach (var lc in logCostingsForSite)
                            {
                                logcost += lc.Qty * (lc.Margin + lc.CostPerUnit);

                            }
                        }
                    }


                }
                var matCostings = new TblOpProjectMaterialEquipmentCostingDto();
                foreach (var pbc in projectBudgetCostings)
                {
                    var matCostingsForSite = await _context.TblOpProjectMaterialEquipmentCostings.ProjectTo<TblOpProjectMaterialEquipmentCostingDto>(_mapper.ConfigurationProvider).Where(e => e.ProjectBudgetCostingId == pbc.ProjectBudgetCostingId).ToListAsync();
                    if (matCostingsForSite.Count > 0)
                    {
                        foreach (var mc in matCostingsForSite)
                        {
                            matcost += mc.Quantity * (mc.CostPerUnit);

                        }
                    }
                }

                var expCostings = new TblOpProjectFinancialExpenseCostingDto();
                foreach (var pbc in projectBudgetCostings)
                {
                    var expCostingsForSite = await _context.TblOpProjectFinancialExpenseCostings.ProjectTo<TblOpProjectFinancialExpenseCostingDto>(_mapper.ConfigurationProvider).Where(e => e.ProjectBudgetCostingId == pbc.ProjectBudgetCostingId).ToListAsync();
                    if (expCostingsForSite.Count > 0)
                    {
                        foreach (var ec in expCostingsForSite)
                        {
                            expcost += ec.CostPerUnit;

                        }
                    }
                }


                request.Input.EstimationCost = rescost + expcost + logcost + matcost;


            }



            DateTime curDate = DateTime.UtcNow;
            request.Input.Status = curDate > request.Input.EndDate ? "Completed" : request.Input.IsSuspended ? "Suspended" : request.Input.IsClosed ? "Closed" : request.Input.IsInActive ? "InActive" : "InProgress";
            return request.Input;


        }
    }
    #endregion

    #endregion


    #region ProjectCountMatrixReports
    #region GetProjectCountMatrixReports
    public class GetProjectCountMatrixReports : IRequest<ProjectSitesReportsOutputDto>
    {
        public UserIdentityDto User { get; set; }
        public ProjectSitesReportsInputDto Input { get; set; }
    }

    public class GetProjectCountMatrixReportsHandler : IRequestHandler<GetProjectCountMatrixReports, ProjectSitesReportsOutputDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetProjectCountMatrixReportsHandler(CINDBOneContext context, AutoMapper.IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ProjectSitesReportsOutputDto> Handle(GetProjectCountMatrixReports request, CancellationToken cancellationToken)
        {


            ProjectSitesReportsOutputDto Res = new();
            List<TblOpProjectSitesDto> resList = new();

            var Branch = request.Input.CityCode == "" || request.Input.CityCode == null ?
                await _context.CompanyBranches.FirstOrDefaultAsync(e => e.Id == request.User.BranchId) :
                await _context.CompanyBranches.FirstOrDefaultAsync(e => e.BranchCode == request.Input.CityCode);





            var Company = await _context.Companies.ProjectTo<TblErpSysCompanyDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.Id == Branch.CompanyId);
            try
            {
                if (request.Input.CustomerCode != null && request.Input.CustomerCode != "")
                {

                    if (resList.Count > 0)
                    {
                        resList = resList.Where(e => e.CustomerCode == request.Input.CustomerCode).ToList();

                    }
                    else
                    {
                        resList = await _context.TblOpProjectSites.Where(e => e.CustomerCode == request.Input.CustomerCode
                         && e.StartDate >= request.Input.FromDate
                         && e.EndDate <= request.Input.ToDate
                         && e.IsConvrtedToContract
                        ).ProjectTo<TblOpProjectSitesDto>(_mapper.ConfigurationProvider).ToListAsync();

                    }

                }

                if (request.Input.CityCode != null && request.Input.CityCode != "")
                {

                    if (resList.Count > 0)
                    {
                        resList = resList.Where(e => e.BranchCode == request.Input.CityCode).ToList();

                    }
                    else
                    {
                        resList = await _context.TblOpProjectSites.Where(e => e.BranchCode == request.Input.CityCode
                         && e.StartDate >= request.Input.FromDate
                         && e.EndDate <= request.Input.ToDate
                         && e.IsConvrtedToContract
                        ).ProjectTo<TblOpProjectSitesDto>(_mapper.ConfigurationProvider).ToListAsync();

                        Company = await _context.Companies.ProjectTo<TblErpSysCompanyDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.Id == Branch.CompanyId);

                    }

                }

                if (request.Input.StatusCode != null && request.Input.StatusCode != "")
                {

                    if (resList.Count > 0)
                    {

                        if (request.Input.StatusCode == "InProgress")
                            resList = resList.Where(e => !e.IsInActive && e.EndDate >= DateTime.UtcNow).ToList();
                        else if (request.Input.StatusCode == "Closed")
                            resList = resList.Where(e => e.IsInActive).ToList();
                        else if (request.Input.StatusCode == "Suspended")
                            resList = resList.Where(e => e.IsSuspended).ToList();
                        else if (request.Input.StatusCode == "InActive")
                            resList = resList.Where(e => e.IsInActive).ToList();
                        else if (request.Input.StatusCode == "Completed")
                            resList = resList.Where(e => !e.IsInActive && e.EndDate < DateTime.UtcNow).ToList();
                    }
                    else
                    {
                        resList = await _context.TblOpProjectSites.Where(e => e.StartDate >= request.Input.FromDate
                         && e.EndDate <= request.Input.ToDate
                         && e.IsConvrtedToContract
                        ).ProjectTo<TblOpProjectSitesDto>(_mapper.ConfigurationProvider).ToListAsync();

                        if (request.Input.StatusCode == "InProgress")
                            resList = resList.Where(e => !e.IsInActive && e.EndDate >= DateTime.UtcNow).ToList();
                        else if (request.Input.StatusCode == "Closed")
                            resList = resList.Where(e => e.IsInActive).ToList();
                        else if (request.Input.StatusCode == "Suspended")
                            resList = resList.Where(e => e.IsSuspended).ToList();
                        else if (request.Input.StatusCode == "InActive")
                            resList = resList.Where(e => e.IsInActive).ToList();
                        else if (request.Input.StatusCode == "Completed")
                            resList = resList.Where(e => !e.IsInActive && e.EndDate < DateTime.UtcNow).ToList();

                    }

                }

                if (request.Input.ServiceCode != null && request.Input.ServiceCode != "")
                {

                    var EnquiryNumbersWithServiceCode = _context.OprEnquiries.Where(e => e.ServiceCode == request.Input.ServiceCode).Select(s => new { s.EnquiryNumber, s.SiteCode, s.ServiceCode }).ToList();
                    var EnquiriesConvertedToProjects = _context.OprEnquiryHeaders.Where(e => e.IsConvertedToProject).ToList();

                    var resSiteList = (from e in EnquiryNumbersWithServiceCode join p in EnquiriesConvertedToProjects on e.EnquiryNumber equals p.EnquiryNumber select new { p.EnquiryNumber, e.ServiceCode, e.SiteCode }).ToList();



                    if (resList.Count > 0 && resSiteList.Count > 0)
                    {
                        var tempList = resList;

                        for (int i = 0; i < resList.Count; i++)
                        {
                            if (!resSiteList.Any(e => e.SiteCode == resList[i].SiteCode && e.ServiceCode == request.Input.ServiceCode))
                            {
                                tempList.RemoveAt(i);

                            }



                        }
                        resList = tempList;
                    }

                    else if (resSiteList.Count > 0)
                    {
                        resList = _context.TblOpProjectSites.Where(e => e.StartDate >= request.Input.FromDate
                        && e.EndDate <= request.Input.ToDate
                        && e.IsConvrtedToContract
                        ).ProjectTo<TblOpProjectSitesDto>(_mapper.ConfigurationProvider).ToList();


                        var tempList = resList;

                        for (int i = 0; i < resList.Count; i++)
                        {
                            if (!resSiteList.Any(e => e.SiteCode == resList[i].SiteCode))
                            {
                                tempList.RemoveAt(i);

                            }



                        }
                        resList = tempList;
                    }
                    else
                    {

                        resList = new();

                    }


                }

                if (request.Input.ServiceCode == "" && request.Input.CityCode == "" && request.Input.CustomerCode == "" && request.Input.StatusCode == "")
                {
                    resList = await _context.TblOpProjectSites.Where(e => e.StartDate >= request.Input.FromDate
                            && e.EndDate <= request.Input.ToDate
                            && e.IsConvrtedToContract
                           ).ProjectTo<TblOpProjectSitesDto>(_mapper.ConfigurationProvider).ToListAsync();

                }




                Res.Company = Company is not null ? Company : new() { CompanyAddress = "", CompanyName = "", LogoImagePath = "", LogoURL = "" };
                Res.ProjectSites = resList;

                if (Res.ProjectSites.Count > 0)
                {
                    var customerCodes = Res.ProjectSites.GroupBy(e => e.CustomerCode).Select(c => new { CustomerCode = c.Key }).ToList();

                    var Customers = (from cc in customerCodes join c in _context.OprCustomers.ProjectTo<TblSndDefCustomerMasterDto>(_mapper.ConfigurationProvider) on cc.CustomerCode equals c.CustCode select c).ToList();
                    Res.Customers = Customers;
                }
                else
                {
                    Res.Customers = new();
                }
                return Res;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetProjectSiteReports Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return new() { Company = Company, ProjectSites = new() };
            }
        }
    }
    #endregion

    #region GetProjectsCountReportByProjectSites

    public class GetProjectsCountReportByProjectSites : IRequest<ProjectSiteReport>
    {
        public UserIdentityDto User { get; set; }
        public ProjectSiteReport Input { get; set; }
    }

    public class GetProjectsCountReportByProjectSitesHandler : IRequestHandler<GetProjectsCountReportByProjectSites, ProjectSiteReport>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetProjectsCountReportByProjectSitesHandler(CINDBOneContext context, AutoMapper.IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ProjectSiteReport> Handle(GetProjectsCountReportByProjectSites request, CancellationToken cancellationToken)
        {
            var siteData = await _context.OprSites.FirstOrDefaultAsync(e => e.SiteCode == request.Input.SiteCode);
            request.Input.SiteNameArb = siteData.SiteArbName;
            request.Input.SiteNameEng = siteData.SiteName;
            DateTime curDate = DateTime.UtcNow;
            request.Input.Status = curDate > request.Input.EndDate ? "Completed" : request.Input.IsSuspended ? "Suspended" : request.Input.IsClosed ? "Closed" : request.Input.IsInActive ? "InActive" : "InProgress";
            return request.Input;


        }
    }
    #endregion

    #endregion
    #region ResourcesOnProjectReports
    #region GetResourcesOnProjectReports
    public class GetResourcesOnProjectReports : IRequest<ProjectSitesReportsOutputDto>
    {
        public UserIdentityDto User { get; set; }
        public ProjectSitesReportsInputDto Input { get; set; }
    }

    public class GetResourcesOnProjectReportsHandler : IRequestHandler<GetResourcesOnProjectReports, ProjectSitesReportsOutputDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetResourcesOnProjectReportsHandler(CINDBOneContext context, AutoMapper.IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ProjectSitesReportsOutputDto> Handle(GetResourcesOnProjectReports request, CancellationToken cancellationToken)
        {


            ProjectSitesReportsOutputDto Res = new();
            List<TblOpProjectSitesDto> resList = new();

            var Branch = request.Input.CityCode == "" || request.Input.CityCode == null ?
                await _context.CompanyBranches.FirstOrDefaultAsync(e => e.Id == request.User.BranchId) :
                await _context.CompanyBranches.FirstOrDefaultAsync(e => e.BranchCode == request.Input.CityCode);
            var Company = await _context.Companies.ProjectTo<TblErpSysCompanyDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.Id == Branch.CompanyId);
            try
            {
                if (request.Input.CustomerCode != null && request.Input.CustomerCode != "")
                {

                    if (resList.Count > 0)
                    {
                        resList = resList.Where(e => e.CustomerCode == request.Input.CustomerCode).ToList();

                    }
                    else
                    {
                        resList = await _context.TblOpProjectSites.Where(e => e.CustomerCode == request.Input.CustomerCode


                         && e.IsConvrtedToContract
                        ).ProjectTo<TblOpProjectSitesDto>(_mapper.ConfigurationProvider).ToListAsync();

                    }

                }

                if (request.Input.SiteCode != null && request.Input.SiteCode != "")
                {

                    if (resList.Count > 0)
                    {
                        resList = resList.Where(e => e.SiteCode == request.Input.SiteCode).ToList();

                    }
                    else
                    {
                        resList = await _context.TblOpProjectSites.Where(e => e.SiteCode == request.Input.SiteCode


                         && e.IsConvrtedToContract
                        ).ProjectTo<TblOpProjectSitesDto>(_mapper.ConfigurationProvider).ToListAsync();

                    }

                }


                if (request.Input.CityCode != null && request.Input.CityCode != "")
                {

                    if (resList.Count > 0)
                    {
                        resList = resList.Where(e => e.BranchCode == request.Input.CityCode).ToList();

                    }
                    else
                    {
                        resList = await _context.TblOpProjectSites.Where(e => e.BranchCode == request.Input.CityCode

                         && e.IsConvrtedToContract
                        ).ProjectTo<TblOpProjectSitesDto>(_mapper.ConfigurationProvider).ToListAsync();

                        Company = await _context.Companies.ProjectTo<TblErpSysCompanyDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.Id == Branch.CompanyId);

                    }

                }


                if (request.Input.CityCode == "" && request.Input.CustomerCode == "" && request.Input.SiteCode=="")
                {
                    resList = await _context.TblOpProjectSites.Where(e =>
                             e.IsConvrtedToContract
                           ).ProjectTo<TblOpProjectSitesDto>(_mapper.ConfigurationProvider).ToListAsync();

                }




                Res.Company = Company is not null ? Company : new() { CompanyAddress = "", CompanyName = "", LogoImagePath = "", LogoURL = "" };
                Res.ProjectSites = resList;

                if (Res.ProjectSites.Count > 0)
                {
                    var customerCodes = Res.ProjectSites.GroupBy(e => e.CustomerCode).Select(c => new { CustomerCode = c.Key }).ToList();

                    var Customers = (from cc in customerCodes join c in _context.OprCustomers.ProjectTo<TblSndDefCustomerMasterDto>(_mapper.ConfigurationProvider) on cc.CustomerCode equals c.CustCode select c).ToList();
                    Res.Customers = Customers;
                }
                else
                {
                    Res.Customers = new();
                }
                return Res;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetProjectSiteReport Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return new() { Company = Company, ProjectSites = new() };
            }
        }
    }
    #endregion

    #region GetResourcesOnProjectsReportByProjectSites

    public class GetResourcesOnProjectsReportByProjectSites : IRequest<ProjectSiteReport>
    {
        public UserIdentityDto User { get; set; }
        public ProjectSiteReport Input { get; set; }
    }

    public class GetResourcesOnProjectsReportByProjectSitesHandler : IRequestHandler<GetResourcesOnProjectsReportByProjectSites, ProjectSiteReport>
    {
        private readonly CINDBOneContext _context;
        private readonly DMCContext _contextDMC;
        private readonly IMapper _mapper;
        public GetResourcesOnProjectsReportByProjectSitesHandler(CINDBOneContext context,DMCContext contextDMC, AutoMapper.IMapper mapper)
        {
            _context = context;
            _contextDMC = contextDMC;
            _mapper = mapper;
        }
        public async Task<ProjectSiteReport> Handle(GetResourcesOnProjectsReportByProjectSites request, CancellationToken cancellationToken)
        {
            try
            {
                request.Input.ResourcesList = new List<EmployeeDtoForReports>();
                var siteData = await _context.OprSites.FirstOrDefaultAsync(e => e.SiteCode == request.Input.SiteCode);
                request.Input.SiteNameArb = siteData.SiteArbName;
                request.Input.SiteNameEng = siteData.SiteName;
                DateTime curDate = DateTime.UtcNow;
                request.Input.Status = curDate > request.Input.EndDate ? "Completed" : request.Input.IsSuspended ? "Suspended" : request.Input.IsClosed ? "Closed" : request.Input.IsInActive ? "InActive" : "InProgress";


                var resourcesOnProject = _context.TblOpMonthlyRoasterForSites.AsNoTracking().OrderByDescending(e=>e.MonthStartDate).Where(e => e.SiteCode == request.Input.SiteCode
         
                  && e.IsPrimaryResource
                  && e.Month==DateTime.UtcNow.Month
                  && e.Year==DateTime.UtcNow.Year
                  ).ToList();
           
                if (resourcesOnProject.Count > 0)
                    foreach (var employee in resourcesOnProject)
                    {
                        var employeeData = await _contextDMC.HRM_TRAN_Employees.AsNoTracking().ProjectTo<EmployeeDtoForReports>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.EmployeeNumber == employee.EmployeeNumber);
                        if (employeeData is not null)
                        {

                            var lastAttendanceDate = _context.EmployeeAttendance.AsNoTracking().OrderByDescending(e => e.AttnDate).FirstOrDefault(e => e.EmployeeNumber == employeeData.EmployeeNumber);

                            if (lastAttendanceDate is null)
                            {
                                request.Input.ResourcesList.Remove(employeeData);
                            }
                            else
                            {
                                if(!request.Input.ResourcesList.Any(e=>e.EmployeeNumber==employeeData.EmployeeNumber))
                                {
                                    employeeData.LastAttendedDay = lastAttendanceDate.AttnDate;
                                    request.Input.ResourcesList.Add(employeeData);
                                }
                               
                            }
                        }
                    }











                return request.Input;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetProjectSiteReport Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return new();
            }
        }
    }
    #endregion

    #endregion
#region SkillsetsOnProjectReports
    #region GetSkillsetsOnProjectReports
    public class GetSkillsetsOnProjectsReports : IRequest<ProjectSitesReportsOutputDto>
    {
        public UserIdentityDto User { get; set; }
        public ProjectSitesReportsInputDto Input { get; set; }
    }

    public class GetSkillsetsOnProjectReportsHandler : IRequestHandler<GetSkillsetsOnProjectsReports, ProjectSitesReportsOutputDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSkillsetsOnProjectReportsHandler(CINDBOneContext context, AutoMapper.IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ProjectSitesReportsOutputDto> Handle(GetSkillsetsOnProjectsReports request, CancellationToken cancellationToken)
        {


            ProjectSitesReportsOutputDto Res = new();
            List<TblOpProjectSitesDto> resList = new();

            var Branch = request.Input.CityCode == "" || request.Input.CityCode == null ?
                await _context.CompanyBranches.FirstOrDefaultAsync(e => e.Id == request.User.BranchId) :
                await _context.CompanyBranches.FirstOrDefaultAsync(e => e.BranchCode == request.Input.CityCode);
            var Company = await _context.Companies.ProjectTo<TblErpSysCompanyDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.Id == Branch.CompanyId);
            try
            {
                if (request.Input.CustomerCode != null && request.Input.CustomerCode != "")
                {

                    if (resList.Count > 0)
                    {
                        resList = resList.Where(e => e.CustomerCode == request.Input.CustomerCode).ToList();

                    }
                    else
                    {
                        resList = await _context.TblOpProjectSites.Where(e => e.CustomerCode == request.Input.CustomerCode


                         && e.IsConvrtedToContract
                        ).ProjectTo<TblOpProjectSitesDto>(_mapper.ConfigurationProvider).ToListAsync();

                    }

                }

                if (request.Input.SiteCode != null && request.Input.SiteCode != "")
                {

                    if (resList.Count > 0)
                    {
                        resList = resList.Where(e => e.SiteCode == request.Input.SiteCode).ToList();

                    }
                    else
                    {
                        resList = await _context.TblOpProjectSites.Where(e => e.SiteCode == request.Input.SiteCode


                         && e.IsConvrtedToContract
                        ).ProjectTo<TblOpProjectSitesDto>(_mapper.ConfigurationProvider).ToListAsync();

                    }

                }


                if (request.Input.CityCode != null && request.Input.CityCode != "")
                {

                    if (resList.Count > 0)
                    {
                        resList = resList.Where(e => e.BranchCode == request.Input.CityCode).ToList();

                    }
                    else
                    {
                        resList = await _context.TblOpProjectSites.Where(e => e.BranchCode == request.Input.CityCode

                         && e.IsConvrtedToContract
                        ).ProjectTo<TblOpProjectSitesDto>(_mapper.ConfigurationProvider).ToListAsync();

                        Company = await _context.Companies.ProjectTo<TblErpSysCompanyDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.Id == Branch.CompanyId);

                    }

                }


                if (request.Input.CityCode == "" && request.Input.CustomerCode == "" && request.Input.SiteCode=="")
                {
                    resList = await _context.TblOpProjectSites.Where(e =>
                             e.IsConvrtedToContract
                           ).ProjectTo<TblOpProjectSitesDto>(_mapper.ConfigurationProvider).ToListAsync();

                }




                Res.Company = Company is not null ? Company : new() { CompanyAddress = "", CompanyName = "", LogoImagePath = "", LogoURL = "" };
                Res.ProjectSites = resList;

                if (Res.ProjectSites.Count > 0)
                {
                    var customerCodes = Res.ProjectSites.GroupBy(e => e.CustomerCode).Select(c => new { CustomerCode = c.Key }).ToList();

                    var Customers = (from cc in customerCodes join c in _context.OprCustomers.ProjectTo<TblSndDefCustomerMasterDto>(_mapper.ConfigurationProvider) on cc.CustomerCode equals c.CustCode select c).ToList();
                    Res.Customers = Customers;
                }
                else
                {
                    Res.Customers = new();
                }
                return Res;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetProjectSiteReport Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return new() { Company = Company, ProjectSites = new() };
            }
        }
    }
    #endregion

    #region GetSkillsetsOnProjectsReportByProjectSites

    public class GetSkillsetsOnProjectsReportByProjectSites : IRequest<ProjectSiteReport>
    {
        public UserIdentityDto User { get; set; }
        public ProjectSiteReport Input { get; set; }
    }

    public class GetSkillsetsOnProjectsReportByProjectSitesHandler : IRequestHandler<GetSkillsetsOnProjectsReportByProjectSites, ProjectSiteReport>
    {
        private readonly CINDBOneContext _context;
        private readonly DMCContext _contextDMC;
        private readonly IMapper _mapper;
        public GetSkillsetsOnProjectsReportByProjectSitesHandler(CINDBOneContext context,DMCContext contextDMC, AutoMapper.IMapper mapper)
        {
            _context = context;
            _contextDMC = contextDMC;
            _mapper = mapper;
        }
        public async Task<ProjectSiteReport> Handle(GetSkillsetsOnProjectsReportByProjectSites request, CancellationToken cancellationToken)
        {
            try
            {
                request.Input.SkillsetsList = new List<OpSkillsetDto>();
                var siteData = await _context.OprSites.FirstOrDefaultAsync(e => e.SiteCode == request.Input.SiteCode);
                request.Input.SiteNameArb = siteData.SiteArbName;
                request.Input.SiteNameEng = siteData.SiteName;
                DateTime curDate = DateTime.UtcNow;
                request.Input.Status = curDate > request.Input.EndDate ? "Completed" : request.Input.IsSuspended ? "Suspended" : request.Input.IsClosed ? "Closed" : request.Input.IsInActive ? "InActive" : "InProgress";
                request.Input.TotalSkillSetsQuantity = 0;

                var SkillsetsOnProject =await _context.TblOpMonthlyRoasterForSites.AsNoTracking().Where(e => e.SiteCode == request.Input.SiteCode
         
                  && e.IsPrimaryResource
                  && e.Month==DateTime.UtcNow.Month
                  && e.Year==DateTime.UtcNow.Year
                  ).ToListAsync();
           
                if (SkillsetsOnProject.Count > 0)
                    foreach (var skillset in SkillsetsOnProject)
                    {
                       
                          var skillsetsData = await _context.TblOpSkillsets.AsNoTracking().ProjectTo<OpSkillsetDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.SkillSetCode == skillset.SkillsetCode);
                       
                        if (skillsetsData is not null)
                        {

                           

                           
                                if(!request.Input.SkillsetsList.Any(e=>e.SkillSetCode==skillset.SkillsetCode))
                                {

                                    skillsetsData.Quantity = SkillsetsOnProject.Where(e=>e.SkillsetCode==skillset.SkillsetCode).Count();   //Counting Primary Resources Only
                                    request.Input.SkillsetsList.Add(skillsetsData);
                                request.Input.TotalSkillSetsQuantity += skillsetsData.Quantity;
                                }
                               
                            
                        }
                    }











                return request.Input;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetProjectSiteReport Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return new();
            }
        }
    }
    #endregion

    #endregion


    


    #region GetCustomerComplaintsReport

    public class GetCustomerComplaintsReport : IRequest<CustomerComplaintsReportsOutputDto>
    {
        public UserIdentityDto User { get; set; }
        public CustomerComplaintsReportsInputDto Input { get; set; }
    }

    public class GetCustomerComplaintsReportHandler : IRequestHandler<GetCustomerComplaintsReport, CustomerComplaintsReportsOutputDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetCustomerComplaintsReportHandler(CINDBOneContext context, AutoMapper.IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<CustomerComplaintsReportsOutputDto> Handle(GetCustomerComplaintsReport request, CancellationToken cancellationToken)
        {
            var Branch = request.Input.BranchCode == "" || request.Input.BranchCode == null ?
                    await _context.CompanyBranches.FirstOrDefaultAsync(e => e.Id == request.User.BranchId) :
                    await _context.CompanyBranches.FirstOrDefaultAsync(e => e.BranchCode == request.Input.CityCode);
            if (Branch is null)
            {
                Branch = await _context.CompanyBranches.FirstOrDefaultAsync(e => e.Id == request.User.BranchId);
            }
            var Company = await _context.Companies.ProjectTo<TblErpSysCompanyDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.Id == Branch.CompanyId);
            var ReasonCodes = await _context.OprReasonCodes.ToListAsync();
            var Projectsites = await _context.TblOpProjectSites.ToListAsync();
            var Sites = await _context.OprSites.ToListAsync();
            var Customers = await _context.OprCustomers.ToListAsync();
            var Users = await _context.SystemLogins.ToListAsync();
            CustomerComplaintsReportsOutputDto res = new() { Company=Company,Complaints=new()};



            var complaints = await _context.TblOpCustomerComplaints.ToListAsync();
            if (request.Input.FromDate!=null)
            {
                complaints = complaints.Where(e=>e.ComplaintDate>=request.Input.FromDate).ToList();
            } 
            if (request.Input.ToDate!=null)
            {
                complaints = complaints.Where(e=>e.ComplaintDate<=request.Input.ToDate).ToList();
            } 
            if (!string.IsNullOrEmpty(request.Input.ProjectCode))
            {
                complaints = complaints.Where(e=>e.ProjectCode==request.Input.ProjectCode).ToList();
            } 
            if (!string.IsNullOrEmpty(request.Input.CustomerCode))
            {
                complaints = complaints.Where(e=>e.CustomerCode==request.Input.CustomerCode).ToList();
            } 
            if (!string.IsNullOrEmpty(request.Input.SiteCode))
            {
                complaints = complaints.Where(e=>e.SiteCode==request.Input.SiteCode).ToList();
               } 
            if (!string.IsNullOrEmpty(request.Input.ReasonCode))
            {
                complaints = complaints.Where(e=>e.ReasonCode==request.Input.ReasonCode).ToList();
                res.NameEngReasonCode = ReasonCodes.FirstOrDefault(x => x.ReasonCode == request.Input.ReasonCode).ReasonCodeNameEng;
                res.NameArbReasonCode = ReasonCodes.FirstOrDefault(x => x.ReasonCode == request.Input.ReasonCode).ReasonCodeNameArb;

            }
            if (!string.IsNullOrEmpty(request.Input.BranchCode))
            {
                complaints = complaints.Where(e=>e.BranchCode==request.Input.CityCode).ToList();
            }

            if (!string.IsNullOrEmpty(request.Input.BookedBy))
            {
                complaints = complaints.Where(e=>e.BookedBy.ToString()==request.Input.BookedBy).ToList();
                res.NameBookedBy = Users.FirstOrDefault(x => x.Id.ToString() == request.Input.BookedBy).UserName;
            }



            if (!string.IsNullOrEmpty(request.Input.StatusCode))
            {
                string aprv = request.Input.StatusCode;

                complaints = aprv switch
                {
                    "Open" => complaints.Where(e => e.IsOpen).ToList(),
                    "Closed" => complaints.Where(e => e.IsClosed).ToList(),
                    "Inprogress" => complaints.Where(e => e.IsInprogress).ToList(),
                   _ => complaints
                };
            }
            res.Complaints = complaints.Select(e=>new GetCustomerComplaintDto { 
            CustomerCode=e.CustomerCode,
            SiteCode=e.SiteCode,
            ProjectCode=e.ProjectCode,
            ReasonCode=e.ReasonCode,
            ComplaintBy=e.ComplaintBy,
            ClosedBy=e.ClosedBy,
            BookedBy=e.BookedBy,
            ComplaintDate=e.ComplaintDate,
            ClosingDate=e.ClosingDate,
            BranchCode=e.BranchCode,
            ReasonCodeNameEng=ReasonCodes.SingleOrDefault(x=>x.ReasonCode==e.ReasonCode).ReasonCodeNameEng,
            ReasonCodeNameAr=ReasonCodes.SingleOrDefault(x=>x.ReasonCode==e.ReasonCode).ReasonCodeNameArb,
            NameBookedBy=Users.SingleOrDefault(x=>x.Id==e.BookedBy).UserName,
            NameClosedBy=e.IsClosed?Users.SingleOrDefault(x=>x.Id==e.ClosedBy).UserName:"",
            CustomerNameArb=Customers.SingleOrDefault(x=>x.CustCode==e.CustomerCode).CustArbName,
            CustomerNameEng=Customers.SingleOrDefault(x=>x.CustCode==e.CustomerCode).CustName,
            ProjectNameAr=Projectsites.SingleOrDefault(x=>x.ProjectCode==e.ProjectCode&&x.SiteCode==e.SiteCode).ProjectNameArb,
            ProjectNameEng=Projectsites.SingleOrDefault(x=>x.ProjectCode==e.ProjectCode&&x.SiteCode==e.SiteCode).ProjectNameEng,
            SiteNameAr=Sites.SingleOrDefault(x=>x.SiteCode==e.SiteCode).SiteArbName,
            SiteNameEng=Sites.SingleOrDefault(x=>x.SiteCode==e.SiteCode).SiteName,
             Status=e.IsOpen?"Open":e.IsInprogress?"Inprogress":e.IsClosed?"Closed":"",
            CustomerAddress = Customers.SingleOrDefault(x => x.CustCode == e.CustomerCode).CustAddress1,
            SiteAddress = Sites.SingleOrDefault(x => x.SiteCode == e.SiteCode).SiteAddress,

            }).ToList();
            return res;


        }
    }
    #endregion



    #region GetCustomerVisitsReport

    public class GetCustomerVisitsReport : IRequest<CustomerVisitsReportsOutputDto>
    {
        public UserIdentityDto User { get; set; }
        public CustomerVisitsReportsInputDto Input { get; set; }
    }

    public class GetCustomerVisitsReportHandler : IRequestHandler<GetCustomerVisitsReport, CustomerVisitsReportsOutputDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetCustomerVisitsReportHandler(CINDBOneContext context, AutoMapper.IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<CustomerVisitsReportsOutputDto> Handle(GetCustomerVisitsReport request, CancellationToken cancellationToken)
        {
            var Branch = request.Input.BranchCode == "" || request.Input.BranchCode == null ?
                   await _context.CompanyBranches.FirstOrDefaultAsync(e => e.Id == request.User.BranchId) :
                   await _context.CompanyBranches.FirstOrDefaultAsync(e => e.BranchCode == request.Input.CityCode);
            if(Branch is null)
            {
                Branch = await _context.CompanyBranches.FirstOrDefaultAsync(e => e.Id == request.User.BranchId);
            }
            var Company = await _context.Companies.ProjectTo<TblErpSysCompanyDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.Id == Branch.CompanyId);
            var ReasonCodes = await _context.OprReasonCodes.ToListAsync();
            var Projectsites = await _context.TblOpProjectSites.ToListAsync();
            var Sites = await _context.OprSites.ToListAsync();
            var Customers = await _context.OprCustomers.ToListAsync();
            var Users = await _context.SystemLogins.ToListAsync();
            CustomerVisitsReportsOutputDto res = new() { Company = Company, Visits = new() };



            var Visits = await _context.TblOpCustomerVisitForms.ToListAsync();
            if (request.Input.FromDate != null)
            {
                Visits = Visits.Where(e => e.ScheduleDateTime >= request.Input.FromDate).ToList();
            }
            if (request.Input.ToDate != null)
            {
                Visits = Visits.Where(e => e.ScheduleDateTime <= request.Input.ToDate).ToList();
            }
            if (!string.IsNullOrEmpty(request.Input.ProjectCode))
            {
                Visits = Visits.Where(e => e.ProjectCode == request.Input.ProjectCode).ToList();
            }
            if (!string.IsNullOrEmpty(request.Input.CustomerCode))
            {
                Visits = Visits.Where(e => e.CustomerCode == request.Input.CustomerCode).ToList();
            }
            if (!string.IsNullOrEmpty(request.Input.SiteCode))
            {
                Visits = Visits.Where(e => e.SiteCode == request.Input.SiteCode).ToList();
            }
            if (!string.IsNullOrEmpty(request.Input.ReasonCode))
            {
                Visits = Visits.Where(e => e.ReasonCode == request.Input.ReasonCode).ToList();
                res.NameEngReasonCode = ReasonCodes.FirstOrDefault(x => x.ReasonCode == request.Input.ReasonCode).ReasonCodeNameEng;
                res.NameArbReasonCode = ReasonCodes.FirstOrDefault(x => x.ReasonCode == request.Input.ReasonCode).ReasonCodeNameArb;

            }
            if (!string.IsNullOrEmpty(request.Input.CityCode))
            {
                Visits = Visits.Where(e => e.BranchCode == request.Input.BranchCode).ToList();
            }

            if (!string.IsNullOrEmpty(request.Input.SupervisorId))
            {
                Visits = Visits.Where(e => e.SupervisorId.ToString() == request.Input.SupervisorId).ToList();
                res.NameSupervisor = Users.FirstOrDefault(x => x.Id.ToString() == request.Input.SupervisorId).UserName;
            }

             if (!string.IsNullOrEmpty(request.Input.VisitedBy))
            {
                Visits = Visits.Where(e => e.VisitedBy.ToString() == request.Input.VisitedBy).ToList();
                res.NameVisitedBy = Users.FirstOrDefault(x => x.Id.ToString() == request.Input.VisitedBy).UserName;
            }



            if (!string.IsNullOrEmpty(request.Input.StatusCode))
            {
                string aprv = request.Input.StatusCode;

                Visits = aprv switch
                {
                    "Open" => Visits.Where(e => e.IsOpen).ToList(),
                    "Closed" => Visits.Where(e => e.IsClosed).ToList(),
                    "Inprogress" => Visits.Where(e => e.IsInprogress).ToList(),
                    _ => Visits
                };
            }
            res.Visits = Visits.Select(e => new GetCustomerVisitFormDto
            {
                CustomerCode = e.CustomerCode,
                SiteCode = e.SiteCode,
                ProjectCode = e.ProjectCode,
                ReasonCode = e.ReasonCode,
                
                BranchCode = e.BranchCode,
                ReasonCodeNameEng = ReasonCodes.SingleOrDefault(x => x.ReasonCode == e.ReasonCode).ReasonCodeNameEng,
                ReasonCodeNameAr = ReasonCodes.SingleOrDefault(x => x.ReasonCode == e.ReasonCode).ReasonCodeNameArb,
                NameVisitedBy = e.IsClosed?Users.SingleOrDefault(x => x.Id == e.VisitedBy).UserName??"":"",
                CustomerNameArb = Customers.SingleOrDefault(x => x.CustCode == e.CustomerCode).CustArbName,
                CustomerNameEng = Customers.SingleOrDefault(x => x.CustCode == e.CustomerCode).CustName,
                ProjectNameAr = Projectsites.SingleOrDefault(x => x.ProjectCode == e.ProjectCode && x.SiteCode == e.SiteCode).ProjectNameArb,
                ProjectNameEng = Projectsites.SingleOrDefault(x => x.ProjectCode == e.ProjectCode && x.SiteCode == e.SiteCode).ProjectNameEng,
                SiteNameAr = Sites.SingleOrDefault(x => x.SiteCode == e.SiteCode).SiteArbName,
                SiteNameEng = Sites.SingleOrDefault(x => x.SiteCode == e.SiteCode).SiteName,
                Status = e.IsOpen ? "Open" : e.IsInprogress ? "Inprogress" : e.IsClosed ? "Closed" : "",
                CustomerAddress = Customers.SingleOrDefault(x => x.CustCode == e.CustomerCode).CustAddress1,
                SiteAddress = Sites.SingleOrDefault(x => x.SiteCode == e.SiteCode).SiteAddress,
                NameSupervisorId= Users.SingleOrDefault(x => x.Id == e.SupervisorId).UserName,
                ScheduleDateTime=e.ScheduleDateTime,
                VisitedDateTime=e.VisitedDateTime,
                IsClosed=e.IsClosed,
                IsOpen=e.IsOpen,
                IsInprogress=e.IsInprogress,
                VisitedBy=e.VisitedBy
            }).ToList();
            return res;


        }
    }
    #endregion









    #region AttendanceStatusReport

    

    public class AttendanceStatusReportForPayRollPeriodQuery : IRequest<Output_OpAttendanceStatusReportForPayRollPeriodDto>
    {
        public UserIdentityDto User { get; set; }
        public Input_OpAttendanceStatusReportForPayRollPeriodDto Input { get; set; }


    }

    public class AttendanceStatusReportForPayRollPeriodQueryHandler : IRequestHandler<AttendanceStatusReportForPayRollPeriodQuery, Output_OpAttendanceStatusReportForPayRollPeriodDto>
    {
        private readonly CINDBOneContext _context;
        private readonly DMCContext _contextDMC;
        private readonly IMapper _mapper;

        public AttendanceStatusReportForPayRollPeriodQueryHandler(CINDBOneContext context, DMCContext contextDMC, IMapper mapper)
        {
            _context = context;
            _contextDMC = contextDMC;
            _mapper = mapper;
        }

        public async Task<Output_OpAttendanceStatusReportForPayRollPeriodDto> Handle(AttendanceStatusReportForPayRollPeriodQuery request, CancellationToken cancellationToken)
        {
            var Branch = request.Input.BranchCode == "" || request.Input.BranchCode == null ?
              await _context.CompanyBranches.FirstOrDefaultAsync(e => e.Id == request.User.BranchId) :
              await _context.CompanyBranches.FirstOrDefaultAsync(e => e.BranchCode == request.Input.BranchCode)?? await _context.CompanyBranches.FirstOrDefaultAsync(e => e.Id == request.User.BranchId);

            



            var Company = await _context.Companies.ProjectTo<TblErpSysCompanyDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.Id == Branch.CompanyId);

            try
            {
              
                DateTime startDate = Convert.ToDateTime(request.Input.PayrollStartDate, CultureInfo.InvariantCulture);
                bool IsSingleMonth = request.Input.PayrollStartDate.Day == 1;
                if (startDate.Day > 28)
                {
                    return new() { IsValidReq = false, ErrorMsg = "Invalid Date (Date Should Between 1-25)" };
                }
                DateTime endDateTemp = IsSingleMonth ? new DateTime(startDate.Year, startDate.Month, DateTime.DaysInMonth(startDate.Year, startDate.Month)) : new DateTime(startDate.Month < 12 ? startDate.Year : startDate.Year + 1, startDate.Month < 12 ? startDate.Month + 1 : 1, startDate.Day - 1);
                DateTime endDate = Convert.ToDateTime(endDateTemp, CultureInfo.InvariantCulture);

                Output_OpAttendanceStatusReportForPayRollPeriodDto Res = new()
                {
                    Columns = new(),
                    Rows = new(),

                    IsSingleMonth = IsSingleMonth,
                    IsValidReq = false,
                    DaysInMonth1 = IsSingleMonth ? DateTime.DaysInMonth(startDate.Year, startDate.Month) : DateTime.DaysInMonth(startDate.Year, startDate.Month) - startDate.Day + 1,
                    DaysInMonth2 = IsSingleMonth ? 0 : endDate.Day,
                    ErrorMsg = "Processing Data Failed",
                    Month1Text = startDate.ToString("MMM"),
                    Month2Text = endDate.ToString("MMM"),
                    Year1Text = startDate.ToString("yyyy"),
                    Year2Text = endDate.ToString("yyyy"),
                    TotalItemsCount=0,
                };





                for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
                {
                    Column_OpAttendanceStatusReportForPayRollPeriodDto column = new()
                    {
                        AttnDate = date,
                        Day = date.Day,
                        Month = date.Month,
                        Year = date.Year,
                        DayText = date.ToString().Substring(0, 1).ToUpper()
                    };
                    Res.Columns.Add(column);

                }
                List<TblOpProjectSites> ProjectSites = _context.TblOpProjectSites.Where(e=>e.StartDate<=request.Input.PayrollStartDate && e.EndDate >= request.Input.PayrollStartDate).OrderBy(e=>e.BranchCode).ThenBy(e=>e.CustomerCode).ToList();
                if(!string.IsNullOrEmpty(request.Input.BranchCode))
                {
                    ProjectSites = ProjectSites.Where(e=>e.BranchCode== request.Input.BranchCode).ToList();
                }
                if(!string.IsNullOrEmpty(request.Input.CustomerCode))
                {
                    ProjectSites = ProjectSites.Where(e=>e.CustomerCode== request.Input.CustomerCode).ToList();
                }
                 if(!string.IsNullOrEmpty(request.Input.ProjectCode))
                {
                    ProjectSites = ProjectSites.Where(e=>e.ProjectCode== request.Input.ProjectCode).ToList();
                }
                Res.TotalItemsCount = ProjectSites.Count();
                ProjectSites = ProjectSites.Skip(request.Input.PageSize.Value * request.Input.PageNumber.Value).Take(request.Input.PageSize.Value).ToList();

                var attandance = _context.EmployeeAttendance.Where(e => ProjectSites.Select(p => p.ProjectCode).Contains(e.ProjectCode)
                           && ProjectSites.Select(s => s.SiteCode).Contains(e.SiteCode)
                           && Res.Columns.Select(a=>a.AttnDate).Contains(e.AttnDate)).ToList();

                foreach (var ps in ProjectSites)
                {
                    Row_OpAttendanceStatusReportForPayRollPeriodDto row = new() { 
                    BranchCode=ps.BranchCode,
                    CustomerCode=ps.CustomerCode,
                    ProjectCode=ps.ProjectCode,
                    SiteCode=ps.SiteCode,
                    AttendanceStatusMatrix=new()
                    };
                   
                    foreach(var col in Res.Columns)
                    {
                        row.AttendanceStatusMatrix.Add(new()
                        {
                            IsAttnDrafted = attandance.Any(e => e.ProjectCode == row.ProjectCode
                              && e.SiteCode == row.SiteCode
                              && e.AttnDate == col.AttnDate
                              && !e.isPosted),
                        
                        IsAttnPosted= attandance.Any(e=>e.ProjectCode==row.ProjectCode
                         &&e.SiteCode==row.SiteCode
                         &&e.AttnDate==col.AttnDate 
                         &&e.isPosted),
                        });

                    }

                    Res.Rows.Add(row);

                }

                Company = await _context.Companies.ProjectTo<TblErpSysCompanyDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.Id == Branch.CompanyId); 
                Res.Company = Company is not null ? Company : new() { CompanyAddress = "", CompanyName = "", LogoImagePath = "", LogoURL = "" };

                Res.IsValidReq = true;

                return Res;
            }
            catch (Exception ex)
            {

                Log.Error("Error in AttendanceStatusReportForPayRollPeriodQuery Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return new() { IsValidReq = false, ErrorMsg = ex.Message };

            }
        }
    }


    #endregion



}
