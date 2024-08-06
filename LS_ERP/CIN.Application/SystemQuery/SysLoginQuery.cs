using AutoMapper;
using AutoMapper.QueryableExtensions;
using CIN.Application.Common;
using CIN.Application.SystemSetupDtos;
using CIN.DB;
using CIN.Domain.SystemSetup;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CIN.Application.SystemQuery
{

    #region GetPagedList

    public class GetUserList : IRequest<List<TblErpSysLoginDto>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetUserListHandler : IRequestHandler<GetUserList, List<TblErpSysLoginDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetUserListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<TblErpSysLoginDto>> Handle(GetUserList request, CancellationToken cancellationToken)
        {
            var list = await _context.SystemLogins.AsNoTracking()
              .ProjectTo<TblErpSysLoginDto>(_mapper.ConfigurationProvider)
               .OrderByDescending(e => e.Id)
                 .ToListAsync(cancellationToken);

            var brachCodes = _context.UserBranches.AsNoTracking();
            foreach (var item in list)
            {
                item.Password = SecurePasswordHasher.DecodePassword(item.Password);
                item.BranchCodes = brachCodes.Where(e => e.LoginId == item.Id).Select(e => e.BranchCode).ToArray(); //&& e.BranchCode != item.PrimaryBranch
            }


            return list;
        }
    }

    #endregion

    #region GetUserById

    public class GetUserById : IRequest<TblErpSysLoginDto>
    {
        public UserIdentityDto User { get; set; }
        public int UserId { get; set; }
    }

    public class GetUserByIdHandler : IRequestHandler<GetUserById, TblErpSysLoginDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetUserByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblErpSysLoginDto> Handle(GetUserById request, CancellationToken cancellationToken)
        {
            var list = await _context.SystemLogins.AsNoTracking()
              .ProjectTo<TblErpSysLoginDto>(_mapper.ConfigurationProvider)
               .Where(e => e.Id == request.UserId)
                 .FirstOrDefaultAsync(cancellationToken);
            return list;
        }
    }

    #endregion

    #region CheckUserName

    public class CheckUserName : IRequest<bool>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class CheckUserNameHandler : IRequestHandler<CheckUserName, bool>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public CheckUserNameHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<bool> Handle(CheckUserName request, CancellationToken cancellationToken)
        {
            Log.Info("----Info CheckUserName method start----");
            return await _context.SystemLogins.AnyAsync(e => e.UserName == request.Input);
        }
    }

    #endregion

    #region CheckUserLoginId

    public class CheckUserLoginId : IRequest<bool>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class CheckUserLoginIdHandler : IRequestHandler<CheckUserLoginId, bool>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public CheckUserLoginIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<bool> Handle(CheckUserLoginId request, CancellationToken cancellationToken)
        {
            Log.Info("----Info CheckUserLoginId method start----");
            return await _context.SystemLogins.AnyAsync(e => e.LoginId == request.Input);
        }
    }

    #endregion

    #region CreateUpdateSysLogin

    public class CreateUpdateSysLogin : IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblErpSysLoginDto Input { get; set; }
    }

    public class CreateUpdateSysLoginHandler : IRequestHandler<CreateUpdateSysLogin, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public CreateUpdateSysLoginHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<AppCtrollerDto> Handle(CreateUpdateSysLogin request, CancellationToken cancellationToken)
        {

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateUpdateSysLogin method start----");
                    var obj = request.Input;

                    TblErpSysLogin login = await _context.SystemLogins.FirstOrDefaultAsync(e => e.Id == obj.Id) ?? new();
                    login.LoginId = obj.LoginId;
                    login.UserName = obj.UserName;
                    login.UserEmail = obj.UserEmail;
                    login.UserType = obj.UserType;

                    //login.Password = SecurePasswordHasher.Hash(obj.Password);
                    login.Password = SecurePasswordHasher.EncodePassword(obj.Password);

                    login.SwpireCardId = obj.SwpireCardId;
                    login.PrimaryBranch = obj.PrimaryBranch;
                    login.IsActive = obj.IsActive;
                    login.ImagePath = obj.ImagePath;


                    if (obj.Id > 0)
                    {
                        _context.SystemLogins.Update(login);
                    }
                    else
                    {
                        await _context.SystemLogins.AddAsync(login);
                    }

                    await _context.SaveChangesAsync();

                    List<TblErpSysUserBranch> branachCodeList = new() { new() { LoginId = login.Id, BranchCode = obj.PrimaryBranch, IsActive = true } };

                    if (obj.BranchCodes is not null && obj.BranchCodes.Count() > 0)
                    {
                        foreach (var bCode in obj.BranchCodes.Where(e => e != obj.PrimaryBranch))
                        {
                            branachCodeList.Add(new() { BranchCode = bCode, LoginId = login.Id, IsActive = true });
                        }
                    }

                    _context.UserBranches.RemoveRange(await _context.UserBranches.Where(e => e.LoginId == login.Id).ToListAsync());
                    await _context.UserBranches.AddRangeAsync(branachCodeList);

                    //TblErpSysUserBranch userBranch = await _context.UserBranches.FirstOrDefaultAsync(e => e.LoginId == login.Id);

                    //if (userBranch is null)
                    //{
                    //    userBranch = new()
                    //    {
                    //        LoginId = login.Id,
                    //        BranchCode = obj.PrimaryBranch,
                    //        IsActive = true
                    //    };
                    //    await _context.UserBranches.AddAsync(userBranch);

                    //}
                    //else
                    //{
                    //    userBranch.BranchCode = obj.PrimaryBranch;
                    //    _context.UserBranches.Update(userBranch);

                    //}

                    await _context.SaveChangesAsync();

                    //if(obj.BranchCodes is not null && obj.BranchCodes.Count() > 0)
                    //{
                    //    var userBranchList = await _context.UserBranches.Where(e => e.LoginId == login.Id && e.IsActive == false);
                    //    await _context.UserBranches.AddAsync(userBranch);
                    //}



                    await transaction.CommitAsync();
                    return ApiMessageInfo.Status(1, login.Id);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateUpdateSysLogin Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return ApiMessageInfo.Status(0);
                }
            }
        }
    }

    #endregion

    #region UpdateUserPicture

    public class UpdateUserPicture : IRequest<MobileCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class UpdateUserPictureHandler : IRequestHandler<UpdateUserPicture, MobileCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public UpdateUserPictureHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<MobileCtrollerDto> Handle(UpdateUserPicture request, CancellationToken cancellationToken)
        {

            try
            {
                Log.Info("----Info UpdateUserPicture method start----");

                TblErpSysLogin login = await _context.SystemLogins.FirstOrDefaultAsync(e => e.Id == request.User.UserId);

                login.ImagePath = request.Input;

                //_context.SystemLogins.Update(login);
                await _context.SaveChangesAsync();

                return ApiMessageInfo.Status(1, true, login.Id);
            }
            catch (Exception ex)
            {
                Log.Error("Error in UpdateUserPicture Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return ApiMessageInfo.Status(0, false);
            }
        }
    }

    #endregion

    #region Delete
    public class DeleteUser : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteUserQueryHandler : IRequestHandler<DeleteUser, int>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteUserQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteUser request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info delte method start----");
                    Log.Info("----Info delete method end----");

                    if (request.Id > 0)
                    {
                        var userBranchList = _context.UserBranches.Where(e => e.LoginId == request.Id);
                        _context.RemoveRange(userBranchList);

                        var User = await _context.SystemLogins.FirstOrDefaultAsync(e => e.Id == request.Id);
                        _context.Remove(User);
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return request.Id;
                    }
                    return 0;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in delete Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return 0;
                }
            }

        }
    }

    #endregion
}
