using CIN.Application;
using CIN.Application.Common;
using CIN.Application.OperationsMgtDtos;
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
    public class ServicesController : BaseController
    {

        public ServicesController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }

        [HttpGet("getServicesPagedList")]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {

            var list = await Mediator.Send(new GetServicesPagedList() { Input = filter.Values(), User = UserInfo() });
            return Ok(list);
        }
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TblSndDefServiceMasterDto dTO)
        {
            var id = await Mediator.Send(new CreateService() { ServiceDto = dTO, User = UserInfo() });
            if (id > 0)
                return Created($"get/{id}", dTO);
            else if (id == -1)
            {
                return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Duplicate(nameof(dTO.ServiceCode)) });
            }
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }

        [HttpPost("AttachSurveyFormToService")]
        public async Task<ActionResult> AttachSurveyFormToService([FromBody] TblSndDefServiceMasterDto service)
        {
            var SurveyForm = await Mediator.Send(new AttachSurveyFormToService() { Service = service,User = UserInfo() });
            if (SurveyForm.Id > 0)
                return Created($"get/{SurveyForm.Id}", service.Id);
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }



        [HttpGet("getServiceById/{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var obj = await Mediator.Send(new GetServiceById() { Id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }


       



        [HttpGet("getServiceByServiceCode/{ServiceCode}")]
        public async Task<IActionResult> Get([FromRoute] string ServiceCode)
        {
            var obj = await Mediator.Send(new GetServiceByServiceCode() { ServiceCode = ServiceCode, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("getSelectServiceList")]
        public async Task<IActionResult> GetSelectServiceList()
        {
            //await Task.Delay(3000);
            var item = await Mediator.Send(new GetSelectServiceList() {User = UserInfo() });
            return Ok(item);
        }

        [HttpGet("getAutoFillServiceList")]
        public async Task<IActionResult> GetAutoFillServiceList(string search)
        {
            //await Task.Delay(3000);
            var item = await Mediator.Send(new GetAutoFillServiceList() { Input = search, User = UserInfo() });
            return Ok(item);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var ServiceId = await Mediator.Send(new DeleteService() { Id = id, User = UserInfo() });
            if (ServiceId > 0)
                return NoContent();
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }
    }
}
