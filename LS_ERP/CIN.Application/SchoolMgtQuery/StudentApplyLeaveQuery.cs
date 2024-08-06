using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CIN.Application;
using CIN.Application.Common;
using CIN.Application.SchoolMgtDtos;
using CIN.DB;
using MediatR;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;
using CIN.Domain.SchoolMgt;
using System.Threading;
using CIN.Application.SchoolMgtDto;

namespace CIN.Application.SchoolMgtQuery
{
    #region GetAllStudentApplyList 

    public class GetStudentApplyLeaveList : IRequest <List<TblDefStudentApplyLeaveDto>>
    { 
        public UserIdentityDto User { get; set; }
     
    }

    public class GetStudentApplyLeaveListHandler:IRequestHandler<GetStudentApplyLeaveList, List<TblDefStudentApplyLeaveDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public GetStudentApplyLeaveListHandler(CINDBOneContext context ,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<List<TblDefStudentApplyLeaveDto>> Handle(GetStudentApplyLeaveList request,CancellationToken cancellationToken)
        {
            var studentApplyLeave = await _context.StudentApplyLeave.AsNoTracking().ProjectTo<TblDefStudentApplyLeaveDto>(_mapper.ConfigurationProvider).ToListAsync();
            return studentApplyLeave;
        }
    }


    #endregion

    #region GetStudentLeaves 

    public class GetStudentLeaves : IRequest<PaginatedList<TblDefStudentApplyLeaveDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetStudentLeavesHandler : IRequestHandler<GetStudentLeaves, PaginatedList<TblDefStudentApplyLeaveDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public GetStudentLeavesHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<PaginatedList<TblDefStudentApplyLeaveDto>> Handle(GetStudentLeaves request, CancellationToken cancellationToken)
        {
            var studentApplyLeave = await _context.StudentApplyLeave.AsNoTracking().ProjectTo<TblDefStudentApplyLeaveDto>(_mapper.ConfigurationProvider)
                                        .Where(x => x.StuAdmNum == request.Input.StuAdmNum)
                                          .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);
            return studentApplyLeave;
        }
    }


    #endregion

    #region GetByID
    public class GetStudentApplyLeaveById : IRequest<TblDefStudentApplyLeaveDto>
    {
        public UserIdentityDto User { get; set; }

        public int Id { get; set; }
    }
    public class GetStudentApplyLeaveByIdHandler : IRequestHandler<GetStudentApplyLeaveById, TblDefStudentApplyLeaveDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public GetStudentApplyLeaveByIdHandler(CINDBOneContext context ,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TblDefStudentApplyLeaveDto> Handle(GetStudentApplyLeaveById request,CancellationToken cancellationToken)
        {
            var StudentApplyLeave = await _context.StudentApplyLeave.AsNoTracking().ProjectTo<TblDefStudentApplyLeaveDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.Id == request.Id);
            return StudentApplyLeave;
        }

    }
    #endregion

    #region Create And Update
    public class CreateUpdateStudentLeaveApply : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public TblDefStudentApplyLeaveDto Input { get; set; }
    }

    public class CreateUpdateStudentLeaveApplyHandler : IRequestHandler<CreateUpdateStudentLeaveApply, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateStudentLeaveApplyHandler(CINDBOneContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateUpdateStudentLeaveApply request,CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info Create Update Student Leave Apply method start----");
                var obj = request.Input;
                TblDefStudentApplyLeave studentApplyLeave = new();

                if (obj.Id > 0)
                    studentApplyLeave = await _context.StudentApplyLeave.AsNoTracking().FirstOrDefaultAsync(e => e.Id == obj.Id);

                studentApplyLeave.Id = obj.Id;
                studentApplyLeave.RegisteredPhone = obj.RegisteredPhone;
                studentApplyLeave.RegisteredEmail = obj.RegisteredEmail;
                studentApplyLeave.StuAdmNum = obj.StuAdmNum;
                studentApplyLeave.LeaveCode = obj.LeaveCode;
                studentApplyLeave.LeaveReason = obj.LeaveReason;
                studentApplyLeave.LeaveStartDate = obj.LeaveStartDate;
                studentApplyLeave.LeaveEndDate = obj.LeaveEndDate;
                studentApplyLeave.LeaveMessage = obj.LeaveMessage;
                studentApplyLeave.Attachment1 = obj.Attachment1;
                studentApplyLeave.Attachment2 = obj.Attachment2;
                studentApplyLeave.Attachment3 = obj.Attachment3;
                studentApplyLeave.IsApproved = obj.IsApproved;
                studentApplyLeave.ApprovedBy = obj.ApprovedBy;
                studentApplyLeave.ApprovalRemarks = obj.ApprovalRemarks;
                studentApplyLeave.ApprovedDate = obj.ApprovedDate;
                studentApplyLeave.AcademicYear = obj.AcademicYear;

                if (obj.Id>0)
                {
                    _context.StudentApplyLeave.Update(studentApplyLeave);
                }
                else
                {
                    await _context.StudentApplyLeave.AddAsync(studentApplyLeave);
                }
                await _context.SaveChangesAsync();
                return studentApplyLeave.Id;
            }
            catch(Exception ex)
            {
                Log.Error("Error in Student Apply Leave Create Update Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
            

        }
    }
    #endregion

    #region Delete
    public class DeleteStudentLeaveApply : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteStudentLeaveApplyHandler : IRequestHandler<DeleteStudentLeaveApply, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteStudentLeaveApplyHandler(CINDBOneContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int>Handle(DeleteStudentLeaveApply request,CancellationToken cancellationToken)
        {
            try 
            { 
                if (request.Id > 0)
                {
                        var studentApplyLeave = await _context.StudentApplyLeave.AsNoTracking().ProjectTo<TblDefStudentApplyLeaveDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.Id == request.Id);
                                               _context.Remove(studentApplyLeave);
                                await _context.SaveChangesAsync();

                        return request.Id;
                }
                return 0;
            }
            catch(Exception ex)
            {
                Log.Error("Error in Delete Student Apply Leave  Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace: " + ex.StackTrace);
                return 0;
            }
        }
    }
    #endregion

    #region GetByID
    public class GetLeaveCodes : IRequest<List<StudentLeaveCodesDto>>
    {
        public UserIdentityDto User { get; set; }
    }
    public class GetLeaveCodesHandler : IRequestHandler<GetLeaveCodes, List<StudentLeaveCodesDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public GetLeaveCodesHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<StudentLeaveCodesDto>> Handle(GetLeaveCodes request, CancellationToken cancellationToken)
        {
            var result = await _context.SysSchoolStuLeaveType.AsNoTracking().ProjectTo<TblSysSchoolStuLeaveTypeDto>(_mapper.ConfigurationProvider)
                                            .Select(x=>new StudentLeaveCodesDto {LeaveCode=x.LeaveCode,LeaveName=x.LeaveName,LeaveName2=x.LeaveName2 })
                                            .ToListAsync();
            return result;
        }

    }
    #endregion
}
