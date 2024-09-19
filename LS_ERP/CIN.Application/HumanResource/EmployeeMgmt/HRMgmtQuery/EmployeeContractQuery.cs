using AutoMapper;
using AutoMapper.QueryableExtensions;
using CIN.Application.Common;
using CIN.Application.HumanResource.EmployeeMgmt.HRMgmtDtos;
using CIN.Application.HumanResource.Utility;
using CIN.DB;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CIN.Application.HumanResource.EmployeeMgmt.HRMgmtQuery
{
    #region GetEmployeeContractInformationById

    public class GetEmployeeContractInformationById : IRequest<TblHRMTrnEmployeeContractInfoDto>
    {
        public UserIdentityDto User { get; set; }
        public int EmployeeID { get; set; }
    }

    public class GetEmployeeContractInformationByIdHandler : IRequestHandler<GetEmployeeContractInformationById, TblHRMTrnEmployeeContractInfoDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetEmployeeContractInformationByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TblHRMTrnEmployeeContractInfoDto> Handle(GetEmployeeContractInformationById request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetEmployeeContractInformationById method start----");
            try
            {
                var contractInformation = await _context.EmployeeContracts.AsNoTracking()
                    .ProjectTo<TblHRMTrnEmployeeContractInfoDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(e => e.EmployeeID == request.EmployeeID);

                Log.Info("----Info GetEmployeeContractInformationById method end----");

                if (contractInformation is not null)
                    return contractInformation;
                else
                    return null;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetEmployeeContractInformationById Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region GetEmployeeListByFilters

    public class GetEmployeeListByFilters : IRequest<List<TblHRMTrnEmployeeContractInfoDto>>
    {
        public UserIdentityDto User { get; set; }
        public EmployeeFilterDto Input { get; set; }
    }

    public class GetEmployeeListByFiltersHandler : IRequestHandler<GetEmployeeListByFilters, List<TblHRMTrnEmployeeContractInfoDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetEmployeeListByFiltersHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<TblHRMTrnEmployeeContractInfoDto>> Handle(GetEmployeeListByFilters request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info GetEmployeeListByFilters method start----");
                var search = request.Input;
                bool isArab = request.User.Culture.IsArab();
                var employeeList = await _context.EmployeeContracts.AsNoTracking()
                    .ProjectTo<TblHRMTrnEmployeeContractInfoDto>(_mapper.ConfigurationProvider)
                    .Where(e =>
                    (search.EmployeeID == 0 || e.EmployeeID == search.EmployeeID) &&
                    (string.IsNullOrEmpty(search.BranchCode) || e.BranchCode.Contains(search.BranchCode)) &&
                    (string.IsNullOrEmpty(search.PayrollGroupCode) || e.PayrollGroupCode.Contains(search.PayrollGroupCode)) &&
                    (e.EmployeeStatusCode == (string.IsNullOrEmpty(search.EmployeeStatusCode) ? EmployeeStatus.ACTIVE : search.EmployeeStatusCode))
                    )
                    .ToListAsync(cancellationToken);

                Log.Info("----Info GetEmployeeListByFilters method end----");
                return employeeList;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetEmployeeListByFilters Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion
}
