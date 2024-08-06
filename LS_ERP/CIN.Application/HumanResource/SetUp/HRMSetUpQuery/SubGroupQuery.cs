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

    public class GetSubGroupList : IRequest<PaginatedList<TblHRMSysSubGroupDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetSubGroupListHandler : IRequestHandler<GetSubGroupList, PaginatedList<TblHRMSysSubGroupDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSubGroupListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<TblHRMSysSubGroupDto>> Handle(GetSubGroupList request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info GetSubGroupList method start----");
                var search = request.Input.Query;
                var list = await (from subGroup in _context.SubGroups
                                  join grp in _context.Groups on subGroup.GroupCode equals grp.GroupCode
                                  select new TblHRMSysSubGroupDto
                                  {
                                      SubGroupCode = subGroup.SubGroupCode,
                                      SubGroupNameEn = subGroup.SubGroupNameEn,
                                      SubGroupNameAr = subGroup.SubGroupNameAr,
                                      GroupNameEn = grp.GroupNameEn,
                                      GroupNameAr = grp.GroupNameAr,
                                      GroupCode = subGroup.GroupCode,
                                      Id = subGroup.Id,
                                      IsActive = subGroup.IsActive
                                  })
                                  .AsNoTracking()
                                  .Where(e => (e.SubGroupCode.Contains(search) || e.SubGroupNameEn.Contains(search)))
                                  .OrderBy(x => x.SubGroupNameEn)
                                  .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

                Log.Info("----Info GetSubGroupList method end----");
                return list;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetSubGroupList Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region GetSubGroupById

    public class GetSubGroupById : IRequest<TblHRMSysSubGroupDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetSubGroupByIdHandler : IRequestHandler<GetSubGroupById, TblHRMSysSubGroupDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSubGroupByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TblHRMSysSubGroupDto> Handle(GetSubGroupById request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetSubGroupById method start----");
            try
            {
                var subGrp = await (from subGroup in _context.SubGroups
                                      join grp in _context.Groups on subGroup.GroupCode equals grp.GroupCode
                                      select new TblHRMSysSubGroupDto
                                      {
                                          Id = subGroup.Id,
                                          SubGroupCode = subGroup.SubGroupCode,
                                          SubGroupNameEn = subGroup.SubGroupNameEn,
                                          SubGroupNameAr = subGroup.SubGroupNameAr,
                                          GroupCode = subGroup.GroupCode,
                                          ReligionCode = grp.ReligionCode,
                                          Created = subGroup.Created,
                                          CreatedBy = subGroup.CreatedBy,
                                          Modified = subGroup.Modified,
                                          ModifiedBy = subGroup.ModifiedBy,
                                          IsActive = subGroup.IsActive,
                                      }).AsNoTracking()
                                  .FirstOrDefaultAsync(e => (e.Id == request.Id));

                Log.Info("----Info GetSubGroupById method end----");

                if (subGrp is not null)
                    return subGrp;
                else
                    return null;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetSubGroupById Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region CreateUpdateSubGroup

    public class CreateUpdateSubGroup : UserIdentityDto, IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblHRMSysSubGroupDto Input { get; set; }
    }
    public class CreateUpdateSubGroupHandler : IRequestHandler<CreateUpdateSubGroup, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateSubGroupHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<AppCtrollerDto> Handle(CreateUpdateSubGroup request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateUpdateSubGroup method start----");
                    var obj = request.Input;
                    TblHRMSysSubGroup subGroup = new();

                    //Check if SubGroup with Code already exists against the group in the database.
                    subGroup = await _context.SubGroups.FirstOrDefaultAsync(e => (e.SubGroupCode == request.Input.SubGroupCode));

                    if (subGroup is not null)
                    {
                        subGroup.SubGroupNameEn = obj.SubGroupNameEn;
                        subGroup.SubGroupNameAr = obj.SubGroupNameAr;
                        subGroup.SubGroupCode = obj.SubGroupCode;
                        subGroup.GroupCode = obj.GroupCode;
                        subGroup.Id = obj.Id;
                        subGroup.IsActive = obj.IsActive;
                        subGroup.ModifiedBy = request.User.UserId;
                        subGroup.Modified = DateTime.Now;

                        _context.SubGroups.Update(subGroup);
                    }
                    else
                    {
                        subGroup = new()
                        {
                            SubGroupNameEn = obj.SubGroupNameEn,
                            SubGroupNameAr = obj.SubGroupNameAr,
                            SubGroupCode = obj.SubGroupCode,
                            GroupCode = obj.GroupCode,
                            IsActive = obj.IsActive,
                            CreatedBy = request.User.UserId,
                            Created = DateTime.Now,
                        };
                        await _context.SubGroups.AddAsync(subGroup);
                    }
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    Log.Info("----Info CreateUpdateSubGroup method Exit----");
                    return ApiMessageInfo.Status(1, subGroup.Id);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateUpdateSubGroup Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return ApiMessageInfo.Status(0);
                }
            }
        }
    }

    #endregion

    #region DeleteSubGroup
    public class DeleteSubGroup : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteSubGroupHandler : IRequestHandler<DeleteSubGroup, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteSubGroupHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteSubGroup request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info DeleteSubGroup method start----");
                if (request.Id > 0)
                {
                    var city = await _context.SubGroups.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(city);
                    await _context.SaveChangesAsync();
                    Log.Info("----Info DeleteSubGroup method end----");
                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {
                Log.Error("Error in DeleteSubGroup Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }
    }

    #endregion

    #region GetSubGroupSelectListItem

    public class GetSubGroupSelectListItem : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string GroupCode { get; set; }
    }

    public class GetSubGroupSelectListItemHandler : IRequestHandler<GetSubGroupSelectListItem, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSubGroupSelectListItemHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetSubGroupSelectListItem request, CancellationToken cancellationToken)
        {
            bool isArab = request.User.Culture.IsArab();
            var search = request.GroupCode;
            var list = await _context.SubGroups.Where(e => e.GroupCode.Contains(search)).AsNoTracking().OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = isArab ? e.SubGroupNameAr : e.SubGroupNameEn, Value = e.SubGroupCode })
                  .ToListAsync(cancellationToken);

            return list;
        }
    }

    #endregion   
}
