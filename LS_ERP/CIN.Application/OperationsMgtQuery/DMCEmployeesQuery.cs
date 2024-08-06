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


    #region GetDMCEmployeesPagedList

    public class GetDMCEmployeesPagedList : IRequest<PaginatedList<HRM_TRAN_EmployeeDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetDMCEmployeesPagedListHandler : IRequestHandler<GetDMCEmployeesPagedList, PaginatedList<HRM_TRAN_EmployeeDto>>
    {
        private readonly DMCContext _context;
        private readonly IMapper _mapper;
        public GetDMCEmployeesPagedListHandler(DMCContext context, AutoMapper.IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<HRM_TRAN_EmployeeDto>> Handle(GetDMCEmployeesPagedList request, CancellationToken cancellationToken)
        {
            var search = request.Input.Query;
            var list = await _context.HRM_TRAN_Employees.AsNoTracking()
              .Where(e => //e.CompanyId == request.CompanyId &&
                            (e.EmployeeID.ToString().Contains(search) ||
                            e.EmployeeNumber.Contains(search) ||
                            e.EmployeeName.Contains(search) ||
                            e.EmployeeName_AR.Contains(search) ||
                           search == "" || search == null

                             ))
               .OrderBy(request.Input.OrderBy)
              .ProjectTo<HRM_TRAN_EmployeeDto>(_mapper.ConfigurationProvider)
                 .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return list;
        }
    }
    #endregion













    #region GetDMCEmployeeByEmployeeNumber
    public class GetDMCEmployeeByEmployeeNumber : IRequest<HRM_TRAN_EmployeeDto>
    {
        public UserIdentityDto User { get; set; }
        public string EmployeeNumber { get; set; }
    }

    public class GetDMCEmployeeByEmployeeNumberHandler : IRequestHandler<GetDMCEmployeeByEmployeeNumber, HRM_TRAN_EmployeeDto>
    {
        private readonly DMCContext _context;

        private readonly IMapper _mapper;
        public GetDMCEmployeeByEmployeeNumberHandler(DMCContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<HRM_TRAN_EmployeeDto> Handle(GetDMCEmployeeByEmployeeNumber request, CancellationToken cancellationToken)
        {
            // HRM_TRAN_EmployeeDto obj = new();
            var Employee = await _context.HRM_TRAN_Employees.AsNoTracking().ProjectTo<HRM_TRAN_EmployeeDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.EmployeeNumber == request.EmployeeNumber);
            //if (Employee is not null)
            //{
            //    obj.Id = Employee.Id;
            //    obj.EmployeeCode = Employee.EmployeeCode;
            //    obj.EmployeeCode = Employee.EmployeeCode;
            //    obj.EmployeeNameEng = Employee.EmployeeNameEng;
            //    obj.EmployeeNameArb = Employee.EmployeeNameArb;
            //    obj.ModifiedOn = Employee.ModifiedOn;
            //    obj.CreatedOn = Employee.CreatedOn;
            //    obj.IsActive = Employee.IsActive;
            //    return obj;
            //}
            //else return null;
            return Employee;
        }
    }

    #endregion

    

    #region GetSelectDMCEmployeeList

    public class GetSelectDMCEmployeeList : IRequest<List<LanCustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetSelectDMCEmployeeListHandler : IRequestHandler<GetSelectDMCEmployeeList, List<LanCustomSelectListItem>>
    {
        private readonly DMCContext _context;
        private readonly IMapper _mapper;
        public GetSelectDMCEmployeeListHandler(DMCContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<LanCustomSelectListItem>> Handle(GetSelectDMCEmployeeList request, CancellationToken cancellationToken)
        {

            var list = await _context.HRM_TRAN_Employees.AsNoTracking()
              .Select(e => new LanCustomSelectListItem { Text = e.EmployeeName, Value = e.EmployeeNumber, TextTwo = e.EmployeeID.ToString(),TextAr=e.EmployeeName_AR })
                 .ToListAsync(cancellationToken);
            return list;
        }
    }

    #endregion


    #region GetAutoFillDMCEmployeeList

    public class GetAutoFillDMCEmployeeList : IRequest<List<HRM_TRAN_EmployeeDto>>
    {
        public UserIdentityDto User { get; set; }
        public string Search { get; set; }
    }

    public class GetAutoFillDMCEmployeeListHandler : IRequestHandler<GetAutoFillDMCEmployeeList, List<HRM_TRAN_EmployeeDto>>
    {
        private readonly DMCContext _context;
        private readonly IMapper _mapper;
        public GetAutoFillDMCEmployeeListHandler(DMCContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<HRM_TRAN_EmployeeDto>> Handle(GetAutoFillDMCEmployeeList request, CancellationToken cancellationToken)
        {

            var list = await _context.HRM_TRAN_Employees.AsNoTracking().ProjectTo<HRM_TRAN_EmployeeDto>(_mapper.ConfigurationProvider).Where(e=>e.EmployeeName.Contains(request.Search)||e.EmployeeNumber.Contains(request.Search)).ToListAsync(cancellationToken);
            return list;
        }
    }

    #endregion

    #region GetAutoSelectDMCEmployeeList

    public class GetAutoSelectDMCEmployeeList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Search { get; set; }
    }

    public class GetAutoSelectDMCEmployeeListHandler : IRequestHandler<GetAutoSelectDMCEmployeeList, List<CustomSelectListItem>>
    {
        private readonly DMCContext _context;
        private readonly IMapper _mapper;
        public GetAutoSelectDMCEmployeeListHandler(DMCContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetAutoSelectDMCEmployeeList request, CancellationToken cancellationToken)
        {

            var list = await _context.HRM_TRAN_Employees.AsNoTracking().Where(e => e.EmployeeName.Contains(request.Search) || e.EmployeeNumber.Contains(request.Search))
              .Select(e => new CustomSelectListItem { Text = e.EmployeeNumber, Value = e.EmployeeID.ToString(), TextTwo = e.EmployeeName })
                 .ToListAsync(cancellationToken);
            return list;
        }
    }

    #endregion

    

}
