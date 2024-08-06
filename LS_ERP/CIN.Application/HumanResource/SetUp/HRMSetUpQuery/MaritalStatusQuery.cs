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

    public class GetMaritalStatusList : IRequest<PaginatedList<TblHRMSysMaritalStatusDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetMaritalStatusListHandler : IRequestHandler<GetMaritalStatusList, PaginatedList<TblHRMSysMaritalStatusDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetMaritalStatusListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<TblHRMSysMaritalStatusDto>> Handle(GetMaritalStatusList request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info GetMaritalStatusList method start----");
                var search = request.Input.Query;
                var list = await _context.MaritalStatuses.AsNoTracking().ProjectTo<TblHRMSysMaritalStatusDto>(_mapper.ConfigurationProvider)
                  .Where(e => (e.MaritalStatusCode.Contains(search) || e.MaritalStatusNameEn.Contains(search)))
                   .OrderByDescending(x => x.Id)
                     .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);
                Log.Info("----Info GetMaritalStatusList method end----");
                return list;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetMaritalStatusList Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region GetMaritalStatusById

    public class GetMaritalStatusById : IRequest<TblHRMSysMaritalStatusDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetMaritalStatusByIdHandler : IRequestHandler<GetMaritalStatusById, TblHRMSysMaritalStatusDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetMaritalStatusByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TblHRMSysMaritalStatusDto> Handle(GetMaritalStatusById request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetMaritalStatusById method start----");
            try
            {
                var maritalStatus = await _context.MaritalStatuses.AsNoTracking()
                    .ProjectTo<TblHRMSysMaritalStatusDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(e => e.Id == request.Id);
                Log.Info("----Info GetMaritalStatusById method end----");

                if (maritalStatus is not null)
                    return maritalStatus;
                else
                    return null;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetMaritalStatusById Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region CreateUpdateMaritalStatus

    public class CreateUpdateMaritalStatus : UserIdentityDto, IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblHRMSysMaritalStatusDto Input { get; set; }
    }
    public class CreateUpdateMaritalStatusHandler : IRequestHandler<CreateUpdateMaritalStatus, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateMaritalStatusHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<AppCtrollerDto> Handle(CreateUpdateMaritalStatus request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateUpdateMaritalStatus method start----");
                    var obj = request.Input;
                    TblHRMSysMaritalStatus maritalStatus = new();

                    maritalStatus = await _context.MaritalStatuses.FirstOrDefaultAsync(e => e.MaritalStatusCode == request.Input.MaritalStatusCode);

                    if (maritalStatus is not null)
                    {
                        maritalStatus.MaritalStatusNameEn = obj.MaritalStatusNameEn;
                        maritalStatus.MaritalStatusNameAr = obj.MaritalStatusNameAr;
                        maritalStatus.Id = obj.Id;
                        maritalStatus.IsActive = obj.IsActive;
                        maritalStatus.ModifiedBy = request.User.UserId;
                        maritalStatus.Modified = DateTime.Now;

                        _context.MaritalStatuses.Update(maritalStatus);
                    }
                    else
                    {
                        maritalStatus = new()
                        {
                            MaritalStatusNameEn = obj.MaritalStatusNameEn,
                            MaritalStatusNameAr = obj.MaritalStatusNameAr,
                            MaritalStatusCode = obj.MaritalStatusCode,
                            IsActive = obj.IsActive,
                            CreatedBy = request.User.UserId,
                            Created = DateTime.Now,
                        };
                        await _context.MaritalStatuses.AddAsync(maritalStatus);
                    }
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    Log.Info("----Info CreateUpdateMaritalStatus method Exit----");
                    return ApiMessageInfo.Status(1, maritalStatus.Id);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateUpdateMaritalStatus Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return ApiMessageInfo.Status(0);
                }
            }
        }
    }

    #endregion

    #region DeleteMaritalStatus
    public class DeleteMaritalStatus : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteMaritalStatusHandler : IRequestHandler<DeleteMaritalStatus, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteMaritalStatusHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteMaritalStatus request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info DeleteMaritalStatus method start----");
                if (request.Id > 0)
                {
                    var city = await _context.MaritalStatuses.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(city);
                    await _context.SaveChangesAsync();
                    Log.Info("----Info DeleteMaritalStatus method end----");
                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {
                Log.Error("Error in DeleteMaritalStatus Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }
    }

    #endregion

    #region GetMaritalStatusSelectListItem
    public class GetMaritalStatusSelectListItem : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetMaritalStatusSelectListItemHandler : IRequestHandler<GetMaritalStatusSelectListItem, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetMaritalStatusSelectListItemHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetMaritalStatusSelectListItem request, CancellationToken cancellationToken)
        {
            bool isArab = request.User.Culture.IsArab();
            var list = await _context.MaritalStatuses.AsNoTracking().OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = isArab ? e.MaritalStatusNameAr : e.MaritalStatusNameEn, Value = e.MaritalStatusCode })
                  .ToListAsync(cancellationToken);

            return list;
        }
    }

    #endregion   
}
