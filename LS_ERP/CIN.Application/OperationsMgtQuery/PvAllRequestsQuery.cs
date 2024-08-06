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
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace CIN.Application.OperationsMgtQuery
{



    
    #region GetPvAllRequestsPagedList

    public class GetPvAllRequestsPagedList : IRequest<PaginatedList<OpPvAllRequestsPaginationDto>>
    {
        public UserIdentityDto User { get; set; }
        public OprPaginationFilterDto Input { get; set; }
    }

    public class GetPvAllRequestsPagedListHandler : IRequestHandler<GetPvAllRequestsPagedList, PaginatedList<OpPvAllRequestsPaginationDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetPvAllRequestsPagedListHandler(CINDBOneContext context, AutoMapper.IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<OpPvAllRequestsPaginationDto>> Handle(GetPvAllRequestsPagedList request, CancellationToken cancellationToken)
        {
            try
            {
                bool isAdmin = _context.SystemLogins.AsNoTracking().FirstOrDefault(e => e.Id == request.User.UserId).LoginId == "Admin" ? true : false;

                var oprAuths = _context.TblOpAuthoritiesList.AsNoTracking();
                var Projects = _context.OP_HRM_TEMP_Projects.AsNoTracking();
                var Customers = _context.OprCustomers.AsNoTracking();
                var Sites = _context.OprSites.AsNoTracking();
                var search = request.Input.Query;
                var Users = _context.SystemLogins.AsNoTracking();
                var resMaps = _context.TblOpPvAddResourceEmployeeToResourceMaps.OrderBy(e=>e.FromDate).AsNoTracking();
                var AuthSites = Sites.Join(oprAuths, s => s.SiteCityCode, a => a.BranchCode, (s, a) => new
                {
                    s.SiteCityCode,
                    a.AppAuth,
                    a.CanApprovePvReq,
                    s.SiteCode
                }).AsNoTracking();


                var list = _context.TblOpPvRemoveResourceReqs
                   .Select(rr => new OpPvAllRequestsPaginationDto
                   {
                       Id=rr.Id,
                       CustomerCode = rr.CustomerCode,
                       SiteCode = rr.SiteCode,
                       SiteName = Sites.FirstOrDefault(s => s.SiteCode == rr.SiteCode).SiteName,
                       SiteNameAr = Sites.FirstOrDefault(s => s.SiteCode == rr.SiteCode).SiteArbName,
                       ProjectCode = rr.ProjectCode,
                       ProjectName = Projects.FirstOrDefault(p => p.ProjectCode == rr.ProjectCode).ProjectNameEng,
                       ProjectNameAr = Projects.FirstOrDefault(p => p.ProjectCode == rr.ProjectCode).ProjectNameArb,
                       EffectiveDate = rr.FromDate,
                       Reamrks = "",
                       RequestedBy = Users.FirstOrDefault(u => u.Id == rr.CreatedBy).UserName,
                       RequestNumber = rr.Id,
                       RequestType = "RemoveResource",

                       RequestedDate = rr.Created,

                       IsApproved = rr.IsApproved,
                       IsMerged = rr.IsActive,
                       IsAdmin = isAdmin,
                       RequestSubType = "",
                       CanEditReq = rr.CreatedBy == request.User.UserId,
                       //CanApproveReq = AuthSites.Any(e => e.AppAuth == request.User.UserId && e.SiteCode == rr.SiteCode && e.CanApprovePvReq) || isAdmin,
                       CanApproveReq = oprAuths.Any(a => a.AppAuth == request.User.UserId && a.CanApprovePvReq && a.BranchCode == Sites.FirstOrDefault(s => s.SiteCode == rr.SiteCode).SiteCityCode) || isAdmin,
                       IsMappedResources = rr.IsApproved,
                      FileUrl=rr.FileUrl,
                       IsFileUploadRequired=true
                   }).Concat(_context.TblOpPvReplaceResourceReqs.AsNoTracking()
                   .Select(rp => new OpPvAllRequestsPaginationDto
                   {
                       Id=rp.Id,
                       CustomerCode = rp.CustomerCode,
                       SiteCode = rp.SiteCode,
                       SiteName = Sites.FirstOrDefault(s => s.SiteCode == rp.SiteCode).SiteName,
                       SiteNameAr = Sites.FirstOrDefault(s => s.SiteCode == rp.SiteCode).SiteArbName,
                       ProjectCode = rp.ProjectCode,
                       ProjectName = Projects.FirstOrDefault(p => p.ProjectCode == rp.ProjectCode).ProjectNameEng,
                       ProjectNameAr = Projects.FirstOrDefault(p => p.ProjectCode == rp.ProjectCode).ProjectNameArb,
                       EffectiveDate = rp.FromDate.Value,
                       Reamrks = "",
                       RequestedBy = Users.FirstOrDefault(u => u.Id == rp.CreatedBy).UserName,
                       RequestNumber = rp.Id,
                       RequestType = "ReplaceResource",
                       RequestedDate = rp.Created,

                       IsApproved = rp.IsApproved,
                       RequestSubType = "",
                       IsAdmin = isAdmin,
                       IsMerged = rp.IsApproved,
                       CanEditReq = rp.CreatedBy == request.User.UserId,
                     //  CanApproveReq = AuthSites.Any(e => e.AppAuth == request.User.UserId && e.SiteCode == rp.SiteCode && e.CanApprovePvReq) || isAdmin,
                       CanApproveReq = oprAuths.Any(a => a.AppAuth == request.User.UserId && a.CanApprovePvReq && a.BranchCode == Sites.FirstOrDefault(s => s.SiteCode == rp.SiteCode).SiteCityCode) || isAdmin,

                       IsMappedResources = rp.IsApproved,
                      FileUrl=rp.FileUrl,
                       IsFileUploadRequired = false
                   })
                   ).Concat(_context.TblOpPvTransferResourceReqs
                   .Select(tr => new OpPvAllRequestsPaginationDto
                   {
                       Id=tr.Id,
                       CustomerCode = tr.SrcCustomerCode,
                       SiteCode = tr.SrcSiteCode,
                       SiteName = Sites.FirstOrDefault(s => s.SiteCode == tr.SrcSiteCode).SiteName,
                       SiteNameAr = Sites.FirstOrDefault(s => s.SiteCode == tr.SrcSiteCode).SiteArbName,
                       ProjectCode = tr.SrcProjectCode,
                       ProjectName = Projects.FirstOrDefault(p => p.ProjectCode == tr.SrcProjectCode).ProjectNameEng,
                       ProjectNameAr = Projects.FirstOrDefault(p => p.ProjectCode == tr.SrcProjectCode).ProjectNameArb,
                       EffectiveDate = tr.FromDate.Value,
                       Reamrks = "",
                       RequestedBy = Users.FirstOrDefault(u => u.Id == tr.CreatedBy).UserName,
                       RequestNumber = tr.Id,
                       RequestType = "TransferResource",
                       RequestedDate = tr.Created,

                       IsApproved = tr.IsApproved,
                       IsAdmin = isAdmin,
                       RequestSubType = "",
                       IsMerged = tr.IsApproved,

                       CanEditReq = tr.CreatedBy == request.User.UserId,
                     //  CanApproveReq = AuthSites.Any(e => e.AppAuth == request.User.UserId && e.SiteCode == tr.SrcSiteCode && e.CanApprovePvReq) || isAdmin,
                       CanApproveReq = oprAuths.Any(a => a.AppAuth == request.User.UserId && a.CanApprovePvReq && a.BranchCode == Sites.FirstOrDefault(s => s.SiteCode == tr.SrcSiteCode).SiteCityCode) || isAdmin,

                       IsMappedResources = tr.IsApproved,
                      FileUrl=tr.FileUrl,
                       IsFileUploadRequired = false
                   })
                   ).Concat(_context.PvAddResorceHeads.AsNoTracking()
                   .Select(ar => new OpPvAllRequestsPaginationDto
                   {
                       Id=ar.Id,
                       CustomerCode = ar.CustomerCode,
                       SiteCode = ar.SiteCode,
                       SiteName = Sites.FirstOrDefault(s => s.SiteCode == ar.SiteCode).SiteName,
                       SiteNameAr = Sites.FirstOrDefault(s => s.SiteCode == ar.SiteCode).SiteArbName,
                       ProjectCode = ar.ProjectCode,
                       ProjectName = Projects.FirstOrDefault(p => p.ProjectCode == ar.ProjectCode).ProjectNameEng,
                       ProjectNameAr = Projects.FirstOrDefault(p => p.ProjectCode == ar.ProjectCode).ProjectNameArb,
                       EffectiveDate = ar.IsEmpMapped ? resMaps.FirstOrDefault(rm => rm.PvAddResReqId == ar.Id).FromDate.Value : new DateTime(),
                       Reamrks = "",
                       RequestedBy = Users.FirstOrDefault(u => u.Id == ar.CreatedBy).UserName,
                       RequestNumber = ar.Id,
                       RequestType = "AddResource",
                       IsAdmin = isAdmin,
                       RequestSubType = "",
                       IsMerged = ar.IsMerged,
                       RequestedDate = ar.Created,
                       IsMappedResources = ar.IsEmpMapped,
                       IsApproved = ar.IsApproved,
                       CanEditReq = ar.CreatedBy == request.User.UserId,
                       //CanApproveReq = AuthSites.Any(e => e.AppAuth == request.User.UserId && e.SiteCode == ar.SiteCode && e.CanApprovePvReq) || isAdmin,
                       CanApproveReq = oprAuths.Any(a => a.AppAuth == request.User.UserId && a.CanApprovePvReq && a.BranchCode == Sites.FirstOrDefault(s => s.SiteCode == ar.SiteCode).SiteCityCode) || isAdmin,


                       FileUrl = ar.FileUrl,
                       IsFileUploadRequired = true


                   })
                   ).Concat(_context.TblOpPvOpenCloseReqs
                   .Select(ar => new OpPvAllRequestsPaginationDto
                   {
                       Id=ar.Id,
                       CustomerCode = ar.CustomerCode,
                       SiteCode = ar.SiteCode,
                       SiteName = Sites.FirstOrDefault(s => s.SiteCode == ar.SiteCode).SiteName,
                       SiteNameAr = Sites.FirstOrDefault(s => s.SiteCode == ar.SiteCode).SiteArbName,
                       ProjectCode = ar.ProjectCode,
                       ProjectName = Projects.FirstOrDefault(p => p.ProjectCode == ar.ProjectCode).ProjectNameEng,
                       ProjectNameAr = Projects.FirstOrDefault(p => p.ProjectCode == ar.ProjectCode).ProjectNameArb,
                       EffectiveDate = ar.EffectiveDate.Value,
                       Reamrks = "",
                       RequestedBy = Users.FirstOrDefault(u => u.Id == ar.CreatedBy).UserName,
                       RequestNumber = ar.Id,
                       RequestType = "ProjectVariations",
                       RequestSubType = ar.IsCloseReq ? "(Project_Closing_Request)"
                       : ar.IsExtendProjReq ? "(Project_Extend_Request)"
                       : ar.IsReOpenReq ? "(Project_Reopen_Request)"
                       : ar.IsCancelReq ? "(Project_Cancel_Request)"
                       : ar.IsCloseReq ? "(Project_Closing_Request)"
                       : ar.IsRevokeSuspReq ? "(Revoke_Project_Suspension_Request)"
                       : ar.IsSuspendReq ? "(Project_Suspension_Request)"
                       : string.Empty,
                       RequestedDate = ar.Created,

                       IsApproved = ar.IsApproved,
                       IsAdmin = isAdmin,
                       IsMerged = ar.IsApproved,
                       CanEditReq = ar.CreatedBy == request.User.UserId,
                      // CanApproveReq = AuthSites.Any(e => e.AppAuth == request.User.UserId && e.SiteCode == ar.SiteCode && e.CanApprovePvReq) || isAdmin,
                       CanApproveReq = oprAuths.Any(a => a.AppAuth == request.User.UserId && a.CanApprovePvReq && a.BranchCode == Sites.FirstOrDefault(s => s.SiteCode == ar.SiteCode).SiteCityCode) || isAdmin,


                       IsMappedResources = ar.IsApproved,

                      FileUrl=ar.FileUrl,
                       IsFileUploadRequired = false
                   })
                   ).Concat(_context.TblOpPvTransferWithReplacementReqs
                   .Select(twr => new OpPvAllRequestsPaginationDto
                   {
                       Id=twr.Id,
                       CustomerCode = twr.SrcCustomerCode,
                       SiteCode = twr.SrcSiteCode,
                       SiteName = Sites.FirstOrDefault(s => s.SiteCode == twr.SrcSiteCode).SiteName,
                       SiteNameAr = Sites.FirstOrDefault(s => s.SiteCode == twr.SrcSiteCode).SiteArbName,
                       ProjectCode = twr.SrcProjectCode,
                       ProjectName = Projects.FirstOrDefault(p => p.ProjectCode == twr.SrcProjectCode).ProjectNameEng,
                       ProjectNameAr = Projects.FirstOrDefault(p => p.ProjectCode == twr.SrcProjectCode).ProjectNameArb,
                       EffectiveDate = twr.FromDate.Value,
                       Reamrks = "",
                       RequestedBy = Users.FirstOrDefault(u => u.Id == twr.CreatedBy).UserName,
                       RequestNumber = twr.Id,
                       RequestType = "TransferWithReplacement",
                       RequestedDate = twr.Created,
                       IsMerged = twr.IsApproved,
                       IsAdmin = isAdmin,
                       RequestSubType = "",
                       IsApproved = twr.IsApproved,
                       CanEditReq = twr.CreatedBy == request.User.UserId,
                      // CanApproveReq = AuthSites.Any(e => e.AppAuth == request.User.UserId && e.SiteCode == twr.SrcSiteCode && e.CanApprovePvReq) || isAdmin,
                       CanApproveReq = oprAuths.Any(a => a.AppAuth == request.User.UserId && a.CanApprovePvReq && a.BranchCode == Sites.FirstOrDefault(s => s.SiteCode == twr.SrcSiteCode).SiteCityCode) || isAdmin,

                       IsMappedResources = twr.IsApproved,
                      FileUrl=twr.FileUrl,
                       IsFileUploadRequired = false

                   })
                   ).Concat(_context.TblOpPvSwapEmployeesReqs
                   .Select(se => new OpPvAllRequestsPaginationDto
                   {
                       Id=se.Id,
                       CustomerCode = se.SrcCustomerCode,
                       SiteCode = se.SrcSiteCode,
                       SiteName = Sites.FirstOrDefault(s => s.SiteCode == se.SrcSiteCode).SiteName,
                       SiteNameAr = Sites.FirstOrDefault(s => s.SiteCode == se.SrcSiteCode).SiteArbName,
                       ProjectCode = se.SrcProjectCode,
                       ProjectName = Projects.FirstOrDefault(p => p.ProjectCode == se.SrcProjectCode).ProjectNameEng,
                       ProjectNameAr = Projects.FirstOrDefault(p => p.ProjectCode == se.SrcProjectCode).ProjectNameArb,
                       EffectiveDate = se.FromDate.Value,
                       Reamrks = "",
                       RequestedBy = Users.FirstOrDefault(u => u.Id == se.CreatedBy).UserName,
                       RequestNumber = se.Id,
                       RequestType = "SwapEmployees",
                       RequestedDate = se.Created,
                       IsMerged = se.IsApproved,
                       IsAdmin = isAdmin,
                       RequestSubType = string.Empty,
                       IsApproved = se.IsApproved,
                       CanEditReq = se.CreatedBy == request.User.UserId,
                       //CanApproveReq = oprAuths.Any(a => a.AppAuth == request.User.UserId && a.CanApprovePvReq && a.BranchCode == Sites.FirstOrDefault(s => s.SiteCode == se.DestSiteCode).SiteCityCode) || isAdmin,
                       CanApproveReq = oprAuths.Any(a => a.AppAuth == request.User.UserId && a.CanApprovePvReq && a.BranchCode == Sites.FirstOrDefault(s => s.SiteCode == se.DestSiteCode).SiteCityCode) || isAdmin,


                       IsMappedResources = false,
                      FileUrl=se.FileUrl,
                       IsFileUploadRequired = false

                   })
                   ) .Where(e =>
                                (e.CustomerCode.Contains(search) ||
                                e.SiteCode.Contains(search) ||
                                e.ProjectCode.Contains(search) ||
                                e.SiteName.Contains(search) ||
                                e.SiteNameAr.Contains(search) ||
                                e.ProjectName.Contains(search) ||
                                e.ProjectNameAr.Contains(search) ||
                                 e.RequestType.Contains(search) ||
                                 e.RequestedBy.Contains(search) ||
                                search == "" || search == null
                                 ) && e.CanApproveReq || e.CanEditReq);

                if (!string.IsNullOrEmpty(request.Input.Approval))
                {
                    string aprv = request.Input.Approval;

                    list = aprv switch
                    {
                        "approved" => list.Where(e => e.IsApproved),
                        "unapproved" => list.Where(e => !e.IsApproved),
                        _ => list
                    };
                }
                if (!string.IsNullOrEmpty(request.Input.ListType))
                {
                    string aprv = request.Input.ListType;

                    list = aprv switch
                    {
                        "addResource" => list.Where(e => e.RequestType.Equals("AddResource")),
                        "removeResource" => list.Where(e => e.RequestType.Equals("RemoveResource")),
                        "replaceResource" => list.Where(e => e.RequestType.Equals("ReplaceResource")),
                        "swapEmployees" => list.Where(e => e.RequestType.Equals("SwapEmployees")),
                        "transferWithReplacement" => list.Where(e => e.RequestType.Equals("TransferWithReplacement")),
                        "transferResource" => list.Where(e => e.RequestType.Equals("TransferResource")),
                        "projectVariations" => list.Where(e => e.RequestType.Equals("ProjectVariations")),
                        _ => list
                    };
                }





                var nreports = await list.OrderBy(request.Input.OrderBy).PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);
                Log.Info("----Info GetSndQuotationPagedList method Exit----");
                return nreports;

            }
            catch (Exception ex)
            {
                Log.Error("Error in GetPvAllRequestsPagedList Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return null;
            }
        }
    }
    #endregion


    #region GetPvAllRequestsPagedListByProjectSite

    public class GetPvAllRequestsPagedListByProjectSite : IRequest<PaginatedList<OpPvAllRequestsPaginationDto>>
    {
        public UserIdentityDto User { get; set; }
        public OprPaginationFilterDto Input { get; set; }
        public string ProjectCode { get; set; }
        public string SiteCode { get; set; }
    }

    public class GetPvAllRequestsPagedListByProjectSiteHandler : IRequestHandler<GetPvAllRequestsPagedListByProjectSite, PaginatedList<OpPvAllRequestsPaginationDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetPvAllRequestsPagedListByProjectSiteHandler(CINDBOneContext context, AutoMapper.IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<OpPvAllRequestsPaginationDto>> Handle(GetPvAllRequestsPagedListByProjectSite request, CancellationToken cancellationToken)
        {
            try
            {
                bool isAdmin = _context.SystemLogins.AsNoTracking().FirstOrDefault(e => e.Id == request.User.UserId).LoginId == "Admin" ? true : false;

                var oprAuths = _context.TblOpAuthoritiesList.AsNoTracking();
                var Projects = _context.OP_HRM_TEMP_Projects.AsNoTracking();
                var Customers = _context.OprCustomers.AsNoTracking();
                var Sites = _context.OprSites.AsNoTracking();
                var search = request.Input.Query;
                var Users = _context.SystemLogins.AsNoTracking();
                var resMaps = _context.TblOpPvAddResourceEmployeeToResourceMaps.OrderBy(e=>e.FromDate).AsNoTracking();
                var AuthSites = Sites.Join(oprAuths, s => s.SiteCityCode, a => a.BranchCode, (s, a) => new
                {
                    s.SiteCityCode,
                    a.AppAuth,
                    a.CanApprovePvReq,
                    s.SiteCode
                }).AsNoTracking();








                var list =  _context.TblOpPvRemoveResourceReqs.Where(e=>e.ProjectCode==request.ProjectCode&& e.SiteCode==request.SiteCode)
                   .Select(rr => new OpPvAllRequestsPaginationDto
                   {
                       Id=rr.Id,
                       CustomerCode = rr.CustomerCode,
                       SiteCode = rr.SiteCode,
                       SiteName = Sites.FirstOrDefault(s => s.SiteCode == rr.SiteCode).SiteName,
                       SiteNameAr = Sites.FirstOrDefault(s => s.SiteCode == rr.SiteCode).SiteArbName,
                       ProjectCode = rr.ProjectCode,
                       ProjectName = Projects.FirstOrDefault(p => p.ProjectCode == rr.ProjectCode).ProjectNameEng,
                       ProjectNameAr = Projects.FirstOrDefault(p => p.ProjectCode == rr.ProjectCode).ProjectNameArb,
                       EffectiveDate = rr.FromDate,
                       Reamrks = "",
                       RequestedBy = Users.FirstOrDefault(u => u.Id == rr.CreatedBy).UserName,
                       RequestNumber = rr.Id,
                       RequestType = "RemoveResource",
                       RequestSubType="",
                       RequestedDate = rr.Created,

                       IsApproved = rr.IsApproved,
                       
                       CanEditReq = rr.CreatedBy == request.User.UserId,
                       //CanApproveReq = AuthSites.Any(e => e.AppAuth == request.User.UserId && e.SiteCode == rr.SiteCode && e.CanApprovePvReq) || isAdmin,
                       CanApproveReq = oprAuths.Any(a => a.AppAuth == request.User.UserId && a.CanApprovePvReq && a.BranchCode == Sites.FirstOrDefault(s => s.SiteCode == rr.SiteCode).SiteCityCode) || isAdmin,

                       IsMappedResources = rr.IsApproved,
                       IsMerged=rr.IsApproved,
                       IsAdmin=isAdmin,
                       FileUrl=rr.FileUrl,
                       IsFileUploadRequired = true
                   }).Concat(_context.TblOpPvReplaceResourceReqs.Where(e => e.ProjectCode == request.ProjectCode && e.SiteCode == request.SiteCode)
                   .Select(rp => new OpPvAllRequestsPaginationDto
                   {
                       Id=rp.Id,
                       CustomerCode = rp.CustomerCode,
                       SiteCode = rp.SiteCode,
                       SiteName = Sites.FirstOrDefault(s => s.SiteCode == rp.SiteCode).SiteName,
                       SiteNameAr = Sites.FirstOrDefault(s => s.SiteCode == rp.SiteCode).SiteArbName,
                       ProjectCode = rp.ProjectCode,
                       ProjectName = Projects.FirstOrDefault(p => p.ProjectCode == rp.ProjectCode).ProjectNameEng,
                       ProjectNameAr = Projects.FirstOrDefault(p => p.ProjectCode == rp.ProjectCode).ProjectNameArb,
                       EffectiveDate = rp.FromDate.Value,
                       Reamrks = "",
                       RequestedBy = Users.FirstOrDefault(u => u.Id == rp.CreatedBy).UserName,
                       RequestNumber = rp.Id,
                       RequestType = "ReplaceResource",
                       RequestSubType = "",

                       RequestedDate = rp.Created,

                       IsApproved = rp.IsApproved,
                       IsMerged=rp.IsApproved,

                       CanEditReq = rp.CreatedBy == request.User.UserId,
                       //CanApproveReq = AuthSites.Any(e => e.AppAuth == request.User.UserId && e.SiteCode == rp.SiteCode && e.CanApprovePvReq) || isAdmin,
                       CanApproveReq = oprAuths.Any(a => a.AppAuth == request.User.UserId && a.CanApprovePvReq && a.BranchCode == Sites.FirstOrDefault(s => s.SiteCode == rp.SiteCode).SiteCityCode) || isAdmin,

                       IsMappedResources = rp.IsApproved,

                       IsAdmin=isAdmin,
                       FileUrl = rp.FileUrl,
                       IsFileUploadRequired = false

                   })
                   ).Concat(_context.TblOpPvTransferResourceReqs.Where(e => e.SrcProjectCode == request.ProjectCode && e.SrcSiteCode == request.SiteCode)
                   .Select(tr => new OpPvAllRequestsPaginationDto
                   {
                       Id=tr.Id,
                       CustomerCode = tr.SrcCustomerCode,
                       SiteCode = tr.SrcSiteCode,
                       SiteName = Sites.FirstOrDefault(s => s.SiteCode == tr.SrcSiteCode).SiteName,
                       SiteNameAr = Sites.FirstOrDefault(s => s.SiteCode == tr.SrcSiteCode).SiteArbName,
                       ProjectCode = tr.SrcProjectCode,
                       ProjectName = Projects.FirstOrDefault(p => p.ProjectCode == tr.SrcProjectCode).ProjectNameEng,
                       ProjectNameAr = Projects.FirstOrDefault(p => p.ProjectCode == tr.SrcProjectCode).ProjectNameArb,
                       EffectiveDate = tr.FromDate.Value,
                       Reamrks = "",
                       RequestedBy = Users.FirstOrDefault(u => u.Id == tr.CreatedBy).UserName,
                       RequestNumber = tr.Id,
                       RequestType = "TransferResource",
                       RequestSubType = "",

                       RequestedDate = tr.Created,

                       IsApproved = tr.IsApproved,
                       CanEditReq = tr.CreatedBy == request.User.UserId,
                     //  CanApproveReq = AuthSites.Any(e => e.AppAuth == request.User.UserId && e.SiteCode == tr.SrcSiteCode && e.CanApprovePvReq) || isAdmin,
                       CanApproveReq = oprAuths.Any(a => a.AppAuth == request.User.UserId && a.CanApprovePvReq && a.BranchCode == Sites.FirstOrDefault(s => s.SiteCode == tr.SrcSiteCode).SiteCityCode) || isAdmin,

                       IsMappedResources = tr.IsApproved,
                       IsAdmin=isAdmin,
                       IsMerged=tr.IsApproved,
                       FileUrl = tr.FileUrl,
                       IsFileUploadRequired = false

                   })
                   ).Concat(_context.PvAddResorceHeads.Where(e => e.ProjectCode == request.ProjectCode && e.SiteCode == request.SiteCode)
                   .Select(ar => new OpPvAllRequestsPaginationDto
                   {
                       Id=ar.Id,
                       CustomerCode = ar.CustomerCode,
                       SiteCode = ar.SiteCode,
                       SiteName = Sites.FirstOrDefault(s => s.SiteCode == ar.SiteCode).SiteName,
                       SiteNameAr = Sites.FirstOrDefault(s => s.SiteCode == ar.SiteCode).SiteArbName,
                       ProjectCode = ar.ProjectCode,
                       ProjectName = Projects.FirstOrDefault(p => p.ProjectCode == ar.ProjectCode).ProjectNameEng,
                       ProjectNameAr = Projects.FirstOrDefault(p => p.ProjectCode == ar.ProjectCode).ProjectNameArb,
                       EffectiveDate = _context.PvAddResorces.OrderBy(e=>e.FromDate).FirstOrDefault(e=>e.AddResReqHeadId==ar.Id).FromDate??new DateTime(),
                       Reamrks = "",
                       RequestedBy = Users.FirstOrDefault(u => u.Id == ar.CreatedBy).UserName,
                       RequestNumber = ar.Id,
                       RequestType = "AddResource",
                       RequestSubType = "",

                       RequestedDate = ar.Created,
                       IsMappedResources = ar.IsEmpMapped,
                       IsApproved = ar.IsApproved,
                       IsMerged=ar.IsMerged,
                       IsAdmin=isAdmin,
                       CanEditReq = ar.CreatedBy == request.User.UserId,
                    //   CanApproveReq = AuthSites.Any(e => e.AppAuth == request.User.UserId && e.SiteCode == ar.SiteCode && e.CanApprovePvReq) || isAdmin,
                       CanApproveReq = oprAuths.Any(a => a.AppAuth == request.User.UserId && a.CanApprovePvReq && a.BranchCode == Sites.FirstOrDefault(s => s.SiteCode == ar.SiteCode).SiteCityCode) || isAdmin,

                       FileUrl = ar.FileUrl,
                       IsFileUploadRequired = true



                   })
                   ).Concat(_context.TblOpPvOpenCloseReqs.Where(e => e.ProjectCode == request.ProjectCode && e.SiteCode == request.SiteCode)
                   .Select(ar => new OpPvAllRequestsPaginationDto
                   {
                       Id=ar.Id,
                       CustomerCode = ar.CustomerCode,
                       SiteCode = ar.SiteCode,
                       SiteName = Sites.FirstOrDefault(s => s.SiteCode == ar.SiteCode).SiteName,
                       SiteNameAr = Sites.FirstOrDefault(s => s.SiteCode == ar.SiteCode).SiteArbName,
                       ProjectCode = ar.ProjectCode,
                       ProjectName = Projects.FirstOrDefault(p => p.ProjectCode == ar.ProjectCode).ProjectNameEng,
                       ProjectNameAr = Projects.FirstOrDefault(p => p.ProjectCode == ar.ProjectCode).ProjectNameArb,
                       EffectiveDate = ar.EffectiveDate.Value,
                       Reamrks = "",
                       RequestedBy = Users.FirstOrDefault(u => u.Id == ar.CreatedBy).UserName,
                       RequestNumber = ar.Id,
                       RequestType = "ProjectVariations",
                       RequestSubType=ar.IsCloseReq?"(Project_Closing_Request)"
                       :ar.IsExtendProjReq?"(Project_Extend_Request)"
                       :ar.IsReOpenReq?"(Project_Reopen_Request)"
                       :ar.IsCancelReq?"(Project_Cancel_Request)"
                       :ar.IsRevokeSuspReq? "(Revoke_Project_Suspension_Request)"
                       :ar.IsSuspendReq? "(Project_Suspension_Request)"
                       : "",
                       RequestedDate = ar.Created,

                       IsApproved = ar.IsApproved,
                       CanEditReq = ar.CreatedBy == request.User.UserId,
                       //CanApproveReq = AuthSites.Any(e => e.AppAuth == request.User.UserId && e.SiteCode == ar.SiteCode && e.CanApprovePvReq) || isAdmin,
                       CanApproveReq = oprAuths.Any(a => a.AppAuth == request.User.UserId && a.CanApprovePvReq && a.BranchCode == Sites.FirstOrDefault(s => s.SiteCode == ar.SiteCode).SiteCityCode) || isAdmin,


                       IsMappedResources = ar.IsApproved,
                       IsAdmin=isAdmin,
                       IsMerged=ar.IsApproved,
                       FileUrl = ar.FileUrl,
                       IsFileUploadRequired = false

                   })
                   ).Concat(_context.TblOpPvTransferWithReplacementReqs.Where(e => e.SrcProjectCode == request.ProjectCode && e.SrcSiteCode == request.SiteCode)
                   .Select(twr => new OpPvAllRequestsPaginationDto
                   {
                       Id=twr.Id,
                       CustomerCode = twr.SrcCustomerCode,
                       SiteCode = twr.SrcSiteCode,
                       SiteName = Sites.FirstOrDefault(s => s.SiteCode == twr.SrcSiteCode).SiteName,
                       SiteNameAr = Sites.FirstOrDefault(s => s.SiteCode == twr.SrcSiteCode).SiteArbName,
                       ProjectCode = twr.SrcProjectCode,
                       ProjectName = Projects.FirstOrDefault(p => p.ProjectCode == twr.SrcProjectCode).ProjectNameEng,
                       ProjectNameAr = Projects.FirstOrDefault(p => p.ProjectCode == twr.SrcProjectCode).ProjectNameArb,
                       EffectiveDate = twr.FromDate.Value,
                       Reamrks = "",
                       RequestedBy = Users.FirstOrDefault(u => u.Id == twr.CreatedBy).UserName,
                       RequestNumber = twr.Id,
                       RequestType = "TransferWithReplacement",
                       RequestSubType = "",

                       RequestedDate = twr.Created,

                       IsApproved = twr.IsApproved,
                       IsMerged=twr.IsApproved,
                       IsAdmin=isAdmin,
                       CanEditReq = twr.CreatedBy == request.User.UserId,
                       //CanApproveReq = AuthSites.Any(e => e.AppAuth == request.User.UserId && e.SiteCode == twr.SrcSiteCode && e.CanApprovePvReq) || isAdmin,
                       CanApproveReq = oprAuths.Any(a => a.AppAuth == request.User.UserId && a.CanApprovePvReq && a.BranchCode == Sites.FirstOrDefault(s => s.SiteCode == twr.SrcSiteCode).SiteCityCode) || isAdmin,


                       IsMappedResources = twr.IsApproved,
                       FileUrl = twr.FileUrl,
                       IsFileUploadRequired = false

                   })
                   ).Concat(_context.TblOpPvSwapEmployeesReqs.Where(e => e.SrcProjectCode == request.ProjectCode && e.SrcSiteCode == request.SiteCode)
                   .Select(se => new OpPvAllRequestsPaginationDto
                   {
                       Id=se.Id,
                       CustomerCode = se.SrcCustomerCode,
                       SiteCode = se.SrcSiteCode,
                       SiteName = Sites.FirstOrDefault(s => s.SiteCode == se.SrcSiteCode).SiteName,
                       SiteNameAr = Sites.FirstOrDefault(s => s.SiteCode == se.SrcSiteCode).SiteArbName,
                       ProjectCode = se.SrcProjectCode,
                       ProjectName = Projects.FirstOrDefault(p => p.ProjectCode == se.SrcProjectCode).ProjectNameEng,
                       ProjectNameAr = Projects.FirstOrDefault(p => p.ProjectCode == se.SrcProjectCode).ProjectNameArb,
                       EffectiveDate = se.FromDate.Value,
                       Reamrks = "",
                       RequestedBy = Users.FirstOrDefault(u => u.Id == se.CreatedBy).UserName,
                       RequestNumber = se.Id,
                       RequestType = "SwapEmployees",
                       RequestSubType = "",

                       RequestedDate = se.Created,

                       IsApproved = se.IsApproved,
                       IsAdmin=isAdmin,
                       IsMerged=se.IsApproved,
                       CanEditReq = se.CreatedBy == request.User.UserId,
                       //CanApproveReq = AuthSites.Any(e => e.AppAuth == request.User.UserId && e.SiteCode == se.SrcSiteCode && e.CanApprovePvReq) || isAdmin,
                       CanApproveReq = oprAuths.Any(a => a.AppAuth == request.User.UserId && a.CanApprovePvReq && a.BranchCode == Sites.FirstOrDefault(s => s.SiteCode == se.SrcSiteCode).SiteCityCode) || isAdmin,


                       IsMappedResources = se.IsApproved,
                       FileUrl = se.FileUrl,
                       IsFileUploadRequired = false

                   })
                   ) ;

                if (!string.IsNullOrEmpty(request.Input.Approval))
                {
                    string aprv = request.Input.Approval;

                    list = aprv switch
                    {
                        "approved" => list.Where(e => e.IsApproved),
                        "unapproved" => list.Where(e => !e.IsApproved),
                        _ => list
                    };
                }
                 if (!string.IsNullOrEmpty(request.Input.ListType))
                {
                    string aprv = request.Input.ListType;

                    list = aprv switch
                    {
                        "addResource" => list.Where(e => e.RequestType.Equals("AddResource")),
                        "removeResource" => list.Where(e => e.RequestType.Equals("RemoveResource")),
                        "replaceResource" => list.Where(e => e.RequestType.Equals("ReplaceResource")),
                        "swapEmployees" => list.Where(e => e.RequestType.Equals("SwapEmployees")),
                        "transferWithReplacement" => list.Where(e => e.RequestType.Equals("TransferWithReplacement")),
                        "transferResource" => list.Where(e => e.RequestType.Equals("TransferResource")),
                        "projectVariations" => list.Where(e => e.RequestType.Equals("ProjectVariations")),
                        _ => list
                    };
                }



               

  var nreports = await list.OrderBy(request.Input.OrderBy).PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);
                Log.Info("----Info GetPvAllRequestsPagedListByProjectSite method Exit----");
                return nreports;




               
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetPvAllRequestsPagedListByProjectSite Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return null;
            }
        }
    }
    #endregion
    
    
    #region GetRecentApprovedPvRequestData

    public class GetRecentApprovedPvRequestData : IRequest<OutputGetRecentPvRequest>
    {
        public UserIdentityDto User { get; set; }
        public OprPaginationFilterDto Input { get; set; }
        public InputGetRecentPvRequest Dto { get; set; }
    }

    public class GetRecentApprovedPvRequestDataHandler : IRequestHandler<GetRecentApprovedPvRequestData, OutputGetRecentPvRequest>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetRecentApprovedPvRequestDataHandler(CINDBOneContext context, AutoMapper.IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<OutputGetRecentPvRequest> Handle(GetRecentApprovedPvRequestData request, CancellationToken cancellationToken)
        {
            try
            {

                OutputGetRecentPvRequest res = new() {Message= "No Data Found",ReqId=0,RequestType=0 };
                DateTime date = DateTime.Parse(request.Dto.Date);
                var removeReq = await _context.TblOpPvRemoveResourceReqs.OrderByDescending(e => e.FromDate).FirstOrDefaultAsync(f=>f.IsApproved&&f.ProjectCode==request.Dto.ProjectCode&&f.SiteCode==request.Dto.SiteCode &&f.EmployeeNumber==request.Dto.EmployeeNumber&& f.FromDate<= new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month)) && f.FromDate>=new DateTime(date.Year, date.Month,1));
                var replaceReq = await _context.TblOpPvReplaceResourceReqs.OrderByDescending(e => e.FromDate).FirstOrDefaultAsync(f=>f.IsApproved&&f.ProjectCode==request.Dto.ProjectCode&&f.SiteCode==request.Dto.SiteCode &&(f.ResignedEmployeeNumber == request.Dto.EmployeeNumber|| f.ReplacedEmployeeNumber == request.Dto.EmployeeNumber) && f.FromDate<= new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month)) && f.FromDate>=new DateTime(date.Year, date.Month,1));
                var transferReq = await _context.TblOpPvTransferResourceReqs.OrderByDescending(e => e.FromDate).FirstOrDefaultAsync(f=>f.IsApproved&&(f.SrcProjectCode == request.Dto.ProjectCode|| f.DestProjectCode == request.Dto.ProjectCode) &&(f.SrcSiteCode == request.Dto.SiteCode|| f.DestSiteCode == request.Dto.SiteCode) &&f.EmployeeNumber==request.Dto.EmployeeNumber&& f.FromDate<= new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month)) && f.FromDate>=new DateTime(date.Year, date.Month,1));
                var transferWithReplace = await _context.TblOpPvTransferWithReplacementReqs.OrderByDescending(e => e.FromDate).FirstOrDefaultAsync(f=>f.IsApproved&&(f.SrcProjectCode == request.Dto.ProjectCode|| f.DestProjectCode == request.Dto.ProjectCode) &&(f.SrcSiteCode == request.Dto.SiteCode|| f.DestSiteCode == request.Dto.SiteCode) &&(f.SrcEmployeeNumber == request.Dto.EmployeeNumber|| f.DestEmployeeNumber == request.Dto.EmployeeNumber) && f.FromDate<= new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month)) && f.FromDate>=new DateTime(date.Year, date.Month,1));
                var swapReq = await _context.TblOpPvSwapEmployeesReqs.OrderByDescending(e => e.FromDate).FirstOrDefaultAsync(f=>f.IsApproved&&(f.SrcProjectCode == request.Dto.ProjectCode|| f.DestProjectCode == request.Dto.ProjectCode) &&(f.SrcSiteCode == request.Dto.SiteCode|| f.DestSiteCode == request.Dto.SiteCode) &&(f.SrcEmployeeNumber == request.Dto.EmployeeNumber|| f.DestEmployeeNumber == request.Dto.EmployeeNumber) && f.FromDate<= new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month)) && f.FromDate>=new DateTime(date.Year, date.Month,1));

                var addReq = await (from r in _context.TblOpPvAddResourceEmployeeToResourceMaps.Where(e => e.EmployeeNumber == request.Dto.EmployeeNumber && (e.FromDate <= new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month)) && e.FromDate >= new DateTime(date.Year, date.Month, 1) || e.ToDate <= new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month)) && e.ToDate >= new DateTime(date.Year, date.Month, 1)))
                              join h in _context.PvAddResorceHeads on r.PvAddResReqId equals h.Id
                              select new { req = r, head = h }).Where(s=>s.head.IsApproved&&s.head.IsEmpMapped&&s.head.IsMerged&&s.head.ProjectCode==request.Dto.ProjectCode &&  s.head.IsApproved &&  s.head.ProjectCode == request.Dto.ProjectCode &&  s.head.SiteCode == request.Dto.SiteCode).OrderByDescending(e=>e.req.FromDate).FirstOrDefaultAsync();
                
                 var addReq2 = await (from r in _context.TblOpPvAddResourceEmployeeToResourceMaps.Where(e => e.EmployeeNumber == request.Dto.EmployeeNumber && (e.FromDate <= new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month)) && e.FromDate >= new DateTime(date.Year, date.Month, 1) || e.ToDate <= new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month)) && e.ToDate >= new DateTime(date.Year, date.Month, 1)))
                              join h in _context.PvAddResorceHeads on r.PvAddResReqId equals h.Id
                              select new { req = r, head = h }).Where(s=>s.head.ProjectCode==request.Dto.ProjectCode &&  s.head.IsApproved &&  s.head.ProjectCode == request.Dto.ProjectCode &&  s.head.SiteCode == request.Dto.SiteCode).OrderBy(e=>e.req.ToDate).FirstOrDefaultAsync();


                if (removeReq is not null)
                {
                    return new() { Message = "remove Request",RequestType=EnumPvRequestType.RemoveResource,ReqId= removeReq.Id};
                }
                if (replaceReq is not null)
                {
                    return new() { Message = "replaceReq", RequestType = EnumPvRequestType.ReplaceResource, ReqId = replaceReq.Id };
                }
                if (transferReq is not null)
                {
                    return new() { Message = "transferReq", RequestType = EnumPvRequestType.Transfer, ReqId = transferReq.Id };

                }
                if (transferWithReplace is not null)
                {
                    return new() { Message = "transferWithReplace", RequestType = EnumPvRequestType.TransferWithReplace, ReqId = transferWithReplace.Id };

                }
                if (swapReq is not null)
                {
                    return new() { Message = "swapReq", RequestType = EnumPvRequestType.SwapResources, ReqId = swapReq.Id };
                }
                if (addReq is not null)
                {
                    return new() { Message = "addReq", RequestType = EnumPvRequestType.AddResource, ReqId = addReq.head.Id };

                }


                return res;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetRecentApprovedPvRequestData Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return null;
            }
        }
    }
    #endregion


    #region PvRequestsFileUpload
    public class PvRequestsFileUpload : IRequest<CreateUpdateResultDto>
    {
        public UserIdentityDto User { get; set; }
        public PvRequestsFileUploadDto dto { get; set; }
    }

    public class PvRequestsFileUploadHandler : IRequestHandler<PvRequestsFileUpload, CreateUpdateResultDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public PvRequestsFileUploadHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CreateUpdateResultDto> Handle(PvRequestsFileUpload request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {

                try
                {
                    Log.Info("----Info PvRequestsFileUpload method start----");
                    var obj = request.dto;


                    string FileName =obj.RequestType=="ProjectVariations"?"PV_"+ Regex.Replace(obj.RequestSubType, @"\(.+?\)", "") +"_"+ obj.Id.ToString():obj.RequestType+"_"+obj.Id.ToString();


                 
                        if (!string.IsNullOrEmpty(obj.FileName))
                        {
                            if (obj.FileIForm != null && obj.FileIForm.Length > 0)
                            {
                                var guid = Guid.NewGuid().ToString();
                               guid = $"{guid}_{FileName}{ Path.GetExtension(obj.FileIForm.FileName)}";
                                obj.FileName += guid;
                                var filePath = Path.Combine(obj.WebRoot, guid);
                                using (var stream = new FileStream(filePath, FileMode.Create))
                                {
                                    obj.FileIForm.CopyTo(stream);
                                }


                                
                            if(obj.RequestType== "AddResource")
                            {
                                var Req = await _context.PvAddResorceHeads.AsNoTracking().SingleOrDefaultAsync(e=>e.Id==obj.Id);
                                if (Req is null)
                                {
                                    return new() { IsSuccess = false, ErrorMsg = "Invalid request Number" };
                                }
                                Req.FileUrl = obj.FileName;
                                Req.FileUploadBy = request.User.UserId;

                                _context.PvAddResorceHeads.Update(Req);
                                await _context.SaveChangesAsync();

                               
                            } 
                            else if(obj.RequestType== "RemoveResource")
                            {
                                var Req = await _context.TblOpPvRemoveResourceReqs.AsNoTracking().SingleOrDefaultAsync(e => e.Id == obj.Id);
                                if (Req is null)
                                {
                                    return new() { IsSuccess = false, ErrorMsg = "Invalid request Number" };
                                }
                                Req.FileUrl = obj.FileName;
                                Req.FileUploadBy = request.User.UserId;

                                _context.TblOpPvRemoveResourceReqs.Update(Req);
                                await _context.SaveChangesAsync();
                            }

 else if(obj.RequestType== "ProjectVariations")
                            {
                                var Req = await _context.TblOpPvOpenCloseReqs.AsNoTracking().SingleOrDefaultAsync(e => e.Id == obj.Id);
                                if (Req is null)
                                {
                                    return new() { IsSuccess = false, ErrorMsg = "Invalid request Number" };
                                }
                                Req.FileUrl = obj.FileName;
                                Req.FileUploadBy = request.User.UserId;
                                _context.TblOpPvOpenCloseReqs.Update(Req);
                                await _context.SaveChangesAsync();
                            }
                            else
                            {
                                return new() { IsSuccess = false, ErrorMsg = "Invalid Request Type" };

                            }

                        }

                        }
                    else
                    {

                        return new() { IsSuccess = false, ErrorMsg = "File is Empty" };
                    }

                   await transaction.CommitAsync();
                    return new() { IsSuccess = true };
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in PvRequestsFileUpload Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return new() { IsSuccess = false, ErrorId = 0, ErrorMsg = "Something went wrong" };
                }
            }
        }
    }

    #endregion


}






