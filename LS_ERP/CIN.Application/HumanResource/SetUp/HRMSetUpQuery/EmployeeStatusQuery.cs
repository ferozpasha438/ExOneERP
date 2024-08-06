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

    public class GetEmployeeStatusList : IRequest<PaginatedList<TblHRMSysEmployeeStatusDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetEmployeeStatusListHandler : IRequestHandler<GetEmployeeStatusList, PaginatedList<TblHRMSysEmployeeStatusDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetEmployeeStatusListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<TblHRMSysEmployeeStatusDto>> Handle(GetEmployeeStatusList request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info GetEmployeeStatusList method start----");
                var search = request.Input.Query;
                var list = await _context.EmployeeStatuses.AsNoTracking().ProjectTo<TblHRMSysEmployeeStatusDto>(_mapper.ConfigurationProvider)
                  .Where(e => (e.EmployeeStatusCode.Contains(search) || e.EmployeeStatusNameEn.Contains(search)))
                   .OrderByDescending(x => x.Id)
                     .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);
                Log.Info("----Info GetEmployeeStatusList method end----");
                return list;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetEmployeeStatusList Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region GetEmployeeStatusById

    public class GetEmployeeStatusById : IRequest<TblHRMSysEmployeeStatusDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetEmployeeStatusByIdHandler : IRequestHandler<GetEmployeeStatusById, TblHRMSysEmployeeStatusDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetEmployeeStatusByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TblHRMSysEmployeeStatusDto> Handle(GetEmployeeStatusById request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetEmployeeStatusById method start----");
            try
            {
                var employeeStatus = await _context.EmployeeStatuses.AsNoTracking()
                    .ProjectTo<TblHRMSysEmployeeStatusDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(e => e.Id == request.Id);
                Log.Info("----Info GetEmployeeStatusById method end----");

                if (employeeStatus is not null)
                    return employeeStatus;
                else
                    return null;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetEmployeeStatusById Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region CreateUpdateEmployeeStatus

    public class CreateUpdateEmployeeStatus : UserIdentityDto, IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblHRMSysEmployeeStatusDto Input { get; set; }
    }
    public class CreateUpdateEmployeeStatusHandler : IRequestHandler<CreateUpdateEmployeeStatus, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateEmployeeStatusHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<AppCtrollerDto> Handle(CreateUpdateEmployeeStatus request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateUpdateEmployeeStatus method start----");
                    var obj = request.Input;
                    TblHRMSysEmployeeStatus employeeStatus = new();

                    if (request.Input.Id > 0)
                    {
                        employeeStatus = await _context.EmployeeStatuses.FirstOrDefaultAsync(e => e.EmployeeStatusCode == request.Input.EmployeeStatusCode);
                        employeeStatus.EmployeeStatusNameEn = obj.EmployeeStatusNameEn;
                        employeeStatus.EmployeeStatusNameAr = obj.EmployeeStatusNameAr;
                        employeeStatus.Id = obj.Id;
                        employeeStatus.IsActive = obj.IsActive;
                        employeeStatus.ModifiedBy = request.User.UserId;
                        employeeStatus.Modified = DateTime.Now;

                        _context.EmployeeStatuses.Update(employeeStatus);
                    }
                    else
                    {
                        employeeStatus = new()
                        {
                            EmployeeStatusCode = obj.EmployeeStatusCode,
                            EmployeeStatusNameEn = obj.EmployeeStatusNameEn,
                            EmployeeStatusNameAr = obj.EmployeeStatusNameAr,
                            IsActive = obj.IsActive,
                            CreatedBy = request.User.UserId,
                            Created = DateTime.Now,
                        };
                        await _context.EmployeeStatuses.AddAsync(employeeStatus);
                    }
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    Log.Info("----Info CreateUpdateEmployeeStatus method Exit----");
                    return ApiMessageInfo.Status(1, employeeStatus.Id);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateUpdateEmployeeStatus Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return ApiMessageInfo.Status(0);
                }
            }
        }
    }

    #endregion

    #region DeleteEmployeeStatus
    public class DeleteEmployeeStatus : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteEmployeeStatusHandler : IRequestHandler<DeleteEmployeeStatus, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteEmployeeStatusHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteEmployeeStatus request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info DeleteEmployeeStatus method start----");
                if (request.Id > 0)
                {
                    var city = await _context.EmployeeStatuses.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(city);
                    await _context.SaveChangesAsync();
                    Log.Info("----Info DeleteEmployeeStatus method end----");
                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {
                Log.Error("Error in DeleteEmployeeStatus Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }
    }

    #endregion

    #region GetEmployeeStatusSelectListItem

    public class GetEmployeeStatusSelectListItem : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetEmployeeStatusSelectListItemHandler : IRequestHandler<GetEmployeeStatusSelectListItem, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetEmployeeStatusSelectListItemHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetEmployeeStatusSelectListItem request, CancellationToken cancellationToken)
        {
            bool isArab = request.User.Culture.IsArab();
            var list = await _context.EmployeeStatuses.AsNoTracking().OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = isArab ? e.EmployeeStatusNameAr : e.EmployeeStatusNameEn, Value = e.EmployeeStatusCode })
                  .ToListAsync(cancellationToken);

            return list;
        }
    }

    #endregion   
}
