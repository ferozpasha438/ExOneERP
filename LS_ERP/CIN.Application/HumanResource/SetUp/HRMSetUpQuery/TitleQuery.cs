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

    public class GetTitleList : IRequest<PaginatedList<TblHRMSysTitleDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetTitleListHandler : IRequestHandler<GetTitleList, PaginatedList<TblHRMSysTitleDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetTitleListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<TblHRMSysTitleDto>> Handle(GetTitleList request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info GetTitleList method start----");
                var search = request.Input.Query;
                var list = await _context.Titles.AsNoTracking().ProjectTo<TblHRMSysTitleDto>(_mapper.ConfigurationProvider)
                  .Where(e => (e.TitleCode.Contains(search) || e.TitleNameEn.Contains(search)))
                  .OrderByDescending(x => x.Id)
                  .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);
                Log.Info("----Info GetTitleList method end----");
                return list;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetTitleList Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region GetTitleById

    public class GetTitleById : IRequest<TblHRMSysTitleDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetTitleByIdHandler : IRequestHandler<GetTitleById, TblHRMSysTitleDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetTitleByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TblHRMSysTitleDto> Handle(GetTitleById request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetTitleById method start----");
            try
            {
                var title = await _context.Titles.AsNoTracking()
                    .ProjectTo<TblHRMSysTitleDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(e => e.Id == request.Id);
                Log.Info("----Info GetTitleById method end----");

                if (title is not null)
                    return title;
                else
                    return null;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetTitleById Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region CreateUpdateTitle

    public class CreateUpdateTitle : UserIdentityDto, IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblHRMSysTitleDto Input { get; set; }
    }
    public class CreateUpdateTitleHandler : IRequestHandler<CreateUpdateTitle, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateTitleHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<AppCtrollerDto> Handle(CreateUpdateTitle request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateUpdateTitle method start----");
                    var obj = request.Input;
                    TblHRMSysTitle title = new();

                    title = await _context.Titles.FirstOrDefaultAsync(e => e.TitleCode == request.Input.TitleCode);

                    if (title is not null)
                    {
                        title.TitleNameEn = obj.TitleNameEn;
                        title.TitleNameAr = obj.TitleNameAr;
                        title.Id = obj.Id;
                        title.IsActive = obj.IsActive;
                        title.ModifiedBy = request.User.UserId;
                        title.Modified = DateTime.Now;

                        _context.Titles.Update(title);
                    }
                    else
                    {
                        title = new()
                        {
                            TitleNameEn = obj.TitleNameEn,
                            TitleNameAr = obj.TitleNameAr,
                            TitleCode = obj.TitleCode,
                            IsActive = obj.IsActive,
                            CreatedBy = request.User.UserId,
                            Created = DateTime.Now,
                        };
                        await _context.Titles.AddAsync(title);
                    }
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    Log.Info("----Info CreateUpdateTitle method Exit----");
                    return ApiMessageInfo.Status(1, title.Id);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateUpdateTitle Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return ApiMessageInfo.Status(0);
                }
            }
        }
    }

    #endregion

    #region DeleteTitle
    public class DeleteTitle : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteTitleHandler : IRequestHandler<DeleteTitle, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteTitleHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteTitle request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info DeleteTitle method start----");
                if (request.Id > 0)
                {
                    var city = await _context.Titles.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(city);
                    await _context.SaveChangesAsync();
                    Log.Info("----Info DeleteTitle method end----");
                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {
                Log.Error("Error in DeleteTitle Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }
    }

    #endregion

    #region GetTitleSelectListItem
    public class GetTitleSelectListItem : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetTitleSelectListItemHandler : IRequestHandler<GetTitleSelectListItem, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetTitleSelectListItemHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetTitleSelectListItem request, CancellationToken cancellationToken)
        {
            bool isArab = request.User.Culture.IsArab();
            var list = await _context.Titles.AsNoTracking().OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = isArab ? e.TitleNameAr : e.TitleNameEn, Value = e.TitleCode })
                  .ToListAsync(cancellationToken);
            return list;
        }
    }

    #endregion   
}
