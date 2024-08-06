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

    public class GetCourseTypeList : IRequest<PaginatedList<TblHRMSysCourseTypeDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetCourseTypeListHandler : IRequestHandler<GetCourseTypeList, PaginatedList<TblHRMSysCourseTypeDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetCourseTypeListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<TblHRMSysCourseTypeDto>> Handle(GetCourseTypeList request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info GetCourseTypeList method start----");
                var search = request.Input.Query;

                var list = await _context.CourseTypes.AsNoTracking()
                    .ProjectTo<TblHRMSysCourseTypeDto>(_mapper.ConfigurationProvider)
                    .Where(e => (e.CourseTypeCode.Contains(search) || e.CourseTypeNameEn.Contains(search)))
                    .OrderByDescending(x => x.Id)
                    .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

                Log.Info("----Info GetCourseTypeList method end----");
                return list;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetCourseTypeList Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region GetCourseTypeById

    public class GetCourseTypeById : IRequest<TblHRMSysCourseTypeDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetCourseTypeByIdHandler : IRequestHandler<GetCourseTypeById, TblHRMSysCourseTypeDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetCourseTypeByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TblHRMSysCourseTypeDto> Handle(GetCourseTypeById request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetCourseTypeById method start----");
            try
            {
                var courseType = await _context.CourseTypes.AsNoTracking()
                    .ProjectTo<TblHRMSysCourseTypeDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(e => e.Id == request.Id);
                Log.Info("----Info GetCourseTypeById method end----");

                if (courseType is not null)
                    return courseType;
                else
                    return null;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetCourseTypeById Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region CreateUpdateCourseType

    public class CreateUpdateCourseType : UserIdentityDto, IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblHRMSysCourseTypeDto Input { get; set; }
    }
    public class CreateUpdateCourseTypeHandler : IRequestHandler<CreateUpdateCourseType, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateCourseTypeHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<AppCtrollerDto> Handle(CreateUpdateCourseType request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateUpdateCourseType method start----");
                    var obj = request.Input;
                    TblHRMSysCourseType courseType = new();

                    courseType = await _context.CourseTypes.FirstOrDefaultAsync(e => e.CourseTypeCode == request.Input.CourseTypeCode);

                    if (courseType is not null)
                    {
                        courseType.CourseTypeNameEn = obj.CourseTypeNameEn;
                        courseType.CourseTypeNameAr = obj.CourseTypeNameAr;
                        courseType.Id = obj.Id;
                        courseType.IsActive = obj.IsActive;
                        courseType.ModifiedBy = request.User.UserId;
                        courseType.Modified = DateTime.Now;

                        _context.CourseTypes.Update(courseType);
                    }
                    else
                    {
                        courseType = new()
                        {
                            CourseTypeCode = obj.CourseTypeCode,
                            CourseTypeNameEn = obj.CourseTypeNameEn,
                            CourseTypeNameAr = obj.CourseTypeNameAr,
                            IsActive = obj.IsActive,
                            CreatedBy = request.User.UserId,
                            Created = DateTime.Now,
                        };
                        await _context.CourseTypes.AddAsync(courseType);
                    }
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    Log.Info("----Info CreateUpdateCourseType method Exit----");
                    return ApiMessageInfo.Status(1, courseType.Id);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateUpdateCourseType Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return ApiMessageInfo.Status(0);
                }
            }
        }
    }

    #endregion

    #region DeleteCourseType
    public class DeleteCourseType : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteCourseTypeHandler : IRequestHandler<DeleteCourseType, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteCourseTypeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteCourseType request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info DeleteCourseType method start----");
                if (request.Id > 0)
                {
                    var city = await _context.CourseTypes.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(city);
                    await _context.SaveChangesAsync();
                    Log.Info("----Info DeleteCourseType method end----");
                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {
                Log.Error("Error in DeleteCourseType Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }
    }

    #endregion

    #region GetCourseTypeSelectListItem

    public class GetCourseTypeSelectListItem : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetCourseTypeSelectListItemHandler : IRequestHandler<GetCourseTypeSelectListItem, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetCourseTypeSelectListItemHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetCourseTypeSelectListItem request, CancellationToken cancellationToken)
        {
            bool isArab = request.User.Culture.IsArab();
            var list = await _context.CourseTypes.AsNoTracking().OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = isArab ? e.CourseTypeNameAr : e.CourseTypeNameEn, Value = e.CourseTypeCode })
                  .ToListAsync(cancellationToken);

            return list;
        }
    }

    #endregion   
}
