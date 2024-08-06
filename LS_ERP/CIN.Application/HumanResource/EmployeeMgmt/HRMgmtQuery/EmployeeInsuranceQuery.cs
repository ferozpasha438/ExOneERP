using AutoMapper;
using AutoMapper.QueryableExtensions;
using CIN.Application.Common;
using CIN.Application.HumanResource.EmployeeMgmt.HRMgmtDtos;
using CIN.DB;
using CIN.Domain.HumanResource.EmployeeMgt;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CIN.Application.HumanResource.EmployeeMgmt.HRMgmtQuery
{
    #region GetPagedList

    public class GetEmployeeInsuranceList : IRequest<PaginatedList<TblHRMTrnEmployeeInsuranceInfoDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetEmployeeInsuranceListHandler : IRequestHandler<GetEmployeeInsuranceList, PaginatedList<TblHRMTrnEmployeeInsuranceInfoDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetEmployeeInsuranceListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<TblHRMTrnEmployeeInsuranceInfoDto>> Handle(GetEmployeeInsuranceList request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info GetEmployeeInsuranceList method start----");
                var search = request.Input.Query;
                bool isArab = request.User.Culture.IsArab();
                var list = await (from employeeInsurance in _context.EmployeeInsurances
                                  join insuranceType in _context.InsuranceTypes on employeeInsurance.InsuranceTypeCode equals insuranceType.InsuranceTypeCode
                                  join insuranceProvider in _context.InsuranceProviders on employeeInsurance.InsuranceProviderCode equals insuranceProvider.InsuranceProviderCode
                                  join insuranceClass in _context.InsuranceClasses on employeeInsurance.InsuranceClassCode equals insuranceClass.InsuranceClassCode
                                  select new TblHRMTrnEmployeeInsuranceInfoDto
                                  {
                                      Id = employeeInsurance.Id,
                                      EmployeeID = employeeInsurance.EmployeeID,
                                      InsuranceTypeCode = employeeInsurance.InsuranceTypeCode,
                                      InsuranceProviderCode = employeeInsurance.InsuranceProviderCode,
                                      InsuranceClassCode = employeeInsurance.InsuranceClassCode,
                                      PolicyName = employeeInsurance.PolicyName,
                                      PolicyNumber = employeeInsurance.PolicyNumber,
                                      PolicyHolder = employeeInsurance.PolicyHolder,
                                      PolicyStartDate = employeeInsurance.PolicyStartDate,
                                      PolicyExpiryDate = employeeInsurance.PolicyExpiryDate,
                                      PremiumPerYear = employeeInsurance.PremiumPerYear,
                                      Remarks = employeeInsurance.Remarks,
                                      InsuranceTypeName = isArab ? insuranceType.InsuranceTypeNameAr : insuranceType.InsuranceTypeNameEn,
                                      InsuranceProviderName = isArab ? insuranceProvider.InsuranceProviderNameAr : insuranceProvider.InsuranceProviderNameEn,
                                      InsuranceClassName = isArab ? insuranceClass.InsuranceClassNameAr : insuranceClass.InsuranceClassNameEn,
                                      IsActive = employeeInsurance.IsActive,
                                  })
                                  .AsNoTracking()
                                  .Where(e => (e.EmployeeID == int.Parse(request.Input.Code) &&
                                  (e.PolicyName.Contains(search) || e.PolicyNumber.Contains(search) || e.PolicyHolder.Contains(search) || e.InsuranceTypeName.Contains(search) || e.InsuranceProviderName.Contains(search) ||
                                  e.InsuranceClassName.Contains(search))))
                                  .OrderByDescending(x => x.Id)
                                  .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);
                Log.Info("----Info GetEmployeeInsuranceList method end----");
                return list;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetEmployeeInsuranceList Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region GetEmployeeInsuranceById

    public class GetEmployeeInsuranceById : IRequest<TblHRMTrnEmployeeInsuranceInfoDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
        public int EmployeeID { get; set; }
    }

    public class GetEmployeeInsuranceByIdHandler : IRequestHandler<GetEmployeeInsuranceById, TblHRMTrnEmployeeInsuranceInfoDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetEmployeeInsuranceByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TblHRMTrnEmployeeInsuranceInfoDto> Handle(GetEmployeeInsuranceById request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetEmployeeInsuranceById method start----");
            try
            {
                var employeeInsurance = await _context.EmployeeInsurances.AsNoTracking()
                    .ProjectTo<TblHRMTrnEmployeeInsuranceInfoDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(e => e.Id == request.Id && e.EmployeeID == request.EmployeeID);
                Log.Info("----Info GetEmployeeInsuranceById method end----");

                if (employeeInsurance is not null)
                    return employeeInsurance;
                else
                    return null;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetEmployeeInsuranceById Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region CreateUpdateEmployeeInsurance

    public class CreateUpdateEmployeeInsurance : UserIdentityDto, IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblHRMTrnEmployeeInsuranceInfoDto Input { get; set; }
    }
    public class CreateUpdateEmployeeInsuranceHandler : IRequestHandler<CreateUpdateEmployeeInsurance, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateEmployeeInsuranceHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<AppCtrollerDto> Handle(CreateUpdateEmployeeInsurance request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateUpdateEmployeeInsurance method start----");
                    var obj = request.Input;
                    TblHRMTrnEmployeeInsuranceInfo employeeInsurance = new();

                    if (request.Input.Id > 0)
                    {
                        employeeInsurance = await _context.EmployeeInsurances
                            .FirstOrDefaultAsync(e => e.Id == request.Input.Id && e.EmployeeID == request.Input.EmployeeID);
                        employeeInsurance.InsuranceTypeCode = obj.InsuranceTypeCode;
                        employeeInsurance.InsuranceProviderCode = obj.InsuranceProviderCode;
                        employeeInsurance.InsuranceClassCode = obj.InsuranceClassCode;
                        employeeInsurance.PolicyName = obj.PolicyName;
                        employeeInsurance.PolicyNumber = obj.PolicyNumber;
                        employeeInsurance.PolicyHolder = obj.PolicyHolder;
                        employeeInsurance.PolicyStartDate = obj.PolicyStartDate;
                        employeeInsurance.PolicyExpiryDate = obj.PolicyExpiryDate;
                        employeeInsurance.PremiumPerYear = obj.PremiumPerYear;
                        employeeInsurance.Remarks = obj.Remarks;
                        employeeInsurance.IsActive = obj.IsActive;
                        employeeInsurance.ModifiedBy = request.User.UserId;
                        employeeInsurance.Modified = DateTime.Now;

                        _context.EmployeeInsurances.Update(employeeInsurance);
                    }
                    else
                    {
                        employeeInsurance = new()
                        {
                            EmployeeID = obj.EmployeeID,
                            InsuranceTypeCode = obj.InsuranceTypeCode,
                            InsuranceProviderCode = obj.InsuranceProviderCode,
                            InsuranceClassCode = obj.InsuranceClassCode,
                            PolicyName = obj.PolicyName,
                            PolicyNumber = obj.PolicyNumber,
                            PolicyHolder = obj.PolicyHolder,
                            PolicyStartDate = obj.PolicyStartDate,
                            PolicyExpiryDate = obj.PolicyExpiryDate,
                            PremiumPerYear = obj.PremiumPerYear,
                            Remarks = obj.Remarks,
                            IsActive = obj.IsActive,
                            CreatedBy = request.User.UserId,
                            Created = DateTime.Now,
                        };
                        await _context.EmployeeInsurances.AddAsync(employeeInsurance);
                    }
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    Log.Info("----Info CreateUpdateEmployeeInsurance method Exit----");
                    return ApiMessageInfo.Status(1, employeeInsurance.Id);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateUpdateEmployeeInsurance Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return ApiMessageInfo.Status(0);
                }
            }
        }
    }

    #endregion

    #region DeleteEmployeeInsurance
    public class DeleteEmployeeInsurance : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteEmployeeInsuranceHandler : IRequestHandler<DeleteEmployeeInsurance, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteEmployeeInsuranceHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteEmployeeInsurance request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info DeleteEmployeeInsurance method start----");
                if (request.Id > 0)
                {
                    var employeeDependent = await _context.EmployeeInsurances
                        .FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(employeeDependent);
                    await _context.SaveChangesAsync();
                    Log.Info("----Info DeleteEmployeeInsurance method end----");
                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {
                Log.Error("Error in DeleteEmployeeInsurance Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }
    }

    #endregion
}
