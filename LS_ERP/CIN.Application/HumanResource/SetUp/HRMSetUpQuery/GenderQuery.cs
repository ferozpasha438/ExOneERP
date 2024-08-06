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

    public class GetGenderList : IRequest<PaginatedList<TblHRMSysGenderDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetGenderListHandler : IRequestHandler<GetGenderList, PaginatedList<TblHRMSysGenderDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetGenderListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<TblHRMSysGenderDto>> Handle(GetGenderList request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info GetGenderList method start----");
                var search = request.Input.Query;
                var list = await _context.Genders.AsNoTracking().ProjectTo<TblHRMSysGenderDto>(_mapper.ConfigurationProvider)
                  .Where(e => (e.GenderCode.Contains(search) || e.GenderNameEn.Contains(search)))
                   .OrderByDescending(x => x.Id)
                     .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);
                Log.Info("----Info GetGenderList method end----");
                return list;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetGenderList Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region GetGenderById

    public class GetGenderById : IRequest<TblHRMSysGenderDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetGenderByIdHandler : IRequestHandler<GetGenderById, TblHRMSysGenderDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetGenderByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TblHRMSysGenderDto> Handle(GetGenderById request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetGenderById method start----");
            try
            {
                var employeeType = await _context.Genders.AsNoTracking()
                    .ProjectTo<TblHRMSysGenderDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(e => e.Id == request.Id);
                Log.Info("----Info GetGenderById method end----");

                if (employeeType is not null)
                    return employeeType;
                else
                    return null;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetGenderById Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region CreateUpdateGender

    public class CreateUpdateGender : UserIdentityDto, IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblHRMSysGenderDto Input { get; set; }
    }
    public class CreateUpdateGenderHandler : IRequestHandler<CreateUpdateGender, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateGenderHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<AppCtrollerDto> Handle(CreateUpdateGender request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateUpdateGender method start----");
                    var obj = request.Input;
                    TblHRMSysGender gender = new();

                    gender = await _context.Genders.FirstOrDefaultAsync(e => e.GenderCode == request.Input.GenderCode);

                    if (gender is not null)
                    {
                        gender.GenderNameEn = obj.GenderNameEn;
                        gender.GenderNameAr = obj.GenderNameAr;
                        gender.Id = obj.Id;
                        gender.IsActive = obj.IsActive;
                        gender.ModifiedBy = request.User.UserId;
                        gender.Modified = DateTime.Now;

                        _context.Genders.Update(gender);
                    }
                    else
                    {
                        gender = new()
                        {
                            GenderNameEn = obj.GenderNameEn,
                            GenderNameAr = obj.GenderNameAr,
                            GenderCode = obj.GenderCode,
                            IsActive = obj.IsActive,
                            CreatedBy = request.User.UserId,
                            Created = DateTime.Now,
                        };
                        await _context.Genders.AddAsync(gender);
                    }
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    Log.Info("----Info CreateUpdateGender method Exit----");
                    return ApiMessageInfo.Status(1, gender.Id);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateUpdateGender Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return ApiMessageInfo.Status(0);
                }
            }
        }
    }

    #endregion

    #region DeleteGender
    public class DeleteGender : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteGenderHandler : IRequestHandler<DeleteGender, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteGenderHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteGender request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info DeleteGender method start----");
                if (request.Id > 0)
                {
                    var gender = await _context.Genders.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(gender);
                    await _context.SaveChangesAsync();
                    Log.Info("----Info DeleteGender method end----");
                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {
                Log.Error("Error in DeleteGender Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }
    }

    #endregion

    #region GetGenderSelectListItem
    public class GetGenderSelectListItem : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetGenderSelectListItemHandler : IRequestHandler<GetGenderSelectListItem, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetGenderSelectListItemHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetGenderSelectListItem request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info GetGenderSelectListItem method start----");
                bool isArab = request.User.Culture.IsArab();
                var list = await _context.Genders.AsNoTracking().OrderByDescending(e => e.Id)
                   .Select(e => new CustomSelectListItem { Text = isArab ? e.GenderNameAr : e.GenderNameEn, Value = e.GenderCode })
                      .ToListAsync(cancellationToken);
                Log.Info("----Info GetGenderSelectListItem method end----");
                return list;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetGenderSelectListItem Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion   
}
