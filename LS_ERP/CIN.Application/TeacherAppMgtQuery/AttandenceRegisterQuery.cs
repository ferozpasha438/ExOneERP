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
    #region AttendanceRegisterList
    public class TeacherAttendanceRegisterList : IRequest<TeacherAttendanceRegisterDto>
    {
        public UserIdentityDto User { get; set; }
        public DateTime Date { get; set; }
        public string TeacherCode { get; set; }
        public string BranchCode { get; set; }
        public string GradeCode { get; set; }
        public string SectionCode { get; set; }

    }

    public class TeacherAttendanceRegisterListHandler : IRequestHandler<TeacherAttendanceRegisterList, TeacherAttendanceRegisterDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public TeacherAttendanceRegisterListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TeacherAttendanceRegisterDto> Handle(TeacherAttendanceRegisterList request, CancellationToken cancellationToken)
        {
            var result = new TeacherAttendanceRegisterDto();
            try
            {
                result = await _context.StudentAttnRegHeader.AsNoTracking()
                                        .Where(x => x.BranchCode == request.BranchCode
                                        && x.GradeCode == request.GradeCode
                                        && x.SectionCode == request.SectionCode
                                        && x.AttnDate.Date == request.Date           
                                        && x.TeacherCode==request.TeacherCode)
                                        .Select(x => new TeacherAttendanceRegisterDto()
                                        {
                                            Id = x.Id,
                                            AttnDate = x.AttnDate,
                                            BranchCode = x.BranchCode,
                                            GradeCode = x.GradeCode,
                                            IsOpen = x.IsOpen,
                                            SectionCode = x.SectionCode,
                                            TeacherCode = x.TeacherCode
                                        })
                                        .FirstOrDefaultAsync();
                if (result == null)
                {
                    result = new();
                    result.AttnDate = request.Date;
                    result.BranchCode = request.BranchCode;
                    result.GradeCode = request.GradeCode;
                    result.SectionCode = request.SectionCode;
                    result.TeacherCode = request.TeacherCode;
                    result.IsOpen = true;
                }
                var studentList = await _context.DefSchoolStudentMaster.AsNoTracking()
                                        .Where(x => x.BranchCode == request.BranchCode && x.GradeCode == request.GradeCode && x.GradeSectionCode == request.SectionCode)
                                          .ToListAsync();
                if (studentList != null && studentList.Count() > 0)
                {
                    List<TeacherAttendanceDataDto> studentsAttnDataList = new();
                    for (int i = 0; i < studentList.Count(); i++)
                    {
                        TeacherAttendanceDataDto studentAttendanceDataDto = new();
                        studentAttendanceDataDto.StudentName = studentList[i].StuName;
                        studentAttendanceDataDto.StudentName2 = studentList[i].StuName2;
                        studentAttendanceDataDto.StudentAdmNumber = studentList[i].StuAdmNum;
                        studentAttendanceDataDto.AttnFlag = 'P';
                        studentAttendanceDataDto.IsPresent = true;
                        studentAttendanceDataDto.InTime = DateTime.Now.TimeOfDay;
                        var leaveData = await _context.StudentApplyLeave.AsNoTracking()
                                       .FirstOrDefaultAsync(x => x.StuAdmNum == studentList[i].StuAdmNum
                                       && x.IsApproved == true
                                       && x.LeaveStartDate <= DateTime.Now
                                       && x.LeaveEndDate >= DateTime.Now);
                        if (leaveData != null)
                        {
                            studentAttendanceDataDto.AttnFlag = 'L';
                            studentAttendanceDataDto.IsLeave = true;
                            studentAttendanceDataDto.IsPresent = false;
                        }
                        if (result != null && result.Id > 0)
                        {
                            var stuAttnData = await _context.StudentAttnRegDetails.AsNoTracking()
                                        .FirstOrDefaultAsync(x => x.AttnRegHeaderId == result.Id && x.StudentAdmNumber == studentList[i].StuAdmNum);
                            if (stuAttnData != null)
                            {
                                studentAttendanceDataDto.AttnFlag = stuAttnData.AttnFlag;
                                studentAttendanceDataDto.InTime = stuAttnData.InTime;
                                studentAttendanceDataDto.OutTime = stuAttnData.OutTime;
                                studentAttendanceDataDto.Remarks = stuAttnData.Remarks;
                                studentAttendanceDataDto.IsLeave = stuAttnData.IsLeave;
                                studentAttendanceDataDto.IsPresent = stuAttnData.IsPresent;
                            }
                        }
                        studentsAttnDataList.Add(studentAttendanceDataDto);
                    }
                    result.TeacherAttendanceDataList = studentsAttnDataList;
                }
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }

    #endregion

    #region CreateAttendanceRegisterData
    public class CreateAttRegisterData : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public TeacherAttendanceRegisterDto AttRegisterData { get; set; }
    }

    public class CreateAttRegisterDataHandler : IRequestHandler<CreateAttRegisterData, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateAttRegisterDataHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateAttRegisterData request, CancellationToken cancellationToken)
        {
            Log.Info("----Info CreateAttendanceRegisterData method start----");
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var obj = request.AttRegisterData;
                    TblStudentAttnRegHeader tblStudentAttnRegHeader = new();
                    if (obj.Id > 0)
                    {
                        tblStudentAttnRegHeader = await _context.StudentAttnRegHeader.AsNoTracking().FirstOrDefaultAsync(e => e.Id == obj.Id);
                    }
                    else
                    {
                        tblStudentAttnRegHeader.AttnDate = obj.AttnDate;
                        tblStudentAttnRegHeader.IsOpen = true;
                    }
                    tblStudentAttnRegHeader.Id = obj.Id;
                    tblStudentAttnRegHeader.TeacherCode = obj.TeacherCode;
                    tblStudentAttnRegHeader.BranchCode = obj.BranchCode;
                    tblStudentAttnRegHeader.GradeCode = obj.GradeCode;
                    tblStudentAttnRegHeader.SectionCode = obj.SectionCode;

                    if (obj.Id > 0)
                    {
                        tblStudentAttnRegHeader.UpdatedDate = DateTime.Now;
                        tblStudentAttnRegHeader.UpdatedBy = Convert.ToString(request.User.UserId);
                        _context.StudentAttnRegHeader.Update(tblStudentAttnRegHeader);
                    }
                    else
                    {
                        tblStudentAttnRegHeader.CreatedDate = DateTime.Now;
                        tblStudentAttnRegHeader.CreatedBy = Convert.ToString(request.User.UserId);
                        await _context.StudentAttnRegHeader.AddAsync(tblStudentAttnRegHeader);
                    }
                    await _context.SaveChangesAsync();
                    if (tblStudentAttnRegHeader.Id > 0)
                    {
                        foreach (var item in obj.TeacherAttendanceDataList)
                        {
                            TblStudentAttnRegDetails tblStudentAttnRegDetails = new();
                            tblStudentAttnRegDetails = await _context.StudentAttnRegDetails.AsNoTracking().FirstOrDefaultAsync(e => e.AttnRegHeaderId == obj.Id && e.StudentAdmNumber == item.StudentAdmNumber);
                            if (tblStudentAttnRegDetails == null)
                            {
                                tblStudentAttnRegDetails = new();
                                tblStudentAttnRegDetails.AttnRegHeaderId = tblStudentAttnRegHeader.Id;
                                tblStudentAttnRegDetails.StudentAdmNumber = item.StudentAdmNumber;
                                tblStudentAttnRegDetails.InTime = DateTime.Now.TimeOfDay;
                            }
                            tblStudentAttnRegDetails.AttnFlag = item.AttnFlag.ToString().ToUpper().ToCharArray()[0];
                            tblStudentAttnRegDetails.Remarks = item.Remarks;
                            tblStudentAttnRegDetails.IsPresent = item.AttnFlag.ToString().ToUpper() == "P" ? true : false;
                            tblStudentAttnRegDetails.IsLeave = item.IsLeave;
                            if (tblStudentAttnRegDetails != null && tblStudentAttnRegDetails.Id > 0)
                                _context.StudentAttnRegDetails.Update(tblStudentAttnRegDetails);
                            else
                                await _context.StudentAttnRegDetails.AddAsync(tblStudentAttnRegDetails);
                            await _context.SaveChangesAsync();
                        }
                    }
                    await transaction.CommitAsync();
                    Log.Info("----Info CreateAttendanceRegisterData method Exit----");
                    return tblStudentAttnRegHeader.Id;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateAttendanceRegisterData Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return 0;
                }
            }
        }

    }



    #endregion

    #region CloseAttendance
    public class CloseAttendance : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public TeacherAttendanceRegisterDto AttRegisterData { get; set; }
    }

    public class CloseAttendanceHandler : IRequestHandler<CloseAttendance, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CloseAttendanceHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(CloseAttendance request, CancellationToken cancellationToken)
        {
            Log.Info("----Info Close Attendance method start----");
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var CurrentDateTime = DateTime.Now;
                    var obj = request.AttRegisterData;
                    TblStudentAttnRegHeader tblStudentAttnRegHeader = new();
                    if (obj.Id > 0)
                    {
                        tblStudentAttnRegHeader = await _context.StudentAttnRegHeader.AsNoTracking().FirstOrDefaultAsync(e => e.Id == obj.Id);
                        tblStudentAttnRegHeader.IsOpen = false;
                        tblStudentAttnRegHeader.UpdatedDate = CurrentDateTime;
                        tblStudentAttnRegHeader.UpdatedBy = Convert.ToString(request.User.UserId);
                        _context.StudentAttnRegHeader.Update(tblStudentAttnRegHeader);
                        await _context.SaveChangesAsync();
                        int? batchYear = await _context.SysSchoolAcademicBatches.AsNoTracking().OrderBy(x => x.Id).Select(x => x.AcademicYear).FirstOrDefaultAsync();
                        foreach (var item in obj.TeacherAttendanceDataList)
                        {
                            TblStudentAttnRegDetails tblStudentAttnRegDetails = new();
                            tblStudentAttnRegDetails = await _context.StudentAttnRegDetails.AsNoTracking().FirstOrDefaultAsync(e => e.AttnRegHeaderId == obj.Id && e.StudentAdmNumber == item.StudentAdmNumber);
                            if (tblStudentAttnRegDetails != null && tblStudentAttnRegDetails.Id > 0)
                            {
                                tblStudentAttnRegDetails.OutTime = CurrentDateTime.TimeOfDay;
                                _context.StudentAttnRegDetails.Update(tblStudentAttnRegDetails);
                                await _context.SaveChangesAsync();
                            }
                            TblDefStudentAttendance tblDefStudentAttendance = new();
                            tblDefStudentAttendance.StuAdmNum = tblStudentAttnRegDetails.StudentAdmNumber;
                            tblDefStudentAttendance.AtnDate = tblStudentAttnRegHeader.AttnDate;
                            tblDefStudentAttendance.AtnTimeIn = DateTime.Now.Date + tblStudentAttnRegDetails.InTime;
                            tblDefStudentAttendance.AtnTimeOut = CurrentDateTime;
                            tblDefStudentAttendance.AtnFlag = Convert.ToString(tblStudentAttnRegDetails.AttnFlag);
                            tblDefStudentAttendance.AcademicYear = batchYear ?? 0;
                            await _context.StudentAttendance.AddAsync(tblDefStudentAttendance);
                            await _context.SaveChangesAsync();
                        }
                    }
                    await transaction.CommitAsync();
                    Log.Info("----Info Close Attendance method Exit----");
                    return tblStudentAttnRegHeader.Id;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in Close Attendance Method");
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
