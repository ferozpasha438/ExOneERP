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
using CIN.Domain.OpeartionsMgt;


namespace CIN.Application.OperationsMgtQuery
{







    #region AssignEmployeesToProjectSite
    public class AssignEmployeesToProjectSite : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public List<TblOpEmployeesToProjectSiteDto> Input { get; set; }
    }

    public class AssignEmployeesToProjectSiteHandler : IRequestHandler<AssignEmployeesToProjectSite, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public AssignEmployeesToProjectSiteHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(AssignEmployeesToProjectSite request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try

                {
                    var oldList =await _context.TblOpEmployeesToProjectSiteList.AsNoTracking().Where(e => e.ProjectCode == request.Input[0].ProjectCode && e.SiteCode == request.Input[0].SiteCode).ToListAsync();

                    if (oldList.Count > 0)
                    {
                        oldList.ForEach(e =>
                        {
                            if (request.Input.FindIndex(x => x.Id == e.Id) < 0)
                            {
                                _context.TblOpEmployeesToProjectSiteList.Remove(e);
                                 _context.SaveChanges();

                            }
                        });
                           



                        //_context.TblOpEmployeesToProjectSiteList.RemoveRange(oldList);
                        //await _context.SaveChangesAsync();
                    }

                    List<TblOpEmployeesToProjectSite> newList = new();
                    
                    request.Input.ForEach(e =>
                    {
                        if (e.Id ==0)
                        {


                            TblOpEmployeesToProjectSite emp = new TblOpEmployeesToProjectSite
                            {
                                Id = 0,
                                ProjectCode = e.ProjectCode,
                                SiteCode = e.SiteCode,
                                EmployeeNumber = e.EmployeeNumber,
                                EmployeeID = e.EmployeeID,
                                EmployeeNameAr = e.EmployeeNameAr,
                                EmployeeName = e.EmployeeName,
                                Created = DateTime.UtcNow,
                                CreatedBy = request.User.UserId,
                                IsActive = true

                            };
                            newList.Add(emp);
                        }
                    });

                    await _context.TblOpEmployeesToProjectSiteList.AddRangeAsync(newList);
                    await _context.SaveChangesAsync();
                    Log.Info("----Info AssignEmployeesToProjectSite method Exit----");
                    await transaction.CommitAsync();
                    return 1;




                }


                catch (Exception ex)
                {
                    Log.Error("Error in AssignEmployeesToProjectSite Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return 0;

                }
            }
        }
    }


    #endregion

    #region GetEmployeesOfProjectSite
 
    public class GetEmployeesOfProjectSite : IRequest<List<TblOpEmployeesToProjectSiteDto>>
    {
        public UserIdentityDto User { get; set; }
        public string SiteCode { get; set; }
       
        public string ProjectCode { get; set; }
      

    }

    public class GetEmployeesOfProjectSiteHandler : IRequestHandler<GetEmployeesOfProjectSite, List<TblOpEmployeesToProjectSiteDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetEmployeesOfProjectSiteHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<TblOpEmployeesToProjectSiteDto>> Handle(GetEmployeesOfProjectSite request, CancellationToken cancellationToken)
        {
            
              List<TblOpEmployeesToProjectSiteDto> emps =await _context.TblOpEmployeesToProjectSiteList.AsNoTracking().ProjectTo<TblOpEmployeesToProjectSiteDto>(_mapper.ConfigurationProvider).Where(e => e.ProjectCode == request.ProjectCode && e.SiteCode == request.SiteCode).ToListAsync();
               
                return emps;
            
        }
    }



    #endregion
    #region GetEmployeeOfProjectSiteByEmpNumber

    public class GetEmployeeOfProjectSiteByEmpNumber : IRequest<TblOpEmployeesToProjectSiteDto>
    {
        public UserIdentityDto User { get; set; }
        public string SiteCode { get; set; }
       
        public string ProjectCode { get; set; }
        public string EmployeeNumber { get; set; }
      

    }

    public class GetEmployeeOfProjectSiteByEmpNumberHandler : IRequestHandler<GetEmployeeOfProjectSiteByEmpNumber,TblOpEmployeesToProjectSiteDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetEmployeeOfProjectSiteByEmpNumberHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblOpEmployeesToProjectSiteDto> Handle(GetEmployeeOfProjectSiteByEmpNumber request, CancellationToken cancellationToken)
        {

            try
            {
                TblOpEmployeesToProjectSiteDto emps =await _context.TblOpEmployeesToProjectSiteList.AsNoTracking().ProjectTo<TblOpEmployeesToProjectSiteDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.ProjectCode == request.ProjectCode && e.SiteCode == request.SiteCode &&e.EmployeeNumber==request.EmployeeNumber);
                

                return emps;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetEmployeesOfProjectSiteHandler");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return null;
            }
        }
    }



    #endregion





    #region GetAutoFillEmployeeListForProjectSite

    public class GetAutoFillEmployeeListForProjectSite : IRequest<List<LanCustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string SiteCode { get; set; }

        public string ProjectCode { get; set; }
        public string Search { get; set; }
    }

    public class GetAutoFillEmployeeListForProjectSiteHandler : IRequestHandler<GetAutoFillEmployeeListForProjectSite, List<LanCustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly DMCContext _contextDMC;

        private readonly IMapper _mapper;
        public GetAutoFillEmployeeListForProjectSiteHandler(CINDBOneContext context, DMCContext contextDMC, IMapper mapper)
        {
            _context = context;
            _contextDMC = contextDMC;
            _mapper = mapper;
        }
        public async Task<List<LanCustomSelectListItem>> Handle(GetAutoFillEmployeeListForProjectSite request, CancellationToken cancellationToken)
        {
            var employeesFromDMC = await _contextDMC.HRM_TRAN_Employees.AsNoTracking().ToListAsync();

            var list = await _context.TblOpEmployeesToProjectSiteList.Where(e=>e.ProjectCode==request.ProjectCode&&e.SiteCode==request.SiteCode).Select(x=>new LanCustomSelectListItem() { Value = x.EmployeeID.ToString(),Text=x.EmployeeNumber,
                TextTwo= x.EmployeeName,TextAr= x.EmployeeNameAr
            }).Where(s=>s.Text.Contains(request.Search)||s.Value.Contains(request.Search)||s.TextTwo.Contains(request.Search)|| s.TextAr.Contains(request.Search) || request.Search==null).ToListAsync(cancellationToken);
            return list;
        }
    }

    #endregion


}






