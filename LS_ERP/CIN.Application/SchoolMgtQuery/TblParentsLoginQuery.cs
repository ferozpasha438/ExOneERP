using AutoMapper;
using CIN.Application;
using CIN.Application.Common;
using CIN.Application.SchoolMgtDto;
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
using CIN.Server;

namespace CIN.Application.SchoolMgtQuery
{

    #region  Login

    public class CheckParentslogin : IRequest<TblParentsLoginDto>
    {
        public UserIdentityDto User { get; set; }
        public CheckParentLoginMetaDataDto Input { get; set; }

    }
    public class CheckParentsloginHandler : IRequestHandler<CheckParentslogin, TblParentsLoginDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public CheckParentsloginHandler(CINDBOneContext context,  IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblParentsLoginDto> Handle(CheckParentslogin request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _context.TblParentsLogin.FirstOrDefaultAsync(e => (e.RegisteredEmail == request.Input.UserName || e.RegisteredPhone==request.Input.UserName) && e.Password==request.Input.Password);
               
                if (user is not null)
                    return new() { Id = -1 };
                else
                    return new() { Id = 1 }; ;
            }
            catch (Exception ex)
            {
                Log.Error("Error in Parentslogin");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return new() { Id = 0 };

            }
        }
    }

    #endregion

    #region Registraion
 
    public class ParentsRegistraion :IRequest<int>
    { 
        public UserIdentityDto User { get; set; }
        public TblParentsLoginDto parentsRegistration { get; set; }

    }

    public class ParentsRegistraionHandler : IRequestHandler<ParentsRegistraion, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public ParentsRegistraionHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(ParentsRegistraion request, CancellationToken cancellationToken)
        {
            Log.Info("----Info CreateUpdateParentsRegistration method start----");

            try
            {
                var obj = request.parentsRegistration;
                TblParentsLogin TblParentsLogin = new();
                var wardnumber = await _context.DefSchoolStudentMaster.AsNoTracking().FirstOrDefaultAsync(e => e.StuAdmNum == obj.WardNumber  );
                if (wardnumber is null)
                {
                    return 2;
                }
                    if (obj.Id > 0)
                    TblParentsLogin = await _context.TblParentsLogin.AsNoTracking().FirstOrDefaultAsync(e => e.Id == obj.Id);

                var regnumber = await _context.DefSchoolStudentMaster.AsNoTracking().FirstOrDefaultAsync(e => e.RegisteredPhone == obj.RegisteredPhone);
                if (regnumber is null)
                {
                    return 3;
                }
                    

                    var ParentsLogin=await _context.TblParentsLogin.AsNoTracking().FirstOrDefaultAsync(e => (e.RegisteredEmail == request.parentsRegistration.RegisteredEmail || e.RegisteredPhone == request.parentsRegistration.RegisteredPhone) && e.Password == request.parentsRegistration.Password);
                if (ParentsLogin is not null)
                {
                    return -2;
                }
                string Name = obj.Name;
                string SchoolId = obj.SchoolId;
                
                        TblParentsLogin.Id = obj.Id;
                        TblParentsLogin.RegisteredPhone = obj.RegisteredPhone;
                        TblParentsLogin.RegisteredEmail = obj.RegisteredEmail;
                        TblParentsLogin.Password = obj.Password;
                        TblParentsLogin.InactiveOn = DateTime.Now;
                        TblParentsLogin.IsApprove = true;
                        TblParentsLogin.ApproveDate = DateTime.Now;
                        TblParentsLogin.CurrentLogin = true;
                        TblParentsLogin.IsActive = obj.IsActive;

                    if (obj.Id > 0)
                    {
                        //_context.TblParentsLogin.Update(TblParentsLogin);

                    }
                    else
                    {
                        TblParentsLogin.RegistedDate = DateTime.Now;
                        await _context.TblParentsLogin.AddAsync(TblParentsLogin);
                    }
                    await _context.SaveChangesAsync();
                    Log.Info("----Info CreateUpdateParentsRegistration method Exit----");
                    return TblParentsLogin.Id;

                
               
            }
            catch (Exception ex)
            {
                Log.Error("Error in Create Update Parents Registration Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
           // throw new NotImplementedException();
        }
    }
    #endregion

    #region Forgot Password
    public class ForgotPassword : IRequest<ForgotPasswordDto>
    {
        public UserIdentityDto User { get; set; }
        public string  EmailId { get; set; }

    }
    public class ForgotPasswordHandler : IRequestHandler<ForgotPassword, ForgotPasswordDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public ForgotPasswordHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ForgotPasswordDto> Handle(ForgotPassword request, CancellationToken cancellationToken)
        {
            try
            {
                

                var password = await _context.TblParentsLogin.AsNoTracking().Where(e => e.RegisteredEmail == request.EmailId).
                           Select(e => new ForgotPasswordDto
                           {
                               Password = e.Password

                           }).FirstOrDefaultAsync(cancellationToken);

                return password;

               

            }
            catch (Exception ex)
            {
                Log.Error("Error in Forgot Password");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return null ;

            }
        }
    }
    #endregion

    #region GET OTP
    public class GetOtpByMobile: IRequest<OTPMobileDto>
    {
        public UserIdentityDto User { get; set; }
        public int  OTP { get; set; }
        public string Mobile { get; set; }
    }

   public class GetOtpByMobileHandler : IRequestHandler<GetOtpByMobile,OTPMobileDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public GetOtpByMobileHandler(CINDBOneContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<OTPMobileDto> Handle(GetOtpByMobile request,CancellationToken cancellationToken)
        {
            try
            {
                var data = await _context.TblParentsLogin.AsNoTracking().Where(e => e.RegisteredPhone == request.Mobile)
                .Select(e => new OTPMobileDto
                 {
                    OTP = request.OTP,
                    Mobile = request.Mobile

                }).FirstOrDefaultAsync(cancellationToken);

                return data;
            }
            catch(Exception ex)
            {
                Log.Error("Error in Get OTP");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return null;
            }
        }
    }
    #endregion





    #region ParentAddRequest

    public class CreateParentAddRequest : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public TblParentAddRequestDto Input { get; set; }

    }

    public class CreateParentAddRequestHandler : IRequestHandler<CreateParentAddRequest, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public CreateParentAddRequestHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateParentAddRequest request, CancellationToken cancellationToken)
        {
            Log.Info("----Info CreateUpdateParentAddRequest method start----");

            try
            {
                var obj = request.Input;
                TblParentAddRequest TblParentAddRequest = new();

                if (obj.Id > 0)
                    TblParentAddRequest = await _context.ParentAddRequest.AsNoTracking().FirstOrDefaultAsync(e => e.Id == obj.Id);

                TblParentAddRequest.Id = obj.Id;
                TblParentAddRequest.RegisteredMobile = obj.RegisteredMobile;
                TblParentAddRequest.RegisteredEmail = obj.RegisteredEmail;
                TblParentAddRequest.RequestDate = DateTime.Now;
                TblParentAddRequest.StuAdmNum = obj.StuAdmNum;
                TblParentAddRequest.Notes = obj.Notes;
                TblParentAddRequest.IsAdded = true;
                TblParentAddRequest.AddedOn = DateTime.Now;
                TblParentAddRequest.AddedBy = null;

                if (obj.Id > 0)
                {
                    //_context.TblParentsLogin.Update(TblParentsLogin);

                }
                else
                {
                    //TblParentsLogin.RegistedDate = DateTime.Now;
                    await _context.ParentAddRequest.AddAsync(TblParentAddRequest);
                }
                await _context.SaveChangesAsync();
                Log.Info("----Info CreateUpdateParentAddRequest method Exit----");
                return TblParentAddRequest.Id;



            }
            catch (Exception ex)
            {
                Log.Error("Error in Create Update Parent Add Request Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
            // throw new NotImplementedException();
        }
    }
    #endregion
}



