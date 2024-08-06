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
using System.Globalization;

namespace CIN.Application.OperationsMgtQuery
{

    #region EnterEmployeeLeave

    public class EnterEmployeeLeave : IRequest<long>
    {
        public UserIdentityDto User { get; set; }
        public TblOpEmployeeLeavesDto Input { get; set; }
    }

    public class EnterEmployeeLeaveHandler : IRequestHandler<EnterEmployeeLeave, long>
    {
        private readonly CINDBOneContext _context;
        
        private readonly IMapper _mapper;

        public EnterEmployeeLeaveHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            
            _mapper = mapper;
        }

        public async Task<long> Handle(EnterEmployeeLeave request, CancellationToken cancellationToken)
        {

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    DateTime InputDate = Convert.ToDateTime(request.Input.AttnDate, CultureInfo.InvariantCulture);
                    var OldAttList = _context.EmployeeAttendance.AsNoTracking().Where(e => e.EmployeeNumber == request.Input.EmployeeNumber && e.AttnDate == request.Input.AttnDate && e.isDefaultEmployee && (e.Attendance == "P" || e.Attendance == "A")).ToList();


                    TblOpEmployeeLeaves obj = _context.EmployeeLeaves.AsNoTracking().FirstOrDefault(e => e.EmployeeNumber == request.Input.EmployeeNumber && e.AttnDate == request.Input.AttnDate);

                    if (obj == null)
                    {
                        obj = new TblOpEmployeeLeaves();
                        obj.Id = 0;
                    }
                        obj.EmployeeNumber = request.Input.EmployeeNumber;
                        obj.AttnDate = InputDate;
                    obj.AL = request.Input.AL;
                    obj.EL = request.Input.EL;
                    obj.SL = request.Input.SL;
                    obj.UL = request.Input.UL;
                    obj.STL = request.Input.STL;
                    obj.W = request.Input.W;
                    obj.ProjectCode = request.Input.ProjectCode;
                    obj.SiteCode = request.Input.SiteCode;
                    obj.ShiftCode = request.Input.ShiftCode;

                 
                    if (obj.Id > 0)
                    {

                       
                        

                        obj.ModifiedBy = request.User.UserId;
                        obj.Modified = DateTime.UtcNow;


                        _context.EmployeeLeaves.Update(obj);
                        _context.SaveChanges();

                    }
                    else
                    {
                       
                        obj.CreatedBy = request.User.UserId;
                        obj.Created = DateTime.UtcNow;


                        _context.EmployeeLeaves.Add(obj);
                        _context.SaveChanges();

                    }
                    if (OldAttList.Count!=0)
                    {
                        if (OldAttList.Any(a => a.isPosted))
                        {
                            transaction.Rollback();
                            return -1;
                        }

                        _context.EmployeeAttendance.RemoveRange(OldAttList);
                        _context.SaveChanges();

                    }
                    transaction.Commit();
                    return obj.Id;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Log.Error("Error in EnterEmployeeAttendance Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return 0;

                }

            }
        }
    }

    #endregion




    #region CancelLeave
    public class CancelLeave : IRequest<long>
    {
        public UserIdentityDto User { get; set; }
        public long Id { get; set; }
    }

    public class CancelLeaveQueryHandler : IRequestHandler<CancelLeave, long>
    {
        private readonly CINDBOneContext _context;
        private readonly DMCContext _contextDMC;
        private readonly IMapper _mapper;

        public CancelLeaveQueryHandler(CINDBOneContext context,DMCContext contextDMC, IMapper mapper)
        {
            _context = context;
            _contextDMC = contextDMC;
            _mapper = mapper;
        }

        public async Task<long> Handle(CancelLeave request, CancellationToken cancellationToken)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                using (var transaction2 = _contextDMC.Database.BeginTransaction())
                {
                    try
                    {
                        Log.Info("----Info CancelLeave start----");

                        
                            var leave = await _context.EmployeeLeaves.FirstOrDefaultAsync(e => e.Id == request.Id);
                            if (leave is not null)
                            {
                                if(leave.W)             // is  Withdrwan
                                {
                                    var postedWithDrawninHRM =await _contextDMC.EmployeePostedAttendance.FirstOrDefaultAsync(e=>e.EmployeeNumber==leave.EmployeeNumber
                                    && Convert.ToDateTime(leave.AttnDate,CultureInfo.InvariantCulture) ==e.Date.Value
                                    && e.AttnFlag=='W'
                                    );
                                    if (postedWithDrawninHRM is not null)
                                    {
                                        if (postedWithDrawninHRM.IsApproved.Value)
                                        {
                                        return -2;          // attendance already approved in HT
                                        }
                                        _contextDMC.EmployeePostedAttendance.Remove(postedWithDrawninHRM);
                                        _contextDMC.SaveChanges();
                                    }

                                }



                                _context.EmployeeLeaves.Remove(leave);
                                _context.SaveChanges();

                                
                                await transaction.CommitAsync();
                                await transaction2.CommitAsync();
                                return request.Id;
                            }
                            else return -1;
                 
                      
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        await transaction2.RollbackAsync();
                        Log.Error("Error in CancelLeave");
                        Log.Error("Error occured time : " + DateTime.UtcNow);
                        Log.Error("Error message : " + ex.Message);
                        Log.Error("Error StackTrace : " + ex.StackTrace);
                        return 0;
                    }
                }
            }

        }
    }

    #endregion










}


