using CIN.Application;
using CIN.Application.Common;
using CIN.Application.HumanResource.EmployeeMgmt.HRMgmtDtos;
using CIN.Application.HumanResource.EmployeeMgmt.HRMgmtQuery;
using CIN.Application.HumanResource.ServiceRequest.HRMServiceRequestDtos;
using CIN.Application.HumanResource.ServiceRequest.HRMServiceRequestQuery;
using CIN.Application.HumanResource.SetUp.HRMSetUpQuery;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace LS.API.HRM.Admin.Controllers.ServiceRequest
{
    public class ServiceRequestController : BaseController
    {
        private readonly IWebHostEnvironment _env;
        public ServiceRequestController(IOptions<AppSettingsJson> appSettings, IWebHostEnvironment env) : base(appSettings)
        {
            _env = env;
        }

        [HttpGet("getWaitingApprovalServiceRequestList")]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {
            var list = await Mediator.Send(new GetWaitingApprovalServiceRequestList() { Input = filter.Values(), User = UserInfo() });
            return Ok(list);
        }

        [HttpGet("getMyServiceRequestList")]
        public async Task<IActionResult> GetMyServiceRequestList([FromQuery] PaginationFilterDto filter)
        {
            var list = await Mediator.Send(new GetMyServiceRequestList() { Input = filter.Values(), User = UserInfo() });
            return Ok(list);
        }

        [HttpGet("getMyServiceRequestById/{id}")]
        public async Task<IActionResult> GetMyServiceRequestById([FromRoute] int id, [FromQuery] bool isFromApproval)
        {
            var obj = await Mediator.Send(new GetMyServiceRequestById() { Id = id, IsFromApproval = isFromApproval, User = UserInfo() });
            if (obj.FileName.HasValue())
                obj.FileName = $"vacreqs/{obj.FileName}";
            return Ok(obj);
        }

        [HttpGet("getRequestApprovalSelectListItem")]
        public async Task<IActionResult> GetRequestApprovalSelectListItem([FromQuery] int id, [FromQuery] int employeeId, [FromQuery] string serviceRequestType)
        {
            var list = await Mediator.Send(new GetRequestApprovalSelectListItem() { Id = id, EmployeeId = employeeId, ServiceRequestType = serviceRequestType, User = UserInfo() });
            return Ok(list);
        }

        [HttpGet("getVacationPolicyForEmployee")]
        public async Task<IActionResult> GetVacationPolicyForEmployee([FromQuery] int employeeId)
        {
            var list = await Mediator.Send(new GetVacationPolicyForEmployee() { EmployeeId = employeeId });
            return Ok(list);
        }

        [HttpGet("getVacationExitReEntryInfoByRequest")]
        public async Task<IActionResult> GetVacationExitReEntryInfoByRequest([FromQuery] int serviceId)
        {
            var list = await Mediator.Send(new GetVacationExitReEntryInfoByRequest() { ServiceId = serviceId });
            return Ok(list);
        }

        [HttpPost]
        public async Task<ActionResult> Create()
        {
            try
            {
                List<TblErpSysFileUploadDto> fileUploads = new();
                var files = HttpContext.Request.Form.Files;
                string guid = string.Empty;
                foreach (var file in files)
                {
                    if (file != null && file.Length > 0)
                    {
                        guid = Guid.NewGuid().ToString();
                        string fileName = file.FileName;
                        string name = file.Name;

                        guid = $"{guid}_{Path.GetExtension(file.FileName)}";
                        var webRoot = $"{_env.ContentRootPath}/files/vacreqs";
                        var filePath = Path.Combine(webRoot, guid);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }
                        break;
                    }
                }

                var obj = Convert.ToString(HttpContext.Request.Form["input"]);
                VacationServiceRequestDto vacItem = new();
                if (obj.HasValue())//&& module.Length > 40
                {
                    vacItem = JsonConvert.DeserializeObject<VacationServiceRequestDto>(obj);
                    vacItem.FileName = guid;

                    var serviceReq = await Mediator.Send(new CreateUpdateVacationRequest() { Input = vacItem, User = UserInfo() });
                    if (serviceReq.Id > 0)
                    {
                        return Ok(vacItem);
                    }
                    return BadRequest(new ApiMessageDto { Message = serviceReq.Message });
                }

                return Ok(obj);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPost("rejectApproveVacationRequest")]
        public async Task<ActionResult> RejectApproveVacationRequest([FromBody] ServiceRequestDataDto input)
        {
            var result = await Mediator.Send(new RejectApproveVacationRequest() { Input = input, User = UserInfo() });

            if (result.Id > 0)
                return NoContent();

            return BadRequest(new ApiMessageDto { Message = result.Message });
        }

        [HttpPost("approvalVacationRequestList")]
        public async Task<ActionResult> ApprovalVacationRequestList([FromBody] ApprovalListDto input)
        {
            var result = await Mediator.Send(new ApprovalVacationRequestList() { Input = input, User = UserInfo() });

            if (result.Id > 0)
                return NoContent();

            return BadRequest(new ApiMessageDto { Message = result.Message });
        }

        [HttpPost("createVacationReleaseExit")]
        public async Task<ActionResult> CreateVacationReleaseExit([FromBody] TblHRMTrnEmployeeExitReEntryInfoDto input)
        {
            var result = await Mediator.Send(new CreateVacationReleaseExit() { Input = input, User = UserInfo() });

            if (result.Id > 0)
                return NoContent();

            return BadRequest(new ApiMessageDto { Message = result.Message });
        }

        [HttpPost("createVacationReportEntry")]
        public async Task<ActionResult> CreateVacationReportEntry([FromBody] TblHRMTrnEmployeeReportingBackInfoDto input)
        {
            var result = await Mediator.Send(new CreateVacationReportEntry() { Input = input, User = UserInfo() });

            if (result.Id > 0)
                return NoContent();

            return BadRequest(new ApiMessageDto { Message = result.Message });
        }

        [HttpDelete("cancelVacationRequest")]
        public async Task<ActionResult> CancelVacationRequest([FromRoute] ApprovalListDto input)
        {
            var result = await Mediator.Send(new ApprovalVacationRequestList() { Input = input, User = UserInfo() });

            if (result.Id > 0)
                return NoContent();

            return BadRequest(new ApiMessageDto { Message = result.Message });
        }


    }
}
