using AutoMapper;
using CIN.Application.Common;
using CIN.Application.OperationsMgtDtos;
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


namespace CIN.Application.OperationsMgtQuery
{
   public class BranchQuery
    {
    }
    #region GetSelectBranchCodeList

    public class GetSelectBranchCodeList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetSelectBranchCodeListHandler : IRequestHandler<GetSelectBranchCodeList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSelectBranchCodeListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetSelectBranchCodeList request, CancellationToken cancellationToken)
        {
            try
            {
              
                Log.Info("----Info GetSelectBranchCodeList method start----");

                var cities = _context.CityCodes.AsNoTracking().ToList();
                var branches = _context.CompanyBranches.Where(e=>e.IsActive).AsNoTracking().ToList();

                List<CustomSelectListItem> items = new();
                branches.ForEach(b=>{
                    CustomSelectListItem item = new();
                    item.Text = b.BranchName;
                    item.Value = b.BranchCode;
                    item.TextTwo = b.BranchAddress;
                    items.Add(item);
                });
                                        
                       
                   //TextTwo = cities.Find(c => c.CityCode == e.City).CityNameAr
               
                Log.Info("----Info GetSelectBranchCodeList method Ends----");
                return items;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetSelectBranchCodeList Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return null;
            }
        }
    }

    #endregion

    #region GetSelectBranchListForUser

    public class GetSelectBranchListForUser : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetSelectBranchListForUserHandler : IRequestHandler<GetSelectBranchListForUser, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSelectBranchListForUserHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetSelectBranchListForUser request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetSelectBranchListForUser method start----");

            bool isAdmin = _context.SystemLogins.AsNoTracking().FirstOrDefault(e => e.Id == request.User.UserId).LoginId == "Admin" ? true : false;

            List<CustomSelectListItem> Result = (from cc in _context.CompanyBranches.Where(e=>e.IsActive)
                                                 join ua in _context.TblOpAuthoritiesList
                                                                       on cc.BranchCode equals ua.BranchCode
                                                 where (ua.AppAuth == request.User.UserId || isAdmin)
                                                 select new CustomSelectListItem
                                                 {
                                                     Text = cc.BranchName,
                                                     Value = cc.BranchCode,
                                                  
                                                 }).Distinct().ToList();



            Log.Info("----Info GetSelectBranchListForUser method Ends----");
            return Result ?? new();
        }
    }

    #endregion

}
