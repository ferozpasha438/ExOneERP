using AutoMapper;
using AutoMapper.QueryableExtensions;
using CIN.Application.Common;
using CIN.Application.TimeAndAttendance.Setup.TNASetUpDtos;
using CIN.DB;
using CIN.Domain.TimeAndAttendance.Setup;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CIN.Application.TimeAndAttendance.Setup.TNASetUpQuery
{
    #region GetPagedList

    public class GetPayrollGroupList : IRequest<PaginatedList<TblTNASysPayrollGroupDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetPayrollGroupListHandler : IRequestHandler<GetPayrollGroupList, PaginatedList<TblTNASysPayrollGroupDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetPayrollGroupListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<TblTNASysPayrollGroupDto>> Handle(GetPayrollGroupList request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info GetPayrollGroupList method start----");
                var search = request.Input.Query;

                var list = await _context.PayrollGroups
                    .AsNoTracking()
                    .ProjectTo<TblTNASysPayrollGroupDto>(_mapper.ConfigurationProvider)
                    .Where(e => (e.PayrollGroupCode.Contains(search) || e.PayrollGroupNameEn.Contains(search)))
                    .OrderByDescending(x => x.Id)
                    .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

                Log.Info("----Info GetPayrollGroupList method end----");
                return list;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetPayrollGroupList Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region GetPayrollGroupById

    public class GetPayrollGroupById : IRequest<TblTNASysPayrollGroupDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetPayrollGroupByIdHandler : IRequestHandler<GetPayrollGroupById, TblTNASysPayrollGroupDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetPayrollGroupByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TblTNASysPayrollGroupDto> Handle(GetPayrollGroupById request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetPayrollGroupById method start----");
            try
            {
                var payrollGroup = await _context.PayrollGroups.AsNoTracking()
                    .ProjectTo<TblTNASysPayrollGroupDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(e => e.Id == request.Id);
                Log.Info("----Info GetPayrollGroupById method end----");

                if (payrollGroup is not null)
                    return payrollGroup;
                else
                    return null;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetPayrollGroupById Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region CreateUpdatePayrollGroup

    public class CreateUpdatePayrollGroup : UserIdentityDto, IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblTNASysPayrollGroupDto Input { get; set; }
    }
    public class CreateUpdatePayrollGroupHandler : IRequestHandler<CreateUpdatePayrollGroup, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdatePayrollGroupHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<AppCtrollerDto> Handle(CreateUpdatePayrollGroup request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateUpdatePayrollGroup method start----");
                    var obj = request.Input;
                    TblTNASysPayrollGroup payrollGroup = new();

                    payrollGroup = await _context.PayrollGroups.FirstOrDefaultAsync(e => e.PayrollGroupCode == request.Input.PayrollGroupCode);

                    if (payrollGroup is not null)
                    {
                        payrollGroup.PayrollGroupNameEn = obj.PayrollGroupNameEn;
                        payrollGroup.PayrollGroupNameAr = obj.PayrollGroupNameAr;
                        payrollGroup.Id = obj.Id;
                        payrollGroup.PayrollGroupStartDate = obj.PayrollGroupStartDate;
                        payrollGroup.PayrollGroupEndDate = obj.PayrollGroupEndDate;
                        payrollGroup.IsActive = obj.IsActive;
                        payrollGroup.ModifiedBy = request.User.UserId;
                        payrollGroup.Modified = DateTime.Now;

                        _context.PayrollGroups.Update(payrollGroup);
                    }
                    else
                    {
                        payrollGroup = new()
                        {
                            PayrollGroupCode = obj.PayrollGroupCode,
                            PayrollGroupNameEn = obj.PayrollGroupNameEn,
                            PayrollGroupNameAr = obj.PayrollGroupNameAr,
                            PayrollGroupStartDate = obj.PayrollGroupStartDate,
                            PayrollGroupEndDate = obj.PayrollGroupEndDate,
                            IsActive = obj.IsActive,
                            CreatedBy = request.User.UserId,
                            Created = DateTime.Now,
                        };
                        await _context.PayrollGroups.AddAsync(payrollGroup);
                    }
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    Log.Info("----Info CreateUpdatePayrollGroup method Exit----");
                    return ApiMessageInfo.Status(1, payrollGroup.Id);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateUpdatePayrollGroup Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return ApiMessageInfo.Status(0);
                }
            }
        }
    }

    #endregion

    #region DeletePayrollGroup
    public class DeletePayrollGroup : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeletePayrollGroupHandler : IRequestHandler<DeletePayrollGroup, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeletePayrollGroupHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeletePayrollGroup request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info DeletePayrollGroup method start----");
                if (request.Id > 0)
                {
                    var payrollGroup = await _context.PayrollGroups.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(payrollGroup);
                    await _context.SaveChangesAsync();
                    Log.Info("----Info DeletePayrollGroup method end----");
                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {
                Log.Error("Error in DeletePayrollGroup Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }
    }

    #endregion

    #region GetPayrollGroupSelectListItem
    public class GetPayrollGroupSelectListItem : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string CountryCode { get; set; }
    }

    public class GetPayrollGroupSelectListItemHandler : IRequestHandler<GetPayrollGroupSelectListItem, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetPayrollGroupSelectListItemHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetPayrollGroupSelectListItem request, CancellationToken cancellationToken)
        {
            bool isArab = request.User.Culture.IsArab();
            var list = await _context.PayrollGroups
                .AsNoTracking()
                .OrderByDescending(e => e.Id)
                .Select(e => new CustomSelectListItem { Text = isArab ? e.PayrollGroupNameAr : e.PayrollGroupNameEn, Value = e.PayrollGroupCode })
                .ToListAsync(cancellationToken);
            return list;
        }
    }

    #endregion   
}
