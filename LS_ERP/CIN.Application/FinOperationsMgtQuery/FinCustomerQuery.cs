using AutoMapper;
using AutoMapper.QueryableExtensions;
using CIN.Application.Common;
using CIN.Application.FinanceMgtDtos;
using CIN.Application.FinPurchaseMgtDto;
using CIN.Application.InvoiceDtos;
using CIN.Application.OperationsMgtDtos;
using CIN.DB;
using CIN.Domain.FinanceMgt;
using CIN.Domain.OpeartionsMgt;
using CIN.Domain.SystemSetup;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;

namespace CIN.Application.OperationsMgtQuery
{
    #region GetCustomersCustomList

    public class GetCustomersCustomList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public bool? IsPayment { get; set; }
    }

    public class GetCustomersCustomListHandler : IRequestHandler<GetCustomersCustomList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetCustomersCustomListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetCustomersCustomList request, CancellationToken cancellationToken)
        {

            Log.Info("----Info GetCustomersCustomList method start----");
            var list = _context.OprCustomers.Where(e => e.IsActive);

            if (request.IsPayment is null)
                list = list.Where(e => !e.CustOnHold);

            var newList = await list.AsNoTracking()
            .OrderBy(e => e.Id)
                 .Select(e => new CustomSelectListItem { Text = e.CustName, TextTwo = e.CustArbName, Value = e.Id.ToString() })
                  .ToListAsync(cancellationToken);
            Log.Info("----Info GetCustomersCustomList method Ends----");
            return newList;
        }
    }


    #endregion


    #region GetLanDepartmentCustomList

    public class GetLanDepartmentCustomList : IRequest<List<LanCustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Search { get; set; }
    }

    public class GetLanDepartmentCustomListHandler : IRequestHandler<GetLanDepartmentCustomList, List<LanCustomSelectListItem>>
    {
        private readonly DMCContext _context;
        private readonly IMapper _mapper;
        public GetLanDepartmentCustomListHandler(DMCContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<LanCustomSelectListItem>> Handle(GetLanDepartmentCustomList request, CancellationToken cancellationToken)
        {

            Log.Info("----Info GetLanDepartmentCustomList method start----");
            var list = _context.HRM_DEF_Departments.Where(e => e.IsActive);

            if (request.Search.HasValue())
                list = list.Where(e => e.DepartmentName_EN.Contains(request.Search)
                                      || e.DepartmentName_AR.Contains(request.Search));

            var newList = await list.AsNoTracking()
                .OrderBy(e => e.DepartmentID)
                 .Select(e => new LanCustomSelectListItem
                 {
                     Text = e.DepartmentName_EN,
                     TextAr = e.DepartmentName_AR,
                     TextTwo = e.DepartmentID.ToString(),
                     Value = e.DepartmentID.ToString()
                 })
                  .ToListAsync(cancellationToken);
            Log.Info("----Info GetLanDepartmentCustomList method Ends----");
            return newList;
        }
    }


    #endregion


    #region GetLanEmployeeCustomList

    public class GetLanEmployeeCustomList : IRequest<List<LanCustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Search { get; set; }
    }

    public class GetLanEmployeeCustomListHandler : IRequestHandler<GetLanEmployeeCustomList, List<LanCustomSelectListItem>>
    {
        private readonly DMCContext _context;
        private readonly IMapper _mapper;
        public GetLanEmployeeCustomListHandler(DMCContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<LanCustomSelectListItem>> Handle(GetLanEmployeeCustomList request, CancellationToken cancellationToken)
        {

            Log.Info("----Info GetLanEmployeeCustomList method start----");
            var list = _context.HRM_TRAN_Employees.Where(e => (bool)e.IsActive);

            if (request.Search.HasValue())
                list = list.Where(e => e.EmployeeName.Contains(request.Search)
                                      || e.EmployeeName_AR.Contains(request.Search));

            var newList = await list.AsNoTracking()
                .OrderBy(e => e.EmployeeID)
                 .Select(e => new LanCustomSelectListItem
                 {
                     Text = e.EmployeeName,
                     TextAr = e.EmployeeName_AR,
                     TextTwo = e.EmployeeID.ToString(),
                     Value = e.EmployeeID.ToString()
                 })
                  .ToListAsync(cancellationToken);
            Log.Info("----Info GetLanEmployeeCustomList method Ends----");
            return newList;
        }
    }


    #endregion


    #region GetLanCustomersCustomList

    public class GetLanCustomersCustomList : IRequest<List<LanCustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public bool? IsPayment { get; set; }
        public string Search { get; set; }
    }

    public class GetLanCustomersCustomListHandler : IRequestHandler<GetLanCustomersCustomList, List<LanCustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetLanCustomersCustomListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<LanCustomSelectListItem>> Handle(GetLanCustomersCustomList request, CancellationToken cancellationToken)
        {

            Log.Info("----Info GetLanCustomersCustomList method start----");
            var list = _context.OprCustomers.Where(e => e.IsActive);

            if (request.Search.HasValue())
                list = list.Where(e => e.CustName.Contains(request.Search)
                                      || e.CustArbName.Contains(request.Search) || e.CustCode.Contains(request.Search));

            if (request.IsPayment is null)
                list = list.Where(e => !e.CustOnHold);

            var newList = await list.AsNoTracking()
                .OrderBy(e => e.Id)
                 .Select(e => new LanCustomSelectListItem { Text = e.CustName, TextAr = e.CustArbName, TextTwo = e.CustCode, Value = e.Id.ToString() })
                  .ToListAsync(cancellationToken);
            Log.Info("----Info GetLanCustomersCustomList method Ends----");
            return newList;
        }
    }


    #endregion

    #region GetCustomerItem

    public class GetCustomerItem : IRequest<TblOprCustomerMasterDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
        public string SiteCode { get; set; }
    }

    public class GetCustomerItemHandler : IRequestHandler<GetCustomerItem, TblOprCustomerMasterDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetCustomerItemHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblOprCustomerMasterDto> Handle(GetCustomerItem request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetCustomerItem method start----");
            var obj = await _context.OprCustomers.AsNoTracking()
                 .Where(e => e.Id == request.Id)
               .ProjectTo<TblOprCustomerMasterDto>(_mapper.ConfigurationProvider)
                  .FirstOrDefaultAsync(cancellationToken);

            if (request.SiteCode.HasValue())
            {
                var site = await _context.OprSites.FirstOrDefaultAsync(e => e.SiteCode == request.SiteCode);
                if (site is not null)
                {
                    obj.CustAddress1 = site.SiteAddress;
                    //obj.CompanyAddressAr = site.SiteAddress;
                    obj.CustName = site.SiteName;
                    obj.CustArbName = site.SiteArbName;
                    obj.VATNumber = site.VATNumber;

                }
            }

            Log.Info("----Info GetCustomerItem method Ends----");
            return obj;
        }
    }


    #endregion


    #region GetCustomerCodeSelectList

    public class GetCustomerCodeSelectList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetCustomerCodeSelectListHandler : IRequestHandler<GetCustomerCodeSelectList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetCustomerCodeSelectListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetCustomerCodeSelectList request, CancellationToken cancellationToken)
        {
            var search = request.Input;
            var list = await _context.OprCustomers
                .Where(e => e.IsActive)
                .AsNoTracking()
                .Where(e => e.CustName.Contains(search) || e.CustCode.Contains(search) || e.CustArbName.Contains(search))
              .Select(e => new CustomSelectListItem { Text = e.CustCode, TextTwo = e.CustArbName, Value = e.CustCode })
                 .ToListAsync(cancellationToken);
            return list;
        }
    }

    #endregion


    #region GetCustomerSitesSelectList

    public class GetCustomerSitesSelectList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
        public string CustCode { get; set; }
    }

    public class GetCustomerSitesSelectListHandler : IRequestHandler<GetCustomerSitesSelectList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetCustomerSitesSelectListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetCustomerSitesSelectList request, CancellationToken cancellationToken)
        {
            var id = request.Id;
            var customerCode = request.CustCode.HasValue() ? request.CustCode : await _context.OprCustomers.Where(e => e.Id == id).Select(e => e.CustCode).FirstOrDefaultAsync();

            var list = await _context.OprSites
                .Where(e => e.IsActive)
                .AsNoTracking()
                .Where(e => e.CustomerCode == customerCode)
              .Select(e => new CustomSelectListItem { Text = e.SiteName, TextTwo = e.SiteArbName, Value = e.SiteCode })
                 .ToListAsync(cancellationToken);
            return list;
        }
    }

    #endregion


    #region GetVendorItem

    public class GetVendorItem : IRequest<TblSndDefVendorMasterDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetVendorItemHandler : IRequestHandler<GetVendorItem, TblSndDefVendorMasterDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetVendorItemHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblSndDefVendorMasterDto> Handle(GetVendorItem request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetVendorItem method start----");
            var list = await _context.VendorMasters.AsNoTracking()
                 .Where(e => e.Id == request.Id)
               .ProjectTo<TblSndDefVendorMasterDto>(_mapper.ConfigurationProvider)
                  .FirstOrDefaultAsync(cancellationToken);
            Log.Info("----Info GetVendorItem method Ends----");
            return list;
        }
    }


    #endregion
}
