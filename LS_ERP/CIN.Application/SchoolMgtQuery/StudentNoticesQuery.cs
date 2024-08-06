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

namespace CIN.Application.SchoolMgtQuery
{
    #region GetAll
    public class GetAllStudentNoticesList : IRequest<PaginatedList<TblDefStudentNoticesDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetAllStudentNoticesListHandler : IRequestHandler<GetAllStudentNoticesList, PaginatedList<TblDefStudentNoticesDto>>
    {
        private CINDBOneContext _context;
        private IMapper _mapper;

        public GetAllStudentNoticesListHandler(CINDBOneContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<TblDefStudentNoticesDto>>Handle(GetAllStudentNoticesList request ,CancellationToken cancellationToken)
        {
            var list = await _context.DefStudentNotices.AsNoTracking().ProjectTo<TblDefStudentNoticesDto>(_mapper.ConfigurationProvider)
                     .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return list;
        }
    }
    #endregion

    #region GetStudentNoticesList
    public class GetStudentNoticesList : IRequest<PaginatedList<StudentNoticesListDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetStudentNoticesListHandler : IRequestHandler<GetStudentNoticesList, PaginatedList<StudentNoticesListDto>>
    {
        private CINDBOneContext _context;
        private IMapper _mapper;

        public GetStudentNoticesListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<StudentNoticesListDto>> Handle(GetStudentNoticesList request, CancellationToken cancellationToken)
        {
           
            var list = await _context.DefStudentNotices.AsNoTracking().ProjectTo<TblDefStudentNoticesDto>(_mapper.ConfigurationProvider).Where(x=>x.StuAdmNum==request.Input.StuAdmNum)
                       .Select(x=>new StudentNoticesListDto { 
                           ActionDate=x.ActionDate,
                           ActionItems=x.ActionItems,
                           ActionRemarks=x.ActionRemarks,
                           ActionTaken=x.ActionTaken,
                           AprovedBy=x.AprovedBy,
                           ClosedBy=x.ClosedBy,
                           ClosedOn=x.ClosedOn,
                           Id=x.Id,
                           IsApproved=x.IsApproved,
                           IsClosed=x.IsClosed,
                           NoticeDate=x.NoticeDate,
                           PosNeg=x.PosNeg,
                           ReasonCode=x.ReasonCode,
                           Remarks=x.Remarks,
                           ReportedBy=x.ReportedBy,
                           StuAdmNum=x.StuAdmNum
                        })
                     .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);
            foreach (var item in list.Items)
            {
                var reasonTypeData = await _context.DefStudentNoticesReasonCode.AsNoTracking().FirstOrDefaultAsync(x=>x.ReasonCode==item.ReasonCode);
                if (reasonTypeData!=null)
                {
                    item.ReasonType= reasonTypeData.ReasonType;
                }
            }

            return list;
        }
    }
    #endregion

    #region GetById
    public class GetStudentNoticesById : IRequest<TblDefStudentNoticesDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetStudentNoticesByIdHandler : IRequestHandler<GetStudentNoticesById, TblDefStudentNoticesDto>
    {
        private CINDBOneContext _context;
        private IMapper _mapper;

        public GetStudentNoticesByIdHandler(CINDBOneContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TblDefStudentNoticesDto>Handle(GetStudentNoticesById request,CancellationToken cancellationToken)
        {
            var studentNotices = await _context.DefStudentNotices.AsNoTracking().ProjectTo<TblDefStudentNoticesDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.Id == request.Id);
            return studentNotices;
        }
    }

    #endregion

    #region Create And Update
    public class CreateUpdateStudentNotices : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public TblDefStudentNoticesDto Input { get; set; }
    }

    public class CreateUpdateStudentNoticesHandler : IRequestHandler<CreateUpdateStudentNotices, int>
    {
        private CINDBOneContext _context;
        private IMapper _mapper;

        public CreateUpdateStudentNoticesHandler(CINDBOneContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int>Handle(CreateUpdateStudentNotices request,CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info CreateUpdate Student Notices method start----");
                var obj = request.Input;
                TblDefStudentNotices studentNotices = new();

                if (obj.Id > 0)
                    studentNotices = await _context.DefStudentNotices.AsNoTracking().FirstOrDefaultAsync(e => e.Id == obj.Id);

                studentNotices.Id = obj.Id;
                studentNotices.StuAdmNum = obj.StuAdmNum;
                studentNotices.PosNeg = obj.PosNeg;
                studentNotices.NoticeDate = obj.NoticeDate;
                studentNotices.ReasonCode = obj.ReasonCode;
                studentNotices.ReportedBy = obj.ReportedBy;
                studentNotices.Remarks = obj.Remarks;
                studentNotices.ActionItems = obj.ActionItems;
                studentNotices.ActionRemarks = obj.ActionRemarks;
                studentNotices.IsApproved = obj.IsApproved;
                studentNotices.AprovedBy = obj.AprovedBy;
                studentNotices.IsClosed = obj.IsClosed;
                studentNotices.ActionTaken = obj.ActionTaken;
                studentNotices.ActionDate = obj.ActionDate;
                studentNotices.ClosedBy = obj.ClosedBy;
                studentNotices.ClosedOn = obj.ClosedOn;

                if (obj.Id > 0)
                {
                    _context.DefStudentNotices.Update(studentNotices);
                }
                else
                {
                  await _context.DefStudentNotices.AddAsync(studentNotices);
                }
                await _context.SaveChangesAsync();
                return studentNotices.Id;
            }
            catch(Exception ex)
            {
                Log.Error("Error in Create Update Student Notices Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }
    }
    #endregion

    #region Delete
    public class DeleteStudentNotices : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteStudentNoticesHandler : IRequestHandler<DeleteStudentNotices, int>
    {
        private CINDBOneContext _context;
        private IMapper _mapprer;

        public DeleteStudentNoticesHandler(CINDBOneContext context , IMapper mapper)
        {
            _context = context;
            _mapprer = mapper;       
        }

        public async Task<int>Handle(DeleteStudentNotices request,CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info Delete Student Notices method start----");
                if (request.Id > 0)
                {
                    var studentNotices = await _context.DefStudentNotices.AsNoTracking().ProjectTo<TblDefStudentNoticesDto>(_mapprer.ConfigurationProvider).FirstOrDefaultAsync(e => e.Id == request.Id);
                                        _context.Remove(studentNotices);
                    await _context.SaveChangesAsync();
                    return request.Id;
                }
                return 0;

            }
            catch(Exception ex)
            {
                Log.Error("Error in School Student Notices");
                Log.Error("Error occured time :" + DateTime.UtcNow);
                Log.Error("Error Message :" +ex.Message);
                Log.Error("Error StackTrace :" + ex.StackTrace);
                return 0;
            }
        }
    }

    #endregion
}
