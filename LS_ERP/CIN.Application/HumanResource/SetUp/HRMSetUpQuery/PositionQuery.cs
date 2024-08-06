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

    public class GetPositionList : IRequest<PaginatedList<TblHRMSysPositionDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetPositionListHandler : IRequestHandler<GetPositionList, PaginatedList<TblHRMSysPositionDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetPositionListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<TblHRMSysPositionDto>> Handle(GetPositionList request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info GetPositionList method start----");
                var search = request.Input.Query;
                var list = await _context.Positions.AsNoTracking().ProjectTo<TblHRMSysPositionDto>(_mapper.ConfigurationProvider)
                  .Where(e => (e.PositionCode.Contains(search) || e.PositionNameEn.Contains(search)))
                   .OrderByDescending(x => x.Id)
                     .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);
                Log.Info("----Info GetPositionList method end----");
                return list;
            }
            catch (Exception ex)
            {
                Log.Error("Error in CreateUpdatePosition Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region GetPositionById

    public class GetPositionById : IRequest<TblHRMSysPositionDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetPositionByIdHandler : IRequestHandler<GetPositionById, TblHRMSysPositionDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetPositionByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TblHRMSysPositionDto> Handle(GetPositionById request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetPositionById method start----");
            try
            {
                var position = await _context.Positions.AsNoTracking()
                    .ProjectTo<TblHRMSysPositionDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(e => e.Id == request.Id);
                Log.Info("----Info GetPositionById method end----");

                if (position is not null)
                    return position;
                else
                    return null;
            }
            catch (Exception ex)
            {
                Log.Error("Error in CreateUpdatePosition Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region CreateUpdatePosition

    public class CreateUpdatePosition : UserIdentityDto, IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblHRMSysPositionDto Input { get; set; }
    }
    public class CreateUpdatePositionHandler : IRequestHandler<CreateUpdatePosition, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdatePositionHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<AppCtrollerDto> Handle(CreateUpdatePosition request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateUpdatePosition method start----");
                    var obj = request.Input;
                    TblHRMSysPosition position = new();

                    if (request.Input.Id > 0)
                    {
                        position = await _context.Positions.FirstOrDefaultAsync(e => e.PositionCode == request.Input.PositionCode);
                        position.PositionNameEn = obj.PositionNameEn;
                        position.PositionNameAr = obj.PositionNameAr;
                        position.Description = obj.Description;
                        position.Id = obj.Id;
                        position.IsActive = obj.IsActive;
                        position.ModifiedBy = request.User.UserId;
                        position.Modified = DateTime.Now;

                        _context.Positions.Update(position);
                    }
                    else
                    {
                        position = new()
                        {
                            PositionNameEn = obj.PositionNameEn,
                            PositionNameAr = obj.PositionNameAr,
                            Description = obj.Description,
                            PositionCode = obj.PositionCode,
                            IsActive = obj.IsActive,
                            CreatedBy = request.User.UserId,
                            Created = DateTime.Now,
                        };
                        await _context.Positions.AddAsync(position);
                    }
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    Log.Info("----Info CreateUpdatePosition method Exit----");
                    return ApiMessageInfo.Status(1, position.Id);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateUpdatePosition Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return ApiMessageInfo.Status(0);
                }
            }
        }
    }

    #endregion

    #region Delete Position
    public class DeletePosition : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeletePositionHandler : IRequestHandler<DeletePosition, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeletePositionHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeletePosition request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info DeletePosition method start----");
                if (request.Id > 0)
                {
                    var city = await _context.Positions.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(city);
                    await _context.SaveChangesAsync();
                    Log.Info("----Info DeletePosition method end----");
                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {
                Log.Error("Error in DeletePosition Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }
    }

    #endregion

    #region GetPositionSelectListItem

    public class GetPositionSelectListItem : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetPositionSelectListItemHandler : IRequestHandler<GetPositionSelectListItem, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetPositionSelectListItemHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetPositionSelectListItem request, CancellationToken cancellationToken)
        {
            bool isArab = request.User.Culture.IsArab();
            var list = await _context.Positions.AsNoTracking().OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = isArab ? e.PositionNameAr : e.PositionNameEn, Value = e.PositionCode })
                  .ToListAsync(cancellationToken);

            return list;
        }
    }

    #endregion   
}
