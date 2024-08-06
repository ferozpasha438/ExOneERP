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

    public class GetDepartmentList : IRequest<PaginatedList<TblHRMSysDepartmentDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetDepartmentListHandler : IRequestHandler<GetDepartmentList, PaginatedList<TblHRMSysDepartmentDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetDepartmentListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<TblHRMSysDepartmentDto>> Handle(GetDepartmentList request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info GetDepartmentList method start----");
                var search = request.Input.Query;
                bool isArab = request.User.Culture.IsArab();

                var list = await (from department in _context.Departments
                                  join division in _context.Divisions on department.DivisionCode equals division.DivisionCode
                                  select new TblHRMSysDepartmentDto
                                  {
                                      Id = department.Id,
                                      DepartmentCode = department.DepartmentCode,
                                      DepartmentNameEn = department.DepartmentNameEn,
                                      DepartmentNameAr = department.DepartmentNameAr,
                                      DivisionCode = department.DivisionCode,
                                      DivisionName = isArab ? division.DivisionNameAr : division.DivisionNameEn,
                                      IsActive = department.IsActive
                                  })
                  .AsNoTracking()
                  .Where(e => (e.DepartmentCode.Contains(search) || e.DepartmentNameEn.Contains(search)))
                  .OrderBy(x => x.Id)
                  .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

                Log.Info("----Info GetDepartmentList method end----");
                return list;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetDepartmentList Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region GetDepartmentById

    public class GetDepartmentById : IRequest<TblHRMSysDepartmentDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetDepartmentByIdHandler : IRequestHandler<GetDepartmentById, TblHRMSysDepartmentDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetDepartmentByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TblHRMSysDepartmentDto> Handle(GetDepartmentById request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetDepartmentById method start----");
            try
            {
                var department = await _context.Departments.AsNoTracking()
                    .ProjectTo<TblHRMSysDepartmentDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(e => e.Id == request.Id);
                Log.Info("----Info GetDepartmentById method end----");

                if (department is not null)
                    return department;
                else
                    return null;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetDepartmentById Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region CreateUpdateDepartment

    public class CreateUpdateDepartment : UserIdentityDto, IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblHRMSysDepartmentDto Input { get; set; }
    }
    public class CreateUpdateDepartmentHandler : IRequestHandler<CreateUpdateDepartment, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateDepartmentHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<AppCtrollerDto> Handle(CreateUpdateDepartment request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateUpdateDepartment method start----");
                    var obj = request.Input;
                    TblHRMSysDepartment department = new();

                    if (request.Input.Id > 0)
                    {
                        department = await _context.Departments.FirstOrDefaultAsync(e => e.DivisionCode == request.Input.DivisionCode);
                        department.DepartmentNameEn = obj.DepartmentNameEn;
                        department.DepartmentNameAr = obj.DepartmentNameAr;
                        department.DivisionCode = obj.DivisionCode;
                        department.Id = obj.Id;
                        department.IsActive = obj.IsActive;
                        department.ModifiedBy = request.User.UserId;
                        department.Modified = DateTime.Now;

                        _context.Departments.Update(department);
                    }
                    else
                    {
                        department = new()
                        {
                            DepartmentCode = obj.DepartmentCode,
                            DepartmentNameEn = obj.DepartmentNameEn,
                            DepartmentNameAr = obj.DepartmentNameAr,
                            DivisionCode = obj.DivisionCode,
                            IsActive = obj.IsActive,
                            CreatedBy = request.User.UserId,
                            Created = DateTime.Now,
                        };
                        await _context.Departments.AddAsync(department);
                    }
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    Log.Info("----Info CreateUpdateDepartment method Exit----");
                    return ApiMessageInfo.Status(1, department.Id);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateUpdateDepartment Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return ApiMessageInfo.Status(0);
                }
            }
        }
    }

    #endregion

    #region DeleteDepartment
    public class DeleteDepartment : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteDepartmentHandler : IRequestHandler<DeleteDepartment, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteDepartmentHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteDepartment request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info DeleteDepartment method start----");
                if (request.Id > 0)
                {
                    var city = await _context.Departments.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(city);
                    await _context.SaveChangesAsync();
                    Log.Info("----Info DeleteDepartment method end----");
                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {
                Log.Error("Error in DeleteDepartment Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }
    }

    #endregion

    #region GetDepartmentSelectListItem

    public class GetDepartmentSelectListItem : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetDepartmentSelectListItemHandler : IRequestHandler<GetDepartmentSelectListItem, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetDepartmentSelectListItemHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetDepartmentSelectListItem request, CancellationToken cancellationToken)
        {
            bool isArab = request.User.Culture.IsArab();
            var list = await _context.Departments.AsNoTracking().OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = isArab ? e.DepartmentNameAr : e.DepartmentNameEn, Value = e.DivisionCode })
                  .ToListAsync(cancellationToken);

            return list;
        }
    }

    #endregion   
}
