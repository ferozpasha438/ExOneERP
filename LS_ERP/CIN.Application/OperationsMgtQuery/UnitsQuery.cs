using AutoMapper;
using CIN.Application.Common;
using CIN.Application.OperationsMgtDtos;
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
using CIN.Domain.OpeartionsMgt;


namespace CIN.Application.OperationsMgtQuery
{

    #region GetCustomerUnitsPagedList

    public class GetUnitsPagedList : IRequest<PaginatedList<TblSndDefUnitMasterDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetUnitsPagedListHandler : IRequestHandler<GetUnitsPagedList, PaginatedList<TblSndDefUnitMasterDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetUnitsPagedListHandler(CINDBOneContext context, AutoMapper.IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<TblSndDefUnitMasterDto>> Handle(GetUnitsPagedList request, CancellationToken cancellationToken)
        {
            var search = request.Input.Query;
            var list = await _context.OprUnits.AsNoTracking()
              .Where(e => //e.CompanyId == request.CompanyId &&
                            (e.UnitCode.Contains(search) ||
                            e.UnitNameEng.Contains(search) ||
                            e.UnitNameArb.Contains(search)||
                            search == "" || search == null
                             ))
               .OrderBy(request.Input.OrderBy)
              .ProjectTo<TblSndDefUnitMasterDto>(_mapper.ConfigurationProvider)
                 .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return list;
        }
    }
    #endregion

    #region CreateUpdateUnit
    public class CreateUnit : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public TblSndDefUnitMasterDto UnitDto { get; set; }
    }

    public class CreateUnitHandler : IRequestHandler<CreateUnit, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUnitHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateUnit request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info CreateUpdateUnit method start----");



                var obj = request.UnitDto;


                TblSndDefUnitMaster Unit = new();
                if (obj.Id > 0)
                    Unit = await _context.OprUnits.FirstOrDefaultAsync(e => e.Id == obj.Id);
                else
                {
                    if (_context.OprUnits.Any(x => x.UnitCode == obj.UnitCode))
                    {
                        return -1;
                    }
                    Unit.UnitCode = obj.UnitCode.ToUpper();
                }
                Unit.Id = obj.Id;
              
                Unit.IsActive = obj.IsActive;
                
                Unit.UnitNameEng = obj.UnitNameEng;
                Unit.UnitNameArb = obj.UnitNameArb;
                    
                if (obj.Id > 0)
                {
                    Unit.ModifiedOn = DateTime.Now;
                    Unit.UnitCode = obj.UnitCode;
                    _context.OprUnits.Update(Unit);
                }

                else
                {

                    Unit.CreatedOn = DateTime.Now;
                    await _context.OprUnits.AddAsync(Unit);
                }
                await _context.SaveChangesAsync();
                Log.Info("----Info CreateUpdateUnit method Exit----");
                return Unit.Id;
            }
            catch (Exception ex)
            {
                Log.Error("Error in CreateUpdateUnit Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }
    }

    #endregion

    #region GetUnitByUnitCode
    public class GetUnitByUnitCode : IRequest<TblSndDefUnitMasterDto>
    {
        public UserIdentityDto User { get; set; }
        public string UnitCode { get; set; }
    }

    public class GetUnitByUnitCodeHandler : IRequestHandler<GetUnitByUnitCode, TblSndDefUnitMasterDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetUnitByUnitCodeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblSndDefUnitMasterDto> Handle(GetUnitByUnitCode request, CancellationToken cancellationToken)
        {
            TblSndDefUnitMasterDto obj = new();
            var Unit = await _context.OprUnits.AsNoTracking().FirstOrDefaultAsync(e => e.UnitCode == request.UnitCode);
            if (Unit is not null)
            {
                obj.Id = Unit.Id;
                obj.UnitCode = Unit.UnitCode;
                obj.UnitCode = Unit.UnitCode;
                obj.UnitNameEng = Unit.UnitNameEng;
                obj.UnitNameArb = Unit.UnitNameArb;
                obj.ModifiedOn = Unit.ModifiedOn;
                obj.CreatedOn = Unit.CreatedOn;
                obj.IsActive = Unit.IsActive;
                return obj;
            }
            else return null;
        }
    }

    #endregion

    #region GetUnitById
    public class GetUnitById : IRequest<TblSndDefUnitMasterDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetUnitByIdHandler : IRequestHandler<GetUnitById, TblSndDefUnitMasterDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetUnitByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblSndDefUnitMasterDto> Handle(GetUnitById request, CancellationToken cancellationToken)
        {
            TblSndDefUnitMasterDto obj = new();
            var Unit = await _context.OprUnits.AsNoTracking().FirstOrDefaultAsync(e => e.Id == request.Id);
            if (Unit is not null)
            {
                obj.Id = Unit.Id;
                obj.UnitCode = Unit.UnitCode;
                obj.UnitCode = Unit.UnitCode;
                obj.UnitNameEng = Unit.UnitNameEng;
                obj.UnitNameArb = Unit.UnitNameArb;
                obj.ModifiedOn = Unit.ModifiedOn;
                obj.CreatedOn = Unit.CreatedOn;
                obj.IsActive = Unit.IsActive;
                return obj;
            }
            else return null;
        }
    }

    #endregion

    #region GetSelectUnitList

    public class GetSelectUnitList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetSelectUnitListHandler : IRequestHandler<GetSelectUnitList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSelectUnitListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetSelectUnitList request, CancellationToken cancellationToken)
        {
            var list = await _context.OprUnits.AsNoTracking()
              .Select(e => new CustomSelectListItem { Text = e.UnitNameEng, Value = e.UnitCode ,TextTwo=e.UnitNameArb})
                 .ToListAsync(cancellationToken);
            return list;
        }
    }

    #endregion
    #region GetSelectUnitListByServiceCode

    public class GetSelectUnitListByServiceCode : IRequest<List<CustomSelectListItem>>
    {
        public string ServiceCode { get; set; }
        public UserIdentityDto User { get; set; }
    }

    public class GetSelectUnitListByServiceCodeHandler : IRequestHandler<GetSelectUnitListByServiceCode, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSelectUnitListByServiceCodeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetSelectUnitListByServiceCode request, CancellationToken cancellationToken)
        {
            var list = await _context.OprServiceUnitMaps.AsNoTracking().Where(e=>e.ServiceCode== request.ServiceCode)
              .Select(e => new CustomSelectListItem { Text = e.UnitCode, Value = e.UnitCode })
                 .ToListAsync(cancellationToken);
            return list;
        }
    }

    #endregion
    #region GetAutoFillUnitList

    public class GetAutoFillUnitList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetAutoFillUnitListHandler : IRequestHandler<GetAutoFillUnitList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetAutoFillUnitListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetAutoFillUnitList request, CancellationToken cancellationToken)
        {
            var search = request.Input;
            var list = await _context.OprUnits.AsNoTracking()
                .Where(e => e.UnitNameEng.Contains(search) || e.UnitCode.Contains(search)||search==null)
              .Select(e => new CustomSelectListItem { Text = e.UnitNameEng, Value = e.UnitCode })
                 .ToListAsync(cancellationToken);
            return list;
        }
    }

    #endregion







    #region DeleteUnit
    public class DeleteUnit : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteUnitQueryHandler : IRequestHandler<DeleteUnit, int>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteUnitQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteUnit request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info DeleteUnit start----");

                if (request.Id > 0)
                {
                    var unit = await _context.OprUnits.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(unit);

                    await _context.SaveChangesAsync();

                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {

                Log.Error("Error in DeleteUnit");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }

        }
    }

    #endregion

}
