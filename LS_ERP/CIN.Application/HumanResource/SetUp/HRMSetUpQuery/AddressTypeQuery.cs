using AutoMapper;
using AutoMapper.QueryableExtensions;
using CIN.Application.Common;
using CIN.Application.HumanResource.SetUp.HRMSetUpDtos;
using CIN.DB;
using CIN.Domain.HumanResource.Setup;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CIN.Application.HumanResource.SetUp.HRMSetUpQuery
{
    #region GetPagedList

    public class GetAddressTypeList : IRequest<PaginatedList<TblHRMSysAddressTypeDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetAddressTypeListHandler : IRequestHandler<GetAddressTypeList, PaginatedList<TblHRMSysAddressTypeDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetAddressTypeListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<TblHRMSysAddressTypeDto>> Handle(GetAddressTypeList request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info GetAddressTypeList method start----");
                var search = request.Input.Query;
                var list = await _context.AddressTypes.AsNoTracking().ProjectTo<TblHRMSysAddressTypeDto>(_mapper.ConfigurationProvider)
                  .Where(e => (e.AddressTypeCode.Contains(search) || e.AddressTypeNameEn.Contains(search)))
                   .OrderByDescending(x => x.Id)
                     .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);
                Log.Info("----Info GetAddressTypeList method end----");
                return list;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetAddressTypeList Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region GetAddressTypeId

    public class GetAddressTypeById : IRequest<TblHRMSysAddressTypeDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetAddressTypeIdHandler : IRequestHandler<GetAddressTypeById, TblHRMSysAddressTypeDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetAddressTypeIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TblHRMSysAddressTypeDto> Handle(GetAddressTypeById request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetAddressTypeById method start----");
            try
            {
                var AddressType = await _context.AddressTypes.AsNoTracking()
                    .ProjectTo<TblHRMSysAddressTypeDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(e => e.Id == request.Id);
                Log.Info("----Info GetAddressTypeById method end----");

                if (AddressType is not null)
                    return AddressType;
                else
                    return null;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetAddressTypeById Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region CreateUpdateAddressType

    public class CreateUpdateAddressType : UserIdentityDto, IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblHRMSysAddressTypeDto Input { get; set; }
    }
    public class CreateUpdateAddressTypeHandler : IRequestHandler<CreateUpdateAddressType, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateAddressTypeHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<AppCtrollerDto> Handle(CreateUpdateAddressType request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateUpdateAddressType method start----");
                    var obj = request.Input;
                    TblHRMSysAddressType AddressType = new();

                    if (request.Input.Id > 0)
                    {
                        AddressType = await _context.AddressTypes.FirstOrDefaultAsync(e => e.AddressTypeCode == request.Input.AddressTypeCode);
                        AddressType.AddressTypeNameEn = obj.AddressTypeNameEn;
                        AddressType.AddressTypeNameAr = obj.AddressTypeNameAr;
                        AddressType.Id = obj.Id;
                        AddressType.IsActive = obj.IsActive;
                        AddressType.ModifiedBy = request.User.UserId;
                        AddressType.Modified = DateTime.Now;

                        _context.AddressTypes.Update(AddressType);
                    }
                    else
                    {
                        AddressType = new()
                        {
                            AddressTypeNameEn = obj.AddressTypeNameEn,
                            AddressTypeNameAr = obj.AddressTypeNameAr,
                            AddressTypeCode = obj.AddressTypeCode,
                            IsActive = obj.IsActive,
                            CreatedBy = request.User.UserId,
                            Created = DateTime.Now,
                        };
                        await _context.AddressTypes.AddAsync(AddressType);
                    }
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    Log.Info("----Info CreateUpdateAddressType method Exit----");
                    return ApiMessageInfo.Status(1, AddressType.Id);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateUpdateAddressType Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return ApiMessageInfo.Status(0);
                }
            }
        }
    }

    #endregion

    #region DeleteAddressType
    public class DeleteAddressType : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteAddressTypeHandler : IRequestHandler<DeleteAddressType, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteAddressTypeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteAddressType request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info DeleteAddressType method start----");
                if (request.Id > 0)
                {
                    var city = await _context.AddressTypes.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(city);
                    await _context.SaveChangesAsync();
                    Log.Info("----Info DeleteAddressType method end----");
                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {
                Log.Error("Error in DeleteAddressType Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }
    }

    #endregion

    #region GetAddressTypeSelectListItem

    public class GetAddressTypeSelectListItem : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetAddressTypeSelectListItemHandler : IRequestHandler<GetAddressTypeSelectListItem, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetAddressTypeSelectListItemHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetAddressTypeSelectListItem request, CancellationToken cancellationToken)
        {
            bool isArab = request.User.Culture.IsArab();
            var list = await _context.AddressTypes.AsNoTracking().OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = isArab ? e.AddressTypeNameAr : e.AddressTypeNameEn, Value = e.AddressTypeCode })
                  .ToListAsync(cancellationToken);

            return list;
        }
    }

    #endregion   
}
