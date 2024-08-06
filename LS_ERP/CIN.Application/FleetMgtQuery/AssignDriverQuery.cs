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
    public class GetAssignDriverList : IRequest<PaginatedList<AssignDriversDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }

    }

    public class GetAssignDriverListHandler : IRequestHandler<GetAssignDriverList, PaginatedList<AssignDriversDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetAssignDriverListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<AssignDriversDto>> Handle(GetAssignDriverList request, CancellationToken cancellationToken)
        {


            var list = await _context.AssignDrivers.AsNoTracking().ProjectTo<AssignDriversDto>(_mapper.ConfigurationProvider)
                                    .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return list;
        }


    }


    #endregion

    #region GetById

    public class GetAssignDriverById : IRequest<AssignDriversDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetAssignDriverByIdHandler : IRequestHandler<GetAssignDriverById, AssignDriversDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetAssignDriverByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AssignDriversDto> Handle(GetAssignDriverById request, CancellationToken cancellationToken)
        {

            AssignDriversDto obj = new();
            var assignDriver = await _context.AssignDrivers.AsNoTracking().ProjectTo<AssignDriversDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.Id == request.Id);
            return assignDriver;
            // throw new NotImplementedException();
        }
    }
    #endregion

    #region Create_And_Update


    public class CreateUpdateAssignDriver : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public AssignDriversDto AssingDriver { get; set; }
    }

    public class CreateUpdateAssignDriverHandler : IRequestHandler<CreateUpdateAssignDriver, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateAssignDriverHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateUpdateAssignDriver request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info CreateUpdate Brand Master method start----");

                var obj = request.AssingDriver;


                TblAssignDrivers AssignDriver = new();
                if (obj.Id > 0)
                    AssignDriver = await _context.AssignDrivers.AsNoTracking().FirstOrDefaultAsync(e => e.Id == obj.Id);


                AssignDriver.Id = obj.Id;
                AssignDriver.VehicleNumber = obj.VehicleNumber;
                AssignDriver.DriverName = obj.DriverName;
                AssignDriver.AssignDate = obj.AssignDate;
                AssignDriver.NotesForDriver = obj.NotesForDriver;
                AssignDriver.Remarks = obj.Remarks;
                AssignDriver.IsActive = obj.IsActive;
                AssignDriver.CreatedOn = DateTime.Now;
                AssignDriver.CreatedBy = obj.CreatedBy;


                if (obj.Id > 0)
                {

                    _context.AssignDrivers.Update(AssignDriver);
                }
                else
                {

                    await _context.AssignDrivers.AddAsync(AssignDriver);
                }
                await _context.SaveChangesAsync();
                Log.Info("----Info Create Update Assign Drivers method Exit----");
                return AssignDriver.Id;
            }
            catch (Exception ex)
            {
                Log.Error("Error in Create Update Assign Driver Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }


    }



    #endregion

    #region Delete
    public class DeleteAssignDriver : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteAssignDriverHandler : IRequestHandler<DeleteAssignDriver, int>
    {

        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteAssignDriverHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteAssignDriver request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info Delete Assign Driver Start----");

                if (request.Id > 0)
                {
                    var assignDriver = await _context.AssignDrivers.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(assignDriver);

                    await _context.SaveChangesAsync();

                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {

                Log.Error("Error in Delete Assign Driver");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }

        }
    }

    #endregion
}
