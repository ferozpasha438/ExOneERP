using AutoMapper;
using AutoMapper.QueryableExtensions;
using CIN.Application.Common;
using CIN.Application.Payroll.SetUp.Dtos;
using CIN.DB;
using CIN.Domain.Payroll.Setup;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CIN.Application.Payroll.SetUp.Query
{
    #region GetPagedList

    public class GetPayrollPeriodList : IRequest<PaginatedList<TblPRLSysPayrollPeriodDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetPayrollPeriodListHandler : IRequestHandler<GetPayrollPeriodList, PaginatedList<TblPRLSysPayrollPeriodDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetPayrollPeriodListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<TblPRLSysPayrollPeriodDto>> Handle(GetPayrollPeriodList request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info GetPayrollPeriodList method start----");
                var search = request.Input.Query;
                var list = await _context.PayrollPeriods.AsNoTracking().ProjectTo<TblPRLSysPayrollPeriodDto>(_mapper.ConfigurationProvider)
                  .Where(e => (e.PayrollPeriodCode.Contains(search) || e.PayrollPeriodNameAr.Contains(search)))
                   .OrderByDescending(x => x.Id)
                     .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);
                Log.Info("----Info GetPayrollPeriodList method end----");
                return list;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetPayrollPeriodList Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }


    #endregion

    #region GetPayrollPeriodById

    public class GetPayrollPeriodById : IRequest<TblPRLSysPayrollPeriodDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetPayrollPeriodByIdHandler : IRequestHandler<GetPayrollPeriodById, TblPRLSysPayrollPeriodDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetPayrollPeriodByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TblPRLSysPayrollPeriodDto> Handle(GetPayrollPeriodById request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetPayrollPeriodById method start----");
            try
            {
                var PayrollPeriod = await _context.PayrollPeriods.AsNoTracking()
                    .ProjectTo<TblPRLSysPayrollPeriodDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(e => e.Id == request.Id);
                Log.Info("----Info GetPayrollPeriodById method end----");

                if (PayrollPeriod is not null)
                    return PayrollPeriod;
                else
                    return null;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetPayrollPeriodById Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region CreateUpdatePayrollPeriod
    public class CreateUpdatePayrollPeriod : UserIdentityDto, IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblPRLSysPayrollPeriodDto Input { get; set; }
    }
    public class CreateUpdatePayrollPeriodHandler : IRequestHandler<CreateUpdatePayrollPeriod, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdatePayrollPeriodHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<AppCtrollerDto> Handle(CreateUpdatePayrollPeriod request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateUpdatePayrollPeriod method start----");
                    var obj = request.Input;
                    TblPRLSysPayrollPeriod PayrollPeriod = new();

                    if (request.Input.Id > 0)
                    {
                        PayrollPeriod = await _context.PayrollPeriods.FirstOrDefaultAsync(e => e.PayrollPeriodCode == request.Input.PayrollPeriodCode);
                        PayrollPeriod.PayrollPeriodNameEn = obj.PayrollPeriodNameEn;
                        PayrollPeriod.PayrollPeriodNameAr = obj.PayrollPeriodNameAr;
                        PayrollPeriod.PayrollPeriodStartDate = obj.PayrollPeriodStartDate;
                        PayrollPeriod.PayrollPeriodEndDate = obj.PayrollPeriodEndDate;
                        PayrollPeriod.IsOpen = obj.IsOpen;
                        PayrollPeriod.IsClose = obj.IsClose;
                        PayrollPeriod.Id = obj.Id;
                        PayrollPeriod.IsActive = obj.IsActive;
                        PayrollPeriod.ModifiedBy = request.User.UserId;
                        PayrollPeriod.Modified = DateTime.Now;

                        _context.PayrollPeriods.Update(PayrollPeriod);
                    }
                    else
                    {
                        PayrollPeriod = new()
                        {
                            PayrollPeriodNameEn = obj.PayrollPeriodNameEn,
                            PayrollPeriodNameAr = obj.PayrollPeriodNameAr,
                            PayrollPeriodCode = obj.PayrollPeriodCode,
                            PayrollPeriodStartDate = obj.PayrollPeriodStartDate,
                            PayrollPeriodEndDate = obj.PayrollPeriodEndDate,
                            IsOpen = obj.IsOpen,
                            IsClose = obj.IsClose,
                            IsActive = obj.IsActive,
                            CreatedBy = request.User.UserId,
                            Created = DateTime.Now,
                        };
                        await _context.PayrollPeriods.AddAsync(PayrollPeriod);
                    }
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    Log.Info("----Info CreateUpdatePayrollPeriod method Exit----");
                    return ApiMessageInfo.Status(1, PayrollPeriod.Id);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateUpdatePayrollPeriod Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return ApiMessageInfo.Status(0);
                }
            }
        }
    }
    #endregion

    #region DeletePayrollPeriod

    public class DeletePayrollPeriod : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeletePayrollPeriodHandler : IRequestHandler<DeletePayrollPeriod, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeletePayrollPeriodHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeletePayrollPeriod request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info DeletePayrollPeriod method start----");
                if (request.Id > 0)
                {
                    var payrollPeriod = await _context.PayrollPeriods.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(payrollPeriod);
                    await _context.SaveChangesAsync();
                    Log.Info("----Info DeletePayrollPeriod method end----");
                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {
                Log.Error("Error in DeletePayrollPeriod Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }
    }

    #endregion

    #region GetPayrollPeriodSelectListItem

    public class GetPayrollPeriodSelectListItem : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetPayrollPeriodSelectListItemHandler : IRequestHandler<GetPayrollPeriodSelectListItem, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetPayrollPeriodSelectListItemHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<CustomSelectListItem>> Handle(GetPayrollPeriodSelectListItem request, CancellationToken cancellationToken)
        {
            bool isArab = request.User.Culture.IsArab();
            var list = await _context.PayrollPeriods.AsNoTracking().OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = isArab ? e.PayrollPeriodNameAr : e.PayrollPeriodNameEn, Value = e.PayrollPeriodCode })
                  .ToListAsync(cancellationToken);

            return list;
        }
    }

        #endregion

  }
