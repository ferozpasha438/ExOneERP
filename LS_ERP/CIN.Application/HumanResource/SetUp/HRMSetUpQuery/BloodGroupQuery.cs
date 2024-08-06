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

    public class GetBloodGroupList : IRequest<PaginatedList<TblHRMSysBloodGroupDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetBloodGroupListHandler : IRequestHandler<GetBloodGroupList, PaginatedList<TblHRMSysBloodGroupDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetBloodGroupListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<TblHRMSysBloodGroupDto>> Handle(GetBloodGroupList request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info GetBloodGroupList method start----");
                var search = request.Input.Query;
                var list = await _context.BloodGroups.AsNoTracking().ProjectTo<TblHRMSysBloodGroupDto>(_mapper.ConfigurationProvider)
                  .Where(e => (e.BloodGroupCode.Contains(search) || e.BloodGroupNameEn.Contains(search)))
                   .OrderByDescending(x => x.Id)
                     .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);
                Log.Info("----Info GetBloodGroupList method end----");
                return list;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetBloodGroupList Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region GetBloodGroupById

    public class GetBloodGroupById : IRequest<TblHRMSysBloodGroupDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetBloodGroupByIdHandler : IRequestHandler<GetBloodGroupById, TblHRMSysBloodGroupDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetBloodGroupByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TblHRMSysBloodGroupDto> Handle(GetBloodGroupById request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetBloodGroupById method start----");
            try
            {
                var employeeType = await _context.BloodGroups.AsNoTracking()
                    .ProjectTo<TblHRMSysBloodGroupDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(e => e.Id == request.Id);
                Log.Info("----Info GetBloodGroupById method end----");

                if (employeeType is not null)
                    return employeeType;
                else
                    return null;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetBloodGroupById Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region CreateUpdateEmployeeType

    public class CreateUpdateBloodGroup : UserIdentityDto, IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblHRMSysBloodGroupDto Input { get; set; }
    }
    public class CreateUpdateBloodGroupHandler : IRequestHandler<CreateUpdateBloodGroup, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateBloodGroupHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<AppCtrollerDto> Handle(CreateUpdateBloodGroup request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateUpdateBloodGroup method start----");
                    var obj = request.Input;
                    TblHRMSysBloodGroup bloodGroup = new();

                    bloodGroup = await _context.BloodGroups.FirstOrDefaultAsync(e => e.BloodGroupCode == request.Input.BloodGroupCode);

                    if (bloodGroup is not null)
                    {
                        bloodGroup.BloodGroupNameEn = obj.BloodGroupNameEn;
                        bloodGroup.BloodGroupNameAr = obj.BloodGroupNameAr;
                        bloodGroup.Id = obj.Id;
                        bloodGroup.IsActive = obj.IsActive;
                        bloodGroup.ModifiedBy = request.User.UserId;
                        bloodGroup.Modified = DateTime.Now;

                        _context.BloodGroups.Update(bloodGroup);
                    }
                    else
                    {
                        bloodGroup = new()
                        {
                            BloodGroupNameEn = obj.BloodGroupNameEn,
                            BloodGroupNameAr = obj.BloodGroupNameAr,
                            BloodGroupCode = obj.BloodGroupCode,
                            IsActive = obj.IsActive,
                            CreatedBy = request.User.UserId,
                            Created = DateTime.Now,
                        };
                        await _context.BloodGroups.AddAsync(bloodGroup);
                    }
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    Log.Info("----Info CreateUpdateBloodGroup method Exit----");
                    return ApiMessageInfo.Status(1, bloodGroup.Id);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateUpdateBloodGroup Method");
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
    public class DeleteBloodGroup : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteBloodGroupHandler : IRequestHandler<DeleteBloodGroup, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteBloodGroupHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteBloodGroup request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info DeleteBloodGroup method start----");
                if (request.Id > 0)
                {
                    var city = await _context.BloodGroups.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(city);
                    await _context.SaveChangesAsync();
                    Log.Info("----Info DeleteBloodGroup method end----");
                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {
                Log.Error("Error in DeleteBloodGroup Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }
    }

    #endregion

    #region GetBloodGroupSelectListItem
    public class GetBloodGroupSelectListItem : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetBloodGroupSelectListItemHandler : IRequestHandler<GetBloodGroupSelectListItem, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetBloodGroupSelectListItemHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetBloodGroupSelectListItem request, CancellationToken cancellationToken)
        {
            bool isArab = request.User.Culture.IsArab();
            var list = await _context.BloodGroups.AsNoTracking().OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = isArab ? e.BloodGroupNameAr : e.BloodGroupNameEn, Value = e.BloodGroupCode })
                  .ToListAsync(cancellationToken);
            return list;
        }
    }

    #endregion   
}
