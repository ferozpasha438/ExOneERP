using CIN.Application;
using CIN.Application.Common;
using CIN.Application.HumanResource.EmployeeMgmt.HRMgmtDtos;
using CIN.Application.HumanResource.EmployeeMgmt.HRMgmtQuery;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LS.API.HRM.Admin.Controllers.EmployeeManagement
{
    public class EmployeeDocumentController : FileBaseController
    {
        private readonly IWebHostEnvironment _env;
        private readonly IOptions<AppSettingsJson> _appSettings;
        public EmployeeDocumentController(IOptions<AppSettingsJson> appSettings, IWebHostEnvironment env) : base(appSettings, env)
        {
            _env = env;
            _appSettings = appSettings;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {
            var list = await Mediator.Send(new GetEmployeeDocumentList() { Input = filter.Values(), User = UserInfo() });
            return Ok(list);
        }

        [HttpGet("GetEmployeeDocumentById")]
        public async Task<IActionResult> Get([FromQuery] int id, [FromQuery] int employeeID, [FromQuery] bool deleteDocument)
        {
            var obj = await Mediator.Send(new GetEmployeeDocumentById() { Id = id, EmployeeID = employeeID, User = UserInfo() });
            if (!deleteDocument)
                return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
            else
            {
                var webRoot = $"{_env.ContentRootPath}/files/employeedocuments";
                if (obj is not null)
                {
                    if (System.IO.File.Exists(Path.Combine(webRoot, obj.FileName)))
                        System.IO.File.Delete(Path.Combine(webRoot, obj.FileName));

                    var employeeAddressId = await Mediator.Send(new DeleteEmployeeDocument() { Id = id, EmployeeID = employeeID, User = UserInfo() });
                    if (employeeAddressId > 0)
                        return NoContent();
                }
            }
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromForm] TblHRMTrnEmployeeDocumentInfoDto dTO)
        {
            List<TblHRMTrnEmployeeDocumentInfoDto> employeeDocuments = new List<TblHRMTrnEmployeeDocumentInfoDto>();

            if (dTO is not null)
            {
                //var fileCollection = await Request.ReadFormAsync();
                //var files = fileCollection.Files;
                var obj = await Mediator.Send(new GetEmployeeDocumentById() { Id = dTO.Id, EmployeeID = dTO.EmployeeID, User = UserInfo() });
                if (dTO.Files is not null)
                    foreach (var file in dTO.Files)
                    {
                        if (file != null && file.Length > 0)
                        {
                            var webRoot = $"{_env.ContentRootPath}/files/employeedocuments";

                            if (obj is not null)
                                if (System.IO.File.Exists(Path.Combine(webRoot, obj.FileName)))
                                    System.IO.File.Delete(Path.Combine(webRoot, obj.FileName));

                            var guid = Guid.NewGuid().ToString();
                            guid = $"EmpID_{dTO.EmployeeID}_{guid}_{ Path.GetFileNameWithoutExtension(file.FileName)}{Path.GetExtension(file.FileName)}";

                            dTO.Name = file.FileName;
                            dTO.FileName = guid;
                            employeeDocuments.Add(dTO);

                            //Create directory if not exists.
                            if (!Directory.Exists(webRoot))
                                Directory.CreateDirectory(webRoot);

                            var filePath = Path.Combine(webRoot, guid);
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                file.CopyTo(stream);
                            }
                        }
                    }
                else
                {
                    dTO.Name = obj.Name;
                    dTO.FileName = obj.FileName;
                    employeeDocuments.Add(dTO);
                }
                var result = await Mediator.Send(new CreateUpdateEmployeeDocument() { Input = employeeDocuments, User = UserInfo() });

                if (result.Id > 0)
                {
                    if (employeeDocuments.FirstOrDefault().Id > 0)
                        return NoContent();
                    else
                        return Created($"get/{result.Id}", employeeDocuments.FirstOrDefault());
                }
            }
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }
    }
}
