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

    public class GetDependentTypeList : IRequest<PaginatedList<TblHRMSysDependentTypeDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetDependentTypeListHandler : IRequestHandler<GetDependentTypeList, PaginatedList<TblHRMSysDependentTypeDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetDependentTypeListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<TblHRMSysDependentTypeDto>> Handle(GetDependentTypeList request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info GetDependentTypeList method start----");
                var search = request.Input.Query;

                var list = await _context.DependentTypes.AsNoTracking().ProjectTo<TblHRMSysDependentTypeDto>(_mapper.ConfigurationProvider)
                  .Where(e => (e.DependentTypeCode.Contains(search) || e.DependentTypeNameEn.Contains(search)))
                   .OrderByDescending(x => x.Id)
                     .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

                Log.Info("----Info GetDependentTypeList method end----");
                return list;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetDependentTypeList Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region GetDependentTypeById

    public class GetDependentTypeById : IRequest<TblHRMSysDependentTypeDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetDependentTypeByIdHandler : IRequestHandler<GetDependentTypeById, TblHRMSysDependentTypeDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetDependentTypeByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TblHRMSysDependentTypeDto> Handle(GetDependentTypeById request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetDependentTypeById method start----");
            try
            {
                var visaType = await _context.DependentTypes.AsNoTracking()
                    .ProjectTo<TblHRMSysDependentTypeDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(e => e.Id == request.Id);
                Log.Info("----Info GetDependentTypeById method end----");

                if (visaType is not null)
                    return visaType;
                else
                    return null;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetDependentTypeById Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region CreateUpdateDependentType

    public class CreateUpdateDependentType : UserIdentityDto, IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblHRMSysDependentTypeDto Input { get; set; }
    }
    public class CreateUpdateDependentTypeHandler : IRequestHandler<CreateUpdateDependentType, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateDependentTypeHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<AppCtrollerDto> Handle(CreateUpdateDependentType request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateUpdateDependentType method start----");
                    var obj = request.Input;
                    TblHRMSysDependentType dependentType = new();

                    dependentType = await _context.DependentTypes.FirstOrDefaultAsync(e => e.DependentTypeCode == request.Input.DependentTypeCode);

                    if (dependentType is not null)
                    {
                        dependentType.DependentTypeNameEn = obj.DependentTypeNameEn;
                        dependentType.DependentTypeNameAr = obj.DependentTypeNameAr;
                        dependentType.Id = obj.Id;
                        dependentType.IsActive = obj.IsActive;
                        dependentType.ModifiedBy = request.User.UserId;
                        dependentType.Modified = DateTime.Now;

                        _context.DependentTypes.Update(dependentType);
                    }
                    else
                    {
                        dependentType = new()
                        {
                            DependentTypeCode = obj.DependentTypeCode,
                            DependentTypeNameEn = obj.DependentTypeNameEn,
                            DependentTypeNameAr = obj.DependentTypeNameAr,
                            IsActive = obj.IsActive,
                            CreatedBy = request.User.UserId,
                            Created = DateTime.Now,
                        };
                        await _context.DependentTypes.AddAsync(dependentType);
                    }
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    Log.Info("----Info CreateUpdateDependentType method Exit----");
                    return ApiMessageInfo.Status(1, dependentType.Id);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateUpdateDependentType Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return ApiMessageInfo.Status(0);
                }
            }
        }
    }

    #endregion

    #region DeleteDependentType
    public class DeleteDependentType : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteDependentTypeHandler : IRequestHandler<DeleteDependentType, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteDependentTypeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteDependentType request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info DeleteDependentType method start----");
                if (request.Id > 0)
                {
                    var city = await _context.DependentTypes.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(city);
                    await _context.SaveChangesAsync();
                    Log.Info("----Info DeleteDependentType method end----");
                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {
                Log.Error("Error in DeleteDependentType Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }
    }

    #endregion

    #region GetDependentTypeSelectListItem
    public class GetDependentTypeSelectListItem : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetDependentTypeSelectListItemHandler : IRequestHandler<GetDependentTypeSelectListItem, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetDependentTypeSelectListItemHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetDependentTypeSelectListItem request, CancellationToken cancellationToken)
        {
            bool isArab = request.User.Culture.IsArab();
            var list = await _context.DependentTypes
                .AsNoTracking()
                .OrderByDescending(e => e.Id)
                .Select(e => new CustomSelectListItem { Text = isArab ? e.DependentTypeNameAr : e.DependentTypeNameEn, Value = e.DependentTypeCode })
                .ToListAsync(cancellationToken);
            return list;
        }
    }

    #endregion   
}
