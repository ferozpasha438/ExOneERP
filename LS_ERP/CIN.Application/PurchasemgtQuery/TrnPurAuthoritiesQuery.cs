using AutoMapper;
using CIN.Application.Common;
//using CIN.Application.OperationsMgtDtos;
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
using CIN.Application.PurchaseMgtDtos;
using CIN.Domain.PurchaseMgt;
using CIN.Domain.InventorySetup;
using CIN.Domain.InvoiceSetup;
//using CIN.Domain.OpeartionsMgt;

namespace CIN.Application.PurchasemgtQuery
{

    #region CreateUpdateOpAuthorities
    public class CreateOpAuthorities : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public TrnPurAuthoritiesDto Input { get; set; }
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
                Log.Info("----Info CreateUpdatePurcAuthorities method start----");



                var obj = request.Input;

                var branchCodes = _context.CompanyBranches.AsNoTracking();

                TblPurAuthorities OpAuthorities = new();
                if (obj.Id > 0)
                    OpAuthorities = await _context.PurAuthorities.FirstOrDefaultAsync(e => e.Id == obj.Id);
                else
                {
                    //if (_context.PurAuthorities.Any(x => x.AppAuth == obj.AppAuth))
                    //{
                    //    return -1;
                    //}
                    OpAuthorities.AppAuth = obj.AppAuth;
                }
                OpAuthorities.Id = obj.Id;
                OpAuthorities.BranchCode = obj.BranchCode;

                OpAuthorities.AppLevel = obj.AppLevel;
                OpAuthorities.IsActive = obj.IsActive;


                OpAuthorities.PurchaseRequest = obj.PurchaseRequest;
                OpAuthorities.PurchaseOrder = obj.PurchaseOrder;
                OpAuthorities.PurchaseReturn = obj.PurchaseReturn;
                OpAuthorities.IsActive = obj.IsActive;






                if (obj.Id > 0)
                {

                    OpAuthorities.ModifiedOn = DateTime.Now;

                    _context.PurAuthorities.Update(OpAuthorities);
                }

                else
                {

                    OpAuthorities.CreatedOn = DateTime.Now;
                    await _context.PurAuthorities.AddAsync(OpAuthorities);
                }
                await _context.SaveChangesAsync();
                Log.Info("----Info CreateUpdatePurcAuthorities method Exit----");
                return OpAuthorities.Id;
            }
            catch (Exception ex)
            {
                Log.Error("Error in CreateUpdatePurcAuthorities Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
           
        }
    }

    #endregion

    #region GetOpAuthoritiesByUserId
    public class GetOpAuthoritiesByUserId : IRequest<TrnPurAuthoritiesDto>
    {
        public UserIdentityDto User { get; set; }

    }

    public class GetOpAuthoritiesByUserIdHandler : IRequestHandler<GetOpAuthoritiesByUserId, TrnPurAuthoritiesDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetOpAuthoritiesByUserIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TrnPurAuthoritiesDto> Handle(GetOpAuthoritiesByUserId request, CancellationToken cancellationToken)
        {
            TrnPurAuthoritiesDto obj = new();
            var OpAuthorities = await _context.PurAuthorities.AsNoTracking().ProjectTo<TrnPurAuthoritiesDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.AppAuth == request.User.UserId);
            if (OpAuthorities is null)
            {
                obj.Id = OpAuthorities.Id;

                obj.IsActive = false; ;
                obj.AppLevel = 0;
                obj.PurchaseRequest = false;
                obj.PurchaseOrder = false;
                obj.PurchaseReturn = false;
              
            }
            else
                obj = OpAuthorities;
            return obj;
        }
    }

    #endregion

    #region GetAuthoritiesPagedList

    public class GetAuthoritiesPagedList : IRequest<PaginatedList<TrnPurAuthoritiesDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetAuthoritiesPagedListHandler : IRequestHandler<GetAuthoritiesPagedList, PaginatedList<TrnPurAuthoritiesDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetAuthoritiesPagedListHandler(CINDBOneContext context, AutoMapper.IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<TrnPurAuthoritiesDto>> Handle(GetAuthoritiesPagedList request, CancellationToken cancellationToken)
        {
            var search = request.Input.Query;
            var list = await _context.PurAuthorities.AsNoTracking()
              .Where(e => e.BranchCode.Contains(search))
               .OrderBy(request.Input.OrderBy)
              .ProjectTo<TrnPurAuthoritiesDto>(_mapper.ConfigurationProvider)
                 .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return list;
        }
    }
    #endregion


    #region GetAuthorityById
    public class GetAuthorityById : IRequest<TrnPurAuthoritiesDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetAuthorityByIdHandler : IRequestHandler<GetAuthorityById, TrnPurAuthoritiesDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetAuthorityByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TrnPurAuthoritiesDto> Handle(GetAuthorityById request, CancellationToken cancellationToken)
        {

            var Authority = await _context.PurAuthorities.AsNoTracking().ProjectTo<TrnPurAuthoritiesDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.Id == request.Id);
            return Authority;
        }
    }
    #region GetAuthorityByBranchUserId
    public class GetAuthorityByBranchUserId : IRequest<TrnPurAuthoritiesDto>
    {
        public UserIdentityDto User { get; set; }
        public string BranchCode { get; set; }
        public int UserId { get; set; }
    }

    public class GetAuthorityByBranchUserIdHandler : IRequestHandler<GetAuthorityByBranchUserId, TrnPurAuthoritiesDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetAuthorityByBranchUserIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TrnPurAuthoritiesDto> Handle(GetAuthorityByBranchUserId request, CancellationToken cancellationToken)
        {

            var Authority = await _context.PurAuthorities.AsNoTracking().ProjectTo<TrnPurAuthoritiesDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.BranchCode == request.BranchCode && e.AppAuth == request.UserId);
            return Authority;
        }
    }

    #endregion
    #endregion

    #region GetSelectBranchCodeList

    public class GetSelectBranchCodeList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetSelectBranchCodeListHandler : IRequestHandler<GetSelectBranchCodeList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSelectBranchCodeListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetSelectBranchCodeList request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetSelectBranchCodeList method start----");
            var item = await _context.CompanyBranches.AsNoTracking()
                .OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = e.BranchCode, Value = e.BranchCode })
                  .ToListAsync(cancellationToken);
            Log.Info("----Info GetSelectBranchCodeList method Ends----");
            return item;
        }
    }

    #endregion

    #region GetUserSelectionList
    public class GetUserSelectionList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Userid { get; set; }
    }

    public class GetUserSelectionListHandler : IRequestHandler<GetUserSelectionList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetUserSelectionListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetUserSelectionList request, CancellationToken cancellationToken)
        {

            var Users = await _context.SystemLogins.AsNoTracking().OrderByDescending(e => e.Id)
                .Select(e => new CustomSelectListItem { Text = e.UserName, Value = e.Id.ToString(), TextTwo = e.PrimaryBranch })
                   .ToListAsync(cancellationToken);
            return Users;

        }
    }

    #endregion

    #region CreateUpdatePurApprovals
    public class CreatePurApprovals : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public TblPurTrnApprovalsDto Input { get; set; }
    }

    public class CreateOpApprovalsHandler : IRequestHandler<CreatePurApprovals, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateOpApprovalsHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreatePurApprovals request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateUpdatePurApprovals method start----");



                    var obj = request.Input;

                    var branchCodes = _context.CompanyBranches.AsNoTracking();

                    TblPurTrnApprovals PurApprovals = new();

                    PurApprovals.IsApproved = obj.IsApproved;
                    PurApprovals.ServiceCode = obj.ServiceCode;
                    PurApprovals.ServiceType = obj.ServiceType;
                    PurApprovals.AppRemarks = obj.AppRemarks;
                    PurApprovals.AppAuth = request.User.UserId;
                    PurApprovals.BranchCode = obj.BranchCode;
                    PurApprovals.CreatedOn = DateTime.UtcNow;
                    await _context.TblPurTrnApprovalsList.AddAsync(PurApprovals);
                    await _context.SaveChangesAsync();
                   

                    if (request.Input.ServiceType == "PO")
                    {
                        TblErpInvItemInventoryHistory cObj = new();
                        cObj.IsActive = true;
                        cObj.ModifiedOn = DateTime.Now;
                        _context.InvItemInventoryHistory.Update(cObj).Property(x => x.TranNumber).IsModified = false;
                        await _context.SaveChangesAsync();
                    }
                    /*Only For Purchase Return Approval */
                    if (request.Input.ServiceType == "PTR")
                    {
                        TblErpInvItemInventoryHistory cObj = new();
                        cObj.IsActive = true;
                        cObj.ModifiedOn = DateTime.Now;
                        _context.InvItemInventoryHistory.Update(cObj).Property(x => x.TranNumber).IsModified = false;
                        await _context.SaveChangesAsync();
                    }




                    await transaction.CommitAsync();
                    Log.Info("----Info CreateUpdatePurApprovals method Exit----");
                    return PurApprovals.Id;
                }
                catch (Exception ex)
                {
                    Log.Error("Error in CreateUpdatePurApprovals Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return 0;
                }
            }
        }
    }

    #endregion
}
