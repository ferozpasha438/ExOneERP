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
    public class GetServiceCodeList : IRequest<PaginatedList<ServiceCodeDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }

    }

    public class GetServiceCodeListHandler : IRequestHandler<GetServiceCodeList, PaginatedList<ServiceCodeDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetServiceCodeListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<ServiceCodeDto>> Handle(GetServiceCodeList request, CancellationToken cancellationToken)
        {


            var list = await _context.ServiceCode.AsNoTracking().ProjectTo<ServiceCodeDto>(_mapper.ConfigurationProvider)
                                    .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return list;
        }


    }


    #endregion

    #region GetById

    public class GetServiceCodeById : IRequest<ServiceCodeDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetServiceCodeByIdHandler : IRequestHandler<GetServiceCodeById, ServiceCodeDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetServiceCodeByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceCodeDto> Handle(GetServiceCodeById request, CancellationToken cancellationToken)
        {

            ServiceCodeDto obj = new();
            var serviceCode = await _context.ServiceCode.AsNoTracking().ProjectTo<ServiceCodeDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.Id == request.Id);
            return serviceCode;
            // throw new NotImplementedException();
        }
    }
    #endregion

    #region Create_And_Update


    public class CreateUpdateServiceCode : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public ServiceCodeDto ServiceCode { get; set; }
    }

    public class CreateUpdateServiceCodeHandler : IRequestHandler<CreateUpdateServiceCode, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateServiceCodeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateUpdateServiceCode request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info Create Update Service Code method start----");

                var obj = request.ServiceCode;


                TblServiceCode ServiceCode = new();
                if (obj.Id > 0)
                    ServiceCode = await _context.ServiceCode.AsNoTracking().FirstOrDefaultAsync(e => e.Id == obj.Id);


                ServiceCode.Id = obj.Id;
                ServiceCode.ServiceCode = obj.ServiceCode;
                ServiceCode.ServiceType = obj.ServiceType;
                ServiceCode.ServiceName_En = obj.ServiceName_En;
                ServiceCode.ServiceName_Ar = obj.ServiceName_Ar;
                ServiceCode.Remarks = obj.Remarks;
                ServiceCode.IsActive = obj.IsActive;
                ServiceCode.CreatedOn = DateTime.Now;
                ServiceCode.CreatedBy = obj.CreatedBy;


                if (obj.Id > 0)
                {

                    _context.ServiceCode.Update(ServiceCode);
                }
                else
                {

                    await _context.ServiceCode.AddAsync(ServiceCode);
                }
                await _context.SaveChangesAsync();
                Log.Info("----Info Create Update Service Code method Exit----");
                return ServiceCode.Id;
            }
            catch (Exception ex)
            {
                Log.Error("Error in Create Update Service Code Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }


    }



    #endregion

    #region Delete
    public class DeleteServiceCode : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteServiceCodeHandler : IRequestHandler<DeleteServiceCode, int>
    {

        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteServiceCodeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteServiceCode request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info Delete Service Code Start----");

                if (request.Id > 0)
                {
                    var serviceCode = await _context.ServiceCode.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(serviceCode);

                    await _context.SaveChangesAsync();

                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {

                Log.Error("Error in Delete Service Code");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }

        }
    }

    #endregion
}
