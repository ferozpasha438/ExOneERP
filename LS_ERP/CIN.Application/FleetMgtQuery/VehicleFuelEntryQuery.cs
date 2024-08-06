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

    public class GetVehicleFuelEntryList : IRequest<PaginatedList<VehicleFuelEntryDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }

    }

    public class GetVehicleFuelEntryListHandler : IRequestHandler<GetVehicleFuelEntryList, PaginatedList<VehicleFuelEntryDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetVehicleFuelEntryListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<VehicleFuelEntryDto>> Handle(GetVehicleFuelEntryList request, CancellationToken cancellationToken)
        {


            var list = await _context.VehicleFuelEntry.AsNoTracking().ProjectTo<VehicleFuelEntryDto>(_mapper.ConfigurationProvider)
                                    .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return list;
        }


    }
    #endregion

    #region Update_Create
    public class CreateUpdateVehicleFuelEntry : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public VehicleFuelEntryDto VehicleFuelEntry { get; set; }
    }

    public class CreateUpdateVehicleFuelEntryHandler : IRequestHandler<CreateUpdateVehicleFuelEntry, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateVehicleFuelEntryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateUpdateVehicleFuelEntry request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info Create Update Vehicle Fuel Entry method start----");

                var obj = request.VehicleFuelEntry;


                TblVehicleFuelEntry FuelEntry = new();
                if (obj.Id > 0)
                    FuelEntry = await _context.VehicleFuelEntry.AsNoTracking().FirstOrDefaultAsync(e => e.Id == obj.Id);


                FuelEntry.Id = obj.Id;
                FuelEntry.VehicleNumber = obj.VehicleNumber;
                FuelEntry.FuelType = obj.FuelType;
                FuelEntry.FuelQuantity = obj.FuelQuantity;
                FuelEntry.FuellingDate = DateTime.Now;
                FuelEntry.Driver = obj.Driver;
                FuelEntry.DocumentNumber = obj.DocumentNumber;
                FuelEntry.ReadingKM = obj.ReadingKM;
                FuelEntry.Remarks = obj.Remarks;
                FuelEntry.IsActive = obj.IsActive;
                FuelEntry.CreatedOn = DateTime.Now;
                FuelEntry.CreatedBy = obj.CreatedBy;


                if (obj.Id > 0)
                {

                    _context.VehicleFuelEntry.Update(FuelEntry);
                }
                else
                {

                    await _context.VehicleFuelEntry.AddAsync(FuelEntry);
                }
                await _context.SaveChangesAsync();
                Log.Info("----Info Create Update Vehicle Fuel Entry method Exit----");
                return FuelEntry.Id;
            }
            catch (Exception ex)
            {
                Log.Error("Error in CreateUpdate Vehicle Fuel Entry Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }


    }
    #endregion

    #region GetById   
    public class GetVehicleFuelEntryById : IRequest<VehicleFuelEntryDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetVehicleFuelEntryByIdHandler : IRequestHandler<GetVehicleFuelEntryById, VehicleFuelEntryDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetVehicleFuelEntryByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<VehicleFuelEntryDto> Handle(GetVehicleFuelEntryById request, CancellationToken cancellationToken)
        {

            VehicleFuelEntryDto obj = new();
            var vehicleFuelEntry = await _context.VehicleFuelEntry.AsNoTracking().ProjectTo<VehicleFuelEntryDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.Id == request.Id);
            return vehicleFuelEntry;
            // throw new NotImplementedException();
        }
    }
    #endregion

    #region Delete

    public class DeleteVehicleFuelEntry : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteVehicleFuelEntryHandler : IRequestHandler<DeleteVehicleFuelEntry, int>
    {

        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteVehicleFuelEntryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteVehicleFuelEntry request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info Delete Vehicle Fuel Entry Start----");

                if (request.Id > 0)
                {
                    var fuelEntry = await _context.VehicleFuelEntry.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(fuelEntry);

                    await _context.SaveChangesAsync();

                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {

                Log.Error("Error in Delete Vehicle Fuel Entry");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }

        }
    }

    #endregion
}
