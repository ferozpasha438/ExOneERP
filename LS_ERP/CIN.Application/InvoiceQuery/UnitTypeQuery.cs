using AutoMapper;
using AutoMapper.QueryableExtensions;
using CIN.Application.Common;
using CIN.Application.InvoiceDtos;
using CIN.DB;
using CIN.Domain.InvoiceSetup;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;

namespace CIN.Application.InvoiceQuery
{

    #region ListOfData

    public class GetUnitTypeList : IRequest<PaginatedList<UnitTypeDTO>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetUnitTypeListQueryHandler : IRequestHandler<GetUnitTypeList, PaginatedList<UnitTypeDTO>>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public GetUnitTypeListQueryHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<PaginatedList<UnitTypeDTO>> Handle(GetUnitTypeList request, CancellationToken cancellationToken)
        {
            try
            {

                Log.Info("----Info GetUnitTypes method start----");
                Log.Info("Store Proc Name : USP_Invoice_GetCompanyUnitTypes");
                Log.Info("----Info GetUnitType method end----");
                var UnitTypes = await _context.TranUnitTypes.AsNoTracking()
                         .Where(e =>
                        //e.CompanyId == request.User.CompanyId &&
                        (e.NameEN.Contains(request.Input.Query) || e.NameAR.Contains(request.Input.Query)
                         ))
                         .OrderBy(request.Input.OrderBy)
                        .ProjectTo<UnitTypeDTO>(_mapper.ConfigurationProvider)
                        .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);
                Log.Info("----Info GetUnitTypes method Exit----");
                return UnitTypes;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetUnitTypes Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return null;
            }
        }
    }

    #endregion

    #region ListOfDataByCompnay

    public class GetUnitTypeCompnayList : IRequest<List<UnitTypeDTO>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetUnitTypeCompnayListQueryHandler : IRequestHandler<GetUnitTypeCompnayList, List<UnitTypeDTO>>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public GetUnitTypeCompnayListQueryHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<List<UnitTypeDTO>> Handle(GetUnitTypeCompnayList request, CancellationToken cancellationToken)
        {
            try
            {

                Log.Info("----Info GetUnitTypes method start----");
                Log.Info("Store Proc Name : USP_Invoice_GetCompanyUnitTypes");
                Log.Info("----Info GetUnitType method end----");
                var UnitTypes = await _context.TranUnitTypes.AsNoTracking()
                         //.Where(e => e.CompanyId == request.User.CompanyId)
                         .OrderByDescending(e => e.Id)
                        .ProjectTo<UnitTypeDTO>(_mapper.ConfigurationProvider)
                        .ToListAsync(cancellationToken);
                Log.Info("----Info GetUnitTypes method Exit----");
                return UnitTypes;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetUnitTypes Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return null;
            }
        }
    }

    #endregion

    #region Single Item

    public class GetUnitType :  IRequest<UnitTypeDTO>
    {
        public UserIdentityDto User { get; set; }
        public int UnitTypeId { get; set; }
    }
    public class GetUnitTypesQueryHandler : IRequestHandler<GetUnitType, UnitTypeDTO>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public GetUnitTypesQueryHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<UnitTypeDTO> Handle(GetUnitType request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info GetUnitType method start----");
                Log.Info("Store Proc Name : USP_Invoice_GetUnitType");
                Log.Info("----Info GetUnitType method end----");
                var UnitType = await _context.TranUnitTypes.AsNoTracking()
                   .Where(e => e.Id == request.UnitTypeId)
                   .ProjectTo<UnitTypeDTO>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(cancellationToken);
                Log.Info("----Info GetUnitType method Exit----");
                return UnitType;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetUnitType Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region CreateUpdate

    public class CreateUnitTypes :  IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public UnitTypeDTO UnitTypeDTO { get; set; }
    }
    public class CreateUnitTypesQueryHandler : IRequestHandler<CreateUnitTypes, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUnitTypesQueryHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<int> Handle(CreateUnitTypes request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info SaveUpdateUnitTypes method start----");
                Log.Info("Store Proc Name : USP_Invoice_SaveUpdateUnitTypes");
                Log.Info("----Info SaveUpdateUnitTypes method end----");

                var obj = request.UnitTypeDTO;
                TblTranDefUnitType UnitType = new();
                if (obj.Id > 0)
                    UnitType = await _context.TranUnitTypes.FirstOrDefaultAsync(e => e.Id == obj.Id);

                UnitType.CompanyId = request.User.CompanyId;
                UnitType.NameEN = obj.NameEN;
                UnitType.NameAR = obj.NameAR;


                if (obj.Id > 0)
                {
                    UnitType.UpdatedBy = (int)request.User.UserId;
                    UnitType.UpdatedOn = DateTime.Now;
                    _context.TranUnitTypes.Update(UnitType);

                }
                else
                {
                    bool isExists = await _context.TranUnitTypes
                        .Where(e => e.NameEN == obj.NameEN && e.NameAR == obj.NameAR && e.CompanyId == request.User.CompanyId)
                        .AnyAsync();
                    if (!isExists)
                    {
                        UnitType.IsDefaultConfig = true;
                        UnitType.CreatedBy = (int)request.User.UserId;
                        UnitType.CreatedOn = DateTime.Now;
                        await _context.TranUnitTypes.AddAsync(UnitType);
                    }
                    else
                        return -1;
                }

                await _context.SaveChangesAsync();
                Log.Info("----Info SaveUpdateUnitTypes method Exit----");
                return UnitType.Id;
            }
            catch (Exception ex)
            {
                Log.Error("Error in SaveUpdateUnitTypes Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }
    }

    #endregion
}
