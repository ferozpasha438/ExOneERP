using AutoMapper;
using CIN.Application.Common;
using CIN.Application.FinanceMgtDtos;
using CIN.DB;
using CIN.Domain.FinanceMgt;
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

    //#region GetSelectAccountCodeListBybranchCode

    //public class GetSelectAccountCodeListBybranchCode : IRequest<List<CustomSelectListItem>>
    //{
    //    public UserIdentityDto User { get; set; }
    //    public string Input { get; set; }
    //}

    //public class GetSelectAccountCodeListBybranchCodeHandler : IRequestHandler<GetSelectAccountCodeListBybranchCode, List<CustomSelectListItem>>
    //{
    //    private readonly CINDBOneContext _context;
    //    private readonly IMapper _mapper;
    //    public GetSelectAccountCodeListBybranchCodeHandler(CINDBOneContext context, IMapper mapper)
    //    {
    //        _context = context;
    //        _mapper = mapper;
    //    }
    //    public async Task<List<CustomSelectListItem>> Handle(GetSelectAccountCodeListBybranchCode request, CancellationToken cancellationToken)
    //    {
    //        Log.Info("----Info GetSelectAccountCodeListBybranchCode method start----");
    //        var item = await _context.FinBranchesMainAccounts.AsNoTracking()
    //            .Where(e => e.FinBranchCode == request.Input)
    //            .OrderByDescending(e => e.Id)
    //           .Select(e => new CustomSelectListItem { Text = e.FinAcCode, Value = e.FinAcCode })
    //              .ToListAsync(cancellationToken);
    //        Log.Info("----Info GetSelectAccountCodeListBybranchCode method Ends----");
    //        return item;
    //    }
    //}

    //#endregion


    #region GetSelectAccountMappingList

    public class GetSelectAccountMappingList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetSelectAccountMappingListHandler : IRequestHandler<GetSelectAccountMappingList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSelectAccountMappingListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetSelectAccountMappingList request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetSelectAccountMappingList method start----");
            var item = await _context.FinBranchesMainAccounts
                .Include(e=>e.FinMainAccounts).AsNoTracking()
                  .Where(e => e.FinBranchCode == request.Input && e.FinMainAccounts.Status == true)
                .OrderByDescending(e => e.Id)
                   .Select(e => new CustomSelectListItem { Text = "(" + e.FinAcCode + ") " + e.FinMainAccounts.FinAcName , Value = e.FinAcCode })
                  .ToListAsync(cancellationToken);
            Log.Info("----Info GetSelectAccountMappingList method Ends----");
            return item;
        }
    }

    #endregion

    #region GetBranchAccountMappingList

    public class GetBranchAccountMappingList : IRequest<List<TblFinDefBranchesMainAccountsDto>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetBranchAccountMappingListHandler : IRequestHandler<GetBranchAccountMappingList, List<TblFinDefBranchesMainAccountsDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetBranchAccountMappingListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<TblFinDefBranchesMainAccountsDto>> Handle(GetBranchAccountMappingList request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetBranchAccountMappingList method start----");
            var item = await _context.FinMainAccounts.AsNoTracking()
                .Where(e => e.Status == true)
                .OrderByDescending(e => e.Id)
               .Select(e => new TblFinDefBranchesMainAccountsDto { Id = e.Id, FinAcCode = e.FinAcCode, FinAcDesc = e.FinAcDesc })
                  .ToListAsync(cancellationToken);
            Log.Info("----Info GetBranchAccountMappingList method Ends----");
            return item;
        }
    }

    #endregion

    #region CreateAcBranchMapping

    public class CreateAcBranchMapping : IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public CreateBranchesMainAccountsDto Input { get; set; }
    }

    public class CreateAcBranchMappingQueryHandler : IRequestHandler<CreateAcBranchMapping, AppCtrollerDto>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateAcBranchMappingQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AppCtrollerDto> Handle(CreateAcBranchMapping request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateAcBranchMapping method start----");

                    var obj = request.Input;
                    string branchCode = request.Input.FinBranchCode;
                    List<TblFinDefBranchesMainAccounts> branchesMainAccounts = new();
                    var BMainAccocunts = _context.FinBranchesMainAccounts.Where(e => e.FinBranchCode == branchCode);
                    _context.FinBranchesMainAccounts.RemoveRange(BMainAccocunts);
                    await _context.SaveChangesAsync();

                    foreach (var acCode in request.Input.AcCodeList)
                    {
                        if (!(await _context.FinBranchesMainAccounts.AnyAsync(e => e.FinBranchCode == branchCode && e.FinAcCode == acCode)))
                        {
                            TblFinDefBranchesMainAccounts BMapping = new()
                            {
                                FinBranchCode = branchCode,
                                FinAcCode = acCode,
                                CreatedOn = DateTime.Now
                            };
                            branchesMainAccounts.Add(BMapping);
                        }
                    }
                    if (branchesMainAccounts.Count() > 0)
                    {
                        await _context.FinBranchesMainAccounts.AddRangeAsync(branchesMainAccounts);
                        await _context.SaveChangesAsync();
                    }
                    await transaction.CommitAsync();
                    Log.Info("----Info CreateAcBranchMapping method Exit----");
                    return ApiMessageInfo.Status(1, 1);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateAcBranchMapping Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return ApiMessageInfo.Status(0);
                }
            }
        }
    }
    #endregion
}
