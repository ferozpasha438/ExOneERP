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

    public class GetEmployeeDependentsList : IRequest<PaginatedList<TblHRMTrnEmployeeDependentInfoDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetEmployeeDependentsListHandler : IRequestHandler<GetEmployeeDependentsList, PaginatedList<TblHRMTrnEmployeeDependentInfoDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetEmployeeDependentsListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<TblHRMTrnEmployeeDependentInfoDto>> Handle(GetEmployeeDependentsList request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info GetEmployeeDependentsList method start----");
                var search = request.Input.Query;
                bool isArab = request.User.Culture.IsArab();
                var list = await (from employeeDependents in _context.EmployeeDependents
                                  join dependentType in _context.DependentTypes on employeeDependents.DependentTypeCode equals dependentType.DependentTypeCode
                                  join gender in _context.Genders on employeeDependents.GenderCode equals gender.GenderCode
                                  select new TblHRMTrnEmployeeDependentInfoDto
                                  {
                                      Id = employeeDependents.Id,
                                      EmployeeID = employeeDependents.EmployeeID,
                                      DependentTypeCode = employeeDependents.DependentTypeCode,
                                      GenderCode = employeeDependents.GenderCode,
                                      NameInIdEn = employeeDependents.NameInIdEn,
                                      NameInIdAr = employeeDependents.NameInIdAr,
                                      IDNumber = employeeDependents.IDNumber,
                                      IDExpiryDate = employeeDependents.IDExpiryDate,
                                      DateOfBirth = employeeDependents.DateOfBirth,
                                      PhoneNumber = employeeDependents.PhoneNumber,
                                      IsEmergencyNumber = employeeDependents.IsEmergencyNumber,
                                      Email = employeeDependents.Email,
                                      PassportExpiryDate = employeeDependents.PassportExpiryDate,
                                      IsEligibleForExitReEntry = employeeDependents.IsEligibleForExitReEntry,
                                      IsEligibleForAirTicket = employeeDependents.IsEligibleForAirTicket,
                                      UseEmployeeAddress = employeeDependents.UseEmployeeAddress,
                                      AddressTypeCode = employeeDependents.AddressTypeCode,
                                      Address = employeeDependents.Address,
                                      IsEligibleForSchooling = employeeDependents.IsEligibleForSchooling,
                                      IsEligibleForInsurance = employeeDependents.IsEligibleForInsurance,
                                      InsuranceClassCode = employeeDependents.InsuranceClassCode,
                                      IsActive = employeeDependents.IsActive,
                                      DependentTypeName = isArab ? dependentType.DependentTypeNameAr : dependentType.DependentTypeNameEn,
                                      Gender = isArab ? gender.GenderNameAr : gender.GenderNameEn,
                                      DependentName = isArab ? employeeDependents.NameInIdAr : employeeDependents.NameInIdEn,
                                  })
                                  .AsNoTracking()
                                  .Where(e => (e.EmployeeID == int.Parse(request.Input.Code) && (e.NameInIdEn.Contains(search) || e.NameInIdAr.Contains(search))))
                                  .OrderByDescending(x => x.Id)
                                  .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);
                Log.Info("----Info GetEmployeeDependentsList method end----");
                return list;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetEmployeeDependentsList Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region GetEmployeeDependentById

    public class GetEmployeeDependentById : IRequest<TblHRMTrnEmployeeDependentInfoDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
        public int EmployeeID { get; set; }
    }

    public class GetEmployeeDependentByIdHandler : IRequestHandler<GetEmployeeDependentById, TblHRMTrnEmployeeDependentInfoDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetEmployeeDependentByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TblHRMTrnEmployeeDependentInfoDto> Handle(GetEmployeeDependentById request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetEmployeeDependentById method start----");
            try
            {
                var employeeDependentInfo = await _context.EmployeeDependents.AsNoTracking()
                    .ProjectTo<TblHRMTrnEmployeeDependentInfoDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(e => e.Id == request.Id && e.EmployeeID == request.EmployeeID);
                Log.Info("----Info GetEmployeeDependentById method end----");

                if (employeeDependentInfo is not null)
                    return employeeDependentInfo;
                else
                    return null;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetEmployeeDependentById Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region CreateUpdateEmployeeDependent

    public class CreateUpdateEmployeeDependent : UserIdentityDto, IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblHRMTrnEmployeeDependentInfoDto Input { get; set; }
    }
    public class CreateUpdateEmployeeDependentHandler : IRequestHandler<CreateUpdateEmployeeDependent, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateEmployeeDependentHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<AppCtrollerDto> Handle(CreateUpdateEmployeeDependent request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateUpdateEmployeeDependent method start----");
                    var obj = request.Input;
                    TblHRMTrnEmployeeDependentInfo employeeDependent = new();

                    if (request.Input.Id > 0)
                    {
                        employeeDependent = await _context.EmployeeDependents
                            .FirstOrDefaultAsync(e => e.Id == request.Input.Id && e.EmployeeID == request.Input.EmployeeID);
                        employeeDependent.DependentTypeCode = obj.DependentTypeCode;
                        employeeDependent.GenderCode = obj.GenderCode;
                        employeeDependent.NameInIdEn = obj.NameInIdEn;
                        employeeDependent.NameInIdAr = obj.NameInIdAr;
                        employeeDependent.IDNumber = obj.IDNumber;
                        employeeDependent.IDExpiryDate = obj.IDExpiryDate;
                        employeeDependent.DateOfBirth = obj.DateOfBirth;
                        employeeDependent.PhoneNumber = obj.PhoneNumber;
                        employeeDependent.IsEmergencyNumber = obj.IsEmergencyNumber;
                        employeeDependent.Email = obj.Email;
                        employeeDependent.PassportExpiryDate = obj.PassportExpiryDate;
                        employeeDependent.IsEligibleForExitReEntry = obj.IsEligibleForExitReEntry;
                        employeeDependent.IsEligibleForAirTicket = obj.IsEligibleForAirTicket;
                        employeeDependent.UseEmployeeAddress = obj.UseEmployeeAddress;
                        employeeDependent.AddressTypeCode = obj.AddressTypeCode;
                        employeeDependent.Address = obj.Address;
                        employeeDependent.IsEligibleForSchooling = obj.IsEligibleForSchooling;
                        employeeDependent.IsEligibleForInsurance = obj.IsEligibleForInsurance;
                        employeeDependent.InsuranceClassCode = obj.InsuranceClassCode;
                        employeeDependent.IsActive = obj.IsActive;
                        employeeDependent.ModifiedBy = request.User.UserId;
                        employeeDependent.Modified = DateTime.Now;

                        _context.EmployeeDependents.Update(employeeDependent);
                    }
                    else
                    {
                        employeeDependent = new()
                        {
                            EmployeeID = obj.EmployeeID,
                            DependentTypeCode = obj.DependentTypeCode,
                            GenderCode = obj.GenderCode,
                            NameInIdEn = obj.NameInIdEn,
                            NameInIdAr = obj.NameInIdAr,
                            IDNumber = obj.IDNumber,
                            IDExpiryDate = obj.IDExpiryDate,
                            DateOfBirth = obj.DateOfBirth,
                            PhoneNumber = obj.PhoneNumber,
                            IsEmergencyNumber = obj.IsEmergencyNumber,
                            Email = obj.Email,
                            PassportExpiryDate = obj.PassportExpiryDate,
                            IsEligibleForExitReEntry = obj.IsEligibleForExitReEntry,
                            IsEligibleForAirTicket = obj.IsEligibleForAirTicket,
                            UseEmployeeAddress = obj.UseEmployeeAddress,
                            AddressTypeCode = obj.AddressTypeCode,
                            Address = obj.Address,
                            IsEligibleForSchooling = obj.IsEligibleForSchooling,
                            IsEligibleForInsurance = obj.IsEligibleForInsurance,
                            InsuranceClassCode = obj.InsuranceClassCode,
                            IsActive = obj.IsActive,
                            CreatedBy = request.User.UserId,
                            Created = DateTime.Now,
                        };
                        await _context.EmployeeDependents.AddAsync(employeeDependent);
                    }
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    Log.Info("----Info CreateUpdateEmployeeDependent method Exit----");
                    return ApiMessageInfo.Status(1, employeeDependent.Id);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateUpdateEmployeeDependent Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return ApiMessageInfo.Status(0);
                }
            }
        }
    }

    #endregion

    #region DeleteEmployeeDependent
    public class DeleteEmployeeDependent : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteEmployeeDependentHandler : IRequestHandler<DeleteEmployeeDependent, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteEmployeeDependentHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteEmployeeDependent request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info DeleteEmployeeDependent method start----");
                if (request.Id > 0)
                {
                    var employeeDependent = await _context.EmployeeDependents.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(employeeDependent);
                    await _context.SaveChangesAsync();
                    Log.Info("----Info DeleteEmployeeDependent method end----");
                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {
                Log.Error("Error in DeleteEmployeeDependent Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }
    }

    #endregion
}
