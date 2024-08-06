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

    public class GetEmployeeVisaList : IRequest<PaginatedList<TblHRMTrnEmployeeVisaInfoDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetEmployeeVisaListHandler : IRequestHandler<GetEmployeeVisaList, PaginatedList<TblHRMTrnEmployeeVisaInfoDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetEmployeeVisaListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<TblHRMTrnEmployeeVisaInfoDto>> Handle(GetEmployeeVisaList request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info GetEmployeeVisaList method start----");
                var search = request.Input.Query;
                bool isArab = request.User.Culture.IsArab();
                var list = await (from employeeVisa in _context.EmployeeVisas
                                  join country in _context.CountryCodes on employeeVisa.CountryCode equals country.CountryCode
                                  join visaType in _context.VisaTypes on employeeVisa.VisaTypeCode equals visaType.VisaTypeCode
                                  select new TblHRMTrnEmployeeVisaInfoDto
                                  {
                                      Id = employeeVisa.Id,
                                      EmployeeID = employeeVisa.EmployeeID,
                                      CountryCode = employeeVisa.CountryCode,
                                      VisaTypeCode = employeeVisa.VisaTypeCode,
                                      VisaNumber = employeeVisa.VisaNumber,
                                      ValidFrom = employeeVisa.ValidFrom,
                                      ValidTo = employeeVisa.ValidTo,
                                      IssueLocation = employeeVisa.IssueLocation,
                                      CountryName = country.CountryName,
                                      VisaTypeName = isArab ? visaType.VisaTypeNameAr : visaType.VisaTypeNameEn,
                                      IsVisaValid = (DateTime.Compare(employeeVisa.ValidFrom, DateTime.Now) < 0 && DateTime.Compare(employeeVisa.ValidTo, DateTime.Now) > 0),
                                      IsActive = employeeVisa.IsActive,
                                  })
                                  .AsNoTracking()
                                  .Where(e => (e.EmployeeID == int.Parse(request.Input.Code) &&
                                  (e.CountryName.Contains(search) || e.VisaTypeName.Contains(search) || e.VisaNumber.Contains(search))))
                                  .OrderByDescending(x => x.Id)
                                  .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);
                Log.Info("----Info GetEmployeeVisaList method end----");
                return list;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetEmployeeVisaList Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region GetEmployeeVisaById

    public class GetEmployeeVisaById : IRequest<TblHRMTrnEmployeeVisaInfoDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
        public int EmployeeID { get; set; }
    }

    public class GetEmployeeVisaByIdHandler : IRequestHandler<GetEmployeeVisaById, TblHRMTrnEmployeeVisaInfoDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetEmployeeVisaByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TblHRMTrnEmployeeVisaInfoDto> Handle(GetEmployeeVisaById request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetEmployeeVisaById method start----");
            try
            {
                var employeeVisa = await _context.EmployeeVisas.AsNoTracking()
                    .ProjectTo<TblHRMTrnEmployeeVisaInfoDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(e => e.Id == request.Id && e.EmployeeID == request.EmployeeID);
                Log.Info("----Info GetEmployeeVisaById method end----");

                if (employeeVisa is not null)
                    return employeeVisa;
                else
                    return null;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetEmployeeVisaById Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region CreateUpdateEmployeeInsurance

    public class CreateUpdateEmployeeVisa : UserIdentityDto, IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblHRMTrnEmployeeVisaInfoDto Input { get; set; }
    }
    public class CreateUpdateEmployeeVisaHandler : IRequestHandler<CreateUpdateEmployeeVisa, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateEmployeeVisaHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<AppCtrollerDto> Handle(CreateUpdateEmployeeVisa request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateUpdateEmployeeVisa method start----");
                    var obj = request.Input;
                    TblHRMTrnEmployeeVisaInfo employeeVisa = new();

                    if (request.Input.Id > 0)
                    {
                        employeeVisa = await _context.EmployeeVisas
                            .FirstOrDefaultAsync(e => e.Id == request.Input.Id && e.EmployeeID == request.Input.EmployeeID);
                        employeeVisa.CountryCode = obj.CountryCode;
                        employeeVisa.VisaTypeCode = obj.VisaTypeCode;
                        employeeVisa.VisaNumber = obj.VisaNumber;
                        employeeVisa.ValidFrom = obj.ValidFrom;
                        employeeVisa.ValidTo = obj.ValidTo;
                        employeeVisa.IssueLocation = obj.IssueLocation;
                        employeeVisa.IsActive = obj.IsActive;
                        employeeVisa.ModifiedBy = request.User.UserId;
                        employeeVisa.Modified = DateTime.Now;
                        _context.EmployeeVisas.Update(employeeVisa);
                    }
                    else
                    {
                        employeeVisa = new()
                        {
                            EmployeeID = obj.EmployeeID,
                            CountryCode = obj.CountryCode,
                            VisaTypeCode = obj.VisaTypeCode,
                            VisaNumber = obj.VisaNumber,
                            ValidFrom = obj.ValidFrom,
                            ValidTo = obj.ValidTo,
                            IssueLocation = obj.IssueLocation,
                            IsActive = obj.IsActive,
                            CreatedBy = request.User.UserId,
                            Created = DateTime.Now,
                        };
                        await _context.EmployeeVisas.AddAsync(employeeVisa);
                    }
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    Log.Info("----Info CreateUpdateEmployeeVisa method Exit----");
                    return ApiMessageInfo.Status(1, employeeVisa.Id);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateUpdateEmployeeVisa Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return ApiMessageInfo.Status(0);
                }
            }
        }
    }

    #endregion

    #region DeleteEmployeeVisa
    public class DeleteEmployeeVisa : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteEmployeeVisaHandler : IRequestHandler<DeleteEmployeeVisa, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteEmployeeVisaHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteEmployeeVisa request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info DeleteEmployeeVisa method start----");
                if (request.Id > 0)
                {
                    var employeeVisa = await _context.EmployeeVisas
                        .FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(employeeVisa);
                    await _context.SaveChangesAsync();
                    Log.Info("----Info DeleteEmployeeVisa method end----");
                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {
                Log.Error("Error in DeleteEmployeeVisa Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }
    }

    #endregion
}
