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

    public class GetReligionList : IRequest<PaginatedList<TblHRMSysReligionDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetReligionHandler : IRequestHandler<GetReligionList, PaginatedList<TblHRMSysReligionDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetReligionHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<TblHRMSysReligionDto>> Handle(GetReligionList request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info GetReligionList method start----");
                var search = request.Input.Query;
                var list = await _context.Religions.AsNoTracking().ProjectTo<TblHRMSysReligionDto>(_mapper.ConfigurationProvider)
                  .Where(e => (e.ReligionCode.Contains(search) || e.ReligionNameEn.Contains(search)))
                   .OrderByDescending(x => x.Id)
                     .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);
                Log.Info("----Info GetReligionList method end----");
                return list;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetReligionList Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region GetReligionById

    public class GetReligionById : IRequest<TblHRMSysReligionDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetReligionByIdHandler : IRequestHandler<GetReligionById, TblHRMSysReligionDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetReligionByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TblHRMSysReligionDto> Handle(GetReligionById request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetReligionById method start----");
            try
            {
                var Religion = await _context.Religions.AsNoTracking()
                    .ProjectTo<TblHRMSysReligionDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(e => e.Id == request.Id);
                Log.Info("----Info GetReligionById method end----");

                if (Religion is not null)
                    return Religion;
                else
                    return null;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetReligionById Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region CreateUpdateReligion

    public class CreateUpdateReligion : UserIdentityDto, IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblHRMSysReligionDto Input { get; set; }
    }
    public class CreateUpdateReligionHandler : IRequestHandler<CreateUpdateReligion, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateReligionHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<AppCtrollerDto> Handle(CreateUpdateReligion request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateUpdateReligion method start----");
                    var obj = request.Input;
                    TblHRMSysReligion Religion = new();

                    if (request.Input.Id > 0)
                    {
                        Religion = await _context.Religions.FirstOrDefaultAsync(e => e.ReligionCode == request.Input.ReligionCode);
                        Religion.ReligionNameEn = obj.ReligionNameEn;
                        Religion.ReligionNameAr = obj.ReligionNameAr;
                        Religion.Id = obj.Id;
                        Religion.IsActive = obj.IsActive;
                        Religion.ModifiedBy = request.User.UserId;
                        Religion.Modified = DateTime.Now;

                        _context.Religions.Update(Religion);
                    }
                    else
                    {
                        Religion = new()
                        {
                            ReligionNameEn = obj.ReligionNameEn,
                            ReligionNameAr = obj.ReligionNameAr,
                            ReligionCode = obj.ReligionCode,
                            IsActive = obj.IsActive,
                            CreatedBy = request.User.UserId,
                            Created = DateTime.Now,
                        };
                        await _context.Religions.AddAsync(Religion);
                    }
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    Log.Info("----Info CreateUpdateReligion method Exit----");
                    return ApiMessageInfo.Status(1, Religion.Id);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateUpdateReligion Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return ApiMessageInfo.Status(0);
                }
            }
        }
    }

    #endregion

    #region DeleteReligion
    public class DeleteReligion : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteReligionHandler : IRequestHandler<DeleteReligion, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteReligionHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteReligion request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info DeleteReligion method start----");
                if (request.Id > 0)
                {
                    var city = await _context.Religions.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(city);
                    await _context.SaveChangesAsync();
                    Log.Info("----Info DeleteReligion method end----");
                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {
                Log.Error("Error in DeleteReligion Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }
    }

    #endregion

    #region GetReligionSelectListItem
    public class GetReligionSelectListItem : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetReligionSelectListItemHandler : IRequestHandler<GetReligionSelectListItem, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetReligionSelectListItemHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetReligionSelectListItem request, CancellationToken cancellationToken)
        {
            bool isArab = request.User.Culture.IsArab();
            var list = await _context.Religions.AsNoTracking().OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = isArab ? e.ReligionNameAr : e.ReligionNameEn, Value = e.ReligionCode })
                  .ToListAsync(cancellationToken);

            return list;
        }
    }

    #endregion   
}
