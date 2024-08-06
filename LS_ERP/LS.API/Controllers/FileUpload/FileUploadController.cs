using CIN.Application;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Principal;
using CIN.Application.SystemQuery;
using CIN.Application.Common;

namespace LS.API.Controllers
{
    public class FileUploadController : ApiControllerBase
    {
        private readonly IWebHostEnvironment _env;

        public FileUploadController(IWebHostEnvironment env, IOptions<AppSettingsJson> appSettings)
        {
            _env = env;
        }

        //[HttpPost("fileupload")]
        //public async Task<IActionResult> FileUpload([FromForm] FileUploadDto input)
        //{
        //    // do the magic here
        //    return NoContent();
        //}
        //[HttpPost("createDocument")]
        //public IActionResult CreateDocument()
        //{
        //    var files = HttpContext.Request.Form;
        //    foreach (var key in Request.Form.Keys)
        //    {
        //        var data = JsonConvert.DeserializeObject<FileUploadDto>(Request.Form[key]);
        //        var file = Request.Form.Files["file" + key];

        //    }
        //    return Ok();
        //}


        [HttpPost("testuploadfiles")]
        public async Task<IActionResult> Testuploadfiles()
        {
            var files = HttpContext.Request.Form.Files;
            var module = Convert.ToString(HttpContext.Request.Form["modulefile"]);
            FileUploadItem fileItem = new();

            if (module.HasValue())//&& module.Length > 40
            {
                fileItem = JsonConvert.DeserializeObject<FileUploadItem>(module);
                return Ok(fileItem);

                //var fileUpload = await Mediator.Send(new UploadingFile() { Input = null });
                //if (fileUpload.Id > 0)
                //{
                //    return Ok(fileItem);
                //}
                //return BadRequest(new ApiMessageDto { Message = fileUpload.Message });
            }

            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });

        }

        [HttpPost("uploadfiles")]
        public IActionResult Uploadfiles()
        {
            try
            {
                var files = HttpContext.Request.Form.Files;
                foreach (var file in files)
                {
                    if (file != null && file.Length > 0)
                    {
                        var guid = Guid.NewGuid().ToString();
                        string name = file.FileName;

                        string description = Convert.ToString(HttpContext.Request.Form[name]);
                        description = string.IsNullOrEmpty(description) ? file.FileName : description;

                        guid = $"{guid}_{name}_{Path.GetExtension(file.FileName)}";
                        var webRoot = $"{_env.ContentRootPath}/files";
                        var filePath = Path.Combine(webRoot, guid);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }
                    }
                }
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }

}

