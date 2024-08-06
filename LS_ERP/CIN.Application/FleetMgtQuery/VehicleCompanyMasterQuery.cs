using AutoMapper;
using CIN.Application;
using CIN.Application.Common;
using CIN.Application.FleetMgtDtos;
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
using CIN.Domain.FleetMgt;

namespace CIN.Application.FleetMgtQuery
{
    #region GetAll
    public class GetVehicleCompanyMasterList : IRequest<PaginatedList<VehicleCompanyMasterDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }

    }

    public class GetVehicleCompanyMasterListHandler : IRequestHandler<GetVehicleCompanyMasterList, PaginatedList<VehicleCompanyMasterDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetVehicleCompanyMasterListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<VehicleCompanyMasterDto>> Handle(GetVehicleCompanyMasterList request, CancellationToken cancellationToken)
        {


            var list = await _context.VehicleCompanyMaster.AsNoTracking().ProjectTo<VehicleCompanyMasterDto>(_mapper.ConfigurationProvider)
                                    .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return list;
        }


    }


    #endregion

    #region GetById

    public class GetVehicleCompanyMasterById : IRequest<VehicleCompanyMasterDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetVehicleCompanyMasterByIdHandler : IRequestHandler<GetVehicleCompanyMasterById, VehicleCompanyMasterDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetVehicleCompanyMasterByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<VehicleCompanyMasterDto> Handle(GetVehicleCompanyMasterById request, CancellationToken cancellationToken)
        {

            VehicleCompanyMasterDto obj = new();
            var CompanyMaster = await _context.VehicleCompanyMaster.AsNoTracking().ProjectTo<VehicleCompanyMasterDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.Id == request.Id);
            return CompanyMaster;
            // throw new NotImplementedException();
        }
    }
    #endregion

    #region Create_And_Update


    public class CreateUpdateVehicleCompanyMaster : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public VehicleCompanyMasterDto VehicleCompanyDto { get; set; }
    }

    public class CreateUpdateVehicleCompanyMasterHandler : IRequestHandler<CreateUpdateVehicleCompanyMaster, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateVehicleCompanyMasterHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateUpdateVehicleCompanyMaster request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info CreateUpdateVehicleCompanyMaster method start----");

                var obj = request.VehicleCompanyDto;


                TblVehicleCompanyMaster VehicleCompanies = new();
                if (obj.Id > 0)
                    VehicleCompanies = await _context.VehicleCompanyMaster.AsNoTracking().FirstOrDefaultAsync(e=>e.Id==obj.Id);


                VehicleCompanies.Id = obj.Id;
                VehicleCompanies.VehicleCompany = obj.VehicleCompany;
                VehicleCompanies.VehicleCompany_Ar = obj.VehicleCompany_Ar;
                VehicleCompanies.IsActive = obj.IsActive;
                VehicleCompanies.CreatedOn = DateTime.Now;
                VehicleCompanies.CreatedBy = obj.CreatedBy;
                

                if (obj.Id > 0)
                {

                    _context.VehicleCompanyMaster.Update(VehicleCompanies);
                }
                else
                {
                    
                    await _context.VehicleCompanyMaster.AddAsync(VehicleCompanies);
                }
                await _context.SaveChangesAsync();
                Log.Info("----Info CreateUpdateVehicleCompanyMaster method Exit----");
                return VehicleCompanies.Id;
            }
            catch (Exception ex)
            {
                Log.Error("Error in CreateUpdateVehicleCompanyMaster Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }


    }



    #endregion

    #region Delete
    public class DeleteVehicleCompanyMaster : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteVehicleCompanyMasterHandler : IRequestHandler<DeleteVehicleCompanyMaster, int>
    {
        
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteVehicleCompanyMasterHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteVehicleCompanyMaster request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info DeleteVehicleCompanyMaster start----");

                if (request.Id > 0)
                {
                    var company = await _context.VehicleCompanyMaster.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(company);

                    await _context.SaveChangesAsync();

                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {

                Log.Error("Error in DeleteVehicleCompanyMaster");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }

        }
    }

    #endregion
}
