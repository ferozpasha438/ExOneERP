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

    public class GetInsuranceProviderList : IRequest<PaginatedList<TblHRMSysInsuranceProviderDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetInsuranceProviderListHandler : IRequestHandler<GetInsuranceProviderList, PaginatedList<TblHRMSysInsuranceProviderDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetInsuranceProviderListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<TblHRMSysInsuranceProviderDto>> Handle(GetInsuranceProviderList request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info GetInsuranceProviderList method start----");
                var search = request.Input.Query;
                var list = await _context.InsuranceProviders
                    .AsNoTracking()
                    .ProjectTo<TblHRMSysInsuranceProviderDto>(_mapper.ConfigurationProvider)
                    .Where(e => (e.InsuranceProviderCode.Contains(search) || e.InsuranceProviderNameEn.Contains(search)))
                    .OrderByDescending(x => x.Id)
                    .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

                Log.Info("----Info GetInsuranceProviderList method end----");
                return list;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetInsuranceProviderList Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region GetInsuranceProviderById

    public class GetInsuranceProviderById : IRequest<TblHRMSysInsuranceProviderDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetInsuranceProviderByIdHandler : IRequestHandler<GetInsuranceProviderById, TblHRMSysInsuranceProviderDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetInsuranceProviderByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TblHRMSysInsuranceProviderDto> Handle(GetInsuranceProviderById request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetInsuranceProviderById method start----");
            try
            {
                var insuranceProvider = await _context.InsuranceProviders.AsNoTracking()
                    .ProjectTo<TblHRMSysInsuranceProviderDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(e => e.Id == request.Id);
                Log.Info("----Info GetInsuranceProviderById method end----");

                if (insuranceProvider is not null)
                    return insuranceProvider;
                else
                    return null;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetInsuranceProviderById Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region CreateUpdateInsuranceProvider

    public class CreateUpdateInsuranceProvider : UserIdentityDto, IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblHRMSysInsuranceProviderDto Input { get; set; }
    }
    public class CreateUpdateInsuranceProviderHandler : IRequestHandler<CreateUpdateInsuranceProvider, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateInsuranceProviderHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<AppCtrollerDto> Handle(CreateUpdateInsuranceProvider request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateUpdateInsuranceProvider method start----");
                    var obj = request.Input;
                    TblHRMSysInsuranceProvider insuranceProvider = new();

                    insuranceProvider = await _context.InsuranceProviders
                        .FirstOrDefaultAsync(e => e.InsuranceProviderCode == request.Input.InsuranceProviderCode);

                    if (insuranceProvider is not null)
                    {
                        insuranceProvider.InsuranceProviderNameEn = obj.InsuranceProviderNameEn;
                        insuranceProvider.InsuranceProviderNameAr = obj.InsuranceProviderNameAr;
                        insuranceProvider.Id = obj.Id;
                        insuranceProvider.IsActive = obj.IsActive;
                        insuranceProvider.ModifiedBy = request.User.UserId;
                        insuranceProvider.Modified = DateTime.Now;

                        _context.InsuranceProviders.Update(insuranceProvider);
                    }
                    else
                    {
                        insuranceProvider = new()
                        {
                            InsuranceProviderCode = obj.InsuranceProviderCode,
                            InsuranceProviderNameEn = obj.InsuranceProviderNameEn,
                            InsuranceProviderNameAr = obj.InsuranceProviderNameAr,
                            IsActive = obj.IsActive,
                            CreatedBy = request.User.UserId,
                            Created = DateTime.Now,
                        };
                        await _context.InsuranceProviders.AddAsync(insuranceProvider);
                    }
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    Log.Info("----Info CreateUpdateInsuranceProvider method Exit----");
                    return ApiMessageInfo.Status(1, insuranceProvider.Id);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateUpdateInsuranceProvider Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return ApiMessageInfo.Status(0);
                }
            }
        }
    }

    #endregion

    #region DeleteInsuranceProvider
    public class DeleteInsuranceProvider : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteInsuranceProviderHandler : IRequestHandler<DeleteInsuranceProvider, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteInsuranceProviderHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteInsuranceProvider request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info DeleteInsuranceProvider method start----");
                if (request.Id > 0)
                {
                    var insuranceProvider = await _context.InsuranceProviders.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(insuranceProvider);
                    await _context.SaveChangesAsync();
                    Log.Info("----Info DeleteInsuranceProvider method end----");
                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {
                Log.Error("Error in DeleteInsuranceProvider Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }
    }

    #endregion

    #region GetInsuranceProviderSelectListItem

    public class GetInsuranceProviderSelectListItem : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetInsuranceProviderSelectListItemHandler : IRequestHandler<GetInsuranceProviderSelectListItem, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetInsuranceProviderSelectListItemHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetInsuranceProviderSelectListItem request, CancellationToken cancellationToken)
        {
            bool isArab = request.User.Culture.IsArab();
            var list = await _context.InsuranceProviders.AsNoTracking().OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = isArab ? e.InsuranceProviderNameAr : e.InsuranceProviderNameEn, Value = e.InsuranceProviderCode })
                  .ToListAsync(cancellationToken);

            return list;
        }
    }

    #endregion   
}
