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

    public class GetLanguageList : IRequest<PaginatedList<TblHRMSysLanguageDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetLanguageListHandler : IRequestHandler<GetLanguageList, PaginatedList<TblHRMSysLanguageDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetLanguageListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<TblHRMSysLanguageDto>> Handle(GetLanguageList request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info GetLanguageList method start----");
                var search = request.Input.Query;
                var list = await _context.Languages.AsNoTracking().ProjectTo<TblHRMSysLanguageDto>(_mapper.ConfigurationProvider)
                  .Where(e => (e.LanguageCode.Contains(search) || e.LanguageNameEn.Contains(search)))
                   .OrderByDescending(x => x.Id)
                     .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);
                Log.Info("----Info GetLanguageList method end----");
                return list;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetLanguageList Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region GetLanguageById

    public class GetLanguageById : IRequest<TblHRMSysLanguageDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetLanguageByIdHandler : IRequestHandler<GetLanguageById, TblHRMSysLanguageDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetLanguageByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TblHRMSysLanguageDto> Handle(GetLanguageById request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetLanguageById method start----");
            try
            {
                var language = await _context.Languages.AsNoTracking()
                    .ProjectTo<TblHRMSysLanguageDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(e => e.Id == request.Id);
                Log.Info("----Info GetLanguageById method end----");

                if (language is not null)
                    return language;
                else
                    return null;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetLanguageById Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region CreateUpdateLanguage

    public class CreateUpdateLanguage : UserIdentityDto, IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblHRMSysLanguageDto Input { get; set; }
    }
    public class CreateUpdateLanguageHandler : IRequestHandler<CreateUpdateLanguage, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateLanguageHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<AppCtrollerDto> Handle(CreateUpdateLanguage request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateUpdateLanguage method start----");
                    var obj = request.Input;
                    TblHRMSysLanguage language = new();

                    if (request.Input.Id > 0)
                    {
                        language = await _context.Languages.FirstOrDefaultAsync(e => e.LanguageCode == request.Input.LanguageCode);
                        language.LanguageNameEn = obj.LanguageNameEn;
                        language.LanguageNameAr = obj.LanguageNameAr;
                        language.Id = obj.Id;
                        language.IsActive = obj.IsActive;
                        language.ModifiedBy = request.User.UserId;
                        language.Modified = DateTime.Now;

                        _context.Languages.Update(language);
                    }
                    else
                    {
                        language = new()
                        {
                            LanguageNameEn = obj.LanguageNameEn,
                            LanguageNameAr = obj.LanguageNameAr,
                            LanguageCode = obj.LanguageCode,
                            IsActive = obj.IsActive,
                            CreatedBy = request.User.UserId,
                            Created = DateTime.Now,
                        };
                        await _context.Languages.AddAsync(language);
                    }
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    Log.Info("----Info CreateUpdateLanguage method Exit----");
                    return ApiMessageInfo.Status(1, language.Id);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateUpdateLanguage Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return ApiMessageInfo.Status(0);
                }
            }
        }
    }

    #endregion

    #region Delete Language
    public class DeleteLanguage : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteLanguageHandler : IRequestHandler<DeleteLanguage, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteLanguageHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteLanguage request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info DeleteLanguage method start----");
                if (request.Id > 0)
                {
                    var city = await _context.Languages.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(city);
                    await _context.SaveChangesAsync();
                    Log.Info("----Info DeleteLanguage method end----");
                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {
                Log.Error("Error in DeleteLanguage Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }
    }

    #endregion

    #region GetLanguageSelectListItem

    public class GetLanguageSelectListItem : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetLanguageSelectListItemHandler : IRequestHandler<GetLanguageSelectListItem, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetLanguageSelectListItemHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetLanguageSelectListItem request, CancellationToken cancellationToken)
        {
            bool isArab = request.User.Culture.IsArab();
            var list = await _context.Languages.AsNoTracking().OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = isArab ? e.LanguageNameAr : e.LanguageNameEn, Value = e.LanguageCode })
                  .ToListAsync(cancellationToken);

            return list;
        }
    }

    #endregion   
}
