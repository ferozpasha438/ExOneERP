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



    #region CreateUpdateOpAuthorities
    public class CreateOpAuthorities : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public TblOpAuthoritiesDto Input { get; set; }
    }

    public class CreateOpAuthoritiesHandler : IRequestHandler<CreateOpAuthorities, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateOpAuthoritiesHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateOpAuthorities request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info CreateUpdateOpAuthorities method start----");



                var obj = request.Input;

                var branchCodes = _context.CompanyBranches.AsNoTracking();

                TblOpAuthorities OpAuthorities = new();
                if (obj.Id > 0)
                    OpAuthorities = await _context.TblOpAuthoritiesList.FirstOrDefaultAsync(e => e.Id == obj.Id);
                else
                {
                    //if (_context.TblOpAuthoritiesList.Any(x => x.AppAuth == obj.AppAuth))
                    //{
                    //    return -1;
                    //}
                    OpAuthorities.AppAuth = obj.AppAuth;
                }
                OpAuthorities.Id = obj.Id;
                OpAuthorities.BranchCode = obj.BranchCode;
               
                OpAuthorities.AppLevel = obj.AppLevel;
         

                OpAuthorities.CanApproveEnquiry = obj.CanApproveEnquiry;
                OpAuthorities.CanAddSurveyorToEnquiry = obj.CanAddSurveyorToEnquiry;
               // OpAuthorities.CanEditSurveyForm = obj.CanEditSurveyForm;
                OpAuthorities.CanApproveSurvey = obj.CanApproveSurvey;
                OpAuthorities.CanApproveEstimation = obj.CanApproveEstimation;
                OpAuthorities.CanApproveProposal = obj.CanApproveProposal;
                OpAuthorities.CanApproveContract = obj.CanApproveContract;
                OpAuthorities.CanModifyEstimation = obj.CanModifyEstimation;
                OpAuthorities.CanConvertEnqToProject = obj.CanConvertEnqToProject;
                OpAuthorities.CanConvertEstimationToProposal = obj.CanConvertEstimationToProposal;
                OpAuthorities.CanConvertProposalToContract = obj.CanConvertProposalToContract;
                OpAuthorities.CanCreateRoaster = obj.CanCreateRoaster;
                OpAuthorities.CanEditRoaster = obj.CanEditRoaster;
                OpAuthorities.IsFinalAuthority = obj.IsFinalAuthority;
               
                OpAuthorities.CanEditEnquiry = obj.CanEditEnquiry;
                OpAuthorities.CanApprovePvReq = obj.CanApprovePvReq;


                OpAuthorities.IsActive = obj.IsActive;

                bool hasAuthority = (obj.CanApproveEnquiry ||
                    obj.CanAddSurveyorToEnquiry ||
                    obj.CanApproveSurvey ||
                    obj.CanApproveEstimation ||
                    obj.CanApproveProposal ||
                    obj.CanApproveContract ||
                    obj.CanModifyEstimation ||
                    obj.CanConvertEnqToProject ||
                    obj.CanConvertEstimationToProposal ||
                    obj.CanConvertProposalToContract ||
                    obj.CanCreateRoaster ||
                    obj.CanEditRoaster ||
                    obj.CanEditEnquiry||
                    obj.CanApprovePvReq);




                if (obj.Id > 0 )
                {

                    if (hasAuthority)
                    {
                        OpAuthorities.ModifiedOn = DateTime.Now;

                        _context.TblOpAuthoritiesList.Update(OpAuthorities);
                    }

                    else
                    {
                        _context.TblOpAuthoritiesList.Remove(OpAuthorities);

                    }
                   
                }

                else
                {
                    if (hasAuthority)
                    {

                        OpAuthorities.CreatedOn = DateTime.Now;
                        await _context.TblOpAuthoritiesList.AddAsync(OpAuthorities);
                    }

                    else
                    {
                        return -2;
                    }
                }
                await _context.SaveChangesAsync();
                Log.Info("----Info CreateUpdateOpAuthorities method Exit----");
                return OpAuthorities.Id;
            }
            catch (Exception ex)
            {
                Log.Error("Error in CreateUpdateOpAuthorities Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }
    }

    #endregion

    #region GetOpAuthoritiesByUserId
    public class GetOpAuthoritiesByUserId : IRequest<TblOpAuthoritiesDto>
    {
        public UserIdentityDto User { get; set; }

    }

    public class GetOpAuthoritiesByUserIdHandler : IRequestHandler<GetOpAuthoritiesByUserId, TblOpAuthoritiesDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetOpAuthoritiesByUserIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblOpAuthoritiesDto> Handle(GetOpAuthoritiesByUserId request, CancellationToken cancellationToken)
        {
            TblOpAuthoritiesDto obj = new();
            var OpAuthorities = await _context.TblOpAuthoritiesList.AsNoTracking().ProjectTo<TblOpAuthoritiesDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.AppAuth == request.User.UserId);
            if (OpAuthorities is null)
            {
                obj.Id = OpAuthorities.Id;

                obj.IsActive = false; ;
                obj.AppLevel = 0;
                obj.CanApproveEnquiry = false;
                obj.CanApproveProposal = false;
                obj.CanApproveSurvey = false;
                obj.CanConvertEnqToProject = false;
                obj.CanCreateRoaster = false;
                obj.CanEditRoaster = false;
                obj.CanModifyEstimation = false;
                obj.IsFinalAuthority = false;
                obj.CanApproveEstimation = false;
                obj.CanApproveContract = false;
                obj.CanConvertEstimationToProposal = false;
                obj.CanConvertProposalToContract = false;
                obj.CanEditEnquiry = false;
                obj.CanAddSurveyorToEnquiry = false;
            }
            else
                obj = OpAuthorities;
            return obj;
        }
    }

    #endregion

    #region GetOpAuthoritiesListByUserId
    public class GetOpAuthoritiesListByUserId : IRequest<List<TblOpAuthoritiesDto>>
    {
        public UserIdentityDto User { get; set; }

    }

    public class GetOpAuthoritiesListByUserIdHandler : IRequestHandler<GetOpAuthoritiesListByUserId, List<TblOpAuthoritiesDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetOpAuthoritiesListByUserIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<TblOpAuthoritiesDto>> Handle(GetOpAuthoritiesListByUserId request, CancellationToken cancellationToken)
        {
            TblOpAuthoritiesDto obj = new();
            var OpAuthorities = await _context.TblOpAuthoritiesList.AsNoTracking().ProjectTo<TblOpAuthoritiesDto>(_mapper.ConfigurationProvider).Where(e => e.AppAuth == request.User.UserId).ToListAsync();
            if (OpAuthorities.Count==0)
            {
                obj.Id = 0;

                obj.IsActive = false; ;
                obj.AppLevel = 0;
                obj.CanApproveEnquiry = false;
                obj.CanApproveProposal = false;
                obj.CanApproveSurvey = false;
                obj.CanConvertEnqToProject = false;
                obj.CanCreateRoaster = false;
                obj.CanEditRoaster = false;
                obj.CanModifyEstimation = false;
                obj.IsFinalAuthority = false;
                obj.CanApproveEstimation = false;
                obj.CanApproveContract = false;
                obj.CanConvertEstimationToProposal = false;
                obj.CanConvertProposalToContract = false;
                obj.CanEditEnquiry = false;
                obj.CanAddSurveyorToEnquiry = false;
               
            }
            else
                OpAuthorities.Add(obj);
            return OpAuthorities;
        }
    }

    #endregion

    #region GetAuthoritiesPagedList

    public class GetAuthoritiesPagedList : IRequest<PaginatedList<TblOpAuthoritiesPagedList>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetAuthoritiesPagedListHandler : IRequestHandler<GetAuthoritiesPagedList, PaginatedList<TblOpAuthoritiesPagedList>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetAuthoritiesPagedListHandler(CINDBOneContext context, AutoMapper.IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<TblOpAuthoritiesPagedList>> Handle(GetAuthoritiesPagedList request, CancellationToken cancellationToken)
        {
            var search = request.Input.Query;
            var userNames = _context.SystemLogins.AsNoTracking(); 
            var list = await _context.TblOpAuthoritiesList.AsNoTracking()
                            
              .OrderBy(request.Input.OrderBy).Select(e=> new TblOpAuthoritiesPagedList { 
               Id=e.Id,
               CanAddSurveyorToEnquiry=e.CanAddSurveyorToEnquiry,
               CanApproveContract=e.CanApproveContract,
               CanApproveEnquiry=e.CanApproveEnquiry,
               CanApproveEstimation=e.CanApproveEstimation,
               CanApproveProposal=e.CanApproveProposal,
               CanApproveSurvey=e.CanApproveSurvey,
               CanConvertEnqToProject=e.CanConvertEnqToProject,
               CanConvertEstimationToProposal=e.CanConvertEstimationToProposal,
               CanConvertProposalToContract=e.CanConvertProposalToContract,
               CanCreateRoaster=e.CanCreateRoaster,
               CanEditEnquiry=e.CanEditEnquiry,
               CanEditRoaster=e.CanEditRoaster,
               CanModifyEstimation=e.CanModifyEstimation,
               CanApprovePvReq=e.CanApprovePvReq,
               CreatedOn=e.CreatedOn,
               BranchCode=e.BranchCode,
               AppAuth=e.AppAuth,
               AppLevel=e.AppLevel,
               IsActive=e.IsActive,
               IsFinalAuthority=e.IsFinalAuthority,
               ModifiedOn=e.ModifiedOn,
               UserName= userNames.FirstOrDefault(u => u.Id == e.AppAuth).UserName,
        }).Where(e => e.UserName.Contains(search) ||
              
              search == "" || search == null
              || e.BranchCode.Contains(search) || e.AppAuth.ToString().Contains(search)
              ||e.UserName.Contains(search)
              )
             
                 .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return list;
        }
    }
    #endregion


    #region GetAuthorityById
    public class GetAuthorityById : IRequest<TblOpAuthoritiesDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetAuthorityByIdHandler : IRequestHandler<GetAuthorityById, TblOpAuthoritiesDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetAuthorityByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblOpAuthoritiesDto> Handle(GetAuthorityById request, CancellationToken cancellationToken)
        {

            var Authority = await _context.TblOpAuthoritiesList.AsNoTracking().ProjectTo<TblOpAuthoritiesDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.Id == request.Id);
            return Authority;
        }
    }
    #region GetAuthorityByBranchUserId
    public class GetAuthorityByBranchUserId : IRequest<TblOpAuthoritiesDto>
    {
        public UserIdentityDto User { get; set; }
        public string BranchCode { get; set; }
        public int UserId { get; set; }
    }

    public class GetAuthorityByBranchUserIdHandler : IRequestHandler<GetAuthorityByBranchUserId, TblOpAuthoritiesDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetAuthorityByBranchUserIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblOpAuthoritiesDto> Handle(GetAuthorityByBranchUserId request, CancellationToken cancellationToken)
        {

            var Authority = await _context.TblOpAuthoritiesList.AsNoTracking().ProjectTo<TblOpAuthoritiesDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.BranchCode == request.BranchCode && e.AppAuth == request.UserId);
            return Authority;
        }
    }

    #endregion
    #endregion
}