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

    public class GetGradeList : IRequest<PaginatedList<TblHRMSysGradeDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetGradeListHandler : IRequestHandler<GetGradeList, PaginatedList<TblHRMSysGradeDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetGradeListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<TblHRMSysGradeDto>> Handle(GetGradeList request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info GetGradeList method start----");
                var search = request.Input.Query;
                var list = await _context.Grades
                    .AsNoTracking().ProjectTo<TblHRMSysGradeDto>(_mapper.ConfigurationProvider)
                    .Where(e => (e.GradeCode.Contains(search) || e.GradeNameEn.Contains(search)))
                    .OrderByDescending(x => x.Id)
                    .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);
                Log.Info("----Info GetGradeList method end----");
                return list;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetGradeList Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region GetGradeById

    public class GetGradeById : IRequest<TblHRMSysGradeDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetGradeByIdHandler : IRequestHandler<GetGradeById, TblHRMSysGradeDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetGradeByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TblHRMSysGradeDto> Handle(GetGradeById request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetGradeById method start----");
            try
            {
                var grade = await _context.Grades.AsNoTracking()
                    .ProjectTo<TblHRMSysGradeDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(e => e.Id == request.Id);
                Log.Info("----Info GetGradeById method end----");

                if (grade is not null)
                    return grade;
                else
                    return null;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetGradeById Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region CreateUpdateGrade

    public class CreateUpdateGrade : UserIdentityDto, IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblHRMSysGradeDto Input { get; set; }
    }
    public class CreateUpdateGradeHandler : IRequestHandler<CreateUpdateGrade, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateGradeHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<AppCtrollerDto> Handle(CreateUpdateGrade request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateUpdateGrade method start----");
                    var obj = request.Input;
                    TblHRMSysGrade grade = new();

                    grade = await _context.Grades.FirstOrDefaultAsync(e => e.GradeCode == request.Input.GradeCode);

                    if (grade is not null)
                    {
                        grade.GradeNameEn = obj.GradeNameEn;
                        grade.GradeNameAr = obj.GradeNameAr;
                        grade.Id = obj.Id;
                        grade.IsActive = obj.IsActive;
                        grade.ModifiedBy = request.User.UserId;
                        grade.Modified = DateTime.Now;

                        _context.Grades.Update(grade);
                    }
                    else
                    {
                        grade = new()
                        {
                            GradeNameEn = obj.GradeNameEn,
                            GradeNameAr = obj.GradeNameAr,
                            GradeCode = obj.GradeCode,
                            IsActive = obj.IsActive,
                            CreatedBy = request.User.UserId,
                            Created = DateTime.Now,
                        };
                        await _context.Grades.AddAsync(grade);
                    }
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    Log.Info("----Info CreateUpdateGrade method Exit----");
                    return ApiMessageInfo.Status(1, grade.Id);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateUpdateGrade Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return ApiMessageInfo.Status(0);
                }
            }
        }
    }

    #endregion

    #region DeleteGrade
    public class DeleteGrade : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteGradeHandler : IRequestHandler<DeleteGrade, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteGradeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteGrade request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info DeleteGrade method start----");
                if (request.Id > 0)
                {
                    var gender = await _context.Grades.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(gender);
                    await _context.SaveChangesAsync();
                    Log.Info("----Info DeleteGrade method end----");
                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {
                Log.Error("Error in DeleteGrade Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }
    }

    #endregion

    #region GetGradeSelectListItem

    public class GetGradeSelectListItem : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetGradeSelectListItemHandler : IRequestHandler<GetGradeSelectListItem, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetGradeSelectListItemHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetGradeSelectListItem request, CancellationToken cancellationToken)
        {
            bool isArab = request.User.Culture.IsArab();
            var list = await _context.Grades.AsNoTracking().OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = isArab ? e.GradeNameAr : e.GradeNameEn, Value = e.GradeCode })
                  .ToListAsync(cancellationToken);

            return list;
        }
    }

    #endregion   
}
