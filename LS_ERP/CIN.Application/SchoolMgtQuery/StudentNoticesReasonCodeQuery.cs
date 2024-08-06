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
    #region GetAll
    public class GetAllStudentNoticesReasonCodeList : IRequest<PaginatedList<TblDefStudentNoticesReasonCodeDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetAllStudentNoticesReasonCodeListHandler : IRequestHandler<GetAllStudentNoticesReasonCodeList, PaginatedList<TblDefStudentNoticesReasonCodeDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public GetAllStudentNoticesReasonCodeListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<TblDefStudentNoticesReasonCodeDto>> Handle(GetAllStudentNoticesReasonCodeList request, CancellationToken cancellationToken)
        {
            var list = await _context.DefStudentNoticesReasonCode.AsNoTracking().ProjectTo<TblDefStudentNoticesReasonCodeDto>(_mapper.ConfigurationProvider)
                     .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return list;
        }
    }
    #endregion

    #region GetReasonCodesByType
    public class GetReasonCodesByType : IRequest<List<TblDefStudentNoticesReasonCodeDto>>
    {
        public UserIdentityDto User { get; set; }
        public string ReasonType { get; set; }
    }

    public class GetReasonCodesByTypeHandler : IRequestHandler<GetReasonCodesByType, List<TblDefStudentNoticesReasonCodeDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public GetReasonCodesByTypeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<TblDefStudentNoticesReasonCodeDto>> Handle(GetReasonCodesByType request, CancellationToken cancellationToken)
        {
            var list = await _context.DefStudentNoticesReasonCode.AsNoTracking().ProjectTo<TblDefStudentNoticesReasonCodeDto>(_mapper.ConfigurationProvider)
                     .Where(x=>x.ReasonType==request.ReasonType).ToListAsync();

            return list;
        }
    }
    #endregion

    #region GetById
    public class GetAllStudentNoticesReasonCodeById : IRequest<TblDefStudentNoticesReasonCodeDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetAllStudentNoticesReasonCodeByIdHandler : IRequestHandler<GetAllStudentNoticesReasonCodeById, TblDefStudentNoticesReasonCodeDto>
    {
        private CINDBOneContext _context;
        private IMapper _mapper;

        public GetAllStudentNoticesReasonCodeByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TblDefStudentNoticesReasonCodeDto> Handle(GetAllStudentNoticesReasonCodeById request, CancellationToken cancellationToken)
        {
            var studentNoticesReasonCode = await _context.DefStudentNoticesReasonCode.AsNoTracking().ProjectTo<TblDefStudentNoticesReasonCodeDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.Id == request.Id);
            return studentNoticesReasonCode;
        }
    }
    #endregion

    #region CreateUpdate
    public class CreateUpdateStudentNoticesReasonCode : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public TblDefStudentNoticesReasonCodeDto Input { get; set; }

    }
    public class CreateUpdateStudentNoticesReasonCodeHandler : IRequestHandler<CreateUpdateStudentNoticesReasonCode, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateStudentNoticesReasonCodeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateUpdateStudentNoticesReasonCode request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info Create Update Student Notices Reason Code method start----");

                var obj = request.Input;


                TblDefStudentNoticesReasonCode NoticesReasonCode = new();
                if (obj.Id > 0)
                    NoticesReasonCode = await _context.DefStudentNoticesReasonCode.AsNoTracking().FirstOrDefaultAsync(e => e.Id == obj.Id);


                NoticesReasonCode.Id = obj.Id;
                NoticesReasonCode.ReasonCode = obj.ReasonCode;
                NoticesReasonCode.ReasonType = obj.ReasonType;
                NoticesReasonCode.ReasonName1 = obj.ReasonName1;
                NoticesReasonCode.ReasonName2 = obj.ReasonName2;
                NoticesReasonCode.RequireAction = obj.RequireAction;
                NoticesReasonCode.IsActive = obj.IsActive;

                if (obj.Id > 0)
                {

                    _context.DefStudentNoticesReasonCode.Update(NoticesReasonCode);
                }
                else
                {
                  
                    await _context.DefStudentNoticesReasonCode.AddAsync(NoticesReasonCode);
                }
                await _context.SaveChangesAsync();
                Log.Info("----Info Create Update Student Notices method Exit----");
                return NoticesReasonCode.Id;
            }
            catch (Exception ex)
            {
                Log.Error("Error in CreateUpdateStudentNotices Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }


    }
    #endregion

    #region Delete
    public class DeleteStudentNoticesReasonCode : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteStudentNoticesReasonCodeHandler : IRequestHandler<DeleteStudentNoticesReasonCode, int>
    {
        
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteStudentNoticesReasonCodeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteStudentNoticesReasonCode request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info DeleteStudentNoticesReasonCode start----");

                if (request.Id > 0)
                {
                    var noticesReasonCode = await _context.DefStudentNoticesReasonCode.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(noticesReasonCode);

                    await _context.SaveChangesAsync();

                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {

                Log.Error("Error in DeleteStudentNoticesReasonCode");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }

        }
    }
    #endregion
}

