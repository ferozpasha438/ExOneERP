using AutoMapper;
using CIN.Application.Common;
using CIN.Application.OperationsMgtDtos;
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
using CIN.Domain.OpeartionsMgt;

namespace CIN.Application.OperationsMgtQuery
{
#region GetCustomerSitesPagedList
 
        public class GetCustomerSitesPagedList : IRequest<PaginatedList<TblSndDefSiteMasterDto>>
        {
            public UserIdentityDto User { get; set; }
            public PaginationFilterDto Input { get; set; }
        }

        public class GetCustomerSitesPagedListHandler : IRequestHandler<GetCustomerSitesPagedList, PaginatedList<TblSndDefSiteMasterDto>>
        {
            private readonly CINDBOneContext _context;
            private readonly IMapper _mapper;
            public GetCustomerSitesPagedListHandler(CINDBOneContext contextDMC, AutoMapper.IMapper mapper)
            {
                _context = contextDMC;
                _mapper = mapper;
            }
            public async Task<PaginatedList<TblSndDefSiteMasterDto>> Handle(GetCustomerSitesPagedList request, CancellationToken cancellationToken)
            {
                var search = request.Input.Query;
                var list = await _context.OprSites.AsNoTracking()
                  .Where(e =>  (e.SiteCityCode.Contains(search) ||
                                e.SiteArbName.Contains(search) ||
                                e.SiteName.Contains(search)||
                                e.SiteCode.Contains(search)||
                                e.CustomerCode.Contains(search)||
                               search == "" || search == null
                                 ))
                   .OrderBy(request.Input.OrderBy)
                  .ProjectTo<TblSndDefSiteMasterDto>(_mapper.ConfigurationProvider)
                     .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

                return list;
            }
        }
    #endregion

    #region GetCustomerSitesByCustCodePagedList

    public class GetCustomerSitesByCustCodePagedList : IRequest<PaginatedList<TblSndDefSiteMasterDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetCustomerSitesByCustCodePagedListHandler : IRequestHandler<GetCustomerSitesByCustCodePagedList, PaginatedList<TblSndDefSiteMasterDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetCustomerSitesByCustCodePagedListHandler(CINDBOneContext contextDMC, AutoMapper.IMapper mapper)
        {
            _context = contextDMC;
            _mapper = mapper;
        }
        public async Task<PaginatedList<TblSndDefSiteMasterDto>> Handle(GetCustomerSitesByCustCodePagedList request, CancellationToken cancellationToken)
        {
            var search = request.Input.Query;
            var list = await _context.OprSites.AsNoTracking()
              .Where(e => e.CustomerCode==request.Input.Query)
               .OrderBy(request.Input.OrderBy)
              .ProjectTo<TblSndDefSiteMasterDto>(_mapper.ConfigurationProvider)
                 .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return list;
        }
    }
    #endregion

    #region CreateUpdateSite
    public class CreateSite : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public TblSndDefSiteMasterDto SiteDto { get; set; }
    }

    public class CreateSiteHandler : IRequestHandler<CreateSite, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateSiteHandler(CINDBOneContext contextDMC, IMapper mapper)
        {
            _context = contextDMC;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateSite request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info CreateUpdateSite method start----");



                var obj = request.SiteDto;


                TblSndDefSiteMaster site = new();
                if (obj.Id > 0)
                    site = await _context.OprSites.FirstOrDefaultAsync(e => e.Id == obj.Id);
                else
                {


                     var st = await _context.OprSites.AsNoTracking().OrderByDescending(e => e.Id).FirstOrDefaultAsync();

                    if (st is not null)
                    {
                        site.SiteCode = "SITE" + (st.Id + 1).ToString().PadLeft(6, '0');
                    }
                    else
                        site.SiteCode = "SITE000001";


                    if (_context.OprSites.Any(x => x.SiteCode == site.SiteCode))
                    {
                        return -1;
                    }
                   // site.SiteCode = obj.SiteCode.ToUpper();
                }
              
                site.Id = obj.Id;
                //site.CreatedOn = obj.CreatedOn;
                site.IsActive = obj.IsActive;
              
                site.SiteName = obj.SiteName;
                site.SiteArbName = obj.SiteArbName;
                site.CustomerCode = obj.CustomerCode;
                site.SiteAddress = obj.SiteAddress;
                site.SiteCityCode = obj.SiteCityCode;
                site.SiteGeoLatitude = obj.SiteGeoLatitude;
                site.SiteGeoLongitude = obj.SiteGeoLongitude;
                site.SiteGeoGain = obj.SiteGeoGain;
                site.SiteGeoLatitudeMeter = 0;
                site.SiteGeoLongitudeMeter = 0;
                site.IsChildCustomer = obj.IsChildCustomer;
                site.StandardDeviation = obj.StandardDeviation;
                site.VATNumber = obj.IsChildCustomer ? obj.VATNumber : string.Empty;

                if (obj.Id > 0)
                {
                    site.ModifiedOn = DateTime.Now;
                    _context.OprSites.Update(site);
                }
                else
                {
                    site.CreatedOn = DateTime.Now;
                    await _context.OprSites.AddAsync(site);
                }
                await _context.SaveChangesAsync();
                Log.Info("----Info CreateUpdateSite method Exit----");
                return site.Id;
            }
            catch (Exception ex)
            {
                Log.Error("Error in CreateUpdateSite Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }
    }

#endregion

#region GetSiteBySiteCode
    public class GetSiteBySiteCode : IRequest<TblSndDefSiteMasterDto>
    {
        public UserIdentityDto User { get; set; }
        public string SiteCode { get; set; }
    }

    public class GetSiteBySiteCodeHandler : IRequestHandler<GetSiteBySiteCode, TblSndDefSiteMasterDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSiteBySiteCodeHandler(CINDBOneContext contextDMC, IMapper mapper)
        {
            _context = contextDMC;
            _mapper = mapper;
        }
        public async Task<TblSndDefSiteMasterDto> Handle(GetSiteBySiteCode request, CancellationToken cancellationToken)
        {
            TblSndDefSiteMasterDto obj = new();
            var site = await _context.OprSites.AsNoTracking().FirstOrDefaultAsync(e => e.SiteCode == request.SiteCode);
            if (site is not null)
            {
                obj.Id = site.Id;
                obj.SiteCode = site.SiteCode;
                obj.CustomerCode = site.CustomerCode;
                obj.SiteName = site.SiteName;
                obj.SiteArbName = site.SiteArbName;
                obj.SiteAddress = site.SiteAddress;
                obj.SiteCityCode = site.SiteCityCode;
                obj.SiteGeoLatitude = site.SiteGeoLatitude;
                obj.SiteGeoLongitude = site.SiteGeoLongitude;
                obj.SiteGeoGain = site.SiteGeoGain;
                obj.ModifiedOn = site.ModifiedOn;
                obj.CreatedOn = site.CreatedOn;
                obj.IsActive = site.IsActive;
                obj.StandardDeviation = site.StandardDeviation;
                return obj;
            }
            else return null;
        }
    }

    #endregion

#region GetSiteById
    public class GetSiteById : IRequest<TblSndDefSiteMasterDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetSiteByIdHandler : IRequestHandler<GetSiteById, TblSndDefSiteMasterDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSiteByIdHandler(CINDBOneContext contextDMC, IMapper mapper)
        {
            _context = contextDMC;
            _mapper = mapper;
        }
        public async Task<TblSndDefSiteMasterDto> Handle(GetSiteById request, CancellationToken cancellationToken)
        {
            TblSndDefSiteMasterDto obj = new();
            var site = await _context.OprSites.AsNoTracking().FirstOrDefaultAsync(e => e.Id == request.Id);
            if (site is not null)
            {
                obj.Id = site.Id;
                obj.SiteCode = site.SiteCode;
                obj.CustomerCode = site.CustomerCode;
                obj.SiteName = site.SiteName;
                obj.SiteArbName = site.SiteArbName;
                obj.SiteAddress = site.SiteAddress;
                obj.SiteCityCode = site.SiteCityCode;
                obj.SiteGeoLatitude = site.SiteGeoLatitude;
                obj.SiteGeoLongitude = site.SiteGeoLongitude;
                obj.SiteGeoGain = site.SiteGeoGain;
                obj.ModifiedOn = site.ModifiedOn;
                obj.CreatedOn = site.CreatedOn;
                obj.IsActive = site.IsActive;
                obj.IsChildCustomer = site.IsChildCustomer;
                obj.VATNumber = site.VATNumber;
                obj.StandardDeviation = site.StandardDeviation;

                return obj;
            }
            else return null;
        }
    }

    #endregion

    #region GetSelectSiteListbyCustCode

    public class GetSelectSiteListbyCustCode : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetSelectSiteListbyCustCodeHandler : IRequestHandler<GetSelectSiteListbyCustCode, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSelectSiteListbyCustCodeHandler(CINDBOneContext contextDMC, IMapper mapper)
        {
            _context = contextDMC;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetSelectSiteListbyCustCode request, CancellationToken cancellationToken)
        {
            var search = request.Input;
            var list = await _context.OprSites.AsNoTracking()
                .Where(e => e.CustomerCode.Contains(search))
              .Select(e => new CustomSelectListItem { Text = e.SiteName, Value = e.SiteCode.ToString(),TextTwo=e.SiteArbName})
                 .ToListAsync(cancellationToken);
            return list;
        }
    }

    #endregion

    #region GetSelectSiteListByCustCode

    public class GetSelectSiteListByCustCode : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Search { get; set; }


    }

    public class GetSelectSiteListByCustCodeHandler : IRequestHandler<GetSelectSiteListByCustCode, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSelectSiteListByCustCodeHandler(CINDBOneContext contextDMC, IMapper mapper)
        {
            _context = contextDMC;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetSelectSiteListByCustCode request, CancellationToken cancellationToken)
        {
            var search = request.Search;
            var list = await _context.OprSites.AsNoTracking()
                .Where(e => e.CustomerCode.Contains(search))
              .Select(e => new CustomSelectListItem { Text = e.SiteName, Value = e.SiteCode ,TextTwo=e.SiteArbName})
                 .ToListAsync(cancellationToken);
            return list;
        }
    }

    #endregion

    #region GetSelectSiteList

    public class GetSelectSiteList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetSelectSiteListHandler : IRequestHandler<GetSelectSiteList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSelectSiteListHandler(CINDBOneContext contextDMC, IMapper mapper)
        {
            _context = contextDMC;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetSelectSiteList request, CancellationToken cancellationToken)
        {
            var search = request.Input;
            var list = await _context.OprSites.AsNoTracking()
                .Where(e => e.SiteName.Contains(search)||e.SiteCode.Contains(search)||e.CustomerCode.Contains(search)||search==null)
              .Select(e => new CustomSelectListItem { Text = e.SiteName, Value = e.SiteCode.ToString()})
                 .ToListAsync(cancellationToken);
            return list;
        }
    }

    #endregion

    #region GetSelectSiteListByProjectCode

    public class GetSelectSiteListByProjectCode : IRequest<List<TblSndDefSiteMasterDto>>
    {
        public UserIdentityDto User { get; set; }
        public string ProjectCode { get; set; }
    }

    public class GetSelectSiteListByProjectCodeHandler : IRequestHandler<GetSelectSiteListByProjectCode, List<TblSndDefSiteMasterDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSelectSiteListByProjectCodeHandler(CINDBOneContext contextDMC, IMapper mapper)
        {
            _context = contextDMC;
            _mapper = mapper;
        }
        public async Task<List<TblSndDefSiteMasterDto>> Handle(GetSelectSiteListByProjectCode request, CancellationToken cancellationToken)
        {
            var sites = await _context.OprSites.AsNoTracking().ToListAsync();
            var enquiries = await _context.OprEnquiries.AsNoTracking().Where(e=>e.EnquiryNumber==request.ProjectCode).ToListAsync();
           List<TblSndDefSiteMasterDto> selectionSiteList =(from e in enquiries
                                    group e by e.SiteCode into sg
                                    join s in sites on sg.FirstOrDefault().SiteCode equals s.SiteCode
                                    select new TblSndDefSiteMasterDto
                                    {
                                        Id=s.Id,
                                        SiteCode=s.SiteCode,
                                        SiteName=s.SiteName,
                                        SiteArbName =s.SiteArbName,
                                        CustomerCode =s.CustomerCode,
                                        SiteAddress =s.SiteAddress,
                                        SiteCityCode=s.SiteCityCode,
                                        SiteGeoLatitude=s.SiteGeoLatitude,
                                        SiteGeoLongitude=s.SiteGeoLongitude,
                                        SiteGeoGain=s.SiteGeoGain
                                    }).ToList();

               
            return selectionSiteList;
        }
    }

    #endregion



    #region DeleteSite
    public class DeleteSite : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteSiteQueryHandler : IRequestHandler<DeleteSite, int>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteSiteQueryHandler(CINDBOneContext contextDMC, IMapper mapper)
        {
            _context = contextDMC;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteSite request, CancellationToken cancellationToken)
        {
            try
                {
                    Log.Info("----Info DeleteSite start----");

                    if (request.Id > 0)
                    {
                        var site = await _context.OprSites.FirstOrDefaultAsync(e => e.Id == request.Id);
                        _context.Remove(site);
                       
                        await _context.SaveChangesAsync();
                      
                        return request.Id;
                    }
                    return 0;
                }
                catch (Exception ex)
                {
                    
                    Log.Error("Error in DeleteSite");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return 0;
                }
            
        }
    }

    #endregion


    #region GetSelectSiteList2     //HRM Project sites

    public class GetSelectSiteList2 : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetSelectSiteList2Handler : IRequestHandler<GetSelectSiteList2, List<CustomSelectListItem>>
    {
        private readonly DMCContext _context;
        private readonly IMapper _mapper;
        public GetSelectSiteList2Handler(DMCContext contextDMC, IMapper mapper)
        {
            _context = contextDMC;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetSelectSiteList2 request, CancellationToken cancellationToken)
        {
            var search = request.Input;
            var list = await _context.HRM_DEF_Sites.GroupBy(x=>x.SiteCode)
                .Where(e => e.Key.Contains(search) || e.FirstOrDefault(k=>k.SiteCode==e.Key).SiteName_EN.Contains(search) || e.FirstOrDefault(k => k.SiteCode == e.Key).SiteName_AR.Contains(search) || search == null )
              .Select(e => new CustomSelectListItem { Text =e.Key+"/"+ e.FirstOrDefault(k => k.SiteCode == e.Key).SiteName_EN + "/"+ e.FirstOrDefault(k => k.SiteCode == e.Key).SiteName_AR, Value = e.Key.ToString() })
                 .ToListAsync(cancellationToken);
            return list;
        }
    }

    #endregion





    #region GetSitesPagedListWithFilter

    public class GetSitesPagedListWithFilter : IRequest<PaginatedList<TblSndDefSiteMasterDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
        public OprFilter FilterInput { get; set; }
    }

    public class GetSitesPagedListWithFilterHandler : IRequestHandler<GetSitesPagedListWithFilter, PaginatedList<TblSndDefSiteMasterDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSitesPagedListWithFilterHandler(CINDBOneContext context, AutoMapper.IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<TblSndDefSiteMasterDto>> Handle(GetSitesPagedListWithFilter request, CancellationToken cancellationToken)
        {
            try
            {
                bool isAdmin = _context.SystemLogins.AsNoTracking().FirstOrDefault(e => e.Id == request.User.UserId).LoginId == "Admin" ? true : false;

                Log.Info("----Info GetSitesPagedListWithFilter method start----");
                var oprAuths = _context.TblOpAuthoritiesList.AsNoTracking();
                var oprApprvls = _context.TblOprTrnApprovalsList.AsNoTracking();

                var search = request.Input.Query;

                var list = _context.OprSites.AsNoTracking()
                 .Where(e => (e.SiteCityCode.Contains(search) ||
                               e.SiteArbName.Contains(search) ||
                               e.SiteName.Contains(search) ||
                               e.SiteCode.Contains(search) ||
                               e.CustomerCode.Contains(search) ||
                              search == "" || search == null
                                ))
                  .OrderBy(request.Input.OrderBy)
                 .ProjectTo<TblSndDefSiteMasterDto>(_mapper.ConfigurationProvider);

                if (!string.IsNullOrEmpty(request.FilterInput.CustomerCode))
                {
                    list = list.Where(e => e.CustomerCode == request.FilterInput.CustomerCode);
                }
                if (!string.IsNullOrEmpty(request.FilterInput.BranchCode))
                {
                    list = list.Where(e => e.SiteCityCode == request.FilterInput.BranchCode);
                }
                

              


                var nreports = await list.OrderBy(request.Input.OrderBy).PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);
                return nreports;
            }

            catch (Exception ex)
            {
                Log.Error("Error in CreateUpdateProject Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return null;
            }
        }
    }
    #endregion
}
