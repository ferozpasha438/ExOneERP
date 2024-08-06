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

    public class GetEmployeeTypeList : IRequest<PaginatedList<TblHRMSysEmployeeTypeDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetEmployeeTypeListHandler : IRequestHandler<GetEmployeeTypeList, PaginatedList<TblHRMSysEmployeeTypeDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetEmployeeTypeListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<TblHRMSysEmployeeTypeDto>> Handle(GetEmployeeTypeList request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info GetEmployeeTypeList method start----");
                var search = request.Input.Query;
                var list = await _context.EmployeeTypes.AsNoTracking().ProjectTo<TblHRMSysEmployeeTypeDto>(_mapper.ConfigurationProvider)
                  .Where(e => (e.EmployeeTypeCode.Contains(search) || e.EmployeeTypeNameEn.Contains(search)))
                   .OrderByDescending(x => x.Id)
                     .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);
                Log.Info("----Info GetEmployeeTypeList method end----");
                return list;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetEmployeeTypeList Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region GetEmployeeTypeById

    public class GetEmployeeTypeById : IRequest<TblHRMSysEmployeeTypeDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetEmployeeTypeByIdHandler : IRequestHandler<GetEmployeeTypeById, TblHRMSysEmployeeTypeDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetEmployeeTypeByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TblHRMSysEmployeeTypeDto> Handle(GetEmployeeTypeById request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetEmployeeTypeById method start----");
            try
            {
                var employeeType = await _context.EmployeeTypes.AsNoTracking()
                    .ProjectTo<TblHRMSysEmployeeTypeDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(e => e.Id == request.Id);
                Log.Info("----Info GetEmployeeTypeById method end----");

                if (employeeType is not null)
                    return employeeType;
                else
                    return null;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetEmployeeTypeById Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region CreateUpdateEmployeeType

    public class CreateUpdateEmployeeType : UserIdentityDto, IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblHRMSysEmployeeTypeDto Input { get; set; }
    }
    public class CreateUpdateEmployeeTypeHandler : IRequestHandler<CreateUpdateEmployeeType, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateEmployeeTypeHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<AppCtrollerDto> Handle(CreateUpdateEmployeeType request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateUpdateEmployeeType method start----");
                    var obj = request.Input;
                    TblHRMSysEmployeeType employeeType = new();

                    employeeType = await _context.EmployeeTypes.FirstOrDefaultAsync(e => e.EmployeeTypeCode == request.Input.EmployeeTypeCode);

                    if (employeeType is not null)
                    {
                        employeeType.EmployeeTypeNameEn = obj.EmployeeTypeNameEn;
                        employeeType.EmployeeTypeNameAr = obj.EmployeeTypeNameAr;
                        employeeType.Id = obj.Id;
                        employeeType.IsActive = obj.IsActive;
                        employeeType.ModifiedBy = request.User.UserId;
                        employeeType.Modified = DateTime.Now;

                        _context.EmployeeTypes.Update(employeeType);
                    }
                    else
                    {
                        employeeType = new()
                        {
                            EmployeeTypeNameEn = obj.EmployeeTypeNameEn,
                            EmployeeTypeNameAr = obj.EmployeeTypeNameAr,
                            EmployeeTypeCode = obj.EmployeeTypeCode,
                            IsActive = obj.IsActive,
                            CreatedBy = request.User.UserId,
                            Created = DateTime.Now,
                        };
                        await _context.EmployeeTypes.AddAsync(employeeType);
                    }
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    Log.Info("----Info CreateUpdateEmployeeType method Exit----");
                    return ApiMessageInfo.Status(1, employeeType.Id);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateUpdateEmployeeType Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return ApiMessageInfo.Status(0);
                }
            }
        }
    }

    #endregion

    #region DeleteEmployeeType
    public class DeleteEmployeeType : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteEmployeeTypeHandler : IRequestHandler<DeleteEmployeeType, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteEmployeeTypeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteEmployeeType request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info DeleteEmployeeType method start----");
                if (request.Id > 0)
                {
                    var city = await _context.EmployeeTypes.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(city);
                    await _context.SaveChangesAsync();
                    Log.Info("----Info DeleteEmployeeType method end----");
                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {
                Log.Error("Error in DeleteEmployeeType Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }
    }

    #endregion

    #region GetEmployeeTypeSelectListItem
    public class GetEmployeeTypeSelectListItem : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetEmployeeTypeSelectListItemHandler : IRequestHandler<GetEmployeeTypeSelectListItem, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetEmployeeTypeSelectListItemHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetEmployeeTypeSelectListItem request, CancellationToken cancellationToken)
        {
            bool isArab = request.User.Culture.IsArab();
            var list = await _context.EmployeeTypes.AsNoTracking().OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = isArab ? e.EmployeeTypeNameAr : e.EmployeeTypeNameEn, Value = e.EmployeeTypeCode })
                  .ToListAsync(cancellationToken);

            return list;
        }
    }

    #endregion   
}
