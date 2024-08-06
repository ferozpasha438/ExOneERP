using CIN.Application;
using Microsoft.Extensions.Options;
using CIN.Application;
using CIN.Application.Common;
using CIN.Application.SystemQuery;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CIN.Application.MobileMgt.Dtos;
using CIN.Application.MobileMgt.Queries;

namespace LS.API.MOB.Controllers
{
    public class CheckInEmpoyeeController : BaseController
    {

        private readonly IWebHostEnvironment _env;
        public CheckInEmpoyeeController(IOptions<AppMobileSettingsJson> appSettings, IWebHostEnvironment env) : base(appSettings)
        {
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {
            //await Task.Delay(3000);
            var list = await Mediator.Send(new CheckOutListQuery() { Input = filter.Values(), User = UserInfo() });
            return Ok(list);
        }

        [HttpPost]
        public async Task<IActionResult> Post()//[FromBody] HRM_TRAN_EmployeeTimeChartDto input
        {
            try
            {
                var files = HttpContext.Request.Form.Files;
                if (files.Count() > 0 && files[0] != null && files[0].Length > 0)
                {
                    var file = files[0];

                    var guid = Guid.NewGuid().ToString();
                    string name = file.FileName;

                    string inTime = Convert.ToString(HttpContext.Request.Form["inTime"]);
                    string outTime = Convert.ToString(HttpContext.Request.Form["outTime"]);


                    guid = $"{guid}_{name}_{ Path.GetExtension(file.FileName)}";
                    var webRoot = $"{_env.ContentRootPath}/files/checkout";
                    var filePath = Path.Combine(webRoot, guid);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                        var obj = await Mediator.Send(new CheckInEmployeeQuery()
                        {
                            Input = new()
                            {
                                InTime = inTime,
                                OutTime = outTime
                            },
                            User = UserInfo()
                        });
                        return obj.Status ? Ok(obj) : BadRequest(obj);
                    }
                    //return Ok(guid);
                }
            }
            catch (Exception ex)
            {

                return BadRequest(ApiMessageInfo.Status(0, false));
            }
            return BadRequest(ApiMessageInfo.Status(0, false));


            //var obj = await Mediator.Send(new CheckInEmployeeQuery() { Input = input, User = UserInfo() });
            //return Ok();
        }


        //public CheckInEmpoyeeController(IOptions<AppMobileSettingsJson> appSettings) : base(appSettings)
        //{
        //}

        //[HttpPost]
        //public async Task<IActionResult> Post([FromBody] HRM_TRAN_EmployeeTimeChartDto input)
        //{
        //    var obj = await Mediator.Send(new CheckInEmployeeQuery() { Input = input, User = UserInfo() });
        //    return Ok();
        //}

        //[HttpPost("checkOutEmpoyee")]
        //public async Task<IActionResult> CheckOutEmpoyee([FromBody] HRM_TRAN_EmployeeTimeChartDto input)
        //{
        //    var obj = await Mediator.Send(new CheckInEmployeeQuery() { Input = input, User = UserInfo() });
        //    return Ok();
        //}
    }
}
