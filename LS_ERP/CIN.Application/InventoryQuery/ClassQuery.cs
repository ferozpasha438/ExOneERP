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

    public class GetClassList : IRequest<PaginatedList<TblInvDefClassDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetClassListHandler : IRequestHandler<GetClassList, PaginatedList<TblInvDefClassDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetClassListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<TblInvDefClassDto>> Handle(GetClassList request, CancellationToken cancellationToken)
        {
            var search = request.Input.Query;
            var list = await _context.InvClasses.AsNoTracking()
              .Where(e => //e.CompanyId == request.CompanyId &&
                            (e.ItemClassCode.Contains(search) || e.ItemClassName.Contains(search) ||
                                e.ItemClassDesce.Contains(search) 
                             ))
               .OrderBy(request.Input.OrderBy)
              .ProjectTo<TblInvDefClassDto>(_mapper.ConfigurationProvider)
                 .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return list;
        }
    }

    #endregion

    #region SingleItem

    public class GetClass : IRequest<TblInvDefClassDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetClassHandler : IRequestHandler<GetClass, TblInvDefClassDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetClassHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblInvDefClassDto> Handle(GetClass request, CancellationToken cancellationToken)
        {
            var item = await _context.InvClasses.AsNoTracking()
                    .Where(e => e.Id == request.Id)
               .ProjectTo<TblInvDefClassDto>(_mapper.ConfigurationProvider)
                  .FirstOrDefaultAsync(cancellationToken);
            //item.ItemCode = (await _context.InvCategories.FirstOrDefaultAsync(e => e.ItemCatCode == item.ItemCode))?.ItemCatCode ?? string.Empty;
            return item;
        }
    }

    #endregion


    #region CreateUpdate

    public class Createclass : IRequest<(string Message, int ClassId)>
    {
        public UserIdentityDto User { get; set; }
        public TblInvDefClassDto Input { get; set; }
    }

    public class CreateClassQueryHandler : IRequestHandler<Createclass, (string msg, int classId)>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateClassQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<(string msg, int classId)> Handle(Createclass request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info SaveUpdateClass method start----");

                var obj = request.Input;
                TblInvDefClass cobj = new();
                if (obj.Id > 0)
                    cobj = await _context.InvClasses.FirstOrDefaultAsync(e => e.Id == obj.Id);
                //if (string.IsNullOrWhiteSpace(obj.ItemCatCode))
                //    return (ApiMessageInfo.Failed, 0);

                //var Itemcode = await _context.InvCategories.FirstOrDefaultAsync(e => e.ItemCatCode == obj.ItemCatCode);

                cobj.ItemClassCode = obj.ItemClassCode;
                cobj.ItemClassName = obj.ItemClassName;
                cobj.ItemClassDesce = obj.ItemClassDesce;
                cobj.IsActive = obj.IsActive;

                if (obj.Id > 0)
                {
                    _context.InvClasses.Update(cobj);
                }
                else
                {
                    cobj.ItemClassCode = obj.ItemClassCode.ToUpper();
                    if (await _context.InvClasses.AnyAsync(e => e.ItemClassCode == obj.ItemClassCode))
                        return (ApiMessageInfo.Duplicate(nameof(obj.ItemClassCode)), 0);

                    await _context.InvClasses.AddAsync(cobj);
                }
                await _context.SaveChangesAsync();
                Log.Info("----Info SaveUpdateClass method Exit----");
                return (string.Empty, cobj.Id);
            }
            catch (Exception ex)
            {
                Log.Error("Error in SaveUpdateClass Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return (ApiMessageInfo.Failed, 0);
            }
        }
    }
    #endregion

    #region Delete
    public class DeleteClass : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteClassQueryHandler : IRequestHandler<DeleteClass, int>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteClassQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteClass request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info delte method start----");
                Log.Info("----Info delete method end----");

                if (request.Id > 0)
                {
                    var Class = await _context.InvClasses.FirstOrDefaultAsync(e => e.Id == request.Id);
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
    #region GetClassItem

    public class GetClassItem : IRequest<TblInvDefClassDto>
    {
        public UserIdentityDto User { get; set; }
        public string ClassCode { get; set; }
    }

    public class GetClassItemHandler : IRequestHandler<GetClassItem, TblInvDefClassDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetClassItemHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblInvDefClassDto> Handle(GetClassItem request, CancellationToken cancellationToken)
        {
            var item = await _context.InvClasses.AsNoTracking()
                    .Where(e => e.ItemClassCode == request.ClassCode)
               .ProjectTo<TblInvDefClassDto>(_mapper.ConfigurationProvider)
                  .FirstOrDefaultAsync(cancellationToken);
            return item;
        }
    }

    #endregion
}
