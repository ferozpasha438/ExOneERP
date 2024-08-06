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
    public class GetVehicleTypeMasterList : IRequest<PaginatedList<VehicleTypeMasterDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }

    }

    public class GetVehicleTypeMasterListHandler : IRequestHandler<GetVehicleTypeMasterList, PaginatedList<VehicleTypeMasterDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetVehicleTypeMasterListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<VehicleTypeMasterDto>> Handle(GetVehicleTypeMasterList request, CancellationToken cancellationToken)
        {


            var list = await _context.VehicleTypeMaster.AsNoTracking().ProjectTo<VehicleTypeMasterDto>(_mapper.ConfigurationProvider)
                                    .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return list;
        }


    }


    #endregion

    #region GetById

    public class GetVehicleTypeMasterById : IRequest<VehicleTypeMasterDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetVehicleTypeMasterByIdHandler : IRequestHandler<GetVehicleTypeMasterById, VehicleTypeMasterDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetVehicleTypeMasterByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<VehicleTypeMasterDto> Handle(GetVehicleTypeMasterById request, CancellationToken cancellationToken)
        {

            VehicleCompanyMasterDto obj = new();
            var vehicleType = await _context.VehicleTypeMaster.AsNoTracking().ProjectTo<VehicleTypeMasterDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.Id == request.Id);
            return vehicleType;
            // throw new NotImplementedException();
        }
    }
    #endregion

    #region Create_And_Update


    public class CreateUpdateVehicleTypeMaster : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public VehicleTypeMasterDto VehicleTypeDto { get; set; }
    }

    public class CreateUpdateVehicleTypeMasterHandler : IRequestHandler<CreateUpdateVehicleTypeMaster, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateVehicleTypeMasterHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateUpdateVehicleTypeMaster request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info CreateUpdateVehicleTypeMaster method start----");

                var obj = request.VehicleTypeDto;


                TblVehicleTypeMaster VehicleTypes = new();
                if (obj.Id > 0)
                    VehicleTypes = await _context.VehicleTypeMaster.AsNoTracking().FirstOrDefaultAsync(e => e.Id == obj.Id);


                VehicleTypes.Id = obj.Id;
                VehicleTypes.VehicleType = obj.VehicleType;
                VehicleTypes.VehicleType_Ar = obj.VehicleType_Ar;
                VehicleTypes.IsActive = obj.IsActive;
                VehicleTypes.CreatedOn = DateTime.Now;
                VehicleTypes.CreatedBy = obj.CreatedBy;


                if (obj.Id > 0)
                {

                    _context.VehicleTypeMaster.Update(VehicleTypes);
                }
                else
                {

                    await _context.VehicleTypeMaster.AddAsync(VehicleTypes);
                }
                await _context.SaveChangesAsync();
                Log.Info("----Info CreateUpdateVehicleTypeMaster method Exit----");
                return VehicleTypes.Id;
            }
            catch (Exception ex)
            {
                Log.Error("Error in CreateUpdateVehicleTypeMaster Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }


    }



    #endregion

    #region Delete
    public class DeleteVehicleTypeMaster : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteVehicleTypeMasterHandler : IRequestHandler<DeleteVehicleTypeMaster, int>
    {

        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteVehicleTypeMasterHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteVehicleTypeMaster request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info DeleteVehicleCompanyMaster start----");

                if (request.Id > 0)
                {
                    var company = await _context.VehicleTypeMaster.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(company);

                    await _context.SaveChangesAsync();

                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {

                Log.Error("Error in DeleteVehicleTypeMaster");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }

        }
    }

    #endregion
}
