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







    #region MapEmployeesToResources
    public class MapEmployeesToResources : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public List<TblOpEmployeeToResourceMapDto> Input { get; set; }
    }

    public class MapEmployeesToResourcesHandler : IRequestHandler<MapEmployeesToResources, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public MapEmployeesToResourcesHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(MapEmployeesToResources request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try

                {
                    for (int i = 0; i < request.Input.Count; i++)
                    {
                        var obj = request.Input[i];
                        TblOpEmployeeToResourceMap map = new TblOpEmployeeToResourceMap();
                        if (obj.MapId > 0)
                        {
                            map = await _context.TblOpEmployeeToResourceMapList.AsNoTracking().FirstOrDefaultAsync(x => x.MapId == obj.MapId);
                        }

                        map.ProjectCode = obj.ProjectCode;
                        map.SiteCode = obj.SiteCode;
                        map.EmployeeID = obj.EmployeeID;
                        map.EmployeeNumber = obj.EmployeeNumber;
                        map.SkillSet = obj.SkillSet;
                        if (obj.MapId > 0)
                        {

                            _context.TblOpEmployeeToResourceMapList.Update(map);
                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            //map.isPrimarySite = true;
                            await _context.TblOpEmployeeToResourceMapList.AddAsync(map);
                            await _context.SaveChangesAsync();

                        }

                    }

                    Log.Info("----Info MapEmployeesToResources method Exit----");
                    await transaction.CommitAsync();
                    return 1;




                }


                catch (Exception ex)
                {
                    Log.Error("Error in MapEmployeesToResources Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return 0;

                }
            }
        }
    }


    #endregion

    #region GetEmployeesToResourcesMapForProjectSite

    public class GetEmployeesToResourcesMapForProjectSite : IRequest<List<TblOpEmployeeToResourceMapDto>>
    {
        public UserIdentityDto User { get; set; }
        public string SiteCode { get; set; }

        public string ProjectCode { get; set; }


    }

    public class GetEmployeesToResourcesMapForProjectSiteHandler : IRequestHandler<GetEmployeesToResourcesMapForProjectSite, List<TblOpEmployeeToResourceMapDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetEmployeesToResourcesMapForProjectSiteHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<TblOpEmployeeToResourceMapDto>> Handle(GetEmployeesToResourcesMapForProjectSite request, CancellationToken cancellationToken)
        {
            //List<OpEmployeesToProjectSiteDto> result = new();
            //try
            //{
            List<TblOpEmployeeToResourceMapDto> emps = await _context.TblOpEmployeeToResourceMapList.AsNoTracking().ProjectTo<TblOpEmployeeToResourceMapDto>(_mapper.ConfigurationProvider).Where(e => e.ProjectCode == request.ProjectCode && e.SiteCode == request.SiteCode).ToListAsync();
            //if (emps.Count > 0)
            //{
            //    emps.ForEach(e=> {
            //       HRM_TRAN_EmployeeDto emp =_context.HRM_TRAN_Employees.AsNoTracking().ProjectTo<HRM_TRAN_EmployeeDto>(_mapper.ConfigurationProvider).FirstOrDefault(x=>x.EmployeeID==e.EmployeeID);
            //        OpEmployeesToProjectSiteDto res = new OpEmployeesToProjectSiteDto
            //        { 
            //        EmployeeID=e.EmployeeID,
            //        EmployeeData=emp,
            //        ProjectCode=e.ProjectCode,
            //        SiteCode=e.SiteCode,
            //        };
            //        result.Add(res);
            //    });
            //}

            return emps;
            //}
            //catch (Exception ex)
            //{
            //    Log.Error("Error in GetEmployeesOfProjectSiteHandler");
            //    Log.Error("Error occured time : " + DateTime.UtcNow);
            //    Log.Error("Error message : " + ex.Message);
            //    Log.Error("Error StackTrace : " + ex.StackTrace);
            //    return null;
            //}
        }
    }



    #endregion








}






