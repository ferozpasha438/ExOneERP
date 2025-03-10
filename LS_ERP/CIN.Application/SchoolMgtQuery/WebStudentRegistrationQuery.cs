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
                   }).PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

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
