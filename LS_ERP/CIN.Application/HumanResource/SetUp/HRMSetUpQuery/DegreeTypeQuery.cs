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

    public class GetDegreeTypeList : IRequest<PaginatedList<TblHRMSysDegreeTypeDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetDegreeTypeListHandler : IRequestHandler<GetDegreeTypeList, PaginatedList<TblHRMSysDegreeTypeDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetDegreeTypeListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<TblHRMSysDegreeTypeDto>> Handle(GetDegreeTypeList request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info GetDegreeTypeList method start----");
                var search = request.Input.Query;

                var list = await _context.DegreeTypes.AsNoTracking()
                    .ProjectTo<TblHRMSysDegreeTypeDto>(_mapper.ConfigurationProvider)
                    .Where(e => (e.DegreeTypeCode.Contains(search) || e.DegreeTypeNameEn.Contains(search)))
                    .OrderByDescending(x => x.Id)
                    .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

                Log.Info("----Info GetDegreeTypeList method end----");
                return list;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetDegreeTypeList Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region GetDegreeTypeById

    public class GetDegreeTypeById : IRequest<TblHRMSysDegreeTypeDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetDegreeTypeByIdHandler : IRequestHandler<GetDegreeTypeById, TblHRMSysDegreeTypeDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetDegreeTypeByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TblHRMSysDegreeTypeDto> Handle(GetDegreeTypeById request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetDegreeTypeById method start----");
            try
            {
                var degreeType = await _context.DegreeTypes.AsNoTracking()
                    .ProjectTo<TblHRMSysDegreeTypeDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(e => e.Id == request.Id);
                Log.Info("----Info GetDegreeTypeById method end----");

                if (degreeType is not null)
                    return degreeType;
                else
                    return null;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetDegreeTypeById Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region CreateUpdateDegreeType

    public class CreateUpdateDegreeType : UserIdentityDto, IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblHRMSysDegreeTypeDto Input { get; set; }
    }
    public class CreateUpdateDegreeTypeHandler : IRequestHandler<CreateUpdateDegreeType, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateDegreeTypeHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<AppCtrollerDto> Handle(CreateUpdateDegreeType request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateUpdateDegreeType method start----");
                    var obj = request.Input;
                    TblHRMSysDegreeType degreeType = new();

                    degreeType = await _context.DegreeTypes.FirstOrDefaultAsync(e => e.DegreeTypeCode == request.Input.DegreeTypeCode);

                    if (degreeType is not null)
                    {
                        degreeType.DegreeTypeNameEn = obj.DegreeTypeNameEn;
                        degreeType.DegreeTypeNameAr = obj.DegreeTypeNameAr;
                        degreeType.Id = obj.Id;
                        degreeType.IsActive = obj.IsActive;
                        degreeType.ModifiedBy = request.User.UserId;
                        degreeType.Modified = DateTime.Now;

                        _context.DegreeTypes.Update(degreeType);
                    }
                    else
                    {
                        degreeType = new()
                        {
                            DegreeTypeCode = obj.DegreeTypeCode,
                            DegreeTypeNameEn = obj.DegreeTypeNameEn,
                            DegreeTypeNameAr = obj.DegreeTypeNameAr,
                            IsActive = obj.IsActive,
                            CreatedBy = request.User.UserId,
                            Created = DateTime.Now,
                        };
                        await _context.DegreeTypes.AddAsync(degreeType);
                    }
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    Log.Info("----Info CreateUpdateDegreeType method Exit----");
                    return ApiMessageInfo.Status(1, degreeType.Id);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateUpdateDegreeType Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return ApiMessageInfo.Status(0);
                }
            }
        }
    }

    #endregion

    #region DeleteDegreeType
    public class DeleteDegreeType : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteDegreeTypeHandler : IRequestHandler<DeleteDegreeType, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteDegreeTypeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteDegreeType request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info DeleteDegreeType method start----");
                if (request.Id > 0)
                {
                    var city = await _context.DegreeTypes.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(city);
                    await _context.SaveChangesAsync();
                    Log.Info("----Info DeleteDegreeType method end----");
                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {
                Log.Error("Error in DeleteDegreeType Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }
    }

    #endregion

    #region GetDegreeTypeSelectListItem

    public class GetDegreeTypeSelectListItem : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetDegreeTypeSelectListItemHandler : IRequestHandler<GetDegreeTypeSelectListItem, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetDegreeTypeSelectListItemHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetDegreeTypeSelectListItem request, CancellationToken cancellationToken)
        {
            bool isArab = request.User.Culture.IsArab();
            var list = await _context.DegreeTypes.AsNoTracking().OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = isArab ? e.DegreeTypeNameAr : e.DegreeTypeNameEn, Value = e.DegreeTypeCode })
                  .ToListAsync(cancellationToken);

            return list;
        }
    }

    #endregion   
}
