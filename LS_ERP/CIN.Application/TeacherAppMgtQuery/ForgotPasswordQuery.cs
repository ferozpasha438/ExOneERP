using AutoMapper;
using CIN.Application;
using CIN.Application.Common;
using CIN.Application.TeacherMgtDtos;
using CIN.Application.TeacherAppMgtQuery;
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

namespace CIN.Application.TeacherAppMgtQuery
{
    #region Teacher_Forgot_Password
    public class GetTeacherForgotPassword : IRequest<ForgotPasswordDto>
    {
       public string TeacherCode { get; set; }
    }

    public class ForgotPasswordHandler : IRequestHandler<GetTeacherForgotPassword, ForgotPasswordDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public ForgotPasswordHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ForgotPasswordDto> Handle(GetTeacherForgotPassword request, CancellationToken cancellationToken)
        {
         
            ForgotPasswordDto Password = new ForgotPasswordDto();
            if(request.TeacherCode != null)
            {
                var teacher = await _context.DefSchoolTeacherMaster.AsNoTracking().FirstOrDefaultAsync();
                if(teacher.TeacherCode == request.TeacherCode) { 
                Password = await _context.SystemLogins.AsNoTracking().Where(e => e.UserEmail == teacher.TeacherEmail).
                           Select(e => new ForgotPasswordDto
                           {
                               Message="Success",
                               Status= true
                           }).FirstOrDefaultAsync(cancellationToken);

                }
                else
                {
                    return new ForgotPasswordDto
                    {
                        Message="Failed",
                        Status = false
                    };
                }
            }
            return Password;

        }

    }

    #endregion
}
