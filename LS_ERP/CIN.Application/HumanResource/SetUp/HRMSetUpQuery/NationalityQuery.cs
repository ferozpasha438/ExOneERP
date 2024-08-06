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

    public class GetNationalityList : IRequest<PaginatedList<TblHRMSysNationalityDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetNationalityListHandler : IRequestHandler<GetNationalityList, PaginatedList<TblHRMSysNationalityDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetNationalityListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<TblHRMSysNationalityDto>> Handle(GetNationalityList request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info GetNationalityList method start----");
                var search = request.Input.Query;
                var list = await _context.Nationalities.AsNoTracking().ProjectTo<TblHRMSysNationalityDto>(_mapper.ConfigurationProvider)
                  .Where(e => (e.NationalityCode.Contains(search) || e.NationalityNameEn.Contains(search)))
                   .OrderByDescending(x => x.Id)
                     .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);
                Log.Info("----Info GetNationalityList method end----");
                return list;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetNationalityList Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region GetNationalityById

    public class GetNationalityById : IRequest<TblHRMSysNationalityDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetNationalityByIdHandler : IRequestHandler<GetNationalityById, TblHRMSysNationalityDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetNationalityByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TblHRMSysNationalityDto> Handle(GetNationalityById request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetNationalityById method start----");
            try
            {
                var Nationality = await _context.Nationalities.AsNoTracking()
                    .ProjectTo<TblHRMSysNationalityDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(e => e.Id == request.Id);
                Log.Info("----Info GetNationalityById method end----");

                if (Nationality is not null)
                    return Nationality;
                else
                    return null;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetNationalityById Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region CreateUpdateNationality

    public class CreateUpdateNationality : UserIdentityDto, IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblHRMSysNationalityDto Input { get; set; }
    }
    public class CreateUpdateNationalityHandler : IRequestHandler<CreateUpdateNationality, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateNationalityHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<AppCtrollerDto> Handle(CreateUpdateNationality request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateUpdateNationality method start----");
                    var obj = request.Input;
                    TblHRMSysNationality Nationality = new();

                    if (request.Input.Id > 0)
                    {
                        Nationality = await _context.Nationalities.FirstOrDefaultAsync(e => e.NationalityCode == request.Input.NationalityCode);
                        Nationality.NationalityNameEn = obj.NationalityNameEn;
                        Nationality.NationalityNameAr = obj.NationalityNameAr;
                        Nationality.Id = obj.Id;
                        Nationality.IsActive = obj.IsActive;
                        Nationality.ModifiedBy = request.User.UserId;
                        Nationality.Modified = DateTime.Now;

                        _context.Nationalities.Update(Nationality);
                    }
                    else
                    {
                        Nationality = new()
                        {
                            NationalityNameEn = obj.NationalityNameEn,
                            NationalityNameAr = obj.NationalityNameAr,
                            NationalityCode = obj.NationalityCode,
                            IsActive = obj.IsActive,
                            CreatedBy = request.User.UserId,
                            Created = DateTime.Now,
                        };
                        await _context.Nationalities.AddAsync(Nationality);
                    }
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    Log.Info("----Info CreateUpdateNationality method Exit----");
                    return ApiMessageInfo.Status(1, Nationality.Id);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateUpdateNationality Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return ApiMessageInfo.Status(0);
                }
            }
        }
    }

    #endregion

    #region DeleteNationality
    public class DeleteNationality : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteNationalityHandler : IRequestHandler<DeleteNationality, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteNationalityHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteNationality request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info DeleteNationality method start----");
                if (request.Id > 0)
                {
                    var city = await _context.Nationalities.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(city);
                    await _context.SaveChangesAsync();
                    Log.Info("----Info DeleteNationality method end----");
                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {
                Log.Error("Error in DeleteNationality Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }
    }

    #endregion

    #region GetNationalitySelectListItem

    public class GetNationalitySelectListItem : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetNationalitySelectListItemHandler : IRequestHandler<GetNationalitySelectListItem, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetNationalitySelectListItemHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetNationalitySelectListItem request, CancellationToken cancellationToken)
        {
            bool isArab = request.User.Culture.IsArab();
            var list = await _context.Nationalities.AsNoTracking().OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = isArab ? e.NationalityNameAr : e.NationalityNameEn, Value = e.NationalityCode })
                  .ToListAsync(cancellationToken);

            return list;
        }
    }

    #endregion

    #region GetCountrySelectListItem

    public class GetCountrySelectListItem : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetCountrySelectListItemHandler : IRequestHandler<GetCountrySelectListItem, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetCountrySelectListItemHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetCountrySelectListItem request, CancellationToken cancellationToken)
        {
            bool isArab = request.User.Culture.IsArab();
            var list = await _context.CountryCodes.AsNoTracking().OrderByDescending(e => e.Id)
                .Select(e => new CustomSelectListItem { Text = e.CountryName, Value = e.CountryCode })
                .ToListAsync(cancellationToken);

            return list;
        }
    }

    #endregion


}
