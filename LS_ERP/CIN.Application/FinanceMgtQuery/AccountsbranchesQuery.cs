using AutoMapper;
using AutoMapper.QueryableExtensions;
using CIN.Application.Common;
using CIN.Application.FinanceMgtDtos;
using CIN.DB;
using CIN.Domain.FinanceMgt;
using CIN.Domain.SystemSetup;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;

namespace CIN.Application.FinanceMgtQuery
{

    #region GetPagedList

    public class GetAccountBranchList : IRequest<PaginatedList<TblFinDefAccountBranchesDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetAccountBranchListHandler : IRequestHandler<GetAccountBranchList, PaginatedList<TblFinDefAccountBranchesDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetAccountBranchListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<TblFinDefAccountBranchesDto>> Handle(GetAccountBranchList request, CancellationToken cancellationToken)
        {
            var search = request.Input.Query;
            var list = await _context.FinAccountBranches.AsNoTracking()
              .Where(e => //e.CompanyId == request.CompanyId &&
                            (e.FinBranchCode.Contains(search) || e.FinBranchName.Contains(search) ||
                                e.FinBranchAddress.Contains(search)
                             ))
               .OrderBy(request.Input.OrderBy)
              .ProjectTo<TblFinDefAccountBranchesDto>(_mapper.ConfigurationProvider)
                 .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return list;
        }
    }

    #endregion


    #region GetSelectSysBranchListByComId

    public class GetSelectSysAccountBranchList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public int Input { get; set; }
    }

    public class GetSelectSysAccountBranchListHandler : IRequestHandler<GetSelectSysAccountBranchList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSelectSysAccountBranchListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetSelectSysAccountBranchList request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetSelectSysAccountBranchList method start----");
            var item = await _context.FinAccountBranches.AsNoTracking()
                .Where(e => e.FinBranchIsActive)
                .OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = e.FinBranchName, Value = e.FinBranchCode })
                  .ToListAsync(cancellationToken);
            Log.Info("----Info GetSelectSysAccountBranchList method Ends----");
            return item;
        }
    }

    #endregion


    #region SingleItem

    public class GetAccountBranche : IRequest<TblFinDefAccountAuthorityBranchesDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetAccountBranchesHandler : IRequestHandler<GetAccountBranche, TblFinDefAccountAuthorityBranchesDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetAccountBranchesHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblFinDefAccountAuthorityBranchesDto> Handle(GetAccountBranche request, CancellationToken cancellationToken)
        {
            TblFinDefAccountAuthorityBranchesDto obj = new();
            var finAcBr = await _context.FinAccountBranches.AsNoTracking().FirstOrDefaultAsync(e => e.Id == request.Id);
            if (finAcBr is not null)
            {
                var finAcBrsCode = finAcBr.FinBranchCode;

                var finAcBrnAuths = await _context.FinBranchesAuthorities.AsNoTracking()
                    .Where(e => finAcBrsCode == e.FinBranchCode)
                    .Select(e => new TblFinDefBranchesAuthorityDto
                    {
                        AppAuth = e.AppAuth,
                        AppLevel = e.AppLevel,
                        AppAuthJV = e.AppAuthJV,
                        AppAuthBV = e.AppAuthBV,
                        AppAuthCV = e.AppAuthCV,
                        AppAuthAP = e.AppAuthAP,
                        AppAuthAR = e.AppAuthAR,
                        AppAuthPurcRequest = e.AppAuthPurcRequest,
                        AppAuthPurcReturn = e.AppAuthPurcReturn,
                        AppAuthPurcOrder = e.AppAuthPurcOrder,
                        AppAuthAdj = e.AppAuthAdj,
                        AppAuthRect = e.AppAuthRect,
                        AppAuthIssue = e.AppAuthIssue,
                        AppAuthTrans = e.AppAuthTrans
                    }).ToListAsync();

                obj.FinBranchCode = finAcBr.FinBranchCode;
                obj.FinBranchName = finAcBr.FinBranchName;
                obj.FinBranchPrefix = finAcBr.FinBranchPrefix;
                obj.FinBranchAddress = finAcBr.FinBranchAddress;
                obj.FinBranchDesc = finAcBr.FinBranchDesc;
                obj.FinBranchType = finAcBr.FinBranchType;
                obj.FinBranchIsActive = finAcBr.FinBranchIsActive;
                obj.AuthList = finAcBrnAuths;
            }
            return obj;
        }
    }

    #endregion

    #region GetFinDistribution

    public class GetFinDistribution : IRequest<TblFinDistributionItemDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetFinDistributionsHandler : IRequestHandler<GetFinDistribution, TblFinDistributionItemDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetFinDistributionsHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblFinDistributionItemDto> Handle(GetFinDistribution request, CancellationToken cancellationToken)
        {

            var finAcBr = await _context.FinAccountBranches.AsNoTracking().FirstOrDefaultAsync(e => e.Id == request.Id);
            var finDisb = await _context.FinDistributions.AsNoTracking().FirstOrDefaultAsync(e => e.Id == request.Id);
            var mainAccounts = _context.FinMainAccounts.Where(e => e.Status == true).AsNoTracking();

            return new TblFinDistributionItemDto
            {
                FinBranchCode = finAcBr.FinBranchCode,
                FinBranchName = finAcBr.FinBranchName,

                List1 = await mainAccounts
                             .Select(e => new CustomSelectListItem { Text = e.FinAcName, Value = e.FinAcCode })
                             .ToListAsync(cancellationToken),

                List2 = await mainAccounts
                             .Select(e => new CustomSelectListItem { Text = e.FinAcName, Value = e.FinAcCode })
                             .ToListAsync(cancellationToken),
                List3 = await mainAccounts
                             .Select(e => new CustomSelectListItem { Text = e.FinAcName, Value = e.FinAcCode })
                             .ToListAsync(cancellationToken),
                List4 = await mainAccounts
                             .Select(e => new CustomSelectListItem { Text = e.FinAcName, Value = e.FinAcCode })
                             .ToListAsync(cancellationToken),

                List5 = await mainAccounts
                             .Select(e => new CustomSelectListItem { Text = e.FinAcName, Value = e.FinAcCode })
                             .ToListAsync(cancellationToken),
                List6 = await mainAccounts
                             .Select(e => new CustomSelectListItem { Text = e.FinAcName, Value = e.FinAcCode })
                             .ToListAsync(cancellationToken),
                List7 = await mainAccounts
                             .Select(e => new CustomSelectListItem { Text = e.FinAcName, Value = e.FinAcCode })
                             .ToListAsync(cancellationToken),
                List8 = await mainAccounts
                             .Select(e => new CustomSelectListItem { Text = e.FinAcName, Value = e.FinAcCode })
                             .ToListAsync(cancellationToken),
                List9 = await mainAccounts
                             .Select(e => new CustomSelectListItem { Text = e.FinAcName, Value = e.FinAcCode })
                             .ToListAsync(cancellationToken),
                List10 = await mainAccounts
                             .Select(e => new CustomSelectListItem { Text = e.FinAcName, Value = e.FinAcCode })
                             .ToListAsync(cancellationToken),
                List11 = await mainAccounts
                             .Select(e => new CustomSelectListItem { Text = e.FinAcName, Value = e.FinAcCode })
                             .ToListAsync(cancellationToken),


            };

        }
    }

    #endregion


    #region CreateAccountBranch

    public class CreateAccountBranch : IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblFinDefAccountAuthorityBranchesDto Input { get; set; }
    }

    public class CreateAccountBranchQueryHandler : IRequestHandler<CreateAccountBranch, AppCtrollerDto>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateAccountBranchQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AppCtrollerDto> Handle(CreateAccountBranch request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateAccountBranchQuery method start----");

                    var obj = request.Input;
                    if (!await _context.CompanyBranches.AnyAsync(e => e.BranchCode == obj.FinBranchCode))
                        return ApiMessageInfo.Status(0);

                    TblFinDefAccountBranches cObj = new();
                    if (obj.Id > 0)
                        cObj = await _context.FinAccountBranches.FirstOrDefaultAsync(e => e.Id == obj.Id);
                    else
                        cObj = await _context.FinAccountBranches.FirstOrDefaultAsync(e => e.FinBranchCode == obj.FinBranchCode);

                    cObj.FinBranchPrefix = obj.FinBranchPrefix;
                    cObj.FinBranchDesc = obj.FinBranchDesc;
                    cObj.FinBranchType = obj.FinBranchType;
                    cObj.FinBranchIsActive = obj.FinBranchIsActive;



                    //cObj.FinBranchCode = obj.FinBranchCode.ToUpper();
                    //cObj.FinBranchName = obj.FinBranchName;
                    //cObj.FinBranchAddress = obj.FinBranchAddress;


                    if (obj.Id > 0)
                    {
                        cObj.ModifiedOn = DateTime.Now;
                        _context.FinAccountBranches.Update(cObj);
                    }
                    else
                    {
                        cObj.CreatedOn = DateTime.Now;
                        _context.FinAccountBranches.Update(cObj);
                    }
                    await _context.SaveChangesAsync();



                    if (request.Input.AuthList.Count() > 0)
                    {
                        var oldAuthList = await _context.FinBranchesAuthorities.Where(e => e.FinBranchCode == obj.FinBranchCode).ToListAsync();
                        _context.FinBranchesAuthorities.RemoveRange(oldAuthList);

                        List<TblFinDefBranchesAuthority> authList = new();
                        foreach (var auth in request.Input.AuthList)
                        {
                            TblFinDefBranchesAuthority authItem = new()
                            {
                                FinBranchCode = obj.FinBranchCode.ToUpper(),
                                AppAuth = auth.AppAuth,
                                AppLevel = auth.AppLevel,
                                AppAuthJV = auth.AppAuthJV,
                                AppAuthBV = auth.AppAuthBV,
                                AppAuthCV = auth.AppAuthCV,
                                AppAuthAP = auth.AppAuthAP,
                                AppAuthAR = auth.AppAuthAR,
                                AppAuthBR = auth.AppAuthBR,
                                AppAuthFA = auth.AppAuthFA,
                                AppAuthPurcOrder = auth.AppAuthPurcOrder,
                                AppAuthPurcRequest = auth.AppAuthPurcRequest,
                                AppAuthPurcReturn = auth.AppAuthPurcReturn,
                                AppAuthAdj = auth.AppAuthAdj,
                                AppAuthIssue = auth.AppAuthIssue,
                                AppAuthRect = auth.AppAuthRect,
                                AppAuthTrans = auth.AppAuthTrans,
                                IsFinalAuthority = auth.IsFinalAuthority,
                                CreatedOn = DateTime.UtcNow
                            };
                            authList.Add(authItem);
                        }
                        await _context.FinBranchesAuthorities.AddRangeAsync(authList);
                        await _context.SaveChangesAsync();
                    }

                    Log.Info("----Info CreateAccountBranchQuery method Exit----");
                    await transaction.CommitAsync();

                    return ApiMessageInfo.Status(1, 1);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateAccountBranchQuery Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return ApiMessageInfo.Status(0);
                }
            }
        }
    }
    #endregion

    #region Delete
    public class DeleteAccountBranches : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteAccountBranchesQueryHandler : IRequestHandler<DeleteAccountBranches, int>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteAccountBranchesQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteAccountBranches request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info delte method start----");

                    if (request.Id > 0)
                    {
                        var Branch = await _context.FinAccountBranches.FirstOrDefaultAsync(e => e.Id == request.Id);
                        _context.Remove(Branch);
                        var oldAuthList = await _context.FinBranchesAuthorities.Where(e => e.FinBranchCode == Branch.FinBranchCode).ToListAsync();
                        _context.FinBranchesAuthorities.RemoveRange(oldAuthList);
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return request.Id;
                    }
                    return 0;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in delete Method");
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
