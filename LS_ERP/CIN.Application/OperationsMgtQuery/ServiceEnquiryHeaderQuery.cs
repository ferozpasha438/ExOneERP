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
    #region GetSevriceEnquiryHeaderPagedList

    public class GetSevriceEnquiryHeaderPagedList : IRequest<PaginatedList<TblSndDefServiceEnquiryHeaderDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetSevriceEnquiryHeaderPagedListHandler : IRequestHandler<GetSevriceEnquiryHeaderPagedList, PaginatedList<TblSndDefServiceEnquiryHeaderDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSevriceEnquiryHeaderPagedListHandler(CINDBOneContext context, AutoMapper.IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<TblSndDefServiceEnquiryHeaderDto>> Handle(GetSevriceEnquiryHeaderPagedList request, CancellationToken cancellationToken)
        {
            var search = request.Input.Query;
            var list = await _context.OprEnquiryHeaders.AsNoTracking()
              .Where(e => //e.CompanyId == request.CompanyId &&
                            (e.EnquiryNumber.Contains(search) || e.CustomerCode.Contains(search) || e.BranchCode.Contains(search)
                             ))
               .OrderBy(request.Input.OrderBy)
              .ProjectTo<TblSndDefServiceEnquiryHeaderDto>(_mapper.ConfigurationProvider)
                 .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return list;
        }
    }
    #endregion
    #region GetEnquiryFormPagedList

    public class GetEnquiryFormPagedList : IRequest<PaginatedList<SurveyFormsPaginationDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetEnquiryFormPagedListHandler : IRequestHandler<GetEnquiryFormPagedList, PaginatedList<SurveyFormsPaginationDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetEnquiryFormPagedListHandler(CINDBOneContext context, AutoMapper.IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<SurveyFormsPaginationDto>> Handle(GetEnquiryFormPagedList request, CancellationToken cancellationToken)
        {


            bool isAdmin = _context.SystemLogins.AsNoTracking().FirstOrDefault(e => e.Id == request.User.UserId).LoginId == "Admin" ? true : false;


            var customers = _context.OprCustomers.AsNoTracking();

            var oprAuths = _context.TblOpAuthoritiesList.AsNoTracking();
            var oprApprvls = _context.TblOprTrnApprovalsList.AsNoTracking();
            var enquiries = _context.OprEnquiries.AsNoTracking();
            var search = request.Input.Query;
            var branches = _context.CompanyBranches.AsNoTracking();
            var list = await _context.OprEnquiryHeaders.AsNoTracking()

               .OrderBy(request.Input.OrderBy).Select(d => new SurveyFormsPaginationDto
               {
                   Id = d.Id,
                   EnquiryNumber = d.EnquiryNumber,
                   CustomerCode = d.CustomerCode,
                   CustomerNameEn = customers.First(c => c.CustCode == d.CustomerCode).CustName,
                   CustomerNameAr = customers.First(c => c.CustCode == d.CustomerCode).CustArbName,
                   DateOfEnquiry = d.DateOfEnquiry,
                   EstimateClosingDate = d.EstimateClosingDate,
                   UserName = d.UserName,
                   TotalEstPrice = d.TotalEstPrice,
                   Remarks = d.Remarks,
                   StusEnquiryHead = d.StusEnquiryHead,
                   BranchCode = d.BranchCode,
                   BranchNameEn = branches.First(b => b.BranchCode == d.BranchCode).BranchName,
                   BranchNameAr = branches.First(b => b.BranchCode == d.BranchCode).BranchName,
                   IsConvertedToProject = d.IsConvertedToProject,
                   IsAssignedSurveyor = enquiries.Where(e => e.EnquiryNumber == d.EnquiryNumber).Count() == enquiries.Where(e => e.EnquiryNumber == d.EnquiryNumber && e.IsAssignedSurveyor).Count(),
                   IsSurveyCompleted = enquiries.Where(e => e.EnquiryNumber == d.EnquiryNumber).Count() == enquiries.Where(e => e.EnquiryNumber == d.EnquiryNumber && e.IsSurveyCompleted).Count(),
                   IsSurveyInProgress = enquiries.Any(e => e.EnquiryNumber == d.EnquiryNumber && e.IsSurveyInProgress),
                   IsActive = d.IsActive,
                   IsAdmin=isAdmin,
                   IsProjectConvertedToContract=_context.TblOpProjectSites.Any(e=>e.ProjectCode==d.EnquiryNumber && e.IsConvrtedToContract && !e.IsAdendum),
Version =d.Version,







                   HasAuthority = oprAuths.Any(e => e.AppAuth == request.User.UserId && e.BranchCode == d.BranchCode) || isAdmin,

                   //IsEnqApproved = _context.TblOpAuthoritiesList.Where(e => e.CanApproveEnquiry && e.BranchCode == d.BranchCode).Count() <= _context.TblOprTrnApprovalsList.Where(e => e.ServiceType == "ENQ" && e.BranchCode == d.BranchCode && e.IsApproved && e.ServiceCode == d.EnquiryNumber).Count(),
                   IsEnqApproved = _context.TblOprTrnApprovalsList.Where(e => e.ServiceType == "ENQ" /*&& e.BranchCode == d.BranchCode*/ && e.IsApproved && e.ServiceCode == d.EnquiryNumber+ (d.Version.HasValue?"/"+d.Version.ToString():"")).Count()>0,
                   ApprovedUser = oprApprvls.Any(e => e.AppAuth == request.User.UserId && e.ServiceType == "ENQ" && e.IsApproved && e.ServiceCode == d.EnquiryNumber + (d.Version.HasValue ? "/" + d.Version.ToString() : "")),
                   //IsApproved = enquiries.Where(e => e.EnquiryNumber == d.EnquiryNumber).Count() == enquiries.Where(e => e.EnquiryNumber == d.EnquiryNumber && e.IsApproved).Count(),
                   IsApproved = enquiries.Where(e => e.EnquiryNumber == d.EnquiryNumber).Count() == enquiries.Where(e => e.EnquiryNumber == d.EnquiryNumber && e.IsApproved).Count(),
                   Authorities = isAdmin ? new TblOpAuthorities
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
                       CanEditEnquiry=true,


                   } : oprAuths.Any(e => e.AppAuth == request.User.UserId && e.BranchCode == d.BranchCode)
                   ? oprAuths.FirstOrDefault(e => e.AppAuth == request.User.UserId && e.BranchCode == d.BranchCode) : new TblOpAuthorities
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
                       CanEditEnquiry = false,

                   }
               }).Where(e => //e.CompanyId == request.CompanyId &&
                            (e.EnquiryNumber.Contains(search) || e.CustomerCode.Contains(search) || e.BranchCode.Contains(search)

                            || e.CustomerNameEn.Contains(search)
                            || e.CustomerNameAr.Contains(search)
                            || search == null
                            || search == ""

                            ) && e.HasAuthority)
                 .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return list;
        }
    }
    #endregion
    //#region GetEnquiryFormPagedList

    //public class GetEnquiryFormPagedList : IRequest<PaginatedList<SurveyFormsPaginationDto>>
    //{
    //    public UserIdentityDto User { get; set; }
    //    public PaginationFilterDto Input { get; set; }
    //}

    //public class GetEnquiryFormPagedListHandler : IRequestHandler<GetEnquiryFormPagedList, PaginatedList<SurveyFormsPaginationDto>>
    //{
    //    private readonly CINDBOneContext _context;
    //    private readonly IMapper _mapper;
    //    public GetEnquiryFormPagedListHandler(CINDBOneContext context, AutoMapper.IMapper mapper)
    //    {
    //        _context = context;
    //        _mapper = mapper;
    //    }
    //    public async Task<PaginatedList<SurveyFormsPaginationDto>> Handle(GetEnquiryFormPagedList request, CancellationToken cancellationToken)
    //    {
    //        var oprAuths = _context.TblOpAuthoritiesList.AsNoTracking();
    //        var oprApprvls = _context.TblOprTrnApprovalsList.AsNoTracking();
    //        var enquiries = _context.OprEnquiries.AsNoTracking();
    //        var search = request.Input.Query;
    //        var list = await _context.OprEnquiryHeaders.AsNoTracking()
    //          .Where(e => //e.CompanyId == request.CompanyId &&
    //                        (e.EnquiryNumber.Contains(search)))
    //           .OrderBy(request.Input.OrderBy).Select(d => new SurveyFormsPaginationDto
    //           {
    //               Id = d.Id,
    //               EnquiryNumber=d.EnquiryNumber,
    //               CustomerCode=d.CustomerCode,
    //               DateOfEnquiry=d.DateOfEnquiry,
    //               EstimateClosingDate=d.EstimateClosingDate,
    //               UserName=d.UserName,
    //               TotalEstPrice=d.TotalEstPrice,
    //               Remarks=d.Remarks,
    //               StusEnquiryHead=d.StusEnquiryHead,
    //               BranchCode = d.BranchCode,
    //               IsConvertedToProject=d.IsConvertedToProject,
    //               IsAssignedSurveyor= enquiries.Where(e => e.EnquiryNumber == d.EnquiryNumber).Count() == enquiries.Where(e => e.EnquiryNumber == d.EnquiryNumber && e.IsAssignedSurveyor).Count(),
    //               IsSurveyCompleted= enquiries.Where(e => e.EnquiryNumber == d.EnquiryNumber).Count() == enquiries.Where(e => e.EnquiryNumber == d.EnquiryNumber && e.IsSurveyCompleted).Count(),
    //               IsSurveyInProgress= enquiries.Any(e => e.EnquiryNumber == d.EnquiryNumber && e.IsSurveyInProgress),

    //               HasAuthority = oprAuths.Any(e => e.AppAuth == request.User.UserId && e.BranchCode == d.BranchCode),
    //               IsApproved = enquiries.Where(e=>e.EnquiryNumber==d.EnquiryNumber).Count()==enquiries.Where(e=>e.EnquiryNumber==d.EnquiryNumber&&e.IsApproved).Count(),
    //               Authorities = oprAuths.Any(e => e.AppAuth == request.User.UserId && e.BranchCode == d.BranchCode)
    //               ? oprAuths.FirstOrDefault(e => e.AppAuth == request.User.UserId && e.BranchCode == d.BranchCode) : new TblOpAuthorities
    //               {
    //                   CanApproveEnquiry = false,
    //                   CanApproveSurvey = false,
    //                   CanConvertEnqToProject = false,
    //                   CanCreateRoaster = false,
    //                   CanEditRoaster = false,
    //                   CanApproveProposal = false,
    //                   CanModifyEstimation = false,
    //                   CanApproveContract = false,
    //                   CanApproveEstimation = false,
    //                   IsFinalAuthority = false,
    //                   CanConvertEstimationToProposal=false,
    //                   CanConvertProposalToContract=false


    //               }
    //           })
    //             .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

    //        return list;
    //    }
    //}
    //#endregion
















    #region GetEnquiryFormHeaderById
    public class GetEnquiryFormHeaderById : IRequest<TblSndDefServiceEnquiryHeaderDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetEnquiryFormHeaderByIdHandler : IRequestHandler<GetEnquiryFormHeaderById, TblSndDefServiceEnquiryHeaderDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetEnquiryFormHeaderByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblSndDefServiceEnquiryHeaderDto> Handle(GetEnquiryFormHeaderById request, CancellationToken cancellationToken)
        {
            TblSndDefServiceEnquiryHeaderDto obj = new();

            var EnqForm = await _context.OprEnquiryHeaders.AsNoTracking().FirstOrDefaultAsync(e => e.Id == request.Id);

            if (EnqForm is not null)

            {
              
                obj.EnquiryNumber = EnqForm.EnquiryNumber;

                obj.CustomerCode = EnqForm.CustomerCode;
                obj.DateOfEnquiry = EnqForm.DateOfEnquiry;
                obj.EstimateClosingDate = EnqForm.EstimateClosingDate;
                obj.UserName = EnqForm.UserName;
                obj.TotalEstPrice = EnqForm.TotalEstPrice;
                obj.Remarks = EnqForm.Remarks;
                obj.StusEnquiryHead = EnqForm.StusEnquiryHead;

                obj.ModifiedOn = EnqForm.ModifiedOn;
                obj.CreatedOn = EnqForm.CreatedOn;
                // obj.DefaultBaseUnit = Service.DefaultBaseUnit;
                //obj.EstimatedServicesPricePerBaseUnit = Service.EstimatedServicesPricePerBaseUnit;
                obj.IsActive = EnqForm.IsActive;
                
                return obj;
            }
            else return null;
        }
    }

    #endregion


    #region GenEnquiryHeadCode
    public class GenEnquiryHeadCode : IRequest<string>
    {
        public UserIdentityDto User { get; set; }
       
    }

    public class GenEnquiryHeadCodeHandler : IRequestHandler<GenEnquiryHeadCode, string>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GenEnquiryHeadCodeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<string> Handle(GenEnquiryHeadCode request, CancellationToken cancellationToken)
        {
            TblSndDefServiceEnquiryHeaderDto obj = new();

            var EnqForm = await _context.OprEnquiryHeaders.AsNoTracking().OrderByDescending(e=>e.Id).FirstOrDefaultAsync();

            if (EnqForm is not null)
            {
                return (EnqForm.Id+1).ToString().PadLeft(10,'0');
            }
            else return "0000000001";
        }
    }

    #endregion










    #region GetEnquiryFormHeaderByEnquiryNumber
    public class GetEnquiryFormHeaderByEnquiryNumber : IRequest<TblSndDefServiceEnquiryHeaderDto>
    {
        public UserIdentityDto User { get; set; }
        public string EnquiryNumber { get; set; }
    }

    public class GetEnquiryFormHeaderByEnquiryNumberHandler : IRequestHandler<GetEnquiryFormHeaderByEnquiryNumber, TblSndDefServiceEnquiryHeaderDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetEnquiryFormHeaderByEnquiryNumberHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblSndDefServiceEnquiryHeaderDto> Handle(GetEnquiryFormHeaderByEnquiryNumber request, CancellationToken cancellationToken)
        {
            TblSndDefServiceEnquiryHeaderDto obj = new();

            var EnqForm = await _context.OprEnquiryHeaders.AsNoTracking().ProjectTo<TblSndDefServiceEnquiryHeaderDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.EnquiryNumber == request.EnquiryNumber);

            
          return EnqForm;
        }
    }

    #endregion



}
