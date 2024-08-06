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

    public class GetSubClassList : IRequest<PaginatedList<TblInvDefSubClassDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetSubClassListHandler : IRequestHandler<GetSubClassList, PaginatedList<TblInvDefSubClassDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSubClassListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<TblInvDefSubClassDto>> Handle(GetSubClassList request, CancellationToken cancellationToken)
        {
            var search = request.Input.Query;
            var list = await _context.InvSubClasses.AsNoTracking()
              .Where(e => //e.CompanyId == request.CompanyId &&
                            (e.ItemSubClassCode.Contains(search) || e.ItemSubClassName.Contains(search) ||
                                e.ItemSubClassDesce.Contains(search)
                             ))
               .OrderBy(request.Input.OrderBy)
              .ProjectTo<TblInvDefSubClassDto>(_mapper.ConfigurationProvider)
                 .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            //var currentUserId = user.Claims.ToList().FirstOrDefault(x => x.Type == "id").Value;
            //var userProfile = (await _context.GetUserByIds.FromSqlInterpolated($"Exec sp_InvDefSubClass @userId = {currentUserId}").ToListAsync()).FirstOrDefault();
            //var userProfile = (await _context.InvSubClasses.FromSqlInterpolated($"Exec sp_InvDefSubClass").ToListAsync()).FirstOrDefault();
            return list;
        }
    }

    #endregion

    #region SingleItem

    public class GetSubClass : IRequest<TblInvDefSubClassDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetSubClassHandler : IRequestHandler<GetSubClass, TblInvDefSubClassDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSubClassHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblInvDefSubClassDto> Handle(GetSubClass request, CancellationToken cancellationToken)
        {
            var item = await _context.InvSubClasses.AsNoTracking()
                    .Where(e => e.Id == request.Id)
               .ProjectTo<TblInvDefSubClassDto>(_mapper.ConfigurationProvider)
                  .FirstOrDefaultAsync(cancellationToken);
            //item.ItemCode = (await _context.InvCategories.FirstOrDefaultAsync(e => e.ItemCatCode == item.ItemCode))?.ItemCatCode ?? string.Empty;
            return item;
        }
    }

    #endregion


    #region CreateUpdate

    public class CreateSubclass : IRequest<(string Message, int SubClassId)>
    {
        public UserIdentityDto User { get; set; }
        public TblInvDefSubClassDto Input { get; set; }
    }

    public class CreateSubClassQueryHandler : IRequestHandler<CreateSubclass, (string msg, int subclassId)>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateSubClassQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<(string msg, int subclassId)> Handle(CreateSubclass request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info SaveUpdateSubClass method start----");

                var obj = request.Input;
                TblInvDefSubClass cobj = new();
                if (obj.Id > 0)
                    cobj = await _context.InvSubClasses.FirstOrDefaultAsync(e => e.Id == obj.Id);
                //if (string.IsNullOrWhiteSpace(obj.ItemCatCode))
                //    return (ApiMessageInfo.Failed, 0);

                //var Itemcode = await _context.InvCategories.FirstOrDefaultAsync(e => e.ItemCatCode == obj.ItemCatCode);

                cobj.ItemSubClassCode = obj.ItemSubClassCode;
                cobj.ItemSubClassName = obj.ItemSubClassName;
                cobj.ItemSubClassDesce = obj.ItemSubClassDesce;
                cobj.IsActive = obj.IsActive;

                if (obj.Id > 0)
                {
                    _context.InvSubClasses.Update(cobj);
                }
                else
                {
                    cobj.ItemSubClassCode = obj.ItemSubClassCode.ToUpper();
                    if (await _context.InvSubClasses.AnyAsync(e => e.ItemSubClassCode == obj.ItemSubClassCode))
                        return (ApiMessageInfo.Duplicate(nameof(obj.ItemSubClassCode)), 0);

                    await _context.InvSubClasses.AddAsync(cobj);
                }
                await _context.SaveChangesAsync();
                Log.Info("----Info SaveUpdateSubClass method Exit----");
                return (string.Empty, cobj.Id);
            }
            catch (Exception ex)
            {
                Log.Error("Error in SaveUpdateSubClass Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return (ApiMessageInfo.Failed, 0);
            }
        }
    }
    #endregion

    #region Delete
    public class DeleteSubClass : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteSubClassQueryHandler : IRequestHandler<DeleteSubClass, int>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteSubClassQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteSubClass request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info delte method start----");
                Log.Info("----Info delete method end----");

                if (request.Id > 0)
                {
                    var Class = await _context.InvSubClasses.FirstOrDefaultAsync(e => e.Id == request.Id);
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
    #region GetSubClassItem

    public class GetSubClassItem : IRequest<TblInvDefSubClassDto>
    {
        public UserIdentityDto User { get; set; }
        public string SubClassCode { get; set; }
    }

    public class GetSubClasItemsHandler : IRequestHandler<GetSubClassItem, TblInvDefSubClassDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSubClasItemsHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblInvDefSubClassDto> Handle(GetSubClassItem request, CancellationToken cancellationToken)
        {
            var item = await _context.InvSubClasses.AsNoTracking()
                    .Where(e => e.ItemSubClassCode == request.SubClassCode)
               .ProjectTo<TblInvDefSubClassDto>(_mapper.ConfigurationProvider)
                  .FirstOrDefaultAsync(cancellationToken);
            return item;
        }
    }

    #endregion
}
