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

    #region For Web Dev

    #region StudentAttendanceByStuAdmNum
    public class StudentAttendanceByStuAdmNum : IRequest<PaginatedList<TblDefStudentAttendanceDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }

    }

    public class StudentAttendanceByStuAdmNumHandler : IRequestHandler<StudentAttendanceByStuAdmNum, PaginatedList<TblDefStudentAttendanceDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public StudentAttendanceByStuAdmNumHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<TblDefStudentAttendanceDto>> Handle(StudentAttendanceByStuAdmNum request, CancellationToken cancellationToken)
        {
            var stuAttendanceList = await _context.StudentAttendance.AsNoTracking().ProjectTo<TblDefStudentAttendanceDto>(_mapper.ConfigurationProvider).Where(x=>x.StuAdmNum==request.Input.StuAdmNum).PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);
            return stuAttendanceList;
        }
    }

    #endregion

    #region Student Attandence List
    public class GetStudentAttandanceList : IRequest<List<TblDefStudentAttendanceDto>>
    {
        public UserIdentityDto User { get; set; }

    }

    public class StudentAttandanceListHandler : IRequestHandler<GetStudentAttandanceList,List<TblDefStudentAttendanceDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public StudentAttandanceListHandler(CINDBOneContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<TblDefStudentAttendanceDto>>Handle(GetStudentAttandanceList request,CancellationToken cancellationToken)
        {
            var stuAttendanceList = await _context.StudentAttendance.AsNoTracking().ProjectTo<TblDefStudentAttendanceDto>(_mapper.ConfigurationProvider).ToListAsync();
            return stuAttendanceList;
        }
    }

    #endregion

    #region Create_Update
    public class CreateUpdateStudentAttendance : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public TblDefStudentAttendanceDto Input { get; set; }
    }

    public class CreateUpdateStudentAttendanceHandler:IRequestHandler<CreateUpdateStudentAttendance, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateStudentAttendanceHandler(CINDBOneContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateUpdateStudentAttendance request,CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info Create Update Method Start-----");
                var obj = request.Input;
                TblDefStudentAttendance StuAttendance = new();
                if (obj.Id > 0)
                    StuAttendance = await _context.StudentAttendance.AsNoTracking().FirstOrDefaultAsync(e => e.Id == obj.Id);

                StuAttendance.Id = obj.Id;
                StuAttendance.StuAdmNum = obj.StuAdmNum;
                StuAttendance.AtnTimeIn = obj.AtnTimeIn;
                StuAttendance.AtnTimeOut = obj.AtnTimeOut;
                StuAttendance.AtnFlag = obj.AtnFlag;
                StuAttendance.IsLeave = obj.IsLeave;
                StuAttendance.LeaveCode = obj.LeaveCode;
                StuAttendance.AcademicYear = obj.AcademicYear;

                if (obj.Id > 0)
                {
                    _context.StudentAttendance.Update(StuAttendance);
                }
                else
                {
                    await _context.StudentAttendance.AddAsync(StuAttendance);
                }
                await _context.SaveChangesAsync();
                Log.Info("----Info Create Update Method Start---- -");
                return StuAttendance.Id;

            }
            catch (Exception ex)
            {
                Log.Error("Error in Create Update Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        
        }
    }

    #endregion

    #region Student Attandence Get By Id

    public class GetStudentAttendanceById : IRequest<TblDefStudentAttendanceDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }
    public class GetStudentAttendanceByIdHandler : IRequestHandler<GetStudentAttendanceById,TblDefStudentAttendanceDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public GetStudentAttendanceByIdHandler(CINDBOneContext context ,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TblDefStudentAttendanceDto> Handle(GetStudentAttendanceById request,CancellationToken cancellationToken)
        {
            var StudentAttendence = await _context.StudentAttendance.AsNoTracking().ProjectTo<TblDefStudentAttendanceDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.Id == request.Id);
            return StudentAttendence;
        }
    }

    #endregion

    #region Delete Student Attendance
    public class DeleteStudentAttendance : IRequest<int>
    {
      public UserIdentityDto User { get; set; }
      public int Id { get; set; }
    }

    public class DeleteStudentAttendanceHandler : IRequestHandler<DeleteStudentAttendance,int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteStudentAttendanceHandler(CINDBOneContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

        }
        public async Task<int> Handle(DeleteStudentAttendance request,CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info DeleteStudentAttendace start----");
                if (request.Id > 0) { 
                var studentAttendance = await _context.StudentAttendance.FirstOrDefaultAsync(e =>e.Id == request.Id);
                _context.Remove(studentAttendance);

                    await _context.SaveChangesAsync();
                    return request.Id;
                }
                return 0;
            }
            catch(Exception ex)
            {

                Log.Error("Error in Delete Student Attendance");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }
    }

    #endregion


    #endregion


    #region For Mobile App

    #region GetSchoolStudentAttnList
    public class GetSchoolStudentAttnList : IRequest<TblSchoolStudentAttnListDto>
    {
        public UserIdentityDto User { get; set; }
        public string StuAdmNum { get; set; }

    }

    public class GetSchoolStudentAttnListHandler : IRequestHandler<GetSchoolStudentAttnList, TblSchoolStudentAttnListDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSchoolStudentAttnListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblSchoolStudentAttnListDto> Handle(GetSchoolStudentAttnList request, CancellationToken cancellationToken)
        {

            return new TblSchoolStudentAttnListDto {
                WeekOff1 ="Fri",
                WeekOff2 ="Sat",
                StuAttnList = await _context.StudentAttendance.AsNoTracking().ProjectTo<TblDefStudentAttendanceDto>(_mapper.ConfigurationProvider).Where(e=>e.StuAdmNum==request.StuAdmNum).ToListAsync(),
                StuLeaveList=await _context.StudentApplyLeave.AsNoTracking().ProjectTo<TblDefStudentApplyLeaveDto>(_mapper.ConfigurationProvider).Where(e=>e.StuAdmNum==request.StuAdmNum).ToListAsync(),
                StuHolidayList=await _context.StudentHolidayClaender.AsNoTracking().ProjectTo<TblSysSchoolHolidayCalanderStudentDto>(_mapper.ConfigurationProvider).ToListAsync()
            } ;
        }


    }

    #endregion

    #endregion

}
