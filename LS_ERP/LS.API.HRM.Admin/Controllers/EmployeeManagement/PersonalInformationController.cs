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
    public class PersonalInformationController : FileBaseController
    {
        private readonly IWebHostEnvironment _env;
        private readonly IOptions<AppSettingsJson> _appSettings;
        public PersonalInformationController(IOptions<AppSettingsJson> appSettings, IWebHostEnvironment env) : base(appSettings, env)
        {
            _env = env;
            _appSettings = appSettings;
        }
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {
            var list = await Mediator.Send(new GetEmployeeList() { Input = filter.Values(), User = UserInfo() });
            return Ok(list);
        }

        [HttpGet("GetEmployeePersonalInformationById")]
        public async Task<IActionResult> GetEmployeePersonalInformationById([FromQuery] string employeeNumber)
        {
            var obj = await Mediator.Send(new GetEmployeePersonalInformationById() { EmployeeNumber = employeeNumber, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("getEmployeeSelectListItem")]
        public async Task<IActionResult> GetEmployeeSelectListItem()
        {
            var obj = await Mediator.Send(new GetEmployeeSelectListItem() { User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        //[HttpPost]
        //public async Task<ActionResult> Create([FromBody] TblHRMTrnPersonalInformationDto dTO)
        //{
        //    if (dTO is not null)
        //    {
        //        //If new profile is uploaded.
        //        if (dTO.File is not null && dTO.File.Length > 0)
        //        {
        //            var webRoot = $"{_env.ContentRootPath}/files/employeeprofiles";

        //            //If employee already exists
        //            if (!string.IsNullOrEmpty(dTO.EmployeeNumber))
        //            {
        //                var obj = await Mediator.Send(new GetEmployeePersonalInformationById() { EmployeeNumber = dTO.EmployeeNumber, User = UserInfo() });
        //                if (obj is not null)
        //                    if (System.IO.File.Exists(Path.Combine(webRoot, obj.ProfileFileName)))
        //                        System.IO.File.Delete(Path.Combine(webRoot, obj.ProfileFileName));
        //            }
        //            var guid = Guid.NewGuid().ToString();
        //            guid = $"EmpID_{dTO.FirstNameEn + "_" + dTO.LastNameEn}_{guid}_{ Path.GetFileNameWithoutExtension(dTO.File.FileName)}{Path.GetExtension(dTO.File.FileName)}";
        //            dTO.ProfileFileName = guid;
        //            dTO.EmployeeImageUrl = Path.Combine(webRoot, guid);

        //            //Create directory if not exists.
        //            if (!Directory.Exists(webRoot))
        //                Directory.CreateDirectory(webRoot);

        //            var filePath = Path.Combine(webRoot, guid);
        //            using (var stream = new FileStream(filePath, FileMode.Create))
        //            {
        //                dTO.File.CopyTo(stream);
        //            }
        //        }
        //        var result = await Mediator.Send(new CreateUpdatePersonalInformation() { Input = dTO, User = UserInfo() });
        //        if (result.Id > 0)
        //        {
        //            if (dTO.Id > 0)
        //                return NoContent();
        //            else
        //                return Created($"get/{result.Id}", dTO);
        //        }
        //    }
        //    return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        //}

        [HttpPost]
        public async Task<ActionResult> Create()
        {
            var files = HttpContext.Request.Form.Files;
            var PersonalInformationDto = Convert.ToString(HttpContext.Request.Form["input"]);
            TblHRMTrnPersonalInformationDto dTO = new();
            if (PersonalInformationDto.HasValue())
            {
                dTO = JsonConvert.DeserializeObject<TblHRMTrnPersonalInformationDto>(PersonalInformationDto);
                //If new profile is uploaded.
                if (files.Count() > 0)
                {
                    var webRoot = $"{_env.ContentRootPath}/files/employeeprofiles";
                    var file = files.First();

                    //If employee already exists
                    if (!string.IsNullOrEmpty(dTO.EmployeeNumber))
                    {
                        var obj = await Mediator.Send(new GetEmployeePersonalInformationById() { EmployeeNumber = dTO.EmployeeNumber, User = UserInfo() });
                        if (obj is not null && !string.IsNullOrEmpty(obj.ProfileFileName))
                            if (System.IO.File.Exists(Path.Combine(webRoot, obj.ProfileFileName)))
                                System.IO.File.Delete(Path.Combine(webRoot, obj.ProfileFileName));
                    }
                    var guid = Guid.NewGuid().ToString();
                    guid = $"EmpID_{dTO.PrimaryNumber}_{guid}_{ Path.GetFileNameWithoutExtension(file.FileName)}{Path.GetExtension(file.FileName)}";
                    dTO.ProfileFileName = guid;
                    dTO.EmployeeImageUrl = guid;

                    //Create directory if not exists.
                    if (!Directory.Exists(webRoot))
                        Directory.CreateDirectory(webRoot);

                    var filePath = Path.Combine(webRoot, guid);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                }
                var result = await Mediator.Send(new CreateUpdatePersonalInformation() { Input = dTO, User = UserInfo() });
                if (result.Id > 0)
                {
                    if (dTO.Id > 0)
                        return NoContent();
                    else
                        return Created($"get/{result.Id}", dTO);
                }
            }
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }
    }
}
