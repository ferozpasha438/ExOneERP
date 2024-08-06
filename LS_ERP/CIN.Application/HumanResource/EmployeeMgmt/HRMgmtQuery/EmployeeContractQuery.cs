using AutoMapper;
using AutoMapper.QueryableExtensions;
using CIN.Application.Common;
using CIN.Application.HumanResource.EmployeeMgmt.HRMgmtDtos;
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
}
