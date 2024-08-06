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

    public class GetVisaTypeList : IRequest<PaginatedList<TblHRMSysVisaTypeDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetVisaTypeListHandler : IRequestHandler<GetVisaTypeList, PaginatedList<TblHRMSysVisaTypeDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetVisaTypeListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<TblHRMSysVisaTypeDto>> Handle(GetVisaTypeList request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info GetVisaTypeList method start----");
                var search = request.Input.Query;

                var list = await (from visaType in _context.VisaTypes
                                  join country in _context.CountryCodes on visaType.CountryCode equals country.CountryCode
                                  select new TblHRMSysVisaTypeDto
                                  {
                                      Id = visaType.Id,
                                      VisaTypeCode = visaType.VisaTypeCode,
                                      VisaTypeNameEn = visaType.VisaTypeNameEn,
                                      VisaTypeNameAr = visaType.VisaTypeNameAr,
                                      Created = visaType.Created,
                                      CreatedBy = visaType.CreatedBy,
                                      Modified = visaType.Modified,
                                      ModifiedBy = visaType.ModifiedBy,
                                      CountryCode = visaType.CountryCode,
                                      CountryName = country.CountryName,
                                      IsActive = visaType.IsActive,
                                  })
                  .AsNoTracking()
                  .Where(e => (e.VisaTypeCode.Contains(search) || e.VisaTypeNameEn.Contains(search)))
                  .OrderByDescending(x => x.Id)
                  .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

                Log.Info("----Info GetVisaTypeList method end----");
                return list;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetVisaTypeList Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region GetVisaTypeById

    public class GetVisaTypeById : IRequest<TblHRMSysVisaTypeDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetVisaTypeByIdHandler : IRequestHandler<GetVisaTypeById, TblHRMSysVisaTypeDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetVisaTypeByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TblHRMSysVisaTypeDto> Handle(GetVisaTypeById request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetVisaTypeById method start----");
            try
            {
                var visaType = await _context.VisaTypes.AsNoTracking()
                    .ProjectTo<TblHRMSysVisaTypeDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(e => e.Id == request.Id);
                Log.Info("----Info GetVisaTypeById method end----");

                if (visaType is not null)
                    return visaType;
                else
                    return null;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetVisaTypeById Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region CreateUpdateVisaType

    public class CreateUpdateVisaType : UserIdentityDto, IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblHRMSysVisaTypeDto Input { get; set; }
    }
    public class CreateUpdateVisaTypeHandler : IRequestHandler<CreateUpdateVisaType, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateVisaTypeHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<AppCtrollerDto> Handle(CreateUpdateVisaType request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateUpdateVisaType method start----");
                    var obj = request.Input;
                    TblHRMSysVisaType visaType = new();

                    visaType = await _context.VisaTypes.FirstOrDefaultAsync(e => e.VisaTypeCode == request.Input.VisaTypeCode);

                    if (visaType is not null)
                    {
                        visaType.VisaTypeNameEn = obj.VisaTypeNameEn;
                        visaType.VisaTypeNameAr = obj.VisaTypeNameAr;
                        visaType.Id = obj.Id;
                        visaType.CountryCode = obj.CountryCode;
                        visaType.IsActive = obj.IsActive;
                        visaType.ModifiedBy = request.User.UserId;
                        visaType.Modified = DateTime.Now;

                        _context.VisaTypes.Update(visaType);
                    }
                    else
                    {
                        visaType = new()
                        {
                            VisaTypeCode = obj.VisaTypeCode,
                            VisaTypeNameEn = obj.VisaTypeNameEn,
                            VisaTypeNameAr = obj.VisaTypeNameAr,
                            CountryCode = obj.CountryCode,
                            IsActive = obj.IsActive,
                            CreatedBy = request.User.UserId,
                            Created = DateTime.Now,
                        };
                        await _context.VisaTypes.AddAsync(visaType);
                    }
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    Log.Info("----Info CreateUpdateVisaType method Exit----");
                    return ApiMessageInfo.Status(1, visaType.Id);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateUpdateVisaType Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return ApiMessageInfo.Status(0);
                }
            }
        }
    }

    #endregion

    #region DeleteVisaType
    public class DeleteVisaType : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteVisaTypeHandler : IRequestHandler<DeleteVisaType, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteVisaTypeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteVisaType request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info DeleteVisaType method start----");
                if (request.Id > 0)
                {
                    var city = await _context.VisaTypes.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(city);
                    await _context.SaveChangesAsync();
                    Log.Info("----Info DeleteVisaType method end----");
                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {
                Log.Error("Error in DeleteVisaType Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }
    }

    #endregion

    #region GetVisaTypesByCountrySelectListItem
    public class GetVisaTypesByCountrySelectListItem : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string CountryCode { get; set; }
    }

    public class GetVisaTypesByCountrySelectListItemHandler : IRequestHandler<GetVisaTypesByCountrySelectListItem, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetVisaTypesByCountrySelectListItemHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetVisaTypesByCountrySelectListItem request, CancellationToken cancellationToken)
        {
            bool isArab = request.User.Culture.IsArab();
            var list = await _context.VisaTypes
                .AsNoTracking()
                .Where(e => (e.CountryCode == request.CountryCode))
                .OrderByDescending(e => e.Id)
                .Select(e => new CustomSelectListItem { Text = isArab ? e.VisaTypeNameAr : e.VisaTypeNameEn, Value = e.VisaTypeCode })
                .ToListAsync(cancellationToken);
            return list;
        }
    }

    #endregion   
}
