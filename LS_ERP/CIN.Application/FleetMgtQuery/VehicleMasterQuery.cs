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

    public class GetVehicleMasterList : IRequest<PaginatedList<VehicleMasterDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }

    }

    public class GetVehicleMasterListHandler : IRequestHandler<GetVehicleMasterList, PaginatedList<VehicleMasterDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetVehicleMasterListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<VehicleMasterDto>> Handle(GetVehicleMasterList request, CancellationToken cancellationToken)
        {


            var list = await _context.VehicleMaster.AsNoTracking().ProjectTo<VehicleMasterDto>(_mapper.ConfigurationProvider)
                                    .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return list;
        }


    }
    #endregion

    #region Update_Create
    public class CreateUpdateVehicleMaster : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public VehicleMasterDto Input { get; set; }
    }

    public class CreateUpdateVehicleMasterHandler : IRequestHandler<CreateUpdateVehicleMaster, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateVehicleMasterHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateUpdateVehicleMaster request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info Create Update Vehicle Master method start----");

                var obj = request.Input;


                TblVehicleMaster VehicleMaster = new();
                if (obj.Id > 0)
                    VehicleMaster = await _context.VehicleMaster.AsNoTracking().FirstOrDefaultAsync(e => e.Id == obj.Id);


                VehicleMaster.Id = obj.Id;
                VehicleMaster.RegistrationNumber = obj.RegistrationNumber;
                VehicleMaster.VehicleCompany = obj.VehicleCompany;
                VehicleMaster.VehicleType = obj.VehicleType;
                VehicleMaster.Brand = obj.Brand;
                VehicleMaster.ChassisNumber = obj.ChassisNumber;
                VehicleMaster.SeatingCapacity = obj.SeatingCapacity;
                VehicleMaster.RegisteredRTORegion = obj.RegisteredRTORegion;
                VehicleMaster.RegistrationAuthority = obj.RegistrationAuthority;
                VehicleMaster.VehicleValidityTill = obj.VehicleValidityTill;
                VehicleMaster.VehicleCondition = obj.VehicleCondition;
                VehicleMaster.VehicleOwnership = obj.VehicleOwnership;
                VehicleMaster.ProcurementLeasedOn = obj.ProcurementLeasedOn;
                VehicleMaster.CurrentBookValue = obj.CurrentBookValue;
                VehicleMaster.AnnualLeaseValue = obj.AnnualLeaseValue;
                VehicleMaster.SalvageBookValue = obj.SalvageBookValue;
                VehicleMaster.LeaseEndDate = obj.LeaseEndDate;
                VehicleMaster.VehicleOwnerEnglish = obj.VehicleOwnerEnglish;
                VehicleMaster.VehicleOwnerArabic = obj.VehicleOwnerArabic;
                VehicleMaster.MeterReadingOnProcurement = obj.MeterReadingOnProcurement;
                VehicleMaster.CurrentMeterReading = obj.CurrentMeterReading;
                VehicleMaster.VehicleNextMaintenanceDate = obj.VehicleNextMaintenanceDate;
                VehicleMaster.EstimatedMileagePerKM = obj.EstimatedMileagePerKM;
                VehicleMaster.VehicleLastMaintenanceDate = obj.VehicleLastMaintenanceDate;
                VehicleMaster.EstimatedServiceYear = obj.EstimatedServiceYear;
                VehicleMaster.FuelType = obj.FuelType;
                VehicleMaster.FuelTankCapacityInLitters = obj.FuelTankCapacityInLitters;
                VehicleMaster.IsActive = obj.IsActive;
                VehicleMaster.IsVehicleGoodsCarrier = obj.IsVehicleGoodsCarrier;
                VehicleMaster.CreatedOn = obj.CreatedOn;
                VehicleMaster.CreatedBy = obj.CreatedBy;


                if (obj.Id > 0)
                {

                    _context.VehicleMaster.Update(VehicleMaster);
                }
                else
                {

                    await _context.VehicleMaster.AddAsync(VehicleMaster);
                }
                await _context.SaveChangesAsync();
                Log.Info("----Info CreateUpdate VehicleMaster method Exit----");
                return VehicleMaster.Id;
            }
            catch (Exception ex)
            {
                Log.Error("Error in CreateUpdateVehicleMaster Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }


    }
    #endregion

    #region GetById
    public class GetVehicleMasterById : IRequest<VehicleMasterDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetVehicleMasterByIdHandler : IRequestHandler<GetVehicleMasterById, VehicleMasterDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetVehicleMasterByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<VehicleMasterDto> Handle(GetVehicleMasterById request, CancellationToken cancellationToken)
        {

            VehicleMasterDto obj = new();
            var vehicleMaster = await _context.VehicleMaster.AsNoTracking().ProjectTo<VehicleMasterDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.Id == request.Id);
            return vehicleMaster;
            // throw new NotImplementedException();
        }
    }
    #endregion

    #region Delete

    public class DeleteVehicleMaster : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteVehicleMasterHandler : IRequestHandler<DeleteVehicleMaster, int>
    {

        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteVehicleMasterHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteVehicleMaster request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info Delete Brand Master start----");

                if (request.Id > 0)
                {
                    var vehicleMaster = await _context.VehicleMaster.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(vehicleMaster);

                    await _context.SaveChangesAsync();

                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {

                Log.Error("Error in DeleteBrandMaster");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }

        }
    }

    #endregion
}
