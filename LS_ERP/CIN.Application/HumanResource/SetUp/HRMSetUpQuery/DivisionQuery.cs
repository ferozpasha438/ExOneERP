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

    public class GetDivisionList : IRequest<PaginatedList<TblHRMSysDivisionDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetDivisionListHandler : IRequestHandler<GetDivisionList, PaginatedList<TblHRMSysDivisionDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetDivisionListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<TblHRMSysDivisionDto>> Handle(GetDivisionList request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info GetDivisionList method start----");
                var search = request.Input.Query;
                var list = await _context.Divisions.AsNoTracking().ProjectTo<TblHRMSysDivisionDto>(_mapper.ConfigurationProvider)
                  .Where(e => (e.DivisionCode.Contains(search) || e.DivisionNameEn.Contains(search)))
                   .OrderByDescending(x => x.Id)
                     .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);
                Log.Info("----Info GetDivisionList method end----");
                return list;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetDivisionList Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region GetDivisionById

    public class GetDivisionById : IRequest<TblHRMSysDivisionDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetDivisionByIdHandler : IRequestHandler<GetDivisionById, TblHRMSysDivisionDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetDivisionByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TblHRMSysDivisionDto> Handle(GetDivisionById request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetDivisionById method start----");
            try
            {
                var division = await _context.Divisions.AsNoTracking()
                    .ProjectTo<TblHRMSysDivisionDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(e => e.Id == request.Id);
                Log.Info("----Info GetDivisionById method end----");

                if (division is not null)
                    return division;
                else
                    return null;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetDivisionById Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region CreateUpdateDivision

    public class CreateUpdateDivision : UserIdentityDto, IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblHRMSysDivisionDto Input { get; set; }
    }
    public class CreateUpdateDivisionHandler : IRequestHandler<CreateUpdateDivision, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateDivisionHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<AppCtrollerDto> Handle(CreateUpdateDivision request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateUpdateDivision method start----");
                    var obj = request.Input;
                    TblHRMSysDivision division = new();

                    if (request.Input.Id > 0)
                    {
                        division = await _context.Divisions.FirstOrDefaultAsync(e => e.DivisionCode == request.Input.DivisionCode);
                        division.DivisionNameEn = obj.DivisionNameEn;
                        division.DivisionNameAr = obj.DivisionNameAr;
                        division.Id = obj.Id;
                        division.IsActive = obj.IsActive;
                        division.ModifiedBy = request.User.UserId;
                        division.Modified = DateTime.Now;

                        _context.Divisions.Update(division);
                    }
                    else
                    {
                        division = new()
                        {
                            DivisionNameEn = obj.DivisionNameEn,
                            DivisionNameAr = obj.DivisionNameAr,
                            DivisionCode = obj.DivisionCode,
                            IsActive = obj.IsActive,
                            CreatedBy = request.User.UserId,
                            Created = DateTime.Now,
                        };
                        await _context.Divisions.AddAsync(division);
                    }
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    Log.Info("----Info CreateUpdateDivision method Exit----");
                    return ApiMessageInfo.Status(1, division.Id);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateUpdateDivision Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return ApiMessageInfo.Status(0);
                }
            }
        }
    }

    #endregion

    #region DeleteDivision
    public class DeleteDivision : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteDivisionHandler : IRequestHandler<DeleteDivision, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteDivisionHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteDivision request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info DeleteDivision method start----");
                if (request.Id > 0)
                {
                    var city = await _context.Divisions.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(city);
                    await _context.SaveChangesAsync();
                    Log.Info("----Info DeleteDivision method end----");
                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {
                Log.Error("Error in DeleteDivision Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }
    }

    #endregion

    #region GetDivisionSelectListItem

    public class GetDivisionSelectListItem : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetDivisionSelectListItemHandler : IRequestHandler<GetDivisionSelectListItem, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetDivisionSelectListItemHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetDivisionSelectListItem request, CancellationToken cancellationToken)
        {
            bool isArab = request.User.Culture.IsArab();
            var list = await _context.Divisions.AsNoTracking().OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = isArab ? e.DivisionNameAr : e.DivisionNameEn, Value = e.DivisionCode })
                  .ToListAsync(cancellationToken);

            return list;
        }
    }

    #endregion   
}
