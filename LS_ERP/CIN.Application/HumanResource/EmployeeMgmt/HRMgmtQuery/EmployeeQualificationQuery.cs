using AutoMapper;
using AutoMapper.QueryableExtensions;
using CIN.Application.Common;
using CIN.Application.HumanResource.EmployeeMgmt.HRMgmtDtos;
using CIN.DB;
using CIN.Domain.HumanResource.EmployeeMgt;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CIN.Application.HumanResource.EmployeeMgmt.HRMgmtQuery
{
    #region GetPagedList

    public class GetEmployeeQualificationsList : IRequest<PaginatedList<TblHRMTrnEmployeeQualificationInfoDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetEmployeeQualificationsListHandler : IRequestHandler<GetEmployeeQualificationsList, PaginatedList<TblHRMTrnEmployeeQualificationInfoDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetEmployeeQualificationsListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<TblHRMTrnEmployeeQualificationInfoDto>> Handle(GetEmployeeQualificationsList request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info GetEmployeeQualificationsList method start----");
                var search = request.Input.Query;
                bool isArab = request.User.Culture.IsArab();
                var list = await (from employeeQualifications in _context.EmployeeQualifications
                                  join degreeTypes in _context.DegreeTypes on employeeQualifications.DegreeTypeCode equals degreeTypes.DegreeTypeCode
                                  join qualifications in _context.Qualifications on employeeQualifications.QualificationCode equals qualifications.QualificationCode
                                  join courseTypes in _context.CourseTypes on employeeQualifications.CourseTypeCode equals courseTypes.CourseTypeCode
                                  join country in _context.CountryCodes on employeeQualifications.CountryCode equals country.CountryCode

                                  select new TblHRMTrnEmployeeQualificationInfoDto
                                  {
                                      Id = employeeQualifications.Id,
                                      EmployeeID = employeeQualifications.EmployeeID,
                                      IsTechnicalQualification = employeeQualifications.IsTechnicalQualification,
                                      DegreeTypeCode = employeeQualifications.DegreeTypeCode,
                                      QualificationCode = employeeQualifications.QualificationCode,
                                      CourseTypeCode = employeeQualifications.CourseTypeCode,
                                      DateOfCertification = employeeQualifications.DateOfCertification,
                                      CollegeOrUniversity = employeeQualifications.CollegeOrUniversity,
                                      CountryCode = employeeQualifications.CountryCode,
                                      Remarks = employeeQualifications.Remarks,
                                      DegreeTypeName = isArab ? degreeTypes.DegreeTypeNameAr : degreeTypes.DegreeTypeNameEn,
                                      QualificationName = isArab ? qualifications.QualificationNameAr : qualifications.QualificationNameEn,
                                      CourseTypeName = isArab ? courseTypes.CourseTypeNameAr : courseTypes.CourseTypeNameEn,
                                      CountryName = country.CountryName,
                                      IsActive = employeeQualifications.IsActive,
                                  })
                                  .AsNoTracking()
                                  .Where(e => (e.EmployeeID == int.Parse(request.Input.Code) && (e.DegreeTypeName.Contains(search) || e.QualificationName.Contains(search) || e.CourseTypeName.Contains(search))))
                                  .OrderByDescending(x => x.Id)
                                  .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);
                Log.Info("----Info GetEmployeeQualificationsList method end----");
                return list;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetEmployeeQualificationsList Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region GetEmployeeQualificationById

    public class GetEmployeeQualificationById : IRequest<TblHRMTrnEmployeeQualificationInfoDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
        public int EmployeeID { get; set; }
    }

    public class GetEmployeeQualificationByIdHandler : IRequestHandler<GetEmployeeQualificationById, TblHRMTrnEmployeeQualificationInfoDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetEmployeeQualificationByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TblHRMTrnEmployeeQualificationInfoDto> Handle(GetEmployeeQualificationById request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetEmployeeQualificationById method start----");
            try
            {
                var employeeQualification = await _context.EmployeeQualifications.AsNoTracking()
                    .ProjectTo<TblHRMTrnEmployeeQualificationInfoDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(e => e.Id == request.Id && e.EmployeeID == request.EmployeeID);
                Log.Info("----Info GetEmployeeQualificationById method end----");

                if (employeeQualification is not null)
                    return employeeQualification;
                else
                    return null;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetEmployeeQualificationById Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region CreateUpdateEmployeeQualification

    public class CreateUpdateEmployeeQualification : UserIdentityDto, IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblHRMTrnEmployeeQualificationInfoDto Input { get; set; }
    }
    public class CreateUpdateEmployeeQualificationHandler : IRequestHandler<CreateUpdateEmployeeQualification, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateEmployeeQualificationHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<AppCtrollerDto> Handle(CreateUpdateEmployeeQualification request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateUpdateEmployeeQualification method start----");
                    var obj = request.Input;
                    TblHRMTrnEmployeeQualificationInfo employeeQualification = new();

                    if (request.Input.Id > 0)
                    {
                        employeeQualification = await _context.EmployeeQualifications
                            .FirstOrDefaultAsync(e => e.Id == request.Input.Id && e.EmployeeID == request.Input.EmployeeID);

                        employeeQualification.IsTechnicalQualification = obj.IsTechnicalQualification;
                        employeeQualification.DegreeTypeCode = obj.DegreeTypeCode;
                        employeeQualification.QualificationCode = obj.QualificationCode;
                        employeeQualification.CourseTypeCode = obj.CourseTypeCode;
                        employeeQualification.DateOfCertification = obj.DateOfCertification;
                        employeeQualification.CollegeOrUniversity = obj.CollegeOrUniversity;
                        employeeQualification.CountryCode = obj.CountryCode;
                        employeeQualification.Remarks = obj.Remarks;
                        employeeQualification.IsActive = obj.IsActive;
                        employeeQualification.ModifiedBy = request.User.UserId;
                        employeeQualification.Modified = DateTime.Now;

                        _context.EmployeeQualifications.Update(employeeQualification);
                    }
                    else
                    {
                        employeeQualification = new()
                        {
                            EmployeeID = obj.EmployeeID,
                            IsTechnicalQualification = obj.IsTechnicalQualification,
                            DegreeTypeCode = obj.DegreeTypeCode,
                            QualificationCode = obj.QualificationCode,
                            CourseTypeCode = obj.CourseTypeCode,
                            DateOfCertification = obj.DateOfCertification,
                            CollegeOrUniversity = obj.CollegeOrUniversity,
                            CountryCode = obj.CountryCode,
                            Remarks = obj.Remarks,
                            IsActive = obj.IsActive,
                            CreatedBy = request.User.UserId,
                            Created = DateTime.Now,
                        };
                        await _context.EmployeeQualifications.AddAsync(employeeQualification);
                    }
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    Log.Info("----Info CreateUpdateEmployeeQualification method Exit----");
                    return ApiMessageInfo.Status(1, employeeQualification.Id);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateUpdateEmployeeQualification Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return ApiMessageInfo.Status(0);
                }
            }
        }
    }

    #endregion

    #region DeleteEmployeeQualification
    public class DeleteEmployeeQualification : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteEmployeeQualificationHandler : IRequestHandler<DeleteEmployeeQualification, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteEmployeeQualificationHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteEmployeeQualification request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info DeleteEmployeeQualification method start----");
                if (request.Id > 0)
                {
                    var employeeQualification = await _context.EmployeeQualifications
                        .FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(employeeQualification);
                    await _context.SaveChangesAsync();
                    Log.Info("----Info DeleteEmployeeQualification method end----");
                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {
                Log.Error("Error in DeleteEmployeeQualification Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }
    }

    #endregion
}
