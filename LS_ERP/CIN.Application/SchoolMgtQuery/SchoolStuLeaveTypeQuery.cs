using AutoMapper;
using CIN.Application;
using CIN.Application.Common;
using CIN.Application.SchoolMgtDto;
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
    #region GetAll
    public class GetSchoolStudLeaveTypeList : IRequest<PaginatedList<TblSysSchoolStuLeaveTypeDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }

    }

    public class GetSchoolStudLeaveTypeListHandler : IRequestHandler<GetSchoolStudLeaveTypeList, PaginatedList<TblSysSchoolStuLeaveTypeDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSchoolStudLeaveTypeListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<TblSysSchoolStuLeaveTypeDto>> Handle(GetSchoolStudLeaveTypeList request, CancellationToken cancellationToken)
        {

           
            var leaves = await _context.SysSchoolStuLeaveType.AsNoTracking().ProjectTo<TblSysSchoolStuLeaveTypeDto>(_mapper.ConfigurationProvider)
                         .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return leaves;
        }


    }


    #endregion

    #region GetById

    public class GetSchoolStudLeaveTypeById : IRequest<TblSysSchoolStuLeaveTypeDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetSchoolStudLeaveTypeByIdHandler : IRequestHandler<GetSchoolStudLeaveTypeById, TblSysSchoolStuLeaveTypeDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSchoolStudLeaveTypeByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TblSysSchoolStuLeaveTypeDto> Handle(GetSchoolStudLeaveTypeById request, CancellationToken cancellationToken)
        {

            TblSysSchoolStuLeaveTypeDto obj = new();
            var leaveType = await _context.SysSchoolStuLeaveType.AsNoTracking().ProjectTo<TblSysSchoolStuLeaveTypeDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.Id == request.Id);
            return leaveType;
            // throw new NotImplementedException();
        }
    }
    #endregion

    #region Create_And_Update


    public class CreateUpdateSchoolStudLeaveType : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public TblSysSchoolStuLeaveTypeDto SchoolStuLeaveTypeDto { get; set; }
    }

    public class CreateUpdateSchoolStudLeaveTypeHandler : IRequestHandler<CreateUpdateSchoolStudLeaveType, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateSchoolStudLeaveTypeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateUpdateSchoolStudLeaveType request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info CreateUpdateSchoolStudLeaveType method start----");

                var obj = request.SchoolStuLeaveTypeDto;


                TblSysSchoolStuLeaveType LeaveType = new();
                if (obj.Id > 0)
                    LeaveType = await _context.SysSchoolStuLeaveType.AsNoTracking().FirstOrDefaultAsync(e => e.Id == obj.Id);


                LeaveType.Id = obj.Id;
                LeaveType.LeaveName = obj.LeaveName;
                LeaveType.LeaveName2 = obj.LeaveName2;
                LeaveType.MaxLeavePerReq = obj.MaxLeavePerReq;
                LeaveType.Remarks = obj.Remarks;
                LeaveType.IsActive = obj.IsActive;
                LeaveType.CreatedOn = DateTime.Now;
                LeaveType.CreatedBy = obj.CreatedBy;

                if (obj.Id > 0)
                {

                    _context.SysSchoolStuLeaveType.Update(LeaveType);
                }
                else
                {
                    LeaveType.LeaveCode = obj.LeaveCode.ToUpper();
                    await _context.SysSchoolStuLeaveType.AddAsync(LeaveType);
                }
                await _context.SaveChangesAsync();
                Log.Info("----Info CreateUpdateSchoolStudLeaveType method Exit----");
                return LeaveType.Id;
            }
            catch (Exception ex)
            {
                Log.Error("Error in CreateUpdateSchoolStudLeaveType Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }


    }



    #endregion

    #region Delete
    public class DeleteSchoolStudLeaveType : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteSchoolStudLeaveTypeHandler : IRequestHandler<DeleteSchoolStudLeaveType, int>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteSchoolStudLeaveTypeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteSchoolStudLeaveType request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info DeleteSchoolStudLeaveType start----");

                if (request.Id > 0)
                {
                    var StuLeaveType = await _context.SysSchoolStuLeaveType.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(StuLeaveType);

                    await _context.SaveChangesAsync();

                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {

                Log.Error("Error in DeleteSchoolStudLeaveType");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }

        }
    }

    #endregion
}
