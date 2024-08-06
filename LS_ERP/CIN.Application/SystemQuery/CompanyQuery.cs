using AutoMapper;
using AutoMapper.QueryableExtensions;
using CIN.Application.Common;
using CIN.Application.SystemSetupDtos;
using CIN.DB;
using CIN.Domain.SystemSetup;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;

namespace CIN.Application.SystemQuery
{

    #region CheckCompanyName

    public class CheckCompanyName : IRequest<bool>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class CheckCompanyNameHandler : IRequestHandler<CheckCompanyName, bool>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public CheckCompanyNameHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<bool> Handle(CheckCompanyName request, CancellationToken cancellationToken)
        {
            Log.Info("----Info CheckCompanyName method start----");
            return await _context.Companies.AnyAsync(e => e.CompanyName.Trim() == request.Input.Trim());
        }
    }

    #endregion

    #region Get CompanySelect List

    public class GetCompanySelectItemList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetCompanySelectItemListHandler : IRequestHandler<GetCompanySelectItemList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetCompanySelectItemListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetCompanySelectItemList request, CancellationToken cancellationToken)
        {
            var search = request.Input;
            var list = _context.Companies.AsNoTracking();
            if (!string.IsNullOrEmpty(search))
                list = list.Where(e => e.CompanyName.Contains(search));

            var newList = await list.Select(e => new CustomSelectListItem { Text = e.CompanyName, Value = e.Id.ToString() })
                .ToListAsync(cancellationToken);
            return newList;
        }
    }

    #endregion

    #region GetPagedList

    public class GetCompanyList : IRequest<PaginatedList<TblErpSysCompanyDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetCompanyListHandler : IRequestHandler<GetCompanyList, PaginatedList<TblErpSysCompanyDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetCompanyListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<TblErpSysCompanyDto>> Handle(GetCompanyList request, CancellationToken cancellationToken)
        {
            var search = request.Input.Query;
            var list = await _context.Companies.AsNoTracking()
              .Where(e => //e.CompanyId == request.CompanyId &&
                            (e.City.Contains(search) || e.CompanyName.Contains(search) ||
                                e.State.Contains(search) || e.Mobile.Contains(search)
                             ))
               .OrderBy(request.Input.OrderBy)
              .ProjectTo<TblErpSysCompanyDto>(_mapper.ConfigurationProvider)
                 .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return list;
        }
    }

    #endregion

    #region SingleItem

    public class GetCompany : IRequest<TblErpSysCompanyDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
        public string SiteCode { get; set; }
    }

    public class GetCompanyHandler : IRequestHandler<GetCompany, TblErpSysCompanyDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetCompanyHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblErpSysCompanyDto> Handle(GetCompany request, CancellationToken cancellationToken)
        {
            TblErpSysCompanyDto obj = new();
            try
            {
                obj = await _context.Companies.AsNoTracking()
                            .Where(e => e.Id == request.Id)
                       .ProjectTo<TblErpSysCompanyDto>(_mapper.ConfigurationProvider)
                          .FirstOrDefaultAsync(cancellationToken);

                //if (request.SiteCode.HasValue())
                //{
                //    var site = await _context.OprSites.FirstOrDefaultAsync(e => e.SiteCode == request.SiteCode);
                //    if (site is not null)
                //    {
                //        obj.CompanyAddress = site.SiteAddress;
                //        obj.CompanyAddressAr = site.SiteAddress;
                //        obj.CompanyName = site.SiteName;
                //        obj.CompanyNameAr = site.SiteArbName;
                //        obj.VATNumber = site.VATNumber;

                //    }
                //}
            }
            catch (Exception ex)
            {
            }
            return obj;
        }
    }

    #endregion


    #region GetCompanyByID

    public class GetCompanyByID : IRequest<ErpSysCompanyDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
        public string SiteCode { get; set; }
    }

    public class GetCompanyByIDHandler : IRequestHandler<GetCompanyByID, ErpSysCompanyDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetCompanyByIDHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ErpSysCompanyDto> Handle(GetCompanyByID request, CancellationToken cancellationToken)
        {
            ErpSysCompanyDto obj = new();
            try
            {
                obj = await _context.Companies.AsNoTracking()
                            .Where(e => e.Id == request.Id)
                            .Select(x=>new ErpSysCompanyDto
                            {
                                CcNumber=x.CcNumber,
                                City=x.City,
                                CompanyAddress=x.CompanyAddress,
                                CompanyAddressAr=x.CompanyAddressAr,
                                CompanyName=x.CompanyName,
                                CompanyNameAr=x.CompanyNameAr,
                                Country=x.Country,
                                CrNumber=x.CrNumber,
                                Csr=x.Csr,
                                DateFormat=x.DateFormat,
                                DevCsid=x.DevCsid,
                                DevRequestId=x.DevRequestId,
                                DevSecret = x.DevSecret,
                                Email=x.Email,
                                FooterLogo=x.FooterLogo,
                                GeoLocLatitude=x.GeoLocLatitude,
                                GeoLocLongitude=x.GeoLocLongitude,
                                HeaderLogo=x.HeaderLogo,
                                Id=x.Id,
                                LogoImagePath=x.LogoImagePath,
                                LogoURL=x.LogoURL,
                                Mobile=x.Mobile,
                                Phone=x.Phone,
                                PriceDecimal=Convert.ToString(x.PriceDecimal),
                                ProdCsid=x.ProdCsid,
                                ProdRequestId=x.ProdRequestId,
                                ProdSecret=x.ProdSecret,
                                QuantityDecimal= Convert.ToString(x.QuantityDecimal),
                                State=x.State,
                                VATNumber=x.VATNumber,
                                Website=x.Website
                            })
                            .FirstOrDefaultAsync();



                //if (request.SiteCode.HasValue())
                //{
                //    var site = await _context.OprSites.FirstOrDefaultAsync(e => e.SiteCode == request.SiteCode);
                //    if (site is not null)
                //    {
                //        obj.CompanyAddress = site.SiteAddress;
                //        obj.CompanyAddressAr = site.SiteAddress;
                //        obj.CompanyName = site.SiteName;
                //        obj.CompanyNameAr = site.SiteArbName;
                //        obj.VATNumber = site.VATNumber;

                //    }
                //}
            }
            catch (Exception ex)
            {
            }
            return obj;
        }
    }

    #endregion

    #region CreateUpdate

    public class CreateCompany : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public TblErpSysCompanyDto CompanyDto { get; set; }
    }

    public class CreateCompanyQueryHandler : IRequestHandler<CreateCompany, int>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateCompanyQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateCompany request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info SaveUpdateCompany method start----");


                var obj = request.CompanyDto;
                TblErpSysCompany Company = new();
                if (obj.Id > 0)
                    Company = await _context.Companies.FirstOrDefaultAsync(e => e.Id == obj.Id);

                Company.CompanyName = obj.CompanyName;
                Company.CompanyAddress = obj.CompanyAddress;
                Company.Phone = obj.Phone;
                Company.Email = obj.Email;
                Company.VATNumber = obj.VATNumber;
                Company.GeoLocLatitude = obj.GeoLocLatitude;
                Company.GeoLocLongitude = obj.GeoLocLongitude;
                Company.LogoURL = obj.LogoURL;
                Company.PriceDecimal = char.Parse(obj.PriceDecimal);
                Company.City = obj.City;
                Company.State = obj.State;
                Company.Country = obj.Country;
                Company.Mobile = obj.Mobile;
                Company.DateFormat = obj.DateFormat;
                Company.Website = obj.Website;
                Company.LogoImagePath = obj.LogoImagePath;
                Company.QuantityDecimal = char.Parse(obj.QuantityDecimal);
                Company.CrNumber = obj.CrNumber;
                Company.CcNumber = obj.CcNumber;
                Company.CompanyNameAr = obj.CompanyNameAr;
                Company.CompanyAddressAr = obj.CompanyAddressAr;

                if (obj.Id > 0)
                {
                    _context.Companies.Update(Company);
                }
                else
                {
                    await _context.Companies.AddAsync(Company);
                }
                await _context.SaveChangesAsync();
                Log.Info("----Info SaveUpdateCompany method Exit----");
                return Company.Id;
            }
            catch (Exception ex)
            {
                Log.Error("Error in SaveUpdateCompany Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }
    }
    #endregion

    #region UpdateZatcaAPISetting

    public class UpdateZatcaAPISetting : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public TblErpSysCompanyDto CompanyDto { get; set; }
    }

    public class UpdateZatcaAPISettingQueryHandler : IRequestHandler<UpdateZatcaAPISetting, int>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public UpdateZatcaAPISettingQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(UpdateZatcaAPISetting request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info SaveUpdateCompany method start----");


                var obj = request.CompanyDto;
                TblErpSysCompany Company = await _context.Companies.FirstOrDefaultAsync(e => e.Id == obj.Id);

                Company.Csr = obj.Csr;
                Company.DevRequestId = obj.DevRequestId;
                Company.DevCsid = obj.DevCsid;
                Company.DevSecret = obj.DevSecret;

                Company.ProdRequestId = obj.ProdRequestId;
                Company.ProdCsid = obj.ProdCsid;
                Company.ProdSecret = obj.ProdSecret;


                if (obj.Id > 0)
                {
                    _context.Companies.Update(Company);
                }

                await _context.SaveChangesAsync();
                Log.Info("----Info SaveUpdateCompany method Exit----");
                return Company.Id;
            }
            catch (Exception ex)
            {
                Log.Error("Error in SaveUpdateCompany Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }
    }
    #endregion

}
