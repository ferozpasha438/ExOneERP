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

namespace CIN.Application.OperationsMgtQuery
{
    #region GetServiceEnquiriesPagedList

    public class GetServiceEnquiriesPagedList : IRequest<PaginatedList<TblSndDefServiceEnquiriesDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetServiceEnquiriesPagedListHandler : IRequestHandler<GetServiceEnquiriesPagedList, PaginatedList<TblSndDefServiceEnquiriesDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetServiceEnquiriesPagedListHandler(CINDBOneContext context, AutoMapper.IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<TblSndDefServiceEnquiriesDto>> Handle(GetServiceEnquiriesPagedList request, CancellationToken cancellationToken)
        {
            var search = request.Input.Query;
            var list = await _context.OprEnquiries.AsNoTracking()
              .Where(e => //e.CompanyId == request.CompanyId &&
                            (e.EnquiryNumber.Contains(search) ||
                            e.SiteCode.Contains(search) ||
                            e.ServiceCode.Contains(search) ||
                            e.UnitCode.Contains(search) ||
                            e.StatusEnquiry.Contains(search) ||
                            e.SurveyorCode.Contains(search) ||
                            e.EnquiryID.ToString().Contains(search) ||
                            e.EstimatedPrice.ToString().Contains(search) ||
                            e.UnitCode.Contains(search)||
                            search == "" || search == null
                             ))
               .OrderBy(request.Input.OrderBy)
              .ProjectTo<TblSndDefServiceEnquiriesDto>(_mapper.ConfigurationProvider)
                 .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return list;
        }
    }
    #endregion


    //#region GetServiceEnquiriesByEnquiryumberPagedList

    //public class GetServiceEnquiriesByEnquiryumberPagedList : IRequest<PaginatedList<TblSndDefServiceEnquiriesDto>>
    //{
    //    public UserIdentityDto User { get; set; }
    //    public PaginationFilterDto Input { get; set; }
    
    //}

    //public class GetServiceEnquiriesByEnquiryumberPagedListHandler : IRequestHandler<GetServiceEnquiriesByEnquiryumberPagedList, PaginatedList<TblSndDefServiceEnquiriesDto>>
    //{
    //    private readonly CINDBOneContext _context;
    //    private readonly IMapper _mapper;
    //    public GetServiceEnquiriesByEnquiryumberPagedListHandler(CINDBOneContext context, AutoMapper.IMapper mapper)
    //    {
    //        _context = context;
    //        _mapper = mapper;
    //    }
    //    public async Task<PaginatedList<TblSndDefServiceEnquiriesDto>> Handle(GetServiceEnquiriesByEnquiryumberPagedList request, CancellationToken cancellationToken)
    //    {
    //        var search = request.Input.Query;
    //        var list = await _context.OprEnquiries.AsNoTracking()
    //          .Where(e => e.EnquiryNumber == request.Input.Query)
    //           .OrderBy(request.Input.OrderBy)
    //          .ProjectTo<TblSndDefServiceEnquiriesDto>(_mapper.ConfigurationProvider)
    //             .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

    //        return list;
    //    }
    //}
    //#endregion

    #region GetEnquiriesByEnquiryNumberPagedList                    update done

    public class GetEnquiriesByEnquiryNumberPagedList : IRequest<PaginatedList<ServiceEnquiriesPaginationDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    
    }

    public class GetEnquiriesByEnquiryNumberPagedListHandler : IRequestHandler<GetEnquiriesByEnquiryNumberPagedList, PaginatedList<ServiceEnquiriesPaginationDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetEnquiriesByEnquiryNumberPagedListHandler(CINDBOneContext context, AutoMapper.IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<ServiceEnquiriesPaginationDto>> Handle(GetEnquiriesByEnquiryNumberPagedList request, CancellationToken cancellationToken)
        {
            bool isAdmin = _context.SystemLogins.AsNoTracking().FirstOrDefault(e => e.Id == request.User.UserId).LoginId == "Admin" ? true : false;


            var search = request.Input.Query;
            var oprAuths = _context.TblOpAuthoritiesList.AsNoTracking();
            var oprApprvls = _context.TblOprTrnApprovalsList.AsNoTracking();
            var enquiryHead = _context.OprEnquiryHeaders.AsNoTracking().First(e=>e.EnquiryNumber==search);
            var ServiceCode = enquiryHead.Version is null ? enquiryHead.EnquiryNumber : enquiryHead.EnquiryNumber + "/" + enquiryHead.Version.ToString();
            var surveyors = _context.OprSurveyors.AsNoTracking();
            var services = _context.OprServices.AsNoTracking();
            
            var sites = _context.OprSites.AsNoTracking().Where(s=>s.CustomerCode== enquiryHead.CustomerCode);
           

            var list = await _context.OprEnquiries.AsNoTracking()
                .OrderBy(request.Input.OrderBy).Select(d => new ServiceEnquiriesPaginationDto
               {
                   EnquiryID = d.EnquiryID,
                   EnquiryNumber = d.EnquiryNumber,
                    EstimatedPrice=d.EstimatedPrice,
                     PricePerUnit=d.PricePerUnit,
                      ServiceCode=d.ServiceCode,
                      ServiceNameEn=services.First(s=>s.ServiceCode== d.ServiceCode).ServiceNameEng,
                      ServiceNameAr=services.First(s=>s.ServiceCode== d.ServiceCode).ServiceNameArb,

                       ServiceQuantity=d.ServiceQuantity,
                       UnitQuantity=d.UnitQuantity,
                        SurveyorCode=d.SurveyorCode,
                         SiteCode=d.SiteCode,
                         SiteNameEn=sites.First(e=>e.SiteCode==d.SiteCode).SiteName,
                         SiteNameAr=sites.First(e => e.SiteCode == d.SiteCode).SiteArbName,
                          UnitCode=d.UnitCode,
                           IsAssignedSurveyor=d.IsAssignedSurveyor,
                           IsSurveyCompleted=d.IsSurveyCompleted,
                           IsSurveyInProgress=d.IsSurveyInProgress,
                            IsApproved=d.IsApproved, 
                            IsSurveyApproved= oprApprvls.Any(e => e.ServiceType == "SUR" && e.IsApproved && e.ServiceCode == d.EnquiryID.ToString()),
                            CanEditSurveyForm = surveyors.Any(x=>x.SurveyorCode==d.SurveyorCode && x.UserId== request.User.UserId),
                 //  IsEnqApproved= _context.TblOpAuthoritiesList.Where(e => e.CanApproveEnquiry && e.BranchCode == enquiryHead.BranchCode).Count() <= _context.TblOprTrnApprovalsList.Where(e => e.ServiceType == "ENQ" && (e.BranchCode ==enquiryHead.BranchCode) && e.IsApproved && e.ServiceCode == d.EnquiryNumber).Count(),
                   IsEnqApproved= _context.TblOprTrnApprovalsList.Where(e => e.ServiceType == "ENQ" /*&& (e.BranchCode ==enquiryHead.BranchCode)*/ && e.IsApproved && e.ServiceCode == ServiceCode).Count()>0,
                   HasAuthority = oprAuths.Any(e => e.AppAuth == request.User.UserId && e.BranchCode == enquiryHead.BranchCode) || isAdmin,
                   ApprovedUser = oprApprvls.Any(e => e.AppAuth == request.User.UserId && e.ServiceType == "SUR" && e.IsApproved && e.ServiceCode == d.EnquiryID.ToString()),
                   Authorities = isAdmin? new TblOpAuthorities
                   {
                       CanApproveEnquiry = true,
                       CanApproveSurvey = true,
                       CanConvertEnqToProject = true,
                       CanCreateRoaster = true,
                       CanEditRoaster = true,
                       CanApproveProposal = true,
                       CanModifyEstimation = true,
                       CanApproveContract = true,
                       CanApproveEstimation = true,
                       IsFinalAuthority = true,
                       CanConvertEstimationToProposal = true,
                       CanConvertProposalToContract = true,
                       CanAddSurveyorToEnquiry = true,
                       //CanEditSurveyForm=false,


                   }:
oprAuths.Any(e => e.AppAuth == request.User.UserId && e.BranchCode == enquiryHead.BranchCode)
                   ? oprAuths.FirstOrDefault(e => e.AppAuth == request.User.UserId && e.BranchCode == enquiryHead.BranchCode) : new TblOpAuthorities
                   {
                       CanApproveEnquiry = false,
                       CanApproveSurvey = false,
                       CanConvertEnqToProject = false,
                       CanCreateRoaster = false,
                       CanEditRoaster = false,
                       CanApproveProposal = false,
                       CanModifyEstimation = false,
                       CanApproveContract = false,
                       CanApproveEstimation = false,
                       IsFinalAuthority = false,
                       CanConvertEstimationToProposal = false,
                       CanConvertProposalToContract = false,
                       CanAddSurveyorToEnquiry=false,
                       //CanEditSurveyForm=false,


                   },


               }).Where(e => //e.CompanyId == request.CompanyId &&
                            (e.EnquiryNumber==search))
                 .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return list;

           
        }
    }
    #endregion

#region GetServiceEnquiriesBySurveyorCodePagedList

    public class GetServiceEnquiriesBySurveyorCodePagedList : IRequest<PaginatedList<TblSndDefServiceEnquiriesDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    
    }

    public class GetServiceEnquiriesBySurveyorCodePagedListHandler : IRequestHandler<GetServiceEnquiriesBySurveyorCodePagedList, PaginatedList<TblSndDefServiceEnquiriesDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetServiceEnquiriesBySurveyorCodePagedListHandler(CINDBOneContext context, AutoMapper.IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<TblSndDefServiceEnquiriesDto>> Handle(GetServiceEnquiriesBySurveyorCodePagedList request, CancellationToken cancellationToken)
        {
            var search = request.Input.Query;
            var list = await _context.OprEnquiries.AsNoTracking()
              .Where(e => e.SurveyorCode == request.Input.Query)
               .OrderBy(request.Input.OrderBy)
              .ProjectTo<TblSndDefServiceEnquiriesDto>(_mapper.ConfigurationProvider)
                 .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return list;
        }
    }
    #endregion

    #region changeEnquiryStatus
    public class ChangeEnquiryStatus : IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        //public TblSndDefServiceEnquiriesDto Input { get; set; }
        public int EnquiryID { get; set; }
        public string StatusEnquiry { get; set; }
    }

    public class ChangeEnquiryStatusHandler : IRequestHandler<ChangeEnquiryStatus, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public ChangeEnquiryStatusHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AppCtrollerDto> Handle(ChangeEnquiryStatus request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    TblSndDefServiceEnquiries ServicesEnquiry = new();

                    TblSndDefServiceEnquiryHeader EnqHead = new();

                    ServicesEnquiry = await _context.OprEnquiries.FirstOrDefaultAsync(e => e.EnquiryID == request.EnquiryID);
                    
                    EnqHead = await _context.OprEnquiryHeaders.FirstOrDefaultAsync(e => e.EnquiryNumber == ServicesEnquiry.EnquiryNumber);
                    if (request.StatusEnquiry == "Cancelled" || request.StatusEnquiry == "Open")
                    {

                        ServicesEnquiry.SurveyorCode = "";
                      

                    }
                    
                        ServicesEnquiry.StatusEnquiry = request.StatusEnquiry;
                        _context.OprEnquiries.Update(ServicesEnquiry);
                        await _context.SaveChangesAsync();
                    
                        var TotEnqList = _context.OprEnquiries.AsNoTracking().Where(e =>
                        e.EnquiryNumber == EnqHead.EnquiryNumber);
                    var CancelledEnqList = _context.OprEnquiries.AsNoTracking().Where(e =>
                        e.EnquiryNumber == EnqHead.EnquiryNumber && e.StatusEnquiry == "Cancelled");


                        var ClosedEnqList = _context.OprEnquiries.AsNoTracking().Where(e =>
                        e.EnquiryNumber == EnqHead.EnquiryNumber &&
                        (e.StatusEnquiry == "Cancelled" || e.StatusEnquiry == "Approved")
                        );
                    if (TotEnqList.Count() == CancelledEnqList.Count())
                    {
                        EnqHead.StusEnquiryHead = "Cancelled";
                    }

                    else if (TotEnqList.Count() == ClosedEnqList.Count())
                        {
                            EnqHead.StusEnquiryHead = "Approved";
                        }
                        else
                            EnqHead.StusEnquiryHead = "Survey_In_Progress";

                    


                    _context.OprEnquiryHeaders.Update(EnqHead);

                    await _context.SaveChangesAsync();
                    

                    Log.Info("----Info ChangeEnquiryStatus method Exit----");

                    await transaction.CommitAsync();
                    return ApiMessageInfo.Status(1, ServicesEnquiry.EnquiryID);

                }
                catch (Exception ex)
                {
                    Log.Error("Error in ChangeEnquiryStatus Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return ApiMessageInfo.Status(0);

                }
            }
        }
    }

    #endregion



    #region AddSurveyorToEnquiry
    public class AddSurveyorToEnquiry : IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public int EnquiryID { get; set; }
        public string SurveyorCode { get; set; }
    }

    public class AddSurveyorToEnquiryHandler : IRequestHandler<AddSurveyorToEnquiry, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public AddSurveyorToEnquiryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AppCtrollerDto> Handle(AddSurveyorToEnquiry request, CancellationToken cancellationToken)
        {
            //using (var transaction = await _context.Database.BeginTransactionAsync())
            //{

            //    try
            //    {
            //        TblSndDefServiceEnquiryHeader EnqHead = new();


            //        TblSndDefServiceEnquiries ServicesEnquiry = new();

            //        ServicesEnquiry = await _context.OprEnquiries.FirstOrDefaultAsync(e => e.EnquiryID == request.EnquiryID);

            //        ServicesEnquiry.SurveyorCode = request.SurveyorCode;
            //        ServicesEnquiry.StatusEnquiry = "Assigned_Surveyor";
            //        _context.OprEnquiries.Update(ServicesEnquiry);

            //        EnqHead = await _context.OprEnquiryHeaders.FirstOrDefaultAsync(e => e.EnquiryNumber == ServicesEnquiry.EnquiryNumber);
            //        EnqHead.StusEnquiryHead = "Survey_In_Progress";
            //        _context.OprEnquiryHeaders.Update(EnqHead);
            //        Log.Info("----Info AddSurveyorToEnquiry method Exit----");
            //        await _context.SaveChangesAsync();
            //        await transaction.CommitAsync();
            //        return ApiMessageInfo.Status(1, ServicesEnquiry.EnquiryID);

            //    }
            //    catch (Exception ex)
            //    {
            //        Log.Error("Error in AddSurveyorToEnquiry Method");
            //        Log.Error("Error occured time : " + DateTime.UtcNow);
            //        Log.Error("Error message : " + ex.Message);
            //        Log.Error("Error StackTrace : " + ex.StackTrace);
            //        return ApiMessageInfo.Status(0);

            //    }


            using (var transaction = await _context.Database.BeginTransactionAsync())
            {

                try
                { 
                    TblSndDefServiceEnquiries enquiry = _context.OprEnquiries.FirstOrDefault(e => e.EnquiryID == request.EnquiryID);

                    enquiry.SurveyorCode = request.SurveyorCode;
                    enquiry.IsAssignedSurveyor = true;
                    enquiry.IsSurveyCompleted = false;
                    _context.OprEnquiries.Update(enquiry);
                    await _context.SaveChangesAsync();

                    List<TblSndDefServiceEnquiries> enquiries = _context.OprEnquiries.AsNoTracking().Where(e => enquiry.EnquiryNumber == e.EnquiryNumber && enquiry.SiteCode==e.SiteCode && e.EnquiryID!=enquiry.EnquiryID).ToList();
                    if (enquiries.Count != 0)
                    {
                        enquiries.ForEach(e =>
                        {


                            e.SurveyorCode =e.SurveyorCode==""? request.SurveyorCode:e.SurveyorCode;
                            e.IsAssignedSurveyor = true;
                            e.IsSurveyCompleted = e.IsSurveyCompleted?true:false;
                            e.IsSurveyInProgress= e.IsSurveyInProgress ? true : false;

                        });

                        _context.OprEnquiries.UpdateRange(enquiries);
                        await _context.SaveChangesAsync();
                    }
                        Log.Info("----Info AddSurveyorToEnquiry method Exit----");
                    _context.SaveChanges();
                    transaction.Commit();
                    return ApiMessageInfo.Status(1, enquiry.EnquiryID);

                }
                catch (Exception ex)
                {
                    Log.Error("Error in AddSurveyorToEnquiry Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return ApiMessageInfo.Status(0);

                }
            }







        }
        }
    

        #endregion


        #region GetEnquiryByEnquiryId
        public class GetEnquiryByEnquiryId : IRequest<TblSndDefServiceEnquiries>
        {
            public UserIdentityDto User { get; set; }
            public int EnquiryID { get; set; }
        }

        public class GetEnquiryByEnquiryIdHandler : IRequestHandler<GetEnquiryByEnquiryId, TblSndDefServiceEnquiries>
        {
            private readonly CINDBOneContext _context;
            private readonly IMapper _mapper;
            public GetEnquiryByEnquiryIdHandler(CINDBOneContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<TblSndDefServiceEnquiries> Handle(GetEnquiryByEnquiryId request, CancellationToken cancellationToken)
            {
                TblSndDefServiceEnquiries obj = await _context.OprEnquiries.AsNoTracking().FirstOrDefaultAsync(e => e.EnquiryID == request.EnquiryID);
                return obj;
            }
        }

    #endregion

    #region DeleteServiceEnquiry
    public class DeleteServiceEnquiry : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteServiceEnquiryHandler : IRequestHandler<DeleteServiceEnquiry, int>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteServiceEnquiryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteServiceEnquiry request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info DeleteServiceEnquiry start----");

                if (request.Id > 0)
                {
                    var service = await _context.OprEnquiries.FirstOrDefaultAsync(e => e.EnquiryID == request.Id);
                    _context.Remove(service);

                    await _context.SaveChangesAsync();

                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {

                Log.Error("Error in DeleteServiceEnquiry");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }

        }
    }

    #endregion






    #region GetEnquiriesByEnquiryNumber                    updatedone

    public class GetEnquiriesByEnquiryNumber : IRequest<List<ServiceEnquiriesPaginationDto>>
    {
        public UserIdentityDto User { get; set; }
        public string   EnquiryNumber { get; set; }


    }

    public class GetEnquiriesByEnquiryNumberHandler : IRequestHandler<GetEnquiriesByEnquiryNumber, List<ServiceEnquiriesPaginationDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetEnquiriesByEnquiryNumberHandler(CINDBOneContext context, AutoMapper.IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<ServiceEnquiriesPaginationDto>> Handle(GetEnquiriesByEnquiryNumber request, CancellationToken cancellationToken)
        {
            
            var oprAuths = _context.TblOpAuthoritiesList.AsNoTracking();
            var oprApprvls = _context.TblOprTrnApprovalsList.AsNoTracking();
            var enquiryHeads = _context.OprEnquiryHeaders.AsNoTracking();
            var surveyors = _context.OprSurveyors.AsNoTracking();
            var list = await _context.OprEnquiries.AsNoTracking()
              .Where(e => //e.CompanyId == request.CompanyId &&
                            (e.EnquiryNumber== request.EnquiryNumber))
               .Select(d => new ServiceEnquiriesPaginationDto
               {
                   EnquiryID = d.EnquiryID,
                   EnquiryNumber = d.EnquiryNumber,
                   EstimatedPrice = d.EstimatedPrice,
                   PricePerUnit = d.PricePerUnit,
                   ServiceCode = d.ServiceCode,
                   ServiceQuantity = d.ServiceQuantity,
                   UnitQuantity=d.UnitQuantity,
                   SurveyorCode = d.SurveyorCode,
                   SiteCode = d.SiteCode,
                   UnitCode = d.UnitCode,
                   IsAssignedSurveyor = d.IsAssignedSurveyor,
                   IsSurveyCompleted = d.IsSurveyCompleted,
                   IsSurveyInProgress = d.IsSurveyInProgress,
                   IsApproved = d.IsApproved,
                   CanEditSurveyForm = surveyors.Any(x => x.SurveyorCode == d.SurveyorCode && x.UserId == request.User.UserId),

                   HasAuthority = oprAuths.Any(e => e.AppAuth == request.User.UserId && e.BranchCode == enquiryHeads.FirstOrDefault(e => e.EnquiryNumber == d.EnquiryNumber).BranchCode),
                   ApprovedUser = oprApprvls.Any(e => e.AppAuth == request.User.UserId && e.ServiceType == "SUR" && e.IsApproved && e.ServiceCode == d.EnquiryID.ToString()),
                   Authorities = oprAuths.Any(e => e.AppAuth == request.User.UserId && e.BranchCode == enquiryHeads.FirstOrDefault(e => e.EnquiryNumber == d.EnquiryNumber).BranchCode)
                   ? oprAuths.FirstOrDefault(e => e.AppAuth == request.User.UserId && e.BranchCode == enquiryHeads.FirstOrDefault(e => e.EnquiryNumber == d.EnquiryNumber).BranchCode) : new TblOpAuthorities
                   {
                       CanApproveEnquiry = false,
                       CanApproveSurvey = false,
                       CanConvertEnqToProject = false,
                       CanCreateRoaster = false,
                       CanEditRoaster = false,
                       CanApproveProposal = false,
                       CanModifyEstimation = false,
                       CanApproveContract = false,
                       CanApproveEstimation = false,
                       IsFinalAuthority = false,
                       CanConvertEstimationToProposal = false,
                       CanConvertProposalToContract = false,
                       CanAddSurveyorToEnquiry = false,
                       //CanEditSurveyForm=false,


                   }
               }).ToListAsync();                 ;

            return list;


        }
    }
    #endregion



}

