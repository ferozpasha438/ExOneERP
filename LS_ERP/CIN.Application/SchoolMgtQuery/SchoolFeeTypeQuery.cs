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
    public class GetSysSchoolFeeTypeList : IRequest<PaginatedList<TblSysSchoolFeeTypeDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }

    }

    public class GetSysSchoolFeeTypeListHandler : IRequestHandler<GetSysSchoolFeeTypeList, PaginatedList<TblSysSchoolFeeTypeDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSysSchoolFeeTypeListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<TblSysSchoolFeeTypeDto>> Handle(GetSysSchoolFeeTypeList request, CancellationToken cancellationToken)
        {

            
            var schoolFeeType = await _context.SysSchoolFeeType.AsNoTracking().ProjectTo<TblSysSchoolFeeTypeDto>(_mapper.ConfigurationProvider)
                                .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return schoolFeeType;
        }


    }


    #endregion

    #region GetById

    public class GetSysSchoolFeeTypeById : IRequest<TblSysSchoolFeeTypeDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetSysSchoolFeeTypeByIdHandler : IRequestHandler<GetSysSchoolFeeTypeById, TblSysSchoolFeeTypeDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSysSchoolFeeTypeByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TblSysSchoolFeeTypeDto> Handle(GetSysSchoolFeeTypeById request, CancellationToken cancellationToken)
        {

            TblSysSchoolFeeTypeDto obj = new();
            var schoolFeeType = await _context.SysSchoolFeeType.AsNoTracking().ProjectTo<TblSysSchoolFeeTypeDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.Id == request.Id);
            return schoolFeeType;
            // throw new NotImplementedException();
        }
    }
    #endregion

    #region Create_And_Update


    public class CreateUpdateSchoolFeeType : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public TblSysSchoolFeeTypeDto SchoolFeeTypeDto { get; set; }
    }

    public class CreateUpdateSchoolFeeTypeHandler : IRequestHandler<CreateUpdateSchoolFeeType, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateSchoolFeeTypeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateUpdateSchoolFeeType request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info CreateUpdateSchoolFeeType method start----");

                var obj = request.SchoolFeeTypeDto;


                TblSysSchoolFeeType SchoolFeeType = new();
                if (obj.Id > 0)
                    SchoolFeeType = await _context.SysSchoolFeeType.AsNoTracking().FirstOrDefaultAsync(e => e.Id == obj.Id);


                SchoolFeeType.Id = obj.Id;
                SchoolFeeType.FeesName = obj.FeesName;
                SchoolFeeType.FeeName2 = obj.FeeName2;
                SchoolFeeType.EstFeeAmount = obj.EstFeeAmount;
                SchoolFeeType.MinFeeAmount = obj.MinFeeAmount;
                SchoolFeeType.MaxFeeAmount = obj.MaxFeeAmount;
                SchoolFeeType.IsDiscountable = obj.IsDiscountable;
                SchoolFeeType.MaxDiscPer = obj.MaxDiscPer;
                SchoolFeeType.TaxApplicable = obj.TaxApplicable;
                SchoolFeeType.TaxCode = obj.TaxCode;
                SchoolFeeType.Frequency = obj.Frequency;
                SchoolFeeType.FeeGLAccount = obj.FeeGLAccount;
                SchoolFeeType.FeeTaxAccount = obj.FeeTaxAccount;
                SchoolFeeType.Remarks = obj.Remarks;
                SchoolFeeType.IsActive = obj.IsActive;
                SchoolFeeType.CreatedOn = DateTime.Now;
                SchoolFeeType.CreatedBy = obj.CreatedBy;

                if (obj.Id > 0)
                {

                    _context.SysSchoolFeeType.Update(SchoolFeeType);
                }
                else
                {
                    SchoolFeeType.FeeCode = obj.FeeCode.ToUpper();
                    await _context.SysSchoolFeeType.AddAsync(SchoolFeeType);
                }
                await _context.SaveChangesAsync();
                Log.Info("----Info CreateUpdateSchoolFeeType  method Exit----");
                return SchoolFeeType.Id;
            }
            catch (Exception ex)
            {
                Log.Error("Error in CreateUpdateSchoolFeeType  Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }


    }



    #endregion

    #region Delete
    public class DeleteSchoolFeeType : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteSchoolFeeTypeHandler : IRequestHandler<DeleteSchoolFeeType, int>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteSchoolFeeTypeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteSchoolFeeType request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info DeleteSchoolFeeType start----");

                if (request.Id > 0)
                {
                    var schoolFeeType = await _context.SysSchoolFeeType.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(schoolFeeType);

                    await _context.SaveChangesAsync();

                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {

                Log.Error("Error in  DeleteSchoolFeeType");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }

        }
    }

    #endregion
}
