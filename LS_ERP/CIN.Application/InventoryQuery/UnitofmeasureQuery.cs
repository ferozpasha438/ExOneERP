using AutoMapper;
using AutoMapper.QueryableExtensions;
using CIN.Application.Common;
using CIN.Application.InventoryDtos;
using CIN.Application.InventoryQuery;
using CIN.DB;
using CIN.Domain.InventorySetup;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;

namespace CIN.Application.InventoryQuery
{
    #region GetPagedList

    public class GetUnitList : IRequest<PaginatedList<TblInvDefUOMDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetUnitListHandler : IRequestHandler<GetUnitList, PaginatedList<TblInvDefUOMDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetUnitListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<TblInvDefUOMDto>> Handle(GetUnitList request, CancellationToken cancellationToken)
        {
            var search = request.Input.Query;
            var list = await _context.InvUoms.AsNoTracking()
              .Where(e => //e.CompanyId == request.CompanyId &&
                            (e.UOMCode.Contains(search) || e.UOMName.Contains(search) ||
                                e.UOMDesc.Contains(search)
                             ))
               .OrderBy(request.Input.OrderBy)
              .ProjectTo<TblInvDefUOMDto>(_mapper.ConfigurationProvider)
                 .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return list;
        }
    }

    #endregion

    #region SingleItem

    public class GetUnit : IRequest<TblInvDefUOMDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetUnitHandler : IRequestHandler<GetUnit, TblInvDefUOMDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetUnitHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblInvDefUOMDto> Handle(GetUnit request, CancellationToken cancellationToken)
        {
            var item = await _context.InvUoms.AsNoTracking()
                    .Where(e => e.Id == request.Id)
               .ProjectTo<TblInvDefUOMDto>(_mapper.ConfigurationProvider)
                  .FirstOrDefaultAsync(cancellationToken);
            //item.ItemCode = (await _context.InvCategories.FirstOrDefaultAsync(e => e.ItemCatCode == item.ItemCode))?.ItemCatCode ?? string.Empty;
            return item;
        }
    }

    #endregion


    #region CreateUpdate

    public class CreateUnit : IRequest<(string Message, int UnitId)>
    {
        public UserIdentityDto User { get; set; }
        public TblInvDefUOMDto Input { get; set; }
    }

    public class CreateUnitQueryHandler : IRequestHandler<CreateUnit, (string msg, int UnitId)>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUnitQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<(string msg, int UnitId)> Handle(CreateUnit request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info SaveUpdateUnit method start----");

                var obj = request.Input;
                TblInvDefUOM cobj = new();
                if (obj.Id > 0)
                    cobj = await _context.InvUoms.FirstOrDefaultAsync(e => e.Id == obj.Id);
                //if (string.IsNullOrWhiteSpace(obj.ItemCatCode))
                //    return (ApiMessageInfo.Failed, 0);

                //var Itemcode = await _context.InvCategories.FirstOrDefaultAsync(e => e.ItemCatCode == obj.ItemCatCode);

                cobj.UOMCode = obj.UOMCode;
                cobj.UOMName = obj.UOMName;
                cobj.UOMDesc = obj.UOMDesc;
                cobj.IsActive = obj.IsActive;

                if (obj.Id > 0)
                {
                    _context.InvUoms.Update(cobj);
                }
                else
                {
                    cobj.UOMCode = obj.UOMCode.ToUpper();
                    if (await _context.InvUoms.AnyAsync(e => e.UOMCode == obj.UOMCode))
                        return (ApiMessageInfo.Duplicate(nameof(obj.UOMCode)), 0);

                    await _context.InvUoms.AddAsync(cobj);
                }
                await _context.SaveChangesAsync();
                Log.Info("----Info SaveUpdateUnit method Exit----");
                return (string.Empty, cobj.Id);
            }
            catch (Exception ex)
            {
                Log.Error("Error in SaveUpdateUnit Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return (ApiMessageInfo.Failed, 0);
            }
        }
    }
    #endregion

    #region Delete
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
                Log.Info("----Info delte method start----");
                Log.Info("----Info delete method end----");

                if (request.Id > 0)
                {
                    var Class = await _context.InvUoms.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(Class);
                    await _context.SaveChangesAsync();
                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {
                Log.Error("Error in delete Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }

        }
    }

    #endregion
    #region GetUomItem

    public class GetUomItems : IRequest<TblInvDefUOMDto>
    {
        public UserIdentityDto User { get; set; }
        public string UomCode { get; set; }
    }

    public class GetUomHandler : IRequestHandler<GetUomItems, TblInvDefUOMDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetUomHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblInvDefUOMDto> Handle(GetUomItems request, CancellationToken cancellationToken)
        {
            var item = await _context.InvUoms.AsNoTracking()
                    .Where(e => e.UOMCode == request.UomCode)
               .ProjectTo<TblInvDefUOMDto>(_mapper.ConfigurationProvider)
                  .FirstOrDefaultAsync(cancellationToken);
            return item;
        }
    }

    #endregion
}
