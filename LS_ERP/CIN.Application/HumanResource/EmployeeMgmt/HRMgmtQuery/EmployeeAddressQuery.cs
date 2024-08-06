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

    public class GetEmployeeAddressList : IRequest<PaginatedList<TblHRMTrnEmployeeAddressDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetEmployeeAddressListHandler : IRequestHandler<GetEmployeeAddressList, PaginatedList<TblHRMTrnEmployeeAddressDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetEmployeeAddressListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<TblHRMTrnEmployeeAddressDto>> Handle(GetEmployeeAddressList request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info GetEmployeeAddressList method start----");
                var search = request.Input.Query;
                bool isArab = request.User.Culture.IsArab();
                var list = await (from employeeAddress in _context.EmployeeAddress
                                  join addressTypes in _context.AddressTypes on employeeAddress.AddressTypeCode equals addressTypes.AddressTypeCode
                                  join country in _context.CountryCodes on employeeAddress.CountryCode equals country.CountryCode
                                  join zone in _context.ZoneSettings on employeeAddress.ZoneCode equals zone.Code
                                  join state in _context.StateCodes on employeeAddress.StateCode equals state.StateCode
                                  join city in _context.CityCodes on employeeAddress.CityCode equals city.CityCode
                                  select new TblHRMTrnEmployeeAddressDto
                                  {
                                      Id = employeeAddress.Id,
                                      EmployeeID = employeeAddress.EmployeeID,
                                      AddressTypeCode = employeeAddress.AddressTypeCode,
                                      CountryCode = employeeAddress.CountryCode,
                                      ZoneCode = employeeAddress.ZoneCode,
                                      StateCode = employeeAddress.StateCode,
                                      CityCode = employeeAddress.CityCode,
                                      PostCode = employeeAddress.PostCode,
                                      BuildingNumber = employeeAddress.BuildingNumber,
                                      AdditionalNumber = employeeAddress.AdditionalNumber,
                                      UnitNumber = employeeAddress.UnitNumber,
                                      AddressTypeName = isArab ? addressTypes.AddressTypeNameAr : addressTypes.AddressTypeNameEn,
                                      CountryName = country.CountryName,
                                      StateName = state.StateName,
                                      ZoneName = isArab ? zone.Name : zone.NameAR,
                                      CityName = isArab ? city.CityName : city.CityNameAr,
                                      IsActive = employeeAddress.IsActive,
                                  })
                                  .AsNoTracking()
                                  .Where(e => (e.EmployeeID == int.Parse(request.Input.Code) &&
                                  (e.CountryName.Contains(search) || e.StateName.Contains(search) || e.ZoneName.Contains(search) || e.CityName.Contains(search) || e.PostCode.Contains(search) ||
                                  e.BuildingNumber.Contains(search) || e.AddressTypeName.Contains(search))))
                                  .OrderByDescending(x => x.Id)
                                  .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);
                Log.Info("----Info GetEmployeeAddressList method end----");
                return list;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetEmployeeAddressList Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region GetEmployeeAddressById

    public class GetEmployeeAddressById : IRequest<TblHRMTrnEmployeeAddressDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
        public int EmployeeID { get; set; }
    }

    public class GetEmployeeAddressByIdHandler : IRequestHandler<GetEmployeeAddressById, TblHRMTrnEmployeeAddressDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetEmployeeAddressByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TblHRMTrnEmployeeAddressDto> Handle(GetEmployeeAddressById request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetEmployeeAddressById method start----");
            try
            {
                var employeeDependentInfo = await _context.EmployeeAddress.AsNoTracking()
                    .ProjectTo<TblHRMTrnEmployeeAddressDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(e => e.Id == request.Id && e.EmployeeID == request.EmployeeID);
                Log.Info("----Info GetEmployeeAddressById method end----");

                if (employeeDependentInfo is not null)
                    return employeeDependentInfo;
                else
                    return null;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetEmployeeAddressById Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region CreateUpdateEmployeeAddress

    public class CreateUpdateEmployeeAddress : UserIdentityDto, IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblHRMTrnEmployeeAddressDto Input { get; set; }
    }
    public class CreateUpdateEmployeeAddressHandler : IRequestHandler<CreateUpdateEmployeeAddress, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateEmployeeAddressHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<AppCtrollerDto> Handle(CreateUpdateEmployeeAddress request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateUpdateEmployeeAddress method start----");
                    var obj = request.Input;
                    TblHRMTrnEmployeeAddress employeeAddress = new();

                    if (request.Input.Id > 0)
                    {
                        employeeAddress = await _context.EmployeeAddress
                            .FirstOrDefaultAsync(e => e.Id == request.Input.Id && e.EmployeeID == request.Input.EmployeeID);
                        employeeAddress.AddressTypeCode = obj.AddressTypeCode;
                        employeeAddress.CountryCode = obj.CountryCode;
                        employeeAddress.ZoneCode = obj.ZoneCode;
                        employeeAddress.StateCode = obj.StateCode;
                        employeeAddress.CityCode = obj.CityCode;
                        employeeAddress.PostCode = obj.PostCode;
                        employeeAddress.BuildingNumber = obj.BuildingNumber;
                        employeeAddress.AdditionalNumber = obj.AdditionalNumber;
                        employeeAddress.UnitNumber = obj.UnitNumber;
                        employeeAddress.IsActive = obj.IsActive;
                        employeeAddress.ModifiedBy = request.User.UserId;
                        employeeAddress.Modified = DateTime.Now;

                        _context.EmployeeAddress.Update(employeeAddress);
                    }
                    else
                    {
                        employeeAddress = new()
                        {
                            EmployeeID = obj.EmployeeID,
                            AddressTypeCode = obj.AddressTypeCode,
                            CountryCode = obj.CountryCode,
                            ZoneCode = obj.ZoneCode,
                            StateCode = obj.StateCode,
                            CityCode = obj.CityCode,
                            PostCode = obj.PostCode,
                            BuildingNumber = obj.BuildingNumber,
                            AdditionalNumber = obj.AdditionalNumber,
                            UnitNumber = obj.UnitNumber,
                            IsActive = obj.IsActive,
                            CreatedBy = request.User.UserId,
                            Created = DateTime.Now,
                        };
                        await _context.EmployeeAddress.AddAsync(employeeAddress);
                    }
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    Log.Info("----Info CreateUpdateEmployeeAddress method Exit----");
                    return ApiMessageInfo.Status(1, employeeAddress.Id);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateUpdateEmployeeAddress Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return ApiMessageInfo.Status(0);
                }
            }
        }
    }

    #endregion

    #region DeleteEmployeeAddress
    public class DeleteEmployeeAddress : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteEmployeeAddressHandler : IRequestHandler<DeleteEmployeeAddress, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteEmployeeAddressHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteEmployeeAddress request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info DeleteEmployeeAddress method start----");
                if (request.Id > 0)
                {
                    var employeeAddress = await _context.EmployeeAddress
                        .FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(employeeAddress);
                    await _context.SaveChangesAsync();
                    Log.Info("----Info DeleteEmployeeAddress method end----");
                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {
                Log.Error("Error in DeleteEmployeeAddress Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }
    }

    #endregion
}
