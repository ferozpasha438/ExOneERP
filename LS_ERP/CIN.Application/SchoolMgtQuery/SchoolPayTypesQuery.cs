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
    public class GetSchoolPayTypeList : IRequest<PaginatedList<TblSysSchoolPayTypesDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }

    }

    public class GetSchoolPayTypeListHandler : IRequestHandler<GetSchoolPayTypeList, PaginatedList<TblSysSchoolPayTypesDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSchoolPayTypeListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<TblSysSchoolPayTypesDto>> Handle(GetSchoolPayTypeList request, CancellationToken cancellationToken)
        {

            
            var payTypes = await _context.SysSchoolPayTypes.AsNoTracking().ProjectTo<TblSysSchoolPayTypesDto>(_mapper.ConfigurationProvider)
                           .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return payTypes;
        }


    }


    #endregion

    #region GetById

    public class GetSchoolPayTypeById : IRequest<TblSysSchoolPayTypesDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetSchoolPayTypeByIdHandler : IRequestHandler<GetSchoolPayTypeById, TblSysSchoolPayTypesDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSchoolPayTypeByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TblSysSchoolPayTypesDto> Handle(GetSchoolPayTypeById request, CancellationToken cancellationToken)
        {

            TblSysSchoolPayTypesDto obj = new();
            var payType = await _context.SysSchoolPayTypes.AsNoTracking().ProjectTo<TblSysSchoolPayTypesDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.Id == request.Id);
            return payType;
            // throw new NotImplementedException();
        }
    }
    #endregion

    #region Create_And_Update


    public class CreateUpdateSchoolPayType : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public TblSysSchoolPayTypesDto SchoolPayTypesDto { get; set; }
    }

    public class CreateUpdateSchoolPayTypeHandler : IRequestHandler<CreateUpdateSchoolPayType, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateSchoolPayTypeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateUpdateSchoolPayType request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info CreateUpdateSchoolPayType method start----");

                var obj = request.SchoolPayTypesDto;


                TblSysSchoolPayTypes PayType = new();
                if (obj.Id > 0)
                    PayType = await _context.SysSchoolPayTypes.AsNoTracking().FirstOrDefaultAsync(e => e.Id == obj.Id);


                PayType.Id = obj.Id;
                PayType.PayName = obj.PayName;
                PayType.PaName2 = obj.PaName2;
                PayType.GLAccount = obj.GLAccount;
                PayType.BranchCode = obj.BranchCode;
                PayType.AllowOtherBranchUse = obj.AllowOtherBranchUse;
                PayType.Remarks = obj.Remarks;
                PayType.IsActive = obj.IsActive;
                PayType.CreatedOn = DateTime.Now;
                PayType.CreatedBy = obj.CreatedBy;

                if (obj.Id > 0)
                {

                    _context.SysSchoolPayTypes.Update(PayType);
                }
                else
                {
                    PayType.PayCode = obj.PayCode.ToUpper();
                    await _context.SysSchoolPayTypes.AddAsync(PayType);
                }
                await _context.SaveChangesAsync();
                Log.Info("----Info CreateUpdateSchoolPayType method Exit----");
                return PayType.Id;
            }
            catch (Exception ex)
            {
                Log.Error("Error in CreateUpdateSchoolPayType Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }


    }



    #endregion

    #region Delete
    public class DeleteSchoolPayType : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteSchoolPayTypeHandler : IRequestHandler<DeleteSchoolPayType, int>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteSchoolPayTypeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteSchoolPayType request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info DeleteSchoolPayType start----");

                if (request.Id > 0)
                {
                    var PayType = await _context.SysSchoolPayTypes.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(PayType);

                    await _context.SaveChangesAsync();

                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {

                Log.Error("Error in DeleteSchoolPayType");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }

        }
    }

    #endregion


    #region GetAll
    public class GetAllBranchPayTypes : IRequest<List<TblSysSchoolPayTypesDto>>
    {
        public UserIdentityDto User { get; set; }
        public string BranchCode { get; set; }

    }

    public class GetAllBranchPayTypesHandler : IRequestHandler<GetAllBranchPayTypes, List<TblSysSchoolPayTypesDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetAllBranchPayTypesHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<TblSysSchoolPayTypesDto>> Handle(GetAllBranchPayTypes request, CancellationToken cancellationToken)
        {


            var payTypes = await _context.SysSchoolPayTypes.AsNoTracking().ProjectTo<TblSysSchoolPayTypesDto>(_mapper.ConfigurationProvider)
                           .Where(x => x.BranchCode == request.BranchCode).ToListAsync();

            return payTypes;
        }


    }


    #endregion
}
