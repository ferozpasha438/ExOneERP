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

    public class GetInsuranceClassList : IRequest<PaginatedList<TblHRMSysInsuranceClassDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetInsuranceClassListHandler : IRequestHandler<GetInsuranceClassList, PaginatedList<TblHRMSysInsuranceClassDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetInsuranceClassListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<TblHRMSysInsuranceClassDto>> Handle(GetInsuranceClassList request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info GetInsuranceClassList method start----");
                var search = request.Input.Query;
                var list = await _context.InsuranceClasses
                    .AsNoTracking()
                    .ProjectTo<TblHRMSysInsuranceClassDto>(_mapper.ConfigurationProvider)
                    .Where(e => (e.InsuranceClassCode.Contains(search) || e.InsuranceClassNameEn.Contains(search)))
                    .OrderByDescending(x => x.Id)
                    .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

                Log.Info("----Info GetInsuranceClassList method end----");
                return list;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetInsuranceClassList Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region GetInsuranceClassById

    public class GetInsuranceClassById : IRequest<TblHRMSysInsuranceClassDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetInsuranceClassByIdHandler : IRequestHandler<GetInsuranceClassById, TblHRMSysInsuranceClassDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetInsuranceClassByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TblHRMSysInsuranceClassDto> Handle(GetInsuranceClassById request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetInsuranceClassById method start----");
            try
            {
                var insuranceClass = await _context.InsuranceClasses.AsNoTracking()
                    .ProjectTo<TblHRMSysInsuranceClassDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(e => e.Id == request.Id);
                Log.Info("----Info GetInsuranceClassById method end----");

                if (insuranceClass is not null)
                    return insuranceClass;
                else
                    return null;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetInsuranceClassById Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region CreateUpdateInsuranceClass

    public class CreateUpdateInsuranceClass : UserIdentityDto, IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblHRMSysInsuranceClassDto Input { get; set; }
    }
    public class CreateUpdateInsuranceClassHandler : IRequestHandler<CreateUpdateInsuranceClass, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateInsuranceClassHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<AppCtrollerDto> Handle(CreateUpdateInsuranceClass request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateUpdateInsuranceClass method start----");
                    var obj = request.Input;
                    TblHRMSysInsuranceClass insuranceClass = new();

                    insuranceClass = await _context.InsuranceClasses
                        .FirstOrDefaultAsync(e => e.InsuranceClassCode == request.Input.InsuranceClassCode);

                    if (insuranceClass is not null)
                    {
                        insuranceClass.InsuranceClassNameEn = obj.InsuranceClassNameEn;
                        insuranceClass.InsuranceClassNameAr = obj.InsuranceClassNameAr;
                        insuranceClass.Id = obj.Id;
                        insuranceClass.IsActive = obj.IsActive;
                        insuranceClass.ModifiedBy = request.User.UserId;
                        insuranceClass.Modified = DateTime.Now;

                        _context.InsuranceClasses.Update(insuranceClass);
                    }
                    else
                    {
                        insuranceClass = new()
                        {
                            InsuranceClassCode = obj.InsuranceClassCode,
                            InsuranceClassNameEn = obj.InsuranceClassNameEn,
                            InsuranceClassNameAr = obj.InsuranceClassNameAr,
                            IsActive = obj.IsActive,
                            CreatedBy = request.User.UserId,
                            Created = DateTime.Now,
                        };
                        await _context.InsuranceClasses.AddAsync(insuranceClass);
                    }
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    Log.Info("----Info CreateUpdateInsuranceClass method Exit----");
                    return ApiMessageInfo.Status(1, insuranceClass.Id);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateUpdateInsuranceClass Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return ApiMessageInfo.Status(0);
                }
            }
        }
    }

    #endregion

    #region DeleteInsuranceClass
    public class DeleteInsuranceClass : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteInsuranceClassHandler : IRequestHandler<DeleteInsuranceClass, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteInsuranceClassHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteInsuranceClass request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info DeleteInsuranceClass method start----");
                if (request.Id > 0)
                {
                    var city = await _context.InsuranceClasses.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(city);
                    await _context.SaveChangesAsync();
                    Log.Info("----Info DeleteInsuranceClass method end----");
                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {
                Log.Error("Error in DeleteInsuranceClass Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }
    }

    #endregion

    #region GetInsuranceClassSelectListItem

    public class GetInsuranceClassSelectListItem : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetInsuranceClassSelectListItemHandler : IRequestHandler<GetInsuranceClassSelectListItem, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetInsuranceClassSelectListItemHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetInsuranceClassSelectListItem request, CancellationToken cancellationToken)
        {
            bool isArab = request.User.Culture.IsArab();
            var list = await _context.InsuranceClasses.AsNoTracking().OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = isArab ? e.InsuranceClassNameAr : e.InsuranceClassNameEn, Value = e.InsuranceClassCode })
                  .ToListAsync(cancellationToken);

            return list;
        }
    }

    #endregion   
}
