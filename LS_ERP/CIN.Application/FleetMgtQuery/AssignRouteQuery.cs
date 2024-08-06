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
    public class GetAssignRouteList : IRequest<PaginatedList<AssignRoutesDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }

    }

    public class GetAssignRouteListHandler : IRequestHandler<GetAssignRouteList, PaginatedList<AssignRoutesDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetAssignRouteListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<AssignRoutesDto>> Handle(GetAssignRouteList request, CancellationToken cancellationToken)
        {


            var list = await _context.AssignRoutes.AsNoTracking().ProjectTo<AssignRoutesDto>(_mapper.ConfigurationProvider)
                                    .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return list;
        }


    }


    #endregion

    #region GetById

    public class GetAssignRoutesId : IRequest<AssignRoutesDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetAssignRoutesIdHandler : IRequestHandler<GetAssignRoutesId, AssignRoutesDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetAssignRoutesIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AssignRoutesDto> Handle(GetAssignRoutesId request, CancellationToken cancellationToken)
        {

            AssignDriversDto obj = new();
            var assignRoutes = await _context.AssignRoutes.AsNoTracking().ProjectTo<AssignRoutesDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.Id == request.Id);
            return assignRoutes;
            // throw new NotImplementedException();
        }
    }
    #endregion

    #region Create_And_Update


    public class CreateUpdateAssignRoutes : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public AssignRoutesDto AssignRoutes { get; set; }
    }

    public class CreateUpdateAssignRoutesHandler : IRequestHandler<CreateUpdateAssignRoutes, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateAssignRoutesHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateUpdateAssignRoutes request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info CreateUpdate Assign Routes method start----");

                var obj = request.AssignRoutes;


                TblAssignRoutes AssignRoutes = new();
                if (obj.Id > 0)
                    AssignRoutes = await _context.AssignRoutes.AsNoTracking().FirstOrDefaultAsync(e => e.Id == obj.Id);


                AssignRoutes.Id = obj.Id;
                AssignRoutes.VehicleNumber = obj.VehicleNumber;
                AssignRoutes.RoutePlan = obj.RoutePlan;
                AssignRoutes.Remarks = obj.Remarks;
                AssignRoutes.IsActive = obj.IsActive;
                AssignRoutes.CreatedOn = DateTime.Now;
                AssignRoutes.CreatedBy = obj.CreatedBy;


                if (obj.Id > 0)
                {

                    _context.AssignRoutes.Update(AssignRoutes);
                }
                else
                {

                    await _context.AssignRoutes.AddAsync(AssignRoutes);
                }
                await _context.SaveChangesAsync();
                Log.Info("----Info Create Update Assign Routes method Exit----");
                return AssignRoutes.Id;
            }
            catch (Exception ex)
            {
                Log.Error("Error in Create Update Assign Routes Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }


    }



    #endregion

    #region Delete
    public class DeleteAssignRoute : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteAssignRouteHandler : IRequestHandler<DeleteAssignRoute, int>
    {

        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteAssignRouteHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteAssignRoute request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info Delete Assign Driver Start----");

                if (request.Id > 0)
                {
                    var assignRoute = await _context.AssignRoutes.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(assignRoute);

                    await _context.SaveChangesAsync();

                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {

                Log.Error("Error in Delete Assign Route");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }

        }
    }

    #endregion
}
