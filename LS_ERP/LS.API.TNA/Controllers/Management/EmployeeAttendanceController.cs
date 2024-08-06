using CIN.Application;
using CIN.Application.TimeAndAttendance.Management.TNAMgmtDtos;
using CIN.Application.TimeAndAttendance.Management.TNAMgmtQuery;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LS.API.TNA.Controllers.Management
{
    public class EmployeeAttendanceController : FileBaseController
    {
        private readonly IWebHostEnvironment _env;
        private readonly IOptions<AppSettingsJson> _appSettings;

        public EmployeeAttendanceController(IOptions<AppSettingsJson> appSettings, IWebHostEnvironment env) : base(appSettings, env)
        {
            _env = env;
            _appSettings = appSettings;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] EmployeeAttendanceFilter filter)
        {
            var obj = await Mediator.Send(new GetEmployeeAttendanceList() { Input = filter, User = UserInfo() });
            return Ok(obj);
        }

        [HttpGet("ApproveEmployeeAttendance")]
        public async Task<IActionResult> ApproveEmployeeAttendance([FromQuery] EmployeeAttendanceFilter filter)
        {
            var obj = await Mediator.Send(new ApproveEmployeeAttendance() { Input = filter, User = UserInfo() });

            if (obj.Id > 0)
                return NoContent();
            else
                return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }

        [HttpGet("ConsolidateEmployeeAttendance")]
        public async Task<IActionResult> ConsolidateEmployeeAttendance([FromQuery] EmployeeAttendanceFilter filter)
        {
            var obj = await Mediator.Send(new ConsolidateEmployeeAttendance() { Input = filter, User = UserInfo() });
            return Ok(obj);
        }


        [HttpPost("ImportEmployeeAttendance")]
        public async Task<ActionResult> ImportEmployeeAttendance(IFormFile file)
        {
            AppCtrollerDto result = null;

            if (file == null || file.Length == 0)
                return Content("FileNotSelected");

            string fileExtension = Path.GetExtension(file.FileName);
            if (fileExtension != ".xls" && fileExtension != ".xlsx")
                return Content("InvalidFileUploaded");

            var webRoot = $"{_env.ContentRootPath}/files/uploadedattendance";
            var fileName = $"{Path.GetFileNameWithoutExtension(file.FileName)}_{DateTime.Now.ToString("ddMMyyyyHHmmssfff", CultureInfo.InvariantCulture)}{Path.GetExtension(file.FileName)}";

            //Create directory if not exists.
            if (!Directory.Exists(webRoot))
                Directory.CreateDirectory(webRoot);

            var filePath = Path.Combine(webRoot, fileName);
            var fileLocation = new FileInfo(filePath);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            using (ExcelPackage package = new ExcelPackage(fileLocation))
            {
                //ExcelWorksheet workSheet = package.Workbook.Worksheets["Table1"];
                var workSheet = package.Workbook.Worksheets.First();
                int totalRows = workSheet.Dimension.Rows;

                var employeeAttendanceList = new List<TblTNATrnEmployeeAttendanceDto>();

                for (int i = 2; i <= totalRows; i++)
                {
                    if (workSheet.Cells[i, 1].Value != null)
                    {
                        employeeAttendanceList.Add(new TblTNATrnEmployeeAttendanceDto
                        {
                            EmployeeID = Convert.ToInt32(workSheet.Cells[i, 1].Value),
                            Date = DateTime.Parse(Convert.ToString(workSheet.Cells[i, 3].Value)),
                            InTime = TimeSpan.Parse(Convert.ToString(workSheet.Cells[i, 4].Value)),
                            OutTime = TimeSpan.Parse(Convert.ToString(workSheet.Cells[i, 5].Value)),
                            AttnFlag = Convert.ToString(workSheet.Cells[i, 6].Value),
                            ShiftNumber = Convert.ToByte(workSheet.Cells[i, 9].Value),
                            ShiftCode = Convert.ToString(workSheet.Cells[i, 14].Value),
                        });
                    }
                }

                if (employeeAttendanceList.Count() > 0)
                    result = await Mediator.Send(new CreateUpdateEmployeeAttendance() { Input = employeeAttendanceList, User = UserInfo() });

                if (result.Id == employeeAttendanceList.Count())
                    return NoContent();
                else if (result.Id < employeeAttendanceList.Count())
                    return Content("PartiallySuccessful");
                else
                    return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
            }
        }
    }
}
