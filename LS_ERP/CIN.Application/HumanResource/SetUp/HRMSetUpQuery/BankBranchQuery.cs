using AutoMapper;
using AutoMapper.QueryableExtensions;
using CIN.Application.Common;
using CIN.Application.HumanResource.SetUp.HRMSetUpDtos;
using CIN.DB;
using CIN.Domain.HumanResource.Setup;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CIN.Application.HumanResource.SetUp.HRMSetUpQuery
{
    #region GetPagedList

    public class GetBankBranchList : IRequest<PaginatedList<TblHRMSysBankBranchDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetBankBranchListHandler : IRequestHandler<GetBankBranchList, PaginatedList<TblHRMSysBankBranchDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetBankBranchListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<TblHRMSysBankBranchDto>> Handle(GetBankBranchList request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info GetBankBranchList method start----");
                var search = request.Input.Query;
                var list = await (from branch in _context.BankBranches
                                  join bank in _context.Banks on branch.BankCode equals bank.BankCode
                                  select new TblHRMSysBankBranchDto
                                  {
                                      IFSCCode = branch.IFSCCode,
                                      BranchNameEn = branch.BranchNameEn,
                                      BranchNameAr = branch.BranchNameAr,
                                      BankNameEn = bank.BankNameEn,
                                      BankNameAr = bank.BankNameAr,
                                      BankCode = branch.BankCode,
                                      Id = branch.Id,
                                      IsActive = branch.IsActive
                                  })
                                  .AsNoTracking()
                                  //       .ProjectTo<TblHRMSysBankBranchDto>(_mapper.ConfigurationProvider)
                                  .Where(e => (e.IFSCCode.Contains(search) || e.BranchNameEn.Contains(search)))
                                  .OrderBy(x => x.BankNameEn)
                                  .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

                Log.Info("----Info GetBankBranchList method end----");
                return list;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetBankBranchList Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region GetBankBranchById

    public class GetBankBranchById : IRequest<TblHRMSysBankBranchDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetBankBranchByIdHandler : IRequestHandler<GetBankBranchById, TblHRMSysBankBranchDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetBankBranchByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TblHRMSysBankBranchDto> Handle(GetBankBranchById request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetBankBranchById method start----");
            try
            {
                var branch = await _context.BankBranches.AsNoTracking()
                    .ProjectTo<TblHRMSysBankBranchDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(e => e.Id == request.Id);
                Log.Info("----Info GetBankBranchById method end----");

                if (branch is not null)
                    return branch;
                else
                    return null;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetBankBranchById Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region CreateUpdateBankBranch

    public class CreateUpdateBankBranch : UserIdentityDto, IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblHRMSysBankBranchDto Input { get; set; }
    }
    public class CreateUpdateBankBranchHandler : IRequestHandler<CreateUpdateBankBranch, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateBankBranchHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<AppCtrollerDto> Handle(CreateUpdateBankBranch request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateUpdateBankBranch method start----");
                    var obj = request.Input;
                    TblHRMSysBankBranch branch = new();

                    //Check if a branch with the IFSC Code already exists against the bank in the database.
                    branch = await _context.BankBranches.FirstOrDefaultAsync(e => (e.IFSCCode == request.Input.IFSCCode));

                    if (branch is not null)
                    {
                        branch.BranchNameEn = obj.BranchNameEn;
                        branch.BranchNameAr = obj.BranchNameAr;
                        branch.IFSCCode = obj.IFSCCode;
                        branch.BankCode = obj.BankCode;
                        branch.Id = obj.Id;
                        branch.IsActive = obj.IsActive;
                        branch.ModifiedBy = request.User.UserId;
                        branch.Modified = DateTime.Now;

                        _context.BankBranches.Update(branch);
                    }
                    else
                    {
                        branch = new()
                        {
                            BranchNameEn = obj.BranchNameEn,
                            BranchNameAr = obj.BranchNameAr,
                            IFSCCode = obj.IFSCCode,
                            BankCode = obj.BankCode,
                            IsActive = obj.IsActive,
                            CreatedBy = request.User.UserId,
                            Created = DateTime.Now,
                        };
                        await _context.BankBranches.AddAsync(branch);
                    }
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    Log.Info("----Info CreateUpdateBankBranch method Exit----");
                    return ApiMessageInfo.Status(1, branch.Id);

                    #region Old Code

                    ////Check if a branch with the IFSC Code already exists against the bank in the database.
                    //branch = await _context.BankBranches.FirstOrDefaultAsync(e => (e.Id == request.Input.Id));

                    //if (request.Input.Id > 0)
                    //{
                    //    duplicatebranch = await _context.BankBranches
                    //    .FirstOrDefaultAsync(e => (e.IFSCCode == request.Input.IFSCCode
                    //    && e.BankCode == request.Input.BankCode
                    //    && e.Id != request.Input.Id));

                    //    if (duplicatebranch is not null)
                    //        return ApiMessageInfo.Status(string.Format("The Bank already has a branch with IFSCCode {0}.", branch.IFSCCode));
                    //    else
                    //    {
                    //        branch.BranchNameEn = obj.BranchNameEn;
                    //        branch.BranchNameAr = obj.BranchNameAr;
                    //        branch.IFSCCode = obj.IFSCCode;
                    //        branch.BankCode = obj.BankCode;
                    //        branch.Id = obj.Id;
                    //        branch.IsActive = obj.IsActive;
                    //        branch.ModifiedBy = request.User.UserId;
                    //        branch.Modified = DateTime.Now;

                    //        _context.BankBranches.Update(branch);
                    //        await _context.SaveChangesAsync();
                    //        await transaction.CommitAsync();

                    //        Log.Info("----Info CreateUpdateBankBranch method Exit----");
                    //        return ApiMessageInfo.Status(1, branch.Id);
                    //    }
                    //}
                    //else
                    //{
                    //    duplicatebranch = await _context.BankBranches
                    //    .FirstOrDefaultAsync(e => (e.IFSCCode == request.Input.IFSCCode
                    //    && e.BankCode == request.Input.BankCode));

                    //    if (duplicatebranch is not null)
                    //        return ApiMessageInfo.Status(string.Format("The Bank already has a branch with IFSCCode {0}.", duplicatebranch.IFSCCode));
                    //    else
                    //    {
                    //        branch = new()
                    //        {
                    //            BranchNameEn = obj.BranchNameEn,
                    //            BranchNameAr = obj.BranchNameAr,
                    //            IFSCCode = obj.IFSCCode,
                    //            BankCode = obj.BankCode,
                    //            IsActive = obj.IsActive,
                    //            CreatedBy = request.User.UserId,
                    //            Created = DateTime.Now,
                    //        };
                    //        await _context.BankBranches.AddAsync(branch);
                    //        await _context.SaveChangesAsync();
                    //        await transaction.CommitAsync();

                    //        Log.Info("----Info CreateUpdateBankBranch method Exit----");
                    //        return ApiMessageInfo.Status(1, branch.Id);
                    //    }
                    //}

                    #endregion

                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateUpdateBankBranch Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return ApiMessageInfo.Status(0);
                }
            }
        }
    }

    #endregion

    #region Delete Bank Branch
    public class DeleteBankBranch : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteBankBranchHandler : IRequestHandler<DeleteBankBranch, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteBankBranchHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteBankBranch request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info DeleteBankBranch method start----");
                if (request.Id > 0)
                {
                    var city = await _context.BankBranches.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(city);
                    await _context.SaveChangesAsync();
                    Log.Info("----Info DeleteBankBranch method end----");
                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {
                Log.Error("Error in DeleteBankBranch Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }
    }

    #endregion

    #region GetBankBranchSelectListItem

    public class GetBankBranchSelectListItem : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string BankCode { get; set; }
    }

    public class GetBankBranchSelectListItemHandler : IRequestHandler<GetBankBranchSelectListItem, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetBankBranchSelectListItemHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetBankBranchSelectListItem request, CancellationToken cancellationToken)
        {
            bool isArab = request.User.Culture.IsArab();
            var search = request.BankCode;
            var list = await _context.BankBranches.Where(e => e.BankCode.Contains(search)).AsNoTracking().OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = isArab ? e.BranchNameAr : e.BranchNameEn, Value = e.BankCode })
                  .ToListAsync(cancellationToken);

            return list;
        }
    }

    #endregion   
}
