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
    public class GetBrandMasterList : IRequest<PaginatedList<VehicleBrandMasterDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }

    }

    public class GetBrandMasterListHandler : IRequestHandler<GetBrandMasterList, PaginatedList<VehicleBrandMasterDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetBrandMasterListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<VehicleBrandMasterDto>> Handle(GetBrandMasterList request, CancellationToken cancellationToken)
        {


            var list = await _context.VehicleBrandMaster.AsNoTracking().ProjectTo<VehicleBrandMasterDto>(_mapper.ConfigurationProvider)
                                    .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return list;
        }


    }


    #endregion

    #region GetById

    public class GetBrandMasterById : IRequest<VehicleBrandMasterDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetBrandMasterByIdHandler : IRequestHandler<GetBrandMasterById, VehicleBrandMasterDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetBrandMasterByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<VehicleBrandMasterDto> Handle(GetBrandMasterById request, CancellationToken cancellationToken)
        {

            VehicleCompanyMasterDto obj = new();
            var brandMaster = await _context.VehicleBrandMaster.AsNoTracking().ProjectTo<VehicleBrandMasterDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.Id == request.Id);
            return brandMaster;
            // throw new NotImplementedException();
        }
    }
    #endregion

    #region Create_And_Update


    public class CreateUpdateBrandMaster : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public VehicleBrandMasterDto BrandMasterDto { get; set; }
    }

    public class CreateUpdateBrandMasterHandler : IRequestHandler<CreateUpdateBrandMaster, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateBrandMasterHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateUpdateBrandMaster request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info CreateUpdate Brand Master method start----");

                var obj = request.BrandMasterDto;


                TblVehicleBrandMaster BrandMaster = new();
                if (obj.Id > 0)
                    BrandMaster = await _context.VehicleBrandMaster.AsNoTracking().FirstOrDefaultAsync(e => e.Id == obj.Id);


                BrandMaster.Id = obj.Id;
                BrandMaster.Brand = obj.Brand;
                BrandMaster.VehicleCompany = obj.VehicleCompany;
                BrandMaster.VehicleType = obj.VehicleType;
                BrandMaster.IsActive = obj.IsActive;
                BrandMaster.CreatedOn = DateTime.Now;
                BrandMaster.CreatedBy = obj.CreatedBy;


                if (obj.Id > 0)
                {

                    _context.VehicleBrandMaster.Update(BrandMaster);
                }
                else
                {

                    await _context.VehicleBrandMaster.AddAsync(BrandMaster);
                }
                await _context.SaveChangesAsync();
                Log.Info("----Info CreateUpdateBrandMaster method Exit----");
                return BrandMaster.Id;
            }
            catch (Exception ex)
            {
                Log.Error("Error in CreateUpdateBrandMaster Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }


    }



    #endregion

    #region Delete
    public class DeleteBrandMaster : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteBrandMasterHandler : IRequestHandler<DeleteBrandMaster, int>
    {

        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteBrandMasterHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteBrandMaster request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info Delete Brand Master start----");

                if (request.Id > 0)
                {
                    var brand = await _context.VehicleBrandMaster.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(brand);

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
