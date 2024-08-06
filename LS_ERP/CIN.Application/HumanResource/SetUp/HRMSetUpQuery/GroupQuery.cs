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

    public class GetGroupList : IRequest<PaginatedList<TblHRMSysGroupDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetGroupListHandler : IRequestHandler<GetGroupList, PaginatedList<TblHRMSysGroupDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetGroupListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<TblHRMSysGroupDto>> Handle(GetGroupList request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info GetGroupList method start----");
                var search = request.Input.Query;
                bool isArab = request.User.Culture.IsArab();
                //var list = await _context.Groups.AsNoTracking().ProjectTo<TblHRMSysGroupDto>(_mapper.ConfigurationProvider)
                //  .Where(e => (e.GroupCode.Contains(search) || e.GroupNameEn.Contains(search)))
                //   .OrderByDescending(x => x.Id)
                //     .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

                var list = await (from grp in _context.Groups
                                  join religion in _context.Religions on grp.ReligionCode equals religion.ReligionCode
                                  select new TblHRMSysGroupDto
                                  {
                                      GroupCode = grp.GroupCode,
                                      GroupNameEn = grp.GroupNameEn,
                                      GroupNameAr = grp.GroupNameAr,
                                      ReligionCode = grp.ReligionCode,
                                      ReligionName = !isArab ? religion.ReligionNameEn : religion.ReligionNameAr,
                                      Id = grp.Id,
                                      IsActive = grp.IsActive
                                  })
                  .AsNoTracking()
                  .Where(e => (e.GroupCode.Contains(search) || e.GroupNameEn.Contains(search)))
                  .OrderBy(x => x.Id)
                  .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

                Log.Info("----Info GetGroupList method end----");
                return list;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetGroupList Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region GetGroupById

    public class GetGroupById : IRequest<TblHRMSysGroupDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetGroupByIdHandler : IRequestHandler<GetGroupById, TblHRMSysGroupDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetGroupByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TblHRMSysGroupDto> Handle(GetGroupById request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetGroupById method start----");
            try
            {
                var group = await _context.Groups.AsNoTracking()
                    .ProjectTo<TblHRMSysGroupDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(e => e.Id == request.Id);
                Log.Info("----Info GetGroupById method end----");

                if (group is not null)
                    return group;
                else
                    return null;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetGroupById Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region CreateUpdateGroup

    public class CreateUpdateGroup : UserIdentityDto, IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblHRMSysGroupDto Input { get; set; }
    }
    public class CreateUpdateGroupHandler : IRequestHandler<CreateUpdateGroup, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateGroupHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<AppCtrollerDto> Handle(CreateUpdateGroup request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateUpdateGroup method start----");
                    var obj = request.Input;
                    TblHRMSysGroup group = new();

                    group = await _context.Groups.FirstOrDefaultAsync(e => e.GroupCode == request.Input.GroupCode);

                    if (group is not null)
                    {
                        group.GroupNameEn = obj.GroupNameEn;
                        group.GroupNameAr = obj.GroupNameAr;
                        group.ReligionCode = obj.ReligionCode;
                        group.Id = obj.Id;
                        group.IsActive = obj.IsActive;
                        group.ModifiedBy = request.User.UserId;
                        group.Modified = DateTime.Now;

                        _context.Groups.Update(group);
                    }
                    else
                    {
                        group = new()
                        {
                            GroupNameEn = obj.GroupNameEn,
                            GroupNameAr = obj.GroupNameAr,
                            GroupCode = obj.GroupCode,
                            ReligionCode = obj.ReligionCode,
                            IsActive = obj.IsActive,
                            CreatedBy = request.User.UserId,
                            Created = DateTime.Now,
                        };
                        await _context.Groups.AddAsync(group);
                    }
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    Log.Info("----Info CreateUpdateGroup method Exit----");
                    return ApiMessageInfo.Status(1, group.Id);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateUpdateGroup Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return ApiMessageInfo.Status(0);
                }
            }
        }
    }

    #endregion

    #region DeleteGroup
    public class DeleteGroup : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteGroupHandler : IRequestHandler<DeleteGroup, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteGroupHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteGroup request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info DeleteGroup method start----");
                if (request.Id > 0)
                {
                    var city = await _context.Groups.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(city);
                    await _context.SaveChangesAsync();
                    Log.Info("----Info DeleteGroup method end----");
                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {
                Log.Error("Error in DeleteGroup Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }
    }

    #endregion

    #region GetGroupSelectListItem
    public class GetGroupSelectListItem : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string ReligionCode { get; set; }
    }

    public class GetGroupSelectListItemHandler : IRequestHandler<GetGroupSelectListItem, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetGroupSelectListItemHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetGroupSelectListItem request, CancellationToken cancellationToken)
        {
            bool isArab = request.User.Culture.IsArab();
            var list = await _context.Groups.AsNoTracking()
                .Where(e => e.ReligionCode.Equals(request.ReligionCode))
                .OrderByDescending(e => e.Id)
                .Select(e => new CustomSelectListItem { Text = isArab ? e.GroupNameAr : e.GroupNameEn, Value = e.GroupCode })
                .ToListAsync(cancellationToken);

            return list;
        }
    }

    #endregion   
}
