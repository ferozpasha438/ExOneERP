using AutoMapper;
using CIN.Application;
using CIN.Application.Common;
using CIN.Application.SchoolMgtDtos;
using CIN.DB;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using System.Linq.Dynamic.Core;
using CIN.Domain.SchoolMgt;

namespace CIN.Application.SchoolMgtQuery
{
    #region GetSchoolScheduleEvents
    public class GetStudentHomeWork : IRequest<List<TblStudentHomeWorkDto>>
    {
        public UserIdentityDto User { get; set; }
        public string Grade { get; set; }
        public DateTime HomeworkDate { get; set; }

    }

    public class GetStudentHomeWorkHandler : IRequestHandler<GetStudentHomeWork, List<TblStudentHomeWorkDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetStudentHomeWorkHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<TblStudentHomeWorkDto>> Handle(GetStudentHomeWork request, CancellationToken cancellationToken)
        {

            var studentHomeWork = await _context.StudentHomeWork.AsNoTracking().ProjectTo<TblStudentHomeWorkDto>(_mapper.ConfigurationProvider).Where(e => e.HomeworkDate.Date == request.HomeworkDate && e.GradeCode == request.Grade).ToListAsync();

            return studentHomeWork;
        }


    }

    #endregion

    #region GetSchoolStudentHomeworkList
    public class GetSchoolStudentHomeworkList : IRequest<PaginatedList<TblStudentHomeWorkDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetSchoolStudentHomeworkListHandler : IRequestHandler<GetSchoolStudentHomeworkList, PaginatedList<TblStudentHomeWorkDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSchoolStudentHomeworkListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<TblStudentHomeWorkDto>> Handle(GetSchoolStudentHomeworkList request, CancellationToken cancellationToken)
        {
            try
            {
                if (!string.IsNullOrEmpty(request.Input.TeacherCode))
                {
                    var list = await _context.StudentHomeWork.AsNoTracking().ProjectTo<TblStudentHomeWorkDto>(_mapper.ConfigurationProvider)
                                        .Where(x => x.TeacherCode == request.Input.TeacherCode)
                                       .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);
                    return list;
                }
                else
                {
                    var list = await _context.StudentHomeWork.AsNoTracking().ProjectTo<TblStudentHomeWorkDto>(_mapper.ConfigurationProvider)
                                      .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);
                    return list;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }

    #endregion

    #region InsertANDUpdateStudentHomework
    public class InsertANDUpdateStudentHomework : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public TblStudentHomeWorkDto StudentHomeWorkDto { get; set; }
    }

    public class InsertANDUpdateStudentHomeworkHandler : IRequestHandler<InsertANDUpdateStudentHomework, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public InsertANDUpdateStudentHomeworkHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(InsertANDUpdateStudentHomework request, CancellationToken cancellationToken)
        {
            Log.Info("----Info InsertANDUpdateStudentHomework method start----");
            TblStudentHomeWork StudentHomeWork = new();
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var obj = request.StudentHomeWorkDto;
                    if (obj.Id > 0)
                        StudentHomeWork = await _context.StudentHomeWork.AsNoTracking().FirstOrDefaultAsync(e => e.Id == obj.Id);
                    StudentHomeWork.Id = obj.Id;
                    StudentHomeWork.TeacherCode = obj.TeacherCode;
                    StudentHomeWork.HomeworkDate = obj.HomeworkDate;
                    StudentHomeWork.GradeCode = obj.GradeCode;
                    StudentHomeWork.SubCodes = obj.SubCodes;
                    StudentHomeWork.HomeWorkDescription = obj.HomeWorkDescription;
                    StudentHomeWork.HomeWorkDescription_Ar = obj.HomeWorkDescription_Ar;
                    StudentHomeWork.Remarks = obj.Remarks;
                    StudentHomeWork.IsActive = obj.IsActive;
                    StudentHomeWork.CreatedOn = DateTime.Now;
                    StudentHomeWork.CreatedBy = Convert.ToString(request.User.UserId);
                    if (obj.Id > 0)
                    {
                        _context.StudentHomeWork.Update(StudentHomeWork);
                    }
                    else
                    {
                        await _context.StudentHomeWork.AddAsync(StudentHomeWork);
                    }
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    Log.Info("----Info InsertANDUpdateStudentHomework method Exit----");
                    return StudentHomeWork.Id;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in InsertANDUpdateStudentHomework Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return 0;
                }
            }
        }

    }
    #endregion

    #region GetStudentHomeWorkById
    public class GetStudentHomeWorkById : IRequest<TblStudentHomeWorkDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }

    }

    public class GetStudentHomeWorkByIdHandler : IRequestHandler<GetStudentHomeWorkById, TblStudentHomeWorkDto>
    {
        private CINDBOneContext _context;
        private IMapper _mapper;
        public GetStudentHomeWorkByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblStudentHomeWorkDto> Handle(GetStudentHomeWorkById request, CancellationToken cancellationToken)
        {
            var result = await _context.StudentHomeWork.AsNoTracking().ProjectTo<TblStudentHomeWorkDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.Id == request.Id);
            return result;
        }
    }

    #endregion
}
