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

    public class GetQualificationList : IRequest<PaginatedList<TblHRMSysQualificationDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetQualificationListHandler : IRequestHandler<GetQualificationList, PaginatedList<TblHRMSysQualificationDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetQualificationListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<TblHRMSysQualificationDto>> Handle(GetQualificationList request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info GetQualificationList method start----");
                var search = request.Input.Query;
                bool isArab = request.User.Culture.IsArab();
                var list = await (from qualification in _context.Qualifications
                                  join degreeType in _context.DegreeTypes on qualification.DegreeTypeCode equals degreeType.DegreeTypeCode
                                  select new TblHRMSysQualificationDto
                                  {
                                      Id = qualification.Id,
                                      QualificationCode = qualification.QualificationCode,
                                      QualificationNameEn = qualification.QualificationNameEn,
                                      QualificationNameAr = qualification.QualificationNameAr,
                                      IsTechnicalQualification = qualification.IsTechnicalQualification,
                                      DegreeTypeCode = qualification.DegreeTypeCode,
                                      DegreeTypeName = !isArab ? degreeType.DegreeTypeNameEn : degreeType.DegreeTypeNameAr,
                                      Created = qualification.Created,
                                      CreatedBy = qualification.CreatedBy,
                                      Modified = qualification.Modified,
                                      ModifiedBy = qualification.ModifiedBy,
                                      IsActive = qualification.IsActive,
                                  })
                  .AsNoTracking()
                  .Where(e => (e.QualificationCode.Contains(search) || e.QualificationNameEn.Contains(search)))
                  .OrderByDescending(x => x.Id)
                  .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

                Log.Info("----Info GetQualificationList method end----");
                return list;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetQualificationList Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region GetQualificationById

    public class GetQualificationById : IRequest<TblHRMSysQualificationDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetQualificationByIdHandler : IRequestHandler<GetQualificationById, TblHRMSysQualificationDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetQualificationByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TblHRMSysQualificationDto> Handle(GetQualificationById request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetQualificationById method start----");
            try
            {
                var qualification = await _context.Qualifications.AsNoTracking()
                    .ProjectTo<TblHRMSysQualificationDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(e => e.Id == request.Id);
                Log.Info("----Info GetQualificationById method end----");

                if (qualification is not null)
                    return qualification;
                else
                    return null;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetQualificationById Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region CreateUpdateQualification

    public class CreateUpdateQualification : UserIdentityDto, IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblHRMSysQualificationDto Input { get; set; }
    }
    public class CreateUpdateQualificationHandler : IRequestHandler<CreateUpdateQualification, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateQualificationHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<AppCtrollerDto> Handle(CreateUpdateQualification request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateUpdateQualification method start----");
                    var obj = request.Input;
                    TblHRMSysQualification qualification = new();

                    qualification = await _context.Qualifications.FirstOrDefaultAsync(e => e.QualificationCode == request.Input.QualificationCode);

                    if (qualification is not null)
                    {
                        qualification.QualificationNameEn = obj.QualificationNameEn;
                        qualification.QualificationNameAr = obj.QualificationNameAr;
                        qualification.Id = obj.Id;
                        qualification.IsTechnicalQualification = obj.IsTechnicalQualification;
                        qualification.DegreeTypeCode = obj.DegreeTypeCode;
                        qualification.IsActive = obj.IsActive;
                        qualification.ModifiedBy = request.User.UserId;
                        qualification.Modified = DateTime.Now;

                        _context.Qualifications.Update(qualification);
                    }
                    else
                    {
                        qualification = new()
                        {
                            QualificationCode = obj.QualificationCode,
                            QualificationNameEn = obj.QualificationNameEn,
                            QualificationNameAr = obj.QualificationNameAr,
                            IsTechnicalQualification = obj.IsTechnicalQualification,
                            DegreeTypeCode = obj.DegreeTypeCode,
                            IsActive = obj.IsActive,
                            CreatedBy = request.User.UserId,
                            Created = DateTime.Now,
                        };
                        await _context.Qualifications.AddAsync(qualification);
                    }
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    Log.Info("----Info CreateUpdateQualification method Exit----");
                    return ApiMessageInfo.Status(1, qualification.Id);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateUpdateQualification Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return ApiMessageInfo.Status(0);
                }
            }
        }
    }

    #endregion

    #region DeleteQualification
    public class DeleteQualification : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteQualificationHandler : IRequestHandler<DeleteQualification, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteQualificationHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteQualification request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info DeleteQualification method start----");
                if (request.Id > 0)
                {
                    var city = await _context.Qualifications.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(city);
                    await _context.SaveChangesAsync();
                    Log.Info("----Info DeleteQualification method end----");
                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {
                Log.Error("Error in DeleteQualification Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }
    }

    #endregion

    #region GetDegreeTypeSelectListItem

    public class GetQualificationSelectListItem : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public bool IsTechnicalQualification { get; set; }
        public string DegreeTypeCode { get; set; }
    }

    public class GetQualificationSelectListItemHandler : IRequestHandler<GetQualificationSelectListItem, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetQualificationSelectListItemHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetQualificationSelectListItem request, CancellationToken cancellationToken)
        {
            bool isArab = request.User.Culture.IsArab();
            var list = await _context.Qualifications
                .Where(x => x.IsTechnicalQualification == request.IsTechnicalQualification && x.DegreeTypeCode == request.DegreeTypeCode)
                .AsNoTracking()
                .OrderByDescending(e => e.Id)
                .Select(e => new CustomSelectListItem { Text = isArab ? e.QualificationNameAr : e.QualificationNameEn, Value = e.QualificationCode })
                .ToListAsync(cancellationToken);

            return list;
        }
    }

    #endregion   
}
