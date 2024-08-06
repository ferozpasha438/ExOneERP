using CIN.Application;
using CIN.Application.Common;
using CIN.Application.MobileMgt.Queries;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LS.API.MOB.Controllers
{
    public class IncidentReportController : BaseController
    {
        private readonly IWebHostEnvironment _env;
        public IncidentReportController(IOptions<AppMobileSettingsJson> appSettings, IWebHostEnvironment env) : base(appSettings)
        {
            _env = env;
        }

        protected string ContentRootPath => _env.ContentRootPath;

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {
            //await Task.Delay(3000);


            var list = await Mediator.Send(new GetIncidentReportList() { Input = filter.Values(), User = UserInfo() });

            ////var webRoot = $"{_env.ContentRootPath}/files/incidents";

            ////foreach (var item in list.Items)
            ////{               
            ////    try
            ////    {
            ////        if (item.ImagePath.HasValue())
            ////        {
            ////            var filePath = Path.Combine(webRoot, item.ImagePath);
            ////            byte[] imageArray = System.IO.File.ReadAllBytes(filePath);
            ////            item.Base64Image = $"data:image/{ Path.GetExtension(item.ImagePath).Substring(1)};base64,{Convert.ToBase64String(imageArray)}";
            ////        }
            ////    }
            ////    catch (Exception ex)
            ////    {
            ////        //item.Base64Image = $"data:image/{ Path.GetExtension(item.ImagePath).Substring(1)};base64,{Convert.ToBase64String(imageArray)}";

            ////    }
            ////}

            return Ok(list);
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            try
            {
                var files = HttpContext.Request.Form.Files;
                if (files.Count() > 0 && files[0] != null && files[0].Length > 0)
                {
                    var file = files[0];

                    var guid = Guid.NewGuid().ToString();
                    string name = file.FileName;

                    string title = Convert.ToString(HttpContext.Request.Form["title"]);
                    string description = Convert.ToString(HttpContext.Request.Form["description"]);
                    decimal siteGeoLatitude = Convert.ToDecimal(HttpContext.Request.Form["siteGeoLatitude"]);
                    decimal siteGeoLongitude = Convert.ToDecimal(HttpContext.Request.Form["siteGeoLongitude"]);
                    //description = string.IsNullOrEmpty(description) ? file.FileName : description;

                    guid = $"{guid}_{name}_{ Path.GetExtension(file.FileName)}";
                    var webRoot = $"{_env.ContentRootPath}/files/incidents";
                    var filePath = Path.Combine(webRoot, guid);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                        var obj = await Mediator.Send(new IncidentReportQuery()
                        {
                            Input = new()
                            {
                                Title = title,
                                Description = description,
                                SiteGeoLatitude = siteGeoLatitude,
                                SiteGeoLongitude = siteGeoLongitude,
                                ImagePath = guid
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
        }



    }
}
