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

    public class GetEmployeeList : IRequest<PaginatedList<TblHRMTrnPersonalInformationDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetEmployeeListHandler : IRequestHandler<GetEmployeeList, PaginatedList<TblHRMTrnPersonalInformationDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetEmployeeListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<TblHRMTrnPersonalInformationDto>> Handle(GetEmployeeList request, CancellationToken cancellationToken)
        {
            try
            {
                bool isArab = request.User.Culture.IsArab();
                Log.Info("----Info GetEmployeeList method start----");
                var search = request.Input.Query;

                var list = await (from personalInformation in _context.PersonalInformation
                                  join country in _context.CountryCodes on personalInformation.CountryCode equals country.CountryCode
                                  select new TblHRMTrnPersonalInformationDto
                                  {
                                      EmployeeNumber = personalInformation.EmployeeNumber,
                                      PrimaryNumber = personalInformation.PrimaryNumber,
                                      EmployeeName = isArab ? string.Concat(personalInformation.FirstNameAr, " ", personalInformation.LastNameAr) : string.Concat(personalInformation.FirstNameEn, " ", personalInformation.LastNameEn),
                                      CountryName = country.CountryName,
                                      IsActiveStatus = personalInformation.IsActive ? "Yes" : "No",
                                  })
                                  .AsNoTracking()
                                  .Where(e => (e.EmployeeNumber.Contains(search)))
                                  .OrderBy(x => x.EmployeeNumber)
                                  .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

                Log.Info("----Info GetEmployeeList method end----");
                return list;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetEmployeeList Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region GetEmployeePersonalInformationById

    public class GetEmployeePersonalInformationById : IRequest<TblHRMTrnPersonalInformationDto>
    {
        public UserIdentityDto User { get; set; }
        public string EmployeeNumber { get; set; }
    }

    public class GetEmployeePersonalInformationByIdHandler : IRequestHandler<GetEmployeePersonalInformationById, TblHRMTrnPersonalInformationDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetEmployeePersonalInformationByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TblHRMTrnPersonalInformationDto> Handle(GetEmployeePersonalInformationById request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetEmployeePersonalInformationById method start----");
            try
            {
                bool isArab = request.User.Culture.IsArab();
                var personalInformation = await _context.PersonalInformation.AsNoTracking()
                    .ProjectTo<TblHRMTrnPersonalInformationDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(e => e.EmployeeNumber == request.EmployeeNumber);

                Log.Info("----Info GetEmployeePersonalInformationById method end----");

                if (personalInformation is not null)
                {
                    personalInformation.EmployeeName = isArab ? string.Concat(personalInformation.FirstNameAr, " ", personalInformation.LastNameAr) :
                        string.Concat(personalInformation.FirstNameEn, " ", personalInformation.LastNameEn);
                    personalInformation.EmployeeImageUrl = !string.IsNullOrEmpty(personalInformation.EmployeeImageUrl) ?
                        personalInformation.EmployeeImageUrl : "/assets/images/profile.jpg";

                    var employeeLanguages = await _context.EmployeeLanguages.AsNoTracking()
                        .Where(e => (e.EmployeeId == int.Parse(request.EmployeeNumber)))
                        .ProjectTo<TblHRMTrnEmployeeLanguageInfoDto>(_mapper.ConfigurationProvider)
                        .OrderByDescending(e => e.Id)
                        .ToListAsync(cancellationToken);

                    if (employeeLanguages is not null && employeeLanguages.Count() > 0)
                        personalInformation.EmployeeLanguages = employeeLanguages;
                    else
                        personalInformation.EmployeeLanguages = new List<TblHRMTrnEmployeeLanguageInfoDto>() {
                            new TblHRMTrnEmployeeLanguageInfoDto(){
                                EmployeeId= int.Parse(request.EmployeeNumber),
                                LanguageCode=string.Empty,
                                CanRead=false,
                                CanWrite=false,
                                CanSpeak=false
                            },
                        };
                    return personalInformation;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetEmployeePersonalInformationById Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region CreateUpdatePersonalInformation

    public class CreateUpdatePersonalInformation : UserIdentityDto, IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblHRMTrnPersonalInformationDto Input { get; set; }
    }
    public class CreateUpdatePersonalInformationHandler : IRequestHandler<CreateUpdatePersonalInformation, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdatePersonalInformationHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<AppCtrollerDto> Handle(CreateUpdatePersonalInformation request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateUpdatePersonalInformation method start----");
                    var obj = request.Input;
                    TblHRMTrnPersonalInformation personalInformation = new();

                    if (!string.IsNullOrEmpty(request.Input.EmployeeNumber))
                    {
                        personalInformation = await _context.PersonalInformation.FirstOrDefaultAsync(e => e.EmployeeNumber == request.Input.EmployeeNumber);
                        personalInformation.PrimaryNumber = obj.PrimaryNumber;
                        personalInformation.FirstNameEn = obj.FirstNameEn;
                        personalInformation.FirstNameAr = obj.FirstNameAr;
                        personalInformation.LastNameEn = obj.LastNameEn;
                        personalInformation.LastNameAr = obj.LastNameAr;
                        personalInformation.NickNameEn = obj.NickNameEn;
                        personalInformation.NickNameAr = obj.NickNameAr;
                        personalInformation.DateOfBirth = obj.DateOfBirth;
                        personalInformation.IDNumber1 = obj.IDNumber1;
                        personalInformation.IDNumber2 = obj.IDNumber2;
                        personalInformation.FatherNameEn = obj.FatherNameEn;
                        personalInformation.FatherNameAr = obj.FatherNameAr;
                        personalInformation.MotherNameEn = obj.MotherNameEn;
                        personalInformation.MotherNameAr = obj.MotherNameAr;
                        personalInformation.IsActive = obj.IsActive;
                        personalInformation.IsPhysicallyChallenged = obj.IsPhysicallyChallenged;
                        personalInformation.CountryCode = obj.CountryCode;
                        personalInformation.ReligionCode = obj.ReligionCode;
                        personalInformation.EmployeeTypeCode = obj.EmployeeTypeCode;
                        personalInformation.BloodGroupCode = obj.BloodGroupCode;
                        personalInformation.GenderCode = obj.GenderCode;
                        personalInformation.MaritalStatusCode = obj.MaritalStatusCode;
                        personalInformation.TitleCode = obj.TitleCode;
                        personalInformation.GroupCode = obj.GroupCode;
                        personalInformation.SubGroupCode = obj.SubGroupCode;
                        personalInformation.MarriageDate = obj.MarriageDate;
                        personalInformation.PHDescription = obj.PHDescription;
                        personalInformation.ModifiedBy = request.User.UserId;
                        personalInformation.Modified = DateTime.Now;

                        var employeeLanguages = _context.EmployeeLanguages.Where(e => e.EmployeeId == int.Parse(request.Input.EmployeeNumber));
                        _context.RemoveRange(employeeLanguages);

                        _context.PersonalInformation.Update(personalInformation);
                    }
                    else
                    {
                        personalInformation = new()
                        {
                            PrimaryNumber = obj.PrimaryNumber,
                            FirstNameEn = obj.FirstNameEn,
                            FirstNameAr = obj.FirstNameAr,
                            LastNameEn = obj.LastNameEn,
                            LastNameAr = obj.LastNameAr,
                            NickNameEn = obj.NickNameEn,
                            NickNameAr = obj.NickNameAr,
                            DateOfBirth = obj.DateOfBirth,
                            IDNumber1 = obj.IDNumber1,
                            IDNumber2 = obj.IDNumber2,
                            FatherNameEn = obj.FatherNameEn,
                            FatherNameAr = obj.FatherNameAr,
                            MotherNameEn = obj.MotherNameEn,
                            MotherNameAr = obj.MotherNameAr,
                            IsActive = obj.IsActive,
                            IsPhysicallyChallenged = obj.IsPhysicallyChallenged,
                            CountryCode = obj.CountryCode,
                            ReligionCode = obj.ReligionCode,
                            EmployeeTypeCode = obj.EmployeeTypeCode,
                            BloodGroupCode = obj.BloodGroupCode,
                            GenderCode = obj.GenderCode,
                            MaritalStatusCode = obj.MaritalStatusCode,
                            TitleCode = obj.TitleCode,
                            GroupCode = obj.GroupCode,
                            SubGroupCode = obj.SubGroupCode,
                            MarriageDate = obj.MarriageDate,
                            PHDescription = obj.PHDescription,
                            CreatedBy = request.User.UserId,
                            Created = DateTime.Now,
                        };
                        await _context.PersonalInformation.AddAsync(personalInformation);
                    }

                    await _context.SaveChangesAsync();

                    //Save Employee Languages
                    var employeeId = personalInformation.Id;
                    var employeeLanguagesDto = request.Input.EmployeeLanguages;
                    if (employeeLanguagesDto.Count > 0)
                    {
                        List<TblHRMTrnEmployeeLanguageInfo> employeeLanguages = new();

                        foreach (var employeeLanguageDto in employeeLanguagesDto)
                        {
                            var employeeLanguage = new TblHRMTrnEmployeeLanguageInfo
                            {
                                EmployeeId = employeeId,
                                LanguageCode = employeeLanguageDto.LanguageCode,
                                CanRead = employeeLanguageDto.CanRead,
                                CanWrite = employeeLanguageDto.CanWrite,
                                CanSpeak = employeeLanguageDto.CanSpeak,
                            };
                            employeeLanguages.Add(employeeLanguage);
                        }
                        if (employeeLanguages.Count > 0)
                            await _context.EmployeeLanguages.AddRangeAsync(employeeLanguages);
                    }

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    Log.Info("----Info CreateUpdatePersonalInformation method Exit----");
                    return ApiMessageInfo.Status(1, personalInformation.Id);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateUpdatePersonalInformation Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return ApiMessageInfo.Status(0);
                }
            }
        }
    }

    #endregion
}
