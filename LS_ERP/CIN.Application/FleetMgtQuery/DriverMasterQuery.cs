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
    public class GetDriverMasterList :IRequest<PaginatedList<DriverMasterDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetDriverMasterListHandler : IRequestHandler<GetDriverMasterList, PaginatedList<DriverMasterDto>>
    {
        private CINDBOneContext _context;
        private IMapper _mapper;

        public GetDriverMasterListHandler(CINDBOneContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

        }

        public async Task<PaginatedList<DriverMasterDto>> Handle(GetDriverMasterList request,CancellationToken cancellationToken)
        {
            var list = await _context.DriverMaster.AsNoTracking().ProjectTo<DriverMasterDto>(_mapper.ConfigurationProvider)
                    .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);
            return list;
        }
    }
    #endregion

    #region Create_Update

    public class CreateUpdateDriverMaster : IRequest<int>
    {
        public UserIdentityDto User { get; set; }

        public DriverMasterDto Input { get; set; }
    }
    public class CreateUpdateDriverMasterHandler : IRequestHandler<CreateUpdateDriverMaster, int>
    {
        private CINDBOneContext _context;
        private IMapper _mapper;

        public CreateUpdateDriverMasterHandler(CINDBOneContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateUpdateDriverMaster request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info Create Update Driver Master method start----");

                var obj = request.Input;


                TblDriverMaster DriverMaster = new();
                if(obj.Id> 0)
                    DriverMaster=await _context.DriverMaster.AsNoTracking().FirstOrDefaultAsync(e => e.Id == obj.Id);

                DriverMaster.Id = obj.Id;
                DriverMaster.DriverName = obj.DriverName;
                DriverMaster.DriverName_Ar = obj.DriverName_Ar;
                DriverMaster.IqamaNumber = obj.IqamaNumber;
                DriverMaster.LicenseNumber = obj.LicenseNumber;
                DriverMaster.Validity = obj.Validity.Date;
                DriverMaster.IsActive = obj.IsActive;
                DriverMaster.CreatedOn = DateTime.Now;
                DriverMaster.CreatedBy = obj.CreatedBy;

                if (obj.Id > 0)
                {

                    _context.DriverMaster.Update(DriverMaster);
                }
                else
                {

                    await _context.DriverMaster.AddAsync(DriverMaster);
                }
                await _context.SaveChangesAsync();
                Log.Info("----Info CreateUpdateDriverMaster method Exit----");
                return DriverMaster.Id;
            }
            catch(Exception ex)
            {
                Log.Error("Error in Create Update Driver Master Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;

            }
        }
    }

    #endregion

    #region GetById

    public class GetDriverMasterById : IRequest<DriverMasterDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetDriverMasterByIdHandler : IRequestHandler<GetDriverMasterById, DriverMasterDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetDriverMasterByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<DriverMasterDto> Handle(GetDriverMasterById request, CancellationToken cancellationToken)
        {

            VehicleCompanyMasterDto obj = new();
            var driverMaster = await _context.DriverMaster.AsNoTracking().ProjectTo<DriverMasterDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.Id == request.Id);
            return driverMaster;
            // throw new NotImplementedException();
        }
    }
    #endregion

    #region Delete
    public class DeleteDriverMaster : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteDriverMasterHandler : IRequestHandler<DeleteDriverMaster, int>
    {

        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteDriverMasterHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteDriverMaster request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info Delete Driver Master start----");

                if (request.Id > 0)
                {
                    var driverMaster = await _context.DriverMaster.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(driverMaster);

                    await _context.SaveChangesAsync();

                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {

                Log.Error("Error in Delete Driver Master");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }

        }
    }

    #endregion
}
