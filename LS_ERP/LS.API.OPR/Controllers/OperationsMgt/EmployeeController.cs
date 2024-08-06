using CIN.Application;
using CIN.Application.Common;
using CIN.Application.OperationsMgtDtos;
using CIN.Application.OperationsMgtDtos.DMC;
using CIN.Application.OperationsMgtQuery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LS.API.OPR.Controllers.OperationsMgt
{
    public class EmployeeController : BaseController
    {

        public EmployeeController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }

        [HttpGet("getEmployeesPagedList")]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {

            var list = await Mediator.Send(new GetEmployeesPagedList() { Input = filter.Values(), User = UserInfo() });
            return Ok(list);
        }
        //[HttpPost]
        //public async Task<ActionResult> Create([FromBody] HRM_TRAN_EmployeeDto dTO)
        //{
        //    var id = await Mediator.Send(new CreateEmployee() { EmployeeDto = dTO, User = UserInfo() });
        //    if (id > 0)
        //    {
        //        return Created($"get/{id}", dTO);
        //    }
        //    else if (id == -1)
        //    {
        //        return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Duplicate(nameof(dTO.EmployeeID)) });
        //    }
        //    return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });

        //}

        [HttpGet("getEmployeeById/{iD}")]
        public async Task<IActionResult> Get([FromRoute] long iD)
        {
            var obj = await Mediator.Send(new GetEmployeeById() { Id = iD, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        





        [HttpGet("getEmployeeByEmployeeNumber/{employeeNumber}")]
        public async Task<IActionResult> Get([FromRoute]string employeeNumber)
        {
            var obj = await Mediator.Send(new GetEmployeeByEmployeeNumber() { EmployeeNumber = employeeNumber, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("getSelectEmployeeList")]  /*from DMC*/
        public async Task<IActionResult> GetSelectEmployeeList()
        {
            var item = await Mediator.Send(new GetSelectEmployeeList() { User = UserInfo() });
            return Ok(item);
        }
         
        [HttpGet("getSelectEmployeeList2")]
        public async Task<IActionResult> GetSelectEmployeeList2()
        {
            var item = await Mediator.Send(new GetSelectEmployeeList2() { User = UserInfo() });
            return Ok(item);
        }

        [HttpGet("getAutoFillEmployeeList")]
        public async Task<IActionResult> GetAutoFillEmployeeList(String Search)
        {
            var item = await Mediator.Send(new GetAutoFillEmployeeList() { Search = Search, User = UserInfo() });
            return Ok(item);
        }

        [HttpGet("getAutoSelectEmployeeList")]
        public async Task<IActionResult> GetAutoSelectEmployeeList(String Search)
        {
            var item = await Mediator.Send(new GetAutoSelectEmployeeList() { Search = Search, User = UserInfo() });
            return Ok(item);
        }



        //[HttpDelete("{id}")]
        //public async Task<ActionResult> Delete([FromRoute] int id)
        //{
        //    var EmployeeId = await Mediator.Send(new DeleteEmployee() { Id = id, User = UserInfo() });
        //    if (EmployeeId > 0)
        //        return NoContent();
        //    return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        //}




        [HttpGet("getEmployeesPrimarySiteLogPagedList")]
        public async Task<IActionResult> getEmployeesPrimarySiteLogPagedList([FromQuery] PaginationFilterDto filter)
        {

            var list = await Mediator.Send(new GetEmployeesPrimarySiteLogsPagedList() { Input = filter.Values(), User = UserInfo() });
            return Ok(list);
        }


        [HttpGet("getEmployeesPrimarySiteLogs/{employeeNumber}")]
        public async Task<IActionResult> GetEmployeesPrimarySiteLogs([FromRoute] string employeeNumber)
        {

            var list = await Mediator.Send(new GetEmployeesPrimarySiteLogs() { EmployeeNumber =employeeNumber, User = UserInfo() });
            return Ok(list);
        }


        [HttpPost("AddUpdatePrimarySiteLog")]
        public async Task<ActionResult> AddUpdatePrimarySiteLog([FromBody] HRM_TRAN_EmployeePrimarySites_LogDto dTO)
        {
            var res = await Mediator.Send(new AddUpdatePrimarySiteLog() { LogDto = dTO, User = UserInfo() });
            if (res.IsSuccess)
            {
                return Created($"get/{res.ErrorId}", dTO);
            }
            else 
            {
                return BadRequest(new ApiMessageDto { Message =res.ErrorMsg });
            }

        }


        [HttpGet("getPrimaryLogById/{id}")]
        public async Task<IActionResult> GetPrimaryLogById([FromRoute] long id)
        {
            var obj = await Mediator.Send(new GetPrimaryLogById() { Id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

    }
}
