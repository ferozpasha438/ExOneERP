using AutoMapper;
using AutoMapper.QueryableExtensions;
using CIN.Application.Common;
using CIN.Application.HumanResource.SetUp.HRMSetUpDtos;
using CIN.DB;
using CIN.Domain.HumanResource.Setup;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CIN.Application.HumanResource.SetUp.HRMSetUpQuery
{
    #region GetPagedList

    public class GetInsuranceTypeList : IRequest<PaginatedList<TblHRMSysInsuranceTypeDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetInsuranceTypeListHandler : IRequestHandler<GetInsuranceTypeList, PaginatedList<TblHRMSysInsuranceTypeDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetInsuranceTypeListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<TblHRMSysInsuranceTypeDto>> Handle(GetInsuranceTypeList request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info GetInsuranceTypeList method start----");
                var search = request.Input.Query;
                var list = await _context.InsuranceTypes
                    .AsNoTracking()
                    .ProjectTo<TblHRMSysInsuranceTypeDto>(_mapper.ConfigurationProvider)
                    .Where(e => (e.InsuranceTypeCode.Contains(search) || e.InsuranceTypeNameEn.Contains(search)))
                    .OrderByDescending(x => x.Id)
                    .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

                Log.Info("----Info GetInsuranceTypeList method end----");
                return list;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetInsuranceTypeList Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region GetInsuranceTypeById

    public class GetInsuranceTypeById : IRequest<TblHRMSysInsuranceTypeDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetInsuranceTypeByIdHandler : IRequestHandler<GetInsuranceTypeById, TblHRMSysInsuranceTypeDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetInsuranceTypeByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TblHRMSysInsuranceTypeDto> Handle(GetInsuranceTypeById request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetInsuranceTypeById method start----");
            try
            {
                var visaType = await _context.InsuranceTypes.AsNoTracking()
                    .ProjectTo<TblHRMSysInsuranceTypeDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(e => e.Id == request.Id);
                Log.Info("----Info GetInsuranceTypeById method end----");

                if (visaType is not null)
                    return visaType;
                else
                    return null;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetInsuranceTypeById Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region CreateUpdateInsuranceType

    public class CreateUpdateInsuranceType : UserIdentityDto, IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblHRMSysInsuranceTypeDto Input { get; set; }
    }
    public class CreateUpdateInsuranceTypeHandler : IRequestHandler<CreateUpdateInsuranceType, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateInsuranceTypeHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<AppCtrollerDto> Handle(CreateUpdateInsuranceType request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateUpdateInsuranceType method start----");
                    var obj = request.Input;
                    TblHRMSysInsuranceType insuranceType = new();

                    insuranceType = await _context.InsuranceTypes.FirstOrDefaultAsync(e => e.InsuranceTypeCode == request.Input.InsuranceTypeCode);

                    if (insuranceType is not null)
                    {
                        insuranceType.InsuranceTypeNameEn = obj.InsuranceTypeNameEn;
                        insuranceType.InsuranceTypeNameAr = obj.InsuranceTypeNameAr;
                        insuranceType.Id = obj.Id;
                        insuranceType.IsActive = obj.IsActive;
                        insuranceType.ModifiedBy = request.User.UserId;
                        insuranceType.Modified = DateTime.Now;

                        _context.InsuranceTypes.Update(insuranceType);
                    }
                    else
                    {
                        insuranceType = new()
                        {
                            InsuranceTypeCode = obj.InsuranceTypeCode,
                            InsuranceTypeNameEn = obj.InsuranceTypeNameEn,
                            InsuranceTypeNameAr = obj.InsuranceTypeNameAr,
                            IsActive = obj.IsActive,
                            CreatedBy = request.User.UserId,
                            Created = DateTime.Now,
                        };
                        await _context.InsuranceTypes.AddAsync(insuranceType);
                    }
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    Log.Info("----Info CreateUpdateInsuranceType method Exit----");
                    return ApiMessageInfo.Status(1, insuranceType.Id);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateUpdateInsuranceType Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return ApiMessageInfo.Status(0);
                }
            }
        }
    }

    #endregion

    #region DeleteInsuranceType
    public class DeleteInsuranceType : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteInsuranceTypeHandler : IRequestHandler<DeleteInsuranceType, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteInsuranceTypeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteInsuranceType request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info DeleteInsuranceType method start----");
                if (request.Id > 0)
                {
                    var city = await _context.InsuranceTypes.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(city);
                    await _context.SaveChangesAsync();
                    Log.Info("----Info DeleteInsuranceType method end----");
                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {
                Log.Error("Error in DeleteInsuranceType Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }
    }

    #endregion

    #region GetInsuranceTypeSelectListItem

    public class GetInsuranceTypeSelectListItem : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetInsuranceTypeSelectListItemHandler : IRequestHandler<GetInsuranceTypeSelectListItem, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetInsuranceTypeSelectListItemHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetInsuranceTypeSelectListItem request, CancellationToken cancellationToken)
        {
            bool isArab = request.User.Culture.IsArab();
            var list = await _context.InsuranceTypes.AsNoTracking().OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = isArab ? e.InsuranceTypeNameAr : e.InsuranceTypeNameEn, Value = e.InsuranceTypeCode })
                  .ToListAsync(cancellationToken);

            return list;
        }
    }

    #endregion   
}
