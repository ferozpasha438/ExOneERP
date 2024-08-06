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
                var webStudentRegistrations = await _context.WebStudentRegistration.AsNoTracking().ProjectTo<TblWebStudentRegistrationDto>(_mapper.ConfigurationProvider)
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
    #region Create_And_Update


    public class CreateUpdateWebStudentRegistration : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public TblWebStudentRegistrationDto webStudentRegistrationDto { get; set; }
    }

    public class CreateUpdateWebStudentRegistrationHandler : IRequestHandler<CreateUpdateWebStudentRegistration, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateWebStudentRegistrationHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateUpdateWebStudentRegistration request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info CreateUpdateWebStudentRegistration method start----");

                var obj = request.webStudentRegistrationDto;


                TblWebStudentRegistration webStudentRegistration = new();
                if (obj.Id > 0)
                    webStudentRegistration = await _context.WebStudentRegistration.AsNoTracking().FirstOrDefaultAsync(e => e.Id == obj.Id);


                webStudentRegistration.Id = obj.Id;
                webStudentRegistration.FullName = obj.FullName;
                webStudentRegistration.Nationality = obj.Nationality;
                webStudentRegistration.GenderName = obj.GenderName;
                webStudentRegistration.DateOfBirth = obj.DateOfBirth;
                webStudentRegistration.Grade = obj.Grade;
                webStudentRegistration.City = obj.City;
                webStudentRegistration.FatherName = obj.FatherName;
                webStudentRegistration.MotherName = obj.MotherName;
                webStudentRegistration.FatherPhoneNumber = obj.FatherPhoneNumber;
                webStudentRegistration.MotherPhoneNumber = obj.MotherPhoneNumber;
                webStudentRegistration.EnglishFluencyLevel = obj.EnglishFluencyLevel;
                webStudentRegistration.Remarks = obj.Remarks;
                webStudentRegistration.IsyourchildPottytrained = obj.IsyourchildPottytrained;
                webStudentRegistration.IsActive = obj.IsActive;
                webStudentRegistration.CreatedOn = DateTime.Now;
                webStudentRegistration.CreatedBy = obj.CreatedBy;

                if (obj.Id > 0)
                {

                    _context.WebStudentRegistration.Update(webStudentRegistration);
                }
                else
                {
                    await _context.WebStudentRegistration.AddAsync(webStudentRegistration);
                }
                await _context.SaveChangesAsync();
                Log.Info("----Info CreateUpdateWebStudentRegistration method Exit----");
                return webStudentRegistration.Id;
            }
            catch (Exception ex)
            {
                Log.Error("Error in CreateUpdateWebStudentRegistration Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }


    }


    #endregion
}
