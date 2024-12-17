using AutoMapper;
using CIN.Application;
using CIN.Application.Common;
using CIN.Application.TeacherMgtDtos;
using CIN.Application.TeacherMgtQuery;
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

namespace CIN.Application.TeacherMgtQuery
{

    #region  Login

    public class CheckTeacherlogin : IRequest<TblDefSchoolTeacherMasterDto>
    {
        public UserIdentityDto User { get; set; }
        public CheckTeacherLoginMetaDataDto Input { get; set; }

    }
    public class CheckTeacherloginHandler : IRequestHandler<CheckTeacherlogin, TblDefSchoolTeacherMasterDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public CheckTeacherloginHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblDefSchoolTeacherMasterDto> Handle(CheckTeacherlogin request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _context.SystemLogins.FirstOrDefaultAsync(e => (e.UserName == request.Input.UserName ) && e.UserType == "Teacher");
                if (SecurePasswordHasher.IsPasswordDecoded(user.Password, request.Input.Password))
                {
                    if (user is not null)
                    {
                        var teacher = _context.DefSchoolTeacherMaster.FirstOrDefault(e => (e.SysLoginId == user.Id));

                        if (teacher is not null)
                            return new() { Id = teacher.Id };
                        else
                            return new() { Id = -1 };
                    }
                }
                return new() { Id = -1 };
            }
            catch (Exception ex)
            {
                Log.Error("Error in Teacher Login");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return new() { Id = 0 };

            }
        }
    }

    #endregion


    //#region  Login2

    //public class CheckTeacherlogin2 : IRequest<TeacherMasterLoginDto>
    //{
    //    public UserIdentityDto User { get; set; }
    //    public CheckTeacherLoginMetaDataDto Input { get; set; }

    //}
    //public class CheckTeacherlogin2Handler : IRequestHandler<CheckTeacherlogin2, TeacherMasterLoginDto>
    //{
    //    private readonly CINDBOneContext _context;
    //    private readonly IMapper _mapper;
    //    public CheckTeacherlogin2Handler(CINDBOneContext context, IMapper mapper)
    //    {
    //        _context = context;
    //        _mapper = mapper;
    //    }
    //    public async Task<TeacherMasterLoginDto> Handle(CheckTeacherlogin2 request, CancellationToken cancellationToken)
    //    {
    //        try
    //        {
    //            var user = await _context.SystemLogins.FirstOrDefaultAsync(e => (e.UserName == request.Input.UserName && e.Password == request.Input.Password) && e.UserType == "Teacher");

    //            if (user is not null)
    //            {
    //                var teacher = _context.DefSchoolTeacherMaster.FirstOrDefault(e => (e.SysLoginId == user.Id));

    //                if (teacher is not null)
    //                    return new() { Id = -1 };
    //                else
    //                    return new() { Id = 1 };
    //            }
    //            return new() { Id = 1 };
    //        }
    //        catch (Exception ex)
    //        {
    //            Log.Error("Error in Teacher Login");
    //            Log.Error("Error occured time : " + DateTime.UtcNow);
    //            Log.Error("Error message : " + ex.Message);
    //            Log.Error("Error StackTrace : " + ex.StackTrace);
    //            return new() { Id = 0 };

    //        }
    //    }
    //}

    //#endregion


}
