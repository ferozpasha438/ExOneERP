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

    public class GetBankList : IRequest<PaginatedList<TblHRMSysBankDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetBankListHandler : IRequestHandler<GetBankList, PaginatedList<TblHRMSysBankDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetBankListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<TblHRMSysBankDto>> Handle(GetBankList request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info GetBankList method start----");
                var search = request.Input.Query;
                var list = await _context.Banks.AsNoTracking().ProjectTo<TblHRMSysBankDto>(_mapper.ConfigurationProvider)
                  .Where(e => (e.BankCode.Contains(search) || e.BankNameEn.Contains(search)))
                   .OrderByDescending(x => x.Id)
                     .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);
                Log.Info("----Info GetBankList method end----");
                return list;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetBankList Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region GetBankById

    public class GetBankById : IRequest<TblHRMSysBankDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetBankByIdHandler : IRequestHandler<GetBankById, TblHRMSysBankDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetBankByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TblHRMSysBankDto> Handle(GetBankById request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetBankById method start----");
            try
            {
                var bank = await _context.Banks.AsNoTracking()
                    .ProjectTo<TblHRMSysBankDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(e => e.Id == request.Id);
                Log.Info("----Info GetBankById method end----");

                if (bank is not null)
                    return bank;
                else
                    return null;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetBankById Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region CreateUpdateBank

    public class CreateUpdateBank : UserIdentityDto, IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblHRMSysBankDto Input { get; set; }
    }
    public class CreateUpdateBankHandler : IRequestHandler<CreateUpdateBank, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateBankHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<AppCtrollerDto> Handle(CreateUpdateBank request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateUpdateBank method start----");
                    var obj = request.Input;
                    TblHRMSysBank bank = new();

                    if (request.Input.Id > 0)
                    {
                        bank = await _context.Banks.FirstOrDefaultAsync(e => e.BankCode == request.Input.BankCode);
                        bank.BankNameEn = obj.BankNameEn;
                        bank.BankNameAr = obj.BankNameAr;
                        bank.Id = obj.Id;
                        bank.IsActive = obj.IsActive;
                        bank.ModifiedBy = request.User.UserId;
                        bank.Modified = DateTime.Now;

                        _context.Banks.Update(bank);
                    }
                    else
                    {
                        bank = new()
                        {
                            BankNameEn = obj.BankNameEn,
                            BankNameAr = obj.BankNameAr,
                            BankCode = obj.BankCode,
                            IsActive = obj.IsActive,
                            CreatedBy = request.User.UserId,
                            Created = DateTime.Now,
                        };
                        await _context.Banks.AddAsync(bank);
                    }
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    Log.Info("----Info CreateUpdateBank method Exit----");
                    return ApiMessageInfo.Status(1, bank.Id);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateUpdateBank Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return ApiMessageInfo.Status(0);
                }
            }
        }
    }

    #endregion

    #region Delete Bank
    public class DeleteBank : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteBankHandler : IRequestHandler<DeleteBank, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteBankHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteBank request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info DeleteBank method start----");
                if (request.Id > 0)
                {
                    var city = await _context.Banks.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(city);
                    await _context.SaveChangesAsync();
                    Log.Info("----Info DeleteBank method end----");
                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {
                Log.Error("Error in DeleteBank Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }
    }

    #endregion

    #region GetBankSelectListItem

    public class GetBankSelectListItem : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetBankSelectListItemHandler : IRequestHandler<GetBankSelectListItem, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetBankSelectListItemHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetBankSelectListItem request, CancellationToken cancellationToken)
        {
            bool isArab = request.User.Culture.IsArab();
            var list = await _context.Banks.AsNoTracking().OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = isArab ? e.BankNameAr : e.BankNameEn, Value = e.BankCode })
                  .ToListAsync(cancellationToken);

            return list;
        }
    }

    #endregion   
}
