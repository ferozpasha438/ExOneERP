using CIN.Application;
using CIN.Application.Common;
using CIN.Application.OperationsMgtDtos;
using CIN.Application.OperationsMgtQuery;
using CIN.Application.OperationsMgtQuery.Shared;
using CIN.Domain.OpeartionsMgt;
using CIN.Domain.SystemSetup;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace LS.API.OPR.Controllers.OperationsMgt
{
    public class EmployeesAttendanceController : BaseController
    {

        public EmployeesAttendanceController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }






        [HttpPost("enterEmployeeAttendance")]
        public async Task<ActionResult> EnterEmployeeAttendance([FromBody] TblOpEmployeeAttendanceDto dto)
        {
            if (dto.Id > 0)
            {
                //if (dto.ShiftCode == "O") { 
                //return BadRequest(new ApiMessageDto { Message = "Cancel the Attendance and Try Again" });
                //}
                var attn = await Mediator.Send(new GetAttendanceById() { User = UserInfo(), Id = dto.Id });
                var altAtt = await Mediator.Send(new GetAltAttendanceById() { Id = dto.Id, User = UserInfo() });


                if (attn is null)
                {
                    return BadRequest(new ApiMessageDto { Message = "Invalid_Attendance_Id" });
                }
                else if (altAtt is not null && (dto.AltEmployeeNumber != altAtt.EmployeeNumber && altAtt.Id != -1))
                {

                    return BadRequest(new ApiMessageDto { Message = "Cancel the attendance and Try" });
                }


            }


            var id = await Mediator.Send(new EnterEmployeeAttendance() { Input = dto, User = UserInfo() });
            if (id > 0)
            {
                return Created($"get/{id}", dto);
            }
            else if (id == -1)
            {
                return BadRequest(new ApiMessageDto { Message = "Attendance_Already_Exist" });
            }
            else if (id == -2)
            {
                return BadRequest(new ApiMessageDto { Message = "Same_Shift_Altready_Scheduled_For_Employee" });
            }
            else if (id == -4)
            {
                return BadRequest(new ApiMessageDto { Message = "Employee_Withdrawn" });

            }
            else if (id == -3)
            {
                return BadRequest(new ApiMessageDto { Message = "Employee_On_Leave" });

            }
            else if (id == -5)
            {
                return BadRequest(new ApiMessageDto { Message = "Employee_Transfered" });

            }
            else if (id == -6)
            {
                return BadRequest(new ApiMessageDto { Message = "Employee_Resigned" });

            }
           
            else if (id == -7)
            {
                return BadRequest(new ApiMessageDto { Message = "Employee_Terminated" });

            }

            else if (id == -8)
            {
                    return BadRequest(new ApiMessageDto { Message = "Incomplete_Shift_Locks_Found" });
            }


            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });

        }


        [HttpPost("enterEmployeeAbsent")]
        public async Task<ActionResult> EnterEmployeeAbsent([FromBody] TblOpEmployeeAttendanceDto dto)
        {
            var id = await Mediator.Send(new EnterEmployeeAbsent() { Input = dto, User = UserInfo() });
            if (id > 0)
            {
                return Created($"get/{id}", dto);
            }
            else if (id == -1)
            {
                return BadRequest(new ApiMessageDto { Message = "Attendance_Already_Exist" });
            }
            else if (id == -2)
            {
                return BadRequest(new ApiMessageDto { Message = "Same_Shift_Altready_Scheduled_For_Employee" });

            }
            else if (id == -3)
            {
                return BadRequest(new ApiMessageDto { Message = "Employee_Is_On_Leave" });

            }
            else if (id == -4)
            {
                return BadRequest(new ApiMessageDto { Message = "Employee_Withdrwan" });

            }
            else if (id == -5)
            {
                return BadRequest(new ApiMessageDto { Message = "Employee_Transfered" });

            }
            else if (id == -6)
            {
                return BadRequest(new ApiMessageDto { Message = "Employee_Resigned" });

            }
              else if (id == -7)
            {
                return BadRequest(new ApiMessageDto { Message = "Employee_Terminated" });

            }
              else if (id == -8)
            {
                return BadRequest(new ApiMessageDto { Message = "Incomplete_Shift_Locks_Found" });

            }
              else if (id == -9)
            {
                return BadRequest(new ApiMessageDto { Message = "Employee_Not_Exist" });

            }

            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });

        }




        [HttpGet("getSingleEmployeeAttendance/{projectCode}/{siteCode}/{employeeNumber}/{attnDate}/{shiftCode}")]
        public async Task<IActionResult> GetSingleEmployeeAttendance([FromRoute] string projectCode, [FromRoute] string siteCode, [FromRoute] string employeeNumber, [FromRoute] string attnDate, [FromRoute] string shiftCode)
        {
            var obj = await Mediator.Send(new GetSingleEmployeeAttendance()
            {
                ProjectCode = projectCode,
                SiteCode = siteCode,
                EmployeeNumber = employeeNumber,
                AttnDate = attnDate,
                ShiftCode = shiftCode,
                User = UserInfo()
            });
            return obj is null ? BadRequest(new ApiMessageDto { Message = ApiMessageInfo.NotFound }) : Ok(obj);
        }

        [HttpPost("getAllEmployeeAttendance")]
        public async Task<IActionResult> GetAllEmployeeAttendance([FromBody] InputGetAllAttendanceDto Dto)
        {
            Log.Info($"EmployeesAttendance Controller GetAllEmployeeAttendance dto : {Dto}");
            List<InputGetAttendanceDto> inputArray = Dto.InputList;
            Log.Info($"EmployeesAttendance Controller GetAllEmployeeAttendance inputArray : {inputArray}");
            List<EmployeeTimeSheetDto> outputList = new();
            for (int i = 0; i < inputArray.Count; i++)
            {
                Log.Info($"EmployeesAttendance Controller GetAllEmployeeAttendance i : {i}");
                var obj = await Mediator.Send(new GetSingleEmployeeAttendance()
                {
                    ProjectCode = inputArray[i].ProjectCode,
                    SiteCode = inputArray[i].SiteCode,
                    EmployeeNumber = inputArray[i].EmployeeNumber,
                    AttnDate = inputArray[i].AttnDate,
                    ShiftCode = inputArray[i].ShiftCode,
                    User = UserInfo()
                });
                Log.Info($"EmployeesAttendance Controller GetAllEmployeeAttendance obj : {obj}");
                outputList.Add(obj);

            }
            Log.Info($"EmployeesAttendance Controller GetAllEmployeeAttendance outputList : {outputList}");
            return outputList is null ? BadRequest(new ApiMessageDto { Message = ApiMessageInfo.NotFound }) : Ok(outputList);
        }




        //[HttpGet("getAutoFillEmployeeListForProjectSite/{projectCode}/{siteCode}/")]
        //public async Task<IActionResult> GetAutoFillEmployeeListForProjectSite([FromRoute] string projectCode, [FromRoute] string siteCode,String Search)
        //{
        //    var item = await Mediator.Send(new GetAutoFillEmployeeListForProjectSite() { ProjectCode = projectCode, SiteCode = siteCode, Search = Search, User = UserInfo() });
        //    return Ok(item);
        //}

        [HttpGet("getAttendanceById/{id}")]
        public async Task<IActionResult> Get([FromRoute] long id)
        {
            var obj = await Mediator.Send(new GetAttendanceById() { Id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("getAltAttendanceById/{id}")]
        public async Task<IActionResult> getAltAttendanceById([FromRoute] long id)
        {
            var obj = await Mediator.Send(new GetAltAttendanceById() { Id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }





        [HttpPost("enterAutoAttendance")]
        public async Task<ActionResult> EnterAutoAttendance([FromBody] List<TblOpEmployeeAttendanceDto> dto)
        {
            if (dto.Count == 0)
            {
                return BadRequest(new ApiMessageDto { Message = "No Records Found" });
            }
            var updatesCount = await Mediator.Send(new EnterAutoAttendance() { Input = dto, User = UserInfo() });
            if (updatesCount > 0)
            {
                return Created($"get/{updatesCount}", dto);
            }
            else if (updatesCount == 0)
            {
                return BadRequest(new ApiMessageDto { Message = "No Updates Found" });
            }
            else if (updatesCount == -1)
            {
                return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
            }
             else if (updatesCount == -2)
            {
                return BadRequest(new ApiMessageDto { Message = "Incomplete shifts locks found" });
            }
            else 
            {
                return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
            }



        }

        [HttpPost("clearMonthlyAttendanceWithDate")]
        public async Task<ActionResult> ClearMonthlyAttendanceWithDate([FromBody] InputClearMonthlyAttendanceWithDate dto)
        {
            var res = await Mediator.Send(new ClearMonthlyAttendanceWithDate() { Input = dto, User = UserInfo() });
            if (res >= 1)
            {
                return Ok(dto);

            }
            else if (res == 0)
            {
                return BadRequest(new ApiMessageDto { Message = "No Updstes Found" });
            }

            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });

        }




        [HttpGet("CancelAttendance/{id}")]
        public async Task<TblOpEmployeeAttendance> CancelAttendance([FromRoute] long id)
        {
            TblOpEmployeeAttendance altAtt = await Mediator.Send(new CancelAttendance() { Id = id, User = UserInfo() });
            return altAtt;
        }


        [HttpPost("enterAutoAttendanceForAllProjectSites")]             //for single Day
        public async Task<ActionResult> EnterAutoAttendanceForAllProjectSites([FromBody] List<DashboardEmployeeAttendanceDto> dtos)
        {
            bool isHoliday = await Mediator.Send(new IsHoliday() { User = UserInfo(), Date = Convert.ToDateTime(dtos[0].AttnDate,CultureInfo.InvariantCulture) });

            
            var dtosGroupByPS = dtos.GroupBy(e => new { e.ProjectCode, e.SiteCode }).ToList();
  
            long totalUpdatesCount = 0;
            if (isHoliday && dtosGroupByPS.Count>1)
            {
                return BadRequest(new ApiMessageDto { Message = "You_Can't_Enter_Auto_Attendance_For_All_Sites_On_Holiday" });
            }
            for (var i=0;i<dtosGroupByPS.Count;i++)
            {
                
                List<DashboardEmployeeAttendanceDto> ipdtos =dtos.Where(e=>e.ProjectCode== dtosGroupByPS[i].Key.ProjectCode &&e.SiteCode== dtosGroupByPS[i].Key.SiteCode).ToList();
                    List<TblOpEmployeeAttendanceDto> cdtos = new();
                if(ipdtos.Count>0)
                for (var j = 0; j < ipdtos.Count; j++)
                {
                    DashboardEmployeeAttendanceDto dto = ipdtos[j];
                    cdtos.Add(new()
                    {
                        EmployeeNumber = dto.EmployeeNumber,
                        ProjectCode = dto.ProjectCode,
                        SiteCode = dto.SiteCode,
                        AttnDate = dto.AttnDate,
                        ShiftCode = dto.ShiftCode,
                    });

                }
              
                 var   updatesCount =await Mediator.Send(new EnterAutoAttendance() { Input = cdtos, User = UserInfo() });
                if (dtosGroupByPS.Count==1 && updatesCount==-2)
                {
                    return BadRequest(new ApiMessageDto { Message = "Incomplete_Shifts_Locks_Found" });
                }
                totalUpdatesCount = updatesCount <= 0 ? totalUpdatesCount : totalUpdatesCount + updatesCount;
                }

            if (totalUpdatesCount > 0)
            {
                return Ok(new ApiMessageDto { Message = ApiMessageInfo.Success });
            }
            else if (totalUpdatesCount == 0)
            {
                return BadRequest(new ApiMessageDto { Message = "No Updates Found" });
            }

            else
                return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });



        }

    }
}
