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
    public class GetServiceProviderList : IRequest<PaginatedList<ServiceProviderDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }

    }

    public class GetServiceProviderListHandler : IRequestHandler<GetServiceProviderList, PaginatedList<ServiceProviderDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetServiceProviderListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<ServiceProviderDto>> Handle(GetServiceProviderList request, CancellationToken cancellationToken)
        {


            var list = await _context.ServiceProvider.AsNoTracking().ProjectTo<ServiceProviderDto>(_mapper.ConfigurationProvider)
                                    .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return list;
        }


    }


    #endregion

    #region GetById

    public class GetServiceProvideById : IRequest<ServiceProviderDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetServiceProvideByIdHandler : IRequestHandler<GetServiceProvideById, ServiceProviderDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetServiceProvideByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceProviderDto> Handle(GetServiceProvideById request, CancellationToken cancellationToken)
        {

            ServiceProviderDto obj = new();
            var serviceProvider = await _context.ServiceProvider.AsNoTracking().ProjectTo<ServiceProviderDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.Id == request.Id);
            return serviceProvider;
            // throw new NotImplementedException();
        }
    }
    #endregion

    #region Create_And_Update


    public class CreateUpdateServiceProvide : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public ServiceProviderDto ServiceProvide { get; set; }
    }

    public class CreateUpdateServiceProvideHandler : IRequestHandler<CreateUpdateServiceProvide, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateServiceProvideHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateUpdateServiceProvide request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info Create Update Service Provide method start----");

                var obj = request.ServiceProvide;


                TblServiceProvider ServiceProvide = new();
                if (obj.Id > 0)
                    ServiceProvide = await _context.ServiceProvider.AsNoTracking().FirstOrDefaultAsync(e => e.Id == obj.Id);


                ServiceProvide.Id = obj.Id;
                ServiceProvide.ServiceProviderCode = obj.ServiceProviderCode;
                ServiceProvide.LocationCity = obj.LocationCity;
                ServiceProvide.ServiceProviderName_En = obj.ServiceProviderName_En;
                ServiceProvide.ServiceProviderName_Ar = obj.ServiceProviderName_Ar;
                ServiceProvide.Remarks = obj.Remarks;
                ServiceProvide.IsActive = obj.IsActive;
                ServiceProvide.CreatedOn = DateTime.Now;
                ServiceProvide.CreatedBy = obj.CreatedBy;


                if (obj.Id > 0)
                {

                    _context.ServiceProvider.Update(ServiceProvide);
                }
                else
                {

                    await _context.ServiceProvider.AddAsync(ServiceProvide);
                }
                await _context.SaveChangesAsync();
                Log.Info("----Info Create Update Service Provide method Exit----");
                return ServiceProvide.Id;
            }
            catch (Exception ex)
            {
                Log.Error("Error in Create Update Service Provide Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }


    }



    #endregion

    #region Delete
    public class DeleteServiceProvide : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteServiceProvideHandler : IRequestHandler<DeleteServiceProvide, int>
    {

        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteServiceProvideHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteServiceProvide request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info Delete Service Provide Start----");

                if (request.Id > 0)
                {
                    var serviceCode = await _context.ServiceProvider.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(serviceCode);

                    await _context.SaveChangesAsync();

                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {

                Log.Error("Error in Delete Service Provide");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }

        }
    }

    #endregion
}
