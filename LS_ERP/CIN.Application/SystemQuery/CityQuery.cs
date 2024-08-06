using AutoMapper;
using AutoMapper.QueryableExtensions;
using CIN.Application.Common;
using CIN.Application.OperationsMgtDtos;
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

    #region GetPagedList

    public class GetCityList : IRequest<PaginatedList<TblErpSysCityCodeDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetCityListHandler : IRequestHandler<GetCityList, PaginatedList<TblErpSysCityCodeDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public async Task<PaginatedList<TblErpSysCityCodeDto>> Handle(GetCityList request, CancellationToken cancellationToken)
        {

            var list = await _context.CityCodes.AsNoTracking()
                    //.Where(e => e.CompanyID == request.CompanyId || e.AccountId == request.AccountId)
                    .OrderBy(request.Input.OrderBy)
               .ProjectTo<TblErpSysCityCodeDto>(_mapper.ConfigurationProvider)
                  .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return list;
        }
    }

    #endregion


    #region GetSelectList

    public class GetSelectList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string City { get; set; }
    }

    public class GetSelectListHandler : IRequestHandler<GetSelectList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSelectListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetSelectList request, CancellationToken cancellationToken)
        {
            bool isArab = request.User.Culture.IsArab();
            var list = _context.CityCodes.AsNoTracking();

            if (request.City.HasValue())
                list = list.Where(e => e.CityName.Contains(request.City) || e.CityNameAr.Contains(request.City));
            //.Where(e => e.CompanyID == request.CompanyId || e.AccountId == request.AccountId)
            var cityList = await list.OrderByDescending(e => e.Id)
                           .Select(e => new CustomSelectListItem { Text = isArab ? e.CityNameAr : e.CityName, Value = e.CityCode })
                           .ToListAsync(cancellationToken);

            return cityList;
        }
    }

    #endregion   


    //#region CreateUpdate

    //public class CreateCity : IRequest<int>
    //{
    //    public UserIdentityDto User { get; set; }
    //    public TblErpSysCityCodeDto CityCodeDto { get; set; }
    //}

    //public class CreateCityQueryHandler : IRequestHandler<CreateCity, int>
    //{
    //    //private readonly ICurrentUserService _currentUserService;
    //    //protected string UserId => _currentUserService.UserId;
    //    private readonly CINDBOneContext _context;
    //    private readonly IMapper _mapper;

    //    public CreateCityQueryHandler(CINDBOneContext context, IMapper mapper)
    //    {
    //        _context = context;
    //        _mapper = mapper;
    //    }

    //    public async Task<int> Handle(CreateCity request, CancellationToken cancellationToken)
    //    {
    //        try
    //        {
    //            Log.Info("----Info SaveUpdateCity method start----");F

    //            var obj = request.CityCodeDto;
    //            TblErpSysCityCode City = new();
    //            if (obj.Id > 0)
    //                City = await _context.CityCodes.FirstOrDefaultAsync(e => e.Id == obj.Id);

    //            City.CityName = obj.CityName;
    //            //City.CountryCode = obj.CountryCode;
    //            //City.CountryCode = obj.CountryCode;

    //            if (obj.Id > 0)
    //            {
    //                _context.CityCodes.Update(City);
    //            }
    //            else
    //            {
    //                City.CityCode = obj.CityCode.ToUpper();
    //                await _context.CityCodes.AddAsync(City);
    //            }
    //            await _context.SaveChangesAsync();

    //            Log.Info("----Info SaveUpdateCity method Exit----");
    //            return City.Id;
    //        }
    //        catch (Exception ex)
    //        {
    //            Log.Error("Error in SaveUpdateCity Method");
    //            Log.Error("Error occured time : " + DateTime.UtcNow);
    //            Log.Error("Error message : " + ex.Message);
    //            Log.Error("Error StackTrace : " + ex.StackTrace);
    //            return 0;
    //        }
    //    }
    //}
    //#endregion

    //#region Delete
    //public class DeleteCity : IRequest<int>
    //{
    //    public UserIdentityDto User { get; set; }
    //    public int Id { get; set; }
    //}

    //public class DeleteCityQueryHandler : IRequestHandler<DeleteCity, int>
    //{
    //    //private readonly ICurrentUserService _currentUserService;
    //    //protected string UserId => _currentUserService.UserId;
    //    private readonly CINDBOneContext _context;
    //    private readonly IMapper _mapper;

    //    public DeleteCityQueryHandler(CINDBOneContext context, IMapper mapper)
    //    {
    //        _context = context;
    //        _mapper = mapper;
    //    }

    //    public async Task<int> Handle(DeleteCity request, CancellationToken cancellationToken)
    //    {
    //        try
    //        {
    //            Log.Info("----Info delte method start----");
    //            Log.Info("----Info delete method end----");

    //            if (request.Id > 0)
    //            {
    //                var City = await _context.CityCodes.FirstOrDefaultAsync(e => e.Id == request.Id);
    //                _context.Remove(City);
    //                await _context.SaveChangesAsync();
    //                return request.Id;
    //            }
    //            return 0;
    //        }
    //        catch (Exception ex)
    //        {
    //            Log.Error("Error in delete Method");
    //            Log.Error("Error occured time : " + DateTime.UtcNow);
    //            Log.Error("Error message : " + ex.Message);
    //            Log.Error("Error StackTrace : " + ex.StackTrace);
    //            return 0;
    //        }

    //    }
    //}

    //#endregion




    #region GetPagedList

    public class GetCityPagedList : IRequest<PaginatedList<TblErpSysCityCodeDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetCityPagedListHandler : IRequestHandler<GetCityPagedList, PaginatedList<TblErpSysCityCodeDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetCityPagedListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<TblErpSysCityCodeDto>> Handle(GetCityPagedList request, CancellationToken cancellationToken)
        {
            try
            {
                var search = request.Input.Query;
                var list = await _context.CityCodes.AsNoTracking()
                  .Where(e => (e.CityCode.Contains(search) || e.CityName.Contains(search) ||
                                    e.StateCode.Contains(search)
                                 ))
                   .OrderBy(request.Input.OrderBy)
                  .ProjectTo<TblErpSysCityCodeDto>(_mapper.ConfigurationProvider)
                     .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

                return list;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }

    #endregion

    #region SingleItem with state and country mapping getCityById

    public class GetCity : IRequest<CityStateCountryMappingDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetCityHandler : IRequestHandler<GetCity, CityStateCountryMappingDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetCityHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<CityStateCountryMappingDto> Handle(GetCity request, CancellationToken cancellationToken)
        {
            CityStateCountryMappingDto obj = new();
            var city = await _context.CityCodes.AsNoTracking().FirstOrDefaultAsync(e => e.Id == request.Id);
            obj.Id = city.Id;
            obj.CityName = city.CityName;
            obj.CityCode = city.CityCode;
            obj.IsActive = city.IsActive;
            if (city is not null)
            {
                var stateCode = city.StateCode;
                var state = await _context.StateCodes.AsNoTracking().FirstOrDefaultAsync(e => e.StateCode == stateCode);

                obj.StateCode = state.StateCode;
                obj.StateName = state.StateName;
                if (state is not null)
                {
                    var countryCode = state.CountryCode;
                    var country = await _context.CountryCodes.AsNoTracking().FirstOrDefaultAsync(e => e.CountryCode == countryCode);
                    obj.CountryCode = country.CountryCode;
                    obj.CountryName = country.CountryName;
                }
            }
            return obj;
        }
    }

    #endregion

    #region CreateCity

    public class CreateCity : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public TblErpSysCityCodeDto CityDto { get; set; }
    }

    public class CreateCityHandler : IRequestHandler<CreateCity, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateCityHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateCity request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info SaveUpdateCity method start----");


                var obj = request.CityDto;
                TblErpSysCityCode city = new();
                if (obj.Id > 0)
                    city = await _context.CityCodes.FirstOrDefaultAsync(e => e.Id == obj.Id);

                city.CityName = obj.CityName;
                city.CityCode = obj.CityCode.ToUpper();
                city.StateCode = obj.StateCode.ToUpper();
                city.IsActive = obj.IsActive;


                if (obj.Id > 0)
                {

                    _context.CityCodes.Update(city);
                }
                else
                {
                    await _context.CityCodes.AddAsync(city);
                }
                await _context.SaveChangesAsync();
                Log.Info("----Info SaveUpdateCompany method Exit----");
                return city.Id;
            }
            catch (Exception ex)
            {
                Log.Error("Error in SaveUpdateCity Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }
    }
    #endregion

    #region Delete City
    public class DeleteCity : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteCityHandler : IRequestHandler<DeleteCity, int>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteCityHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteCity request, CancellationToken cancellationToken)
        {

            try
            {
                Log.Info("----Info delte city method start----");

                if (request.Id > 0)
                {
                    var city = await _context.CityCodes.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(city);
                    await _context.SaveChangesAsync();

                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {

                Log.Error("Error in delete city Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }

    }

    #endregion

    #region GetSelectCityList

    public class GetSelectCityList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string CityCode { get; set; }
        public bool IsAutoComplete { get; set; }
        public string Search { get; set; }
    }

    public class GetSelectCityListHandler : IRequestHandler<GetSelectCityList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSelectCityListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetSelectCityList request, CancellationToken cancellationToken)
        {
            bool isArab = request.User.Culture.IsArab();
            Log.Info("----Info GetSelectCityList method start----");
            var obj = _context.CityCodes.Where(e => e.IsActive).AsNoTracking();
            if (request.IsAutoComplete)
                obj = obj.Where(e => e.CityCode == request.CityCode
            && (e.CityCode.Contains(request.Search) || e.CityName.Contains(request.Search)));

            var newObj = await obj
               .OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = isArab ? e.CityNameAr : e.CityName, Value = e.CityCode, TextTwo = e.StateCode })
                  .ToListAsync(cancellationToken);

            Log.Info("----Info GetSelectCityList method Ends----");
            return newObj;
        }
    }

    #endregion

    #region GetSelectStateList

    public class GetSelectStateList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string StateCode { get; set; }
        public bool IsAutoComplete { get; set; }
        public string Search { get; set; }
    }

    public class GetSelectStateListHandler : IRequestHandler<GetSelectStateList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSelectStateListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetSelectStateList request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetSelectStateList method start----");
            var obj = _context.StateCodes.AsNoTracking();
            if (request.IsAutoComplete)
                obj = obj.Where(e => e.StateCode == request.StateCode
            && (e.StateName.Contains(request.Search) || e.StateName.Contains(request.Search)));

            var newObj = await obj
               .OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = e.StateName, Value = e.StateCode, TextTwo = e.CountryCode })
                  .ToListAsync(cancellationToken);

            Log.Info("----Info GetSelectStateList method Ends----");
            return newObj;
        }
    }

    #endregion

    #region GetStatebyCityId

    public class GetStatebyCityId : IRequest<CustomSelectListItem>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetStatebyCityIdHandler : IRequestHandler<GetStatebyCityId, CustomSelectListItem>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetStatebyCityIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<CustomSelectListItem> Handle(GetStatebyCityId request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetSelectStateList method start----");
            var obj = await _context.StateCodes.AsNoTracking()
                .Where(e => e.StateCode == request.Input)
               .Select(e => new CustomSelectListItem { Text = e.StateName, Value = e.StateCode, TextTwo = e.CountryCode })
                  .FirstOrDefaultAsync(cancellationToken);
            Log.Info("----Info GetSelectStateList method Ends----");
            return obj;
        }
    }

    #endregion

    #region GetStateCountrybyCityCode

    public class GetStateCountrybyCityCode : IRequest<CityStateCountryMappingDto>
    {
        public UserIdentityDto User { get; set; }
        public string CityCode { get; set; }
    }

    public class GetStateCountrybyCityCodeHandler : IRequestHandler<GetStateCountrybyCityCode, CityStateCountryMappingDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetStateCountrybyCityCodeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<CityStateCountryMappingDto> Handle(GetStateCountrybyCityCode request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetStateCountrybyCityCode method start----");


            CityStateCountryMappingDto obj = new();
            try
            {
                bool isArab = request.User.Culture.IsArab();
                var city = await _context.CityCodes.AsNoTracking().FirstOrDefaultAsync(e => e.CityCode == request.CityCode);
                if (city is not null)
                {
                    obj.Id = city.Id;
                    obj.CityName = city.CityName;
                    obj.CityNameAr = city.CityNameAr;
                    obj.CityCode = city.CityCode;
                    obj.IsActive = city.IsActive;
                    if (city is not null)
                    {
                        var stateCode = city.StateCode;
                        var state = await _context.StateCodes.AsNoTracking().FirstOrDefaultAsync(e => e.StateCode == stateCode);

                        obj.StateCode = state.StateCode;
                        obj.StateName = state.StateName;
                        if (state is not null)
                        {
                            var countryCode = state.CountryCode;
                            var country = await _context.CountryCodes.AsNoTracking().FirstOrDefaultAsync(e => e.CountryCode == countryCode);
                            obj.CountryCode = country.CountryCode;
                            obj.CountryName = country.CountryName;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            Log.Info("----Info GetStateCountrybyCityCode method Ends----");
            return obj;
        }
    }

    #endregion

    #region GetCountrybyStateCode

    public class GetCountrybyStateCode : IRequest<CustomSelectListItem>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetCountrybyStateCodeHandler : IRequestHandler<GetCountrybyStateCode, CustomSelectListItem>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetCountrybyStateCodeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<CustomSelectListItem> Handle(GetCountrybyStateCode request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetCountrybyStateCode method start----");
            var obj = await _context.CountryCodes.AsNoTracking()
                .Where(e => e.CountryCode == request.Input)
               .Select(e => new CustomSelectListItem { Text = e.CountryName, Value = e.CountryName, TextTwo = e.CountryCode })
                  .FirstOrDefaultAsync(cancellationToken);
            Log.Info("----Info GetCountrybyStateCode method Ends----");
            return obj;
        }
    }

    #endregion

    #region GetStatesByCountry

    public class GetStatesByCountry : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string CountryCode { get; set; }
    }

    public class GetStatesByCountryHandler : IRequestHandler<GetStatesByCountry, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetStatesByCountryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetStatesByCountry request, CancellationToken cancellationToken)
        {
            bool isArab = request.User.Culture.IsArab();
            var list = _context.StateCodes.AsNoTracking();
            if (request.CountryCode.HasValue())
                list = list.Where(e => e.CountryCode.Contains(request.CountryCode));
            var statesSelectListItem = await list.OrderByDescending(e => e.Id)
                           .Select(e => new CustomSelectListItem { Text = e.StateName, Value = e.StateCode })
                           .ToListAsync(cancellationToken);

            return statesSelectListItem;
        }
    }

    #endregion

    #region GetCitesByState

    public class GetCitiesByState : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string StateCode { get; set; }
    }

    public class GetCitiesByStateHandler : IRequestHandler<GetCitiesByState, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetCitiesByStateHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetCitiesByState request, CancellationToken cancellationToken)
        {
            bool isArab = request.User.Culture.IsArab();
            var list = _context.CityCodes.AsNoTracking();
            if (request.StateCode.HasValue())
                list = list.Where(e => e.StateCode.Contains(request.StateCode));
            var citiesSelectListItem = await list.OrderByDescending(e => e.Id)
                           .Select(e => new CustomSelectListItem { Text = isArab ? e.CityNameAr : e.CityName, Value = e.CityCode })
                           .ToListAsync(cancellationToken);

            return citiesSelectListItem;
        }
    }

    #endregion   
}
