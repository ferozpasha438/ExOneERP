using AutoMapper;
using AutoMapper.QueryableExtensions;
using CIN.Application.Common;
using CIN.Application.Payroll.Management.Dtos;
using CIN.Application.Payroll.SetUp.Dtos;
using CIN.Application.Payroll.Utility;
using CIN.DB;
using CIN.Domain.Payroll.Management;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CIN.Application.Payroll.Management.Query
{
    #region GetPagedList

    public class GetPayrollPackageList : IRequest<PaginatedList<TblPRLTrnPayrollPackageDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetPayrollPackageListHandler : IRequestHandler<GetPayrollPackageList, PaginatedList<TblPRLTrnPayrollPackageDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetPayrollPackageListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<TblPRLTrnPayrollPackageDto>> Handle(GetPayrollPackageList request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info GetPayrollPackageList method start----");
                var search = request.Input.Query;
                bool isArab = request.User.Culture.IsArab();

                var list = await (from payrollPackages in _context.PayrollPackages
                                  join grade in _context.Grades on payrollPackages.GradeCode equals grade.GradeCode into grades
                                  from g in grades.DefaultIfEmpty()
                                  join position in _context.Positions on payrollPackages.PositionCode equals position.PositionCode into positions
                                  from p in positions.DefaultIfEmpty()
                                  select new TblPRLTrnPayrollPackageDto
                                  {
                                      Id = payrollPackages.Id,
                                      PackageCode = payrollPackages.PackageCode,
                                      PackageNameEn = payrollPackages.PackageNameEn,
                                      PackageNameAr = payrollPackages.PackageNameAr,
                                      GradeCode = payrollPackages.GradeCode,
                                      GradeName = isArab ? g.GradeNameAr : g.GradeNameEn,
                                      PositionCode = payrollPackages.PositionCode,
                                      PositionName = isArab ? p.PositionNameAr : p.PositionNameEn,
                                      Created = payrollPackages.Created,
                                      CreatedBy = payrollPackages.CreatedBy,
                                      IsActive = payrollPackages.IsActive,
                                      Modified = payrollPackages.Modified,
                                      ModifiedBy = payrollPackages.ModifiedBy,
                                      PackageName = isArab ? payrollPackages.PackageNameAr : payrollPackages.PackageNameEn
                                  })
                                  .AsNoTracking()
                                  .Where(e => (e.PackageCode.Contains(search) || e.PackageNameEn.Contains(search)))
                                  .OrderByDescending(x => x.Id)
                                  .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

                Log.Info("----Info GetPayrollPackageList method end----");
                return list;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetPayrollPackageList Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region GetPayrollPackageById

    public class GetPayrollPackageById : IRequest<TblPRLTrnPayrollPackageDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetPayrollPackageByIdHandler : IRequestHandler<GetPayrollPackageById, TblPRLTrnPayrollPackageDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetPayrollPackageByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TblPRLTrnPayrollPackageDto> Handle(GetPayrollPackageById request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetPayrollPackageById method start----");
            try
            {
                bool isArab = request.User.Culture.IsArab();
                var payrollPackage = await _context.PayrollPackages.AsNoTracking()
                    .ProjectTo<TblPRLTrnPayrollPackageDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(e => e.Id == request.Id);
                Log.Info("----Info GetPayrollPackageById method end----");

                if (payrollPackage is not null)
                {
                    var payrollPackageComponents = await (from payrollPackageComponent in _context.PayrollPackageComponents
                                                          join payrollComponent in _context.PayrollComponents on payrollPackageComponent.PayrollComponentCode equals payrollComponent.PayrollComponentCode
                                                          select new TblPRLTrnPayrollPackageComponentDto
                                                          {
                                                              Id = payrollPackageComponent.Id,
                                                              PackageCode = payrollPackageComponent.PackageCode,
                                                              PayrollComponentCode = payrollPackageComponent.PayrollComponentCode,
                                                              PayrollComponentName = isArab ? payrollComponent.PayrollComponentNameAr : payrollComponent.PayrollComponentNameEn,
                                                              IsFormula = payrollComponent.IsFormula,
                                                              PayValue = !payrollComponent.IsFormula ? payrollPackageComponent.PayValue : 0,
                                                              PayrollComponentType = payrollComponent.PayrollComponentType,
                                                              FormulaQueryString = payrollComponent.FormulaQueryString
                                                          }).AsNoTracking()
                                                          .Where(e => e.PackageCode == payrollPackage.PackageCode)
                                                          .OrderByDescending(e => e.Id)
                                                          .ToListAsync(cancellationToken);

                    //Retrieve basic Salary.
                    payrollPackage.BasicSalary = payrollPackageComponents.Where(e => e.PayrollComponentCode.ToLower().Equals("basic")).FirstOrDefault().PayValue;

                    //Calculate Payvalue for all the PayrollComponent of type Earnings or deductions & whose PayValue is calculated based on Formula and Basic Salary.
                    foreach (var payrollPackageComponent in payrollPackageComponents)
                    {
                        //Check if PayrollComponent is of type Earnings or deductions & PayValue is calculated based on gross salary.
                        if (payrollPackageComponent.IsFormula && !payrollPackageComponent.FormulaQueryString.ToLower().Contains("gross") &&
                            (payrollPackageComponent.PayrollComponentType == (int)PayrollComponentType.Earning || payrollPackageComponent.PayrollComponentType == (int)PayrollComponentType.Deduction))
                        {
                            payrollPackageComponent.PayValue = CalculatePayValue(payrollPackageComponent.FormulaQueryString, payrollPackageComponents);
                        }
                    }

                    //Calculate Payvalue for all the PayrollComponent of type deductions & whose PayValue is calculated based on gross Salary using formula.
                    foreach (var payrollPackageComponent in payrollPackageComponents)
                    {
                        //Check if PayrollComponent of type deductions & whose PayValue is calculated based on gross Salary using formula.
                        if (payrollPackageComponent.IsFormula && payrollPackageComponent.FormulaQueryString.ToLower().Contains("gross") &&
                            payrollPackageComponent.PayrollComponentType == (int)PayrollComponentType.Deduction)
                        {
                            payrollPackageComponent.PayValue = CalculatePayValue(payrollPackageComponent.FormulaQueryString, payrollPackageComponents);
                        }
                    }

                    if (payrollPackageComponents is not null && payrollPackageComponents.Count() > 0)
                    {
                        payrollPackage.PackageComponents = payrollPackageComponents;
                    }
                    return payrollPackage;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetPayrollPackageById Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
        public decimal CalculatePayValue(string formulaeQueryString, List<TblPRLTrnPayrollPackageComponentDto> payrollPackageComponents)
        {
            decimal _payValue = 0;

            //formulaQueryString  will be in the format of "Basic*(10/100);HRA*(5/100)"
            if (!string.IsNullOrEmpty(formulaeQueryString))
            {

                string[] formulae = formulaeQueryString.Split(new char[] { ';' });
                if (formulae.Count() > 0)
                {
                    foreach (var formula in formulae)
                    {
                        if (!string.IsNullOrEmpty(formula))
                        {
                            string[] expression = formula.Trim().Split(new char[] { '*' });

                            if (expression[1].Contains("(")) expression[1] = expression[1].Replace("(", "");
                            if (expression[1].Contains(")")) expression[1] = expression[1].Replace(")", "");

                            if (expression.Count() == 2)
                            {
                                string[] _fraction = expression[1].Trim().Split(new char[] { '/' });
                                decimal _percentage = 0;

                                if (_fraction.Count() == 2)
                                    _percentage = decimal.Parse(_fraction[0]);

                                string _payrollComponentCode = expression[0].Trim();
                                if (!formulaeQueryString.ToLower().Contains("gross"))
                                {
                                    var payrollPackageComponent = payrollPackageComponents.FirstOrDefault(e => e.PayrollComponentCode.ToLower() == _payrollComponentCode.ToLower());
                                    if (payrollPackageComponent is not null)
                                    {
                                        if (payrollPackageComponent.IsFormula)
                                        {
                                            //Calculates payvalue for all the earning or deduction payroll components those are based on 'BASIC' salary.
                                            _payValue = _payValue + ((_percentage / 100) * CalculatePayValue(payrollPackageComponent.FormulaQueryString, payrollPackageComponents));
                                        }
                                        else
                                            _payValue = _payValue + ((_percentage / 100) * payrollPackageComponent.PayValue);
                                    }
                                }
                                else
                                {
                                    //Calculate gross Salary using all the earnings.
                                    decimal grossSalary = payrollPackageComponents.Where(e => e.PayrollComponentType == (int)PayrollComponentType.Earning).Sum(x => x.PayValue);
                                    _payValue = _payValue + ((_percentage / 100) * grossSalary);
                                }
                            }
                        }
                    }
                }

            }
            return _payValue;
        }
    }

    #endregion

    #region CreateUpdatePayrollPackage

    public class CreateUpdatePayrollPackage : UserIdentityDto, IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblPRLTrnPayrollPackageDto Input { get; set; }
    }
    public class CreateUpdatePayrollPackageHandler : IRequestHandler<CreateUpdatePayrollPackage, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdatePayrollPackageHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<AppCtrollerDto> Handle(CreateUpdatePayrollPackage request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateUpdatePayrollPackage method start----");
                    var obj = request.Input;
                    TblPRLTrnPayrollPackage payrollPackage = new();

                    payrollPackage = await _context.PayrollPackages.FirstOrDefaultAsync(e => e.PackageCode == request.Input.PackageCode);

                    if (payrollPackage is not null)
                    {
                        payrollPackage.PackageNameEn = obj.PackageNameEn;
                        payrollPackage.PackageNameAr = obj.PackageNameAr;
                        payrollPackage.GradeCode = obj.GradeCode;
                        payrollPackage.PositionCode = obj.PositionCode;
                        payrollPackage.Id = obj.Id;
                        payrollPackage.IsActive = obj.IsActive;
                        payrollPackage.ModifiedBy = request.User.UserId;
                        payrollPackage.Modified = DateTime.Now;

                        var payrollPackageComponents = _context.PayrollPackageComponents.Where(e => e.PackageCode == request.Input.PackageCode);
                        _context.RemoveRange(payrollPackageComponents);

                        _context.PayrollPackages.Update(payrollPackage);
                    }
                    else
                    {
                        payrollPackage = new()
                        {
                            PackageCode = obj.PackageCode,
                            PackageNameEn = obj.PackageNameEn,
                            PackageNameAr = obj.PackageNameAr,
                            GradeCode = obj.GradeCode,
                            PositionCode = obj.PositionCode,
                            IsActive = obj.IsActive,
                            CreatedBy = request.User.UserId,
                            Created = DateTime.Now,
                        };
                        await _context.PayrollPackages.AddAsync(payrollPackage);
                    }
                    await _context.SaveChangesAsync();

                    //Add Payroll Package Components.
                    if (request.Input.PackageComponents.Count() > 0)
                    {
                        List<TblPRLTrnPayrollPackageComponent> payrollPackageComponents = new();

                        request.Input.PackageComponents.ForEach(e =>
                        {
                            TblPRLTrnPayrollPackageComponent payrollPackageComponent = new TblPRLTrnPayrollPackageComponent()
                            {
                                PackageCode = e.PackageCode,
                                PayrollComponentCode = e.PayrollComponentCode,
                                PayValue = !e.IsFormula ? e.PayValue : 0,
                            };
                            payrollPackageComponents.Add(payrollPackageComponent);
                        });

                        if (payrollPackageComponents.Count > 0)
                            await _context.PayrollPackageComponents.AddRangeAsync(payrollPackageComponents);

                        await _context.SaveChangesAsync();
                    }

                    await transaction.CommitAsync();

                    Log.Info("----Info CreateUpdatePayrollPackage method Exit----");
                    return ApiMessageInfo.Status(1, payrollPackage.Id);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateUpdatePayrollPackage Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return ApiMessageInfo.Status(0);
                }
            }
        }
    }

    #endregion

    #region DeletePayrollPackage
    public class DeletePayrollPackage : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeletePayrollPackageHandler : IRequestHandler<DeletePayrollPackage, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeletePayrollPackageHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeletePayrollPackage request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info DeletePayrollPackage method start----");
                    if (request.Id > 0)
                    {
                        var payrollPackage = await _context.PayrollPackages.FirstOrDefaultAsync(e => e.Id == request.Id);

                        var payrollPackageComponents = await _context.PayrollPackageComponents
                                .Where(e => e.PackageCode == payrollPackage.PackageCode).ToListAsync();

                        if (payrollPackageComponents is not null)
                            _context.PayrollPackageComponents.RemoveRange(payrollPackageComponents);

                        _context.PayrollPackages.Remove(payrollPackage);

                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();
                        Log.Info("----Info DeletePayrollPackage method end----");
                        return request.Id;
                    }
                    return 0;
                }
                catch (Exception ex)
                {
                    Log.Error("Error in DeletePayrollPackage Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return 0;
                }
            }
        }
    }

    #endregion

    #region GetPayrollPackageSelectListItem
    public class GetPayrollPackageSelectListItem : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public PayrollPackageFilterDto Filter { get; set; }
    }

    public class GetPayrollPackageSelectListItemHandler : IRequestHandler<GetPayrollPackageSelectListItem, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetPayrollPackageSelectListItemHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetPayrollPackageSelectListItem request, CancellationToken cancellationToken)
        {
            bool isArab = request.User.Culture.IsArab();
            var filter = request.Filter;

            var list = await _context.PayrollPackages
                .AsNoTracking()
                .Where(e => (string.IsNullOrEmpty(filter.PositionCode) ? e.PositionCode == null : e.PositionCode == filter.PositionCode) &&
                (string.IsNullOrEmpty(filter.GradeCode) ? e.GradeCode == null : e.GradeCode == filter.GradeCode)).
                OrderByDescending(e => e.Id).
                Select(e => new CustomSelectListItem { Text = isArab ? e.PackageNameAr : e.PackageNameEn, Value = e.Id.ToString() }).
                ToListAsync(cancellationToken);
            return list;
        }
    }

    #endregion   
}
