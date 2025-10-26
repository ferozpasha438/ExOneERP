using AutoMapper;
using CIN.Application;
using CIN.Application.Common;
using CIN.Application.SchoolMgtDtos;
using CIN.DB;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using System.Linq.Dynamic.Core;
using CIN.Domain.SchoolMgt;
namespace CIN.Application.SchoolMgtQuery
{

    #region GetAll
    public class GetWebStudentRegistrationList : IRequest<PaginatedList<TblWebStudentRegistrationDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }

    }

    public class GetWebStudentRegistrationListHandler : IRequestHandler<GetWebStudentRegistrationList, PaginatedList<TblWebStudentRegistrationDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetWebStudentRegistrationListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<TblWebStudentRegistrationDto>> Handle(GetWebStudentRegistrationList request, CancellationToken cancellationToken)
        {
            try
            {
                var webStudentRegistrations = await _context.WebStudentRegistration.AsNoTracking()
                   .Select(e => new TblWebStudentRegistrationDto
                   {
                       Id = e.Id,
                       RegNum = e.RegNum,
                       FullName = e.FullName,
                       Grade = e.Grade,
                       FatherPhoneNumber = e.FatherPhoneNumber,
                       FatherEmail = e.FatherEmail,
                       City = e.City
                   })
                   .OrderByDescending(e=>e.Id)
                   .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

                return webStudentRegistrations;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


    }

    #endregion

    #region GetSchoolStudentRegistrationById
    public class GetSchoolStudentRegistrationById : IRequest<TblWebStudentRegistrationDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }

    }

    public class GetSchoolStudentRegistrationByIdHandler : IRequestHandler<GetSchoolStudentRegistrationById, TblWebStudentRegistrationDto>
    {
        private CINDBOneContext _context;
        private IMapper _mapper;

        public GetSchoolStudentRegistrationByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblWebStudentRegistrationDto> Handle(GetSchoolStudentRegistrationById request, CancellationToken cancellationToken)
        {
            var student = await _context.WebStudentRegistration.AsNoTracking()
                .ProjectTo<TblWebStudentRegistrationDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.Id == request.Id);
            return student;
        }
    }

    #endregion


    #region ImportExcelStudentRegistration

    public class ImportExcelStudentRegistration : IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public List<TblWebStudentRegistrationDto> Input { get; set; }
    }

    public class ImportExcelStudentRegistrationHandler : IRequestHandler<ImportExcelStudentRegistration, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public ImportExcelStudentRegistrationHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AppCtrollerDto> Handle(ImportExcelStudentRegistration request, CancellationToken cancellationToken)
        {
            int savedCount = 0, duplicateCount = 0;

            var allRegNumbers = request.Input.Select(e => e.RegNum.Trim()).GroupBy(regNum => regNum).Where(e => e.Count() > 1).ToList();
            if (allRegNumbers.Count() > 0)
                return ApiMessageInfo.Status($"Duplicate Registration Numbers:  {string.Join(", ", allRegNumbers.Select(e => e.Key))}");

            var regNumbers = request.Input.Select(e => e.RegNum.Trim()).ToList();
            var regNumberList = regNumbers.Intersect(_context.WebStudentRegistration.AsNoTracking().Select(e => e.RegNum.Trim()).ToArray());

            if (regNumberList != null && regNumberList.Count() > 0)
                return ApiMessageInfo.Status($"Duplicate Registration Numbers:  {string.Join(", ", regNumberList)}");

            foreach (var obj in request.Input)
            {
                //var hasAssetCode = await _context.WebStudentRegistration.AnyAsync(e => e.AssetCode == obj.AssetCode.Trim().Replace(" ", ""));
                //if (!hasAssetCode)
                //{
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        Log.Info("----Info ImportExcelStudentRegistration method start----");

                        TblWebStudentRegistration stuReg = new()
                        {
                            RegNum = obj.RegNum.Trim().Replace(" ", ""),
                            FullName = $"{obj.FullName} {obj.NameAr}",
                            NameAr = obj.NameAr,
                            RegDate = obj.RegDate,
                            DateOfBirth = obj.DateOfBirth,
                            Age = obj.Age,
                            Grade = obj.Grade,
                            LangCode = obj.LangCode,
                            GenderName = obj.GenderName,
                            IDNumber = obj.IDNumber,
                            Nationality = obj.Nationality,
                            ReligionCode = obj.ReligionCode,
                            PhysicalDisabilityNotes = string.Empty,                            
                            MedicalIssueNotes = string.Empty,
                            City = obj.City,
                            FatherName = obj.FatherName,
                            MotherName = obj.MotherName,
                            FatherEmail = obj.FatherEmail,
                            FatherPhoneNumber = obj.FatherPhoneNumber,
                            MotherPhoneNumber = obj.MotherPhoneNumber,
                            EnglishFluencyLevel = 0,
                            Remarks = obj.Remarks,
                            IsyourchildPottytrained = false,
                            IsActive = obj.IsActive,
                            CreatedBy = "exl"
                        };


                        await _context.WebStudentRegistration.AddAsync(stuReg);
                        await _context.SaveChangesAsync();

                        Log.Info("----Info ImportExcelStudentRegistration method Exit----");
                        await transaction.CommitAsync();
                        savedCount = savedCount + 1;
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        Log.Error("Error in ImportExcelStudentRegistration Method");
                        Log.Error("Error occured time : " + DateTime.UtcNow);
                        Log.Error("Error message : " + ex.Message);
                        Log.Error("Error StackTrace : " + ex.StackTrace);
                        //return ApiMessageInfo.Status(0);
                        //return ApiMessageInfo.Status(ex.Message + " " + ex.InnerException?.Message);
                    }
                }
                //}
                //else
                //    duplicateCount++;
            }

            if (savedCount > 0)
                return ApiMessageInfo.Status(1, savedCount);
            else
                return ApiMessageInfo.Status(0);

        }

    }



    #endregion

    #region Create_And_Update


    public class CreateUpdateWebStudentRegistration : IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblWebStudentRegistrationDto webStudentRegistrationDto { get; set; }
    }

    public class CreateUpdateWebStudentRegistrationHandler : IRequestHandler<CreateUpdateWebStudentRegistration, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateWebStudentRegistrationHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AppCtrollerDto> Handle(CreateUpdateWebStudentRegistration request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info CreateUpdateWebStudentRegistration method start----");

                var obj = request.webStudentRegistrationDto;


                TblWebStudentRegistration webStudentRegistration = new();
                if (obj.Id > 0)
                    webStudentRegistration = await _context.WebStudentRegistration.AsNoTracking().FirstOrDefaultAsync(e => e.Id == obj.Id);


                webStudentRegistration.Id = obj.Id;
                webStudentRegistration.RegNum = obj.RegNum;
                webStudentRegistration.FullName = obj.FullName;
                webStudentRegistration.NameAr = obj.NameAr;
                webStudentRegistration.RegDate = obj.RegDate;
                webStudentRegistration.DateOfBirth = obj.DateOfBirth;
                webStudentRegistration.Age = obj.Age;
                webStudentRegistration.Grade = obj.Grade;
                webStudentRegistration.LangCode = obj.LangCode;
                webStudentRegistration.GenderName = obj.GenderName;
                webStudentRegistration.IDNumber = obj.IDNumber;
                webStudentRegistration.Nationality = obj.Nationality;
                webStudentRegistration.ReligionCode = obj.ReligionCode;
                webStudentRegistration.PhysicalDisability = obj.PhysicalDisability;
                webStudentRegistration.PhysicalDisabilityNotes = obj.PhysicalDisabilityNotes;
                webStudentRegistration.MedicalIssue = obj.MedicalIssue;
                webStudentRegistration.MedicalIssueNotes = obj.MedicalIssueNotes;
                webStudentRegistration.City = obj.City;
                webStudentRegistration.FatherName = obj.FatherName;
                webStudentRegistration.MotherName = obj.MotherName;
                webStudentRegistration.FatherEmail = obj.FatherEmail;
                webStudentRegistration.FatherPhoneNumber = obj.FatherPhoneNumber;
                webStudentRegistration.MotherPhoneNumber = obj.MotherPhoneNumber;
                webStudentRegistration.EnglishFluencyLevel = obj.EnglishFluencyLevel;
                webStudentRegistration.Remarks = obj.Remarks;
                webStudentRegistration.IsyourchildPottytrained = obj.IsyourchildPottytrained;
                webStudentRegistration.IsActive = obj.IsActive;

                if (obj.Id > 0)
                {
                    _context.WebStudentRegistration.Update(webStudentRegistration);
                }
                else
                {
                    webStudentRegistration.CreatedOn = DateTime.Now;
                    webStudentRegistration.CreatedBy = obj.CreatedBy;

                    if (await _context.WebStudentRegistration.AnyAsync(e => e.RegNum == webStudentRegistration.RegNum))
                        return ApiMessageInfo.DuplicateInfo("Duplicate Registration Number");

                    await _context.WebStudentRegistration.AddAsync(webStudentRegistration);
                }
                await _context.SaveChangesAsync();
                Log.Info("----Info CreateUpdateWebStudentRegistration method Exit----");
                return ApiMessageInfo.Status(1, webStudentRegistration.Id);
            }
            catch (Exception ex)
            {
                Log.Error("Error in CreateUpdateWebStudentRegistration Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return ApiMessageInfo.Status(ex.Message + " " + ex.InnerException?.Message);
            }
        }


    }


    #endregion

    #region Delete
    public class DeleteSchoolStudentRegistration : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }
    public class DeleteSchoolStudentRegistrationHandler : IRequestHandler<DeleteSchoolStudentRegistration, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteSchoolStudentRegistrationHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteSchoolStudentRegistration request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info Delete Student Master start----");

                if (request.Id > 0)
                {
                    var studentmaster = await _context.WebStudentRegistration.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.WebStudentRegistration.Remove(studentmaster);

                    await _context.SaveChangesAsync();

                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {

                Log.Error("Error in Delete Student Master");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }
    }
    #endregion
}
