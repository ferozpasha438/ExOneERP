using AutoMapper;
using AutoMapper.QueryableExtensions;
using CIN.Application.Common;
using CIN.Application.Payroll.Management.Dtos;
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
    #region GetEmployeePayrollStructuredById

    public class GetEmployeePayrollStructuredById : IRequest<BaseEmployeePayrollStructureDto>
    {
        public UserIdentityDto User { get; set; }
        public int EmployeeID { get; set; }
    }

    public class GetEmployeePayrollStructuredByIdHandler : IRequestHandler<GetEmployeePayrollStructuredById, BaseEmployeePayrollStructureDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetEmployeePayrollStructuredByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseEmployeePayrollStructureDto> Handle(GetEmployeePayrollStructuredById request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetEmployeePayrollStructuredById method start----");
            try
            {
                bool isArab = request.User.Culture.IsArab();
                BaseEmployeePayrollStructureDto baseEmployeePayrollStructure = new();

                if (request.EmployeeID > 0)
                {
                    var payrollComponents = await (from employeePayrollStructure in _context.EmployeePayrollStructure
                                                   join payrollPackage in _context.PayrollPackages on employeePayrollStructure.PackageCode equals payrollPackage.PackageCode
                                                   join payrollComponent in _context.PayrollComponents on employeePayrollStructure.PayrollComponentCode equals payrollComponent.PayrollComponentCode
                                                   select new TblPRLTrnEmployeePayrollStructureDto
                                                   {
                                                       Id = payrollPackage.Id,
                                                       EmployeeID = employeePayrollStructure.EmployeeID,
                                                       PackageCode = employeePayrollStructure.PackageCode,
                                                       PayrollComponentCode = employeePayrollStructure.PayrollComponentCode,
                                                       PayrollComponentName = isArab ? payrollComponent.PayrollComponentNameAr : payrollComponent.PayrollComponentNameEn,
                                                       IsFormula = payrollComponent.IsFormula,
                                                       PayValue = employeePayrollStructure.PayValue,
                                                       PayrollComponentType = payrollComponent.PayrollComponentType,
                                                       FormulaQueryString = !string.IsNullOrEmpty(employeePayrollStructure.FormulaQueryString) ? employeePayrollStructure.FormulaQueryString : payrollComponent.FormulaQueryString
                                                   }).AsNoTracking()
                                                   .Where(e => e.EmployeeID == request.EmployeeID &&
                                                   (e.PayrollComponentType == (int)PayrollComponentType.Earning || e.PayrollComponentType == (int)PayrollComponentType.Deduction))
                                                   .OrderBy(e => e.Id)
                                                   .ToListAsync(cancellationToken);

                    if (payrollComponents is not null && payrollComponents.Count() > 0)
                    {
                        //Retrieve PackageCode.
                        baseEmployeePayrollStructure.Id = payrollComponents.FirstOrDefault().Id;
                        baseEmployeePayrollStructure.PackageComponents = payrollComponents;
                    }
                    Log.Info("----Info GetEmployeePayrollStructuredById method end----");
                    return baseEmployeePayrollStructure;
                }
                else
                {
                    Log.Info("----Info GetEmployeePayrollStructuredById method end----");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetEmployeePayrollStructuredById Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region CreateUpdateEmployeePayrollStructure

    public class CreateUpdateEmployeePayrollStructure : UserIdentityDto, IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public BaseEmployeePayrollStructureDto Input { get; set; }
    }
    public class CreateUpdateEmployeePayrollStructureHandler : IRequestHandler<CreateUpdateEmployeePayrollStructure, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateEmployeePayrollStructureHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<AppCtrollerDto> Handle(CreateUpdateEmployeePayrollStructure request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateUpdateEmployeePayrollStructure method start----");
                    var obj = request.Input;
                    int _employeeID = 0;
                    TblPRLTrnPayrollPackage payrollPackage = new();

                    //Retrieve PayrollPackage details based on Id.
                    payrollPackage = await _context.PayrollPackages.FirstOrDefaultAsync(e => e.Id == obj.Id);

                    if (payrollPackage is not null)
                    {
                        //Add Payroll Package Components.
                        if (obj.PackageComponents.Count() > 0)
                        {
                            _employeeID = obj.PackageComponents.FirstOrDefault().EmployeeID;

                            //Retrieve the list of old EmployeePayrollComponents and delete.
                            var _oldEmployeePayrollStructureComponents = await _context.EmployeePayrollStructure
                                .Where(e => e.EmployeeID == _employeeID).ToListAsync();
                            if (_oldEmployeePayrollStructureComponents is not null)
                                _context.EmployeePayrollStructure.RemoveRange(_oldEmployeePayrollStructureComponents);

                            List<TblPRLTrnEmployeePayrollStructure> employeePayrollStructureComponents = new();
                            obj.PackageComponents.ForEach(e =>
                            {
                                TblPRLTrnEmployeePayrollStructure employeePayrollStructureComponent = new TblPRLTrnEmployeePayrollStructure()
                                {
                                    EmployeeID = e.EmployeeID,
                                    PackageCode = payrollPackage.PackageCode,
                                    PayrollComponentCode = e.PayrollComponentCode,
                                    FormulaQueryString = e.IsFormula ? e.FormulaQueryString : string.Empty,
                                    PayValue = e.PayValue,
                                    IsActive = true,
                                    CreatedBy = request.User.UserId,
                                    Created = DateTime.Now,
                                    ModifiedBy = request.User.UserId,
                                    Modified = DateTime.Now
                                };
                                employeePayrollStructureComponents.Add(employeePayrollStructureComponent);
                            });

                            if (employeePayrollStructureComponents.Count > 0)
                            {
                                await _context.EmployeePayrollStructure.AddRangeAsync(employeePayrollStructureComponents);
                                await _context.SaveChangesAsync();
                            }
                            await transaction.CommitAsync();
                        }
                        else
                        {
                            Log.Info("----Info CreateUpdateEmployeePayrollStructure method Exit with PayrollComponentsDoesNotExist message ----");
                            return ApiMessageInfo.Status("PayrollComponentsDoesNotExist");
                        }
                    }
                    else
                    {
                        Log.Info("----Info CreateUpdateEmployeePayrollStructure method Exit with PayrollPackageDoesNotExist message ----");
                        return ApiMessageInfo.Status("PayrollPackageDoesNotExist");
                    }
                    Log.Info("----Info CreateUpdateEmployeePayrollStructure method Exit----");
                    return ApiMessageInfo.Status(1, payrollPackage.Id);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateUpdateEmployeePayrollStructure Method");
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
