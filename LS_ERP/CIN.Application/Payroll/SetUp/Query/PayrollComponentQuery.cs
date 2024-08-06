using AutoMapper;
using AutoMapper.QueryableExtensions;
using CIN.Application.Common;
using CIN.Application.Payroll.SetUp.Dtos;
using CIN.Application.Payroll.Utility;
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

    public class GetPayrollComponentList : IRequest<PaginatedList<TblPRLSysPayrollComponentDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetPayrollComponentListHandler : IRequestHandler<GetPayrollComponentList, PaginatedList<TblPRLSysPayrollComponentDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetPayrollComponentListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<TblPRLSysPayrollComponentDto>> Handle(GetPayrollComponentList request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info GetPayrollComponentList method start----");
                var search = request.Input.Query;
                var list = await _context.PayrollComponents
                    .AsNoTracking()
                    .Where(e => (e.PayrollComponentCode.Contains(search) || e.PayrollComponentNameEn.Contains(search)))
                    .Select(e => new TblPRLSysPayrollComponentDto
                    {
                        Id = e.Id,
                        PayrollComponentCode = e.PayrollComponentCode,
                        PayrollComponentNameEn = e.PayrollComponentNameEn,
                        PayrollComponentNameAr = e.PayrollComponentNameAr,
                        PayrollComponentType = e.PayrollComponentType,
                        PayrollComponentTypeName = ((PayrollComponentType)e.PayrollComponentType).ToString(),
                        FormulaQueryString = e.FormulaQueryString,
                        IsUsedForOtherPayrollComponent = e.IsUsedForOtherPayrollComponent,
                        IsApplicableForDeduction = e.IsApplicableForDeduction,
                        Created = e.Created,
                        CreatedBy = e.CreatedBy,
                        IsActive = e.IsActive,
                        IsFormula = e.IsFormula,
                        Modified = e.Modified,
                        ModifiedBy = e.ModifiedBy
                    })
                    .OrderByDescending(x => x.Id)
                    .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);
                Log.Info("----Info GetPayrollComponentList method end----");
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

    #region GetPayrollComponentById

    public class GetPayrollComponentById : IRequest<TblPRLSysPayrollComponentDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetPayrollComponentByIdHandler : IRequestHandler<GetPayrollComponentById, TblPRLSysPayrollComponentDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetPayrollComponentByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TblPRLSysPayrollComponentDto> Handle(GetPayrollComponentById request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetPayrollComponentById method start----");
            try
            {
                var PayrollComponent = await _context.PayrollComponents.AsNoTracking()
                    .ProjectTo<TblPRLSysPayrollComponentDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(e => e.Id == request.Id);
                Log.Info("----Info GetPayrollComponentById method end----");

                if (PayrollComponent is not null)
                    return PayrollComponent;
                else
                    return null;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetPayrollComponentById Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }
    #endregion

    #region GetPayrollComponentByCode

    public class GetPayrollComponentByCode : IRequest<TblPRLSysPayrollComponentDto>
    {
        public UserIdentityDto User { get; set; }
        public string PayrollComponentCode { get; set; }
    }

    public class GetPayrollComponentByCodeHandler : IRequestHandler<GetPayrollComponentByCode, TblPRLSysPayrollComponentDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetPayrollComponentByCodeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TblPRLSysPayrollComponentDto> Handle(GetPayrollComponentByCode request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetPayrollComponentByCode method start----");
            try
            {
                var PayrollComponent = await _context.PayrollComponents.AsNoTracking()
                    .ProjectTo<TblPRLSysPayrollComponentDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(e => e.PayrollComponentCode == request.PayrollComponentCode);
                Log.Info("----Info GetPayrollComponentByCode method end----");

                if (PayrollComponent is not null)
                    return PayrollComponent;
                else
                    return null;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetPayrollComponentByCode Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }
    #endregion

    #region CreateUpdatePayrollPeriod
    public class CreateUpdatePayrollComponent : UserIdentityDto, IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblPRLSysPayrollComponentDto Input { get; set; }
    }
    public class CreateUpdatePayrollComponentHandler : IRequestHandler<CreateUpdatePayrollComponent, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdatePayrollComponentHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<AppCtrollerDto> Handle(CreateUpdatePayrollComponent request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateUpdatePayrollComponent method start----");
                    var obj = request.Input;
                    TblPRLSysPayrollComponent payrollComponent = new();

                    if (request.Input.Id > 0)
                    {
                        payrollComponent = await _context.PayrollComponents.FirstOrDefaultAsync(e => e.PayrollComponentCode == request.Input.PayrollComponentCode);
                        payrollComponent.PayrollComponentNameEn = obj.PayrollComponentNameEn;
                        payrollComponent.PayrollComponentNameAr = obj.PayrollComponentNameAr;
                        payrollComponent.PayrollComponentType = obj.PayrollComponentType;
                        payrollComponent.IsFormula = obj.IsFormula;
                        payrollComponent.FormulaQueryString = obj.FormulaQueryString;
                        payrollComponent.IsUsedForOtherPayrollComponent = obj.IsUsedForOtherPayrollComponent;
                        payrollComponent.IsApplicableForDeduction = obj.IsApplicableForDeduction;
                        payrollComponent.IsActive = obj.IsActive;
                        payrollComponent.ModifiedBy = request.User.UserId;
                        payrollComponent.Modified = DateTime.Now;

                        _context.PayrollComponents.Update(payrollComponent);
                    }
                    else
                    {
                        payrollComponent = new()
                        {
                            PayrollComponentCode = obj.PayrollComponentCode,
                            PayrollComponentNameEn = obj.PayrollComponentNameEn,
                            PayrollComponentNameAr = obj.PayrollComponentNameAr,
                            PayrollComponentType = obj.PayrollComponentType,
                            IsFormula = obj.IsFormula,
                            FormulaQueryString = obj.FormulaQueryString,
                            IsUsedForOtherPayrollComponent = obj.IsUsedForOtherPayrollComponent,
                            IsApplicableForDeduction = obj.IsApplicableForDeduction,
                            IsActive = obj.IsActive,
                            CreatedBy = request.User.UserId,
                            Created = DateTime.Now,
                        };
                        await _context.PayrollComponents.AddAsync(payrollComponent);
                    }
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    Log.Info("----Info CreateUpdatePayrollComponent method Exit----");
                    return ApiMessageInfo.Status(1, payrollComponent.Id);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateUpdatePayrollComponent Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return ApiMessageInfo.Status(0);
                }
            }
        }
    }
    #endregion

    #region DeletePayrollComponent

    public class DeletePayrollComponent : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeletePayrollComponentHandler : IRequestHandler<DeletePayrollComponent, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeletePayrollComponentHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeletePayrollComponent request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info DeletePayrollComponent method start----");
                if (request.Id > 0)
                {
                    var payrollComponent = await _context.PayrollComponents.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(payrollComponent);
                    await _context.SaveChangesAsync();
                    Log.Info("----Info DeletePayrollComponent method end----");
                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {
                Log.Error("Error in DeletePayrollComponent Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }
    }
    #endregion

    #region GetPayrollComponentSelectListItem

    public class GetPayrollComponentSelectListItem : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public bool IsStructured { get; set; }
    }

    public class GetPayrollComponentSelectListItemHandler : IRequestHandler<GetPayrollComponentSelectListItem, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetPayrollComponentSelectListItemHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<CustomSelectListItem>> Handle(GetPayrollComponentSelectListItem request, CancellationToken cancellationToken)
        {
            bool isArab = request.User.Culture.IsArab();
            bool isStructured = request.IsStructured;
            List<CustomSelectListItem> list = new();

            if (isStructured)
            {
                list = await _context.PayrollComponents
                    .AsNoTracking()
                    .Where(e => (e.PayrollComponentType == (int)PayrollComponentType.Earning || e.PayrollComponentType == (int)PayrollComponentType.Deduction))
                    .OrderByDescending(e => e.Id)
                    .Select(e => new CustomSelectListItem { Text = isArab ? e.PayrollComponentNameAr : e.PayrollComponentNameEn, Value = e.PayrollComponentCode })
                    .ToListAsync(cancellationToken);
            }
            else
            {
                list = await _context.PayrollComponents
                    .AsNoTracking()
                    .Where(e => (e.PayrollComponentType == (int)PayrollComponentType.UnStructuredEarning || e.PayrollComponentType == (int)PayrollComponentType.UnStructuredDeduction))
                    .OrderByDescending(e => e.Id).Select(e => new CustomSelectListItem { Text = isArab ? e.PayrollComponentNameAr : e.PayrollComponentNameEn, Value = e.PayrollComponentCode })
                    .ToListAsync(cancellationToken);
            }

            return list;
        }
    }
    #endregion

    #region GetPayrollComponents

    public class GetPayrollComponents : IRequest<List<TblPRLSysPayrollComponentDto>>
    {
        public UserIdentityDto User { get; set; }
        public bool IsStructured { get; set; }
    }

    public class GetPayrollComponentsHandler : IRequestHandler<GetPayrollComponents, List<TblPRLSysPayrollComponentDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetPayrollComponentsHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<TblPRLSysPayrollComponentDto>> Handle(GetPayrollComponents request, CancellationToken cancellationToken)
        {
            bool isArab = request.User.Culture.IsArab();
            bool isStructured = request.IsStructured;
            List<TblPRLSysPayrollComponentDto> list = new();

            if (isStructured)
            {
                list = await _context.PayrollComponents
                    .AsNoTracking().ProjectTo<TblPRLSysPayrollComponentDto>(_mapper.ConfigurationProvider)
                    .Where(e => (e.PayrollComponentType == (int)PayrollComponentType.Earning || e.PayrollComponentType == (int)PayrollComponentType.Deduction))
                    .OrderByDescending(e => e.Id)
                    .ToListAsync(cancellationToken);
            }
            else
            {
                list = await _context.PayrollComponents
                    .AsNoTracking().ProjectTo<TblPRLSysPayrollComponentDto>(_mapper.ConfigurationProvider)
                    .Where(e => (e.PayrollComponentType == (int)PayrollComponentType.UnStructuredEarning || e.PayrollComponentType == (int)PayrollComponentType.UnStructuredDeduction))
                    .OrderByDescending(e => e.Id)
                    .ToListAsync(cancellationToken);
            }

            return list;
        }
    }
    #endregion

    #region GetPayrollComponentTypeSelectListItem

    public class GetPayrollComponentTypeSelectListItem : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetPayrollComponentTypeSelectListItemHandler : IRequestHandler<GetPayrollComponentTypeSelectListItem, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetPayrollComponentTypeSelectListItemHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<CustomSelectListItem>> Handle(GetPayrollComponentTypeSelectListItem request, CancellationToken cancellationToken)
        {
            bool isArab = request.User.Culture.IsArab();
            await Task.Run(() => { Thread.Sleep(100); });
            var list = Enum.GetValues(typeof(PayrollComponentType))
                .Cast<PayrollComponentType>()
                .Select(v => new CustomSelectListItem
                {
                    Text = v.ToString(),
                    Value = ((int)v).ToString()
                }).ToList();
            return list;
        }
    }
    #endregion
}
