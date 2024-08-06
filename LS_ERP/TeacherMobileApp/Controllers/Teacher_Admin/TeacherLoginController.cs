using AutoMapper;
using AutoMapper.QueryableExtensions;
using CIN.Application;
using CIN.Application.Common;
using CIN.Application.TeacherMgtDtos;
using CIN.Application.TeacherMgtQuery;
using CIN.DB;
using CIN.Server;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LS.API.TeacherApp.Controllers.Teacher_Admin
{
    public class TeacherLoginController : ApiControllerBase
    {
        private IConfiguration _Config;
        private CINServerDbContext _cinDbContext;
        private readonly CINDBOneContext _context;
        private IOptions<AppMobileSettingsJson> _appSetting;
        private IMapper _mapper;


        //   IConfiguration config, CINServerDbContext cinDbContext, CINDBOneContext context, IOptions<AppMobileSettingsJson> appSettings
        public TeacherLoginController(IConfiguration config,CINServerDbContext cinDbContext, CINDBOneContext context, IMapper mapper,IOptions<AppMobileSettingsJson> appSetting) 
        {
            _Config = config;
            _cinDbContext = cinDbContext;
            _context = context;
            _appSetting = appSetting;
            _mapper = mapper;

        }
        [HttpPost("ValidateTeacherCIN")]
        public async Task<IActionResult> ValidateCinNumber([FromBody] CheckCINTeacherDto Input)
        {
            var metaData = await _cinDbContext.MetaDataList.FirstOrDefaultAsync(e => e.CINNumber == Input.CINNumber);
            if(metaData != null)
            {
                byte[] connectionString= ASCIIEncoding.ASCII.GetBytes(metaData.DBConnectionString);
                var DbConString = Convert.ToBase64String(connectionString);
                var BaseUrl = metaData.SctUrl;
                var ParentUrl = metaData.SchUrl;
                return Ok(new TeacherLoginSuccessMessageDto { Message = "Successfully Validate CIN", Connectionstring = DbConString, BaseUrl = BaseUrl,ParentbaseUrl=ParentUrl, Status = true });
            }
            return BadRequest(new ValidateCinFailedMessageDto { Message = "Invalid CIN Number", Status = false });
        }






        [HttpPost("ValidateTeacherLogin")]
        public async Task<IActionResult> ValidateTeacherLogin([FromBody] CheckTeacherLoginMetaDataDto Input)
        {


            if (Input.UserName != null && Input.Password != null)
            {
                var user = await Mediator.Send(new CheckTeacherlogin()
                {

                    Input = Input

                });
                if (user.Id == -1)
                {
                    var teacher = await _context.DefSchoolTeacherMaster.AsNoTracking().ProjectTo<TblDefSchoolTeacherMasterDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();

                    if(teacher != null) { 
                    var TeacherGradeMapList = await _context.DefSchoolTeacherClassMapping.AsNoTracking().ProjectTo<TblDefSchoolTeacherClassMappingDto>(_mapper.ConfigurationProvider).Where(e => (e.TeacherCode == teacher.TeacherCode)).ToListAsync();

                    var TeacherSubMapList = await _context.DefSchoolTeacherSubjectsMapping.AsNoTracking().ProjectTo<TblDefSchoolTeacherSubjectsMappingDto>(_mapper.ConfigurationProvider).Where(e => e.TeacherCode == teacher.TeacherCode).ToListAsync();



                    var TeacherProfile = await _context.DefSchoolTeacherMaster.AsNoTracking().Select(x => new TblDefSchoolTeacherMasterDto
                    {
                        TeacherCode = x.TeacherCode,
                        TeacherName1 = x.TeacherName1,
                        TeacherName2 = x.TeacherName2,
                        TeacherShortName = x.TeacherShortName,
                        FatherName = x.FatherName,
                        TeachingSkills = x.TeachingSkills,
                        TechnologyCompetence = x.TechnologyCompetence,
                        TotalExperience = x.TotalExperience,
                        HiringType = x.HiringType,
                        ThumbNailImagePath = x.ThumbNailImagePath,
                        AdministrativeSkills = x.AdministrativeSkills,
                        ComminicationSkills = x.ComminicationSkills,
                        DisciplineSkills = x.DisciplineSkills,
                        Saddress = x.Saddress,
                        SMobile2 = x.SMobile2,
                        SpouseName = x.SpouseName,
                        Subjectknowledge = x.Subjectknowledge,
                        SPhone2 = x.SPhone2,
                        SysLoginId = x.SysLoginId,
                        MaritalStatus = x.MaritalStatus,
                        DateJoin = x.DateJoin,
                        FullImageParh = x.FullImageParh,
                        NationalityID = x.NationalityID,
                        Gender = x.Gender,
                        HighestQualification = x.HighestQualification,
                        NationalityCode = x.NationalityCode,
                        PAddress = x.PAddress,
                        Passport = x.Passport,
                        Pcity = x.Pcity,
                        PMobile1 = x.PMobile1,
                        PrimaryBranchCode = x.PrimaryBranchCode,
                        PPhone1 = x.PPhone1,
                        Id = x.Id,
                        CreatedBy = x.CreatedBy,
                        CreatedOn = x.CreatedOn
                    }).FirstOrDefaultAsync();

                    


                    var result= new TeacherMasterLoginDto
                    {
                        SchoolTeacherMaster = TeacherProfile,
                        TeacherMapGradeList = TeacherGradeMapList,
                        TeacherMapSubject = TeacherSubMapList,
                        Message="Success",
                        Status=true

                    };
                    return Ok(result);
                    }
                }

                return BadRequest(new TeacherLoginFailedMessageDto { Message = "Invalid Credential", Status = false });

            }
            return BadRequest(new TeacherLoginFailedMessageDto { Message = "Empty Username & Password", Status = false });
        }


        ///////////////////////////////(FOR TEST PURPOSE ONLY)///////////////////////////


        //[HttpPost("ValidateTeacherLogin2")]
        //public async Task<IActionResult> ValidateTeacherLogin2([FromBody] CheckTeacherLoginMetaDataDto Input)
        //{


        //    if (Input.UserName != null && Input.Password != null)
        //    {
        //        var user = await Mediator.Send(new CheckTeacherlogin2()
        //        {

        //            Input = Input

        //        });
        //        if (user.Id == -1)
        //        {
        //            var teacher = await _context.DefSchoolTeacherMaster.AsNoTracking().FirstOrDefaultAsync();
        //            return new TeacherMasterLoginDto {
        //            Id=teacher.Id,
        //            Message="Sucessful",
        //            Status=true,
        //            TeacherMapGradeList = await _context.DefSchoolTeacherClassMapping.AsNoTracking().ProjectTo<TblDefSchoolTeacherClassMappingDto>(_mapper.ConfigurationProvider).Where(e=>e.TeacherCode==teacher.TeacherCode).ToListAsync(),
        //            TeacherMapSubject =await _context.DefSchoolTeacherSubjectsMapping.AsNoTracking().ProjectTo<TblDefSchoolTeacherSubjectsMappingDto>(_mapper.ConfigurationProvider).Where(e=>e.TeacherCode==teacher.TeacherCode).ToListAsync()


        //            };


        //        }

        //        return BadRequest(new TeacherLoginFailedMessageDto { Message = "Invalid Credential", Status = false });

        //    }
        //    return BadRequest(new TeacherLoginFailedMessageDto { Message = "Empty Username & Password", Status = false });
        //}

    }
}
