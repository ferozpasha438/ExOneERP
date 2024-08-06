using AutoMapper;
using CIN.Application;
using CIN.Application.Common;
using CIN.Application.TeacherMgtDtos;
using CIN.Application.TeacherAppMgtQuery;
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

namespace CIN.Application.TeacherAppMgtQuery
{
    #region Get Teacher Homework List

    public class GetTeacherHomeworkList : IRequest<List<TeacherStudentHomeWorkDto>>
    {
        public UserIdentityDto User { get; set; }
        public string TeacherCode { get; set; }
        public string GradeCode { get; set; }
        public DateTime Date { get; set; }

    }


    public class GetTeacherHomeworkListHandler : IRequestHandler<GetTeacherHomeworkList, List<TeacherStudentHomeWorkDto>>
    {
        private CINDBOneContext _context;
        private IMapper _mapper;

        public GetTeacherHomeworkListHandler(CINDBOneContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<TeacherStudentHomeWorkDto>> Handle(GetTeacherHomeworkList request,CancellationToken cancellationToken)
        {
            var list = await _context.StudentHomeWork.AsNoTracking().ProjectTo<TeacherStudentHomeWorkDto>(_mapper.ConfigurationProvider).Where(e => (e.TeacherCode == request.TeacherCode && e.GradeCode == request.GradeCode) && e.HomeworkDate.Date == request.Date).ToListAsync();
            return list;
        }
    }

    #endregion

    #region InsertUpdateTeacherHomework
    public class InsertUpdateTeacherHomework : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public TeacherStudentHomeWorkDto TeacherHomeWorkDto { get; set; }
    }

    public class InsertUpdateTeacherHomeworkHandler : IRequestHandler<InsertUpdateTeacherHomework, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public InsertUpdateTeacherHomeworkHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(InsertUpdateTeacherHomework request, CancellationToken cancellationToken)
        {
            Log.Info("----Info InsertUpdateTeacherHomework method start----");
            TblStudentHomeWork StudentHomeWork = new();
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var obj = request.TeacherHomeWorkDto;
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
                    Log.Info("----Info InsertUpdateTeacherHomework method Exit----");
                    return StudentHomeWork.Id;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in InsertUpdateTeacherHomework Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return 0;
                }
            }
        }

    }
    #endregion
}
