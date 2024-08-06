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

    

    #region CreateUpdateShiftMaster
    public class CreateEmployeeShifts : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public HRM_DEF_EmployeeShiftDto EmployeeShiftDto { get; set; }
    }

    public class CreateEmployeeShiftsHandler : IRequestHandler<CreateEmployeeShifts, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateEmployeeShiftsHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateEmployeeShifts request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info CreateUpdateShiftMaster method start----");



                var obj = request.EmployeeShiftDto;


                HRM_DEF_EmployeeShift ShiftMaster = new();
                if (obj.ID > 0)
                    ShiftMaster = await _context.HRM_DEF_EmployeeShifts.AsNoTracking().FirstOrDefaultAsync(e => e.ID == obj.ID);


                ShiftMaster.EmployeeID = obj.EmployeeID;
                ShiftMaster.MondayShiftId = obj.MondayShiftId;
                ShiftMaster.TuesdayShiftId = obj.TuesdayShiftId;
                ShiftMaster.WednesdayShiftId = obj.WednesdayShiftId;
                ShiftMaster.ThursdayShiftId = obj.ThursdayShiftId;
                ShiftMaster.FridayShiftId = obj.FridayShiftId;
                ShiftMaster.SaturdayShiftId = obj.SaturdayShiftId;
                ShiftMaster.SundayShiftId = obj.SundayShiftId;

                if (obj.ID > 0)
                {
                    ShiftMaster.ID = obj.ID;
                
                    _context.HRM_DEF_EmployeeShifts.Update(ShiftMaster);
                }
                else
                {

                    await _context.HRM_DEF_EmployeeShifts.AddAsync(ShiftMaster);
                }
                await _context.SaveChangesAsync();
                Log.Info("----Info CreateUpdateShiftMaster method Exit----");
                return ShiftMaster.ID;
            }
            catch (Exception ex)
            {
                Log.Error("Error in CreateUpdateShiftMaster Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }
    }

    #endregion

    #region GetEmployeeShiftsByEmployeeID
    public class GetEmployeeShiftsByEmployeeID : IRequest<HRM_DEF_EmployeeShiftDto>
    {
        public UserIdentityDto User { get; set; }
        public long EmployeeID { get; set; }
    }

    public class GetEmployeeShiftsByEmployeeCodeHandler : IRequestHandler<GetEmployeeShiftsByEmployeeID,HRM_DEF_EmployeeShiftDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetEmployeeShiftsByEmployeeCodeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<HRM_DEF_EmployeeShiftDto> Handle(GetEmployeeShiftsByEmployeeID request, CancellationToken cancellationToken)
        {
            
            var ShiftMaster = await _context.HRM_DEF_EmployeeShifts.AsNoTracking().ProjectTo<HRM_DEF_EmployeeShiftDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.EmployeeID == request.EmployeeID);
  
            return ShiftMaster;
        }
    }

    #endregion

    

    

    

}
